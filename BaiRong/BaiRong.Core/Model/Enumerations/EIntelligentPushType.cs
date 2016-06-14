
using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
    /// <summary>
    /// by 20151207 sofuny
    /// 
    /// 智能推送统计类型
    /// </summary>
    public enum EIntelligentPushType
    {
        ALL,//全部
        CurrentYear,//本年内
        OneMonth,//一个月内
        Trimester,//三个月内
        HalfYear,//半年内
        OneYear// 一年内
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
                return "全部";
            }
            else if (type == EIntelligentPushType.CurrentYear)
            {
                return "本年内";
            }
            else if (type == EIntelligentPushType.OneMonth)
            {
                return "一个月内";
            }
            else if (type == EIntelligentPushType.Trimester)
            {
                return "三个月内";
            }
            else if (type == EIntelligentPushType.HalfYear)
            {
                return "半年内";
            }
            else if (type == EIntelligentPushType.OneYear)
            {
                return "一年内";
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
