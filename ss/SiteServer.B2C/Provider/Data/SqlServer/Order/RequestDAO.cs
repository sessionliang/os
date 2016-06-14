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
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;

using System.Collections.Generic;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class RequestDAO : DataProviderBase, IRequestDAO
    {
        private const string TABLE_NAME = "b2c_Request";

        public const string PARM_USER_NAME = "@UserName";

        public int Insert(RequestInfo requestInfo)
        {
            int requestID = 0;

            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(requestInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        requestID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

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
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(requestInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateStatus(int requestID, ERequestStatus status)
        {
            string sqlString = string.Format("UPDATE {0} SET Status = '{1}' WHERE ID = {2}", TABLE_NAME, ERequestStatusUtils.GetValue(status), requestID);
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public RequestInfo GetRequestInfo(int requestID)
        {
            RequestInfo requestInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", requestID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    requestInfo = new RequestInfo(rdr);
                }
                rdr.Close();
            }

            return requestInfo;
        }

        public List<RequestInfo> GetRequestInfoList(string userName)
        {
            List<RequestInfo> requestInfoList = new List<RequestInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = @UserName", RequestAttribute.UserName);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {                    
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)	                    　
            };
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, selectParms))
            {
                while (rdr.Read())
                {
                    RequestInfo requestInfo = new RequestInfo(rdr);
                    requestInfoList.Add(requestInfo);
                }
                rdr.Close();
            }

            return requestInfoList;
        }

        public RequestInfo GetLastRequestInfo(string userName)
        {
            RequestInfo requestInfo = null;

            if (!string.IsNullOrEmpty(userName))
            {
                string SQL_WHERE = string.Format("WHERE {0} = @UserName", RequestAttribute.UserName);
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 1, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");
                IDbDataParameter[] selectParms = new IDbDataParameter[]
                {                    
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)	                    　
                };
                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, selectParms))
                {
                    if (rdr.Read())
                    {
                        requestInfo = new RequestInfo(rdr);
                    }
                    rdr.Close();
                }
            }

            return requestInfo;
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", TABLE_NAME);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public string GetSelectString(string userName)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                whereString = string.Format("WHERE {0} = '{1}'", RequestAttribute.UserName, userName);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSelectString(string userName, string status, string keyword)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                whereString += string.Format(" ({0} = '{1}')", RequestAttribute.UserName, userName);
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
                whereString += string.Format(" ({0} LIKE '%{1}%' OR {2} LIKE '%{0}%' OR {3} LIKE '%{0}%' OR {4} LIKE '%{0}%' OR {5} LIKE '%{0}%' OR {6} LIKE '%{0}%' OR {7} LIKE '%{0}%' OR {8} LIKE '%{0}%') ", RequestAttribute.SN, keyword, RequestAttribute.AdminUserName, RequestAttribute.RequestType, RequestAttribute.Subject, RequestAttribute.Website, RequestAttribute.Email, RequestAttribute.Mobile, RequestAttribute.QQ);
            }

            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = "WHERE" + whereString;
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, whereString, null);
        }

        public void Estimate(int requestID, ERequestEstimate estimateValue, string estimateComment)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}', {3} = '{4}', {5} = '{6}', {7} = getdate() WHERE ID = {8}", TABLE_NAME, RequestAttribute.IsEstimate, true, RequestAttribute.EstimateValue, ERequestEstimateUtils.GetValue(estimateValue), RequestAttribute.EstimateComment, estimateComment, RequestAttribute.EstimateDate, requestID);
            this.ExecuteNonQuery(sqlString);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
    }
}
