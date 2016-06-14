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
    public class LeadDAO : DataProviderBase, ILeadDAO
    {
        private const string TABLE_NAME = "crm_Lead";

        public int Insert(LeadInfo leadInfo)
        {
            int leadID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(leadInfo.ToNameValueCollection(), TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        leadID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return leadID;
        }

        public void Update(LeadInfo leadInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(leadInfo.ToNameValueCollection(), TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateStatus(int leadID, ELeadStatus status)
        {
            string sqlString = string.Format("UPDATE {0} SET Status = '{1}' WHERE ID = {2}", TABLE_NAME, ELeadStatusUtils.GetValue(status), leadID);
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

        public LeadInfo GetLeadInfo(int leadID)
        {
            LeadInfo leadInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", leadID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    leadInfo = new LeadInfo(rdr);
                }
                rdr.Close();
            }

            return leadInfo;
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", TABLE_NAME);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentID(int departmentID)
        {
            ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, false);
            if (userNameArrayList.Count > 0)
            {
                string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE ChargeUserName IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList));
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

        public int GetCountByDepartmentIDAndStatus(int departmentID, ELeadStatus status, DateTime begin, DateTime end)
        {
            ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, false);
            if (userNameArrayList.Count > 0)
            {
                string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE ChargeUserName IN ({1}) AND Status = '{2}' AND (AddDate BETWEEN '{3}' AND '{4}')", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList), ELeadStatusUtils.GetValue(status), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            }
            return 0;
        }

        public int GetCountByChargeUserName(string chargeUserName, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE ChargeUserName ='{1}' AND (AddDate BETWEEN '{2}' AND '{3}')", TABLE_NAME, chargeUserName, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByChargeUserNameAndStatus(string chargeUserName, ELeadStatus status, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE ChargeUserName ='{1}' AND Status = '{2}' AND (AddDate BETWEEN '{3}' AND '{4}')", TABLE_NAME, chargeUserName, ELeadStatusUtils.GetValue(status), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public Dictionary<string, int> GetSourceDictionaryByDepartmentID(int departmentID, DateTime begin, DateTime end)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList(departmentID, false);
            if (userNameArrayList.Count > 0)
            {
                string sqlString = string.Format("SELECT Source, COUNT(*) FROM {0} WHERE ChargeUserName IN ({1}) AND (AddDate BETWEEN '{2}' AND '{3}') GROUP BY Source ORDER BY Source", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        string source = rdr.GetValue(0).ToString();
                        int count = rdr.GetInt32(1);
                        dictionary.Add(source, count);
                    }
                    rdr.Close();
                }
            }

            return dictionary;
        }

        public Dictionary<string, int> GetSourceDictionaryByChargeUserName(string chargeUserName, DateTime begin, DateTime end)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            string sqlString = string.Format("SELECT Source, COUNT(*) FROM {0} WHERE ChargeUserName = '{1}' AND (AddDate BETWEEN '{2}' AND '{3}') GROUP BY Source ORDER BY Source", TABLE_NAME, chargeUserName, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    string source = rdr.GetValue(0).ToString();
                    int count = rdr.GetInt32(1);
                    dictionary.Add(source, count);
                }
                rdr.Close();
            }

            return dictionary;
        }

        //public string GetSelectStringByStatus(string userName, ELeadStatus status)
        //{
        //    StringBuilder whereBuilder = new StringBuilder();
        //    if (!string.IsNullOrEmpty(userName))
        //    {
        //        whereBuilder.AppendFormat("WHERE UserName = '{0}' AND ", userName);
        //    }
        //    else
        //    {
        //        whereBuilder.Append("WHERE ");
        //    }
        //    whereBuilder.AppendFormat(" Status = '{0}' ", ELeadStatusUtils.GetValue(status));

        //    whereBuilder.Append(" ORDER BY ID DESC");
        //    return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, whereBuilder.ToString());
        //}

        public string GetSelectString(string userName)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                whereString = string.Format("WHERE (AddUserName = '{0}' AND ChargeUserName = '') OR ChargeUserName = '{0}'", userName);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public string GetSelectString(string userName, string status, string keyword)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                whereString += string.Format(" ((AddUserName = '{0}' AND ChargeUserName = '') OR ChargeUserName = '{0}')", userName);
            }
            if (!string.IsNullOrEmpty(status))
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
                whereString += string.Format(" (Subject LIKE '%{0}%' OR AccountName LIKE '%{0}%' OR Website LIKE '%{0}%' OR Province LIKE '%{0}%' OR City LIKE '%{0}%' OR Area LIKE '%{0}%' OR Address LIKE '%{0}%' OR ContactName LIKE '%{0}%' OR Mobile LIKE '%{0}%' OR Telephone LIKE '%{0}%' OR Email LIKE '%{0}%' OR QQ LIKE '%{0}%' OR BackgroundInfo LIKE '%{0}%' OR ChatOrNote LIKE '%{0}%') ", keyword);
            }

            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = "WHERE" + whereString;
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public string GetOrderByString()
        {
            return "ORDER BY Priority DESC, ID DESC";
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
