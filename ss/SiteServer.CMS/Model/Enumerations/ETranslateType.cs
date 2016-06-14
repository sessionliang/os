using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	/// <summary>
	/// ����ת������
	/// </summary>
	public enum ETranslateType
	{
		Content,				//��ת������
		Channel,				//��ת����Ŀ
		All						//ת����Ŀ������
	}

	public class ETranslateTypeUtils
	{
		public static string GetValue(ETranslateType type)
		{
			if (type == ETranslateType.Content)
			{
				return "Content";
			}
			else if (type == ETranslateType.Channel)
			{
				return "Channel";
			}
			else if (type == ETranslateType.All)
			{
				return "All";
			}
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(ETranslateType type)
		{
			if (type == ETranslateType.Content)
			{
				return "��ת������";
			}
			else if (type == ETranslateType.Channel)
			{
				return "��ת����Ŀ";
			}
			else if (type == ETranslateType.All)
			{
				return "ת����Ŀ������";
			}
			else
			{
				throw new Exception();
			}
		}

		public static ETranslateType GetEnumType(string typeStr)
		{
			ETranslateType retval = ETranslateType.Content;

			if (Equals(ETranslateType.Content, typeStr))
			{
				retval = ETranslateType.Content;
			}
			else if (Equals(ETranslateType.Channel, typeStr))
			{
				retval = ETranslateType.Channel;
			}
			else if (Equals(ETranslateType.All, typeStr))
			{
				retval = ETranslateType.All;
			}

			return retval;
		}

		public static bool Equals(ETranslateType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ETranslateType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(ETranslateType type, bool selected)
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
				listControl.Items.Add(GetListItem(ETranslateType.Content, false));
				listControl.Items.Add(GetListItem(ETranslateType.Channel, false));
				listControl.Items.Add(GetListItem(ETranslateType.All, false));
			}
		}

	}
}
