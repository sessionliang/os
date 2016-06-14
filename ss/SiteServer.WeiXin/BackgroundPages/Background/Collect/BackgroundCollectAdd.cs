using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using BaiRong.Controls;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.WeiXin.BackgroundPages
{
	public class BackgroundCollectAdd : BackgroundBasePageWX
	{
        public Literal ltlPageTitle;

        public PlaceHolder phStep1;
        public TextBox tbKeywords;
        public TextBox tbTitle;
        public TextBox tbSummary;
        public DateTimeTextBox dtbStartDate;
        public DateTimeTextBox dtbEndDate;
        public CheckBox cbIsEnabled;
        public Literal ltlImageUrl;

        public PlaceHolder phStep2;
        public TextBox tbContentDescription;
        public TextBox tbContentMaxVote;
        public DropDownList ddlContentIsCheck;
        public Literal ltlContentImageUrl;

        public PlaceHolder phStep3;
        public TextBox tbEndTitle;
        public TextBox tbEndSummary;
        public Literal ltlEndImageUrl;

        public HtmlInputHidden imageUrl;
        public HtmlInputHidden contentImageUrl;
        public HtmlInputHidden endImageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int collectID;

        public static string GetRedirectUrl(int publishmentSystemID, int collectID)
        {
            return PageUtils.GetWXUrl(string.Format("background_collectAdd.aspx?publishmentSystemID={0}&collectID={1}", publishmentSystemID, collectID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.collectID = TranslateUtils.ToInt(base.GetQueryString("collectID"));

			if (!IsPostBack)
            {
                string pageTitle = this.collectID > 0 ? "编辑征集活动" : "添加征集活动";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Collect, pageTitle, AppManager.WeiXin.Permission.WebSite.Collect);
                this.ltlPageTitle.Text = pageTitle;

                EBooleanUtils.AddListItems(this.ddlContentIsCheck, "需要审核", "无需审核");
                ControlUtils.SelectListItems(this.ddlContentIsCheck, false.ToString());

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", CollectManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", CollectManager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", CollectManager.GetEndImageUrl(base.PublishmentSystemInfo, string.Empty));

                if (this.collectID == 0)
                {
                    this.dtbEndDate.DateTime = DateTime.Now.AddMonths(1);
                }
                else
                {
                    CollectInfo collectInfo = DataProviderWX.CollectDAO.GetCollectInfo(this.collectID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(collectInfo.KeywordID);
                    this.cbIsEnabled.Checked = !collectInfo.IsDisabled;
                    this.dtbStartDate.DateTime = collectInfo.StartDate;
                    this.dtbEndDate.DateTime = collectInfo.EndDate;
                    this.tbTitle.Text = collectInfo.Title;
                    if (!string.IsNullOrEmpty(collectInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, collectInfo.ImageUrl));
                    }
                    this.tbSummary.Text = collectInfo.Summary;
                    if (!string.IsNullOrEmpty(collectInfo.ContentImageUrl))
                    {
                        this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, collectInfo.ContentImageUrl));
                    }

                    this.tbContentDescription.Text = collectInfo.ContentDescription;

                    this.tbContentMaxVote.Text = collectInfo.ContentMaxVote.ToString();
                    ControlUtils.SelectListItems(this.ddlContentIsCheck, collectInfo.ContentIsCheck.ToString());

                    this.tbEndTitle.Text = collectInfo.EndTitle;
                    this.tbEndSummary.Text = collectInfo.EndSummary;
                    if (!string.IsNullOrEmpty(collectInfo.EndImageUrl))
                    {
                        this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, collectInfo.EndImageUrl));
                    }

                    this.imageUrl.Value = collectInfo.ImageUrl;
                    this.contentImageUrl.Value = collectInfo.ContentImageUrl;
                    this.endImageUrl.Value = collectInfo.EndImageUrl;
                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundCollect.GetRedirectUrl(base.PublishmentSystemID)));
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                int selectedStep = 0;
                if (this.phStep1.Visible)
                {
                    selectedStep = 1;
                }
                else if (this.phStep2.Visible)
                {
                    selectedStep = 2;
                }
                else if (this.phStep3.Visible)
                {
                    selectedStep = 3;
                }

                this.phStep1.Visible = this.phStep2.Visible = this.phStep3.Visible = false;

                if (selectedStep == 1)
                {
                    bool isConflict = false;
                    string conflictKeywords = string.Empty;
                    if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                    {
                        if (this.collectID > 0)
                        {
                            CollectInfo collectInfo = DataProviderWX.CollectDAO.GetCollectInfo(this.collectID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, collectInfo.KeywordID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                        }
                        else
                        {
                            isConflict = KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                        }
                    }
                    
                    if (isConflict)
                    {
                        base.FailMessage(string.Format("触发关键词“{0}”已存在，请设置其他关键词", conflictKeywords));
                        this.phStep1.Visible = true;
                    }
                    else
                    {
                        this.phStep2.Visible = true;
                    }
                }
                else if (selectedStep == 2)
                {
                    this.phStep3.Visible = true;
                    this.btnSubmit.Text = "确 认";
                }
                else if (selectedStep == 3)
                {
                    CollectInfo collectInfo = new CollectInfo();
                    if (this.collectID > 0)
                    {
                        collectInfo = DataProviderWX.CollectDAO.GetCollectInfo(this.collectID);
                    }
                    collectInfo.PublishmentSystemID = base.PublishmentSystemID;

                    collectInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.collectID > 0, this.tbKeywords.Text, EKeywordType.Collect, collectInfo.KeywordID);
                    collectInfo.IsDisabled = !this.cbIsEnabled.Checked;

                    collectInfo.StartDate = this.dtbStartDate.DateTime;
                    collectInfo.EndDate = this.dtbEndDate.DateTime;
                    collectInfo.Title = PageUtils.FilterXSS(this.tbTitle.Text);
                    collectInfo.ImageUrl = this.imageUrl.Value; ;
                    collectInfo.Summary = this.tbSummary.Text;

                    collectInfo.ContentImageUrl = this.contentImageUrl.Value;
                    collectInfo.ContentDescription = this.tbContentDescription.Text;
                    collectInfo.ContentMaxVote = TranslateUtils.ToInt(this.tbContentMaxVote.Text);
                    collectInfo.ContentIsCheck = TranslateUtils.ToBool(this.ddlContentIsCheck.SelectedValue);

                    collectInfo.EndTitle = this.tbEndTitle.Text;
                    collectInfo.EndImageUrl = this.endImageUrl.Value;
                    collectInfo.EndSummary = this.tbEndSummary.Text;

                    try
                    {
                        if (this.collectID > 0)
                        {
                            DataProviderWX.CollectDAO.Update(collectInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改征集活动", string.Format("征集活动:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改征集活动成功！");
                        }
                        else
                        {
                            this.collectID = DataProviderWX.CollectDAO.Insert(collectInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加征集活动", string.Format("征集活动:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加征集活动成功！");
                        }

                        string redirectUrl = PageUtils.GetWXUrl(string.Format("background_collect.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "征集活动设置失败！");
                    }

                    this.btnSubmit.Visible = false;
                    this.btnReturn.Visible = false;
                }
			}
		}
	}
}
