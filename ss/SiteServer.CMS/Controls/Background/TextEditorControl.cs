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
	public class TextEditorControl : Control
	{
        private NameValueCollection formCollection;
        private bool isEdit;
        private bool isPostBack;
        private PublishmentSystemInfo publishmentSystemInfo;
        private string attributeName;

        public void SetParameters(PublishmentSystemInfo publishmentSystemInfo, string attributeName, NameValueCollection formCollection, bool isEdit, bool isPostBack)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.attributeName = attributeName;
            this.formCollection = formCollection;
            this.isEdit = isEdit;
            this.isPostBack = isPostBack;
        }

		protected override void Render(HtmlTextWriter output)
		{
            if (!base.Page.IsPostBack)
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

                NameValueCollection pageScripts = new NameValueCollection();

                bool isAddAndNotPostBack = false;
                if (!isEdit && !isPostBack) isAddAndNotPostBack = true;//Ìí¼ÓÇÒÎ´Ìá½»×´Ì¬

                string inputHtml = InputTypeParser.ParseTextEditor(this.publishmentSystemInfo, this.attributeName, this.formCollection, isAddAndNotPostBack, pageScripts, publishmentSystemInfo.Additional.TextEditorType, string.Empty, string.Empty, 0, true);

                output.Write(inputHtml);

                foreach (string key in pageScripts.Keys)
                {
                    output.Write(pageScripts[key]);
                }
            }
		}
    }
}
