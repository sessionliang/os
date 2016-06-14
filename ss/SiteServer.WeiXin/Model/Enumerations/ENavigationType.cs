using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.WeiXin.Model
{
	public enum ENavigationType
	{
        Url,
        Site,
        Function
	}

    public class ENavigationTypeUtils
	{
        public static string GetValue(ENavigationType type)
		{
            if (type == ENavigationType.Url)
            {
                return "Url";
            }
            else if (type == ENavigationType.Site)
            {
                return "Site";
            }
            else if (type == ENavigationType.Function)
            {
                return "Function";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(ENavigationType type)
		{
            if (type == ENavigationType.Url)
            {
                return "ָ����ַ";
            }
            else if (type == ENavigationType.Site)
            {
                return "΢��վҳ��";
            }
            else if (type == ENavigationType.Function)
            {
                return "΢����ҳ��";
            }
			else
			{
				throw new Exception();
			}
		}

		public static ENavigationType GetEnumType(string typeStr)
		{
            ENavigationType retval = ENavigationType.Url;

            if (Equals(ENavigationType.Url, typeStr))
            {
                retval = ENavigationType.Url;
            }
            else if (Equals(ENavigationType.Site, typeStr))
            {
                retval = ENavigationType.Site;
            }
            else if (Equals(ENavigationType.Function, typeStr))
            {
                retval = ENavigationType.Function;
            }

			return retval;
		}

		public static bool Equals(ENavigationType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ENavigationType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ENavigationType type, bool selected)
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
                listControl.Items.Add(GetListItem(ENavigationType.Url, false));
                listControl.Items.Add(GetListItem(ENavigationType.Site, false));
                listControl.Items.Add(GetListItem(ENavigationType.Function, false));
            }
        }
	}
}
