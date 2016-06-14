using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EAdvLevelType
    {
        Hold,           //��ռ
        Standard       //��׼
    }

    public class EAdvLevelTypeUtils
    {
        public static string GetValue(EAdvLevelType type)
        {
            if (type == EAdvLevelType.Hold)
            {
                return "Hold";
            }
            else if (type == EAdvLevelType.Standard)
            {
                return "Standard";
            }
           else
            {
                throw new Exception();
            }
        }

        public static string GetText(EAdvLevelType type)
        {
            if (type == EAdvLevelType.Hold )
            {
                return "��ռ";
            }
            else if (type == EAdvLevelType.Standard)
            {
                return "��׼";
            }
           else
            {
                throw new Exception();
            }
        }

        public static EAdvLevelType GetEnumType(string typeStr)
        {
            EAdvLevelType retval = EAdvLevelType.Hold;

            if (Equals(EAdvLevelType.Hold, typeStr))
            {
                retval = EAdvLevelType.Hold;
            }
            else if (Equals(EAdvLevelType.Standard, typeStr))
            {
                retval = EAdvLevelType.Standard;
            }
          
            return retval;
        }

        public static bool Equals(EAdvLevelType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EAdvType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAdvLevelType type, bool selected)
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
                listControl.Items.Add(GetListItem(EAdvLevelType.Hold, false));
                listControl.Items.Add(GetListItem(EAdvLevelType.Standard, false));
                
            }
        }
    }
}
