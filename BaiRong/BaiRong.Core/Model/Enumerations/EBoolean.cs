using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
	public enum EBoolean
	{
		True,
		False
	}

	public class EBooleanUtils
	{
		private static string GetValue(EBoolean type)
		{
			if (type == EBoolean.True)
			{
				return "True";
			}
			else if (type == EBoolean.False)
			{
				return "False";
			}
			else
			{
				throw new Exception();
			}
		}

        private static string GetText(EBoolean type)
		{
			if (type == EBoolean.True)
			{
				return "��";
			}
			else if (type == EBoolean.False)
			{
				return "��";
			}
			else
			{
				throw new Exception();
			}
		}

		public static EBoolean GetEnumType(string typeStr)
		{
			EBoolean retval = EBoolean.False;

			if (Equals(EBoolean.True, typeStr))
			{
				retval = EBoolean.True;
			}
			else if (Equals(EBoolean.False, typeStr))
			{
				retval = EBoolean.False;
			}

			return retval;
		}

		public static bool Equals(EBoolean type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

		public static bool Equals(string typeStr, EBoolean type)
		{
			return Equals(type, typeStr);
		}

		public static ListItem GetListItem(EBoolean type, bool selected)
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
				listControl.Items.Add(GetListItem(EBoolean.True, false));
				listControl.Items.Add(GetListItem(EBoolean.False, false));
			}
		}

		public static void AddListItems(ListControl listControl, string trueText, string falseText)
		{
			if (listControl != null)
			{
				ListItem item = new ListItem(trueText, GetValue(EBoolean.True));
				listControl.Items.Add(item);
				item = new ListItem(falseText, GetValue(EBoolean.False));
				listControl.Items.Add(item);
			}
		}

	}
}
