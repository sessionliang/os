using System;
using System.Text;
using BaiRong.Model;
using System.Collections;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Model
{
    public class CommentAttribute
    {
        protected CommentAttribute()
        {
        }

        public const string CommentID = "CommentID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string NodeID = "NodeID";
        public const string ContentID = "ContentID";
        public const string Good = "Good";
        public const string UserName = "UserName";
        public const string IPAddress = "IPAddress";
        public const string AddDate = "AddDate";
        public const string Taxis = "Taxis";
        public const string IsChecked = "IsChecked";
        public const string IsRecommend = "IsRecommend";
        public const string Content = "Content";
        public const string AdminName = "AdminName";
    }

    public class CommentInfo : ExtendedAttributes
    {
        public CommentInfo(object dataItem)
            : base(dataItem)
        {
        }

        public CommentInfo()
        {
            this.CommentID = 0;
            this.PublishmentSystemID = 0;
            this.NodeID = 0;
            this.ContentID = 0;
            this.Good = 0;
            this.UserName = string.Empty;
            this.IPAddress = string.Empty;
            this.AddDate = DateTime.Now;
            this.Taxis = 0;
            this.IsChecked = false;
            this.IsRecommend = false;
            this.Content = string.Empty;
            this.AdminName = string.Empty;
        }

        public CommentInfo(int commentID, int publishmentSystemID, int nodeID, int contentID, int good, string userName, string ipAddress, DateTime addDate, int taxis, bool isChecked, bool isRecommend, string content, string adminName)
        {
            this.CommentID = commentID;
            this.PublishmentSystemID = publishmentSystemID;
            this.NodeID = nodeID;
            this.ContentID = contentID;
            this.Good = good;
            this.UserName = userName;
            this.IPAddress = ipAddress;
            this.AddDate = addDate;
            this.Taxis = taxis;
            this.IsChecked = isChecked;
            this.IsRecommend = isRecommend;
            this.Content = content;
            this.AdminName = adminName;
        }

        public int CommentID
        {
            get { return base.GetInt(CommentAttribute.CommentID, 0); }
            set { base.SetExtendedAttribute(CommentAttribute.CommentID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(CommentAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(CommentAttribute.PublishmentSystemID, value.ToString()); }
        }

        public int NodeID
        {
            get { return base.GetInt(CommentAttribute.NodeID, 0); }
            set { base.SetExtendedAttribute(CommentAttribute.NodeID, value.ToString()); }
        }

        public int ContentID
        {
            get { return base.GetInt(CommentAttribute.ContentID, 0); }
            set { base.SetExtendedAttribute(CommentAttribute.ContentID, value.ToString()); }
        }

        public int Good
        {
            get { return base.GetInt(CommentAttribute.Good, 0); }
            set { base.SetExtendedAttribute(CommentAttribute.Good, value.ToString()); }
        }

        public string UserName
        {
            get { return this.GetExtendedAttribute(CommentAttribute.UserName); }
            set { base.SetExtendedAttribute(CommentAttribute.UserName, value); }
        }

        public string IPAddress
        {
            get { return this.GetExtendedAttribute(CommentAttribute.IPAddress); }
            set { base.SetExtendedAttribute(CommentAttribute.IPAddress, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(CommentAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(CommentAttribute.AddDate, value.ToString()); }
        }

        public int Taxis
        {
            get { return base.GetInt(CommentAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(CommentAttribute.Taxis, value.ToString()); }
        }

        public bool IsChecked
        {
            get { return base.GetBool(CommentAttribute.IsChecked, false); }
            set { base.SetExtendedAttribute(CommentAttribute.IsChecked, value.ToString()); }
        }

        public bool IsRecommend
        {
            get { return base.GetBool(CommentAttribute.IsRecommend, false); }
            set { base.SetExtendedAttribute(CommentAttribute.IsRecommend, value.ToString()); }
        }

        public string Content
        {
            get { return this.GetExtendedAttribute(CommentAttribute.Content); }
            set { base.SetExtendedAttribute(CommentAttribute.Content, value); }
        }

        public string AdminName
        {
            get { return this.GetExtendedAttribute(CommentAttribute.AdminName); }
            set { base.SetExtendedAttribute(CommentAttribute.AdminName, value); }
        }
    }
}
