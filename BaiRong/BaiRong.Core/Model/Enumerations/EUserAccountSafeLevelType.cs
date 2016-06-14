using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum EUserAccountSafeLevelType
    {
        Low = 60,
        Middle = 80,
        Heigh = 100
    }

    public class EUserAccountSafeLevelTypeUtils
    {
        public static string GetValue(EUserAccountSafeLevelType type)
        {
            if (type == EUserAccountSafeLevelType.Low)
            {
                return "Low";
            }
            else if (type == EUserAccountSafeLevelType.Middle)
            {
                return "Middle";
            }
            else if (type == EUserAccountSafeLevelType.Heigh)
            {
                return "Heigh";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EUserAccountSafeLevelType type)
        {
            if (type == EUserAccountSafeLevelType.Low)
            {
                return "低";
            }
            else if (type == EUserAccountSafeLevelType.Middle)
            {
                return "中";
            }
            else if (type == EUserAccountSafeLevelType.Heigh)
            {
                return "高";
            }
            else
            {
                throw new Exception();
            }
        }

        public static int GetPercent(EUserAccountSafeLevelType type)
        {
            if (type == EUserAccountSafeLevelType.Low)
            {
                return (int)EUserAccountSafeLevelType.Low;
            }
            else if (type == EUserAccountSafeLevelType.Middle)
            {
                return (int)EUserAccountSafeLevelType.Middle;
            }
            else if (type == EUserAccountSafeLevelType.Heigh)
            {
                return (int)EUserAccountSafeLevelType.Heigh;
            }
            else
            {
                throw new Exception();
            }
        }

        public static EUserAccountSafeLevelType GetSaveLevel(int score)
        {
            if (score <= GetPercent(EUserAccountSafeLevelType.Low))
                return EUserAccountSafeLevelType.Low;
            else if (score <= GetPercent(EUserAccountSafeLevelType.Middle))
                return EUserAccountSafeLevelType.Middle;
            else
                return EUserAccountSafeLevelType.Heigh;
        }


        public static EUserAccountSafeLevelType GetEnumType(string typeStr)
        {
            EUserAccountSafeLevelType retval = EUserAccountSafeLevelType.Low;

            if (Equals(EUserAccountSafeLevelType.Low, typeStr))
            {
                retval = EUserAccountSafeLevelType.Low;
            }
            else if (Equals(EUserAccountSafeLevelType.Middle, typeStr))
            {
                retval = EUserAccountSafeLevelType.Middle;
            }
            else if (Equals(EUserAccountSafeLevelType.Heigh, typeStr))
            {
                retval = EUserAccountSafeLevelType.Heigh;
            }


            return retval;
        }

        public static bool Equals(EUserAccountSafeLevelType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EUserAccountSafeLevelType type)
        {
            return Equals(type, typeStr);
        }

        public static string GetSaveNumLevel(int score)
        {
            if (score <= GetPercent(EUserAccountSafeLevelType.Low))
                return "1";
            else if (score <= GetPercent(EUserAccountSafeLevelType.Middle))
                return "2";
            else
                return "3";
        }
    }
}
