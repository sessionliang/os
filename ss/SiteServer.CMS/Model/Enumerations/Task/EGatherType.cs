using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EGatherType
	{
        Undefined,
        Web,              //Webҳ��
        Database,	      //���ݿ�
        File			  //���ļ�ҳ
	}

    public class EGatherTypeUtils
	{
		public static string GetValue(EGatherType type)
		{
            if (type == EGatherType.Web)
			{
                return "Web";
            }
            else if (type == EGatherType.Database)
            {
                return "Database";
            }
            else if (type == EGatherType.File)
			{
                return "File";
            }
			else
			{
                return "Undefined";
			}
		}

		public static string GetText(EGatherType type)
		{
            if (type == EGatherType.Web)
            {
                return "Webҳ��";
            }
            else if (type == EGatherType.Database)
            {
                return "���ݿ�";
            }
            else if (type == EGatherType.File)
			{
                return "���ļ�ҳ";
            }
			
			else
			{
                return "Undefined";
			}
		}

		public static EGatherType GetEnumType(string typeStr)
		{
            EGatherType retval = EGatherType.Undefined;

            if (Equals(EGatherType.Web, typeStr))
			{
                retval = EGatherType.Web;
            }
            else if (Equals(EGatherType.Database, typeStr))
            {
                retval = EGatherType.Database;
            }
            else if (Equals(EGatherType.File, typeStr))
			{
                retval = EGatherType.File;
            }

			return retval;
		}

		public static bool Equals(EGatherType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

		public static bool Equals(string typeStr, EGatherType type)
		{
			return Equals(type, typeStr);
		}

        public static ListItem GetListItem(EGatherType type, bool selected)
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
                listControl.Items.Add(GetListItem(EGatherType.Web, false));
                listControl.Items.Add(GetListItem(EGatherType.Database, false));
                listControl.Items.Add(GetListItem(EGatherType.File, false));
            }
        }

	}
}
