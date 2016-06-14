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
    public class InvoiceDAO : DataProviderBase, IInvoiceDAO
    {
        private const string TABLE_NAME = "crm_Invoice";

        public int Insert(InvoiceInfo invoiceInfo)
        {
            int invoiceID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(invoiceInfo.ToNameValueCollection(), TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        invoiceID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return invoiceID;
        }

        public void Update(InvoiceInfo invoiceInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(invoiceInfo.ToNameValueCollection(), TABLE_NAME, out parms);

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

        public List<int> GetOrderIDList(List<int> idArrayList)
        {
            string sqlString = string.Format("SELECT OrderID FROM {0} WHERE ID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));
            return BaiRongDataProvider.DatabaseDAO.GetIntList(sqlString);
        }

        public InvoiceInfo GetInvoiceInfo(int invoiceID)
        {
            InvoiceInfo invoiceInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", invoiceID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    invoiceInfo = new InvoiceInfo(rdr);
                }
                rdr.Close();
            }

            return invoiceInfo;
        }

        public InvoiceInfo GetInvoiceInfoByOrderID(int orderID)
        {
            InvoiceInfo invoiceInfo = null;
            string SQL_WHERE = string.Format("WHERE OrderID = {0}", orderID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    invoiceInfo = new InvoiceInfo(rdr);
                }
                rdr.Close();
            }

            return invoiceInfo;
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
                whereString += string.Format(" (SN LIKE '%{0}%' OR InvoiceTitle LIKE '%{0}%' OR InvoiceReceiver LIKE '%{0}%' OR InvoiceTel LIKE '%{0}%' OR InvoiceAddress LIKE '%{0}%' OR VATTaxpayerID LIKE '%{0}%' OR VATBankName LIKE '%{0}%' OR VATBankCardNo LIKE '%{0}%' OR Summary LIKE '%{0}%') ", keyword);
            }

            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = "WHERE" + whereString;
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, 0, SqlUtils.Asterisk, whereString, null);
        }

        public string GetInvoiceByString()
        {
            return "ORDER BY ID DESC";
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
