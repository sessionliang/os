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
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundCardSN : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;
         
        public TextBox tbCardSN;
        public TextBox tbUserName;
        public TextBox tbMobile;
        public TextBox tbUserNameList;
         
        public Button btnAdd;
        public Button btnStatus;
        public Button btnExport;
        public Button btnDelete;
        public Button btnReturn;
        public HtmlInputHidden isEntity;

       public int cardID;
      
        public static string GetRedirectUrl(int publishmentSystemID, int cardID, string cardSN, string userName, string mobile, bool isEntity)
        {
            return PageUtils.GetWXUrl(string.Format("background_cardSN.aspx?PublishmentSystemID=" + publishmentSystemID + "&cardID=" + cardID + "&cardSN=" + cardSN + "&userName=" + userName + "&mobile=" + mobile + "&isEntity=" + isEntity));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.isEntity.Value = base.Request.QueryString["isEntity"];
            this.cardID = TranslateUtils.ToInt(base.Request.QueryString["cardID"]);
           
            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"])) 
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.CardSNDAO.Delete(base.PublishmentSystemID, list);

                        base.SuccessMessage("会员卡删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "会员卡删除失败！");
                    }
                }
            }
            
            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.CardSNDAO.GetSelectString(base.PublishmentSystemID, TranslateUtils.ToInt(base.Request.QueryString["cardID"]), base.Request.QueryString["cardSN"], base.Request.QueryString["userName"], base.Request.QueryString["mobile"]);
            this.spContents.SortField = CardSNAttribute.AddDate;
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
 
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Card, "会员卡管理", AppManager.WeiXin.Permission.WebSite.Card);
                this.spContents.DataBind();

                this.tbCardSN.Text = base.Request.QueryString["cardSN"];
                this.tbUserName.Text = base.Request.QueryString["userName"];
                this.tbMobile.Text = base.Request.QueryString["mobile"];

                string urlAdd = BackgroundCardSNAdd.GetRedirectUrl(base.PublishmentSystemID, this.cardID);
                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlAdd));
               
                this.btnStatus.Attributes.Add("onclick",Modal.CardSNSetting.GetOpenWindowString(base.PublishmentSystemID,this.cardID,TranslateUtils.ToBool(this.isEntity.Value)));
                this.btnExport.Attributes.Add("onclick",Modal.ExportCardSN.GetOpenWindowString (base.PublishmentSystemID,this.cardID));
                string urlDelete = PageUtils.AddQueryString(BackgroundCardSN.GetRedirectUrl(base.PublishmentSystemID,this.cardID,this.tbCardSN.Text,this.tbUserName.Text,this.tbMobile.Text,TranslateUtils.ToBool(this.isEntity.Value)), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的会员卡", "此操作将删除所选会员卡，确认吗？"));
 
                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false;", BackgroundCard.GetRedirectUrl(base.PublishmentSystemID)));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CardSNInfo cardSNInfo = new CardSNInfo(e.Item.DataItem);
                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(base.PublishmentSystemInfo.GroupSN, cardSNInfo.UserName);
                UserContactInfo userContactInfo = BaiRongDataProvider.UserContactDAO.GetContactInfo(cardSNInfo.UserName);
                
                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlAmount = e.Item.FindControl("ltlAmount") as Literal;
                Literal ltlCredits = e.Item.FindControl("ltlCredits") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlIsDisabled = e.Item.FindControl("ltlIsDisabled") as Literal;
                
                Literal ltlConsumeUrl = e.Item.FindControl("ltlConsumeUrl") as Literal;
                Literal ltlRechargeUrl = e.Item.FindControl("ltlRechargeUrl") as Literal;
                Literal ltlCreditesUrl = e.Item.FindControl("ltlCreditesUrl") as Literal;
               
                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlSN.Text = cardSNInfo.SN;
                if(userInfo!=null)
                {
                    ltlUserName.Text = userInfo.DisplayName;
                    ltlMobile.Text = userInfo.Mobile;
                    ltlCredits.Text = userInfo.Credits.ToString();
                }
                ltlAmount.Text = cardSNInfo.Amount.ToString();
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(cardSNInfo.AddDate);
                ltlIsDisabled.Text = cardSNInfo.IsDisabled ? "使用" : "冻结";
                ltlConsumeUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">消费</a>",Modal.CardConsume.GetOpenWindowString(base.PublishmentSystemID,cardSNInfo.CardID,cardSNInfo.ID));
                ltlRechargeUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">充值</a>", Modal.CardRecharge.GetOpenWindowString(base.PublishmentSystemID,cardSNInfo.CardID,cardSNInfo.ID));
                ltlCreditesUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">积分</a>",Modal.CardCredits.GetOpenWindowString(base.PublishmentSystemID,cardSNInfo.CardID,cardSNInfo.ID));
                
            } 
        }
         
        public void Search_OnClick(object sender, EventArgs E)
        {
            Button btn = sender as Button;

            if (btn.Text == "微信会员")
            {
                this.isEntity.Value = "false";
             }
            else if (btn.Text == "实体卡会员")
            {
                this.isEntity.Value = "true";
            }
              
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    if (TranslateUtils.ToBool(this.isEntity.Value))
                    {
                        this._pageUrl = PageUtils.GetWXUrl(string.Format("background_cardEntitySN.aspx?PublishmentSystemID={0}&cardID={1}&cardSN={2}&userName={3}&mobile={4}&isEntity={5}", base.PublishmentSystemID, this.cardID, this.tbCardSN.Text, this.tbUserName.Text, this.tbMobile.Text, this.isEntity.Value));
                    }
                    else
                    {
                        this._pageUrl = PageUtils.GetWXUrl(string.Format("background_cardSN.aspx?PublishmentSystemID={0}&cardID={1}&cardSN={2}&userName={3}&mobile={4}&isEntity={5}", base.PublishmentSystemID, this.cardID, this.tbCardSN.Text, this.tbUserName.Text, this.tbMobile.Text, this.isEntity.Value));
                    }
                   
                }
                return this._pageUrl;
            }
        }
    }
}
