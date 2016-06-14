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
    public class BackgroundOrderRefund : BackgroundBasePage
    {
        public DropDownList ddlTaxis;
        public TextBox tbKeyword;
        public Literal ltlTotalCount;

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
                        ArrayList orderIDArrayList = DataProvider.OrderRefundDAO.GetOrderIDArrayList(arraylist);
                        DataProvider.OrderDAO.UpdateIsRefund(orderIDArrayList, false);
                        DataProvider.OrderRefundDAO.Delete(arraylist);
                        base.SuccessMessage("删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProvider.OrderRefundDAO.GetSortFieldName();
            this.spContents.SortMode = this.GetSortMode();
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                bool isTaxisDESC = true;
                EBooleanUtils.AddListItems(this.ddlTaxis, "倒序", "正序");
                if (!string.IsNullOrEmpty(base.Request.QueryString["isTaxisDESC"]))
                {
                    isTaxisDESC = TranslateUtils.ToBool(base.Request.QueryString["isTaxisDESC"]);
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlTaxis, isTaxisDESC.ToString());
                }

                this.tbKeyword.Text = base.Request.QueryString["keyword"];

                this.spContents.DataBind();

                this.hlSetting.Attributes.Add("onclick", Modal.OrderRefundSetting.GetShowPopWinString());

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的退款！", "此操作将删除所选退款，确定吗？"));

                this.ltlTotalCount.Text = this.spContents.TotalCount.ToString();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderRefundInfo refundInfo = new OrderRefundInfo(e.Item.DataItem);

                Literal ltlOrderSN = e.Item.FindControl("ltlOrderSN") as Literal;
                Literal ltlLoginName = e.Item.FindControl("ltlLoginName") as Literal;
                Literal ltlAmount = e.Item.FindControl("ltlAmount") as Literal;
                Literal ltlIsAliyunRefund = e.Item.FindControl("ltlIsAliyunRefund") as Literal;
                Literal ltlAccountRealName = e.Item.FindControl("ltlAccountRealName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlIsRefund = e.Item.FindControl("ltlIsRefund") as Literal;
                Literal ltlRefundDate = e.Item.FindControl("ltlRefundDate") as Literal;
                Literal ltlIsConfirm = e.Item.FindControl("ltlIsConfirm") as Literal;
                Literal ltlConfirmDate = e.Item.FindControl("ltlConfirmDate") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlOrderSN.Text = refundInfo.OrderSN;
                ltlLoginName.Text = refundInfo.LoginName;
                ltlAmount.Text = refundInfo.Amount.ToString("c");
                ltlIsAliyunRefund.Text = refundInfo.IsAliyunRefund ? "通过阿里云退款" : "直接退款";
                if (refundInfo.IsAliyunRefund)
                {
                    if (!string.IsNullOrEmpty(refundInfo.AliyunFileUrl))
                    {
                        ltlIsAliyunRefund.Text = string.Format(@"<a href=""{0}"" target=""_blank"">通过阿里云退款</a>", PageUtils.ParseNavigationUrl(refundInfo.AliyunFileUrl));
                    }
                    else
                    {
                        ltlIsAliyunRefund.Text = string.Format(@"通过阿里云退款<br /><code>未添加阿里云退款申请单</code>");
                    }
                }
                else
                {
                    ltlIsAliyunRefund.Text = "直接退款";
                }
                ltlAccountRealName.Text = refundInfo.AccountRealName;
                ltlAddDate.Text = DateUtils.GetDateString(refundInfo.AddDate);

                if (refundInfo.IsRefund)
                {
                    ltlIsRefund.Text = StringUtils.GetTrueImageHtml(true);
                    ltlRefundDate.Text = DateUtils.GetDateString(refundInfo.RefundDate);
                }
                if (refundInfo.IsConfirm)
                {
                    ltlIsConfirm.Text = StringUtils.GetTrueImageHtml(refundInfo.IsConfirm);
                    ltlConfirmDate.Text = DateUtils.GetDateString(refundInfo.ConfirmDate);
                }

                ltlEditUrl.Text = string.Format(@"<a href=""{0}""><i class=""icon-edit""></i></a>", BackgroundOrderRefundAdd.GetRedirectUrl(refundInfo.OrderID, this.PageUrl));
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        protected string GetSelectString()
        {
            if (base.Request.QueryString["keyword"] != null)
            {
                return DataProvider.OrderRefundDAO.GetSelectString(base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.OrderRefundDAO.GetSelectString();
            }
        }

        protected SortMode GetSortMode()
        {
            bool isTaxisDESC = true;
            if (!string.IsNullOrEmpty(base.Request.QueryString["isTaxisDESC"]))
            {
                isTaxisDESC = TranslateUtils.ToBool(base.Request.QueryString["isTaxisDESC"]);
            }
            return isTaxisDESC ? SortMode.DESC : SortMode.ASC;
        }

        private string _pageUrl;
        protected string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = BackgroundOrderRefund.GetRedirectUrl(TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(bool isTaxisDESC, string keyword, int page)
        {
            return string.Format("background_orderRefund.aspx?isTaxisDESC={0}&keyword={1}&page={2}", isTaxisDESC, keyword, page);
        }
    }
}
