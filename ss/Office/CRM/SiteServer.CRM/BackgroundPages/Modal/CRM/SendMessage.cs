using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CRM.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

using SiteServer.CRM.Model;
using System.Text;
using BaiRong.Model;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages.Modal
{
	public class SendMessage : BackgroundBasePage
	{
        public TextBox tbMessageTo;
        public TextBox tbMessage;
        private bool isSMS;
        private OrderInfo orderInfo;

        public static string GetShowPopWinStringBySMS(int orderID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("isSMS", true.ToString());
            arguments.Add("orderID", orderID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("发送短信", "modal_sendMessage.aspx", arguments, 500, 350);
        }

        public static string GetShowPopWinStringByEmail(int orderID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("isSMS", false.ToString());
            arguments.Add("orderID", orderID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("发送邮件", "modal_sendMessage.aspx", arguments, 500, 350);
        }       
 
		public void Page_Load(object sender, EventArgs E)
		{
            this.isSMS = TranslateUtils.ToBool(base.Request.QueryString["isSMS"]);
            int orderID = TranslateUtils.ToInt(base.Request.QueryString["orderID"]);
            this.orderInfo = DataProvider.OrderDAO.GetOrderInfo(orderID);

			if (!IsPostBack)
			{
                if (this.isSMS)
                {
                    if (this.orderInfo != null && !string.IsNullOrEmpty(orderInfo.Mobile))
                    {
                        this.tbMessageTo.Text = orderInfo.Mobile;
                    }
                }
                else
                {
                    if (this.orderInfo != null && !string.IsNullOrEmpty(orderInfo.Email))
                    {
                        this.tbMessageTo.Text = orderInfo.Email;
                    }
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            try
            {
                string errorMessage = string.Empty;

                if (this.isSMS)
                {
                    isChanged = MessageManager.SendSMS(this.tbMessageTo.Text, this.tbMessage.Text, out errorMessage);
                }
                else
                {
                    isChanged = MessageManager.SendMail(this.tbMessageTo.Text, this.tbMessage.Text, string.Empty, out errorMessage);
                }
                
                if (isChanged == false)
                {
                    base.FailMessage(errorMessage);
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
                isChanged = false;
            }

			if (isChanged)
			{
                if (this.orderInfo != null)
                {
                    TouchInfo touchInfo = null;
                    if (this.isSMS)
                    {
                        touchInfo = new TouchInfo { OrderID = this.orderInfo.ID, TouchBy = "SMS", AddDate = DateTime.Now, AddUserName = AdminManager.Current.UserName, ContactName = this.tbMessageTo.Text, Summary = this.tbMessage.Text };
                    }
                    else
                    {
                        touchInfo = new TouchInfo { OrderID = this.orderInfo.ID, TouchBy = "Email", AddDate = DateTime.Now, AddUserName = AdminManager.Current.UserName, ContactName = this.tbMessageTo.Text, Summary = this.tbMessage.Text };
                    }
                    DataProvider.TouchDAO.Insert(touchInfo);
                }

                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page, "parent.openTips('消息发送成功', 'success')");
			}
		}

	}
}
