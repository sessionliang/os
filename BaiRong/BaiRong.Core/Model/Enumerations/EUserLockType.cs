using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiRong.Model
{
    public enum EUserLockType
    {
        Forever, //永久锁定
        Day,  //天
    }

    public class EUserLockTypeUtils
    {
        public static string GetValue(EUserLockType type)
        {
            if (type == EUserLockType.Forever)
            {
                return "Forever";
            }
            else if (type == EUserLockType.Day)
            {
                return "Day";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EUserLockType GetEnumType(string typeStr)
        {
            if (Equals(typeStr, EUserLockType.Forever))
                return EUserLockType.Forever;
            else if (Equals(typeStr, EUserLockType.Day))
                return EUserLockType.Day;
            else
                return EUserLockType.Forever;
        }

        public static bool Equals(string typeStr, EUserLockType type)
        {
            if (string.IsNullOrEmpty(typeStr))
                return false;
            if (string.Equals(typeStr.ToLower(), GetValue(type).ToLower()))
                return true;
            return false;
        }

        public static bool Equals(EUserLockType type, string typeStr)
        {
            return Equals(typeStr, type);
        }
    }
}
