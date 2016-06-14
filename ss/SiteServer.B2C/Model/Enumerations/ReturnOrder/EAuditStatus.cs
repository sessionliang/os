using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteServer.B2C.Model
{
    /// <summary>
    /// 退换货申请状态
    /// </summary>
    public enum EAuditStatus
    {
        Wait,       //等待审核
        Pass,       //通过
        UnPass       //未通过
    }

    public class EAuditStatusUtils
    {
        public static string GetValue(EAuditStatus type)
        {
            if (type == EAuditStatus.Wait)
            {
                return "Wait";
            }
            else if (type == EAuditStatus.Pass)
            {
                return "Pass";
            }
            else if (type == EAuditStatus.UnPass)
            {
                return "UnPass";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(string strType)
        {
            return GetText(GetEnumType(strType));
        }

        public static string GetText(EAuditStatus type)
        {

            if (type == EAuditStatus.Wait)
            {
                return "等待审核";
            }
            else if (type == EAuditStatus.Pass)
            {
                return "通过";
            }
            else if (type == EAuditStatus.UnPass)
            {
                return "未通过";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EAuditStatus GetEnumType(string typeStr)
        {
            EAuditStatus retval = EAuditStatus.UnPass;

            if (Equals(EAuditStatus.Wait, typeStr))
            {
                retval = EAuditStatus.Wait;
            }
            else if (Equals(EAuditStatus.Pass, typeStr))
            {
                retval = EAuditStatus.Pass;
            }
            else if (Equals(EAuditStatus.UnPass, typeStr))
            {
                retval = EAuditStatus.UnPass;
            }
            return retval;
        }

        public static bool Equals(EAuditStatus type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EAuditStatus type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAuditStatus type, bool selected)
        {
            ListItem item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EAuditStatus.Wait, false));
                listControl.Items.Add(GetListItem(EAuditStatus.Pass, false));
                listControl.Items.Add(GetListItem(EAuditStatus.UnPass, false));
            }
        }
    }
}
