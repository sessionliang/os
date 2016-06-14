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
    public class VoteLogDAO : DataProviderBase, IVoteLogDAO
    {
        private const string TABLE_NAME = "wx_VoteLog";

        public void Insert(VoteLogInfo logInfo)
        {
            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(logInfo.ToNameValueCollection(), this.ConnectionString, VoteLogDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_INSERT, parms);
        }

        public void DeleteAll(int voteID)
        {
            if (voteID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2}", VoteLogDAO.TABLE_NAME, VoteLogAttribute.VoteID, voteID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(List<int> logIDList)
        {
            if (logIDList != null && logIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", VoteLogDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(logIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public int GetCount(int voteID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2}", VoteLogDAO.TABLE_NAME, VoteLogAttribute.VoteID, voteID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public bool IsVoted(int voteID, string cookieSN, string wxOpenID)
        {
            bool isVoted = false;
            string sqlString;
            if (string.IsNullOrEmpty(wxOpenID))
            {
                sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = '{4}' ", VoteLogDAO.TABLE_NAME, VoteLogAttribute.VoteID, voteID, VoteLogAttribute.CookieSN, cookieSN);
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = '{4}' ", VoteLogDAO.TABLE_NAME, VoteLogAttribute.VoteID, voteID, VoteLogAttribute.WXOpenID, wxOpenID);
            } 

            isVoted = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString) > 0;

            return isVoted;
        }

        public string GetSelectString(int voteID)
        {
            string whereString = string.Format("WHERE {0} = {1}", VoteLogAttribute.VoteID, voteID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(VoteLogDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<VoteLogInfo> GetVoteLogInfoListByVoteID(int publishmentSystemID, int voteID)
        {
            List<VoteLogInfo> voteLogInfoList = new List<VoteLogInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = '{3}'", VoteLogAttribute.PublishmentSystemID, publishmentSystemID, VoteLogAttribute.VoteID, voteID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, VoteLogDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    VoteLogInfo voteLogInfo = new VoteLogInfo(rdr);
                    voteLogInfoList.Add(voteLogInfo);
                }
                rdr.Close();
            }

            return voteLogInfoList;
        }

        public List<VoteLogInfo> GetVoteLogInfoList(int publishmentSystemID)
        {
            List<VoteLogInfo> voteLogInfoList = new List<VoteLogInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", VoteLogAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, VoteLogDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    VoteLogInfo voteLogInfo = new VoteLogInfo(rdr);
                    voteLogInfoList.Add(voteLogInfo);
                }
                rdr.Close();
            }

            return voteLogInfoList;
        }

        public int GetCount(int voteID, string iPAddress)
        {

            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = '{4}'", VoteLogDAO.TABLE_NAME, VoteLogAttribute.VoteID, voteID, VoteLogAttribute.IPAddress, iPAddress);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }
    }
}
