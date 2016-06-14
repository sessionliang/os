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
    public class CouponActDAO : DataProviderBase, ICouponActDAO
    {
        private const string TABLE_NAME = "wx_CouponAct";

        public int Insert(CouponActInfo actInfo)
        {
            int actID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(actInfo.ToNameValueCollection(), this.ConnectionString, CouponActDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        actID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, CouponActDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return actID;
        }

        public void Update(CouponActInfo actInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(actInfo.ToNameValueCollection(), this.ConnectionString, CouponActDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateUserCount(int actID, int publishmentSystemID)
        {
            if (actID > 0)
            {
                string sqlString = string.Format("UPDATE {0} set UserCount= UserCount+1 WHERE ID = {1} AND publishmentSystemID = {2}", TABLE_NAME, actID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void UpdatePVCount(int actID, int publishmentSystemID)
        {
            if (actID > 0)
            {
                string sqlString = string.Format("UPDATE {0} set PVCount= PVCount+1 WHERE ID = {1} AND publishmentSystemID = {2}", TABLE_NAME, actID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
        }
        
        public void Delete(List<int> actIDList)
        {
            if (actIDList != null && actIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(actIDList));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(actIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> actIDList)
        {
            List<int> keywordIDList = new List<int>();

            if (actIDList != null && actIDList.Count > 0)
            {
                string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", CouponActAttribute.KeywordID, TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(actIDList));

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        keywordIDList.Add(rdr.GetInt32(0));
                    }
                    rdr.Close();
                }
            }

            return keywordIDList;
        }

        public CouponActInfo GetActInfo(int actID)
        {
            CouponActInfo actInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", actID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponActDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    actInfo = new CouponActInfo(rdr);
                }
                rdr.Close();
            }

            return actInfo;
        }

        public List<int> GetActIDList(int publishmentSystemID)
        {
            List<int> list = new List<int>();

            string sqlString = string.Format("SELECT ID FROM {0} WHERE {1} = {2}", TABLE_NAME, CouponActAttribute.PublishmentSystemID, publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }

        public List<CouponActInfo> GetActInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<CouponActInfo> actInfoList = new List<CouponActInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", CouponActAttribute.PublishmentSystemID, publishmentSystemID, CouponActAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", CouponActAttribute.KeywordID, keywordID);
            }
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponActDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CouponActInfo actInfo = new CouponActInfo(rdr);
                    actInfoList.Add(actInfo);
                }
                rdr.Close();
            }

            return actInfoList;
        }

        public int GetKeywordID(int actID)
        {
            int keywordID = 0;

            string SQL_WHERE = string.Format("WHERE ID = {0}", actID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponActDAO.TABLE_NAME, 0, CouponActAttribute.KeywordID, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    keywordID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return keywordID;
        }

        public string GetTitle(int actID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", actID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponActDAO.TABLE_NAME, 0, CouponActAttribute.Title, SQL_WHERE, null);

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
            string whereString = string.Format("WHERE {0} = {1}", CouponActAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CouponActDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, CouponActAttribute.PublishmentSystemID, publishmentSystemID, CouponActAttribute.IsDisabled, true, CouponActAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public List<CouponActInfo> GetCouponActInfoList(int publishmentSystemID)
        {
            List<CouponActInfo> couponActInfoList = new List<CouponActInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", CouponActAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CouponActDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CouponActInfo couponActInfo = new CouponActInfo(rdr);
                    couponActInfoList.Add(couponActInfo);
                }
                rdr.Close();
            }

            return couponActInfoList;
        }
    }
}
