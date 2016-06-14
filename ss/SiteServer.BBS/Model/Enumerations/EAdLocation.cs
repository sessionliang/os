using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace SiteServer.BBS.Model
{
    public enum EAdLocation
    {
        NavBanner,          //����ͨ��
    }

    public class EAdLocationUtils
    {
        public static string GetValue(EAdLocation type)
        {
            if (type == EAdLocation.NavBanner)
            {
                return "NavBanner";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EAdLocation type)
        {
            if (type == EAdLocation.NavBanner)
            {
                return "����ͨ��";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetDescription(EAdLocation type)
        {
            if (type == EAdLocation.NavBanner)
            {
                return "��ʾ�������������棬һ����ͼƬ��flash��ʽ��ʾ���������ʱϵͳ�����ѡȡһ����ʾ";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EAdLocation GetEnumType(string typeStr)
        {
            EAdLocation retval = EAdLocation.NavBanner;

            if (Equals(EAdLocation.NavBanner, typeStr))
            {
                retval = EAdLocation.NavBanner;
            }

            return retval;
        }

        public static bool Equals(EAdLocation type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EAdLocation type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAdLocation type, bool selected)
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
                listControl.Items.Add(GetListItem(EAdLocation.NavBanner, false));
            }
        }

        public static ArrayList GetArrayList()
        {
            ArrayList arraylist = new ArrayList();
            arraylist.Add(EAdLocation.NavBanner);

            return arraylist;
        }
    }
}
