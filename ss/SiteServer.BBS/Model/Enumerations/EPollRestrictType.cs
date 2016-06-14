using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.BBS.Model
{
	
	public enum EPollRestrictType
	{
		NoRestrict,				//�����ظ�ͶƱ
		RestrictOneDay,			//һ���ڽ�ֹͬһIP�ظ�ͶƱ
		RestrictOnlyOnce,		//ÿ̨��ֻ��ͶһƱ
        RestrictUser    		//ÿ�û�ֻ��ͶһƱ
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
				return "�����ظ�ͶƱ";
			}
			else if (type == EPollRestrictType.RestrictOneDay)
			{
				return "һ���ڽ�ֹ�ظ�ͶƱ";
			}
			else if (type == EPollRestrictType.RestrictOnlyOnce)
			{
				return "ÿ̨��ֻ��ͶһƱ";
            }
            else if (type == EPollRestrictType.RestrictUser)
            {
                return "ÿ�û�ֻ��ͶһƱ";
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
