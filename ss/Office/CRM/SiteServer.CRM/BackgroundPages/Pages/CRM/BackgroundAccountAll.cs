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
    public class BackgroundAccountAll : BackgroundBasePage
    {
        public DropDownList ddlTaxis;
        public DropDownList ddlAccountType;
        public DropDownList ddlUserName;
        public TextBox tbKeyword;

        public HyperLink hlAdd_SiteServer_Customer;
        public HyperLink hlAdd_SiteServer_Agent;
        public HyperLink hlAdd_SiteYun_Partner;
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
                        DataProvider.AccountDAO.Delete(deleteIDList);
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
            this.spContents.SortField = DataProvider.AccountDAO.GetSortFieldName();
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

                ListItem listItem = new ListItem("全部", string.Empty);
                this.ddlAccountType.Items.Add(listItem);
                EAccountTypeUtils.AddListItems(this.ddlAccountType);
                if (!string.IsNullOrEmpty(base.Request.QueryString["accountType"]))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlAccountType, base.Request.QueryString["accountType"]);
                }

                listItem = new ListItem("全部", string.Empty);
                this.ddlUserName.Items.Add(listItem);
                ArrayList userNameArrayList = BaiRongDataProvider.AdministratorDAO.GetUserNameArrayList();
                foreach (string userName in userNameArrayList)
                {
                    listItem = new ListItem(AdminManager.GetDisplayName(userName, true), userName);
                    this.ddlUserName.Items.Add(listItem);
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["userName"]))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlUserName, base.Request.QueryString["userName"]);
                }

                this.tbKeyword.Text = base.Request.QueryString["keyword"];

                this.spContents.DataBind();

                this.hlAdd_SiteServer_Customer.NavigateUrl = BackgroundAccountAdd.GetAddUrl(EAccountType.SiteServer_Customer, this.PageUrl);
                this.hlAdd_SiteServer_Customer.Text = "新增" + EAccountTypeUtils.GetText(EAccountType.SiteServer_Customer);
                this.hlAdd_SiteServer_Agent.NavigateUrl = BackgroundAccountAdd.GetAddUrl(EAccountType.SiteServer_Agent, this.PageUrl);
                this.hlAdd_SiteServer_Agent.Text = "新增" + EAccountTypeUtils.GetText(EAccountType.SiteServer_Agent);
                this.hlAdd_SiteYun_Partner.NavigateUrl = BackgroundAccountAdd.GetAddUrl(EAccountType.SiteYun_Partner, this.PageUrl);
                this.hlAdd_SiteYun_Partner.Text = "新增" + EAccountTypeUtils.GetText(EAccountType.SiteYun_Partner);

                this.hlSetting.Attributes.Add("onclick", Modal.AccountSetting.GetShowPopWinString());

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的客户！", "此操作将删除所选客户，确定吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                AccountInfo accountInfo = new AccountInfo(e.Item.DataItem);

                Literal ltlTR = e.Item.FindControl("ltlTR") as Literal;
                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                Literal ltlAccountName = e.Item.FindControl("ltlAccountName") as Literal;
                Literal ltlAccountType = e.Item.FindControl("ltlAccountType") as Literal;
                Literal ltlBusinessType = e.Item.FindControl("ltlBusinessType") as Literal;
                Literal ltlLocation = e.Item.FindControl("ltlLocation") as Literal;
                Literal ltlWebsite = e.Item.FindControl("ltlWebsite") as Literal;
                Literal ltlChargeUserName = e.Item.FindControl("ltlChargeUserName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;
                Literal ltlInvoice = e.Item.FindControl("ltlInvoice") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlTR.Text = "<tr>";
                string priority = string.Empty;
                if (accountInfo.Priority == 2)
                {
                    ltlTR.Text = @"<tr class=""warning"">";
                    priority = @"<span style=""color:red"">（高）</span>";
                }
                else if (accountInfo.Priority == 3)
                {
                    ltlTR.Text = @"<tr class=""error"">";
                    priority = @"<span style=""color:red"">（重点）</span>";
                }

                ltlSN.Text = accountInfo.SN;

                ltlAccountName.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}{2}</a>", Modal.AccountView.GetShowPopWinString(accountInfo.ID), accountInfo.AccountName, priority);
                ltlAccountType.Text = EAccountTypeUtils.GetText(EAccountTypeUtils.GetEnumType(accountInfo.AccountType));
                ltlChargeUserName.Text = AdminManager.GetDisplayName(accountInfo.ChargeUserName, true);
                ltlBusinessType.Text = accountInfo.BusinessType;
                ltlLocation.Text = accountInfo.Province + " " + accountInfo.City + " " + accountInfo.Area;
                ltlWebsite.Text = accountInfo.Website;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(accountInfo.AddDate);

                ltlStatus.Text = EAccountStatusUtils.GetText(EAccountStatusUtils.GetEnumType(accountInfo.Status));
                ltlInvoice.Text = string.Format(@"<a href=""{0}""><i class=""icon-plus""></i></a>", BackgroundInvoiceAdd.GetAddUrlToSiteServer(accountInfo.ID, this.PageUrl));
                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", BackgroundAccountAdd.GetEditUrl(accountInfo.ID, this.PageUrl));
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
                return DataProvider.AccountDAO.GetSelectString(base.Request.QueryString["userName"], base.Request.QueryString["accountType"], base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.AccountDAO.GetSelectString(string.Empty);
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
                    _pageUrl = BackgroundAccountAll.GetRedirectUrl(TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.ddlAccountType.SelectedValue, this.ddlUserName.SelectedValue, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(bool isTaxisDESC, string accountType, string userName, string keyword, int page)
        {
            return string.Format("background_accountAll.aspx?isTaxisDESC={0}&accountType={1}&userName={2}&keyword={3}&page={4}", isTaxisDESC, accountType, userName, keyword, page);
        }
    }
}
