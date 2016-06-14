using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
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
    public class BackgroundAppointmentContent : BackgroundBasePageWX
    {
        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnHandle;
        public Button btnDelete;
        public Button btnExport;
        public Button btnReturn;

        public int appointmentID;

        public string SettingsXML;
        public string appointmentTitle;
        public Literal ltlExtendTitle;
        public int tdCount = 0;

        public static string GetRedirectUrl(int publishmentSystemID, int appointmentID)
        {
            return PageUtils.GetWXUrl(string.Format("background_appointmentContent.aspx?publishmentSystemID={0}&appointmentID={1}", publishmentSystemID, appointmentID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.appointmentID = TranslateUtils.ToInt(base.Request["appointmentID"]);

            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                List<int> list = TranslateUtils.StringCollectionToIntList(Request.QueryString["IDCollection"]);
                if (list.Count > 0)
                {
                    try
                    {
                        DataProviderWX.AppointmentContentDAO.Delete(base.PublishmentSystemID, list);
                        base.SuccessMessage("预约删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "预约删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProviderWX.AppointmentContentDAO.GetSelectString(base.PublishmentSystemID, this.appointmentID);
            this.spContents.SortField = AppointmentContentAttribute.ID;
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Appointment, "查看预约", AppManager.WeiXin.Permission.WebSite.Appointment);
                this.spContents.DataBind();

                this.btnHandle.Attributes.Add("onclick", Modal.AppointmentHandle.GetOpenWindowStringToMultiple(base.PublishmentSystemID));

                string urlDelete = PageUtils.AddQueryString(BackgroundAppointmentContent.GetRedirectUrl(base.PublishmentSystemID, this.appointmentID), "Delete", "True");
                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(urlDelete, "IDCollection", "IDCollection", "请选择需要删除的微预约", "此操作将删除所选微预约，确认吗？"));

                this.btnExport.Attributes.Add("onclick", Modal.ExportAppointmentContent.GetOpenWindowStringByAppointmentContent(base.PublishmentSystemID, this.appointmentID, this.appointmentTitle));

                string returnUrl = BackgroundAppointment.GetRedirectUrl(base.PublishmentSystemID);
                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false", returnUrl));
            }
        }

        private Dictionary<int, AppointmentItemInfo> itemInfoDictionary = new Dictionary<int, AppointmentItemInfo>();

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                AppointmentContentInfo contentInfo = new AppointmentContentInfo(e.Item.DataItem);

                Literal ltlItemIndex = e.Item.FindControl("ltlItemIndex") as Literal;
                Literal ltlRealName = e.Item.FindControl("ltlRealName") as Literal;
                Literal ltlAppointementTitle = e.Item.FindControl("ltlAppointementTitle") as Literal;
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlExtendVal = e.Item.FindControl("ltlExtendVal") as Literal;
                Literal ltlEmail = e.Item.FindControl("ltlEmail") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlStatus = e.Item.FindControl("ltlStatus") as Literal;
                Literal ltlMessage = e.Item.FindControl("ltlMessage") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlSelectUrl = e.Item.FindControl("ltlSelectUrl") as Literal;



                AppointmentItemInfo itemInfo = null;
                if (this.itemInfoDictionary.ContainsKey(contentInfo.AppointmentItemID))
                {
                    itemInfo = this.itemInfoDictionary[contentInfo.AppointmentItemID];
                }
                else
                {
                    itemInfo = DataProviderWX.AppointmentItemDAO.GetItemInfo(contentInfo.AppointmentItemID);
                    this.itemInfoDictionary.Add(contentInfo.AppointmentItemID, itemInfo);
                }

                ltlItemIndex.Text = (e.Item.ItemIndex + 1).ToString();
                ltlRealName.Text = contentInfo.RealName;
                if (itemInfo != null)
                {
                    ltlAppointementTitle.Text = itemInfo.Title;
                    appointmentTitle = itemInfo.Title;
                }
                ltlMobile.Text = contentInfo.Mobile;
                ltlEmail.Text = contentInfo.Email;
                ltlAddDate.Text = DateUtils.GetDateAndTimeString(contentInfo.AddDate);
                ltlStatus.Text = EAppointmentStatusUtils.GetText(EAppointmentStatusUtils.GetEnumType(contentInfo.Status));
                ltlMessage.Text = contentInfo.Message;

                ltlEditUrl.Text = string.Format(@"<a href=""javascrip:;"" onclick=""{0}"">预约处理</a>", Modal.AppointmentHandle.GetOpenWindowStringToSingle(base.PublishmentSystemID, contentInfo.ID));

                ltlSelectUrl.Text = string.Format(@"<a href=""javascrip:;"" onclick=""{0}"">预约详情</a>", Modal.AppointmentContentDetail.GetOpenWindowStringToSingle(base.PublishmentSystemID, contentInfo.ID));

              
            }
        }

    }
}
