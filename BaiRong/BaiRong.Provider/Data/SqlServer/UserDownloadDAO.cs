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
    public class UserDownloadDAO : DataProviderBase, IUserDownloadDAO
	{
        public void Insert(UserDownloadInfo downloadInfo)
        {
            downloadInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(downloadInfo.Attributes, UserDownloadInfo.TableName, out parms);
            this.ExecuteNonQuery(SQL_INSERT, parms);
        }

        public void Update(UserDownloadInfo downloadInfo)
        {
            downloadInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(downloadInfo.Attributes, UserDownloadInfo.TableName, out parms);
            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", UserDownloadInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int downloadID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID ={1}", UserDownloadInfo.TableName, downloadID);
            this.ExecuteNonQuery(sqlString);
        }

        public UserDownloadInfo GetDownloadInfo(string createUserName, int taxis, int downloads, NameValueCollection form)
        {
            UserDownloadInfo downloadInfo = new UserDownloadInfo(0, createUserName, taxis, downloads);

            foreach (string name in form.AllKeys)
            {
                if (UserContactAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        downloadInfo.SetExtendedAttribute(name, value);
                    }
                }
            }

            return downloadInfo;
        }

        public UserDownloadInfo GetDownloadInfo(int downloadID)
        {
            UserDownloadInfo downloadInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", downloadID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(UserDownloadInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    downloadInfo = new UserDownloadInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, downloadInfo);
                }
                rdr.Close();
            }

            if (downloadInfo != null) downloadInfo.AfterExecuteReader();
            return downloadInfo;
        }

        public string GetSelectString()
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.Append(" ORDER BY ID DESC");
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(UserDownloadInfo.TableName, SqlUtils.Asterisk, whereBuilder.ToString());
        }

        public string GetSelectString(string createUserName)
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.AppendFormat(" WHERE CreateUserName = '{0}' ORDER BY ID DESC", PageUtils.FilterSql(createUserName));
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(UserDownloadInfo.TableName, SqlUtils.Asterisk, whereBuilder.ToString());
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
