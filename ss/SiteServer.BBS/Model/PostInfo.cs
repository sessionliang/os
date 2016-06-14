using System;
using System.Text;
using BaiRong.Model;

namespace SiteServer.BBS.Model {

    public class PostInfo : ExtendedAttributes
    {
        public const string TableName = "bbs_Post";

        public PostInfo(int publishmentSystemID)
        {
            this.ID = 0;
            this.PublishmentSystemID = publishmentSystemID;
            this.ThreadID = 0;
            this.ForumID = 0;
            this.UserName = string.Empty;
            this.LastEditUserName = string.Empty;
            this.LastEditDate = DateTime.Now;
            this.Title = string.Empty;
            this.Content = string.Empty;
            this.IsChecked = true;
            this.AddDate = DateTime.Now;
            this.IPAddress = string.Empty;
            this.IsThread = false;
            this.IsBanned = false;
            this.IsAnonymous = false;
            this.IsHtml = false;
            this.IsBBCodeOff = false;
            this.IsSmileyOff = false;
            this.IsUrlOff = false;
            this.IsSignature = false;
            this.IsAttachment = false;
            this.IsHandled = false;
            this.State = string.Empty;
            
        }

        public PostInfo(object dataItem)
            : base(dataItem)
		{
		}

        public PostInfo(int id, int publishmentSystemID, int threadID, int forumID, string userName, string title, string content, bool isChecked, DateTime addDate, string ipAddress, bool isThread, bool isBanned, bool isAnonymous, bool isHtml, bool isBBCodeOff, bool isSmileyOff, bool isUrlOff, bool isSignature, bool isAttachment, bool isHandled, string state)
        {
            this.ID = id;
            this.PublishmentSystemID = publishmentSystemID;
            this.ThreadID = threadID;
            this.ForumID = forumID;
            this.UserName = userName;
            this.Title = title;
            this.Content = content;
            this.IsChecked = isChecked;
            this.AddDate = addDate;
            this.IPAddress = ipAddress;
            this.IsThread = isThread;
            this.IsBanned = isBanned;
            this.IsAnonymous = isAnonymous;
            this.IsHtml = isHtml;
            this.IsBBCodeOff = isBBCodeOff;
            this.IsSmileyOff = isSmileyOff;
            this.IsUrlOff = isUrlOff;
            this.IsSignature = isSignature;
            this.IsAttachment = isAttachment;
            this.IsHandled = IsHandled;
            this.State = state;
        }

        public int ID
        {
            get { return base.GetInt(PostAttribute.ID, 0); }
            set { base.SetExtendedAttribute(PostAttribute.ID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(PostAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(PostAttribute.PublishmentSystemID, value.ToString()); }
        }

        public int ThreadID
        {
            get { return base.GetInt(PostAttribute.ThreadID, 0); }
            set { base.SetExtendedAttribute(PostAttribute.ThreadID, value.ToString()); }
        }

        public int ForumID
        {
            get { return base.GetInt(PostAttribute.ForumID, 0); }
            set { base.SetExtendedAttribute(PostAttribute.ForumID, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(PostAttribute.UserName); }
            set { base.SetExtendedAttribute(PostAttribute.UserName, value); }
        }

        public string Title
        {
            get { return base.GetExtendedAttribute(PostAttribute.Title); }
            set { base.SetExtendedAttribute(PostAttribute.Title, value); }
        }

        public string Content
        {
            get { return base.GetExtendedAttribute(PostAttribute.Content); }
            set { base.SetExtendedAttribute(PostAttribute.Content, value); }
        }

        public int Taxis
        {
            get { return base.GetInt(PostAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(PostAttribute.Taxis, value.ToString()); }
        }

        public bool IsChecked
        {
            get { return base.GetBool(PostAttribute.IsChecked, true); }
            set { base.SetExtendedAttribute(PostAttribute.IsChecked, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(PostAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(PostAttribute.AddDate, value.ToString()); }
        }

        public string IPAddress
        {
            get { return base.GetExtendedAttribute(PostAttribute.IPAddress); }
            set { base.SetExtendedAttribute(PostAttribute.IPAddress, value); }
        }

        public bool IsThread
        {
            get { return base.GetBool(PostAttribute.IsThread, false); }
            set { base.SetExtendedAttribute(PostAttribute.IsThread, value.ToString()); }
        }

        public bool IsBanned
        {
            get { return base.GetBool(PostAttribute.IsBanned, false); }
            set { base.SetExtendedAttribute(PostAttribute.IsBanned, value.ToString()); }
        }

        public bool IsAnonymous
        {
            get { return base.GetBool(PostAttribute.IsAnonymous, false); }
            set { base.SetExtendedAttribute(PostAttribute.IsAnonymous, value.ToString()); }
        }

        public bool IsHtml
        {
            get { return base.GetBool(PostAttribute.IsHtml, false); }
            set { base.SetExtendedAttribute(PostAttribute.IsHtml, value.ToString()); }
        }

        public bool IsBBCodeOff
        {
            get { return base.GetBool(PostAttribute.IsBBCodeOff, false); }
            set { base.SetExtendedAttribute(PostAttribute.IsBBCodeOff, value.ToString()); }
        }

        public bool IsSmileyOff
        {
            get { return base.GetBool(PostAttribute.IsSmileyOff, false); }
            set { base.SetExtendedAttribute(PostAttribute.IsUrlOff, value.ToString()); }
        }

        public bool IsUrlOff
        {
            get { return base.GetBool(PostAttribute.IsUrlOff, false); }
            set { base.SetExtendedAttribute(PostAttribute.IsUrlOff, value.ToString()); }
        }

        public bool IsSignature
        {
            get { return base.GetBool(PostAttribute.IsSignature, false); }
            set { base.SetExtendedAttribute(PostAttribute.IsSignature, value.ToString()); }
        }

        public bool IsAttachment
        {
            get { return base.GetBool(PostAttribute.IsAttachment, false); }
            set { base.SetExtendedAttribute(PostAttribute.IsAttachment, value.ToString()); }
        }

        public bool IsHandled
        {
            get { return base.GetBool(PostAttribute.IsHandled, false); }
            set { base.SetExtendedAttribute(PostAttribute.IsHandled, value.ToString()); }
        }

        public string State
        {
            get { return base.GetExtendedAttribute(PostAttribute.State); }
            set { base.SetExtendedAttribute(PostAttribute.State, value); }
        }
        public string LastEditUserName
        {
            get { return base.GetExtendedAttribute(PostAttribute.LastEditUserName); }
            set { base.SetExtendedAttribute(PostAttribute.LastEditUserName, value); }
        }
        public DateTime LastEditDate
        {
           get { return base.GetDateTime(PostAttribute.LastEditDate, DateTime.Now); }
            set { base.SetExtendedAttribute(PostAttribute.LastEditDate, value.ToString());}
        }
    }
}
