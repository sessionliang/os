using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.BackgroundPages;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundWebsiteMessageTemplateSMS : BackgroundBasePage
    {
        public Literal ltlWebsiteMessageName;
        public RadioButtonList rblIsTemplate;

        public PlaceHolder phTemplate;
        public Button IsCreateTemplate;
        public TextBox Content;

        public Button Preview;

        private WebsiteMessageInfo websiteMessageInfo;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("WebsiteMessageName");

            this.Preview.Visible = false;

            string websiteMessageName = base.GetQueryStringNoSqlAndXss("WebsiteMessageName");
            this.websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageName, PublishmentSystemID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_WebsiteMessage, "短信回复模板", AppManager.CMS.Permission.WebSite.WebsiteMessage);

                this.ltlWebsiteMessageName.Text = this.websiteMessageInfo.WebsiteMessageName;

                EBooleanUtils.AddListItems(this.rblIsTemplate, "自定义模板", "默认模板");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsTemplate, this.websiteMessageInfo.IsTemplate.ToString());
                //this.phTemplate.Visible = this.websiteMessageInfo.IsTemplate;
                this.Content.Enabled = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                string previewUrl = PageUtils.GetSTLUrl(string.Format("background_websiteMessagePreview.aspx?PublishmentSystemID={0}&WebsiteMessageID={1}&ReturnUrl={2}", base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID, PageUtils.UrlEncode(base.GetQueryString("ReturnUrl"))));
                this.Preview.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", previewUrl));

                if (this.websiteMessageInfo.IsTemplate)
                {
                    this.Content.Text = this.websiteMessageInfo.Additional.SMSContent;
                }
                else
                {
                    this.IsCreateTemplate_CheckedChanged(sender, E);
                }
            }
        }

        public void rblIsTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phTemplate.Visible = true;
            this.Content.Enabled = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
            if (this.phTemplate.Visible && string.IsNullOrEmpty(this.Content.Text))
            {
                this.IsCreateTemplate_CheckedChanged(sender, e);
            }
        }

        public void IsCreateTemplate_CheckedChanged(object sender, EventArgs e)
        {
            WebsiteMessageTemplate websiteMessageTemplate = new WebsiteMessageTemplate(base.PublishmentSystemInfo, this.websiteMessageInfo);
            ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID);
            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, relatedIdentities);
            this.Content.Text = websiteMessageTemplate.GetSMSContentReply(tableStyleInfoArrayList);

        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                this.websiteMessageInfo.IsTemplate = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                this.websiteMessageInfo.Additional.SMSContent = this.Content.Text;

                try
                {
                    DataProvider.WebsiteMessageDAO.Update(this.websiteMessageInfo);
                    base.SuccessMessage("模板修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "模板修改失败," + ex.Message);
                }
            }
        }
    }
}
