using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Controls;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class HotfixAdd : BackgroundBasePage
	{
        public TextBox tbVersion;
        public DropDownList ddlIsBeta;
        public TextBox tbFileUrl;
        public TextBox tbPageUrl;
        public DateTimeTextBox tbPubDate;
        public TextBox tbMessage;
        public RadioButtonList rblIsEnabled;
        public RadioButtonList rblIsRestrict;
        public PlaceHolder phRestrict;
        public TextBox tbRestrictDomain;
        public TextBox tbRestrictProductIDCollection;
        public TextBox tbRestrictDatabase;
        public TextBox tbRestrictVersion;
        public DropDownList ddlRestrictIsBeta;

        private int hotfixID;

        public static string GetShowPopWinStringToAdd()
        {
            NameValueCollection arguments = new NameValueCollection();
            return JsUtils.OpenWindow.GetOpenWindowString("添加升级包", "modal_product_hotfixAdd.aspx", arguments, 550, 600);
        }

        public static string GetShowPopWinStringToEdit(int hotfixID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("hotfixID", hotfixID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("修改升级包", "modal_product_hotfixAdd.aspx", arguments, 550, 600);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.hotfixID = TranslateUtils.ToInt(base.Request.QueryString["hotfixID"]);

			if (!IsPostBack)
			{
                EBooleanUtils.AddListItems(this.rblIsEnabled, "启用", "禁用");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsEnabled, true.ToString());

                EBooleanUtils.AddListItems(this.rblIsRestrict, "指定对象升级", "不限制");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsRestrict, false.ToString());

                EBooleanUtils.AddListItems(this.ddlIsBeta, "Beta 版本", "正式版本");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsBeta, false.ToString());

                EBooleanUtils.AddListItems(this.ddlRestrictIsBeta, "Beta 版本", "正式版本");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlRestrictIsBeta, true.ToString());

                if (this.hotfixID > 0)
                {
                    HotfixInfo hotfixInfo = DataProvider.HotfixDAO.GetHotfixInfo(this.hotfixID);
                    if (hotfixInfo != null)
                    {
                        this.tbVersion.Text = hotfixInfo.Version;
                        this.ddlIsBeta.SelectedValue = hotfixInfo.IsBeta.ToString();
                        this.tbFileUrl.Text = hotfixInfo.FileUrl;
                        this.tbPageUrl.Text = hotfixInfo.PageUrl;
                        this.tbPubDate.DateTime = hotfixInfo.PubDate;
                        this.tbMessage.Text = hotfixInfo.Message;
                        ControlUtils.SelectListItemsIgnoreCase(this.rblIsEnabled, hotfixInfo.IsEnabled.ToString());
                        ControlUtils.SelectListItemsIgnoreCase(this.rblIsRestrict, hotfixInfo.IsRestrict.ToString());
                        this.tbRestrictDomain.Text = hotfixInfo.RestrictDomain;
                        this.tbRestrictProductIDCollection.Text = hotfixInfo.RestrictProductIDCollection;
                        this.tbRestrictDatabase.Text = hotfixInfo.RestrictDatabase;
                        this.tbRestrictVersion.Text = hotfixInfo.RestrictVersion;
                        this.ddlRestrictIsBeta.SelectedValue = hotfixInfo.RestrictIsBeta.ToString();
                    }
                }

                this.rblIsRestrict_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

        public void rblIsRestrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phRestrict.Visible = TranslateUtils.ToBool(this.rblIsRestrict.SelectedValue);
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            HotfixInfo hotfixInfo = null;
				
			if (this.hotfixID > 0)
			{
                hotfixInfo = DataProvider.HotfixDAO.GetHotfixInfo(this.hotfixID);
                if (hotfixInfo != null)
                {
                    hotfixInfo.Version = this.tbVersion.Text;
                    hotfixInfo.IsBeta = TranslateUtils.ToBool(this.ddlIsBeta.SelectedValue);
                    hotfixInfo.FileUrl = this.tbFileUrl.Text;
                    hotfixInfo.PageUrl = this.tbPageUrl.Text;
                    hotfixInfo.PubDate = this.tbPubDate.DateTime;
                    hotfixInfo.Message = this.tbMessage.Text;
                    hotfixInfo.IsEnabled = TranslateUtils.ToBool(this.rblIsEnabled.SelectedValue);
                    hotfixInfo.IsRestrict = TranslateUtils.ToBool(this.rblIsRestrict.SelectedValue);
                    hotfixInfo.RestrictDomain = this.tbRestrictDomain.Text;
                    hotfixInfo.RestrictProductIDCollection = this.tbRestrictProductIDCollection.Text;
                    hotfixInfo.RestrictDatabase = this.tbRestrictDatabase.Text;
                    hotfixInfo.RestrictVersion = this.tbRestrictVersion.Text;
                    hotfixInfo.RestrictIsBeta = TranslateUtils.ToBool(this.ddlRestrictIsBeta.SelectedValue);

                    DataProvider.HotfixDAO.Update(hotfixInfo);
                }

                isChanged = true;
			}
			else
			{
                hotfixInfo = new HotfixInfo();

                hotfixInfo.Version = this.tbVersion.Text;
                hotfixInfo.IsBeta = TranslateUtils.ToBool(this.ddlIsBeta.SelectedValue);
                hotfixInfo.FileUrl = this.tbFileUrl.Text;
                hotfixInfo.PageUrl = this.tbPageUrl.Text;
                hotfixInfo.PubDate = this.tbPubDate.DateTime;
                hotfixInfo.Message = this.tbMessage.Text;
                hotfixInfo.IsEnabled = TranslateUtils.ToBool(this.rblIsEnabled.SelectedValue);
                hotfixInfo.IsRestrict = TranslateUtils.ToBool(this.rblIsRestrict.SelectedValue);
                hotfixInfo.RestrictDomain = this.tbRestrictDomain.Text;
                hotfixInfo.RestrictProductIDCollection = this.tbRestrictProductIDCollection.Text;
                hotfixInfo.RestrictDatabase = this.tbRestrictDatabase.Text;
                hotfixInfo.RestrictVersion = this.tbRestrictVersion.Text;
                hotfixInfo.RestrictIsBeta = TranslateUtils.ToBool(this.ddlRestrictIsBeta.SelectedValue);

                DataProvider.HotfixDAO.Insert(hotfixInfo);

                isChanged = true;
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, "product_hotfix.aspx");
			}
		}
	}
}
