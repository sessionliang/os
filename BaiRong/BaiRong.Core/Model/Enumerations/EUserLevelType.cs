using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum EUserLevelType
    {
        Credits,
        Administrator,          //管理员
        SuperModerator,         //超级版主
        Moderator,              //论坛版主
        WriteForbidden,         //禁止发言
        ReadForbidden,          //禁止访问
        Guest                   //游客
    }

    public class EUserLevelTypeUtils
    {
        public static string GetValue(EUserLevelType type)
        {
            if (type == EUserLevelType.Credits)
            {
                return "Credits";
            }
            else if (type == EUserLevelType.Administrator)
            {
                return "Administrator";
            }
            else if (type == EUserLevelType.SuperModerator)
            {
                return "SuperModerator";
            }
            else if (type == EUserLevelType.Moderator)
            {
                return "Moderator";
            }
            else if (type == EUserLevelType.WriteForbidden)
            {
                return "WriteForbidden";
            }
            else if (type == EUserLevelType.ReadForbidden)
            {
                return "ReadForbidden";
            }
            else if (type == EUserLevelType.Guest)
            {
                return "Guest";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EUserLevelType type)
        {
            if (type == EUserLevelType.Credits)
            {
                return "积分用户组";
            }
            else if (type == EUserLevelType.Administrator)
            {
                return "管理员";
            }
            else if (type == EUserLevelType.SuperModerator)
            {
                return "超级版主";
            }
            else if (type == EUserLevelType.Moderator)
            {
                return "论坛版主";
            }
            else if (type == EUserLevelType.WriteForbidden)
            {
                return "禁止发言";
            }
            else if (type == EUserLevelType.ReadForbidden)
            {
                return "禁止访问";
            }
            else if (type == EUserLevelType.Guest)
            {
                return "游客";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EUserLevelType GetEnumType(string typeStr)
        {
            EUserLevelType retval = EUserLevelType.Credits;

            if (Equals(EUserLevelType.Credits, typeStr))
            {
                retval = EUserLevelType.Credits;
            }
            else if (Equals(EUserLevelType.Administrator, typeStr))
            {
                retval = EUserLevelType.Administrator;
            }
            else if (Equals(EUserLevelType.SuperModerator, typeStr))
            {
                retval = EUserLevelType.SuperModerator;
            }
            else if (Equals(EUserLevelType.Moderator, typeStr))
            {
                retval = EUserLevelType.Moderator;
            }
            else if (Equals(EUserLevelType.WriteForbidden, typeStr))
            {
                retval = EUserLevelType.WriteForbidden;
            }
            else if (Equals(EUserLevelType.ReadForbidden, typeStr))
            {
                retval = EUserLevelType.ReadForbidden;
            }
            else if (Equals(EUserLevelType.Guest, typeStr))
            {
                retval = EUserLevelType.Guest;
            }

            return retval;
        }

        public static bool Equals(EUserLevelType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EUserLevelType type)
        {
            return Equals(type, typeStr);
        }
    }
}
