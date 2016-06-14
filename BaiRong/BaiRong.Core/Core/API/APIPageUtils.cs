using System;
using System.Collections.Generic;

namespace BaiRong.Core
{
    public class APIPageUtils
    {
        public static string ParseUrl(string url)
        {
            url = StringUtils.ReplaceFirst("\\api", url.ToLower(), string.Empty);
            url = StringUtils.ReplaceFirst("\\cms\\siteserver.api",url.ToLower(),"\\web");
            return StringUtils.ReplaceFirst("/api", url.ToLower(), string.Empty);
        }

        public static string ParseUrlWithCase(string url)
        {
            if (url.ToLower().IndexOf("\\api") == 0)
            {
                url = url.Substring(4);
            }
            if (url.ToLower().IndexOf("/api") == 0)
            {
                url = url.Substring(4);
            }
            if (url.ToLower().IndexOf("\\cms\\siteserver.api") == 0)
            {
                url = url.Substring(19);
            }
            return url;
        }

        public static string GetAPIUrlByPhysicalPath(string physicalPath)
        {
            string apiUrl = "/api";

            if (!string.IsNullOrEmpty(physicalPath))
            {
                physicalPath = physicalPath.ToLower();
                string apiPath = APIPathUtils.GetPath(string.Empty).ToLower();

                string requestPath = string.Empty;
                if (physicalPath.StartsWith(apiPath))
                {
                    requestPath = StringUtils.ReplaceStartsWith(physicalPath, apiPath, string.Empty);
                }
                else
                {
                    requestPath = physicalPath.Replace(apiPath, string.Empty);
                }
                requestPath = requestPath.Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar);
                return PageUtils.Combine(apiUrl, requestPath);
            }
            else
            {
                return apiUrl;
            }
        }
    }
}