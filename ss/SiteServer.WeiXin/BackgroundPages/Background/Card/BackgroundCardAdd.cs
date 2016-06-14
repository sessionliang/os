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
    public class BackgroundCardAdd : BackgroundBasePageWX
    {

        public Literal ltlPageTitle;
        public PlaceHolder phStep1;
        public TextBox tbKeywords;
        public TextBox tbTitle;
        public TextBox tbSummary;
        public CheckBox cbIsEnabled;
        public Literal ltlImageUrl;
        public HtmlInputHidden imageUrl;

        public PlaceHolder phStep2;
        public TextBox tbCardTitle;
        public TextBox tbCardTitleColor;
        public TextBox tbCardSNColor;
        public Literal ltlContentFrontImageUrl;
        public HtmlInputHidden contentFrontImageUrl;
        public Literal ltlContentBackImageUrl;
        public HtmlInputHidden contentBackImageUrl;

        public PlaceHolder phStep3;
        public TextBox tbShopName;
        public TextBox tbShopAddress;

        public Literal ltlMap;
        public HtmlInputHidden shopPosition;
        public TextBox tbShopTel;
        public TextBox tbShopManage;
        public TextBox tbShopPassword;

        public Button btnSubmit;
        public Button btnReturn;

        private int cardID;

        public static string GetRedirectUrl(int publishmentSystemID, int CardID)
        {
            return PageUtils.GetWXUrl(string.Format("background_cardAdd.aspx?publishmentSystemID={0}&CardID={1}", publishmentSystemID, CardID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.cardID = TranslateUtils.ToInt(base.GetQueryString("CardID"));

            if (!IsPostBack)
            {
                string pageTitle = this.cardID > 0 ? "编辑会员卡" : "添加会员卡";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Card, pageTitle, AppManager.WeiXin.Permission.WebSite.Card);
                this.ltlPageTitle.Text = pageTitle;

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", CardManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlContentFrontImageUrl.Text = string.Format(@"<img id=""preview_contentFrontImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", CardManager.GetContentFrontImageUrl(base.PublishmentSystemInfo, string.Empty));
                this.ltlContentBackImageUrl.Text = string.Format(@"<img id=""preview_contentBackImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", CardManager.GetContentBackImageUrl(base.PublishmentSystemInfo, string.Empty));

                if (this.cardID > 0)
                {
                    CardInfo cardInfo = DataProviderWX.CardDAO.GetCardInfo(this.cardID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(cardInfo.KeywordID);
                    this.cbIsEnabled.Checked = !cardInfo.IsDisabled;
                    this.tbTitle.Text = cardInfo.Title;
                    if (!string.IsNullOrEmpty(cardInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, cardInfo.ImageUrl));
                    }
                    if (!string.IsNullOrEmpty(cardInfo.ContentFrontImageUrl))
                    {
                        this.ltlContentFrontImageUrl.Text = string.Format(@"<img id=""preview_contentFrontImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, cardInfo.ContentFrontImageUrl));
                    }
                    if (!string.IsNullOrEmpty(cardInfo.ContentBackImageUrl))
                    {
                        this.ltlContentBackImageUrl.Text = string.Format(@"<img id=""preview_contentBackImageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, cardInfo.ContentBackImageUrl));
                    }

                    this.tbSummary.Text = cardInfo.Summary;

                    this.tbCardTitle.Text = cardInfo.CardTitle;
                    this.tbCardTitleColor.Text = cardInfo.CardTitleColor;
                    this.tbCardSNColor.Text = cardInfo.CardNoColor;

                    this.tbShopName.Text = cardInfo.ShopName;
                    if (!string.IsNullOrEmpty(cardInfo.ShopPosition))
                    {
                        this.shopPosition.Value = cardInfo.ShopPosition;

                    }
                    if (!string.IsNullOrEmpty(cardInfo.ShopAddress))
                    {
                        this.tbShopAddress.Text = cardInfo.ShopAddress;
                        this.ltlMap.Text = string.Format(@"<iframe style=""width:100%;height:100%;background-color:#ffffff;margin-bottom:15px;"" scrolling=""auto"" frameborder=""0"" width=""100%"" height=""100%"" src=""{0}""></iframe>", MapManager.GetMapUrl(cardInfo.ShopAddress));
                    }
                    this.tbShopTel.Text = cardInfo.ShopTel;
                    this.tbShopPassword.Text = cardInfo.ShopPassword;

                    this.imageUrl.Value = cardInfo.ImageUrl;
                    this.contentFrontImageUrl.Value = cardInfo.ContentFrontImageUrl;
                    this.contentBackImageUrl.Value = cardInfo.ContentBackImageUrl;

                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundCard.GetRedirectUrl(base.PublishmentSystemID)));
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
                        if (this.cardID > 0)
                        {
                            CardInfo cardInfo = DataProviderWX.CardDAO.GetCardInfo(this.cardID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, cardInfo.KeywordID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
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
                    CardInfo cardInfo = new CardInfo();
                    if (this.cardID > 0)
                    {
                        cardInfo = DataProviderWX.CardDAO.GetCardInfo(this.cardID);
                    }
                    else
                    {
                        cardInfo.ShopOperatorList = "[{\"UserName\":" + this.tbShopManage.Text + ",\"Password\":" + this.tbShopPassword.Text + ",\"ID\":0}]";
                    }
                    cardInfo.PublishmentSystemID = base.PublishmentSystemID;
                    cardInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.cardID > 0, PageUtils.FilterXSS(this.tbKeywords.Text), EKeywordType.Card, cardInfo.KeywordID);
                    cardInfo.IsDisabled = !this.cbIsEnabled.Checked;

                    cardInfo.Title = PageUtils.FilterXSS(this.tbTitle.Text);
                    cardInfo.ImageUrl = this.imageUrl.Value; 
                    cardInfo.Summary = this.tbSummary.Text;

                    cardInfo.CardTitle = this.tbCardTitle.Text;
                    cardInfo.CardTitleColor = this.tbCardTitleColor.Text;
                    cardInfo.CardNoColor = this.tbCardSNColor.Text;
                    cardInfo.ContentFrontImageUrl = this.contentFrontImageUrl.Value;
                    cardInfo.ContentBackImageUrl = this.contentBackImageUrl.Value;

                    cardInfo.ShopName = this.tbShopName.Text;
                    cardInfo.ShopAddress = this.tbShopAddress.Text;
                    if (!string.IsNullOrEmpty(this.shopPosition.Value))
                    {
                        cardInfo.ShopPosition = this.shopPosition.Value;
                    }
                    cardInfo.ShopTel = this.tbShopTel.Text;
                    cardInfo.ShopPassword = this.tbShopPassword.Text;

                    try
                    {
                        if (this.cardID > 0)
                        {
                            DataProviderWX.CardDAO.Update(cardInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改会员卡", string.Format("会员卡:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改会员卡成功！");
                        }
                        else
                        {
                            this.cardID = DataProviderWX.CardDAO.Insert(cardInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加会员卡", string.Format("会员卡:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加会员卡成功！");
                        }

                        string redirectUrl = PageUtils.GetWXUrl(string.Format("background_card.aspx?publishmentSystemID={0}&CardID={1}", base.PublishmentSystemID, this.cardID));
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "会员卡设置失败！");
                    }
                }
            }
        }
    }
}
