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

using System.Collections.Generic;

namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class ContactDAO : DataProviderBase, IContactDAO
    {
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }

        public int Insert(ContactInfo contactInfo)
        {
            int contactID = 0;

            contactInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(contactInfo.Attributes, this.ConnectionString, ContactInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        contactID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, ContactInfo.TableName);

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
            contactInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(contactInfo.Attributes, this.ConnectionString, ContactInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", ContactInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public ContactInfo GetContactInfo(string loginName, string addUserName, int accountID, int leadID, NameValueCollection form)
        {
            ContactInfo contactInfo = new ContactInfo(0, loginName, addUserName, DateTime.Now, accountID, leadID);

            foreach (string name in form.AllKeys)
            {
                if (ContactAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        contactInfo.SetExtendedAttribute(name, value.Trim());
                    }
                }
            }

            return contactInfo;
        }

        public ContactInfo GetContactInfo(int contactID)
        {
            ContactInfo contactInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", contactID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ContactInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    contactInfo = new ContactInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, contactInfo);
                }
                rdr.Close();
            }

            if (contactInfo != null) contactInfo.AfterExecuteReader();
            return contactInfo;
        }

        public List<ContactInfo> GetContactInfoList(int leadID)
        {
            List<ContactInfo> list = new List<ContactInfo>();

            string SQL_WHERE = string.Format("WHERE LeadID = {0}", leadID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ContactInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    ContactInfo contactInfo = new ContactInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, contactInfo);
                    contactInfo.AfterExecuteReader();
                    list.Add(contactInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public int GetCountByAccountID(int accountID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (AccountID = {1})", ContactInfo.TableName, accountID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public int GetCountByLeadID(int leadID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (LeadID = {1})", ContactInfo.TableName, leadID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public string GetSelectString(string addUserName)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(addUserName))
            {
                whereString = string.Format("WHERE AddUserName = '{0}'", addUserName);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ContactInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
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

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ContactInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
