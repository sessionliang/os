using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	/// <summary>
	/// ���뵼��Ԫ������
	/// </summary>
	public enum EImportExportType
	{
		Template,
		DisplayMode,
		MenuDisplay,
		Vote,
		//DataList,
		//SiteContent,
	}

	public class EImportExportTypeUtils
	{
		public static string GetValue(EImportExportType type)
		{
			if (type == EImportExportType.Template)
			{
				return "Template";
			}
			else if (type == EImportExportType.DisplayMode)
			{
				return "DisplayMode";
			}
			else if (type == EImportExportType.MenuDisplay)
			{
				return "MenuDisplay";
			}
			else if (type == EImportExportType.Vote)
			{
				return "Vote";
			}
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EImportExportType type)
		{
			if (type == EImportExportType.Template)
			{
				return "��վģ��";
			}
			else if (type == EImportExportType.DisplayMode)
			{
				return "�б���ʾ��ʽ";
			}
			else if (type == EImportExportType.MenuDisplay)
			{
				return "�˵���ʾ��ʽ";
			}
			if (type == EImportExportType.Vote)
			{
				return "ͶƱ������";
			}
			else
			{
				throw new Exception();
			}
		}

		public static EImportExportType GetEnumType(string typeStr)
		{
			EImportExportType retval = EImportExportType.Template;

			if (Equals(EImportExportType.Template, typeStr))
			{
				retval = EImportExportType.Template;
			}
			else if (Equals(EImportExportType.DisplayMode, typeStr))
			{
				retval = EImportExportType.DisplayMode;
			}
			else if (Equals(EImportExportType.MenuDisplay, typeStr))
			{
				retval = EImportExportType.MenuDisplay;
			}
			else if (Equals(EImportExportType.Vote, typeStr))
			{
				retval = EImportExportType.Vote;
			}

			return retval;
		}

		public static bool Equals(EImportExportType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EImportExportType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EImportExportType type, bool selected)
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
				listControl.Items.Add(GetListItem(EImportExportType.Template, false));
				listControl.Items.Add(GetListItem(EImportExportType.DisplayMode, false));
				listControl.Items.Add(GetListItem(EImportExportType.MenuDisplay, false));
				listControl.Items.Add(GetListItem(EImportExportType.Vote, false));
			}
		}

	}
}
