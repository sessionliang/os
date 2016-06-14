using System;
using BaiRong.Model;
using System.Collections.Specialized;
using System.Net;
using BaiRong.Core;

namespace BaiRong.Model
{
    public class SMSServerInfoExtend : ExtendedAttributes
    {
        public SMSServerInfoExtend()
        {
        }

        public SMSServerInfoExtend(string extendValues)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(extendValues);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public SMSServerInfoExtend(string sendUrl, string sendParams, NameValueCollection sendSettings, string sendedSmsSearchUrl, string sendedSmsSearchParams, NameValueCollection sendedSmsSearchSettings, string lastSmsSearchUrl, string lastSmsSearchParams, NameValueCollection lastSmsSearchSettings, string insertTemplateUrl, string insertTemplateParams, NameValueCollection insertTemplateSettings, string updateTemplateUrl, string updateTemplateParams, NameValueCollection updateTemplateSettings, string deleteTemplateUrl, string deleteTemplateParams, NameValueCollection deleteTemplateSettings, string searchTemplateUrl, string searchTemplateParams, NameValueCollection searchTemplateSettings)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            base.SetExtendedAttribute(nameValueCollection);

            this.SendUrl = sendUrl;
            this.SendParams = sendParams;
            this.SendSettings = sendSettings;

            this.SendedSmsSearchUrl = sendedSmsSearchUrl;
            this.SendedSmsSearchParams = sendedSmsSearchParams;
            this.SendedSmsSearchSettings = sendedSmsSearchSettings;

            this.LastSmsSearchUrl = lastSmsSearchUrl;
            this.LastSmsSearchParams = lastSmsSearchParams;
            this.LastSmsSearchSettings = lastSmsSearchSettings;

            this.InsertTemplateUrl = insertTemplateUrl;
            this.InsertTemplateParams = insertTemplateParams;
            this.InsertTemplateSettings = insertTemplateSettings;

            this.UpdateTemplateUrl = updateTemplateUrl;
            this.UpdateTemplateParams = updateTemplateParams;
            this.UpdateTemplateSettings = updateTemplateSettings;

            this.DeleteTemplateUrl = deleteTemplateUrl;
            this.DeleteTemplateParams = deleteTemplateParams;
            this.DeleteTemplateSettings = deleteTemplateSettings;

            this.SearchTemplateUrl = searchTemplateUrl;
            this.SearchTemplateParams = searchTemplateParams;
            this.SearchTemplateSettings = searchTemplateSettings;

        }

        public string SendUrl
        {
            get { return base.GetString("SendUrl", string.Empty); }
            set { base.SetExtendedAttribute("SendUrl", value.ToLower()); }
        }

        public string SendParams
        {
            get { return base.GetString("SendParams", string.Empty); }
            set { base.SetExtendedAttribute("SendParams", value); }
        }

        public NameValueCollection SendSettings
        {
            get { return TranslateUtils.ToNameValueCollection(base.GetString("SendSettings", string.Empty), '|'); }
            set { base.SetExtendedAttribute("SendSettings", TranslateUtils.NameValueCollectionToString(value, '|')); }
        }

        public string SendedSmsSearchUrl
        {
            get { return base.GetString("SendedSmsSearchUrl", string.Empty); }
            set { base.SetExtendedAttribute("SendedSmsSearchUrl", value.ToLower()); }
        }

        public string SendedSmsSearchParams
        {
            get { return base.GetString("SendedSmsSearchParams", string.Empty); }
            set { base.SetExtendedAttribute("SendedSmsSearchParams", value); }
        }

        public NameValueCollection SendedSmsSearchSettings
        {
            get { return TranslateUtils.ToNameValueCollection(base.GetString("SendedSmsSearchSettings", string.Empty), '|'); }
            set { base.SetExtendedAttribute("SendedSmsSearchSettings", TranslateUtils.NameValueCollectionToString(value, '|')); }
        }

        public string LastSmsSearchUrl
        {
            get { return base.GetString("LastSmsSearchUrl", string.Empty); }
            set { base.SetExtendedAttribute("LastSmsSearchUrl", value.ToLower()); }
        }

        public string LastSmsSearchParams
        {
            get { return base.GetString("LastSmsSearchParams", string.Empty); }
            set { base.SetExtendedAttribute("LastSmsSearchParams", value); }
        }

        public NameValueCollection LastSmsSearchSettings
        {
            get { return TranslateUtils.ToNameValueCollection(base.GetString("LastSmsSearchSettings", string.Empty), '|'); }
            set { base.SetExtendedAttribute("LastSmsSearchSettings", TranslateUtils.NameValueCollectionToString(value, '|')); }
        }

        public string InsertTemplateUrl
        {
            get { return base.GetString("InsertTemplateUrl", string.Empty); }
            set { base.SetExtendedAttribute("InsertTemplateUrl", value.ToLower()); }
        }

        public string InsertTemplateParams
        {
            get { return base.GetString("InsertTemplateParams", string.Empty); }
            set { base.SetExtendedAttribute("InsertTemplateParams", value); }
        }

        public NameValueCollection InsertTemplateSettings
        {
            get { return TranslateUtils.ToNameValueCollection(base.GetString("InsertTemplateSettings", string.Empty), '|'); }
            set { base.SetExtendedAttribute("InsertTemplateSettings", TranslateUtils.NameValueCollectionToString(value, '|')); }
        }

        public string UpdateTemplateUrl
        {
            get { return base.GetString("UpdateTemplateUrl", string.Empty); }
            set { base.SetExtendedAttribute("UpdateTemplateUrl", value.ToLower()); }
        }

        public string UpdateTemplateParams
        {
            get { return base.GetString("UpdateTemplateParams", string.Empty); }
            set { base.SetExtendedAttribute("UpdateTemplateParams", value); }
        }

        public NameValueCollection UpdateTemplateSettings
        {
            get { return TranslateUtils.ToNameValueCollection(base.GetString("UpdateTemplateSettings", string.Empty), '|'); }
            set { base.SetExtendedAttribute("UpdateTemplateSettings", TranslateUtils.NameValueCollectionToString(value, '|')); }
        }

        public string DeleteTemplateUrl
        {
            get { return base.GetString("DeleteTemplateUrl", string.Empty); }
            set { base.SetExtendedAttribute("DeleteTemplateUrl", value.ToLower()); }
        }

        public string DeleteTemplateParams
        {
            get { return base.GetString("DeleteTemplateParams", string.Empty); }
            set { base.SetExtendedAttribute("DeleteTemplateParams", value); }
        }

        public NameValueCollection DeleteTemplateSettings
        {
            get { return TranslateUtils.ToNameValueCollection(base.GetString("DeleteTemplateSettings", string.Empty), '|'); }
            set { base.SetExtendedAttribute("DeleteTemplateSettings", TranslateUtils.NameValueCollectionToString(value, '|')); }
        }

        public string SearchTemplateUrl
        {
            get { return base.GetString("SearchTemplateUrl", string.Empty); }
            set { base.SetExtendedAttribute("SearchTemplateUrl", value.ToLower()); }
        }

        public string SearchTemplateParams
        {
            get { return base.GetString("SearchTemplateParams", string.Empty); }
            set { base.SetExtendedAttribute("SearchTemplateParams", value); }
        }

        public NameValueCollection SearchTemplateSettings
        {
            get { return TranslateUtils.ToNameValueCollection(base.GetString("SearchTemplateSettings", string.Empty), '|'); }
            set { base.SetExtendedAttribute("SearchTemplateSettings", TranslateUtils.NameValueCollectionToString(value, '|')); }
        }
    }
}
