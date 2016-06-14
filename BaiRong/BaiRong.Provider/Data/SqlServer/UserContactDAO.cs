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


namespace BaiRong.Provider.Data.SqlServer
{
    public class UserContactDAO : DataProviderBase, IUserContactDAO
	{
        public void Insert(UserContactInfo contactInfo)
        {
            contactInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(contactInfo.Attributes, UserContactInfo.TableName, out parms);
            this.ExecuteNonQuery(SQL_INSERT, parms);
        }

        public void Update(UserContactInfo contactInfo)
        {
            contactInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(contactInfo.Attributes, UserContactInfo.TableName, out parms);
            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", UserContactInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int contactID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", UserContactInfo.TableName, contactID);
            this.ExecuteNonQuery(sqlString);
        }

        public UserContactInfo GetContactInfo(string relatedUserName, string createUserName, int taxis, NameValueCollection form)
        {
            UserContactInfo contactInfo = new UserContactInfo(0, relatedUserName, createUserName, taxis);

            foreach (string name in form.AllKeys)
            {
                if (UserContactAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        contactInfo.SetExtendedAttribute(name, value);
                    }
                }
            }

            return contactInfo;
        }

        public UserContactInfo GetContactInfo(int contactID)
        {
            UserContactInfo contactInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", contactID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(UserContactInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    contactInfo = new UserContactInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, contactInfo);
                }
                rdr.Close();
            }

            if (contactInfo != null) contactInfo.AfterExecuteReader();
            return contactInfo;
        }

        public UserContactInfo GetContactInfo(string relatedUserName)
        {
            UserContactInfo contactInfo = null;
            string SQL_WHERE = string.Format("WHERE RelatedUserName = '{0}'", PageUtils.FilterSql(relatedUserName));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(UserContactInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    contactInfo = new UserContactInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, contactInfo);
                }
                rdr.Close();
            }

            if (contactInfo != null) contactInfo.AfterExecuteReader();
            return contactInfo;
        }

        public string GetSelectString()
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.Append(" ORDER BY ID DESC");
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(UserContactInfo.TableName, SqlUtils.Asterisk, whereBuilder.ToString());
        }

        public string GetSelectString(string createUserName)
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.AppendFormat(" WHERE CreateUserName = '{0}' ORDER BY ID DESC", PageUtils.FilterSql(createUserName));
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(UserContactInfo.TableName, SqlUtils.Asterisk, whereBuilder.ToString());
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
