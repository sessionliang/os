using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class CompareContentInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_CompareContent";

        public CompareContentInfo()
        {
            this.CCID = 0;
            this.PublishmentSystemID = 0;
            this.NodeID = 0;
            this.ContentID = 0;
            this.Taxis = 0;
            this.CompareStatus = false;
            this.UserName = string.Empty;
            this.IPAddress = string.Empty;
            this.Location = string.Empty;
            this.AddDate = DateTime.Now;
            this.AdminName = string.Empty; 
        }

        public CompareContentInfo(int id, int publishmentSystemID, int nodeID, int contentID, int taxis, bool compareStatus, string adminName, string userName, string ipAddress, string location, DateTime addDate)
        {
            this.CCID = id;
            this.PublishmentSystemID = publishmentSystemID;
            this.NodeID = nodeID;
            this.ContentID = contentID;
            this.Taxis = taxis;
            this.CompareStatus = compareStatus;
            this.AdminName = adminName;
            this.UserName = userName;
            this.IPAddress = ipAddress;
            this.Location = location;
            this.AddDate = addDate;
        }

        public int CCID
        {
            get { return base.GetInt(CompareContentAttribute.CCID, 0); }
            set { base.SetExtendedAttribute(CompareContentAttribute.CCID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(CompareContentAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(CompareContentAttribute.PublishmentSystemID, value.ToString()); }
        }
        public int NodeID 
        {
            get { return base.GetInt(CompareContentAttribute.NodeID, 0); }
            set { base.SetExtendedAttribute(CompareContentAttribute.NodeID, value.ToString()); }
        }
        public int ContentID
        {   
            get { return base.GetInt(CompareContentAttribute.ContentID, 0); }
            set { base.SetExtendedAttribute(CompareContentAttribute.ContentID, value.ToString()); }
        }

        public int Taxis
        {
            get { return base.GetInt(CompareContentAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(CompareContentAttribute.Taxis, value.ToString()); }
        }

        public bool CompareStatus
        {
            get { return base.GetBool(CompareContentAttribute.CompareStatus, false); }
            set { base.SetExtendedAttribute(CompareContentAttribute.CompareStatus, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(CompareContentAttribute.UserName); }
            set { base.SetExtendedAttribute(CompareContentAttribute.UserName, value); }
        }

        public string IPAddress
        {
            get { return base.GetExtendedAttribute(CompareContentAttribute.IPAddress); }
            set { base.SetExtendedAttribute(CompareContentAttribute.IPAddress, value); }
        }

        public string Location
        {
            get { return base.GetExtendedAttribute(WebsiteMessageContentAttribute.Location); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Location, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(CompareContentAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(CompareContentAttribute.AddDate, value.ToString()); }
        }

        public string AdminName
        {
            get { return base.GetExtendedAttribute(CompareContentAttribute.AdminName); }
            set { base.SetExtendedAttribute(CompareContentAttribute.AdminName, value); }
        }
         
        #region 比较功能字段

        public string Description
        {
            get { return base.GetString(CompareContentAttribute.Description, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.Description, value); }
        }
        public string CompositeScore1
        {
            get { return base.GetString(CompareContentAttribute.CompositeScore1, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.CompositeScore1, value); }
        }
        public string CompositeScore2
        {
            get { return base.GetString(CompareContentAttribute.CompositeScore2, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.CompositeScore2, value); }
        }  
        public string Ext1
        {
            get { return base.GetString(CompareContentAttribute.Ext1, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.Ext1, value); }
        }
        public string Ext2
        {
            get { return base.GetString(CompareContentAttribute.Ext2, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.Ext2, value); }
        }
        public string Ext3
        {
            get { return base.GetString(CompareContentAttribute.Ext3, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.Ext3, value); }
        }
        public string Ext4
        {
            get { return base.GetString(CompareContentAttribute.Ext4, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.Ext4, value); }
        }
        public string Ext5
        {
            get { return base.GetString(CompareContentAttribute.Ext5, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.Ext5, value); }
        }
        public string Ext6
        {
            get { return base.GetString(CompareContentAttribute.Ext6, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.Ext6, value); }
        }
        public string Ext7
        {
            get { return base.GetString(CompareContentAttribute.Ext7, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.Ext7, value); }
        }
        public string Ext8
        {
            get { return base.GetString(CompareContentAttribute.Ext8, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.Ext8, value); }
        }
        public string Ext9
        {
            get { return base.GetString(CompareContentAttribute.Ext9, string.Empty); }
            set { base.SetExtendedAttribute(CompareContentAttribute.Ext9, value); }
        }
        #endregion


        public override void SetExtendedAttribute(string name, string value)
        {
            base.SetExtendedAttribute(name, value);
        }

        public override string GetExtendedAttribute(string name)
        {
            return base.GetExtendedAttribute(name);
        }
         
        protected override ArrayList GetDefaultAttributesNames()
        {
            return CompareContentAttribute.AllAttributes;
        }
    }
}
