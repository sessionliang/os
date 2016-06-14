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
    public class WifiDAO : DataProviderBase, IWifiDAO
    {
        private const string TABLE_NAME = "wx_Wifi";

        public int Insert(WifiInfo wifiInfo)
        {
            int voteID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(wifiInfo.ToNameValueCollection(), this.ConnectionString, WifiDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        voteID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, WifiDAO.TABLE_NAME);

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

        public void Update(WifiInfo wifiInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(wifiInfo.ToNameValueCollection(), this.ConnectionString, WifiDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void AddPVCount(int wifiID)
        {
            if (wifiID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", WifiDAO.TABLE_NAME, WifiAttribute.PVCount, wifiID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", WifiAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(WifiDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public WifiInfo GetWifiInfo(int wifiID)
        {
            WifiInfo wifiInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", wifiID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, WifiDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    wifiInfo = new WifiInfo(rdr);
                }
                rdr.Close();
            }

            return wifiInfo;
        }

        public WifiInfo GetWifiInfoByPublishmentSystemID(int publishmentSystemID)
        {
            WifiInfo wifiInfo = null;

            string SQL_WHERE = string.Format("WHERE publishmentSystemID = {0}", publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, WifiDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    wifiInfo = new WifiInfo(rdr);
                }
                rdr.Close();
            }             
            return wifiInfo;
        }

        public List<WifiInfo> GetWifiInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<WifiInfo> wifiInfoList = new List<WifiInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", StoreAttribute.PublishmentSystemID, publishmentSystemID, WifiAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", WifiAttribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, WifiDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    WifiInfo WifiInfo = new WifiInfo(rdr);
                    wifiInfoList.Add(WifiInfo);
                }
                rdr.Close();
            }

            return wifiInfoList;
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, WifiAttribute.PublishmentSystemID, publishmentSystemID, WifiAttribute.IsDisabled, true, WifiAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public List<WifiInfo> GetWifiInfoList(int publishmentSystemID)
        {
            List<WifiInfo> wifiInfoList = new List<WifiInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", StoreAttribute.PublishmentSystemID, publishmentSystemID);
             
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, WifiDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    WifiInfo WifiInfo = new WifiInfo(rdr);
                    wifiInfoList.Add(WifiInfo);
                }
                rdr.Close();
            }

            return wifiInfoList;
        }
        
    }
}
