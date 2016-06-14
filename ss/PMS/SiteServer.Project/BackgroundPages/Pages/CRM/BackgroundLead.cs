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
    public class BackgroundLead : BackgroundBasePage
    {
        public DropDownList ddlTaxis;
        public DropDownList ddlStatus;
        public TextBox tbKeyword;

        public HyperLink hlAdd;

        public Repeater rptContents;
        public SqlPager spContents;

        private Hashtable typeHashtable = new Hashtable();
        private List<int> leadIDList = new List<int>();

        public void Page_Load(object sender, EventArgs E)
        {
            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProvider.LeadDAO.GetSortFieldName();
            this.spContents.SortMode = this.GetSortMode();
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                bool isTaxisDESC = true;
                EBooleanUtils.AddListItems(this.ddlTaxis, "����", "����");
                if (!string.IsNullOrEmpty(base.Request.QueryString["isTaxisDESC"]))
                {
                    isTaxisDESC = TranslateUtils.ToBool(base.Request.QueryString["isTaxisDESC"]);
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlTaxis, isTaxisDESC.ToString());
                }

                ListItem listItem = new ListItem("ȫ��", string.Empty);
                this.ddlStatus.Items.Add(listItem);
                ELeadStatusUtils.AddListItems(this.ddlStatus);
                if (!string.IsNullOrEmpty(base.Request.QueryString["status"]))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlStatus, base.Request.QueryString["status"]);
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
                        ltlTouch.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">��ϵ{1}</a>", Modal.Touch.GetShowPopWinString(leadID, 0), countHtml);
                    }
                }

                this.hlAdd.NavigateUrl = BackgroundLeadAdd.GetAddUrl(this.PageUrl);
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
                    priority = @"<span style=""color:red"">�����ȣ�</span>";
                }
                else if (leadInfo.Priority == 3)
                {
                    ltlTR.Text = @"<tr class=""error"">";
                    priority = @"<span style=""color:red"">��������Ҫ��</span>";
                }

                ltlID.Text = leadInfo.ID.ToString();

                ltlID.Text = leadInfo.ID.ToString();

                ltlSubject.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}{2}</a>", BackgroundApplyToReplyDetail.GetRedirectUrl(leadInfo.ID), leadInfo.Subject, priority);
                ltlPossibility.Text = (leadInfo.Possibility > 0) ? leadInfo.Possibility.ToString() + "%" : string.Empty;
                ltlContactName.Text = leadInfo.ContactName;
                ltlAccountName.Text = leadInfo.AccountName;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(leadInfo.AddDate);
                ltlTelephone.Text = leadInfo.Telephone + "," + leadInfo.Mobile;
                ltlTelephone.Text = ltlTelephone.Text.Trim(',');

                ltlSource.Text = leadInfo.Source;
                ltlTouch.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">��ϵ</a>", Modal.Touch.GetShowPopWinString(leadInfo.ID, 0));
                
                ltlStatus.Text = ELeadStatusUtils.GetText(leadInfo.Status);
                if (leadInfo.Status == ELeadStatus.New)
                {
                    ltlStatus.Text = string.Format("<span style='color:red'>{0}</span>", ltlStatus.Text);
                }
                ltlChargeUserName.Text = AdminManager.GetDisplayName(leadInfo.ChargeUserName, false);

                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">�༭</a>", BackgroundLeadAdd.GetEditUrl(leadInfo.ID, this.PageUrl));
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        protected string GetSelectString()
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["status"]) || !string.IsNullOrEmpty(base.Request.QueryString["typeID"]) || !string.IsNullOrEmpty(base.Request.QueryString["addUserName"]) || !string.IsNullOrEmpty(base.Request.QueryString["userName"]) || !string.IsNullOrEmpty(base.Request.QueryString["keyword"]))
            {
                return DataProvider.LeadDAO.GetSelectString(AdminManager.Current.UserName, base.Request.QueryString["status"], base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.LeadDAO.GetSelectString(AdminManager.Current.UserName);
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
                    _pageUrl = BackgroundLead.GetRedirectUrl(TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.ddlStatus.SelectedValue, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(bool isTaxisDESC, string status, string keyword, int page)
        {
            return string.Format("background_lead.aspx?isTaxisDESC={0}&status={1}&keyword={2}&page={3}", isTaxisDESC, status, keyword, page);
        }
    }
}
