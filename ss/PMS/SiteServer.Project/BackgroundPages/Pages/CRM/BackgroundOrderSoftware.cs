using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Web.UI;


namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundOrderSoftware : BackgroundBasePage
    {
        public DropDownList ddlType;
        public TextBox tbLoginName;
        public TextBox tbKeyword;
        public Literal ltlTotalCount;

        public HyperLink hlAdd;
        public HyperLink hlSetting;
        public HyperLink hlDelete;

        public Repeater rptContents;
        public SqlPager spContents;

        private Hashtable typeHashtable = new Hashtable();

        public void Page_Load(object sender, EventArgs E)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.QueryString["IDCollection"]);
                if (arraylist.Count > 0)
                {
                    try
                    {
                        DataProvider.OrderDAO.Delete(arraylist);
                        base.SuccessMessage("删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["deleteOrderForm"]))
            {
                int orderID = TranslateUtils.ToInt(Request.QueryString["orderID"]);
                if (orderID > 0)
                {
                    try
                    {
                        DataProvider.OrderFormDAO.DeleteByOrderID(orderID);
                        base.SuccessMessage("表单删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "表单删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProvider.OrderDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                EOrderStatusUtils.AddListItems(EOrderType.Aliyun_Software, this.ddlType);
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

                this.hlAdd.NavigateUrl = BackgroundOrderSoftwareAdd.GetAddUrl(this.PageUrl);

                this.hlSetting.Attributes.Add("onclick", Modal.OrderSetting.GetShowPopWinString(EOrderType.Aliyun_Software));

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的订单！", "此操作将删除所选订单，确定吗？"));

                this.ltlTotalCount.Text = this.spContents.TotalCount.ToString();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderInfo orderInfo = new OrderInfo(e.Item.DataItem);

                Literal ltlLoginName = e.Item.FindControl("ltlLoginName") as Literal;
                Literal ltlAmount = e.Item.FindControl("ltlAmount") as Literal;
                Literal ltlBizType = e.Item.FindControl("ltlBizType") as Literal;
                Literal ltlEmail = e.Item.FindControl("ltlEmail") as Literal;
                Literal ltlQQ = e.Item.FindControl("ltlQQ") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlIsContract = e.Item.FindControl("ltlIsContract") as Literal;
                Literal ltlIsInvoice = e.Item.FindControl("ltlIsInvoice") as Literal;
                Literal ltlIsRefund = e.Item.FindControl("ltlIsRefund") as Literal;
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlLoginName.Text = orderInfo.LoginName;
                if (orderInfo.IsReNew)
                {
                    ltlLoginName.Text += string.Format(@"<code>续费{0}月</code>", orderInfo.Duration);
                }

                ltlAmount.Text = orderInfo.Amount.ToString("c");
                ltlBizType.Text = orderInfo.BizType;
                ltlEmail.Text = orderInfo.Email;
                ltlQQ.Text = orderInfo.QQ;
                ltlAddDate.Text = DateUtils.GetDateString(orderInfo.AddDate);

                if (!orderInfo.IsContract)
                {
                    ltlIsContract.Text = string.Format(@"<a href=""{0}""><i class=""icon-plus""></i></a>", BackgroundContractAdd.GetAddUrlToSiteYun(orderInfo.ID, this.PageUrl));
                }
                else
                {
                    ltlIsContract.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundContractAdd.GetEditUrlToSiteYun(orderInfo.ID, this.PageUrl), StringUtils.GetTrueImageHtml(true));
                }

                if (!orderInfo.IsInvoice)
                {
                    ltlIsInvoice.Text = string.Format(@"<a href=""{0}""><i class=""icon-plus""></i></a>", BackgroundInvoiceAdd.GetAddUrlToSiteYun(orderInfo.ID, this.PageUrl));
                }
                else
                {
                    ltlIsInvoice.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundInvoiceAdd.GetEditUrlToSiteYun(orderInfo.ID, this.PageUrl), StringUtils.GetTrueImageHtml(true));
                }

                if (!orderInfo.IsRefund)
                {
                    ltlIsRefund.Text = string.Format(@"<a href=""{0}""><i class=""icon-plus""></i></a>", BackgroundOrderRefundAdd.GetRedirectUrl(orderInfo.ID, this.PageUrl));
                }
                else
                {
                    ltlIsRefund.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundOrderRefundAdd.GetRedirectUrl(orderInfo.ID, this.PageUrl), StringUtils.GetTrueImageHtml(true));
                }

                ltlStatus.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"" class=""btn {1}"">{2}</a>", Modal.OrderSetting.GetShowPopWinString(EOrderType.Aliyun_Software, orderInfo.ID), EOrderStatusUtils.GetClass(orderInfo.Status), EOrderStatusUtils.GetText(orderInfo.Status));

                ltlEditUrl.Text = string.Format(@"<a href=""{0}""><i class=""icon-edit""></i></a>", BackgroundOrderSoftwareAdd.GetEditUrl(orderInfo.ID, this.PageUrl));
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
                return DataProvider.OrderDAO.GetSelectString(EOrderType.Aliyun_Software, base.Request.QueryString["type"], base.Request.QueryString["loginName"], base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.OrderDAO.GetSelectString(EOrderType.Aliyun_Software);
            }
        }

        private string _pageUrl;
        protected string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = BackgroundOrderSoftware.GetRedirectUrl(this.ddlType.SelectedValue, this.tbLoginName.Text, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(string type, string loginName, string keyword, int page)
        {
            return string.Format("background_orderSoftware.aspx?type={0}&loginName={1}&keyword={2}&page={3}", type, loginName, keyword, page);
        }
    }
}
