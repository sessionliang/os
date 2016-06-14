using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using System.Web.UI;

namespace BaiRong.Controls
{
    public class Help : Control
	{
        public string HelpText
        {
            get
            {
                return this.ViewState["HelpText"] as string;
            }
            set
            {
                this.ViewState["HelpText"] = value;
            }
        }

        public string Text
        {
            get
            {
                return this.ViewState["Text"] as string;
            }
            set
            {
                this.ViewState["Text"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.Text);
        }
	}
}
