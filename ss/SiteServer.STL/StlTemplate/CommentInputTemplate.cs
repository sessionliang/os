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
using System.Xml;
using SiteServer.CMS.Core;

namespace SiteServer.STL.StlTemplate
{
	public class CommentInputTemplate : InputTemplateBase
	{
        private PublishmentSystemInfo publishmentSystemInfo;
        private int commentID;
        private TagStyleInfo tagStyleInfo;
        private TagStyleCommentInputInfo tagStyleCommentInputInfo;
        private bool isValidateCode;

        public const string Holder_StyleID = "{StyleID}";

        public CommentInputTemplate(PublishmentSystemInfo publishmentSystemInfo, int commentID, TagStyleInfo tagStyleInfo, TagStyleCommentInputInfo tagStyleCommentInputInfo)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.commentID = commentID;
            this.tagStyleInfo = tagStyleInfo;
            this.tagStyleCommentInputInfo = tagStyleCommentInputInfo;
            this.isValidateCode = this.tagStyleCommentInputInfo.IsValidateCode;
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
            string script = @"
function submit_comment()
{
	if (checkFormValueById('frmComment'))
	{
		$('#frmComment').showLoading();
		var frmComment = document.getElementById('frmComment');
		frmComment.action = '[actionUrl]&channelID={Channel.ChannelID}&contentID={Content.ContentID}';
		frmComment.target = 'iframeComment';
		frmComment.submit();
	}
}
function stlCommentCallback(jsonString){
    $('#frmComment').hideLoading();
	var obj = eval('(' + jsonString + ')');
	if (obj){
		$('#commentSuccess').hide();
		$('#commentFailure').hide();
		if (obj.isSuccess == 'false'){
			$('#commentFailure').show();
			$('#commentFailureMessage').html(obj.failureMessage);
		}else{
			$('#commentSuccess').show();
			$('#commentContainer').hide();
		}
	}
}
";
            script = script.Replace("[actionUrl]", PageUtility.Services.GetActionUrlOfCommentInput(publishmentSystemInfo, tagStyleInfo.StyleID));

            return script;
        }

        public string GetFileInputTemplate()
        {
            string content = FileUtils.ReadText(PageUtility.Services.GetPath("commentInput/inputTemplate.html"), ECharset.utf_8);

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
", StlTemplateManager.CommentInput.StyleUrl) + content;

            StringBuilder builder = new StringBuilder();
            ArrayList tableStyleInfoArrayList = RelatedIdentities.GetTableStyleInfoArrayList(publishmentSystemInfo, ETableStyle.Comment, this.commentID);

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
                    string value = InputTypeParser.Parse(this.publishmentSystemInfo, this.publishmentSystemInfo.PublishmentSystemID, styleInfo, ETableStyle.Comment, styleInfo.AttributeName, null, false, false, null, pageScripts, false, styleInfo.Additional.IsValidate);

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

            return content;
        }

        public string GetFileSuccessTemplate()
        {
            return FileUtils.ReadText(PageUtility.Services.GetPath("commentInput/successTemplate.html"), ECharset.utf_8);
        }

        public string GetFileFailureTemplate()
        {
            return FileUtils.ReadText(PageUtility.Services.GetPath("commentInput/failureTemplate.html"), ECharset.utf_8);
        }

