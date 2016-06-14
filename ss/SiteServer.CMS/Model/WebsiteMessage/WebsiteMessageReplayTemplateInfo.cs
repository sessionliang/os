using System;
using System.Xml.Serialization;
using BaiRong.Model;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public class WebsiteMessageReplayTemplateAttribute
    {
        protected WebsiteMessageReplayTemplateAttribute()
        { }

        public const string ID = "ID";
        public const string TemplateTitle = "TemplateTitle";
        public const string TemplateContent = "TemplateContent";
        public const string IsEnabled = "IsEnabled";
        public const string ClassifyID = "ClassifyID";
        public const string AddDate = "AddDate";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arrayList = new ArrayList();
                arrayList.Add(ID.ToLower());
                arrayList.Add(TemplateTitle.ToLower());
                arrayList.Add(TemplateContent.ToLower());
                arrayList.Add(IsEnabled.ToLower());
                arrayList.Add(ClassifyID.ToLower());
                arrayList.Add(AddDate.ToLower());
                return arrayList;
            }
        }
    }

    public class WebsiteMessageReplayTemplateInfo : ExtendedAttributes
    {
        public const string TableName = "siteserver_WebsiteMessageReplayTemplate";

        public WebsiteMessageReplayTemplateInfo()
        {

        }

        public int ID
        {
            get { return base.GetInt(WebsiteMessageReplayTemplateAttribute.ID, 0); }
            set { base.SetExtendedAttribute(WebsiteMessageReplayTemplateAttribute.ID, value.ToString()); }
        }

        public string TemplateTitle
        {
            get { return base.GetString(WebsiteMessageReplayTemplateAttribute.TemplateTitle, string.Empty); }
            set { base.SetExtendedAttribute(WebsiteMessageReplayTemplateAttribute.TemplateTitle, value); }
        }

        public string TemplateContent
        {
            get { return base.GetString(WebsiteMessageReplayTemplateAttribute.TemplateContent, string.Empty); }
            set { base.SetExtendedAttribute(WebsiteMessageReplayTemplateAttribute.TemplateContent, value); }
        }

        public bool IsEnabled
        {
            get { return base.GetBool(WebsiteMessageReplayTemplateAttribute.IsEnabled, true); }
            set { base.SetExtendedAttribute(WebsiteMessageReplayTemplateAttribute.IsEnabled, value.ToString()); }
        }

        public int ClassifyID
        {
            get { return base.GetInt(WebsiteMessageReplayTemplateAttribute.ClassifyID, 0); }
            set { base.SetExtendedAttribute(WebsiteMessageReplayTemplateAttribute.ClassifyID, value.ToString()); }
        }

        public DateTime AddDate
        {
            get { return base.GetDateTime(WebsiteMessageReplayTemplateAttribute.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(WebsiteMessageReplayTemplateAttribute.ClassifyID, value.ToString()); }
        }
    }
}
