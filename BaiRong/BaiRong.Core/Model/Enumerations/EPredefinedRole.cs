using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
	public enum EPredefinedRole
	{
        ConsoleAdministrator,				//超级管理员
		SystemAdministrator,				//站点总管理员
        Administrator,						//管理员
	}

	public class EPredefinedRoleUtils
	{
		public static string GetValue(EPredefinedRole type)
		{
			if (type == EPredefinedRole.ConsoleAdministrator)
			{
				return "ConsoleAdministrator";
			}
            else if (type == EPredefinedRole.SystemAdministrator)
			{
				return "SystemAdministrator";
			}
			else if (type == EPredefinedRole.Administrator)
			{
				return "Administrator";
            }
			else
			{
				return string.Empty;
			}
		}

		public static string GetText(EPredefinedRole type)
		{
			if (type == EPredefinedRole.ConsoleAdministrator)
			{
				return "超级管理员";
			}
            else if (type == EPredefinedRole.SystemAdministrator)
			{
                return "站点总管理员";
			}
			else if (type == EPredefinedRole.Administrator)
			{
				return "管理员";
            }
			else
			{
				return string.Empty;
			}
		}

		public static bool IsPredefinedRole(string roleName)
		{
			bool retval = false;
			if (Equals(EPredefinedRole.ConsoleAdministrator, roleName))
			{
				retval = true;
			}
            else if (Equals(EPredefinedRole.SystemAdministrator, roleName))
			{
				retval = true;
			}
			else if (Equals(EPredefinedRole.Administrator, roleName))
			{
				retval = true;
            }

			return retval;
		}

        public static EPredefinedRole GetEnumType(string typeStr)
        {
            EPredefinedRole retval = EPredefinedRole.Administrator;

            if (Equals(EPredefinedRole.ConsoleAdministrator, typeStr))
            {
                retval = EPredefinedRole.ConsoleAdministrator;
            }
            else if (Equals(EPredefinedRole.SystemAdministrator, typeStr))
            {
                retval = EPredefinedRole.SystemAdministrator;
            }

            return retval;
        }

		public static EPredefinedRole GetEnumTypeByRoles(string[] roles)
		{
			bool isConsoleAdministrator = false;
			bool isSystemAdministrator = false;

			if (roles != null && roles.Length > 0)
			{
				foreach (string role in roles)
				{
					if (Equals(EPredefinedRole.ConsoleAdministrator, role))
					{
						isConsoleAdministrator = true;
					}
                    else if (Equals(EPredefinedRole.SystemAdministrator, role))
					{
						isSystemAdministrator = true;
					}
				}
			}
			if (isConsoleAdministrator) return EPredefinedRole.ConsoleAdministrator;
            if (isSystemAdministrator) return EPredefinedRole.SystemAdministrator;
            return EPredefinedRole.Administrator;
		}

		public static bool Equals(EPredefinedRole type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EPredefinedRole type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EPredefinedRole type, bool selected)
		{
			ListItem item = new ListItem(GetText(type), GetValue(type));
			if (selected)
			{
				item.Selected = true;
			}
			return item;
		}

		public static bool IsConsoleAdministrator(string[] roles)
		{
			bool retval = false;
			if (roles != null && roles.Length > 0)
			{
				foreach (string role in roles)
				{
					if (Equals(EPredefinedRole.ConsoleAdministrator, role))
					{
						retval = true;
						break;
					}
				}
			}
			return retval;
		}

		public static bool IsSystemAdministrator(string[] roles)
		{
			bool retval = false;
			if (roles != null && roles.Length > 0)
			{
				foreach (string role in roles)
				{
					if (Equals(EPredefinedRole.ConsoleAdministrator, role))
					{
						retval = true;
						break;
					}
                    else if (Equals(EPredefinedRole.SystemAdministrator, role))
					{
						retval = true;
						break;
					}
				}
			}
			return retval;
		}

        public static bool IsAdministrator(string[] roles)
        {
            bool retval = false;
            if (roles != null && roles.Length > 0)
            {
                foreach (string role in roles)
                {
                    if (Equals(EPredefinedRole.ConsoleAdministrator, role))
                    {
                        retval = true;
                        break;
                    }
                    else if (Equals(EPredefinedRole.SystemAdministrator, role))
                    {
                        retval = true;
                        break;
                    }
                    else if(Equals(EPredefinedRole.Administrator,role))
                    {
                        retval = true;
                        break;
                    }
                }
            }
            return retval;
        }

		public static ArrayList GetAllPredefinedRole()
		{
			ArrayList arraylist = new ArrayList();
			arraylist.Add(EPredefinedRole.Administrator);
			arraylist.Add(EPredefinedRole.ConsoleAdministrator);
            arraylist.Add(EPredefinedRole.SystemAdministrator);

			return arraylist;
		}

	}
}
