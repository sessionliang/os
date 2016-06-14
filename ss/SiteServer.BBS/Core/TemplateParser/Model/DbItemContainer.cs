namespace SiteServer.BBS.Core.TemplateParser.Model
{
    public class DbItemContainer
    {
        private DbItemInfo forumItem;
        private DbItemInfo threadItem;
        private DbItemInfo postItem;
        private DbItemInfo sqlItem;

        private DbItemContainer() { }

        public static void PopForumItem(PageInfo pageInfo)
        {
            if (pageInfo.ForumItems.Count > 0)
            {
                pageInfo.ForumItems.Pop();
            }
        }

        public static void PopThreadItem(PageInfo pageInfo)
        {
            if (pageInfo.ThreadItems.Count > 0)
            {
                pageInfo.ThreadItems.Pop();
            }
        }

        public static void PopPostItem(PageInfo pageInfo)
        {
            if (pageInfo.PostItems.Count > 0)
            {
                pageInfo.PostItems.Pop();
            }
        }

        public static void PopSqlItem(PageInfo pageInfo)
        {
            if (pageInfo.SqlItems.Count > 0)
            {
                pageInfo.SqlItems.Pop();
            }
        }

        public static DbItemContainer GetItemContainer(PageInfo pageInfo)
        {
            DbItemContainer dbItemContainer = new DbItemContainer();
            if (pageInfo.ForumItems.Count > 0)
            {
                dbItemContainer.forumItem = (DbItemInfo)pageInfo.ForumItems.Peek();
            }
            if (pageInfo.ThreadItems.Count > 0)
            {
                dbItemContainer.threadItem = (DbItemInfo)pageInfo.ThreadItems.Peek();
            }
            if (pageInfo.PostItems.Count > 0)
            {
                dbItemContainer.postItem = (DbItemInfo)pageInfo.PostItems.Peek();
            }
            if (pageInfo.SqlItems.Count > 0)
            {
                dbItemContainer.sqlItem = (DbItemInfo)pageInfo.SqlItems.Peek();
            }
            return dbItemContainer;
        }

        public DbItemInfo ForumItem
        {
            get { return forumItem; }
        }

        public DbItemInfo ThreadItem
        {
            get { return threadItem; }
        }

        public DbItemInfo PostItem
        {
            get { return postItem; }
        }

        public DbItemInfo SqlItem
        {
            get { return sqlItem; }
        }
    }
}
