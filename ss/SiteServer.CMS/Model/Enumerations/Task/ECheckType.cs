using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EAfterCheckType
    {
        Undefined,
        None,                    //不处理
        Publish,                 //审核之后，生成，发布
    }

    public class EAfterCheckTypeUtils
    {
        public static string GetValue(EAfterCheckType type)
        {
            if (type == EAfterCheckType.None)
            {
                return "None";
            }
            else if (type == EAfterCheckType.Publish)
            {
                return "Publish";
            }
            else
            {
                return "Undefined";
            }
        }

        public static string GetText(EAfterCheckType type)
        {
            if (type == EAfterCheckType.None)
            {
                return "只进行审核";
            }
            else if (type == EAfterCheckType.Publish)
            {
                return "审核之后，先生成，后发布";
            }

            else
            {
                return "Undefined";
            }
        }

        public static EAfterCheckType GetEnumType(string typeStr)
        {
            EAfterCheckType retval = EAfterCheckType.Undefined;

            if (Equals(EAfterCheckType.None, typeStr))
            {
                retval = EAfterCheckType.None;
            }
            else if (Equals(EAfterCheckType.Publish, typeStr))
            {
                retval = EAfterCheckType.Publish;
            }

            return retval;
        }

        public static bool Equals(EAfterCheckType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EAfterCheckType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAfterCheckType type, bool selected)
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
                listControl.Items.Add(GetListItem(EAfterCheckType.None, false));
                listControl.Items.Add(GetListItem(EAfterCheckType.Publish, false));
            }
        }

    }
}
