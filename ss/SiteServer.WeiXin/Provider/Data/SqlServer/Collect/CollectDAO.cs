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
    public class CollectDAO : DataProviderBase, ICollectDAO
    {
        private const string TABLE_NAME = "wx_Collect";

        public int Insert(CollectInfo collectInfo)
        {
            int collectID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(collectInfo.ToNameValueCollection(), this.ConnectionString, CollectDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        collectID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, CollectDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return collectID;
        }

        public void Update(CollectInfo collectInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(collectInfo.ToNameValueCollection(), this.ConnectionString, CollectDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void AddUserCount(int collectID)
        {
            if (collectID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", CollectDAO.TABLE_NAME, CollectAttribute.UserCount, collectID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void AddPVCount(int collectID)
        {
            if (collectID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", CollectDAO.TABLE_NAME, CollectAttribute.PVCount, collectID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, int collectID)
        {
            if (collectID > 0)
            {
                List<int> collectIDList = new List<int>();
                collectIDList.Add(collectID);
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(collectIDList));

                DataProviderWX.CollectLogDAO.DeleteAll(collectID);
                DataProviderWX.CollectItemDAO.DeleteAll(publishmentSystemID, collectID);

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", CollectDAO.TABLE_NAME, collectID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> collectIDList)
        {
            if (collectIDList != null && collectIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(collectIDList));

                foreach (int collectID in collectIDList)
                {
                    DataProviderWX.CollectLogDAO.DeleteAll(collectID);
                    DataProviderWX.CollectItemDAO.DeleteAll(publishmentSystemID, collectID);
                }

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", CollectDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(collectIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> collectIDList)
        {
            List<int> keywordIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", CollectAttribute.KeywordID, CollectDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(collectIDList));

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

        public CollectInfo GetCollectInfo(int collectID)
        {
            CollectInfo collectInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", collectID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CollectDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    collectInfo = new CollectInfo(rdr);
                }
                rdr.Close();
            }

            return collectInfo;
        }

        public List<CollectInfo> GetCollectInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<CollectInfo> collectInfoList = new List<CollectInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", CollectAttribute.PublishmentSystemID, publishmentSystemID, CollectAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", CollectAttribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CollectDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CollectInfo collectInfo = new CollectInfo(rdr);
                    collectInfoList.Add(collectInfo);
                }
                rdr.Close();
            }

            return collectInfoList;
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, CollectAttribute.PublishmentSystemID, publishmentSystemID, CollectAttribute.IsDisabled, true, CollectAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetTitle(int collectID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", collectID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CollectDAO.TABLE_NAME, 0, CollectAttribute.Title, SQL_WHERE, null);

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
            string whereString = string.Format("WHERE {0} = {1}", CollectAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CollectDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<CollectInfo> GetCollectInfoList(int publishmentSystemID)
        {
            List<CollectInfo> collectInfoList = new List<CollectInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", CollectAttribute.PublishmentSystemID, publishmentSystemID);            

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CollectDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CollectInfo collectInfo = new CollectInfo(rdr);
                    collectInfoList.Add(collectInfo);
                }
                rdr.Close();
            }

            return collectInfoList;
        }
    }
}
