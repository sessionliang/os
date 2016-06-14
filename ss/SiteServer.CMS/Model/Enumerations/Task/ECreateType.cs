using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum ECreateType
	{
        Undefined,
        Channel,          //栏目页
        Content,	      //内容页
        File			  //文件页
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
                return "栏目页";
            }
            else if (type == ECreateType.Content)
            {
                return "内容页";
            }
            else if (type == ECreateType.File)
			{
                return "文件页";
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
