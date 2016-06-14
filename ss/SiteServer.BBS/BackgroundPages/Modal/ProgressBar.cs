using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Core;

using System.Web;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class ProgressBar : BackgroundBasePage
    {
        public Literal RegisterScripts;

        public static string GetOpenWindowStringWithCreateFiles(int publishmentSystemID, string templateDir, string directoryName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("directoryName", directoryName);
            arguments.Add("CreateFiles", true.ToString());
            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("生成页面", PageUtils.GetBBSUrl("modal_progressBar.aspx"), arguments, "FileNameCollection", "需要选择文件进行生成", 500, 300);
        }

        public static string GetOpenWindowStringWithCreateAll(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("CreateAll", true.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("生成全部", PageUtils.GetBBSUrl("modal_progressBar.aspx"), arguments, 500, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (Request.QueryString["CreateFiles"] != null)
            {
                string directoryName = base.GetQueryString("directoryName");
                string userKeyPrefix = StringUtils.GUID();

                string pars = string.Format("directoryName={0}&userKeyPrefix={1}", directoryName, userKeyPrefix);

                DbCacheManager.Insert(userKeyPrefix + "FileNameCollection", Request.QueryString["FileNameCollection"]);

                this.RegisterScripts.Text = this.RegisterProgressTaskScript("CreateFiles", pars, userKeyPrefix);
            }
            else if (Request.QueryString["CreateAll"] != null)
            {
                string userKeyPrefix = StringUtils.GUID();

                string pars = string.Format("userKeyPrefix={0}", userKeyPrefix);

                this.RegisterScripts.Text = this.RegisterProgressTaskScript("CreateAll", pars, userKeyPrefix);
            }
        }

        private string RegisterProgressTaskScript(string methodName, string pars, string userKeyPrefix)
        {
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
        writeResult(data.resultMessage, data.errorMessage);
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
", Service.GetRedirectUrl(base.PublishmentSystemID, methodName), pars, Service.GetRedirectUrl(base.PublishmentSystemID, "GetCountArray"), userKeyPrefix);
        }
    }
}
