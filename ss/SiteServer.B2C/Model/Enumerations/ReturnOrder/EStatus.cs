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
    public enum EStatus
    {
        ToDo,       //未处理
        Doing,       //处理中
        Done       //已处理
    }

    public class EStatusUtils
    {
        public static string GetValue(EStatus type)
        {
            if (type == EStatus.ToDo)
            {
                return "ToDo";
            }
            else if (type == EStatus.Doing)
            {
                return "Doing";
            }
            else if (type == EStatus.Done)
            {
                return "Done";
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

        public static string GetText(EStatus type)
        {

            if (type == EStatus.ToDo)
            {
                return "未处理";
            }
            else if (type == EStatus.Doing)
            {
                return "处理中";
            }
            else if (type == EStatus.Done)
            {
                return "已处理";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EStatus GetEnumType(string typeStr)
        {
            EStatus retval = EStatus.Done;

            if (Equals(EStatus.ToDo, typeStr))
            {
                retval = EStatus.ToDo;
            }
            else if (Equals(EStatus.Doing, typeStr))
            {
                retval = EStatus.Doing;
            }
            else if (Equals(EStatus.Done, typeStr))
            {
                retval = EStatus.Done;
            }
            return retval;
        }

        public static bool Equals(EStatus type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EOrderStatus type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EStatus type, bool selected)
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
                listControl.Items.Add(GetListItem(EStatus.ToDo, false));
                listControl.Items.Add(GetListItem(EStatus.Doing, false));
                listControl.Items.Add(GetListItem(EStatus.Done, false));
            }
        }
    }
}
