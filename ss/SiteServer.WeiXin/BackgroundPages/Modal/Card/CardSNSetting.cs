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
using System.Web.UI;
using System.Collections.Generic;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class CardSNSetting : BackgroundBasePage
	{ 
        public DropDownList ddlIsDisabled;
        public DropDownList ddlIsBinding;
        public Control IsDisabledRow;
        public Control IsBindingRow;

        private int cardID;
        private bool isEntity;
        public static string GetOpenWindowString(int publishmentSystemID,int cardID,bool isEntity)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("cardID",cardID.ToString());
            arguments.Add("isEntity",isEntity.ToString());
            return PageUtilityWX.GetOpenWindowStringWithCheckBoxValue("���û�Ա��״̬", "modal_cardSNSetting.aspx", arguments, "IDCollection", "��ѡ����Ҫ���õĻ�Ա��", 400, 300);
        }

        public static string GetOpenWindowString(int publishmentSystemID, bool isEntity)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("isEntity", isEntity.ToString());
            return PageUtilityWX.GetOpenWindowStringWithCheckBoxValue("����ʵ�忨״̬", "modal_cardSNSetting.aspx", arguments, "IDCollection", "��ѡ����Ҫ���õĻ�Ա��", 400, 300);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.cardID = TranslateUtils.ToInt(base.Request.QueryString["cardID"]);
            this.isEntity = TranslateUtils.ToBool(base.Request.QueryString["isEntity"]);

			if (!IsPostBack)
			{
                this.ddlIsDisabled.Items.Add(new ListItem("����", "false"));
                this.ddlIsDisabled.Items.Add(new ListItem("�������", "true"));

                this.ddlIsBinding.Items.Add(new ListItem("��", "true"));
                this.ddlIsBinding.Items.Add(new ListItem("�����", "false"));

                if (!this.isEntity)
                { 
                    this.IsBindingRow.Visible = false;
                }
                else
                {
                    this.IsDisabledRow.Visible = false;
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                if (!this.isEntity)
                {
                    DataProviderWX.CardSNDAO.UpdateStatus(this.cardID, TranslateUtils.ToBool(this.ddlIsDisabled.SelectedValue), TranslateUtils.StringCollectionToIntList(base.Request.QueryString["IDCollection"]));
                }
                else
                {
                    List<int> cardEntitySNIDList = TranslateUtils.StringCollectionToIntList(base.Request.QueryString["IDCollection"]);
                    if (cardEntitySNIDList.Count > 0)
                    {
                        for (int i = 0; i < cardEntitySNIDList.Count; i++)
                        {
                            CardEntitySNInfo cardEntitySNInfo = DataProviderWX.CardEntitySNDAO.GetCardEntitySNInfo(cardEntitySNIDList[i]);

                            int userID = BaiRongDataProvider.UserDAO.GetUserIDByEmailOrMobile(base.PublishmentSystemInfo.GroupSN, string.Empty, cardEntitySNInfo.Mobile);
                            UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);
                            if (userInfo != null)
                            {
                                CardSNInfo cardSNInfo = DataProviderWX.CardSNDAO.GetCardSNInfo(base.PublishmentSystemID,this.cardID, string.Empty, userInfo.UserName);
                                 
                                CardCashLogInfo cardCashLogInfo = new CardCashLogInfo();
                                cardCashLogInfo.PublishmentSystemID = base.PublishmentSystemID;
                                cardCashLogInfo.UserName = userInfo.UserName;
                                cardCashLogInfo.CardID = cardSNInfo.CardID;
                                cardCashLogInfo.CardSNID = cardSNInfo.ID;
                                cardCashLogInfo.Amount = cardEntitySNInfo.Amount;
                                cardCashLogInfo.CurAmount += cardEntitySNInfo.Amount; ;
                                cardCashLogInfo.CashType = ECashTypeUtils.GetValue(ECashType.Recharge);
                                cardCashLogInfo.Operator = AdminManager.Current.UserName;
                                cardCashLogInfo.Description = "��ʵ�忨��ֵ";
                                cardCashLogInfo.AddDate = DateTime.Now;

                                UserCreditsLogInfo userCreditsLogInfo = new UserCreditsLogInfo();
                                userCreditsLogInfo.UserName = userInfo.UserName;
                                userCreditsLogInfo.ProductID = AppManager.WeiXin.AppID;
                                userCreditsLogInfo.Num = cardEntitySNInfo.Credits;
                                userCreditsLogInfo.IsIncreased = true;
                                userCreditsLogInfo.Action = "��ʵ�忨��ӻ���";
                                userCreditsLogInfo.AddDate = DateTime.Now;
                                 

                                if (!cardEntitySNInfo.IsBinding)
                                { 
                                    cardEntitySNInfo.IsBinding = true;
                                    DataProviderWX.CardEntitySNDAO.Update(cardEntitySNInfo);

                                    DataProviderWX.CardCashLogDAO.Insert(cardCashLogInfo);
                                    DataProviderWX.CardSNDAO.Recharge(cardSNInfo.ID, userInfo.UserName, cardEntitySNInfo.Amount);
                                     
                                    BaiRongDataProvider.UserCreditsLogDAO.Insert(userCreditsLogInfo);
                                    BaiRongDataProvider.UserDAO.AddCredits(userInfo.GroupSN, userInfo.UserName, cardEntitySNInfo.Credits);
                                }
                            }
                        }
                     }
                 }

                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "ʧ�ܣ�" + ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
		}
	}
}
