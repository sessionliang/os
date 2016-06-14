using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using SiteServer.Project.Model;
using SiteServer.Project.Core;
using BaiRong.Controls;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class OrderSelect : BackgroundBasePage
	{
        public DropDownList ddlType;
        public TextBox tbLoginName;
        public TextBox tbKeyword;
        public Literal ltlTotalCount;

        public Repeater rptContents;
        public SqlPager spContents;

        private string scriptName;

        public static string GetShowPopWinString(string scriptName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("scriptName", scriptName);
            return JsUtils.OpenWindow.GetOpenWindowString("选择订单", "modal_orderSelect.aspx", arguments, true);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.scriptName = base.Request.QueryString["scriptName"];

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProvider.OrderDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(base.Request.QueryString["OrderID"]))
                {
                    int orderID = TranslateUtils.ToInt(base.Request.QueryString["OrderID"]);
                    OrderInfo orderInfo = DataProvider.OrderDAO.GetOrderInfo(orderID);
                    string scripts = string.Format("window.parent.{0}('{1}', '{2}', '{3}', '{4}', '{5}', '{6}');", this.scriptName, orderInfo.SN, orderID, orderInfo.InvoiceTitle, orderInfo.InvoiceReceiver, orderInfo.InvoiceTel, orderInfo.InvoiceAddress);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else
                {
                    EOrderStatusUtils.AddListItems(EOrderType.Aliyun_Moban, this.ddlType);
                    this.ddlType.Items.Add(new ListItem("续费订单", OrderAttribute.IsReNew));
                    this.ddlType.Items.Add(new ListItem("已开发票", OrderAttribute.IsInvoice));
                    this.ddlType.Items.Add(new ListItem("已退款", OrderAttribute.IsRefund));
                    this.ddlType.Items.Insert(0, new ListItem("请选择", string.Empty));
                    if (!string.IsNullOrEmpty(base.Request.QueryString["type"]))
                    {
                        ControlUtils.SelectListItemsIgnoreCase(this.ddlType, base.Request.QueryString["type"]);
                    }

                    this.tbLoginName.Text = base.Request.QueryString["loginName"];
                    this.tbKeyword.Text = base.Request.QueryString["keyword"];

                    this.spContents.DataBind();

                    this.ltlTotalCount.Text = this.spContents.TotalCount.ToString();
                }
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderInfo orderInfo = new OrderInfo(e.Item.DataItem);

                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                Literal ltlLoginName = e.Item.FindControl("ltlLoginName") as Literal;
                Literal ltlAmount = e.Item.FindControl("ltlAmount") as Literal;
                Literal ltlDomainTemp = e.Item.FindControl("ltlDomainTemp") as Literal;

                Literal ltlMobanID = e.Item.FindControl("ltlMobanID") as Literal;
                Literal ltlEmail = e.Item.FindControl("ltlEmail") as Literal;
                Literal ltlQQ = e.Item.FindControl("ltlQQ") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;

                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;

                ltlSN.Text = string.Format(@"<a href=""modal_orderSelect.aspx?scriptName={0}&OrderID={1}"">{2}</a>", this.scriptName, orderInfo.ID, orderInfo.SN);
                ltlLoginName.Text = orderInfo.LoginName;
                if (orderInfo.IsReNew)
                {
                    ltlLoginName.Text += string.Format(@"<code>续费{0}年</code>", orderInfo.Duration);
                }

                ltlAmount.Text = orderInfo.Amount.ToString("c");
                ltlDomainTemp.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtils.AddProtocolToUrl(orderInfo.DomainTemp), orderInfo.DomainTemp);
                if (!string.IsNullOrEmpty(orderInfo.MobanID))
                {
                    ltlMobanID.Text = orderInfo.MobanID + "<code>" + DataProvider.OrderDAO.GetMobanUsedCount(orderInfo.MobanID).ToString() + "</code>";
                }
                ltlEmail.Text = orderInfo.Email;
                ltlQQ.Text = orderInfo.QQ;
                ltlAddDate.Text = DateUtils.GetDateString(orderInfo.AddDate);

                ltlStatus.Text = string.Format(@"<a href=""javascript:;"" class=""btn {0}"">{1}</a>", EOrderStatusUtils.GetClass(orderInfo.Status), EOrderStatusUtils.GetText(orderInfo.Status));
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        protected string GetSelectString()
        {
            if (base.Request.QueryString["type"] != null)
            {
                return DataProvider.OrderDAO.GetSelectString(EOrderType.Aliyun_Moban, base.Request.QueryString["type"], base.Request.QueryString["loginName"], base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.OrderDAO.GetSelectString(EOrderType.Aliyun_Moban);
            }
        }

        private string _pageUrl;
        protected string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = OrderSelect.GetRedirectUrl(this.scriptName, this.ddlType.SelectedValue, this.tbLoginName.Text, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(string scriptName, string type, string loginName, string keyword, int page)
        {
            return string.Format("modal_orderSelect.aspx?scriptName={0}&type={1}&loginName={2}&keyword={3}&page={4}", scriptName, type, loginName, keyword, page);
        }
	}
}
