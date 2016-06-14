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
    public class BackgroundSearchwordInputTemplateSubmit : BackgroundBasePage
    {
        public Literal ltlElement;
        public RadioButtonList rblIsTemplate;

        public PlaceHolder phTemplate;
        public Button IsCreateTemplate;
        public TextBox Content;
        public TextBox Style;
        public TextBox Script;

        public Button Preview;

        private SearchwordSettingInfo searchwordSettingInfo;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.searchwordSettingInfo = DataProvider.SearchwordSettingDAO.GetSearchwordSettingInfo(base.PublishmentSystemID);
            if (this.searchwordSettingInfo == null)
                this.searchwordSettingInfo = new SearchwordSettingInfo();

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Searchword, "自定义模板", AppManager.CMS.Permission.WebSite.Searchword);

                this.ltlElement.Text = string.Format(@"
&lt;stl:searchwordInput&gt;&lt;/stl:searchwordInput&gt;");

                EBooleanUtils.AddListItems(this.rblIsTemplate, "自定义模板", "默认模板");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsTemplate, this.searchwordSettingInfo.IsTemplate.ToString());
                //this.phTemplate.Visible = this.searchwordSettingInfo.IsTemplate;
                this.Content.Enabled = this.Style.Enabled = this.Script.Enabled = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                string previewUrl = PageUtils.GetSTLUrl(string.Format("background_searchwordInputPreview.aspx?PublishmentSystemID={0}&ReturnUrl={1}", base.PublishmentSystemID, PageUtils.UrlEncode(base.GetQueryString("ReturnUrl"))));
                this.Preview.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", previewUrl));

                if (this.searchwordSettingInfo.IsTemplate)
                {
                    this.Style.Text = this.searchwordSettingInfo.StyleTemplate;
                    this.Script.Text = this.searchwordSettingInfo.ScriptTemplate;
                    this.Content.Text = this.searchwordSettingInfo.ContentTemplate;
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
            TagStyleSearchInputInfo inputInfo = new TagStyleSearchInputInfo(string.Empty);
            inputInfo.IsType = true;
            inputInfo.IsChannel = true;
            inputInfo.IsDate = true;
            SearchwordInputTemplate searchwordInputTemplate = new SearchwordInputTemplate(base.PublishmentSystemInfo, this.searchwordSettingInfo, inputInfo, string.Empty, string.Empty);
            this.Content.Text = searchwordInputTemplate.GetContent(true);
            this.Style.Text = searchwordInputTemplate.GetStyle();
            this.Script.Text = searchwordInputTemplate.GetScript(true);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                this.searchwordSettingInfo.IsTemplate = TranslateUtils.ToBool(this.rblIsTemplate.SelectedValue);
                this.searchwordSettingInfo.StyleTemplate = this.Style.Text;
                this.searchwordSettingInfo.ScriptTemplate = this.Script.Text;
                this.searchwordSettingInfo.ContentTemplate = this.Content.Text;
                this.searchwordSettingInfo.PublishmentSystemID = base.PublishmentSystemID;
                try
                {
                    if (this.searchwordSettingInfo.ID == 0)
                    {
                        DataProvider.SearchwordSettingDAO.Insert(this.searchwordSettingInfo);
                    }
                    else
                    {
                    DataProvider.SearchwordSettingDAO.Update(this.searchwordSettingInfo);
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
