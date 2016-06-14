using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using System.Collections;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CRM.Model;
using System.Text;

using System;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CRM.Core
{
	public class ApplyManager
	{
        public static string GetPriorityText(int priority)
        {
            if (priority == 1)
            {
                return "低";
            }
            else if (priority == 3)
            {
                return "高";
            }
            return "普通";
        }

        public static string GetRemark(int applyID)
        {
            StringBuilder remarkBuilder = new StringBuilder();
            ArrayList remarkInfoArrayList = DataProvider.RemarkDAO.GetRemarkInfoArrayList(applyID);
            foreach (RemarkInfo remarkInfo in remarkInfoArrayList)
            {
                if (!string.IsNullOrEmpty(remarkInfo.Remark))
                {
                    remarkBuilder.AppendFormat(@"<span style=""color:gray;"">{0}{1}: </span><br />{2}<br />", AdminManager.GetDisplayName(remarkInfo.UserName, true), ERemarkTypeUtils.GetText(remarkInfo.RemarkType), StringUtils.MaxLengthText(remarkInfo.Remark, 25));
                }
            }
            if (remarkBuilder.Length > 0) remarkBuilder.Length -= 6;
            return remarkBuilder.ToString();
        }

        public static void LogNew(int applyID, string toDepartmentName)
        {
            ProjectLogInfo logInfo = new ProjectLogInfo(0, applyID, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, EProjectLogType.New, PageUtils.GetIPAddress(), DateTime.Now, string.Format("{0}提交办件{1}", AdminManager.GetDisplayName(AdminManager.Current.UserName, true), toDepartmentName));
            DataProvider.ProjectLogDAO.Insert(logInfo);
        }

        public static void LogSwitchTo(int applyID, string switchToUserName)
        {
            ProjectLogInfo logInfo = new ProjectLogInfo(0, applyID, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, EProjectLogType.SwitchTo, PageUtils.GetIPAddress(), DateTime.Now, string.Format("{0}转办办件至{1} ", AdminManager.GetDisplayName(AdminManager.Current.UserName, true), AdminManager.GetDisplayName(switchToUserName, true)));
            DataProvider.ProjectLogDAO.Insert(logInfo);
        }

        public static void Log(int applyID, EProjectLogType logType)
        {
            ProjectLogInfo logInfo = new ProjectLogInfo(0, applyID, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, logType, PageUtils.GetIPAddress(), DateTime.Now, string.Empty);

            if (logType == EProjectLogType.Accept)
            {
                logInfo.Summary = string.Format("{0}受理办件", AdminManager.GetDisplayName(AdminManager.Current.UserName, true));
            }
            else if (logType == EProjectLogType.Deny)
            {
                logInfo.Summary = string.Format("{0}拒绝受理办件", AdminManager.GetDisplayName(AdminManager.Current.UserName, true));
            }
            else if (logType == EProjectLogType.Reply)
            {
                logInfo.Summary = string.Format("{0}回复办件", AdminManager.GetDisplayName(AdminManager.Current.UserName, true));
            }
            else if (logType == EProjectLogType.Comment)
            {
                logInfo.Summary = string.Format("{0}批注办件", AdminManager.GetDisplayName(AdminManager.Current.UserName, true));
            }
            else if (logType == EProjectLogType.Redo)
            {
                logInfo.Summary = string.Format("{0}要求返工", AdminManager.GetDisplayName(AdminManager.Current.UserName, true));
            }
            else if (logType == EProjectLogType.Check)
            {
                logInfo.Summary = string.Format("{0}审核通过", AdminManager.GetDisplayName(AdminManager.Current.UserName, true));
            }
            DataProvider.ProjectLogDAO.Insert(logInfo);
        }

        public static string GetDateLimit(PublishmentSystemInfo publishmentSystemInfo, ApplyInfo applyInfo)
        {
            if (applyInfo.State == EApplyState.Checked || applyInfo.State == EApplyState.Denied)
            {
                return string.Empty;
            }
            if (publishmentSystemInfo == null)
            {
                publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(applyInfo.ProjectID);
            }
            string dateLimit = string.Empty;
            DateTime endDate = applyInfo.EndDate;
            if (endDate == DateUtils.SqlMinValue)
            {
                endDate = applyInfo.ExpectedDate.AddDays(ConfigurationManager.Additional.ProjectApplyDateLimit);
            }

            int days = new TimeSpan(endDate.Ticks - DateTime.Now.Ticks).Days;
            if (days < 0)
            {
                dateLimit = string.Format("已超期{0}天", -days);
            }
            else if (days == 0)
            {
                dateLimit = "今天需完成";
            }
            else
            {
                dateLimit = string.Format("剩余{0}天", days);
            }

            return dateLimit;
        }

        public static ELimitType GetLimitType(ApplyInfo applyInfo)
        {
            if (applyInfo.State == EApplyState.Checked || applyInfo.State == EApplyState.Denied)
            {
                return ELimitType.Green;
            }

            string dateLimit = string.Empty;
            DateTime endDate = applyInfo.EndDate;
            if (endDate == DateUtils.SqlMinValue)
            {
                endDate = applyInfo.ExpectedDate.AddDays(ConfigurationManager.Additional.ProjectApplyDateLimit);
            }

            int days = new TimeSpan(endDate.Ticks - DateTime.Now.Ticks).Days;

            int alert = ConfigurationManager.Additional.ProjectApplyAlertDate;
            int yellow = alert + ConfigurationManager.Additional.ProjectApplyYellowAlertDate;
            int red = yellow + ConfigurationManager.Additional.ProjectApplyRedAlertDate;

            if (days + red <=0)
            {
                return ELimitType.Red;
            }
            else if (days + yellow <=0)
            {
                return ELimitType.Yellow;
            }
            else if (days + alert <=0)
            {
                return ELimitType.Alert;
            }

            return ELimitType.Normal;
        }
	}
}
