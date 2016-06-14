using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using Atom.Utils;
using BaiRong.Core.Net;
using System.Web.Script.Serialization;

namespace BaiRong.Core
{
    public class PageUtils
    {
        public const char SeparatorChar = '/';

        public const string UNCLICKED_URL = "javascript:;";

        public static string ParseNavigationUrl(string url)
        {
            //			string retval = string.Empty;
            //			if (string.IsNullOrEmpty(url))
            //			{
            //				return retval;
            //			}
            //			if (url.StartsWith("~"))
            //			{
            //				retval = Combine(HttpContext.Current.Request.ApplicationPath ,url.Substring(1));
            //			}
            //			else
            //			{
            //				retval = url;
            //			}
            //			return retval;
            //            //return AddProtocolToUrl(retval);
            return ParseNavigationUrl(url, ConfigUtils.Instance.ApplicationPath);
        }

        public static string ParseNavigationUrl(string url, string domainUrl)
        {
            string retval = string.Empty;
            if (string.IsNullOrEmpty(url))
            {
                return retval;
            }
            if (url.StartsWith("~"))
            {
                retval = Combine(domainUrl, url.Substring(1));
            }
            else
            {
                retval = url;
            }
            return retval;
            //return AddProtocolToUrl(retval);
        }

        public static string AddProtocolToUrl(string url)
        {
            return AddProtocolToUrl(url, string.Empty);
        }

        /// <summary>
        /// 按照给定的host，添加Protocol
        /// Demo: 发送的邮件中，需要内容标题的链接为全连接，那么需要指定他的host
        /// </summary>
        /// <param name="url"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string AddProtocolToUrl(string url, string host)
        {
            if (url == PageUtils.UNCLICKED_URL)
            {
                return url;
            }
            string retval = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                url = url.Trim();
                if (IsProtocolUrl(url))
                {
                    retval = url;
                }
                else
                {
                    if (string.IsNullOrEmpty(host))
                        retval = url.StartsWith("/") ? (PageUtils.GetScheme() + "://" + PageUtils.GetHost() + url) : (PageUtils.GetScheme() + "://" + url);
                    else
                        retval = url.StartsWith("/") ? (host.TrimEnd('/') + url) : (host + url);
                }
            }
            return retval;
        }

        public static string AddQuestionOrAndToUrl(string pageUrl)
        {
            string url = pageUrl;
            if (string.IsNullOrEmpty(url))
            {
                url = "?";
            }
            else
            {
                if (url.IndexOf('?') == -1)
                {
                    url = url + "?";
                }
                else if (!url.EndsWith("?"))
                {
                    url = url + "&";
                }
            }
            return url;
        }

