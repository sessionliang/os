using System.Web.UI;
using BaiRong.Core;

namespace SiteServer.STL.Parser
{
	public class StlAjaxLoading
	{
        private string containerID;
        private string ajaxUrl;
        private string scriptName;
        private string updater;
        private string theEvent;

        public string ContainerID
        {
            get { return containerID; }
            set { containerID = value; }
        }

        public string AjaxUrl
        {
            get { return ajaxUrl; }
            set { ajaxUrl = value; }
        }

        public string ScriptName
        {
            get { return scriptName; }
            set { scriptName = value; }
        }

        public string Updater
        {
            get { return updater; }
            set { updater = value; }
        }

        public string Event
        {
            get { return theEvent; }
            set { theEvent = value; }
        }

        public StlAjaxLoading(string containerID, string ajaxUrl, string scriptName)
        {
            this.containerID = containerID;
            this.ajaxUrl = ajaxUrl;
            this.scriptName = scriptName;
        }

        private StlAjaxLoading()
        {

        }

        public string GetScript()
        {
            if (!string.IsNullOrEmpty(this.AjaxUrl))
            {
                string ajaxUrl = PageUtils.ParseNavigationUrl(this.AjaxUrl);

                string updateScript = string.Format("{0}();", this.ScriptName);
                if (!string.IsNullOrEmpty(this.Updater) && !string.IsNullOrEmpty(this.Event))
                {
                    updateScript = string.Format(@"
<script type=""text/javascript"" language=""javascript"">
	Event.observe({0}, '{1}', function(){{{2}()}}, false);
</script>
", this.Updater, this.Event, this.ScriptName);
                }

                return string.Format(@"
<script type=""text/javascript"" language=""javascript"">
function {0}(ajaxUrl) {{
    if (!ajaxUrl){{
        ajaxUrl = '{1}';
        ajaxUrl += stlGetQueryString(true);
    }}
	var option = {{
		method:'get',
		evalScripts:true,
		onSuccess:function(){{

		}},
		onFailure:function(){{
			$('{2}').innerHTML = ""网络繁忙，请稍后再试...."";
		}}
	}};
	new Ajax.Updater ({{success:'{2}'}}, ajaxUrl, option);
}}
{3}
</script>
	", this.ScriptName, ajaxUrl, this.containerID, updateScript);
            }

            return string.Empty;
        }

        public string GetRenderHtml(bool isScript)
        {
            if (!string.IsNullOrEmpty(this.AjaxUrl))
            {
                return string.Format(@"
<div id=""{0}""></div>{1}
", this.ContainerID, (isScript) ? this.GetScript() : string.Empty);
            }

            return string.Empty;
        }
	}
}
