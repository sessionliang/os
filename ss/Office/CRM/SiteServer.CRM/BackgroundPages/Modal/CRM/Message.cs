using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using SiteServer.CRM.Model;
using SiteServer.CRM.Core;
using BaiRong.Controls;
using System.Collections.Generic;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages.Modal
{
	public class Message : BackgroundBasePage
	{
        public Repeater rptContents;
        public SqlPager spContents;

        private int orderID;

        public static string GetShowPopWinString(int orderID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("orderID", orderID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("发送消息", "modal_message.aspx", arguments, true);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.orderID = TranslateUtils.ToInt(base.Request.QueryString["orderID"]);

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.OrderMessageDAO.GetSelectString(); ;
            this.spContents.SortField = DataProvider.OrderMessageDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(base.Request.QueryString["messageID"]))
                {
                    int messageID = TranslateUtils.ToInt(base.Request.QueryString["messageID"]);
                    OrderMessageInfo messageInfo = DataProvider.OrderMessageDAO.GetOrderMessageInfo(messageID);
                    OrderInfo orderInfo = DataProvider.OrderDAO.GetOrderInfo(this.orderID);
                    if (messageInfo != null && orderInfo != null)
                    {
                        string errorMessage = string.Empty;
                        bool isSuccess = false;

                        //orderInfo.Mobile = "13911897774";
                        //orderInfo.Email = "laixx@siteserver.cn";

                        MobanInfo mobanInfo = DataProvider.MobanDAO.GetMobanInfo(orderInfo.MobanID);
                        Dictionary<string, string> placeList = new Dictionary<string, string>();
                        foreach (string attributeName in OrderAttribute.AllAttributes)
                        {
                            placeList.Add("{{order." + attributeName + "}}", orderInfo.GetExtendedAttribute(attributeName).ToLower());
                        }
                        foreach (string attributeName in MobanAttribute.AllAttributes)
                        {
                            placeList.Add("{{moban." + attributeName + "}}", mobanInfo.GetExtendedAttribute(attributeName).ToLower());
                        }
                        placeList.Add("{{moban.Url}}", DataProvider.MobanDAO.GetMobanUrl(mobanInfo));

                        foreach (var place in placeList)
                        {
                            messageInfo.TemplateSMS = StringUtils.ReplaceIgnoreCase(messageInfo.TemplateSMS, place.Key, place.Value);
                            messageInfo.TemplateEmail = StringUtils.ReplaceIgnoreCase(messageInfo.TemplateEmail, place.Key, place.Value);
                        }

                        if (messageInfo.IsSMS)
                        {
                            string messageTo = orderInfo.Mobile;
                            string message = messageInfo.TemplateSMS;

                            isSuccess = MessageManager.SendSMS(messageTo, message, out errorMessage);
                            if (isSuccess)
                            {
                                TouchInfo touchInfo = new TouchInfo { OrderID = this.orderID, MessageID = messageID, TouchBy = "SMS", AddDate = DateTime.Now, AddUserName = AdminManager.Current.UserName, ContactName = messageTo, Summary = message };
                                DataProvider.TouchDAO.Insert(touchInfo);
                            }
                        }
                        if (messageInfo.IsEmail)
                        {
                            string messageTo = orderInfo.Email;
                            string message = messageInfo.TemplateEmail;

                            isSuccess = MessageManager.SendMail(messageTo, message, string.Empty, out errorMessage);
                            if (isSuccess)
                            {
                                TouchInfo touchInfo = new TouchInfo { OrderID = this.orderID, MessageID = messageID, TouchBy = "Email", AddDate = DateTime.Now, AddUserName = AdminManager.Current.UserName, ContactName = messageTo, Summary = message };
                                DataProvider.TouchDAO.Insert(touchInfo);
                            }
                        }

                        if (isSuccess)
                        {
                             base.SuccessMessage("消息发送成功");
                        }
                        else
                        {
                            base.FailMessage(string.Format("消息发送失败：{0}", errorMessage));
                        }
                    }
                }

                this.spContents.DataBind();
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderMessageInfo messageInfo = new OrderMessageInfo(e.Item.DataItem);

                Literal ltlIndex = e.Item.FindControl("ltlIndex") as Literal;
                Literal ltlMessageName = e.Item.FindControl("ltlMessageName") as Literal;
                Literal ltlIsSMS = e.Item.FindControl("ltlIsSMS") as Literal;
                Literal ltlTemplateSMS = e.Item.FindControl("ltlTemplateSMS") as Literal;
                Literal ltlIsEmail = e.Item.FindControl("ltlIsEmail") as Literal;
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;

                ltlIndex.Text = Convert.ToString(e.Item.ItemIndex + 1);
                ltlMessageName.Text = messageInfo.MessageName;

                ltlIsSMS.Text = StringUtils.GetTrueImageHtml(messageInfo.IsSMS);
                ltlTemplateSMS.Text = messageInfo.TemplateSMS;
                ltlIsEmail.Text = StringUtils.GetTrueImageHtml(messageInfo.IsEmail);

                if (!DataProvider.TouchDAO.IsMessageIDExists(0, this.orderID, messageInfo.ID))
                {
                    ltlStatus.Text = string.Format(@"<a href=""modal_message.aspx?orderID={0}&messageID={1}"" class=""btn btn-success"">发送消息</a>", this.orderID, messageInfo.ID);
                }
                else
                {
                    ltlStatus.Text = @"<code>消息已发送</code>";
                }
            }
        }
	}
}
