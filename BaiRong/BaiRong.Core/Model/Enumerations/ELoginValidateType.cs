using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiRong.Model
{
    public enum ELoginValidateType
    {
        Phone, //手机号登录
        Email,  //邮箱登录
        UserName, //用户名登录
    }

    public class ELoginValidateTypeUtils
    {
        public static string GetValue(ELoginValidateType type)
        {
            if (type == ELoginValidateType.Email)
            {
                return "Email";
            }
            else if (type == ELoginValidateType.Phone)
            {
                return "Phone";
            }
            else if (type == ELoginValidateType.UserName)
            {
                return "UserName";
            }
            else
            {
                throw new Exception();
            }
        }

        public static ELoginValidateType GetEnumType(string typeStr)
        {
            if (Equals(typeStr, ELoginValidateType.Email))
                return ELoginValidateType.Email;
            else if (Equals(typeStr, ELoginValidateType.Phone))
                return ELoginValidateType.Phone;
            else if (Equals(typeStr, ELoginValidateType.UserName))
                return ELoginValidateType.UserName;
            else
                return ELoginValidateType.UserName;
        }

        public static bool Equals(string typeStr, ELoginValidateType type)
        {
            if (string.IsNullOrEmpty(typeStr))
                return false;
            if (string.Equals(typeStr.ToLower(), GetValue(type).ToLower()))
                return true;
            return false;
        }

        public static bool Equals(ELoginValidateType type, string typeStr)
        {
            return Equals(typeStr, type);
        }
    }
}
