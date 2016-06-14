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
	public class GovPublicQueryTemplate
	{
        private PublishmentSystemInfo publishmentSystemInfo;
        private TagStyleInfo tagStyleInfo;

        public const string Holder_StyleID = "{StyleID}";

        public GovPublicQueryTemplate(PublishmentSystemInfo publishmentSystemInfo, TagStyleInfo tagStyleInfo)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.tagStyleInfo = tagStyleInfo;
        }

        public string GetTemplate(bool isTemplate, string inputTemplateString, string successTemplateString, string failureTemplateString)
        {
            StringBuilder inputBuilder = new StringBuilder();

            inputBuilder.AppendFormat(@"
<link href=""{0}"" type=""text/css"" rel=""stylesheet"" />
<script type=""text/javascript"">
var govPublicQueryActionUrl = '{1}';
</script>
<script type=""text/javascript"" charset=""utf-8"" src=""{2}""></script>
", StlTemplateManager.GovPublicQuery.StyleUrl, PageUtility.Services.GetActionUrlOfGovPublicQuery(this.publishmentSystemInfo, this.tagStyleInfo.StyleID), StlTemplateManager.GovPublicQuery.ScriptUrl);

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

            return GovPublicQueryTemplate.ReplacePlaceHolder(inputBuilder.ToString(), successBuilder.ToString(), failureBuilder.ToString());
        }

        public string GetScript()
        {
            return @"
function stlQueryCallback(jsonString){
    $('#frmQuery').hideLoading();
	var obj = eval('(' + jsonString + ')');
	if (obj){
		$('#querySuccess').hide();
		$('#queryFailure').hide();
		if (obj.isSuccess == 'false'){
			$('#queryFailure').show();
			$('#queryFailureMessage').html(obj.failureMessage);
		}else{
			$('#querySuccess').show();
			$('#querySuccess').setTemplateElement('successTemplate');
			$('#querySuccess').processTemplate(obj);
			$('#queryContainer').hide();
            if (obj.isOrg == 'true'){
                $('#dataContainer1').hide();
                $('#dataContainer2').show();
            }
		}
	}
}
";
        }

        public string GetFileInputTemplate()
        {
            return FileUtils.ReadText(PageUtility.Services.GetPath("govpublicquery/inputTemplate.html"), ECharset.utf_8);
        }

        public string GetFileSuccessTemplate()
        {
            return FileUtils.ReadText(PageUtility.Services.GetPath("govpublicquery/successTemplate.html"), ECharset.utf_8);
        }

        public string GetFileFailureTemplate()
        {
            return FileUtils.ReadText(PageUtility.Services.GetPath("govpublicquery/failureTemplate.html"), ECharset.utf_8);
        }

        public static string GetCallbackScript(PublishmentSystemInfo publishmentSystemInfo, bool isSuccess, GovPublicApplyInfo applyInfo, string failureMessage)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();
            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
            if (isSuccess && applyInfo != null)
            {
                jsonAttributes.Add("isOrg", applyInfo.IsOrganization.ToString().ToLower());
                foreach (string attributeName in applyInfo.Attributes.Keys)
                {
                    jsonAttributes.Add(attributeName, applyInfo.GetExtendedAttribute(attributeName));
                }
                jsonAttributes.Add("replystate", EGovPublicApplyStateUtils.GetFrontText(applyInfo.State));
                if (applyInfo.State == EGovPublicApplyState.Checked || applyInfo.State == EGovPublicApplyState.Denied)
                {
                    GovPublicApplyReplyInfo replyInfo = DataProvider.GovPublicApplyReplyDAO.GetReplyInfoByApplyID(applyInfo.ID);
                    if (replyInfo != null)
                    {
                        jsonAttributes.Add("replycontent", replyInfo.Reply);
                        jsonAttributes.Add("replyfileurl", replyInfo.FileUrl);
                        jsonAttributes.Add("replydepartmentname", DepartmentManager.GetDepartmentName(replyInfo.DepartmentID));
                        jsonAttributes.Add("replyusername", replyInfo.UserName);
                        jsonAttributes.Add("replyadddate", DateUtils.GetDateAndTimeString(replyInfo.AddDate));
                    }
                }
            }
            jsonAttributes.Add("failureMessage", failureMessage);

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);

            if (PageUtility.IsCrossDomain(publishmentSystemInfo))
            {
                string script = string.Format("<script>window.parent.parent.stlQueryCallback('{0}');</script>", jsonString);
                string proxyUrl = PageUtility.GetProxyUrl(publishmentSystemInfo, script);
                return string.Format(@"<script>document.write(""<iframe src='{0}' style='display:none'></iframe>"");</script>", proxyUrl);
            }
            else
            {
                return string.Format("<script>window.parent.stlQueryCallback('{0}');</script>", jsonString);
            }
        }

        private static string ReplacePlaceHolder(string fileInputTemplate, string fileSuccessTemplate, string fileFailureTemplate)
        {
            return string.Format(@"
<div id=""querySuccess"" style=""display:none"">{0}</div>
<div id=""queryFailure"" style=""display:none"">{1}</div>
<div id=""queryContainer"" class=""queryContainer"">
  <form id=""frmQuery"" name=""frmQuery"" style=""margin:0;padding:0"" method=""post"" enctype=""multipart/form-data"">
  {2}
  </form>
  <iframe id=""iframeQuery"" name=""iframeQuery"" width=""0"" height=""0"" frameborder=""0""></iframe>
</div>
", fileSuccessTemplate, fileFailureTemplate, fileInputTemplate);
        }
	}
}
