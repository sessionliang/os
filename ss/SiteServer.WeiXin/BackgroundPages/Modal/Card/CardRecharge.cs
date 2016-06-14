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
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
  

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class CardRecharge : BackgroundBasePage
    {
        public DropDownList ddlCard;
        public PlaceHolder phKeyWord;
        public DropDownList ddlKeyWordType;
        public TextBox tbKeyWord;
        public TextBox tbRechargeAmount;
        public DropDownList ddlOperator;
        public TextBox tbDescription;

         
        private int cardID;
        private int cardSNID;
        
        public static string GetOpenWindowString(int publishmentSystemID,int cardID,int cardSNID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("cardID", cardID.ToString());
            arguments.Add("cardSNID", cardSNID.ToString());
            return PageUtilityWX.GetOpenWindowString("会员卡充值", "modal_cardRecharge.aspx", arguments, 400,380);
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
                  
                CardCashLogInfo cardCashLogInfo = new CardCashLogInfo();
                cardCashLogInfo.PublishmentSystemID = base.PublishmentSystemID;
                cardCashLogInfo.UserName = cardSNInfo.UserName;
                cardCashLogInfo.CardID = cardSNInfo.CardID;
                cardCashLogInfo.CardSNID = cardSNInfo.ID;
                cardCashLogInfo.Amount = TranslateUtils.ToInt(this.tbRechargeAmount.Text);
                cardCashLogInfo.CurAmount = cardSNInfo.Amount + TranslateUtils.ToDecimal(this.tbRechargeAmount.Text);
                cardCashLogInfo.CashType = ECashTypeUtils.GetValue(ECashType.Recharge);
                cardCashLogInfo.Operator = this.ddlOperator.SelectedValue;
                cardCashLogInfo.Description = this.tbDescription.Text;
                cardCashLogInfo.AddDate = DateTime.Now;

                try
                {
                    DataProviderWX.CardCashLogDAO.Insert(cardCashLogInfo);
                    DataProviderWX.CardSNDAO.Recharge(cardSNInfo.ID, cardSNInfo.UserName, TranslateUtils.ToDecimal(this.tbRechargeAmount.Text));

                    this.tbRechargeAmount.Text = string.Empty;

                    base.SuccessMessage("充值成功！");

                    //JsUtils.OpenWindow.CloseModalPage(Page);
                    
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "充值失败！");
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
                    this._pageUrl = PageUtils.GetWXUrl(string.Format("modal_cardRecharge.aspx?PublishmentSystemID={0}&cardID={1}&cardSNID={2}&cardType={3}", base.PublishmentSystemID, this.ddlCard.SelectedValue, this.cardSNID, this.ddlCard.SelectedValue));
                }
                return this._pageUrl;
            }
        }
    }
}