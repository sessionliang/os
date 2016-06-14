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
    public class BackgroundFlapAdd : BackgroundBasePageWX
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
        public Literal ltlContentImageUrl;

        public PlaceHolder phStep3;
        public TextBox tbAwardUsage;
        public CheckBox cbIsAwardTotalNum;
        public TextBox tbAwardMaxCount;
        public TextBox tbAwardMaxDailyCount;
        public TextBox tbAwardCode;
        public Literal ltlAwardImageUrl;
        public Literal ltlAwardItems;

        public PlaceHolder phStep4;
        public CheckBox cbIsFormRealName;
        public TextBox tbFormRealNameTitle;
        public CheckBox cbIsFormMobile;
        public TextBox tbFormMobileTitle;
        public CheckBox cbIsFormEmail;
        public TextBox tbFormEmailTitle;
        public CheckBox cbIsFormAddress;
        public TextBox tbFormAddressTitle;

        public PlaceHolder phStep5;
        public TextBox tbEndTitle;
        public TextBox tbEndSummary;
        public Literal ltlEndImageUrl;

        public HtmlInputHidden imageUrl;
        public HtmlInputHidden contentImageUrl;
        public HtmlInputHidden awardImageUrl;
        public HtmlInputHidden endImageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        private int lotteryID;

        public static string GetRedirectUrl(int publishmentSystemID, int lotteryID)
        {
            return PageUtils.GetWXUrl(string.Format("background_flapAdd.aspx?publishmentSystemID={0}&lotteryID={1}", publishmentSystemID, lotteryID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.lotteryID = TranslateUtils.ToInt(base.GetQueryString("lotteryID"));

			if (!IsPostBack)
            {
                string pageTitle = this.lotteryID > 0 ? "编辑大翻牌" : "添加大翻牌";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Flap, pageTitle, AppManager.WeiXin.Permission.WebSite.Flap);
                this.ltlPageTitle.Text = pageTitle;

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", LotteryManager.GetImageUrl(base.PublishmentSystemInfo, ELotteryType.Flap, string.Empty));
                this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", LotteryManager.GetContentImageUrl(base.PublishmentSystemInfo, ELotteryType.Flap, string.Empty));
                this.ltlAwardImageUrl.Text = string.Format(@"<img id=""preview_awardImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", LotteryManager.GetAwardImageUrl(base.PublishmentSystemInfo, ELotteryType.Flap, string.Empty));
                this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", LotteryManager.GetEndImageUrl(base.PublishmentSystemInfo, ELotteryType.Flap, string.Empty));

                if (this.lotteryID == 0)
                {
                    this.ltlAwardItems.Text = "itemController.itemCount = 2;itemController.items = [{}, {}];";
                    this.dtbEndDate.DateTime = DateTime.Now.AddMonths(1);
                }
                else
                {
                    LotteryInfo lotteryInfo = DataProviderWX.LotteryDAO.GetLotteryInfo(this.lotteryID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(lotteryInfo.KeywordID);
                    this.cbIsEnabled.Checked = !lotteryInfo.IsDisabled;
                    this.dtbStartDate.DateTime = lotteryInfo.StartDate;
                    this.dtbEndDate.DateTime = lotteryInfo.EndDate;
                    this.tbTitle.Text = lotteryInfo.Title;
                    if (!string.IsNullOrEmpty(lotteryInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, lotteryInfo.ImageUrl));
                    }
                    this.tbSummary.Text = lotteryInfo.Summary;
                    if (!string.IsNullOrEmpty(lotteryInfo.ContentImageUrl))
                    {
                        this.ltlContentImageUrl.Text = string.Format(@"<img id=""preview_contentImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, lotteryInfo.ContentImageUrl));
                    }
                    this.tbContentUsage.Text = lotteryInfo.ContentUsage;

                    this.tbAwardUsage.Text = lotteryInfo.AwardUsage;
                    this.cbIsAwardTotalNum.Checked = lotteryInfo.IsAwardTotalNum;
                    this.tbAwardMaxCount.Text = lotteryInfo.AwardMaxCount.ToString();
                    this.tbAwardMaxDailyCount.Text = lotteryInfo.AwardMaxDailyCount.ToString();
                    this.tbAwardCode.Text = lotteryInfo.AwardCode;

                    this.cbIsFormRealName.Checked = lotteryInfo.IsFormRealName;
                    this.tbFormRealNameTitle.Text = lotteryInfo.FormRealNameTitle;
                    this.cbIsFormMobile.Checked = lotteryInfo.IsFormMobile;
                    this.tbFormMobileTitle.Text = lotteryInfo.FormMobileTitle;
                    this.cbIsFormEmail.Checked = lotteryInfo.IsFormEmail;
                    this.tbFormEmailTitle.Text = lotteryInfo.FormEmailTitle;
                    this.cbIsFormAddress.Checked = lotteryInfo.IsFormAddress;
                    this.tbFormAddressTitle.Text = lotteryInfo.FormAddressTitle;

                    if (!string.IsNullOrEmpty(lotteryInfo.AwardImageUrl))
                    {
                        this.ltlAwardImageUrl.Text = string.Format(@"<img id=""preview_awardImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, lotteryInfo.AwardImageUrl));
                    }

                    List<LotteryAwardInfo> awardInfoList = DataProviderWX.LotteryAwardDAO.GetLotteryAwardInfoList(base.PublishmentSystemID, this.lotteryID);
                    StringBuilder itemBuilder = new StringBuilder();
                    foreach (LotteryAwardInfo awardInfo in awardInfoList)
                    {
                        itemBuilder.AppendFormat(@"{{id: '{0}', awardName: '{1}', title: '{2}', totalNum: '{3}', probability: '{4}'}},", awardInfo.ID, awardInfo.AwardName, awardInfo.Title, awardInfo.TotalNum, awardInfo.Probability);
                    }
                    if (itemBuilder.Length > 0) itemBuilder.Length--;

                    this.ltlAwardItems.Text = string.Format(@"
itemController.itemCount = {0};itemController.items = [{1}];", awardInfoList.Count, itemBuilder.ToString());

                    this.tbEndTitle.Text = lotteryInfo.EndTitle;
                    this.tbEndSummary.Text = lotteryInfo.EndSummary;
                    if (!string.IsNullOrEmpty(lotteryInfo.EndImageUrl))
                    {
                        this.ltlEndImageUrl.Text = string.Format(@"<img id=""preview_endImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, lotteryInfo.EndImageUrl));
                    }

                    this.imageUrl.Value = lotteryInfo.ImageUrl;
                    this.contentImageUrl.Value = lotteryInfo.ContentImageUrl;
                    this.awardImageUrl.Value = lotteryInfo.AwardImageUrl;
                    this.endImageUrl.Value = lotteryInfo.EndImageUrl;
                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundLottery.GetRedirectUrl(base.PublishmentSystemID, ELotteryType.Flap)));
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
                else if (this.phStep5.Visible)
                {
                    selectedStep = 5;
                }

                this.phStep1.Visible = this.phStep2.Visible = this.phStep3.Visible = this.phStep4.Visible = this.phStep5.Visible = false;

                if (selectedStep == 1)
                {
                    bool isConflict = false;
                    string conflictKeywords = string.Empty;
                    if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                    {
                        if (this.lotteryID > 0)
                        {
                            LotteryInfo lotteryInfo = DataProviderWX.LotteryDAO.GetLotteryInfo(this.lotteryID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, lotteryInfo.KeywordID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
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
                }
                else if (selectedStep == 3)
                {
                    bool isItemReady = true;

                    int awardMaxCount = TranslateUtils.ToInt(this.tbAwardMaxCount.Text);
                    int awardMaxDailyCount = TranslateUtils.ToInt(this.tbAwardMaxDailyCount.Text);
                    if (awardMaxDailyCount > awardMaxCount)
                    {
                        base.FailMessage("大翻牌保存失败，每人每天最多允许抽奖次数必须小于每人最多抽奖次数");
                        isItemReady = false;
                    }
                     
                    if (isItemReady)
                    {
                        int itemCount = TranslateUtils.ToInt(base.Request.Form["itemCount"]);

                        if (itemCount < 1)
                        {
                            base.FailMessage("大翻牌保存失败，至少需要设置一个奖项");
                            isItemReady = false;
                        }
                        else
                        {
                            List<int> itemIDList = TranslateUtils.StringCollectionToIntList(base.Request.Form["itemID"]);
                            List<string> awardNameList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemAwardName"]);
                            List<string> titleList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemTitle"]);
                            List<int> totalNumList = TranslateUtils.StringCollectionToIntList(base.Request.Form["itemTotalNum"]);
                            List<decimal> probabilityList = TranslateUtils.StringCollectionToDecimalList(base.Request.Form["itemProbability"]);

                            decimal probabilityAll = 0;

                            List<LotteryAwardInfo> awardInfoList = new List<LotteryAwardInfo>();
                            for (int i = 0; i < itemCount; i++)
                            {
                                LotteryAwardInfo awardInfo = new LotteryAwardInfo { ID = itemIDList[i], PublishmentSystemID = base.PublishmentSystemID, LotteryID = this.lotteryID, AwardName = awardNameList[i], Title = titleList[i], TotalNum = totalNumList[i], Probability = probabilityList[i] };

                                if (string.IsNullOrEmpty(awardInfo.AwardName))
                                {
                                    base.FailMessage("保存失败，奖项名称为必填项");
                                    isItemReady = false;
                                }
                                if (string.IsNullOrEmpty(awardInfo.Title))
                                {
                                    base.FailMessage("保存失败，奖品名为必填项");
                                    isItemReady = false;
                                }
                                if (awardInfo.Probability < 0 || awardInfo.Probability > 100)
                                {
                                    base.FailMessage("保存失败，各项中奖概率总和不能超过100%");
                                    isItemReady = false;
                                }

                                probabilityAll += awardInfo.Probability;

                                awardInfoList.Add(awardInfo);
                            }

                            if (probabilityAll <= 0 || probabilityAll > 100)
                            {
                                base.FailMessage("大翻牌保存失败，获奖概率之和必须在1%到100%之间");
                                isItemReady = false;
                            }

                            if (isItemReady)
                            {
                                DataProviderWX.LotteryAwardDAO.DeleteAllNotInIDList(base.PublishmentSystemID, this.lotteryID, itemIDList);

                                foreach (LotteryAwardInfo awardInfo in awardInfoList)
                                {
                                    LotteryAwardInfo newAwardInfo = DataProviderWX.LotteryAwardDAO.GetAwardInfo(awardInfo.ID);
                                    if (awardInfo.ID > 0)
                                    {
                                        int wonNum = DataProviderWX.LotteryWinnerDAO.GetTotalNum(awardInfo.ID);
                                        if (awardInfo.TotalNum < wonNum)
                                        {
                                            awardInfo.TotalNum = wonNum;
                                        }
                                        awardInfo.WonNum = newAwardInfo.WonNum;
                                        DataProviderWX.LotteryAwardDAO.Update(awardInfo);
                                    }
                                    else
                                    {
                                        DataProviderWX.LotteryAwardDAO.Insert(awardInfo);
                                    }
                                }
                            }
                        }
                    }

                    if (isItemReady)
                    {
                        this.phStep4.Visible = true;
                    }
                    else
                    {
                        this.phStep3.Visible = true;
                    }
                }
                else if (selectedStep == 4)
                {
                    this.phStep5.Visible = true;
                    this.btnSubmit.Text = "确 认";
                }
                else if (selectedStep == 5)
                {
                    LotteryInfo lotteryInfo = new LotteryInfo();
                    if (this.lotteryID > 0)
                    {
                        lotteryInfo = DataProviderWX.LotteryDAO.GetLotteryInfo(this.lotteryID);
                    }
                    lotteryInfo.PublishmentSystemID = base.PublishmentSystemID;
                    lotteryInfo.LotteryType = ELotteryTypeUtils.GetValue(ELotteryType.Flap);

                    lotteryInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.lotteryID > 0, this.tbKeywords.Text, EKeywordType.Flap, lotteryInfo.KeywordID);
                    lotteryInfo.IsDisabled = !this.cbIsEnabled.Checked;

                    lotteryInfo.StartDate = this.dtbStartDate.DateTime;
                    lotteryInfo.EndDate = this.dtbEndDate.DateTime;
                    lotteryInfo.Title = PageUtils.FilterXSS(this.tbTitle.Text);
                    lotteryInfo.ImageUrl = this.imageUrl.Value; ;
                    lotteryInfo.Summary = this.tbSummary.Text;
                    lotteryInfo.ContentImageUrl = this.contentImageUrl.Value;
                    lotteryInfo.ContentUsage = this.tbContentUsage.Text;

                    lotteryInfo.AwardUsage = this.tbAwardUsage.Text;
                    lotteryInfo.IsAwardTotalNum = this.cbIsAwardTotalNum.Checked;
                    lotteryInfo.AwardMaxCount = TranslateUtils.ToInt(this.tbAwardMaxCount.Text);
                    lotteryInfo.AwardMaxDailyCount = TranslateUtils.ToInt(this.tbAwardMaxDailyCount.Text);
                    lotteryInfo.AwardCode = this.tbAwardCode.Text;
                    lotteryInfo.AwardImageUrl = this.awardImageUrl.Value;

                    lotteryInfo.IsFormRealName = this.cbIsFormRealName.Checked;
                    lotteryInfo.FormRealNameTitle = this.tbFormRealNameTitle.Text;
                    lotteryInfo.IsFormMobile = this.cbIsFormMobile.Checked;
                    lotteryInfo.FormMobileTitle = this.tbFormMobileTitle.Text;
                    lotteryInfo.IsFormEmail = this.cbIsFormEmail.Checked;
                    lotteryInfo.FormEmailTitle = this.tbFormEmailTitle.Text;
                    lotteryInfo.IsFormAddress = this.cbIsFormAddress.Checked;
                    lotteryInfo.FormAddressTitle = this.tbFormAddressTitle.Text;

                    lotteryInfo.EndTitle = this.tbEndTitle.Text;
                    lotteryInfo.EndImageUrl = this.endImageUrl.Value;
                    lotteryInfo.EndSummary = this.tbEndSummary.Text;

                    try
                    {
                        if (this.lotteryID > 0)
                        {
                            DataProviderWX.LotteryDAO.Update(lotteryInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改大翻牌", string.Format("大翻牌:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改大翻牌成功！");
                        }
                        else
                        {
                            this.lotteryID = DataProviderWX.LotteryDAO.Insert(lotteryInfo);

                            DataProviderWX.LotteryAwardDAO.UpdateLotteryID(base.PublishmentSystemID, this.lotteryID);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加大翻牌", string.Format("大翻牌:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加大翻牌成功！");
                        }

                        string redirectUrl = BackgroundLottery.GetRedirectUrl(base.PublishmentSystemID, ELotteryType.Flap);
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "大翻牌设置失败！");
                    }
                }
			}
		}
	}
}
