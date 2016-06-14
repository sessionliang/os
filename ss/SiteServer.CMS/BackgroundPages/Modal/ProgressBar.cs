using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.HtmlControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;
using System.Web.UI.WebControls;
using System.Collections;


namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class ProgressBar : BackgroundBasePage
    {
        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public Literal RegisterScripts;

        private int publishmentSystemID;

        public static string GetOpenWindowStringWithCreateContentsOneByOne(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("CreateContentsOneByOne", true.ToString());
            return JsUtils.Layer.GetOpenLayerStringWithCheckBoxValue("生成内容页", PageUtils.GetCMSUrl("modal_progressBar.aspx"), arguments, "ContentIDCollection", "请选择需要生成的内容！", 500, 360);
        }


        public static string GetOpenWindowStringWithCreateContentsByMlib(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("CreateContentsByMlib", true.ToString());
            return JsUtils.Layer.GetOpenLayerStringWithCheckBoxValue("生成内容页", PageUtils.GetCMSUrl("modal_progressBar.aspx"), arguments, "IDsCollection", "请选择需要生成的内容！", 500, 360);
        }

        public static string GetOpenWindowStringWithCreateByTemplate(int publishmentSystemID, int templateID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("CreateByTemplate", true.ToString());
            return JsUtils.Layer.GetOpenLayerString("生成页面", PageUtils.GetCMSUrl("modal_progressBar.aspx"), arguments, 500, 360);
        }

        public static string GetRedirectUrlStringWithCreateChannelsOneByOne(int publishmentSystemID, string channelIDCollection, string isIncludeChildren, string isCreateContents)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_progressBar.aspx?PublishmentSystemID={0}&CreateChannelsOneByOne=True&ChannelIDCollection={1}&IsIncludeChildren={2}&IsCreateContents={3}", publishmentSystemID, channelIDCollection, isIncludeChildren, isCreateContents));
        }

        public static string GetRedirectUrlStringWithCreateContentsOneByOne(int publishmentSystemID, int nodeID, string contentIDCollection)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_progressBar.aspx?PublishmentSystemID={0}&NodeID={1}&CreateContentsOneByOne=True&ContentIDCollection={2}", publishmentSystemID, nodeID, contentIDCollection));
        }

        public static string GetRedirectUrlStringWithCreateByIDsCollection(int publishmentSystemID, string idsCollection)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_progressBar.aspx?PublishmentSystemID={0}&CreateByIDsCollection=True&IDsCollection={1}", publishmentSystemID, idsCollection));
        }

        public static string GetRedirectUrlStringWithPublishChannelsOneByOne(int publishmentSystemID, string channelIDCollection, bool isCreate, bool isIncludeChildren, bool isIncludeContents)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_progressBar.aspx?PublishmentSystemID={0}&PublishChannelsOneByOne=True&ChannelIDCollection={1}&IsCreate={2}&IsIncludeChildren={3}&isIncludeContents={4}", publishmentSystemID, channelIDCollection, isCreate, isIncludeChildren, isIncludeContents));
        }

        public static string GetRedirectUrlStringWithPublishContentsOneByOne(int publishmentSystemID, int nodeID, string contentIDCollection, bool isCreate)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_progressBar.aspx?PublishmentSystemID={0}&NodeID={1}&PublishContentsOneByOne=True&ContentIDCollection={2}&IsCreate={3}", publishmentSystemID, nodeID, contentIDCollection, isCreate));
        }

        public static string GetRedirectUrlStringWithGather(int publishmentSystemID, string gatherRuleNameCollection)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_progressBar.aspx?PublishmentSystemID={0}&Gather=True&GatherRuleNameCollection={1}", publishmentSystemID, gatherRuleNameCollection));
        }

        public static string GetOpenWindowStringWithGather(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("Gather", "True");
            return JsUtils.Layer.GetOpenLayerStringWithCheckBoxValue("采集内容", PageUtils.GetCMSUrl("modal_progressBar.aspx"), arguments, "GatherRuleNameCollection", "请选择需要开始采集的采集规则名称!", 660, 360);
        }

        public static string GetRedirectUrlStringWithGatherDatabase(int publishmentSystemID, string gatherRuleNameCollection)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_progressBar.aspx?PublishmentSystemID={0}&GatherDatabase=True&GatherRuleNameCollection={1}", publishmentSystemID, gatherRuleNameCollection));
        }

        public static string GetOpenWindowStringWithGatherDatabase(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("GatherDatabase", "True");
            return JsUtils.Layer.GetOpenLayerStringWithCheckBoxValue("数据库采集", PageUtils.GetCMSUrl("modal_progressBar.aspx"), arguments, "GatherRuleNameCollection", "请选择需要开始采集的采集规则名称!", 660, 360);
        }

        public static string GetRedirectUrlStringWithGatherFile(int publishmentSystemID, string gatherRuleNameCollection)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_progressBar.aspx?PublishmentSystemID={0}&GatherFile=True&GatherRuleNameCollection={1}", publishmentSystemID, gatherRuleNameCollection));
        }

        public static string GetOpenWindowStringWithGatherFile(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("GatherFile", "True");
            return JsUtils.Layer.GetOpenLayerStringWithCheckBoxValue("文件采集", PageUtils.GetCMSUrl("modal_progressBar.aspx"), arguments, "GatherRuleNameCollection", "请选择需要开始采集的采集规则名称!", 660, 360);
        }

        public static string GetOpenWindowStringWithSiteTemplateDownload(string downloadUrl, string directoryName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("SiteTemplateDownload", "True");
            arguments.Add("DownloadUrl", downloadUrl);
            arguments.Add("DirectoryName", directoryName);
            return JsUtils.Layer.GetOpenLayerString("下载在线模板", PageUtils.GetCMSUrl("modal_progressBar.aspx"), arguments, 460, 360);
        }

        public static string GetRedirectUrlStringWithSiteTemplateDownload(string downloadUrl, string directoryName)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_progressBar.aspx?SiteTemplateDownload=True&DownloadUrl={0}&DirectoryName={1}", downloadUrl, directoryName));
        }

        public static string GetOpenWindowStringWithSiteTemplateZip(string directoryName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("SiteTemplateZip", "True");
            arguments.Add("DirectoryName", directoryName);
            return JsUtils.Layer.GetOpenLayerString("应用模板压缩", PageUtils.GetCMSUrl("modal_progressBar.aspx"), arguments, 460, 360);
        }

        public static string GetRedirectUrlStringWithIndependentTemplateDownload(string downloadUrl, string directoryName)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_progressBar.aspx?IndependentTemplateDownload=True&DownloadUrl={0}&DirectoryName={1}", downloadUrl, directoryName));
        }

        public static string GetOpenWindowStringWithIndependentTemplateZip(string directoryName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("IndependentTemplateZip", "True");
            arguments.Add("DirectoryName", directoryName);
            return JsUtils.Layer.GetOpenLayerString("独立模板压缩", PageUtils.GetCMSUrl("modal_progressBar.aspx"), arguments, 460, 360);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("PublishmentSystemID") != null)
            {
                this.publishmentSystemID = TranslateUtils.ToInt(base.GetQueryString("PublishmentSystemID"));
            }

            this.Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (base.GetQueryString("Gather") != null && base.GetQueryString("GatherRuleNameCollection") != null)
            {
                string userKeyPrefix = StringUtils.GUID();
                string pars = string.Format("publishmentSystemID={0}&gatherRuleNameCollection={1}&userKeyPrefix={2}", this.publishmentSystemID, base.GetQueryString("GatherRuleNameCollection"), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.CMSService.RegisterProgressTaskScript(JsManager.CMSService.GatherServiceName, "Gather", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("GatherDatabase") != null && base.GetQueryString("GatherRuleNameCollection") != null)
            {
                string userKeyPrefix = StringUtils.GUID();
                string pars = string.Format("publishmentSystemID={0}&gatherRuleNameCollection={1}&userKeyPrefix={2}", this.publishmentSystemID, base.GetQueryString("GatherRuleNameCollection"), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.CMSService.RegisterProgressTaskScript(JsManager.CMSService.GatherServiceName, "GatherDatabase", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("GatherFile") != null && base.GetQueryString("GatherRuleNameCollection") != null)
            {
                string userKeyPrefix = StringUtils.GUID();
                string pars = string.Format("publishmentSystemID={0}&gatherRuleNameCollection={1}&userKeyPrefix={2}", this.publishmentSystemID, base.GetQueryString("GatherRuleNameCollection"), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.CMSService.RegisterProgressTaskScript(JsManager.CMSService.GatherServiceName, "GatherFile", pars, userKeyPrefix);

            }
            //----------------------------------------------------------------------------------------//
            else if (base.GetQueryString("CreateChannelsOneByOne") != null && base.GetQueryString("ChannelIDCollection") != null)
            {
                string userKeyPrefix = StringUtils.GUID();

                DbCacheManager.Insert(userKeyPrefix + Constants.CACHE_CREATE_CHANNELS_NODE_ID_ARRAYLIST, base.GetQueryString("ChannelIDCollection"));
                string pars = string.Format("publishmentSystemID={0}&userKeyPrefix={1}&isIncludeChildren={2}&isCreateContents={3}", this.publishmentSystemID, userKeyPrefix, base.GetQueryString("IsIncludeChildren"), base.GetQueryString("IsCreateContents"));
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.CreateServiceName, "CreateChannelsOneByOne", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("CreateContentsOneByOne") != null && base.GetQueryString("NodeID") != null && base.GetQueryString("ContentIDCollection") != null)
            {
                string userKeyPrefix = StringUtils.GUID();

                DbCacheManager.Insert(userKeyPrefix + Constants.CACHE_CREATE_CONTENTS_CONTENT_ID_ARRAYLIST, base.GetQueryString("ContentIDCollection"));
                string pars = string.Format("publishmentSystemID={0}&nodeID={1}&userKeyPrefix={2}", base.PublishmentSystemID, TranslateUtils.ToInt(base.GetQueryString("NodeID")), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.CreateServiceName, "CreateContentsOneByOne", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("CreateContentsByMlib") != null && base.GetQueryString("IDsCollection") != null)
            {
                string userKeyPrefix = StringUtils.GUID();
                //投稿管理生成
                DbCacheManager.Insert(userKeyPrefix + Constants.CACHE_CREATE_IDS_COLLECTION, base.GetQueryString("IDsCollection"));
                string pars = string.Format("publishmentSystemID={0}&userKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.CreateServiceName, "CreateByIDsCollection", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("CreateByTemplate") != null && base.GetQueryString("templateID") != null)
            {
                string userKeyPrefix = StringUtils.GUID();

                string pars = string.Format("publishmentSystemID={0}&templateID={1}&userKeyPrefix={2}", base.PublishmentSystemID, base.GetQueryString("templateID"), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.CreateServiceName, "CreateByTemplate", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("CreateByIDsCollection") != null && base.GetQueryString("IDsCollection") != null)
            {
                string userKeyPrefix = StringUtils.GUID();

                DbCacheManager.Insert(userKeyPrefix + Constants.CACHE_CREATE_IDS_COLLECTION, base.GetQueryString("IDsCollection"));
                string pars = string.Format("publishmentSystemID={0}&userKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.CreateServiceName, "CreateByIDsCollection", pars, userKeyPrefix);
            }
            //---------------------------------------------------------------------------------------//
            else if (base.GetQueryString("PublishChannelsOneByOne") != null && base.GetQueryString("ChannelIDCollection") != null)
            {
                string userKeyPrefix = StringUtils.GUID();

                DbCacheManager.Insert(userKeyPrefix + Constants.CACHE_PUBLISH_CHANNELS_NODE_ID_ARRAYLIST, base.GetQueryString("ChannelIDCollection"));

                string pars = string.Format("publishmentSystemID={0}&isCreate={1}&isIncludeChildren={2}&isIncludeContents={3}&userKeyPrefix={4}", base.PublishmentSystemID, TranslateUtils.ToBool(base.GetQueryString("IsCreate")), TranslateUtils.ToBool(base.GetQueryString("IsIncludeChildren")), TranslateUtils.ToBool(base.GetQueryString("IsIncludeContents")), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.PublishServiceName, "PublishChannelsOneByOne", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("PublishContentsOneByOne") != null && base.GetQueryString("ContentIDCollection") != null)
            {
                string userKeyPrefix = StringUtils.GUID();

                DbCacheManager.Insert(userKeyPrefix + Constants.CACHE_PUBLISH_CONTENTS_CONTENT_ID_ARRAYLIST, base.GetQueryString("ContentIDCollection"));

                string pars = string.Format("publishmentSystemID={0}&nodeID={1}&isCreate={2}&userKeyPrefix={3}", base.PublishmentSystemID, base.GetIntQueryString("NodeID"), TranslateUtils.ToBool(base.GetQueryString("IsCreate")), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.PublishServiceName, "PublishContentsOneByOne", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("SiteTemplateDownload") != null)
            {
                string userKeyPrefix = StringUtils.GUID();

                string pars = string.Format("downloadUrl={0}&directoryName={1}&userKeyPrefix={2}", base.GetQueryString("DownloadUrl"), base.GetQueryString("DirectoryName"), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.CMSService.RegisterProgressTaskScript(JsManager.CMSService.OtherServiceName, "SiteTemplateDownload", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("SiteTemplateZip") != null)
            {
                string userKeyPrefix = StringUtils.GUID();

                string pars = string.Format("directoryName={0}&userKeyPrefix={1}", base.GetQueryString("DirectoryName"), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.CMSService.RegisterProgressTaskScript(JsManager.CMSService.OtherServiceName, "SiteTemplateZip", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("IndependentTemplateDownload") != null)
            {
                string userKeyPrefix = StringUtils.GUID();

                string pars = string.Format("downloadUrl={0}&directoryName={1}&userKeyPrefix={2}", base.GetQueryString("DownloadUrl"), base.GetQueryString("DirectoryName"), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.CMSService.RegisterProgressTaskScript(JsManager.CMSService.OtherServiceName, "IndependentTemplateDownload", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("IndependentTemplateZip") != null)
            {
                string userKeyPrefix = StringUtils.GUID();

                string pars = string.Format("directoryName={0}&userKeyPrefix={1}", base.GetQueryString("DirectoryName"), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.CMSService.RegisterProgressTaskScript(JsManager.CMSService.OtherServiceName, "IndependentTemplateZip", pars, userKeyPrefix);
            }
        }
    }
}
