using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CRM.Model;
using SiteServer.CRM.Core;
using System.Text;
using BaiRong.Model;

namespace SiteServer.CRM.Provider.Data.SqlServer
{
    public class ProjectLogDAO : DataProviderBase, IProjectLogDAO
    {
        private const string PARM_LOG_ID = "@LogID";
        private const string PARM_APPLY_ID = "@ApplyID";
        private const string PARM_DEPARTMENT_ID = "@DepartmentID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_LOG_TYPE = "@LogType";
        private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_SUMMARY = "@Summary";

        public void Insert(ProjectLogInfo logInfo)
        {
            string sqlString = "INSERT INTO pms_ProjectLog(ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary) VALUES (@ApplyID, @DepartmentID, @UserName, @LogType, @IPAddress, @AddDate, @Summary)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO pms_ProjectLog(LogID, ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary) VALUES (pms_ProjectLog_SEQ.NEXTVAL, @ApplyID, @DepartmentID, @UserName, @LogType, @IPAddress, @AddDate, @Summary)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(ProjectLogDAO.PARM_APPLY_ID, EDataType.Integer, logInfo.ApplyID),
                this.GetParameter(ProjectLogDAO.PARM_DEPARTMENT_ID, EDataType.Integer, logInfo.DepartmentID),
				this.GetParameter(ProjectLogDAO.PARM_USER_NAME, EDataType.VarChar, 50, logInfo.UserName),
                this.GetParameter(ProjectLogDAO.PARM_LOG_TYPE, EDataType.VarChar, 50, EProjectLogTypeUtils.GetValue(logInfo.LogType)),
				this.GetParameter(ProjectLogDAO.PARM_IP_ADDRESS, EDataType.VarChar, 50, logInfo.IPAddress),
                this.GetParameter(ProjectLogDAO.PARM_ADD_DATE, EDataType.DateTime, logInfo.AddDate),
				this.GetParameter(ProjectLogDAO.PARM_SUMMARY, EDataType.NVarChar, 255, logInfo.Summary)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null || idArrayList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM pms_ProjectLog WHERE LogID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll()
        {
            string sqlString = "DELETE FROM pms_ProjectLog";

            this.ExecuteNonQuery(sqlString);
        }

        public IEnumerable GetDataSourceByApplyID(int applyID)
        {
            string sqlString = string.Format("SELECT LogID, ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM pms_ProjectLog WHERE ApplyID = {0}", applyID);

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public ArrayList GetLogInfoArrayList(int applyID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT LogID, ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM pms_ProjectLog WHERE ApplyID = {0}", applyID);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    ProjectLogInfo logInfo = new ProjectLogInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), EProjectLogTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetDateTime(6), rdr.GetValue(7).ToString());
                    arraylist.Add(logInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public string GetSelectCommend()
        {
            return "SELECT LogID, ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM pms_ProjectLog";
        }

        public string GetSelectCommend(string keyword, string dateFrom, string dateTo)
        {
            if (string.IsNullOrEmpty(keyword) && string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
            {
                return this.GetSelectCommend();
            }

            StringBuilder whereString = new StringBuilder();

            if (!string.IsNullOrEmpty(keyword))
            {
                whereString.AppendFormat(" AND (UserName LIKE '%{0}%' OR Summary LIKE '%{0}%')", keyword);
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

            return "SELECT LogID, ApplyID, DepartmentID, UserName, LogType, IPAddress, AddDate, Summary FROM pms_ProjectLog " + whereString.ToString();
        }
    }
}
