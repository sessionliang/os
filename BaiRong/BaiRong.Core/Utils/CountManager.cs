using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core.Data.Provider;
using BaiRong.Model;

namespace BaiRong.Core
{
	public class CountManager
	{
		private CountManager()
		{
		}

		public static void AddCount(string applicationName, string relatedTableName, string relatedIdentity, ECountType countType)
		{
            if (BaiRongDataProvider.CountDAO.IsExists(applicationName, relatedTableName, relatedIdentity, countType))
			{
				BaiRongDataProvider.CountDAO.AddCountNum(applicationName, relatedTableName, relatedIdentity, countType);
			}
			else
			{
                BaiRongDataProvider.CountDAO.Insert(applicationName, relatedTableName, relatedIdentity, countType, 1);
			}
		}

		public static void DeleteByRelatedTableName(string applicationName, string relatedTableName)
		{
            BaiRongDataProvider.CountDAO.DeleteByRelatedTableName(applicationName, relatedTableName);
		}

		public static void DeleteByIdentity(string applicationName, string relatedTableName, string relatedIdentity)
		{
            BaiRongDataProvider.CountDAO.DeleteByIdentity(applicationName, relatedTableName, relatedIdentity);
		}

        public static int GetCount(string applicationName, string relatedTableName, string relatedIdentity, ECountType countType)
        {
            return BaiRongDataProvider.CountDAO.GetCountNum(applicationName, relatedTableName, relatedIdentity, countType);
        }

        public static int GetCount(string applicationName, string relatedTableName, int publishmentSystemID, ECountType countType)
        {
            return BaiRongDataProvider.CountDAO.GetCountNum(applicationName, relatedTableName, publishmentSystemID, countType);
        }
	}
}
