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
    public class ContractDAO : DataProviderBase, IContractDAO
    {
        private const string TABLE_NAME = "crm_Contract";

        public int Insert(ContractInfo contractInfo)
        {
            int contractID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(contractInfo.ToNameValueCollection(), TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        contractID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

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
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(contractInfo.ToNameValueCollection(), TABLE_NAME, out parms);

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

        public List<int> GetOrderIDList(List<int> idList)
        {
            if (idList != null && idList.Count > 0)
            {
                string sqlString = string.Format("SELECT OrderID FROM {0} WHERE ID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idList));
                return BaiRongDataProvider.DatabaseDAO.GetIntList(sqlString);
            }
            return new List<int>();
        }

        public ContractInfo GetContractInfo(int contractID)
        {
            ContractInfo contractInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", contractID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    contractInfo = new ContractInfo(rdr);
                }
                rdr.Close();
            }

            return contractInfo;
        }

        public ContractInfo GetContractInfoByOrderID(int orderID)
        {
            ContractInfo contractInfo = null;
            string SQL_WHERE = string.Format("WHERE OrderID = {0}", orderID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    contractInfo = new ContractInfo(rdr);
                }
                rdr.Close();
            }

            return contractInfo;
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", TABLE_NAME);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectString()
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, null, null);
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

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, whereString, null);
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
