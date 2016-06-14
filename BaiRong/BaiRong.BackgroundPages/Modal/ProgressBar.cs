using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.HtmlControls;
using BaiRong.Controls;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Core.Data.Provider;


namespace BaiRong.BackgroundPages.Modal
{
    public class ProgressBar : BackgroundBasePage
    {
        public Literal ltlScripts;

        public static string GetRedirectUrlStringOfHotfixWithoutDownload()
        {
            return PageUtils.GetPlatformUrl("modal_progressBar.aspx?isDownload=True&hotfix=True");
        }

        public static string GetRedirectUrlStringOfHotfix(string hotfixID)
        {
            return PageUtils.GetPlatformUrl(string.Format("modal_progressBar.aspx?isDownload=False&hotfixID={0}&hotfix=True", hotfixID));
        }

        public static string GetInitSMSTemplate(string smsServerEName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("initSMSTemplate", "True");
            arguments.Add("smsServerEName", smsServerEName);
            return JsUtils.Layer.GetOpenLayerString("初始化短信模板页面", PageUtils.GetPlatformUrl(string.Format("modal_progressBar.aspx")), arguments, 500, 360);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (base.GetQueryString("hotfix") != null)
            {
                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "升级产品到最新版本");
                bool isDownload = base.GetBoolQueryString("isDownload");
                string hotfixID = base.GetQueryString("hotfixID");
                this.ltlScripts.Text = this.RegisterProgressHotfix(isDownload, hotfixID);
            }
            else if (base.GetQueryString("initSMSTemplate") != null)
            {
                this.ltlScripts.Text = this.InitSMSTemplate();
            }
        }

        public string RegisterProgressHotfix(bool isDownload, string hotfixID)
        {
            string userKeyPrefix = StringUtils.GUID();
            string pars = string.Format("isDownload={0}&hotfixID={1}&userKeyPrefix={2}", isDownload, hotfixID, userKeyPrefix);
            return string.Format(@"
<script type=""text/javascript"" language=""javascript"">
var intervalID;
function WebServiceUtility_Task()
{{
	var url = '{0}';
	var pars = '{1}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        clearInterval(intervalID);
        writeResult(data.isUpgrade, data.resultMessage, data.errorMessage);
    }}, 'json');
}}

function WebServiceUtility_GetCountArray()
{{
	var url = '{2}';
	var pars = 'userKeyPrefix={3}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        if (data.totalCount)
        {{
            var totalCount = parseInt(data.totalCount);
            var currentCount = parseInt(data.currentCount);
            writeProgressBar(totalCount, currentCount, data.message);
        }}
    }}, 'json');
}}

function WebServiceUtility_Initialize()
{{
	WebServiceUtility_Task();
    intervalID = setInterval(WebServiceUtility_GetCountArray, 3000);
}}

$(document).ready(function(){{
    WebServiceUtility_Initialize();
}});
</script>
", PageUtils.GetPlatformSystemServiceUrl("Hotfix"), pars, PageUtils.GetPlatformSystemServiceUrl("GetCountArray"), userKeyPrefix);
        }

        public string InitSMSTemplate()
        {
            string userKeyPrefix = StringUtils.GUID();
            string pars = string.Format("userKeyPrefix={0}", userKeyPrefix);
            return string.Format(@"
<script type=""text/javascript"" language=""javascript"">
var intervalID;
function WebServiceUtility_Task()
{{
	var url = '{0}';
	var pars = '{1}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        clearInterval(intervalID);
        writeResult(data.isUpgrade, data.resultMessage, data.errorMessage);
    }}, 'json');
}}

function WebServiceUtility_GetCountArray()
{{
	var url = '{2}';
	var pars = 'userKeyPrefix={3}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        if (data.totalCount)
        {{
            var totalCount = parseInt(data.totalCount);
            var currentCount = parseInt(data.currentCount);
            writeProgressBar(totalCount, currentCount, data.message);
        }}
    }}, 'json');
}}

function WebServiceUtility_Initialize()
{{
	WebServiceUtility_Task();
    intervalID = setInterval(WebServiceUtility_GetCountArray, 3000);
}}

$(document).ready(function(){{
    WebServiceUtility_Initialize();
}});
</script>
", PageUtils.GetPlatformUserServiceUrl("InitSMSTemplate"), pars, PageUtils.GetPlatformUserServiceUrl("GetCountArray"), userKeyPrefix);
        }
    }
}
