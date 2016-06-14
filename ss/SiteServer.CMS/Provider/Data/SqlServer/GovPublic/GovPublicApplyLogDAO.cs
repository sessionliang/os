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
    public class GovPublicApplyLogDAO : DataProviderBase, IGovPublicApplyLogDAO
    {
        private const string PARM_LOG_ID = "@LogID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_APPLY_ID = "@ApplyID";
        private const string PARM_DEPARTMENT_ID = "@DepartmentID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_LOG_TYPE = "@LogType";
        private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_SUMMARY = "@Summary";

        public void Insert(GovPublicApplyLogInfo logInfo)
        {
            string sqlString = "INSERT INTO siteserver_GovPublicApplyLog(PublishmentSystemID, ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary) VALUES (@PublishmentSystemID, @ApplyID, @DepartmentID, @UserName, @LogType, @IPAddress, @AddDate, @Summary)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_GovPublicApplyLog(LogID, PublishmentSystemID, ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary) VALUES (siteserver_GovPublicApplyLog_SEQ.NEXTVAL, @PublishmentSystemID, @ApplyID, @DepartmentID, @UserName, @LogType, @IPAddress, @AddDate, @Summary)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(GovPublicApplyLogDAO.PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, logInfo.PublishmentSystemID),
                this.GetParameter(GovPublicApplyLogDAO.PARM_APPLY_ID, EDataType.Integer, logInfo.ApplyID),
                this.GetParameter(GovPublicApplyLogDAO.PARM_DEPARTMENT_ID, EDataType.Integer, logInfo.DepartmentID),
				this.GetParameter(GovPublicApplyLogDAO.PARM_USER_NAME, EDataType.VarChar, 50, logInfo.UserName),
                this.GetParameter(GovPublicApplyLogDAO.PARM_LOG_TYPE, EDataType.VarChar, 50, EGovPublicApplyLogTypeUtils.GetValue(logInfo.LogType)),
				this.GetParameter(GovPublicApplyLogDAO.PARM_IP_ADDRESS, EDataType.VarChar, 50, logInfo.IPAddress),
                this.GetParameter(GovPublicApplyLogDAO.PARM_ADD_DATE, EDataType.DateTime, logInfo.AddDate),
				this.GetParameter(GovPublicApplyLogDAO.PARM_SUMMARY, EDataType.NVarChar, 255, logInfo.Summary)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM siteserver_GovPublicApplyLog WHERE LogID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll(int publishmentSystemID)
        {
            string sqlString = string.Format("DELETE FROM siteserver_GovPublicApplyLog WHERE PublishmentSystemID = {0}", publishmentSystemID);

            this.ExecuteNonQuery(sqlString);
        }

        public IEnumerable GetDataSourceByApplyID(int applyID)
        {
            string sqlString = string.Format("SELECT LogID, PublishmentSystemID, ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM siteserver_GovPublicApplyLog WHERE ApplyID = {0}", applyID);

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public ArrayList GetLogInfoArrayList(int applyID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT LogID, PublishmentSystemID, ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM siteserver_GovPublicApplyLog WHERE ApplyID = {0}", applyID);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    GovPublicApplyLogInfo logInfo = new GovPublicApplyLogInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetValue(4).ToString(), EGovPublicApplyLogTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), rdr.GetDateTime(7), rdr.GetValue(8).ToString());
                    arraylist.Add(logInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public string GetSelectCommend(int publishmentSystemID)
        {
            return string.Format("SELECT LogID, PublishmentSystemID, ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM siteserver_GovPublicApplyLog WHERE PublishmentSystemID = {0}", publishmentSystemID);
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
                whereString.AppendFormat(" AND (UserName LIKE '%{0}%' OR Summary LIKE '%{0}%')",PageUtils.FilterSql(keyword));
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

            return "SELECT LogID, PublishmentSystemID, ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM siteserver_GovPublicApplyLog " + whereString.ToString();
        }
    }
}
