using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core;
using SiteServer.BBS.Model;

using System.Collections.Specialized;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Collections;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Core
{
    public class StringUtilityBBS
    {
        public static string GetFloorByTaxis(int taxis)
        {
            return GetFloor(taxis + 1);
        }

        public static string GetFloor(int itemIndex)
        {
            if (itemIndex == 1)
            {
                return "楼主";
            }
            else if (itemIndex == 2)
            {
                return "沙发";
            }
            else if (itemIndex == 3)
            {
                return "板凳";
            }
            else
            {
                return itemIndex + "楼";
            }
        }

        public static string GetFloorLinkHtml(int pageNum, int postID, int itemIndex)
        {
            return string.Format(@"<a id=""{0}"" class=""lz"" style=""cursor:pointer"" title=""复制此楼地址"" onclick=""copyFloorUrl({1}, '{0}')"">{2}</a>", postID, pageNum, StringUtilityBBS.GetFloor(itemIndex));
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                        startIndex = startIndex - length;
                }

                if (startIndex > str.Length)
                    return "";
            }
            else
            {
                if (length < 0)
                    return "";
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                        return "";
                }
            }

            if (str.Length - startIndex < length)
                length = str.Length - startIndex;

            return str.Substring(startIndex, length);
        }

        public static string GetTrueImageHtml(string isDefaultStr)
        {
            return GetTrueImageHtml(TranslateUtils.ToBool(isDefaultStr));
        }

        public static string GetTrueImageHtml(bool isDefault)
        {
            string retval = string.Empty;
            if (isDefault)
            {
                retval = "<img src='../pic/icon/right.gif' border='0'/>";
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
                retval = "<img src='../pic/icon/wrong.gif' border='0'/>";
            }
            return retval;
        }

        public static string GetTrueOrFalseImageHtml(string isDefaultStr)
        {
            return GetTrueOrFalseImageHtml(TranslateUtils.ToBool(isDefaultStr));
        }

        public static string GetTrueOrFalseImageHtml(bool isDefault)
        {
            string retval;
            if (isDefault)
            {
                retval = "<img src='../pic/icon/right.gif' border='0'/>";
            }
            else
                retval = "<img src='../pic/icon/wrong.gif' border='0'/>";
            return retval;
        }

        public static string GetShowImageScript(string imageClientID)
        {
            return GetShowImageScript("this", imageClientID);
        }

        public static string GetShowImageScript(string objString, string imageClientID)
        {
            return string.Format("ShowImg({0}, '{1}', '{2}')", objString, imageClientID, ConfigUtils.Instance.ApplicationPath);
        }

        public static void AddLog(int publishmentSystemID, string action)
        {
            StringUtilityBBS.AddLog(publishmentSystemID, 0, 0, 0, action, string.Empty);
        }

        public static void AddLog(int publishmentSystemID, string action, string summary)
        {
            StringUtilityBBS.AddLog(publishmentSystemID, 0, 0, 0, action, summary);
        }

        public static void AddLog(int publishmentSystemID, int forumID, int threadID, int postID, string action, string summary)
        {
            if (ConfigurationManager.GetAdditional(publishmentSystemID).IsLogBBS)
            {
                try
                {
                    if (!string.IsNullOrEmpty(action))
                    {
                        action = StringUtils.MaxLengthText(action, 250);
                    }
                    if (!string.IsNullOrEmpty(summary))
                    {
                        summary = StringUtils.MaxLengthText(summary, 250);
                    }
                    BBSLogInfo logInfo = new BBSLogInfo(0, publishmentSystemID, forumID, threadID, postID, BaiRongDataProvider.AdministratorDAO.UserName, PageUtils.GetIPAddress(), DateTime.Now, action, summary);
                    DataProvider.BBSLogDAO.Insert(logInfo);
                }
                catch { }
            }
        }

        public static string TextEditorContentEncode(int publishmentSystemID, string content)
        {
            return TextEditorContentEncode(publishmentSystemID, content, ConfigurationManager.GetAdditional(publishmentSystemID).IsSaveImageInTextEditor);
        }

        public static string TextEditorContentEncode(int publishmentSystemID, string content, bool isSaveImage)
        {
            string bbsUrl = PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Empty);
            string bbsUrl2 = PageUtils.AddProtocolToUrl(bbsUrl);
            if (isSaveImage)
            {
                content = PathUtilityBBS.SaveImage(publishmentSystemID, content);
            }

            StringBuilder builder = new StringBuilder(content);

            if (bbsUrl == string.Empty)
            {
                bbsUrl = "/";
            }
            //if (bbsUrl == "/")
            //{
            //    bbsUrl = string.Empty;
            //}

            builder.Replace("src=\"" + bbsUrl, "src=\"@");
            builder.Replace("src='" + bbsUrl, "src='@");
            builder.Replace("src=" + bbsUrl, "src=@");
            builder.Replace("src=&quot;" + bbsUrl, "src=&quot;@");

            builder.Replace("src=\"" + bbsUrl2, "src=\"@");
            builder.Replace("src='" + bbsUrl2, "src='@");
            builder.Replace("src=" + bbsUrl2, "src=@");
            builder.Replace("src=&quot;" + bbsUrl2, "src=&quot;@");

            builder.Replace("@'@", "'@");
            builder.Replace("@\"@", "\"@");

            return builder.ToString();
        }

        public static string TextEditorContentDecode(int publishmentSystemID, string content)
        {
            string bbsUrl = PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Empty);

            StringBuilder builder = new StringBuilder(content);

            if (bbsUrl == string.Empty)
            {
                bbsUrl = "/";
            }

            //if (bbsUrl == "/")
            //{
            //    bbsUrl = string.Empty;
            //}

            builder.Replace("src=\"@", "src=\"" + bbsUrl);
            builder.Replace("src='@", "src='" + bbsUrl);
            builder.Replace("src=@", "src=" + bbsUrl);
            builder.Replace("src=&quot;@", "src=&quot;" + bbsUrl);

            return builder.ToString();
        }

        public static bool IsImageExists(string content)
        {
            ArrayList originalImageSrcs = RegexUtils.GetOriginalImageSrcs(content);
            foreach (string originalImageSrc in originalImageSrcs)
            {
                if (!originalImageSrc.StartsWith("@editor/plugins/emoticons/") && !originalImageSrc.StartsWith("@smile/"))
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetFaceLinks(int publishmentSystemID)
        {
            StringBuilder builder = new StringBuilder();
            IList<FaceInfo> faceList = DataProvider.FaceDAO.GetFaces(publishmentSystemID, true);
            int index = 0;
            foreach (FaceInfo faceInfo in faceList)
            {
                string current = string.Empty;
                if (index == 0)
                {
                    current = @" class=""current""";
                    index++;
                }
                builder.AppendFormat(@"<li{0}><a id=""{1}"" href=""{2}"">{3}</a></li>", current, faceInfo.FaceName, PageUtilityBBS.GetAjaxPageUrl(publishmentSystemID, string.Format("face.aspx?publishmentSystemID={0}&faceName={1}", publishmentSystemID, faceInfo.FaceName)), faceInfo.Title);
            }
            return builder.ToString();
        }

        public static string GetFaceDefaultContents(int publishmentSystemID)
        {
            string faceName = DataProvider.FaceDAO.GetFirstFaceName(publishmentSystemID);
            if (!string.IsNullOrEmpty(faceName))
            {
                string directoryPath = PathUtility.GetPublishmentSystemPath(publishmentSystemID, "smile", faceName);
                string[] fileNames = DirectoryUtils.GetFileNames(directoryPath);
                if (fileNames != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendFormat(@"<div id=""tabs-{0}"" class=""face_info"">", faceName);
                    int totalNum = (fileNames.Length > 16) ? 16 : fileNames.Length;
                    for (int i = 0; i < totalNum; i++)
                    {
                        builder.AppendFormat(@"<a href=""javascript:void(0);""><img src=""{0}"" border=""0"" /></a>", PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("smile/{0}/{1}", faceName, fileNames[i])));
                    }
                    if (totalNum < fileNames.Length)
                    {
                        int pageNum = fileNames.Length / totalNum;
                        if (pageNum > 9) pageNum = 9;
                        builder.AppendFormat(@"<div class=""face_page""><span class=""face_cur_page"">1</span>");
                        for (int i = 2; i <= pageNum; i++)
                        {
                            builder.AppendFormat(@"<a href=""{0}"">{1}</a>", PageUtilityBBS.GetAjaxPageUrl(publishmentSystemID, string.Format("face.aspx?publishmentSystemID={0}&faceName={1}&page={2}", publishmentSystemID, faceName, i)), i);
                        }
                        builder.Append("</div>");
                    }
                    builder.Append("</div>");
                    return builder.ToString();
                }
            }
            return string.Empty;
        }

        public static string GetHighlightFormatString(bool isStrong, bool isEM, bool isU, string color)
        {
            return string.Format("{0}_{1}_{2}_{3}", isStrong, isEM, isU, color);
        }

        public static string GetHighlightStyle(string highlight)
        {
            string style = string.Empty;
            if (!string.IsNullOrEmpty(highlight))
            {
                string[] formats = highlight.Split('_');
                if (formats.Length == 4)
                {
                    bool isStrong = TranslateUtils.ToBool(formats[0]);
                    bool isEM = TranslateUtils.ToBool(formats[1]);
                    bool isU = TranslateUtils.ToBool(formats[2]);
                    string color = formats[3];

                    if (!string.IsNullOrEmpty(color))
                    {
                        style += string.Format(@"color:{0};", color);
                    }
                    if (isStrong)
                    {
                        style += "font-weight: bold;";
                    }
                    if (isEM)
                    {
                        style += "font-style: italic;";
                    }
                    if (isU)
                    {
                        style += "text-decoration: underline;";
                    }
                }
            }
            return style;
        }

        public static void GetHighlight(string highlight, out bool isStrong, out bool isEM, out bool isU, out string color)
        {
            isStrong = false;
            isEM = false;
            isU = false;
            color = string.Empty;
            if (!string.IsNullOrEmpty(highlight))
            {
                string[] formats = highlight.Split('_');
                if (formats.Length == 4)
                {
                    isStrong = TranslateUtils.ToBool(formats[0]);
                    isEM = TranslateUtils.ToBool(formats[1]);
                    isU = TranslateUtils.ToBool(formats[2]);
                    color = formats[3];
                }
            }
        }

        public static string Statistics(int publishmentSystemID)
        {
            bool isReCalc = false;
            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);
            if (string.IsNullOrEmpty(additional.StatDateTime))
            {
                isReCalc = true;
            }
            else
            {
                DateTime statDateTime = TranslateUtils.ToDateTime(additional.StatDateTime);
                if (statDateTime.DayOfYear != DateTime.Now.DayOfYear)
                {
                    isReCalc = true;
                }
            }

            if (isReCalc)
            {
                additional.StatTodayPostCount = DataProvider.PostDAO.GetPostCount(publishmentSystemID, DateTime.Now);
                additional.StatYesterdayPostCount = DataProvider.PostDAO.GetPostCount(publishmentSystemID, DateTime.Now.AddDays(-1));
                additional.StatPostCount = DataProvider.PostDAO.GetPostCount(publishmentSystemID);
                additional.StatUserCount = BaiRongDataProvider.UserDAO.GetTotalCount();
                additional.StatDateTime = DateUtils.GetDateString(DateTime.Now);

                ConfigurationManager.Update(publishmentSystemID);
            }

            string stats = string.Format(@"
今日: {0}<span class=""pipe"">|</span>昨日: {1}<span class=""pipe"">|</span>帖子: {2}<span class=""pipe"">|</span>会员: {3}", additional.StatTodayPostCount, additional.StatYesterdayPostCount, additional.StatPostCount, additional.StatUserCount);

            if (!string.IsNullOrEmpty(additional.StatUserNameRecently))
            {
                stats += string.Format(@"<span class=""pipe"">|</span>欢迎新会员: <a class=""xi2"" href=""{0}"" target=""_blank"">{1}</a>", UserUtils.GetInstance(publishmentSystemID).GetUserUrl(additional.StatUserNameRecently), additional.StatUserNameRecently);
            }

            return stats;
        }

        public static string GetSystemMessageContent(string action, string postTitle, DateTime addDate, string forumName, string reason)
        {
            return string.Format(@"
您发表的帖子被 {0} 执行 {1} 操作
<br /><br />
帖子：{2}
<br />
发表日期：{3}
<br />
所在版块：{4}
<br />
操作时间：{5}
<br />
操作理由：{6}
", BaiRongDataProvider.UserDAO.CurrentUserName, action, postTitle, DateUtils.GetDateAndTimeString(addDate), forumName, DateUtils.GetDateAndTimeString(DateTime.Now), reason);
        }

        public static string GetSystemMessageContent(string action, string reason)
        {
            return string.Format(@"
您的用户被 {0} 执行 {1} 操作
<br /><br />

操作时间：{2}
<br />
操作理由：{3}
", BaiRongDataProvider.UserDAO.CurrentUserName, action, DateUtils.GetDateAndTimeString(DateTime.Now), reason);
        }
    }
}
