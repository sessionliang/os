using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.Model
{
    public enum ECreditType
    {
        Prestige,           //ÍþÍû
        Contribution,       //¹±Ï×
        Currency,           //½ðÇ®
        ExtCredit1,
        ExtCredit2,
        ExtCredit3
    }

    public class ECreditTypeUtils
    {
        public static string GetCreditID(ECreditType type)
        {
            if (type == ECreditType.Prestige)
            {
                return "Prestige";
            }
            else if (type == ECreditType.Contribution)
            {
                return "Contribution";
            }
            else if (type == ECreditType.Currency)
            {
                return "Currency";
            }
            else if (type == ECreditType.ExtCredit1)
            {
                return "ExtCredit1";
            }
            else if (type == ECreditType.ExtCredit2)
            {
                return "ExtCredit2";
            }
            else if (type == ECreditType.ExtCredit3)
            {
                return "ExtCredit3";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetCreditName(int publishmentSystemID, ECreditType type)
        {
            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            if (type == ECreditType.Prestige)
            {
                return additional.CreditNamePrestige;
            }
            else if (type == ECreditType.Contribution)
            {
                return additional.CreditNameContribution;
            }
            else if (type == ECreditType.Currency)
            {
                return additional.CreditNameCurrency;
            }
            else if (type == ECreditType.ExtCredit1)
            {
                return additional.CreditNameExtCredit1;
            }
            else if (type == ECreditType.ExtCredit2)
            {
                return additional.CreditNameExtCredit2;
            }
            else if (type == ECreditType.ExtCredit3)
            {
                return additional.CreditNameExtCredit3;
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetCreditUnit(int publishmentSystemID, ECreditType type)
        {
            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            if (type == ECreditType.Prestige)
            {
                return additional.CreditUnitPrestige;
            }
            else if (type == ECreditType.Contribution)
            {
                return additional.CreditUnitContribution;
            }
            else if (type == ECreditType.Currency)
            {
                return additional.CreditUnitCurrency;
            }
            else if (type == ECreditType.ExtCredit1)
            {
                return additional.CreditUnitExtCredit1;
            }
            else if (type == ECreditType.ExtCredit2)
            {
                return additional.CreditUnitExtCredit2;
            }
            else if (type == ECreditType.ExtCredit3)
            {
                return additional.CreditUnitExtCredit3;
            }
            else
            {
                throw new Exception();
            }
        }

        public static int GetCreditInitial(int publishmentSystemID, ECreditType type)
        {
            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            if (type == ECreditType.Prestige)
            {
                return additional.CreditInitialPrestige;
            }
            else if (type == ECreditType.Contribution)
            {
                return additional.CreditInitialContribution;
            }
            else if (type == ECreditType.Currency)
            {
                return additional.CreditInitialCurrency;
            }
            else if (type == ECreditType.ExtCredit1)
            {
                return additional.CreditInitialExtCredit1;
            }
            else if (type == ECreditType.ExtCredit2)
            {
                return additional.CreditInitialExtCredit2;
            }
            else if (type == ECreditType.ExtCredit3)
            {
                return additional.CreditInitialExtCredit3;
            }
            else
            {
                throw new Exception();
            }
        }

        public static bool GetIsUsing(int publishmentSystemID, ECreditType type)
        {
            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            if (type == ECreditType.Prestige)
            {
                return true;
            }
            else if (type == ECreditType.Contribution)
            {
                return true;
            }
            else if (type == ECreditType.Currency)
            {
                return true;
            }
            else if (type == ECreditType.ExtCredit1)
            {
                return additional.CreditUsingExtCredit1;
            }
            else if (type == ECreditType.ExtCredit2)
            {
                return additional.CreditUsingExtCredit2;
            }
            else if (type == ECreditType.ExtCredit3)
            {
                return additional.CreditUsingExtCredit3;
            }
            else
            {
                throw new Exception();
            }
        }

        public static bool IsDefaultCredit(ECreditType type)
        {
            if (type == ECreditType.Prestige || type == ECreditType.Contribution || type == ECreditType.Currency)
            {
                return true;
            }
            return false;
        }

        public static ECreditType GetEnumType(string typeStr)
        {
            ECreditType retval = ECreditType.Prestige;

            if (Equals(ECreditType.Prestige, typeStr))
            {
                retval = ECreditType.Prestige;
            }
            else if (Equals(ECreditType.Contribution, typeStr))
            {
                retval = ECreditType.Contribution;
            }
            else if (Equals(ECreditType.Currency, typeStr))
            {
                retval = ECreditType.Currency;
            }
            else if (Equals(ECreditType.ExtCredit1, typeStr))
            {
                retval = ECreditType.ExtCredit1;
            }
            else if (Equals(ECreditType.ExtCredit2, typeStr))
            {
                retval = ECreditType.ExtCredit2;
            }
            else if (Equals(ECreditType.ExtCredit3, typeStr))
            {
                retval = ECreditType.ExtCredit3;
            }

            return retval;
        }

        public static bool Equals(ECreditType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetCreditID(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ECreditType type)
        {
            return Equals(type, typeStr);
        }
    }
}
