using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiRong.Model
{
    public enum EUserActionType
    {
        Login, //登录
        Logout,  //登出
        UpdateInfo, //修改用户信息
        UpdatePassword,//修改密码
    }

    public class EUserActionTypeUtils
    {
        public static string GetValue(EUserActionType type)
        {
            if (type == EUserActionType.Login)
            {
                return "Login";
            }
            else if (type == EUserActionType.Logout)
            {
                return "Logout";
            }
            else if (type == EUserActionType.UpdateInfo)
            {
                return "UpdateInfo";
            }
            else if (type == EUserActionType.UpdatePassword)
            {
                return "UpdatePassword";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EUserActionType type)
        {
            if (type == EUserActionType.Login)
            {
                return "用户登录";
            }
            else if (type == EUserActionType.Logout)
            {
                return "用户登出";
            }
            else if (type == EUserActionType.UpdateInfo)
            {
                return "用户修改信息";
            }
            else if (type == EUserActionType.UpdatePassword)
            {
                return "用户修改密码";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EUserActionType GetEnumType(string typeStr)
        {
            if (Equals(typeStr, EUserActionType.Login))
                return EUserActionType.Login;
            else if (Equals(typeStr, EUserActionType.Logout))
                return EUserActionType.Logout;
            else if (Equals(typeStr, EUserActionType.UpdateInfo))
                return EUserActionType.UpdateInfo;
            else if (Equals(typeStr, EUserActionType.UpdatePassword))
                return EUserActionType.UpdatePassword;
            else
                throw new Exception();
        }

        public static bool Equals(string typeStr, EUserActionType type)
        {
            if (string.IsNullOrEmpty(typeStr))
                return false;
            if (string.Equals(typeStr.ToLower(), GetValue(type).ToLower()))
                return true;
            return false;
        }

        public static bool Equals(EUserActionType type, string typeStr)
        {
            return Equals(typeStr, type);
        }
    }
}
