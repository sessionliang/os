using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model.Service
{
	public enum EStorageType
	{
		Ftp,
		Local
	}

	public class EStorageTypeUtils
	{
		public static string GetValue(EStorageType type)
		{
            if (type == EStorageType.Ftp)
			{
                return "Ftp";
			}
            else if (type == EStorageType.Local)
			{
                return "Local";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EStorageType type)
		{
            if (type == EStorageType.Ftp)
			{
                return "远程FTP空间";
			}
            else if (type == EStorageType.Local)
			{
                return "本地空间";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EStorageType GetEnumType(string typeStr)
		{
			EStorageType retval = EStorageType.Local;

            if (Equals(EStorageType.Ftp, typeStr))
			{
                retval = EStorageType.Ftp;
			}
            else if (Equals(EStorageType.Local, typeStr))
			{
                retval = EStorageType.Local;
            }

			return retval;
		}

		public static bool Equals(EStorageType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EStorageType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EStorageType type, bool selected)
		{
			ListItem item = new ListItem(GetText(type), GetValue(type));
			if (selected)
			{
				item.Selected = true;
			}
			return item;
		}

		public static void AddListItems(ListControl listControl)
		{
			if (listControl != null)
			{
                listControl.Items.Add(GetListItem(EStorageType.Ftp, false));
                listControl.Items.Add(GetListItem(EStorageType.Local, false));
			}
		}

	}
}
