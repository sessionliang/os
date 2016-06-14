using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class EvaluationContentInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_EvaluationContent";

        public EvaluationContentInfo()
        {
            this.ECID = 0;
            this.PublishmentSystemID = 0;
            this.NodeID = 0;
            this.ContentID = 0;
            this.Taxis = 0;
            this.IsChecked = false;
            this.UserName = string.Empty;
            this.IPAddress = string.Empty;
            this.Location = string.Empty;
            this.AddDate = DateTime.Now;
            this.Reply = string.Empty;
            this.ClassifyID = 0;
        }

        public EvaluationContentInfo(int id, int publishmentSystemID, int nodeID, int contentID, int taxis, bool isChecked, string userName, string ipAddress, string location, DateTime addDate, string reply)
        {
            this.ECID = id;
            this.PublishmentSystemID = publishmentSystemID;
            this.NodeID = nodeID;
            this.ContentID = contentID;
            this.Taxis = taxis;
            this.IsChecked = isChecked;
            this.UserName = userName;
            this.IPAddress = ipAddress;
            this.Location = location;
            this.AddDate = addDate;
            this.Reply = reply;
        }

        public int ECID
        {
            get { return base.GetInt(EvaluationContentAttribute.ECID, 0); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.ECID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(EvaluationContentAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.PublishmentSystemID, value.ToString()); }
        }
        public int NodeID 
        {
            get { return base.GetInt(EvaluationContentAttribute.NodeID, 0); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.NodeID, value.ToString()); }
        }
        public int ContentID
        {   
            get { return base.GetInt(EvaluationContentAttribute.ContentID, 0); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.ContentID, value.ToString()); }
        }

        public int Taxis
        {
            get { return base.GetInt(EvaluationContentAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Taxis, value.ToString()); }
        }

        public bool IsChecked
        {
            get { return base.GetBool(EvaluationContentAttribute.IsChecked, false); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.IsChecked, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(EvaluationContentAttribute.UserName); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.UserName, value); }
        }

        public string IPAddress
        {
            get { return base.GetExtendedAttribute(EvaluationContentAttribute.IPAddress); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.IPAddress, value); }
        }

        public string Location
        {
            get { return base.GetExtendedAttribute(WebsiteMessageContentAttribute.Location); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Location, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(EvaluationContentAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.AddDate, value.ToString()); }
        }

        public string Reply
        {
            get { return base.GetExtendedAttribute(EvaluationContentAttribute.Reply); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Reply, value); }
        }

        public int ClassifyID
        {
            get { return base.GetInt(EvaluationContentAttribute.ClassifyID, 0); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.ClassifyID, value.ToString()); }
        }
        #region ÆÀ¼Û×Ö¶Î

        public string Description
        {
            get { return base.GetString(EvaluationContentAttribute.Description, string.Empty); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Description, value); }
        }
        public string CompositeScore
        {
            get { return base.GetString(EvaluationContentAttribute.CompositeScore, string.Empty); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.CompositeScore, value); }
        }  
        public string Ext1
        {
            get { return base.GetString(EvaluationContentAttribute.Ext1, string.Empty); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Ext1, value); }
        }
        public string Ext2
        {
            get { return base.GetString(EvaluationContentAttribute.Ext2, string.Empty); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Ext2, value); }
        }
        public string Ext3
        {
            get { return base.GetString(EvaluationContentAttribute.Ext3, string.Empty); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Ext3, value); }
        }
        public string Ext4
        {
            get { return base.GetString(EvaluationContentAttribute.Ext4, string.Empty); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Ext4, value); }
        }
        public string Ext5
        {
            get { return base.GetString(EvaluationContentAttribute.Ext5, string.Empty); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Ext5, value); }
        }
        public string Ext6
        {
            get { return base.GetString(EvaluationContentAttribute.Ext6, string.Empty); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Ext6, value); }
        }
        public string Ext7
        {
            get { return base.GetString(EvaluationContentAttribute.Ext7, string.Empty); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Ext7, value); }
        }
        public string Ext8
        {
            get { return base.GetString(EvaluationContentAttribute.Ext8, string.Empty); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Ext8, value); }
        }
        public string Ext9
        {
            get { return base.GetString(EvaluationContentAttribute.Ext9, string.Empty); }
            set { base.SetExtendedAttribute(EvaluationContentAttribute.Ext9, value); }
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
            return EvaluationContentAttribute.AllAttributes;
        }
    }
}
