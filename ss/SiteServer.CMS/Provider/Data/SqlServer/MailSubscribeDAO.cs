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
    public class MailSubscribeDAO : DataProviderBase, SiteServer.CMS.Core.IMailSubscribeDAO
    {
        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_RECEIVER = "@Receiver";
        private const string PARM_MAIL = "@Mail";
        private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_ADD_DATE = "@AddDate";

        private const string SQL_SELECT_ALL = "SELECT ID, PublishmentSystemID, Receiver, Mail, IPAddress, AddDate FROM siteserver_MailSubscribe WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_MAIL = "SELECT Mail FROM siteserver_MailSubscribe WHERE PublishmentSystemID = @PublishmentSystemID AND Mail = @Mail";

        public void Insert(MailSubscribeInfo msInfo)
        {
            string sqlString = "INSERT INTO siteserver_MailSubscribe(PublishmentSystemID, Receiver, Mail, IPAddress, AddDate) VALUES (@PublishmentSystemID, @Receiver, @Mail, @IPAddress, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_MailSubscribe(ID, PublishmentSystemID, Receiver, Mail, IPAddress, AddDate) VALUES (siteserver_MailSubscribe_SEQ.NEXTVAL, @PublishmentSystemID, @Receiver, @Mail, @IPAddress, @AddDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(MailSubscribeDAO.PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, msInfo.PublishmentSystemID),
                this.GetParameter(MailSubscribeDAO.PARM_RECEIVER, EDataType.NVarChar, 255, msInfo.Receiver),
                this.GetParameter(MailSubscribeDAO.PARM_MAIL, EDataType.NVarChar, 255, msInfo.Mail),
				this.GetParameter(MailSubscribeDAO.PARM_IP_ADDRESS, EDataType.VarChar, 50, msInfo.IPAddress),
                this.GetParameter(MailSubscribeDAO.PARM_ADD_DATE, EDataType.DateTime, msInfo.AddDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM siteserver_MailSubscribe WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public bool IsExists(int publishmentSystemID, string mail)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_MAIL, EDataType.NVarChar, 255, mail)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MAIL, parms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public string GetSelectCommend()
        {
            return "SELECT ID, PublishmentSystemID, Receiver, Mail, IPAddress, AddDate FROM siteserver_MailSubscribe";
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
                whereString.AppendFormat("(Receiver LIKE '%{0}%' OR Mail LIKE '%{0}%')",PageUtils.FilterSql(keyword));
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

            return "SELECT ID, PublishmentSystemID, Receiver, Mail, IPAddress, AddDate FROM siteserver_MailSubscribe " + whereString.ToString();
        }

        public ArrayList GetMailSubscribeInfoArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    MailSubscribeInfo msInfo = new MailSubscribeInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetDateTime(5));
                    arraylist.Add(msInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }
    }
}
