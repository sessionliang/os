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
    public class BackgroundWebsiteMessageTemplateDetail : BackgroundBasePage
    {
        public Literal ltlWebsiteMessageName;
        public Literal ltlElement;
        public RadioButtonList rblIsTemplate;

        public PlaceHolder phTemplate;
        public Button IsCreateTemplate;
        public TextBox Content;
        public TextBox Style;
        public TextBox Script;

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
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_WebsiteMessage, "留言详情模板", AppManager.CMS.Permission.WebSite.WebsiteMessage);

                this.ltlWebsiteMessageName.Text = this.websiteMessageInfo.WebsiteMessageName;
                this.ltlElement.Text = string.Format(@"
&lt;stl:websiteMessageContent&gt;&lt;/stl:websiteMessageContent&gt;");

                EBooleanUtils.AddListItems(this.rblIsTemplate, "自定义模板", "默认模板");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsTemplate, this.websiteMessageInfo.IsTemplateDetail.ToString());
                //this.phTemplate.Visible = this.websiteMessageInfo.IsTemplateDetail;
                this.Content.Enabled = this.Style.Enabled = this.Script.Enabled = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                string previewUrl = PageUtils.GetSTLUrl(string.Format("background_websiteMessagePreview.aspx?PublishmentSystemID={0}&WebsiteMessageName={1}&ReturnUrl={2}", base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageName, PageUtils.UrlEncode(base.GetQueryString("ReturnUrl"))));
                this.Preview.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", previewUrl));

                if (this.websiteMessageInfo.IsTemplateDetail)
                {
                    this.Style.Text = this.websiteMessageInfo.StyleTemplateDetail;
                    this.Script.Text = this.websiteMessageInfo.ScriptTemplateDetail;
                    this.Content.Text = this.websiteMessageInfo.ContentTemplateDetail;
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
            this.Content.Enabled = this.Style.Enabled = this.Script.Enabled = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
            if (this.phTemplate.Visible && string.IsNullOrEmpty(this.Content.Text))
            {
                this.IsCreateTemplate_CheckedChanged(sender, e);
            }
        }

        public void IsCreateTemplate_CheckedChanged(object sender, EventArgs e)
        {
            WebsiteMessageTemplate websiteMessageTemplate = new WebsiteMessageTemplate(base.PublishmentSystemInfo, this.websiteMessageInfo);
            ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, base.PublishmentSystemID, websiteMessageInfo.WebsiteMessageID);

            this.Content.Text = websiteMessageTemplate.GetDetailContent(relatedIdentities);
            this.Style.Text = websiteMessageTemplate.GetDetailStyle();
            this.Script.Text = websiteMessageTemplate.GetDetailScript();

        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                this.websiteMessageInfo.IsTemplateDetail = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                this.websiteMessageInfo.StyleTemplateDetail = this.Style.Text;
                this.websiteMessageInfo.ScriptTemplateDetail = this.Script.Text;
                this.websiteMessageInfo.ContentTemplateDetail = this.Content.Text;

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
