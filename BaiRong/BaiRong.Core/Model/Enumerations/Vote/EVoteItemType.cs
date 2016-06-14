using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
	
	public enum EVoteItemType
	{
		Text,				//������ͶƱ
		Image,				//ͼƬ��ͶƱ
		TextAndImage		//ͼ�Ļ����ͶƱ
	}

	public class EVoteItemTypeUtils
	{
		public static string GetValue(EVoteItemType type)
		{
			if (type == EVoteItemType.Text)
			{
				return "Text";
			}
			else if (type == EVoteItemType.Image)
			{
				return "Image";
			}
			else if (type == EVoteItemType.TextAndImage)
			{
				return "TextAndImage";
			}
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EVoteItemType type)
		{
			if (type == EVoteItemType.Text)
			{
				return "������ͶƱ";
			}
			else if (type == EVoteItemType.Image)
			{
				return "ͼƬ��ͶƱ";
			}
			else if (type == EVoteItemType.TextAndImage)
			{
				return "ͼ�Ļ����ͶƱ";
			}
			else
			{
				throw new Exception();
			}
		}

		public static EVoteItemType GetEnumType(string typeStr)
		{
			EVoteItemType retval = EVoteItemType.Text;

			if (Equals(EVoteItemType.Text, typeStr))
			{
				retval = EVoteItemType.Text;
			}
			else if (Equals(EVoteItemType.Image, typeStr))
			{
				retval = EVoteItemType.Image;
			}
			else if (Equals(EVoteItemType.TextAndImage, typeStr))
			{
				retval = EVoteItemType.TextAndImage;
			}

			return retval;
		}

		public static bool Equals(EVoteItemType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EVoteItemType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EVoteItemType type, bool selected)
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
				listControl.Items.Add(GetListItem(EVoteItemType.Text, false));
				listControl.Items.Add(GetListItem(EVoteItemType.Image, false));
				listControl.Items.Add(GetListItem(EVoteItemType.TextAndImage, false));
			}
		}

	}
}
