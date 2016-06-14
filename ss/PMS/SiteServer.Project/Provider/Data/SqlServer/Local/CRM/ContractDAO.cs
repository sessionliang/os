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
    public class ContractDAO : DataProviderBase, IContractDAO
    {
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }
        public int Insert(ContractInfo contractInfo)
        {
            int contractID = 0;

            contractInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(contractInfo.Attributes, this.ConnectionString, ContractInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        contractID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, ContractInfo.TableName);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return contractID;
        }

        public void Update(ContractInfo contractInfo)
        {
            contractInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(contractInfo.Attributes, this.ConnectionString, ContractInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", ContractInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public ArrayList GetOrderIDArrayList(ArrayList idArrayList)
        {
            string sqlString = string.Format("SELECT OrderID FROM {0} WHERE ID IN ({1})", ContractInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));
            return BaiRongDataProvider.DatabaseDAO.GetIntArrayList(this.ConnectionString, sqlString);
        }

        public ContractInfo GetContractInfo(NameValueCollection form)
        {
            ContractInfo contractInfo = new ContractInfo(0);

            foreach (string name in form.AllKeys)
            {
                if (ContractAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        contractInfo.SetExtendedAttribute(name, value.Trim());
                    }
                }
            }

            return contractInfo;
        }

        public ContractInfo GetContractInfo(int contractID)
        {
            ContractInfo contractInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", contractID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ContractInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    contractInfo = new ContractInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, contractInfo);
                }
                rdr.Close();
            }

            if (contractInfo != null) contractInfo.AfterExecuteReader();
            return contractInfo;
        }

        public ContractInfo GetContractInfoByOrderID(int orderID)
        {
            ContractInfo contractInfo = null;
            string SQL_WHERE = string.Format("WHERE OrderID = {0}", orderID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ContractInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    contractInfo = new ContractInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, contractInfo);
                }
                rdr.Close();
            }

            if (contractInfo != null) contractInfo.AfterExecuteReader();
            return contractInfo;
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", ContractInfo.TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public string GetSelectString()
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ContractInfo.TableName, 0, SqlUtils.Asterisk, null, null);
        }

        public string GetSelectString(string keyword)
        {
            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(keyword))
            {
                whereString += string.Format(" (SN LIKE '%{0}%' OR ContractTitle LIKE '%{0}%' OR ContractReceiver LIKE '%{0}%' OR ContractTel LIKE '%{0}%' OR ContractAddress LIKE '%{0}%' OR Summary LIKE '%{0}%') ", keyword);
            }

            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = "WHERE" + whereString;
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ContractInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetContractByString()
        {
            return "ORDER BY ID DESC";
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
