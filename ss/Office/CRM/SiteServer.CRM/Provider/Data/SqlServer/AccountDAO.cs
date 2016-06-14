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
using SiteServer.CRM.Model;
using SiteServer.CRM.Core;

using System.Collections.Generic;

namespace SiteServer.CRM.Provider.Data.SqlServer
{
    public class AccountDAO : DataProviderBase, IAccountDAO
    {
        private const string TABLE_NAME = "crm_Account";

        public int Insert(AccountInfo accountInfo)
        {
            int accountID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(accountInfo.ToNameValueCollection(), TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        accountID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return accountID;
        }

        public void Update(AccountInfo accountInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(accountInfo.ToNameValueCollection(), TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateStatus(int accountID, EAccountStatus status)
        {
            string sqlString = string.Format("UPDATE {0} SET Status = '{1}' WHERE ID = {2}", TABLE_NAME, EAccountStatusUtils.GetValue(status), accountID);
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(List<int> deleteIDList)
        {
            if (deleteIDList != null && deleteIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public AccountInfo GetAccountInfo(int accountID)
        {
            AccountInfo accountInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", accountID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    accountInfo = new AccountInfo(rdr);
                }
                rdr.Close();
            }

            return accountInfo;
        }

        public string GetAccountName(int accountID)
        {
            string accountName = string.Empty;

            string sqlString = string.Format("SELECT AccountName FROM {0} WHERE ID = {1}", TABLE_NAME, accountID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    accountName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return accountName;
        }

        public int GetCount(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE {1} = {2}", TABLE_NAME, AccountAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentID(int publishmentSystemID, int departmentID)
        {
            ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, false);
            if (userNameArrayList.Count > 0)
            {
                string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE {1} = {2} AND ChargeUserName IN ({3})", TABLE_NAME, AccountAttribute.PublishmentSystemID, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList));
                return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            }
            return 0;
        }

        public int GetCountByDepartmentID(int departmentID, DateTime begin, DateTime end)
        {
            ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, false);
            if (userNameArrayList.Count > 0)
            {
                string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE ChargeUserName IN ({1}) AND (AddDate BETWEEN '{2}' AND '{3}')", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
                return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            }
            return 0;
        }

        public int GetCountByDepartmentIDAndStatus(int departmentID, EAccountStatus status, DateTime begin, DateTime end)
        {
            ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, false);
            if (userNameArrayList.Count > 0)
            {
                string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE ChargeUserName IN ({1}) AND Status = '{2}' AND (AddDate BETWEEN '{3}' AND '{4}')", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList), EAccountStatusUtils.GetValue(status), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            }
            return 0;
        }

        public int GetCountByUserName(string chargeUserName, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE ChargeUserName ='{1}' AND (AddDate BETWEEN '{2}' AND '{3}')", TABLE_NAME, chargeUserName, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByUserNameAndStatus(string chargeUserName, EAccountStatus status, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE ChargeUserName ='{1}' AND Status = '{2}' AND (AddDate BETWEEN '{3}' AND '{4}')", TABLE_NAME, chargeUserName, EAccountStatusUtils.GetValue(status), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectStringByStatus(string chargeUserName, EAccountStatus status)
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
            whereBuilder.AppendFormat(" Status = '{0}' ", EAccountStatusUtils.GetValue(status));

            whereBuilder.Append(" ORDER BY ID DESC");
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, whereBuilder.ToString(), null);
        }

        public string GetSelectString(string userName)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                whereString = string.Format("WHERE AddUserName = '{0}' OR ChargeUserName = '{0}'", userName);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSelectString(string userName, string accountType, string keyword)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                whereString += string.Format(" (AddUserName = '{0}' OR ChargeUserName = '{0}')", userName);
            }
            if (!string.IsNullOrEmpty(accountType))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (AccountType = '{0}')", accountType);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (BusinessType LIKE '%{0}%' OR Classification LIKE '%{0}%' OR Website LIKE '%{0}%' OR Province LIKE '%{0}%' OR City LIKE '%{0}%' OR Area LIKE '%{0}%' OR Address LIKE '%{0}%' OR Description LIKE '%{0}%') ", keyword);
            }

            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = "WHERE" + whereString;
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
