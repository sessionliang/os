using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EStorageClassify
	{
		Site,
		Image,
		Video,
        File
	}

    public class EStorageClassifyUtils
	{
		public static string GetValue(EStorageClassify type)
		{
            if (type == EStorageClassify.Site)
			{
                return "Site";
			}
            else if (type == EStorageClassify.Image)
			{
                return "Image";
			}
            else if (type == EStorageClassify.Video)
			{
                return "Video";
            }
            else if (type == EStorageClassify.File)
            {
                return "File";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EStorageClassify type)
		{
            if (type == EStorageClassify.Site)
            {
                return "应用";
            }
            else if (type == EStorageClassify.Image)
            {
                return "图片";
            }
            else if (type == EStorageClassify.Video)
            {
                return "视频";
            }
            else if (type == EStorageClassify.File)
            {
                return "附件";
            }
            else
            {
                throw new Exception();
            }
		}

		public static EStorageClassify GetEnumType(string typeStr)
		{
			EStorageClassify retval = EStorageClassify.Site;

            if (Equals(EStorageClassify.Site, typeStr))
            {
                retval = EStorageClassify.Site;
            }
            else if (Equals(EStorageClassify.Image, typeStr))
			{
                retval = EStorageClassify.Image;
			}
            else if (Equals(EStorageClassify.Video, typeStr))
			{
                retval = EStorageClassify.Video;
			}
            else if (Equals(EStorageClassify.File, typeStr))
			{
                retval = EStorageClassify.File;
            }

			return retval;
		}

		public static bool Equals(EStorageClassify type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EStorageClassify type)
        {
            return Equals(type, typeStr);
        }
	}
}
