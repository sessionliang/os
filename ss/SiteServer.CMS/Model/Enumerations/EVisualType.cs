using System;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public enum EVisualType
	{
		Static,
		Dynamic,
        Design
	}

	public class EVisualTypeUtils
	{
		public static string GetValue(EVisualType type)
		{
            if (type == EVisualType.Static)
			{
                return "Static";
			}
            else if (type == EVisualType.Dynamic)
			{
                return "Dynamic";
            }
            else if (type == EVisualType.Design)
            {
                return "Design";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EVisualType type)
		{
            if (type == EVisualType.Static)
			{
				return "静态页面";
			}
            else if (type == EVisualType.Dynamic)
			{
                return "动态页面";
            }
            else if (type == EVisualType.Design)
            {
                return "设计模式";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EVisualType GetEnumType(string typeStr)
		{
            EVisualType retval = EVisualType.Static;

            if (Equals(EVisualType.Dynamic, typeStr))
			{
                retval = EVisualType.Dynamic;
            }
            else if (Equals(EVisualType.Design, typeStr))
            {
                retval = EVisualType.Design;
            }
			return retval;
		}

		public static bool Equals(EVisualType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EVisualType type)
        {
            return Equals(type, typeStr);
        }
	}
}
