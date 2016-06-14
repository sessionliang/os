using System;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	
	public enum ETrackerType
	{
        Site,					//����Ӧ�õķÿ�����
        Page,					//����ҳ��ķ�����
	}

	public class ETrackerTypeUtils
	{
		public static string GetValue(ETrackerType type)
		{
            if (type == ETrackerType.Site)
			{
                return "Site";
			}
            else if (type == ETrackerType.Page)
			{
                return "Page";
            }
			else
			{
				throw new Exception();
			}
		}

		public static ETrackerType GetEnumType(string typeStr)
		{
            ETrackerType retval = ETrackerType.Page;

            if (Equals(ETrackerType.Site, typeStr))
			{
                retval = ETrackerType.Site;
			}
            else if (Equals(ETrackerType.Page, typeStr))
			{
                retval = ETrackerType.Page;
            }

			return retval;
		}

		public static bool Equals(ETrackerType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ETrackerType type)
        {
            return Equals(type, typeStr);
        }

	}
}
