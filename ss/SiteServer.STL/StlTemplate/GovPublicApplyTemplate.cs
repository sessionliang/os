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
	public class GovPublicApplyTemplate
	{
        private PublishmentSystemInfo publishmentSystemInfo;
        private TagStyleInfo tagStyleInfo;
        private TagStyleGovPublicApplyInfo applyInfo;

        public const string Holder_StyleID = "{StyleID}";
        public const string Holder_ActionUrl = "{ActionUrl}";

        public GovPublicApplyTemplate(PublishmentSystemInfo publishmentSystemInfo, TagStyleInfo tagStyleInfo, TagStyleGovPublicApplyInfo applyInfo)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.tagStyleInfo = tagStyleInfo;
            this.applyInfo = applyInfo;
        }

        public string GetTemplate(bool isTemplate, string inputTemplateString, string successTemplateString, string failureTemplateString)
        {
            StringBuilder inputBuilder = new StringBuilder();

            inputBuilder.AppendFormat(@"
<link href=""{0}"" type=""text/css"" rel=""stylesheet"" />
<script type=""text/javascript"">
var govPublicActionUrl = '{1}';
var govPublicAjaxUploadUrl = '{2}';
</script>
<script type=""text/javascript"" charset=""utf-8"" src=""{3}""></script>
", StlTemplateManager.GovPublicApply.StyleUrl, PageUtility.Services.GetActionUrlOfGovPublicApply(this.publishmentSystemInfo, this.tagStyleInfo.StyleID), StlTemplateManager.GovPublicApply.GetAjaxUploadUrl(this.publishmentSystemInfo.PublishmentSystemID), StlTemplateManager.GovPublicApply.ScriptUrl);

            if (!string.IsNullOrEmpty(inputTemplateString))
            {
                inputBuilder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());
                inputBuilder.Append(inputTemplateString);
            }
            else
            {
                if (isTemplate)
                {
                    if (!string.IsNullOrEmpty(this.tagStyleInfo.ScriptTemplate))
                    {
                        inputBuilder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.tagStyleInfo.ScriptTemplate);
                    }
                    inputBuilder.Append(this.tagStyleInfo.ContentTemplate);
                }
                else
                {
                    inputBuilder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());
                    inputBuilder.Append(this.GetFileInputTemplate());
                }
            }

            StringBuilder successBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(successTemplateString))
            {
                successBuilder.Append(successTemplateString);
            }
            else
            {
                successBuilder.Append(this.GetFileSuccessTemplate());
            }

            StringBuilder failureBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(failureTemplateString))
            {
                failureBuilder.Append(failureTemplateString);
            }
            else
            {
                failureBuilder.Append(this.GetFileFailureTemplate());
            }

            return GovPublicApplyTemplate.ReplacePlaceHolder(inputBuilder.ToString(), successBuilder.ToString(), failureBuilder.ToString());
        }

        public string GetScript()
        {
            return @"
function stlApplyCallback(jsonString){
    $('#frmApply').hideLoading();
	var obj = eval('(' + jsonString + ')');
	if (obj){
		$('#applySuccess').hide();
		$('#applyFailure').hide();
		if (obj.isSuccess == 'false'){
			$('#applyFailure').show();
			$('#applyFailureMessage').html(obj.failureMessage);
		}else{
			$('#applySuccess').show();
			$('#applyQueryCode').html(obj.queryCode);
			$('#applyContainer').hide();
		}
	}
}
";
        }

        public string GetFileInputTemplate()
        {
            return FileUtils.ReadText(PageUtility.Services.GetPath("govpublicapply/inputTemplate.html"), ECharset.utf_8);
        }

        public string GetFileSuccessTemplate()
        {
            return FileUtils.ReadText(PageUtility.Services.GetPath("govpublicapply/successTemplate.html"), ECharset.utf_8);
        }

        public string GetFileFailureTemplate()
        {
            return FileUtils.ReadText(PageUtility.Services.GetPath("govpublicapply/failureTemplate.html"), ECharset.utf_8);
        }

        public static string GetCallbackScript(PublishmentSystemInfo publishmentSystemInfo, bool isSuccess, string queryCode, string failureMessage)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();
            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
            jsonAttributes.Add("queryCode", queryCode);
            jsonAttributes.Add("failureMessage", failureMessage);

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);

            if (PageUtility.IsCrossDomain(publishmentSystemInfo))
            {
                string script = string.Format("<script>window.parent.parent.stlApplyCallback('{0}');</script>", jsonString);
                string proxyUrl = PageUtility.GetProxyUrl(publishmentSystemInfo, script);
                return string.Format(@"<script>document.write(""<iframe src='{0}' style='display:none'></iframe>"");</script>", proxyUrl);
            }
            else
            {
                return string.Format("<script>window.parent.stlApplyCallback('{0}');</script>", jsonString);
            }
        }

        private static string ReplacePlaceHolder(string fileInputTemplate, string fileSuccessTemplate, string fileFailureTemplate)
        {
            return string.Format(@"
<div id=""applySuccess"" style=""display:none"">{0}</div>
<div id=""applyFailure"" style=""display:none"">{1}</div>
<div id=""applyContainer"" class=""applyContainer"">
  <form id=""frmApply"" name=""frmApply"" style=""margin:0;padding:0"" method=""post"" enctype=""multipart/form-data"">
  {2}
  </form>
  <iframe id=""iframeApply"" name=""iframeApply"" width=""0"" height=""0"" frameborder=""0""></iframe>
</div>
", fileSuccessTemplate, fileFailureTemplate, fileInputTemplate);
        }
	}
}
