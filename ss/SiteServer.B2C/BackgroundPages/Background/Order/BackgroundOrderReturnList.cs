using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;

using SiteServer.CMS.BackgroundPages;
using SiteServer.B2C.Model.Enumerations.ReturnOrder;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundOrderReturnList : BackgroundBasePage
    {
        public HyperLink hlSetting;

        public DropDownList ddlReturnType;
        public DropDownList ddlAuditStatus;
        public DropDownList ddlReturnOrderStatus;
        public DropDownList ddlReturnMoneyStatus;

        public TextBox tbReturnSN;
        public TextBox tbContact;
        public TextBox tbKeyword;

        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlWCLCount;
        public Literal ltlCLZCount;
        public Literal ltlYCLCount;

        public HyperLink hlAuditStatusPass;
        public HyperLink hlAuditStatusUnPass;

        private Hashtable typeHashtable = new Hashtable();

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;



            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProviderB2C.OrderDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {

                base.BreadCrumb(AppManager.B2C.LeftMenu.ID_Order, "返修单列表", string.Empty);

                ListItem listItem = new ListItem("全部", string.Empty);
                this.ddlReturnType.Items.Add(listItem);
                EOrderReturnTypeUtils.AddListItems(this.ddlReturnType);
                if (!string.IsNullOrEmpty(base.GetQueryString("returnType")))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlReturnType, base.GetQueryString("returnType"));
                }

                listItem = new ListItem("全部", string.Empty);
                this.ddlAuditStatus.Items.Add(listItem);
                EAuditStatusUtils.AddListItems(this.ddlAuditStatus);
                if (!string.IsNullOrEmpty(base.GetQueryString("auditStatus")))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlAuditStatus, base.GetQueryString("auditStatus"));
                }

                listItem = new ListItem("全部", string.Empty);
                this.ddlReturnOrderStatus.Items.Add(listItem);
                EReturnOrderStatusUtils.AddListItems(this.ddlReturnOrderStatus);
                if (!string.IsNullOrEmpty(base.GetQueryString("returnOrderStatus")))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlReturnOrderStatus, base.GetQueryString("returnOrderStatus"));
                }

                listItem = new ListItem("全部", string.Empty);
                this.ddlReturnMoneyStatus.Items.Add(listItem);
                EReturnOrderStatusUtils.AddListItems(this.ddlReturnMoneyStatus);
                if (!string.IsNullOrEmpty(base.GetQueryString("returnMoneyStatus")))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlReturnMoneyStatus, base.GetQueryString("returnMoneyStatus"));
                }

                this.tbReturnSN.Text = base.GetQueryString("returnSN");
                this.tbContact.Text = base.GetQueryString("contact");
                this.tbKeyword.Text = base.GetQueryString("keyword");


                ltlWCLCount.Text = DataProviderB2C.OrderItemReturnDAO.GetCount(string.Format("b2c_OrderItemReturn.PublishmentSystemID={0} and Status='{1}'", PublishmentSystemID, EStatusUtils.GetValue(EStatus.ToDo))).ToString();
                ltlCLZCount.Text = DataProviderB2C.OrderItemReturnDAO.GetCount(string.Format("b2c_OrderItemReturn.PublishmentSystemID={0} and Status='{1}'", PublishmentSystemID, EStatusUtils.GetValue(EStatus.Doing))).ToString();
                ltlYCLCount.Text = DataProviderB2C.OrderItemReturnDAO.GetCount(string.Format("b2c_OrderItemReturn.PublishmentSystemID={0} and Status='{1}'", PublishmentSystemID, EStatusUtils.GetValue(EStatus.Done))).ToString();
                if (base.GetQueryString("Pass") != null)
                {

                    List<int> idList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("IDCollection"));
                    if (base.GetQueryString("Pass") == "True")
                    {
                        try
                        {
                            DataProviderB2C.OrderItemReturnDAO.UpdateStatus(idList, new OrderItemReturnStatus()
                            {
                                AuditDate = DateTime.Now,
                                AuditUser = BaiRongDataProvider.AdministratorDAO.UserName,
                                AuditStatus = EAuditStatusUtils.GetValue(EAuditStatus.Pass),
                                Status = EStatusUtils.GetValue(EStatus.Doing)
                            });

                            base.SuccessMessage("已成功受理返修单！");
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "受理失败！");
                        }
                    }
                    else
                    {
                        try
                        {
                            DataProviderB2C.OrderItemReturnDAO.UpdateStatus(idList, new OrderItemReturnStatus()
                            {
                                AuditDate = DateTime.Now,
                                AuditUser = BaiRongDataProvider.AdministratorDAO.UserName,
                                AuditStatus = EAuditStatusUtils.GetValue(EAuditStatus.UnPass),
                                Status = EStatusUtils.GetValue(EStatus.Done)
                            });

                            base.SuccessMessage("已成功废弃返修单！");
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "废弃失败！");
                        }
                    }
                    base.AddWaitAndRedirectScript(PageUrl);
                }
                else
                {

                    this.spContents.DataBind();
                }

                this.hlAuditStatusPass.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&Pass=True", PageUrl), "IDCollection", "IDCollection", "请选择需要受理的返修单！", "此操作将开启返修单处理流程，确认吗？"));
                this.hlAuditStatusUnPass.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&Pass=False", PageUrl), "IDCollection", "IDCollection", "请选择需要废弃的返修单！", "此操作将使返修单作废，确认吗？"));


            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderItemReturnInfo oirInfo = new OrderItemReturnInfo(e.Item.DataItem);
                if (oirInfo != null)
                {
                    OrderItemInfo orderItemInfo = DataProviderB2C.OrderItemDAO.GetItemInfo(oirInfo.OrderItemID);
                    if (orderItemInfo != null)
                    {

                        OrderInfo orderInfo = DataProviderB2C.OrderDAO.GetOrderInfo(orderItemInfo.OrderID);

                        Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                        Literal ltlOIRID = e.Item.FindControl("ltlOIRID") as Literal;
                        Literal ltlOrderSN = e.Item.FindControl("ltlOrderSN") as Literal;
                        Literal ltlType = e.Item.FindControl("ltlType") as Literal;
                        Literal ltlGoodsTitle = e.Item.FindControl("ltlGoodsTitle") as Literal;
                        Literal ltlPriceSale = e.Item.FindControl("ltlPriceSale") as Literal;
                        Literal ltlReturnCount = e.Item.FindControl("ltlReturnCount") as Literal;
                        Literal ltlApplyUser = e.Item.FindControl("ltlApplyUser") as Literal;
                        Literal ltlContact = e.Item.FindControl("ltlContact") as Literal;
                        Literal ltlContactPhone = e.Item.FindControl("ltlContactPhone") as Literal;
                        Literal ltlApplyDate = e.Item.FindControl("ltlApplyDate") as Literal;
                        Literal ltlAuditStatus = e.Item.FindControl("ltlAuditStatus") as Literal;
                        Literal ltlBtnOperate = e.Item.FindControl("ltlBtnOperate") as Literal;
                        Literal ltlGoodsSN = e.Item.FindControl("ltlGoodsSN") as Literal;
                        Literal ltlReturnMoney = e.Item.FindControl("ltlReturnMoney") as Literal;
                        Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                        

                        ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                        ltlOIRID.Text = oirInfo.ID.ToString();
                        ltlOrderSN.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", BackgroundOrderDetail.GetRedirectUrl(base.PublishmentSystemID, orderInfo.ID), orderInfo.OrderSN);
                        ltlType.Text = EOrderReturnTypeUtils.GetText(oirInfo.Type);

                        ltlGoodsTitle.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtility.GetContentUrl(base.PublishmentSystemInfo, NodeManager.GetNodeInfo(base.PublishmentSystemID, orderItemInfo.ChannelID), orderItemInfo.ContentID, base.PublishmentSystemInfo.Additional.VisualType), orderItemInfo.Title);
                        ltlPriceSale.Text = orderItemInfo.PriceSale.ToString("c");
                        ltlReturnCount.Text = oirInfo.ReturnCount.ToString() + "/" + orderItemInfo.PurchaseNum.ToString();
                        ltlApplyUser.Text = oirInfo.ApplyUser;
                        ltlContact.Text = oirInfo.Contact;
                        ltlContactPhone.Text = oirInfo.ContactPhone;
                        ltlApplyDate.Text = oirInfo.ApplyDate.ToString("yyyy-MM-dd");
                        ltlAuditStatus.Text = EAuditStatusUtils.GetText(oirInfo.AuditStatus);
                        //ltlBtnOperate.Text = string.Format("<a href=\"{0}\" >");
                        ltlGoodsSN.Text = oirInfo.GoodsSN;
                        ltlReturnMoney.Text = (oirInfo.ReturnCount * orderItemInfo.PriceSale).ToString();
                        ltlDescription.Text = oirInfo.Description;
                    }
                }
            }
        }



        protected string GetSelectString()
        {
            if (!string.IsNullOrEmpty(base.GetQueryString("returnType"))
                || !string.IsNullOrEmpty(base.GetQueryString("auditStatus"))
                || !string.IsNullOrEmpty(base.GetQueryString("returnOrderStatus"))
                || !string.IsNullOrEmpty(base.GetQueryString("returnMoneyStatus"))
                || !string.IsNullOrEmpty(base.GetQueryString("returnSN"))
                || !string.IsNullOrEmpty(base.GetQueryString("contact"))
                || !string.IsNullOrEmpty(base.GetQueryString("keyword")))
            {
                return DataProviderB2C.OrderItemReturnDAO.GetSelectString(base.PublishmentSystemID, EStatusUtils.GetValue(EStatus.ToDo),
                   base.GetQueryString("returnType"),
                   base.GetQueryString("auditStatus"),
                   base.GetQueryString("returnOrderStatus"),
                   base.GetQueryString("returnMoneyStatus"),
                   base.GetQueryString("returnSN"),
                   base.GetQueryString("contact"),
                   base.GetQueryString("keyword"));
            }
            else
            {
                return DataProviderB2C.OrderItemReturnDAO.GetSelectString(base.PublishmentSystemID, EStatusUtils.GetValue(EStatus.ToDo));
            }
        }

        private string _pageUrl;
        protected string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = BackgroundOrderReturnList.GetRedirectUrl(base.PublishmentSystemID, this.ddlReturnType.SelectedValue, this.ddlAuditStatus.SelectedValue,
                        this.ddlReturnOrderStatus.SelectedValue, this.ddlReturnMoneyStatus.SelectedValue, this.tbReturnSN.Text, this.tbContact.Text, this.tbKeyword.Text, TranslateUtils.ToInt(base.GetQueryString("page"), 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, string status)
        {
            return PageUtils.GetB2CUrl(string.Format("background_orderReturnList.aspx?PublishmentSystemID={0}", publishmentSystemID));
        }


        private static string GetRedirectUrl(int publishmentSystemID, string returnType, string auditStatus, string returnOrderStatus, string returnMoneyStatus, string returnSN, string contact, string keyword, int page)

        {
            return PageUtils.GetB2CUrl(string.Format("background_orderReturnList.aspx?PublishmentSystemID={0}&returnType={1}&auditStatus={2}&returnOrderStatus={3}&returnMoneyStatus={4}&returnSN={5}&contact={6}&keyword={7}&page={8}", publishmentSystemID, returnType, auditStatus, returnOrderStatus, returnMoneyStatus, returnSN, contact, keyword, page));
        }
    }
}
