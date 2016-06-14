using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;

namespace BaiRong.Controls
{
    public class AjaxTextEditor : Control
    {
        public AjaxTextEditor()
        {
        }

        private bool _canUserEdit = false;
        public virtual bool CanUserEdit
        {
            get { return _canUserEdit; }
            set { _canUserEdit = value; }
        }

        private string _updateFunction = null;
        public string AjaxFunction
        {
            get { return _updateFunction; }
            set { _updateFunction = value; }
        }

        public string EmptyText
        {
            get
            {
                Object state = ViewState["EmptyText"];
                if (state != null)
                {
                    return (string)state;
                }
                return string.Empty;
            }
            set
            {
                ViewState["EmptyText"] = value;
            }
        }

        public int Maxlength
        {
            get
            {
                Object state = ViewState["Maxlength"];
                if (state != null)
                {
                    return (int)state;
                }
                return 0;
            }
            set
            {
                ViewState["Maxlength"] = value;
            }
        }

        public bool Multiple
        {
            get
            {
                Object state = ViewState["Multiple"];
                if (state != null)
                {
                    return (bool)state;
                }
                return false;
            }
            set
            {
                ViewState["Multiple"] = value;
            }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            bool enableEdit = CanUserEdit && !string.IsNullOrEmpty(this.AjaxFunction);

            if (enableEdit)
            {
                AjaxTextEditorHelper.RenderEditable(writer, this);
            }
            else
            {
                writer.Write(this.Text);
            }
        }
    }

    public class AjaxTextEditorHelper
    {
        public static void RenderEditable(HtmlTextWriter writer, AjaxTextEditor editor)
        {
            string textDivID = editor.ClientID + "_textDiv";
            string editorDivID = editor.ClientID + "_editorDiv";

            string textContent = (string.IsNullOrEmpty(editor.Text)) ? "<i>" + editor.EmptyText + "</i>" : editor.Text + "&nbsp;";
            string editorContent = (string.IsNullOrEmpty(editor.Text)) ? string.Empty : StringUtils.ReplaceBRToNewline(editor.Text.Replace("\"", "&#34;"));

            writer.Write(@"<div id=""{0}"" title=""点击进行编辑"" onmouseover=""highlight(this);"" onmouseout=""unhighlight(this);"" onclick=""this.style.display='none';_get('{1}').style.display='';_get('{2}').focus();_get('{2}').select();"">{3}</div>", textDivID, editorDivID, editor.ClientID, textContent);
            writer.Write("<div id=\"{0}\" style=\"display:none\">", editorDivID);
            if (editor.Multiple)
            {
                writer.Write(@"<textarea style=""font-family:arial; font-size:12px; padding:3px; margin-top:0px; width:95%; height:75px;border:1px inset #e9e9ae; background-color:#ffffd3; margin-bottom:5px;"" id=""{0}"">{1}</textarea><br />", editor.ClientID, editorContent);
            }
            else
            {
                writer.Write(@"<input type=""text"" id=""{0}"" style=""font-size:14px; font-weight:bold; font-family:arial; padding:3px; width:95%; border:1px inset #e9e9ae; background-color:#ffffd3; margin-bottom:5px;"" value=""{1}"" onkeyup=""{2}_chkkey(event)"" {3}/><br />", editor.ClientID, editorContent, textDivID, (editor.Maxlength == 0) ? string.Empty : string.Format("maxlength=\"{0}\"", editor.Maxlength));
            }
            writer.Write(@"
<script>
function {0}_chkkey(evt){{
    if(evt==null) evt=window.event;
    if(evt.keyCode==13) {{
        {0}_click();
    }}
}}
function {0}_click()
{{
    _get('{0}').style.display='';
    _get('{1}').style.display='none';
    _get('{0}').innerHTML=_get('{2}').value.trim().nl2br(); + '&nbsp;';
    {3};
}}
</script>
<input type=""button"" onclick=""{0}_click()"" class=""button"" value=""保 存"" style=""font-size:12px;font-weight:normal;height:20px;line-height:18px;"" />&nbsp;&nbsp;<input type=""button"" onclick=""_get('{0}').style.display='';_get('{1}').style.display='none';"" class=""button"" value=""取 消"" style=""font-size:12px;font-weight:normal;height:20px;line-height:18px;"" /></div>", textDivID, editorDivID, editor.ClientID, editor.AjaxFunction);
        }
    }
}
