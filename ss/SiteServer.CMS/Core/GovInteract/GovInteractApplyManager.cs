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
	public class GovInteractApplyManager
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

        public static string GetApplyRemark(int publishmentSystemID, int contentID)
        {
            StringBuilder remarkBuilder = new StringBuilder();
            ArrayList remarkInfoArrayList = DataProvider.GovInteractRemarkDAO.GetRemarkInfoArrayList(publishmentSystemID, contentID);
            foreach (GovInteractRemarkInfo remarkInfo in remarkInfoArrayList)
            {
                if (!string.IsNullOrEmpty(remarkInfo.Remark))
                {
                    if (remarkBuilder.Length > 0) remarkBuilder.Append("<br />");
                    remarkBuilder.AppendFormat(@"<span style=""color:gray;"">{0}意见: </span>{1}", EGovInteractRemarkTypeUtils.GetText(remarkInfo.RemarkType), StringUtils.MaxLengthText(remarkInfo.Remark, 25));
                }
            }
            return remarkBuilder.ToString();
        }

        public static void LogNew(int publishmentSystemID, int nodeID, int contentID, string realName, string toDepartmentName)
        {
            GovInteractLogInfo logInfo = new GovInteractLogInfo(0, publishmentSystemID, nodeID, contentID, 0, string.Empty, EGovInteractLogType.New, PageUtils.GetIPAddress(), DateTime.Now, string.Format("前台{0}提交办件{1}", realName, toDepartmentName));
            DataProvider.GovInteractLogDAO.Insert(logInfo);
        }

        public static void LogSwitchTo(int publishmentSystemID, int nodeID, int contentID, string switchToDepartmentName)
        {
            GovInteractLogInfo logInfo = new GovInteractLogInfo(0, publishmentSystemID, nodeID, contentID, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, EGovInteractLogType.SwitchTo, PageUtils.GetIPAddress(), DateTime.Now, string.Format("{0}({1})转办办件至{2} ", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName, switchToDepartmentName));
            DataProvider.GovInteractLogDAO.Insert(logInfo);
        }

        public static void LogTranslate(int publishmentSystemID, int nodeID, int contentID, string nodeName)
        {
            GovInteractLogInfo logInfo = new GovInteractLogInfo(0, publishmentSystemID, nodeID, contentID, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, EGovInteractLogType.Translate, PageUtils.GetIPAddress(), DateTime.Now, string.Format("{0}({1})从分类“{2}”转移办件至此 ", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName, nodeName));
            DataProvider.GovInteractLogDAO.Insert(logInfo);
        }

        public static void Log(int publishmentSystemID, int nodeID, int contentID, EGovInteractLogType logType)
        {
            GovInteractLogInfo logInfo = new GovInteractLogInfo(0, publishmentSystemID, nodeID, contentID, AdminManager.Current.DepartmentID, AdminManager.Current.UserName, logType, PageUtils.GetIPAddress(), DateTime.Now, string.Empty);

            if (logType == EGovInteractLogType.Accept)
            {
                logInfo.Summary = string.Format("{0}({1})受理办件", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            else if (logType == EGovInteractLogType.Deny)
            {
                logInfo.Summary = string.Format("{0}({1})拒绝受理办件", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            else if (logType == EGovInteractLogType.Reply)
            {
                logInfo.Summary = string.Format("{0}({1})回复办件", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            else if (logType == EGovInteractLogType.Comment)
            {
                logInfo.Summary = string.Format("{0}({1})批示办件", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            else if (logType == EGovInteractLogType.Redo)
            {
                logInfo.Summary = string.Format("{0}({1})要求返工", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            else if (logType == EGovInteractLogType.Check)
            {
                logInfo.Summary = string.Format("{0}({1})审核通过", AdminManager.CurrrentDepartmentName, AdminManager.Current.UserName);
            }
            DataProvider.GovInteractLogDAO.Insert(logInfo);
        }

        public static EGovInteractLimitType GetLimitType(PublishmentSystemInfo publishmentSystemInfo, GovInteractContentInfo contentInfo)
        {
            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - contentInfo.AddDate.Ticks);

            int alert = publishmentSystemInfo.Additional.GovInteractApplyDateLimit + publishmentSystemInfo.Additional.GovInteractApplyAlertDate;
            int yellow = alert + publishmentSystemInfo.Additional.GovInteractApplyYellowAlertDate;
            int red = yellow + publishmentSystemInfo.Additional.GovInteractApplyRedAlertDate;

            if (ts.Days >= red)
            {
                return EGovInteractLimitType.Red;
            }
            else if (ts.Days >= yellow)
            {
                return EGovInteractLimitType.Yellow;
            }
            else if (ts.Days >= alert)
            {
                return EGovInteractLimitType.Alert;
            }
           
            return EGovInteractLimitType.Normal;
        }
	}
}
