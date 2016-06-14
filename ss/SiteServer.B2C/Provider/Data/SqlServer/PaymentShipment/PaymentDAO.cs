using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;
using System.Collections.Generic;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class PaymentDAO : DataProviderBase, IPaymentDAO
	{
        private const string SQL_UPDATE_PAYMENT = "UPDATE b2c_Payment SET PaymentName = @PaymentName, IsEnabled = @IsEnabled, IsOnline = @IsOnline, Description = @Description, SettingsXML = @SettingsXML WHERE ID = @ID";

        private const string SQL_DELETE_PAYMENT = "DELETE FROM b2c_Payment WHERE ID = @ID";

        private const string SQL_SELECT_PAYMENT = "SELECT ID, PublishmentSystemID, PaymentType, PaymentName, IsEnabled, IsOnline, Taxis, Description, SettingsXML FROM b2c_Payment WHERE ID = @ID";

        private const string SQL_SELECT_BY_NAME = "SELECT ID, PublishmentSystemID, PaymentType, PaymentName, IsEnabled, IsOnline, Taxis, Description, SettingsXML FROM b2c_Payment WHERE PublishmentSystemID = @PublishmentSystemID AND PaymentName = @PaymentName";

        private const string SQL_SELECT_BY_TYPE = "SELECT ID, PublishmentSystemID, PaymentType, PaymentName, IsEnabled, IsOnline, Taxis, Description, SettingsXML FROM b2c_Payment WHERE PublishmentSystemID = @PublishmentSystemID AND PaymentType = @PaymentType";

        private const string SQL_SELECT_ALL_PAYMENT = "SELECT ID, PublishmentSystemID, PaymentType, PaymentName, IsEnabled, IsOnline, Taxis, Description, SettingsXML FROM b2c_Payment WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC";

        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_TYPE = "@PaymentType";
        private const string PARM_NAME = "@PaymentName";
        private const string PARM_IS_ENABLED = "@IsEnabled";
        private const string PARM_IS_ONLINE = "@IsOnline";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_SETTINGS_XML = "@SettingsXML";

		public int Insert(PaymentInfo paymentInfo) 
		{
            int paymentID = 0;

            string sqlString = "INSERT INTO b2c_Payment (PublishmentSystemID, PaymentType, PaymentName, IsEnabled, IsOnline, Taxis, Description, SettingsXML) VALUES (@PublishmentSystemID, @PaymentType, @PaymentName, @IsEnabled, @IsOnline, @Taxis, @Description, @SettingsXML)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO b2c_Payment (ID, PublishmentSystemID, PaymentType, PaymentName, IsEnabled, IsOnline, Taxis, Description, SettingsXML) VALUES (b2c_Payment_SEQ.NEXTVAL, @PublishmentSystemID, @PaymentType, @PaymentName, @IsEnabled, @IsOnline, @Taxis, @Description, @SettingsXML)";
            }

            int taxis = this.GetMaxTaxis(paymentInfo.PublishmentSystemID) + 1;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, paymentInfo.PublishmentSystemID),
				this.GetParameter(PARM_TYPE, EDataType.VarChar, 50, EPaymentTypeUtils.GetValue(paymentInfo.PaymentType)),
                this.GetParameter(PARM_NAME, EDataType.NVarChar, 50, paymentInfo.PaymentName),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, paymentInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_IS_ONLINE, EDataType.VarChar, 18, paymentInfo.IsOnline.ToString()),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NText, paymentInfo.Description),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, paymentInfo.SettingsXML)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        paymentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "b2c_Payment");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return paymentID;
		}

        public void Update(PaymentInfo paymentInfo) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_NAME, EDataType.NVarChar, 50, paymentInfo.PaymentName),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, paymentInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_IS_ONLINE, EDataType.VarChar, 18, paymentInfo.IsOnline.ToString()),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NText, paymentInfo.Description),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, paymentInfo.SettingsXML),
				this.GetParameter(PARM_ID, EDataType.Integer, paymentInfo.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_PAYMENT, parms);
		}

		public void Delete(int paymentID)
		{
			IDbDataParameter[] deleteParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, paymentID)
			};

            this.ExecuteNonQuery(SQL_DELETE_PAYMENT, deleteParms);
		}

        public PaymentInfo GetPaymentInfo(int paymentID)
		{
            PaymentInfo paymentInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, paymentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PAYMENT, parms))
            {
                if (rdr.Read())
                {
                    paymentInfo = new PaymentInfo(rdr.GetInt32(0), rdr.GetInt32(1), EPaymentTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString());
                }
                rdr.Close();
            }

            return paymentInfo;
		}

        public bool IsExists(int publishmentSystemID, string paymentName)
		{
			bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 50, paymentName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_NAME, parms))
			{
				if (rdr.Read()) 
				{					
					exists = true;
				}
				rdr.Close();
			}

			return exists;
		}

        public bool IsExists(int publishmentSystemID, EPaymentType paymentType)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_TYPE, EDataType.VarChar, 50, EPaymentTypeUtils.GetValue(paymentType))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_TYPE, parms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

		public IEnumerable GetDataSource(int publishmentSystemID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_PAYMENT, parms);
			return enumerable;
		}

        public List<PaymentInfo> GetPaymentInfoList(int publishmentSystemID)
        {
            List<PaymentInfo> list = new List<PaymentInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_PAYMENT, parms))
            {
                while (rdr.Read())
                {
                    PaymentInfo paymentInfo = new PaymentInfo(rdr.GetInt32(0), rdr.GetInt32(1), EPaymentTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString());
                    list.Add(paymentInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public bool UpdateTaxisToUp(int publishmentSystemID, int paymentID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM b2c_Payment WHERE Taxis > (SELECT Taxis FROM b2c_Payment WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis", paymentID, publishmentSystemID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(paymentID);

            if (higherID != 0)
            {
                SetTaxis(paymentID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, int paymentID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM b2c_Payment WHERE Taxis < (SELECT Taxis FROM b2c_Payment WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis DESC", paymentID, publishmentSystemID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(paymentID);

            if (lowerID != 0)
            {
                SetTaxis(paymentID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM b2c_Payment WHERE PublishmentSystemID = {0}", publishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int paymentID)
        {
            string sqlString = string.Format("SELECT Taxis FROM b2c_Payment WHERE (ID = {0})", paymentID);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int paymentID, int taxis)
        {
            string sqlString = string.Format("UPDATE b2c_Payment SET Taxis = {0} WHERE ID = {1}", taxis, paymentID);
            this.ExecuteNonQuery(sqlString);
        }
	}
}