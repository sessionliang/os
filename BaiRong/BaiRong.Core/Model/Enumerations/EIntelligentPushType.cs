
using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
    /// <summary>
    /// by 20151207 sofuny
    /// 
    /// ��������ͳ������
    /// </summary>
    public enum EIntelligentPushType
    {
        ALL,//ȫ��
        CurrentYear,//������
        OneMonth,//һ������
        Trimester,//��������
        HalfYear,//������
        OneYear// һ����
    }

    public class EIntelligentPushTypeUtils
    {
        public static string GetValue(EIntelligentPushType type)
        {
            if (type == EIntelligentPushType.ALL)
            {
                return "ALL";
            }
            else if (type == EIntelligentPushType.CurrentYear)
            {
                return "CurrentYear";
            }
            else if (type == EIntelligentPushType.OneMonth)
            {
                return "OneMonth";
            }
            else if (type == EIntelligentPushType.Trimester)
            {
                return "Trimester";
            }
            else if (type == EIntelligentPushType.HalfYear)
            {
                return "HalfYear";
            }
            else if (type == EIntelligentPushType.OneYear)
            {
                return "OneYear";
            } 
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EIntelligentPushType type)
        {
            if (type == EIntelligentPushType.ALL)
            {
                return "ȫ��";
            }
            else if (type == EIntelligentPushType.CurrentYear)
            {
                return "������";
            }
            else if (type == EIntelligentPushType.OneMonth)
            {
                return "һ������";
            }
            else if (type == EIntelligentPushType.Trimester)
            {
                return "��������";
            }
            else if (type == EIntelligentPushType.HalfYear)
            {
                return "������";
            }
            else if (type == EIntelligentPushType.OneYear)
            {
                return "һ����";
            } 
            else
            {
                throw new Exception();
            }
        }

        public static EIntelligentPushType GetEnumType(string typeStr)
        {
            EIntelligentPushType retval = EIntelligentPushType.ALL;

            if (Equals(EIntelligentPushType.CurrentYear, typeStr))
            {
                retval = EIntelligentPushType.CurrentYear;
            }
            else if (Equals(EIntelligentPushType.OneMonth, typeStr))
            {
                retval = EIntelligentPushType.OneMonth;
            }
            else if (Equals(EIntelligentPushType.Trimester, typeStr))
            {
                retval = EIntelligentPushType.Trimester;
            }
            else if (Equals(EIntelligentPushType.HalfYear, typeStr))
            {
                retval = EIntelligentPushType.HalfYear;
            }
            else if (Equals(EIntelligentPushType.OneYear, typeStr))
            {
                retval = EIntelligentPushType.OneYear;
            } 

            return retval;
        }

        public static bool Equals(EIntelligentPushType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EIntelligentPushType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EIntelligentPushType type, bool selected)
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
                listControl.Items.Add(GetListItem(EIntelligentPushType.ALL, false));
                listControl.Items.Add(GetListItem(EIntelligentPushType.CurrentYear, false));
                listControl.Items.Add(GetListItem(EIntelligentPushType.OneMonth, false));
                listControl.Items.Add(GetListItem(EIntelligentPushType.Trimester, false));
                listControl.Items.Add(GetListItem(EIntelligentPushType.HalfYear, false));
                listControl.Items.Add(GetListItem(EIntelligentPushType.OneYear, false)); 
            }
        }
         
    }
}
