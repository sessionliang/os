using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Model;


using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CMS.Core
{
    public class InputTypeParser
    {
        private InputTypeParser()
        {
        }

        public const string CURRENT = "{Current}";

        public static string Parse(PublishmentSystemInfo publishmentSystemInfo, int nodeID, TableStyleInfo styleInfo, ETableStyle tableStyle, string attributeName, NameValueCollection formCollection, bool isEdit, bool isPostBack, string additionalAttributes, NameValueCollection pageScripts, bool isBackground, bool isValidate)
        {
            string retval = string.Empty;

            bool isAddAndNotPostBack = false;
            if (!isEdit && !isPostBack) isAddAndNotPostBack = true;//添加且未提交状态

            bool oriIsValidate = styleInfo.Additional.IsValidate;
            if (!isValidate)
            {
                styleInfo.Additional.IsValidate = false;
            }

            if (styleInfo.InputType == EInputType.Text)
            {
                retval = ParseText(publishmentSystemInfo, nodeID, attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, styleInfo, tableStyle, isBackground);
            }
            else if (styleInfo.InputType == EInputType.TextArea)
            {
                retval = ParseTextArea(attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, styleInfo);
            }
            else if (styleInfo.InputType == EInputType.TextEditor)
            {
                retval = ParseTextEditor(publishmentSystemInfo, attributeName, formCollection, isAddAndNotPostBack, pageScripts, styleInfo, isBackground);
            }
            else if (styleInfo.InputType == EInputType.SelectOne)
            {
                retval = ParseSelectOne(attributeName, formCollection, isAddAndNotPostBack, styleInfo);
            }
            else if (styleInfo.InputType == EInputType.SelectMultiple)
            {
                retval = ParseSelectMultiple(attributeName, formCollection, isAddAndNotPostBack, styleInfo);
            }
            else if (styleInfo.InputType == EInputType.CheckBox)
            {
                retval = ParseCheckBox(attributeName, formCollection, isAddAndNotPostBack, styleInfo);
            }
            else if (styleInfo.InputType == EInputType.Radio)
            {
                retval = ParseRadio(attributeName, formCollection, isAddAndNotPostBack, styleInfo);
            }
            else if (styleInfo.InputType == EInputType.Date)
            {
                retval = ParseDate(attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, pageScripts, styleInfo);
            }
            else if (styleInfo.InputType == EInputType.DateTime)
            {
                retval = ParseDateTime(attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, pageScripts, styleInfo);
            }
            else if (styleInfo.InputType == EInputType.Image)
            {
                if (isBackground)
                {
                    retval = ParseImage(publishmentSystemInfo, nodeID, attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, styleInfo, tableStyle);
                }
                else
                {
                    if (!isValidate) //提交表单
                    {
                        retval = ParseImageUpload(publishmentSystemInfo, formCollection, isAddAndNotPostBack, attributeName, additionalAttributes, styleInfo);
                    }
                    else//用户中心
                    {
                        retval = ParseImageUploadForTouGao(publishmentSystemInfo, formCollection, isAddAndNotPostBack, attributeName, additionalAttributes, styleInfo);
                    }
                }
            }
            else if (styleInfo.InputType == EInputType.Video)
            {
                if (isBackground)
                {
                    retval = ParseVideo(publishmentSystemInfo, nodeID, attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, styleInfo, tableStyle);
                }
                else
                {
                    if (!isValidate) //提交表单
                    {
                        retval = ParseVideoUpload(publishmentSystemInfo, formCollection, isAddAndNotPostBack, attributeName, additionalAttributes, styleInfo);
                    }
                    else//用户中心
                    {
                        retval = ParseVideoUploadForTouGao(publishmentSystemInfo, formCollection, isAddAndNotPostBack, attributeName, additionalAttributes, styleInfo);
                    }
                }
            }
            else if (styleInfo.InputType == EInputType.File)
            {
                if (isBackground)
                {
                    retval = ParseFile(publishmentSystemInfo, attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, styleInfo, tableStyle);
                }
                else
                {
                    if (!isValidate) //提交表单
                    {
                        retval = ParseFileUpload(attributeName, additionalAttributes, formCollection, isAddAndNotPostBack, styleInfo);
                    }
                    else//用户中心
                    {
                        retval = ParseFileUploadForTouGao(attributeName, additionalAttributes, formCollection, isAddAndNotPostBack, styleInfo);
                    }
                }
            }
            else if (styleInfo.InputType == EInputType.RelatedField)
            {
                retval = ParseRelatedField(publishmentSystemInfo, attributeName, formCollection, isAddAndNotPostBack, styleInfo, pageScripts);
            }
            else if (styleInfo.InputType == EInputType.SpecifiedValue)
            {
                retval = ParseSpecifiedValue(publishmentSystemInfo, nodeID, tableStyle, attributeName, formCollection, isAddAndNotPostBack, styleInfo);
            }

            styleInfo.Additional.IsValidate = oriIsValidate;

            return retval;
        }

        public static string GetValidateHtmlString(TableStyleInfo styleInfo, out string validateAttributes)
        {
            StringBuilder builder = new StringBuilder();

            validateAttributes = string.Empty;

            if (styleInfo.Additional.IsValidate && styleInfo.InputType != EInputType.TextEditor)
            {
                validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }
            return builder.ToString();
        }

        public static string ParseText(PublishmentSystemInfo publishmentSystemInfo, int nodeID, string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, TableStyleInfo styleInfo, ETableStyle tableStyle, bool isBackground)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, styleInfo.DefaultValue);
            value = StringUtils.HtmlDecode(value);

            string width = styleInfo.Additional.Width;
            if (string.IsNullOrEmpty(width))
            {
                width = styleInfo.IsSingleLine ? "380px" : "220px";
            }
            string style = string.Format(@"style=""width:{0};""", TranslateUtils.ToWidth(width));
            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""text"" class=""input_text"" value=""{1}"" {2} {3} {4} />", attributeName, value, additionalAttributes, style, validateAttributes);

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"<script>event_observe('{0}', 'blur', checkAttributeValue);</script>", styleInfo.AttributeName);
            }

            if (styleInfo.Additional.IsFormatString && isBackground)
            {
                bool isFormatted = false;
                bool formatStrong = false;
                bool formatEM = false;
                bool formatU = false;
                string formatColor = string.Empty;
                if (!isAddAndNotPostBack)
                {
                    if (formCollection != null && formCollection[ContentAttribute.GetFormatStringAttributeName(attributeName)] != null)
                    {
                        isFormatted = ContentUtility.SetTitleFormatControls(formCollection[ContentAttribute.GetFormatStringAttributeName(attributeName)], out formatStrong, out formatEM, out formatU, out formatColor);
                    }
                }

                builder.AppendFormat(@"<a class=""btn"" href=""javascript:;"" onclick=""$('#div_{0}').toggle();return false;""><i class=""icon-text-height""></i></a>
<script type=""text/javascript"">
function {0}_strong(e){{
var e = $(e);
if ($('#{0}_formatStrong').val() == 'true'){{
$('#{0}_formatStrong').val('false');
e.removeClass('btn-success');
}}else{{
$('#{0}_formatStrong').val('true');
e.addClass('btn-success');
}}
}}
function {0}_em(e){{
var e = $(e);
if ($('#{0}_formatEM').val() == 'true'){{
$('#{0}_formatEM').val('false');
e.removeClass('btn-success');
}}else{{
$('#{0}_formatEM').val('true');
e.addClass('btn-success');
}}
}}
function {0}_u(e){{
var e = $(e);
if ($('#{0}_formatU').val() == 'true'){{
$('#{0}_formatU').val('false');
e.removeClass('btn-success');
}}else{{
$('#{0}_formatU').val('true');
e.addClass('btn-success');
}}
}}
function {0}_color(){{
if ($('#{0}_formatColor').val()){{
$('#{0}_colorBtn').css('color', $('#{0}_formatColor').val());
$('#{0}_colorBtn').addClass('btn-success');
}}else{{
$('#{0}_colorBtn').css('color', '');
$('#{0}_colorBtn').removeClass('btn-success');
}}
$('#{0}_colorContainer').hide();
}}
</script>
", styleInfo.AttributeName);

                builder.AppendFormat(@"
<div id=""div_{0}"" style=""display:{1};margin-top:5px;"">
<div class=""btn-group"" style=""float:left;"">
    <button class=""btn{5}"" style=""font-weight:bold;font-size:12px;"" onclick=""{0}_strong(this);return false;"">粗体</button>
    <button class=""btn{6}"" style=""font-style:italic;font-size:12px;"" onclick=""{0}_em(this);return false;"">斜体</button>
    <button class=""btn{7}"" style=""text-decoration:underline;font-size:12px;"" onclick=""{0}_u(this);return false;"">下划线</button>
    <button class=""btn{8}"" style=""font-size:12px;"" id=""{0}_colorBtn"" onclick=""$('#{0}_colorContainer').toggle();return false;"">颜色</button>
</div>
<div id=""{0}_colorContainer"" class=""input-append"" style=""float:left;display:none"">
    <input id=""{0}_formatColor"" name=""{0}_formatColor"" class=""input-mini color {{required:false}}"" type=""text"" value=""{9}"" placeholder=""颜色值"">
    <button class=""btn"" type=""button"" onclick=""Title_color();return false;"">确定</button>
</div>
<input id=""{0}_formatStrong"" name=""{0}_formatStrong"" type=""hidden"" value=""{2}"" />
<input id=""{0}_formatEM"" name=""{0}_formatEM"" type=""hidden"" value=""{3}"" />
<input id=""{0}_formatU"" name=""{0}_formatU"" type=""hidden"" value=""{4}"" />
</div>
", styleInfo.AttributeName, isFormatted ? string.Empty : "none", formatStrong.ToString().ToLower(), formatEM.ToString().ToLower(), formatU.ToString().ToLower(), formatStrong ? @" btn-success" : string.Empty, formatEM ? " btn-success" : string.Empty, formatU ? " btn-success" : string.Empty, !string.IsNullOrEmpty(formatColor) ? " btn-success" : string.Empty, formatColor);
            }

            if (nodeID > 0 && (tableStyle == ETableStyle.BackgroundContent || tableStyle == ETableStyle.GovInteractContent || tableStyle == ETableStyle.GovPublicContent || tableStyle == ETableStyle.VoteContent) && styleInfo.AttributeName == ContentAttribute.Title)
            {
                builder.Append(@"
<script type=""text/javascript"">
function getTitles(title){
	$.get('[url]&title=' + encodeURIComponent(title) + '&channelID=' + $('#channelID').val() + '&r=' + Math.random(), function(data) {
		if(data !=''){
			var arr = data.split('|');
			var temp='';
			for(i=0;i<arr.length;i++)
			{
				temp += '<li><a>'+arr[i].replace(title,'<b>' + title + '</b>') + '</a></li>';
			}
			var myli='<ul>'+temp+'</ul>';
			$('#titleTips').html(myli);
			$('#titleTips').show();
		}else{
            $('#titleTips').hide();
        }
		$('#titleTips li').click(function () {
			$('#Title').val($(this).text());
			$('#titleTips').hide();
		})
	});	
}
$(document).ready(function () {
$('#Title').keyup(function (e) {
    if (e.keyCode != 40 && e.keyCode != 38) {
        var title = $('#Title').val();
        if (title != ''){
            window.setTimeout(""getTitles('"" + title + ""');"", 200);
        }else{
            $('#titleTips').hide();
        }
    }
}).blur(function () {
	window.setTimeout(""$('#titleTips').hide();"", 200);
})});
</script>
<div id=""titleTips"" class=""inputTips""></div>");
                builder.Replace("[url]", BackgroundService.GetTitlesUrl(publishmentSystemInfo.PublishmentSystemID, nodeID));
            }

            return builder.ToString();
        }

        public static string ParseTextArea(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, styleInfo.DefaultValue);
            value = StringUtils.HtmlDecode(value);

            string width = styleInfo.Additional.Width;
            if (string.IsNullOrEmpty(width))
            {
                width = styleInfo.IsSingleLine ? "98%" : "220px";
            }
            int height = styleInfo.Additional.Height;
            if (height == 0)
            {
                height = 80;
            }
            string style = string.Format(@"style=""width:{0};height:{1}px;""", TranslateUtils.ToWidth(width), height);

            builder.AppendFormat(@"<textarea id=""{0}"" name=""{0}"" class=""textarea"" {1} {2} {3}>{4}</textarea>", attributeName, additionalAttributes, style, validateAttributes, value);

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            return builder.ToString();
        }

        private static string ParseTextEditor(PublishmentSystemInfo publishmentSystemInfo, string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, NameValueCollection pageScripts, TableStyleInfo styleInfo, bool isBackground)
        {
            ETextEditorType editorType = ETextEditorType.EWebEditor;
            if (string.IsNullOrEmpty(styleInfo.Additional.EditorTypeString))
            {
                editorType = publishmentSystemInfo.Additional.TextEditorType;
            }
            else
            {
                editorType = ETextEditorTypeUtils.GetEnumType(styleInfo.Additional.EditorTypeString);
            }

            return ParseTextEditor(publishmentSystemInfo, attributeName, formCollection, isAddAndNotPostBack, pageScripts, editorType, styleInfo.DefaultValue, styleInfo.Additional.Width, styleInfo.Additional.Height, isBackground);
        }

        public static string ParseTextEditor(PublishmentSystemInfo publishmentSystemInfo, string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, NameValueCollection pageScripts, ETextEditorType editorType, string defaultValue, string width, int height, bool isBackground)
        {
            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, defaultValue);

            /****获取编辑器中内容，解析@符号，添加了远程路径处理 20151103****/
            value = StringUtility.TextEditorContentDecode(value, publishmentSystemInfo, isBackground);
            if (ETextEditorTypeUtils.IsInsertHtmlTranslateStlElement(editorType))
            {
                value = ETextEditorTypeUtils.TranslateToHtml(editorType, value);
            }
            value = StringUtils.HtmlEncode(value);

            StringBuilder builder = new StringBuilder();

            string snapHostUrl;
            string uploadImageUrl;
            string uploadScrawlUrl;
            string uploadFileUrl;
            string imageManagerUrl;
            string getMovieUrl;

            string editorUrl = PageUtility.GetTextEditorUrl(publishmentSystemInfo.PublishmentSystemID, editorType, isBackground, out snapHostUrl, out uploadImageUrl, out uploadScrawlUrl, out uploadFileUrl, out imageManagerUrl, out getMovieUrl);

            if (editorType == ETextEditorType.UEditor)
            {
                if (height <= 0)
                {
                    height = 280;
                }
                if (string.IsNullOrEmpty(width))
                {
                    width = "100%";
                }

                if (pageScripts["uEditor"] == null)
                {
                    builder.AppendFormat(@"<script type=""text/javascript"">window.UEDITOR_HOME_URL = ""{0}/"";window.UEDITOR_IMAGE_URL = ""{1}"";window.UEDITOR_SCRAWL_URL = ""{2}"";window.UEDITOR_FILE_URL=""{3}"";window.UEDITOR_SNAP_HOST=""{4}"";window.UEDITOR_IMAGE_MANAGER_URL=""{5}"";window.UEDITOR_MOVIE_URL=""{6}""</script><script type=""text/javascript"" src=""{0}/editor_config.js""></script><script type=""text/javascript"" src=""{0}/editor_all.js""></script>", editorUrl, uploadImageUrl, uploadScrawlUrl, uploadFileUrl, snapHostUrl, imageManagerUrl, getMovieUrl);
                }
                pageScripts["uEditor"] = string.Empty;

                builder.AppendFormat(@"
<textarea id=""{0}"" name=""{0}"" style=""display:none"">{1}</textarea>
<script type=""text/javascript"">
$(function(){{
  UE.getEditor('{0}');
  $('#{0}').show();
}});
</script>", attributeName, value);
            }
            else if (editorType == ETextEditorType.CKEditor)
            {
                string size = string.Empty;
                if (!string.IsNullOrEmpty(width))
                {
                    size = string.Format(@"
width : '{0}',", TranslateUtils.ToWidth(width));
                }

                if (height <= 0)
                {
                    height = 280;
                }

                size += string.Format(@"
height : {0},", height);

                if (pageScripts["ckEditor"] == null)
                {
                    builder.AppendFormat(@"<script type=""text/javascript"" src=""{0}/ckeditor.js""></script>", editorUrl);
                }
                pageScripts["ckEditor"] = string.Empty;

                builder.AppendFormat(@"<textarea name=""{0}"" id=""{0}"" style=""display:none"">{1}</textarea>", attributeName, value);
                builder.AppendFormat(@"
<script type=""text/javascript"">
CKEDITOR.replace( '{0}',
{{
        customConfig : '{1}/my_config.js',{2}
        filebrowserImageUploadUrl : '{3}',
        filebrowserFlashUploadUrl : '{4}'
}});
</script>
", attributeName, editorUrl, size, uploadImageUrl, uploadFileUrl);
            }
            else if (editorType == ETextEditorType.EWebEditor)
            {
                editorUrl = PageUtils.Combine(editorUrl, string.Format("ewebeditor.htm?id={0}&style={1}&PublishmentSystemID={2}", attributeName, "coolblue", publishmentSystemInfo.PublishmentSystemID));

                string hiddenString = string.Format(@"<input name=""{0}"" id=""{0}"" type=""hidden"" value=""{1}"" />", attributeName, value);

                if (string.IsNullOrEmpty(width))
                {
                    width = "550";
                }
                else
                {
                    width = width.Replace("%", string.Empty).Replace("px", string.Empty);
                }

                if (height == 0)
                {
                    height = 400;
                }

                builder.AppendFormat(@"
	{0}
	<IFRAME ID=""{1}"" SRC=""{2}"" FRAMEBORDER=""0"" SCROLLING=""no"" WIDTH=""{3}"" HEIGHT=""{4}""></IFRAME>
", hiddenString, "eWebEditor_" + attributeName, editorUrl, width, height);
            }
            else if (editorType == ETextEditorType.FCKEditor)
            {
                if (string.IsNullOrEmpty(width))
                {
                    width = "550px";
                }

                if (height == 0)
                {
                    height = 400;
                }

                PublishmentSystemManager.SetPublishmentSystemIDByCache(publishmentSystemInfo.PublishmentSystemID);

                if (pageScripts["fckEditor"] == null)
                {
                    builder.AppendFormat(@"<script type=""text/javascript"" src=""{0}/fckeditor.js""></script>", editorUrl);
                }
                pageScripts["fckEditor"] = string.Empty;

                builder.AppendFormat(@"<textarea name=""{0}"" id=""{0}"" class=""textarea"">{1}</textarea>", attributeName, value);
                builder.AppendFormat(@"
<script type=""text/javascript"">
<!--
var oFCKeditor_{0} = new FCKeditor('{0}') ;
oFCKeditor_{0}.BasePath = '{1}/' ;
oFCKeditor_{0}.Config['CustomConfigurationsPath'] = '{1}/my.config.js' ;
oFCKeditor_{0}.ToolbarSet = 'MyToolbarSet' ;
oFCKeditor_{0}.Height = {2} ;
oFCKeditor_{0}.Width = '{3}' ;
oFCKeditor_{0}.ReplaceTextarea();
//-->
</script>
", attributeName, editorUrl, height, TranslateUtils.ToWidth(width));
            }
            else if (editorType == ETextEditorType.KindEditor)
            {
                string size = string.Empty;
                if (string.IsNullOrEmpty(width))
                {
                    width = "100%";
                }
                size = string.Format(@"
width : {0};", TranslateUtils.ToWidth(width));

                if (height <= 0)
                {
                    height = 350;
                }

                size += string.Format(@"
height : {0}px", height);

                if (pageScripts["kindEditor"] == null)
                {
                    builder.AppendFormat(@"<script type=""text/javascript"" src=""{0}/kindeditor-min.js""></script>", editorUrl);
                }
                pageScripts["kindEditor"] = string.Empty;

                builder.AppendFormat(@"<textarea name=""{0}"" id=""{0}"" style=""{1};visibility:hidden;"">{2}</textarea>", attributeName, size, value);
                builder.AppendFormat(@"
<script language=""javascript"" type=""text/javascript"">
KE.show({{
	id : '{0}',
	imageUploadJson : '{1}/upload_json.ashx?PublishmentSystemID={2}',
	allowFileManager : false,
	items : [
		'fullscreen', 'undo', 'redo', 'print', 'cut', 'copy', 'paste',
		'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
		'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
		'superscript', '|', 'selectall', '-',
		'title', 'fontname', 'fontsize', '|', 'textcolor', 'bgcolor', 'bold',
		'italic', 'underline', 'strikethrough', 'removeformat', '|', 'image',
		'flash', 'media', 'advtable', 'hr', 'emoticons', 'link', 'unlink'
	]
}});
</script>
", attributeName, editorUrl, publishmentSystemInfo.PublishmentSystemID);
            }
            else if (editorType == ETextEditorType.xHtmlEditor)
            {
                editorUrl = PageUtils.Combine(editorUrl, string.Format("editor.htm?id={0}&ReadCookie=0&publishmentSystemID={1}", attributeName, publishmentSystemInfo.PublishmentSystemID));

                if (string.IsNullOrEmpty(width))
                {
                    width = "621";
                }
                else
                {
                    width = width.Replace("%", string.Empty).Replace("px", string.Empty);
                }

                if (height == 0)
                {
                    height = 457;
                }

                builder.AppendFormat(@"
<textarea name=""{0}"" id=""{0}"" style=""display:none"">{1}</textarea>
<iframe src=""{2}"" frameBorder=""0"" marginHeight=""0"" marginWidth=""0"" scrolling=""No"" width=""{3}"" height=""{4}""></iframe>
", attributeName, value, editorUrl, TranslateUtils.ToWidth(width), height);
            }

            return builder.ToString();
        }

        private static string ParseDate(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, NameValueCollection pageScripts, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            DateTime dateTime = DateUtils.SqlMinValue;
            if (isAddAndNotPostBack)
            {
                if (!string.IsNullOrEmpty(styleInfo.DefaultValue))
                {
                    if (styleInfo.DefaultValue == CURRENT)
                    {
                        dateTime = DateTime.Now;
                    }
                    else
                    {
                        dateTime = TranslateUtils.ToDateTime(styleInfo.DefaultValue);
                    }
                }
            }
            else
            {
                if (formCollection != null && formCollection[attributeName] != null)
                {
                    dateTime = TranslateUtils.ToDateTime(formCollection[attributeName]);
                }
            }

            if (pageScripts != null)
            {
                pageScripts["calendar"] = string.Format(@"<script language=""javascript"" src=""{0}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.DatePicker.Js));
            }

            string value = string.Empty;
            if (dateTime > DateUtils.SqlMinValue)
            {
                value = DateUtils.GetDateString(dateTime);
            }

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""text"" class=""input_text"" value=""{1}"" {2} onfocus=""{3}"" />", attributeName, value, additionalAttributes, SiteFiles.DatePicker.OnFocusDateOnly);

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            return builder.ToString();
        }

        private static string ParseDateTime(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, NameValueCollection pageScripts, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            DateTime dateTime = DateUtils.SqlMinValue;
            if (isAddAndNotPostBack)
            {
                if (!string.IsNullOrEmpty(styleInfo.DefaultValue))
                {
                    if (styleInfo.DefaultValue == CURRENT)
                    {
                        dateTime = DateTime.Now;
                    }
                    else
                    {
                        dateTime = TranslateUtils.ToDateTime(styleInfo.DefaultValue);
                    }
                }
            }
            else
            {
                if (formCollection != null && formCollection[attributeName] != null)
                {
                    dateTime = TranslateUtils.ToDateTime(formCollection[attributeName]);
                }
            }

            if (pageScripts != null)
            {
                pageScripts["calendar"] = string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.DatePicker.Js));
            }

            string value = string.Empty;
            if (dateTime > DateUtils.SqlMinValue)
            {
                value = DateUtils.GetDateAndTimeString(dateTime, EDateFormatType.Day, ETimeFormatType.LongTime);
            }

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""text"" class=""input_text"" value=""{1}"" {2} onfocus=""{3}"" />", attributeName, value, additionalAttributes, SiteFiles.DatePicker.OnFocus);

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            return builder.ToString();
        }

        private static string ParseCheckBox(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList styleItems = styleInfo.StyleItems;
            if (styleItems == null)
            {
                styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
            }
            string retval = string.Empty;

            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.CssClass = "checkboxlist";
            checkBoxList.ID = attributeName;
            if (styleInfo.IsHorizontal)
            {
                checkBoxList.RepeatDirection = RepeatDirection.Horizontal;
            }
            else
            {
                checkBoxList.RepeatDirection = RepeatDirection.Vertical;
            }
            checkBoxList.RepeatColumns = styleInfo.Additional.Columns;
            string selectedValues = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : string.Empty;
            ArrayList selectedValueArrayList = TranslateUtils.StringCollectionToArrayList(selectedValues);

            //验证属性
            InputParserUtils.GetValidateAttributesForListItem(checkBoxList, styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            foreach (TableStyleItemInfo styleItem in styleItems)
            {
                bool isSelected = false;
                if (isAddAndNotPostBack)
                {
                    isSelected = styleItem.IsSelected;
                }
                else
                {
                    isSelected = (selectedValueArrayList.Contains(styleItem.ItemValue));
                }
                ListItem listItem = new ListItem(styleItem.ItemTitle, styleItem.ItemValue);
                listItem.Selected = isSelected;

                checkBoxList.Items.Add(listItem);
            }
            checkBoxList.Attributes.Add("isListItem", "true");
            builder.Append(ControlUtils.GetControlRenderHtml(checkBoxList));

            int i = 0;
            foreach (TableStyleItemInfo styleItem in styleItems)
            {
                builder.Replace(string.Format(@"name=""{0}${1}""", attributeName, i), string.Format(@"name=""{0}"" value=""{1}""", attributeName, styleItem.ItemValue));
                i++;
            }

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            //StringBuilder builder = new StringBuilder();

            //builder.AppendFormat(@"<table cellpadding=""2"" cellspacing=""2"" border=""0"">");
            //if (isHorizontal) builder.AppendFormat("<tr>");

            //string selectedValues = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : string.Empty;
            //ArrayList selectedValueArrayList = TranslateUtils.StringCollectionToArrayList(selectedValues);

            //int index = 0;
            //foreach (TableStyleItemInfo styleItem in styleItems)
            //{
            //    string isSelected;
            //    if (isAddAndNotPostBack)
            //    {
            //        isSelected = (styleItem.IsSelected == EBoolean.True) ? "checked" : string.Empty;
            //    }
            //    else
            //    {
            //        isSelected = (selectedValueArrayList.Contains(styleItem.ItemValue)) ? "checked" : string.Empty;
            //    }
            //    if (!isHorizontal) builder.AppendFormat("<tr>");
            //    builder.AppendFormat(@"<td><input name=""{0}"" id=""{1}"" type=""checkbox"" value=""{2}"" {3}/><label for=""{1}"">{4}</label></td>", attributeName, (attributeName + ++index), styleItem.ItemValue, isSelected, styleItem.ItemTitle);
            //    if (!isHorizontal) builder.AppendFormat("</tr>");
            //}

            //if (isHorizontal) builder.AppendFormat("</tr>");
            //builder.AppendFormat(@"</table>");

            return builder.ToString();
        }

        private static string ParseRadio(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList styleItems = styleInfo.StyleItems;
            if (styleItems == null)
            {
                styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
            }
            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.CssClass = "radiobuttonlist";
            radioButtonList.ID = attributeName;
            if (styleInfo.IsHorizontal)
            {
                radioButtonList.RepeatDirection = RepeatDirection.Horizontal;
            }
            else
            {
                radioButtonList.RepeatDirection = RepeatDirection.Vertical;
            }
            radioButtonList.RepeatColumns = styleInfo.Additional.Columns;
            string selectedValue = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : null;

            //验证属性
            InputParserUtils.GetValidateAttributesForListItem(radioButtonList, styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            foreach (TableStyleItemInfo styleItem in styleItems)
            {
                bool isSelected = false;
                if (isAddAndNotPostBack)
                {
                    isSelected = styleItem.IsSelected;
                }
                else
                {
                    isSelected = (styleItem.ItemValue == selectedValue);
                }
                ListItem listItem = new ListItem(styleItem.ItemTitle, styleItem.ItemValue);
                listItem.Selected = isSelected;
                listItem.Attributes.Add("class", "input_radio");
                radioButtonList.Items.Add(listItem);
            }
            radioButtonList.Attributes.Add("isListItem", "true");
            builder.Append(ControlUtils.GetControlRenderHtml(radioButtonList));

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);


            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            //builder.AppendFormat(@"<table cellpadding=""2"" cellspacing=""2"" border=""0"">");
            //if (isHorizontal) builder.AppendFormat("<tr>");

            //string selectedValue = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : null;

            //int index = 0;
            //foreach (TableStyleItemInfo styleItem in styleItems)
            //{
            //    string isSelected;
            //    if (isAddAndNotPostBack)
            //    {
            //        isSelected = (styleItem.IsSelected == EBoolean.True) ? "checked" : string.Empty;
            //    }
            //    else
            //    {
            //        isSelected = (styleItem.ItemValue == selectedValue) ? "checked" : string.Empty;
            //    }
            //    if (!isHorizontal) builder.AppendFormat("<tr>");
            //    builder.AppendFormat(@"<td><input name=""{0}"" id=""{1}"" type=""radio"" value=""{2}"" {3}/><label for=""{1}"">{4}</label></td>", attributeName, (attributeName + ++index), styleItem.ItemValue, isSelected, styleItem.ItemTitle);
            //    if (!isHorizontal) builder.AppendFormat("</tr>");
            //}

            //if (isHorizontal) builder.AppendFormat("</tr>");
            //builder.AppendFormat(@"</table>");

            return builder.ToString();
        }

        private static string ParseSelectOne(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList styleItems = styleInfo.StyleItems;
            if (styleItems == null)
            {
                styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
            }

            string selectedValue = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : null;
            //验证属性
            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);
            builder.AppendFormat(@"<select id=""{0}"" name=""{0}"" class=""select""  isListItem=""true"" {1}>", attributeName, validateAttributes);
            foreach (TableStyleItemInfo styleItem in styleItems)
            {
                string isSelected;
                if (isAddAndNotPostBack)
                {
                    isSelected = styleItem.IsSelected ? "selected" : string.Empty;
                }
                else
                {
                    isSelected = (styleItem.ItemValue == selectedValue) ? "selected" : string.Empty;
                }

                builder.AppendFormat(@"<option value=""{0}"" {1}>{2}</option>", styleItem.ItemValue, isSelected, styleItem.ItemTitle);
            }
            builder.Append("</select>");

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);
            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }
            return builder.ToString();
        }

        private static string ParseSelectMultiple(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList styleItems = styleInfo.StyleItems;
            if (styleItems == null)
            {
                styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
            }

            string selectedValues = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : string.Empty;
            ArrayList selectedValueArrayList = TranslateUtils.StringCollectionToArrayList(selectedValues);
            //验证属性
            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);
            builder.AppendFormat(@"<select id=""{0}"" name=""{0}"" class=""select_multiple""  isListItem=""true"" multiple  {1}>", attributeName, validateAttributes);
            foreach (TableStyleItemInfo styleItem in styleItems)
            {
                string isSelected;
                if (isAddAndNotPostBack)
                {
                    isSelected = styleItem.IsSelected ? "selected" : string.Empty;
                }
                else
                {
                    isSelected = (selectedValueArrayList.Contains(styleItem.ItemValue)) ? "selected" : string.Empty;
                }

                builder.AppendFormat(@"<option value=""{0}"" {1}>{2}</option>", styleItem.ItemValue, isSelected, styleItem.ItemTitle);
            }
            builder.Append("</select>");

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);
            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }
            return builder.ToString();
        }

        public static string GetAttributeNameToUploadForTouGao(string attributeName)
        {
            return attributeName + "_uploader";
        }

        private static string ParseImage(PublishmentSystemInfo publishmentSystemInfo, int nodeID, string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, TableStyleInfo styleInfo, ETableStyle tableStyle)
        {
            StringBuilder builder = new StringBuilder();

            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);

            string width = styleInfo.Additional.Width;
            if (string.IsNullOrEmpty(width))
            {
                width = styleInfo.IsSingleLine ? "380px" : "220px";
            }
            string style = string.Format(@"style=""width:{0};""", TranslateUtils.ToWidth(width));

            string btnAddHtml = string.Empty;
            if (ETableStyleUtils.IsContent(tableStyle))
            {
                btnAddHtml = string.Format(@"
    <a class=""btn"" href=""javascript:;"" onclick=""add_{0}('',true)"" title=""新增""><i class=""icon-plus""></i></a>
", attributeName);
            }

            builder.AppendFormat(@"
<div class=""clearfix"">
  <div class=""pull-left"">
    <input id=""{0}"" name=""{0}"" type=""text"" class=""input_text"" value=""{1}"" {2} {3} />&nbsp;
  </div>
  <div class=""pull-left btn-group"">
    <a class=""btn"" href=""javascript:;"" onclick=""{4}"" title=""选择""><i class=""icon-th""></i></a>
    <a class=""btn"" href=""javascript:;"" onclick=""{5}"" title=""上传""><i class=""icon-arrow-up""></i></a>
    <a class=""btn"" href=""javascript:;"" onclick=""{6}"" title=""裁切""><i class=""icon-crop""></i></a>
    <a class=""btn"" href=""javascript:;"" onclick=""{7}"" title=""预览""><i class=""icon-eye-open""></i></a>
    {8}
  </div>
</div>
", attributeName, value, additionalAttributes, style, SiteServer.CMS.BackgroundPages.Modal.SelectImage.GetOpenWindowString(publishmentSystemInfo, attributeName), SiteServer.CMS.BackgroundPages.Modal.UploadImage.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeID, attributeName), SiteServer.CMS.BackgroundPages.Modal.CuttingImage.GetOpenWindowStringWithTextBox(publishmentSystemInfo.PublishmentSystemID, attributeName), SiteServer.CMS.BackgroundPages.Modal.Message.GetOpenWindowStringToPreviewImage(publishmentSystemInfo.PublishmentSystemID, attributeName), btnAddHtml);

            string extendAttributeName = ContentAttribute.GetExtendAttributeName(attributeName);

            builder.AppendFormat(@"
<script type=""text/javascript"">
function select_{0}(obj, index){{
  var cmd = ""{1}"".replace('{0}', '{0}_' + index).replace('return false;', '');
  eval(cmd);
}}
function upload_{0}(obj, index){{
  var cmd = ""{2}"".replace('{0}', '{0}_' + index).replace('return false;', '');
  eval(cmd);
}}
function cutting_{0}(obj, index){{
  var cmd = ""{3}"".replace('{0}', '{0}_' + index).replace('return false;', '');
  eval(cmd);
}}
function preview_{0}(obj, index){{
  var cmd = ""{4}"".replace(/{0}/g, '{0}_' + index).replace('return false;', '');
  eval(cmd);
}}
function delete_{0}(obj){{
  $(obj).closest('tr').remove();
}}
var index_{0} = 0;
function add_{0}(val,foucs){{
    index_{0}++;
    var html = '<div class=""clearfix""><div class=""pull-left"">';
    html += '<input id=""{0}_'+index_{0}+'"" name=""{5}"" type=""text"" class=""input_text"" value=""'+val+'"" {6} {7} />&nbsp;';
    html += '</div>';
    html += '<div class=""pull-left btn-group"">';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""select_{0}(this, '+index_{0}+')"" title=""选择""><i class=""icon-th""></i></a>';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""upload_{0}(this, '+index_{0}+')"" title=""上传""><i class=""icon-arrow-up""></i></a>';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""cutting_{0}(this, '+index_{0}+')"" title=""裁切""><i class=""icon-crop""></i></a>';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""preview_{0}(this, '+index_{0}+')"" title=""预览""><i class=""icon-eye-open""></i></a>';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""delete_{0}(this)"" title=""删除""><i class=""icon-remove""></i></a>';
    html += '</div></div>';
    var tr = $('.{5}').length == 0 ? $('#{0}').closest('tr') : $('.{5}:last');
    tr.after('<tr class=""{5}""><td>&nbsp;</td><td colspan=""3"">'+html+'</td></tr>');
    if (foucs) $('#{0}_'+index_{0}).focus();
}}
", attributeName, SiteServer.CMS.BackgroundPages.Modal.SelectImage.GetOpenWindowString(publishmentSystemInfo, attributeName), SiteServer.CMS.BackgroundPages.Modal.UploadImage.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, nodeID, attributeName), SiteServer.CMS.BackgroundPages.Modal.CuttingImage.GetOpenWindowStringWithTextBox(publishmentSystemInfo.PublishmentSystemID, attributeName), SiteServer.CMS.BackgroundPages.Modal.Message.GetOpenWindowStringToPreviewImage(publishmentSystemInfo.PublishmentSystemID, attributeName), extendAttributeName, additionalAttributes, style);

            string extendValues = formCollection[extendAttributeName];
            if (!string.IsNullOrEmpty(extendValues))
            {
                foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                {
                    if (!string.IsNullOrEmpty(extendValue))
                    {
                        builder.AppendFormat("add_{0}('{1}',false);", attributeName, extendValue);
                    }
                }
            }

            builder.Append("</script>");

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            return builder.ToString();
        }

        private static string ParseImageUpload(PublishmentSystemInfo publishmentSystemInfo, NameValueCollection formCollection, bool isAddAndNotPostBack, string attributeName, string additionalAttributes, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""file"" class=""input_file"" {1} {2} />", attributeName, additionalAttributes, validateAttributes);

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            return builder.ToString();
        }

        private static string ParseImageUploadForTouGao(PublishmentSystemInfo publishmentSystemInfo, NameValueCollection formCollection, bool isAddAndNotPostBack, string attributeName, string additionalAttributes, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            string attributeNameToUpload = InputTypeParser.GetAttributeNameToUploadForTouGao(attributeName);
            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);

            builder.AppendFormat(@"
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
<tr>
    <td width=""400"">
        <input id=""{0}"" name=""{0}"" type=""text"" class=""input-txt"" value='{1}' style=""display:;width:320px"" />
        <input id=""{2}"" name=""{2}"" style=""width:320px;display:none"" type=""file"" class=""input-txt"" />
    </td>
</tr>
<tr>
    <td valign=""top"">
        <a id=""{0}_link1"" style=""font-weight:bold"" href=""javascript:;"" onclick=""document.getElementById('{0}_link2').style.fontWeight = '';this.style.fontWeight = 'bold';document.getElementById('{0}').style.display = '';document.getElementById('{2}').style.display = 'none';"">输入 URL</a>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <a id=""{0}_link2"" href=""javascript:;"" onclick=""document.getElementById('{0}_link1').style.fontWeight = '';this.style.fontWeight = 'bold';document.getElementById('{0}').style.display = 'none';document.getElementById('{2}').style.display = '';"" >上传图片</a>
    </td>
</tr>
</table>", attributeName, value, attributeNameToUpload);

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            return builder.ToString();
        }

        private static string ParseVideo(PublishmentSystemInfo publishmentSystemInfo, int nodeID, string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, TableStyleInfo styleInfo, ETableStyle tableStyle)
        {
            StringBuilder builder = new StringBuilder();

            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);

            string width = styleInfo.Additional.Width;
            if (string.IsNullOrEmpty(width))
            {
                width = styleInfo.IsSingleLine ? "380px" : "220px";
            }
            string style = string.Format(@"style=""width:{0};""", TranslateUtils.ToWidth(width));

            string btnAddHtml = string.Empty;
            if (ETableStyleUtils.IsContent(tableStyle))
            {
                btnAddHtml = string.Format(@"
    <a class=""btn"" href=""javascript:;"" onclick=""add_{0}('',true)"" title=""新增""><i class=""icon-plus""></i></a>
", attributeName);
            }

            builder.AppendFormat(@"
<div class=""clearfix"">
  <div class=""pull-left"">
    <input id=""{0}"" name=""{0}"" type=""text"" class=""input_text"" value=""{1}"" {2} {3} />&nbsp;
  </div>
  <div class=""pull-left btn-group"">
    <a class=""btn"" href=""javascript:;"" onclick=""{4}"" title=""选择""><i class=""icon-th""></i></a>
    <a class=""btn"" href=""javascript:;"" onclick=""{5}"" title=""上传""><i class=""icon-arrow-up""></i></a>
    <a class=""btn"" href=""javascript:;"" onclick=""{6}"" title=""预览""><i class=""icon-eye-open""></i></a>
    {7}
  </div>
</div>
", attributeName, value, additionalAttributes, style, SiteServer.CMS.BackgroundPages.Modal.SelectVideo.GetOpenWindowString(publishmentSystemInfo, attributeName), SiteServer.CMS.BackgroundPages.Modal.UploadVideo.GetOpenWindowStringToTextBox(publishmentSystemInfo.PublishmentSystemID, attributeName), SiteServer.CMS.BackgroundPages.Modal.Message.GetOpenWindowStringToPreviewVideo(publishmentSystemInfo.PublishmentSystemID, attributeName), btnAddHtml);

            string extendAttributeName = ContentAttribute.GetExtendAttributeName(attributeName);

            builder.AppendFormat(@"
<script type=""text/javascript"">
function select_{0}(obj, index){{
  var cmd = ""{1}"".replace('{0}', '{0}_' + index).replace('return false;', '');
  eval(cmd);
}}
function upload_{0}(obj, index){{
  var cmd = ""{2}"".replace('{0}', '{0}_' + index).replace('return false;', '');
  eval(cmd);
}}
function preview_{0}(obj, index){{
  var cmd = ""{3}"".replace(/{0}/g, '{0}_' + index).replace('return false;', '');
  eval(cmd);
}}
function delete_{0}(obj){{
  $(obj).closest('tr').remove();
}}
var index_{0} = 0;
function add_{0}(val,foucs){{
    index_{0}++;
    var html = '<div class=""clearfix""><div class=""pull-left"">';
    html += '<input id=""{0}_'+index_{0}+'"" name=""{4}"" type=""text"" class=""input_text"" value=""'+val+'"" {5} {6} />&nbsp;';
    html += '</div>';
    html += '<div class=""pull-left btn-group"">';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""select_{0}(this, '+index_{0}+')"" title=""选择""><i class=""icon-th""></i></a>';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""upload_{0}(this, '+index_{0}+')"" title=""上传""><i class=""icon-arrow-up""></i></a>';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""preview_{0}(this, '+index_{0}+')"" title=""预览""><i class=""icon-eye-open""></i></a>';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""delete_{0}(this)"" title=""删除""><i class=""icon-remove""></i></a>';
    html += '</div></div>';
    var tr = $('.{4}').length == 0 ? $('#{0}').closest('tr') : $('.{4}:last');
    tr.after('<tr class=""{4}""><td>&nbsp;</td><td colspan=""3"">'+html+'</td></tr>');
    if (foucs) $('#{0}_'+index_{0}).focus();
}}
", attributeName, SiteServer.CMS.BackgroundPages.Modal.SelectVideo.GetOpenWindowString(publishmentSystemInfo, attributeName), SiteServer.CMS.BackgroundPages.Modal.UploadVideo.GetOpenWindowStringToTextBox(publishmentSystemInfo.PublishmentSystemID, attributeName), SiteServer.CMS.BackgroundPages.Modal.Message.GetOpenWindowStringToPreviewVideo(publishmentSystemInfo.PublishmentSystemID, attributeName), extendAttributeName, additionalAttributes, style);

            string extendValues = formCollection[extendAttributeName];
            if (!string.IsNullOrEmpty(extendValues))
            {
                foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                {
                    if (!string.IsNullOrEmpty(extendValue))
                    {
                        builder.AppendFormat("add_{0}('{1}',false);", attributeName, extendValue);
                    }
                }
            }

            builder.Append("</script>");

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            return builder.ToString();
        }

        private static string ParseVideoUpload(PublishmentSystemInfo publishmentSystemInfo, NameValueCollection formCollection, bool isAddAndNotPostBack, string attributeName, string additionalAttributes, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""file"" class=""input_file"" {1} {2} />", attributeName, additionalAttributes, validateAttributes);

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            return builder.ToString();
        }

        private static string ParseVideoUploadForTouGao(PublishmentSystemInfo publishmentSystemInfo, NameValueCollection formCollection, bool isAddAndNotPostBack, string attributeName, string additionalAttributes, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            string attributeNameToUpload = InputTypeParser.GetAttributeNameToUploadForTouGao(attributeName);
            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);

            builder.AppendFormat(@"
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
<tr>
    <td width=""400"">
        <input id=""{0}"" name=""{0}"" type=""text"" class=""input-txt"" value='{1}' style=""display:;width:320px"" />
        <input id=""{2}"" name=""{2}"" style=""width:320px;display:none"" type=""file"" class=""input-txt"" />
    </td>
</tr>
<tr>
    <td valign=""top"">
        <a id=""{0}_link1"" style=""font-weight:bold"" href=""javascript:;"" onclick=""document.getElementById('{0}_link2').style.fontWeight = '';this.style.fontWeight = 'bold';document.getElementById('{0}').style.display = '';document.getElementById('{2}').style.display = 'none';"">输入 URL</a>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <a id=""{0}_link2"" href=""javascript:;"" onclick=""document.getElementById('{0}_link1').style.fontWeight = '';this.style.fontWeight = 'bold';document.getElementById('{0}').style.display = 'none';document.getElementById('{2}').style.display = '';"" >上传视频</a>
    </td>
</tr>
</table>", attributeName, value, attributeNameToUpload);

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            return builder.ToString();
        }

        private static string ParseFile(PublishmentSystemInfo publishmentSystemInfo, string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, TableStyleInfo styleInfo, ETableStyle tableStyle)
        {
            StringBuilder builder = new StringBuilder();

            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);
            string relatedPath = string.Empty;
            string fileName = value;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim('/');
                int i = value.LastIndexOf('/');
                if (i != -1)
                {
                    relatedPath = value.Substring(0, i + 1);
                    fileName = value.Substring(i + 1, value.Length - i - 1);
                }
            }

            string width = styleInfo.Additional.Width;
            if (string.IsNullOrEmpty(width))
            {
                width = styleInfo.IsSingleLine ? "380px" : "220px";
            }
            string style = string.Format(@"style=""width:{0};""", TranslateUtils.ToWidth(width));

            string btnAddHtml = string.Empty;
            if (ETableStyleUtils.IsContent(tableStyle))
            {
                btnAddHtml = string.Format(@"
    <a class=""btn"" href=""javascript:;"" onclick=""add_{0}('',true)"" title=""新增""><i class=""icon-plus""></i></a>
", attributeName);
            }

            builder.AppendFormat(@"
<div class=""clearfix"">
  <div class=""pull-left"">
    <input id=""{0}"" name=""{0}"" type=""text"" class=""input_text"" value=""{1}"" {2} {3} />&nbsp;
  </div>
  <div class=""pull-left btn-group"">
    <a class=""btn"" href=""javascript:;"" onclick=""{4}"" title=""选择""><i class=""icon-th""></i></a>
    <a class=""btn"" href=""javascript:;"" onclick=""{5}"" title=""上传""><i class=""icon-arrow-up""></i></a>
    <a class=""btn"" href=""javascript:;"" onclick=""{6}"" title=""查看""><i class=""icon-eye-open""></i></a>
    {7}
  </div>
</div>
", attributeName, value, additionalAttributes, style, SiteServer.CMS.BackgroundPages.Modal.SelectFile.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, attributeName, relatedPath), SiteServer.CMS.BackgroundPages.Modal.UploadFile.GetOpenWindowStringToTextBox(publishmentSystemInfo.PublishmentSystemID, EUploadType.File, attributeName), SiteServer.CMS.BackgroundPages.Modal.FileView.GetOpenWindowStringWithTextBoxValue(publishmentSystemInfo.PublishmentSystemID, attributeName), btnAddHtml);

            string extendAttributeName = ContentAttribute.GetExtendAttributeName(attributeName);

            builder.AppendFormat(@"
<script type=""text/javascript"">
function select_{0}(obj, index){{
  var cmd = ""{1}"".replace('{0}', '{0}_' + index).replace('return false;', '');
  eval(cmd);
}}
function upload_{0}(obj, index){{
  var cmd = ""{2}"".replace('{0}', '{0}_' + index).replace('return false;', '');
  eval(cmd);
}}
function preview_{0}(obj, index){{
  var cmd = ""{3}"".replace(/{0}/g, '{0}_' + index).replace('return false;', '');
  eval(cmd);
}}
function delete_{0}(obj){{
  $(obj).closest('tr').remove();
}}
var index_{0} = 0;
function add_{0}(val,foucs){{
    index_{0}++;
    var html = '<div class=""clearfix""><div class=""pull-left"">';
    html += '<input id=""{0}_'+index_{0}+'"" name=""{4}"" type=""text"" class=""input_text"" value=""'+val+'"" {5} {6} />&nbsp;';
    html += '</div>';
    html += '<div class=""pull-left btn-group"">';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""select_{0}(this, '+index_{0}+')"" title=""选择""><i class=""icon-th""></i></a>';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""upload_{0}(this, '+index_{0}+')"" title=""上传""><i class=""icon-arrow-up""></i></a>';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""preview_{0}(this, '+index_{0}+')"" title=""查看""><i class=""icon-eye-open""></i></a>';
    html += '<a class=""btn"" href=""javascript:;"" onclick=""delete_{0}(this)"" title=""删除""><i class=""icon-remove""></i></a>';
    html += '</div></div>';
    var tr = $('.{4}').length == 0 ? $('#{0}').closest('tr') : $('.{4}:last');
    tr.after('<tr class=""{4}""><td>&nbsp;</td><td colspan=""3"">'+html+'</td></tr>');
    if (foucs) $('#{0}_'+index_{0}).focus();
}}
", attributeName, SiteServer.CMS.BackgroundPages.Modal.SelectFile.GetOpenWindowString(publishmentSystemInfo.PublishmentSystemID, attributeName, relatedPath), SiteServer.CMS.BackgroundPages.Modal.UploadFile.GetOpenWindowStringToTextBox(publishmentSystemInfo.PublishmentSystemID, EUploadType.File, attributeName), SiteServer.CMS.BackgroundPages.Modal.FileView.GetOpenWindowStringWithTextBoxValue(publishmentSystemInfo.PublishmentSystemID, attributeName), extendAttributeName, additionalAttributes, style);

            string extendValues = formCollection[extendAttributeName];
            if (!string.IsNullOrEmpty(extendValues))
            {
                foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                {
                    if (!string.IsNullOrEmpty(extendValue))
                    {
                        builder.AppendFormat("add_{0}('{1}',false);", attributeName, extendValue);
                    }
                }
            }

            builder.Append("</script>");

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            return builder.ToString();
        }

        private static string ParseFileUpload(string attributeName, string additionalAttributes, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);
            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""file"" class=""input_file"" {1} {2} />", attributeName, additionalAttributes, validateAttributes);

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            return builder.ToString();
        }

        private static string ParseFileUploadForTouGao(string attributeName, string additionalAttributes, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);
            string attributeNameToUpload = InputTypeParser.GetAttributeNameToUploadForTouGao(attributeName);
            string value = InputTypeParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);

            builder.AppendFormat(@"
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
<tr>
    <td width=""400"">
        <input id=""{0}"" name=""{0}"" type=""text"" class=""input-txt"" value='{1}' style=""display:;width:320px"" />
        <input id=""{2}"" name=""{2}"" style=""width:320px;display:none"" type=""file"" class=""input-txt"" />
    </td>
</tr>
<tr>
    <td valign=""top"">
        <a id=""{0}_link1"" style=""font-weight:bold"" href=""javascript:;"" onclick=""document.getElementById('{0}_link2').style.fontWeight = '';this.style.fontWeight = 'bold';document.getElementById('{0}').style.display = '';document.getElementById('{2}').style.display = 'none';"">输入 URL</a>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <a id=""{0}_link2"" href=""javascript:;"" onclick=""document.getElementById('{0}_link1').style.fontWeight = '';this.style.fontWeight = 'bold';document.getElementById('{0}').style.display = 'none';document.getElementById('{2}').style.display = '';"" >上传附件</a>
    </td>
</tr>
</table>", attributeName, value, attributeNameToUpload);

            InputTypeParser.AddHelpText(builder, styleInfo.HelpText);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            return builder.ToString();
        }

        private static string ParseRelatedField(PublishmentSystemInfo publishmentSystemInfo, string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo, NameValueCollection pageScripts)
        {
            StringBuilder builder = new StringBuilder();

            RelatedFieldInfo fieldInfo = DataProvider.RelatedFieldDAO.GetRelatedFieldInfo(styleInfo.Additional.RelatedFieldID);
            if (fieldInfo != null)
            {
                ArrayList arraylist = DataProvider.RelatedFieldItemDAO.GetRelatedFieldItemInfoArrayList(styleInfo.Additional.RelatedFieldID, 0);

                StringCollection prefixes = TranslateUtils.StringCollectionToStringCollection(fieldInfo.Prefixes);
                StringCollection suffixes = TranslateUtils.StringCollectionToStringCollection(fieldInfo.Suffixes);

                ERelatedFieldStyle style = ERelatedFieldStyleUtils.GetEnumType(styleInfo.Additional.RelatedFieldStyle);

                builder.AppendFormat(@"
<span id=""c_{0}_1"">{1}<select name=""{0}"" id=""{0}_1"" class=""select"" onchange=""getRelatedField_{2}(2);"">
	<option value="""">请选择</option>", attributeName, prefixes[0], fieldInfo.RelatedFieldID);

                string values = formCollection[attributeName];
                string value = string.Empty;
                if (!string.IsNullOrEmpty(values))
                {
                    value = values.Split(',')[0];
                }

                bool isLoad = false;
                foreach (RelatedFieldItemInfo itemInfo in arraylist)
                {
                    string selected = (!string.IsNullOrEmpty(itemInfo.ItemValue) && value == itemInfo.ItemValue) ? @" selected=""selected""" : string.Empty;
                    if (!string.IsNullOrEmpty(selected)) isLoad = true;
                    builder.AppendFormat(@"
	<option value=""{0}"" itemID=""{1}""{2}>{3}</option>", itemInfo.ItemValue, itemInfo.ID, selected, itemInfo.ItemName);
                }

                builder.AppendFormat(@"
</select>{0}</span>", suffixes[0]);

                if (fieldInfo.TotalLevel > 1)
                {
                    for (int i = 2; i <= fieldInfo.TotalLevel; i++)
                    {
                        builder.AppendFormat(@"<span id=""c_{0}_{1}"" style=""display:none"">", attributeName, i);
                        if (style == ERelatedFieldStyle.Virtical)
                        {
                            builder.Append(@"<br />");
                        }
                        else
                        {
                            builder.Append("&nbsp;");
                        }
                        builder.AppendFormat(@"
{0}<select name=""{1}"" id=""{1}_{2}"" class=""select"" onchange=""getRelatedField_{3}({2} + 1);""></select>{4}</span>
", prefixes[i - 1], attributeName, i, fieldInfo.RelatedFieldID, suffixes[i - 1]);
                    }
                }

                builder.AppendFormat(@"
<script>
function getRelatedField_{0}(level){{
    var attributeName = '{1}';
    var totalLevel = {2};
    for(i=level;i<=totalLevel;i++){{
        $('#c_' + attributeName + '_' + i).hide();
    }}
    var obj = $('#c_' + attributeName + '_' + (level - 1));
    var itemID = $('option:selected', obj).attr('itemID');
    if (itemID){{
        var url = '{3}' + itemID;
        var values = '{4}';
        var value = (values) ? values.split(',')[level - 1] : '';
        $.post(url + '&callback=?', '', function(data, textStatus){{
            var $sel = $('#' + attributeName + '_' + level);
            $('option', $sel).each(function(){{
	            $(this).remove();
            }})
            $sel.append('<option value="""">请选择</option>');
            var show = false;
            var isLoad = false;
            $.each(data, function(i, item){{
                show = true;
                var selected = '';
                if (value == item.value){{
                    isLoad = true;
                    selected = ' selected=""selected""'
                }}
                $opt = $('<option value=""' + item.value + '"" itemID=""' + item.id + '""' + selected + '>' + item.name + '</option>');
                $opt.appendTo($sel);
            }});
            if (show) $('#c_' + attributeName + '_' + level).show();
            if (isLoad && level <= totalLevel){{
                getRelatedField_{0}(level + 1);
            }}
        }}, 'jsonp');
    }}
}}
", fieldInfo.RelatedFieldID, styleInfo.AttributeName, fieldInfo.TotalLevel, BackgroundService.GetRelatedFieldUrlPrefix(publishmentSystemInfo, styleInfo.Additional.RelatedFieldID), values);

                if (isLoad)
                {
                    builder.AppendFormat(@"
$(document).ready(function(){{
    getRelatedField_{0}(2);
}});
", fieldInfo.RelatedFieldID);
                }

                builder.Append("</script>");

                InputTypeParser.AddHelpText(builder, styleInfo.HelpText);
            }
            return builder.ToString();
        }

        private static string ParseSpecifiedValue(PublishmentSystemInfo publishmentSystemInfo, int nodeID, ETableStyle tableStyle, string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            if (tableStyle == ETableStyle.GovInteractContent)
            {
                if (StringUtils.EqualsIgnoreCase(attributeName, GovInteractContentAttribute.TypeID))
                {
                    styleInfo.StyleItems = new ArrayList();
                    TableStyleItemInfo itemInfo = new TableStyleItemInfo(0, styleInfo.TableStyleID, "<<请选择>>", string.Empty, false);
                    styleInfo.StyleItems.Add(itemInfo);
                    ArrayList typeInfoArrayList = DataProvider.GovInteractTypeDAO.GetTypeInfoArrayList(nodeID);
                    foreach (GovInteractTypeInfo typeInfo in typeInfoArrayList)
                    {
                        bool isSelected = false;
                        if (!isAddAndNotPostBack)
                        {
                            isSelected = formCollection[attributeName] == typeInfo.TypeID.ToString();
                        }
                        itemInfo = new TableStyleItemInfo(0, styleInfo.TableStyleID, typeInfo.TypeName, typeInfo.TypeID.ToString(), isSelected);
                        styleInfo.StyleItems.Add(itemInfo);
                    }
                    return ParseSelectOne(attributeName, formCollection, isAddAndNotPostBack, styleInfo);
                }
                else if (StringUtils.EqualsIgnoreCase(attributeName, GovInteractContentAttribute.DepartmentID))
                {
                    styleInfo.StyleItems = new ArrayList();
                    TableStyleItemInfo itemInfo = new TableStyleItemInfo(0, styleInfo.TableStyleID, "<<请选择>>", string.Empty, false);
                    styleInfo.StyleItems.Add(itemInfo);
                    GovInteractChannelInfo channelInfo = DataProvider.GovInteractChannelDAO.GetChannelInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                    ArrayList departmentIDArrayList = GovInteractManager.GetFirstDepartmentIDArrayList(channelInfo);
                    foreach (int departmentID in departmentIDArrayList)
                    {
                        DepartmentInfo departmentInfo = DepartmentManager.GetDepartmentInfo(departmentID);
                        if (departmentInfo != null)
                        {
                            bool isSelected = false;
                            if (!isAddAndNotPostBack)
                            {
                                isSelected = formCollection[attributeName] == departmentInfo.DepartmentID.ToString();
                            }
                            itemInfo = new TableStyleItemInfo(0, styleInfo.TableStyleID, departmentInfo.DepartmentName, departmentInfo.DepartmentID.ToString(), isSelected);
                            styleInfo.StyleItems.Add(itemInfo);
                        }
                    }
                    return ParseSelectOne(attributeName, formCollection, isAddAndNotPostBack, styleInfo);
                }
            }

            return string.Empty;
        }

        private static string GetValue(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string defaultValue)
        {
            string value = string.Empty;
            if (formCollection != null && formCollection[attributeName] != null)
            {
                value = formCollection[attributeName];
            }
            if (isAddAndNotPostBack && string.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }
            return value;
        }

        private static string GetValueByForm(TableStyleInfo styleInfo, PublishmentSystemInfo publishmentSystemInfo, NameValueCollection formCollection)
        {
            string theValue = formCollection[styleInfo.AttributeName];
            if (theValue == null) theValue = string.Empty;

            EInputType inputType = styleInfo.InputType;

            if (inputType == EInputType.TextEditor)
            {
                theValue = StringUtility.TextEditorContentEncode(theValue, publishmentSystemInfo);
                ETextEditorType editorType = ETextEditorTypeUtils.GetEnumType(styleInfo.Additional.EditorTypeString);
                if (ETextEditorTypeUtils.IsInsertHtmlTranslateStlElement(editorType))
                {
                    theValue = ETextEditorTypeUtils.TranslateToStlElement(editorType, theValue);
                }
            }

            return theValue;
        }

        private static string GetValueByForm(TableStyleInfo styleInfo, PublishmentSystemInfo publishmentSystemInfo, NameValueCollection formCollection, bool isSaveImage)
        {
            string theValue = formCollection[styleInfo.AttributeName];

            if (theValue == null) theValue = string.Empty;

            EInputType inputType = styleInfo.InputType;

            if (inputType == EInputType.TextEditor)
            {
                theValue = StringUtility.TextEditorContentEncode(theValue, publishmentSystemInfo, isSaveImage && publishmentSystemInfo.Additional.IsSaveImageInTextEditor);
                ETextEditorType editorType = ETextEditorTypeUtils.GetEnumType(styleInfo.Additional.EditorTypeString);
                if (ETextEditorTypeUtils.IsInsertHtmlTranslateStlElement(editorType))
                {
                    theValue = ETextEditorTypeUtils.TranslateToStlElement(editorType, theValue);
                }
            }

            return theValue;
        }

        private static string GetValueByControl(TableStyleInfo styleInfo, PublishmentSystemInfo publishmentSystemInfo, Control containerControl)
        {
            string theValue = ControlUtils.GetInputValue(containerControl, styleInfo.AttributeName);

            if (theValue == null) theValue = string.Empty;
            //if (styleInfo.IsRequired == EBoolean.True && string.IsNullOrEmpty(theValue))
            //{
            //    throw new Exception(string.Format("“{0}”不能为空！", styleInfo.DisplayName));
            //}

            //if ((metadataInfo.DataType == EDataType.Char || metadataInfo.DataType == EDataType.VarChar || metadataInfo.DataType == EDataType.NChar || metadataInfo.DataType == EDataType.NVarChar) && theValue.Length > metadataInfo.DataLength)
            //{
            //    throw new Exception(string.Format("“{0}”字符超过了最大长度 {1} ！", styleInfo.DisplayName, metadataInfo.DataLength));
            //}

            EInputType inputType = styleInfo.InputType;

            if (inputType == EInputType.TextEditor)
            {
                theValue = StringUtility.TextEditorContentEncode(theValue, publishmentSystemInfo);
            }

            return theValue;
        }

        private static string GetValueByControl(TableStyleInfo styleInfo, PublishmentSystemInfo publishmentSystemInfo, Control containerControl, bool isSaveImg)
        {
            string theValue = ControlUtils.GetInputValue(containerControl, styleInfo.AttributeName);
            if (theValue == null) theValue = string.Empty;
            //if (styleInfo.IsRequired == EBoolean.True && string.IsNullOrEmpty(theValue))
            //{
            //    throw new Exception(string.Format("“{0}”不能为空！", styleInfo.DisplayName));
            //}

            //if ((metadataInfo.DataType == EDataType.Char || metadataInfo.DataType == EDataType.VarChar || metadataInfo.DataType == EDataType.NChar || metadataInfo.DataType == EDataType.NVarChar) && theValue.Length > metadataInfo.DataLength)
            //{
            //    throw new Exception(string.Format("“{0}”字符超过了最大长度 {1} ！", styleInfo.DisplayName, metadataInfo.DataLength));
            //}

            EInputType inputType = styleInfo.InputType;

            if (inputType == EInputType.TextEditor)
            {
                theValue = StringUtility.TextEditorContentEncode(theValue, publishmentSystemInfo, isSaveImg);
            }

            return theValue;
        }

        public static void AddValuesToAttributes(ETableStyle tableStyle, string tableName, PublishmentSystemInfo publishmentSystemInfo, ArrayList relatedIdentities, NameValueCollection formCollection, NameValueCollection attributes, bool isBackground)
        {
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false) continue;
                string theValue = InputTypeParser.GetValueByForm(styleInfo, publishmentSystemInfo, formCollection);

                if (styleInfo.InputType != EInputType.TextEditor && styleInfo.InputType != EInputType.Image && styleInfo.InputType != EInputType.File && styleInfo.InputType != EInputType.Video && styleInfo.AttributeName != BackgroundContentAttribute.LinkUrl)
                {
                    //如果是前台提交，那么需要过滤
                    if (!isBackground)
                    {
                        theValue = PageUtils.FilterSqlAndXss(theValue);
                    }
                    else if (ConfigManager.Instance.Additional.IsFilterXss)
                    {
                        //过滤xss
                        theValue = PageUtils.FilterSqlAndXss(theValue);
                    }
                }

                ExtendedAttributes.SetExtendedAttribute(attributes, styleInfo.AttributeName, theValue);
            }
        }

        public static void AddValuesToAttributes(ETableStyle tableStyle, string tableName, PublishmentSystemInfo publishmentSystemInfo, ArrayList relatedIdentities, NameValueCollection formCollection, NameValueCollection attributes)
        {
             AddValuesToAttributes(tableStyle, tableName, publishmentSystemInfo, relatedIdentities, formCollection, attributes, true);
        }

        public static void AddValuesToAttributes(ETableStyle tableStyle, string tableName, PublishmentSystemInfo publishmentSystemInfo, ArrayList relatedIdentities, NameValueCollection formCollection, NameValueCollection attributes, ArrayList dontAddAttributes)
        {
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false || dontAddAttributes.Contains(styleInfo.AttributeName.ToLower())) continue;
                string theValue = InputTypeParser.GetValueByForm(styleInfo, publishmentSystemInfo, formCollection);
                if (styleInfo.InputType != EInputType.TextEditor && styleInfo.InputType != EInputType.Image && styleInfo.InputType != EInputType.File && styleInfo.InputType != EInputType.Video && styleInfo.AttributeName != BackgroundContentAttribute.LinkUrl)
                {
                    //过滤xss
                    if (ConfigManager.Instance.Additional.IsFilterXss)
                        theValue = PageUtils.FilterSqlAndXss(theValue);
                }

                ExtendedAttributes.SetExtendedAttribute(attributes, styleInfo.AttributeName, theValue);

                if (styleInfo.Additional.IsFormatString)
                {
                    bool formatString = TranslateUtils.ToBool(formCollection[styleInfo.AttributeName + "_formatStrong"]);
                    bool formatEM = TranslateUtils.ToBool(formCollection[styleInfo.AttributeName + "_formatEM"]);
                    bool formatU = TranslateUtils.ToBool(formCollection[styleInfo.AttributeName + "_formatU"]);
                    string formatColor = formCollection[styleInfo.AttributeName + "_formatColor"];
                    string theFormatString = ContentUtility.GetTitleFormatString(formatString, formatEM, formatU, formatColor);

                    ExtendedAttributes.SetExtendedAttribute(attributes, ContentAttribute.GetFormatStringAttributeName(styleInfo.AttributeName), theFormatString);
                }

                if (styleInfo.InputType == EInputType.Image || styleInfo.InputType == EInputType.Video || styleInfo.InputType == EInputType.File)
                {
                    string attributeName = ContentAttribute.GetExtendAttributeName(styleInfo.AttributeName);
                    ExtendedAttributes.SetExtendedAttribute(attributes, attributeName, formCollection[attributeName]);
                }
            }
        }

        public static void AddValuesToAttributes(ETableStyle tableStyle, string tableName, PublishmentSystemInfo publishmentSystemInfo, ArrayList relatedIdentities, NameValueCollection formCollection, NameValueCollection attributes, ArrayList dontAddAttributes, bool isSaveImage)
        {
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false || dontAddAttributes.Contains(styleInfo.AttributeName.ToLower())) continue;
                string theValue = InputTypeParser.GetValueByForm(styleInfo, publishmentSystemInfo, formCollection, isSaveImage);

                if (styleInfo.InputType != EInputType.TextEditor && styleInfo.InputType != EInputType.Image && styleInfo.InputType != EInputType.File && styleInfo.InputType != EInputType.Video && styleInfo.AttributeName != BackgroundContentAttribute.LinkUrl)
                {
                    //过滤xss
                    if (ConfigManager.Instance.Additional.IsFilterXss)
                        theValue = PageUtils.FilterSqlAndXss(theValue);
                }

                ExtendedAttributes.SetExtendedAttribute(attributes, styleInfo.AttributeName, theValue);

                if (styleInfo.Additional.IsFormatString)
                {
                    bool formatString = TranslateUtils.ToBool(formCollection[styleInfo.AttributeName + "_formatStrong"]);
                    bool formatEM = TranslateUtils.ToBool(formCollection[styleInfo.AttributeName + "_formatEM"]);
                    bool formatU = TranslateUtils.ToBool(formCollection[styleInfo.AttributeName + "_formatU"]);
                    string formatColor = formCollection[styleInfo.AttributeName + "_formatColor"];
                    string theFormatString = ContentUtility.GetTitleFormatString(formatString, formatEM, formatU, formatColor);

                    ExtendedAttributes.SetExtendedAttribute(attributes, ContentAttribute.GetFormatStringAttributeName(styleInfo.AttributeName), theFormatString);
                }

                if (styleInfo.InputType == EInputType.Image || styleInfo.InputType == EInputType.Video || styleInfo.InputType == EInputType.File)
                {
                    string attributeName = ContentAttribute.GetExtendAttributeName(styleInfo.AttributeName);
                    ExtendedAttributes.SetExtendedAttribute(attributes, attributeName, formCollection[attributeName]);
                }
            }
        }

        public static void AddValuesToAttributes(ETableStyle tableStyle, string tableName, PublishmentSystemInfo publishmentSystemInfo, ArrayList relatedIdentities, Control containerControl, NameValueCollection attributes)
        {
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false) continue;
                string theValue = InputTypeParser.GetValueByControl(styleInfo, publishmentSystemInfo, containerControl);

                if (styleInfo.InputType != EInputType.TextEditor && styleInfo.InputType != EInputType.Image && styleInfo.InputType != EInputType.File && styleInfo.InputType != EInputType.Video && styleInfo.AttributeName != BackgroundContentAttribute.LinkUrl)
                {
                    //过滤xss
                    if (ConfigManager.Instance.Additional.IsFilterXss)
                        theValue = PageUtils.FilterSqlAndXss(theValue);
                }

                ExtendedAttributes.SetExtendedAttribute(attributes, styleInfo.AttributeName, theValue);
            }

            //ArrayList metadataInfoArrayList = TableManager.GetTableMetadataInfoArrayList(tableName);
            //foreach (TableMetadataInfo metadataInfo in metadataInfoArrayList)
            //{
            //    if (!isSystemContained && metadataInfo.IsSystem == EBoolean.True) continue;

            //    TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(tableType, metadataInfo, relatedIdentities);
            //    if (styleInfo.IsVisible == EBoolean.False) continue;

            //    string theValue = InputTypeParser.GetValueByControl(metadataInfo, styleInfo, publishmentSystemInfo, containerControl);
            //    ExtendedAttributes.SetExtendedAttribute(attributes, metadataInfo.AttributeName, theValue);
            //}
        }


        public static void AddSingleValueToAttributes(ETableStyle tableStyle, string tableName, PublishmentSystemInfo publishmentSystemInfo, ArrayList relatedIdentities, NameValueCollection formCollection, string attributeName, NameValueCollection attributes, bool isSystemContained)
        {
            TableMetadataInfo metadataInfo = TableManager.GetTableMetadataInfo(tableName, attributeName);
            if (!isSystemContained && metadataInfo.IsSystem) return;

            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(tableStyle, tableName, attributeName, relatedIdentities);
            if (styleInfo.IsVisible == false) return;

            string theValue = InputTypeParser.GetValueByForm(styleInfo, publishmentSystemInfo, formCollection);

            ExtendedAttributes.SetExtendedAttribute(attributes, metadataInfo.AttributeName, theValue);
        }

        //public static void AddRequestFormToNameValueCollection(EAuxiliaryTableType tableType, string tableName, NameValueCollection formCollection, NameValueCollection attributes, bool isSystemContained)
        //{
        //    ArrayList metadataInfoArrayList = TableManager.GetTableMetadataInfoArrayList(tableName);
        //    foreach (TableMetadataInfo metadataInfo in metadataInfoArrayList)
        //    {
        //        if (!isSystemContained && metadataInfo.IsSystem == EBoolean.True) continue;

        //        TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(tableType, metadataInfo);
        //        if (styleInfo.IsVisible == EBoolean.False) continue;

        //        string theValue = formCollection[metadataInfo.AttributeName];
        //        if (theValue == null) theValue = string.Empty;
        //        if (styleInfo.IsRequired == EBoolean.True && string.IsNullOrEmpty(theValue))
        //        {
        //            throw new Exception(string.Format("“{0}”不能为空！", styleInfo.DisplayName));
        //        }

        //        if ((metadataInfo.DataType == EDataType.Char || metadataInfo.DataType == EDataType.VarChar || metadataInfo.DataType == EDataType.NChar || metadataInfo.DataType == EDataType.NVarChar) && theValue.Length > metadataInfo.DataLength)
        //        {
        //            throw new Exception(string.Format("“{0}”字符超过了最大长度 {1} ！", styleInfo.DisplayName, metadataInfo.DataLength));
        //        }

        //        if (theValue == null)
        //            attributes.Remove(metadataInfo.AttributeName);
        //        else
        //            attributes[metadataInfo.AttributeName] = theValue;
        //    }
        //}

        public static string GetContentByTableStyle(string content, PublishmentSystemInfo publishmentSystemInfo, ETableStyle tableStyle, TableStyleInfo styleInfo)
        {
            if (!string.IsNullOrEmpty(content))
            {
                return GetContentByTableStyle(content, ",", publishmentSystemInfo, tableStyle, styleInfo, string.Empty, null, string.Empty, false);
            }
            return string.Empty;
        }

        public static string GetContentByTableStyle(string content, string separator, PublishmentSystemInfo publishmentSystemInfo, ETableStyle tableStyle, TableStyleInfo styleInfo, string formatString, StringDictionary attributes, string innerXml, bool isStlEntity)
        {
            string parsedContent = content;

            if (styleInfo.InputType == EInputType.Date)
            {
                DateTime dateTime = TranslateUtils.ToDateTime(content);
                if (dateTime != DateUtils.SqlMinValue)
                {
                    if (string.IsNullOrEmpty(formatString))
                    {
                        formatString = DateUtils.FormatStringDateOnly;
                    }
                    parsedContent = DateUtils.Format(dateTime, formatString);
                }
                else
                {
                    parsedContent = string.Empty;
                }
            }
            else if (styleInfo.InputType == EInputType.DateTime)
            {
                DateTime dateTime = TranslateUtils.ToDateTime(content);
                if (dateTime != DateUtils.SqlMinValue)
                {
                    if (string.IsNullOrEmpty(formatString))
                    {
                        formatString = DateUtils.FormatStringDateTime;
                    }
                    parsedContent = DateUtils.Format(dateTime, formatString);
                }
                else
                {
                    parsedContent = string.Empty;
                }
            }
            else if (styleInfo.InputType == EInputType.CheckBox || styleInfo.InputType == EInputType.Radio || styleInfo.InputType == EInputType.SelectMultiple || styleInfo.InputType == EInputType.SelectOne)//选择类型
            {
                ArrayList selectedTexts = new ArrayList();
                ArrayList selectedValues = TranslateUtils.StringCollectionToArrayList(content);
                ArrayList styleItems = styleInfo.StyleItems;
                if (styleItems == null)
                {
                    styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
                }
                foreach (TableStyleItemInfo itemInfo in styleItems)
                {
                    if (selectedValues.Contains(itemInfo.ItemValue))
                    {
                        if (isStlEntity)
                        {
                            selectedTexts.Add(itemInfo.ItemValue);
                        }
                        else
                        {
                            selectedTexts.Add(itemInfo.ItemTitle);
                        }
                    }
                }
                if (separator == null)
                {
                    parsedContent = TranslateUtils.ObjectCollectionToString(selectedTexts);
                }
                else
                {
                    parsedContent = TranslateUtils.ObjectCollectionToString(selectedTexts, separator);
                }
            }
            //else if (styleInfo.InputType == EInputType.TextArea)
            //{
            //    parsedContent = StringUtils.ReplaceNewlineToBR(parsedContent);
            //}
            else if (styleInfo.InputType == EInputType.TextEditor)
            {
                /****获取编辑器中内容，解析@符号，添加了远程路径处理 20151103****/
                parsedContent = StringUtility.TextEditorContentDecode(parsedContent, publishmentSystemInfo, true);
            }
            else if (styleInfo.InputType == EInputType.Image)
            {
                parsedContent = InputParserUtility.GetImageOrFlashHtml(publishmentSystemInfo, parsedContent, attributes, isStlEntity);
            }
            else if (styleInfo.InputType == EInputType.Video)
            {
                parsedContent = InputParserUtility.GetVideoHtml(publishmentSystemInfo, parsedContent, attributes, isStlEntity);
            }
            else if (styleInfo.InputType == EInputType.File)
            {
                parsedContent = InputParserUtility.GetFileHtmlWithoutCount(publishmentSystemInfo, parsedContent, attributes, innerXml, isStlEntity);
            }

            return parsedContent;
        }

        public static string GetContentByTableStyle(ContentInfo contentInfo, string separator, PublishmentSystemInfo publishmentSystemInfo, ETableStyle tableStyle, TableStyleInfo styleInfo, string formatString, int no, StringDictionary attributes, string innerXml, bool isStlEntity)
        {
            string value = contentInfo.GetExtendedAttribute(styleInfo.AttributeName);
            string parsedContent = string.Empty;

            if (styleInfo.InputType == EInputType.Date)
            {
                DateTime dateTime = TranslateUtils.ToDateTime(value);
                if (dateTime != DateUtils.SqlMinValue)
                {
                    if (string.IsNullOrEmpty(formatString))
                    {
                        formatString = DateUtils.FormatStringDateOnly;
                    }
                    parsedContent = DateUtils.Format(dateTime, formatString);
                }
            }
            else if (styleInfo.InputType == EInputType.DateTime)
            {
                DateTime dateTime = TranslateUtils.ToDateTime(value);
                if (dateTime != DateUtils.SqlMinValue)
                {
                    if (string.IsNullOrEmpty(formatString))
                    {
                        formatString = DateUtils.FormatStringDateTime;
                    }
                    parsedContent = DateUtils.Format(dateTime, formatString);
                }
            }
            else if (styleInfo.InputType == EInputType.CheckBox || styleInfo.InputType == EInputType.Radio || styleInfo.InputType == EInputType.SelectMultiple || styleInfo.InputType == EInputType.SelectOne)//选择类型
            {
                ArrayList selectedTexts = new ArrayList();
                ArrayList selectedValues = TranslateUtils.StringCollectionToArrayList(value);
                ArrayList styleItems = styleInfo.StyleItems;
                if (styleItems == null)
                {
                    styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
                }
                foreach (TableStyleItemInfo itemInfo in styleItems)
                {
                    if (selectedValues.Contains(itemInfo.ItemValue))
                    {
                        if (isStlEntity)
                        {
                            selectedTexts.Add(itemInfo.ItemValue);
                        }
                        else
                        {
                            selectedTexts.Add(itemInfo.ItemTitle);
                        }
                    }
                }
                if (separator == null)
                {
                    parsedContent = TranslateUtils.ObjectCollectionToString(selectedTexts);
                }
                else
                {
                    parsedContent = TranslateUtils.ObjectCollectionToString(selectedTexts, separator);
                }
            }
            else if (styleInfo.InputType == EInputType.TextEditor)
            {
                /****获取编辑器中内容，解析@符号，添加了远程路径处理 20151103****/
                parsedContent = StringUtility.TextEditorContentDecode(value, publishmentSystemInfo, true);
            }
            else if (styleInfo.InputType == EInputType.Image)
            {
                if (no <= 1)
                {
                    parsedContent = InputParserUtility.GetImageOrFlashHtml(publishmentSystemInfo, value, attributes, isStlEntity);
                }
                else
                {
                    string extendAttributeName = ContentAttribute.GetExtendAttributeName(styleInfo.AttributeName);
                    string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                    if (!string.IsNullOrEmpty(extendValues))
                    {
                        int index = 2;
                        foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                        {
                            if (index == no)
                            {
                                parsedContent = InputParserUtility.GetImageOrFlashHtml(publishmentSystemInfo, extendValue, attributes, isStlEntity);
                                break;
                            }
                            index++;
                        }
                    }
                }
            }
            else if (styleInfo.InputType == EInputType.Video)
            {
                if (no <= 1)
                {
                    parsedContent = InputParserUtility.GetVideoHtml(publishmentSystemInfo, value, attributes, isStlEntity);
                }
                else
                {
                    string extendAttributeName = ContentAttribute.GetExtendAttributeName(styleInfo.AttributeName);
                    string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                    if (!string.IsNullOrEmpty(extendValues))
                    {
                        int index = 2;
                        foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                        {
                            if (index == no)
                            {
                                parsedContent = InputParserUtility.GetVideoHtml(publishmentSystemInfo, extendValue, attributes, isStlEntity);
                                break;
                            }
                            index++;
                        }
                    }
                }
            }
            else if (styleInfo.InputType == EInputType.File)
            {
                if (no <= 1)
                {
                    parsedContent = InputParserUtility.GetFileHtmlWithoutCount(publishmentSystemInfo, value, attributes, innerXml, isStlEntity);
                }
                else
                {
                    string extendAttributeName = ContentAttribute.GetExtendAttributeName(styleInfo.AttributeName);
                    string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                    if (!string.IsNullOrEmpty(extendValues))
                    {
                        int index = 2;
                        foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                        {
                            if (index == no)
                            {
                                parsedContent = InputParserUtility.GetFileHtmlWithoutCount(publishmentSystemInfo, extendValue, attributes, innerXml, isStlEntity);
                                break;
                            }
                            index++;
                        }
                    }
                }
            }
            else
            {
                parsedContent = value;
            }

            return parsedContent;
        }

        private static void AddHelpText(StringBuilder builder, string helpText)
        {
            if (!string.IsNullOrEmpty(helpText))
            {
                builder.AppendFormat(@"&nbsp;<span style=""color:#999"">{0}</span>", helpText);
            }
        }
    }
}
