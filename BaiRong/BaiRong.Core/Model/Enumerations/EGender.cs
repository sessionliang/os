using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
	public enum EGender
	{
		NotSet,
		Male,
		Female
	}

	public class EGenderUtils
	{
		public static string GetValue(EGender type)
		{
			if (type == EGender.NotSet)
			{
				return "NotSet";
			}
			else if (type == EGender.Male)
			{
				return "Male";
			}
			else if (type == EGender.Female)
			{
				return "Female";
			}
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EGender type)
		{
			if (type == EGender.NotSet)
			{
				return "δ����";
			}
			else if (type == EGender.Male)
			{
				return "��";
			}
			else if (type == EGender.Female)
			{
				return "Ů";
			}
			else
			{
				throw new Exception();
			}
		}

		public static EGender GetEnumType(string typeStr)
		{
			EGender retval = EGender.NotSet;

			if (Equals(EGender.Male, typeStr))
			{
				retval = EGender.Male;
			}
			else if (Equals(EGender.Female, typeStr))
			{
				retval = EGender.Female;
			}

			return retval;
		}

		public static bool Equals(EGender type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EGender type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EGender type, bool selected)
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
                listControl.Items.Add(GetListItem(EGender.NotSet, false));
                listControl.Items.Add(GetListItem(EGender.Male, false));
                listControl.Items.Add(GetListItem(EGender.Female, false));
            }
        }
	}
}
