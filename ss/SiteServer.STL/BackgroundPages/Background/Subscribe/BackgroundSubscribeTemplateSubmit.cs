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
    public class BackgroundSubscribeTemplateSubmit : BackgroundBasePage
    {
        public Literal ltlElement;
        public RadioButtonList rblIsTemplate;

        public PlaceHolder phTemplate;
        public Button IsCreateTemplate;
        public TextBox Content;
        public TextBox Style;
        public TextBox Script;

        public Button Preview;

        private SubscribeSetInfo subscribeSetInfo;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.subscribeSetInfo = DataProvider.SubscribeSetDAO.GetSubscribeSetInfo(base.PublishmentSystemID);
            if (this.subscribeSetInfo == null)
                this.subscribeSetInfo = new SubscribeSetInfo();

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Subscribe, "自定义模板", AppManager.CMS.Permission.WebSite.Subscribe);

                this.ltlElement.Text = string.Format(@"
&lt;stl:subscribe&gt;&lt;/stl:subscribe&gt;");

                EBooleanUtils.AddListItems(this.rblIsTemplate, "自定义模板", "默认模板");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsTemplate, this.subscribeSetInfo.IsTemplate.ToString());
                this.Content.Enabled = this.Style.Enabled = this.Script.Enabled = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                string previewUrl = PageUtils.GetSTLUrl(string.Format("background_subscribePreview.aspx?PublishmentSystemID={0}&ReturnUrl={1}", base.PublishmentSystemID, PageUtils.UrlEncode(base.GetQueryString("ReturnUrl"))));
                this.Preview.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", previewUrl));

                if (this.subscribeSetInfo.IsTemplate)
                {
                    this.Style.Text = this.subscribeSetInfo.StyleTemplate;
                    this.Script.Text = this.subscribeSetInfo.ScriptTemplate;
                    this.Content.Text = this.subscribeSetInfo.ContentTemplate;
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
            EInputType inputType = EInputType.SelectOne;
            SubscribeTemplate subscribeTemplate = new SubscribeTemplate(base.PublishmentSystemInfo, this.subscribeSetInfo, inputType);
            this.Content.Text = subscribeTemplate.GetContent(inputType);
            this.Style.Text = subscribeTemplate.GetStyle();
            this.Script.Text = subscribeTemplate.GetScript(inputType);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                this.subscribeSetInfo.IsTemplate = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                this.subscribeSetInfo.StyleTemplate = this.Style.Text;
                this.subscribeSetInfo.ScriptTemplate = this.Script.Text;
                this.subscribeSetInfo.ContentTemplate = this.Content.Text;
                this.subscribeSetInfo.PublishmentSystemID = base.PublishmentSystemID;
                try
                {
                    if (this.subscribeSetInfo.SubscribeSetID == 0)
                    {
                        DataProvider.SubscribeSetDAO.Insert(this.subscribeSetInfo);
                    }
                    else
                    {
                        DataProvider.SubscribeSetDAO.Update(this.subscribeSetInfo);
                    }
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
