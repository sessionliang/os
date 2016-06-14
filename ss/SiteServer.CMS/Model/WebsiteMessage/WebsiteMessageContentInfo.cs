using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class WebsiteMessageContentInfo : ExtendedAttributes
    {
        public WebsiteMessageContentInfo()
        {
            this.ID = 0;
            this.WebsiteMessageID = 0;
            this.Taxis = 0;
            this.IsChecked = false;
            this.UserName = string.Empty;
            this.IPAddress = string.Empty;
            this.Location = string.Empty;
            this.AddDate = DateTime.Now;
            this.Reply = string.Empty;
        }

        public WebsiteMessageContentInfo(int id, int websiteMessageID, int taxis, bool isChecked, string userName, string ipAddress, string location, DateTime addDate, string reply)
        {
            this.ID = id;
            this.WebsiteMessageID = websiteMessageID;
            this.Taxis = taxis;
            this.IsChecked = isChecked;
            this.UserName = userName;
            this.IPAddress = ipAddress;
            this.Location = location;
            this.AddDate = addDate;
            this.Reply = reply;
        }

        public int ID
        {
            get { return base.GetInt(WebsiteMessageContentAttribute.ID, 0); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.ID, value.ToString()); }
        }

        public int WebsiteMessageID
        {
            get { return base.GetInt(WebsiteMessageContentAttribute.WebsiteMessageID, 0); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.WebsiteMessageID, value.ToString()); }
        }

        public int Taxis
        {
            get { return base.GetInt(WebsiteMessageContentAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Taxis, value.ToString()); }
        }

        public bool IsChecked
        {
            get { return base.GetBool(WebsiteMessageContentAttribute.IsChecked, false); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.IsChecked, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(WebsiteMessageContentAttribute.UserName); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.UserName, value); }
        }

        public string IPAddress
        {
            get { return base.GetExtendedAttribute(WebsiteMessageContentAttribute.IPAddress); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.IPAddress, value); }
        }

        public string Location
        {
            get { return base.GetExtendedAttribute(WebsiteMessageContentAttribute.Location); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Location, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(WebsiteMessageContentAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.AddDate, value.ToString()); }
        }

        public string Reply
        {
            get { return base.GetExtendedAttribute(WebsiteMessageContentAttribute.Reply); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Reply, value); }
        }

        public int ClassifyID
        {
            get { return base.GetInt(WebsiteMessageContentAttribute.ClassifyID, 0); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.ClassifyID, value.ToString()); }
        }
        #region ÁôÑÔ×Ö¶Î

        public string Name
        {
            get { return base.GetString(WebsiteMessageContentAttribute.Name, string.Empty); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Name, value); }
        }
        public string Phone
        {
            get { return base.GetString(WebsiteMessageContentAttribute.Phone, string.Empty); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Phone, value); }
        }
        public string Email
        {
            get { return base.GetString(WebsiteMessageContentAttribute.Email, string.Empty); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Email, value); }
        }
        public string Question
        {
            get { return base.GetString(WebsiteMessageContentAttribute.Question, string.Empty); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Question, value); }
        }
        public string Supplement
        {
            get { return base.GetString(WebsiteMessageContentAttribute.Description, string.Empty); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Description, value); }
        }
        public string Ext1
        {
            get { return base.GetString(WebsiteMessageContentAttribute.Ext1, string.Empty); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Ext1, value); }
        }
        public string Ext2
        {
            get { return base.GetString(WebsiteMessageContentAttribute.Ext2, string.Empty); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Ext2, value); }
        }
        public string Ext3
        {
            get { return base.GetString(WebsiteMessageContentAttribute.Ext3, string.Empty); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Ext3, value); }
        }
        #endregion

        protected override ArrayList GetDefaultAttributesNames()
        {
            return WebsiteMessageContentAttribute.AllAttributes;
        }
    }
}
