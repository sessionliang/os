using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using System.Collections;
using SiteServer.BBS.Core;
using System.Text;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class BBSLogDAO : DataProviderBase, SiteServer.BBS.IBBSLogDAO
    {
        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_FORUM_ID = "@ForumID";
        private const string PARM_THREAD_ID = "@ThreadID";
        private const string PARM_POST_ID = "@PostID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_ACTION = "@Action";
        private const string PARM_SUMMARY = "@Summary";

        public void Insert(BBSLogInfo log)
        {
            string sqlString = "INSERT INTO bbs_Log(PublishmentSystemID, ForumID, ThreadID, PostID, UserName, IPAddress, AddDate, Action, Summary) VALUES (@PublishmentSystemID, @ForumID, @ThreadID, @PostID, @UserName, @IPAddress, @AddDate, @Action, @Summary)";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(BBSLogDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, log.PublishmentSystemID),
                this.GetParameter(BBSLogDAO.PARM_FORUM_ID, EDataType.Integer, log.ForumID),
                this.GetParameter(BBSLogDAO.PARM_THREAD_ID, EDataType.Integer, log.ThreadID),
                this.GetParameter(BBSLogDAO.PARM_POST_ID, EDataType.Integer, log.PostID),
				this.GetParameter(BBSLogDAO.PARM_USER_NAME, EDataType.VarChar, 50, log.UserName),
				this.GetParameter(BBSLogDAO.PARM_IP_ADDRESS, EDataType.VarChar, 50, log.IPAddress),
                this.GetParameter(BBSLogDAO.PARM_ADD_DATE, EDataType.DateTime, log.AddDate),
				this.GetParameter(BBSLogDAO.PARM_ACTION, EDataType.NVarChar, 255, log.Action),
				this.GetParameter(BBSLogDAO.PARM_SUMMARY, EDataType.NVarChar, 255, log.Summary)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM bbs_Log WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll(int publishmentSystemID)
        {
            string sqlString = string.Format("DELETE FROM bbs_Log WHERE PublishmentSystemID = {0}", publishmentSystemID);

            this.ExecuteNonQuery(sqlString);
        }

        public string GetSelectCommend(int publishmentSystemID)
        {
            return string.Format("SELECT ID, ForumID, ThreadID, PostID, UserName, IPAddress, AddDate, Action, Summary FROM bbs_Log WHERE PublishmentSystemID = {0}", publishmentSystemID);
        }

        public string GetSelectCommend(int publishmentSystemID, int forumID, string logType, string userName, string keyword, string dateFrom, string dateTo)
        {
            if (forumID == 0 && (string.IsNullOrEmpty(logType) || StringUtils.EqualsIgnoreCase(logType, "All")) && string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
            {
                return this.GetSelectCommend(publishmentSystemID);
            }

            StringBuilder whereString = new StringBuilder(string.Format("WHERE PublishmentSystemID = {0}", publishmentSystemID));

            if (forumID > 0)
            {
                whereString.AppendFormat(" AND ForumID = {0}", forumID);
            }

            if (!string.IsNullOrEmpty(logType) && !StringUtils.EqualsIgnoreCase(logType, "All"))
            {
                if (StringUtils.EqualsIgnoreCase(logType, "Channel"))
                {
                    whereString.Append(" AND ThreadID > 0 AND PostID = 0");
                }
                else if (StringUtils.EqualsIgnoreCase(logType, "Content"))
                {
                    whereString.Append(" AND ThreadID > 0 AND PostID > 0");
                }
            }

            if (!string.IsNullOrEmpty(userName))
            {
                whereString.AppendFormat(" AND UserName = '{0}'", userName);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                whereString.AppendFormat(" AND (Action LIKE '%{0}%' OR Summary LIKE '%{0}%')", keyword);
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                whereString.AppendFormat(" AND AddDate >= '{0}'", dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                whereString.AppendFormat(" AND AddDate <= '{0}'", dateTo);
            }

            return "SELECT ID, PublishmentSystemID, ForumID, ThreadID, PostID, UserName, IPAddress, AddDate, Action, Summary FROM bbs_Log " + whereString.ToString();
        }
    }
}