        public static string GetCallbackScript(PublishmentSystemInfo publishmentSystemInfo, bool isSuccess, string failureMessage)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();
            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
            jsonAttributes.Add("failureMessage", failureMessage);

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);

            string retval = string.Empty;
            if (PageUtility.IsCrossDomain(publishmentSystemInfo))
            {
                string script = string.Format("<script>window.parent.parent.stlCommentCallback('{0}');</script>", jsonString);
                string proxyUrl = PageUtility.GetProxyUrl(publishmentSystemInfo, script);
                retval = string.Format(@"<script>document.write(""<iframe src='{0}' style='display:none'></iframe>"");</script>", proxyUrl);
            }
            else
            {
                retval = string.Format("<script>window.parent.stlCommentCallback('{0}');</script>", jsonString);
            }

            return retval;
        }

        private string ReplacePlaceHolder(string fileInputTemplate, string fileSuccessTemplate, string fileFailureTemplate)
        {
            StringBuilder parsedContent = new StringBuilder();
            parsedContent.AppendFormat(@"
<div id=""commentSuccess"" style=""display:none"">{0}</div>
<div id=""commentFailure"" style=""display:none"">{1}</div>
<div id=""commentContainer"" class=""commentContainer"">
  <form id=""frmComment"" name=""frmComment"" style=""margin:0;padding:0"" method=""post"" enctype=""multipart/form-data"">
  {2}
  </form>
  <iframe id=""iframeComment"" name=""iframeComment"" width=""0"" height=""0"" frameborder=""0""></iframe>
</div>
", fileSuccessTemplate, fileFailureTemplate, fileInputTemplate);

            if (isValidateCode)
            {
                bool isCrossDomain = PageUtility.IsCrossDomain(publishmentSystemInfo);
                ValidateCodeManager vcManager = ValidateCodeManager.GetInstance(publishmentSystemInfo.PublishmentSystemID, tagStyleInfo.StyleID, isCrossDomain);
                base.ReWriteCheckCode(parsedContent, publishmentSystemInfo, vcManager, isCrossDomain);
            }

            return parsedContent.ToString();
        }

//        private string Render(NameValueCollection pageScripts)
//        {
//            StringBuilder output = new StringBuilder();

//            bool isValidateCode = this.inputInfo.IsValidateCode;
//            if (isValidateCode)
//            {
//                isValidateCode = FileConfigManager.Instance.IsValidateCode;
//            }

//            if (styleInfoArrayList != null)
//            {
//                foreach (TableStyleInfo styleInfo in styleInfoArrayList)
//                {
//                    if (styleInfo.IsVisible == false)
//                    {
//                        continue;
//                    }

//                    if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, CommentAttribute.Content))
//                    {
//                        if (styleInfo.Additional.IsValidate == false)
//                        {
//                            styleInfo.Additional.IsValidate = true;
//                            styleInfo.Additional.IsRequired = true;
//                            styleInfo.Additional.ValidateType = EInputValidateType.None;
//                        }
//                    }

//                    this.GetAttributeHtml(styleInfo, pageScripts, output);
//                }

//                if (isValidateCode)
//                {
//                    TableStyleInfo styleInfo = new TableStyleInfo();
//                    styleInfo.AttributeName = ValidateCodeManager.AttributeName;
//                    styleInfo.DisplayName = "验证码";
//                    styleInfo.Additional.Width = "50px";
//                    string inputHtml = InputTypeParser.ParseText(ValidateCodeManager.AttributeName, null, true, base.GetInnerAdditionalAttributes(styleInfo), styleInfo, false);

//                    output.AppendFormat(@"
//<tr><td height=""30"">验证码:</td><td>{0}</td></tr>
//", inputHtml);
//                }
//            }
//            return output.ToString();
//        }

//        private void GetAttributeHtml(TableStyleInfo styleInfo, NameValueCollection pageScripts, StringBuilder output)
//        {
//            string helpHtml = styleInfo.DisplayName + ":";
//            string inputHtml = InputTypeParser.Parse(publishmentSystemInfo, 0, styleInfo, ETableStyle.Comment, styleInfo.AttributeName, null, false, false, base.GetInnerAdditionalAttributes(styleInfo), pageScripts, false, true);

//            output.AppendFormat(@"
//<tr><td width=""70""><nobr>{0}</nobr></td><td>{1}</td></tr>
//", helpHtml, inputHtml);
//        }

//        public string GetTemplate(bool isTemplate, int channelID, int contentID, string inputTemplateString, string successTemplateString, string failureTemplateString)
//        {
//            StringBuilder builder = new StringBuilder();

//            builder.AppendFormat(@"
//<script type=""text/javascript"" charset=""{0}"" src=""{1}""></script>", ClientManager.Validate.Charset, PageUtils.ParseConfigRootUrl(ClientManager.Validate.Js));

//            if (string.IsNullOrEmpty(inputTemplateString))
//            {
//                if (isTemplate)
//                {
//                    if (!string.IsNullOrEmpty(this.tagStyleInfo.StyleTemplate))
//                    {
//                        builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.tagStyleInfo.StyleTemplate);
//                    }
//                    if (!string.IsNullOrEmpty(this.tagStyleInfo.ScriptTemplate))
//                    {
//                        builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.tagStyleInfo.ScriptTemplate);
//                    }

//                    builder.Append(this.tagStyleInfo.ContentTemplate);
//                }
//                else
//                {
//                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.GetStyle(ETableStyle.Comment));
//                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());

//                    builder.Append(this.GetContent());
//                }
//            }
//            else
//            {
//                if (isTemplate)
//                {
//                    if (!string.IsNullOrEmpty(this.tagStyleInfo.StyleTemplate))
//                    {
//                        builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.tagStyleInfo.StyleTemplate);
//                    }
//                    if (!string.IsNullOrEmpty(this.tagStyleInfo.ScriptTemplate))
//                    {
//                        builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.tagStyleInfo.ScriptTemplate);
//                    }
//                }
//                else
//                {
//                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.GetStyle(ETableStyle.Comment));
//                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());
//                }
//                builder.Append(inputTemplateString);
//            }

//            bool isValidateCode = this.inputInfo.IsValidateCode;
//            if (isValidateCode)
//            {
//                isValidateCode = FileConfigManager.Instance.IsValidateCode;
//            }

//            return ReplacePlaceHolder(builder.ToString(), isValidateCode, channelID, contentID, successTemplateString, failureTemplateString);
//        }

//        public string GetScript()
//        {
//            StringBuilder builder = new StringBuilder();

//            string additionalScript = string.Empty;
//            if (this.inputInfo.IsSuccessHide)
//            {
//                additionalScript += string.Format(@"
//			document.getElementById('commentContainer_{0}').style.display = 'none';", CommentInputTemplate.Holder_StyleID);
//            }
//            if (this.inputInfo.IsSuccessReload)
//            {
//                additionalScript += string.Format(@"
//			setTimeout('window.location.reload(false)', 2000);");
//            }
//            additionalScript += string.Format(@"
//			try{{{0};}}catch(e){{try{{{1};}}catch(e){{}}}}", StlTemplateManager.Comments.GetDynamicUpdateScriptName(), StlTemplateManager.Comments.GetDynamicUpdateScriptName2());
//            builder.AppendFormat(@"
//function stlCommentCallback_{0}(jsonString){{
//	var obj = eval('(' + jsonString + ')');
//	if (obj){{
//		document.getElementById('commentSuccess_{0}').style.display = 'none';
//		document.getElementById('commentFailure_{0}').style.display = 'none';
//		if (obj.isSuccess == 'false'){{
//			document.getElementById('commentFailure_{0}').style.display = '';
//			document.getElementById('commentFailure_{0}').innerHTML = obj.message;
//		}}else{{
//			document.getElementById('commentSuccess_{0}').style.display = '';
//			document.getElementById('commentSuccess_{0}').innerHTML = obj.message;{1}
//		}}
//	}}
//}}
//function stlCommentGetQuery_{0}(type, defaultValue)
//{{
//  var re = eval(""/"" + type + ""=([^&]*)/"");   
//  return (re.test(window.location.search)) ? RegExp.$1 : defaultValue;
//}}
//function stlCommentSubmit_{0}(channelID, contentID)
//{{
//    if (checkFormValueById('frmComment_{0}'))
//    {{
//        channelID = stlCommentGetQuery_{0}('channelID', channelID);
//        contentID = stlCommentGetQuery_{0}('contentID', contentID);
//        document.getElementById('frmComment_{0}').action = document.getElementById('frmComment_{0}').action + '&channelID=' + channelID + '&contentID=' + contentID;
//        document.getElementById('frmComment_{0}').submit();
//    }}
//    return false;
//}}", CommentInputTemplate.Holder_StyleID, additionalScript);

//            return builder.ToString();
//        }

//        public string GetContent()
//        {
//            StringBuilder builder = new StringBuilder();

////            builder.AppendFormat(@"
////<div id=""commentSuccess_{0}"" class=""is_success"" style=""display:none""></div>
////<div id=""commentFailure_{0}"" class=""is_failure"" style=""display:none""></div>
////<div id=""commentContainer_{0}"">", CommentInputTemplate.Holder_StyleID);

//            NameValueCollection pageScripts = new NameValueCollection();
//            string attributesHtml = this.Render(pageScripts);

//            builder.Append(@"
//<table cellSpacing=""2"" cellPadding=""4"" border=""0"" width=""98%"">");

//            //builder.AppendFormat(@"<form id=""frmComment_{0}"" method=""post"" enctype=""multipart/form-data"" action=""{1}"" target=""loadComment_{0}"">", CommentInputTemplate.Holder_StyleID, CommentInputTemplate.Holder_ActionUrl);

//            //string clickString = string.Format(@"stlCommentSubmit_{0}('{{@ChannelID}}', '{{@ContentID}}');", CommentInputTemplate.Holder_StyleID);

//            builder.AppendFormat(@"{0}<tr><td>&nbsp;</td><td><input id=""submit"" type=""button"" value=""发表评论"" /></td></tr>
//", attributesHtml);

////            builder.AppendFormat(@"
////</form>
////<iframe id=""loadComment_{0}"" name=""loadComment_{0}"" width=""0"" height=""0"" frameborder=""0""></iframe>", CommentInputTemplate.Holder_StyleID);

//            builder.Append(@"</table></div>");

//            return builder.ToString();
//        }

//        public string ReplacePlaceHolder(string template, bool isValidateCode, int channelID, int contentID, string successTemplateString, string failureTemplateString)
//        {
//            StringBuilder parsedContent = new StringBuilder();

//            parsedContent.AppendFormat(@"
//<div id=""commentSuccess_{0}"" class=""is_success"" style=""display:none""></div>
//<div id=""commentFailure_{0}"" class=""is_failure"" style=""display:none""></div>
//<div id=""commentContainer_{0}"">", this.tagStyleInfo.StyleID);

//            string actionUrl = PageUtility.Services.GetActionUrlOfCommentInput(publishmentSystemInfo, tagStyleInfo.StyleID);

//            parsedContent.AppendFormat(@"<form id=""frmComment_{0}"" method=""post"" enctype=""multipart/form-data"" action=""{1}"" target=""loadComment_{0}"">", tagStyleInfo.StyleID, actionUrl);

//            if (!string.IsNullOrEmpty(successTemplateString))
//            {
//                parsedContent.AppendFormat(@"<input type=""hidden"" id=""successTemplateString"" value=""{0}"" />", RuntimeUtils.EncryptStringByTranslate(successTemplateString));
//            }
//            if (!string.IsNullOrEmpty(failureTemplateString))
//            {
//                parsedContent.AppendFormat(@"<input type=""hidden"" id=""failureTemplateString"" value=""{0}"" />", RuntimeUtils.EncryptStringByTranslate(failureTemplateString));
//            }

//            parsedContent.Append(template);

//            parsedContent.AppendFormat(@"
//</form>
//<iframe id=""loadComment_{0}"" name=""loadComment_{0}"" width=""0"" height=""0"" frameborder=""0""></iframe>", tagStyleInfo.StyleID);

//            NameValueCollection pageScripts = new NameValueCollection();
//            this.Render(pageScripts);

//            foreach (string key in pageScripts.Keys)
//            {
//                parsedContent.Append(pageScripts[key]);
//            }

//            //replace

//            if (isValidateCode)
//            {
//                bool isCrossDomain = PageUtility.IsCrossDomain(publishmentSystemInfo.PublishmentSystemUrl);
//                ValidateCodeManager vcManager = ValidateCodeManager.GetInstance(publishmentSystemInfo.PublishmentSystemID, tagStyleInfo.StyleID, isCrossDomain);
//                base.ReWriteCheckCode(parsedContent, publishmentSystemInfo, vcManager, isCrossDomain);
//            }

//            string clickString = string.Format(@"stlCommentSubmit_{0}('{1}', '{2}');", tagStyleInfo.StyleID, channelID, contentID);

//            parsedContent.Replace(@"<input id=""submit""", string.Format(@"<input id=""submit_{0}"" onclick=""{1}""", tagStyleInfo.StyleID, clickString));

//            ArrayList stlFormElements = StlHtmlUtility.GetStlFormElementsArrayList(parsedContent.ToString());
//            if (stlFormElements != null && stlFormElements.Count > 0)
//            {
//                foreach (string stlFormElement in stlFormElements)
//                {
//                    XmlNode elementNode;
//                    NameValueCollection attributes;
//                    StlHtmlUtility.ReWriteFormElements(stlFormElement, out elementNode, out attributes);

//                    string validateAttributes = string.Empty;
//                    string validateHtmlString = string.Empty;

//                    if (!string.IsNullOrEmpty(attributes["id"]))
//                    {
//                        foreach (TableStyleInfo styleInfo in styleInfoArrayList)
//                        {
//                            if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, attributes["id"]))
//                            {
//                                validateHtmlString = InputTypeParser.GetValidateHtmlString(styleInfo, out validateAttributes);
//                                attributes["id"] = styleInfo.AttributeName;
//                                break;
//                            }
//                        }
//                    }

//                    if (StringUtils.EqualsIgnoreCase(elementNode.Name, "input"))
//                    {
//                        parsedContent.Replace(stlFormElement, string.Format(@"<{0} {1} {2}/>{3}", elementNode.Name, TranslateUtils.ToAttributesString(attributes), validateAttributes, validateHtmlString));
//                    }
//                    else
//                    {
//                        parsedContent.Replace(stlFormElement, string.Format(@"<{0} {1} {2}>{3}</{0}>{4}", elementNode.Name, TranslateUtils.ToAttributesString(attributes), validateAttributes, elementNode.InnerXml, validateHtmlString));
//                    }
//                }
//            }

//            parsedContent = parsedContent.Replace(CommentInputTemplate.Holder_StyleID, tagStyleInfo.StyleID.ToString());

//            return parsedContent.ToString();
//        }

//        public static string GetInputCallbackScript(PublishmentSystemInfo publishmentSystemInfo, int styleID, bool isSuccess, string message)
//        {
//            NameValueCollection jsonAttributes = new NameValueCollection();
//            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
//            jsonAttributes.Add("message", message);

//            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
//            jsonString = StringUtils.ToJsString(jsonString);

//            if (PageUtility.IsCrossDomain(publishmentSystemInfo.PublishmentSystemUrl))
//            {
//                string script = string.Format("<script>window.parent.parent.stlCommentCallback_{0}('{1}');</script>", styleID, jsonString);
//                string proxyUrl = PageUtility.GetProxyUrl(publishmentSystemInfo, script);
//                return string.Format(@"<script>document.write(""<iframe src='{0}' style='display:none'></iframe>"");</script>", proxyUrl);
//            }
//            else
//            {
//                return string.Format("<script>window.parent.stlCommentCallback_{0}('{1}');</script>", styleID, jsonString);
//            }
//        }
	}
}
