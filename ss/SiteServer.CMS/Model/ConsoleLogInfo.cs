using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{

    public class ConsoleLogInfoAttributes
    {

        protected ConsoleLogInfoAttributes()
        {
        }

        public const string CLID = "CLID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string NodeID = "NodeID";
        public const string ContentID = "ContentID";
        public const string TableName = "TableName";//操作内容表名
        public const string SourceID = "SourceID";//内容ID
        public const string TargetDesc = "TargetDesc";//操作描述
        public const string ActionType = "ActionType";//操作类型
        public const string AddDate = "AddDate";
        public const string UserName = "UserName";
        public const string RedirectUrl = "RedirectUrl";//操作文章路径

        private static ArrayList allAttributes;
        public static ArrayList AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new ArrayList(otherAttributes);
                    allAttributes.Add(CLID.ToLower());
                }
                return allAttributes;
            }
        }


        private static ArrayList otherAttributes;
        public static ArrayList OtherAttributes
        {
            get
            {
                if (otherAttributes == null)
                {
                    otherAttributes = new ArrayList();
                    otherAttributes.Add(PublishmentSystemID.ToLower());
                    otherAttributes.Add(NodeID.ToLower());
                    otherAttributes.Add(ContentID.ToLower());
                    otherAttributes.Add(TableName.ToLower());
                    otherAttributes.Add(SourceID.ToLower());
                    otherAttributes.Add(AddDate.ToLower());
                    otherAttributes.Add(UserName.ToLower());
                    otherAttributes.Add(TargetDesc.ToLower());
                    otherAttributes.Add(ActionType.ToLower());
                    otherAttributes.Add(RedirectUrl.ToLower());
                }
                return otherAttributes;
            }
        }

    }
    public class ConsoleLogInfo : ExtendedAttributes
    {
        public const string TableNameStr = "siteserver_ConsoleLog";

        public ConsoleLogInfo()
        {
            this.CLID = 0;
            this.PublishmentSystemID = 0;
            this.NodeID = 0;
            this.ContentID = 0;
            this.TableName = string.Empty;
            this.TargetDesc = string.Empty;
            this.ActionType = string.Empty;
            this.UserName = string.Empty;
            this.AddDate = DateTime.Now;
            this.SourceID = 0;
            this.RedirectUrl = string.Empty;
        }

        public ConsoleLogInfo(int id, int publishmentSystemID, int nodeID, int contentID, string tableName, int sourceID, string targetDesc, string actionType, string userName, DateTime addDate, string redirectUrl)
        {
            this.CLID = id;
            this.PublishmentSystemID = publishmentSystemID;
            this.NodeID = nodeID;
            this.ContentID = contentID;
            this.TableName = tableName;
            this.TargetDesc = targetDesc;
            this.ActionType = actionType;
            this.UserName = userName;
            this.AddDate = addDate;
            this.SourceID = sourceID;
            this.RedirectUrl = redirectUrl;
        }

        public int CLID
        {
            get { return base.GetInt(ConsoleLogInfoAttributes.CLID, 0); }
            set { base.SetExtendedAttribute(ConsoleLogInfoAttributes.CLID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(ConsoleLogInfoAttributes.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(ConsoleLogInfoAttributes.PublishmentSystemID, value.ToString()); }
        }
        public int NodeID
        {
            get { return base.GetInt(ConsoleLogInfoAttributes.NodeID, 0); }
            set { base.SetExtendedAttribute(ConsoleLogInfoAttributes.NodeID, value.ToString()); }
        }
        public int ContentID
        {
            get { return base.GetInt(ConsoleLogInfoAttributes.ContentID, 0); }
            set { base.SetExtendedAttribute(ConsoleLogInfoAttributes.ContentID, value.ToString()); }
        }


        public string UserName
        {
            get { return base.GetExtendedAttribute(ConsoleLogInfoAttributes.UserName); }
            set { base.SetExtendedAttribute(ConsoleLogInfoAttributes.UserName, value); }
        }


        public DateTime AddDate
        {
            get { return base.GetDateTime(ConsoleLogInfoAttributes.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(ConsoleLogInfoAttributes.AddDate, value.ToString()); }
        }

        public int SourceID
        {
            get { return base.GetInt(ConsoleLogInfoAttributes.SourceID, 0); }
            set { base.SetExtendedAttribute(ConsoleLogInfoAttributes.SourceID, value.ToString()); }
        }

        public string TableName
        {
            get { return base.GetString(ConsoleLogInfoAttributes.TableName, string.Empty); }
            set { base.SetExtendedAttribute(ConsoleLogInfoAttributes.TableName, value); }
        }
        public string TargetDesc
        {
            get { return base.GetString(ConsoleLogInfoAttributes.TargetDesc, string.Empty); }
            set { base.SetExtendedAttribute(ConsoleLogInfoAttributes.TargetDesc, value); }
        }
        public string ActionType
        {
            get { return base.GetString(ConsoleLogInfoAttributes.ActionType, string.Empty); }
            set { base.SetExtendedAttribute(ConsoleLogInfoAttributes.ActionType, value); }
        }
        public string RedirectUrl
        {
            get { return base.GetString(ConsoleLogInfoAttributes.RedirectUrl, string.Empty); }
            set { base.SetExtendedAttribute(ConsoleLogInfoAttributes.RedirectUrl, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return ConsoleLogInfoAttributes.AllAttributes;
        }
    }
}
