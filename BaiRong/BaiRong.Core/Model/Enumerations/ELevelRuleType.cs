using BaiRong.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiRong.Model
{
    public enum ELevelRuleType
    {
        CreditNum,
        CashNum

    }

    public class ELevelRuleTypeUtils
    {
        public static string GetLevelRuleTypID(ELevelRuleType type)
        {
            if (type == ELevelRuleType.CreditNum)
            {
                return "CreditNum";
            }
            else if (type == ELevelRuleType.CashNum)
            {
                return "CashNum";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetLevelRuleName(ELevelRuleType type)
        {
            UserConfigInfoExtend additional = UserConfigManager.Additional;

            if (type == ELevelRuleType.CreditNum)
            {
                return additional.CreditNumName;
            }
            else if (type == ELevelRuleType.CashNum)
            {
                return additional.CashNumName;
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetCreditUnit(ELevelRuleType type)
        {
            UserConfigInfoExtend additional = UserConfigManager.Additional;

            if (type == ELevelRuleType.CreditNum)
            {
                return additional.CreditNumUnit;
            }
            else if (type == ELevelRuleType.CashNum)
            {
                return additional.CashNumUnit;
            }
            else
            {
                throw new Exception();
            }
        }

        public static int GetCreditInitial(ELevelRuleType type)
        {
            UserConfigInfoExtend additional = UserConfigManager.Additional;

            if (type == ELevelRuleType.CreditNum)
            {
                return additional.CreditNumInitial;
            }
            else if (type == ELevelRuleType.CashNum)
            {
                return additional.CashNumInitial;
            }
            else
            {
                throw new Exception();
            }
        }

        public static bool GetIsUsing(ELevelRuleType type)
        {            

            if (type == ELevelRuleType.CreditNum)
            {
                return true;
            }
            else if (type == ELevelRuleType.CashNum)
            {
                return true;
            }
            else
            {
                throw new Exception();
            }
        }

        public static bool IsDefaultCredit(ELevelRuleType type)
        {
            if (type == ELevelRuleType.CreditNum || type == ELevelRuleType.CashNum)
            {
                return true;
            }
            return false;
        }

        public static ELevelRuleType GetEnumType(string typeStr)
        {
            ELevelRuleType retval = ELevelRuleType.CreditNum;

            if (Equals(ELevelRuleType.CreditNum, typeStr))
            {
                retval = ELevelRuleType.CreditNum;
            }
            else if (Equals(ELevelRuleType.CashNum, typeStr))
            {
                retval = ELevelRuleType.CashNum;
            }            

            return retval;
        }

        public static bool Equals(ELevelRuleType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetLevelRuleTypID(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ELevelRuleType type)
        {
            return Equals(type, typeStr);
        }
    }
}
