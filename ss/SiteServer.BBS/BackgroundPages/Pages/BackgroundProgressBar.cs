using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections;
using System.Collections.Specialized;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
	public class BackgroundProgressBar : BackgroundBasePage
	{
        public Literal ltlTitle;
        public Literal RegisterScripts;

        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public static string GetCreateForumsUrl(int publishmentSystemID, string userKeyPrefix)
        {
            return string.Format("background_progressBar.aspx?publishmentSystemID={0}&CreateForums=True&UserKeyPrefix={1}", publishmentSystemID, userKeyPrefix);
        }

        public static string GetCreateFilesUrl(int publishmentSystemID, string userKeyPrefix)
        {
            return string.Format("background_progressBar.aspx?publishmentSystemID={0}&CreateFiles=True&UserKeyPrefix={1}", publishmentSystemID, userKeyPrefix);
        }

        public static string GetTranslateUrl(int publishmentSystemID, string userKeyPrefix, string connectionString, bool isImportUsers, bool isImportAvatars, bool isImportForums)
        {
            return string.Format("background_progressBar.aspx?publishmentSystemID={0}&Translate=True&UserKeyPrefix={1}", publishmentSystemID, userKeyPrefix);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            string userKeyPrefix = Request.QueryString["UserKeyPrefix"];

            if (Request.QueryString["CreateForums"] != null && userKeyPrefix != null)
            {
                this.ltlTitle.Text = "生成板块页";
                string pars = string.Format("userKeyPrefix={0}", userKeyPrefix);
                this.RegisterScripts.Text = this.RegisterProgressTaskScript("CreateForums", pars, userKeyPrefix);
            }
            else if (Request.QueryString["CreateThreads"] != null && Request.QueryString["UserKeyPrefix"] != null)
            {
                this.ltlTitle.Text = "生成主题页";
                string pars = string.Format("userKeyPrefix={0}", userKeyPrefix);
                this.RegisterScripts.Text = this.RegisterProgressTaskScript("CreateThreads", pars, userKeyPrefix);
            }
            else if (Request.QueryString["CreateFiles"] != null && Request.QueryString["UserKeyPrefix"] != null)
            {
                this.ltlTitle.Text = "生成文件页";
                string pars = string.Format("userKeyPrefix={0}", userKeyPrefix);
                this.RegisterScripts.Text = this.RegisterProgressTaskScript("CreateFiles", pars, userKeyPrefix);
            }
            else if (Request.QueryString["CreateIndex"] != null)
			{
                CreateIndex();
			}			
		}

		//生成首页
        private void CreateIndex()
        {
            this.ltlTitle.Text = "生成首页";
            HyperLink link = new HyperLink();
            link.NavigateUrl = PageUtilityBBS.GetIndexPageUrl(base.PublishmentSystemID);
            link.Text = "浏览";
            if (link.NavigateUrl != PageUtils.UNCLICKED_URL)
            {
                link.Target = "_blank";
            }
            link.Style.Add("text-decoration", "underline");
            try
            {
                FileSystemObject FSO = new FileSystemObject(base.PublishmentSystemID);

                FSO.CreateIndex();

                this.RegisterScripts.Text = @"
<script>
$(document).ready(function(){
    writeResult('首页生成成功。', '');
})
</script>
";
            }
            catch (Exception ex)
            {
                this.RegisterScripts.Text = string.Format(@"
<script>
$(document).ready(function(){{
    writeResult('', '{0}');
}})
</script>
", ex.Message);
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
