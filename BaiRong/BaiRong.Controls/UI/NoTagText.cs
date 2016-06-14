using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;

namespace BaiRong.Controls
{
	public class NoTagText : Control
	{
		public NoTagText()
		{
		}

		public virtual String Text 
		{
			get 
			{
				Object state = ViewState["Text"];
				if ( state != null ) 
				{
					return (String)state;
				}
				return string.Empty;
			}
			set 
			{
				ViewState["Text"] = value;
			}
		}

        public virtual String NoWrap
        {
            get
            {
                Object state = ViewState["NoWrap"];
                if (state != null)
                {
                    return (String)state;
                }
                return string.Empty;
            }
            set
            {
                ViewState["NoWrap"] = value;
            }
        }

		protected override void Render(HtmlTextWriter writer)
		{
            if (!string.IsNullOrEmpty(this.NoWrap))
            {
                writer.Write("<nobr>");
            }
			writer.Write(this.Text);
            if (!string.IsNullOrEmpty(this.NoWrap))
            {
                writer.Write("</nobr>");
            }
		}
	}

}
