using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Collections.Generic;

namespace BaiRong.Model
{
    public enum ESiteserverThirdLoginType
    {
        QQ,
        Weibo,
        WeixinPC,
        WeixinMob,
    }

    public class ESiteserverThirdLoginTypeUtils
    {
        public static string GetValue(ESiteserverThirdLoginType type)
        {
            if (type == ESiteserverThirdLoginType.Weibo)
            {
                return "Weibo";
            }
            else if (type == ESiteserverThirdLoginType.QQ)
            {
                return "QQ";
            }
            else if (type == ESiteserverThirdLoginType.WeixinPC)
            {
                return "WeixinPC";
            }
            else if (type == ESiteserverThirdLoginType.WeixinMob)
            {
                return "WeixinMob";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(ESiteserverThirdLoginType type)
        {
            if (type == ESiteserverThirdLoginType.Weibo)
            {
                return "–¬¿ÀŒ¢≤©";
            }
            else if (type == ESiteserverThirdLoginType.QQ)
            {
                return "QQ’À∫≈";
            }
            else if (type == ESiteserverThirdLoginType.WeixinPC)
            {
                return "Œ¢–≈’À∫≈";
            }
            else if (type == ESiteserverThirdLoginType.WeixinMob)
            {
                return "Œ¢–≈’À∫≈";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetDescription(ESiteserverThirdLoginType type)
        {
            if (type == ESiteserverThirdLoginType.Weibo)
            {
                return "–¬¿ÀŒ¢≤©";
            }
            else if (type == ESiteserverThirdLoginType.QQ)
            {
                return "QQ’À∫≈";
            }
            else if (type == ESiteserverThirdLoginType.WeixinPC)
            {
                return "Œ¢–≈’À∫≈";
            }
            else if (type == ESiteserverThirdLoginType.WeixinMob)
            {
                return "Œ¢–≈’À∫≈";
            }
            else
            {
                throw new Exception();
            }
        }

        public static ESiteserverThirdLoginType GetEnumType(string typeStr)
        {
            ESiteserverThirdLoginType retval = ESiteserverThirdLoginType.Weibo;

            if (Equals(ESiteserverThirdLoginType.Weibo, typeStr))
            {
                retval = ESiteserverThirdLoginType.Weibo;
            }
            else if (Equals(ESiteserverThirdLoginType.QQ, typeStr))
            {
                retval = ESiteserverThirdLoginType.QQ;
            }
            else if (Equals(ESiteserverThirdLoginType.WeixinPC, typeStr))
            {
                retval = ESiteserverThirdLoginType.WeixinPC;
            }
            else if (Equals(ESiteserverThirdLoginType.WeixinMob, typeStr))
            {
                retval = ESiteserverThirdLoginType.WeixinMob;
            }
            return retval;
        }

        public static bool Equals(ESiteserverThirdLoginType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ESiteserverThirdLoginType type)
        {
            return Equals(type, typeStr);
        }

        public static List<ESiteserverThirdLoginType> GetESiteserverThirdLoginTypeList()
        {
            List<ESiteserverThirdLoginType> list = new List<ESiteserverThirdLoginType>();
            list.Add(ESiteserverThirdLoginType.QQ);
            list.Add(ESiteserverThirdLoginType.Weibo);
            list.Add(ESiteserverThirdLoginType.WeixinPC);
            return list;
        }

        public static ListItem GetListItem(ESiteserverThirdLoginType type, bool selected)
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
                listControl.Items.Add(GetListItem(ESiteserverThirdLoginType.Weibo, false));
                listControl.Items.Add(GetListItem(ESiteserverThirdLoginType.QQ, false));
                listControl.Items.Add(GetListItem(ESiteserverThirdLoginType.WeixinPC, false));
                listControl.Items.Add(GetListItem(ESiteserverThirdLoginType.WeixinMob, false));
            }
        }
    }
}
