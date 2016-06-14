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
using SiteServer.CMS.Controls;
using System.Text.RegularExpressions;

namespace SiteServer.STL.StlTemplate
{
    public class WebsiteMessageTemplate : InputTemplateBase
    {
        private PublishmentSystemInfo publishmentSystemInfo;
        private ArrayList styleInfoArrayList;
        private WebsiteMessageInfo websiteMessageInfo;

        public const string Holder_WebsiteMessageID = "{WebsiteMessageID}";

        public WebsiteMessageTemplate(PublishmentSystemInfo publishmentSystemInfo, WebsiteMessageInfo websiteMessageInfo)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, publishmentSystemInfo.PublishmentSystemID, websiteMessageInfo.WebsiteMessageID);
            this.styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, relatedIdentities);
            this.websiteMessageInfo = websiteMessageInfo;
        }

        public string GetTemplate(bool isTemplate, bool isLoadValues, string websiteMessageTemplateString, string classifyTemplateString, string successTemplateString, string failureTemplateString)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(@"<script type=""text/javascript"" charset=""{0}"" src=""{1}""></script>", SiteFiles.Validate.Charset, PageUtility.GetSiteFilesUrl(this.publishmentSystemInfo, SiteFiles.Validate.Js));

            if (string.IsNullOrEmpty(websiteMessageTemplateString))
            {
                if (isTemplate)
                {
                    if (!string.IsNullOrEmpty(this.websiteMessageInfo.StyleTemplate))
                    {
                        builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.websiteMessageInfo.StyleTemplate);
                    }
                    if (!string.IsNullOrEmpty(this.websiteMessageInfo.ScriptTemplate))
                    {
                        builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.websiteMessageInfo.ScriptTemplate);
                    }

                    builder.Append(this.websiteMessageInfo.ContentTemplate);
                }
                else
                {
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", base.GetStyle(ETableStyle.WebsiteMessageContent));
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());

                    builder.Append(this.GetContent());
                }
            }
            else
            {
                if (isTemplate)
                {
                    if (!string.IsNullOrEmpty(this.websiteMessageInfo.StyleTemplate))
                    {
                        builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.websiteMessageInfo.StyleTemplate);
                    }
                    if (!string.IsNullOrEmpty(this.websiteMessageInfo.ScriptTemplate))
                    {
                        builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.websiteMessageInfo.ScriptTemplate);
                    }
                }
                else
                {
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", base.GetStyle(ETableStyle.WebsiteMessageContent));
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());
                }

                //需要替换反馈类型
                var parentClassifyList = DataProvider.WebsiteMessageClassifyDAO.GetItemInfoArrayListByParentID(publishmentSystemInfo.PublishmentSystemID, 0);
                string classifyString = string.Empty;
                if (parentClassifyList.Count > 0)
                {
                    TreeBaseItem parentInfo = parentClassifyList[0] as TreeBaseItem;
                    var classifyList = DataProvider.WebsiteMessageClassifyDAO.GetItemInfoArrayListByParentID(publishmentSystemInfo.PublishmentSystemID, parentInfo.ItemID);
                    if (classifyList.Count > 0)
                    {
                        int index = 0;
                        foreach (TreeBaseItem classifyInfo in classifyList)
                        {
                            if (classifyList.Count == 1 && classifyInfo.ItemIndexName == "Default")
                                continue;
                            classifyString += classifyTemplateString.Replace("{classify.itemindex}", index.ToString())
                                .Replace("{classify.itemid}", classifyInfo.ItemID.ToString())
                                .Replace("{classify.itemname}", classifyInfo.ItemName);
                            index++;
                        }
                    }
                }
                string classifyRegexStr = "<stl:WebstieMessageClassifyTemplate[\\s.\\S]*</stl:WebstieMessageClassifyTemplate>";
                RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase);
                Regex reg = new Regex(classifyRegexStr, options);
                websiteMessageTemplateString = reg.Replace(websiteMessageTemplateString, classifyString);

                builder.Append(websiteMessageTemplateString);
            }

            bool isValidateCode = this.websiteMessageInfo.Additional.IsValidateCode;
            if (isValidateCode)
            {
                isValidateCode = FileConfigManager.Instance.IsValidateCode;
            }

            return this.ReplacePlaceHolder(builder.ToString(), isValidateCode, isLoadValues, successTemplateString, failureTemplateString);
        }

        public string GetScript()
        {
            StringBuilder builder = new StringBuilder();

            string additionalScript = string.Empty;
            if (this.websiteMessageInfo.Additional.IsSuccessHide)
            {
                additionalScript += string.Format(@"
			document.getElementById('websiteMessageContainer_{0}').style.display = 'none';", WebsiteMessageTemplate.Holder_WebsiteMessageID);
            }
            if (this.websiteMessageInfo.Additional.IsSuccessReload)
            {
                additionalScript += string.Format(@"
			setTimeout('window.location.reload(false)', 2000);");
            }
            builder.AppendFormat(@"
function stlWebsiteMessageCallback_{0}(jsonString){{
    closeModal();
	var obj = eval('(' + jsonString + ')');
	if (obj){{
		document.getElementById('websiteMessageSuccess_{0}').style.display = 'none';
		document.getElementById('websiteMessageFailure_{0}').style.display = 'none';
		if (obj.isSuccess == 'false'){{
			document.getElementById('websiteMessageFailure_{0}').style.display = '';
			document.getElementById('websiteMessageFailure_{0}').innerHTML = obj.message;
		}}else{{
			document.getElementById('websiteMessageSuccess_{0}').style.display = '';
			document.getElementById('websiteMessageSuccess_{0}').innerHTML = obj.message;{1}
		}}
	}}
}}
", WebsiteMessageTemplate.Holder_WebsiteMessageID, additionalScript);

            builder.Append(@"
jQuery(document).ready(function($) {{
	$('.questions').click(function(event) {{
		 event.stopPropagation();  
		 $('.tiwen_fc').toggle();
	}});
	$('.close').click(function(){{
		$('.tiwen_fc').hide();
	}});
	$('.leixing li').click(function(){{
		$(this).addClass('current').siblings().removeClass('current');
	}});
}});
");
            return builder.ToString();
        }

        public string GetContent()
        {
            StringBuilder builder = new StringBuilder();

            //NameValueCollection pageScripts = new NameValueCollection();
            //string attributesHtml = base.GetAttributesHtml(pageScripts, publishmentSystemInfo, this.websiteMessageInfo.Additional.IsValidateCode, styleInfoArrayList);

            //StringBuilder scripts = new StringBuilder();
            //foreach (string key in pageScripts.Keys)
            //{
            //    scripts.Append(pageScripts[key]);
            //}

            //builder.Append(scripts.ToString());

            //            if (websiteMessageInfo.Additional.IsLoginForm)
            //            {
            //                builder.AppendFormat(@"
            //<table cellSpacing=""2"" cellPadding=""4"" border=""0"" width=""98%"">
            //<tr><td>{0}</td></tr>
            //</table>", LoginTemplate.GetStlLoginEmbedded());
            //            }

            builder.Append(@"<!-- 提问浮层 -->
        <div class='tiwen_fc'>
            <div class='tiwen_bg'></div>
            <div class='tiwen_fc_c'>
                <span class='close'></span>
");
            #region 表单字段
            //表单
            NameValueCollection pageScripts = new NameValueCollection();
            string attributesHtml = GetAttributesHtml(pageScripts, publishmentSystemInfo, this.websiteMessageInfo.Additional.IsValidateCode, styleInfoArrayList);
            builder.Append(attributesHtml.Replace(@"style=""width:380px;""", ""));
            #endregion
            builder.Append(@"
                <div class='leixing'>
                    <ul>");
            #region 添加反馈类型
            var parentClassifyList = DataProvider.WebsiteMessageClassifyDAO.GetItemInfoArrayListByParentID(publishmentSystemInfo.PublishmentSystemID, 0);
            if (parentClassifyList.Count > 0)
            {
                TreeBaseItem parentInfo = parentClassifyList[0] as TreeBaseItem;
                var classifyList = DataProvider.WebsiteMessageClassifyDAO.GetItemInfoArrayListByParentID(publishmentSystemInfo.PublishmentSystemID, parentInfo.ItemID);
                if (classifyList.Count > 0)
                {
                    int index = 0;
                    foreach (TreeBaseItem classifyInfo in classifyList)
                    {
                        if (classifyList.Count == 1 && classifyInfo.ItemIndexName == "Default")
                            continue;
                        if (index == 0)
                        {
                            builder.AppendFormat(@"
                        <li class='current'><input id='WebsiteMessageClassifyID_{0}' type='radio' checked='checked' name='WebsiteMessageClassifyID' value='{1}' /><label for='WebsiteMessageClassifyID_{0}'>{2}</label></li>
", index, classifyInfo.ItemID, classifyInfo.ItemName);
                        }
                        else
                        {
                            builder.AppendFormat(@"
                        <li class=''><input id='WebsiteMessageClassifyID_{0}' type='radio' name='WebsiteMessageClassifyID' value='{1}' /><label for='WebsiteMessageClassifyID_{0}'>{2}</label></li>
", index, classifyInfo.ItemID, classifyInfo.ItemName);
                        }
                        index++;
                    }
                }
            }
            #endregion
            builder.Append(@"
                        </ul>
                    <input id='submit' type='submit' value='提交'>
                </div>
            </div>
        </div>");

            return builder.ToString();
        }

        protected new string GetAttributesHtml(NameValueCollection pageScripts, PublishmentSystemInfo publishmentSystemInfo, bool isValidateCode, ArrayList styleInfoArrayList)
        {
            StringBuilder output = new StringBuilder();

            if (isValidateCode)
            {
                isValidateCode = FileConfigManager.Instance.IsValidateCode;
            }

            if (styleInfoArrayList != null)
            {
                foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                {
                    if (styleInfo.IsVisible == false) continue;

                    string helpHtml = styleInfo.DisplayName + (styleInfo.Additional.IsRequired ? "必填" : string.Empty) + ":";
                    NameValueCollection formCollection = new NameValueCollection();
                    formCollection[styleInfo.AttributeName] = string.Empty;
                    string inputHtml = InputTypeParser.Parse(publishmentSystemInfo, 0, styleInfo, ETableStyle.InputContent, styleInfo.AttributeName, formCollection, false, false, this.GetInnerAdditionalAttributes(styleInfo), pageScripts, false, false);
                    output.AppendFormat(@"
                <div class='leixing'>
                    <p>{0}</p>
                    {1}
                </div>
", helpHtml, inputHtml.Replace(@"style=""width:380px;""", @""));
                }

                if (isValidateCode)
                {
                    TableStyleInfo styleInfo = new TableStyleInfo();
                    styleInfo.Additional.IsValidate = false;
                    styleInfo.AttributeName = ValidateCodeManager.AttributeName;
                    styleInfo.DisplayName = "验证码";
                    styleInfo.Additional.Width = "50px";
                    string inputHtml = InputTypeParser.ParseText(publishmentSystemInfo, 0, styleInfo.AttributeName, null, true, this.GetInnerAdditionalAttributes(styleInfo), styleInfo, ETableStyle.InputContent, false);

                    output.AppendFormat(@"
                <div class='leixing'>
                    <p>验证码:</p>
                    {0}
                </div>
", inputHtml.Replace(@"style=""width:50px;""", @"style=""width:135px;"""));
                }
            }
            return output.ToString();
        }

        public string ReplacePlaceHolder(string template, bool isValidateCode, bool isLoadValues, string successTemplateString, string failureTemplateString)
        {
            StringBuilder parsedContent = new StringBuilder();

            parsedContent.AppendFormat(@"
<div id=""websiteMessageSuccess_{0}"" class=""is_success"" style=""display:none""></div>
<div id=""websiteMessageFailure_{0}"" class=""is_failure"" style=""display:none""></div>
<div id=""websiteMessageContainer_{0}"">", this.websiteMessageInfo.WebsiteMessageID);

            //添加遮罩层
            parsedContent.AppendFormat(@"	
<div id=""websiteMessageModal_{0}"" times=""2"" id=""xubox_shade2"" class=""xubox_shade"" style=""z-index:19891016; background-color: #FFF; opacity: 0.5; filter:alpha(opacity=10);top: 0;left: 0;width: 100%;height: 100%;position: fixed;display:none;""></div>
<div id=""websiteMessageModalMsg_{0}"" times=""2"" showtime=""0"" style=""z-index: 19891016; left: 50%; top: 206px; width: 500px; height: 360px; margin-left: -250px;position: fixed;text-align: center;display:none;"" id=""xubox_layer2"" class=""xubox_layer"" type=""iframe""><img src = ""/sitefiles/bairong/icons/waiting.gif"" style="""">
<br>
<span style=""font-size:10px;font-family:Microsoft Yahei"">正在提交...</span>
</div>
<script>
		function openModal()
        {{
			document.getElementById(""websiteMessageModal_{0}"").style.display = '';
            document.getElementById(""websiteMessageModalMsg_{0}"").style.display = '';
        }}
        function closeModal()
        {{
			document.getElementById(""websiteMessageModal_{0}"").style.display = 'none';
            document.getElementById(""websiteMessageModalMsg_{0}"").style.display = 'none';
        }}
</script>", this.websiteMessageInfo.WebsiteMessageID);

            //web api sessionliang
            string actionUrl = PageUtility.Services.GetActionUrlOfWebsiteMessage(publishmentSystemInfo, this.websiteMessageInfo.WebsiteMessageID);

            parsedContent.AppendFormat(@"
<form id=""frmWebsiteMessage_{0}"" name=""frmWebsiteMessage_{0}"" style=""margin:0;padding:0"" method=""post"" enctype=""multipart/form-data"" action=""{1}"" target=""loadWebsiteMessage_{0}"">
", this.websiteMessageInfo.WebsiteMessageID, actionUrl);

            if (!string.IsNullOrEmpty(successTemplateString))
            {
                parsedContent.AppendFormat(@"<input type=""hidden"" id=""successTemplateString"" value=""{0}"" />", RuntimeUtils.EncryptStringByTranslate(successTemplateString));
            }
            if (!string.IsNullOrEmpty(failureTemplateString))
            {
                parsedContent.AppendFormat(@"<input type=""hidden"" id=""failureTemplateString"" value=""{0}"" />", RuntimeUtils.EncryptStringByTranslate(failureTemplateString));
            }

            parsedContent.Append(template);

            parsedContent.AppendFormat(@"
</form>
<iframe id=""loadWebsiteMessage_{0}"" name=""loadWebsiteMessage_{0}"" width=""0"" height=""0"" frameborder=""0""></iframe>
</div>", this.websiteMessageInfo.WebsiteMessageID);

            NameValueCollection pageScripts = new NameValueCollection();
            base.GetAttributesHtml(pageScripts, publishmentSystemInfo, this.websiteMessageInfo.Additional.IsValidateCode, styleInfoArrayList);

            foreach (string key in pageScripts.Keys)
            {
                parsedContent.Append(pageScripts[key]);
            }

            //replace
            if (isValidateCode)
            {
                bool isCrossDomain = PageUtility.IsCrossDomain(publishmentSystemInfo);
                bool IsCorsCross = PageUtility.IsCorsCrossDomain(publishmentSystemInfo);
                ValidateCodeManager vcManager = ValidateCodeManager.GetInstance(publishmentSystemInfo.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID, isCrossDomain);
                base.ReWriteCheckCode(parsedContent, publishmentSystemInfo, vcManager, isCrossDomain, IsCorsCross);
            }

            if (websiteMessageInfo.Additional.IsCtrlEnter)
            {
                parsedContent.AppendFormat(@"
<script>document.body.onkeydown=function(e)
{{e=e?e:window.event;var tagname=e.srcElement?e.srcElement.tagName:e.target.tagName;if(tagname=='INPUT'||tagname=='TEXTAREA'){{if(e!=null&&e.ctrlKey&&e.keyCode==13){{document.getElementById('submit_{0}').click();}}}}}}</script>", websiteMessageInfo.WebsiteMessageID);
            }
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                //web api sessionliang
                parsedContent.AppendFormat(@"
<script type=""text/javascript"" src=""{1}""></script>
<script type=""text/javascript"">
    function wsm_clickFun_{0}(){{
        if (checkFormValueById('frmWebsiteMessage_{0}')){{
            openModal();
            if(window.FormData !== undefined){{
            $.ajax({{
                url: $(""#frmWebsiteMessage_{0}"").attr(""action""),
                type: ""POST"",
                mimeType:""multipart/form-data"",
                contentType: false,
                processData: false,
                cache: false,
                xhrFields: {{   
                    withCredentials: true   
                }},
                data: new FormData($(""#frmWebsiteMessage_{0}"")[0]), //$(""#frmWebsiteMessage_{0}"").serialize(),
                success: function(json, textStatus, jqXHR){{
                    execFun(json);
                }}
            }});
            }}
            else{{
                //generate a random id
                var  iframeId = 'unique' + (new Date().getTime());
                //create an empty iframe
                var iframe = $('<iframe src=""javascript:false;"" name=""'+iframeId+'"" />');
                //hide it
                iframe.hide();
                //set form target to iframe
                formObj.attr('target',iframeId);
                //Add iframe to body
                iframe.appendTo('body');
                iframe.load(function(e){{
                    var doc = getDoc(iframe[0]);
                    var docRoot = doc.body ? doc.body : doc.documentElement;
                    var data = docRoot.innerHTML;
                    //data is returned from server.
                        execFun(data);
                }});
            }}
            return false;
        }}
    }}
    
    function execFun(json){{
        if(!!json){{
            if(typeof(json) == ""string"")
                json = eval(""(""+json+"")"");
            if(!!json.scriptString){{
                //eval(""(""+json.scriptString+"")"");
                $(json.scriptString).appendTo(document.body);
            }}
        }}
    }}
</script>
", WebsiteMessageTemplate.Holder_WebsiteMessageID, PageUtils.GetSiteFilesUrl("bairong/jquery/jquery.form.js"));
            }

            string clickString = string.Empty;
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                //web api sofuny 20151201
                clickString = string.Format(@"wsm_clickFun_{0}();return false;", WebsiteMessageTemplate.Holder_WebsiteMessageID);
            }
            else
            {
                clickString = string.Format(@"if (checkFormValueById('frmWebsiteMessage_{0}')){{openModal(); document.getElementById('frmWebsiteMessage_{0}').submit();}}", WebsiteMessageTemplate.Holder_WebsiteMessageID);
            }

            StlHtmlUtility.ReWriteSubmitButton(parsedContent, clickString);

            ArrayList stlFormElements = StlHtmlUtility.GetStlFormElementsArrayList(parsedContent.ToString());
            if (stlFormElements != null && stlFormElements.Count > 0)
            {
                foreach (string stlFormElement in stlFormElements)
                {
                    XmlNode elementNode;
                    NameValueCollection attributes;
                    StlHtmlUtility.ReWriteFormElements(stlFormElement, out elementNode, out attributes);

                    string validateAttributes = string.Empty;
                    string validateHtmlString = string.Empty;

                    if (!string.IsNullOrEmpty(attributes["id"]))
                    {
                        foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                        {
                            if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, attributes["id"]))
                            {
                                validateHtmlString = InputTypeParser.GetValidateHtmlString(styleInfo, out validateAttributes);
                                attributes["id"] = styleInfo.AttributeName;
                            }
                        }
                    }

                    if (StringUtils.EqualsIgnoreCase(elementNode.Name, "websiteMessage"))
                    {
                        parsedContent.Replace(stlFormElement, string.Format(@"<{0} {1} {2}/>{3}", elementNode.Name, TranslateUtils.ToAttributesString(attributes), validateAttributes, validateHtmlString));
                    }
                    else
                    {
                        parsedContent.Replace(stlFormElement, string.Format(@"<{0} {1} {2}>{3}</{0}>{4}", elementNode.Name, TranslateUtils.ToAttributesString(attributes), validateAttributes, elementNode.InnerXml, validateHtmlString));
                    }
                }
            }

            parsedContent.Replace(WebsiteMessageTemplate.Holder_WebsiteMessageID, this.websiteMessageInfo.WebsiteMessageID.ToString());

            if (isLoadValues)
            {
                parsedContent.AppendFormat(@"
<script type=""text/javascript"">stlWebsiteMessageLoadValues('frmWebsiteMessage_{0}');</script>
", websiteMessageInfo.WebsiteMessageID);
            }

            return parsedContent.ToString();
        }

        public static string GetWebsiteMessageCallbackScript(PublishmentSystemInfo publishmentSystemInfo, int websiteMessageID, bool isSuccess, string message)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();
            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
            jsonAttributes.Add("message", message);

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);

            if (PageUtility.IsAgentCrossDomain(publishmentSystemInfo))
            {
                string script = string.Format("<script>window.parent.parent.stlWebsiteMessageCallback_{0}('{1}');</script>", websiteMessageID, jsonString);
                string proxyUrl = PageUtility.GetProxyUrl(publishmentSystemInfo, script);
                return string.Format(@"<script>document.write(""<iframe src='{0}' style='display:none'></iframe>"");</script>", proxyUrl);
            }
            else if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                //web api sessionliang
                return string.Format("<script>window.stlWebsiteMessageCallback_{0}('{1}');</script>", websiteMessageID, jsonString);
            }
            else
            {
                return string.Format("<script>window.parent.stlWebsiteMessageCallback_{0}('{1}');</script>", websiteMessageID, jsonString);
            }
        }

        #region 列表
        public string GetListContentTemplate(bool isTemplate, WebsiteMessageInfo websiteMessageInfo, ArrayList relatedIdentities)
        {
            StringBuilder builder = new StringBuilder();
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, relatedIdentities);

            if (isTemplate)
            {
                //自定义模板
                builder.AppendFormat(@"<style type=""text/css"">{0}</style>", websiteMessageInfo.StyleTemplateList);
                builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", websiteMessageInfo.ScriptTemplateList);
                builder.Append(websiteMessageInfo.ContentTemplateList);
            }
            else
            {
                //默认模板
                builder.Append(this.GetListContent(relatedIdentities));
            }

            return builder.ToString();
        }


        public string GetListContent(ArrayList relatedIdentities)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.GetListStyle());
            builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetListScript());
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, relatedIdentities);

            builder.Append(@"<ul>
<stl:websiteMessageContents>
<li>");
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                builder.AppendFormat(@"
<span>{0}:</span><span><stl:websiteMessageContent type=""{1}""></stl:websiteMessageContent></span>
", styleInfo.DisplayName, styleInfo.AttributeName);
            }
            builder.AppendFormat(@"
<a href=""websiteMessageDetail.html?id={{stl:websiteMessageContent type='id'}}"">查看回复</a>
");
            builder.Append(@"</li>
</stl:websiteMessageContents>
</ul>");
            return builder.ToString();
        }

        public string GetListStyle()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"");
            return sb.ToString();
        }

        public string GetListScript()
        {
            StringBuilder builder = new StringBuilder();
            //            builder.Append(@"function showWSMReply(element){
            //    if(element){
            //        $(element).next(""div"").show();
            //    }
            //}");
            return builder.ToString();
        }
        #endregion

        #region 详情
        public string GetDetailContentTemplate(bool isTemplate, WebsiteMessageInfo websiteMessageInfo, ArrayList relatedIdentities)
        {
            StringBuilder builder = new StringBuilder();
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, relatedIdentities);

            if (isTemplate)
            {
                //自定义模板
                builder.AppendFormat(@"<style type=""text/css"">{0}</style>", websiteMessageInfo.StyleTemplateDetail);
                builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", websiteMessageInfo.ScriptTemplateDetail);
                builder.Append(websiteMessageInfo.ContentTemplateDetail);
            }
            else
            {
                //默认模板
                builder.Append(this.GetDetailContent(relatedIdentities));

            }

            return builder.ToString();
        }

        public string GetDetailContent(ArrayList relatedIdentities)
        {
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, relatedIdentities);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.GetDetailStyle());
            builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetDetailScript());
            //默认模板
            builder.Append(@"
<table>");
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                builder.AppendFormat(@"
<tr><td>{0}:</td><td><stl:websiteMessageContent type=""{1}""></stl:websiteMessageContent></td></tr>
", styleInfo.DisplayName, styleInfo.AttributeName);
            }
            builder.Append(@"
<tr><td>提交时间：</td><td><stl:websiteMessageContent type=""adddate""></stl:websiteMessageContent></td></tr>
");
            builder.Append(@"
<tr><td>回复：</td><td><stl:websiteMessageContent type=""reply""></stl:websiteMessageContent></td></tr>
");
            builder.Append(@"</table>");
            return builder.ToString();
        }

        public string GetDetailStyle()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"");
            return sb.ToString();
        }

        public string GetDetailScript()
        {
            return string.Empty;
        }
        #endregion

        #region 邮箱
        public string GetEmailTitleReply()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"网站留言【回复】");
            return sb.ToString();
        }

        public string GetEmailContentReply(ArrayList styleInfoArrayList)
        {
            return MessageManager.GetMailContentWebsiteMessageReply(styleInfoArrayList);
        }
        #endregion

        #region 短信
        public string GetSMSTitleReply()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"网站留言【回复】");
            return sb.ToString();
        }

        public string GetSMSContentReply(ArrayList styleInfoArrayList)
        {
            return MessageManager.GetSMSContentWebsiteMessageReply(styleInfoArrayList);
        }
        #endregion
    }
}
