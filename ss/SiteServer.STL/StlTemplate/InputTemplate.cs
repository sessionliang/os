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
    public class InputTemplate : InputTemplateBase
    {
        private PublishmentSystemInfo publishmentSystemInfo;
        private ArrayList styleInfoArrayList;
        private InputInfo inputInfo;

        public const string Holder_InputID = "{InputID}";

        public InputTemplate(PublishmentSystemInfo publishmentSystemInfo, InputInfo inputInfo)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, publishmentSystemInfo.PublishmentSystemID, inputInfo.InputID);
            this.styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, relatedIdentities);
            this.inputInfo = inputInfo;
        }

        public string GetTemplate(bool isTemplate, bool isLoadValues, string inputTemplateString, string successTemplateString, string failureTemplateString)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(@"<script type=""text/javascript"" charset=""{0}"" src=""{1}""></script>", SiteFiles.Validate.Charset, PageUtility.GetSiteFilesUrl(this.publishmentSystemInfo, SiteFiles.Validate.Js));

            if (string.IsNullOrEmpty(inputTemplateString))
            {
                if (isTemplate)
                {
                    if (!string.IsNullOrEmpty(this.inputInfo.StyleTemplate))
                    {
                        builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.inputInfo.StyleTemplate);
                    }
                    if (!string.IsNullOrEmpty(this.inputInfo.ScriptTemplate))
                    {
                        builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.inputInfo.ScriptTemplate);
                    }

                    builder.Append(this.inputInfo.ContentTemplate);
                }
                else
                {
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", base.GetStyle(ETableStyle.InputContent));
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());

                    builder.Append(this.GetContent());
                }
            }
            else
            {
                if (isTemplate)
                {
                    if (!string.IsNullOrEmpty(this.inputInfo.StyleTemplate))
                    {
                        builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.inputInfo.StyleTemplate);
                    }
                    if (!string.IsNullOrEmpty(this.inputInfo.ScriptTemplate))
                    {
                        builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.inputInfo.ScriptTemplate);
                    }
                }
                else
                {
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", base.GetStyle(ETableStyle.InputContent));
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript());
                }
                builder.Append(inputTemplateString);
            }

            bool isValidateCode = this.inputInfo.Additional.IsValidateCode;
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
            if (this.inputInfo.Additional.IsSuccessHide)
            {
                additionalScript += string.Format(@"
			document.getElementById('inputContainer_{0}').style.display = 'none';", InputTemplate.Holder_InputID);
            }
            if (this.inputInfo.Additional.IsSuccessReload)
            {
                additionalScript += string.Format(@"
			setTimeout('window.location.reload(false)', 2000);");
            }
            builder.AppendFormat(@"
function stlInputCallback_{0}(jsonString){{
    closeModal();
	var obj = eval('(' + jsonString + ')');
	if (obj){{
		document.getElementById('inputSuccess_{0}').style.display = 'none';
		document.getElementById('inputFailure_{0}').style.display = 'none';
		if (obj.isSuccess == 'false'){{
			document.getElementById('inputFailure_{0}').style.display = '';
			document.getElementById('inputFailure_{0}').innerHTML = obj.message;
		}}else{{
			document.getElementById('inputSuccess_{0}').style.display = '';
			document.getElementById('inputSuccess_{0}').innerHTML = obj.message;{1}
		}}
	}}
}}
", InputTemplate.Holder_InputID, additionalScript);

            return builder.ToString();
        }

        public string GetContent()
        {
            StringBuilder builder = new StringBuilder();

            //NameValueCollection pageScripts = new NameValueCollection();
            //string attributesHtml = base.GetAttributesHtml(pageScripts, publishmentSystemInfo, this.inputInfo.Additional.IsValidateCode, styleInfoArrayList);

            //StringBuilder scripts = new StringBuilder();
            //foreach (string key in pageScripts.Keys)
            //{
            //    scripts.Append(pageScripts[key]);
            //}

            //builder.Append(scripts.ToString());

            //            if (inputInfo.Additional.IsLoginForm)
            //            {
            //                builder.AppendFormat(@"
            //<table cellSpacing=""2"" cellPadding=""4"" border=""0"" width=""98%"">
            //<tr><td>{0}</td></tr>
            //</table>", LoginTemplate.GetStlLoginEmbedded());
            //            }

            builder.Append(@"
<table cellSpacing=""3"" cellPadding=""3"" border=""0"" width=""98%"">");

            NameValueCollection pageScripts = new NameValueCollection();
            string attributesHtml = base.GetAttributesHtml(pageScripts, publishmentSystemInfo, this.inputInfo.Additional.IsValidateCode, styleInfoArrayList);

            builder.Append(attributesHtml);

            builder.AppendFormat(@"
<tr><td>&nbsp;</td><td><input id=""submit"" type=""button"" class=""is_btn"" value="" 提 交 "" />&nbsp;&nbsp;&nbsp;<input type=""reset"" class=""is_btn"" value="" 重 置 "" />&nbsp;&nbsp;{0}</td></tr>
</table>
", inputInfo.Additional.IsCtrlEnter ? "[Ctrl+Enter]" : string.Empty);

            return builder.ToString();
        }

        public string ReplacePlaceHolder(string template, bool isValidateCode, bool isLoadValues, string successTemplateString, string failureTemplateString)
        {
            StringBuilder parsedContent = new StringBuilder();

            parsedContent.AppendFormat(@"
<div id=""inputSuccess_{0}"" class=""is_success"" style=""display:none""></div>
<div id=""inputFailure_{0}"" class=""is_failure"" style=""display:none""></div>
<div id=""inputContainer_{0}"">", this.inputInfo.InputID);

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
</script>", this.inputInfo.InputID);

            string actionUrl = PageUtility.Services.GetActionUrlOfInput(publishmentSystemInfo, this.inputInfo.InputID);
            parsedContent.AppendFormat(@"
<form id=""frmInput_{0}"" name=""frmInput_{0}"" style=""margin:0;padding:0"" method=""post"" enctype=""multipart/form-data"" action=""{1}"" target=""loadInput_{0}"">
", this.inputInfo.InputID, actionUrl);

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
<iframe id=""loadInput_{0}"" name=""loadInput_{0}"" width=""0"" height=""0"" frameborder=""0""></iframe>
</div>", this.inputInfo.InputID);

            NameValueCollection pageScripts = new NameValueCollection();
            base.GetAttributesHtml(pageScripts, publishmentSystemInfo, this.inputInfo.Additional.IsValidateCode, styleInfoArrayList);

            foreach (string key in pageScripts.Keys)
            {
                parsedContent.Append(pageScripts[key]);
            }

            //replace
            if (isValidateCode)
            {
                bool isCrossDomain = PageUtility.IsCrossDomain(publishmentSystemInfo);
                bool IsCorsCross = PageUtility.IsCorsCrossDomain(publishmentSystemInfo);
                ValidateCodeManager vcManager = ValidateCodeManager.GetInstance(publishmentSystemInfo.PublishmentSystemID, this.inputInfo.InputID, isCrossDomain);
                base.ReWriteCheckCode(parsedContent, publishmentSystemInfo, vcManager, isCrossDomain, IsCorsCross);
            }

            if (inputInfo.Additional.IsCtrlEnter)
            {
                parsedContent.AppendFormat(@"
<script>document.body.onkeydown=function(e)
{{e=e?e:window.event;var tagname=e.srcElement?e.srcElement.tagName:e.target.tagName;if(tagname=='INPUT'||tagname=='TEXTAREA'){{if(e!=null&&e.ctrlKey&&e.keyCode==13){{document.getElementById('submit_{0}').click();}}}}}}</script>", inputInfo.InputID);
            }
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                //web api sofuny
                parsedContent.AppendFormat(@"
<script type=""text/javascript"" src=""{1}""></script>
<script type=""text/javascript"">
    function input_clickFun_{0}(){{
        if (checkFormValueById('frmInput_{0}')){{
            openModal();

            $.ajax({{
                url: $(""#frmInput_{0}"").attr(""action""),
                type: ""POST"",
                mimeType:""multipart/form-data"",
                contentType: false,
                processData: false,
                cache: false,
                xhrFields: {{   
                    withCredentials: true   
                }},
                data: new FormData($(""#frmInput_{0}"")[0]), //$(""#frmInput_{0}"").serialize(),
                success: function(json, textStatus, jqXHR){{
                    execFun(json);
                }}
            }});
            
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
", InputTemplate.Holder_InputID, PageUtils.GetSiteFilesUrl("bairong/jquery/jquery.form.js"));
            }
            string clickString = string.Empty;
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                //web api sofuny 20151201
                clickString = string.Format(@"input_clickFun_{0}();return false;", InputTemplate.Holder_InputID);
            }
            else
            {
                clickString = string.Format(@"if (checkFormValueById('frmInput_{0}')){{openModal(); document.getElementById('frmInput_{0}').submit();}}", InputTemplate.Holder_InputID);
            }
            //parsedContent.Replace(@"<input id=""submit""", string.Format(@"<input id=""submit_{0}"" onclick=""{1}""", inputInfo.InputID, clickString));

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

                    if (StringUtils.EqualsIgnoreCase(elementNode.Name, "input"))
                    {
                        parsedContent.Replace(stlFormElement, string.Format(@"<{0} {1} {2}/>{3}", elementNode.Name, TranslateUtils.ToAttributesString(attributes), validateAttributes, validateHtmlString));
                    }
                    else
                    {
                        parsedContent.Replace(stlFormElement, string.Format(@"<{0} {1} {2}>{3}</{0}>{4}", elementNode.Name, TranslateUtils.ToAttributesString(attributes), validateAttributes, elementNode.InnerXml, validateHtmlString));
                    }
                }
            }

            parsedContent.Replace(InputTemplate.Holder_InputID, this.inputInfo.InputID.ToString());

            if (isLoadValues)
            {
                parsedContent.AppendFormat(@"
<script type=""text/javascript"">stlInputLoadValues('frmInput_{0}');</script>
", inputInfo.InputID);
            }

            return parsedContent.ToString();
        }

        public static string GetInputCallbackScript(PublishmentSystemInfo publishmentSystemInfo, int inputID, bool isSuccess, string message)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();
            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
            jsonAttributes.Add("message", message);

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);
            if (PageUtility.IsAgentCrossDomain(publishmentSystemInfo))
            {
                string script = string.Format("<script>window.parent.parent.stlInputCallback_{0}('{1}');</script>", inputID, jsonString);
                string proxyUrl = PageUtility.GetProxyUrl(publishmentSystemInfo, script);
                return string.Format(@"<script>document.write(""<iframe src='{0}' style='display:none'></iframe>"");</script>", proxyUrl);
            }
            else if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                //web api sofuny 20151201
                return string.Format("<script>window.stlInputCallback_{0}('{1}');</script>", inputID, jsonString);
            }
            else
            {
                return string.Format("<script>window.parent.stlInputCallback_{0}('{1}');</script>", inputID, jsonString);
            }
        }
    }
}
