using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using ECountType = SiteServer.WeiXin.Model.ECountType;
using ECountTypeUtils = SiteServer.WeiXin.Model.ECountTypeUtils;
using BaiRong.Model;
using System.Text;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class View360DAO : DataProviderBase, IView360DAO
    {
        private const string TABLE_NAME = "wx_View360";

        public int Insert(View360Info view360Info)
        {
            int view360ID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(view360Info.ToNameValueCollection(), this.ConnectionString, View360DAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        view360ID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, View360DAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return view360ID;
        }

        public void Update(View360Info view360Info)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(view360Info.ToNameValueCollection(), this.ConnectionString, View360DAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void AddPVCount(int view360ID)
        {
            if (view360ID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", View360DAO.TABLE_NAME, View360Attribute.PVCount, view360ID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int view360ID)
        {
            if (view360ID > 0)
            {
                List<int> view360IDList = new List<int>();
                view360IDList.Add(view360ID);
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(view360IDList));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", View360DAO.TABLE_NAME, view360ID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(List<int> view360IDList)
        {
            if (view360IDList != null && view360IDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(view360IDList));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", View360DAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(view360IDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> view360IDList)
        {
            List<int> keywordIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", View360Attribute.KeywordID, View360DAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(view360IDList));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    keywordIDList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return keywordIDList;
        }

        public View360Info GetView360Info(int view360ID)
        {
            View360Info view360Info = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", view360ID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, View360DAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    view360Info = new View360Info(rdr);
                }
                rdr.Close();
            }

            return view360Info;
        }

        public List<View360Info> GetView360InfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<View360Info> view360InfoList = new List<View360Info>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", View360Attribute.PublishmentSystemID, publishmentSystemID, View360Attribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", View360Attribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, View360DAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    View360Info view360Info = new View360Info(rdr);
                    view360InfoList.Add(view360Info);
                }
                rdr.Close();
            }

            return view360InfoList;
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, View360Attribute.PublishmentSystemID, publishmentSystemID, View360Attribute.IsDisabled, true, View360Attribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetTitle(int view360ID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", view360ID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, View360DAO.TABLE_NAME, 0, View360Attribute.Title, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    title = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return title;
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", View360Attribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(View360DAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<View360Info> GetView360InfoList(int publishmentSystemID)
        {
            List<View360Info> view360InfoList = new List<View360Info>();

            string SQL_WHERE = string.Format(" AND {0} = {1}", View360Attribute.PublishmentSystemID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, View360DAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    View360Info view360Info = new View360Info(rdr);
                    view360InfoList.Add(view360Info);
                }
                rdr.Close();
            }

            return view360InfoList;
        }

    }
}
