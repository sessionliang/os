using System;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public enum EDesignMode
	{
		Edit,
        Preview
	}

	public class EDesignModeUtils
	{
		public static string GetValue(EDesignMode type)
		{
            if (type == EDesignMode.Edit)
			{
                return "Edit";
			}
            else if (type == EDesignMode.Preview)
            {
                return "Preview";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EDesignMode type)
		{
            if (type == EDesignMode.Edit)
			{
				return "�༭ģʽ";
			}
            else if (type == EDesignMode.Preview)
			{
                return "Ԥ��ģʽ";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EDesignMode GetEnumType(string typeStr)
		{
            EDesignMode retval = EDesignMode.Edit;

            if (Equals(EDesignMode.Preview, typeStr))
			{
                retval = EDesignMode.Preview;
            }
			return retval;
		}

		public static bool Equals(EDesignMode type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EDesignMode type)
        {
            return Equals(type, typeStr);
        }
	}
}
