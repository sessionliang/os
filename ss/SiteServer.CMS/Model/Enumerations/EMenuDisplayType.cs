using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	
	public enum EMenuDisplayType
	{
		UseChildrenNodeToDisplay,	//��ʾ����Ŀ������Ŀ
		UseNodeGroupToDisplay,		//��ʾ������Ŀ���е���Ŀ
		Both						//��ʾͬʱ������������������Ŀ
	}

	public class EMenuDisplayTypeUtils
	{
		public static string GetValue(EMenuDisplayType type)
		{
			if (type == EMenuDisplayType.UseChildrenNodeToDisplay)
			{
				return "UseChildrenNodeToDisplay";
			}
			else if (type == EMenuDisplayType.UseNodeGroupToDisplay)
			{
				return "UseNodeGroupToDisplay";
			}
			else if (type == EMenuDisplayType.Both)
			{
				return "Both";
			}
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EMenuDisplayType type)
		{
			if (type == EMenuDisplayType.UseChildrenNodeToDisplay)
			{
				return "��ʾ����Ŀ������Ŀ";
			}
			else if (type == EMenuDisplayType.UseNodeGroupToDisplay)
			{
				return "��ʾ������Ŀ���е���Ŀ";
			}
			else if (type == EMenuDisplayType.Both)
			{
				return "��ʾͬʱ������������������Ŀ";
			}
			else
			{
				throw new Exception();
			}
		}

		public static EMenuDisplayType GetEnumType(string typeStr)
		{
			EMenuDisplayType retval = EMenuDisplayType.UseChildrenNodeToDisplay;

			if (Equals(EMenuDisplayType.UseChildrenNodeToDisplay, typeStr))
			{
				retval = EMenuDisplayType.UseChildrenNodeToDisplay;
			}
			else if (Equals(EMenuDisplayType.UseNodeGroupToDisplay, typeStr))
			{
				retval = EMenuDisplayType.UseNodeGroupToDisplay;
			}
			else if (Equals(EMenuDisplayType.Both, typeStr))
			{
				retval = EMenuDisplayType.Both;
			}

			return retval;
		}

		public static bool Equals(EMenuDisplayType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EMenuDisplayType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EMenuDisplayType type, bool selected)
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
				listControl.Items.Add(GetListItem(EMenuDisplayType.UseChildrenNodeToDisplay, false));
				listControl.Items.Add(GetListItem(EMenuDisplayType.UseNodeGroupToDisplay, false));
				listControl.Items.Add(GetListItem(EMenuDisplayType.Both, false));
			}
		}

	}
}
