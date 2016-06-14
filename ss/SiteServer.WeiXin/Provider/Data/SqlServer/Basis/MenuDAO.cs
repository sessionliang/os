using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class MenuDAO : DataProviderBase, IMenuDAO
    {
        private const string SQL_UPDATE = "UPDATE wx_Menu SET PublishmentSystemID = @PublishmentSystemID, MenuName = @MenuName, MenuType = @MenuType, Keyword = @Keyword, Url = @Url, ChannelID = @ChannelID, ContentID = @ContentID, ParentID = @ParentID, Taxis = @Taxis WHERE MenuID = @MenuID";

        private const string SQL_DELETE = "DELETE FROM wx_Menu WHERE MenuID = @MenuID OR ParentID = @MenuID";

        private const string SQL_SELECT = "SELECT MenuID, PublishmentSystemID, MenuName, MenuType, Keyword, Url, ChannelID, ContentID, ParentID, Taxis FROM wx_Menu WHERE MenuID = @MenuID";

        private const string SQL_SELECT_ALL = "SELECT MenuID, PublishmentSystemID, MenuName, MenuType, Keyword, Url, ChannelID, ContentID, ParentID, Taxis FROM wx_Menu WHERE PublishmentSystemID = @PublishmentSystemID AND ParentID = @ParentID ORDER BY Taxis";

        private const string SQL_SELECT_ALLBY = "SELECT MenuID, PublishmentSystemID, MenuName, MenuType, Keyword, Url, ChannelID, ContentID, ParentID, Taxis FROM wx_Menu WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis";


        private const string PARM_MENU_ID = "@MenuID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_MENU_NAME = "@MenuName";
        private const string PARM_MENU_TYPE = "@MenuType";
        private const string PARM_KEYWORD = "@Keyword";
        private const string PARM_URL = "@Url";
        private const string PARM_CHANNEL_ID = "@ChannelID";
        private const string PARM_CONTENT_ID = "@ContentID";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_TAXIS = "@Taxis";

        public int Insert(MenuInfo menuInfo)
        {
            int menuID = 0;

            string sqlString = "INSERT INTO wx_Menu (PublishmentSystemID, MenuName, MenuType, Keyword, Url, ChannelID, ContentID, ParentID, Taxis) VALUES (@PublishmentSystemID, @MenuName, @MenuType, @Keyword, @Url, @ChannelID, @ContentID, @ParentID, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO wx_Menu(MenuID, PublishmentSystemID, MenuName, MenuType, Keyword, Url, ChannelID, ContentID, ParentID, Taxis) VALUES (wx_Menu_SEQ.NEXTVAL, @PublishmentSystemID, @MenuName, @MenuType, @Keyword, @Url, @ChannelID, @ContentID, @ParentID, @Taxis)";
            }

            int taxis = this.GetMaxTaxis(menuInfo.ParentID) + 1;
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, menuInfo.PublishmentSystemID),
                this.GetParameter(PARM_MENU_NAME, EDataType.NVarChar, 50, menuInfo.MenuName),
                this.GetParameter(PARM_MENU_TYPE, EDataType.VarChar, 50, EMenuTypeUtils.GetValue(menuInfo.MenuType)),
                this.GetParameter(PARM_KEYWORD, EDataType.NVarChar, 50, menuInfo.Keyword),
                this.GetParameter(PARM_URL, EDataType.VarChar, 200, menuInfo.Url),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, menuInfo.ChannelID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, menuInfo.ContentID),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, menuInfo.ParentID),
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
                        menuID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "wx_Menu");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return menuID;
        }

        public void Update(MenuInfo menuInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, menuInfo.PublishmentSystemID),
                this.GetParameter(PARM_MENU_NAME, EDataType.NVarChar, 50, menuInfo.MenuName),
                this.GetParameter(PARM_MENU_TYPE, EDataType.VarChar, 50, EMenuTypeUtils.GetValue(menuInfo.MenuType)),
                this.GetParameter(PARM_KEYWORD, EDataType.NVarChar, 50, menuInfo.Keyword),
                this.GetParameter(PARM_URL, EDataType.VarChar, 200, menuInfo.Url),
                this.GetParameter(PARM_CHANNEL_ID, EDataType.Integer, menuInfo.ChannelID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, menuInfo.ContentID),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, menuInfo.ParentID),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, menuInfo.Taxis),
                this.GetParameter(PARM_MENU_ID, EDataType.Integer, menuInfo.MenuID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int menuID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_MENU_ID, EDataType.Integer, menuID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        public MenuInfo GetMenuInfo(int menuID)
        {
            MenuInfo menuInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_MENU_ID, EDataType.Integer, menuID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    menuInfo = new MenuInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), EMenuTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9));
                }
                rdr.Close();
            }

            return menuInfo;
        }

        public IEnumerable GetDataSource(int publishmentSystemID, int parentID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL, parms);
            return enumerable;
        }

        public int GetCount(int parentID)
        {
            string sqlString = "SELECT COUNT(*) FROM wx_Menu WHERE ParentID = " + parentID;
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public List<MenuInfo> GetMenuInfoList(int publishmentSystemID, int parentID)
        {
            List<MenuInfo> list = new List<MenuInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    MenuInfo menuInfo = new MenuInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), EMenuTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9));
                    list.Add(menuInfo);
                }
                rdr.Close();
            }

            if (parentID > 0)
            {
                list.Reverse();
            }

            return list;
        }

        public bool UpdateTaxisToUp(int parentID, int menuID)
        {
            string sqlString = string.Format("SELECT TOP 1 MenuID, Taxis FROM wx_Menu WHERE (Taxis > (SELECT Taxis FROM wx_Menu WHERE MenuID = {0} AND ParentID = {1})) AND ParentID = {1} ORDER BY Taxis", menuID, parentID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = rdr.GetInt32(0);
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(menuID);

            if (higherID > 0)
            {
                SetTaxis(menuID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int parentID, int menuID)
        {
            string sqlString = string.Format("SELECT TOP 1 MenuID, Taxis FROM wx_Menu WHERE (Taxis < (SELECT Taxis FROM wx_Menu WHERE MenuID = {0} AND ParentID = {1})) AND ParentID = {1} ORDER BY Taxis DESC", menuID, parentID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = rdr.GetInt32(0);
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(menuID);

            if (lowerID > 0)
            {
                SetTaxis(menuID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int parentID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM wx_Menu WHERE ParentID = {0}", parentID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int menuID)
        {
            string sqlString = string.Format("SELECT Taxis FROM wx_Menu WHERE MenuID = {0}", menuID);
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

        private void SetTaxis(int menuID, int taxis)
        {
            string sqlString = string.Format("UPDATE wx_Menu SET Taxis = {0} WHERE MenuID = {1}", taxis, menuID);
            this.ExecuteNonQuery(sqlString);
        }

        public List<MenuInfo> GetMenuInfoList(int publishmentSystemID)
        {
            List<MenuInfo> list = new List<MenuInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, 0)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALLBY, parms))
            {
                while (rdr.Read())
                {
                    MenuInfo menuInfo = new MenuInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), EMenuTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9));
                    list.Add(menuInfo);
                }
                rdr.Close();
            }

            return list;
        }
    }
}