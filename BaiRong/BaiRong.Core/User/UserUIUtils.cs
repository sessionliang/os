using System;
using System.Text;
using BaiRong.Core;

namespace BaiRong.Core
{
    public class UserUIUtils
    {
        public static string GetTrueImageHtml(string isDefaultStr)
        {
            return GetTrueImageHtml(TranslateUtils.ToBool(isDefaultStr));
        }

        public static string GetTrueImageHtml(bool isDefault)
        {
            string retval = string.Empty;
            if (isDefault)
            {
                string imageUrl = PageUtils.GetIconUrl("right.gif");
                retval = string.Format("<img src='{0}' border='0'/>", imageUrl);
            }
            return retval;
        }

        public static string GetFalseImageHtml(string isDefaultStr)
        {
            return GetFalseImageHtml(TranslateUtils.ToBool(isDefaultStr));
        }

        public static string GetFalseImageHtml(bool isDefault)
        {
            string retval = string.Empty;
            if (isDefault == false)
            {
                string imageUrl = PageUtils.GetIconUrl("wrong.gif");
                retval = string.Format("<img src='{0}' border='0'/>", imageUrl);
            }
            return retval;
        }

        public static string GetTrueOrFalseImageHtml(string isDefaultStr)
        {
            return GetTrueOrFalseImageHtml(TranslateUtils.ToBool(isDefaultStr));
        }

        public static string GetTrueOrFalseImageHtml(bool isDefault)
        {
            string imageUrl;
            if (isDefault)
            {
                imageUrl = PageUtils.GetIconUrl("right.gif");
            }
            else
            {
                imageUrl = PageUtils.GetIconUrl("wrong.gif");
            }

            return string.Format("<img src='{0}' border='0'/>", imageUrl);
        }
    }
}
