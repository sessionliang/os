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

using System.Web.UI.WebControls;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Core;
using SiteServer.CMS.Services;
using System.Xml;
using System.Text.RegularExpressions;

namespace SiteServer.STL.StlTemplate
{
    /// <summary>
    /// 订阅内容模板
    /// </summary>
    public class SubscribeTemplate
    {
        private PublishmentSystemInfo publishmentSystemInfo;
        private SubscribeSetInfo settingInfo;
        private EInputType subscribeStyle;
        private string formID;

        public SubscribeTemplate(PublishmentSystemInfo publishmentSystemInfo, SubscribeSetInfo settingInfo, EInputType subscribeStyle)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.settingInfo = settingInfo;
            this.formID = string.Format("subscribeForm_{0}", StringUtils.GetRandomInt(1, 100000));
            this.subscribeStyle = subscribeStyle;
        }

        public string GetTemplate(bool isTemplate, string subscribeTemplateString, EInputType subscribeStyle)
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(subscribeTemplateString))
            {
                if (isTemplate)
                {
                    //自定义模板
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.settingInfo.StyleTemplate);
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.settingInfo.ScriptTemplate.Replace("{formID}", this.formID));
                    builder.Append(this.settingInfo.ContentTemplate.Replace("{formID}", this.formID));
                }
                else
                {
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.GetStyle());
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript(subscribeStyle).Replace("{formID}", this.formID));
                    builder.Append(this.GetContent(subscribeStyle).Replace("{formID}", this.formID));
                }
            }
            else
            {
                if (isTemplate)
                {
                    if (!string.IsNullOrEmpty(this.settingInfo.StyleTemplate))
                    {
                        builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.settingInfo.StyleTemplate);
                    }
                    if (!string.IsNullOrEmpty(this.settingInfo.ScriptTemplate))
                    {
                        builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.settingInfo.ScriptTemplate);
                    }
                }
                else
                {
                    builder.AppendFormat(@"<style type=""text/css"">{0}</style>", this.GetStyle());
                    builder.AppendFormat(@"<script type=""text/javascript"">{0}</script>", this.GetScript(subscribeStyle).Replace("{formID}", this.formID));
                }

                builder.Append(subscribeTemplateString);
            }
            //return builder.ToString();

            return this.ReplacePlaceHolder(ParseContentToHtml(builder.ToString()), false, "", "");
        }

        public string GetContent(EInputType subscribeStyle)
        {
            StringBuilder builder = new StringBuilder();

            string actionUrl = PageUtility.Services.GetActionUrlOfSubscribe(publishmentSystemInfo);

            //            builder.AppendFormat(@"
            //<form id=""{{formID}}"" method=""get"" action=""{1}"" onSubmit=""return false;"" >", this.formID, actionUrl);

            builder.Append(@"
<table>");

            if (EInputTypeUtils.Equals(EInputType.SelectOne, subscribeStyle))
            {
                builder.Append(@"<tr><td>
订阅内容：&nbsp;<select style=""border: #ccc 1px solid;width:120;"" id=""subscribeSel"" name=""SubscribeID""  ></select>&nbsp;");
            }
            else if (EInputTypeUtils.Equals(EInputType.CheckBox, subscribeStyle))
            {
                builder.AppendFormat(@"<tr><td><input type=""hidden"" id=""SubscribeID"" name=""SubscribeID"" value="""" />");
                builder.Append(@"
订阅内容：&nbsp;<div id=""subscribeSel""></div> </td><td></td><tr><td>");
            }
            builder.Append(@"
邮箱：&nbsp;<input style=""border: #ccc 1px solid;width:120;"" id=""Email"" name=""Email"" />&nbsp;");

            // if (SMSStatus)
            builder.Append(@"
手机号：&nbsp;<input style=""border: #ccc 1px solid;width:120;"" id=""Mobile"" name=""Mobile"" />&nbsp;");

            builder.AppendFormat(@"</td>
<td><input type=""button"" onclick=""submitCode();"" value="" 确 定 "" />
</td></tr></table>", this.formID);

            return builder.ToString();
        }

        private string ParseContentToHtml(string content)
        {
            string clickString = string.Empty;
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                clickString = string.Format(@"subscribe_clickFun();return false;");
            }
            else
            {
                clickString = string.Format(@"submitCode()");
            }
            return content.Replace("{formID}", this.FormID)
                                  .Replace("{clickStr}", clickString);
        }

        public string FormID
        {
            get { return this.formID; }
        }

        public bool SMSStatus
        {
            get
            {
                int totalCount = 0;
                string message = "";
                return SMSManager.GetTotalCount(out totalCount, out message);
            }
        }
        internal string GetStyle()
        {
            StringBuilder builder = new StringBuilder();
            return builder.ToString();
        }

        internal string GetScript(EInputType subscribeStyle)
        {
            StringBuilder builder = new StringBuilder();

            string getUrlString = PageUtility.Services.GetUrlOfSubscribe(publishmentSystemInfo);

            builder.AppendFormat(@"
function stlSubscribeCallback(jsonString){{ 
	var obj = eval('(' + jsonString + ')');
	if (obj){{  
        alert(obj.message);  
	}}
}}
");

            if (EInputTypeUtils.Equals(EInputType.SelectOne, subscribeStyle))
            {
                builder.AppendFormat(@"
jQuery(document).ready(function($) {{
      $.ajax({{
          cache: true,
          type: ""POST"",
          url: ""{0}"",
          async: false,
          error: function (request) {{
          }},
          success: function (data) {{ 
              if(typeof(data) == ""string"")
                data = eval(""(""+data+"")"");
              if (data.isSuccess) {{
                  var inputStr="""";
                  $.each(data.subscribe, function (key, item) {{ 
                    inputStr=inputStr+""<option value="" + item.itemID + "">"" + item.itemName + ""</option>""
                  }});
                  $(""#subscribeSel"").html(inputStr); 
                  $(""#Email"").val(data.currentEmail);
                  $(""#Mobile"").val(data.currentMobile);
              }}
          }}
      }}); 
  }}); 
