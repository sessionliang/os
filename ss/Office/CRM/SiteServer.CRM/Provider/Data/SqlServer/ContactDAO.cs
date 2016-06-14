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
    public class ContactDAO : DataProviderBase, IContactDAO
    {
        private const string TABLE_NAME = "crm_Contact";

        public int Insert(ContactInfo contactInfo)
        {
            int contactID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(contactInfo.ToNameValueCollection(), TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        contactID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return contactID;
        }

        public void Update(ContactInfo contactInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(contactInfo.ToNameValueCollection(), TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(List<int> deleteIDList)
        {
            if (deleteIDList != null && deleteIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public ContactInfo GetContactInfo(string loginName, string addUserName, int accountID, int leadID, NameValueCollection form)
        {
            ContactInfo contactInfo = new ContactInfo(form);

            contactInfo.LoginName = loginName;
            contactInfo.AddUserName = addUserName;
            contactInfo.AccountID = accountID;
            contactInfo.LeadID = leadID;

            return contactInfo;
        }

        public ContactInfo GetContactInfo(int contactID)
        {
            ContactInfo contactInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", contactID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    contactInfo = new ContactInfo(rdr);
                }
                rdr.Close();
            }

            return contactInfo;
        }

        public List<ContactInfo> GetContactInfoList(int leadID)
        {
            List<ContactInfo> list = new List<ContactInfo>();

            string SQL_WHERE = string.Format("WHERE LeadID = {0}", leadID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    ContactInfo contactInfo = new ContactInfo(rdr);
                    list.Add(contactInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public int GetCountByAccountID(int accountID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (AccountID = {1})", TABLE_NAME, accountID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByLeadID(int leadID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (LeadID = {1})", TABLE_NAME, leadID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectString(string addUserName)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(addUserName))
            {
                whereString = string.Format("WHERE AddUserName = '{0}'", addUserName);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSelectString(string addUserName, string keyword)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(addUserName))
            {
                whereString += string.Format(" AddUserName = '{0}'", addUserName);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                if (whereString.Length > 0)
                {
                    whereString += " AND ";
                }
                whereString += string.Format(" (RealName LIKE '%{0}%' OR JobTitle LIKE '%{0}%' OR AccountRole LIKE '%{0}%' OR Mobile LIKE '%{0}%' OR Telephone LIKE '%{0}%' OR Email LIKE '%{0}%' OR QQ LIKE '%{0}%') ", keyword);
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
