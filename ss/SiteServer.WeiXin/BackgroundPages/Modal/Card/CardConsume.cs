using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;


namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class CardConsume : BackgroundBasePage
    {
        public DropDownList ddlCard;
        public PlaceHolder phKeyWord;
        public DropDownList ddlKeyWordType;
        public TextBox tbKeyWord;
        public TextBox tbConsumeAmount;
        public DropDownList ddlConsumeType;
        public DropDownList ddlOperator;

        private int cardID;
        private int cardSNID;
         
        public static string GetOpenWindowString(int publishmentSystemID, int cardID, int cardSNID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("cardID", cardID.ToString());
            arguments.Add("cardSNID", cardSNID.ToString());
            return PageUtilityWX.GetOpenWindowString("会员消费", "modal_cardConsume.aspx", arguments, 400, 380);
        }
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.cardID = TranslateUtils.ToInt(base.GetQueryString("cardID"));
            this.cardSNID = TranslateUtils.ToInt(base.GetQueryString("cardSNID"));
            
            if (!IsPostBack)
            {
                List<CardInfo> cardInfoList = DataProviderWX.CardDAO.GetCardInfoList(base.PublishmentSystemID);
                foreach (CardInfo cardInfo in cardInfoList)
                {
                    if (this.cardID <= 0)
                    {
                        this.cardID = cardInfo.ID;
                    }
                   this.ddlCard.Items.Add(new ListItem(cardInfo.CardTitle, cardInfo.ID.ToString()));
                }

                this.ddlKeyWordType.Items.Add(new ListItem("卡号", "cardSN"));
                this.ddlKeyWordType.Items.Add(new ListItem("手机", "mobile"));

                EConsumeTypeUtils.AddListItems(this.ddlConsumeType);
                ControlUtils.SelectListItems(this.ddlConsumeType, EConsumeTypeUtils.GetValue(EConsumeType.CardAmount));

                List<CardOperatorInfo> operatorInfoList = new List<CardOperatorInfo>();
                CardInfo theCardInfo = DataProviderWX.CardDAO.GetCardInfo(this.cardID);
                if (theCardInfo != null)
                {
                    operatorInfoList = TranslateUtils.JsonToObject(theCardInfo.ShopOperatorList, operatorInfoList) as List<CardOperatorInfo>;
                    if (operatorInfoList != null)
                    {
                        foreach (CardOperatorInfo operaotorInfo in operatorInfoList)
                        {
                            this.ddlOperator.Items.Add(new ListItem(operaotorInfo.UserName, operaotorInfo.UserName));
                        }
                    }
                }
                if (!string.IsNullOrEmpty(base.GetQueryString("cardType")))
                {
                    ControlUtils.SelectListItems(this.ddlCard, base.GetQueryString("cardType"));
                }

                if (this.cardSNID > 0)
                {
                    this.phKeyWord.Visible = false;
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (IsPostBack && IsValid)
            {
                UserInfo userInfo = null;
                CardSNInfo cardSNInfo = null;

                if (this.cardSNID > 0)
                {
                    cardSNInfo = DataProviderWX.CardSNDAO.GetCardSNInfo(this.cardSNID);
                }
                else
                {
                    if (this.ddlKeyWordType.SelectedValue == "cardSN")
                    {
                        cardSNInfo = DataProviderWX.CardSNDAO.GetCardSNInfo(base.PublishmentSystemID, TranslateUtils.ToInt(this.ddlCard.SelectedValue), this.tbKeyWord.Text, string.Empty);
                    }
                    else if (this.ddlKeyWordType.SelectedValue == "mobile")
                    {
                        int userID = BaiRongDataProvider.UserDAO.GetUserIDByEmailOrMobile(base.PublishmentSystemInfo.GroupSN, string.Empty, this.tbKeyWord.Text);
                        userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);
                        if (userInfo != null)
                        {
                            cardSNInfo = DataProviderWX.CardSNDAO.GetCardSNInfo(base.PublishmentSystemID, TranslateUtils.ToInt(this.ddlCard.SelectedValue), string.Empty, userInfo.UserName);
                        }
                    }
                }

                if (cardSNInfo == null)
                {
                    base.FailMessage("会员卡不存在");
                    return;
                }

                EConsumeType consumeType = EConsumeTypeUtils.GetEnumType(this.ddlConsumeType.SelectedValue);

                if (consumeType == EConsumeType.CardAmount)
                {
                    decimal amount = DataProviderWX.CardSNDAO.GetAmount(cardSNInfo.ID, cardSNInfo.UserName);
                    if (amount < TranslateUtils.ToDecimal(this.tbConsumeAmount.Text))
                    {
                        base.FailMessage("会员卡余额不足");
                        return;
                    }
                }

                CardCashLogInfo cardCashLogInfo = new CardCashLogInfo();
                cardCashLogInfo.PublishmentSystemID = base.PublishmentSystemID;
                cardCashLogInfo.UserName = cardSNInfo.UserName;
                cardCashLogInfo.CardID = cardSNInfo.CardID;
                cardCashLogInfo.CardSNID = cardSNInfo.ID;
                cardCashLogInfo.Amount = TranslateUtils.ToDecimal(this.tbConsumeAmount.Text);
                cardCashLogInfo.CurAmount = cardSNInfo.Amount;
                if (consumeType == EConsumeType.CardAmount)
                {
                    cardCashLogInfo.CurAmount = cardSNInfo.Amount - TranslateUtils.ToInt(this.tbConsumeAmount.Text);
                }

                cardCashLogInfo.CashType = ECashTypeUtils.GetValue(ECashType.Consume);
                cardCashLogInfo.ConsumeType = this.ddlConsumeType.SelectedValue;
                cardCashLogInfo.Operator = this.ddlOperator.SelectedValue;
                cardCashLogInfo.AddDate = DateTime.Now;

                try
                {
                    DataProviderWX.CardCashLogDAO.Insert(cardCashLogInfo);

                    if (consumeType == EConsumeType.CardAmount)
                    {
                        DataProviderWX.CardSNDAO.Consume(cardSNInfo.ID, cardSNInfo.UserName, TranslateUtils.ToDecimal(this.tbConsumeAmount.Text));

                        if (base.PublishmentSystemInfo.Additional.Card_IsClaimCardCredits)
                        {
                            decimal amount = TranslateUtils.ToDecimal(this.tbConsumeAmount.Text);
                            decimal consumeAmount = base.PublishmentSystemInfo.Additional.Card_ConsumeAmount;
                            int giveCredits = base.PublishmentSystemInfo.Additional.Card_GiveCredits;

                            UserCreditsLogInfo userCreditsLogInfo = new UserCreditsLogInfo();
                            userCreditsLogInfo.UserName = cardSNInfo.UserName;
                            userCreditsLogInfo.ProductID = AppManager.WeiXin.AppID;
                            userCreditsLogInfo.Num = (int)Math.Round(amount * (giveCredits / consumeAmount), 0);
                            userCreditsLogInfo.AddDate = DateTime.Now;
                            userCreditsLogInfo.IsIncreased = true;
                            userCreditsLogInfo.Action = "消费送积分";

                            BaiRongDataProvider.UserCreditsLogDAO.Insert(userCreditsLogInfo);
                            BaiRongDataProvider.UserDAO.AddCredits(base.PublishmentSystemInfo.GroupSN, cardSNInfo.UserName, (int)Math.Round(amount * (giveCredits / consumeAmount), 0));
                        }
                    }
                    this.tbConsumeAmount.Text = string.Empty;

                    base.SuccessMessage("操作成功！");

                    //JsUtils.OpenWindow.CloseModalPage(Page);
                   
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "操作失败！");
                }
            }
        }
        public void Refrush(object sender, EventArgs E)
        {
            this.cardID = TranslateUtils.ToInt(this.ddlCard.SelectedValue);

            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = PageUtils.GetWXUrl(string.Format("modal_cardConsume.aspx?PublishmentSystemID={0}&cardID={1}&cardSNID={2}&cardType={3}", base.PublishmentSystemID, this.ddlCard.SelectedValue,this.cardSNID,this.ddlCard.SelectedValue));
                }
                return this._pageUrl;
            }
        }
    }
}