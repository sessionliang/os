using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using BaiRong.Core;

namespace BaiRong.Core
{
    public class TableInputParser
    {
        private TableInputParser()
        {
        }

        public const string CURRENT = "{Current}";

        public static string Parse(TableStyleInfo styleInfo, string attributeName, NameValueCollection formCollection, bool isEdit, bool isPostBack, string additionalAttributes, NameValueCollection pageScripts, bool isBackground)
        {
            string retval = string.Empty;

            bool isAddAndNotPostBack = false;
            if (!isEdit && !isPostBack) isAddAndNotPostBack = true;//添加且未提交状态

            if (styleInfo.InputType == EInputType.Text)
            {
                retval = ParseText(attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, styleInfo);
            }
            else if (styleInfo.InputType == EInputType.TextArea)
            {
                retval = ParseTextArea(attributeName, formCollection, isAddAndNotPostBack, additionalAttributes, styleInfo);
            }
            else if (styleInfo.InputType == EInputType.TextEditor)
            {
                retval = ParseTextEditor(attributeName, formCollection, isAddAndNotPostBack, pageScripts, styleInfo, isBackground);
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
                retval = ParseImage(formCollection, isAddAndNotPostBack, attributeName, additionalAttributes, styleInfo);
            }
            else if (styleInfo.InputType == EInputType.Video)
            {
                retval = ParseVideo(formCollection, isAddAndNotPostBack, attributeName, additionalAttributes, styleInfo);
            }
            else if (styleInfo.InputType == EInputType.File)
            {
                retval = ParseFile(attributeName, additionalAttributes, formCollection, isAddAndNotPostBack, styleInfo);
            }

            return retval;
        }

        public static string GetValidateHtmlString(TableStyleInfo styleInfo, out string validateAttributes)
        {
            StringBuilder builder = new StringBuilder();

            validateAttributes = string.Empty;

            if (styleInfo.Additional.IsValidate)
            {
                validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none""></span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }
            return builder.ToString();
        }

        public static string ParseText(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            string value = string.Empty;
            if (isAddAndNotPostBack)
            {
                value = styleInfo.DefaultValue;
            }
            else
            {
                if (formCollection != null && formCollection[attributeName] != null)
                {
                    value = formCollection[attributeName];
                }
            }
            value = StringUtils.HtmlEncode(value);

            //string width = styleInfo.Additional.Width;
            //if (string.IsNullOrEmpty(width))
            //{
            //    width = styleInfo.IsSingleLine ? "380px" : "220px";
            //}
            //string style = string.Format(@"style=""width:{0};""", TranslateUtils.ToWidth(width));


            string width = styleInfo.Additional.Width;
            string style = string.Empty;
            if (!string.IsNullOrEmpty(width))
            {
                style = string.Format(@"style=""width:{0};""", TranslateUtils.ToWidth(width));
            }

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""text"" class=""input_text"" value=""{1}"" {2} {3} {4} />", attributeName, value, additionalAttributes, style, validateAttributes);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none""></span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }
            return builder.ToString();
        }

        public static string ParseTextArea(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            string value = string.Empty;
            if (isAddAndNotPostBack)
            {
                value = styleInfo.DefaultValue;
            }
            else
            {
                if (formCollection != null && formCollection[attributeName] != null)
                {
                    value = formCollection[attributeName];
                }
            }
            string style = string.Empty;
            if (styleInfo.Additional.Height > 0)
            {
                style = string.Format(@"height:{0}px;", styleInfo.Additional.Height);
            }
            if (!string.IsNullOrEmpty(styleInfo.Additional.Width))
            {
                style += string.Format(@"width:{0};", TranslateUtils.ToWidth(styleInfo.Additional.Width));
            }
            if (!string.IsNullOrEmpty(style))
            {
                style = string.Format(@"style=""{0}""", style);
            }
            value = StringUtils.HtmlEncode(value);
            builder.AppendFormat(@"<textarea id=""{0}"" name=""{0}"" class=""textarea"" {1} {2} {3}>{4}</textarea>", attributeName, additionalAttributes, style, validateAttributes, value);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none""></span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            return builder.ToString();
        }

