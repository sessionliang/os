using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
	public enum EDiggType
	{
		Good,
		Bad,
        All
	}

    public class EDiggTypeUtils
	{
		public static string GetValue(EDiggType type)
		{
            if (type == EDiggType.Good)
			{
                return "Good";
			}
            else if (type == EDiggType.Bad)
			{
                return "Bad";
            }
            else if (type == EDiggType.All)
            {
                return "All";
            }
			else
			{
				throw new Exception();
			}
		}

        public static string GetText(EDiggType type)
        {
            if (type == EDiggType.Good)
            {
                return "����ʾ��ͬ";
            }
            else if (type == EDiggType.Bad)
            {
                return "����ʾ����ͬ";
            }
            else if (type == EDiggType.All)
            {
                return "��ʾȫ��";
            }
            else
            {
                throw new Exception();
            }
        }

		public static EDiggType GetEnumType(string typeStr)
		{
            EDiggType retval = EDiggType.All;

            if (Equals(EDiggType.Good, typeStr))
			{
                retval = EDiggType.Good;
			}
            else if (Equals(EDiggType.Bad, typeStr))
			{
                retval = EDiggType.Bad;
			}

			return retval;
		}

		public static bool Equals(EDiggType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EDiggType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EDiggType type, bool selected)
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
                listControl.Items.Add(GetListItem(EDiggType.All, false));
                listControl.Items.Add(GetListItem(EDiggType.Good, false));
                listControl.Items.Add(GetListItem(EDiggType.Bad, false));
            }
        }
	}
}
