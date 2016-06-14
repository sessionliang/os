using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    /// <summary>
    /// by 20151030 sofuny ��Ϣ����������������
    /// </summary>
	public enum ESubscribePushDateType
    {
        Week,         //ÿ��
        Month      //ÿ��
	}

    public class ESubscribePushDateTypeUtils
    {
        public static string GetValue(ESubscribePushDateType type)
        {
            if (type == ESubscribePushDateType.Week)
            {
                return "Week";
            }
            else if (type == ESubscribePushDateType.Month)
            {
                return "Month";
            } 
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(ESubscribePushDateType type)
        {
            if (type == ESubscribePushDateType.Week)
            {
                return "ÿ��";
            }
            else if (type == ESubscribePushDateType.Month)
            {
                return "ÿ��";
            } 
            else
            {
                throw new Exception();
            }
        }

        public static ESubscribePushDateType GetEnumType(string typeStr)
        {
            ESubscribePushDateType retval = ESubscribePushDateType.Month;

            if (Equals(ESubscribePushDateType.Week, typeStr))
            {
                retval = ESubscribePushDateType.Week;
            }
            else if (Equals(ESubscribePushDateType.Month, typeStr))
            {
                retval = ESubscribePushDateType.Month;
            } 

            return retval;
        }

        public static bool Equals(ESubscribePushDateType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ESubscribePushDateType type)
        {
            return Equals(type, typeStr);
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                ListItem item = new ListItem(GetText(ESubscribePushDateType.Week), GetValue(ESubscribePushDateType.Week));
                listControl.Items.Add(item);
                item = new ListItem(GetText(ESubscribePushDateType.Month), GetValue(ESubscribePushDateType.Month));
                listControl.Items.Add(item);
            }
        }
    }
}
