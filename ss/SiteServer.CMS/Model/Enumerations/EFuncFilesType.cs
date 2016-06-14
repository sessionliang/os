using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EFuncFilesType
    {
        Direct,
        CrossDomain,
        CopyToSite,
        Cors,
    }

    public class EFuncFilesTypeUtils
    {
        public static string GetValue(EFuncFilesType type)
        {
            if (type == EFuncFilesType.Direct)
            {
                return "Direct";
            }
            else if (type == EFuncFilesType.CrossDomain)
            {
                return "CrossDomain";
            }
            else if (type == EFuncFilesType.CopyToSite)
            {
                return "CopyToSite";
            }
            else if (type == EFuncFilesType.Cors)
            {
                return "Cors";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EFuncFilesType type)
        {
            if (type == EFuncFilesType.Direct)
            {
                return "ֱ�ӷ��ʣ��ǿ���";
            }
            else if (type == EFuncFilesType.CrossDomain)
            {
                return "ͨ��������ʣ�����";
            }
            else if (type == EFuncFilesType.CopyToSite)
            {
                return "���Ƶ�վ�ڷ��ʣ�����";
            }
            else if (type == EFuncFilesType.Cors)
            {
                return "ͨ��CORS���ʣ�����";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EFuncFilesType GetEnumType(string typeStr)
        {
            EFuncFilesType retval = EFuncFilesType.Direct;

            if (Equals(EFuncFilesType.Direct, typeStr))
            {
                retval = EFuncFilesType.Direct;
            }
            else if (Equals(EFuncFilesType.CrossDomain, typeStr))
            {
                retval = EFuncFilesType.CrossDomain;
            }
            else if (Equals(EFuncFilesType.CopyToSite, typeStr))
            {
                retval = EFuncFilesType.CopyToSite;
            }
            else if (Equals(EFuncFilesType.Cors, typeStr))
            {
                retval = EFuncFilesType.Cors;
            }
            return retval;
        }

        public static bool Equals(EFuncFilesType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EFuncFilesType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EFuncFilesType type, bool selected)
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
                listControl.Items.Add(GetListItem(EFuncFilesType.Direct, false));
                listControl.Items.Add(GetListItem(EFuncFilesType.CrossDomain, false));
                listControl.Items.Add(GetListItem(EFuncFilesType.CopyToSite, false));
                listControl.Items.Add(GetListItem(EFuncFilesType.Cors, false));
            }
        }

    }
}
