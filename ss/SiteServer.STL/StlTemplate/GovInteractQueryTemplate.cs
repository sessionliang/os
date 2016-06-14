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
	public class GovInteractQueryTemplate
	{
        private PublishmentSystemInfo publishmentSystemInfo;
        private int nodeID;
        private TagStyleInfo tagStyleInfo;

        public GovInteractQueryTemplate(PublishmentSystemInfo publishmentSystemInfo, int nodeID, TagStyleInfo tagStyleInfo)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.nodeID = nodeID;
            this.tagStyleInfo = tagStyleInfo;
        }

        public string GetTemplate(bool isTemplate, string inputTemplateString, string successTemplateString, string failureTemplateString)
        {
            StringBuilder inputBuilder = new StringBuilder();

            inputBuilder.AppendFormat(@"
<link href=""{0}"" type=""text/css"" rel=""stylesheet"" />
", StlTemplateManager.GovInteractQuery.StyleUrl);

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
                string content = string.Format(@"<textarea id=""successTemplate"" style=""display:none"">{0}</textarea>", this.GetFileSuccessTemplate());
                successBuilder.Append(content);
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

            return GovInteractQueryTemplate.ReplacePlaceHolder(inputBuilder.ToString(), successBuilder.ToString(), failureBuilder.ToString());
        }

        public string GetScript()
        {
            string script = @"function submit_query()
{
	if (checkFormValueById('frmQuery'))
	{
		$('#frmQuery').showLoading();
		var frmQuery = document.getElementById('frmQuery');
		frmQuery.action = '[actionUrl]';
		frmQuery.target = 'iframeQuery';
		frmQuery.submit();
	}
}
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
		}
	}
}
";
            script = script.Replace("[actionUrl]", PageUtility.Services.GetActionUrlOfGovInteractQuery(this.publishmentSystemInfo, this.nodeID, this.tagStyleInfo.StyleID));

            return script;
        }

        public string GetFileInputTemplate()
        {
            return FileUtils.ReadText(PageUtility.Services.GetPath("govinteractquery/inputTemplate.html"), ECharset.utf_8);
        }

        public string GetFileSuccessTemplate()
        {
            string content = FileUtils.ReadText(PageUtility.Services.GetPath("govinteractquery/successTemplate.html"), ECharset.utf_8);

            string regex = "<!--parameters:(?<params>[^\"]*)-->";
            string paramstring = RegexUtils.GetContent("params", regex, content);
            NameValueCollection parameters = TranslateUtils.ToNameValueCollection(paramstring);
            string tdNameClass = parameters["tdNameClass"];
            string tdInputClass = parameters["tdInputClass"];

            if (parameters.Count > 0)
            {
                content = content.Replace(string.Format("<!--parameters:{0}-->\r\n", paramstring), string.Empty);
            }

            StringBuilder builder = new StringBuilder();
            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.GovInteractContent, publishmentSystemInfo.AuxiliaryTableForGovInteract, RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, this.nodeID));
            NameValueCollection pageScripts = new NameValueCollection();

            bool isPreviousSingleLine = true;
            bool isPreviousLeftColumn = false;
            foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
            {
                if (styleInfo.IsVisible)
                {
                    if (StringUtils.EqualsIgnoreCase(GovInteractContentAttribute.IsPublic, styleInfo.AttributeName) || StringUtils.EqualsIgnoreCase(GovInteractContentAttribute.DepartmentID, styleInfo.AttributeName) || StringUtils.EqualsIgnoreCase(GovInteractContentAttribute.TypeID, styleInfo.AttributeName)) continue;
                    string value = string.Format("{{$T.{0}}}", styleInfo.AttributeName.ToLower());

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

            if (content.Contains("<!--查询表单循环-->"))
            {
                content = content.Replace("<!--查询表单循环-->", builder.ToString());
            }

            return content;
        }

        public string GetFileFailureTemplate()
        {
            return FileUtils.ReadText(PageUtility.Services.GetPath("govinteractquery/failureTemplate.html"), ECharset.utf_8);
        }

        public static string GetCallbackScript(PublishmentSystemInfo publishmentSystemInfo, bool isSuccess, GovInteractContentInfo contentInfo, string failureMessage)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();
            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
            if (isSuccess && contentInfo != null)
            {
                foreach (string attributeName in contentInfo.Attributes.Keys)
                {
                    jsonAttributes.Add(attributeName, contentInfo.GetExtendedAttribute(attributeName));
                }
                jsonAttributes.Add("replystate", EGovInteractStateUtils.GetFrontText(contentInfo.State));
                if (contentInfo.State == EGovInteractState.Checked || contentInfo.State == EGovInteractState.Denied)
                {
                    GovInteractReplyInfo replyInfo = DataProvider.GovInteractReplyDAO.GetReplyInfoByContentID(publishmentSystemInfo.PublishmentSystemID, contentInfo.ID);
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
