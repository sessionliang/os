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
    public class BackgroundAppointment : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAddSingle;
        public Button btnAddMultiple;
        public Button btnDelete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_appointment.aspx?publishmentSystemID={0}", publishmentSystemID));
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
                        DataProviderWX.AppointmentDAO.Delete(base.PublishmentSystemID,list) ;
                        base.SuccessMessage("微预约删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "微预约删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.AppointmentDAO.GetSelectString(base.PublishmentSystemID);
            this.spContents.SortField = AppointmentAttribute.ID;
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Appointment, "微预约", AppManager.WeiXin.Permission.WebSite.Appointment);
                this.spContents.DataBind();

                string urlAddSingle = BackgroundAppointmentSingleAdd.GetRedirectUrl(base.PublishmentSystemID, 0,0);
                string urlAddMultiple = BackgroundAppointmentMultipleAdd.GetRedirectUrl(base.PublishmentSystemID, 0,0);
                this.btnAddSingle.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlAddSingle));
                this.btnAddMultiple.Attributes.Add("onclick", string.Format("location.href='{0}';return false", urlAddMultiple));

                string urlDelete = PageUtils.AddQueryString(BackgroundAppointment.GetRedirectUrl(base.PublishmentSystemID), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的微预约", "此操作将删除所选微预约，确认吗？"));
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                AppointmentInfo appointmentInfo = new AppointmentInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                Literal ltlKeywords = e.Item.FindControl("ltlKeywords") as Literal;
                Literal ltlStartDate = e.Item.FindControl("ltlStartDate") as Literal;
                Literal ltlEndDate = e.Item.FindControl("ltlEndDate") as Literal;
                Literal ltlContentIsSingle = e.Item.FindControl("ltlContentIsSingle") as Literal;
                Literal ltlPVCount = e.Item.FindControl("ltlPVCount") as Literal;
                Literal ltlUserCount = e.Item.FindControl("ltlUserCount") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlAppointmentContentUrl = e.Item.FindControl("ltlAppointmentContentUrl") as Literal;
                Literal ltlPreviewUrl = e.Item.FindControl("ltlPreviewUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlTitle.Text = appointmentInfo.Title;
                ltlKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(appointmentInfo.KeywordID);
                ltlStartDate.Text = DateUtils.GetDateAndTimeString(appointmentInfo.StartDate);
                ltlEndDate.Text = DateUtils.GetDateAndTimeString(appointmentInfo.EndDate);
                ltlContentIsSingle.Text = appointmentInfo.ContentIsSingle == true ? "单预约" : "多预约";
                ltlPVCount.Text = appointmentInfo.PVCount.ToString();
                ltlUserCount.Text = appointmentInfo.UserCount.ToString();
                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(!appointmentInfo.IsDisabled);

                string urlAppointmentContent = BackgroundAppointmentContent.GetRedirectUrl(base.PublishmentSystemID, appointmentInfo.ID);
                ltlAppointmentContentUrl.Text = string.Format(@"<a href=""{0}"">预约查看</a>", urlAppointmentContent);

                int itemID = 0;
                if (appointmentInfo.ContentIsSingle)
                {
                    itemID = DataProviderWX.AppointmentItemDAO.GetItemID(base.PublishmentSystemID, appointmentInfo.ID);
                }

                string urlPreview = AppointmentManager.GetIndexUrl(base.PublishmentSystemID, appointmentInfo.ID, string.Empty);
                if (appointmentInfo.ContentIsSingle)
                {
                    urlPreview = AppointmentManager.GetItemUrl(base.PublishmentSystemID, appointmentInfo.ID, itemID, string.Empty);
                }
                urlPreview = BackgroundPreview.GetRedirectUrlToMobile(urlPreview);
                ltlPreviewUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">预览</a>", urlPreview);

                string urlEdit = BackgroundAppointmentMultipleAdd.GetRedirectUrl(base.PublishmentSystemID, appointmentInfo.ID, itemID);
                if (appointmentInfo.ContentIsSingle)
                {
                    urlEdit = BackgroundAppointmentSingleAdd.GetRedirectUrl(base.PublishmentSystemID, appointmentInfo.ID, itemID);
                }
                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", urlEdit);
            }
        }
    }
}
