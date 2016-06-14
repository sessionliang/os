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
    public class WifiNodeDAO : DataProviderBase, IWifiNodeDAO
    {
        private const string TABLE_NAME = "wx_WifiNode";
        public int Insert(WifiNodeInfo wifiNodeInfo)
        {
            int voteID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(wifiNodeInfo.ToNameValueCollection(), this.ConnectionString, WifiNodeDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        voteID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, WifiNodeDAO.TABLE_NAME);

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

        public void Update(WifiNodeInfo wifiNodeInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(wifiNodeInfo.ToNameValueCollection(), this.ConnectionString, WifiNodeDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public List<WifiNodeInfo> GetWifiNodeInfoList(int publishmentSystemID)
        {
            List<WifiNodeInfo> wifiNodeInfoList = new List<WifiNodeInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", StoreAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, WifiNodeDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    WifiNodeInfo wifiNodeInfo = new WifiNodeInfo(rdr);
                    wifiNodeInfoList.Add(wifiNodeInfo);
                }
                rdr.Close();
            }

            return wifiNodeInfoList;
        }
        
    }
}
