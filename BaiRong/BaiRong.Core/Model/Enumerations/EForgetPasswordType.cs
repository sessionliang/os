using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiRong.Model
{
    public enum EForgetPasswordType
    {
        Email,            //邮箱
        Phone,            //手机号
        SecretQuestion,   //密保问题
    }

    public static class EForgetPasswordTypeUtils
    {
        public static string GetValue(EForgetPasswordType type)
        {
            if (type == EForgetPasswordType.Email)
                return "Email";
            else if (type == EForgetPasswordType.Phone)
                return "Phone";
            else if (type == EForgetPasswordType.SecretQuestion)
                return "SecretQuestion";
            else
                throw new Exception();
        }

        public static EForgetPasswordType GetEnumType(string typeStr)
        {
            if (Equals(typeStr, EForgetPasswordType.Email))
            {
                return EForgetPasswordType.Email;
            }
            else if (Equals(typeStr, EForgetPasswordType.Phone))
            {
                return EForgetPasswordType.Phone;
            }
            else
            {
                return EForgetPasswordType.SecretQuestion;
            }
        }

        public static bool Equals(string typeStr, EForgetPasswordType type)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(typeStr.ToLower(), GetValue(type).ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(EForgetPasswordType type, string typeStr)
        {
            return Equals(typeStr, type);
        }
    }
}
