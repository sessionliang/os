using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.BBS.Model
{
	
	public enum EPollRestrictType
	{
		NoRestrict,				//允许重复投票
		RestrictOneDay,			//一天内禁止同一IP重复投票
		RestrictOnlyOnce,		//每台机只能投一票
        RestrictUser    		//每用户只能投一票
	}

	public class EPollRestrictTypeUtils
	{
		public static string GetValue(EPollRestrictType type)
		{
			if (type == EPollRestrictType.NoRestrict)
			{
				return "NoRestrict";
			}
			else if (type == EPollRestrictType.RestrictOneDay)
			{
				return "RestrictOneDay";
			}
			else if (type == EPollRestrictType.RestrictOnlyOnce)
			{
				return "RestrictOnlyOnce";
            }
            else if (type == EPollRestrictType.RestrictUser)
            {
                return "RestrictUser";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EPollRestrictType type)
		{
			if (type == EPollRestrictType.NoRestrict)
			{
				return "允许重复投票";
			}
			else if (type == EPollRestrictType.RestrictOneDay)
			{
				return "一天内禁止重复投票";
			}
			else if (type == EPollRestrictType.RestrictOnlyOnce)
			{
				return "每台机只能投一票";
            }
            else if (type == EPollRestrictType.RestrictUser)
            {
                return "每用户只能投一票";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EPollRestrictType GetEnumType(string typeStr)
		{
			EPollRestrictType retval = EPollRestrictType.NoRestrict;

			if (Equals(EPollRestrictType.NoRestrict, typeStr))
			{
				retval = EPollRestrictType.NoRestrict;
			}
			else if (Equals(EPollRestrictType.RestrictOneDay, typeStr))
			{
				retval = EPollRestrictType.RestrictOneDay;
			}
			else if (Equals(EPollRestrictType.RestrictOnlyOnce, typeStr))
			{
				retval = EPollRestrictType.RestrictOnlyOnce;
            }
            else if (Equals(EPollRestrictType.RestrictUser, typeStr))
            {
                retval = EPollRestrictType.RestrictUser;
            }

			return retval;
		}

		public static bool Equals(EPollRestrictType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EPollRestrictType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EPollRestrictType type, bool selected)
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
				listControl.Items.Add(GetListItem(EPollRestrictType.NoRestrict, false));
				listControl.Items.Add(GetListItem(EPollRestrictType.RestrictOneDay, false));
				listControl.Items.Add(GetListItem(EPollRestrictType.RestrictOnlyOnce, false));
                listControl.Items.Add(GetListItem(EPollRestrictType.RestrictUser, false));
			}
		}

	}
}
