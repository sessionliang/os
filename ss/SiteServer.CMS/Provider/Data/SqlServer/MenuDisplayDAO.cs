using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
	public class MenuDisplayDAO : DataProviderBase, IMenuDisplayDAO
	{

		// Static constants
		private const string SQL_SELECT_MENU_DISPLAY = "SELECT MenuDisplayID, PublishmentSystemID, MenuDisplayName, Vertical, FontFamily, FontSize, FontWeight, FontStyle, MenuItemHAlign, MenuItemVAlign, FontColor, MenuItemBgColor, FontColorHilite, MenuHiliteBgColor, XPosition, YPosition, HideOnMouseOut, MenuWidth, MenuItemHeight, MenuItemPadding, MenuItemSpacing, MenuItemIndent, HideTimeout, MenuBgOpaque, MenuBorder, BGColor, MenuBorderBgColor, MenuLiteBgColor, ChildMenuIcon, AddDate, IsDefault, Description FROM siteserver_MenuDisplay WHERE MenuDisplayID = @MenuDisplayID";

		private const string SQL_SELECT_MENU_DISPLAY_BY_MENU_DISPLAY_NAME = "SELECT MenuDisplayID, PublishmentSystemID, MenuDisplayName, Vertical, FontFamily, FontSize, FontWeight, FontStyle, MenuItemHAlign, MenuItemVAlign, FontColor, MenuItemBgColor, FontColorHilite, MenuHiliteBgColor, XPosition, YPosition, HideOnMouseOut, MenuWidth, MenuItemHeight, MenuItemPadding, MenuItemSpacing, MenuItemIndent, HideTimeout, MenuBgOpaque, MenuBorder, BGColor, MenuBorderBgColor, MenuLiteBgColor, ChildMenuIcon, AddDate, IsDefault, Description FROM siteserver_MenuDisplay WHERE PublishmentSystemID = @PublishmentSystemID AND MenuDisplayName = @MenuDisplayName";

		private const string SQL_SELECT_ALL_MENU_DISPLAY = "SELECT MenuDisplayID, PublishmentSystemID, MenuDisplayName, Vertical, FontFamily, FontSize, FontWeight, FontStyle, MenuItemHAlign, MenuItemVAlign, FontColor, MenuItemBgColor, FontColorHilite, MenuHiliteBgColor, XPosition, YPosition, HideOnMouseOut, MenuWidth, MenuItemHeight, MenuItemPadding, MenuItemSpacing, MenuItemIndent, HideTimeout, MenuBgOpaque, MenuBorder, BGColor, MenuBorderBgColor, MenuLiteBgColor, ChildMenuIcon, AddDate, IsDefault, Description FROM siteserver_MenuDisplay WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY MenuDisplayID";

		private const string SQL_SELECT_ALL_MENU_DISPLAY_NAME = "SELECT MenuDisplayName FROM siteserver_MenuDisplay WHERE PublishmentSystemID = @PublishmentSystemID";

		private const string SQL_SELECT_MENU_DISPLAY_NAME = "SELECT MenuDisplayName FROM siteserver_MenuDisplay WHERE MenuDisplayID = @MenuDisplayID";

		private const string SQL_SELECT_MENU_DISPLAY_ID_BY_NAME = "SELECT MenuDisplayID FROM siteserver_MenuDisplay WHERE PublishmentSystemID = @PublishmentSystemID AND MenuDisplayName = @MenuDisplayName";

		private const string SQL_UPDATE_MENU_DISPLAY = "UPDATE siteserver_MenuDisplay SET MenuDisplayName = @MenuDisplayName, Vertical = @Vertical, FontFamily = @FontFamily, FontSize = @FontSize, FontWeight = @FontWeight, FontStyle = @FontStyle, MenuItemHAlign = @MenuItemHAlign, MenuItemVAlign = @MenuItemVAlign, FontColor = @FontColor, MenuItemBgColor = @MenuItemBgColor, FontColorHilite = @FontColorHilite, MenuHiliteBgColor = @MenuHiliteBgColor, XPosition = @XPosition, YPosition = @YPosition, HideOnMouseOut = @HideOnMouseOut, MenuWidth = @MenuWidth, MenuItemHeight = @MenuItemHeight, MenuItemPadding = @MenuItemPadding, MenuItemSpacing = @MenuItemSpacing, MenuItemIndent = @MenuItemIndent, HideTimeout = @HideTimeout, MenuBgOpaque = @MenuBgOpaque, MenuBorder = @MenuBorder, BGColor = @BGColor, MenuBorderBgColor = @MenuBorderBgColor, MenuLiteBgColor = @MenuLiteBgColor, ChildMenuIcon = @ChildMenuIcon, AddDate = @AddDate, IsDefault = @IsDefault, Description = @Description WHERE  MenuDisplayID = @MenuDisplayID";

		private const string SQL_DELETE_MENU_DISPLAY = "DELETE FROM siteserver_MenuDisplay WHERE  MenuDisplayID = @MenuDisplayID";

		private const string PARM_MENU_DISPLAY_ID = "@MenuDisplayID";
		private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
		private const string PARM_MENU_DISPLAY_NAME = "@MenuDisplayName";
		private const string PARM_VERTICAL = "@Vertical";
		private const string PARM_FONT_FAMILY = "@FontFamily";
		private const string PARM_FONT_SIZE = "@FontSize";

		private const string PARM_FONT_WEIGHT = "@FontWeight";
		private const string PARM_FONT_STYLE = "@FontStyle";
		private const string PARM_MENU_ITEM_H_ALIGN = "@MenuItemHAlign";
		private const string PARM_MENU_ITEM_V_ALIGN = "@MenuItemVAlign";
		private const string PARM_FONT_COLOR = "@FontColor";
		private const string PARM_MENU_ITEM_BG_COLOR = "@MenuItemBgColor";
		private const string PARM_FONT_COLOR_HILITE = "@FontColorHilite";
		private const string PARM_MENU_HILITE_BG_COLOR = "@MenuHiliteBgColor";
		private const string PARM_X_POSITION = "@XPosition";
		private const string PARM_Y_POSITION = "@YPosition";
		private const string PARM_HIDE_ON_MOUSE_OUT = "@HideOnMouseOut";
		private const string PARM_MENU_WIDTH = "@MenuWidth";
		private const string PARM_MENU_ITEM_HEIGHT = "@MenuItemHeight";
		private const string PARM_MENU_ITEM_PADDING = "@MenuItemPadding";
		private const string PARM_MENU_ITEM_SPACING = "@MenuItemSpacing";
		private const string PARM_MENU_ITEM_INDENT = "@MenuItemIndent";
		private const string PARM_HIDE_TIMEOUT = "@HideTimeout";
		private const string PARM_MENU_BG_OPAQUE = "@MenuBgOpaque";
		private const string PARM_MENU_BORDER = "@MenuBorder";
		private const string PARM_BG_COLOR = "@BGColor";
		private const string PARM_MENU_BORDER_BG_COLOR = "@MenuBorderBgColor";
		private const string PARM_MENU_LITE_BG_COLOR = "@MenuLiteBgColor";
		private const string PARM_CHILD_MENU_ICON = "@ChildMenuIcon";
		private const string PARM_ADD_DATE = "@AddDate";
		private const string PARM_IS_DEFAULT = "@IsDefault";
		private const string PARM_DESCRIPTION = "@Description";

		public void Insert(MenuDisplayInfo info) 
		{
			if (info.IsDefault)
			{
				this.SetAllMenuDisplayDefaultToFalse(info.PublishmentSystemID);
			}

            string sqlString = "INSERT INTO siteserver_MenuDisplay (PublishmentSystemID, MenuDisplayName, Vertical, FontFamily, FontSize, FontWeight, FontStyle, MenuItemHAlign, MenuItemVAlign, FontColor, MenuItemBgColor, FontColorHilite, MenuHiliteBgColor, XPosition, YPosition, HideOnMouseOut, MenuWidth, MenuItemHeight, MenuItemPadding, MenuItemSpacing, MenuItemIndent, HideTimeout, MenuBgOpaque, MenuBorder, BGColor, MenuBorderBgColor, MenuLiteBgColor, ChildMenuIcon, AddDate, IsDefault, Description) VALUES (@PublishmentSystemID, @MenuDisplayName, @Vertical, @FontFamily, @FontSize, @FontWeight, @FontStyle, @MenuItemHAlign, @MenuItemVAlign, @FontColor, @MenuItemBgColor, @FontColorHilite, @MenuHiliteBgColor, @XPosition, @YPosition, @HideOnMouseOut, @MenuWidth, @MenuItemHeight, @MenuItemPadding, @MenuItemSpacing, @MenuItemIndent, @HideTimeout, @MenuBgOpaque, @MenuBorder, @BGColor, @MenuBorderBgColor, @MenuLiteBgColor, @ChildMenuIcon, @AddDate, @IsDefault, @Description)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_MenuDisplay (MenuDisplayID, PublishmentSystemID, MenuDisplayName, Vertical, FontFamily, FontSize, FontWeight, FontStyle, MenuItemHAlign, MenuItemVAlign, FontColor, MenuItemBgColor, FontColorHilite, MenuHiliteBgColor, XPosition, YPosition, HideOnMouseOut, MenuWidth, MenuItemHeight, MenuItemPadding, MenuItemSpacing, MenuItemIndent, HideTimeout, MenuBgOpaque, MenuBorder, BGColor, MenuBorderBgColor, MenuLiteBgColor, ChildMenuIcon, AddDate, IsDefault, Description) VALUES (siteserver_MenuDisplay_SEQ.NEXTVAL, @PublishmentSystemID, @MenuDisplayName, @Vertical, @FontFamily, @FontSize, @FontWeight, @FontStyle, @MenuItemHAlign, @MenuItemVAlign, @FontColor, @MenuItemBgColor, @FontColorHilite, @MenuHiliteBgColor, @XPosition, @YPosition, @HideOnMouseOut, @MenuWidth, @MenuItemHeight, @MenuItemPadding, @MenuItemSpacing, @MenuItemIndent, @HideTimeout, @MenuBgOpaque, @MenuBorder, @BGColor, @MenuBorderBgColor, @MenuLiteBgColor, @ChildMenuIcon, @AddDate, @IsDefault, @Description)";
            }

			IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
				this.GetParameter(PARM_MENU_DISPLAY_NAME, EDataType.VarChar, 50, info.MenuDisplayName),
				this.GetParameter(PARM_VERTICAL, EDataType.VarChar, 50, info.Vertical),
				this.GetParameter(PARM_FONT_FAMILY, EDataType.VarChar, 200, info.FontFamily),
				this.GetParameter(PARM_FONT_SIZE, EDataType.Integer, info.FontSize),
				this.GetParameter(PARM_FONT_WEIGHT, EDataType.VarChar, 50, info.FontWeight),
				this.GetParameter(PARM_FONT_STYLE, EDataType.VarChar, 50, info.FontStyle),
				this.GetParameter(PARM_MENU_ITEM_H_ALIGN, EDataType.VarChar, 50, info.MenuItemHAlign),
				this.GetParameter(PARM_MENU_ITEM_V_ALIGN, EDataType.VarChar, 50, info.MenuItemVAlign),
				this.GetParameter(PARM_FONT_COLOR, EDataType.VarChar, 50, info.FontColor),
				this.GetParameter(PARM_MENU_ITEM_BG_COLOR, EDataType.VarChar, 50, info.MenuItemBgColor),
				this.GetParameter(PARM_FONT_COLOR_HILITE, EDataType.VarChar, 50, info.FontColorHilite),
				this.GetParameter(PARM_MENU_HILITE_BG_COLOR, EDataType.VarChar, 200, info.MenuHiliteBgColor),
				this.GetParameter(PARM_X_POSITION, EDataType.VarChar, 50, info.XPosition),
				this.GetParameter(PARM_Y_POSITION, EDataType.VarChar, 50, info.YPosition),
				this.GetParameter(PARM_HIDE_ON_MOUSE_OUT, EDataType.VarChar, 50, info.HideOnMouseOut),
				this.GetParameter(PARM_MENU_WIDTH, EDataType.Integer, info.MenuWidth),
				this.GetParameter(PARM_MENU_ITEM_HEIGHT, EDataType.Integer, info.MenuItemHeight),
				this.GetParameter(PARM_MENU_ITEM_PADDING, EDataType.Integer, info.MenuItemPadding),
				this.GetParameter(PARM_MENU_ITEM_SPACING, EDataType.Integer, info.MenuItemSpacing),
				this.GetParameter(PARM_MENU_ITEM_INDENT, EDataType.Integer, info.MenuItemIndent),
				this.GetParameter(PARM_HIDE_TIMEOUT, EDataType.Integer, info.HideTimeout),
				this.GetParameter(PARM_MENU_BG_OPAQUE, EDataType.VarChar, 50, info.MenuBgOpaque),
				this.GetParameter(PARM_MENU_BORDER, EDataType.Integer, info.MenuBorder),
				this.GetParameter(PARM_BG_COLOR, EDataType.VarChar, 50, info.BGColor),
				this.GetParameter(PARM_MENU_BORDER_BG_COLOR, EDataType.VarChar, 50, info.MenuBorderBgColor),
				this.GetParameter(PARM_MENU_LITE_BG_COLOR, EDataType.VarChar, 50, info.MenuLiteBgColor),
				this.GetParameter(PARM_CHILD_MENU_ICON, EDataType.VarChar, 200, info.ChildMenuIcon),
				this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, info.AddDate),
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, info.IsDefault.ToString()),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NText, info.Description)
			};

            this.ExecuteNonQuery(sqlString, insertParms);
		}

		public void Update(MenuDisplayInfo info) 
		{
			if (info.IsDefault)
			{
				this.SetAllMenuDisplayDefaultToFalse(info.PublishmentSystemID);
			}

			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_MENU_DISPLAY_NAME, EDataType.VarChar, 50, info.MenuDisplayName),
				this.GetParameter(PARM_VERTICAL, EDataType.VarChar, 50, info.Vertical),
				this.GetParameter(PARM_FONT_FAMILY, EDataType.VarChar, 200, info.FontFamily),
				this.GetParameter(PARM_FONT_SIZE, EDataType.Integer, info.FontSize),
				this.GetParameter(PARM_FONT_WEIGHT, EDataType.VarChar, 50, info.FontWeight),
				this.GetParameter(PARM_FONT_STYLE, EDataType.VarChar, 50, info.FontStyle),
				this.GetParameter(PARM_MENU_ITEM_H_ALIGN, EDataType.VarChar, 50, info.MenuItemHAlign),
				this.GetParameter(PARM_MENU_ITEM_V_ALIGN, EDataType.VarChar, 50, info.MenuItemVAlign),
				this.GetParameter(PARM_FONT_COLOR, EDataType.VarChar, 50, info.FontColor),
				this.GetParameter(PARM_MENU_ITEM_BG_COLOR, EDataType.VarChar, 50, info.MenuItemBgColor),
				this.GetParameter(PARM_FONT_COLOR_HILITE, EDataType.VarChar, 50, info.FontColorHilite),
				this.GetParameter(PARM_MENU_HILITE_BG_COLOR, EDataType.VarChar, 200, info.MenuHiliteBgColor),
				this.GetParameter(PARM_X_POSITION, EDataType.VarChar, 50, info.XPosition),
				this.GetParameter(PARM_Y_POSITION, EDataType.VarChar, 50, info.YPosition),
				this.GetParameter(PARM_HIDE_ON_MOUSE_OUT, EDataType.VarChar, 50, info.HideOnMouseOut),
				this.GetParameter(PARM_MENU_WIDTH, EDataType.Integer, info.MenuWidth),
				this.GetParameter(PARM_MENU_ITEM_HEIGHT, EDataType.Integer, info.MenuItemHeight),
				this.GetParameter(PARM_MENU_ITEM_PADDING, EDataType.Integer, info.MenuItemPadding),
				this.GetParameter(PARM_MENU_ITEM_SPACING, EDataType.Integer, info.MenuItemSpacing),
				this.GetParameter(PARM_MENU_ITEM_INDENT, EDataType.Integer, info.MenuItemIndent),
				this.GetParameter(PARM_HIDE_TIMEOUT, EDataType.Integer, info.HideTimeout),
				this.GetParameter(PARM_MENU_BG_OPAQUE, EDataType.VarChar, 50, info.MenuBgOpaque),
				this.GetParameter(PARM_MENU_BORDER, EDataType.Integer, info.MenuBorder),
				this.GetParameter(PARM_BG_COLOR, EDataType.VarChar, 50, info.BGColor),
				this.GetParameter(PARM_MENU_BORDER_BG_COLOR, EDataType.VarChar, 50, info.MenuBorderBgColor),
				this.GetParameter(PARM_MENU_LITE_BG_COLOR, EDataType.VarChar, 50, info.MenuLiteBgColor),
				this.GetParameter(PARM_CHILD_MENU_ICON, EDataType.VarChar, 200, info.ChildMenuIcon),
				this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, info.AddDate),
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, info.IsDefault.ToString()),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NText, info.Description),
				this.GetParameter(PARM_MENU_DISPLAY_ID, EDataType.Integer, info.MenuDisplayID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_MENU_DISPLAY, updateParms);
		}

		private void SetAllMenuDisplayDefaultToFalse(int publishmentSystemID)
		{
            string sqlString = "UPDATE siteserver_MenuDisplay SET IsDefault = @IsDefault WHERE PublishmentSystemID = @PublishmentSystemID";

			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, false.ToString()),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            this.ExecuteNonQuery(sqlString, updateParms);
		}

		public void SetDefault(int menuDisplayID)
		{
			MenuDisplayInfo info = this.GetMenuDisplayInfo(menuDisplayID);
			this.SetAllMenuDisplayDefaultToFalse(info.PublishmentSystemID);

            string sqlString = "UPDATE siteserver_MenuDisplay SET IsDefault = @IsDefault WHERE MenuDisplayID = @MenuDisplayID";

			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, true.ToString()),
				this.GetParameter(PARM_MENU_DISPLAY_ID, EDataType.Integer, menuDisplayID)
			};

            this.ExecuteNonQuery(sqlString, updateParms);
		}

		public void Delete(int menuDisplayID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_MENU_DISPLAY_ID, EDataType.Integer, menuDisplayID)
			};

            this.ExecuteNonQuery(SQL_DELETE_MENU_DISPLAY, parms);
		}

		public MenuDisplayInfo GetMenuDisplayInfo(int menuDisplayID)
		{
			MenuDisplayInfo info = null;
			
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_MENU_DISPLAY_ID, EDataType.Integer, menuDisplayID)
			};
			
			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MENU_DISPLAY, parms)) 
			{
				if (rdr.Read()) 
				{
                    info = new MenuDisplayInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString(), rdr.GetInt32(17), rdr.GetInt32(18), rdr.GetInt32(19), rdr.GetInt32(20), rdr.GetInt32(21), rdr.GetInt32(22), rdr.GetValue(23).ToString(), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString(), rdr.GetValue(28).ToString(), rdr.GetDateTime(29), TranslateUtils.ToBool(rdr.GetValue(30).ToString()), rdr.GetValue(31).ToString());
				}
				rdr.Close();
			}

			return info;
		}

		public string GetMenuDisplayName(int menuDisplayID)
		{
			string menuDisplayName = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(MenuDisplayDAO.PARM_MENU_DISPLAY_ID, EDataType.Integer, menuDisplayID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MENU_DISPLAY_NAME, parms)) 
			{
				if (rdr.Read()) 
				{
                    menuDisplayName = rdr.GetValue(0).ToString();
				}
				rdr.Close();
			}

			return menuDisplayName;
		}

		public int GetMenuDisplayIDByName(int publishmentSystemID, string menuDisplayName)
		{
			int menuDisplayID = 0;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(MenuDisplayDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_MENU_DISPLAY_NAME, EDataType.VarChar, 50, menuDisplayName)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MENU_DISPLAY_ID_BY_NAME, parms)) 
			{
				if (rdr.Read()) 
				{
					if (!rdr.IsDBNull(0))
					{
						menuDisplayID = Convert.ToInt32(rdr[0]);
					}
				}
				rdr.Close();
			}

			return menuDisplayID;
		}

		public MenuDisplayInfo GetMenuDisplayInfoByMenuDisplayName(int publishmentSystemID, string menuDisplayName)
		{
			MenuDisplayInfo info = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_MENU_DISPLAY_NAME, EDataType.VarChar, 50, menuDisplayName)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MENU_DISPLAY_BY_MENU_DISPLAY_NAME, parms))
			{
				if (rdr.Read())
				{
                    info = new MenuDisplayInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString(), rdr.GetInt32(17), rdr.GetInt32(18), rdr.GetInt32(19), rdr.GetInt32(20), rdr.GetInt32(21), rdr.GetInt32(22), rdr.GetValue(23).ToString(), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString(), rdr.GetValue(28).ToString(), rdr.GetDateTime(29), TranslateUtils.ToBool(rdr.GetValue(30).ToString()), rdr.GetValue(31).ToString());
				}
				rdr.Close();
			}

			return info;
		}

		public MenuDisplayInfo GetDefaultMenuDisplayInfo(int publishmentSystemID)
		{
			MenuDisplayInfo info = null;
			
			string sqlString = "SELECT MenuDisplayID, PublishmentSystemID, MenuDisplayName, Vertical, FontFamily, FontSize, FontWeight, FontStyle, MenuItemHAlign, MenuItemVAlign, FontColor, MenuItemBgColor, FontColorHilite, MenuHiliteBgColor, XPosition, YPosition, HideOnMouseOut, MenuWidth, MenuItemHeight, MenuItemPadding, MenuItemSpacing, MenuItemIndent, HideTimeout, MenuBgOpaque, MenuBorder, BGColor, MenuBorderBgColor, MenuLiteBgColor, ChildMenuIcon, AddDate, IsDefault, Description FROM siteserver_MenuDisplay WHERE PublishmentSystemID = @PublishmentSystemID AND IsDefault = @IsDefault";

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, true.ToString())
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms)) 
			{
				if (rdr.Read()) 
				{
                    info = new MenuDisplayInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString(), rdr.GetInt32(17), rdr.GetInt32(18), rdr.GetInt32(19), rdr.GetInt32(20), rdr.GetInt32(21), rdr.GetInt32(22), rdr.GetValue(23).ToString(), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString(), rdr.GetValue(28).ToString(), rdr.GetDateTime(29), TranslateUtils.ToBool(rdr.GetValue(30).ToString()), rdr.GetValue(31).ToString());
				}
				rdr.Close();
			}

			return info;
		}

		public string GetImportMenuDisplayName(int publishmentSystemID, string menuDisplayName)
		{
			string importMenuDisplayName = "";
			if (menuDisplayName.IndexOf("_") != -1)
			{
				int menuDisplayName_Count = 0;
				string lastMenuDisplayName = menuDisplayName.Substring(menuDisplayName.LastIndexOf("_") + 1);
				string firstMenuDisplayName = menuDisplayName.Substring(0, menuDisplayName.Length - lastMenuDisplayName.Length);
				try
				{
					menuDisplayName_Count = int.Parse(lastMenuDisplayName);
				}
				catch { }
				menuDisplayName_Count++;
				importMenuDisplayName = firstMenuDisplayName + menuDisplayName_Count;
			}
			else
			{
				importMenuDisplayName = menuDisplayName + "_1";
			}

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_MENU_DISPLAY_NAME, EDataType.VarChar, 50, importMenuDisplayName)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MENU_DISPLAY_BY_MENU_DISPLAY_NAME, parms))
			{
				if (rdr.Read())
				{
					importMenuDisplayName = GetImportMenuDisplayName(publishmentSystemID, importMenuDisplayName);
				}
				rdr.Close();
			}

			return importMenuDisplayName;
		}

		public IEnumerable GetDataSource(int publishmentSystemID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(MenuDisplayDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_MENU_DISPLAY, parms);
			return enumerable;
		}

		public ArrayList GetMenuDisplayInfoArrayList(int publishmentSystemID)
		{
			ArrayList arraylist = new ArrayList();

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(MenuDisplayDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_MENU_DISPLAY, parms))
			{
				while (rdr.Read())
				{
                    MenuDisplayInfo info = new MenuDisplayInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString(), rdr.GetValue(13).ToString(), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString(), rdr.GetInt32(17), rdr.GetInt32(18), rdr.GetInt32(19), rdr.GetInt32(20), rdr.GetInt32(21), rdr.GetInt32(22), rdr.GetValue(23).ToString(), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString(), rdr.GetValue(28).ToString(), rdr.GetDateTime(29), TranslateUtils.ToBool(rdr.GetValue(30).ToString()), rdr.GetValue(31).ToString());
					arraylist.Add(info);
				}
				rdr.Close();
			}

			return arraylist;
		}

		public ArrayList GetMenuDisplayNameCollection(int publishmentSystemID)
		{
			ArrayList list = new ArrayList();

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(MenuDisplayDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_MENU_DISPLAY_NAME, parms)) 
			{
				while (rdr.Read()) 
				{					
					list.Add(rdr.GetValue(0).ToString());
				}
				rdr.Close();
			}

			return list;
		}

		public void CreateDefaultMenuDisplayInfo(int publishmentSystemID)
		{
			ArrayList arraylist = BaseTable.GetDefaultMenuDisplayArrayList(publishmentSystemID);
			foreach (MenuDisplayInfo menuDisplayInfo in arraylist)
			{
				this.Insert(menuDisplayInfo);
			}
		}
	}
}
