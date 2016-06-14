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
    public class FilterItemDAO : DataProviderBase, IFilterItemDAO
	{
        private const string SQL_DELETE = "DELETE FROM b2c_FilterItem WHERE FilterID = @FilterID";

        private const string SQL_SELECT_ALL = "SELECT ItemID, FilterID, Title, Value, Taxis FROM b2c_FilterItem WHERE FilterID = @FilterID ORDER BY Taxis";

        private const string PARM_ITEM_ID = "@ItemID";
        private const string PARM_FILTER_ID = "@FilterID";
        private const string PARM_TITLE = "@Title";
        private const string PARM_VALUE = "@Value";
        private const string PARM_TAXIS = "@Taxis";

		public void Insert(FilterItemInfo itemInfo) 
		{
            string sqlString = "INSERT INTO b2c_FilterItem (FilterID, Title, Value, Taxis) VALUES (@FilterID, @Title, @Value, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO b2c_FilterItem (ItemID, FilterID, Title, Value, Taxis) VALUES (b2c_FilterItem_SEQ.NEXTVAL, @FilterID, @Title, @Value, @Taxis)";
            }

            int taxis = this.GetMaxTaxis(itemInfo.FilterID) + 1;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_FILTER_ID, EDataType.Integer, itemInfo.FilterID),
                this.GetParameter(PARM_TITLE, EDataType.NVarChar, 255, itemInfo.Title),
                this.GetParameter(PARM_VALUE, EDataType.NVarChar, 255, itemInfo.Value),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis)
			};

            this.ExecuteNonQuery(sqlString, parms);
		}

		public void Delete(int filterID)
		{
			IDbDataParameter[] deleteParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_FILTER_ID, EDataType.Integer, filterID)
			};

            this.ExecuteNonQuery(SQL_DELETE, deleteParms);
		}

        public IEnumerable GetDataSource(int filterID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_FILTER_ID, EDataType.Integer, filterID)
			};
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL, parms);
            return enumerable;
        }

        public List<FilterItemInfo> GetFilterItemInfoList(int filterID)
        {
            List<FilterItemInfo> arraylist = new List<FilterItemInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_FILTER_ID, EDataType.Integer, filterID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    FilterItemInfo itemInfo = new FilterItemInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetInt32(4));
                    arraylist.Add(itemInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }      

        private int GetMaxTaxis(int filterID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM b2c_FilterItem WHERE FilterID = {0}", filterID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public IEnumerable GetStlDataSource(int filterID, int startNum, int totalNum)
        {
            string orderByString = "ORDER BY Taxis";

            string whereString = string.Format("WHERE FilterID = {0}", filterID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString("b2c_FilterItem", startNum, totalNum, SqlUtils.Asterisk, whereString, orderByString);

            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }
	}
}