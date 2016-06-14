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
    public class GovInteractLogDAO : DataProviderBase, IGovInteractLogDAO
    {
        private const string PARM_LOG_ID = "@LogID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_CONTENT_ID = "@ContentID";
        private const string PARM_DEPARTMENT_ID = "@DepartmentID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_LOG_TYPE = "@LogType";
        private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_SUMMARY = "@Summary";

        public void Insert(GovInteractLogInfo logInfo)
        {
            string sqlString = "INSERT INTO siteserver_GovInteractLog(PublishmentSystemID, NodeID, ContentID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary) VALUES (@PublishmentSystemID, @NodeID, @ContentID, @DepartmentID, @UserName, @LogType, @IPAddress, @AddDate, @Summary)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_GovInteractLog(LogID, PublishmentSystemID, NodeID, ContentID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary) VALUES (siteserver_GovInteractLog_SEQ.NEXTVAL, @PublishmentSystemID, @NodeID, @ContentID, @DepartmentID, @UserName, @LogType, @IPAddress, @AddDate, @Summary)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(GovInteractLogDAO.PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, logInfo.PublishmentSystemID),
                this.GetParameter(GovInteractLogDAO.PARM_NODE_ID, EDataType.Integer, logInfo.NodeID),
                this.GetParameter(GovInteractLogDAO.PARM_CONTENT_ID, EDataType.Integer, logInfo.ContentID),
                this.GetParameter(GovInteractLogDAO.PARM_DEPARTMENT_ID, EDataType.Integer, logInfo.DepartmentID),
				this.GetParameter(GovInteractLogDAO.PARM_USER_NAME, EDataType.VarChar, 50, logInfo.UserName),
                this.GetParameter(GovInteractLogDAO.PARM_LOG_TYPE, EDataType.VarChar, 50, EGovInteractLogTypeUtils.GetValue(logInfo.LogType)),
				this.GetParameter(GovInteractLogDAO.PARM_IP_ADDRESS, EDataType.VarChar, 50, logInfo.IPAddress),
                this.GetParameter(GovInteractLogDAO.PARM_ADD_DATE, EDataType.DateTime, logInfo.AddDate),
				this.GetParameter(GovInteractLogDAO.PARM_SUMMARY, EDataType.NVarChar, 255, logInfo.Summary)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM siteserver_GovInteractLog WHERE LogID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll(int publishmentSystemID)
        {
            string sqlString = string.Format("DELETE FROM siteserver_GovInteractLog WHERE PublishmentSystemID = {0}", publishmentSystemID);

            this.ExecuteNonQuery(sqlString);
        }

        public IEnumerable GetDataSourceByContentID(int publishmentSystemID, int contentID)
        {
            string sqlString = string.Format("SELECT LogID, PublishmentSystemID, NodeID, ContentID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM siteserver_GovInteractLog WHERE PublishmentSystemID = {0} AND ContentID = {1} ORDER BY LogID", publishmentSystemID, contentID);

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public ArrayList GetLogInfoArrayList(int publishmentSystemID, int contentID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT LogID, PublishmentSystemID, NodeID, ContentID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM siteserver_GovInteractLog WHERE PublishmentSystemID = {0} AND ContentID = {1} ORDER BY LogID", publishmentSystemID, contentID);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    GovInteractLogInfo logInfo = new GovInteractLogInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetValue(5).ToString(), EGovInteractLogTypeUtils.GetEnumType(rdr.GetValue(6).ToString()), rdr.GetValue(7).ToString(), rdr.GetDateTime(8), rdr.GetValue(9).ToString());
                    arraylist.Add(logInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public string GetSelectCommend(int publishmentSystemID)
        {
            return string.Format("SELECT LogID, PublishmentSystemID, NodeID, ContentID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM siteserver_GovInteractLog WHERE PublishmentSystemID = {0}", publishmentSystemID);
        }

        public string GetSelectCommend(int publishmentSystemID, string keyword, string dateFrom, string dateTo)
        {
            if (string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
            {
                return this.GetSelectCommend(publishmentSystemID);
            }

            StringBuilder whereString = new StringBuilder();
            whereString.AppendFormat("WHERE (PublishmentSystemID = {0})", publishmentSystemID);

            if (!string.IsNullOrEmpty(keyword))
            {
                whereString.AppendFormat(" AND (UserName LIKE '%{0}%' OR Summary LIKE '%{0}%')", PageUtils.FilterSql(keyword));
            }

            if (!string.IsNullOrEmpty(dateFrom))
            {
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereString.AppendFormat(" AND (to_char(AddDate,'YYYY-MM-DD') >= '{0}')", dateFrom);
                }
                else
                {
                    whereString.AppendFormat(" AND (AddDate >= '{0}')", dateFrom);
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereString.AppendFormat(" AND (to_char(AddDate,'YYYY-MM-DD') <= '{0}')", dateFrom);
                }
                else
                {
                    whereString.AppendFormat(" AND (AddDate <= '{0}')", dateTo);
                }
            }

            return "SELECT LogID, PublishmentSystemID, NodeID, ContentID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM siteserver_GovInteractLog " + whereString.ToString();
        }
    }
}
