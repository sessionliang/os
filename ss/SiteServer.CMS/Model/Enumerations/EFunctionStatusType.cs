using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    /// <summary>
    /// 功能内容信息状态
    /// </summary>
    public enum EFunctionStatusType
    {
        NotViewed,
        AlreadyView,
        AuditNotPassed,
        AuditPassed,
	}

    public class EFunctionStatusTypeUtils
    {
        public static string GetValue(EFunctionStatusType type)
        {
            if (type == EFunctionStatusType.NotViewed)
            {
                return "NotViewed";
            }
            else if (type == EFunctionStatusType.AlreadyView)
            {
                return "AlreadyView";
            }
            else if (type == EFunctionStatusType.AuditNotPassed)
            {
                return "AuditNotPassed";
            }
            else if (type == EFunctionStatusType.AuditPassed)
            {
                return "AuditPassed";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EFunctionStatusType type)
        {
            if (type == EFunctionStatusType.NotViewed)
            {
                return "未查看";
            }
            if (type == EFunctionStatusType.AlreadyView)
            {
                return "未审核";
            }
            else if (type == EFunctionStatusType.AuditNotPassed)
            {
                return "审核未通过";
            }
            else if (type == EFunctionStatusType.AuditPassed)
            {
                return "审核通过";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EFunctionStatusType GetEnumType(string typeStr)
        {
            EFunctionStatusType retval = EFunctionStatusType.NotViewed;

            if (Equals(EFunctionStatusType.NotViewed, typeStr))
            {
                retval = EFunctionStatusType.NotViewed;
            }
            else if (Equals(EFunctionStatusType.AlreadyView, typeStr))
            {
                retval = EFunctionStatusType.AlreadyView;
            }
            else if (Equals(EFunctionStatusType.AuditNotPassed, typeStr))
            {
                retval = EFunctionStatusType.AuditNotPassed;
            }
            else if (Equals(EFunctionStatusType.AuditPassed, typeStr))
            {
                retval = EFunctionStatusType.AuditPassed;
            }

            return retval;
        }

        public static bool Equals(EFunctionStatusType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EFunctionStatusType type)
        {
            return Equals(type, typeStr);
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EFunctionStatusType.NotViewed, false));
                listControl.Items.Add(GetListItem(EFunctionStatusType.AlreadyView, false));
                listControl.Items.Add(GetListItem(EFunctionStatusType.AuditNotPassed, false));
                listControl.Items.Add(GetListItem(EFunctionStatusType.AuditPassed, false));
            }
        }

        public static ListItem GetListItem(EFunctionStatusType type, bool selected)
        {
            ListItem item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

    }
}
