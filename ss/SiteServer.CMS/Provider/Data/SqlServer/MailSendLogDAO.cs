using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Model;
using System.Text;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class MailSendLogDAO : DataProviderBase, SiteServer.CMS.Core.IMailSendLogDAO
    {
        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_CHANNELID = "@ChannelID";
        private const string PARM_CONTENTID = "@ContentID";
        private const string PARM_TITLE = "@Title";
        private const string PARM_PAGE_URL = "@PageUrl";
        private const string PARM_RECEIVER = "@Receiver";
        private const string PARM_MAIL = "@Mail";
        private const string PARM_SENDER = "@Sender";
        private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_ADD_DATE = "@AddDate";

        public void Insert(MailSendLogInfo log)
        {
            string sqlString = "INSERT INTO siteserver_MailSendLog(PublishmentSystemID, ChannelID, ContentID, Title, PageUrl, Receiver, Mail, Sender, IPAddress, AddDate) VALUES (@PublishmentSystemID, @ChannelID, @ContentID, @Title, @PageUrl, @Receiver, @Mail, @Sender, @IPAddress, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_MailSendLog(ID, PublishmentSystemID, ChannelID, ContentID, Title, PageUrl, Receiver, Mail, Sender, IPAddress, AddDate) VALUES (siteserver_MailSendLog_SEQ.NEXTVAL, @PublishmentSystemID, @ChannelID, @ContentID, @Title, @PageUrl, @Receiver, @Mail, @Sender, @IPAddress, @AddDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(MailSendLogDAO.PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, log.PublishmentSystemID),
                this.GetParameter(MailSendLogDAO.PARM_CHANNELID, EDataType.Integer, log.ChannelID),
                this.GetParameter(MailSendLogDAO.PARM_CONTENTID, EDataType.Integer, log.ContentID),
				this.GetParameter(MailSendLogDAO.PARM_TITLE, EDataType.NVarChar, 255, log.Title),
                this.GetParameter(MailSendLogDAO.PARM_PAGE_URL, EDataType.VarChar, 200, log.PageUrl),
                this.GetParameter(MailSendLogDAO.PARM_RECEIVER, EDataType.NVarChar, 255, log.Receiver),
                this.GetParameter(MailSendLogDAO.PARM_MAIL, EDataType.NVarChar, 255, log.Mail),
                this.GetParameter(MailSendLogDAO.PARM_SENDER, EDataType.NVarChar, 255, log.Sender),
				this.GetParameter(MailSendLogDAO.PARM_IP_ADDRESS, EDataType.VarChar, 50, log.IPAddress),
                this.GetParameter(MailSendLogDAO.PARM_ADD_DATE, EDataType.DateTime, log.AddDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM siteserver_MailSendLog WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll()
        {
            string sqlString = "DELETE FROM siteserver_MailSendLog";

            this.ExecuteNonQuery(sqlString);
        }

        public string GetSelectCommend()
        {
            return "SELECT ID, PublishmentSystemID, ChannelID, ContentID, Title, PageUrl, Receiver, Mail, Sender, IPAddress, AddDate FROM siteserver_MailSendLog";
        }

        public string GetSelectCommend(int publishmentSystemID, string keyword, string dateFrom, string dateTo)
        {
            if (publishmentSystemID == 0 && string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
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

            if (!string.IsNullOrEmpty(keyword))
            {
                if (isWhere)
                {
                    whereString.Append(" AND ");
                }
                isWhere = true;
                whereString.AppendFormat("(Title LIKE '%{0}%' OR PageUrl LIKE '%{0}%' OR Receiver LIKE '%{0}%' OR Mail LIKE '%{0}%' OR Sender LIKE '%{0}%')",PageUtils.FilterSql(keyword));
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
                    whereString.AppendFormat("(to_char(AddDate,'YYYY-MM-DD') <= '{0}')", dateTo);
                }
                else
                {
                    whereString.AppendFormat("(AddDate <= '{0}')", dateTo);
                }
            }

            return "SELECT ID, PublishmentSystemID, ChannelID, ContentID, Title, PageUrl, Receiver, Mail, Sender, IPAddress, AddDate FROM siteserver_MailSendLog " + whereString.ToString();
        }
    }
}