", getUrlString);

                builder.AppendFormat(@" 
function submitCode(){{
  if($(""#Email"").val()=="""")
  {{
      alert(""请输入邮箱"");
      return;
  }}
  if($(""#SubscribeID"").val()==""""){{
      alert(""请选择订阅内容"");
      return;
  }}   
    {0}
   document.getElementById('{{formID}}').submit(); 
    $(""#Email"").val("""");
    {1}
}}", getMstr(), getMnullstr());
            }
            else if (EInputTypeUtils.Equals(EInputType.CheckBox, subscribeStyle))
            {
                builder.AppendFormat(@"
jQuery(document).ready(function($) {{
      $.ajax({{
          cache: true,
          type: ""POST"",
          url: ""{0}"",
          async: false,
          error: function (request) {{
          }},
          success: function (data) {{  
              if(typeof(data) == ""string"")
                data = eval(""(""+data+"")"");
              if (data.isSuccess) {{
                  var inputStr="""";
                  $.each(data.subscribe, function (key, item) {{ 
                    inputStr=inputStr+""<input id='"" + item.itemID + ""' type='checkbox' name='IDsSub' value='"" + item.itemID + ""' />"" + item.itemName + "" ""
                  }});
                  $(""#subscribeSel"").html(inputStr); 
                  $(""#Email"").val(data.currentEmail);
                  $(""#Mobile"").val(data.currentMobile);
              }}
          }}
      }}); 
  }}); 
", getUrlString);


                builder.AppendFormat(@" 
function getCheck(){{
  var checkID="""";
  $('input[name=""IDsSub""]:checked').each(function(){{
    checkID=checkID+$(this).val()+"",""; 
  }}); 
  if(checkID.length>0)
    checkID= checkID.substring(0,checkID.length-1); 
  return checkID;
  }}");

                builder.AppendFormat(@" 
function submitCode(){{
      if($(""#Email"").val()=="""")
      {{
          alert(""请输入邮箱"");
          return;
      }}
      if(getCheck()==""""){{  
          alert(""请选择订阅内容"");
          return;
      }} 
    var myreg = /^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){{1,63}}[a-z0-9]+$/;
     if (!myreg.test($(""#Email"").val())) {{
      alert(""请输入正确的邮箱"");
      return;
    }}
     {0}
    $(""#SubscribeID"").val(getCheck()); 
     document.getElementById('{{formID}}').submit(); 
    $(""#Email"").val("""");
    {1}
}}", getMstr(), getMnullstr());

            }
            return builder.ToString();
        }

        public string getMstr()
        {
            // if (SMSStatus)
            return @"
    var msreg = /^1[0-9]{10}$/;
    if($(""#Mobile"").length>0&&$(""#Mobile"").val()!="""")
     if (!msreg.test($(""#Mobile"").val())) {{
      alert(""请输入正确的手机号"");
      return;
    }}";
            // else
            // return "";
        }

        public string getMnullstr()
        {
            //if (SMSStatus)
            return @"
    $(""#Mobile"").val("""");";
            // else
            //return "";
        }

        public string ReplacePlaceHolder(string template, bool isValidateCode, string successTemplateString, string failureTemplateString)
        {
            StringBuilder parsedContent = new StringBuilder();

            parsedContent.AppendFormat(@"
<div id=""subscribeSuccess"" class=""is_success"" style=""display:none""></div>
<div id=""subscribeFailure"" class=""is_failure"" style=""display:none""></div>
<div id=""subscribeContainer"">");

            //添加遮罩层
            parsedContent.AppendFormat(@"	
<div id=""subscribeModal"" times=""2"" id=""xubox_shade2"" class=""xubox_shade"" style=""z-index:19891016; background-color: #FFF; opacity: 0.5; filter:alpha(opacity=10);top: 0;left: 0;width: 100%;height: 100%;position: fixed;display:none;""></div>
<div id=""subscribeModalMsg"" times=""2"" showtime=""0"" style=""z-index: 19891016; left: 50%; top: 206px; width: 500px; height: 360px; margin-left: -250px;position: fixed;text-align: center;display:none;"" id=""xubox_layer2"" class=""xubox_layer"" type=""iframe""><img src = ""/sitefiles/bairong/icons/waiting.gif"" style="""">
<br>
<span style=""font-size:10px;font-family:Microsoft Yahei"">正在提交...</span>
</div>
<script>
		function openModal()
        {{
			document.getElementById(""subscribeModal"").style.display = '';
            document.getElementById(""subscribeModalMsg"").style.display = '';
        }}
        function closeModal()
        {{
			document.getElementById(""subscribeModal"").style.display = 'none';
            document.getElementById(""subscribeModalMsg"").style.display = 'none';
        }}
</script>");

            string actionUrl = PageUtility.Services.GetActionUrlOfSubscribe(publishmentSystemInfo);

            parsedContent.AppendFormat(@"
<form id=""{1}"" name=""{1}"" style=""margin:0;padding:0"" method=""post"" enctype=""multipart/form-data"" action=""{0}"" target=""loadSubscribe"">
", actionUrl, this.formID);

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
<iframe id=""loadSubscribe"" name=""loadSubscribe"" width=""0"" height=""0"" frameborder=""0""></iframe>
</div>");

            //replace
            if (isValidateCode)
            {
                bool isCrossDomain = PageUtility.IsCrossDomain(publishmentSystemInfo);
                ValidateCodeManager vcManager = ValidateCodeManager.GetInstance(publishmentSystemInfo.PublishmentSystemID, 0, isCrossDomain);
                ReWriteCheckCode(parsedContent, publishmentSystemInfo, vcManager, isCrossDomain);
            }
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                parsedContent.AppendFormat(@"
<script type=""text/javascript"" src=""{0}""></script>
<script type=""text/javascript"">
    function getCheck(){{
        var checkID="""";
        if($(""input[name='IDsSub']"").length > 0){{
        $('input[name=""IDsSub""]:checked').each(function(){{
                checkID=checkID+$(this).val()+"",""; 
        }}); 
        if(checkID.length>0)
                checkID= checkID.substring(0,checkID.length-1); 
        }}
        else if($(""#SubscribeID"")){{
            checkID = $(""#SubscribeID"").find(""option:selected"").val();
        }}
        return checkID;
    }}
    function subscribe_clickFun(){{
        if($(""#Email"").val()=="""")
      {{
          alert(""请输入邮箱"");
          return;
      }}
      if(getCheck()==""""){{  
          alert(""请选择订阅内容"");
          return;
      }} 
    var myreg = /^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){{1,63}}[a-z0-9]+$/;
     if (!myreg.test($(""#Email"").val())) {{
      alert(""请输入正确的邮箱"");
      return;
    }}
     {1}
            if(window.FormData !== undefined){{
            $.ajax({{
                url: $(""#{3}"").attr(""action""),
                type: ""POST"",
                mimeType:""multipart/form-data"",
                contentType: false,
                processData: false,
                cache: false,
                xhrFields: {{   
                    withCredentials: true   
                }},
                data: new FormData($(""#{3}"")[0]), //$(""#{3}"").serialize(),
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

    function execFun(json){{
        if(!!json){{
            if(typeof(json) == ""string"")
                json = eval(""(""+json+"")""); 
            if (json){{  
                    alert(json.message);  
	            }} 
            if (json.isSuccess){{  
                    $(""#Email"").val("""");
                    {2}
	            }} 
        }}
    }}
</script>
", PageUtils.GetSiteFilesUrl("bairong/jquery/jquery.form.js"), getMstr(), getMnullstr(), this.FormID);
            }
            string clickString = string.Empty;
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                clickString = string.Format(@"subscribe_clickFun();return false;");
            }
            else
            {
                clickString = string.Format(@"submitCode()");
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
                }

            }

            return parsedContent.ToString();
        }


        public void ReWriteCheckCode(StringBuilder builder, PublishmentSystemInfo publishmentSystemInfo, VCManager vcManager, bool isCrossDomain)
        {
            RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase);
            string element = null;

            string imageUrl = string.Empty;

            TableStyleInfo styleInfo = new TableStyleInfo();
            styleInfo.Additional.IsValidate = true;
            styleInfo.Additional.IsRequired = true;
            styleInfo.Additional.MinNum = 4;
            styleInfo.Additional.MaxNum = 4;
            styleInfo.AttributeName = ValidateCodeManager.AttributeName;
            styleInfo.DisplayName = "验证码";
            styleInfo.Additional.Width = "50px";
            styleInfo.Additional.ValidateType = EInputValidateType.None;
            string validateAttributes = string.Empty;
            string validateHtmlString = InputTypeParser.GetValidateHtmlString(styleInfo, out validateAttributes);

            string regex = string.Format("<input\\s*[^>]*?id\\s*=\\s*(\"{0}\"|\'{0}\'|{0}).*?>", ValidateCodeManager.AttributeName);
            Regex reg = new Regex(regex, options);
            Match match = reg.Match(builder.ToString());
            if (match.Success)
            {
                imageUrl = vcManager.GetImageUrl(false);
                if (isCrossDomain)
                {
                    imageUrl = PageUtils.GetRootUrl(imageUrl);
                }

                element = match.Value;
                XmlDocument document = StlParserUtility.GetXmlDocument(element, false);
                XmlNode inputNode = document.DocumentElement;
                inputNode = inputNode.FirstChild;
                IEnumerator inputIE = inputNode.Attributes.GetEnumerator();
                StringDictionary attributes = new StringDictionary();
                while (inputIE.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)inputIE.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals("id"))
                    {
                        attributes.Add("id", ValidateCodeManager.AttributeName);
                        attributes.Add("name", ValidateCodeManager.AttributeName);
                    }
                    else if (!attributeName.Equals("name"))
                    {
                        attributes.Add(attr.Name, attr.Value);
                    }
                }
                //string inputReplace = string.Format(@"<input {0} {1}/>&nbsp;<img border=""0"" align=""absmiddle"" src=""{2}""/>{3}", TranslateUtils.ToAttributesString(attributes), validateAttributes, imageUrl, validateHtmlString);
                //ys修改，点击验证码刷新
                string inputReplace = string.Format(@"<input {0} {1}/>&nbsp;<img border=""0"" align=""absmiddle"" src=""{2}""  onclick=""this.src='{3}?id='+Math.random()"" style=""cursor:pointer;""/>{4}", TranslateUtils.ToAttributesString(attributes), validateAttributes, imageUrl, imageUrl, validateHtmlString);

                builder.Replace(element, inputReplace);
            }
        }

        public static string GetSubscribeCallbackScript(PublishmentSystemInfo publishmentSystemInfo, bool isSuccess, string message)
        {
            NameValueCollection jsonAttributes = new NameValueCollection();
            jsonAttributes.Add("isSuccess", isSuccess.ToString().ToLower());
            jsonAttributes.Add("message", message);

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(jsonAttributes);
            jsonString = StringUtils.ToJsString(jsonString);
            if (PageUtility.IsAgentCrossDomain(publishmentSystemInfo))
            {
                string script = string.Format("<script>window.parent.stlSubscribeCallback('{0}');</script>", jsonString);
                string proxyUrl = PageUtility.GetProxyUrl(publishmentSystemInfo, script);
                return string.Format(@"<script>document.write(""<iframe src='{0}' style='display:none'></iframe>"");</script>", proxyUrl);
            }
            else if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                return string.Format("<script>window.stlSubscribeCallback('{0}');</script>", jsonString);
            }
            else
            {
                return string.Format("<script>window.parent.stlSubscribeCallback('{0}');</script>", jsonString);
            }
        }
    }
}
