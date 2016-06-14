using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Web.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Controls
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
                return ETextEditorTypeUtils.GetValue(ETextEditorType.KindEditor);
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

		public virtual int PublishmentSystemID
		{
			get
			{
				Object state = ViewState["PublishmentSystemID"];
				if (state != null)
				{
					return (int)state;
				}
				else if (!string.IsNullOrEmpty(base.Page.Request.QueryString["PublishmentSystemID"]))
				{
					return TranslateUtils.ToInt(base.Page.Request.QueryString["PublishmentSystemID"]);
				}
				return 0;
			}
			set
			{
				ViewState["PublishmentSystemID"] = value;
			}
		}

        public int PageNodeID
        {
            get
            {
                object o = ViewState["PageNodeID"];
                return (o == null) ? 0 : (int)o;
            }
            set { ViewState["PageNodeID"] = value; }
        }

        public int PageContentID
        {
            get
            {
                object o = ViewState["PageContentID"];
                return (o == null) ? 0 : (int)o;
            }
            set { ViewState["PageContentID"] = value; }
        }

		protected override void Render(HtmlTextWriter writer)
		{
            string controlString = string.Empty;
            ETextEditorType editorType = ETextEditorTypeUtils.GetEnumType(this.Type);

            string snapHostUrl;
            string uploadImageUrl;
            string uploadScrawlUrl;
            string uploadFileUrl;
            string imageManagerUrl;
            string getMovieUrl;

            string editorUrl = PageUtils.GetTextEditorUrl(editorType, true, out snapHostUrl, out uploadImageUrl, out uploadScrawlUrl, out uploadFileUrl, out imageManagerUrl, out getMovieUrl);

            if (editorType == ETextEditorType.EWebEditor)
            {
                editorUrl = PageUtils.Combine(editorUrl, string.Format("ewebeditor.htm?id={0}&style={1}&PublishmentSystemID={2}", this.ClientID, this.Style, this.PublishmentSystemID));

                string hiddenString = string.Format(@"<input name=""{0}"" id=""{1}"" type=""hidden"" value=""{2}"" />", this.UniqueID, this.ClientID, HttpUtility.HtmlEncode(this.Text));

                controlString = string.Format(@"
	{0}
	<IFRAME ID=""{1}"" SRC=""{2}"" FRAMEBORDER=""0"" SCROLLING=""no"" WIDTH=""{3}"" HEIGHT=""{4}""></IFRAME>
", hiddenString, this.IFrameID, editorUrl, this.Width, this.Height);
            }
            else if (editorType == ETextEditorType.FCKEditor)
            {
                PublishmentSystemManager.SetPublishmentSystemIDByCache(this.PublishmentSystemID);
                int widthInt = TranslateUtils.ToInt(base.Width);
                int heightInt = TranslateUtils.ToInt(base.Height);
                controlString = string.Format(@"
<script type=""text/javascript"" src=""{1}/fckeditor.js""></script>
<script type=""text/javascript"">
<!--
var oFCKeditor = new FCKeditor( '{0}' ) ;
oFCKeditor.BasePath	= '{1}/' ;
oFCKeditor.Width	= {2} ;
oFCKeditor.Height	= {3} ;
oFCKeditor.ToolbarSet = ""Default"" ;
oFCKeditor.Config['ToolbarStartExpanded'] = false ;
oFCKeditor.Value	= ""{4}"" ;
oFCKeditor.Create() ;
//-->
		</script>
", this.ClientID, editorUrl, widthInt, heightInt, this.Text.Replace("\r\n", "\\n").Replace("\"", "\\\"").Replace("/", "\\/"));
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
