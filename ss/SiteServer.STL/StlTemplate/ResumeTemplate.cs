using System.Text;
using BaiRong.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using BaiRong.Core.Data.Provider;

using System;
using System.Collections.Specialized;
using System.Collections;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.STL.Parser;

using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Core;

namespace SiteServer.STL.StlTemplate
{
    public class ResumeTemplate
    {
        private PublishmentSystemInfo publishmentSystemInfo;
        private TagStyleInfo tagStyleInfo;
        private TagStyleResumeInfo resumeInfo;

        public const string Holder_StyleID = "{StyleID}";
        public const string Holder_ActionUrl = "{ActionUrl}";

        public ResumeTemplate(PublishmentSystemInfo publishmentSystemInfo, TagStyleInfo tagStyleInfo, TagStyleResumeInfo resumeInfo)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.tagStyleInfo = tagStyleInfo;
            this.resumeInfo = resumeInfo;
        }

        public string GetTemplate(bool isTemplate, string successTemplateString, string failureTemplateString)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(@"
<link href=""{0}"" type=""text/css"" rel=""stylesheet"" />
<script type=""text/javascript"">
var resumeActionUrl = '{1}';
var resumePreviewUrl = '{2}';
var resumeAjaxUploadUrl = '{3}';
</script>
<script type=""text/javascript"" charset=""utf-8"" src=""{4}""></script>
", StlTemplateManager.Resume.StyleUrl, PageUtility.Services.GetActionUrlOfResume(this.publishmentSystemInfo, this.tagStyleInfo.StyleID), PageUtility.GetResumePreviewUrl(this.publishmentSystemInfo.PublishmentSystemID, 0), StlTemplateManager.Resume.GetAjaxUploadUrl(this.publishmentSystemInfo.PublishmentSystemID), StlTemplateManager.Resume.ScriptUrl);

            if (isTemplate)
            {
                if (!string.IsNullOrEmpty(this.tagStyleInfo.ScriptTemplate))
                {
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.tagStyleInfo.ScriptTemplate);
                }

                builder.Append(this.tagStyleInfo.ContentTemplate);
            }
            else
            {
                builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());

                builder.Append(this.GetContent());
            }

            return ResumeTemplate.ReplacePlaceHolder(builder.ToString(), successTemplateString, failureTemplateString);
        }

        public string GetScript()
        {
            return @"
function stlResumeCallback(jsonString){
	var obj = eval('(' + jsonString + ')');
	if (obj){
		document.getElementById('resumeSuccess').style.display = 'none';
		document.getElementById('resumeFailure').style.display = 'none';
		if (obj.isSuccess == 'false'){
			document.getElementById('resumeFailure').style.display = '';
			document.getElementById('resumeFailure').innerHTML = obj.message;
		}else{
			document.getElementById('resumeSuccess').style.display = '';
			document.getElementById('resumeSuccess').innerHTML = obj.message;
			document.getElementById('resumeContainer').style.display = 'none';
		}
	}
}
";
        }

        public string GetContent()
        {
            return FileUtils.ReadText(PageUtility.Services.GetPath("resume/template.html"), ECharset.utf_8);
        }

        public static string GetCallbackScript(PublishmentSystemInfo publishmentSystemInfo, bool isSuccess, string message)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();
            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
            jsonAttributes.Add("message", message);

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);

            if (PageUtility.IsAgentCrossDomain(publishmentSystemInfo))
            {
                string script = string.Format("<script>window.parent.parent.stlResumeCallback('{0}');</script>", jsonString);
                string proxyUrl = PageUtility.GetProxyUrl(publishmentSystemInfo, script);
                return string.Format(@"<script>document.write(""<iframe src='{0}' style='display:none'></iframe>"");</script>", proxyUrl);
            }
            else if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                return string.Format("<script>window.stlResumeCallback('{0}');</script>", jsonString);
            }
            else
            {
                return string.Format("<script>window.parent.stlResumeCallback('{0}');</script>", jsonString);
            }
        }

        public static string ReplacePlaceHolder(string template, string successTemplateString, string failureTemplateString)
        {
            StringBuilder parsedContent = new StringBuilder();

            parsedContent.AppendFormat(@"
<div id=""resumeSuccess"" class=""successContainer"" style=""display:none""></div>
<div id=""resumeFailure"" class=""failureContainer"" style=""display:none""></div>
<div id=""resumeContainer"" class=""resumeContainer"">
  <form id=""frmResume"" name=""frmResume"" style=""margin:0;padding:0"" method=""post"" enctype=""multipart/form-data"">
  <input type=""hidden"" id=""JobContentID"" name=""JobContentID"" value="""" />
");
            //添加遮罩层
            parsedContent.AppendFormat(@"	
<div id=""resumeModal"" times=""2"" id=""xubox_shade2"" class=""xubox_shade"" style=""z-index:19891016; background-color: #FFF; opacity: 0.5; filter:alpha(opacity=10);top: 0;left: 0;width: 100%;height: 100%;position: fixed;display:none;""></div>
<div id=""resumeModalMsg"" times=""2"" showtime=""0"" style=""z-index: 19891016; left: 50%; top: 206px; width: 500px; height: 360px; margin-left: -250px;position: fixed;text-align: center;display:none;"" id=""xubox_layer2"" class=""xubox_layer"" type=""iframe""><img src = ""/sitefiles/bairong/icons/waiting.gif"" style="""">
<br>
<span style=""font-size:10px;font-family:Microsoft Yahei"">正在提交...</span>
</div>
<script>
		function openModal()
        {{
			document.getElementById(""resumeModal"").style.display = '';
            document.getElementById(""resumeModalMsg"").style.display = '';
        }}
        function closeModal()
        {{
			document.getElementById(""resumeModal"").style.display = 'none';
            document.getElementById(""resumeModalMsg"").style.display = 'none';
        }}
</script>");

            if (!string.IsNullOrEmpty(successTemplateString))
            {
                parsedContent.AppendFormat(@"<input type=""hidden"" id=""successTemplateString"" value=""{0}"" />", RuntimeUtils.EncryptStringByTranslate(successTemplateString));
            }
            if (!string.IsNullOrEmpty(failureTemplateString))
            {
                parsedContent.AppendFormat(@"<input type=""hidden"" id=""failureTemplateString"" value=""{0}"" />", RuntimeUtils.EncryptStringByTranslate(failureTemplateString));
            }

            parsedContent.Append(template);

            parsedContent.Append(@"
</form>
<iframe id=""iframeResume"" name=""iframeResume"" width=""0"" height=""0"" frameborder=""0""></iframe>
</div>");

            return parsedContent.ToString();
        }
    }
}
