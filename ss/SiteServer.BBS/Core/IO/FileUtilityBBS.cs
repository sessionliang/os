using System;
using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Drawing;
using SiteServer.BBS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Core
{
    public class FileUtilityBBS
    {
        public static void AddWaterMark(int publishmentSystemID, string imagePath)
        {
            string fileExtName = PathUtils.GetExtension(imagePath);
            if (EFileSystemTypeUtils.IsImage(fileExtName))
            {
                ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);
                if (additional.IsWaterMark)
                {
                    if (additional.IsImageWaterMark)
                    {
                        if (!string.IsNullOrEmpty(additional.WaterMarkImagePath))
                        {
                            ImageUtils.AddImageWaterMark(imagePath, PathUtility.GetPublishmentSystemPath(publishmentSystemID, additional.WaterMarkImagePath), additional.WaterMarkPosition, additional.WaterMarkTransparency, additional.WaterMarkMinWidth, additional.WaterMarkMinHeight, 100);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(additional.WaterMarkFormatString))
                        {
                            DateTime now = DateTime.Now;
                            ImageUtils.AddTextWaterMark(imagePath, string.Format(additional.WaterMarkFormatString, DateUtils.GetDateString(now), DateUtils.GetTimeString(now)), additional.WaterMarkFontName, additional.WaterMarkFontSize, additional.WaterMarkPosition, additional.WaterMarkTransparency, additional.WaterMarkMinWidth, additional.WaterMarkMinHeight);
                        }
                    }
                }
            }
        }
    }
}
