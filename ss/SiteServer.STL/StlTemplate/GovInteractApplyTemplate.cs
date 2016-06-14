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
    public class GovInteractApplyTemplate : InputTemplateBase
    {
        private PublishmentSystemInfo publishmentSystemInfo;
        private int nodeID;
        private TagStyleInfo tagStyleInfo;
        private TagStyleGovInteractApplyInfo tagStyleApplyInfo;
        private bool isValidateCode;

        public GovInteractApplyTemplate(PublishmentSystemInfo publishmentSystemInfo, int nodeID, TagStyleInfo tagStyleInfo, TagStyleGovInteractApplyInfo tagStyleApplyInfo)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.nodeID = nodeID;
            this.tagStyleInfo = tagStyleInfo;
            this.tagStyleApplyInfo = tagStyleApplyInfo;
            this.isValidateCode = this.tagStyleApplyInfo.IsValidateCode;
            if (this.isValidateCode)
            {
                this.isValidateCode = FileConfigManager.Instance.IsValidateCode;
            }
        }

        public string GetTemplate(bool isTemplate, string inputTemplateString, string successTemplateString, string failureTemplateString)
        {
            StringBuilder inputBuilder = new StringBuilder();

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
                if (isTemplate)
                {
                    successBuilder.Append(this.tagStyleInfo.SuccessTemplate);
                }
                else
                {
                    successBuilder.Append(this.GetFileSuccessTemplate());
                }
            }

            StringBuilder failureBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(failureTemplateString))
            {
                failureBuilder.Append(failureTemplateString);
            }
            else
            {
                if (isTemplate)
                {
                    failureBuilder.Append(this.tagStyleInfo.FailureTemplate);
                }
                else
                {
                    failureBuilder.Append(this.GetFileFailureTemplate());
                }
            }

            return this.ReplacePlaceHolder(inputBuilder.ToString(), successBuilder.ToString(), failureBuilder.ToString());
        }

        public string GetScript()
        {
            string script = @"function submit_apply_[nodeID]()
{
	if (checkFormValueById('frmApply_[nodeID]'))
	{
		$('#frmApply_[nodeID]').showLoading();
		var frmApply_[nodeID] = document.getElementById('frmApply_[nodeID]');
		frmApply_[nodeID].action = '[actionUrl]';
		frmApply_[nodeID].target = 'iframeApply_[nodeID]';
		frmApply_[nodeID].submit();
	}
}
function stlApplyCallback_[nodeID](jsonString){
    $('#frmApply_[nodeID]').hideLoading();
	var obj = eval('(' + jsonString + ')');
	if (obj){
		$('#applySuccess_[nodeID]').hide();
		$('#applyFailure_[nodeID]').hide();
		if (obj.isSuccess == 'false'){
			$('#applyFailure_[nodeID]').show();
			$('#applyFailureMessage_[nodeID]').html(obj.failureMessage);
		}else{
			$('#applySuccess_[nodeID]').show();
			$('#applyQueryCode_[nodeID]').html(obj.queryCode);
			$('#applyContainer_[nodeID]').hide();
		}
	}
}
";
            script = script.Replace("[nodeID]", this.nodeID.ToString());
            script = script.Replace("[actionUrl]", PageUtility.Services.GetActionUrlOfGovInteractApply(this.publishmentSystemInfo, this.nodeID, this.tagStyleInfo.StyleID));
            return script;
        }

        public string GetFileInputTemplate()
        {
            string content = FileUtils.ReadText(PageUtility.Services.GetPath("govinteractapply/inputTemplate.html"), ECharset.utf_8);

            string regex = "<!--parameters:(?<params>[^\"]*)-->";
            string paramstring = RegexUtils.GetContent("params", regex, content);
            NameValueCollection parameters = TranslateUtils.ToNameValueCollection(paramstring);
            string tdNameClass = parameters["tdNameClass"];
            string tdInputClass = parameters["tdInputClass"];

            if (parameters.Count > 0)
            {
                content = content.Replace(string.Format("<!--parameters:{0}-->\r\n", paramstring), string.Empty);
            }

            content = string.Format(@"<link href=""{0}"" type=""text/css"" rel=""stylesheet"" />
", StlTemplateManager.GovInteractApply.StyleUrl) + content;

            StringBuilder builder = new StringBuilder();
            ArrayList tableStyleInfoArrayList = RelatedIdentities.GetTableStyleInfoArrayList(publishmentSystemInfo, ETableStyle.GovInteractContent, this.nodeID);

            if (this.isValidateCode)
            {
                TableStyleInfo styleInfo = new TableStyleInfo();
                styleInfo.AttributeName = ValidateCodeManager.AttributeName;
                styleInfo.DisplayName = "验证码";
                styleInfo.Additional.Width = "50px";
                tableStyleInfoArrayList.Add(styleInfo);
            }

            NameValueCollection pageScripts = new NameValueCollection();

            bool isPreviousSingleLine = true;
            bool isPreviousLeftColumn = false;
            foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
            {
                if (styleInfo.IsVisible)
                {
                    string value = InputTypeParser.Parse(this.publishmentSystemInfo, nodeID, styleInfo, ETableStyle.GovInteractContent, styleInfo.AttributeName, null, false, false, null, pageScripts, false, styleInfo.Additional.IsValidate);

                    if (builder.Length > 0)
                    {
                        if (isPreviousSingleLine)
                        {
                            builder.Append("</tr>");
                        }
                        else
                        {
                            if (!isPreviousLeftColumn)
                            {
                                builder.Append("</tr>");
                            }
                            else if (styleInfo.IsSingleLine)
                            {
                                builder.AppendFormat(@"<td class=""{0}""></td><td class=""{1}""></td></tr>", tdNameClass, tdInputClass);
                            }
                        }
                    }

                    //this line

                    if (styleInfo.IsSingleLine || isPreviousSingleLine || !isPreviousLeftColumn)
                    {
                        builder.Append("<tr>");
                    }

                    builder.AppendFormat(@"<td class=""{0}"">{1}</td><td {2} class=""{3}"">{4}</td>", tdNameClass, styleInfo.DisplayName, styleInfo.IsSingleLine ? @"colspan=""3""" : string.Empty, tdInputClass, value);


                    if (styleInfo.IsSingleLine)
                    {
                        isPreviousSingleLine = true;
                        isPreviousLeftColumn = false;
                    }
                    else
                    {
                        isPreviousSingleLine = false;
                        isPreviousLeftColumn = !isPreviousLeftColumn;
                    }
                }
            }

            if (builder.Length > 0)
            {
                if (isPreviousSingleLine || !isPreviousLeftColumn)
                {
                    builder.Append("</tr>");
                }
                else
                {
                    builder.AppendFormat(@"<td class=""{0}""></td><td class=""{1}""></td></tr>", tdNameClass, tdInputClass);
                }
            }

            if (content.Contains("<!--提交表单循环-->"))
            {
                content = content.Replace("<!--提交表单循环-->", builder.ToString());
            }

            return content.Replace("[nodeID]", this.nodeID.ToString());
        }

        public string GetFileSuccessTemplate()
        {
            string retval = FileUtils.ReadText(PageUtility.Services.GetPath("govinteractapply/successTemplate.html"), ECharset.utf_8);
            return retval.Replace("[nodeID]", this.nodeID.ToString());
        }

        public string GetFileFailureTemplate()
        {
            string retval = FileUtils.ReadText(PageUtility.Services.GetPath("govinteractapply/failureTemplate.html"), ECharset.utf_8);
            return retval.Replace("[nodeID]", this.nodeID.ToString());
        }

        public static string GetCallbackScript(PublishmentSystemInfo publishmentSystemInfo, int nodeID, bool isSuccess, string queryCode, string failureMessage)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();
            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
            jsonAttributes.Add("queryCode", queryCode);
            jsonAttributes.Add("failureMessage", failureMessage);

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);

            string retval = string.Empty;
            if (PageUtility.IsCrossDomain(publishmentSystemInfo))
            {
                string script = string.Format("<script>window.parent.parent.stlApplyCallback_[nodeID]('{0}');</script>", jsonString);
                string proxyUrl = PageUtility.GetProxyUrl(publishmentSystemInfo, script);
                retval = string.Format(@"<script>document.write(""<iframe src='{0}' style='display:none'></iframe>"");</script>", proxyUrl);
            }
            else
            {
                retval = string.Format("<script>window.parent.stlApplyCallback_[nodeID]('{0}');</script>", jsonString);
            }

            return retval.Replace("[nodeID]", nodeID.ToString());
        }

        private string ReplacePlaceHolder(string fileInputTemplate, string fileSuccessTemplate, string fileFailureTemplate)
        {
            StringBuilder parsedContent = new StringBuilder();
            parsedContent.AppendFormat(@"
<div id=""applySuccess_[nodeID]"" style=""display:none"">{0}</div>
<div id=""applyFailure_[nodeID]"" style=""display:none"">{1}</div>
<div id=""applyContainer_[nodeID]"" class=""applyContainer"">
  <form id=""frmApply_[nodeID]"" name=""frmApply_[nodeID]"" style=""margin:0;padding:0"" method=""post"" enctype=""multipart/form-data"">
  {2}
  </form>
  <iframe id=""iframeApply_[nodeID]"" name=""iframeApply_[nodeID]"" width=""0"" height=""0"" frameborder=""0""></iframe>
</div>
", fileSuccessTemplate, fileFailureTemplate, fileInputTemplate);

            if (isValidateCode)
            {
                bool isCrossDomain = PageUtility.IsCrossDomain(publishmentSystemInfo);
                bool isCorsCross = PageUtility.IsCorsCrossDomain(publishmentSystemInfo);
                ValidateCodeManager vcManager = ValidateCodeManager.GetInstance(publishmentSystemInfo.PublishmentSystemID, tagStyleInfo.StyleID, isCrossDomain);
                base.ReWriteCheckCode(parsedContent, publishmentSystemInfo, vcManager, isCrossDomain, isCorsCross);
            }

            parsedContent.Replace("[nodeID]", this.nodeID.ToString());

            return parsedContent.ToString();
        }
    }
}
