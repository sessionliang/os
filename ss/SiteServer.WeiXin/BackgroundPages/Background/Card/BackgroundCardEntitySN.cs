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
    public class BackgroundCardEntitySN : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;
         
        public TextBox tbCardSN;
        public TextBox tbUserName;
        public TextBox tbMobile;
         
        public Button btnAdd;
        public Button btnStatus;
        public Button btnImport;
        public Button btnDelete;
        public Button btnReturn;
        public HtmlInputHidden isEntity;

       public int cardID;
      
        public static string GetRedirectUrl(int publishmentSystemID, int cardID, string cardSN, string userName, string mobile, bool isEntity)
        {
            return PageUtils.GetWXUrl(string.Format("background_cardEntitySN.aspx?PublishmentSystemID=" + publishmentSystemID + "&cardID=" + cardID + "&cardSN=" + cardSN + "&userName=" + userName + "&mobile=" + mobile + "&isEntity=" + isEntity));
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
                        DataProviderWX.CardEntitySNDAO.Delete(base.PublishmentSystemID, list);

                        base.SuccessMessage("实体卡删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "实体卡删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.CardEntitySNDAO.GetSelectString(base.PublishmentSystemID, TranslateUtils.ToInt(base.Request.QueryString["cardID"]), base.Request.QueryString["cardSN"], base.Request.QueryString["userName"], base.Request.QueryString["mobile"]);
            this.spContents.SortField = CardSNAttribute.AddDate;
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            { 
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Card, "实体卡管理", AppManager.WeiXin.Permission.WebSite.Card);
                this.spContents.DataBind();

                this.tbCardSN.Text = base.Request.QueryString["cardSN"];
                this.tbUserName.Text = base.Request.QueryString["userName"];
                this.tbMobile.Text = base.Request.QueryString["mobile"];

                this.btnAdd.Attributes.Add("onclick",Modal.CardEntitySNAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID,this.cardID,0));
                this.btnStatus.Attributes.Add("onclick",Modal.CardSNSetting.GetOpenWindowString(base.PublishmentSystemID,TranslateUtils.ToBool(this.isEntity.Value)));
                this.btnImport.Attributes.Add("onclick",Modal.CardEntitySNImport.GetOpenUploadWindowString(base.PublishmentSystemID,this.cardID));
                
                string urlDelete = PageUtils.AddQueryString(BackgroundCardEntitySN.GetRedirectUrl(base.PublishmentSystemID,this.cardID,this.tbCardSN.Text,this.tbUserName.Text,this.tbMobile.Text,TranslateUtils.ToBool(this.isEntity.Value)), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的会员卡", "此操作将删除所选会员卡，确认吗？"));
 
                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false;", BackgroundCard.GetRedirectUrl(base.PublishmentSystemID)));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CardEntitySNInfo cardEntitySNInfo = new CardEntitySNInfo(e.Item.DataItem);
                
                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlAmount = e.Item.FindControl("ltlAmount") as Literal;
                Literal ltlCredits = e.Item.FindControl("ltlCredits") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlIsBinding = e.Item.FindControl("ltlIsBinding") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                 
                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlSN.Text = cardEntitySNInfo.SN;
                ltlUserName.Text = cardEntitySNInfo.UserName;
                ltlMobile.Text = cardEntitySNInfo.Mobile;
                ltlAmount.Text = cardEntitySNInfo.Amount.ToString();
                ltlCredits.Text = cardEntitySNInfo.Credits.ToString();

                ltlAddDate.Text = DateUtils.GetDateAndTimeString(cardEntitySNInfo.AddDate);
                ltlIsBinding.Text = cardEntitySNInfo.IsBinding ? "已绑定" : "未绑定";
                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">修改</a>", Modal.CardEntitySNAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, cardEntitySNInfo.CardID, cardEntitySNInfo.ID));
                  
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
