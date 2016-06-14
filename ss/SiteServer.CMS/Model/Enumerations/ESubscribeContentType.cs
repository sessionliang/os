using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    /// <summary>
    /// by 20151030 sofuny ��Ϣ������������
    /// </summary>
	public enum ESubscribeContentType
    {
        Label,         //���ݱ�ǩ
        Column      //��վ��Ŀ 
	}

    public class ESubscribeContentTypeUtils
    {
        public static string GetValue(ESubscribeContentType type)
        {
            if (type == ESubscribeContentType.Label)
            {
                return "Label";
            }
            else if (type == ESubscribeContentType.Column)
            {
                return "Column";
            } 
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(ESubscribeContentType type)
        {
            if (type == ESubscribeContentType.Label)
            {
                return "���ݱ�ǩ";
            }
            else if (type == ESubscribeContentType.Column)
            {
                return "��վ��Ŀ";
            } 
            else
            {
                throw new Exception();
            }
        }

        public static ESubscribeContentType GetEnumType(string typeStr)
        {
            ESubscribeContentType retval = ESubscribeContentType.Label;

            if (Equals(ESubscribeContentType.Label, typeStr))
            {
                retval = ESubscribeContentType.Label;
            }
            else if (Equals(ESubscribeContentType.Column, typeStr))
            {
                retval = ESubscribeContentType.Column;
            } 

            return retval;
        }

        public static bool Equals(ESubscribeContentType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ESubscribeContentType type)
        {
            return Equals(type, typeStr);
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                ListItem item = new ListItem(GetText(ESubscribeContentType.Label), GetValue(ESubscribeContentType.Label));
                listControl.Items.Add(item);
                item = new ListItem(GetText(ESubscribeContentType.Column), GetValue(ESubscribeContentType.Column));
                listControl.Items.Add(item);
            }
        }
    }
}
