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
    public class ContentInputTemplate : InputTemplateBase
    {
        private PublishmentSystemInfo publishmentSystemInfo;
        private ArrayList styleInfoArrayList;
        private TagStyleContentInputInfo inputInfo;
        private TagStyleInfo tagStyleInfo;

        public const string Holder_StyleID = "{StyleID}";

        public ContentInputTemplate(PublishmentSystemInfo publishmentSystemInfo, TagStyleInfo tagStyleInfo, TagStyleContentInputInfo inputInfo)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(this.publishmentSystemInfo.PublishmentSystemID, inputInfo.ChannelID);

            this.styleInfoArrayList = new ArrayList();
            TableStyleInfo styleInfo = null;

            if (inputInfo.IsChannel)
            {
                styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, ContentAttribute.NodeID, relatedIdentities);
                styleInfo.InputType = EInputType.SelectOne;
                styleInfo.DisplayName = "栏目";
                styleInfoArrayList.Add(styleInfo);
            }

            styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, ContentAttribute.Title, relatedIdentities);
            styleInfoArrayList.Add(styleInfo);

            styleInfoArrayList.Add(TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.SubTitle, relatedIdentities));

            styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.ImageUrl, relatedIdentities);
            styleInfo.InputType = EInputType.Image;
            styleInfoArrayList.Add(styleInfo);

            styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.VideoUrl, relatedIdentities);
            styleInfo.InputType = EInputType.Video;
            styleInfoArrayList.Add(styleInfo);

            styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.FileUrl, relatedIdentities);
            styleInfo.InputType = EInputType.File;
            styleInfoArrayList.Add(styleInfo);

            TableStyleInfo styleInfoIsRecommend = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.IsRecommend, relatedIdentities);
            TableStyleInfo styleInfoIsHot = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.IsHot, relatedIdentities);
            TableStyleInfo styleInfoIsColor = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.IsColor, relatedIdentities);
            TableStyleInfo styleInfoIsTop = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, ContentAttribute.IsTop, relatedIdentities);
            styleInfoArrayList.Add(styleInfoIsRecommend);
            styleInfoArrayList.Add(styleInfoIsHot);
            styleInfoArrayList.Add(styleInfoIsColor);
            styleInfoArrayList.Add(styleInfoIsTop);

            styleInfoArrayList.Add(TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.LinkUrl, relatedIdentities));

            styleInfoArrayList.Add(TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.Content, relatedIdentities));

            styleInfoArrayList.Add(TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.Author, relatedIdentities));

            styleInfoArrayList.Add(TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.Source, relatedIdentities));

            styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, BackgroundContentAttribute.Summary, relatedIdentities);
            styleInfo.InputType = EInputType.TextArea;
            styleInfoArrayList.Add(styleInfo);

            ArrayList attributeNameArrayList = new ArrayList();
            foreach (TableStyleInfo styleInfo2 in styleInfoArrayList)
            {
                attributeNameArrayList.Add(styleInfo2.AttributeName.ToLower());
            }

            ArrayList theStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, relatedIdentities);
            foreach (TableStyleInfo theStyleInfo in theStyleInfoArrayList)
            {
                if (!BackgroundContentAttribute.SystemAttributes.Contains(theStyleInfo.AttributeName.ToLower()) && !attributeNameArrayList.Contains(theStyleInfo.AttributeName.ToLower()))
                {
                    styleInfoArrayList.Add(theStyleInfo);
                }
            }

            this.tagStyleInfo = tagStyleInfo;
            this.inputInfo = inputInfo;
        }

        private string Render(NameValueCollection pageScripts)
        {
            StringBuilder output = new StringBuilder();

            bool isValidateCode = this.inputInfo.IsValidateCode;
            if (isValidateCode)
            {
                isValidateCode = FileConfigManager.Instance.IsValidateCode;
            }

            TableStyleInfo styleInfoIsRecommend = null;
            TableStyleInfo styleInfoIsHot = null;
            TableStyleInfo styleInfoIsColor = null;
            TableStyleInfo styleInfoIsTop = null;
            foreach (TableStyleInfo styleInfo in this.styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false)
                {
                    continue;
                }

                if (styleInfo.AttributeName == BackgroundContentAttribute.IsRecommend || styleInfo.AttributeName == BackgroundContentAttribute.IsColor || styleInfo.AttributeName == BackgroundContentAttribute.IsHot || styleInfo.AttributeName == ContentAttribute.IsTop)
                {
                    if (styleInfo.AttributeName == BackgroundContentAttribute.IsRecommend)
                    {
                        styleInfoIsRecommend = styleInfo;
                    }
                    else if (styleInfo.AttributeName == BackgroundContentAttribute.IsHot)
                    {
                        styleInfoIsHot = styleInfo;
                    }
                    else if (styleInfo.AttributeName == BackgroundContentAttribute.IsColor)
                    {
                        styleInfoIsColor = styleInfo;
                    }
                    else if (styleInfo.AttributeName == ContentAttribute.IsTop)
                    {
                        styleInfoIsTop = styleInfo;
                    }

                    if (styleInfoIsRecommend != null && styleInfoIsHot != null && styleInfoIsColor != null && styleInfoIsTop != null)
                    {
                        if (styleInfoIsRecommend.IsVisible && styleInfoIsHot.IsVisible && styleInfoIsColor.IsVisible && styleInfoIsTop.IsVisible)
                        {
                            string inputHtml = string.Empty;
                            if (styleInfoIsRecommend.IsVisible)
                            {
                                inputHtml += string.Format(@"<input id=""{0}"" type=""checkbox"" name=""{0}"" /><label for=""{0}"">{1}</label>", styleInfoIsRecommend.AttributeName, styleInfoIsRecommend.DisplayName);
                            }
                            if (styleInfoIsHot.IsVisible)
                            {
                                inputHtml += string.Format(@"<input id=""{0}"" type=""checkbox"" name=""{0}"" /><label for=""{0}"">{1}</label>", styleInfoIsHot.AttributeName, styleInfoIsHot.DisplayName);
                            }
                            if (styleInfoIsColor.IsVisible)
                            {
                                inputHtml += string.Format(@"<input id=""{0}"" type=""checkbox"" name=""{0}"" /><label for=""{0}"">{1}</label>", styleInfoIsColor.AttributeName, styleInfoIsColor.DisplayName);
                            }
                            if (styleInfoIsTop.IsVisible)
                            {
                                inputHtml += string.Format(@"<input id=""{0}"" type=""checkbox"" name=""{0}"" /><label for=""{0}"">{1}</label>", styleInfoIsTop.AttributeName, styleInfoIsTop.DisplayName);
                            }
                            output.AppendFormat(@"
<tr height=""30""><td>显示属性:</td><td>{0}</td></tr>
", inputHtml);
                        }
                    }
                }
                else
                {
                    this.GetAttributeHtml(styleInfo, pageScripts, output);
                }
            }

            if (isValidateCode)
            {
                TableStyleInfo styleInfo = new TableStyleInfo();
                styleInfo.Additional.IsValidate = false;
                styleInfo.AttributeName = ValidateCodeManager.AttributeName;
                styleInfo.DisplayName = "验证码";
                styleInfo.Additional.Width = "50px";
                string inputHtml = InputTypeParser.ParseText(publishmentSystemInfo, inputInfo.ChannelID, ValidateCodeManager.AttributeName, null, true, base.GetInnerAdditionalAttributes(styleInfo), styleInfo, ETableStyle.BackgroundContent, false);

                output.AppendFormat(@"
<tr><td height=""30"">验证码:</td><td>{0}</td></tr>
", inputHtml);
            }

            return output.ToString();
        }

        private void GetAttributeHtml(TableStyleInfo styleInfo, NameValueCollection pageScripts, StringBuilder output)
        {
            NameValueCollection formCollection = new NameValueCollection();
            formCollection[styleInfo.AttributeName] = string.Empty;
            string helpHtml = styleInfo.DisplayName + ":";

            string inputHtml = InputTypeParser.Parse(publishmentSystemInfo, 0, styleInfo, ETableStyle.BackgroundContent, styleInfo.AttributeName, formCollection, false, false, base.GetInnerAdditionalAttributes(styleInfo), pageScripts, false, false);

            output.AppendFormat(@"
<tr><td width=""70"" valign=""top""><nobr>{0}</nobr></td><td>{1}</td></tr>
", helpHtml, inputHtml);
        }

        public string GetTemplate(bool isTemplate, int channelID, string inputTemplateString, string successTemplateString, string failureTemplateString)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(@"<script type=""text/javascript"" charset=""{0}"" src=""{1}""></script>", SiteFiles.Validate.Charset, PageUtility.GetSiteFilesUrl(this.publishmentSystemInfo, SiteFiles.Validate.Js));

            if (string.IsNullOrEmpty(inputTemplateString))
            {
                if (isTemplate)
                {
                    if (!string.IsNullOrEmpty(this.tagStyleInfo.StyleTemplate))
                    {
                        builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.tagStyleInfo.StyleTemplate);
                    }
                    if (!string.IsNullOrEmpty(this.tagStyleInfo.ScriptTemplate))
                    {
                        builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.tagStyleInfo.ScriptTemplate);
                    }

                    builder.Append(this.tagStyleInfo.ContentTemplate);
                }
                else
                {
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.GetStyle(ETableStyle.BackgroundContent));
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());

                    builder.Append(this.GetContent());
                }
            }
            else
            {
                if (isTemplate)
                {
                    if (!string.IsNullOrEmpty(this.tagStyleInfo.StyleTemplate))
                    {
                        builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.tagStyleInfo.StyleTemplate);
                    }
                    if (!string.IsNullOrEmpty(this.tagStyleInfo.ScriptTemplate))
                    {
                        builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.tagStyleInfo.ScriptTemplate);
                    }
                }
                else
                {
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.GetStyle(ETableStyle.BackgroundContent));
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());
                }
                builder.Append(inputTemplateString);
            }

            bool isValidateCode = this.inputInfo.IsValidateCode;
            if (isValidateCode)
            {
                isValidateCode = FileConfigManager.Instance.IsValidateCode;
            }

            //if (isValidateCode)
            //{
            //    bool isCrossDomain = PageUtility.IsCrossDomain(publishmentSystemInfo.PublishmentSystemUrl);
            //    ValidateCodeManager vcManager = ValidateCodeManager.GetInstance(publishmentSystemInfo.PublishmentSystemID, tagStyleInfo.StyleID, isCrossDomain);

            //    builder.Replace(@"<img id=""validateImage"" ", string.Format(@"<img id=""validateImage"" src=""{0}"" ", vcManager.GetImageUrl(false)));
            //}

            return this.ReplacePlaceHolder(builder.ToString(), isValidateCode, channelID, successTemplateString, failureTemplateString);
        }

        public string GetScript()
        {
            StringBuilder builder = new StringBuilder();

            string additionalScript = string.Empty;
            if (this.inputInfo.IsSuccessHide)
            {
                additionalScript += string.Format(@"
			document.getElementById('contentContainer_{0}').style.display = 'none';", ContentInputTemplate.Holder_StyleID);
            }
            if (this.inputInfo.IsSuccessReload)
            {
                additionalScript += string.Format(@"
			setTimeout('window.location.reload(false)', 2000);");
            }
            builder.AppendFormat(@"
function stlContentCallback_{0}(jsonString){{
	var obj = eval('(' + jsonString + ')');
	if (obj){{
		document.getElementById('contentSuccess_{0}').style.display = 'none';
		document.getElementById('contentFailure_{0}').style.display = 'none';
		if (obj.isSuccess == 'false'){{
			document.getElementById('contentFailure_{0}').style.display = '';
			document.getElementById('contentFailure_{0}').innerHTML = obj.message;
		}}else{{
			document.getElementById('contentSuccess_{0}').style.display = '';
			document.getElementById('contentSuccess_{0}').innerHTML = obj.message;{1}
		}}
	}}
}}", ContentInputTemplate.Holder_StyleID, additionalScript);

            return builder.ToString();
        }

        public string GetContent()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(@"
<table cellSpacing=""2"" cellPadding=""4"" border=""0"" width=""98%"">");

            NameValueCollection pageScripts = new NameValueCollection();
            string attributesHtml = this.Render(pageScripts);

            builder.Append(attributesHtml);

            builder.AppendFormat(@"<tr height=""30""><td>&nbsp;</td><td><input id=""submit"" type=""button"" value="" 提 交 "" />&nbsp;&nbsp;&nbsp;<input type=""reset"" value="" 重 置 "" />&nbsp;&nbsp;{0}</td></tr>
", this.inputInfo.IsCtrlEnter ? "[Ctrl+Enter]" : string.Empty);

            builder.Append("</table>");

            return builder.ToString();
        }

        public string ReplacePlaceHolder(string template, bool isValidateCode, int channelID, string successTemplateString, string failureTemplateString)
        {
            StringBuilder parsedContent = new StringBuilder();

            parsedContent.AppendFormat(@"
<div id=""contentSuccess_{0}"" class=""is_success"" style=""display:none""></div>
<div id=""contentFailure_{0}"" class=""is_failure"" style=""display:none""></div>
<div id=""contentContainer_{0}"">", tagStyleInfo.StyleID);
            //添加遮罩层
            parsedContent.AppendFormat(@"	
<div id=""inputModal_{0}"" times=""2"" id=""xubox_shade2"" class=""xubox_shade"" style=""z-index:19891016; background-color: #FFF; opacity: 0.5; filter:alpha(opacity=10);top: 0;left: 0;width: 100%;height: 100%;position: fixed;display:none;""></div>
<div id=""inputModalMsg_{0}"" times=""2"" showtime=""0"" style=""z-index: 19891016; left: 50%; top: 206px; width: 500px; height: 360px; margin-left: -250px;position: fixed;text-align: center;display:none;"" id=""xubox_layer2"" class=""xubox_layer"" type=""iframe""><img src = ""/sitefiles/bairong/icons/waiting.gif"" style="""">
<br>
<span style=""font-size:10px;font-family:Microsoft Yahei"">正在提交...</span>
</div>
<script>
		function openModal()
        {{
			document.getElementById(""inputModal_{0}"").style.display = '';
            document.getElementById(""inputModalMsg_{0}"").style.display = '';
        }}
        function closeModal()
        {{
			document.getElementById(""inputModal_{0}"").style.display = 'none';
            document.getElementById(""inputModalMsg_{0}"").style.display = 'none';
        }}
</script>", tagStyleInfo.StyleID);
            string actionUrl = PageUtility.Services.GetActionUrlOfContentInput(publishmentSystemInfo, tagStyleInfo.StyleID, channelID);

            parsedContent.AppendFormat(@"
<form id=""frmContent_{0}"" method=""post"" enctype=""multipart/form-data"" action=""{1}"" accept-charset=""utf-8"" target=""loadContent_{0}"">", tagStyleInfo.StyleID, actionUrl);

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
<iframe id=""loadContent_{0}"" name=""loadContent_{0}"" width=""0"" height=""0"" frameborder=""0""></iframe>
</div>", tagStyleInfo.StyleID);

            NameValueCollection pageScripts = new NameValueCollection();
            this.Render(pageScripts);

            foreach (string key in pageScripts.Keys)
            {
                parsedContent.Append(pageScripts[key]);
            }

            //replace

            if (isValidateCode)
            {
                bool isCrossDomain = PageUtility.IsCrossDomain(publishmentSystemInfo);
                bool IsCorsCross = PageUtility.IsCorsCrossDomain(publishmentSystemInfo);
                ValidateCodeManager vcManager = ValidateCodeManager.GetInstance(publishmentSystemInfo.PublishmentSystemID, tagStyleInfo.StyleID, isCrossDomain);
                base.ReWriteCheckCode(parsedContent, publishmentSystemInfo, vcManager, isCrossDomain, IsCorsCross);
            }
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                //web api sofuny
                parsedContent.AppendFormat(@"
<script type=""text/javascript"" src=""{1}""></script>
<script type=""text/javascript"">
    function contentInput_clickFun_{0}(){{
        if (checkFormValueById('frmContent_{0}')){{
            openModal();
            if(window.FormData !== undefined){{
            $.ajax({{
                url: $(""#frmContent_{0}"").attr(""action""),
                type: ""POST"",
                mimeType:""multipart/form-data"",
                contentType: false,
                processData: false,
                cache: false,
                xhrFields: {{   
                    withCredentials: true   
                }},
                data: new FormData($(""#frmContent_{0}"")[0]), //$(""#frmContent_{0}"").serialize(),
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
", tagStyleInfo.StyleID, PageUtils.GetSiteFilesUrl("bairong/jquery/jquery.form.js"));
            }
            string clickString = string.Empty;
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                //web api sessionliang 20151201
                clickString = string.Format(@"contentInput_clickFun_{0}();return false;", tagStyleInfo.StyleID);
            }
            else
            {
                clickString = string.Format(@"if (checkFormValueById('frmContent_{0}')){{document.getElementById('frmContent_{0}').submit();}}", tagStyleInfo.StyleID);
            }

            parsedContent.Replace(@"<input id=""submit""", string.Format(@"<input id=""submit_{0}"" onclick=""{1}""", tagStyleInfo.StyleID, clickString));

            if (this.inputInfo.IsCtrlEnter)
            {
                parsedContent.AppendFormat(@"
<script>document.body.onkeydown=function(e)
{{e=e?e:window.event;var tagname=e.srcElement?e.srcElement.tagName:e.target.tagName;if(tagname=='INPUT'||tagname=='TEXTAREA'){{if(e!=null&&e.ctrlKey&&e.keyCode==13){{document.getElementById('submit_{0}').click();}}}}}}</script>", tagStyleInfo.StyleID);
            }

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
                                break;
                            }
                        }
                    }


                    if (StringUtils.EqualsIgnoreCase(elementNode.Name, "input"))
                    {
                        parsedContent.Replace(stlFormElement, string.Format(@"<{0} {1} {2}/>{3}", elementNode.Name, TranslateUtils.ToAttributesString(attributes), validateAttributes, validateHtmlString));
                    }
                    else if (StringUtils.EqualsIgnoreCase(elementNode.Name, "select") && StringUtils.EqualsIgnoreCase(attributes["id"], ContentAttribute.NodeID) && string.IsNullOrEmpty(elementNode.InnerXml))
                    {
                        StringBuilder itemBuilder = new StringBuilder();
                        ArrayList arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
                        int nodeCount = arraylist.Count;
                        bool[] isLastNodeArray = new bool[nodeCount];
                        foreach (int nodeID in arraylist)
                        {
                            bool enabled = true;

                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                            if (enabled)
                            {
                                if (nodeInfo.Additional.IsContentAddable == false) enabled = false;
                            }

                            if (!enabled)
                            {
                                continue;
                            }

                            if (nodeID == channelID)
                            {
                                itemBuilder.AppendFormat(@"<option value=""{0}"" selected=""selected"">{1}</option>", nodeID, NodeManager.GetSelectText(publishmentSystemInfo, nodeInfo, isLastNodeArray, false, false));
                            }
                            else
                            {
                                itemBuilder.AppendFormat(@"<option value=""{0}"">{1}</option>", nodeID, NodeManager.GetSelectText(publishmentSystemInfo, nodeInfo, isLastNodeArray, false, false));
                            }
                        }

                        parsedContent.Replace(stlFormElement, string.Format(@"<{0} {1}>{2}</{0}>", elementNode.Name, TranslateUtils.ToAttributesString(attributes), itemBuilder.ToString()));
                    }
                    else
                    {
                        parsedContent.Replace(stlFormElement, string.Format(@"<{0} {1} {2}>{3}</{0}>{4}", elementNode.Name, TranslateUtils.ToAttributesString(attributes), validateAttributes, elementNode.InnerXml, validateHtmlString));
                    }
                }
            }

            parsedContent = parsedContent.Replace(ContentInputTemplate.Holder_StyleID, tagStyleInfo.StyleID.ToString());

            return parsedContent.ToString();
        }

        public static string GetInputCallbackScript(PublishmentSystemInfo publishmentSystemInfo, int styleID, bool isSuccess, string message)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();
            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
            jsonAttributes.Add("message", message);

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);

            if (PageUtility.IsAgentCrossDomain(publishmentSystemInfo))
            {
                string script = string.Format("<script>window.parent.parent.stlContentCallback_{0}('{1}');</script>", styleID, jsonString);
                string proxyUrl = PageUtility.GetProxyUrl(publishmentSystemInfo, script);
                return string.Format(@"<script>document.write(""<iframe src='{0}' style='display:none'></iframe>"");</script>", proxyUrl);
            }
            else if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                //web api sessionliang 20151201
                return string.Format("<script>window.stlContentCallback_{0}('{1}');</script>", styleID, jsonString);
            }
            else
            {
                return string.Format("<script>window.parent.stlContentCallback_{0}('{1}');</script>", styleID, jsonString);
            }
        }
    }
}
