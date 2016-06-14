using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using BaiRong.Model;
using System.Collections.Generic;

namespace BaiRong.Core
{
    public class RegexUtils
    {

        /*
         * 通用：.*?
         * 所有链接：<a\s*.*?href=(?:"(?<url>[^"]*)"|'(?<url>[^']*)'|(?<url>\S+)).*?>
         * */

        public static RegexOptions OPTIONS = ((RegexOptions.Singleline | RegexOptions.IgnoreCase) | RegexOptions.IgnorePatternWhitespace);

        public static ArrayList GetImageSrcs(string baseUrl, string html)
        {
            string regex = "(img|input)[^><]*\\s+src\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))";
            return GetUrls(regex, html, baseUrl);
        }

        public static ArrayList GetOriginalImageSrcs(string html)
        {
            string regex = "(img|input)[^><]*\\s+src\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))";
            return GetContents("url", regex, html);
        }

        public static ArrayList GetFlashSrcs(string baseUrl, string html)
        {
            string regex = "embed\\s+[^><]*src\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))|param\\s+[^><]*value\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))";
            return GetUrls(regex, html, baseUrl);
        }

        public static ArrayList GetOriginalFlashSrcs(string html)
        {
            string regex = "embed\\s+[^><]*src\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))|param\\s+[^><]*value\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))";
            return GetContents("url", regex, html);
        }

        public static ArrayList GetStyleImageUrls(string baseUrl, string html)
        {
            string regex = "url\\((?<url>[^\\(\\)]*)\\)";
            ArrayList arraylist = GetUrls(regex, html, baseUrl);
            ArrayList urlArrayList = new ArrayList();
            foreach (string url in arraylist)
            {
                if (!urlArrayList.Contains(url) && EFileSystemTypeUtils.IsImage(PathUtils.GetExtension(url)))
                {
                    urlArrayList.Add(url);
                }
            }
            return urlArrayList;
        }

        public static ArrayList GetOriginalStyleImageUrls(string html)
        {
            //background-image: url(../images/leftline.gif);
            string regex = "url\\((?<url>[^\\(\\)]*)\\)";
            ArrayList arraylist = GetContents("url", regex, html);
            ArrayList urlArrayList = new ArrayList();
            foreach (string url in arraylist)
            {
                if (!urlArrayList.Contains(url) && EFileSystemTypeUtils.IsImage(PathUtils.GetExtension(url)))
                {
                    urlArrayList.Add(url);
                }
            }
            return urlArrayList;
        }

        public static ArrayList GetBackgroundImageSrcs(string baseUrl, string html)
        {
            string regex = "background\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))";
            return GetUrls(regex, html, baseUrl);
        }

        public static ArrayList GetOriginalBackgroundImageSrcs(string html)
        {
            string regex = "background\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))";
            return GetContents("url", regex, html);
        }

        public static ArrayList GetCssHrefs(string baseUrl, string html)
        {
            //string regex = "link\\s+[^><]*href\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>\\S+))|@import\\s*url\\((?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>\\S+))\\)";
            string regex = "link\\s+[^><]*href\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))|\\@import\\s*url\\s*\\(\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>.*?))\\s*\\)";
            return GetUrls(regex, html, baseUrl);
        }

        public static ArrayList GetOriginalCssHrefs(string html)
        {
            string regex = "link\\s+[^><]*href\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))|\\@import\\s*url\\s*\\(\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>.*?))\\s*\\)";
            return GetContents("url", regex, html);
        }

        public static ArrayList GetScriptSrcs(string baseUrl, string html)
        {
            string regex = "script\\s+[^><]*src\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))";
            return GetUrls(regex, html, baseUrl);
        }

        public static ArrayList GetOriginalScriptSrcs(string html)
        {
            string regex = "script\\s+[^><]*src\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*))";
            return GetContents("url", regex, html);
        }

        public static ArrayList GetTagInnerContents(string tagName, string html)
        {
            string regex = string.Format("<{0}\\s+[^><]*>\\s*(?<content>[\\s\\S]+?)\\s*</{0}>", tagName);
            return GetContents("content", regex, html);
        }

        public static List<string> GetTagContents(string tagName, string html)
        {
            List<string> list = new List<string>();

            string regex = string.Format(@"<({0})[^>]*>(.*?)</\1>|<{0}[^><]*/>", tagName);

            MatchCollection matches = Regex.Matches(html, regex, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    list.Add(match.Result("$0"));
                }
            }