        public static string ParseTextEditor(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, NameValueCollection pageScripts, TableStyleInfo styleInfo)
        {
            return ParseTextEditor(attributeName, formCollection, isAddAndNotPostBack, pageScripts, styleInfo, true);
        }

        public static string ParseTextEditor(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, NameValueCollection pageScripts, TableStyleInfo styleInfo, bool isBackground)
        {
            string html = string.Empty;
            if (isAddAndNotPostBack)
            {
                html = styleInfo.DefaultValue;
            }
            else
            {
                if (formCollection != null && formCollection[attributeName] != null)
                {
                    html = formCollection[attributeName];
                }
            }

            string value = StringUtils.HtmlEncode(html);

            string controlString = string.Empty;
            ETextEditorType editorType = ETextEditorTypeUtils.GetEnumType(styleInfo.Additional.EditorTypeString);
            string snapHostUrl;
            string uploadImageUrl;
            string uploadScrawlUrl;
            string uploadFileUrl;
            string imageManagerUrl;
            string getMovieUrl;

            string editorUrl = PageUtils.GetTextEditorUrl(editorType, isBackground, out snapHostUrl, out uploadImageUrl, out uploadScrawlUrl, out uploadFileUrl, out imageManagerUrl, out getMovieUrl);

            if (editorType == ETextEditorType.UEditor)
            {
                if (styleInfo.Additional.Height <= 0)
                {
                    styleInfo.Additional.Height = 280;
                }
                if (string.IsNullOrEmpty(styleInfo.Additional.Width))
                {
                    styleInfo.Additional.Width = "100%";
                }

                if (pageScripts["uEditor"] == null)
                {
                    controlString += string.Format(@"<script type=""text/javascript"">window.UEDITOR_HOME_URL = ""{0}/"";window.UEDITOR_IMAGE_URL = ""{1}"";window.UEDITOR_SCRAWL_URL = ""{2}"";window.UEDITOR_FILE_URL=""{3}"";window.UEDITOR_SNAP_HOST=""{4}"";window.UEDITOR_IMAGE_MANAGER_URL=""{5}"";window.UEDITOR_MOVIE_URL=""{6}""</script><script type=""text/javascript"" src=""{0}/editor_config.js""></script><script type=""text/javascript"" src=""{0}/editor_all.js""></script>", editorUrl, uploadImageUrl, uploadScrawlUrl, uploadFileUrl, snapHostUrl, imageManagerUrl, getMovieUrl);
                }
                pageScripts["uEditor"] = string.Empty;



                controlString += string.Format(@"
<textarea id=""{0}"" name=""{0}"" style=""display:none"">{1}</textarea>
<script type=""text/javascript"">
$(function(){{
  UE.getEditor('{0}');
  $('#{0}').show();
}});
</script>", attributeName, value);

            }
            else if (editorType == ETextEditorType.EWebEditor)
            {
                string hiddenString = string.Format(@"<input name=""{0}"" id=""{0}"" type=""hidden"" value=""{1}"" />", attributeName, value);

                if (styleInfo.Additional.Height == 0)
                {
                    styleInfo.Additional.Height = 400;
                }
                if (string.IsNullOrEmpty(styleInfo.Additional.Width))
                {
                    styleInfo.Additional.Width = "550px";
                }

                controlString = string.Format(@"
	{0}
	<IFRAME ID=""{1}"" SRC=""{2}"" FRAMEBORDER=""0"" SCROLLING=""no"" WIDTH=""{3}"" HEIGHT=""{4}""></IFRAME>
", hiddenString, "eWebEditor_" + attributeName, PageUtils.Combine(editorUrl, string.Format("ewebeditor.htm?id={0}&style={1}", attributeName, "coolblue")), styleInfo.Additional.Width, styleInfo.Additional.Height);
            }
            else if (editorType == ETextEditorType.FCKEditor)
            {
                if (styleInfo.Additional.Height == 0)
                {
                    styleInfo.Additional.Height = 400;
                }
                if (string.IsNullOrEmpty(styleInfo.Additional.Width))
                {
                    styleInfo.Additional.Width = "550px";
                }

                pageScripts["fckEditor"] = string.Format(@"<script type=""text/javascript"" src=""{0}/fckeditor.js""></script>", editorUrl);
                pageScripts["oFCKeditor_" + attributeName] = string.Format(@"
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
</script>", attributeName, editorUrl, styleInfo.Additional.Height, styleInfo.Additional.Width);

                controlString = string.Format(@"
<textarea name=""{0}"" id=""{0}"">{1}</textarea>
", attributeName, value);
            }
            else if (editorType == ETextEditorType.CKEditor)
            {
                string size = string.Empty;
                if (!string.IsNullOrEmpty(styleInfo.Additional.Width))
                {
                    size = string.Format(@"
width : '{0}',", TranslateUtils.ToWidth(styleInfo.Additional.Width));
                }

                if (styleInfo.Additional.Height <= 0)
                {
                    styleInfo.Additional.Height = 280;
                }

                size += string.Format(@"
height : {0},", styleInfo.Additional.Height);

                pageScripts["ckEditor"] = string.Format(@"<script type=""text/javascript"">
if(typeof(CKEDITOR)!=""founction""){{
var head = document.getElementsByTagName('head')[0];
var script = document.createElement('script');
script.type = 'text/javascript';
script.src = ""{0}/ckeditor.js"";
head.insertBefore( script, head.firstChild );}}
</script>", editorUrl);
                //<script type=""text/javascript"" src=""{0}/ckeditor.js""></script>
                pageScripts[string.Format("ckEditor_{0}", attributeName)] = string.Format(@"
<script type=""text/javascript"">
$(document).ready(function(){{
    CKEDITOR.replace( '{0}',
    {{
            customConfig : '{1}/my_config.js',{2}
            filebrowserImageUploadUrl : '{1}/upload.aspx?type=Image',
            filebrowserFlashUploadUrl : '{1}/upload.aspx?type=Flash'
    }});
}});
</script>
", attributeName, editorUrl, size);

                controlString = string.Format(@"<textarea name=""{0}"" id=""{0}"">{1}</textarea>", attributeName, value);
            }
            else if (editorType == ETextEditorType.KindEditor)
            {
                string size = string.Empty;
                if (string.IsNullOrEmpty(styleInfo.Additional.Width))
                {
                    styleInfo.Additional.Width = "100%";
                }
                size = string.Format(@"
width : {0};", TranslateUtils.ToWidth(styleInfo.Additional.Width));

                if (styleInfo.Additional.Height <= 0)
                {
                    styleInfo.Additional.Height = 350;
                }

                size += string.Format(@"
height : {0}px", styleInfo.Additional.Height);

                pageScripts["kindEditor"] = string.Format(@"<script type=""text/javascript"" src=""{0}/kindeditor-min.js""></script>", editorUrl);
                pageScripts[string.Format("kindEditor_{0}", attributeName)] = string.Format(@"
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

                controlString = string.Format(@"<textarea name=""{0}"" id=""{0}"" style=""{1};visibility:hidden;"">{2}</textarea>", attributeName, size, value);
            }
            else if (editorType == ETextEditorType.xHtmlEditor)
            {
                if (styleInfo.Additional.Height == 0)
                {
                    styleInfo.Additional.Height = 457;
                }
                if (string.IsNullOrEmpty(styleInfo.Additional.Width))
                {
                    styleInfo.Additional.Width = "621px";
                }

                controlString = string.Format(@"
<textarea name=""{0}"" id=""{0}"" style=""display:none"">{1}</textarea>
<iframe src=""{2}"" frameBorder=""0"" marginHeight=""0"" marginWidth=""0"" scrolling=""No"" width=""{3}"" height=""{4}""></iframe>
", attributeName, value, PageUtils.Combine(editorUrl, string.Format("editor.htm?id={0}&ReadCookie=0", attributeName)), styleInfo.Additional.Width, styleInfo.Additional.Height);
            }

            return controlString;
        }

        private static string ParseDate(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, NameValueCollection pageScripts, TableStyleInfo styleInfo)
        {
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
                pageScripts["calendar"] = string.Format(@"<script language=""javascript"" src=""{0}""></script>", APIPageUtils.ParseUrlWithCase(PageUtils.GetSiteFilesUrl(SiteFiles.DatePicker.Js)));
            }

            string value = string.Empty;
            if (dateTime > DateUtils.SqlMinValue)
            {
                value = DateUtils.GetDateString(dateTime);
            }

            return string.Format(@"<input id=""{0}"" name=""{0}"" type=""text"" class=""input_text"" value=""{1}"" {2} onfocus=""{3}"" />", attributeName, value, additionalAttributes, SiteFiles.DatePicker.OnFocusDateOnly);
        }

        private static string ParseDateTime(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, string additionalAttributes, NameValueCollection pageScripts, TableStyleInfo styleInfo)
        {
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
                pageScripts["calendar"] = string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", APIPageUtils.ParseUrlWithCase(PageUtils.GetSiteFilesUrl(SiteFiles.DatePicker.Js)));
            }

            string value = string.Empty;
            if (dateTime > DateUtils.SqlMinValue)
            {
                value = DateUtils.GetDateAndTimeString(dateTime);
            }

            return string.Format(@"<input id=""{0}"" name=""{0}"" type=""text"" class=""input_text"" value=""{1}"" {2} onfocus=""{3}"" />", attributeName, value, additionalAttributes, SiteFiles.DatePicker.OnFocus);
        }

        private static string ParseCheckBox(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            ArrayList styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
            //添加默认的选择项
            if (styleItems.Count == 0 && styleInfo.TableStyleID == 0 && styleInfo.StyleItems.Count > 0)
            {
                styleItems.AddRange(styleInfo.StyleItems);
            }
            string retval = string.Empty;

            CheckBoxList checkBoxList = new CheckBoxList();
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
            retval = ControlUtils.GetControlRenderHtml(checkBoxList);

            int i = 0;
            foreach (TableStyleItemInfo styleItem in styleItems)
            {
                retval = retval.Replace(string.Format(@"name=""{0}${1}""", attributeName, i), string.Format(@"name=""{0}"" value=""{1}""", attributeName, styleItem.ItemValue));
                i++;
            }

            return retval;
        }

        private static string ParseRadio(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
            //添加默认的选择项
            if (styleItems.Count == 0 && styleInfo.TableStyleID == 0 && styleInfo.StyleItems.Count > 0)
            {
                styleItems.AddRange(styleInfo.StyleItems);
            }
            RadioButtonList radioButtonList = new RadioButtonList();
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
                radioButtonList.Items.Add(listItem);
            }
            radioButtonList.Attributes.Add("isListItem", "true");
            builder.Append(ControlUtils.GetControlRenderHtml(radioButtonList));

            return builder.ToString();
        }

        private static string ParseSelectOne(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            ArrayList styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
            //添加默认的选择项
            if (styleItems.Count == 0 && styleInfo.TableStyleID == 0 && styleInfo.StyleItems.Count > 0)
            {
                styleItems.AddRange(styleInfo.StyleItems);
            }
            StringBuilder builder = new StringBuilder();
            string selectedValue = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : null;

            //验证属性
            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            builder.AppendFormat(@"<select id=""{0}"" name=""{0}"" isListItem=""true"" {1}>", attributeName, validateAttributes);
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

            return builder.ToString();
        }

        private static string ParseSelectMultiple(string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            ArrayList styleItems = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
            //添加默认的选择项
            if (styleItems.Count == 0 && styleInfo.TableStyleID == 0 && styleInfo.StyleItems.Count > 0)
            {
                styleItems.AddRange(styleInfo.StyleItems);
            }

            StringBuilder builder = new StringBuilder();
            string selectedValues = (formCollection != null && !string.IsNullOrEmpty(formCollection[attributeName])) ? formCollection[attributeName] : string.Empty;
            ArrayList selectedValueArrayList = TranslateUtils.StringCollectionToArrayList(selectedValues);

            //验证属性
            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            builder.AppendFormat(@"<select id=""{0}"" name=""{0}""  isListItem=""true"" multiple {1}>", attributeName, validateAttributes);
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

            return builder.ToString();
        }

        private static string ParseImage(NameValueCollection formCollection, bool isAddAndNotPostBack, string attributeName, string additionalAttributes, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            string value = string.Empty;
            if (isAddAndNotPostBack)
            {
                value = styleInfo.DefaultValue;
            }
            else
            {
                if (formCollection != null && formCollection[attributeName] != null)
                {
                    value = formCollection[attributeName];
                }
            }
            value = StringUtils.HtmlEncode(value);

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""file"" class=""input_file"" {1} {2} />", attributeName, additionalAttributes, validateAttributes);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            return builder.ToString();
        }

        private static string ParseVideo(NameValueCollection formCollection, bool isAddAndNotPostBack, string attributeName, string additionalAttributes, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            string value = string.Empty;
            if (isAddAndNotPostBack)
            {
                value = styleInfo.DefaultValue;
            }
            else
            {
                if (formCollection != null && formCollection[attributeName] != null)
                {
                    value = formCollection[attributeName];
                }
            }
            value = StringUtils.HtmlEncode(value);

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""file"" class=""input_file"" {1} {2} />", attributeName, additionalAttributes, validateAttributes);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            return builder.ToString();
        }

        private static string ParseFile(string attributeName, string additionalAttributes, NameValueCollection formCollection, bool isAddAndNotPostBack, TableStyleInfo styleInfo)
        {
            StringBuilder builder = new StringBuilder();

            string validateAttributes = InputParserUtils.GetValidateAttributes(styleInfo.Additional.IsValidate, styleInfo.DisplayName, styleInfo.Additional.IsRequired, styleInfo.Additional.MinNum, styleInfo.Additional.MaxNum, styleInfo.Additional.ValidateType, styleInfo.Additional.RegExp, styleInfo.Additional.ErrorMessage);

            string value = string.Empty;
            if (isAddAndNotPostBack)
            {
                value = styleInfo.DefaultValue;
            }
            else
            {
                if (formCollection != null && formCollection[attributeName] != null)
                {
                    value = formCollection[attributeName];
                }
            }
            value = StringUtils.HtmlEncode(value);

            builder.AppendFormat(@"<input id=""{0}"" name=""{0}"" type=""file"" class=""input_file"" {1} {2} />", attributeName, additionalAttributes, validateAttributes);

            if (styleInfo.Additional.IsValidate)
            {
                builder.AppendFormat(@"&nbsp;<span id=""{0}_msg"" style=""color:red;display:none;"">*</span>", styleInfo.AttributeName);
                builder.AppendFormat(@"
<script>event_observe('{0}', 'blur', checkAttributeValue);</script>
", styleInfo.AttributeName);
            }

            return builder.ToString();
        }

        private static string GetValueByForm(TableStyleInfo styleInfo, NameValueCollection formCollection)
        {
            string theValue = formCollection[styleInfo.AttributeName];
            if (theValue == null) theValue = string.Empty;

            return theValue;
        }

        private static string GetValueByControl(TableStyleInfo styleInfo, Control containerControl)
        {
            string theValue = ControlUtils.GetInputValue(containerControl, styleInfo.AttributeName);
            if (theValue == null) theValue = string.Empty;

            return theValue;
        }

        public static void AddValuesToAttributes(ETableStyle tableStyle, string tableName, ArrayList relatedIdentities, NameValueCollection formCollection, NameValueCollection attributes)
        {
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false) continue;
                string theValue = TableInputParser.GetValueByForm(styleInfo, formCollection);
                ExtendedAttributes.SetExtendedAttribute(attributes, styleInfo.AttributeName, theValue);
            }
        }

        public static void AddValuesToAttributes(ETableStyle tableStyle, string tableName, ArrayList relatedIdentities, Control containerControl, NameValueCollection attributes)
        {
            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
            foreach (TableStyleInfo styleInfo in styleInfoArrayList)
            {
                if (styleInfo.IsVisible == false) continue;
                string theValue = TableInputParser.GetValueByControl(styleInfo, containerControl);
                ExtendedAttributes.SetExtendedAttribute(attributes, styleInfo.AttributeName, theValue);
            }
        }

        public static void AddSingleValueToAttributes(ETableStyle tableStyle, string tableName, ArrayList relatedIdentities, NameValueCollection formCollection, string attributeName, NameValueCollection attributes, bool isSystemContained)
        {
            TableMetadataInfo metadataInfo = TableManager.GetTableMetadataInfo(tableName, attributeName);
            if (!isSystemContained && metadataInfo.IsSystem) return;

            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(tableStyle, tableName, attributeName, relatedIdentities);
            if (styleInfo.IsVisible == false) return;

            string theValue = TableInputParser.GetValueByForm(styleInfo, formCollection);

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

        public static string GetContentByTableStyle(string content, TableStyleInfo styleInfo)
        {
            if (!string.IsNullOrEmpty(content))
            {
                return GetContentByTableStyle(content, ",", styleInfo, string.Empty, null, string.Empty, false);
            }
            return string.Empty;
        }

        public static string GetContentByTableStyle(string content, string separator, TableStyleInfo styleInfo, string formatString, StringDictionary attributes, string innerXml, bool isStlEntity)
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
                ArrayList styleItemArrayList = TableStyleManager.GetStyleItemArrayList(styleInfo.TableStyleID);
                foreach (TableStyleItemInfo itemInfo in styleItemArrayList)
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
            else if (styleInfo.InputType == EInputType.TextArea)
            {
                parsedContent = StringUtils.ReplaceNewlineToBR(parsedContent);
            }

            return parsedContent;
        }

        public static string GetLiveContent(string content, string separator, int wordNum, string ellipsis, string formatString, string replace, string to, bool isClearTags, StringDictionary attributes, string innerXml, bool isStlEntity, EInputType inputType)
        {
            string parsedContent = content;
            bool isPureString = true;

            if (inputType == EInputType.Date)
            {
                isPureString = false;
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
            else if (inputType == EInputType.DateTime)
            {
                isPureString = false;
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
            else if (inputType == EInputType.CheckBox || inputType == EInputType.Radio || inputType == EInputType.SelectMultiple || inputType == EInputType.SelectOne)//选择类型
            {
                isPureString = false;
                ArrayList selectedValues = TranslateUtils.StringCollectionToArrayList(content);

                if (separator == null)
                {
                    parsedContent = TranslateUtils.ObjectCollectionToString(selectedValues);
                }
                else
                {
                    parsedContent = TranslateUtils.ObjectCollectionToString(selectedValues, separator);
                }
            }
            else if (inputType == EInputType.TextArea || inputType == EInputType.TextEditor)
            {
                parsedContent = StringUtils.ReplaceNewlineToBR(parsedContent);
            }

            if (isPureString)
            {
                if (isClearTags)
                {
                    parsedContent = StringUtils.StripTags(parsedContent);
                }

                if (!string.IsNullOrEmpty(replace))
                {
                    parsedContent = StringUtils.Replace(replace, parsedContent, to);
                }

                if (wordNum > 0 && !string.IsNullOrEmpty(parsedContent))
                {
                    parsedContent = StringUtils.MaxLengthText(parsedContent, wordNum, ellipsis);
                }

                if (!string.IsNullOrEmpty(formatString))
                {
                    parsedContent = string.Format(formatString, parsedContent);
                }
            }

            return parsedContent;
        }
    }
}
