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
    public class InvoiceDAO : DataProviderBase, IInvoiceDAO
    {
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.InnerConnectionString;
            }
        }

        public int Insert(InvoiceInfo invoiceInfo)
        {
            int invoiceID = 0;

            invoiceInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(invoiceInfo.Attributes, this.ConnectionString, InvoiceInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        invoiceID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, InvoiceInfo.TableName);

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
            invoiceInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(invoiceInfo.Attributes, this.ConnectionString, InvoiceInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", InvoiceInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public ArrayList GetOrderIDArrayList(ArrayList idArrayList)
        {
            string sqlString = string.Format("SELECT OrderID FROM {0} WHERE ID IN ({1})", InvoiceInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));
            return BaiRongDataProvider.DatabaseDAO.GetIntArrayList(this.ConnectionString, sqlString);
        }

        public InvoiceInfo GetInvoiceInfo(NameValueCollection form)
        {
            InvoiceInfo invoiceInfo = new InvoiceInfo(0);

            foreach (string name in form.AllKeys)
            {
                if (InvoiceAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        invoiceInfo.SetExtendedAttribute(name, value.Trim());
                    }
                }
            }

            return invoiceInfo;
        }

        public InvoiceInfo GetInvoiceInfo(int invoiceID)
        {
            InvoiceInfo invoiceInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", invoiceID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, InvoiceInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    invoiceInfo = new InvoiceInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, invoiceInfo);
                }
                rdr.Close();
            }

            if (invoiceInfo != null) invoiceInfo.AfterExecuteReader();
            return invoiceInfo;
        }

        public InvoiceInfo GetInvoiceInfoByOrderID(int orderID)
        {
            InvoiceInfo invoiceInfo = null;
            string SQL_WHERE = string.Format("WHERE OrderID = {0}", orderID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, InvoiceInfo.TableName, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    invoiceInfo = new InvoiceInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, invoiceInfo);
                }
                rdr.Close();
            }

            if (invoiceInfo != null) invoiceInfo.AfterExecuteReader();
            return invoiceInfo;
        }

        public int GetCount()
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0}", InvoiceInfo.TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        public string GetSelectString()
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, InvoiceInfo.TableName, 0, SqlUtils.Asterisk, null, null);
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

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, InvoiceInfo.TableName, 0, SqlUtils.Asterisk, whereString, null);
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
