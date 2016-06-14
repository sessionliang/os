using System.Web.UI.WebControls;
using BaiRong.Core;
using System;
using System.Collections;

namespace SiteServer.CMS.Model
{
    public enum EPublishType
	{
        Site,
        Image,
        Video,
        File
	}

    public class EPublishTypeUtils
	{
		public static string GetValue(EPublishType type)
		{
            if (type == EPublishType.Site)
			{
                return "Site";
            }
            else if (type == EPublishType.Image)
            {
                return "Image";
            }
            else if (type == EPublishType.Video)
			{
                return "Video";
            }
            else if (type == EPublishType.File)
            {
                return "File";
            }
			else
			{
                throw new Exception();
			}
		}

		public static string GetText(EPublishType type)
		{
            if (type == EPublishType.Site)
            {
                return "应用";
            }
            else if (type == EPublishType.Image)
            {
                return "图片";
            }
            else if (type == EPublishType.Video)
            {
                return "视频";
            }
            else if (type == EPublishType.File)
			{
                return "附件";
            }
			
			else
			{
                throw new Exception();
			}
		}

		public static EPublishType GetEnumType(string typeStr)
		{
            EPublishType retval = EPublishType.Site;

            if (Equals(EPublishType.Site, typeStr))
			{
                retval = EPublishType.Site;
            }
            else if (Equals(EPublishType.Image, typeStr))
            {
                retval = EPublishType.Image;
            }
            else if (Equals(EPublishType.Video, typeStr))
            {
                retval = EPublishType.Video;
            }
            else if (Equals(EPublishType.File, typeStr))
			{
                retval = EPublishType.File;
            }

			return retval;
		}

		public static bool Equals(EPublishType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

		public static bool Equals(string typeStr, EPublishType type)
		{
			return Equals(type, typeStr);
		}

        public static ListItem GetListItem(EPublishType type, bool selected)
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
                listControl.Items.Add(GetListItem(EPublishType.Site, false));
                listControl.Items.Add(GetListItem(EPublishType.Image, false));
                listControl.Items.Add(GetListItem(EPublishType.Video, false));
                listControl.Items.Add(GetListItem(EPublishType.File, false));
            }
        }

        public static string GetAllPublishTypes()
        {
            ArrayList arraylist = new ArrayList();
            arraylist.Add(EPublishTypeUtils.GetValue(EPublishType.Site));
            arraylist.Add(EPublishTypeUtils.GetValue(EPublishType.Image));
            arraylist.Add(EPublishTypeUtils.GetValue(EPublishType.Video));
            arraylist.Add(EPublishTypeUtils.GetValue(EPublishType.File));

            return TranslateUtils.ObjectCollectionToString(arraylist);
        }
	}
}
