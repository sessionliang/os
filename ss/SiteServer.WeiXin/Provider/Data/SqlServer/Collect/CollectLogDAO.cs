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
    public class CollectLogDAO : DataProviderBase, ICollectLogDAO
    {
        private const string TABLE_NAME = "wx_CollectLog";

        public void Insert(CollectLogInfo logInfo)
        {
            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(logInfo.ToNameValueCollection(), this.ConnectionString, CollectLogDAO.TABLE_NAME, out parms);



            this.ExecuteNonQuery(SQL_INSERT, parms);
        }

        public void DeleteAll(int collectID)
        {
            if (collectID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2}", CollectLogDAO.TABLE_NAME, CollectLogAttribute.CollectID, collectID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(List<int> logIDList)
        {
            if (logIDList != null && logIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", CollectLogDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(logIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public int GetCount(int collectID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2}", CollectLogDAO.TABLE_NAME, CollectLogAttribute.CollectID, collectID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public bool IsCollectd(int collectID, string cookieSN, string wxOpenID)
        {
            bool isCollectd = false;

            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = '{4}'", CollectLogDAO.TABLE_NAME, CollectLogAttribute.CollectID, collectID, CollectLogAttribute.CookieSN, cookieSN);

            isCollectd = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString) > 0;

            return isCollectd;
        }

        public List<int> GetVotedItemIDList(int collectID, string cookieSN)
        {
            string sqlString = string.Format("SELECT ItemID FROM {0} WHERE {1} = {2} AND {3} = '{4}'", CollectLogDAO.TABLE_NAME, CollectLogAttribute.CollectID, collectID, CollectLogAttribute.CookieSN, cookieSN);
            return BaiRongDataProvider.DatabaseDAO.GetIntList(sqlString);
        }

        public string GetSelectString(int collectID)
        {
            string whereString = string.Format("WHERE {0} = {1}", CollectLogAttribute.CollectID, collectID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CollectLogDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<CollectLogInfo> GetCollectLogInfoList(int publishmentSystemID, int collectID, int collectItemID)
        {

            List<CollectLogInfo> list = new List<CollectLogInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3} AND {4} = {5}", CollectLogAttribute.PublishmentSystemID, publishmentSystemID, CollectLogAttribute.CollectID, collectID, CollectLogAttribute.ItemID, collectItemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CollectLogDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CollectLogInfo logInfo = new CollectLogInfo(rdr);
                    list.Add(logInfo);
                }
                rdr.Close();
            }

            return list;
        }
    }
}
