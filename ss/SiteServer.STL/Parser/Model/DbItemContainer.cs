namespace SiteServer.STL.Parser.Model
{
    public class DbItemContainer
    {
        private DbItemInfo channelItem;
        private DbItemInfo contentItem;
        private DbItemInfo commentItem;
        private DbItemInfo inputItem;
        private DbItemInfo websiteMessageItem;
        private DbItemInfo sqlItem;
        private DbItemInfo siteItem;
        private DbItemInfo photoItem;
        private DbItemInfo teleplayItem;
        private DbItemInfo eachItem;
        private DbItemInfo specItem;
        private DbItemInfo filterItem;

        private DbItemContainer() { }

        public static void PopChannelItem(PageInfo pageInfo)
        {
            if (pageInfo.ChannelItems.Count > 0)
            {
                pageInfo.ChannelItems.Pop();
            }
        }

        public static void PopContentItem(PageInfo pageInfo)
        {
            if (pageInfo.ContentItems.Count > 0)
            {
                pageInfo.ContentItems.Pop();
            }
        }

        public static void PopCommentItem(PageInfo pageInfo)
        {
            if (pageInfo.CommentItems.Count > 0)
            {
                pageInfo.CommentItems.Pop();
            }
        }

        public static void PopInputItem(PageInfo pageInfo)
        {
            if (pageInfo.InputItems.Count > 0)
            {
                pageInfo.InputItems.Pop();
            }
        }

        public static void PopWebsiteMessageItem(PageInfo pageInfo)
        {
            if (pageInfo.WebsiteMessageItems.Count > 0)
            {
                pageInfo.WebsiteMessageItems.Pop();
            }
        }

        public static void PopSqlItem(PageInfo pageInfo)
        {
            if (pageInfo.SqlItems.Count > 0)
            {
                pageInfo.SqlItems.Pop();
            }
        }

        public static void PopPhotoItem(PageInfo pageInfo)
        {
            if (pageInfo.PhotoItems.Count > 0)
            {
                pageInfo.PhotoItems.Pop();
            }
        }

        public static void PopTeleplayItem(PageInfo pageInfo)
        {
            if (pageInfo.TeleplayItems.Count > 0)
            {
                pageInfo.TeleplayItems.Pop();
            }
        }

        public static void PopEachItem(PageInfo pageInfo)
        {
            if (pageInfo.EachItems.Count > 0)
            {
                pageInfo.EachItems.Pop();
            }
        }

        public static void PopSpecItem(PageInfo pageInfo)
        {
            if (pageInfo.SpecItems.Count > 0)
            {
                pageInfo.SpecItems.Pop();
            }
        }

        public static void PopFilterItem(PageInfo pageInfo)
        {
            if (pageInfo.FilterItems.Count > 0)
            {
                pageInfo.FilterItems.Pop();
            }
        }

        public static DbItemContainer GetItemContainer(PageInfo pageInfo)
        {
            DbItemContainer dbItemContainer = new DbItemContainer();
            if (pageInfo.ChannelItems.Count > 0)
            {
                dbItemContainer.channelItem = (DbItemInfo)pageInfo.ChannelItems.Peek();
            }
            if (pageInfo.ContentItems.Count > 0)
            {
                dbItemContainer.contentItem = (DbItemInfo)pageInfo.ContentItems.Peek();
            }
            if (pageInfo.CommentItems.Count > 0)
            {
                dbItemContainer.commentItem = (DbItemInfo)pageInfo.CommentItems.Peek();
            }
            if (pageInfo.InputItems.Count > 0)
            {
                dbItemContainer.inputItem = (DbItemInfo)pageInfo.InputItems.Peek();
            }
            if (pageInfo.WebsiteMessageItems.Count > 0)
            {
                dbItemContainer.websiteMessageItem = (DbItemInfo)pageInfo.WebsiteMessageItems.Peek();
            }
            if (pageInfo.SqlItems.Count > 0)
            {
                dbItemContainer.sqlItem = (DbItemInfo)pageInfo.SqlItems.Peek();
            }
            if (pageInfo.SiteItems.Count > 0)
            {
                dbItemContainer.siteItem = (DbItemInfo)pageInfo.SiteItems.Peek();
            }
            if (pageInfo.PhotoItems.Count > 0)
            {
                dbItemContainer.photoItem = (DbItemInfo)pageInfo.PhotoItems.Peek();
            }
            if (pageInfo.TeleplayItems.Count > 0)
            {
                dbItemContainer.teleplayItem = (DbItemInfo)pageInfo.TeleplayItems.Peek();
            }
            if (pageInfo.EachItems.Count > 0)
            {
                dbItemContainer.eachItem = (DbItemInfo)pageInfo.EachItems.Peek();
            }
            if (pageInfo.SpecItems.Count > 0)
            {
                dbItemContainer.specItem = (DbItemInfo)pageInfo.SpecItems.Peek();
            }
            if (pageInfo.FilterItems.Count > 0)
            {
                dbItemContainer.filterItem = (DbItemInfo)pageInfo.FilterItems.Peek();
            }
            return dbItemContainer;
        }

        public DbItemInfo ChannelItem
        {
            get { return channelItem; }
        }

        public DbItemInfo ContentItem
        {
            get { return contentItem; }
        }

        public DbItemInfo CommentItem
        {
            get { return commentItem; }
        }

        public DbItemInfo InputItem
        {
            get { return inputItem; }
        }

        public DbItemInfo WebsiteMessageItem
        {
            get { return websiteMessageItem; }
        }

        public DbItemInfo SqlItem
        {
            get { return sqlItem; }
        }

        public DbItemInfo SiteItem
        {
            get { return siteItem; }
        }

        public DbItemInfo PhotoItem
        {
            get { return photoItem; }
        }

        public DbItemInfo TeleplayItem
        {
            get { return teleplayItem; }
        }

        public DbItemInfo EachItem
        {
            get { return eachItem; }
        }

        public DbItemInfo SpecItem
        {
            get { return specItem; }
        }

        public DbItemInfo FilterItem
        {
            get { return filterItem; }
        }
    }
}
