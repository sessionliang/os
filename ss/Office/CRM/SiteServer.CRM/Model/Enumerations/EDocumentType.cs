using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CRM.Model
{
    public enum EDocumentType
	{
        Category,           //分类文档
        Contract,           //合同文档
	}

    public class EDocumentTypeUtils
	{
		public static string GetValue(EDocumentType type)
		{
            if (type == EDocumentType.Category)
			{
                return "Category";
			}
            else if (type == EDocumentType.Contract)
            {
                return "Contract";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EDocumentType type)
		{
            if (type == EDocumentType.Category)
			{
                return "分类文档";
			}
            else if (type == EDocumentType.Contract)
            {
                return "合同文档";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EDocumentType GetEnumType(string typeStr)
		{
            EDocumentType retval = EDocumentType.Category;

            if (Equals(EDocumentType.Category, typeStr))
			{
                retval = EDocumentType.Category;
			}
            else if (Equals(EDocumentType.Contract, typeStr))
            {
                retval = EDocumentType.Contract;
            }
			return retval;
		}

		public static bool Equals(EDocumentType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EDocumentType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EDocumentType type, bool selected)
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
                listControl.Items.Add(GetListItem(EDocumentType.Category, false));
                listControl.Items.Add(GetListItem(EDocumentType.Contract, false));
            }
        }
	}
}
