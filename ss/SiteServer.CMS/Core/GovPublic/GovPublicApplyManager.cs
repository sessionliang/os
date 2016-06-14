using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using SiteServer.CMS.Core.Security;
using System.Collections;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Model;
using System.Text;

using System;

namespace SiteServer.CMS.Core
{
	public class GovPublicApplyManager
	{
        public static string GetQueryCode()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks).ToUpper();
        }

        public static string GetApplyRemark(int applyID)
        {
            StringBuilder remarkBuilder = new StringBuilder();
            ArrayList remarkInfoArrayList = DataProvider.GovPublicApplyRemarkDAO.GetRemarkInfoArrayList(applyID);
            foreach (GovPublicApplyRemarkInfo remarkInfo in remarkInfoArrayList)
            {
                if (!string.IsNullOrEmpty(remarkInfo.Remark))
                {
                    remarkBuilder.AppendFormat(@"<span style=""color:gray;"">{0}({1}){2}意见: </span><br />{3}<br />", DepartmentManager.GetDepartmentName(remarkInfo.DepartmentID), remarkInfo.UserName, EGovPublicApplyRemarkTypeUtils.GetText(remarkInfo.RemarkType), StringUtils.MaxLengthText(remarkInfo.Remark, 25));
                }
            }
            if (remarkBuilder.Length > 0) remarkBuilder.Length -= 6;
            return remarkBuilder.ToString();
        }

        public static void LogNew(int publishmentSystemID, int applyID, string fromName, string toDepartmentName)
        {
            GovPublicApplyLogInfo logInfo = new GovPublicApplyLogInfo(0, publishmentSystemID, applyID, 0, string.Empty, EGovPublicApplyLogType.New, PageUtils.GetIPAddress(), DateTime.Now, string.Format("前台{0}提交申请{1}", fromName, toDepartmentName));
            DataProvider.GovPublicApplyLogDAO.Insert(logInfo);
        }

        public static void LogSwitchTo(int publishmentSystemID, int applyID, string switchToDepartmentName)
        {
            GovPublicApplyLogInfo logInfo = new GovPublicApplyLogInfo(0, publishmentSystemID, applyID, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, EGovPublicApplyLogType.SwitchTo, PageUtils.GetIPAddress(), DateTime.Now, string.Format("{0}({1})转办申请至{2} ", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName, switchToDepartmentName));
            DataProvider.GovPublicApplyLogDAO.Insert(logInfo);
        }

        public static void Log(int publishmentSystemID, int applyID, EGovPublicApplyLogType logType)
        {
            GovPublicApplyLogInfo logInfo = new GovPublicApplyLogInfo(0, publishmentSystemID, applyID, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, logType, PageUtils.GetIPAddress(), DateTime.Now, string.Empty);

            if (logType == EGovPublicApplyLogType.Accept)
            {
                logInfo.Summary = string.Format("{0}({1})受理申请", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            else if (logType == EGovPublicApplyLogType.Deny)
            {
                logInfo.Summary = string.Format("{0}({1})拒绝受理申请", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            else if (logType == EGovPublicApplyLogType.Reply)
            {
                logInfo.Summary = string.Format("{0}({1})回复申请", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            else if (logType == EGovPublicApplyLogType.Comment)
            {
                logInfo.Summary = string.Format("{0}({1})批示申请", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            else if (logType == EGovPublicApplyLogType.Redo)
            {
                logInfo.Summary = string.Format("{0}({1})要求返工", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            else if (logType == EGovPublicApplyLogType.Check)
            {
                logInfo.Summary = string.Format("{0}({1})审核通过", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            DataProvider.GovPublicApplyLogDAO.Insert(logInfo);
        }

        public static EGovPublicApplyLimitType GetLimitType(PublishmentSystemInfo publishmentSystemInfo, GovPublicApplyInfo applyInfo)
        {
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - applyInfo.AddDate.Ticks);

            int alert = publishmentSystemInfo.Additional.GovPublicApplyDateLimit + publishmentSystemInfo.Additional.GovPublicApplyAlertDate;
            int yellow = alert + publishmentSystemInfo.Additional.GovPublicApplyYellowAlertDate;
            int red = yellow + publishmentSystemInfo.Additional.GovPublicApplyRedAlertDate;

            if (ts.Days >= red)
            {
                return EGovPublicApplyLimitType.Red;
            }
            else if (ts.Days >= yellow)
            {
                return EGovPublicApplyLimitType.Yellow;
            }
            else if (ts.Days >= alert)
            {
                return EGovPublicApplyLimitType.Alert;
            }
           
            return EGovPublicApplyLimitType.Normal;
        }
	}
}
