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
    public class XHtmlEditor : TextEditorBase, IPostBackDataHandler
	{
		protected override void Render(HtmlTextWriter writer)
		{
            string controlString = string.Empty;

            string snapHostUrl;
            string uploadImageUrl;
            string uploadScrawlUrl;
            string uploadFileUrl;
            string imageManagerUrl;
            string getMovieUrl;

            string editorUrl = PageUtils.GetTextEditorUrl(ETextEditorType.xHtmlEditor, false, out snapHostUrl, out uploadImageUrl, out uploadScrawlUrl, out uploadFileUrl, out imageManagerUrl, out getMovieUrl);

            editorUrl = PageUtils.Combine(editorUrl, string.Format("editor.htm?id={0}&ReadCookie=0", this.ClientID));

            int widthInt = TranslateUtils.ToInt(base.Width);
            int heightInt = TranslateUtils.ToInt(base.Height);

            if (heightInt == 0)
            {
                heightInt = 457;
            }
            if (widthInt == 0)
            {
                widthInt = 621;
            }

            controlString = string.Format(@"
<textarea name=""{0}"" id=""{0}"" style=""display:none"">{1}</textarea>
<iframe id=""{0}_iframe"" src=""{2}"" frameBorder=""0"" marginHeight=""0"" marginWidth=""0"" scrolling=""No"" width=""{3}"" height=""{4}""></iframe>
", this.ClientID, HttpUtility.HtmlEncode(this.Text), editorUrl, widthInt, heightInt);
            
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
