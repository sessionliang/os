using System;
using System.Text;
using BaiRong.Model;

namespace SiteServer.BBS.Model {

    public class ThreadInfo : ExtendedAttributes
    {
        public const string TableName = "bbs_Thread";

        public ThreadInfo(int publishmentSystemID)
        {
            this.PublishmentSystemID = publishmentSystemID;
        }

        public ThreadInfo(object dataItem)
            : base(dataItem)
		{
		}

        public ThreadInfo(ThreadInfo info)
        {
            this.SetExtendedAttribute(info.Attributes);
        }

        public int ID
        {
            get { return base.GetInt(ThreadAttribute.ID, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.ID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(ThreadAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.PublishmentSystemID, value.ToString()); }
        }

        public int AreaID
        {
            get { return base.GetInt(ThreadAttribute.AreaID, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.AreaID, value.ToString()); }
        }

        public int ForumID
        {
            get { return base.GetInt(ThreadAttribute.ForumID, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.ForumID, value.ToString()); }
        }

        public int IconID
        {
            get { return base.GetInt(ThreadAttribute.IconID, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.IconID, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(ThreadAttribute.UserName); }
            set { base.SetExtendedAttribute(ThreadAttribute.UserName, value); }
        }

        public string Title
        {
            get { return base.GetExtendedAttribute(ThreadAttribute.Title); }
            set { base.SetExtendedAttribute(ThreadAttribute.Title, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(ThreadAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ThreadAttribute.AddDate, value.ToString()); }
        }

        public DateTime LastDate
        {
            get { return base.GetDateTime(ThreadAttribute.LastDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ThreadAttribute.LastDate, value.ToString()); }
        }

        public int LastPostID
        {
            get { return base.GetInt(ThreadAttribute.LastPostID, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.LastPostID, value.ToString()); }
        }

        public string LastUserName
        {
            get { return base.GetExtendedAttribute(ThreadAttribute.LastUserName); }
            set { base.SetExtendedAttribute(ThreadAttribute.LastUserName, value); }
        }

        public int Hits
        {
            get { return base.GetInt(ThreadAttribute.Hits, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.Hits, value.ToString()); }
        }

        public int Replies
        {
            get { return base.GetInt(ThreadAttribute.Replies, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.Replies, value.ToString()); }
        }

        public int Taxis
        {
            get { return base.GetInt(ThreadAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.Taxis, value.ToString()); }
        }

        public bool IsChecked
        {
            get { return base.GetBool(ThreadAttribute.IsChecked, false); }
            set { base.SetExtendedAttribute(ThreadAttribute.IsChecked, value.ToString()); }
        }

        public bool IsLocked
        {
            get { return base.GetBool(ThreadAttribute.IsLocked, false); }
            set { base.SetExtendedAttribute(ThreadAttribute.IsLocked, value.ToString()); }
        }

        public bool IsImage
        {
            get { return base.GetBool(ThreadAttribute.IsImage, false); }
            set { base.SetExtendedAttribute(ThreadAttribute.IsImage, value.ToString()); }
        }

        public bool IsAttachment
        {
            get { return base.GetBool(ThreadAttribute.IsAttachment, false); }
            set { base.SetExtendedAttribute(ThreadAttribute.IsAttachment, value.ToString()); }
        }

        public int CategoryID
        {
            get { return base.GetInt(ThreadAttribute.CategoryID, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.CategoryID, value.ToString()); }
        }

        public int TopLevel
        {
            get { return base.GetInt(ThreadAttribute.TopLevel, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.TopLevel, value.ToString()); }
        }

        public DateTime TopLevelDate
        {
            get { return base.GetDateTime(ThreadAttribute.TopLevelDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ThreadAttribute.TopLevelDate, value.ToString()); }
        }

        public int DigestLevel
        {
            get { return base.GetInt(ThreadAttribute.DigestLevel, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.DigestLevel, value.ToString()); }
        }

        public DateTime DigestDate
        {
            get { return base.GetDateTime(ThreadAttribute.DigestDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ThreadAttribute.DigestDate, value.ToString()); }
        }

        public string Highlight
        {
            get { return base.GetExtendedAttribute(ThreadAttribute.Highlight); }
            set { base.SetExtendedAttribute(ThreadAttribute.Highlight, value); }
        }

        public DateTime HighlightDate
        {
            get { return base.GetDateTime(ThreadAttribute.HighlightDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ThreadAttribute.HighlightDate, value.ToString()); }
        }

        public int IdentifyID
        {
            get { return base.GetInt(ThreadAttribute.IdentifyID, 0); }
            set { base.SetExtendedAttribute(ThreadAttribute.IdentifyID, value.ToString()); }
        }

        public EThreadType ThreadType
        {
            get { return EThreadTypeUtils.GetEnumType(base.GetExtendedAttribute(ThreadAttribute.ThreadType)); }
            set { base.SetExtendedAttribute(ThreadAttribute.ThreadType, EThreadTypeUtils.GetValue(value)); }
        }
    }
}
