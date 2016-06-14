using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum ECreateType
	{
        Undefined,
        Channel,          //��Ŀҳ
        Content,	      //����ҳ
        File			  //�ļ�ҳ
	}

    public class ECreateTypeUtils
	{
		public static string GetValue(ECreateType type)
		{
            if (type == ECreateType.Channel)
			{
                return "Channel";
            }
            else if (type == ECreateType.Content)
            {
                return "Content";
            }
            else if (type == ECreateType.File)
			{
                return "File";
            }
			else
			{
                return "Undefined";
			}
		}

		public static string GetText(ECreateType type)
		{
            if (type == ECreateType.Channel)
            {
                return "��Ŀҳ";
            }
            else if (type == ECreateType.Content)
            {
                return "����ҳ";
            }
            else if (type == ECreateType.File)
			{
                return "�ļ�ҳ";
            }
			
			else
			{
                return "Undefined";
			}
		}

		public static ECreateType GetEnumType(string typeStr)
		{
            ECreateType retval = ECreateType.Undefined;

            if (Equals(ECreateType.Channel, typeStr))
			{
                retval = ECreateType.Channel;
            }
            else if (Equals(ECreateType.Content, typeStr))
            {
                retval = ECreateType.Content;
            }
            else if (Equals(ECreateType.File, typeStr))
			{
                retval = ECreateType.File;
            }

			return retval;
		}

		public static bool Equals(ECreateType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

		public static bool Equals(string typeStr, ECreateType type)
		{
			return Equals(type, typeStr);
		}

        public static ListItem GetListItem(ECreateType type, bool selected)
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
                listControl.Items.Add(GetListItem(ECreateType.Channel, false));
                listControl.Items.Add(GetListItem(ECreateType.Content, false));
                listControl.Items.Add(GetListItem(ECreateType.File, false));
            }
        }

	}
}
