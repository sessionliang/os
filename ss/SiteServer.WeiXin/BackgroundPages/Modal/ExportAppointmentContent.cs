using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;
using System.Data.OleDb;
using SiteServer.CMS.BackgroundPages;
using System.Web.UI;
using System.Collections.Generic;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.IO;
using System.Web;
using SiteServer.CMS.Core.Office;
using BaiRong.Core.IO;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class ExportAppointmentContent : BackgroundBasePage
    {
        public static string GetOpenWindowStringByAppointmentContent(int publishmentSystemID, int appointmentID, string appointmentTitle)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("appointmentID", appointmentID.ToString());
            arguments.Add("appointmentTitle", appointmentTitle);

            return JsUtils.OpenWindow.GetOpenWindowString("导出CSV", "modal_exportAppointmentContent.aspx", arguments, 400, 240, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                int appointmentID = TranslateUtils.ToInt(base.GetQueryString("appointmentID"));
                string appointmentTitle = base.GetQueryString("appointmentTitle");

                List<AppointmentContentInfo> appointmentContentInfolList = DataProviderWX.AppointmentContentDAO.GetAppointmentContentInfoList(base.PublishmentSystemID, appointmentID);

                if (appointmentContentInfolList.Count == 0)
                {
                    base.FailMessage("暂无数据导出！");
                    return;
                }

                string docFileName = "预约名单" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                string filePath = PathUtils.GetTemporaryFilesPath(docFileName);

                this.ExportAppointmentContentCSV(filePath, appointmentContentInfolList, appointmentTitle, appointmentID);

                string fileUrl = PageUtils.GetTemporaryFilesUrl(docFileName);
                base.SuccessMessage(string.Format(@"成功导出文件，请点击 <a href=""{0}"">这里</a> 进行下载！", fileUrl));
            }
        }

        public void ExportAppointmentContentCSV(string filePath, List<AppointmentContentInfo> appointmentContentInfolList, string appointmentTitle, int appointmentID)
        {
            AppointmentInfo appointmentInfo = DataProviderWX.AppointmentDAO.GetAppointmentInfo(appointmentID);

            List<string> nameList = new List<string>();
            nameList.Add("序号");
            nameList.Add("预约名称");
            if (appointmentInfo.IsFormRealName == "True")
            {
                nameList.Add(appointmentInfo.FormRealNameTitle);
            }
            if (appointmentInfo.IsFormMobile == "True")
            {
                nameList.Add(appointmentInfo.FormMobileTitle);
            }
            if (appointmentInfo.IsFormEmail == "True")
            {
                nameList.Add(appointmentInfo.FormEmailTitle);
            }
            nameList.Add("预约时间");
            nameList.Add("预约状态");
            nameList.Add("留言");
            List<ConfigExtendInfo> configExtendInfoList = DataProviderWX.ConfigExtendDAO.GetConfigExtendInfoList(base.PublishmentSystemID, appointmentID, EKeywordTypeUtils.GetValue(EKeywordType.Appointment));
            foreach (var cList in configExtendInfoList)
            {
                nameList.Add(cList.AttributeName);
            }


            List<List<string>> valueListOfList = new List<List<string>>();

            int index = 1;
            foreach (AppointmentContentInfo applist in appointmentContentInfolList)
            {

                List<string> valueList = new List<string>();

                valueList.Add((index++).ToString());
                valueList.Add(appointmentTitle);
                if (appointmentInfo.IsFormRealName == "True")
                {
                    valueList.Add(applist.RealName);
                }
                if (appointmentInfo.IsFormMobile == "True")
                {
                    valueList.Add(applist.Mobile);
                }
                if (appointmentInfo.IsFormEmail == "True")
                {
                    valueList.Add(applist.Email);
                }
                valueList.Add(DateUtils.GetDateAndTimeString(applist.AddDate));
                valueList.Add(EAppointmentStatusUtils.GetText(EAppointmentStatusUtils.GetEnumType(applist.Status)));
                valueList.Add(applist.Message);

                string SettingsXML = applist.SettingsXML.Replace("{", "").Replace("}", "");
                string[] arr = SettingsXML.Split(',');
                if (arr[0] != "")
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string[] arr1 = arr[i].Replace("\"", "").Split(':');
                        valueList.Add(arr1[1]);
                    }
                }
                valueListOfList.Add(valueList);
            }

            CSVUtils.ExportCSVFile(filePath, nameList, valueListOfList);
        }

    }
}
