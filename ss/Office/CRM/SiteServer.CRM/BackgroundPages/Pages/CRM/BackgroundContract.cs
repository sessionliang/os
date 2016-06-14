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
    public class BackgroundContract : BackgroundBasePage
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
                List<int> deleteIDList = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (deleteIDList.Count > 0)
                {
                    try
                    {
                        List<int> orderIDList = DataProvider.ContractDAO.GetOrderIDList(deleteIDList);
                        DataProvider.OrderDAO.UpdateIsContract(orderIDList, false);
                        DataProvider.ContractDAO.Delete(deleteIDList);
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
            this.spContents.SortField = DataProvider.ContractDAO.GetSortFieldName();
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

                this.hlSetting.Attributes.Add("onclick", Modal.ContractSetting.GetShowPopWinString());

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的发票！", "此操作将删除所选发票，确定吗？"));

                this.ltlTotalCount.Text = this.spContents.TotalCount.ToString();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ContractInfo contractInfo = new ContractInfo(e.Item.DataItem);

                Literal ltlTr = e.Item.FindControl("ltlTr") as Literal;
                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                Literal ltlContractType = e.Item.FindControl("ltlContractType") as Literal;
                Literal ltlChargeUserName = e.Item.FindControl("ltlChargeUserName") as Literal;
                Literal ltlAmount = e.Item.FindControl("ltlAmount") as Literal;
                Literal ltlContractTitle = e.Item.FindControl("ltlContractTitle") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlIsContract = e.Item.FindControl("ltlIsContract") as Literal;
                Literal ltlContractDate = e.Item.FindControl("ltlContractDate") as Literal;
                Literal ltlIsConfirm = e.Item.FindControl("ltlIsConfirm") as Literal;
                Literal ltlConfirmDate = e.Item.FindControl("ltlConfirmDate") as Literal;
                Literal ltlDocumentUrl = e.Item.FindControl("ltlDocumentUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlTr.Text = "<tr>";

                //if (contractInfo.ContractType == EContractType.SiteYun_Order)
                //{
                //    ltlSN.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", Modal.OrderView.GetShowPopWinString(contractInfo.OrderID), contractInfo.SN);
                //}
                //else
                //{
                    ltlSN.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", Modal.AccountView.GetShowPopWinString(contractInfo.AccountID), contractInfo.SN);
                //}
                
                ltlContractType.Text = EContractTypeUtils.GetText(EContractTypeUtils.GetEnumType(contractInfo.ContractType));
                ltlChargeUserName.Text = AdminManager.GetDisplayName(contractInfo.ChargeUserName, false);
                ltlAmount.Text = contractInfo.Amount.ToString("c").Replace(".00", string.Empty);
                ltlContractTitle.Text = contractInfo.ContractTitle;
                ltlAddDate.Text = DateUtils.GetDateString(contractInfo.AddDate);

                if (contractInfo.IsContract)
                {
                    ltlIsContract.Text = StringUtils.GetTrueImageHtml(true);
                    ltlContractDate.Text = DateUtils.GetDateString(contractInfo.ContractDate);

                    ltlTr.Text = @"<tr class=""warning"">";
                }
                if (contractInfo.IsConfirm)
                {
                    ltlIsConfirm.Text = StringUtils.GetTrueImageHtml(contractInfo.IsConfirm);
                    ltlConfirmDate.Text = DateUtils.GetDateString(contractInfo.ConfirmDate);

                    ltlTr.Text = @"<tr class=""info"">";
                }

                int count = DataProvider.DocumentDAO.GetCountByContract(contractInfo.ID);
                if (count == 0)
                {
                    ltlDocumentUrl.Text = string.Format(@"<a href=""{0}"">文档列表</a>", BackgroundDocument.GetRedirectUrlByContract(contractInfo.ID, this.PageUrl));

                    ltlTr.Text = @"<tr class=""error"" title=""请添加合同电子档"">";
                }
                else
                {
                    ltlDocumentUrl.Text = string.Format(@"<a href=""{0}"">文档列表({1})</a>", BackgroundDocument.GetRedirectUrlByContract(contractInfo.ID, this.PageUrl), count);

                    if (contractInfo.IsContract && contractInfo.IsConfirm)
                    {
                        ltlTr.Text = @"<tr class=""success"">";
                    }
                }

                ltlEditUrl.Text = string.Format(@"<a href=""{0}""><i class=""icon-edit""></i></a>", BackgroundContractAdd.GetEditUrl(contractInfo.ID, this.PageUrl));
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
                return DataProvider.ContractDAO.GetSelectString(base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.ContractDAO.GetSelectString();
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
                    _pageUrl = BackgroundContract.GetRedirectUrl(TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(bool isTaxisDESC, string keyword, int page)
        {
            return string.Format("background_contract.aspx?isTaxisDESC={0}&keyword={1}&page={2}", isTaxisDESC, keyword, page);
        }
    }
}
