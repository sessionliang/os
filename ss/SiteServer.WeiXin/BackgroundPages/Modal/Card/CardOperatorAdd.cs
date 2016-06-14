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
using SiteServer.CMS;
using System.Collections.Generic;
using System.Text;


namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class CardOperatorAdd : BackgroundBasePage
    {
        public Literal ltlOperatorItems;

        private int cardID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int cardID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("cardID", cardID.ToString());
            return PageUtilityWX.GetOpenWindowString("»áÔ±¿¨²Ù×÷Ô±", "modal_cardOperatorAdd.aspx", arguments, 450, 450);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.cardID = TranslateUtils.ToInt(base.GetQueryString("cardID"));

            if (!IsPostBack)
            {
                CardInfo cardInfo = DataProviderWX.CardDAO.GetCardInfo(this.cardID);

                List<CardOperatorInfo> operatorInfoList = new List<CardOperatorInfo>();
                operatorInfoList = TranslateUtils.JsonToObject(cardInfo.ShopOperatorList, operatorInfoList) as List<CardOperatorInfo>;
                if (operatorInfoList != null)
                {
                    StringBuilder operatorBuilder = new StringBuilder();
                    foreach (CardOperatorInfo operatorInfo in operatorInfoList)
                    {
                        operatorBuilder.AppendFormat(@"{{userName: '{0}', password: '{1}'}},", operatorInfo.UserName, operatorInfo.Password);
                    }
                    if (operatorBuilder.Length > 0) operatorBuilder.Length--;

                    this.ltlOperatorItems.Text = string.Format(@"itemController.itemCount = {0};itemController.items = [{1}];", operatorInfoList.Count, operatorBuilder.ToString());
                }
                else
                {
                    this.ltlOperatorItems.Text = "itemController.itemCount = 0;itemController.items = [{}];";
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {

            if (IsPostBack && IsValid)
            {
                string errorMessage = string.Empty;

                try
                {
                    int itemCount = TranslateUtils.ToInt(base.Request.Form["itemCount"]);
                    List<string> userNameList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemUserName"]);
                    List<string> passwordList = TranslateUtils.StringCollectionToStringList(base.Request.Form["itemPassword"]);
                    List<CardOperatorInfo> operatorInfoList = new List<CardOperatorInfo>();

                    if ( userNameList.Count ==0)
                    {
                       errorMessage=string.Format("±£´æÊ§°Ü,ÐÕÃûÎª¿Õ£¡");
                    }
                    else if (passwordList.Count == 0)
                    {
                        errorMessage = string.Format("±£´æÊ§°Ü,ÃÜÂëÎª¿Õ£¡");
                    }
                    if (itemCount == userNameList.Count && itemCount == passwordList.Count)
                    {
                        for (int i = 0; i < itemCount; i++)
                        {
                            string userName = userNameList[i];
                            string password = passwordList[i];
                            if (string.IsNullOrEmpty(userName))
                            {
                                errorMessage = string.Format("±£´æÊ§°Ü,ÐÕÃûÎª¿Õ£¡");
                                break;
                            }
                            if (string.IsNullOrEmpty(password))
                            {
                                errorMessage = string.Format("±£´æÊ§°Ü,ÃÜÂëÎª¿Õ£¡");
                                break;
                            }

                            CardOperatorInfo operatorInfo = new CardOperatorInfo { UserName = userName, Password = password };

                            operatorInfoList.Add(operatorInfo);
                        }
                    }

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        base.FailMessage(errorMessage);
                    }
                    else
                    {
                        CardInfo cardInfo = DataProviderWX.CardDAO.GetCardInfo(this.cardID);
                        cardInfo.ShopOperatorList = TranslateUtils.ObjectToJson(operatorInfoList);
                        DataProviderWX.CardDAO.Update(cardInfo);

                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage("±£´æÊ§°Ü"+ex.ToString());
                }
             }
        }
    }
}