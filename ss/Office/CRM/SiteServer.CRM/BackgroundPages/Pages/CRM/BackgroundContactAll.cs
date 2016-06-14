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
    public class BackgroundContactAll : BackgroundBasePage
    {
        public DropDownList ddlTaxis;
        public TextBox tbKeyword;

        public HyperLink hlAdd;
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
                        DataProvider.ContactDAO.Delete(deleteIDList);
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
            this.spContents.SortField = DataProvider.ContactDAO.GetSortFieldName();
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

                this.hlAdd.NavigateUrl = BackgroundContactAdd.GetAddUrl(this.PageUrl);

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的联系人！", "此操作将删除所选联系人，确定吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ContactInfo contactInfo = new ContactInfo(e.Item.DataItem);

                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlContactName = e.Item.FindControl("ltlContactName") as Literal;
                Literal ltlJobTitle = e.Item.FindControl("ltlJobTitle") as Literal;
                Literal ltlAccountRole = e.Item.FindControl("ltlAccountRole") as Literal;
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlEmail = e.Item.FindControl("ltlEmail") as Literal;
                Literal ltlQQ = e.Item.FindControl("ltlQQ") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlID.Text = contactInfo.ID.ToString();

                ltlContactName.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", BackgroundApplyToReplyDetail.GetRedirectUrl(contactInfo.ID), contactInfo.ContactName);
                ltlJobTitle.Text = contactInfo.JobTitle;
                ltlAccountRole.Text = contactInfo.AccountRole;
                ltlMobile.Text = contactInfo.Mobile;
                ltlEmail.Text = contactInfo.Email;
                ltlQQ.Text = contactInfo.QQ;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(contactInfo.AddDate);
                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", BackgroundContactAdd.GetEditUrl(contactInfo.ID, this.PageUrl));
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
                return DataProvider.ContactDAO.GetSelectString(AdminManager.Current.UserName, base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.ContactDAO.GetSelectString(AdminManager.Current.UserName);
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
                    _pageUrl = BackgroundContactAll.GetRedirectUrl(TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(bool isTaxisDESC, string keyword, int page)
        {
            return string.Format("background_contactAll.aspx?isTaxisDESC={0}&keyword={1}&page={2}", isTaxisDESC, keyword, page);
        }
    }
}
