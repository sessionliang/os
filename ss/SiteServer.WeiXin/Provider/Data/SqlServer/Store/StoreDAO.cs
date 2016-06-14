using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class StoreDAO : DataProviderBase, IStoreDAO
    {
        private const string TABLE_NAME = "wx_Store";

        public int Insert(StoreInfo storeInfo)
        {
            int voteID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(storeInfo.ToNameValueCollection(), this.ConnectionString, StoreDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        voteID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, StoreDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return voteID;
        }


        public void Update(StoreInfo storeInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(storeInfo.ToNameValueCollection(), this.ConnectionString, StoreDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void AddPVCount(int storeID)
        {
            if (storeID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", StoreDAO.TABLE_NAME, StoreAttribute.PVCount, storeID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, int storeID)
        {
            if (storeID > 0)
            {
                List<int> StoreIDList = new List<int>();
                StoreIDList.Add(storeID);
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(StoreIDList));                 
                DataProviderWX.StoreItemDAO.DeleteAll(publishmentSystemID, storeID);

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", StoreDAO.TABLE_NAME, storeID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> storeIDList)
        {
            if (storeIDList != null && storeIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(storeIDList));

                foreach (int storeID in storeIDList)
                {                     
                    DataProviderWX.StoreItemDAO.DeleteAll(publishmentSystemID, storeID);
                }

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", StoreDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(storeIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> storeIDList)
        {
            List<int> keywordIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", StoreAttribute.KeywordID, StoreDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(storeIDList));

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

        public StoreInfo GetStoreInfo(int storeID)
        {
            StoreInfo storeInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", storeID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    storeInfo = new StoreInfo(rdr);
                }
                rdr.Close();
            }

            return storeInfo;
        }

        public List<StoreInfo> GetStoreInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<StoreInfo> storeInfoList = new List<StoreInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", StoreAttribute.PublishmentSystemID, publishmentSystemID, StoreAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", StoreAttribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    StoreInfo StoreInfo = new StoreInfo(rdr);
                    storeInfoList.Add(StoreInfo);
                }
                rdr.Close();
            }

            return storeInfoList;
        }

        public string GetTitle(int storeID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", storeID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreDAO.TABLE_NAME, 0, StoreAttribute.Title, SQL_WHERE, null);

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
            string whereString = string.Format("WHERE {0} = {1}", StoreAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(StoreDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, StoreAttribute.PublishmentSystemID, publishmentSystemID, StoreAttribute.IsDisabled, true, StoreAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public List<StoreInfo> GetStoreInfoList(int publishmentSystemID)
        {
            List<StoreInfo> storeInfoList = new List<StoreInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", StoreAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    StoreInfo StoreInfo = new StoreInfo(rdr);
                    storeInfoList.Add(StoreInfo);
                }
                rdr.Close();
            }

            return storeInfoList;
        }
        

    }
}
