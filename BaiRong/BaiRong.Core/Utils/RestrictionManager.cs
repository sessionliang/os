using System;
using System.Net;
using System.Web;

using BaiRong.Core.Diagnostics;
using System.Web.UI;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.Core
{
	public sealed class RestrictionManager
	{
        private RestrictionManager()
		{
		}

        public static bool IsVisitAllowed(ERestrictionType restrictionType, StringCollection restrictionBlackList, StringCollection restrictionWhiteList)
        {
            if (restrictionType == ERestrictionType.NoRestriction) return true;
            StringCollection restrictionList = new StringCollection();
            if (restrictionType == ERestrictionType.BlackList)
            {
                restrictionList = restrictionBlackList;
            }
            else if (restrictionType == ERestrictionType.WhiteList)
            {
                restrictionList = restrictionWhiteList;
            }
            return IsVisitAllowed(restrictionType, restrictionList);
        }

        private static bool IsVisitAllowed(ERestrictionType restrictionType, StringCollection restrictionList)
        {
            bool isAllowed = true;
            if (restrictionType != ERestrictionType.NoRestriction)
            {
                string userIP = PageUtils.GetIPAddress();
                if (restrictionType == ERestrictionType.BlackList)
                {
                    IPList list = new IPList();
                    foreach (string restriction in restrictionList)
                    {
                        AddRestrictionToIPList(list, restriction);
                    }
                    if (list.CheckNumber(userIP))
                    {
                        isAllowed = false;
                    }
                }
                else if (restrictionType == ERestrictionType.WhiteList)
                {
                    if (restrictionList.Count > 0)
                    {
                        isAllowed = false;
                        IPList list = new IPList();
                        foreach (string restriction in restrictionList)
                        {
                            AddRestrictionToIPList(list, restriction);
                        }
                        if (list.CheckNumber(userIP))
                        {
                            isAllowed = true;
                        }
                    }
                }
            }
            return isAllowed;
        }

        private static void AddRestrictionToIPList(IPList list, string restriction)
        {
            if (!string.IsNullOrEmpty(restriction))
            {
                if (StringUtils.Contains(restriction, "-"))
                {
                    restriction = restriction.Trim(' ', '-');
                    string[] arr = restriction.Split('-');
                    list.AddRange(arr[0].Trim(), arr[1].Trim());
                }
                else if (StringUtils.Contains(restriction, "*"))
                {
                    string ipPrefix = restriction.Substring(0, restriction.IndexOf('*'));
                    ipPrefix = ipPrefix.Trim(' ', '.');
                    int dotNum = StringUtils.GetCount(".", ipPrefix);

                    string ipNumber = ipPrefix;
                    string mask = "255.255.255.255";
                    if (dotNum == 0)
                    {
                        ipNumber = ipPrefix + ".0.0.0";
                        mask = "255.0.0.0";
                    }
                    else if (dotNum == 1)
                    {
                        ipNumber = ipPrefix + ".0.0";
                        mask = "255.255.0.0";
                    }
                    else
                    {
                        ipNumber = ipPrefix + ".0";
                        mask = "255.255.255.0";
                    }
                    list.Add(ipNumber, mask);
                }
                else
                {
                    list.Add(restriction);
                }
            }
        }
	}
}
