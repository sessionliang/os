using System;
using System.Text;
using System.Xml;
using System.Collections;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.IO;
using BaiRong.Core.Net;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public class SeoManager
	{
		private SeoManager()
		{
		}

		#region 页面元数据查询

		public static SeoMetaInfo GetSeoMetaInfo(string content)
		{
			SeoMetaInfo seoMetaInfo = new SeoMetaInfo();

			if (!string.IsNullOrEmpty(content))
			{
				string metaRegex = @"<title>(?<meta>[^<]*?)</title>";
				string metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.PageTitle = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')keywords(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')keywords(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.Keywords = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')description(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')description(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.Description = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')copyright(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')copyright(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.Copyright = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')author(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')author(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.Author = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')email(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')email(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.Email = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')language(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')language(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.Language = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')charset(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')charset(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.Charset = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')distribution(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')distribution(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.Distribution = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')rating(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')rating(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.Rating = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')robots(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')robots(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.Robots = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')revisit-after(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')revisit-after(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.RevisitAfter = metaContent;
				}

				metaRegex = @"<META\s+NAME=(?:""|')expires(?:""|')\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')(?:[^>]*)>|<META\s+CONTENT=(?:""|')(?<meta>[^<]*?)(?:""|')\s+NAME=(?:""|')expires(?:""|')(?:[^>]*)>";
				metaContent = RegexUtils.GetContent("meta", metaRegex, content);
				if (!string.IsNullOrEmpty(metaContent))
				{
					seoMetaInfo.Expires = metaContent;
				}
			}

			return seoMetaInfo;
		}

		public static SeoMetaInfo GetSeoMetaInfo(string siteUrl, ECharset charset)
		{
			siteUrl = PageUtils.AddProtocolToUrl(siteUrl);
			string content = WebClientUtils.GetRemoteFileSource(siteUrl, charset);
			return GetSeoMetaInfo(content);
		}


		public static string GetMetaContent(SeoMetaInfo seoMetaInfo)
		{
			StringBuilder codeBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(seoMetaInfo.PageTitle))
			{
				codeBuilder.AppendFormat("<TITLE>{0}</TITLE>\r\n", seoMetaInfo.PageTitle);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.Keywords))
			{
				codeBuilder.AppendFormat("<META NAME=\"keywords\" CONTENT=\"{0}\">\r\n", seoMetaInfo.Keywords);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.Description))
			{
				codeBuilder.AppendFormat("<META NAME=\"description\" CONTENT=\"{0}\">\r\n", seoMetaInfo.Description);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.Copyright))
			{
				codeBuilder.AppendFormat("<META NAME=\"copyright\" CONTENT=\"{0}\">\r\n", seoMetaInfo.Copyright);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.Author))
			{
				codeBuilder.AppendFormat("<META NAME=\"author\" CONTENT=\"{0}\">\r\n", seoMetaInfo.Author);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.Email))
			{
				codeBuilder.AppendFormat("<META NAME=\"email\" CONTENT=\"{0}\">\r\n", seoMetaInfo.Email);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.Language))
			{
				codeBuilder.AppendFormat("<META NAME=\"language\" CONTENT=\"{0}\">\r\n", seoMetaInfo.Language);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.Charset))
			{
				codeBuilder.AppendFormat("<META NAME=\"charset\" CONTENT=\"{0}\">\r\n", seoMetaInfo.Charset);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.Distribution))
			{
				codeBuilder.AppendFormat("<META NAME=\"distribution\" CONTENT=\"{0}\">\r\n", seoMetaInfo.Distribution);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.Rating))
			{
				codeBuilder.AppendFormat("<META NAME=\"rating\" CONTENT=\"{0}\">\r\n", seoMetaInfo.Rating);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.Robots))
			{
				codeBuilder.AppendFormat("<META NAME=\"robots\" CONTENT=\"{0}\">\r\n", seoMetaInfo.Robots);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.RevisitAfter))
			{
				codeBuilder.AppendFormat("<META NAME=\"revisit-after\" CONTENT=\"{0}\">\r\n", seoMetaInfo.RevisitAfter);
			}
			if (!string.IsNullOrEmpty(seoMetaInfo.Expires))
			{
				codeBuilder.AppendFormat("<META NAME=\"expires\" CONTENT=\"{0}\">\r\n", seoMetaInfo.Expires);
			}
			return codeBuilder.ToString();
		}

		#endregion

		#region 应用地图

		private const string siteMapGoogleHead = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset
      xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9""
      xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
      xsi:schemaLocation=""
            http://www.sitemaps.org/schemas/sitemap/0.9
            http://www.sitemaps.org/schemas/sitemap/09/sitemap.xsd"">";

		private const string siteMapGoogleFoot = @"
