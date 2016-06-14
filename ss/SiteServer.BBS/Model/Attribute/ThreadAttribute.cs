using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SiteServer.BBS.Model
{
    public class ThreadAttribute
    {
        private ThreadAttribute()
        {
        }

        public static string ID = "ID";
        public static string PublishmentSystemID = "PublishmentSystemID";
        public static string AreaID = "AreaID";
        public static string ForumID = "ForumID";
        public static string IconID = "IconID";
        public static string UserName = "UserName";
        public static string Title = "Title";
        public static string AddDate = "AddDate";
        public static string LastDate = "LastDate";
        public static string LastPostID = "LastPostID";
        public static string LastUserName = "LastUserName";
        public static string Hits = "Hits";
        public static string Replies = "Replies";
        public static string Taxis = "Taxis";
        public static string IsChecked = "IsChecked";
        public static string IsLocked = "IsLocked";
        public static string IsImage = "IsImage";
        public static string IsAttachment = "IsAttachment";
        public static string CategoryID = "CategoryID";
        public static string TopLevel = "TopLevel";
        public static string TopLevelDate = "TopLevelDate";
        public static string DigestLevel = "DigestLevel";
        public static string DigestDate = "DigestDate";
        public static string Highlight = "Highlight";
        public static string HighlightDate = "HighlightDate";
        public static string IdentifyID = "IdentifyID";
        public static string ThreadType = "ThreadType";

        //不存在
        public static string ThreadID = "ThreadID";
        public static string NavigationUrl = "NavigationUrl";
        public static string Content = "Content";
    }
}
