using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Web.Controls;

namespace BaiRong.Controls
{
    public class BREditor : TextEditorBase, IPostBackDataHandler
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
                return ETextEditorTypeUtils.GetValue(ETextEditorType.UEditor);
            }
            set
            {
                ViewState["Type"] = value;
            }
        }

		public virtual bool IsBackground
		{
			get
			{
                Object state = ViewState["IsBackground"];
				if (state != null)
				{
					return (bool)state;
				}
				return true;
			}
			set
			{
                ViewState["IsBackground"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
            string controlString = string.Empty;
            ETextEditorType editorType = ETextEditorType.UEditor;

            string snapHostUrl;
            string uploadImageUrl;
            string uploadScrawlUrl;
            string uploadFileUrl;
            string imageManagerUrl;
            string getMovieUrl;

            string editorUrl = PageUtils.GetTextEditorUrl(editorType, this.IsBackground, out snapHostUrl, out uploadImageUrl, out uploadScrawlUrl, out uploadFileUrl, out imageManagerUrl, out getMovieUrl);

            if (editorType == ETextEditorType.UEditor)
            {
                if (string.IsNullOrEmpty(base.Height) || base.Height == "0")
                {
                    base.Height = "280";
                }
                if (string.IsNullOrEmpty(base.Width) || base.Width == "0")
                {
                    base.Width = "100%";
                }

                controlString = string.Format(@"<script type=""text/javascript"">window.UEDITOR_HOME_URL = ""{0}/"";window.UEDITOR_IMAGE_URL = ""{1}"";window.UEDITOR_SCRAWL_URL = ""{2}"";window.UEDITOR_FILE_URL=""{3}"";window.UEDITOR_SNAP_HOST=""{4}"";window.UEDITOR_IMAGE_MANAGER_URL=""{5}"";window.UEDITOR_MOVIE_URL=""{6}""</script><script type=""text/javascript"" src=""{0}/editor_config.js""></script><script type=""text/javascript"" src=""{0}/editor_all_min.js""></script>", editorUrl, uploadImageUrl, uploadScrawlUrl, uploadFileUrl, snapHostUrl, imageManagerUrl, getMovieUrl);

                controlString += string.Format(@"
<textarea id=""{0}"" name=""{0}"" style=""display:none"">{1}</textarea>
<script type=""text/javascript"">
var _editor;
$(function(){{
    _editor = UE.getEditor('{0}', {{initialFrameHeight:'{2}',initialFrameWidth:'{3}',autoHeightEnabled:false }});
    $('#{0}').show();
}});
</script>", this.ClientID, HttpUtility.HtmlEncode(this.Text), base.Height, base.Width);
            }
			writer.Write(controlString);
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
