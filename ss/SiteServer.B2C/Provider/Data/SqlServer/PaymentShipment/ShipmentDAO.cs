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
    public class ShipmentDAO : DataProviderBase, IShipmentDAO
	{
        private const string SQL_UPDATE_SHIPMENT = "UPDATE b2c_Shipment SET ShipmentName = @ShipmentName, ShipmentPeriod = @ShipmentPeriod, IsEnabled = @IsEnabled, Description = @Description WHERE ID = @ID";

        private const string SQL_DELETE_SHIPMENT = "DELETE FROM b2c_Shipment WHERE ID = @ID";

        private const string SQL_SELECT_SHIPMENT = "SELECT ID, PublishmentSystemID, ShipmentName, ShipmentPeriod, IsEnabled, Taxis, Description FROM b2c_Shipment WHERE ID = @ID";

        private const string SQL_SELECT_SHIPMENT_BY_NAME = "SELECT ID, PublishmentSystemID, ShipmentName, ShipmentPeriod, IsEnabled, Taxis, Description FROM b2c_Shipment WHERE PublishmentSystemID = @PublishmentSystemID AND ShipmentName = @ShipmentName";

        private const string SQL_SELECT_ALL_SHIPMENT = "SELECT ID, PublishmentSystemID, ShipmentName, ShipmentPeriod, IsEnabled, Taxis, Description FROM b2c_Shipment WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC";

        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_SHIPMENT_NAME = "@ShipmentName";
        private const string PARM_SHIPMENT_PERIOD = "@ShipmentPeriod";
        private const string PARM_IS_ENABLED = "@IsEnabled";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_DESCRIPTION = "@Description";

		public int Insert(ShipmentInfo shipmentInfo) 
		{
            int shipmentID = 0;

            string sqlString = "INSERT INTO b2c_Shipment (PublishmentSystemID, ShipmentName, ShipmentPeriod, IsEnabled, Taxis, Description) VALUES (@PublishmentSystemID, @ShipmentName, @ShipmentPeriod, @IsEnabled, @Taxis, @Description)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO b2c_Shipment (ID, PublishmentSystemID, ShipmentName, ShipmentPeriod, IsEnabled, Taxis, Description) VALUES (b2c_Shipment_SEQ.NEXTVAL, @PublishmentSystemID, @ShipmentName, @ShipmentPeriod, @IsEnabled, @Taxis, @Description)";
            }

            int taxis = this.GetMaxTaxis(shipmentInfo.PublishmentSystemID) + 1;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, shipmentInfo.PublishmentSystemID),
                this.GetParameter(PARM_SHIPMENT_NAME, EDataType.NVarChar, 50, shipmentInfo.ShipmentName),
                this.GetParameter(PARM_SHIPMENT_PERIOD, EDataType.VarChar, 50, EShipmentPeriodUtils.GetValue(shipmentInfo.ShipmentPeriod)),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, shipmentInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NText, shipmentInfo.Description)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        shipmentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "b2c_Shipment");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return shipmentID;
		}

        public void Update(ShipmentInfo shipmentInfo) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_SHIPMENT_NAME, EDataType.NVarChar, 50, shipmentInfo.ShipmentName),
                this.GetParameter(PARM_SHIPMENT_PERIOD, EDataType.VarChar, 50, EShipmentPeriodUtils.GetValue(shipmentInfo.ShipmentPeriod)),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, shipmentInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NText, shipmentInfo.Description),
				this.GetParameter(PARM_ID, EDataType.Integer, shipmentInfo.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_SHIPMENT, parms);
		}

		public void Delete(int shipmentID)
		{
			IDbDataParameter[] deleteParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, shipmentID)
			};

            this.ExecuteNonQuery(SQL_DELETE_SHIPMENT, deleteParms);
		}

        public ShipmentInfo GetShipmentInfo(int shipmentID)
		{
            ShipmentInfo shipmentInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, shipmentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SHIPMENT, parms))
            {
                if (rdr.Read())
                {
                    shipmentInfo = new ShipmentInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), EShipmentPeriodUtils.GetEnumType(rdr.GetValue(3).ToString()), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetInt32(5), rdr.GetValue(6).ToString());
                }
                rdr.Close();
            }

            return shipmentInfo;
		}

        public bool IsExists(int publishmentSystemID, string shipmentName)
		{
			bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_SHIPMENT_NAME, EDataType.NVarChar, 50, shipmentName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SHIPMENT_BY_NAME, parms))
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

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_SHIPMENT, parms);
			return enumerable;
		}

        public List<ShipmentInfo> GetShipmentInfoList(int publishmentSystemID)
        {
            List<ShipmentInfo> list = new List<ShipmentInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_SHIPMENT, parms))
            {
                while (rdr.Read())
                {
                    ShipmentInfo shipmentInfo = new ShipmentInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), EShipmentPeriodUtils.GetEnumType(rdr.GetValue(3).ToString()), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetInt32(5), rdr.GetValue(6).ToString());
                    list.Add(shipmentInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public bool UpdateTaxisToUp(int publishmentSystemID, int shipmentID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM b2c_Shipment WHERE Taxis > (SELECT Taxis FROM b2c_Shipment WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis", shipmentID, publishmentSystemID);
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

            int selectedTaxis = GetTaxis(shipmentID);

            if (higherID != 0)
            {
                SetTaxis(shipmentID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, int shipmentID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM b2c_Shipment WHERE Taxis < (SELECT Taxis FROM b2c_Shipment WHERE ID = {0}) AND PublishmentSystemID = {1} ORDER BY Taxis DESC", shipmentID, publishmentSystemID);
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

            int selectedTaxis = GetTaxis(shipmentID);

            if (lowerID != 0)
            {
                SetTaxis(shipmentID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM b2c_Shipment WHERE PublishmentSystemID = {0}", publishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int shipmentID)
        {
            string sqlString = string.Format("SELECT Taxis FROM b2c_Shipment WHERE (ID = {0})", shipmentID);
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

        private void SetTaxis(int shipmentID, int taxis)
        {
            string sqlString = string.Format("UPDATE b2c_Shipment SET Taxis = {0} WHERE ID = {1}", taxis, shipmentID);
            this.ExecuteNonQuery(sqlString);
        }
	}
}