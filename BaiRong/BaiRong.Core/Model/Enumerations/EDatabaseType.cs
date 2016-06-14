using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using BaiRong.Core;

namespace BaiRong.Model
{
	public enum EDatabaseType
	{
		SqlServer,
        Oracle,
		Unknown
	}

	public class EDatabaseTypeUtils
	{
		public static string GetValue(EDatabaseType type)
		{
			if (type == EDatabaseType.SqlServer)
			{
				return "SqlServer";
			}
            else if (type == EDatabaseType.Oracle)
            {
                return "Oracle";
            }
			else if (type == EDatabaseType.Unknown)
			{
				return "Unknown";
			}
			else
			{
				throw new Exception();
			}
		}

        public static string GetText(EDatabaseType type)
        {
            if (type == EDatabaseType.SqlServer)
            {
                return "Microsoft SQL Server";
            }
            else if (type == EDatabaseType.Oracle)
            {
                return "Oracle";
            }
            else if (type == EDatabaseType.Unknown)
            {
                return "Unknown";
            }
            else
            {
                throw new Exception();
            }
        }

		public static EDatabaseType GetEnumType(string typeStr)
		{
			EDatabaseType retval = EDatabaseType.Unknown;

			if (Equals(EDatabaseType.SqlServer, typeStr))
			{
				retval = EDatabaseType.SqlServer;
            }
            else if (Equals(EDatabaseType.Oracle, typeStr))
            {
                retval = EDatabaseType.Oracle;
            }
			else if (Equals(EDatabaseType.Unknown, typeStr))
			{
				retval = EDatabaseType.Unknown;
			}

			return retval;
		}

		public static bool Equals(EDatabaseType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EDatabaseType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EDatabaseType type, bool selected)
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
                listControl.Items.Add(GetListItem(EDatabaseType.SqlServer, false));
                listControl.Items.Add(GetListItem(EDatabaseType.Oracle, false));
            }
        }

        public static void AddListItemsToInstall(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EDatabaseType.SqlServer, false));
            }
        }
	}
}
