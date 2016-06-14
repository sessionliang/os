using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CRM.Model
{
    public enum EApplicationType
	{
        Document,
        Demo,
        Try,
        GeXia
	}

    public class EApplicationTypeUtils
	{
		public static string GetValue(EApplicationType type)
		{
            if (type == EApplicationType.Document)
			{
                return "Document";
			}
            else if (type == EApplicationType.Demo)
			{
                return "Demo";
            }
            else if (type == EApplicationType.Try)
            {
                return "Try";
            }
            else if (type == EApplicationType.GeXia)
            {
                return "GeXia";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EApplicationType type)
		{
            if (type == EApplicationType.Document)
			{
                return "文档";
			}
            else if (type == EApplicationType.Demo)
			{
                return "产品演示";
            }
            else if (type == EApplicationType.Try)
            {
                return "产品试用";
            }
            else if (type == EApplicationType.GeXia)
            {
                return "阁下微信";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EApplicationType GetEnumType(string typeStr)
		{
            EApplicationType retval = EApplicationType.Document;

            if (Equals(EApplicationType.Document, typeStr))
			{
                retval = EApplicationType.Document;
			}
            else if (Equals(EApplicationType.Demo, typeStr))
			{
                retval = EApplicationType.Demo;
            }
            else if (Equals(EApplicationType.Try, typeStr))
            {
                retval = EApplicationType.Try;
            }
            else if (Equals(EApplicationType.GeXia, typeStr))
            {
                retval = EApplicationType.GeXia;
            }
			return retval;
		}

		public static bool Equals(EApplicationType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EApplicationType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EApplicationType type, bool selected)
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
                listControl.Items.Add(GetListItem(EApplicationType.Document, false));
                listControl.Items.Add(GetListItem(EApplicationType.Demo, false));
                listControl.Items.Add(GetListItem(EApplicationType.Try, false));
                listControl.Items.Add(GetListItem(EApplicationType.GeXia, false));
            }
        }
	}
}
