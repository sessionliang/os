using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.Project.Model;
using SiteServer.Project.Core;
using BaiRong.Model;
using System.Collections.Generic;

namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class PaymentDAO : DataProviderBase, SiteServer.Project.Core.IPaymentDAO
	{
        private const string SQL_UPDATE = "UPDATE pms_Payment SET IsCashBack = @IsCashBack, PaymentType = @PaymentType, PaymentOrder = @PaymentOrder, Premise = @Premise, ExpectDate = @ExpectDate, AmountExpect = @AmountExpect, IsInvoice = @IsInvoice, InvoiceNO = @InvoiceNO, InvoiceDate = @InvoiceDate, IsPayment = @IsPayment, AmountPaid = @AmountPaid, PaymentDate = @PaymentDate WHERE PaymentID = @PaymentID";

        private const string SQL_DELETE = "DELETE FROM pms_Payment WHERE PaymentID = @PaymentID";

        private const string SQL_SELECT = "SELECT PaymentID, ProjectID, IsCashBack, PaymentType, PaymentOrder, Premise, ExpectDate, AmountExpect, IsInvoice, InvoiceNO, InvoiceDate, IsPayment, AmountPaid, PaymentDate FROM pms_Payment WHERE PaymentID = @PaymentID";

        private const string SQL_SELECT_ALL_BY_PROJECTID = "SELECT PaymentID, ProjectID, IsCashBack, PaymentType, PaymentOrder, Premise, ExpectDate, AmountExpect, IsInvoice, InvoiceNO, InvoiceDate, IsPayment, AmountPaid, PaymentDate FROM pms_Payment WHERE ProjectID = @ProjectID AND IsCashBack = @IsCashBack ORDER BY PaymentOrder";

        private const string SQL_SELECT_ALL = "SELECT PaymentID, ProjectID, IsCashBack, PaymentType, PaymentOrder, Premise, ExpectDate, AmountExpect, IsInvoice, InvoiceNO, InvoiceDate, IsPayment, AmountPaid, PaymentDate FROM pms_Payment ORDER BY ProjectID, PaymentOrder";

        private const string PARM_PAYMENT_ID = "@PaymentID";
        private const string PARM_PROJECT_ID = "@ProjectID";
        private const string PARM_IS_CASH_BACK = "@IsCashBack";
        private const string PARM_PAYMENT_TYPE = "@PaymentType";
        private const string PARM_PAYMENT_ORDER = "@PaymentOrder";
        private const string PARM_PREMISE = "@Premise";
        private const string PARM_EXPECT_DATE = "@ExpectDate";
        private const string PARM_AMOUNT_EXPECT = "@AmountExpect";
        private const string PARM_IS_INVOICE = "@IsInvoice";
        private const string PARM_INVOICE_NO = "@InvoiceNO";
        private const string PARM_INVOICE_DATE = "@InvoiceDate";
        private const string PARM_IS_PAYMENT = "@IsPayment";
        private const string PARM_AMOUNT_PAID = "@AmountPaid";
        private const string PARM_PAYMENT_DATE = "@PaymentDate";

		public void Insert(PaymentInfo paymentInfo) 
		{
            string sqlString = "INSERT INTO pms_Payment (ProjectID, IsCashBack, PaymentType, PaymentOrder, Premise, ExpectDate, AmountExpect, IsInvoice, InvoiceNO, InvoiceDate, IsPayment, AmountPaid, PaymentDate) VALUES (@ProjectID, @IsCashBack, @PaymentType, @PaymentOrder, @Premise, @ExpectDate, @AmountExpect, @IsInvoice, @InvoiceNO, @InvoiceDate, @IsPayment, @AmountPaid, @PaymentDate)";

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PROJECT_ID, EDataType.Integer, paymentInfo.ProjectID),
                this.GetParameter(PARM_IS_CASH_BACK, EDataType.VarChar, 18, paymentInfo.IsCashBack.ToString()),
                this.GetParameter(PARM_PAYMENT_TYPE, EDataType.NVarChar, 50, paymentInfo.PaymentType),
                this.GetParameter(PARM_PAYMENT_ORDER, EDataType.Integer, paymentInfo.PaymentOrder),
                this.GetParameter(PARM_PREMISE, EDataType.NVarChar, 255, paymentInfo.Premise),
                this.GetParameter(PARM_EXPECT_DATE, EDataType.DateTime, paymentInfo.ExpectDate),
                this.GetParameter(PARM_AMOUNT_EXPECT, EDataType.Integer, paymentInfo.AmountExpect),
                this.GetParameter(PARM_IS_INVOICE, EDataType.VarChar, 18, paymentInfo.IsInvoice.ToString()),
                this.GetParameter(PARM_INVOICE_NO, EDataType.NVarChar, 50, paymentInfo.InvoiceNO),
                this.GetParameter(PARM_INVOICE_DATE, EDataType.DateTime, paymentInfo.InvoiceDate),
                this.GetParameter(PARM_IS_PAYMENT, EDataType.VarChar, 18, paymentInfo.IsPayment.ToString()),
                this.GetParameter(PARM_AMOUNT_PAID, EDataType.Integer, paymentInfo.AmountPaid),
                this.GetParameter(PARM_PAYMENT_DATE, EDataType.DateTime, paymentInfo.PaymentDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
		}

        public void Update(PaymentInfo paymentInfo) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_IS_CASH_BACK, EDataType.VarChar, 18, paymentInfo.IsCashBack.ToString()),
                this.GetParameter(PARM_PAYMENT_TYPE, EDataType.NVarChar, 50, paymentInfo.PaymentType),
                this.GetParameter(PARM_PAYMENT_ORDER, EDataType.Integer, paymentInfo.PaymentOrder),
                this.GetParameter(PARM_PREMISE, EDataType.NVarChar, 255, paymentInfo.Premise),
                this.GetParameter(PARM_EXPECT_DATE, EDataType.DateTime, paymentInfo.ExpectDate),
                this.GetParameter(PARM_AMOUNT_EXPECT, EDataType.Integer, paymentInfo.AmountExpect),
                this.GetParameter(PARM_IS_INVOICE, EDataType.VarChar, 18, paymentInfo.IsInvoice.ToString()),
                this.GetParameter(PARM_INVOICE_NO, EDataType.NVarChar, 50, paymentInfo.InvoiceNO),
                this.GetParameter(PARM_INVOICE_DATE, EDataType.DateTime, paymentInfo.InvoiceDate),
                this.GetParameter(PARM_IS_PAYMENT, EDataType.VarChar, 18, paymentInfo.IsPayment.ToString()),
                this.GetParameter(PARM_AMOUNT_PAID, EDataType.Integer, paymentInfo.AmountPaid),
                this.GetParameter(PARM_PAYMENT_DATE, EDataType.DateTime, paymentInfo.PaymentDate),
                this.GetParameter(PARM_PAYMENT_ID, EDataType.Integer, paymentInfo.PaymentID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
		}

		public void Delete(int paymentID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PAYMENT_ID, EDataType.Integer, paymentID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
		}

        public int GetAmountPaid(int projectID, bool isCashBack)
        {
            string sqlString = string.Format("SELECT SUM(AmountPaid) FROM pms_Payment WHERE ProjectID = {0} AND IsCashBack = '{1}' AND IsPayment = 'True'", projectID, isCashBack);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetAmountPaidPure(int projectID)
        {
            string sqlString = string.Format("SELECT SUM(AmountPaid) FROM pms_Payment WHERE ProjectID = {0} AND IsCashBack = '{1}' AND IsPayment = 'True'", projectID, false);
            int amount1 = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            sqlString = string.Format("SELECT SUM(AmountPaid) FROM pms_Payment WHERE ProjectID = {0} AND IsCashBack = '{1}' AND IsPayment = 'True'", projectID, true);
            int amount2 = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            return amount1 - amount2;
        }

        public int GetAmountExpect(int projectID, bool isCashBack)
        {
            string sqlString = string.Format("SELECT SUM(AmountExpect) FROM pms_Payment WHERE ProjectID = {0} AND IsCashBack = '{1}'", projectID, isCashBack);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCount(int projectID, ETriState isPayment)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM pms_Payment WHERE ProjectID = {0}", projectID);
            if (isPayment != ETriState.All)
            {
                sqlString = string.Format("SELECT COUNT(*) FROM pms_Payment WHERE ProjectID = {0} AND IsPayment = '{1}'", projectID, ETriStateUtils.GetValue(isPayment));
            }
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public PaymentInfo GetPaymentInfo(int paymentID)
		{
            PaymentInfo paymentInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PAYMENT_ID, EDataType.Integer, paymentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    paymentInfo = new PaymentInfo(rdr.GetInt32(0), rdr.GetInt32(1), TranslateUtils.ToBool(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetDateTime(6), rdr.GetInt32(7), TranslateUtils.ToBool(rdr.GetValue(8).ToString()), rdr.GetValue(9).ToString(), rdr.GetDateTime(10), TranslateUtils.ToBool(rdr.GetValue(11).ToString()), rdr.GetInt32(12), rdr.GetDateTime(13));
                }
                rdr.Close();
            }

            return paymentInfo;
		}

        public IEnumerable GetDataSource(int projectID, bool isCashBack)
		{
            if (projectID > 0)
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_PROJECT_ID, EDataType.Integer, projectID),
                    this.GetParameter(PARM_IS_CASH_BACK, EDataType.VarChar, 18, isCashBack.ToString())
			    };
                IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_BY_PROJECTID, parms);
                return enumerable;
            }
            else
            {
                IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL);
                return enumerable;
            }
		}

        public IEnumerable GetDataSourceCashBack(bool isPayment)
        {
            string sqlString = string.Format("SELECT PaymentID, ProjectID, IsCashBack, PaymentType, PaymentOrder, Premise, ExpectDate, AmountExpect, IsInvoice, InvoiceNO, InvoiceDate, IsPayment, AmountPaid, PaymentDate FROM pms_Payment WHERE IsCashBack = 'True' AND IsPayment = '{0}' ORDER BY ExpectDate, PaymentOrder", isPayment.ToString());

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public IEnumerable GetDataSourceNotPay(string type)
        {
            string sqlString = "SELECT PaymentID, ProjectID, IsCashBack, PaymentType, PaymentOrder, Premise, ExpectDate, AmountExpect, IsInvoice, InvoiceNO, InvoiceDate, IsPayment, AmountPaid, PaymentDate FROM pms_Payment WHERE IsCashBack = 'False' AND IsPayment = 'False' ORDER BY ExpectDate, PaymentOrder";

            if (!string.IsNullOrEmpty(type) && type == "Month")
            {
                sqlString = string.Format("SELECT PaymentID, ProjectID, IsCashBack, PaymentType, PaymentOrder, Premise, ExpectDate, AmountExpect, IsInvoice, InvoiceNO, InvoiceDate, IsPayment, AmountPaid, PaymentDate FROM pms_Payment WHERE IsCashBack = 'False' AND IsPayment = 'False' AND ExpectDate <> '1754-01-01' AND (ExpectDate <= getdate() OR (year(ExpectDate) = {0} AND month(ExpectDate) = {1})) ORDER BY ExpectDate", DateTime.Now.Year, DateTime.Now.Month);
            }
            else if (!string.IsNullOrEmpty(type) && type == "NextMonth")
            {
                sqlString = string.Format("SELECT PaymentID, ProjectID, IsCashBack, PaymentType, PaymentOrder, Premise, ExpectDate, AmountExpect, IsInvoice, InvoiceNO, InvoiceDate, IsPayment, AmountPaid, PaymentDate FROM pms_Payment WHERE IsCashBack = 'False' AND IsPayment = 'False' AND ExpectDate <> '1754-01-01' AND ( (year(ExpectDate) = {0} AND month(ExpectDate) = {1})) ORDER BY ExpectDate", DateTime.Now.Year, DateTime.Now.Month + 1);
            }

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public IEnumerable GetDataSourcePaied(string type)
        {
            string sqlString = "SELECT PaymentID, ProjectID, IsCashBack, PaymentType, PaymentOrder, Premise, ExpectDate, AmountExpect, IsInvoice, InvoiceNO, InvoiceDate, IsPayment, AmountPaid, PaymentDate FROM pms_Payment WHERE IsCashBack = 'False' AND IsPayment = 'True' ORDER BY ExpectDate, PaymentOrder";

            if (!string.IsNullOrEmpty(type))
            {
                if (type == "Year")
                {
                    sqlString = string.Format("SELECT PaymentID, ProjectID, IsCashBack, PaymentType, PaymentOrder, Premise, ExpectDate, AmountExpect, IsInvoice, InvoiceNO, InvoiceDate, IsPayment, AmountPaid, PaymentDate FROM pms_Payment WHERE IsCashBack = 'False' AND IsPayment = 'True' AND year(PaymentDate) = {0} ORDER BY PaymentDate", DateTime.Now.Year);
                }
                else if (type == "Month")
                {
                    sqlString = string.Format("SELECT PaymentID, ProjectID, IsCashBack, PaymentType, PaymentOrder, Premise, ExpectDate, AmountExpect, IsInvoice, InvoiceNO, InvoiceDate, IsPayment, AmountPaid, PaymentDate FROM pms_Payment WHERE IsCashBack = 'False' AND IsPayment = 'True' AND year(PaymentDate) = {0} AND month(PaymentDate) = {1} ORDER BY PaymentDate", DateTime.Now.Year, DateTime.Now.Month);
                }
            }

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public ArrayList GetPaymentInfoArrayList(int projectID, bool isCashBack)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PROJECT_ID, EDataType.Integer, projectID),
                this.GetParameter(PARM_IS_CASH_BACK, EDataType.VarChar, 18, isCashBack.ToString())
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_BY_PROJECTID, parms))
            {
                while (rdr.Read())
                {
                    PaymentInfo paymentInfo = new PaymentInfo(rdr.GetInt32(0), rdr.GetInt32(1), TranslateUtils.ToBool(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetDateTime(6), rdr.GetInt32(7), TranslateUtils.ToBool(rdr.GetValue(8).ToString()), rdr.GetValue(9).ToString(), rdr.GetDateTime(10), TranslateUtils.ToBool(rdr.GetValue(11).ToString()), rdr.GetInt32(12), rdr.GetDateTime(13));
                    arraylist.Add(paymentInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public int GetMaxPaymentOrder(int projectID)
        {
            string sqlString = string.Format("SELECT MAX(PaymentOrder) FROM pms_Payment WHERE ProjectID = {0}", projectID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetAmountPaid(int year, int month)
        {
            string sqlString = string.Format("SELECT SUM(AmountPaid) FROM pms_Payment WHERE IsCashBack = 'False' AND IsPayment = 'True' AND year(PaymentDate) = {0} AND month(PaymentDate) = {1}", year, month);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetAmountNet(int year, int month)
        {
            string sqlString = string.Format("SELECT SUM(AmountPaid) FROM pms_Payment WHERE IsCashBack = 'False' AND IsPayment = 'True' AND year(PaymentDate) = {0} AND month(PaymentDate) = {1}", year, month);
            int payment = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            sqlString = string.Format("SELECT SUM(AmountPaid) FROM pms_Payment WHERE IsCashBack = 'True' AND IsPayment = 'True' AND year(PaymentDate) = {0} AND month(PaymentDate) = {1}", year, month);
            int cashBack = BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
            return payment - cashBack;
        }

        public Dictionary<int, int> GetProjectIDAmountPaidDictionary(int year, int month)
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            string sqlString = string.Format("SELECT ProjectID, SUM(AmountPaid) FROM pms_Payment WHERE IsCashBack = 'False' AND IsPayment = 'True' AND year(PaymentDate) = {0} AND month(PaymentDate) = {1} GROUP BY ProjectID", year, month);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int projectID = rdr.GetInt32(0);
                    int amountPaid = rdr.GetInt32(1);
                    dictionary.Add(projectID, amountPaid);
                }
                rdr.Close();
            }

            return dictionary;
        }
	}
}