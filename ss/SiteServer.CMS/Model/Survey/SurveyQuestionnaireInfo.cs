using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class SurveyQuestionnaireInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_SurveyQuestionnaire";

        public SurveyQuestionnaireInfo()
        {
            this.SQID = 0;
            this.PublishmentSystemID = 0;
            this.NodeID = 0;
            this.ContentID = 0;
            this.Taxis = 0;
            this.SurveyStatus = false;
            this.UserName = string.Empty;
            this.IPAddress = string.Empty;
            this.Location = string.Empty;
            this.AddDate = DateTime.Now;
            this.Reply = string.Empty;
            this.ReplyAdmin = string.Empty;
            this.AdminName = string.Empty;
            this.IsEmail = false;
            this.IsMobile = false;
        }

        public SurveyQuestionnaireInfo(int id, int publishmentSystemID, int nodeID, int contentID, int taxis, bool surveyStatus, string userName, string adminName, string ipAddress, string location, DateTime addDate, string reply, string replyAdmin, bool isEmail, bool isMobile)
        {
            this.SQID = id;
            this.PublishmentSystemID = publishmentSystemID;
            this.NodeID = nodeID;
            this.ContentID = contentID;
            this.Taxis = taxis;
            this.SurveyStatus = surveyStatus;
            this.UserName = userName;
            this.IPAddress = ipAddress;
            this.Location = location;
            this.AddDate = addDate;
            this.Reply = reply;
            this.AdminName = adminName;
            this.ReplyAdmin = replyAdmin;
            this.IsEmail = isEmail;
            this.IsMobile = isMobile;
        }

        public int SQID
        {
            get { return base.GetInt(SurveyQuestionnaireAttribute.SQID, 0); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.SQID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(SurveyQuestionnaireAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.PublishmentSystemID, value.ToString()); }
        }
        public int NodeID
        {
            get { return base.GetInt(SurveyQuestionnaireAttribute.NodeID, 0); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.NodeID, value.ToString()); }
        }
        public int ContentID
        {
            get { return base.GetInt(SurveyQuestionnaireAttribute.ContentID, 0); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.ContentID, value.ToString()); }
        }

        public int Taxis
        {
            get { return base.GetInt(SurveyQuestionnaireAttribute.Taxis, 0); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Taxis, value.ToString()); }
        }

        public bool SurveyStatus
        {
            get { return base.GetBool(SurveyQuestionnaireAttribute.SurveyStatus, false); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.SurveyStatus, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(SurveyQuestionnaireAttribute.UserName); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.UserName, value); }
        }
        public string AdminName
        {
            get { return base.GetExtendedAttribute(SurveyQuestionnaireAttribute.AdminName); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.AdminName, value); }
        }

        public string IPAddress
        {
            get { return base.GetExtendedAttribute(SurveyQuestionnaireAttribute.IPAddress); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.IPAddress, value); }
        }

        public string Location
        {
            get { return base.GetExtendedAttribute(WebsiteMessageContentAttribute.Location); }
            set { base.SetExtendedAttribute(WebsiteMessageContentAttribute.Location, value); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(SurveyQuestionnaireAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.AddDate, value.ToString()); }
        }

        public string Reply
        {
            get { return base.GetExtendedAttribute(SurveyQuestionnaireAttribute.Reply); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Reply, value); }
        }

        public string ReplyAdmin
        {
            get { return base.GetExtendedAttribute(SurveyQuestionnaireAttribute.ReplyAdmin); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.ReplyAdmin, value); }
        }

        public bool IsEmail
        {
            get { return base.GetBool(SurveyQuestionnaireAttribute.IsEmail, false); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.IsEmail, value.ToString()); }
        }

        public bool IsMobile
        {
            get { return base.GetBool(SurveyQuestionnaireAttribute.IsMobile, false); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.IsMobile, value.ToString()); }
        }
        #region µ÷²éÎÊ¾í×Ö¶Î

        public string Description
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Description, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Description, value); }
        }
        public string CompositeScore
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.CompositeScore, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.CompositeScore, value); }
        }
        public string Ext1
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext1, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext1, value); }
        }
        public string Ext2
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext2, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext2, value); }
        }
        public string Ext3
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext3, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext3, value); }
        }
        public string Ext4
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext4, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext4, value); }
        }
        public string Ext5
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext5, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext5, value); }
        }
        public string Ext6
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext6, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext6, value); }
        }
        public string Ext7
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext7, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext7, value); }
        }
        public string Ext8
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext8, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext8, value); }
        }
        public string Ext9
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext9, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext9, value); }
        }
        public string Ext10
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext10, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext10, value); }
        }
        public string Ext11
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext11, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext11, value); }
        }
        public string Ext12
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext12, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext12, value); }
        }
        public string Ext13
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext13, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext13, value); }
        }
        public string Ext14
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext14, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext14, value); }
        }
        public string Ext15
        {
            get { return base.GetString(SurveyQuestionnaireAttribute.Ext15, string.Empty); }
            set { base.SetExtendedAttribute(SurveyQuestionnaireAttribute.Ext15, value); }
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
            return SurveyQuestionnaireAttribute.AllAttributes;
        }
    }
}
