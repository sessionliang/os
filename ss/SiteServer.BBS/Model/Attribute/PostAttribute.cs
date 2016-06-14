using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SiteServer.BBS.Model
{
    public class PostAttribute
    {
        private PostAttribute()
        {
        }

        public static string ID = "ID";
        public static string PublishmentSystemID = "PublishmentSystemID";
        public static string ForumID = "ForumID";
        public static string ThreadID = "ThreadID";
        public static string UserName = "UserName";
        public static string Title = "Title";
        public static string Content = "Content";
        public static string Taxis = "Taxis";
        public static string IsChecked = "IsChecked";
        public static string AddDate = "AddDate";
        public static string IPAddress = "IPAddress";
        public static string IsThread = "IsThread";
        public static string IsBanned = "IsBanned";
        public static string IsAnonymous = "IsAnonymous";
        public static string IsHtml = "IsHtml";
        public static string IsBBCodeOff = "IsBBCodeOff";
        public static string IsSmileyOff = "IsSmileyOff";
        public static string IsUrlOff = "IsUrlOff";
        public static string IsSignature = "IsSignature";
        public static string IsAttachment = "IsAttachment";
        public static string IsHandled = "IsHandled";
        public static string State = "State";
        public static string LastEditUserName = "LastEditUserName";
        public static string LastEditDate = "LastEditDate";
        //不存在
        public static string PostID = "PostID";
        public static string Floor = "Floor";
        public static string Poll = "Poll";
        public static string Attachment = "Attachment";
    }
}
