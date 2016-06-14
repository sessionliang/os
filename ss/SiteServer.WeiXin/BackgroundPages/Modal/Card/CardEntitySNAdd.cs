using System;
using System.Collections;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;
using System.Web.UI.HtmlControls;
using SiteServer.WeiXin.Model;
using SiteServer.WeiXin.Core;


namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class CardEntitySNAdd : BackgroundBasePage
    {
        public TextBox tbCardSN;
        public TextBox tbUserName;
        public TextBox tbAmount;
        public TextBox tbCredits;
        public TextBox tbEmail;
        public TextBox tbMobile;
        public TextBox tbAddress;

        private int cardID;
        private int cardEntitySNID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int cardID, int cardEntitySNID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("cardID", cardID.ToString());
            arguments.Add("cardEntitySNID", cardEntitySNID.ToString());
            return PageUtilityWX.GetOpenWindowString("新增实体卡", "modal_cardEntitySNAdd.aspx", arguments, 450, 450);
        }
        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int cardID, int cardEntitySNID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("cardID", cardID.ToString());
            arguments.Add("cardEntitySNID", cardEntitySNID.ToString());
            return PageUtilityWX.GetOpenWindowString("编辑实体卡", "modal_cardEntitySNAdd.aspx", arguments, 450, 450);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.cardID = TranslateUtils.ToInt(base.GetQueryString("cardID"));
            this.cardEntitySNID = TranslateUtils.ToInt(base.GetQueryString("cardEntitySNID"));

            if (!IsPostBack)
            {

                if (this.cardEntitySNID > 0)
                {
                    CardEntitySNInfo cardEntitySNInfo = DataProviderWX.CardEntitySNDAO.GetCardEntitySNInfo(this.cardEntitySNID);

                    if (cardEntitySNInfo != null)
                    {
                        this.tbCardSN.Text = cardEntitySNInfo.SN;
                        this.tbCardSN.Enabled = false;
                        this.tbUserName.Text = cardEntitySNInfo.UserName;
                        this.tbAmount.Text = cardEntitySNInfo.Amount.ToString();
                        this.tbCredits.Text = cardEntitySNInfo.Credits.ToString();
                        this.tbEmail.Text = cardEntitySNInfo.Email;
                        this.tbMobile.Text = cardEntitySNInfo.Mobile;
                        this.tbAddress.Text = cardEntitySNInfo.Address;
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {

            if (IsPostBack && IsValid)
            {
                CardEntitySNInfo cardEntitySNInfo = new CardEntitySNInfo();

                if (this.cardEntitySNID > 0)
                {
                    cardEntitySNInfo = DataProviderWX.CardEntitySNDAO.GetCardEntitySNInfo(this.cardEntitySNID);
                }

                cardEntitySNInfo.PublishmentSystemID = base.PublishmentSystemID;
                cardEntitySNInfo.CardID = this.cardID;
                cardEntitySNInfo.SN = this.tbCardSN.Text;
                cardEntitySNInfo.UserName = this.tbUserName.Text;
                cardEntitySNInfo.Amount = TranslateUtils.ToDecimal(this.tbAmount.Text);
                cardEntitySNInfo.Credits = TranslateUtils.ToInt(this.tbCredits.Text);
                cardEntitySNInfo.Mobile = this.tbMobile.Text;
                cardEntitySNInfo.Email = this.tbEmail.Text;
                cardEntitySNInfo.Address = this.tbAddress.Text;

                if (cardEntitySNID > 0)
                {
                    try
                    {
                        DataProviderWX.CardEntitySNDAO.Update(cardEntitySNInfo);

                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(string.Format("实体卡修改失败：{0}", ex.ToString()));
                    }
                }
                else
                {
                    bool isExist = DataProviderWX.CardEntitySNDAO.IsExist(base.PublishmentSystemID, this.cardID, cardEntitySNInfo.SN);
                    bool isExistMobile = DataProviderWX.CardEntitySNDAO.IsExistMobile(base.PublishmentSystemID, this.cardID, cardEntitySNInfo.Mobile);
                   
                    if (isExistMobile)
                    {
                        base.FailMessage(string.Format("手机号使用，请更换手机号！"));
                        return;
                    }

                    if (isExist)
                    {
                        base.FailMessage(string.Format("{0}实体卡号已存在！", cardEntitySNInfo.SN));
                        return;
                    }
                    else
                    { 
                        try
                        {
                            cardEntitySNInfo.IsBinding = false;
                            cardEntitySNInfo.AddDate = DateTime.Now;
                            DataProviderWX.CardEntitySNDAO.Insert(cardEntitySNInfo);

                            JsUtils.OpenWindow.CloseModalPage(Page);
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(string.Format("实体卡新增失败：{0}", ex.ToString()));
                        }
                    }
                }
            }
        }
    }
}