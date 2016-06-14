using BaiRong.Core;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class AppointmentContentDetail : BackgroundBasePage
    {
        public Literal ltlAppointementTitle;
        public Literal ltlRealName;
        public Literal ltlMobile;
        public Literal ltlEmail;
        public Literal ltlAddDate;
        public Literal ltlExtendVal;

        private int appointmentContentID;

        public static string GetOpenWindowStringToSingle(int publishmentSystemID, int appointmentContentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("appointmentContentID", appointmentContentID.ToString());
            return PageUtilityWX.GetOpenWindowString("预约详情查看", "modal_appointmentContentDetail.aspx", arguments, 450, 550);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.appointmentContentID = TranslateUtils.ToInt(base.GetQueryString("appointmentContentID"));

            if (!IsPostBack)
            {
                AppointmentContentInfo appointmentContentInfo = DataProviderWX.AppointmentContentDAO.GetContentInfo(this.appointmentContentID);
                AppointmentItemInfo appointmentItemInfo = DataProviderWX.AppointmentItemDAO.GetItemInfo(appointmentContentInfo.AppointmentItemID);
                this.ltlAppointementTitle.Text = appointmentItemInfo.Title;
                this.ltlMobile.Text = appointmentContentInfo.Mobile;
                this.ltlEmail.Text = appointmentContentInfo.Email;
                this.ltlAddDate.Text = DateUtils.GetDateAndTimeString(appointmentContentInfo.AddDate);
                ltlExtendVal.Text = "";
                if (!string.IsNullOrEmpty(appointmentContentInfo.SettingsXML) && appointmentContentInfo.SettingsXML.ToString().Trim() != "{}")
                {
                    string SettingsXML = appointmentContentInfo.SettingsXML.Replace("{", "").Replace("}", "");
                    StringBuilder stringBuilderHtml = new StringBuilder();

                    string[] arr = SettingsXML.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] arr1 = arr[i].Replace("\"", "").Split(':');
                        stringBuilderHtml.AppendFormat(@"<tr>");
                        stringBuilderHtml.AppendFormat(@"<td>{0}:</td>", arr1[0]);
                        stringBuilderHtml.AppendFormat(@"<td>{0}</td>", arr1[1]);
                        stringBuilderHtml.AppendFormat(@"</tr>");
                    }
                    ltlExtendVal.Text = stringBuilderHtml.ToString();
                }

            }
        }
    }
}
