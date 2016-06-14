using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Model;
using BaiRong.Model;
using System.Data;
using BaiRong.Core;
using System.Collections;
using SiteServer.B2C.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class InvoiceDAO : DataProviderBase, IInvoiceDAO
    {
        private const string TABLE_NAME = "b2c_Invoice";

        public const string PARM_USER_NAME = "@UserName";
        public const string PARM_GROUP_SN = "@GroupSN";

        public int Insert(InvoiceInfo invoiceInfo)
        {
            if (invoiceInfo.IsDefault)
            {
                this.SetAllDefaultToFalse(invoiceInfo.GroupSN, invoiceInfo.UserName);
            }

            int invoiceID = 0;

            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(invoiceInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

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
            if (invoiceInfo.IsDefault)
            {
                this.SetAllDefaultToFalse(invoiceInfo.GroupSN, invoiceInfo.UserName);
            }

            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(invoiceInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int invoiceID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", TABLE_NAME, invoiceID);
            this.ExecuteNonQuery(sqlString);
        }

        public InvoiceInfo GetInvoiceInfo(int invoiceID)
        {
            InvoiceInfo invoiceInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", invoiceID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

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

        public List<InvoiceInfo> GetInvoiceInfoList(string groupSN, string userName)
        {
            // update by wujq
            List<InvoiceInfo> list = new List<InvoiceInfo>();

            string SQL_WHERE = string.Format("WHERE GroupSN = @GroupSN AND UserName = @UserName AND IsOrder = 'false'");
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY IsDefault DESC, ID DESC");

            IDbDataParameter[] selectParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN), 
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)	                    　
                };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, selectParms))
            {
                while (rdr.Read())
                {
                    InvoiceInfo invoiceInfo = new InvoiceInfo(rdr);

                    list.Add(invoiceInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public void SetDefault(int invoiceID)
        {
            InvoiceInfo invoiceInfo = this.GetInvoiceInfo(invoiceID);
            if (invoiceInfo != null)
            {
                this.SetAllDefaultToFalse(invoiceInfo.GroupSN, invoiceInfo.UserName);

                string sqlString = string.Format("UPDATE {0} SET IsDefault = '{1}' WHERE ID = {2}", TABLE_NAME, true.ToString(), invoiceID);

                this.ExecuteNonQuery(sqlString);
            }
        }

        private void SetAllDefaultToFalse(string groupSN, string userName)
        {
            // update by wujq

            //string sqlString = string.Format("UPDATE {0} SET IsDefault = '{1}' WHERE GroupSN = '{2}' AND UserName = '{3}' AND IsOrder = '{4}'", TABLE_NAME, false, groupSN, userName, false);

            string sqlString = string.Format("UPDATE {0} SET IsDefault = 'false' WHERE GroupSN = @GroupSN AND UserName = @UserName AND IsOrder = 'false'", TABLE_NAME);

            IDbDataParameter[] updateParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN), 
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)	                    　
                };

            this.ExecuteNonQuery(sqlString, updateParms);
        }
    }
}
