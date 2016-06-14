using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class TrialReportInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_TrialReport";

        public TrialReportInfo()
        {
            this.TRID = 0;
            this.PublishmentSystemID = 0;
            this.NodeID = 0;
            this.ContentID = 0;
            this.TAID = 0;
            this.Taxis = 0;
            this.ReportStatus = false;
            this.UserName = string.Empty;
            this.IPAddress = string.Empty;
            this.Location = string.Empty;
            this.AddDate = DateTime.Now;
            this.Reply = string.Empty;
            this.AdminName = string.Empty; 
        }

        public TrialReportInfo(int id, int publishmentSystemID, int nodeID, int contentID, int taID, int taxis, bool reportStatus, string userName, string adminName, string ipAddress, string location, DateTime addDate, string reply)
        {
            this.TRID = id;
            this.PublishmentSystemID = publishmentSystemID;
            this.NodeID = nodeID;
            this.ContentID = contentID;
            this.TAID = taID;
            this.Taxis = taxis;
            this.ReportStatus = reportStatus;
            this.UserName = userName;
            this.IPAddress = ipAddress;
            this.Location = location;
            this.AddDate = addDate;
            this.Reply = reply;
            this.AdminName = adminName; 
        }

        public int TRID
        {
            get { return base.GetInt(TrialReportAttribute.TRID, 0); }
            set { base.SetExtendedAttribute(TrialReportAttribute.TRID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(TrialReportAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(TrialReportAttribute.PublishmentSystemID, value.ToString()); }
        }
        public int NodeID 
        {
            get { return base.GetInt(TrialReportAttribute.NodeID, 0); }
            set { base.SetExtendedAttribute(TrialReportAttribute.NodeID, value.ToString()); }
        }
        public int ContentID
        {   
            get { return base.GetInt(TrialReportAttribute.ContentID, 0); }
            set { base.SetExtendedAttribute(TrialReportAttribute.ContentID, value.ToString()); }
        }

        public int Taxis
        {
            get { return base.GetInt(TrialReportAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Taxis, value.ToString()); }
        }

        public bool ReportStatus
        {
            get { return base.GetBool(TrialReportAttribute.ReportStatus, false); }
            set { base.SetExtendedAttribute(TrialReportAttribute.ReportStatus, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(TrialReportAttribute.UserName); }
            set { base.SetExtendedAttribute(TrialReportAttribute.UserName, value); }
        }
        public string AdminName
        {
            get { return base.GetExtendedAttribute(TrialReportAttribute.AdminName); }
            set { base.SetExtendedAttribute(TrialReportAttribute.AdminName, value); }
        }

        public string IPAddress
        {
            get { return base.GetExtendedAttribute(TrialReportAttribute.IPAddress); }
            set { base.SetExtendedAttribute(TrialReportAttribute.IPAddress, value); }
        }

        public string Location
        {
            get { return base.GetExtendedAttribute(WebsiteMessageContentAttribute.Location); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Location, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(TrialReportAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(TrialReportAttribute.AddDate, value.ToString()); }
        }

        public string Reply
        {
            get { return base.GetExtendedAttribute(TrialReportAttribute.Reply); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Reply, value); }
        }

        public int TAID
        {
            get { return base.GetInt(TrialReportAttribute.TAID, 0); }
            set { base.SetExtendedAttribute(TrialReportAttribute.TAID, value.ToString()); }
        }
        #region  ‘”√±®∏Ê◊÷∂Œ

        public string Description
        {
            get { return base.GetString(TrialReportAttribute.Description, string.Empty); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Description, value); }
        }
        public string CompositeScore
        {
            get { return base.GetString(TrialReportAttribute.CompositeScore, string.Empty); }
            set { base.SetExtendedAttribute(TrialReportAttribute.CompositeScore, value); }
        }  
        public string Ext1
        {
            get { return base.GetString(TrialReportAttribute.Ext1, string.Empty); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Ext1, value); }
        }
        public string Ext2
        {
            get { return base.GetString(TrialReportAttribute.Ext2, string.Empty); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Ext2, value); }
        }
        public string Ext3
        {
            get { return base.GetString(TrialReportAttribute.Ext3, string.Empty); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Ext3, value); }
        }
        public string Ext4
        {
            get { return base.GetString(TrialReportAttribute.Ext4, string.Empty); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Ext4, value); }
        }
        public string Ext5
        {
            get { return base.GetString(TrialReportAttribute.Ext5, string.Empty); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Ext5, value); }
        }
        public string Ext6
        {
            get { return base.GetString(TrialReportAttribute.Ext6, string.Empty); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Ext6, value); }
        }
        public string Ext7
        {
            get { return base.GetString(TrialReportAttribute.Ext7, string.Empty); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Ext7, value); }
        }
        public string Ext8
        {
            get { return base.GetString(TrialReportAttribute.Ext8, string.Empty); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Ext8, value); }
        }
        public string Ext9
        {
            get { return base.GetString(TrialReportAttribute.Ext9, string.Empty); }
            set { base.SetExtendedAttribute(TrialReportAttribute.Ext9, value); }
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
            return TrialReportAttribute.AllAttributes;
        }
    }
}
