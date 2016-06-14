using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Model;
using System.Collections.Specialized;
using SiteServer.WeiXin.Core;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundCardExchangeLog : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public DropDownList ddlCard;
        public TextBox tbCardSN;
        public TextBox tbUserName;
        public TextBox tbMobile;
         
        public Button btnDelete;
        public Button btnReturn;
        public static string GetRedirectUrl(int publishmentSystemID,string cardSN,string userName,string mobile)
        {
            return PageUtils.GetWXUrl(string.Format("background_cardExchangeLog.aspx?PublishmentSystemID=" + publishmentSystemID + "&cardSN=" + cardSN + "&userName=" + userName + "&mobile=" + mobile));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
             
            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"])) 
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.CardCashLogDAO.Delete(base.PublishmentSystemID, list);

                        base.SuccessMessage("积分兑换记录删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "积分兑换记录删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.CardCashLogDAO.GetSelectString(base.PublishmentSystemID, ECashType.Exchange, TranslateUtils.ToInt(base.Request.QueryString["cardID"]), base.Request.QueryString["cardSN"], base.Request.QueryString["userName"], base.Request.QueryString["mobile"]);
            this.spContents.SortField = CardCashLogAttribute.AddDate;
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
               
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Card, "会员积分兑换管理", AppManager.WeiXin.Permission.WebSite.Card);
                List<CardInfo> cardInfoList = DataProviderWX.CardDAO.GetCardInfoList(base.PublishmentSystemID);
                foreach (CardInfo cardInfo in cardInfoList)
                {
                    this.ddlCard.Items.Add(new ListItem(cardInfo.CardTitle, cardInfo.ID.ToString()));
                }
                 
                this.spContents.DataBind();

                this.ddlCard.SelectedValue = base.Request.QueryString["cardID"];
                this.tbCardSN.Text = base.Request.QueryString["cardSN"];
                this.tbUserName.Text = base.Request.QueryString["userName"];
                this.tbMobile.Text = base.Request.QueryString["mobile"];

                string urlDelete = PageUtils.AddQueryString(BackgroundCardConsumeLog.GetRedirectUrl(base.PublishmentSystemID, this.tbCardSN.Text, this.tbUserName.Text, this.tbMobile.Text), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的兑换记录", "此操作将删除所选兑换记录，确认吗？"));

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false;", BackgroundNavTransaction.GetRedirectUrl(base.PublishmentSystemID)));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CardCashLogInfo cardCashLogInfo = new CardCashLogInfo(e.Item.DataItem);
                CardSNInfo cardSNInfo = DataProviderWX.CardSNDAO.GetCardSNInfo(cardCashLogInfo.CardSNID);
                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(base.PublishmentSystemInfo.GroupSN, cardCashLogInfo.UserName);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlAmount = e.Item.FindControl("ltlAmount") as Literal;
                Literal ltlBeforeAmount = e.Item.FindControl("ltlBeforeAmount") as Literal;
                Literal ltlAfterAmount = e.Item.FindControl("ltlAfterAmount") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();

                ltlAmount.Text = cardCashLogInfo.Amount.ToString();
                ltlBeforeAmount.Text = (cardCashLogInfo.CurAmount - cardCashLogInfo.Amount).ToString();
                ltlAfterAmount.Text = cardCashLogInfo.CurAmount.ToString();
                ltlUserName.Text = userInfo != null ? userInfo.UserName : string.Empty;
                ltlMobile.Text = userInfo != null ? userInfo.Mobile : string.Empty;
                ltlSN.Text = cardSNInfo != null ? cardSNInfo.SN : string.Empty;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(cardCashLogInfo.AddDate);
                ltlDescription.Text = cardCashLogInfo.Description;
            } 
        }
         
        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = PageUtils.GetWXUrl(string.Format("background_cardExchangeLog.aspx?PublishmentSystemID={0}&cardID={1}&cardSN={2}&userName={3}&mobile={4}", base.PublishmentSystemID, this.ddlCard.SelectedValue, this.tbCardSN.Text, this.tbUserName.Text, this.tbMobile.Text));
                }
                return this._pageUrl;
            }
        }
    }
}
