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
    public class CardCredits : BackgroundBasePage
    {
        public DropDownList ddlCard;
        public PlaceHolder phKeyWord;
        public DropDownList ddlKeyWordType;
        public TextBox tbKeyWord;
        public DropDownList ddlOperatType;
        public TextBox tbCredits;

        private int cardID;
        private int cardSNID;
        public static string GetOpenWindowString(int publishmentSystemID, int cardID, int cardSNID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("cardID", cardID.ToString());
            arguments.Add("cardSNID", cardSNID.ToString());
            return PageUtilityWX.GetOpenWindowString("会员卡充值", "modal_cardCredits.aspx", arguments, 400, 380);
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
                    this.ddlCard.Items.Add(new ListItem(cardInfo.CardTitle, cardInfo.ID.ToString()));
                }

                this.ddlKeyWordType.Items.Add(new ListItem("卡号", "cardSN"));
                this.ddlKeyWordType.Items.Add(new ListItem("手机", "mobile"));

                this.ddlOperatType.Items.Add(new ListItem("增加","add"));
                this.ddlOperatType.Items.Add(new ListItem("减少", "reduce"));

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
                    userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(base.PublishmentSystemInfo.GroupSN,cardSNInfo!=null? cardSNInfo.UserName:string.Empty);
                }
                else
                {
                    if (this.ddlKeyWordType.SelectedValue == "cardSN")
                    {
                        cardSNInfo = DataProviderWX.CardSNDAO.GetCardSNInfo(base.PublishmentSystemID,TranslateUtils.ToInt(this.ddlCard.SelectedValue), this.tbKeyWord.Text, string.Empty);
                        userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(base.PublishmentSystemInfo.GroupSN, cardSNInfo != null ? cardSNInfo.UserName : string.Empty);
                    }
                    else if (this.ddlKeyWordType.SelectedValue == "mobile")
                    {
                        int userID = BaiRongDataProvider.UserDAO.GetUserIDByEmailOrMobile(base.PublishmentSystemInfo.GroupSN, string.Empty, this.tbKeyWord.Text);
                        userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);
                    }
                }

                if (userInfo == null)
                {
                    base.FailMessage("会员不存在");
                    return;
                }

                UserCreditsLogInfo userCreditsLogInfo = new UserCreditsLogInfo();
                userCreditsLogInfo.UserName = userInfo.UserName;
                userCreditsLogInfo.ProductID = AppManager.WeiXin.AppID;
                userCreditsLogInfo.Num = TranslateUtils.ToInt(this.tbCredits.Text);
                userCreditsLogInfo.AddDate = DateTime.Now;

                try
                {
                    if (this.ddlOperatType.SelectedValue == "add")
                    {
                        userCreditsLogInfo.IsIncreased = true;
                        userCreditsLogInfo.Action = "手动添加积分";
                        BaiRongDataProvider.UserCreditsLogDAO.Insert(userCreditsLogInfo);
                        BaiRongDataProvider.UserDAO.AddCredits(userInfo.GroupSN, userInfo.UserName, TranslateUtils.ToInt(this.tbCredits.Text));
                    }
                    else if (this.ddlOperatType.SelectedValue == "reduce")
                    {
                        userCreditsLogInfo.IsIncreased = false;
                        userCreditsLogInfo.Action = "手动扣除积分";
                        BaiRongDataProvider.UserCreditsLogDAO.Insert(userCreditsLogInfo);
                        BaiRongDataProvider.UserDAO.AddCredits(userInfo.GroupSN, userInfo.UserName, -TranslateUtils.ToInt(this.tbCredits.Text));
                    }
                    this.tbCredits.Text = string.Empty;
 
                    base.SuccessMessage("操作成功！");

                    //JsUtils.OpenWindow.CloseModalPage(Page);
                    
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "操作失败！");
                }
             }
        }
    }
}