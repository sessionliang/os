using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core;

namespace SiteServer.CMS.Controls
{
	public class AuxiliarySingleControl : Control
	{
        private NameValueCollection formCollection;
        private bool isEdit;
        private bool isPostBack;
        private PublishmentSystemInfo publishmentSystemInfo;
        private int nodeID;
        private ETableStyle tableStyle;
        private string tableName;
        private string attributeName;

        private ArrayList relatedIdentities;

        public void SetParameters(PublishmentSystemInfo publishmentSystemInfo, int nodeID, ETableStyle tableStyle, string tableName, string attributeName, NameValueCollection formCollection, bool isEdit, bool isPostBack)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.nodeID = nodeID;
            this.tableStyle = tableStyle;
            this.tableName = tableName;
            this.attributeName = attributeName;
            this.formCollection = formCollection;
            this.isEdit = isEdit;
            this.isPostBack = isPostBack;

            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeID);
        }

		protected override void Render(HtmlTextWriter output)
		{
            if (this.formCollection == null)
            {
                if (HttpContext.Current.Request.Form != null && HttpContext.Current.Request.Form.Count > 0)
                {
                    this.formCollection = HttpContext.Current.Request.Form;
                }
                else
                {
                    this.formCollection = new NameValueCollection();
                }
            }

            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(this.tableStyle, this.tableName, this.attributeName, this.relatedIdentities);
            if (!string.IsNullOrEmpty(this.DefaultValue))
            {
                styleInfo.DefaultValue = this.DefaultValue;
            }

            if (styleInfo.IsVisible == false) return;

            string helpHtml = StringUtility.GetHelpHtml(styleInfo.DisplayName, styleInfo.HelpText);
            NameValueCollection pageScripts = new NameValueCollection();

            if (string.IsNullOrEmpty(this.AdditionalAttributes))
            {
                this.AdditionalAttributes = InputParserUtils.GetAdditionalAttributes(string.Empty, styleInfo.InputType);
            }

            styleInfo.Additional.IsValidate = TranslateUtils.ToBool(this.IsValidate);
            styleInfo.Additional.IsRequired = TranslateUtils.ToBool(this.IsRequire);

            string inputHtml = InputTypeParser.Parse(this.publishmentSystemInfo, nodeID, styleInfo, this.tableStyle, this.attributeName, this.formCollection, this.isEdit, this.isPostBack, this.AdditionalAttributes, pageScripts, true, true);

            if (string.IsNullOrEmpty(this.FormatString))
            {
                if (styleInfo.InputType == EInputType.TextEditor)
                {
                    output.Write(@"
<tr><td colspan=""4"" align=""left"">{0}</td></tr>
<tr><td colspan=""4"" align=""left"">{1}</td></tr>
", helpHtml, inputHtml);
                }
                else if (styleInfo.InputType == EInputType.Image)
                {
                    output.Write(@"
<tr height=""80"" valign=""middle""><td>{0}</td><td colspan=""3"">{1}</td></tr>
", helpHtml, inputHtml);
                }
                else
                {
                    output.Write(@"
<tr><td>{0}</td><td colspan=""3"">{1}</td></tr>
", helpHtml, inputHtml);
                }
            }
            else
            {
                output.Write(this.FormatString, helpHtml, inputHtml);
            }

            foreach (string key in pageScripts.Keys)
            {
                output.Write(pageScripts[key]);
            }
		}

        public virtual string IsValidate
        {
            get
            {
                Object state = ViewState["IsValidate"];
                if (state != null)
                {
                    return (string)state;
                }
                return string.Empty;
            }
            set
            {
                ViewState["IsValidate"] = value;
            }
        }

        public virtual string IsRequire
        {
            get
            {
                Object state = ViewState["IsRequire"];
                if (state != null)
                {
                    return (string)state;
                }
                return string.Empty;
            }
            set
            {
                ViewState["IsRequire"] = value;
            }
        }

        public virtual string ErrorMessage
        {
            get
            {
                Object state = ViewState["ErrorMessage"];
                if (state != null)
                {
                    return (string)state;
                }
                return string.Empty;
            }
            set
            {
                ViewState["ErrorMessage"] = value;
            }
        }

        public string DefaultValue
        {
            get
            {
                Object state = ViewState["DefaultValue"];
                if (state != null)
                {
                    return (string)state;
                }
                return string.Empty;
            }
            set
            {
                ViewState["DefaultValue"] = value;
            }
        }

        public string FormatString
        {
            get
            {
                Object state = ViewState["FormatString"];
                if (state != null)
                {
                    return (string)state;
                }
                return string.Empty;
            }
            set
            {
                ViewState["FormatString"] = value;
            }
        }

        public string AdditionalAttributes
        {
            get
            {
                Object state = ViewState["AdditionalAttributes"];
                if (state != null)
                {
                    return (string)state;
                }
                return string.Empty;
            }
            set
            {
                ViewState["AdditionalAttributes"] = value;
            }
        }
    }
}
