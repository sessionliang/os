using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;

using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class AppointmentHandle : BackgroundBasePage
    { 
        public DropDownList ddlStatus;
        public PlaceHolder phMessage;
        public TextBox tbMessage;

        private int contentID;
        private List<int> contentIDList;

        public static string GetOpenWindowStringToSingle(int publishmentSystemID, int contentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("contentID", contentID.ToString());
            return PageUtilityWX.GetOpenWindowString("预约处理", "modal_appointmentHandle.aspx", arguments, 360, 380);
        }

        public static string GetOpenWindowStringToMultiple(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            return PageUtilityWX.GetOpenWindowStringWithCheckBoxValue("预约处理", "modal_appointmentHandle.aspx", arguments, "IDCollection", "请选择需要处理的预约申请", 360, 380);
        }
       
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.contentID = TranslateUtils.ToInt(base.GetQueryString("contentID"));
            if (this.contentID == 0)
            {
                this.contentIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("IDCollection"));
            }
             
            if (!IsPostBack)
            {
                EAppointmentStatusUtils.AddListItems(this.ddlStatus);
                ControlUtils.SelectListItems(this.ddlStatus, EAppointmentStatusUtils.GetValue(EAppointmentStatus.Agree));

                if (this.contentID > 0)
                {
                    AppointmentContentInfo contentInfo = DataProviderWX.AppointmentContentDAO.GetContentInfo(this.contentID);
                    if (contentInfo != null)
                    {
                        this.ddlStatus.SelectedValue = contentInfo.Status;
                        this.tbMessage.Text = contentInfo.Message;
                    }
                }
            }
        }

        public void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            EAppointmentStatus status = EAppointmentStatusUtils.GetEnumType(this.ddlStatus.SelectedValue);
            if (status == EAppointmentStatus.Agree || status == EAppointmentStatus.Refuse)
            {
                this.phMessage.Visible = true;
            }
            else
            {
                this.phMessage.Visible = false;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {
                if (this.contentID > 0)
                {
                    AppointmentContentInfo contentInfo = DataProviderWX.AppointmentContentDAO.GetContentInfo(this.contentID);
                    if (contentInfo != null)
                    {
                        contentInfo.Status = this.ddlStatus.SelectedValue;
                        contentInfo.Message = this.tbMessage.Text;
                        DataProviderWX.AppointmentContentDAO.Update(contentInfo);
                    }
                }
                else if (this.contentIDList != null && this.contentIDList.Count > 0)
                {
                    foreach (int theContentID in this.contentIDList)
                    {
                        AppointmentContentInfo contentInfo = DataProviderWX.AppointmentContentDAO.GetContentInfo(theContentID);
                        if (contentInfo != null)
                        {
                            contentInfo.Status = this.ddlStatus.SelectedValue;
                            contentInfo.Message = this.tbMessage.Text;
                            DataProviderWX.AppointmentContentDAO.Update(contentInfo);
                        }
                    }
                }
                base.SuccessMessage("预约处理成功！");

                JsUtils.OpenWindow.CloseModalPage(Page);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "失败：" + ex.Message);
            }
        }
    }
}
