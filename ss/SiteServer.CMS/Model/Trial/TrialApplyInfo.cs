using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class TrialApplyInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_TrialApply";

        public TrialApplyInfo()
        {
            this.TAID = 0;
            this.PublishmentSystemID = 0;
            this.NodeID = 0;
            this.ContentID = 0;
            this.Taxis = 0;
            this.ApplyStatus = string.Empty;
            this.IsChecked = false;
            this.CheckAdmin = string.Empty;
            this.CheckDate = DateTime.Now;
            this.UserName = string.Empty;
            this.IPAddress = string.Empty;
            this.Location = string.Empty;
            this.AddDate = DateTime.Now;
            this.IsReport = false;
            this.IsEmail = false;
            this.IsMobile = false;
        }

        public TrialApplyInfo(int id, int publishmentSystemID, int nodeID, int contentID, int taxis,string applyStatus, bool isChecked, string checkAdmin, DateTime checkDate, string userName, string ipAddress, string location, DateTime addDate, bool isReport, bool isEmail, bool isMobile)
        {
            this.TAID = id;
            this.PublishmentSystemID = publishmentSystemID;
            this.NodeID = nodeID;
            this.ContentID = contentID;
            this.Taxis = taxis;
            this.ApplyStatus = applyStatus;
            this.IsChecked = isChecked;
            this.CheckAdmin = checkAdmin;
            this.CheckDate = checkDate;
            this.UserName = userName;
            this.IPAddress = ipAddress;
            this.Location = location;
            this.AddDate = addDate;
            this.IsReport = isReport;
            this.IsEmail = isEmail;
            this.IsMobile = isMobile;
        }

        public int TAID
        {
            get { return base.GetInt(TrialApplyAttribute.TAID, 0); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.TAID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(TrialApplyAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.PublishmentSystemID, value.ToString()); }
        }
        public int NodeID
        {
            get { return base.GetInt(TrialApplyAttribute.NodeID, 0); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.NodeID, value.ToString()); }
        }
        public int ContentID
        {
            get { return base.GetInt(TrialApplyAttribute.ContentID, 0); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.ContentID, value.ToString()); }
        }

        public int Taxis
        {
            get { return base.GetInt(TrialApplyAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.Taxis, value.ToString()); }
        }

        /// <summary>
        /// …Í«Î–≈œ¢◊¥Ã¨
        /// </summary>
        public string ApplyStatus
        {
            get { return base.GetExtendedAttribute(TrialApplyAttribute.ApplyStatus); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.ApplyStatus, value); }
        }

        public bool IsChecked
        {
            get { return base.GetBool(TrialApplyAttribute.IsChecked, false); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.IsChecked, value.ToString()); }
        }

        public bool IsReport
        {
            get { return base.GetBool(TrialApplyAttribute.IsReport, false); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.IsReport, value.ToString()); }
        }

        public bool IsEmail
        {
            get { return base.GetBool(TrialApplyAttribute.IsEmail, false); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.IsEmail, value.ToString()); }
        }

        public bool IsMobile
        {
            get { return base.GetBool(TrialApplyAttribute.IsMobile, false); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.IsMobile, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(TrialApplyAttribute.UserName); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.UserName, value); }
        }

        public string IPAddress
        {
            get { return base.GetExtendedAttribute(TrialApplyAttribute.IPAddress); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.IPAddress, value); }
        }

        public string Location
        {
            get { return base.GetExtendedAttribute(WebsiteMessageContentAttribute.Location); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Location, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(TrialApplyAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.AddDate, value.ToString()); }
        }

        public string CheckAdmin
        {
            get { return base.GetExtendedAttribute(TrialApplyAttribute.CheckAdmin); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.CheckAdmin, value); }
        }
        public DateTime CheckDate
        {
            get { return base.GetDateTime(TrialApplyAttribute.CheckDate, DateTime.Now); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.CheckDate, value.ToString()); }
        }

        #region …Í«Î◊÷∂Œ

        public string Name
        {
            get { return base.GetString(TrialApplyAttribute.Name, string.Empty); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.Name, value); }
        }
        public string Phone
        {
            get { return base.GetString(TrialApplyAttribute.Phone, string.Empty); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.Phone, value); }
        }
        public string Ext1
        {
            get { return base.GetString(TrialApplyAttribute.Ext1, string.Empty); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.Ext1, value); }
        }
        public string Ext2
        {
            get { return base.GetString(TrialApplyAttribute.Ext2, string.Empty); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.Ext2, value); }
        }
        public string Ext3
        {
            get { return base.GetString(TrialApplyAttribute.Ext3, string.Empty); }
            set { base.SetExtendedAttribute(TrialApplyAttribute.Ext3, value); }
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
            return TrialApplyAttribute.AllAttributes;
        }
    }
}
