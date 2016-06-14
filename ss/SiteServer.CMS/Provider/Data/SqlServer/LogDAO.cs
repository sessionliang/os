using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Text;
using BaiRong.Model;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class LogDAO : DataProviderBase, SiteServer.CMS.Core.ILogDAO
    {
        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_CHANNELID = "@ChannelID";
        private const string PARM_CONTENTID = "@ContentID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_ACTION = "@Action";
        private const string PARM_SUMMARY = "@Summary";

        public void Insert(SiteServer.CMS.Model.LogInfo log)
        {
            string sqlString = "INSERT INTO siteserver_Log(PublishmentSystemID, ChannelID, ContentID, UserName, IPAddress, AddDate, Action, Summary) VALUES (@PublishmentSystemID, @ChannelID, @ContentID, @UserName, @IPAddress, @AddDate, @Action, @Summary)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_Log(ID, PublishmentSystemID, ChannelID, ContentID, UserName, IPAddress, AddDate, Action, Summary) VALUES (siteserver_Log_SEQ.NEXTVAL, @PublishmentSystemID, @ChannelID, @ContentID, @UserName, @IPAddress, @AddDate, @Action, @Summary)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(LogDAO.PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, log.PublishmentSystemID),
                this.GetParameter(LogDAO.PARM_CHANNELID, EDataType.Integer, log.ChannelID),
                this.GetParameter(LogDAO.PARM_CONTENTID, EDataType.Integer, log.ContentID),
				this.GetParameter(LogDAO.PARM_USER_NAME, EDataType.VarChar, 50, log.UserName),
				this.GetParameter(LogDAO.PARM_IP_ADDRESS, EDataType.VarChar, 50, log.IPAddress),
                this.GetParameter(LogDAO.PARM_ADD_DATE, EDataType.DateTime, log.AddDate),
				this.GetParameter(LogDAO.PARM_ACTION, EDataType.NVarChar, 255, log.Action),
				this.GetParameter(LogDAO.PARM_SUMMARY, EDataType.NVarChar, 255, log.Summary)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM siteserver_Log WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll()
        {
            string sqlString = "DELETE FROM siteserver_Log";

            this.ExecuteNonQuery(sqlString);
        }

        public string GetSelectCommend()
        {
            return "SELECT ID, PublishmentSystemID, ChannelID, ContentID, UserName, IPAddress, AddDate, Action, Summary FROM siteserver_Log";
        }

        public string GetSelectCommend(int publishmentSystemID, string logType, string userName, string keyword, string dateFrom, string dateTo)
        {
            if (publishmentSystemID == 0 && (string.IsNullOrEmpty(logType) || StringUtils.EqualsIgnoreCase(logType, "All")) && string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
            {
                return this.GetSelectCommend();
            }

            StringBuilder whereString = new StringBuilder("WHERE ");

            bool isWhere = false;

            if (publishmentSystemID > 0)
            {
                isWhere = true;
                whereString.AppendFormat("(PublishmentSystemID = {0})", publishmentSystemID);
            }

            if (!string.IsNullOrEmpty(logType) && !StringUtils.EqualsIgnoreCase(logType, "All"))
            {
                if (isWhere)
                {
                    whereString.Append(" AND ");
                }
                isWhere = true;

                if (StringUtils.EqualsIgnoreCase(logType, "Channel"))
                {
                    whereString.Append("(ChannelID > 0 AND ContentID = 0)");
                }
                else if (StringUtils.EqualsIgnoreCase(logType, "Content"))
                {
                    whereString.Append("(ChannelID > 0 AND ContentID > 0)");
                }
            }

            if (!string.IsNullOrEmpty(userName))
            {
                if (isWhere)
                {
                    whereString.Append(" AND ");
                }
                isWhere = true;
                whereString.AppendFormat("(UserName = '{0}')", userName);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                if (isWhere)
                {
                    whereString.Append(" AND ");
                }
                isWhere = true;
                whereString.AppendFormat("(Action LIKE '%{0}%' OR Summary LIKE '%{0}%')",PageUtils.FilterSql(keyword));
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                if (isWhere)
                {
                    whereString.Append(" AND ");
                }
                isWhere = true;
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereString.AppendFormat("(to_char(AddDate,'YYYY-MM-DD') >= '{0}')", dateFrom);
                }
                else
                {
                    whereString.AppendFormat("(AddDate >= '{0}')", dateFrom);
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                if (isWhere)
                {
                    whereString.Append(" AND ");
                }
                isWhere = true;
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereString.AppendFormat("(to_char(AddDate,'YYYY-MM-DD') <= '{0}')", dateFrom);
                }
                else
                {
                    whereString.AppendFormat("(AddDate <= '{0}')", dateTo);
                }
            }

            return "SELECT ID, PublishmentSystemID, ChannelID, ContentID, UserName, IPAddress, AddDate, Action, Summary FROM siteserver_Log " + whereString.ToString();
        }
    }
}
