using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundConference : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button btnDelete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_conference.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.ConferenceDAO.Delete(base.PublishmentSystemID, list);
                        base.SuccessMessage("会议（活动）删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "会议（活动）删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.ConferenceDAO.GetSelectString(base.PublishmentSystemID);
            this.spContents.SortField = ConferenceAttribute.ID;
            this.spContents.SortMode = SortMode.ASC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Conference, "微会议（活动）", AppManager.WeiXin.Permission.WebSite.Conference);
                this.spContents.DataBind();

                string urlAdd = BackgroundConferenceAdd.GetRedirectUrl(base.PublishmentSystemID, 0);
                this.btnAdd.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlAdd));

                string urlDelete = PageUtils.AddQueryString(BackgroundConference.GetRedirectUrl(base.PublishmentSystemID), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的会议（活动）活动", "此操作将删除所选会议（活动）活动，确认吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ConferenceInfo conferenceInfo = new ConferenceInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlKeywords = e.Item.FindControl("ltlKeywords") as Literal;
                Literal ltlStartDate = e.Item.FindControl("ltlStartDate") as Literal;
                Literal ltlEndDate = e.Item.FindControl("ltlEndDate") as Literal;
                Literal ltlUserCount = e.Item.FindControl("ltlUserCount") as Literal;
                Literal ltlPVCount = e.Item.FindControl("ltlPVCount") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlLogUrl = e.Item.FindControl("ltlLogUrl") as Literal;
                Literal ltlPreviewUrl = e.Item.FindControl("ltlPreviewUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlTitle.Text = conferenceInfo.Title;
                ltlKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(conferenceInfo.KeywordID);
                ltlStartDate.Text = DateUtils.GetDateAndTimeString(conferenceInfo.StartDate);
                ltlEndDate.Text = DateUtils.GetDateAndTimeString(conferenceInfo.EndDate);
                ltlUserCount.Text = conferenceInfo.UserCount.ToString();
                ltlPVCount.Text = conferenceInfo.PVCount.ToString();

                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(!conferenceInfo.IsDisabled);

                string urlContent = BackgroundConferenceContent.GetRedirectUrl(base.PublishmentSystemID, conferenceInfo.ID, BackgroundConference.GetRedirectUrl(base.PublishmentSystemID));
                ltlLogUrl.Text = string.Format(@"<a href=""{0}"">申请参会列表</a>", urlContent);

                string urlPreview = ConferenceManager.GetConferenceUrl(conferenceInfo, string.Empty);
                urlPreview = BackgroundPreview.GetRedirectUrlToMobile(urlPreview);
                ltlPreviewUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">预览</a>", urlPreview);

                string urlEdit = BackgroundConferenceAdd.GetRedirectUrl(base.PublishmentSystemID, conferenceInfo.ID);
                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", urlEdit);
            }
        }
    }
}
