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

using System.Collections.Generic;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundOrder : BackgroundBasePage
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
        private List<int> orderIDList = new List<int>();

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

                Dictionary<int, int> touchCounts = DataProvider.TouchDAO.GetCountByOrderIDList(this.orderIDList);

                foreach (RepeaterItem item in this.rptContents.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        int orderID = this.orderIDList[item.ItemIndex];
                        Literal ltlTouch = item.FindControl("ltlTouch") as Literal;

                        int count = 0;
                        touchCounts.TryGetValue(orderID, out count);
                        string countHtml = string.Empty;
                        if (count > 0){
                            countHtml = string.Format("<code>{0}</code>", count);
                        }
                        ltlTouch.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">联系{1}</a>", Modal.Touch.GetShowPopWinString(0, orderID), countHtml);
                    }
                }

                this.hlAdd.NavigateUrl = BackgroundOrderAdd.GetAddUrl(this.PageUrl);

                this.hlSetting.Attributes.Add("onclick", Modal.OrderSetting.GetShowPopWinString(EOrderType.Aliyun_Moban));

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的订单！", "此操作将删除所选订单，确定吗？"));

                this.ltlTotalCount.Text = this.spContents.TotalCount.ToString();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderInfo orderInfo = new OrderInfo(e.Item.DataItem);
                this.orderIDList.Add(orderInfo.ID);

                Literal ltlLoginName = e.Item.FindControl("ltlLoginName") as Literal;
                Literal ltlDomainTemp = e.Item.FindControl("ltlDomainTemp") as Literal;
                Literal ltlMobanID = e.Item.FindControl("ltlMobanID") as Literal;
                Literal ltlBackgroundPassword = e.Item.FindControl("ltlBackgroundPassword") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlLicense = e.Item.FindControl("ltlLicense") as Literal;
                Literal ltlIsContract = e.Item.FindControl("ltlIsContract") as Literal;
                Literal ltlIsInvoice = e.Item.FindControl("ltlIsInvoice") as Literal;
                Literal ltlIsRefund = e.Item.FindControl("ltlIsRefund") as Literal;
                Literal ltlTouch = e.Item.FindControl("ltlTouch") as Literal;
                Literal ltlSendMessage = e.Item.FindControl("ltlSendMessage") as Literal;
                Literal ltlForm = e.Item.FindControl("ltlForm") as Literal;
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlLoginName.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a><br /><code>{2}</code>", Modal.OrderView.GetShowPopWinString(orderInfo.ID), orderInfo.LoginName, orderInfo.BizType);
                if (orderInfo.IsReNew)
                {
                    ltlLoginName.Text += string.Format(@"<code>续费{0}年</code>", orderInfo.Duration);
                }

                bool isInitializationForm = DataProvider.MobanDAO.IsInitializationForm(orderInfo.MobanID);
                if (isInitializationForm)
                {
                    int orderFormID = 0;
                    bool isCompleted = DataProvider.OrderFormDAO.IsCompleted(orderInfo.ID, out orderFormID);
                    if (isCompleted)
                    {
                        if (orderInfo.Status == EOrderStatus.New || orderInfo.Status == EOrderStatus.Messaged || orderInfo.Status == EOrderStatus.Formed)
                        {
                            ltlForm.Text = string.Format(@"<a href=""javascript:;"" class=""btn btn-danger"" onclick=""{0}"">已提交，待开通</a>&nbsp;&nbsp;<a href=""{1}"" onclick=""javascript:return confirm('此操作将删除已上传的表单，确认吗？');""><i class=""icon-remove""></i></a>", Modal.OrderFormView.GetShowPopWinString(orderFormID), string.Format("background_order.aspx?deleteOrderForm=True&orderID={0}", orderInfo.ID));
                        }
                        else
                        {
                            ltlForm.Text = string.Format(@"<a href=""javascript:;"" class=""btn btn-success"" onclick=""{0}"">已提交</a>&nbsp;&nbsp;<a href=""{1}"" onclick=""javascript:return confirm('此操作将删除已上传的表单，确认吗？');""><i class=""icon-remove""></i></a>", Modal.OrderFormView.GetShowPopWinString(orderFormID), string.Format("background_order.aspx?deleteOrderForm=True&orderID={0}", orderInfo.ID));
                        }
                    }
                    else
                    {
                        if (orderFormID > 0)
                        {
                            ltlForm.Text = string.Format(@"<a href=""javascript:;"" class=""btn btn-info"" onclick=""{0}"">已提交，未完成</a>&nbsp;&nbsp;<a href=""{1}"" onclick=""javascript:return confirm('此操作将删除已上传的表单，确认吗？');""><i class=""icon-remove""></i></a>", Modal.OrderFormView.GetShowPopWinString(orderFormID), string.Format("background_order.aspx?deleteOrderForm=True&orderID={0}", orderInfo.ID));
                        }
                        else
                        {
                            ltlForm.Text = string.Format(@"<a href=""{0}"" class=""btn btn-warning"" target=""_blank"">未提交</a>", RequestForm.GetRedirectUrl(orderInfo.DomainTemp));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(orderInfo.DomainTemp))
                {
                    ltlDomainTemp.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a><br /><a href=""{0}/siteserver"" target=""_blank"">后台</a>", PageUtils.AddProtocolToUrl(orderInfo.DomainTemp), orderInfo.DomainTemp);
                }
                
                if (!string.IsNullOrEmpty(orderInfo.MobanID))
                {
                    MobanInfo mobanInfo = DataProvider.MobanDAO.GetMobanInfo(orderInfo.MobanID);
                    if (mobanInfo != null)
                    {
                        ltlMobanID.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a><code>{2}</code>", DataProvider.MobanDAO.GetMobanUrl(mobanInfo), mobanInfo.SN, DataProvider.OrderDAO.GetMobanUsedCount(mobanInfo.SN).ToString());
                    }
                }
                ltlBackgroundPassword.Text = orderInfo.BackgroundPassword;
                ltlAddDate.Text = DateUtils.GetDateString(orderInfo.AddDate);

                if (!string.IsNullOrEmpty(orderInfo.DomainFormal))
                {
                    if (!orderInfo.IsLicense)
                    {
                        ltlLicense.Text = string.Format(@"<a href=""{0}""><i class=""icon-plus""></i></a>", BackgroundLicenseAdd.GetAddUrlToSiteYun(orderInfo.ID, this.PageUrl));
                    }
                    else
                    {
                        ltlLicense.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundLicenseAdd.GetAddUrlToSiteYun(orderInfo.ID, this.PageUrl), StringUtils.GetTrueImageHtml(true));
                    }
                }

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

                if (!orderInfo.IsReNew)
                {
                    ltlStatus.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"" class=""btn {1}"">{2}</a>", Modal.OrderSetting.GetShowPopWinString(EOrderType.Aliyun_Moban, orderInfo.ID), EOrderStatusUtils.GetClass(orderInfo.Status), EOrderStatusUtils.GetText(orderInfo.Status));
                }                

                ltlEditUrl.Text = string.Format(@"<a href=""{0}""><i class=""icon-edit""></i></a>", BackgroundOrderAdd.GetEditUrl(orderInfo.ID, this.PageUrl));

                if (!string.IsNullOrEmpty(orderInfo.Mobile) && !string.IsNullOrEmpty(orderInfo.Email))
                {
                    ltlSendMessage.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}""><i class=""icon-heart""></i></a>&nbsp;", Modal.Message.GetShowPopWinString(orderInfo.ID));
                    ltlSendMessage.Text += string.Format(@"<a href=""javascript:;"" onclick=""{0}""><i class=""icon-bell""></i></a>&nbsp;", Modal.SendMessage.GetShowPopWinStringBySMS(orderInfo.ID));
                    ltlSendMessage.Text += string.Format(@"<a href=""javascript:;"" onclick=""{0}""><i class=""icon-envelope""></i></a>", Modal.SendMessage.GetShowPopWinStringByEmail(orderInfo.ID));
                }
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
                    _pageUrl = BackgroundOrder.GetRedirectUrl(this.ddlType.SelectedValue, this.tbLoginName.Text, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(string type, string loginName, string keyword, int page)
        {
            return string.Format("background_order.aspx?type={0}&loginName={1}&keyword={2}&page={3}", type, loginName, keyword, page);
        }
    }
}
