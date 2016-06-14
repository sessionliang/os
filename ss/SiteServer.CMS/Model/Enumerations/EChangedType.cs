using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public enum EChangedType
	{
		Add,
		Edit,
        Check,
        Taxis,
        Delete
	}

	public class EChangedTypeUtils
	{
		public static string GetValue(EChangedType type)
		{
            if (type == EChangedType.Add)
			{
                return "Add";
			}
            else if (type == EChangedType.Edit)
			{
                return "Edit";
            }
            else if (type == EChangedType.Check)
            {
                return "Check";
            }
            else if (type == EChangedType.Taxis)
            {
                return "Taxis";
            }
            else if (type == EChangedType.Delete)
            {
                return "Delete";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EChangedType type)
		{
            if (type == EChangedType.Add)
			{
				return "ÐÂÔö";
			}
            else if (type == EChangedType.Edit)
			{
				return "ÐÞ¸Ä";
            }
            else if (type == EChangedType.Check)
            {
                return "ÉóºË";
            }
            else if (type == EChangedType.Taxis)
            {
                return "ÅÅÐò";
            }
            else if (type == EChangedType.Delete)
            {
                return "É¾³ý";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EChangedType GetEnumType(string typeStr)
		{
            EChangedType retval = EChangedType.Edit;

            if (Equals(EChangedType.Add, typeStr))
			{
                retval = EChangedType.Add;
			}
            else if (Equals(EChangedType.Edit, typeStr))
			{
                retval = EChangedType.Edit;
            }
            else if (Equals(EChangedType.Check, typeStr))
            {
                retval = EChangedType.Check;
            }
            else if (Equals(EChangedType.Taxis, typeStr))
            {
                retval = EChangedType.Taxis;
            }
            else if (Equals(EChangedType.Delete, typeStr))
            {
                retval = EChangedType.Delete;
            }
			return retval;
		}

		public static bool Equals(EChangedType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EChangedType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EChangedType type, bool selected)
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
				listControl.Items.Add(GetListItem(EChangedType.Add, false));
                listControl.Items.Add(GetListItem(EChangedType.Edit, false));
                listControl.Items.Add(GetListItem(EChangedType.Check, false));
                listControl.Items.Add(GetListItem(EChangedType.Taxis, false));
                listControl.Items.Add(GetListItem(EChangedType.Delete, false));
			}
		}

	}
}
