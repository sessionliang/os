using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public enum ETranslateContentType
	{
		Copy,				//����
		Cut,				//����
		Reference,           //���õ�ַ
        ReferenceContent,   //��������
	}

	public class ETranslateContentTypeUtils
	{
		public static string GetValue(ETranslateContentType type)
		{
            if (type == ETranslateContentType.Copy)
			{
                return "Copy";
			}
            else if (type == ETranslateContentType.Cut)
			{
                return "Cut";
			}
            else if (type == ETranslateContentType.Reference)
			{
                return "Reference";
			}
            else if (type == ETranslateContentType.ReferenceContent)
            {
                return "ReferenceContent";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(ETranslateContentType type)
		{
            if (type == ETranslateContentType.Copy)
			{
                return "����";
			}
            else if (type == ETranslateContentType.Cut)
			{
                return "����";
			}
            else if (type == ETranslateContentType.Reference)
			{
                return "���õ�ַ";
			}
            else if (type == ETranslateContentType.ReferenceContent)
            {
                return "��������";
            }
			else
			{
				throw new Exception();
			}
		}

		public static ETranslateContentType GetEnumType(string typeStr)
		{
			ETranslateContentType retval = ETranslateContentType.Copy;

            if (Equals(ETranslateContentType.Copy, typeStr))
			{
                retval = ETranslateContentType.Copy;
			}
            else if (Equals(ETranslateContentType.Cut, typeStr))
			{
                retval = ETranslateContentType.Cut;
			}
            else if (Equals(ETranslateContentType.Reference, typeStr))
			{
                retval = ETranslateContentType.Reference;
			}
            else if (Equals(ETranslateContentType.ReferenceContent, typeStr))
            {
                retval = ETranslateContentType.ReferenceContent;
            }

			return retval;
		}

		public static bool Equals(ETranslateContentType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ETranslateContentType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(ETranslateContentType type, bool selected)
		{
			ListItem item = new ListItem(GetText(type), GetValue(type));
			if (selected)
			{
				item.Selected = true;
			}
			return item;
		}

		public static void AddListItems(ListControl listControl, bool isCut)
		{
			if (listControl != null)
			{
				listControl.Items.Add(GetListItem(ETranslateContentType.Copy, false));
                if (isCut)
                {
                    listControl.Items.Add(GetListItem(ETranslateContentType.Cut, false));
                }
                listControl.Items.Add(GetListItem(ETranslateContentType.Reference, false));
                listControl.Items.Add(GetListItem(ETranslateContentType.ReferenceContent, false));
			}
		}
	}
}
