using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class SubscribeSetAttribute
    {
        protected SubscribeSetAttribute()
        {
        }

        public const string SubscribeSetID = "SubscribeSetID";
        public const string EmailContentAddress = "EmailContentAddress";
        public const string MobileContentAddress = "MobileContentAddress";
        public const string PushType = "PushType";
        public const string PushDate = "PushDate";
        public const string AddDate = "AddDate";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string UserName = "UserName";

        #region 模板
        public const string IsTemplate = "IsTemplate";
        public const string StyleTemplate = "StyleTemplate";
        public const string ScriptTemplate = "ScriptTemplate";
        public const string ContentTemplate = "ContentTemplate";
        #endregion

        private static ArrayList allAttributes;
        public static ArrayList AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new ArrayList();
                    allAttributes.Add(SubscribeSetID.ToLower());
                    allAttributes.Add(EmailContentAddress.ToLower());
                    allAttributes.Add(MobileContentAddress.ToLower());
                    allAttributes.Add(PushType.ToLower());
                    allAttributes.Add(PushDate.ToLower());
                    allAttributes.Add(AddDate.ToLower());
                    allAttributes.Add(PublishmentSystemID.ToLower());
                    allAttributes.Add(UserName.ToLower());
                    #region 模板
                    allAttributes.Add(IsTemplate.ToLower());
                    allAttributes.Add(StyleTemplate.ToLower());
                    allAttributes.Add(ScriptTemplate.ToLower());
                    allAttributes.Add(ContentTemplate.ToLower());
                    #endregion
                }
                return allAttributes;
            }
        }

    }

    public class SubscribeSetInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_SubscribeSet";

        public SubscribeSetInfo()
        {
            this.SubscribeSetID = 0;
            this.EmailContentAddress = string.Empty;
            this.MobileContentAddress = string.Empty;
            this.PushDate = string.Empty;
            this.PushType = ESubscribePushDateType.Month.ToString();
            this.AddDate = DateTime.Now;
            this.PublishmentSystemID = 0;
            this.UserName = string.Empty;
            #region 模板
            this.IsTemplate = false;
            this.ContentTemplate = string.Empty;
            this.ScriptTemplate = string.Empty;
            this.StyleTemplate = string.Empty;
            #endregion
        }

        public SubscribeSetInfo(object dataItem)
            : base(dataItem)
        {
        }

        public SubscribeSetInfo(int id, string emailContentAddress, string mobileContentAddress, string pushDate, ESubscribePushDateType pushType, DateTime addDate, int publishmentSystemID, string userName)
        {
            this.SubscribeSetID = id;
            this.EmailContentAddress = emailContentAddress;
            this.MobileContentAddress = mobileContentAddress;
            this.PushDate = pushDate;
            this.PushType = pushType.ToString();
            this.AddDate = addDate;
            this.PublishmentSystemID = publishmentSystemID;
            this.UserName = userName;
        }

        public SubscribeSetInfo(int id, string emailContentAddress, string mobileContentAddress, string pushDate, ESubscribePushDateType pushType, DateTime addDate, int publishmentSystemID, string userName, bool isTemplate, string contentTemplate, string scriptTemplate, string styleTemplate)
        {
            this.SubscribeSetID = id;
            this.EmailContentAddress = emailContentAddress;
            this.MobileContentAddress = mobileContentAddress;
            this.PushDate = pushDate;
            this.PushType = pushType.ToString();
            this.AddDate = addDate;
            this.PublishmentSystemID = publishmentSystemID;
            this.UserName = userName;
            #region 模板
            this.IsTemplate = isTemplate;
            this.ContentTemplate = contentTemplate;
            this.ScriptTemplate = scriptTemplate;
            this.StyleTemplate = styleTemplate;
            #endregion
        }
        public int SubscribeSetID
        {
            get { return base.GetInt(SubscribeSetAttribute.SubscribeSetID, 0); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.SubscribeSetID, value.ToString()); }
        }

        public string EmailContentAddress
        {
            get { return base.GetExtendedAttribute(SubscribeSetAttribute.EmailContentAddress); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.EmailContentAddress, value); }
        }

        public string MobileContentAddress
        {
            get { return base.GetExtendedAttribute(SubscribeSetAttribute.MobileContentAddress); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.MobileContentAddress, value); }
        }

        public string PushType
        {
            get { return base.GetExtendedAttribute(SubscribeSetAttribute.PushType); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.PushType, value); }
        }

        public string PushDate
        {
            get { return base.GetExtendedAttribute(SubscribeSetAttribute.PushDate); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.PushDate, value); }
        }


        public DateTime AddDate
        {
            get { return base.GetDateTime(SubscribeSetAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.AddDate, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(SubscribeSetAttribute.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.PublishmentSystemID, value.ToString()); }
        }

        public string UserName
        {
            get { return base.GetExtendedAttribute(SubscribeSetAttribute.UserName); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.UserName, value); }
        }

        #region 模板
        public bool IsTemplate
        {
            get { return base.GetBool(SubscribeSetAttribute.IsTemplate, false); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.IsTemplate, value.ToString()); }
        }
        public string StyleTemplate
        {
            get { return base.GetString(SubscribeSetAttribute.StyleTemplate, string.Empty); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.StyleTemplate, value); }
        }
        public string ScriptTemplate
        {
            get { return base.GetString(SubscribeSetAttribute.ScriptTemplate, string.Empty); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.ScriptTemplate, value); }
        }
        public string ContentTemplate
        {
            get { return base.GetString(SubscribeSetAttribute.ContentTemplate, string.Empty); }
            set { base.SetExtendedAttribute(SubscribeSetAttribute.ContentTemplate, value); }
        }
        #endregion

        protected override ArrayList GetDefaultAttributesNames()
        {
            return SubscribeSetAttribute.AllAttributes;
        }
    }
}