            return list;
        }

        public static string GetTagName(string html)
        {
            Match match = Regex.Match(html, "<([^>\\s]+)[\\s\\SS]*>", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Result("$1");
            }
            return string.Empty;
        }

        public static string GetInnerContent(string tagName, string html)
        {
            string regex = string.Format("<{0}[^><]*>(?<content>[\\s\\S]+?)</{0}>", tagName);
            return GetContent("content", regex, html);
        }

        public static string GetAttributeContent(string attributeName, string html)
        {
            string regex = string.Format("<[^><]+\\s*{0}\\s*=\\s*(?:\"(?<value>[^\"]*)\"|'(?<value>[^']*)'|(?<value>[^>\\s]*)).*?>", attributeName);
            return GetContent("value", regex, html);
        }

        public static ArrayList GetUrls(string html, string baseUrl)
        {
            string regex = "<a\\s*.*?href\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*)).*?>";
            string regexEncode = "&lt;a\\s*.*?href\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*)).*?&gt;";
            //防止编码后，采集不到
            regex = string.Format("({0})|({1})", regex, regexEncode);
            return GetUrls(regex, html, baseUrl);
        }

        public static ArrayList GetUrls(string regex, string html, string baseUrl)
        {
            ArrayList urlArrayList = new ArrayList();
            if (string.IsNullOrEmpty(regex))
            {
                regex = "<a\\s*.*?href\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*)).*?>";
                string regexEncode = "&lt;a\\s*.*?href\\s*=\\s*(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>[^>\\s]*)).*?&gt;";
                //防止编码后，采集不到
                regex = string.Format("({0})|({1})", regex, regexEncode);
            }
            string groupName = "url";
            string domain = PageUtils.GetUrlWithoutPathInfo(baseUrl);
            ArrayList arraylist = GetContents(groupName, regex, html);
            foreach (string rawUrl in arraylist)
            {
                string url = PageUtils.GetUrlByBaseUrl(rawUrl, baseUrl);
                if (!string.IsNullOrEmpty(url) && !urlArrayList.Contains(url))
                {
                    urlArrayList.Add(url);
                }
            }
            return urlArrayList;
        }

        public static string GetUrl(string regex, string html, string baseUrl)
        {
            return PageUtils.GetUrlByBaseUrl(GetContent("url", regex, html), baseUrl);
        }

        public static string GetContent(string groupName, string regex, string html)
        {
            string content = string.Empty;
            if (regex == null || regex.Length == 0) return content;
            if (regex.IndexOf("<" + groupName + ">") == -1)
            {
                return regex;
            }

            Regex reg = new Regex(regex, OPTIONS);
            Match match = reg.Match(html);
            if (match.Success)
            {
                content = match.Groups[groupName].Value;
            }

            return content;
        }

        public static string Replace(string regex, string input, string replacement)
        {
            if (string.IsNullOrEmpty(input)) return input;
            Regex reg = new Regex(regex, OPTIONS);
            return reg.Replace(input, replacement);
        }

        public static string Replace(string regex, string input, string replacement, int count)
        {
            if (count == 0)
            {
                return Replace(regex, input, replacement);
            }
            else
            {
                if (string.IsNullOrEmpty(input)) return input;
                Regex reg = new Regex(regex, OPTIONS);
                return reg.Replace(input, replacement, count);
            }
        }

        public static bool IsMatch(string regex, string input)
        {
            Regex reg = new Regex(regex, OPTIONS);
            return reg.IsMatch(input);
        }

        public static ArrayList GetContents(string groupName, string regex, string html)
        {
            ArrayList arraylist = new ArrayList();
            if (regex == null || regex.Length == 0) return arraylist;
            Regex reg = new Regex(regex, OPTIONS);

            for (Match match = reg.Match(html); match.Success; match = match.NextMatch())
            {
                //arraylist.Add(match.Groups[groupName].Value);
                string theValue = match.Groups[groupName].Value;
                if (!arraylist.Contains(theValue))
                {
                    arraylist.Add(theValue);
                }
            }
            return arraylist;
        }

        public static string RemoveScripts(string html)
        {
            string regex = "<script[^><]*>.*?<\\/script>";
            return Replace(regex, html, string.Empty);
        }

        public static string RemoveImages(string html)
        {
            string regex = "<img[^><]*>";
            return Replace(regex, html, string.Empty);
        }
    }
}
