using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CRM.Core;
using SiteServer.CRM.Model;
using System.Web.UI;

using SiteServer.CMS.BackgroundPages;
using System.Collections.Generic;

namespace SiteServer.CRM.BackgroundPages
{
    public class BackgroundInvoice : BackgroundBasePage
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
                List<int> idList = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (idList.Count > 0)
                {
                    try
                    {
                        List<int> orderIDList = DataProvider.InvoiceDAO.GetOrderIDList(idList);
                        DataProvider.OrderDAO.UpdateIsInvoice(orderIDList, false);
                        DataProvider.InvoiceDAO.Delete(idList);
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
            this.spContents.SortField = DataProvider.InvoiceDAO.GetSortFieldName();
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

                this.hlSetting.Attributes.Add("onclick", Modal.InvoiceSetting.GetShowPopWinString());

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的发票！", "此操作将删除所选发票，确定吗？"));

                this.ltlTotalCount.Text = this.spContents.TotalCount.ToString();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                InvoiceInfo invoiceInfo = new InvoiceInfo(e.Item.DataItem);

                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                Literal ltlInvoiceType = e.Item.FindControl("ltlInvoiceType") as Literal;
                Literal ltlAmount = e.Item.FindControl("ltlAmount") as Literal;
                Literal ltlIsVAT = e.Item.FindControl("ltlIsVAT") as Literal;
                Literal ltlInvoiceTitle = e.Item.FindControl("ltlInvoiceTitle") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlIsInvoice = e.Item.FindControl("ltlIsInvoice") as Literal;
                Literal ltlInvoiceDate = e.Item.FindControl("ltlInvoiceDate") as Literal;
                Literal ltlIsConfirm = e.Item.FindControl("ltlIsConfirm") as Literal;
                Literal ltlConfirmDate = e.Item.FindControl("ltlConfirmDate") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                //if (invoiceInfo.InvoiceType == EInvoiceType.SiteYun)
                //{
                //    ltlSN.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", Modal.OrderView.GetShowPopWinString(invoiceInfo.OrderID), invoiceInfo.SN);
                //}
                //else if (invoiceInfo.InvoiceType == EInvoiceType.SiteServer)
                //{
                    ltlSN.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", Modal.AccountView.GetShowPopWinString(invoiceInfo.AccountID), invoiceInfo.SN);
                //}
                
                ltlInvoiceType.Text = EInvoiceTypeUtils.GetText(EInvoiceTypeUtils.GetEnumType(invoiceInfo.InvoiceType));
                ltlAmount.Text = invoiceInfo.Amount.ToString("c");
                ltlIsVAT.Text = invoiceInfo.IsVAT ? "增值税发票" : "普通发票";
                ltlInvoiceTitle.Text = invoiceInfo.InvoiceTitle;
                ltlAddDate.Text = DateUtils.GetDateString(invoiceInfo.AddDate);

                if (invoiceInfo.IsInvoice)
                {
                    ltlIsInvoice.Text = StringUtils.GetTrueImageHtml(true);
                    ltlInvoiceDate.Text = DateUtils.GetDateString(invoiceInfo.InvoiceDate);
                }
                if (invoiceInfo.IsConfirm)
                {
                    ltlIsConfirm.Text = StringUtils.GetTrueImageHtml(invoiceInfo.IsConfirm);
                    ltlConfirmDate.Text = DateUtils.GetDateString(invoiceInfo.ConfirmDate);
                }

                ltlEditUrl.Text = string.Format(@"<a href=""{0}""><i class=""icon-edit""></i></a>", BackgroundInvoiceAdd.GetEditUrl(invoiceInfo.ID, this.PageUrl));
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
                return DataProvider.InvoiceDAO.GetSelectString(base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.InvoiceDAO.GetSelectString();
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
                    _pageUrl = BackgroundInvoice.GetRedirectUrl(TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(bool isTaxisDESC, string keyword, int page)
        {
            return string.Format("background_invoice.aspx?isTaxisDESC={0}&keyword={1}&page={2}", isTaxisDESC, keyword, page);
        }
    }
}
