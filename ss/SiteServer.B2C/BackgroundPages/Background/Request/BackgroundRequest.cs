using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Web.UI;


using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundRequest : BackgroundBasePage
    {
        public DropDownList ddlTaxis;
        public DropDownList ddlStatus;
        public TextBox tbKeyword;

        public HyperLink hlAdd;
        public HyperLink hlSetting;
        public HyperLink hlDelete;

        public Repeater rptContents;
        public SqlPager spContents;

        public HyperLink hlConfiguration;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!string.IsNullOrEmpty(base.GetQueryString("Delete")))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("IDCollection"));
                if (arraylist.Count > 0)
                {
                    try
                    {
                        DataProviderB2C.RequestDAO.Delete(arraylist);
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
            this.spContents.SortField = DataProviderB2C.RequestDAO.GetSortFieldName();
            this.spContents.SortMode = this.GetSortMode();
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                bool isTaxisDESC = true;
                EBooleanUtils.AddListItems(this.ddlTaxis, "倒序", "正序");
                if (!string.IsNullOrEmpty(base.GetQueryString("isTaxisDESC")))
                {
                    isTaxisDESC = base.GetBoolQueryString("isTaxisDESC");
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlTaxis, isTaxisDESC.ToString());
                }

                ListItem listItem = new ListItem("全部", string.Empty);
                this.ddlStatus.Items.Add(listItem);
                ERequestStatusUtils.AddListItems(this.ddlStatus);
                if (!string.IsNullOrEmpty(base.GetQueryString("status")))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlStatus, base.GetQueryString("status"));
                }

                this.tbKeyword.Text = base.GetQueryString("keyword");

                this.spContents.DataBind();

                this.hlAdd.NavigateUrl = BackgroundRequestAdd.GetAddUrl(base.PublishmentSystemID, this.PageUrl);

                this.hlSetting.Attributes.Add("onclick", Modal.RequestSetting.GetShowPopWinString());

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的工单！", "此操作将删除所选工单，确定吗？"));

                this.hlConfiguration.Attributes.Add("onclick", Modal.RequestConfiguration.GetShowPopWinString(base.PublishmentSystemID));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RequestInfo requestInfo = new RequestInfo(e.Item.DataItem);

                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                Literal ltlSubject = e.Item.FindControl("ltlSubject") as Literal;
                Literal ltlAdminUserName = e.Item.FindControl("ltlAdminUserName") as Literal;
                Literal ltlRequestType = e.Item.FindControl("ltlRequestType") as Literal;
                Literal ltlWebsite = e.Item.FindControl("ltlWebsite") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlEstimateUrl = e.Item.FindControl("ltlEstimateUrl") as Literal;
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlSN.Text = requestInfo.SN;

                ltlSubject.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundRequestAnswer.GetRedirectUrl(base.PublishmentSystemID, requestInfo.ID, this.PageUrl), requestInfo.Subject);

                int userID = BaiRongDataProvider.UserDAO.GetUserID(base.PublishmentSystemInfo.GroupSN, requestInfo.UserName);
                ltlUserName.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", UserView.GetOpenWindowString(base.PublishmentSystemID, userID), requestInfo.UserName);
                ltlAdminUserName.Text = AdminManager.GetDisplayName(requestInfo.AdminUserName, true);
                ltlRequestType.Text = requestInfo.RequestType;

                if (!string.IsNullOrEmpty(requestInfo.Website))
                {
                    ltlWebsite.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtils.AddProtocolToUrl(requestInfo.Website), requestInfo.Website);
                }
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(requestInfo.AddDate);

                ltlStatus.Text = ERequestStatusUtils.GetText(requestInfo.Status);
                if (ERequestStatusUtils.Equals(requestInfo.Status, ERequestStatus.Processing))
                {
                    ltlStatus.Text = string.Format("<span style='color:red'>{0}</span>", ltlStatus.Text);
                }

                if (requestInfo.IsEstimate)
                {
                    ltlEstimateUrl.Text = string.Format(@"<code>{0}</code> {1}", ERequestEstimateUtils.GetText(requestInfo.EstimateValue), DateUtils.GetDateAndTimeString(requestInfo.EstimateDate));
                    if (!string.IsNullOrEmpty(requestInfo.EstimateComment))
                    {
                        ltlEstimateUrl.Text += string.Format(@"<br />{0}", requestInfo.EstimateComment);
                    }
                }

                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", BackgroundRequestAdd.GetEditUrl(base.PublishmentSystemID, requestInfo.ID, this.PageUrl));
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        protected string GetSelectString()
        {
            if (base.GetQueryString("keyword") != null)
            {
                return DataProviderB2C.RequestDAO.GetSelectString(string.Empty, base.GetQueryString("status"), base.GetQueryString("keyword"));
            }
            else
            {
                return DataProviderB2C.RequestDAO.GetSelectString(string.Empty);
            }
        }

        protected SortMode GetSortMode()
        {
            bool isTaxisDESC = true;
            if (!string.IsNullOrEmpty(base.GetQueryString("isTaxisDESC")))
            {
                isTaxisDESC = base.GetBoolQueryString("isTaxisDESC");
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
                    _pageUrl = BackgroundRequest.GetRedirectUrl(base.PublishmentSystemID, TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.ddlStatus.SelectedValue, this.tbKeyword.Text, TranslateUtils.ToInt(base.GetQueryString("page"), 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, bool isTaxisDESC, string status, string keyword, int page)
        {
            return PageUtils.GetB2CUrl(string.Format("background_request.aspx?publishmentSystemID={0}&isTaxisDESC={1}&status={2}&keyword={3}&page={4}", publishmentSystemID, isTaxisDESC, status, keyword, page));
        }
    }
}