        public static string RemovePortFromUrl(string url)
        {
            string retval = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                var regex = new Regex(@":\d+");
                retval = regex.Replace(url, "");
            }
            return retval;
        }

        public static string RemoveProtocolFromUrl(string url)
        {
            string retval = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                url = url.Trim();
                if (IsProtocolUrl(url))
                {
                    retval = url.Substring(url.IndexOf("://") + 3);
                }
                else
                {
                    retval = url;
                }
            }
            return retval;
        }

        public static bool IsProtocolUrl(string url)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(url))
            {
                if (url.IndexOf("://") != -1 || url.StartsWith("javascript:"))
                {
                    retval = true;
                }
            }
            return retval;
        }

        public static string GetAbsoluteUrl()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri;
        }

        public static string PathDifference(string path1, string path2, bool compareCase)
        {
            int num2 = -1;
            int num1 = 0;
            while ((num1 < path1.Length) && (num1 < path2.Length))
            {
                if ((path1[num1] != path2[num1]) && (compareCase || (char.ToLower(path1[num1], CultureInfo.InvariantCulture) != char.ToLower(path2[num1], CultureInfo.InvariantCulture))))
                {
                    break;
                }
                if (path1[num1] == '/')
                {
                    num2 = num1;
                }
                num1++;
            }
            if (num1 == 0)
            {
                return path2;
            }
            if ((num1 == path1.Length) && (num1 == path2.Length))
            {
                return string.Empty;
            }
            StringBuilder builder1 = new StringBuilder();
            while (num1 < path1.Length)
            {
                if (path1[num1] == '/')
                {
                    builder1.Append("../");
                }
                num1++;
            }
            return (builder1 + path2.Substring(num2 + 1));
        }

        /// <summary>
        /// 获取服务器根域名  
        /// </summary>
        /// <returns></returns>
        public static string GetMainDomain(string url)
        {
            if (string.IsNullOrEmpty(url)) return url;

            url = PageUtils.RemoveProtocolFromUrl(url.ToLower());
            if (url.IndexOf('/') != -1)
            {
                url = url.Substring(0, url.IndexOf('/'));
            }

            if (url.IndexOf('.') > 0)
            {
                string[] strArr = url.Split('.');
                string lastStr = strArr.GetValue(strArr.Length - 1).ToString();
                if (StringUtils.IsNumber(lastStr)) //如果最后一位是数字，那么说明是IP地址
                {
                    return url;
                }
                else //否则为域名
                {
                    string[] domainRules = ".com.cn|.net.cn|.org.cn|.gov.cn|.com|.net|.cn|.org|.cc|.me|.tel|.mobi|.asia|.biz|.info|.name|.tv|.hk|.公司|.中国|.网络".Split('|');
                    string findStr = string.Empty;
                    string replaceStr = string.Empty;
                    string returnStr = string.Empty;
                    for (int i = 0; i < domainRules.Length; i++)
                    {
                        if (url.EndsWith(domainRules[i].ToLower())) //如果最后有找到匹配项
                        {
                            findStr = domainRules[i].ToString(); //www.px915.COM
                            replaceStr = url.Replace(findStr, ""); //将匹配项替换为空，便于再次判断
                            if (replaceStr.IndexOf('.') > 0) //存在二级域名或者三级，比如：www.px915
                            {
                                string[] replaceArr = replaceStr.Split('.'); // www px915
                                returnStr = replaceArr.GetValue(replaceArr.Length - 1).ToString() + findStr;
                                return returnStr;
                            }
                            else //px915
                            {
                                returnStr = replaceStr + findStr; //连接起来输出为：px915.com
                                return returnStr;
                            };
                        }
                        else
                        { returnStr = url; }
                    }
                    return returnStr;
                }
            }
            else
            {
                return url;
            }
        }

        public static string GetHost()
        {
            string host = string.Empty;
            if (HttpContext.Current != null)
            {
                host = HttpContext.Current.Request.Headers["HOST"];
                if (string.IsNullOrEmpty(host))
                {
                    host = HttpContext.Current.Request.Url.Host;
                }
            }

            return (string.IsNullOrEmpty(host)) ? string.Empty : host.Trim().ToLower();
        }

        public static string GetScheme()
        {
            string scheme = string.Empty;
            if (HttpContext.Current != null)
            {
                scheme = HttpContext.Current.Request.Headers["SCHEME"];
                if (string.IsNullOrEmpty(scheme))
                {
                    scheme = HttpContext.Current.Request.Url.Scheme;
                }
            }

            return (string.IsNullOrEmpty(scheme)) ? "http" : scheme.Trim().ToLower();
        }

        // 系统根目录访问地址
        public static string GetRootUrl(string relatedUrl)
        {
            if (ConfigManager.Instance.IsRelatedUrl)
            {
                return PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, relatedUrl);
            }
            else
            {
                return PageUtils.Combine(ConfigManager.Instance.RootUrl, relatedUrl);
            }
        }

        public static string GetTemporaryFilesUrl(string relatedUrl)
        {
            return PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.TemporaryFiles, relatedUrl);
        }

        //public static string GetStlInnerUrl(string relatedUrl)
        //{
        //    string innerPageUrl = string.Format("~/{0}/{1}/{2}", DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.Inner, relatedUrl);
        //    return PageUtils.ParseNavigationUrl(innerPageUrl);
        //}

        public static NameValueCollection GetQueryString(string url)
        {
            NameValueCollection attributes = new NameValueCollection();
            if (!string.IsNullOrEmpty(url) && url.IndexOf("?") != -1)
            {
                string querystring = url.Substring(url.IndexOf("?") + 1);
                attributes = TranslateUtils.ToNameValueCollection(querystring);
            }
            return attributes;
        }

        public static NameValueCollection GetQueryStringFilterXSS(string url)
        {
            NameValueCollection attributes = new NameValueCollection();
            if (!string.IsNullOrEmpty(url) && url.IndexOf("?") != -1)
            {
                string querystring = url.Substring(url.IndexOf("?") + 1);
                NameValueCollection originals = TranslateUtils.ToNameValueCollection(querystring);
                foreach (string key in originals.Keys)
                {
                    attributes[key] = PageUtils.FilterXSS(originals[key]);

                }
            }
            return attributes;
        }

        public static string Combine(params string[] urls)
        {
            string retval = string.Empty;
            if (urls != null && urls.Length > 0)
            {
                retval = (urls[0] != null) ? urls[0].Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar) : string.Empty;
                for (int i = 1; i < urls.Length; i++)
                {
                    string url = (urls[i] != null) ? urls[i].Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar) : string.Empty;
                    retval = PageUtils.Combine(retval, url);
                }
            }
            return retval;
        }

        private static string Combine(string url1, string url2)
        {
            if ((url1 == null) || (url2 == null))
            {
                throw new ArgumentNullException((url1 == null) ? "url1" : "url2");
            }
            if (url2.Length == 0)
            {
                return url1;
            }
            if (url1.Length == 0)
            {
                return url2;
            }

            return (url1.TrimEnd(SeparatorChar) + SeparatorChar + url2.TrimStart(SeparatorChar));
        }

        public static string AddQueryString(string url, string queryStringKey, string queryStringValue)
        {
            NameValueCollection queryString = new NameValueCollection();
            queryString.Add(queryStringKey, queryStringValue);
            return AddQueryString(url, queryString);
        }

        public static string AddQueryString(string url, string queryString)
        {
            if (queryString == null || url == null)
                return url;

            queryString = queryString.TrimStart(new char[] { '?', '&' });

            if (url.IndexOf("?") == -1)
            {
                return string.Concat(url, "?", queryString);
            }
            else
            {
                if (url.EndsWith("?"))
                {
                    return string.Concat(url, queryString);
                }
                else
                {
                    return string.Concat(url, "&", queryString);
                }
            }
        }

        public static string AddQueryString(string url, NameValueCollection queryString)
        {
            if (queryString == null || url == null || queryString.Count == 0)
                return url;

            StringBuilder builder = new StringBuilder();
            foreach (string key in queryString.Keys)
            {
                builder.AppendFormat("&{0}={1}", key, HttpUtility.UrlEncode(queryString[key]));
            }
            if (url.IndexOf("?") == -1)
            {
                if (builder.Length > 0) builder.Remove(0, 1);
                return string.Concat(url, "?", builder.ToString());
            }
            else
            {
                if (url.EndsWith("?"))
                {
                    if (builder.Length > 0) builder.Remove(0, 1);
                }
                return string.Concat(url, builder.ToString());
            }
        }

        public static string RemoveQueryString(string url, string queryString)
        {
            if (queryString == null || url == null)
                return url;

            if (url.IndexOf("?") == -1 || url.EndsWith("?"))
            {
                return url;
            }
            else
            {
                NameValueCollection attributes = PageUtils.GetQueryString(url);
                attributes.Remove(queryString);
                url = url.Substring(0, url.IndexOf("?"));
                return PageUtils.AddQueryString(url, attributes);
            }
        }

        //public static string GetIPAddress()
        //{
        //    string result = string.Empty;
        //    if (HttpContext.Current != null)
        //    {
        //        result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //        if (string.IsNullOrEmpty(result))
        //            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        //        if (string.IsNullOrEmpty(result))
        //            result = HttpContext.Current.Request.UserHostAddress;
        //    }

        //    if (string.IsNullOrEmpty(result) || !StringUtils.IsIPAddress(result))
        //        return "127.0.0.1";

        //    return result;
        //}

        public static string GetIPAddress()
        {
            //取CDN用户真实IP的方法
            //当用户使用代理时，取到的是代理IP
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(result))
            {
                //可能有代理
                if (result.IndexOf(".") == -1)
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        result = result.Replace("  ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIP(temparyip[i]) && temparyip[i].Substring(0, 3) != "10." && temparyip[i].Substring(0, 7) != "192.168" && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                result = temparyip[i];
                            }
                        }
                        string[] str = result.Split(',');
                        if (str.Length > 0)
                            result = str[0].ToString().Trim();
                    }
                    else if (IsIP(result))
                        return result;
                }
            }

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrEmpty(result))
                result = "127.0.0.1";


            return result;
        }

        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static string SessionID
        {
            get
            {
                string sessionID = CookieUtils.GetCookie("SiteServer.SessionID");
                if (string.IsNullOrEmpty(sessionID))
                {
                    long i = 1;
                    foreach (byte b in Guid.NewGuid().ToByteArray())
                    {
                        i *= ((int)b + 1);
                    }
                    sessionID = string.Format("{0:x}", i - DateTime.Now.Ticks);
                    CookieUtils.SetCookie("SiteServer.SessionID", sessionID, DateTime.Now.AddDays(100));
                }
                return sessionID;
            }
        }

        public static string GetRefererUrl()
        {
            string url = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
            return url;
        }

        public static string GetUrlWithReturnUrl(string pageUrl, string returnUrl)
        {
            string retval = pageUrl;
            returnUrl = string.Format("ReturnUrl={0}", returnUrl);
            if (pageUrl.IndexOf("?") != -1)
            {
                if (pageUrl.EndsWith("&"))
                {
                    retval += returnUrl;
                }
                else
                {
                    retval += "&" + returnUrl;
                }
            }
            else
            {
                retval += "?" + returnUrl;
            }
            return PageUtils.ParseNavigationUrl(retval);
        }

        public static string GetReturnUrl()
        {
            return GetReturnUrl(true);
        }

        public static string GetReturnUrl(bool toReferer)
        {
            string redirectUrl = string.Empty;
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ReturnUrl"]))
            {
                redirectUrl = PageUtils.ParseNavigationUrl(HttpContext.Current.Request.QueryString["ReturnUrl"]);
            }
            else if (toReferer)
            {
                string referer = PageUtils.GetRefererUrl();
                if (!string.IsNullOrEmpty(referer))
                {
                    redirectUrl = referer;
                }
                else
                {
                    redirectUrl = PageUtils.GetHost();
                }
            }
            return redirectUrl;
        }

        public static string GetUrlByBaseUrl(string rawUrl, string baseUrl)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(rawUrl))
            {
                rawUrl = rawUrl.Trim().TrimEnd('#');
            }
            if (!string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = baseUrl.Trim();
            }
            if (!string.IsNullOrEmpty(rawUrl))
            {
                rawUrl = rawUrl.Trim();
                if (PageUtils.IsProtocolUrl(rawUrl))
                {
                    url = rawUrl;
                }
                else if (rawUrl.StartsWith("/"))
                {
                    string domain = GetUrlWithoutPathInfo(baseUrl);
                    url = domain + rawUrl;
                }
                else if (rawUrl.StartsWith("../"))
                {
                    int count = StringUtils.GetStartCount("../", rawUrl);
                    rawUrl = rawUrl.Remove(0, 3 * count);
                    baseUrl = GetUrlWithoutFileName(baseUrl).TrimEnd('/');
                    baseUrl = PageUtils.RemoveProtocolFromUrl(baseUrl);
                    for (int i = 0; i < count; i++)
                    {
                        int j = baseUrl.LastIndexOf('/');
                        if (j != -1)
                        {
                            baseUrl = StringUtils.Remove(baseUrl, j);
                        }
                        else
                        {
                            break;
                        }
                    }
                    url = PageUtils.Combine(PageUtils.AddProtocolToUrl(baseUrl), rawUrl);
                }
                else
                {
                    if (baseUrl.EndsWith("/"))
                    {
                        url = baseUrl + rawUrl;
                    }
                    else
                    {
                        string urlWithoutFileName = GetUrlWithoutFileName(baseUrl);
                        if (!urlWithoutFileName.EndsWith("/"))
                        {
                            urlWithoutFileName += "/";
                        }
                        url = urlWithoutFileName + rawUrl;
                    }
                }
            }
            return url;
        }

        /// <summary>
        /// 将Url地址的查询字符串去掉
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public static string GetUrlWithoutQueryString(string rawUrl)
        {
            string urlWithoutQueryString;
            if (rawUrl != null && rawUrl.IndexOf("?") != -1)
            {
                string queryString = rawUrl.Substring(rawUrl.IndexOf("?"));
                urlWithoutQueryString = rawUrl.Replace(queryString, "");
            }
            else
            {
                urlWithoutQueryString = rawUrl;
            }
            return urlWithoutQueryString;
        }

        /// <summary>
        /// 将Url地址域名后的字符去掉
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public static string GetUrlWithoutPathInfo(string rawUrl)
        {
            string urlWithoutPathInfo = string.Empty;
            if (rawUrl != null && rawUrl.Trim().Length > 0)
            {
                if (rawUrl.ToLower().StartsWith("http://"))
                {
                    urlWithoutPathInfo = rawUrl.Substring("http://".Length);
                }
                if (urlWithoutPathInfo.IndexOf("/") != -1)
                {
                    urlWithoutPathInfo = urlWithoutPathInfo.Substring(0, urlWithoutPathInfo.IndexOf("/"));
                }
                if (string.IsNullOrEmpty(urlWithoutPathInfo))
                {
                    urlWithoutPathInfo = rawUrl;
                }
                urlWithoutPathInfo = "http://" + urlWithoutPathInfo;
            }
            return urlWithoutPathInfo;
        }

        /// <summary>
        /// 将Url地址后的文件名称去掉
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public static string GetUrlWithoutFileName(string rawUrl)
        {
            string urlWithoutFileName = string.Empty;
            if (rawUrl != null && rawUrl.Trim().Length > 0)
            {
                if (rawUrl.ToLower().StartsWith("http://"))
                {
                    urlWithoutFileName = rawUrl.Substring("http://".Length);
                }
                if (urlWithoutFileName.IndexOf("/") != -1 && !urlWithoutFileName.EndsWith("/"))
                {
                    string regex = "/(?<filename>[^/]*\\.[^/]*)[^/]*$";
                    RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase);
                    Regex reg = new Regex(regex, options);
                    Match match = reg.Match(urlWithoutFileName);
                    if (match.Success)
                    {
                        string fileName = match.Groups["filename"].Value;
                        urlWithoutFileName = urlWithoutFileName.Substring(0, urlWithoutFileName.LastIndexOf(fileName));
                    }
                }
                urlWithoutFileName = "http://" + urlWithoutFileName;
            }
            return urlWithoutFileName;
        }

        public static string GetUrlQueryString(string url)
        {
            string queryString = string.Empty;
            if (!string.IsNullOrEmpty(url) && url.IndexOf("?") != -1)
            {
                queryString = url.Substring(url.IndexOf("?") + 1);
            }
            return queryString;
        }

        public static string GetFileNameFromUrl(string rawUrl)
        {
            string fileName = string.Empty;
            if (!string.IsNullOrEmpty(rawUrl))
            {
                //if (rawUrl.ToLower().StartsWith("http://"))
                //{
                //    rawUrl = rawUrl.Substring("http://".Length);
                //}
                //if (rawUrl.IndexOf("?") != -1)
                //{
                //    int index = rawUrl.IndexOf("?");
                //    rawUrl = rawUrl.Remove(index, rawUrl.Length - index);
                //}
                rawUrl = PageUtils.RemoveProtocolFromUrl(rawUrl);
                rawUrl = PageUtils.GetUrlWithoutQueryString(rawUrl);
                if (rawUrl.IndexOf("/") != -1 && !rawUrl.EndsWith("/"))
                {
                    string regex = "/(?<filename>[^/]*\\.[^/]*)[^/]*$";
                    RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase);
                    Regex reg = new Regex(regex, options);
                    Match match = reg.Match(rawUrl);
                    if (match.Success)
                    {
                        fileName = match.Groups["filename"].Value;
                    }
                }
                else
                {
                    fileName = rawUrl;
                }
            }
            return fileName;
        }

        public static string GetExtensionFromUrl(string rawUrl)
        {
            string extension = string.Empty;
            if (!string.IsNullOrEmpty(rawUrl))
            {
                rawUrl = PageUtils.RemoveProtocolFromUrl(rawUrl);
                rawUrl = PageUtils.GetUrlWithoutQueryString(rawUrl);
                rawUrl = rawUrl.TrimEnd('/');
                if (rawUrl.IndexOf('/') != -1)
                {
                    rawUrl = rawUrl.Substring(rawUrl.LastIndexOf('/'));
                    if (rawUrl.IndexOf('.') != -1)
                    {
                        extension = rawUrl.Substring(rawUrl.LastIndexOf('.'));
                    }
                }
            }
            return extension;
        }

        public static string UrlEncode(string urlString)
        {
            if (urlString == null || urlString == "$4")
            {
                return string.Empty;
            }

            string newValue = urlString.Replace("\"", "'");
            newValue = System.Web.HttpUtility.UrlEncode(newValue);
            newValue = newValue.Replace("%2f", "/");
            return newValue;
        }

        public static string UrlEncode(string urlString, string encoding)
        {
            if (urlString == null || urlString == "$4")
            {
                return string.Empty;
            }

            string newValue = urlString.Replace("\"", "'");
            newValue = System.Web.HttpUtility.UrlEncode(newValue, System.Text.Encoding.GetEncoding(encoding));
            newValue = newValue.Replace("%2f", "/");
            return newValue;
        }

        public static string UrlEncode(string urlString, ECharset charset)
        {
            if (urlString == null || urlString == "$4")
            {
                return string.Empty;
            }

            string newValue = urlString.Replace("\"", "'");
            newValue = System.Web.HttpUtility.UrlEncode(newValue, ECharsetUtils.GetEncoding(charset));
            newValue = newValue.Replace("%2f", "/");
            return newValue;
        }

        public static string UrlDecode(string urlString, string encoding)
        {
            return System.Web.HttpUtility.UrlDecode(urlString, System.Text.Encoding.GetEncoding(encoding));
        }

        public static string UrlDecode(string urlString, ECharset charset)
        {
            return System.Web.HttpUtility.UrlDecode(urlString, ECharsetUtils.GetEncoding(charset));
        }

        public static string UrlDecode(string urlString)
        {
            return HttpContext.Current.Server.UrlDecode(urlString);
        }

        public static void Redirect(string url)
        {
            HttpContext.Current.Response.Redirect(url, true);
        }

        public static void Download(System.Web.HttpResponse response, string filePath, string fileName)
        {
            string fileType = PathUtils.GetExtension(filePath);
            EFileSystemType fileSystemType = EFileSystemTypeUtils.GetEnumType(fileType);
            response.Buffer = true;
            response.Clear();
            response.ContentType = EFileSystemTypeUtils.GetResponseContentType(fileSystemType); //读取文件类型
            //response.AddHeader("Content-Disposition", "attachment; filename=" + fileName); 
            response.AddHeader("Content-Disposition", "attachment; filename=" + PageUtils.UrlEncode(fileName));
            //当要下载的文件名是中文时,需加上HttpUtility.UrlEncode
            response.WriteFile(filePath);
            response.Flush();
            response.End();
        }

        public static void Download(System.Web.HttpResponse response, string filePath)
        {
            string fileName = PathUtils.GetFileName(filePath);
            Download(response, filePath, fileName);
        }

        public static string GetClientUserUrl(string applicationName, string userName, string relatedPath)
        {
            string systemName;
            if (!string.IsNullOrEmpty(applicationName) && applicationName.IndexOf("_") != -1)
            {
                systemName = applicationName.Split('_')[1];
            }
            else
            {
                systemName = applicationName;
            }
            return PageUtils.GetSiteFilesUrl(string.Format("{0}/{1}/{2}/{3}", SiteFiles.Directory.Users, userName, systemName, relatedPath));
        }

        public static string GetClientFileSystemIconUrl(EFileSystemType fileSystemType, bool isLargeIcon)
        {
            string urlFormat = string.Format("{0}/{1}{2}.gif", SiteFiles.Directory.FileSystem, (isLargeIcon) ? "large" : "small", (fileSystemType != EFileSystemType.Directory) ? EFileSystemTypeUtils.GetValue(fileSystemType) : ".directory");
            return PageUtils.GetSiteFilesUrl(urlFormat);
        }

        public static string GetClientIconUrl(string iconFileName)
        {
            return PageUtils.GetSiteFilesUrl(string.Format("{0}/{1}", SiteFiles.Directory.Icons, iconFileName));
        }

        public static string GetTrackBackPingUrl(string applicationName, string relatedTableName, string relatedIdentity)
        {
            return PageUtils.ParseNavigationUrl(string.Format("~/trackback/{0}/{1}/{2}.aspx", applicationName, relatedTableName, relatedIdentity));
        }

        public static string GetAdminDirectoryUrl(string relatedUrl)
        {
            return PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, FileConfigManager.Instance.AdminDirectoryName, relatedUrl);
        }

        public static string GetPlatformUrl(string relatedUrl)
        {
            return PageUtils.GetAdminDirectoryUrl(PageUtils.Combine("platform", relatedUrl));
        }

        public static string GetCMSUrl(string relatedUrl)
        {
            return PageUtils.GetAdminDirectoryUrl(PageUtils.Combine("cms", relatedUrl));
        }

        /// <summary>
        /// 获取投稿系统的文件地址
        /// add by sessionliang at 20151207
        /// </summary>
        /// <param name="relatedUrl"></param>
        /// <returns></returns>
        public static string GetMLibUrl(string relatedUrl)
        {
            return PageUtils.GetAdminDirectoryUrl(PageUtils.Combine("mlib", relatedUrl));
        }

        public static string GetB2CUrl(string relatedUrl)
        {
            return PageUtils.GetAdminDirectoryUrl(PageUtils.Combine("b2c", relatedUrl));
        }

        public static string GetWCMUrl(string relatedUrl)
        {
            return PageUtils.GetAdminDirectoryUrl(PageUtils.Combine("wcm", relatedUrl));
        }

        public static string GetSTLUrl(string relatedUrl)
        {
            return PageUtils.GetAdminDirectoryUrl(PageUtils.Combine("stl", relatedUrl));
        }

        public static string GetWXUrl(string relatedUrl)
        {
            return PageUtils.GetAdminDirectoryUrl(PageUtils.Combine("weixin", relatedUrl));
        }

        public static string GetBBSUrl(string relatedUrl)
        {
            return PageUtils.GetAdminDirectoryUrl(PageUtils.Combine("bbs", relatedUrl));
        }

        public static string GetCRMUrl(string relatedUrl)
        {
            return PageUtils.GetAdminDirectoryUrl(PageUtils.Combine("crm", relatedUrl));
        }

        public static string GetAdminDirectoryUrlByPage(string relatedUrl)
        {
            string url = string.Format("~/{0}/{1}", FileConfigManager.Instance.AdminDirectoryName, relatedUrl);
            return PageUtils.ParseConfigRootUrl(url);
        }

        //public static string GetTextEditorUrl(ETextEditorType editorType, string relatedUrl)
        //{
        //    return PageUtils.GetSiteFilesUrl(PageUtils.Combine("bairong/texteditor", ETextEditorTypeUtils.GetValue(editorType), relatedUrl));
        //}

        public static string GetTextEditorUrl(ETextEditorType editorType, bool isBackground, out string snapHostUrl, out string uploadImageUrl, out string uploadScrawlUrl, out string uploadFileUrl, out string imageManagerUrl, out string getMovieUrl)
        {
            snapHostUrl = PageUtils.GetHost();

            if (isBackground)
            {
                uploadImageUrl = APIPageUtils.ParseUrlWithCase(PageUtils.GetPlatformUrl(string.Format("background_textEditorUpload.aspx?EditorType={0}&FileType={1}", ETextEditorTypeUtils.GetValue(editorType), "Image")));
                uploadScrawlUrl = APIPageUtils.ParseUrlWithCase(PageUtils.GetPlatformUrl(string.Format("background_textEditorUpload.aspx?EditorType={0}&FileType={1}", ETextEditorTypeUtils.GetValue(editorType), "Scrawl")));
                uploadFileUrl = APIPageUtils.ParseUrlWithCase(PageUtils.GetPlatformUrl(string.Format("background_textEditorUpload.aspx?EditorType={0}&FileType={1}", ETextEditorTypeUtils.GetValue(editorType), "File")));
                imageManagerUrl = APIPageUtils.ParseUrlWithCase(PageUtils.GetPlatformUrl(string.Format("background_textEditorUpload.aspx?EditorType={0}&FileType={1}", ETextEditorTypeUtils.GetValue(editorType), "ImageManager")));
                getMovieUrl = APIPageUtils.ParseUrlWithCase(PageUtils.GetPlatformUrl(string.Format("background_textEditorUpload.aspx?EditorType={0}&FileType={1}", ETextEditorTypeUtils.GetValue(editorType), "GetMovie")));
            }
            else
            {
                uploadImageUrl = APIPageUtils.ParseUrlWithCase(PageUtils.GetServicesUrl(AppManager.Platform.AppID, string.Format("textEditorUpload.aspx?EditorType={0}&FileType={1}", ETextEditorTypeUtils.GetValue(editorType), "Image")));
                uploadScrawlUrl = APIPageUtils.ParseUrlWithCase(PageUtils.GetServicesUrl(AppManager.Platform.AppID, string.Format("textEditorUpload.aspx?EditorType={0}&FileType={1}", ETextEditorTypeUtils.GetValue(editorType), "Scrawl")));
                uploadFileUrl = APIPageUtils.ParseUrlWithCase(PageUtils.GetServicesUrl(AppManager.Platform.AppID, string.Format("textEditorUpload.aspx?EditorType={0}&FileType={1}", ETextEditorTypeUtils.GetValue(editorType), "File")));
                imageManagerUrl = APIPageUtils.ParseUrlWithCase(PageUtils.GetServicesUrl(AppManager.Platform.AppID, string.Format("textEditorUpload.aspx?EditorType={0}&FileType={1}", ETextEditorTypeUtils.GetValue(editorType), "ImageManager")));
                getMovieUrl = APIPageUtils.ParseUrlWithCase(PageUtils.GetServicesUrl(AppManager.Platform.AppID, string.Format("textEditorUpload.aspx?EditorType={0}&FileType={1}", ETextEditorTypeUtils.GetValue(editorType), "GetMovie")));
            }

            return APIPageUtils.ParseUrlWithCase((PageUtils.GetSiteFilesUrl(PageUtils.Combine("bairong/texteditor", ETextEditorTypeUtils.GetValue(editorType)))));
        }

        //public static string ParseSSOUrl(string url)
        //{
        //    if (FileConfigManager.Instance.SSOConfig.AuthenticationType == EAuthenticationType.SSORemote)
        //    {
        //        return PageUtils.ParseNavigationUrl(url, FileConfigManager.Instance.SSOConfig.SSOUrl);
        //    }
        //    else
        //    {
        //        return PageUtils.ParseNavigationUrl(url);
        //    }
        //}

        public static string GetUserFilesUrl(string userName, string relatedUrl)
        {
            if (PageUtils.IsVirtualUrl(relatedUrl))
            {
                return PageUtils.ParseNavigationUrl(relatedUrl);
            }
            return PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.UserFiles, userName, relatedUrl);
        }

        public static string GetUserFileSystemManagementDirectoryUrl(string userName, string currentRootPath)
        {
            string directoryUrl = string.Empty;
            if (string.IsNullOrEmpty(currentRootPath) || !(currentRootPath.StartsWith("~/")))
            {
                currentRootPath = "~/" + currentRootPath;
            }
            string[] directoryNames = currentRootPath.Split('/');
            foreach (string directoryName in directoryNames)
            {
                if (!string.IsNullOrEmpty(directoryName))
                {
                    if (directoryName.Equals("~"))
                    {
                        directoryUrl = PageUtils.GetUserFilesUrl(string.Empty, userName);
                    }
                    else
                    {
                        directoryUrl = PageUtils.Combine(directoryUrl, directoryName);
                    }
                }

            }
            return directoryUrl;
        }

        /// <summary>
        /// 判断是否需要安装或升级，并转到页面。
        /// </summary>
        public static bool DetermineRedirectToInstaller()
        {
            if (ProductManager.IsNeedInstall())
            {
                PageUtils.Redirect(PageUtils.GetAdminDirectoryUrl("Installer"));
                return true;
            }
            return false;
        }

        public static void RedirectToErrorPage(string errorMessage)
        {
            PageUtils.Redirect(PageUtils.GetAdminDirectoryUrl(string.Format("error.aspx?ErrorMessage={0}", StringUtils.ValueToUrl(errorMessage))));
        }

        public static void CheckRequestParameter(params string[] parameters)
        {
            foreach (string parameter in parameters)
            {
                if (!string.IsNullOrEmpty(parameter) && HttpContext.Current.Request.QueryString[parameter] == null)
                {
                    PageUtils.RedirectToErrorPage(MessageUtils.PageErrorParameterIsNotCorrect);
                    return;
                }
            }
        }

        public static void RedirectToLoginPage()
        {
            PageUtils.RedirectToLoginPage(string.Empty);
        }

        public static void RedirectToLoginPage(string error)
        {
            string pageUrl = PageUtils.GetAdminDirectoryUrl("login.aspx");

            if (FileConfigManager.Instance.IsSaas)
            {
                if (FileConfigManager.Instance.SSOConfig.IntegrationType == EIntegrationType.GeXia)
                {
                    pageUrl = "http://www.gexia.com/home/login.html";
                }
                else if (FileConfigManager.Instance.SSOConfig.IntegrationType == EIntegrationType.QCloud)
                {
                    pageUrl = PageUtils.GetAdminDirectoryUrl("qcloud.aspx");
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                pageUrl = pageUrl + "?error=" + error;
            }
            PageUtils.Redirect(pageUrl);
        }

        public static void RedirectToLoadingPage(string pageUrl)
        {
            string url;
            string loadingPageUrl = PageUtils.GetAdminDirectoryUrl("loading.aspx?RedirectUrl={0}");
            if (pageUrl.IndexOf("?") != -1)
            {
                string redirectUrl = pageUrl.Substring(0, pageUrl.IndexOf("?"));
                url = string.Format(loadingPageUrl, redirectUrl);
                url = PageUtils.AddQueryString(url, PageUtils.GetQueryString(pageUrl));
            }
            else
            {
                string redirectUrl = pageUrl;
                url = string.Format(loadingPageUrl, redirectUrl);
            }
            PageUtils.Redirect(url);
        }

        public static string AddReturnUrl(string url, string returnUrl)
        {
            return PageUtils.AddQueryString(url, "ReturnUrl", returnUrl);
        }

        public static string GetRootUrlByPhysicalPath(string physicalPath)
        {
            string requestPath = PathUtils.GetPathDifference(ConfigUtils.Instance.PhysicalApplicationPath, physicalPath);
            requestPath = requestPath.Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar);
            return PageUtils.GetRootUrl(requestPath);
        }

        public static string GetSiteFilesUrl(string relatedUrl)
        {
            return PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, relatedUrl).ToLower();
        }

        public static string GetServicesUrl(string appID, string relatedUrl)
        {
            return PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.Services, appID, relatedUrl).ToLower();
        }

        //public static string GetStlIconUrl(string iconName)
        //{
        //    return PageUtils.ParseNavigationUrl(SiteFilesAbsolute.Icons.GetIcon(iconName));
        //}

        public static string GetAbsoluteSiteFilesUrl(string relatedUrl)
        {
            return PageUtils.Combine("~/sitefiles", relatedUrl);
        }

        public static string GetIconUrl(string iconName)
        {
            return PageUtils.GetSiteFilesUrl(PageUtils.Combine(SiteFiles.Directory.Icons, iconName));
        }

        public static string GetSiteFilesModuleUrl(string relatedUrl)
        {
            return PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, DirectoryUtils.SiteFiles.Module, relatedUrl);
        }

        public static string GetModuleUrl(string moduleID, string relatedUrl)
        {
            return PageUtils.ParseNavigationUrl(string.Format("~/{0}/{1}", moduleID, relatedUrl));
        }

        public static string ParseConfigRootUrl(string url)
        {
            if (ConfigManager.Instance.IsRelatedUrl)
            {
                return PageUtils.ParseNavigationUrl(url);
            }
            else
            {
                return PageUtils.ParseNavigationUrl(url, ConfigManager.Instance.RootUrl);
            }
        }

        public static string GetFileSystemManagementDirectoryUrl(string currentRootPath, string publishementSystemDir)
        {
            string directoryUrl = string.Empty;
            if (string.IsNullOrEmpty(currentRootPath) || !(currentRootPath.StartsWith("~/") || currentRootPath.StartsWith("@/")))
            {
                currentRootPath = "@/" + currentRootPath;
            }
            string[] directoryNames = currentRootPath.Split('/');
            foreach (string directoryName in directoryNames)
            {
                if (!string.IsNullOrEmpty(directoryName))
                {
                    if (directoryName.Equals("~"))
                    {
                        directoryUrl = ConfigUtils.Instance.ApplicationPath;
                    }
                    else if (directoryName.Equals("@"))
                    {
                        directoryUrl = PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, publishementSystemDir);
                    }
                    else
                    {
                        directoryUrl = PageUtils.Combine(directoryUrl, directoryName);
                    }
                }
            }
            return directoryUrl;
        }

        public static string GetAjaxServiceUrlByPage(string serviceName, string methodName)
        {
            return PageUtils.ParseNavigationUrl(string.Format("~/{0}/cms/services/{1}.aspx?type={2}", FileConfigManager.Instance.AdminDirectoryName, serviceName, methodName));
        }

        public static bool IsVirtualUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (url.StartsWith("~") || url.StartsWith("@"))
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetLoadingUrl(string url)
        {
            return PageUtils.GetAdminDirectoryUrl(string.Format("loading.aspx?RedirectType=Loading&RedirectUrl={0}", FilterXSS(url)));
        }

        public static string GetSafeHtmlFragment(string content)
        {
            return Microsoft.Security.Application.AntiXss.GetSafeHtmlFragment(content);
        }

        /// <summary> 
        ///sql和xss脚本过滤
        /// </summary> 
        /// <param name="input">传入字符串</param> 
        /// <returns>过滤后的字符串</returns> 
        public static string FilterSqlAndXss(string objStr)
        {
            return FilterXSS(FilterSql(objStr));
        }

        /// <summary>     
        /// 过滤xss攻击脚本     
        /// </summary>     
        /// <param name="input">传入字符串</param>     
        /// <returns>过滤后的字符串</returns>     
        public static string FilterXSS(string html)
        {
            string retval = html;
            if (!string.IsNullOrEmpty(retval))
            {
                retval = retval.Replace("@", "_at_");
                retval = retval.Replace("&", "_and_");
                retval = retval.Replace("#", "_sharp_");
                retval = retval.Replace(";", "_semicolon_");
                retval = retval.Replace(":", "_colon_");
                retval = retval.Replace("=", "_equal_");
                retval = retval.Replace("，", "_cn_comma_");
                retval = retval.Replace("“", "_quotel_");
                retval = retval.Replace("”", "_quoter_");
                retval = retval.Replace("/", "_slash_");
                retval = retval.Replace("|", "_or_");
                retval = retval.Replace("-", "_shortOne_");
                retval = retval.Replace(",", "_comma_");

                //中文标点符号
                retval = retval.Replace("；", "_cn_semicolon_");
                retval = retval.Replace("：", "_cn_colon_");
                retval = retval.Replace("。", "_cn_stop_");
                retval = retval.Replace("、", "_cn_tempstop_");
                retval = retval.Replace("？", "_cn_question_");
                retval = retval.Replace("《", "_cn_lbracket_");
                retval = retval.Replace("》", "_cn_rbracket_");
                retval = retval.Replace("‘", "_cn_rmark_");
                retval = retval.Replace("’", "_cn_lmark_");
                retval = retval.Replace("【", "_cn_slbracket_");
                retval = retval.Replace("】", "_cn_srbracket_");
                retval = retval.Replace("——", "_cn_extension_");
                retval = Microsoft.Security.Application.AntiXss.HtmlEncode(retval);
                //中文标点符号
                retval = retval.Replace("_cn_semicolon_", "；");
                retval = retval.Replace("_cn_colon_", "：");
                retval = retval.Replace("_cn_stop_", "。");
                retval = retval.Replace("_cn_tempstop_", "、");
                retval = retval.Replace("_cn_question_", "？");
                retval = retval.Replace("_cn_lbracket_", "《");
                retval = retval.Replace("_cn_rbracket_", "》");
                retval = retval.Replace("_cn_rmark_", "‘");
                retval = retval.Replace("_cn_lmark_", "’");
                retval = retval.Replace("_cn_slbracket_", "【");
                retval = retval.Replace("_cn_srbracket_", "】");
                retval = retval.Replace("_cn_extension_", "——");

                retval = retval.Replace("_at_", "@");
                retval = retval.Replace("_and_", "&");
                retval = retval.Replace("_sharp_", "#");
                retval = retval.Replace("_semicolon_", ";");
                retval = retval.Replace("_colon_", ":");
                retval = retval.Replace("_equal_", "=");
                retval = retval.Replace("_cn_comma_", "，");
                retval = retval.Replace("_quotel_", "“");
                retval = retval.Replace("_quoter_", "”");
                retval = retval.Replace("_slash_", "/");
                retval = retval.Replace("_or_", "|");
                retval = retval.Replace("_shortOne_", "-");
                retval = retval.Replace("_comma_", ",");
            }
            return retval;
        }

        //public static string FilterXSS(string html)
        //{
        //    if (string.IsNullOrEmpty(html)) return string.Empty;

        //    html = RegexUtils.RemoveScripts(html);

        //    // CR(0a) ，LF(0b) ，TAB(9) 除外，过滤掉所有的不打印出来字符.     
        //    // 目的防止这样形式的入侵 ＜java\0script＞     
        //    // 注意：\n, \r,  \t 可能需要单独处理，因为可能会要用到     
        //    string ret = System.Text.RegularExpressions.Regex.Replace(
        //        html, "([\x00-\x08][\x0b-\x0c][\x0e-\x20])", string.Empty);

        //    //替换所有可能的16进制构建的恶意代码     
        //    //<IMG SRC=&#X40&#X61&#X76&#X61&#X73&#X63&#X72&#X69&#X70&#X74
        //    //&#X3A&#X61&_#X6C&#X65&#X72&#X74&#X28&#X27&#X58&#X53&#X53&#X27&#X29>     
        //    string chars = "abcdefghijklmnopqrstuvwxyz" +
        //                "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" +
        //                "!@#$%^&*()~`;:?+/={}[]-_|'\"\\";
        //    for (int i = 0; i < chars.Length; i++)
        //    {
        //        ret =
        //            System.Text.RegularExpressions.Regex.Replace(ret,
        //                string.Concat("(&#[x|X]0{0,}",
        //                    Convert.ToString((int)chars[i], 16).ToLower(),
        //                    ";?)"),
        //                chars[i].ToString(),
        //                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        //    }

        //    //过滤\t, \n, \r构建的恶意代码   
        //    string[] keywords = {"javascript", "vbscript", "expression", 
        //        "applet", "meta", "xml", "blink", "link", "style",
        //        "script", "embed", "object", "iframe", "frame", 
        //        "frameset", "ilayer", "layer", "bgsound", "title",
        //        "base" ,"onabort", "onactivate", "onafterprint",
        //        "onafterupdate", "onbeforeactivate", "onbeforecopy", 
        //        "onbeforecut", "onbeforedeactivate", "onbeforeeditfocus",
        //        "onbeforepaste", "onbeforeprint", "onbeforeunload",
        //        "onbeforeupdate", "onblur", "onbounce", "oncellchange",
        //        "onchange", "onclick", "oncontextmenu", "oncontrolselect",
        //        "oncopy", "oncut", "ondataavailable", "ondatasetchanged", 
        //        "ondatasetcomplete", "ondblclick", "ondeactivate",
        //        "ondrag", "ondragend", "ondragenter", "ondragleave",
        //        "ondragover", "ondragstart", "ondrop", "onerror", 
        //        "onerrorupdate", "onfilterchange", "onfinish", 
        //        "onfocus", "onfocusin", "onfocusout", "onhelp", 
        //        "onkeydown", "onkeypress", "onkeyup", "onlayoutcomplete",
        //        "onload", "onlosecapture", "onmousedown", "onmouseenter", 
        //        "onmouseleave", "onmousemove", "onmouseout", "onmouseover",
        //        "onmouseup", "onmousewheel", "onmove", "onmoveend", 
        //        "onmovestart", "onpaste", "onpropertychange", 
        //        "onreadystatechange", "onreset", "onresize",
        //        "onresizeend", "onresizestart", "onrowenter", 
        //        "onrowexit", "onrowsdelete", "onrowsinserted",
        //        "onscroll", "onselect", "onselectionchange",
        //        "onselectstart", "onstart", "onstop", "onsubmit",
        //        "onunload"};

        //    bool found = true;
        //    while (found)
        //    {
        //        string retBefore = ret;
        //        for (int i = 0; i < keywords.Length; i++)
        //        {
        //            string pattern = "/";
        //            for (int j = 0; j < keywords[i].Length; j++)
        //            {
        //                if (j > 0)
        //                    pattern = string.Concat(pattern, '(', "(&#[x|X]0{0,8}([9][a][b]);?)?", "|(&#0{0,8}([9][10][13]);?)?", ")?");
        //                pattern = string.Concat(pattern, keywords[i][j]);
        //            }
        //            string replacement = string.Concat(keywords[i].Substring(0, 2), "＜x＞", keywords[i].Substring(2));
        //            ret = Regex.Replace(ret, pattern, replacement, RegexOptions.IgnoreCase);
        //            if (ret == retBefore)
        //                found = false;
        //        }

        //    }

        //    return ret;
        //}

        /// <summary> 
        /// 过滤sql攻击脚本 
        /// </summary> 
        /// <param name="input">传入字符串</param> 
        /// <returns>过滤后的字符串</returns> 
        public static string FilterSql(string objStr)
        {
            if (string.IsNullOrEmpty(objStr)) return string.Empty;

            bool isSqlExists = false;
            //string strSql = "',shell,cmd,alter,drop,union,exec,declare,delete,create,update,insert,select,dbo.,--,\\(,\\)";
            string strSql = "',--,\\(,\\)";
            string[] strSqls = strSql.Split(',');
            foreach (string sql in strSqls)
            {
                if (objStr.IndexOf(sql) != -1)
                {
                    isSqlExists = true;
                    break;
                }
            }
            if (isSqlExists)
            {
                //return lower.Replace("'", "''").Replace("shell", "s hell").Replace("cmd", "c md").Replace("alter", "a lter").Replace("drop", "d rop").Replace("union", "u nion").Replace("exec", "e xec").Replace("declare", "d eclare").Replace("delete", "d elete").Replace("create", "c reate").Replace("update", "u pdate").Replace("insert", "i nsert").Replace("select", "s elect").Replace("dbo.", ".d bo").Replace("--", "－－").Replace("\\(", "（").Replace("\\)", "）");
                return objStr.Replace("'", "’").Replace("--", "－－").Replace("\\(", "（").Replace("\\)", "）");
            }
            return objStr;
        }

        public static string GetPlatformSystemServiceUrl(string methodName)
        {
            return PageUtils.ParseNavigationUrl(string.Format("~/{0}/platform/services/SystemService.aspx?type={1}", FileConfigManager.Instance.AdminDirectoryName, methodName));
        }

        public static string GetPlatformUserServiceUrl(string methodName)
        {
            return PageUtils.ParseNavigationUrl(string.Format("~/{0}/platform/services/UserService.aspx?type={1}", FileConfigManager.Instance.AdminDirectoryName, methodName));
        }

        public static void ResponseToJSON(string jsonString)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.Write(jsonString);
            HttpContext.Current.Response.End();
        }

        public class API
        {
            private static string GetUrl(string relatedPath, string proco)
            {
                if (string.IsNullOrEmpty(proco))
                    return PageUtils.Combine(PageUtils.GetRootUrl("api"), relatedPath);
                else
                    return PageUtils.Combine(proco, relatedPath);
            }

            public static string GetPublishmentSystemClearCacheUrl()
            {
                return GetUrl("cache/clearPublishmentSystemCache", string.Empty);
            }

            public static string GetUserClearCacheUrl()
            {
                return GetUrl("cache/removeUserCache", string.Empty);
            }

            public static string GetTableStyleClearCacheUrl()
            {
                return GetUrl("cache/RemoveTableManagerCache", string.Empty);
            }

            public static string GetUserConfigClearCacheUrl()
            {
                return GetUrl("cache/RemoveUserConfigCache", string.Empty);
            }

            public static string GetValidateCodeUrl(bool isBigImage, string cookieName, bool isCorsCross, string proco)
            {
                return GetUrl(string.Format("/Platform/ValidateCode?isBigImage={0}&cookieName={1}&isCorsCross={2}", isBigImage, cookieName, isCorsCross), proco);
            }
        }

        public class Services
        {
            internal const string DIRECTORY_PATH = "sitefiles/services/platform";

            public static string GetUrl(string relatedPath)
            {
                if (string.IsNullOrEmpty(relatedPath))
                {
                    return PageUtils.GetRootUrl(DIRECTORY_PATH);
                }
                else
                {
                    return PageUtils.Combine(PageUtils.GetRootUrl(DIRECTORY_PATH), relatedPath);
                }
            }

            public static string GetPath(string relatedPath)
            {
                return PathUtils.MapPath(PathUtils.Combine(DIRECTORY_PATH, relatedPath));
            }
        }

        public static string GetUserAvatarUrl(string groupSN, string userName, EAvatarSize size)
        {
            string imageUrl = string.Empty;

            UserInfo userInfo = UserManager.GetUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                if (size == EAvatarSize.Large)
                {
                    imageUrl = userInfo.AvatarLarge;
                }
                else if (size == EAvatarSize.Middle)
                {
                    imageUrl = userInfo.AvatarMiddle;
                }
                else if (size == EAvatarSize.Small)
                {
                    imageUrl = userInfo.AvatarSmall;
                }

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    if (PageUtils.IsProtocolUrl(imageUrl))
                    {
                        return imageUrl;
                    }
                    else
                    {
                        return PageUtils.GetUserFilesUrl(userName, imageUrl);
                    }
                }
            }

            int r = StringUtils.GetRandomInt(1, 100);
            string url = string.Format("~/sitefiles/bairong/icons/avatars/atavar_small_{0}.jpg", r);
            if (size == EAvatarSize.Large)
            {
                url = string.Format("~/sitefiles/bairong/icons/avatars/atavar_large_{0}.jpg", r);
            }
            else if (size == EAvatarSize.Middle)
            {
                url = string.Format("~/sitefiles/bairong/icons/avatars/atavar_middle_{0}.jpg", r);
            }

            return PageUtils.ParseNavigationUrl(url);
        }

        public static string GetUserAvatarUrl(string groupSN, string userName, string fileName, EAvatarSize size)
        {
            string imageUrl = string.Empty;

            UserInfo userInfo = UserManager.GetUserInfo(groupSN, userName);
            if (userInfo != null)
            {
                if (size == EAvatarSize.Large)
                {
                    imageUrl = userInfo.AvatarLarge;
                }
                else if (size == EAvatarSize.Middle)
                {
                    imageUrl = userInfo.AvatarMiddle;
                }
                else if (size == EAvatarSize.Small)
                {
                    imageUrl = userInfo.AvatarSmall;
                }

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    if (PageUtils.IsProtocolUrl(imageUrl))
                    {
                        return imageUrl;
                    }
                    else
                    {
                        return PageUtils.GetUserFilesUrl(userName, imageUrl);
                    }
                }
            }

            int r = StringUtils.GetRandomInt(1, 100);
            string url = string.Format("~/sitefiles/bairong/icons/avatars/atavar_small_{0}.jpg", r);
            if (size == EAvatarSize.Large)
            {
                url = string.Format("~/sitefiles/bairong/icons/avatars/atavar_large_{0}.jpg", r);
            }
            else if (size == EAvatarSize.Middle)
            {
                url = string.Format("~/sitefiles/bairong/icons/avatars/atavar_middle_{0}.jpg", r);
            }

            return PageUtils.ParseNavigationUrl(url);
        }

        public static string GetUserUploadFileUrl(string userName, string relatedUrl)
        {
            string directoryUrl;
            EDateFormatType dateFormatType = EDateFormatTypeUtils.GetEnumType(UserConfigManager.Additional.UploadDateFormatString);
            DateTime datetime = DateTime.Now;
            string userFilesUrl = PageUtils.GetUserFilesUrl(userName, string.Empty);
            if (dateFormatType == EDateFormatType.Year)
            {
                directoryUrl = PageUtils.Combine(userFilesUrl, datetime.Year.ToString());
            }
            else if (dateFormatType == EDateFormatType.Day)
            {
                directoryUrl = PageUtils.Combine(userFilesUrl, datetime.Year.ToString(), datetime.Month.ToString(), datetime.Day.ToString());
            }
            else
            {
                directoryUrl = PageUtils.Combine(userFilesUrl, datetime.Year.ToString(), datetime.Month.ToString());
            }

            return PageUtils.Combine(directoryUrl, relatedUrl);
        }

        public static string SinaShortUrl(string url)
        {
            string builder = WebClientUtils.GetRemoteFileSource("http://api.t.sina.com.cn/short_url/shorten.json?source=1681459862&url_long=" + HttpContext.Current.Server.UrlEncode(url), ECharset.utf_8);
            builder = builder.Replace("[", "").Replace("]", "");
            return new JavaScriptSerializer().Deserialize<ShortUrl>(builder).url_short;
        }


        [Serializable]
        public class ShortUrl
        {
            /// <summary>
            /// 短域名
            /// </summary>
            public virtual string url_short { get; set; }

            /// <summary>
            /// 长域名
            /// </summary>
            public virtual string url_long { get; set; }

            /// <summary>
            /// 类型
            /// </summary>
            public virtual int type { get; set; }
        }
    }
}
