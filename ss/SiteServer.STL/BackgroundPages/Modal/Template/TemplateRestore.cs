using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using System.Collections.Generic;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser;
using SiteServer.STL.Parser.TemplateDesign;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
	public class TemplateRestore : BackgroundBasePage
	{
        public DropDownList ddlLogID;
        public TextBox tbContent;

        private int templateID;
        private string includeUrl;
        private int logID;
        private bool isInDesignPage;

        protected override bool IsSinglePage
        {
            get{ return true; }
        }

        public static string GetOpenLayerString(int publishmentSystemID, int templateID, string includeUrl, bool isInDesignPage)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isInDesignPage", isInDesignPage.ToString());
            return JsUtils.Layer.GetOpenLayerString("还原历史版本", PageUtils.GetSTLUrl("modal_templateRestore.aspx"), arguments);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.templateID = base.GetIntQueryString("templateID");
            this.includeUrl = base.GetQueryString("includeUrl");
            this.logID = base.GetIntQueryString("logID");
            this.isInDesignPage = base.GetBoolQueryString("isInDesignPage");
           
			if (!IsPostBack)
			{
                Dictionary<int, string> logDictionary = DataProvider.TemplateLogDAO.GetLogIDWithNameDictionary(base.PublishmentSystemID, this.templateID);
                foreach (var value in logDictionary)
                {
                    ListItem listItem = new ListItem(value.Value, value.Key.ToString());
                    this.ddlLogID.Items.Add(listItem);
                }
                if (this.logID > 0)
                {
                    ControlUtils.SelectListItems(this.ddlLogID, this.logID.ToString());
                }

                if (this.ddlLogID.Items.Count > 0)
                {
                    if (this.logID == 0)
                    {
                        this.logID = TranslateUtils.ToInt(this.ddlLogID.Items[0].Value);
                    }
                    this.tbContent.Text = DataProvider.TemplateLogDAO.GetTemplateContent(this.logID);
                }
			}
		}

        public void ddlLogID_SelectedIndexChanged(object sender, EventArgs E)
        {
            PageUtils.Redirect(PageUtils.GetSTLUrl(string.Format("modal_templateRestore.aspx?PublishmentSystemID={0}&templateID={1}&includeUrl={2}&logID={3}&isInDesignPage={4}", base.PublishmentSystemID, this.templateID, this.includeUrl, this.ddlLogID.SelectedValue, this.isInDesignPage)));
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            int templateLogID = TranslateUtils.ToInt(this.ddlLogID.SelectedValue);
            if (templateLogID == 0)
            {
                base.FailMessage("当前模板不存在历史版本，无法进行还原");
            }
            else
            {
                if (this.isInDesignPage)
                {
                    TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, this.templateID);
                    TemplateDesignManager.UpdateTemplateInfo(base.PublishmentSystemInfo, templateInfo, this.includeUrl, this.tbContent.Text);
                    JsUtils.Layer.CloseModalLayer(base.Page);

                }
                else
                {
                    JsUtils.Layer.CloseModalLayerAndRedirect(base.Page, PageUtils.GetSTLUrl(string.Format("background_templateAdd.aspx?PublishmentSystemID={0}&TemplateID={1}&TemplateLogID={2}", base.PublishmentSystemID, this.templateID, templateLogID)));
                }
            }
        }
	}
}
