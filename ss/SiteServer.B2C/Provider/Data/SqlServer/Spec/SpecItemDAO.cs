using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;
using System.Text;
using System.Collections.Generic;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class SpecItemDAO : DataProviderBase, ISpecItemDAO
	{
        private const string SQL_UPDATE = "UPDATE b2c_SpecItem SET Title = @Title, IconUrl = @IconUrl, IsDefault = @IsDefault WHERE ItemID = @ItemID";

        private const string SQL_DELETE = "DELETE FROM b2c_SpecItem WHERE ItemID = @ItemID";

        private const string SQL_SELECT = "SELECT ItemID, PublishmentSystemID, SpecID, Title, IconUrl, IsDefault, Taxis FROM b2c_SpecItem WHERE ItemID = @ItemID";

        private const string SQL_SELECT_ALL = "SELECT ItemID, PublishmentSystemID, SpecID, Title, IconUrl, IsDefault, Taxis FROM b2c_SpecItem WHERE SpecID = @SpecID ORDER BY Taxis";

        private const string SQL_SELECT_ALL_PUBLISHMENT_SYSTEM_ID = "SELECT ItemID, PublishmentSystemID, SpecID, Title, IconUrl, IsDefault, Taxis FROM b2c_SpecItem WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string PARM_ITEM_ID = "@ItemID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_SPEC_ID = "@SpecID";
        private const string PARM_TITLE = "@Title";
        private const string PARM_ICON_URL = "@IconUrl";
        private const string PARM_IS_DEFAULT = "@IsDefault";
        private const string PARM_TAXIS = "@Taxis";

		public int Insert(int publishmentSystemID, SpecItemInfo itemInfo) 
		{
            int itemID = 0;

            string sqlString = "INSERT INTO b2c_SpecItem (PublishmentSystemID, SpecID, Title, IconUrl, IsDefault, Taxis) VALUES (@PublishmentSystemID, @SpecID, @Title, @IconUrl, @IsDefault, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO b2c_SpecItem (ItemID, PublishmentSystemID, SpecID, Title, IconUrl, IsDefault, Taxis) VALUES (b2c_SpecItem_SEQ.NEXTVAL, @PublishmentSystemID, @SpecID, @Title, @IconUrl, @IsDefault, @Taxis)";
            }

            int taxis = this.GetMaxTaxis(itemInfo.SpecID) + 1;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SPEC_ID, EDataType.Integer, itemInfo.SpecID),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, itemInfo.PublishmentSystemID),
                this.GetParameter(PARM_TITLE, EDataType.NVarChar, 50, itemInfo.Title),
                this.GetParameter(PARM_ICON_URL, EDataType.VarChar, 200, itemInfo.IconUrl),
                this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, itemInfo.IsDefault.ToString()),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        itemID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "b2c_Item");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            SpecItemManager.ClearCache(publishmentSystemID);

            return itemID;
		}

        public void Update(int publishmentSystemID, SpecItemInfo itemInfo)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_TITLE, EDataType.NVarChar, 50, itemInfo.Title),
                this.GetParameter(PARM_ICON_URL, EDataType.VarChar, 200, itemInfo.IconUrl),
                this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, itemInfo.IsDefault.ToString()),
				this.GetParameter(PARM_ITEM_ID, EDataType.Integer, itemInfo.ItemID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);

            SpecItemManager.ClearCache(publishmentSystemID);
		}

		public void Delete(int publishmentSystemID, int itemID)
		{
			IDbDataParameter[] deleteParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ITEM_ID, EDataType.Integer, itemID)
			};

            this.ExecuteNonQuery(SQL_DELETE, deleteParms);

            SpecItemManager.ClearCache(publishmentSystemID);
		}

        //public SpecItemInfo GetSpecItemInfo(int itemID)
        //{
        //    SpecItemInfo itemInfo = null;

        //    IDbDataParameter[] parms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_ITEM_ID, EDataType.Integer, itemID)
        //    };

        //    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
        //    {
        //        if (rdr.Read())
        //        {
        //            itemInfo = new SpecItemInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6));
        //        }
        //        rdr.Close();
        //    }

        //    return itemInfo;
        //}

		public IEnumerable GetDataSource(int specID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SPEC_ID, EDataType.Integer, specID)
			};
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL, parms);
			return enumerable;
		}

        public List<SpecItemInfo> GetSpecItemInfoList(int specID)
        {
            List<SpecItemInfo> list = new List<SpecItemInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_SPEC_ID, EDataType.Integer, specID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    SpecItemInfo itemInfo = new SpecItemInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6));
                    list.Add(itemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        //public List<int> GetItemIDList(int specID, ETriState isDefaultState)
        //{
        //    List<int> list = new List<int>();

        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendFormat("SELECT ItemID FROM b2c_SpecItem WHERE SpecID = {0}", specID);
        //    if (isDefaultState != ETriState.All)
        //    {
        //        builder.AppendFormat(" AND IsDefault = '{0}'", ETriStateUtils.GetValue(isDefaultState));
        //    }
        //    builder.Append(" ORDER BY Taxis");

        //    using (IDataReader rdr = this.ExecuteReader(builder.ToString()))
        //    {
        //        while (rdr.Read())
        //        {
        //            list.Add(rdr.GetInt32(0));
        //        }
        //        rdr.Close();
        //    }

        //    return list;
        //}

        public bool UpdateTaxisToDown(int specID, int itemID)
        {
            string sqlString = string.Format("SELECT TOP 1 ItemID, Taxis FROM b2c_SpecItem WHERE ((Taxis > (SELECT Taxis FROM b2c_SpecItem WHERE ItemID = {0})) AND SpecID ={1}) ORDER BY Taxis", itemID, specID);
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

            int selectedTaxis = GetTaxis(itemID);

            if (higherID != 0)
            {
                SetTaxis(itemID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToUp(int specID, int itemID)
        {
            string sqlString = string.Format("SELECT TOP 1 ItemID, Taxis FROM b2c_SpecItem WHERE ((Taxis < (SELECT Taxis FROM b2c_SpecItem WHERE (ItemID = {0}))) AND SpecID = {1}) ORDER BY Taxis DESC", itemID, specID);
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

            int selectedTaxis = GetTaxis(itemID);

            if (lowerID != 0)
            {
                SetTaxis(itemID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int specID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM b2c_SpecItem WHERE SpecID = {0}", specID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int itemID)
        {
            string sqlString = string.Format("SELECT Taxis FROM b2c_SpecItem WHERE (ItemID = {0})", itemID);
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

        private void SetTaxis(int itemID, int taxis)
        {
            string sqlString = string.Format("UPDATE b2c_SpecItem SET Taxis = {0} WHERE ItemID = {1}", taxis, itemID);
            this.ExecuteNonQuery(sqlString);
        }

        public Dictionary<int, SpecItemInfo> GetSpecItemInfoDictionary(int publishmentSystemID)
        {
            Dictionary<int, SpecItemInfo> dictionary = new Dictionary<int, SpecItemInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_PUBLISHMENT_SYSTEM_ID, parms))
            {
                while (rdr.Read())
                {
                    SpecItemInfo itemInfo = new SpecItemInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6));
                    dictionary.Add(itemInfo.ItemID, itemInfo);
                }
                rdr.Close();
            }

            return dictionary;
        }
	}
}