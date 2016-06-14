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
    public class BackgroundLeadAll : BackgroundBasePage
    {
        public DropDownList ddlTaxis;
        public DropDownList ddlStatus;
        public DropDownList ddlUserName;
        public TextBox tbKeyword;

        public HyperLink hlAdd;
        public HyperLink hlSetting;
        public HyperLink hlDelete;

        public Repeater rptContents;
        public SqlPager spContents;

        private Hashtable typeHashtable = new Hashtable();
        private List<int> leadIDList = new List<int>();

        public void Page_Load(object sender, EventArgs E)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.QueryString["IDCollection"]);
                if (arraylist.Count > 0)
                {
                    try
                    {
                        DataProvider.LeadDAO.Delete(arraylist);
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
            this.spContents.SortField = DataProvider.LeadDAO.GetSortFieldName();
            this.spContents.SortMode = this.GetSortMode();
            this.spContents.OrderByString = DataProvider.LeadDAO.GetOrderByString();
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
                this.ddlStatus.Items.Add(listItem);
                ELeadStatusUtils.AddListItems(this.ddlStatus);
                if (!string.IsNullOrEmpty(base.Request.QueryString["status"]))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlStatus, base.Request.QueryString["status"]);
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

                Dictionary<int, int> touchCounts = DataProvider.TouchDAO.GetCountByLeadIDList(this.leadIDList);

                foreach (RepeaterItem item in this.rptContents.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        int leadID = this.leadIDList[item.ItemIndex];
                        Literal ltlTouch = item.FindControl("ltlTouch") as Literal;

                        int count = 0;
                        touchCounts.TryGetValue(leadID, out count);
                        string countHtml = string.Empty;
                        if (count > 0)
                        {
                            countHtml = string.Format("<code>{0}</code>", count);
                        }
                        ltlTouch.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">联系{1}</a>", Modal.Touch.GetShowPopWinString(leadID, 0), countHtml);
                    }
                }

                this.hlAdd.NavigateUrl = BackgroundLeadAdd.GetAddUrl(this.PageUrl);

                this.hlSetting.Attributes.Add("onclick", Modal.LeadSetting.GetShowPopWinString());

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的线索！", "此操作将删除所选线索，确定吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LeadInfo leadInfo = new LeadInfo(e.Item.DataItem);
                this.leadIDList.Add(leadInfo.ID);

                Literal ltlTR = e.Item.FindControl("ltlTR") as Literal;
                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlSubject = e.Item.FindControl("ltlSubject") as Literal;
                Literal ltlPossibility = e.Item.FindControl("ltlPossibility") as Literal;
                Literal ltlContactName = e.Item.FindControl("ltlContactName") as Literal;
                Literal ltlAccountName = e.Item.FindControl("ltlAccountName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlTelephone = e.Item.FindControl("ltlTelephone") as Literal;
                Literal ltlChargeUserName = e.Item.FindControl("ltlChargeUserName") as Literal;
                Literal ltlSource = e.Item.FindControl("ltlSource") as Literal;
                Literal ltlTouch = e.Item.FindControl("ltlTouch") as Literal;
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlTR.Text = "<tr>";
                string priority = string.Empty;
                if (leadInfo.Priority == 2)
                {
                    ltlTR.Text = @"<tr class=""warning"">";
                    priority = @"<span style=""color:red"">（优先）</span>";
                }
                else if (leadInfo.Priority == 3)
                {
                    ltlTR.Text = @"<tr class=""error"">";
                    priority = @"<span style=""color:red"">（紧急重要）</span>";
                }

                ltlID.Text = leadInfo.ID.ToString();

                ltlSubject.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}{2}</a>", BackgroundApplyToReplyDetail.GetRedirectUrl(leadInfo.ID), leadInfo.Subject, priority);
                ltlPossibility.Text = (leadInfo.Possibility > 0) ? leadInfo.Possibility.ToString() + "%" : string.Empty;
                ltlContactName.Text = leadInfo.ContactName;
                ltlAccountName.Text = leadInfo.AccountName;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(leadInfo.AddDate);
                ltlTelephone.Text = leadInfo.Telephone + "," + leadInfo.Mobile;
                ltlTelephone.Text = ltlTelephone.Text.Trim(',');

                ltlSource.Text = leadInfo.Source;

                ltlTouch.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">联系</a>", Modal.Touch.GetShowPopWinString(leadInfo.ID, 0));

                ltlStatus.Text = ELeadStatusUtils.GetText(leadInfo.Status);
                if (leadInfo.Status == ELeadStatus.New)
                {
                    ltlStatus.Text = string.Format("<span style='color:red'>{0}</span>", ltlStatus.Text);
                }
                else if (leadInfo.Status == ELeadStatus.Success)
                {
                    //ltlStatus.Text = string.Format(@"<a style='color:red' href=""{0}"">关联/新增项目</a>", Modal.ProjectAdd.GetShowPopWinStringToAdd);
                }
                ltlChargeUserName.Text = AdminManager.GetDisplayName(leadInfo.ChargeUserName, false);

                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", BackgroundLeadAdd.GetEditUrl(leadInfo.ID, this.PageUrl));
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
                return DataProvider.LeadDAO.GetSelectString(base.Request.QueryString["userName"], base.Request.QueryString["status"], base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.LeadDAO.GetSelectString(string.Empty);
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
                    _pageUrl = BackgroundLeadAll.GetRedirectUrl(TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.ddlStatus.SelectedValue, this.ddlUserName.SelectedValue, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(bool isTaxisDESC, string status, string userName, string keyword, int page)
        {
            return string.Format("background_leadAll.aspx?isTaxisDESC={0}&status={1}&userName={2}&keyword={3}&page={4}", isTaxisDESC, status, userName, keyword, page);
        }
    }
}
