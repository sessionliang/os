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
    public class BackgroundConsultation : BackgroundBasePage
    {
        public DropDownList ddlTaxis;
        public DropDownList ddlStatus;
        public TextBox tbKeyword;

        public HyperLink hlSetting;
        public HyperLink hlDelete;

        public Repeater rptContents;
        public SqlPager spContents;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!string.IsNullOrEmpty(base.GetQueryString("Delete")))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("IDCollection"));
                if (arraylist.Count > 0)
                {
                    try
                    {
                        DataProviderB2C.ConsultationDAO.Delete(arraylist);
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
            this.spContents.SortField = "AddDate";
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
                EBooleanUtils.AddListItems(this.ddlStatus, "已关闭", "处理中");
                if (!string.IsNullOrEmpty(base.GetQueryString("status")))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlStatus, base.GetQueryString("status"));
                }

                this.tbKeyword.Text = base.GetQueryString("keyword");

                this.spContents.DataBind();

                this.hlSetting.Attributes.Add("onclick", Modal.ConsultationSetting.GetShowPopWinString());

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的咨询！", "此操作将删除所选咨询，确定吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ConsultationInfo consultationInfo = new ConsultationInfo(e.Item.DataItem);

                Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlQuestion = e.Item.FindControl("ltlQuestion") as Literal;
                Literal ltlConsultationType = e.Item.FindControl("ltlConsultationType") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlEstimateUrl = e.Item.FindControl("ltlEstimateUrl") as Literal;
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlID.Text = consultationInfo.ID.ToString();

                ltlQuestion.Text = string.Format(@"<a href=""{0}"">{1}</a>", BackgroundConsultationAnswer.GetRedirectUrl(base.PublishmentSystemID, consultationInfo.ChannelID, consultationInfo.ContentID, consultationInfo.ID, this.PageUrl), consultationInfo.Question);

                int userID = BaiRongDataProvider.UserDAO.GetUserID(base.PublishmentSystemInfo.GroupSN, consultationInfo.AddUser);
                ltlUserName.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", UserView.GetOpenWindowString(base.PublishmentSystemID, userID), consultationInfo.AddUser);

                ltlConsultationType.Text = EConsultationTypeUtils.GetText(consultationInfo.Type);

                ltlAddDate.Text = DateUtils.GetDateAndTimeString(consultationInfo.AddDate);

                ltlStatus.Text = consultationInfo.IsReply ? "已关闭" : "处理中";

                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", BackgroundConsultationAdd.GetEditUrl(base.PublishmentSystemID, consultationInfo.ChannelID, consultationInfo.ContentID, consultationInfo.ID, this.PageUrl));
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        protected string GetSelectString()
        {
            if (!string.IsNullOrEmpty(base.GetQueryString("keyword"))|| !string.IsNullOrEmpty(base.GetQueryString("status")))
            {
                return DataProviderB2C.ConsultationDAO.GetSelectString(TranslateUtils.ToBool(base.GetQueryString("status")), base.GetQueryString("keyword"));
            }
            else
            {
                return DataProviderB2C.ConsultationDAO.GetSelectString();
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
                    _pageUrl = BackgroundConsultation.GetRedirectUrl(base.PublishmentSystemID, TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.ddlStatus.SelectedValue, this.tbKeyword.Text, TranslateUtils.ToInt(base.GetQueryString("page"), 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, bool isTaxisDESC, string status, string keyword, int page)
        {
            return PageUtils.GetB2CUrl(string.Format("background_consultation.aspx?publishmentSystemID={0}&isTaxisDESC={1}&status={2}&keyword={3}&page={4}", publishmentSystemID, isTaxisDESC, status, keyword, page));
        }
    }
}
