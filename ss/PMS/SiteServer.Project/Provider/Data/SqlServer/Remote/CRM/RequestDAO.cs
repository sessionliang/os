using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Model;
using SiteServer.Project.Core;


namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class RequestDAO : DataProviderBase, IRequestDAO
	{
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }
        public int Insert(RequestInfo requestInfo)
        {
            int requestID = 0;

            requestInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(requestInfo.Attributes, this.ConnectionString, RequestInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        requestID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, RequestInfo.TableName);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return requestID;
        }

        public void Update(RequestInfo requestInfo)
        {
            requestInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(requestInfo.Attributes, this.ConnectionString, RequestInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateStatus(int requestID, ERequestStatus status)
        {
            string sqlString = string.Format("UPDATE {0} SET Status = '{1}' WHERE ID = {2}", RequestInfo.TableName, ERequestStatusUtils.GetValue(status), requestID);
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", RequestInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public RequestInfo GetRequestInfo(NameValueCollection form)
        {
            RequestInfo requestInfo = new RequestInfo();

            foreach (string name in form.AllKeys)
            {
                if (RequestAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        requestInfo.SetExtendedAttribute(name, value.Trim());
                    }
                }
            }

            return requestInfo;
        }

        public RequestInfo GetRequestInfo(int requestID)
        {
            RequestInfo requestInfo = null;
            if (requestID > 0)
            {
                string SQL_WHERE = string.Format("WHERE ID = {0}", requestID);
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, RequestInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                {
                    if (rdr.Read())
                    {
                        requestInfo = new RequestInfo();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, requestInfo);
                    }
                    rdr.Close();
                }

                if (requestInfo != null) requestInfo.AfterExecuteReader();
            }            
            return requestInfo;
        }

        public RequestInfo GetLastRequestInfo(string domain)
        {
            RequestInfo requestInfo = null;
            if (!string.IsNullOrEmpty(domain))
            {
                string SQL_WHERE = string.Format("WHERE Domain = '{0}'", domain);
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, RequestInfo.TableName, 1, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                {
                    if (rdr.Read())
                    {
                        requestInfo = new RequestInfo();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, requestInfo);
                    }
                    rdr.Close();
                }

                if (requestInfo != null) requestInfo.AfterExecuteReader();
            }
            return requestInfo;
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", RequestInfo.TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public int GetCountByDepartmentID(int departmentID)
        {
            ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, false);
            if (userNameArrayList.Count > 0)
            {
                string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE AddUserName IN ({1})", RequestInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList));
                return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
            }
            return 0;
        }

        public int GetCountByDepartmentID(int departmentID, DateTime begin, DateTime end)
        {
            ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, false);
            if (userNameArrayList.Count > 0)
            {
                string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE AddUserName IN ({1}) AND (AddDate BETWEEN '{2}' AND '{3}')", RequestInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
                return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
            }
            return 0;
        }

        public int GetCountByDepartmentIDAndStatus(int departmentID, ERequestStatus status, DateTime begin, DateTime end)
        {
            ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, false);
            if (userNameArrayList.Count > 0)
            {
                string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE AddUserName IN ({1}) AND Status = '{2}' AND (AddDate BETWEEN '{3}' AND '{4}')", RequestInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList), ERequestStatusUtils.GetValue(status), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
                return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
            }
            return 0;
        }

        public int GetCountByAddUserName(string chargeUserName, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE AddUserName ='{1}' AND (AddDate BETWEEN '{2}' AND '{3}')", RequestInfo.TableName, chargeUserName, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public int GetCountByAddUserNameAndStatus(string chargeUserName, ERequestStatus status, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE AddUserName ='{1}' AND Status = '{2}' AND (AddDate BETWEEN '{3}' AND '{4}')", RequestInfo.TableName, chargeUserName, ERequestStatusUtils.GetValue(status), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public string GetSelectStringByStatus(string chargeUserName, ERequestStatus status)
        {
            StringBuilder whereBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(chargeUserName))
            {
                whereBuilder.AppendFormat("WHERE ChargeUserName = '{0}' AND ", chargeUserName);
            }
            else
            {
                whereBuilder.Append("WHERE ");
            }
            whereBuilder.AppendFormat(" Status = '{0}' ", ERequestStatusUtils.GetValue(status));

            whereBuilder.Append(" ORDER BY ID DESC");
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, RequestInfo.TableName, 0, SqlUtils.Asterisk, whereBuilder.ToString(), null);
        }

        public string GetSelectString(string userName)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                whereString = string.Format("WHERE AddUserName = '{0}' OR ChargeUserName = '{0}'", userName);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, RequestInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSelectString(string userName, string status, string keyword)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                whereString += string.Format(" (AddUserName = '{0}' OR ChargeUserName = '{0}')", userName);
            }
            if (!string.IsNullOrEmpty(status) && status == ERequestStatusUtils.GetValue(ERequestStatus.Closed))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (Status = '{0}')", status);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (RequestSN LIKE '%{0}%' OR Subject LIKE '%{0}%' OR Website LIKE '%{0}%' OR Email LIKE '%{0}%' OR Mobile LIKE '%{0}%' OR QQ LIKE '%{0}%') ", keyword);
            }

            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = "WHERE" + whereString;
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, RequestInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSelectString(int licenseID, string domain)
        {
            string whereString = string.Format("WHERE LicenseID = {0} AND Domain = '{1}'", licenseID, domain.ToLower());
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, RequestInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSelectString(int licenseID, string domain, string status, string keyword)
        {
            string whereString = string.Format(" LicenseID = {0} AND Domain = '{1}'", licenseID, domain.ToLower());
            if (!string.IsNullOrEmpty(status) && status == ERequestStatusUtils.GetValue(ERequestStatus.Closed))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (Status = '{0}')", status);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (RequestSN LIKE '%{0}%' OR Subject LIKE '%{0}%' OR Website LIKE '%{0}%' OR Email LIKE '%{0}%' OR Mobile LIKE '%{0}%' OR QQ LIKE '%{0}%') ", keyword);
            }

            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = "WHERE" + whereString;
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, RequestInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
