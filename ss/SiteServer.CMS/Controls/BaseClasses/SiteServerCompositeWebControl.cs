using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;

namespace SiteServer.CMS.Controls
{
    public abstract class SiteServerCompositeWebControl : BaiRong.Controls.CompositeWebControl
	{
        private Exception exception = null;
        protected void SetException(Exception ex)
        {
            this.exception = ex;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.exception == null)
            {
                base.Render(writer);
            }
            else
            {
                writer.Write(this.exception.Message);
            }
        }
	}
}