</urlset>";

		private const string siteMapGoogleUrlFotmat = @"
	<url>
		<loc><![CDATA[{0}]]></loc>
		<priority>{1}</priority>
		<changefreq>{2}</changefreq>
	</url>
";

		private const string siteMapGoogleUrlWithLastModifiedFotmat = @"
	<url>
		<loc><![CDATA[{0}]]></loc>
		<priority>{1}</priority>
		<changefreq>{2}</changefreq>
		<lastmod>{3}</lastmod>
	</url>
";

        public static void CreateSiteMapGoogle(PublishmentSystemInfo publishmentSystemInfo, EVisualType visualType)
		{
            int totalNum = DataProvider.NodeDAO.GetContentNumByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);

            if (totalNum == 0 || totalNum <= publishmentSystemInfo.Additional.SiteMapGooglePageCount)
            {
                StringBuilder siteMapBuilder = new StringBuilder();
                siteMapBuilder.Append(siteMapGoogleHead);

                string urlFormat = publishmentSystemInfo.Additional.SiteMapGoogleIsShowLastModified ? siteMapGoogleUrlWithLastModifiedFotmat : siteMapGoogleUrlFotmat;
                string lastmode = DateUtils.GetDateString(DateTime.Now);
                ArrayList urlArrayList = new ArrayList();
                //首页
                string publishmentSystemUrl = PageUtils.AddProtocolToUrl(publishmentSystemInfo.PublishmentSystemUrl.ToLower());
                siteMapBuilder.AppendFormat(urlFormat, publishmentSystemUrl, "1.0", publishmentSystemInfo.Additional.SiteMapGoogleChangeFrequency, lastmode);

                //栏目页
                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
                if (nodeIDArrayList != null && nodeIDArrayList.Count > 0)
                {
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        string channelUrl = PageUtils.AddProtocolToUrl(PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, visualType));
                        if (!string.IsNullOrEmpty(channelUrl))
                        {
                            if (urlArrayList.Contains(channelUrl.ToLower()))
                            {
                                continue;
                            }
                            else
                            {
                                urlArrayList.Add(channelUrl.ToLower());
                            }
                            if (channelUrl.ToLower().StartsWith(publishmentSystemUrl))
                            {
                                siteMapBuilder.AppendFormat(urlFormat, channelUrl, "0.8", publishmentSystemInfo.Additional.SiteMapGoogleChangeFrequency, lastmode);
                            }
                        }
                    }
                }

                if (nodeIDArrayList != null && nodeIDArrayList.Count > 0)
                {
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                        ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, string.Empty);

                        //内容页
                        if (contentIDArrayList != null && contentIDArrayList.Count > 0)
                        {
                            foreach (int contentID in contentIDArrayList)
                            {
                                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                                string contentUrl = PageUtils.AddProtocolToUrl(PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, visualType));
                                if (!string.IsNullOrEmpty(contentUrl))
                                {
                                    if (urlArrayList.Contains(contentUrl.ToLower()))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        urlArrayList.Add(contentUrl.ToLower());
                                    }
                                    if (contentUrl.ToLower().StartsWith(publishmentSystemUrl))
                                    {
                                        siteMapBuilder.AppendFormat(urlFormat, contentUrl, "0.5", publishmentSystemInfo.Additional.SiteMapGoogleChangeFrequency, lastmode);
                                    }
                                }
                            }
                        }
                    }
                }

                siteMapBuilder.Append(siteMapGoogleFoot);

                string siteMapPath = PathUtility.MapPath(publishmentSystemInfo, publishmentSystemInfo.Additional.SiteMapGooglePath);
                FileUtils.WriteText(siteMapPath, ECharset.utf_8, siteMapBuilder.ToString());
            }
            else
            {
                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
                ArrayList nodeIDWithContentIDArrayList = new ArrayList();

                if (nodeIDArrayList != null && nodeIDArrayList.Count > 0)
                {
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                        ArrayList idArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, string.Empty);
                        foreach (int contentID in idArrayList)
                        {
                            nodeIDWithContentIDArrayList.Add(string.Format("{0}_{1}", nodeID, contentID));
                        }
                    }
                }

                double deci = (double)nodeIDWithContentIDArrayList.Count / publishmentSystemInfo.Additional.SiteMapGooglePageCount;
                int count = Convert.ToInt32(Math.Ceiling(deci));
                StringBuilder siteMapIndexBuilder = new StringBuilder();

                string siteMapGooglePath = publishmentSystemInfo.Additional.SiteMapGooglePath.ToLower();
                string ext = PageUtils.GetExtensionFromUrl(siteMapGooglePath);

                string urlFormat = publishmentSystemInfo.Additional.SiteMapGoogleIsShowLastModified ? siteMapGoogleUrlWithLastModifiedFotmat : siteMapGoogleUrlFotmat;
                string lastmode = DateUtils.GetDateString(DateTime.Now);
                string publishmentSystemUrl = PageUtils.AddProtocolToUrl(publishmentSystemInfo.PublishmentSystemUrl.ToLower());

                for (int i = 1; i <= count; i++)
                {
                    string virtualPath = StringUtils.InsertBefore(ext, siteMapGooglePath, i.ToString());

                    siteMapIndexBuilder.AppendFormat(@"
  <sitemap>
    <loc>{0}</loc>
    <lastmod>{1}</lastmod>
  </sitemap>
", PageUtility.ParseNavigationUrl(publishmentSystemInfo, virtualPath), DateUtils.GetDateString(DateTime.Now));

                    StringBuilder siteMapBuilder = new StringBuilder();
                    siteMapBuilder.Append(siteMapGoogleHead);
                    ArrayList urlArrayList = new ArrayList();

                    if (i == 1)
                    {
                        //首页
                        siteMapBuilder.AppendFormat(urlFormat, publishmentSystemUrl, "1.0", publishmentSystemInfo.Additional.SiteMapGoogleChangeFrequency, lastmode);

                        //栏目页
                        if (nodeIDArrayList != null && nodeIDArrayList.Count > 0)
                        {
                            foreach (int nodeID in nodeIDArrayList)
                            {
                                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                                string channelUrl = PageUtils.AddProtocolToUrl(PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, visualType));
                                if (!string.IsNullOrEmpty(channelUrl))
                                {
                                    if (urlArrayList.Contains(channelUrl.ToLower()))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        urlArrayList.Add(channelUrl.ToLower());
                                    }
                                    if (channelUrl.ToLower().StartsWith(publishmentSystemUrl))
                                    {
                                        siteMapBuilder.AppendFormat(urlFormat, channelUrl, "0.8", publishmentSystemInfo.Additional.SiteMapGoogleChangeFrequency, lastmode);
                                    }
                                }
                            }
                        }
                    }

                    int pageCount = publishmentSystemInfo.Additional.SiteMapGooglePageCount;
                    if (i == count)
                    {
                        pageCount = nodeIDWithContentIDArrayList.Count - (count - 1) * publishmentSystemInfo.Additional.SiteMapGooglePageCount;
                    }
                    ArrayList pageNodeIDWithContentIDArrayList = nodeIDWithContentIDArrayList.GetRange((i - 1) * publishmentSystemInfo.Additional.SiteMapGooglePageCount, pageCount);

                    //内容页
                    foreach (string nodeIDWithContentID in pageNodeIDWithContentIDArrayList)
                    {
                        int nodeID = TranslateUtils.ToInt(nodeIDWithContentID.Split('_')[0]);
                        int contentID = TranslateUtils.ToInt(nodeIDWithContentID.Split('_')[1]);

                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                        string contentUrl = PageUtils.AddProtocolToUrl(PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, visualType));
                        if (!string.IsNullOrEmpty(contentUrl))
                        {
                            if (urlArrayList.Contains(contentUrl.ToLower()))
                            {
                                continue;
                            }
                            else
                            {
                                urlArrayList.Add(contentUrl.ToLower());
                            }
                            if (contentUrl.ToLower().StartsWith(publishmentSystemUrl))
                            {
                                siteMapBuilder.AppendFormat(urlFormat, contentUrl, "0.5", publishmentSystemInfo.Additional.SiteMapGoogleChangeFrequency, lastmode);
                            }
                        }
                    }

                    siteMapBuilder.Append(siteMapGoogleFoot);

                    string siteMapPagePath = PathUtility.MapPath(publishmentSystemInfo, virtualPath);
                    FileUtils.WriteText(siteMapPagePath, ECharset.utf_8, siteMapBuilder.ToString());
                }

                string sitemapIndexString = string.Format(@"
<?xml version=""1.0"" encoding=""UTF-8""?>
<sitemapindex xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd"">
{0}
</sitemapindex>
", siteMapIndexBuilder.ToString());

                string siteMapPath = PathUtility.MapPath(publishmentSystemInfo, publishmentSystemInfo.Additional.SiteMapGooglePath);
                FileUtils.WriteText(siteMapPath, ECharset.utf_8, sitemapIndexString);
            }
		}

        public static void CreateSiteMapBaidu(PublishmentSystemInfo publishmentSystemInfo, EVisualType visualType)
        {
            string publishmentSystemUrl = PageUtils.AddProtocolToUrl(publishmentSystemInfo.PublishmentSystemUrl.ToLower());

            StringBuilder siteMapBuilder = new StringBuilder();
            siteMapBuilder.AppendFormat(@"<?xml version=""1.0"" encoding=""GB2312"" ?>
<document>
<webSite>{0}</webSite>
<webMaster>{1}</webMaster>
<updatePeri>{2}</updatePeri>
", publishmentSystemUrl, publishmentSystemInfo.Additional.SiteMapBaiduWebMaster, publishmentSystemInfo.Additional.SiteMapBaiduUpdatePeri);

            ArrayList urlArrayList = new ArrayList();

            //内容页
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);

            if (nodeIDArrayList != null && nodeIDArrayList.Count > 0)
            {
                foreach (int nodeID in nodeIDArrayList)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                    ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, string.Empty);

                    if (contentIDArrayList != null && contentIDArrayList.Count > 0)
                    {
                        foreach (int contentID in contentIDArrayList)
                        {
                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                            string contentUrl = PageUtils.AddProtocolToUrl(PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, visualType));
                            if (!string.IsNullOrEmpty(contentUrl))
                            {
                                if (urlArrayList.Contains(contentUrl.ToLower()))
                                {
                                    continue;
                                }
                                else
                                {
                                    urlArrayList.Add(contentUrl.ToLower());
                                }
                                if (contentUrl.ToLower().StartsWith(publishmentSystemUrl))
                                {
                                    siteMapBuilder.AppendFormat(@"
<item>
    <link><![CDATA[{0}]]></link>
    <title><![CDATA[{1}]]></title>
    <text><![CDATA[{2}]]></text>
    <image><![CDATA[{3}]]></image>
    <category><![CDATA[{4}]]></category>
    <pubDate>{5}</pubDate>
</item>
", contentUrl, contentInfo.Title, StringUtils.StripTags(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.Content)), PageUtility.ParseNavigationUrl(publishmentSystemInfo, contentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl)), NodeManager.GetNodeName(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID), DateUtils.GetDateAndTimeString(contentInfo.AddDate));
                                }
                            }
                        }
                    }
                }
            }

            siteMapBuilder.Append(@"
</document>");

            string siteMapPath = PathUtility.MapPath(publishmentSystemInfo, publishmentSystemInfo.Additional.SiteMapBaiduPath);
            FileUtils.WriteText(siteMapPath, ECharset.gb2312, siteMapBuilder.ToString());
        }

		#endregion

        #region 缓存

        public static ArrayList[] GetSeoMetaArrayLists(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            lock (lockObject)
            {
                if (CacheUtils.Get(cacheKey) == null)
                {
                    ArrayList[] arraylists = DataProvider.SeoMetaDAO.GetSeoMetaArrayLists(publishmentSystemID);
                    CacheUtils.Insert(cacheKey, arraylists, 30);
                    return arraylists;
                }
                return CacheUtils.Get(cacheKey) as ArrayList[];
            }
        }

        public static void RemoveCache(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        private static string GetCacheKey(int publishmentSystemID)
        {
            return cacheKeyPrefix + publishmentSystemID;
        }

        private static readonly object lockObject = new object();
        private const string cacheKeyPrefix = "SiteServer.CMS.Core.SeoMeta.";

        #endregion
	}
}
