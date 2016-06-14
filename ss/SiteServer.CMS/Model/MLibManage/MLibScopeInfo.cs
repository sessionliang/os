using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.Model
{
    public class MLibScopeAttribute
    {
        protected MLibScopeAttribute()
        {
        }

        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string NodeID = "NodeID";
        public const string ContentNum = "ContentNum";
        public const string IsChecked = "IsChecked";
        public const string Field = "Field";
        public const string AddDate = "AddDate";
        public const string UserName = "UserName";
        public const string SetXML = "SetXML";

        private static ArrayList allAttributes;
        public static ArrayList AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes.Add(PublishmentSystemID.ToLower());
                    allAttributes.Add(NodeID.ToLower());
                    allAttributes.Add(ContentNum.ToLower());
                    allAttributes.Add(IsChecked.ToLower());
                    allAttributes.Add(Field.ToLower());
                    allAttributes.Add(AddDate.ToLower());
                    allAttributes.Add(UserName.ToLower());
                    allAttributes.Add(SetXML.ToLower());
                }
                return allAttributes;
            }
        }
    }

    public class MLibScopeInfo : ExtendedAttributes
    {
        public const string TableName = "bairong_MLibScope";

        public MLibScopeInfo()
        {
            this.PublishmentSystemID = 0;
            this.NodeID = 0;
            this.ContentNum = 0;
            this.IsChecked = false;
            this.Field = string.Empty;
            this.UserName = string.Empty;
            this.SetXML = string.Empty;
        }

        public MLibScopeInfo(object dataItem)
            : base(dataItem)
        {
        }


        public MLibScopeInfo(int publishmentSystemID, int nodeID, int contentNum, bool isChecked, string field, DateTime addDate, string userName, string setXML)
        {
            this.PublishmentSystemID = publishmentSystemID;
            this.NodeID = nodeID;
            this.ContentNum = contentNum;
            this.IsChecked = isChecked; 
            this.ContentNum = contentNum; 
            this.Field =field;
            this.AddDate = addDate;
            this.UserName = userName;
            this.SetXML = setXML;
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(MLibScopeAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(MLibScopeAttribute.PublishmentSystemID, value.ToString()); }
        }

        public int NodeID
        {
            get { return base.GetInt(MLibScopeAttribute.NodeID, 0); }
            set { base.SetExtendedAttribute(MLibScopeAttribute.NodeID, value.ToString()); }
        }

        public int ContentNum
        {
            get { return base.GetInt(MLibScopeAttribute.ContentNum, 0); }
            set { base.SetExtendedAttribute(MLibScopeAttribute.ContentNum, value.ToString()); }
        }

        public bool IsChecked
        {
            get { return base.GetBool(MLibScopeAttribute.IsChecked, false); }
            set { base.SetExtendedAttribute(MLibScopeAttribute.IsChecked, value.ToString()); }
        }


        public string Field
        {
            get { return base.GetExtendedAttribute(MLibScopeAttribute.Field); }
            set { base.SetExtendedAttribute(MLibScopeAttribute.Field, value); }
        }
          
        public DateTime AddDate
        {
            get { return base.GetDateTime(MLibScopeAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(MLibScopeAttribute.AddDate, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(MLibScopeAttribute.UserName); }
            set { base.SetExtendedAttribute(MLibScopeAttribute.UserName, value); }
        }

        public string SetXML
        {
            get { return base.GetExtendedAttribute(MLibScopeAttribute.SetXML); }
            set { base.SetExtendedAttribute(MLibScopeAttribute.SetXML, value); }
        }
          

        protected override ArrayList GetDefaultAttributesNames()
        {
            return MLibScopeAttribute.AllAttributes;
        }
    }
}
