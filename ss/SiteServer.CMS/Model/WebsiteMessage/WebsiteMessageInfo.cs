using System;
using System.Xml.Serialization;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
    public class WebsiteMessageInfo
    {
        private int websiteMessageID;
        private string websiteMessageName;
        private int publishmentSystemID;
        private DateTime addDate;
        private bool isChecked;
        private bool isReply;
        private int taxis;
        private bool isTemplate;
        private string styleTemplate;
        private string scriptTemplate;
        private string contentTemplate;
        private string settingsXML;

        #region List
        private bool isTemplateList;
        private string styleTemplateList;
        private string scriptTemplateList;
        private string contentTemplateList;
        #endregion

        #region Detail
        private bool isTemplateDetail;
        private string styleTemplateDetail;
        private string scriptTemplateDetail;
        private string contentTemplateDetail;
        #endregion

        public WebsiteMessageInfo()
        {
            this.websiteMessageID = 0;
            this.websiteMessageName = string.Empty;
            this.publishmentSystemID = 0;
            this.addDate = DateTime.Now;
            this.isChecked = true;
            this.isReply = false;
            this.taxis = 0;
            this.isTemplate = false;
            this.styleTemplate = string.Empty;
            this.scriptTemplate = string.Empty;
            this.contentTemplate = string.Empty;
            this.settingsXML = string.Empty;

            #region List
            this.isTemplateList = false;
            this.styleTemplateList = string.Empty;
            this.scriptTemplateList = string.Empty;
            this.contentTemplateList = string.Empty;
            #endregion

            #region Detail
            this.isTemplateDetail = false;
            this.styleTemplateDetail = string.Empty;
            this.scriptTemplateDetail = string.Empty;
            this.contentTemplateDetail = string.Empty;
            #endregion
        }

        public WebsiteMessageInfo(int websiteMessageID, string websiteMessageName, int publishmentSystemID, DateTime addDate, bool isChecked, bool isReply, int taxis, bool isTemplate, string styleTemplate, string scriptTemplate, string contentTemplate, bool isTemplateList, string styleTemplateList, string scriptTemplateList, string contentTemplateList, bool isTemplateDetail, string styleTemplateDetail, string scriptTemplateDetail, string contentTemplateDetail, string settingsXML)
        {
            this.websiteMessageID = websiteMessageID;
            this.websiteMessageName = websiteMessageName;
            this.publishmentSystemID = publishmentSystemID;
            this.addDate = addDate;
            this.isChecked = isChecked;
            this.isReply = isReply;
            this.taxis = taxis;
            this.isTemplate = isTemplate;
            this.styleTemplate = styleTemplate;
            this.scriptTemplate = scriptTemplate;
            this.contentTemplate = contentTemplate;
            this.settingsXML = settingsXML;

            #region List
            this.isTemplateList = isTemplateList;
            this.styleTemplateList = styleTemplateList;
            this.scriptTemplateList = scriptTemplateList;
            this.contentTemplateList = contentTemplateList;
            #endregion

            #region Detail
            this.isTemplateDetail = isTemplateDetail;
            this.styleTemplateDetail = styleTemplateDetail;
            this.scriptTemplateDetail = scriptTemplateDetail;
            this.contentTemplateDetail = contentTemplateDetail;
            #endregion
        }

        public int WebsiteMessageID
        {
            get { return websiteMessageID; }
            set { websiteMessageID = value; }
        }

        public string WebsiteMessageName
        {
            get { return websiteMessageName; }
            set { websiteMessageName = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public bool IsReply
        {
            get { return isReply; }
            set { isReply = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

        public bool IsTemplate
        {
            get { return isTemplate; }
            set { isTemplate = value; }
        }

        public string StyleTemplate
        {
            get { return styleTemplate; }
            set { styleTemplate = value; }
        }

        public string ScriptTemplate
        {
            get { return scriptTemplate; }
            set { scriptTemplate = value; }
        }

        public string ContentTemplate
        {
            get { return contentTemplate; }
            set { contentTemplate = value; }
        }

        #region List
        public bool IsTemplateList
        {
            get { return isTemplateList; }
            set { isTemplateList = value; }
        }

        public string StyleTemplateList
        {
            get { return styleTemplateList; }
            set { styleTemplateList = value; }
        }

        public string ScriptTemplateList
        {
            get { return scriptTemplateList; }
            set { scriptTemplateList = value; }
        }

        public string ContentTemplateList
        {
            get { return contentTemplateList; }
            set { contentTemplateList = value; }
        }

        #endregion

        #region Detail
        public bool IsTemplateDetail
        {
            get { return isTemplateDetail; }
            set { isTemplateDetail = value; }
        }

        public string StyleTemplateDetail
        {
            get { return styleTemplateDetail; }
            set { styleTemplateDetail = value; }
        }

        public string ScriptTemplateDetail
        {
            get { return scriptTemplateDetail; }
            set { scriptTemplateDetail = value; }
        }

        public string ContentTemplateDetail
        {
            get { return contentTemplateDetail; }
            set { contentTemplateDetail = value; }
        }

        #endregion

        public string SettingsXML
        {
            get { return settingsXML; }
            set { settingsXML = value; }
        }

        WebsiteMessageInfoExtend additional;
        public WebsiteMessageInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new WebsiteMessageInfoExtend(this.settingsXML);
                }
                return this.additional;
            }
        }
    }
}
