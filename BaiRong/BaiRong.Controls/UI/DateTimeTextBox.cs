using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;

namespace BaiRong.Controls
{
    public class DateTimeTextBox : TextBox
    {
        public bool Now
        {
            get
            {
                object o = ViewState["Now"];
                if (o == null)
                {
                    return false;
                }
                return (bool)o;
            }
            set { ViewState["Now"] = value; }
        }

        public bool ShowTime
        {
            get
            {
                object o = ViewState["ShowTime"];
                if (o == null)
                {
                    return false;
                }
                return (bool)o;
            }
            set { ViewState["ShowTime"] = value; }
        }

        public override string Text
        {
            get
            {
                string formatString = (this.ShowTime) ? DateUtils.FormatStringDateTime : DateUtils.FormatStringDateOnly;
                if (this.Now && string.IsNullOrEmpty(base.Text))
                {
                    base.Text = DateTime.Now.ToString(formatString);
                }
                if (!string.IsNullOrEmpty(base.Text))
                {
                    base.Text = TranslateUtils.ToDateTime(base.Text).ToString(formatString);
                }
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        public virtual DateTime DateTime
        {
            get
            {
                return TranslateUtils.ToDateTime(this.Text);
            }
            set
            {
                string formatString = (this.ShowTime) ? DateUtils.FormatStringDateTime : DateUtils.FormatStringDateOnly;
                this.Text = value.ToString(formatString);
            }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            string onfocus = (this.ShowTime) ? SiteFiles.DatePicker.OnFocus : SiteFiles.DatePicker.OnFocusDateOnly;
            this.Attributes.Add("onfocus", onfocus);
            base.AddAttributesToRender(writer);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (base.Page != null)
            {
                if (!base.Page.IsStartupScriptRegistered("DateTimeTextBox_Calendar"))
                {
                    base.Page.RegisterStartupScript("DateTimeTextBox_Calendar", string.Format(@"<script language=""javascript"" src=""{0}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.DatePicker.Js)));
                }
            }
            base.OnLoad(e);
        }
    }
}
