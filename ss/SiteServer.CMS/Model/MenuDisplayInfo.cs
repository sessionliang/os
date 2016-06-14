using System;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	[Serializable]
	public class MenuDisplayInfo
	{
		private int menuDisplayID;
		private int publishmentSystemID;
		private string menuDisplayName;
		//外观
		private string vertical;
		private string fontFamily;
		private int fontSize;
		private string fontWeight;
		private string fontStyle;
		private string menuItemHAlign;
		private string menuItemVAlign;
		private string fontColor;
		private string menuItemBgColor;
		private string fontColorHilite;
		private string menuHiliteBgColor;
		//位置
		private string xPosition;
		private string yPosition;
		private string hideOnMouseOut;
		//高级
		private int menuWidth;
		private int menuItemHeight;
		private int menuItemPadding;
		private int menuItemSpacing;
		private int menuItemIndent;
		private int hideTimeout;
		private string menuBgOpaque;
		private int menuBorder;
		private string bgColor;
		private string menuBorderBgColor;
		private string menuLiteBgColor;
		private string childMenuIcon;
		//其他
		private DateTime addDate;
        private bool isDefault;
		private string description;

		public MenuDisplayInfo()
		{
			this.menuDisplayID = 0;
			this.publishmentSystemID = 0;
			this.menuDisplayName = string.Empty;
			this.vertical = string.Empty;
			this.fontFamily = string.Empty;
			this.fontSize = 0;
			this.fontWeight = string.Empty;
			this.fontStyle = string.Empty;
			this.menuItemHAlign = string.Empty;
			this.menuItemVAlign = string.Empty;
			this.fontColor = string.Empty;
			this.menuItemBgColor = string.Empty;
			this.fontColorHilite = string.Empty;
			this.menuHiliteBgColor = string.Empty;
			this.xPosition = "0";
			this.yPosition = "0";
			this.hideOnMouseOut = string.Empty;
			this.menuWidth = 0;
			this.menuItemHeight = 0;
			this.menuItemPadding = 0;
			this.menuItemSpacing = 0;
			this.menuItemIndent = 0;
			this.hideTimeout = 0;
			this.menuBgOpaque = string.Empty;
			this.menuBorder = 0;
			this.bgColor = string.Empty;
			this.menuBorderBgColor = string.Empty;
			this.menuLiteBgColor = string.Empty;
			this.childMenuIcon = string.Empty;
			this.addDate = DateTime.Now;
			this.isDefault = false;
			this.description = string.Empty;
		}

        public MenuDisplayInfo(int menuDisplayID, int publishmentSystemID, string menuDisplayName, string vertical, string fontFamily, int fontSize, string fontWeight, string fontStyle, string menuItemHAlign, string menuItemVAlign, string fontColor, string menuItemBgColor, string fontColorHilite, string menuHiliteBgColor, string xPosition, string yPosition, string hideOnMouseOut, int menuWidth, int menuItemHeight, int menuItemPadding, int menuItemSpacing, int menuItemIndent, int hideTimeout, string menuBgOpaque, int menuBorder, string bgColor, string menuBorderBgColor, string menuLiteBgColor, string childMenuIcon, DateTime addDate, bool isDefault, string description) 
		{
			this.menuDisplayID = menuDisplayID;
			this.publishmentSystemID = publishmentSystemID;
			this.menuDisplayName = menuDisplayName;
			this.vertical = vertical;
			this.fontFamily = fontFamily;
			this.fontSize = fontSize;
			this.fontWeight = fontWeight;
			this.fontStyle = fontStyle;
			this.menuItemHAlign = menuItemHAlign;
			this.menuItemVAlign = menuItemVAlign;
			this.fontColor = fontColor;
			this.menuItemBgColor = menuItemBgColor;
			this.fontColorHilite = fontColorHilite;
			this.menuHiliteBgColor = menuHiliteBgColor;
			this.xPosition = xPosition;
			this.yPosition = yPosition;
			this.hideOnMouseOut = hideOnMouseOut;
			this.menuWidth = menuWidth;
			this.menuItemHeight = menuItemHeight;
			this.menuItemPadding = menuItemPadding;
			this.menuItemSpacing = menuItemSpacing;
			this.menuItemIndent = menuItemIndent;
			this.hideTimeout = hideTimeout;
			this.menuBgOpaque = menuBgOpaque;
			this.menuBorder = menuBorder;
			this.bgColor = bgColor;
			this.menuBorderBgColor = menuBorderBgColor;
			this.menuLiteBgColor = menuLiteBgColor;
			this.childMenuIcon = childMenuIcon;
			this.addDate = addDate;
			this.isDefault = isDefault;
			this.description = description;
		}

		public int MenuDisplayID
		{
			get{ return menuDisplayID; }
			set{ menuDisplayID = value; }
		}

		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

		public string MenuDisplayName
		{
			get{ return menuDisplayName; }
			set{ menuDisplayName = value; }
		}

		public string Vertical
		{
			get{ return vertical; }
			set{ vertical = value; }
		}

		public string FontFamily
		{
			get{ return fontFamily; }
			set{ fontFamily = value; }
		}

		public int FontSize
		{
			get{ return fontSize; }
			set{ fontSize = value; }
		}

		//
		public string FontWeight
		{
			get{ return fontWeight; }
			set{ fontWeight = value; }
		}

		public string FontStyle
		{
			get{ return fontStyle; }
			set{ fontStyle = value; }
		}

		public string MenuItemHAlign
		{
			get{ return menuItemHAlign; }
			set{ menuItemHAlign = value; }
		}

		public string MenuItemVAlign
		{
			get{ return menuItemVAlign; }
			set{ menuItemVAlign = value; }
		}

		public string FontColor
		{
			get{ return fontColor; }
			set{ fontColor = value; }
		}

		public string MenuItemBgColor
		{
			get{ return menuItemBgColor; }
			set{ menuItemBgColor = value; }
		}

		public string FontColorHilite
		{
			get{ return fontColorHilite; }
			set{ fontColorHilite = value; }
		}

		public string MenuHiliteBgColor
		{
			get{ return menuHiliteBgColor; }
			set{ menuHiliteBgColor = value; }
		}

		public string YPosition
		{
			get{ return yPosition; }
			set{ yPosition = value; }
		}

		public string XPosition
		{
			get{ return xPosition; }
			set{ xPosition = value; }
		}

		public string HideOnMouseOut
		{
			get{ return hideOnMouseOut; }
			set{ hideOnMouseOut = value; }
		}

		public int MenuWidth
		{
			get{ return menuWidth; }
			set{ menuWidth = value; }
		}

		public int MenuItemHeight
		{
			get{ return menuItemHeight; }
			set{ menuItemHeight = value; }
		}

		public int MenuItemPadding
		{
			get{ return menuItemPadding; }
			set{ menuItemPadding = value; }
		}

		public int MenuItemSpacing
		{
			get{ return menuItemSpacing; }
			set{ menuItemSpacing = value; }
		}

		public int MenuItemIndent
		{
			get{ return menuItemIndent; }
			set{ menuItemIndent = value; }
		}

		public int HideTimeout
		{
			get{ return hideTimeout; }
			set{ hideTimeout = value; }
		}

		public string MenuBgOpaque
		{
			get{ return menuBgOpaque; }
			set{ menuBgOpaque = value; }
		}

		public int MenuBorder
		{
			get{ return menuBorder; }
			set{ menuBorder = value; }
		}

		public string BGColor
		{
			get{ return bgColor; }
			set{ bgColor = value; }
		}

		public string MenuBorderBgColor
		{
			get{ return menuBorderBgColor; }
			set{ menuBorderBgColor = value; }
		}

		public string MenuLiteBgColor
		{
			get{ return menuLiteBgColor; }
			set{ menuLiteBgColor = value; }
		}

		public string ChildMenuIcon
		{
			get{ return childMenuIcon; }
			set{ childMenuIcon = value; }
		}
		//

		public DateTime AddDate
		{
			get{ return addDate; }
			set{ addDate = value; }
		}

        public bool IsDefault
		{
			get{ return isDefault; }
			set{ isDefault = value; }
		}

		public string Description
		{
			get{ return description; }
			set{ description = value; }
		}

	}
}
