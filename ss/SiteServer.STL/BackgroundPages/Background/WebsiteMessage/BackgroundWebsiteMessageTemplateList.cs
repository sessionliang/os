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
    public class BackgroundWebsiteMessageTemplateList : BackgroundBasePage
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
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_WebsiteMessage, "留言列表模板", AppManager.CMS.Permission.WebSite.WebsiteMessage);

                this.ltlWebsiteMessageName.Text = this.websiteMessageInfo.WebsiteMessageName;
                this.ltlElement.Text = string.Format(@"
&lt;stl:websiteMessageContents&gt;&lt;/stl:websiteMessageContents&gt;");

                EBooleanUtils.AddListItems(this.rblIsTemplate, "自定义模板", "默认模板");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsTemplate, this.websiteMessageInfo.IsTemplateList.ToString());
                //this.phTemplate.Visible = this.websiteMessageInfo.IsTemplateList;
                this.Content.Enabled = this.Style.Enabled = this.Script.Enabled = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                string previewUrl = PageUtils.GetSTLUrl(string.Format("background_websiteMessagePreview.aspx?PublishmentSystemID={0}&WebsiteMessageName={1}&ReturnUrl={2}", base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageName, PageUtils.UrlEncode(base.GetQueryString("ReturnUrl"))));
                this.Preview.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", previewUrl));

                if (this.websiteMessageInfo.IsTemplateList)
                {
                    this.Style.Text = this.websiteMessageInfo.StyleTemplateList;
                    this.Script.Text = this.websiteMessageInfo.ScriptTemplateList;
                    this.Content.Text = this.websiteMessageInfo.ContentTemplateList;
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
            ArrayList relatedIndentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, base.PublishmentSystemID, websiteMessageInfo.WebsiteMessageID);

            this.Content.Text = websiteMessageTemplate.GetListContent(relatedIndentities);
            this.Style.Text = websiteMessageTemplate.GetListStyle();
            this.Script.Text = websiteMessageTemplate.GetListStyle();
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                this.websiteMessageInfo.IsTemplateList = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                this.websiteMessageInfo.StyleTemplateList = this.Style.Text;
                this.websiteMessageInfo.ScriptTemplateList = this.Script.Text;
                this.websiteMessageInfo.ContentTemplateList = this.Content.Text;

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
