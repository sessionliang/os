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

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundCouponActAdd : BackgroundBasePageWX
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
        public TextBox tbContentUsage;
        public TextBox tbContentDescription;
        public TextBox tbAwardCode;
        public Literal ltlContentImageUrl;

        public PlaceHolder phStep3;
        public CheckBox cbIsFormRealName;
        public TextBox tbFormRealNameTitle;
        public CheckBox cbIsFormMobile;
        public TextBox tbFormMobileTitle;
        public CheckBox cbIsFormEmail;
        public TextBox tbFormEmailTitle;
        public CheckBox cbIsFormAddress;
        public TextBox tbFormAddressTitle;

        public PlaceHolder phStep4;
        public TextBox tbEndTitle;
        public TextBox tbEndSummary;
        public Literal ltlEndImageUrl;

        public HtmlInputHidden imageUrl;
        public HtmlInputHidden contentImageUrl;
        public HtmlInputHidden endImageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int actID;

        public static string GetRedirectUrl(int publishmentSystemID, int actID)
        {
            return PageUtils.GetWXUrl(string.Format("background_couponActAdd.aspx?publishmentSystemID={0}&actID={1}", publishmentSystemID, actID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.actID = TranslateUtils.ToInt(base.GetQueryString("actID"));

			if (!IsPostBack)
            {
                string pageTitle = this.actID > 0 ? "编辑优惠劵活动" : "添加优惠劵活动";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Coupon, pageTitle, AppManager.WeiXin.Permission.WebSite.Coupon);
                this.ltlPageTitle.Text = pageTitle;

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", CouponManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", CouponManager.GetContentImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", CouponManager.GetEndImageUrl(base.PublishmentSystemInfo, string.Empty));

                if (this.actID > 0)
                {
                    CouponActInfo actInfo = DataProviderWX.CouponActDAO.GetActInfo(this.actID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(actInfo.KeywordID);
                    this.cbIsEnabled.Checked = !actInfo.IsDisabled;
                    this.dtbStartDate.DateTime = actInfo.StartDate;
                    this.dtbEndDate.DateTime = actInfo.EndDate;
                    this.tbTitle.Text = actInfo.Title;
                    if (!string.IsNullOrEmpty(actInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, actInfo.ImageUrl));
                    }
                    this.tbSummary.Text = actInfo.Summary;
                    if (!string.IsNullOrEmpty(actInfo.ContentImageUrl))
                    {
                        this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, actInfo.ContentImageUrl));
                    }
                    this.tbContentUsage.Text = actInfo.ContentUsage;
                    this.tbContentDescription.Text = actInfo.ContentDescription;
                    this.tbAwardCode.Text = actInfo.AwardCode;

                    this.tbEndTitle.Text = actInfo.EndTitle;
                    this.tbEndSummary.Text = actInfo.EndSummary;
                    if (!string.IsNullOrEmpty(actInfo.EndImageUrl))
                    {
                        this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, actInfo.EndImageUrl));
                    }

                    this.imageUrl.Value = actInfo.ImageUrl;
                    this.contentImageUrl.Value = actInfo.ContentImageUrl;
                    this.endImageUrl.Value = actInfo.EndImageUrl;

                    this.cbIsFormRealName.Checked = actInfo.IsFormRealName;
                    this.tbFormRealNameTitle.Text = actInfo.FormRealNameTitle;
                    this.cbIsFormMobile.Checked = actInfo.IsFormMobile;
                    this.tbFormMobileTitle.Text = actInfo.FormMobileTitle;
                    this.cbIsFormEmail.Checked = actInfo.IsFormEmail;
                    this.tbFormEmailTitle.Text = actInfo.FormEmailTitle;
                    this.cbIsFormAddress.Checked = actInfo.IsFormAddress;
                    this.tbFormAddressTitle.Text = actInfo.FormAddressTitle;
                }
                else
                {
                    this.dtbEndDate.DateTime = DateTime.Now.AddMonths(1);
                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundCouponAct.GetRedirectUrl(base.PublishmentSystemID)));
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
                else if (this.phStep4.Visible)
                {
                    selectedStep = 4;
                }
                this.phStep1.Visible = this.phStep2.Visible = this.phStep3.Visible = this.phStep4.Visible = false;

                if (selectedStep == 1)
                {
                    bool isConflict = false;
                    string conflictKeywords = string.Empty;
                    if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                    {
                        if (this.actID > 0)
                        {
                            CouponActInfo actInfo = DataProviderWX.CouponActDAO.GetActInfo(this.actID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, actInfo.KeywordID, this.tbKeywords.Text, out conflictKeywords);
                        }
                        else
                        {
                            isConflict = KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, this.tbKeywords.Text, out conflictKeywords);
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
                }
                else if (selectedStep == 3)
                {
                    this.phStep4.Visible = true;
                    this.btnSubmit.Text = "确 认";
                }
                else if (selectedStep == 4)
                {
                    CouponActInfo actInfo = new CouponActInfo();
                    if (this.actID > 0)
                    {
                        actInfo = DataProviderWX.CouponActDAO.GetActInfo(this.actID);
                    }
                    actInfo.PublishmentSystemID = base.PublishmentSystemID;

                    actInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.actID > 0,PageUtils.FilterXSS(this.tbKeywords.Text), EKeywordType.Coupon, actInfo.KeywordID);
                    actInfo.IsDisabled = !this.cbIsEnabled.Checked;

                    actInfo.StartDate = this.dtbStartDate.DateTime;
                    actInfo.EndDate = this.dtbEndDate.DateTime;
                    actInfo.Title = this.tbTitle.Text;
                    actInfo.ImageUrl = this.imageUrl.Value; ;
                    actInfo.Summary = this.tbSummary.Text;
                    actInfo.ContentImageUrl = this.contentImageUrl.Value; ;
                    actInfo.ContentUsage = this.tbContentUsage.Text;
                    actInfo.ContentDescription = this.tbContentDescription.Text;
                    actInfo.AwardCode = this.tbAwardCode.Text;                    

                    actInfo.IsFormRealName = this.cbIsFormRealName.Checked;
                    actInfo.FormRealNameTitle = this.tbFormRealNameTitle.Text;
                    actInfo.IsFormMobile = this.cbIsFormMobile.Checked;
                    actInfo.FormMobileTitle = this.tbFormMobileTitle.Text;
                    actInfo.IsFormEmail = this.cbIsFormEmail.Checked;
                    actInfo.FormEmailTitle = this.tbFormEmailTitle.Text;
                    actInfo.IsFormAddress = this.cbIsFormAddress.Checked;
                    actInfo.FormAddressTitle = this.tbFormAddressTitle.Text;

                    actInfo.EndTitle = this.tbEndTitle.Text;
                    actInfo.EndImageUrl = this.endImageUrl.Value;
                    actInfo.EndSummary = this.tbEndSummary.Text;

                    try
                    {
                        if (this.actID > 0)
                        {
                            DataProviderWX.CouponActDAO.Update(actInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改优惠劵活动", string.Format("优惠劵活动:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改优惠劵活动成功！");
                        }
                        else
                        {
                            this.actID = DataProviderWX.CouponActDAO.Insert(actInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加优惠劵活动", string.Format("优惠劵活动:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加优惠劵活动成功！");
                        }

                        string redirectUrl = PageUtils.GetWXUrl(string.Format("background_couponAct.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "优惠劵活动设置失败！");
                    }

                    this.btnSubmit.Visible = false;
                    this.btnReturn.Visible = false;
                }
			}
		}
	}
}
