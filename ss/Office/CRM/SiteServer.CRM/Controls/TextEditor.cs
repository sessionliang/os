using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Web.Controls;
using System.Web.UI;
using BaiRong.Model;
using BaiRong.Core;
using System.Web;
using System.Collections.Specialized;

namespace SiteServer.CRM.Controls
{
    public class TextEditor : TextEditorBase, IPostBackDataHandler
    {
        public virtual string Type
        {
            get
            {
                Object state = ViewState["Type"];
                if (state != null)
                {
                    return (string)state;
                }
                return ETextEditorTypeUtils.GetValue(ETextEditorType.FCKEditor);
            }
            set
            {
                ViewState["Type"] = value;
            }
        }

        public virtual string Style
        {
            get
            {
                Object state = ViewState["Style"];
                if (state != null)
                {
                    return (string)state;
                }
                return "coolblue";
            }
            set
            {
                ViewState["Style"] = value;
            }
        }

        public virtual string IFrameID
        {
            get
            {
                Object state = ViewState["IFrameID"];
                if (state != null)
                {
                    return (string)state;
                }
                return "eWebEditor_" + this.ClientID;
            }
            set
            {
                ViewState["IFrameID"] = value;
            }
        }

        public virtual int ProjectID
        {
            get
            {
                Object state = ViewState["ProjectID"];
                if (state != null)
                {
                    return (int)state;
                }
                else if (!string.IsNullOrEmpty(base.Page.Request.QueryString["ProjectID"]))
                {
                    return TranslateUtils.ToInt(base.Page.Request.QueryString["ProjectID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ProjectID"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string controlString = string.Empty;

            ETextEditorType editorType = ETextEditorTypeUtils.GetEnumType(this.Type);

            NameValueCollection pageScripts = new NameValueCollection();
            TextEditor.ParseTextEditor(this.ProjectID, this.ClientID, null, true, pageScripts, editorType, string.Empty, this.Width, TranslateUtils.ToInt(this.Height));

            writer.Write(controlString);
        }

        public static string ParseTextEditor(int projectID, string attributeName, NameValueCollection formCollection, bool isAddAndNotPostBack, NameValueCollection pageScripts, ETextEditorType editorType, string defaultValue, string width, int height)
        {
            string value = string.Empty;
            value = StringUtils.HtmlEncode(value);

            string controlString = string.Empty;

            if (editorType == ETextEditorType.CKEditor)
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

                string editorUrl = PageUtils.GetAdminDirectoryUrlByPage("TextEditor/ckeditor");
                pageScripts[string.Format("ckEditor_{0}", attributeName)] = string.Format(@"
<script type=""text/javascript"">
CKEDITOR.replace( '{0}',
{{
        customConfig : '{1}/my_config.js',{2}
        filebrowserImageUploadUrl : '{1}/upload.aspx?type=Image&publishmentSystemID={3}',
        filebrowserFlashUploadUrl : '{1}/upload.aspx?type=Flash&publishmentSystemID={3}'
}});
</script>
", attributeName, editorUrl, size, projectID);

                controlString = string.Format(@"<textarea name=""{0}"" id=""{0}"" style=""display:none"">{1}</textarea>", attributeName, value);
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

                string editorUrl = PageUtils.GetAdminDirectoryUrlByPage("TextEditor/kindeditor");

                pageScripts["kindEditor"] = string.Format(@"<script type=""text/javascript"" src=""{0}/kindeditor-min.js""></script>", editorUrl);
                pageScripts[string.Format("kindEditor_{0}", attributeName)] = string.Format(@"
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
", attributeName, editorUrl, projectID);

                controlString = string.Format(@"<textarea name=""{0}"" id=""{0}"" style=""{1};visibility:hidden;"">{2}</textarea>", attributeName, size, value);
            }

            return controlString;
        }

        #region IPostBackDataHandler Members

        public event EventHandler TextChanged;

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string presentValue = this.Text;
            string postedValue = postCollection[postDataKey];
            if (!presentValue.Equals(postedValue))
            {
                this.Text = postedValue;
                return true;
            }
            return false;
        }

        public void RaisePostDataChangedEvent()
        {
            OnTextChanged(EventArgs.Empty);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null)
                TextChanged(this, e);
        }

        #endregion
    }
}
