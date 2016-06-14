using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using System.Collections;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Model;
using System.Text;

using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace SiteServer.Project.Core
{
    public class FormElementParser
    {
        private FormElementParser()
        {
        }

        public const string CURRENT = "{Current}";

        public static string Parse(FormElementInfo elementInfo, string attributeName, NameValueCollection formCollection, bool isEdit, bool isPostBack, string additionalAttributes, NameValueCollection pageScripts)
        {
            string retval = string.Empty;

            bool isAddAndNotPostBack = false;
            if (!isEdit && !isPostBack) isAddAndNotPostBack = true;//添加且未提交状态

            bool oriIsValidate = elementInfo.Additional.IsValidate;

            if (elementInfo.InputType == EInputType.Text)
            {
                retval = ParseText(attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, elementInfo);
            }
            else if (elementInfo.InputType == EInputType.TextArea)
            {
                retval = ParseTextArea(attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, elementInfo);
            }
            else if (elementInfo.InputType == EInputType.TextEditor)
            {
                retval = ParseTextEditor(attributeName, formCollection, isAddAndNotPostBack, pageScripts, elementInfo);
            }
            else if (elementInfo.InputType == EInputType.SelectOne)
            {
                retval = ParseSelectOne(attributeName, formCollection, isAddAndNotPostBack, elementInfo);
            }
            else if (elementInfo.InputType == EInputType.SelectMultiple)
            {
                retval = ParseSelectMultiple(attributeName, formCollection, isAddAndNotPostBack, elementInfo);
            }
            else if (elementInfo.InputType == EInputType.CheckBox)
            {
                retval = ParseCheckBox(attributeName, formCollection, isAddAndNotPostBack, elementInfo);
            }
            else if (elementInfo.InputType == EInputType.Radio)
            {
                retval = ParseRadio(attributeName, formCollection, isAddAndNotPostBack, elementInfo);
            }
            else if (elementInfo.InputType == EInputType.Date)
            {
                retval = ParseDate(attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, pageScripts, elementInfo);
            }
            else if (elementInfo.InputType == EInputType.DateTime)
            {
                retval = ParseDateTime(attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, pageScripts, elementInfo);
            }
            else if (elementInfo.InputType == EInputType.Image)
            {
                retval = ParseImageUpload(formCollection, isAddAndNotPostBack, attributeName, additionalAttributes, elementInfo);
            }
            else if (elementInfo.InputType == EInputType.Video)
            {
                retval = ParseVideoUpload(formCollection, isAddAndNotPostBack, attributeName, additionalAttributes, elementInfo);
            }
            else if (elementInfo.InputType == EInputType.File)
            {
                retval = ParseFileUpload(attributeName, additionalAttributes, formCollection, isAddAndNotPostBack, elementInfo);
            }

            elementInfo.Additional.IsValidate = oriIsValidate;

            return retval;
        }

        public static string GetValidateHtmlString(FormElementInfo elementInfo, out string validateAttributes)
        {
            StringBuilder builder = new StringBuilder();

            validateAttributes = string.Empty;

//            if (elementInfo.Additional.IsValidate && elementInfo.InputType != EInputType.TextEditor)
//            {
//                validateAttributes = InputParserUtils.GetValidateAttributes(elementInfo.Additional.IsValidate, elementInfo.DisplayName, elementInfo.Additional.IsRequired, elementInfo.Additional.MinNum, elementInfo.Additional.MaxNum, elementInfo.Additional.ValidateType, elementInfo.Additional.RegExp, elementInfo.Additional.ErrorMessage);

//                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", elementInfo.AttributeName);
//                builder.AppendFormat(@"
//<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
//", elementInfo.AttributeName);
//            }
            return builder.ToString();
        }

        public static string ParseText(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, FormElementInfo elementInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(elementInfo.Additional.IsValidate, elementInfo.DisplayName, elementInfo.Additional.IsRequired, elementInfo.Additional.MinNum, elementInfo.Additional.MaxNum, elementInfo.Additional.ValidateType, elementInfo.Additional.RegExp, elementInfo.Additional.ErrorMessage);

            string value = FormElementParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, elementInfo.DefaultValue);
            value = StringUtils.HtmlEncode(value);
            string width = elementInfo.Additional.Width;
            if (string.IsNullOrEmpty(width))
            {
                width = elementInfo.IsSingleLine ? "380px" : "220px";
            }
            string style = string.Format(@"style=""width:{0};""", TranslateUtils.ToWidth(width));
            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""text"" class=""input_text"" value=""{1}"" {2} {3} {4} {5} />", attributeName, value, additionalAttributes, style, validateAttributes, FormElementParser.GetValidateString(elementInfo));

            FormElementParser.AddHelpText(builder, elementInfo.HelpText);

            //if (elementInfo.Additional.IsValidate)
            //{
            //    builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", elementInfo.AttributeName);
            //    builder.AppendFormat(@"<script>event_observe('{0}', 'blur', checkAttributeValue);</script>", elementInfo.AttributeName);
            //}

            if (elementInfo.Additional.IsFormatString)
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
                        isFormatted = FormElementParser.SetTitleFormatControls(formCollection[ContentAttribute.GetFormatStringAttributeName(attributeName)], out formatStrong, out formatEM, out formatU, out formatColor);
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
", elementInfo.AttributeName);

                builder.AppendFormat(@"
<div id=""div_{0}"" style=""display:{1};margin-top:5px;"">
<div class=""btn-group"" style=""float:left;"">
    <button class=""btn{5}"" style=""font-weight:bold;font-size:12px;"" onclick=""{0}_strong(this);return false;"">粗体</button>
    <button class=""btn{6}"" style=""font-style:italic;font-size:12px;"" onclick=""{0}_em(this);return false;"">斜体</button>
    <button class=""btn{7}"" style=""text-decoration:underline;font-size:12px;"" onclick=""{0}_u(this);return false;"">下划线</button>
    <button class=""btn{8}"" style=""font-size:12px;"" id=""{0}_colorBtn"" onclick=""$('#{0}_colorContainer').toggle();return false;"">颜色</button>
</div>
<div id=""{0}_colorContainer"" class=""input-append"" style=""float:left;display:none"">
    <input id=""{0}_formatColor"" name=""{0}_formatColor"" class=""input-mini"" type=""text"" value=""{9}"" placeholder=""颜色值"">
    <button class=""btn"" type=""button"" onclick=""Title_color();return false;"">确定</button>
</div>
<input id=""{0}_formatStrong"" name=""{0}_formatStrong"" type=""hidden"" value=""{2}"" />
<input id=""{0}_formatEM"" name=""{0}_formatEM"" type=""hidden"" value=""{3}"" />
<input id=""{0}_formatU"" name=""{0}_formatU"" type=""hidden"" value=""{4}"" />
</div>
", elementInfo.AttributeName, isFormatted ? string.Empty : "none", formatStrong.ToString().ToLower(), formatEM.ToString().ToLower(), formatU.ToString().ToLower(), formatStrong ? @" btn-success" : string.Empty, formatEM ? " btn-success" : string.Empty, formatU ? " btn-success" : string.Empty, !string.IsNullOrEmpty(formatColor) ? " btn-success" : string.Empty, formatColor);
            }

            return builder.ToString();
        }

        public static string ParseTextArea(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, FormElementInfo elementInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(elementInfo.Additional.IsValidate, elementInfo.DisplayName, elementInfo.Additional.IsRequired, elementInfo.Additional.MinNum, elementInfo.Additional.MaxNum, elementInfo.Additional.ValidateType, elementInfo.Additional.RegExp, elementInfo.Additional.ErrorMessage);

            string value = FormElementParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, elementInfo.DefaultValue);
            value = StringUtils.HtmlEncode(value);

            string width = elementInfo.Additional.Width;
            if (string.IsNullOrEmpty(width))
            {
                width = elementInfo.IsSingleLine ? "98%" : "220px";
            }
            int height = elementInfo.Additional.Height;
            if (height == 0)
            {
                height = 80;
            }
            string style = string.Format(@"style=""width:{0};height:{1}px;""", TranslateUtils.ToWidth(width), height);

            builder.AppendFormat(@"<textarea id=""{0}"" name=""{0}"" class=""textarea"" {1} {2} {3} {4}>{5}</textarea>", attributeName, additionalAttributes, style, validateAttributes, FormElementParser.GetValidateString(elementInfo), value);

            FormElementParser.AddHelpText(builder, elementInfo.HelpText);

//            if (elementInfo.Additional.IsValidate)
//            {
//                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", elementInfo.AttributeName);
//                builder.AppendFormat(@"
//<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
//", elementInfo.AttributeName);
//            }

            return builder.ToString();
        }

        private static string ParseTextEditor(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, NameValueCollection pageScripts, FormElementInfo elementInfo)
        {
            ETextEditorType editorType = ETextEditorTypeUtils.GetEnumType(elementInfo.Additional.EditorTypeString);

            return ParseTextEditor(attributeName, formCollection, isAddAndNotPostBack, pageScripts, editorType, elementInfo.DefaultValue, elementInfo.Additional.Width, elementInfo.Additional.Height);
        }

        public static string ParseTextEditor(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, NameValueCollection pageScripts, ETextEditorType editorType, string defaultValue, string width, int height)
        {
            string value = FormElementParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, defaultValue);
            value = FormElementParser.TextEditorContentDecode(value);
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

            string editorUrl = PageUtils.GetTextEditorUrl(editorType, false, out snapHostUrl, out uploadImageUrl, out uploadScrawlUrl, out uploadFileUrl, out imageManagerUrl, out getMovieUrl);

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
                editorUrl = PageUtils.Combine(editorUrl, string.Format("ewebeditor.htm?id={0}&style={1}", attributeName, "coolblue"));

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
	imageUploadJson : '{1}/upload_json.ashx',
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
", attributeName, editorUrl);
            }
            else if (editorType == ETextEditorType.xHtmlEditor)
            {
                editorUrl = PageUtils.Combine(editorUrl, string.Format("editor.htm?id={0}&ReadCookie=0", attributeName));

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

        private static string ParseDate(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, NameValueCollection pageScripts, FormElementInfo elementInfo)
        {
            StringBuilder builder = new StringBuilder();

            DateTime dateTime = DateUtils.SqlMinValue;
            if (isAddAndNotPostBack)
            {
                if (!string.IsNullOrEmpty(elementInfo.DefaultValue))
                {
                    if (elementInfo.DefaultValue == CURRENT)
                    {
                        dateTime = DateTime.Now;
                    }
                    else
                    {
                        dateTime = TranslateUtils.ToDateTime(elementInfo.DefaultValue);
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

            FormElementParser.AddHelpText(builder, elementInfo.HelpText);

            return builder.ToString();
        }

        private static string ParseDateTime(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, NameValueCollection pageScripts, FormElementInfo elementInfo)
        {
            StringBuilder builder = new StringBuilder();

            DateTime dateTime = DateUtils.SqlMinValue;
            if (isAddAndNotPostBack)
            {
                if (!string.IsNullOrEmpty(elementInfo.DefaultValue))
                {
                    if (elementInfo.DefaultValue == CURRENT)
                    {
                        dateTime = DateTime.Now;
                    }
                    else
                    {
                        dateTime = TranslateUtils.ToDateTime(elementInfo.DefaultValue);
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
                value = DateUtils.GetDateAndTimeString(dateTime);
            }

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""text"" class=""input_text"" value=""{1}"" {2} onfocus=""{3}"" />", attributeName, value, additionalAttributes, SiteFiles.DatePicker.OnFocus);

            FormElementParser.AddHelpText(builder, elementInfo.HelpText);

            return builder.ToString();
        }

        private static string ParseCheckBox(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, FormElementInfo elementInfo)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList styleItems = elementInfo.StyleItems;
            if (styleItems == null)
            {
                styleItems = TableStyleManager.GetStyleItemArrayList(elementInfo.ID);
            }
            string retval = string.Empty;

            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.CssClass = "checkboxlist";
            checkBoxList.ID = attributeName;
            if (elementInfo.IsHorizontal)
            {
                checkBoxList.RepeatDirection = RepeatDirection.Horizontal;
            }
            else
            {
                checkBoxList.RepeatDirection = RepeatDirection.Vertical;
            }
            checkBoxList.RepeatColumns = elementInfo.Additional.Columns;
            string selectedValues = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : string.Empty;
            ArrayList selectedValueArrayList = TranslateUtils.StringCollectionToArrayList(selectedValues);

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
            builder.Append(ControlUtils.GetControlRenderHtml(checkBoxList));

            int i = 0;
            foreach (TableStyleItemInfo styleItem in styleItems)
            {
                builder.Replace(string.Format(@"name=""{0}${1}""", attributeName, i), string.Format(@"name=""{0}"" value=""{1}""", attributeName, styleItem.ItemValue));
                i++;
            }

            FormElementParser.AddHelpText(builder, elementInfo.HelpText);

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

        private static string ParseRadio(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, FormElementInfo elementInfo)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList styleItems = elementInfo.StyleItems;
            if (styleItems == null)
            {
                styleItems = DataProvider.FormElementDAO.GetElementItemArrayList(elementInfo.ID);
            }
            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.CssClass = "radiobuttonlist";
            radioButtonList.ID = attributeName;
            if (elementInfo.IsHorizontal)
            {
                radioButtonList.RepeatDirection = RepeatDirection.Horizontal;
            }
            else
            {
                radioButtonList.RepeatDirection = RepeatDirection.Vertical;
            }
            radioButtonList.RepeatColumns = elementInfo.Additional.Columns;
            string selectedValue = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : null;

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
            builder.Append(ControlUtils.GetControlRenderHtml(radioButtonList));

            FormElementParser.AddHelpText(builder, elementInfo.HelpText);

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

        private static string ParseSelectOne(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, FormElementInfo elementInfo)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList styleItems = elementInfo.StyleItems;
            if (styleItems == null)
            {
                styleItems = TableStyleManager.GetStyleItemArrayList(elementInfo.ID);
            }

            string selectedValue = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : null;

            builder.AppendFormat(@"<select id=""{0}"" name=""{0}"" class=""select"">", attributeName);
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

            FormElementParser.AddHelpText(builder, elementInfo.HelpText);

            return builder.ToString();
        }

        private static string ParseSelectMultiple(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, FormElementInfo elementInfo)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList styleItems = elementInfo.StyleItems;
            if (styleItems == null)
            {
                styleItems = TableStyleManager.GetStyleItemArrayList(elementInfo.ID);
            }

            string selectedValues = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : string.Empty;
            ArrayList selectedValueArrayList = TranslateUtils.StringCollectionToArrayList(selectedValues);

            builder.AppendFormat(@"<select id=""{0}"" name=""{0}"" class=""select_multiple"" multiple>", attributeName);
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

            FormElementParser.AddHelpText(builder, elementInfo.HelpText);

            return builder.ToString();
        }

        public static string GetAttributeNameToUploadForTouGao(string attributeName)
        {
            return attributeName + "_uploader";
        }

        private static string ParseImageUpload(NameValueCollection formCollection, bool isAddAndNotPostBack, string attributeName, string additionalAttributes, FormElementInfo elementInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(elementInfo.Additional.IsValidate, elementInfo.DisplayName, elementInfo.Additional.IsRequired, elementInfo.Additional.MinNum, elementInfo.Additional.MaxNum, elementInfo.Additional.ValidateType, elementInfo.Additional.RegExp, elementInfo.Additional.ErrorMessage);

            string value = FormElementParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""file"" class=""input_file"" {1} {2} {3} />", attributeName, additionalAttributes, validateAttributes, FormElementParser.GetValidateString(elementInfo));

            FormElementParser.AddHelpText(builder, elementInfo.HelpText);

//            if (elementInfo.Additional.IsValidate)
//            {
//                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", elementInfo.AttributeName);
//                builder.AppendFormat(@"
//<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
//", elementInfo.AttributeName);
//            }

            return builder.ToString();
        }

        private static string ParseVideoUpload(NameValueCollection formCollection, bool isAddAndNotPostBack, string attributeName, string additionalAttributes, FormElementInfo elementInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(elementInfo.Additional.IsValidate, elementInfo.DisplayName, elementInfo.Additional.IsRequired, elementInfo.Additional.MinNum, elementInfo.Additional.MaxNum, elementInfo.Additional.ValidateType, elementInfo.Additional.RegExp, elementInfo.Additional.ErrorMessage);

            string value = FormElementParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""file"" class=""input_file"" {1} {2} {3} />", attributeName, additionalAttributes, validateAttributes, FormElementParser.GetValidateString(elementInfo));

            FormElementParser.AddHelpText(builder, elementInfo.HelpText);

//            if (elementInfo.Additional.IsValidate)
//            {
//                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", elementInfo.AttributeName);
//                builder.AppendFormat(@"
//<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
//", elementInfo.AttributeName);
//            }

            return builder.ToString();
        }

        private static string ParseFileUpload(string attributeName, string additionalAttributes, NameValueCollection formCollection, bool isAddAndNotPostBack, FormElementInfo elementInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(elementInfo.Additional.IsValidate, elementInfo.DisplayName, elementInfo.Additional.IsRequired, elementInfo.Additional.MinNum, elementInfo.Additional.MaxNum, elementInfo.Additional.ValidateType, elementInfo.Additional.RegExp, elementInfo.Additional.ErrorMessage);
            string value = FormElementParser.GetValue(attributeName, formCollection, isAddAndNotPostBack, string.Empty);

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""file"" class=""input_file"" {1} {2} {3} />", attributeName, additionalAttributes, validateAttributes, FormElementParser.GetValidateString(elementInfo));

            FormElementParser.AddHelpText(builder, elementInfo.HelpText);

//            if (elementInfo.Additional.IsValidate)
//            {
//                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", elementInfo.AttributeName);
//                builder.AppendFormat(@"
//<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
//", elementInfo.AttributeName);
//            }

            return builder.ToString();
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

        private static string GetValueByForm(FormElementInfo elementInfo, NameValueCollection formCollection)
        {
            string theValue = formCollection[elementInfo.AttributeName];
            if (theValue == null) theValue = string.Empty;

            EInputType inputType = elementInfo.InputType;

            if (inputType == EInputType.TextEditor)
            {
                theValue = FormElementParser.TextEditorContentEncode(theValue);
                ETextEditorType editorType = ETextEditorTypeUtils.GetEnumType(elementInfo.Additional.EditorTypeString);
                if (ETextEditorTypeUtils.IsInsertHtmlTranslateStlElement(editorType))
                {
                    theValue = ETextEditorTypeUtils.TranslateToStlElement(editorType, theValue);
                }
            }

            return theValue;
        }

        private static string GetValueByControl(FormElementInfo elementInfo, Control containerControl)
        {
            string theValue = ControlUtils.GetInputValue(containerControl, elementInfo.AttributeName);
            if (theValue == null) theValue = string.Empty;
            //if (elementInfo.IsRequired == EBoolean.True && string.IsNullOrEmpty(theValue))
            //{
            //    throw new Exception(string.Format("“{0}”不能为空！", elementInfo.DisplayName));
            //}

            //if ((metadataInfo.DataType == EDataType.Char || metadataInfo.DataType == EDataType.VarChar || metadataInfo.DataType == EDataType.NChar || metadataInfo.DataType == EDataType.NVarChar) && theValue.Length > metadataInfo.DataLength)
            //{
            //    throw new Exception(string.Format("“{0}”字符超过了最大长度 {1} ！", elementInfo.DisplayName, metadataInfo.DataLength));
            //}

            EInputType inputType = elementInfo.InputType;

            if (inputType == EInputType.TextEditor)
            {
                theValue = FormElementParser.TextEditorContentEncode(theValue);
            }

            return theValue;
        }

        public static void AddValuesToAttributes(int pageID, int groupID, NameValueCollection formCollection, NameValueCollection attributes)
        {
            List<FormElementInfo> elementInfoArrayList = DataProvider.FormElementDAO.GetFormElementInfoList(pageID, groupID);
            foreach (FormElementInfo elementInfo in elementInfoArrayList)
            {
                if (elementInfo.IsVisible == false) continue;
                string theValue = FormElementParser.GetValueByForm(elementInfo, formCollection);
                ExtendedAttributes.SetExtendedAttribute(attributes, elementInfo.AttributeName, theValue);
            }
        }

        public static void AddValuesToAttributes(int pageID, int groupID, NameValueCollection formCollection, NameValueCollection attributes, ArrayList dontAddAttributes)
        {
            List<FormElementInfo> elementInfoArrayList = DataProvider.FormElementDAO.GetFormElementInfoList(pageID, groupID);
            foreach (FormElementInfo elementInfo in elementInfoArrayList)
            {
                if (elementInfo.IsVisible == false || dontAddAttributes.Contains(elementInfo.AttributeName.ToLower())) continue;
                string theValue = FormElementParser.GetValueByForm(elementInfo, formCollection);
                ExtendedAttributes.SetExtendedAttribute(attributes, elementInfo.AttributeName, theValue);

                if (elementInfo.Additional.IsFormatString)
                {
                    bool formatString = TranslateUtils.ToBool(formCollection[elementInfo.AttributeName + "_formatStrong"]);
                    bool formatEM = TranslateUtils.ToBool(formCollection[elementInfo.AttributeName + "_formatEM"]);
                    bool formatU = TranslateUtils.ToBool(formCollection[elementInfo.AttributeName + "_formatU"]);
                    string formatColor = formCollection[elementInfo.AttributeName + "_formatColor"];
                    string theFormatString = FormElementParser.GetTitleFormatString(formatString, formatEM, formatU, formatColor);

                    ExtendedAttributes.SetExtendedAttribute(attributes, ContentAttribute.GetFormatStringAttributeName(elementInfo.AttributeName), theFormatString);
                }

                if (elementInfo.InputType == EInputType.Image || elementInfo.InputType == EInputType.Video || elementInfo.InputType == EInputType.File)
                {
                    string attributeName = ContentAttribute.GetExtendAttributeName(elementInfo.AttributeName);
                    ExtendedAttributes.SetExtendedAttribute(attributes, attributeName, formCollection[attributeName]);
                }
            }
        }

        public static void AddValuesToAttributes(int pageID, int groupID, Control containerControl, NameValueCollection attributes)
        {
            List<FormElementInfo> elementInfoArrayList = DataProvider.FormElementDAO.GetFormElementInfoList(pageID, groupID);
            foreach (FormElementInfo elementInfo in elementInfoArrayList)
            {
                if (elementInfo.IsVisible == false) continue;
                string theValue = FormElementParser.GetValueByControl(elementInfo, containerControl);
                ExtendedAttributes.SetExtendedAttribute(attributes, elementInfo.AttributeName, theValue);
            }

            //ArrayList metadataInfoArrayList = TableManager.GetTableMetadataInfoArrayList(tableName);
            //foreach (TableMetadataInfo metadataInfo in metadataInfoArrayList)
            //{
            //    if (!isSystemContained && metadataInfo.IsSystem == EBoolean.True) continue;

            //    FormElementInfo elementInfo = TableStyleManager.GetFormElementInfo(tableType, metadataInfo, relatedIdentities);
            //    if (elementInfo.IsVisible == EBoolean.False) continue;

            //    string theValue = FormElementParser.GetValueByControl(metadataInfo, elementInfo, containerControl);
            //    ExtendedAttributes.SetExtendedAttribute(attributes, metadataInfo.AttributeName, theValue);
            //}
        }

        public static void AddSingleValueToAttributes(int pageID, int groupID, NameValueCollection formCollection, string attributeName, NameValueCollection attributes, bool isSystemContained)
        {
            FormElementInfo elementInfo = DataProvider.FormElementDAO.GetFormElementInfo(pageID, groupID, attributeName);
            if (elementInfo.IsVisible == false) return;

            string theValue = FormElementParser.GetValueByForm(elementInfo, formCollection);

            ExtendedAttributes.SetExtendedAttribute(attributes, attributeName, theValue);
        }

        //public static void AddRequestFormToNameValueCollection(EAuxiliaryTableType tableType, string tableName, NameValueCollection formCollection, NameValueCollection attributes, bool isSystemContained)
        //{
        //    ArrayList metadataInfoArrayList = TableManager.GetTableMetadataInfoArrayList(tableName);
        //    foreach (TableMetadataInfo metadataInfo in metadataInfoArrayList)
        //    {
        //        if (!isSystemContained && metadataInfo.IsSystem == EBoolean.True) continue;

        //        FormElementInfo elementInfo = TableStyleManager.GetFormElementInfo(tableType, metadataInfo);
        //        if (elementInfo.IsVisible == EBoolean.False) continue;

        //        string theValue = formCollection[metadataInfo.AttributeName];
        //        if (theValue == null) theValue = string.Empty;
        //        if (elementInfo.IsRequired == EBoolean.True && string.IsNullOrEmpty(theValue))
        //        {
        //            throw new Exception(string.Format("“{0}”不能为空！", elementInfo.DisplayName));
        //        }

        //        if ((metadataInfo.DataType == EDataType.Char || metadataInfo.DataType == EDataType.VarChar || metadataInfo.DataType == EDataType.NChar || metadataInfo.DataType == EDataType.NVarChar) && theValue.Length > metadataInfo.DataLength)
        //        {
        //            throw new Exception(string.Format("“{0}”字符超过了最大长度 {1} ！", elementInfo.DisplayName, metadataInfo.DataLength));
        //        }

        //        if (theValue == null)
        //            attributes.Remove(metadataInfo.AttributeName);
        //        else
        //            attributes[metadataInfo.AttributeName] = theValue;
        //    }
        //}

        public static string GetContent(string content, FormElementInfo elementInfo)
        {
            if (!string.IsNullOrEmpty(content))
            {
                return GetContent(content, ",", elementInfo, string.Empty, null, string.Empty, false);
            }
            return string.Empty;
        }

        public static string GetContent(string content, string separator, FormElementInfo elementInfo, string formatString, StringDictionary attributes, string innerXml, bool isStlEntity)
        {
            string parsedContent = content;

            if (elementInfo.InputType == EInputType.Date)
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
            else if (elementInfo.InputType == EInputType.DateTime)
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
            else if (elementInfo.InputType == EInputType.CheckBox || elementInfo.InputType == EInputType.Radio || elementInfo.InputType == EInputType.SelectMultiple || elementInfo.InputType == EInputType.SelectOne)//选择类型
            {
                ArrayList selectedTexts = new ArrayList();
                ArrayList selectedValues = TranslateUtils.StringCollectionToArrayList(content);
                ArrayList styleItems = elementInfo.StyleItems;
                if (styleItems == null)
                {
                    styleItems = TableStyleManager.GetStyleItemArrayList(elementInfo.ID);
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
            //else if (elementInfo.InputType == EInputType.TextArea)
            //{
            //    parsedContent = StringUtils.ReplaceNewlineToBR(parsedContent);
            //}
            else if (elementInfo.InputType == EInputType.TextEditor)
            {
                parsedContent = FormElementParser.TextEditorContentDecode(parsedContent);
            }
            else if (elementInfo.InputType == EInputType.Image)
            {
                parsedContent = FormElementParser.GetImageOrFlashHtml(parsedContent, attributes, isStlEntity);
            }
            else if (elementInfo.InputType == EInputType.Video)
            {
                parsedContent = FormElementParser.GetVideoHtml(parsedContent, attributes, isStlEntity);
            }
            else if (elementInfo.InputType == EInputType.File)
            {
                parsedContent = FormElementParser.GetFileHtmlWithoutCount(parsedContent, attributes, innerXml);
            }

            return parsedContent;
        }

        public static string GetContent(ContentInfo contentInfo, string separator, FormElementInfo elementInfo, string formatString, int no, StringDictionary attributes, string innerXml, bool isStlEntity)
        {
            string value = contentInfo.GetExtendedAttribute(elementInfo.AttributeName);
            string parsedContent = string.Empty;

            if (elementInfo.InputType == EInputType.Date)
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
            else if (elementInfo.InputType == EInputType.DateTime)
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
            else if (elementInfo.InputType == EInputType.CheckBox || elementInfo.InputType == EInputType.Radio || elementInfo.InputType == EInputType.SelectMultiple || elementInfo.InputType == EInputType.SelectOne)//选择类型
            {
                ArrayList selectedTexts = new ArrayList();
                ArrayList selectedValues = TranslateUtils.StringCollectionToArrayList(value);
                ArrayList styleItems = elementInfo.StyleItems;
                if (styleItems == null)
                {
                    styleItems = TableStyleManager.GetStyleItemArrayList(elementInfo.ID);
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
            else if (elementInfo.InputType == EInputType.TextEditor)
            {
                parsedContent = FormElementParser.TextEditorContentDecode(value);
            }
            else if (elementInfo.InputType == EInputType.Image)
            {
                if (no <= 1)
                {
                    parsedContent = FormElementParser.GetImageOrFlashHtml(value, attributes, isStlEntity);
                }
                else
                {
                    string extendAttributeName = ContentAttribute.GetExtendAttributeName(elementInfo.AttributeName);
                    string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                    if (!string.IsNullOrEmpty(extendValues))
                    {
                        int index = 2;
                        foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                        {
                            if (index == no)
                            {
                                parsedContent = FormElementParser.GetImageOrFlashHtml(extendValue, attributes, isStlEntity);
                                break;
                            }
                            index++;
                        }
                    }
                }
            }
            else if (elementInfo.InputType == EInputType.Video)
            {
                if (no <= 1)
                {
                    parsedContent = FormElementParser.GetVideoHtml(value, attributes, isStlEntity);
                }
                else
                {
                    string extendAttributeName = ContentAttribute.GetExtendAttributeName(elementInfo.AttributeName);
                    string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                    if (!string.IsNullOrEmpty(extendValues))
                    {
                        int index = 2;
                        foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                        {
                            if (index == no)
                            {
                                parsedContent = FormElementParser.GetVideoHtml(extendValue, attributes, isStlEntity);
                                break;
                            }
                            index++;
                        }
                    }
                }
            }
            else if (elementInfo.InputType == EInputType.File)
            {
                if (no <= 1)
                {
                    parsedContent = FormElementParser.GetFileHtmlWithoutCount(value, attributes, innerXml);
                }
                else
                {
                    string extendAttributeName = ContentAttribute.GetExtendAttributeName(elementInfo.AttributeName);
                    string extendValues = contentInfo.GetExtendedAttribute(extendAttributeName);
                    if (!string.IsNullOrEmpty(extendValues))
                    {
                        int index = 2;
                        foreach (string extendValue in TranslateUtils.StringCollectionToArrayList(extendValues))
                        {
                            if (index == no)
                            {
                                parsedContent = FormElementParser.GetFileHtmlWithoutCount(extendValue, attributes, innerXml);
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

        public static string GetTitleFormatString(bool isStrong, bool isEM, bool isU, string color)
        {
            return string.Format("{0}_{1}_{2}_{3}", isStrong, isEM, isU, color);
        }

        public static bool SetTitleFormatControls(string titleFormatString, out bool formatStrong, out bool formatEM, out bool formatU, out string formatColor)
        {
            bool isTitleFormatted = false;

            formatStrong = formatEM = formatU = false;
            formatColor = string.Empty;

            if (!string.IsNullOrEmpty(titleFormatString))
            {
                string[] formats = titleFormatString.Split('_');
                if (formats.Length == 4)
                {
                    formatStrong = TranslateUtils.ToBool(formats[0]);
                    formatEM = TranslateUtils.ToBool(formats[1]);
                    formatU = TranslateUtils.ToBool(formats[2]);
                    formatColor = formats[3];
                    if (formatStrong || formatEM || formatU || !string.IsNullOrEmpty(formatColor))
                    {
                        isTitleFormatted = true;
                    }
                }
            }
            return isTitleFormatted;
        }

        public static string TextEditorContentDecode(string content)
        {
            string publishmentSystemUrl = ConfigUtils.Instance.ApplicationPath;
            StringBuilder builder = new StringBuilder(content);

            if (publishmentSystemUrl == "/")
            {
                publishmentSystemUrl = string.Empty;
            }

            builder.Replace("href=\"@", "href=\"" + publishmentSystemUrl);
            builder.Replace("href='@", "href='" + publishmentSystemUrl);
            builder.Replace("href=@", "href=" + publishmentSystemUrl);
            builder.Replace("href=&quot;@", "href=&quot;" + publishmentSystemUrl);
            builder.Replace("src=\"@", "src=\"" + publishmentSystemUrl);
            builder.Replace("src='@", "src='" + publishmentSystemUrl);
            builder.Replace("src=@", "src=" + publishmentSystemUrl);
            builder.Replace("src=&quot;@", "src=&quot;" + publishmentSystemUrl);

            return builder.ToString();
        }

        public static string TextEditorContentEncode(string content)
        {
            string publishmentSystemUrl = ConfigUtils.Instance.ApplicationPath;

            StringBuilder builder = new StringBuilder(content);

            if (publishmentSystemUrl == "/")
            {
                publishmentSystemUrl = string.Empty;
            }

            builder.Replace("href=\"" + publishmentSystemUrl, "href=\"@");
            builder.Replace("href='" + publishmentSystemUrl, "href='@");
            builder.Replace("href=" + publishmentSystemUrl, "href=@");
            builder.Replace("href=&quot;" + publishmentSystemUrl, "href=&quot;@");
            builder.Replace("src=\"" + publishmentSystemUrl, "src=\"@");
            builder.Replace("src='" + publishmentSystemUrl, "src='@");
            builder.Replace("src=" + publishmentSystemUrl, "src=@");
            builder.Replace("src=&quot;" + publishmentSystemUrl, "src=&quot;@");

            builder.Replace("@'@", "'@");
            builder.Replace("@\"@", "\"@");

            return builder.ToString();
        }

        public static string GetImageOrFlashHtml(string imageUrl, StringDictionary attributes, bool isStlEntity)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                imageUrl = PageUtils.ParseNavigationUrl(imageUrl);
                if (isStlEntity)
                {
                    retval = imageUrl;
                }
                else
                {
                    if (!imageUrl.ToUpper().Trim().EndsWith(".SWF"))
                    {
                        HtmlImage htmlImage = new HtmlImage();
                        ControlUtils.AddAttributesIfNotExists(htmlImage, attributes);
                        htmlImage.Src = imageUrl;
                        retval = ControlUtils.GetControlRenderHtml(htmlImage);
                    }
                    else
                    {
                        int width = 100;
                        int height = 100;
                        if (attributes != null)
                        {
                            if (!string.IsNullOrEmpty(attributes["width"]))
                            {
                                try
                                {
                                    width = int.Parse(attributes["width"]);
                                }
                                catch { }
                            }
                            if (!string.IsNullOrEmpty(attributes["height"]))
                            {
                                try
                                {
                                    height = int.Parse(attributes["height"]);
                                }
                                catch { }
                            }
                        }
                        retval = string.Format(@"
<object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0"" width=""{0}"" height=""{1}"">
                <param name=""movie"" value=""{2}"">
                <param name=""quality"" value=""high"">
                <param name=""wmode"" value=""transparent"">
                <embed src=""{2}"" width=""{0}"" height=""{1}"" quality=""high"" pluginspage=""http://www.macromedia.com/go/getflashplayer"" type=""application/x-shockwave-flash"" wmode=""transparent""></embed></object>
", width, height, imageUrl);
                    }
                }
            }
            return retval;
        }

        public static string GetVideoHtml(string videoUrl, StringDictionary attributes, bool isStlEntity)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(videoUrl))
            {
                videoUrl = PageUtils.ParseNavigationUrl(videoUrl);
                if (isStlEntity)
                {
                    retval = videoUrl;
                }
                else
                {
                    retval = string.Format(@"
<embed src=""{0}"" allowfullscreen=""true"" flashvars=""controlbar=over&autostart={1}&image={2}&file={3}"" width=""{4}"" height=""{5}""/>
", PageUtils.GetSiteFilesUrl(SiteFiles.BRPlayer.Swf), true.ToString().ToLower(), string.Empty, videoUrl, 450, 350);
                }
            }
            return retval;
        }

        public static string GetFileHtmlWithoutCount(string fileUrl, StringDictionary attributes, string innerXml)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(fileUrl))
            {
                HtmlAnchor stlAnchor = new HtmlAnchor();
                ControlUtils.AddAttributesIfNotExists(stlAnchor, attributes);
                stlAnchor.HRef = PageUtils.ParseNavigationUrl(fileUrl);
                if (string.IsNullOrEmpty(innerXml))
                {
                    stlAnchor.InnerHtml = PageUtils.GetFileNameFromUrl(fileUrl);
                }
                else
                {
                    stlAnchor.InnerHtml = innerXml;
                }

                retval = ControlUtils.GetControlRenderHtml(stlAnchor);
            }
            return retval;
        }

        private static string GetValidateString(FormElementInfo elementInfo)
        {
            if (elementInfo.Additional.IsValidate && elementInfo.Additional.IsRequired)
            {
                return "required";
            }
            return string.Empty;
        }
    }
}
