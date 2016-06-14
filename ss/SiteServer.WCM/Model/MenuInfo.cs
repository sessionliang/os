using System;

namespace SiteServer.WeiXin.Model
{
	public class MenuInfo
	{
        private int menuID;
        private int publishmentSystemID;
        private string menuName;
        private EMenuType menuType;
        private string keyword;
        private string url;
        private int channelID;
        private int contentID;
        private int parentID;
        private int taxis;

		public MenuInfo()
		{
            this.menuID = 0;
            this.publishmentSystemID = 0;
            this.menuName = string.Empty;
            this.menuType = EMenuType.Keyword;
            this.keyword = string.Empty;
            this.url = string.Empty;
            this.channelID = 0;
            this.contentID = 0;
            this.parentID = 0;
            this.taxis = 0;
		}

        public MenuInfo(int menuID, int publishmentSystemID, string menuName, EMenuType menuType, string keyword, string url, int channelID, int contentID, int parentID, int taxis)
		{
            this.menuID = menuID;
            this.publishmentSystemID = publishmentSystemID;
            this.menuName = menuName;
            this.menuType = menuType;
            this.keyword = keyword;
            this.url = url;
            this.channelID = channelID;
            this.contentID = contentID;
            this.parentID = parentID;
            this.taxis = taxis;
		}

        public int MenuID
        {
            get { return menuID; }
            set { menuID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }

        public EMenuType MenuType
        {
            get { return menuType; }
            set { menuType = value; }
        }

        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public int ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }

        public int ContentID
        {
            get { return contentID; }
            set { contentID = value; }
        }

        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
	}
}
