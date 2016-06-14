using System;
using System.Text;
using System.Xml;
using System.Collections;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.IO;
using BaiRong.Core.Net;
using SiteServer.CMS.Model;
using SiteServer.CMS.BackgroundPages.Modal;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.Core
{
	public class TagStyleManager
	{
        private TagStyleManager()
		{
		}

        public static TagStyleInfo GetTagStyleInfo(int styleID)
        {
            TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);
            if (styleInfo == null)
            {
                styleInfo = new TagStyleInfo();
            }
            return styleInfo;
        }

        public static TagStyleInfo GetTagStyleInfo(int publishmentSystemID, string elementName, string styleName)
        {
            if (publishmentSystemID == 0 || string.IsNullOrEmpty(elementName) || string.IsNullOrEmpty(styleName))
            {
                return null;
            }
            string cacheKey = GetCacheKey(publishmentSystemID, elementName, styleName);
            lock (lockObject)
            {
                if (CacheUtils.Get(cacheKey) == null)
                {
                    TagStyleInfo tagStyleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(publishmentSystemID, elementName, styleName);
                    CacheUtils.Insert(cacheKey, tagStyleInfo, 30);
                    return tagStyleInfo;
                }
                return CacheUtils.Get(cacheKey) as TagStyleInfo;
            }
        }

        public static void RemoveCache(int publishmentSystemID, string elementName, string styleName)
        {
            string cacheKey = GetCacheKey(publishmentSystemID, elementName, styleName);
            CacheUtils.Remove(cacheKey);
        }

        private static string GetCacheKey(int publishmentSystemID, string elementName, string styleName)
        {
            return string.Format("SiteServer.CMS.Core.TagStyleManager.{0}.{1}.{2}", publishmentSystemID, elementName, styleName);
        }

        private static readonly object lockObject = new object();
	}
}
