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
    public class BackgroundCardSNAdd : BackgroundBasePageWX
    {
        public Literal ltlPageTitle;
        public TextBox tbUserNameList;
        public Literal ltlSelectUser;

        public Button btnSubmit;
        public Button btnReturn;

        private int cardID;

        public static string GetRedirectUrl(int publishmentSystemID, int CardID)
        {
            return PageUtils.GetWXUrl(string.Format("background_cardSNAdd.aspx?publishmentSystemID={0}&CardID={1}", publishmentSystemID, CardID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.cardID = TranslateUtils.ToInt(base.GetQueryString("CardID"));

            if (!IsPostBack)
            {
                string pageTitle = "领取会员卡"; 
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Card, pageTitle, AppManager.WeiXin.Permission.WebSite.Card);
                this.ltlPageTitle.Text = pageTitle;

                if (this.cardID > 0)
                {

                }
                this.ltlSelectUser.Text = string.Format(@"&nbsp;<a href=""javascript:;"" onclick=""{0}"" style=""vertical-align:bottom"">选择</a>", SiteServer.CMS.BackgroundPages.Modal.UserSelect.GetOpenWindowString(base.PublishmentSystemID, this.tbUserNameList.ClientID));

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundCardSN.GetRedirectUrl(base.PublishmentSystemID, this.cardID, string.Empty, string.Empty, string.Empty, false)));
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            { 
                try
                {
                    if (!string.IsNullOrEmpty(this.tbUserNameList.Text))
                    {
                        List<string> userNameList = TranslateUtils.StringCollectionToStringList(this.tbUserNameList.Text);
                        foreach (string userName in userNameList)
                        {
                            CardSNInfo cardSNInfo = new CardSNInfo();
                            cardSNInfo.PublishmentSystemID = base.PublishmentSystemID;
                            cardSNInfo.CardID = this.cardID;
                            cardSNInfo.SN = DataProviderWX.CardSNDAO.GetNextCardSN(base.PublishmentSystemID, this.cardID);
                            cardSNInfo.Amount = 0;
                            cardSNInfo.UserName = userName;
                            cardSNInfo.IsDisabled = true;
                            cardSNInfo.AddDate = DateTime.Now;

                            bool isExist = DataProviderWX.CardSNDAO.isExists(base.PublishmentSystemID, this.cardID, userName);
                            if(!isExist)
                            {
                                DataProviderWX.CardSNDAO.Insert(cardSNInfo);
                            }
                          
                        }
                   }

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "领取会员卡成功", string.Format("会员卡:{0}", this.tbUserNameList.Text));
                    base.SuccessMessage("领取会员卡成功！");

                   // string redirectUrl = PageUtils.GetWXUrl(string.Format("background_cardSN.aspx?publishmentSystemID={0}&CardID={1}&cardSN={2}&userName={3}&mobile={4}&isEntity={5}", base.PublishmentSystemID, this.cardID,string.Empty,string.Empty,string.Empty,false));
                    //base.AddWaitAndRedirectScript(redirectUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "领取会员卡失败！");
                }
            }

        }
    }
}