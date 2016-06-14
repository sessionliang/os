using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class WebsiteMessageReplayTemplateAdd : BackgroundBasePage
    {
        protected TextBox tbTitle;
        protected BREditor brContent;
        protected DropDownList ddlIsEnabled;

        private string returnUrl;
        private int templateID;
        private int classifyID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int classifyID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ClassifyID", classifyID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtility.GetOpenWindowString("添加信息", "modal_websiteMessageReplayTemplateAdd.aspx", arguments);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int templateID, int classifyID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("TemplateID", templateID.ToString());
            arguments.Add("ClassifyID", classifyID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtility.GetOpenWindowString("修改信息", "modal_websiteMessageReplayTemplateAdd.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.classifyID = TranslateUtils.ToInt(base.GetQueryStringNoSqlAndXss("ClassifyID"));
            this.templateID = TranslateUtils.ToInt(base.GetQueryStringNoSqlAndXss("TemplateID"));

            EBooleanUtils.AddListItems(this.ddlIsEnabled, "启用", "禁用");

            if (!IsPostBack)
            {
                if (this.templateID != 0)
                {
                    WebsiteMessageReplayTemplateInfo contentInfo = DataProvider.WebsiteMessageReplayTemplateDAO.GetWebsiteMessageReplayTemplateInfo(this.templateID);
                    if (contentInfo != null)
                    {
                        this.tbTitle.Text = contentInfo.TemplateTitle;
                        this.brContent.Text = contentInfo.TemplateContent;
                        ControlUtils.SelectListItems(this.ddlIsEnabled, contentInfo.IsEnabled.ToString());
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {

            if (this.templateID != 0)
            {
                try
                {
                    WebsiteMessageReplayTemplateInfo contentInfo = DataProvider.WebsiteMessageReplayTemplateDAO.GetWebsiteMessageReplayTemplateInfo(this.templateID);
                    contentInfo.ClassifyID = this.classifyID;
                    contentInfo.TemplateTitle = this.tbTitle.Text;
                    contentInfo.TemplateContent = this.brContent.Text;
                    contentInfo.IsEnabled = TranslateUtils.ToBool(this.ddlIsEnabled.SelectedValue);

                    DataProvider.WebsiteMessageReplayTemplateDAO.Update(contentInfo);

                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "信息修改失败:" + ex.Message);
                }
            }
            else
            {
                try
                {
                    WebsiteMessageReplayTemplateInfo contentInfo = new WebsiteMessageReplayTemplateInfo();
                    contentInfo.ClassifyID = this.classifyID;
                    contentInfo.TemplateTitle = this.tbTitle.Text;
                    contentInfo.TemplateContent = this.brContent.Text;
                    contentInfo.IsEnabled = TranslateUtils.ToBool(this.ddlIsEnabled.SelectedValue);

                    DataProvider.WebsiteMessageReplayTemplateDAO.Insert(contentInfo);

                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "信息添加失败:" + ex.Message);
                }
            }
            JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
        }
    }
}
