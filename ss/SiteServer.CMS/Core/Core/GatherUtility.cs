using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using BaiRong.Core.Net;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;


namespace SiteServer.CMS.Core
{
    public class GatherUtility
    {
        public static string GetRegexString(string normalString)
        {
            string retval = normalString;
            if (!string.IsNullOrEmpty(normalString))
            {
                char[] replaceChar = new char[] { '\\', '^', '$', '.', '{', '[', '(', ')', ']', '}', '+', '?', '!', '#' };
                foreach (char theChar in replaceChar)
                {
                    retval = retval.Replace(theChar.ToString(), "\\" + theChar);
                }
                retval = retval.Replace("*", ".*?");
                retval = RegexUtils.Replace("\\s+", retval, "\\s+");
            }
            return retval;
        }

        public static string GetRegexArea(string normalAreaStart, string normalAreaEnd)
        {
            if (!string.IsNullOrEmpty(normalAreaStart) && !string.IsNullOrEmpty(normalAreaEnd))
            {
                return string.Format("{0}\\s*(?<area>[\\s\\S]+?)\\s*{1}", GetRegexString(normalAreaStart), GetRegexString(normalAreaEnd));
            }
            return string.Empty;
        }

        public static string GetRegexUrl(string normalUrlStart, string normalUrlEnd)
        {
            if (!string.IsNullOrEmpty(normalUrlStart) && !string.IsNullOrEmpty(normalUrlEnd))
            {
                return string.Format("{0}(?:\"(?<url>[^\"]*)\"|'(?<url>[^']*)'|(?<url>\\S+)){1}", GetRegexString(normalUrlStart), GetRegexString(normalUrlEnd));
            }
            return string.Empty;
        }

        public static string GetRegexChannel(string normalChannelStart, string normalChannelEnd)
        {
            if (!string.IsNullOrEmpty(normalChannelStart) && !string.IsNullOrEmpty(normalChannelEnd))
            {
                return string.Format("{0}\\s*(?<channel>[\\s\\S]+?)\\s*{1}", GetRegexString(normalChannelStart), GetRegexString(normalChannelEnd));
            }
            return string.Empty;
        }

        public static string GetRegexTitle(string normalTitleStart, string normalTitleEnd)
        {
            if (!string.IsNullOrEmpty(normalTitleStart) && !string.IsNullOrEmpty(normalTitleEnd))
            {
                return string.Format("{0}\\s*(?<title>[\\s\\S]+?)\\s*{1}", GetRegexString(normalTitleStart), GetRegexString(normalTitleEnd));
            }
            return string.Empty;
        }

        public static string GetRegexContent(string normalContentStart, string normalContentEnd)
        {
            if (!string.IsNullOrEmpty(normalContentStart) && !string.IsNullOrEmpty(normalContentEnd))
            {
                return string.Format("{0}\\s*(?<content>[\\s\\S]+?)\\s*{1}", GetRegexString(normalContentStart), GetRegexString(normalContentEnd));
            }
            return string.Empty;
        }

        public static string GetRegexAttributeName(string attributeName, string normalAuthorStart, string normalAuthorEnd)
        {
            if (!string.IsNullOrEmpty(normalAuthorStart) && !string.IsNullOrEmpty(normalAuthorEnd))
            {
                return string.Format("{0}\\s*(?<{1}>[\\s\\S]+?)\\s*{2}", GetRegexString(normalAuthorStart), attributeName, GetRegexString(normalAuthorEnd));
            }
            return string.Empty;
        }

        public static ArrayList GetGatherUrlArrayList(GatherRuleInfo gatherRuleInfo)
        {
            ArrayList gatherUrls = new ArrayList();
            if (gatherRuleInfo.GatherUrlIsCollection)
            {
                gatherUrls.AddRange(TranslateUtils.StringCollectionToArrayList(gatherRuleInfo.GatherUrlCollection, '\n'));
            }

            if (gatherRuleInfo.GatherUrlIsSerialize)
            {
                if (gatherRuleInfo.SerializeFrom <= gatherRuleInfo.SerializeTo)
                {
                    int count = 1;
                    for (int i = gatherRuleInfo.SerializeFrom; i <= gatherRuleInfo.SerializeTo; i = i + gatherRuleInfo.SerializeInterval)
                    {
                        count++;
                        if (count > 200) break;
                        string thePageNumber = i.ToString();
                        if (gatherRuleInfo.SerializeIsAddZero && thePageNumber.Length == 1)
                        {
                            thePageNumber = "0" + i;
                        }
                        gatherUrls.Add(gatherRuleInfo.GatherUrlSerialize.Replace("*", thePageNumber));
                    }
                }

                if (gatherRuleInfo.SerializeIsOrderByDesc)
                {
                    gatherUrls.Reverse();
                }
            }

            return gatherUrls;
        }

        public static ArrayList GetContentUrlArrayList(GatherRuleInfo gatherRuleInfo, string regexListArea, string regexUrlInclude, bool isCache, string cacheMessageKey, StringBuilder errorBuilder)
        {
            ArrayList gatherUrls = GatherUtility.GetGatherUrlArrayList(gatherRuleInfo);
            ArrayList contentUrls = new ArrayList();
            foreach (string gatherUrl in gatherUrls)
            {
                if (isCache)
                {
                    CacheUtils.Max(cacheMessageKey, "获取链接" + gatherUrl);//存储消息
                }
                contentUrls.AddRange(GatherUtility.GetContentUrls(gatherUrl, gatherRuleInfo.Charset, gatherRuleInfo.CookieString, regexListArea, regexUrlInclude, errorBuilder));
            }

            if (gatherRuleInfo.Additional.IsOrderByDesc)
            {
                contentUrls.Reverse();
            }
            return contentUrls;
        }

        public static ArrayList GetContentUrls(string gatherUrl, ECharset charset, string cookieString, string regexListArea, string regexUrlInclude, StringBuilder errorBuilder)
        {
            ArrayList contentUrls = new ArrayList();
            try
            {
                string listHtml = WebClientUtils.GetRemoteFileSource(gatherUrl, charset, cookieString);
                string areaHtml = string.Empty;

                if (!string.IsNullOrEmpty(regexListArea))
                {
                    areaHtml = RegexUtils.GetContent("area", regexListArea, listHtml);
                }
                ArrayList urlsArrayList;
                if (!string.IsNullOrEmpty(areaHtml))
                {
                    urlsArrayList = RegexUtils.GetUrls(areaHtml, gatherUrl);
                }
                else
                {
                    urlsArrayList = RegexUtils.GetUrls(listHtml, gatherUrl);
                }

                bool isInclude = !string.IsNullOrEmpty(regexUrlInclude);

                foreach (string url in urlsArrayList)
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        string contentUrl = url.Replace("&amp;", "&");
                        if (isInclude && !RegexUtils.IsMatch(regexUrlInclude, contentUrl))
                        {
                            continue;
                        }
                        if (!contentUrls.Contains(contentUrl))
                        {
                            contentUrls.Add(contentUrl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorBuilder.Append("<br/>").Append(ex.Message);
                LogUtils.AddErrorLog(ex);
            }
            return contentUrls;
        }

        public static NameValueCollection GetContentNameValueCollection(ECharset charset, string url, string cookieString, string regexContentExclude, string contentHtmlClearCollection, string contentHtmlClearTagCollection, string regexTitle, string regexContent, string regexContent2, string regexContent3, string regexNextPage, string regexChannel, ArrayList contentAttributes, NameValueCollection contentAttributesXML)
        {
            NameValueCollection attributes = new NameValueCollection();

            string contentHtml = WebClientUtils.GetRemoteFileSource(url, charset, cookieString);
            string title = RegexUtils.GetContent("title", regexTitle, contentHtml);
            string content = RegexUtils.GetContent("content", regexContent, contentHtml);
            if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(regexContent2))
            {
                content = RegexUtils.GetContent("content", regexContent2, contentHtml);
            }
            if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(regexContent3))
            {
                content = RegexUtils.GetContent("content", regexContent3, contentHtml);
            }

            if (!string.IsNullOrEmpty(regexContentExclude))
            {
                content = RegexUtils.Replace(regexContentExclude, content, string.Empty);
            }
            if (!string.IsNullOrEmpty(contentHtmlClearCollection))
            {
                ArrayList htmlClearArrayList = TranslateUtils.StringCollectionToArrayList(contentHtmlClearCollection);
                foreach (string htmlClear in htmlClearArrayList)
                {
                    string clearRegex = string.Format(@"<{0}[^>]*>.*?<\/{0}>", htmlClear);
                    content = RegexUtils.Replace(clearRegex, content, string.Empty);
                }
            }
            if (!string.IsNullOrEmpty(contentHtmlClearTagCollection))
            {
                ArrayList htmlClearTagArrayList = TranslateUtils.StringCollectionToArrayList(contentHtmlClearTagCollection);
                foreach (string htmlClearTag in htmlClearTagArrayList)
                {
                    string clearRegex = string.Format(@"<{0}[^>]*>", htmlClearTag);
                    content = RegexUtils.Replace(clearRegex, content, string.Empty);
                    clearRegex = string.Format(@"<\/{0}>", htmlClearTag);
                    content = RegexUtils.Replace(clearRegex, content, string.Empty);
                }
            }

            string contentNextPageUrl = RegexUtils.GetUrl(regexNextPage, contentHtml, url);
            if (!string.IsNullOrEmpty(contentNextPageUrl))
            {
                content = GatherUtility.GetPageContent(content, charset, contentNextPageUrl, cookieString, regexContentExclude, contentHtmlClearCollection, contentHtmlClearTagCollection, regexContent, regexContent2, regexContent3, regexNextPage);
            }

            string channel = RegexUtils.GetContent("channel", regexChannel, contentHtml);

            attributes.Add("title", title);
            attributes.Add("channel", channel);
            attributes.Add("content", StringUtils.HtmlEncode(content));

            foreach (string attributeName in contentAttributes)
            {
                string normalStart = StringUtils.ValueFromUrl(contentAttributesXML[attributeName + "_ContentStart"]);
                string normalEnd = StringUtils.ValueFromUrl(contentAttributesXML[attributeName + "_ContentEnd"]);
                string regex = GatherUtility.GetRegexAttributeName(attributeName, normalStart, normalEnd);
                string value = RegexUtils.GetContent(attributeName, regex, contentHtml);
                attributes.Set(attributeName, value);
            }

            return attributes;
        }

        public static bool GatherOneByUrl(PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, bool isSaveImage, bool isSetFirstImageAsImageUrl, bool isEmptyContentAllowed, bool isSameTitleAllowed, bool isChecked, ECharset charset, string url, string cookieString, string regexTitleInclude, string regexContentExclude, string contentHtmlClearCollection, string contentHtmlClearTagCollection, string contentReplaceFrom, string contentReplaceTo, string regexTitle, string regexContent, string regexContent2, string regexContent3, string regexNextPage, string regexChannel, ArrayList contentAttributes, NameValueCollection contentAttributesXML, Hashtable contentTitleHashtable, ArrayList nodeIDAndContentIDArrayList, bool isCache, string cacheMessageKey)
        {
            try
            {
                //TODO:采集文件、链接标题为内容标题、链接提示为内容标题
                //string extension = PathUtils.GetExtension(url);
                //if (!EFileSystemTypeUtils.IsTextEditable(extension))
                //{
                //    if (EFileSystemTypeUtils.IsImageOrFlashOrPlayer(extension))
                //    {

                //    }
                //}
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                string contentHtml = WebClientUtils.GetRemoteFileSource(url, charset, cookieString);
                string title = RegexUtils.GetContent("title", regexTitle, contentHtml);
                string content = RegexUtils.GetContent("content", regexContent, contentHtml);
                if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(regexContent2))
                {
                    content = RegexUtils.GetContent("content", regexContent2, contentHtml);
                }
                if (string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(regexContent3))
                {
                    content = RegexUtils.GetContent("content", regexContent3, contentHtml);
                }

                //如果标题或内容为空，返回false并退出
                if (string.IsNullOrEmpty(title))
                {
                    return false;
                }
                if (isEmptyContentAllowed == false && string.IsNullOrEmpty(content))
                {
                    return false;
                }

                title = StringUtils.StripTags(title);

                if (!string.IsNullOrEmpty(regexTitleInclude))
                {
                    if (RegexUtils.IsMatch(regexTitleInclude, title) == false)
                    {
                        return false;
                    }
                }
                if (!string.IsNullOrEmpty(regexContentExclude))
                {
                    content = RegexUtils.Replace(regexContentExclude, content, string.Empty);
                }
                if (!string.IsNullOrEmpty(contentHtmlClearCollection))
                {
                    ArrayList htmlClearArrayList = TranslateUtils.StringCollectionToArrayList(contentHtmlClearCollection);
                    foreach (string htmlClear in htmlClearArrayList)
                    {
                        string clearRegex = string.Format(@"<{0}[^>]*>.*?<\/{0}>", htmlClear);
                        content = RegexUtils.Replace(clearRegex, content, string.Empty);
                    }
                }
                if (!string.IsNullOrEmpty(contentHtmlClearTagCollection))
                {
                    ArrayList htmlClearTagArrayList = TranslateUtils.StringCollectionToArrayList(contentHtmlClearTagCollection);
                    foreach (string htmlClearTag in htmlClearTagArrayList)
                    {
                        string clearRegex = string.Format(@"<{0}[^>]*>", htmlClearTag);
                        content = RegexUtils.Replace(clearRegex, content, string.Empty);
                        clearRegex = string.Format(@"<\/{0}>", htmlClearTag);
                        content = RegexUtils.Replace(clearRegex, content, string.Empty);
                    }
                }

                if (!string.IsNullOrEmpty(contentReplaceFrom))
                {
                    StringCollection froms = TranslateUtils.StringCollectionToStringCollection(contentReplaceFrom);
                    bool isMulti = false;
                    if (!string.IsNullOrEmpty(contentReplaceTo) && contentReplaceTo.IndexOf(',') != -1)
                    {
                        if (StringUtils.GetCount(",", contentReplaceTo) + 1 == froms.Count)
                        {
                            isMulti = true;
                        }
                    }
                    if (isMulti == false)
                    {
                        foreach (string from in froms)
                        {
                            title = RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><]*>)", from.Replace(" ", "\\s")), title, contentReplaceTo);
                            content = RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><]*>)", from.Replace(" ", "\\s")), content, contentReplaceTo);
                        }
                    }
                    else
                    {
                        StringCollection tos = TranslateUtils.StringCollectionToStringCollection(contentReplaceTo);
                        for (int i = 0; i < froms.Count; i++)
                        {
                            title = RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><]*>)", froms[i].Replace(" ", "\\s")), title, tos[i]);
                            content = RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><]*>)", froms[i].Replace(" ", "\\s")), content, tos[i]);
                        }
                    }
                }

                string contentNextPageUrl = RegexUtils.GetUrl(regexNextPage, contentHtml, url);
                if (!string.IsNullOrEmpty(contentNextPageUrl))
                {
                    try
                    {
                        content = GatherUtility.GetPageContent(content, charset, contentNextPageUrl, cookieString, regexContentExclude, contentHtmlClearCollection, contentHtmlClearTagCollection, regexContent, regexContent2, regexContent3, regexNextPage);
                    }
                    catch
                    {
                        return false;
                    }
                }

                string channel = RegexUtils.GetContent("channel", regexChannel, contentHtml);
                int channelID = nodeInfo.NodeID;
                if (!string.IsNullOrEmpty(channel))
                {
                    int nodeIDByNodeName = DataProvider.NodeDAO.GetNodeIDByParentIDAndNodeName(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, channel, false);
                    if (nodeIDByNodeName == 0)
                    {
                        channelID = DataProvider.NodeDAO.InsertNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, channel, string.Empty, nodeInfo.ContentModelID);
                    }
                    else
                    {
                        channelID = nodeIDByNodeName;
                    }
                }

                if (!isSameTitleAllowed)
                {
                    ArrayList contentTitles = contentTitleHashtable[channelID] as ArrayList;
                    if (contentTitles == null)
                    {
                        contentTitles = BaiRongDataProvider.ContentDAO.GetValueArrayList(tableName, channelID, ContentAttribute.Title);
                    }

                    if (contentTitles.Contains(title))
                    {
                        return false;
                    }

                    contentTitles.Add(title);
                    contentTitleHashtable[channelID] = contentTitles;
                }

                BackgroundContentInfo contentInfo = new BackgroundContentInfo();
                contentInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                contentInfo.NodeID = channelID;
                contentInfo.AddUserName = AdminManager.Current.UserName;
                contentInfo.AddDate = DateTime.Now;
                contentInfo.LastEditUserName = contentInfo.AddUserName;
                contentInfo.LastEditDate = contentInfo.AddDate;
                contentInfo.IsChecked = isChecked;
                contentInfo.CheckedLevel = 0;

                contentInfo.Title = title;

                foreach (string attributeName in contentAttributes)
                {
                    if (!StringUtils.EqualsIgnoreCase(attributeName, ContentAttribute.Title) && !StringUtils.EqualsIgnoreCase(attributeName, BackgroundContentAttribute.Content))
                    {
                        string normalStart = StringUtils.ValueFromUrl(contentAttributesXML[attributeName + "_ContentStart"]);
                        string normalEnd = StringUtils.ValueFromUrl(contentAttributesXML[attributeName + "_ContentEnd"]);

                        //采集为空时的默认值
                        string normalDefault = StringUtils.ValueFromUrl(contentAttributesXML[attributeName + "_ContentDefault"]);

                        string regex = GatherUtility.GetRegexAttributeName(attributeName, normalStart, normalEnd);
                        string value = RegexUtils.GetContent(attributeName, regex, contentHtml);

                        //采集为空时的默认值
                        if (string.IsNullOrEmpty(value))
                        {
                            value = normalDefault;
                        }

                        if (BackgroundContentAttribute.SystemAttributes.Contains(attributeName))
                        {
                            if (StringUtils.EqualsIgnoreCase(ContentAttribute.AddDate, attributeName))
                            {
                                contentInfo.AddDate = TranslateUtils.ToDateTime(value, DateTime.Now);
                            }
                            else if (StringUtils.EqualsIgnoreCase(BackgroundContentAttribute.IsColor, attributeName))
                            {
                                contentInfo.IsColor = TranslateUtils.ToBool(value, false);
                            }
                            else if (StringUtils.EqualsIgnoreCase(BackgroundContentAttribute.IsHot, attributeName))
                            {
                                contentInfo.IsHot = TranslateUtils.ToBool(value, false);
                            }
                            else if (StringUtils.EqualsIgnoreCase(BackgroundContentAttribute.IsRecommend, attributeName))
                            {
                                contentInfo.IsRecommend = TranslateUtils.ToBool(value, false);
                            }
                            else if (StringUtils.EqualsIgnoreCase(ContentAttribute.IsTop, attributeName))
                            {
                                contentInfo.IsTop = TranslateUtils.ToBool(value, false);
                            }
                            else if (StringUtils.EqualsIgnoreCase(BackgroundContentAttribute.ImageUrl, attributeName))
                            {
                                if (!string.IsNullOrEmpty(value))
                                {
                                    string attachmentUrl = PageUtils.GetUrlByBaseUrl(value, url);
                                    string fileName = PathUtility.GetUploadFileName(publishmentSystemInfo, attachmentUrl);
                                    string fileExtension = PageUtils.GetExtensionFromUrl(attachmentUrl);
                                    string directoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, fileExtension);
                                    DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
                                    string filePath = PathUtils.Combine(directoryPath, fileName);
                                    try
                                    {
                                        WebClientUtils.SaveRemoteFileToLocal(attachmentUrl, filePath);
                                        contentInfo.ImageUrl = PageUtility.GetPublishmentSystemVirtualUrlByPhysicalPath(publishmentSystemInfo, filePath);
                                    }
                                    catch { }
                                }
                            }
                            else if (StringUtils.EqualsIgnoreCase(BackgroundContentAttribute.VideoUrl, attributeName))
                            {
                                if (!string.IsNullOrEmpty(value))
                                {
                                    string attachmentUrl = PageUtils.GetUrlByBaseUrl(value, url);
                                    string fileExtName = PageUtils.GetExtensionFromUrl(attachmentUrl);
                                    string fileName = PathUtility.GetUploadFileName(publishmentSystemInfo, attachmentUrl);
                                    string directoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, fileExtName);
                                    DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
                                    string filePath = PathUtils.Combine(directoryPath, fileName);
                                    try
                                    {
                                        WebClientUtils.SaveRemoteFileToLocal(attachmentUrl, filePath);
                                        contentInfo.VideoUrl = PageUtility.GetPublishmentSystemVirtualUrlByPhysicalPath(publishmentSystemInfo, filePath);
                                    }
                                    catch { }
                                }
                            }
                            else if (StringUtils.EqualsIgnoreCase(BackgroundContentAttribute.FileUrl, attributeName))
                            {
                                if (!string.IsNullOrEmpty(value))
                                {
                                    string attachmentUrl = PageUtils.GetUrlByBaseUrl(value, url);
                                    string fileExtName = PageUtils.GetExtensionFromUrl(attachmentUrl);
                                    string fileName = PathUtility.GetUploadFileName(publishmentSystemInfo, attachmentUrl);
                                    string directoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, fileExtName);
                                    DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
                                    string filePath = PathUtils.Combine(directoryPath, fileName);
                                    try
                                    {
                                        WebClientUtils.SaveRemoteFileToLocal(attachmentUrl, filePath);
                                        contentInfo.FileUrl = PageUtility.GetPublishmentSystemVirtualUrlByPhysicalPath(publishmentSystemInfo, filePath);
                                    }
                                    catch { }
                                }
                            }
                            else if (StringUtils.EqualsIgnoreCase(ContentAttribute.Hits, attributeName))
                            {
                                contentInfo.Hits = TranslateUtils.ToInt(value);
                            }
                            else
                            {
                                contentInfo.SetExtendedAttribute(attributeName, value);
                            }
                        }
                        else
                        {
                            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, attributeName, null);
                            value = InputTypeParser.GetContentByTableStyle(value, publishmentSystemInfo, ETableStyle.BackgroundContent, styleInfo);

                            if (styleInfo.InputType == EInputType.Image || styleInfo.InputType == EInputType.Video || styleInfo.InputType == EInputType.File)
                            {
                                if (!string.IsNullOrEmpty(value))
                                {
                                    string attachmentUrl = PageUtils.GetUrlByBaseUrl(value, url);
                                    string fileExtension = PathUtils.GetExtension(attachmentUrl);
                                    string fileName = PathUtility.GetUploadFileName(publishmentSystemInfo, attachmentUrl);
                                    string directoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, fileExtension);
                                    DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
                                    string filePath = PathUtils.Combine(directoryPath, fileName);
                                    try
                                    {
                                        WebClientUtils.SaveRemoteFileToLocal(attachmentUrl, filePath);
                                        value = PageUtility.GetPublishmentSystemVirtualUrlByPhysicalPath(publishmentSystemInfo, filePath);
                                    }
                                    catch { }
                                }
                            }

                            contentInfo.SetExtendedAttribute(attributeName, value);
                        }
                    }
                }

                if (string.IsNullOrEmpty(contentInfo.ImageUrl))
                {
                    string firstImageUrl = string.Empty;
                    if (isSaveImage)
                    {
                        ArrayList originalImageSrcs = RegexUtils.GetOriginalImageSrcs(content);
                        ArrayList imageSrcs = RegexUtils.GetImageSrcs(url, content);
                        if (originalImageSrcs.Count == imageSrcs.Count)
                        {
                            for (int i = 0; i < originalImageSrcs.Count; i++)
                            {
                                string originalImageSrc = (string)originalImageSrcs[i];
                                string imageSrc = (string)imageSrcs[i];
                                string fileName = PathUtility.GetUploadFileName(publishmentSystemInfo, imageSrc);
                                string fileExtName = PathUtils.GetExtension(originalImageSrc);
                                string directoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, contentInfo.AddDate, fileExtName);
                                DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
                                string filePath = PathUtils.Combine(directoryPath, fileName);
                                try
                                {
                                    WebClientUtils.SaveRemoteFileToLocal(imageSrc, filePath);
                                    string fileUrl = PageUtility.GetPublishmentSystemVirtualUrlByPhysicalPath(publishmentSystemInfo, filePath);
                                    content = content.Replace(originalImageSrc, fileUrl);
                                    if (firstImageUrl == string.Empty)
                                    {
                                        firstImageUrl = fileUrl;
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    else if (isSetFirstImageAsImageUrl)
                    {
                        ArrayList imageSrcs = RegexUtils.GetImageSrcs(url, content);
                        if (imageSrcs.Count > 0)
                        {
                            firstImageUrl = (string)imageSrcs[0];
                        }
                    }

                    if (isSetFirstImageAsImageUrl)
                    {
                        contentInfo.ImageUrl = firstImageUrl;
                    }
                }
                //contentInfo.Content = StringUtility.TextEditorContentEncode(content, publishmentSystemInfo, false);
                contentInfo.Content = content;

                contentInfo.SourceID = SourceManager.CaiJi;

                int theContentID = DataProvider.ContentDAO.Insert(tableName, publishmentSystemInfo, contentInfo);
                nodeIDAndContentIDArrayList.Add(new int[] { contentInfo.NodeID, theContentID });

                if (isCache)
                {
                    CacheUtils.Max(cacheMessageKey, "采集内容：" + title);//存储消息
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        private static string GetPageContent(string previousPageContent, ECharset charset, string url, string cookieString, string regexContentExclude, string contentHtmlClearCollection, string contentHtmlClearTagCollection, string regexContent, string regexContent2, string regexContent3, string regexNextPage)
        {
            string content = previousPageContent;
            string contentHtml = WebClientUtils.GetRemoteFileSource(url, charset, cookieString);
            string nextPageContent = RegexUtils.GetContent("content", regexContent, contentHtml);
            if (string.IsNullOrEmpty(nextPageContent) && !string.IsNullOrEmpty(regexContent2))
            {
                nextPageContent = RegexUtils.GetContent("content", regexContent2, contentHtml);
            }
            if (string.IsNullOrEmpty(nextPageContent) && !string.IsNullOrEmpty(regexContent3))
            {
                nextPageContent = RegexUtils.GetContent("content", regexContent3, contentHtml);
            }

            if (!string.IsNullOrEmpty(nextPageContent))
            {
                if (string.IsNullOrEmpty(content))
                {
                    content += nextPageContent;
                }
                else
                {
                    content += ContentUtility.PagePlaceHolder + nextPageContent;
                }
            }

            if (!string.IsNullOrEmpty(regexContentExclude))
            {
                content = RegexUtils.Replace(regexContentExclude, content, string.Empty);
            }
            if (!string.IsNullOrEmpty(contentHtmlClearCollection))
            {
                ArrayList htmlClearArrayList = TranslateUtils.StringCollectionToArrayList(contentHtmlClearCollection);
                foreach (string htmlClear in htmlClearArrayList)
                {
                    string clearRegex = string.Format(@"<{0}[^>]*>.*?<\/{0}>", htmlClear);
                    content = RegexUtils.Replace(clearRegex, content, string.Empty);
                }
            }
            if (!string.IsNullOrEmpty(contentHtmlClearTagCollection))
            {
                ArrayList htmlClearTagArrayList = TranslateUtils.StringCollectionToArrayList(contentHtmlClearTagCollection);
                foreach (string htmlClearTag in htmlClearTagArrayList)
                {
                    string clearRegex = string.Format(@"<{0}[^>]*>", htmlClearTag);
                    content = RegexUtils.Replace(clearRegex, content, string.Empty);
                    clearRegex = string.Format(@"<\/{0}>", htmlClearTag);
                    content = RegexUtils.Replace(clearRegex, content, string.Empty);
                }
            }

            string contentNextPageUrl = RegexUtils.GetUrl(regexNextPage, contentHtml, url);
            if (!string.IsNullOrEmpty(contentNextPageUrl))
            {
                if (StringUtils.EqualsIgnoreCase(url, contentNextPageUrl))
                {
                    contentNextPageUrl = string.Empty;
                }
            }
            if (!string.IsNullOrEmpty(contentNextPageUrl))
            {
                return GetPageContent(content, charset, contentNextPageUrl, cookieString, regexContentExclude, contentHtmlClearCollection, contentHtmlClearTagCollection, regexContent, regexContent2, regexContent3, regexNextPage);
            }
            else
            {
                return content;
            }
        }


        #region 外部调用

        public const string CACHE_TOTAL_COUNT = "_TotalCount";
        public const string CACHE_CURRENT_COUNT = "_CurrentCount";
        public const string CACHE_MESSAGE = "_Message";

        public static void GatherWeb(int publishmentSystemID, string gatherRuleName, StringBuilder resultBuilder, StringBuilder errorBuilder, bool isCache, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            if (isCache)
            {
                CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
                CacheUtils.Max(cacheMessageKey, "开始获取链接...");//存储消息
            }

            int totalCount;
            int currentCount = 0;

            GatherRuleInfo gatherRuleInfo = DataProvider.GatherRuleDAO.GetGatherRuleInfo(gatherRuleName, publishmentSystemID);

            if (!DataProvider.NodeDAO.IsExists(gatherRuleInfo.NodeID))
            {
                gatherRuleInfo.NodeID = publishmentSystemID;
            }

            string regexUrlInclude = GatherUtility.GetRegexString(gatherRuleInfo.UrlInclude);
            string regexTitleInclude = GatherUtility.GetRegexString(gatherRuleInfo.TitleInclude);
            string regexContentExclude = GatherUtility.GetRegexString(gatherRuleInfo.ContentExclude);
            string regexListArea = GatherUtility.GetRegexArea(gatherRuleInfo.ListAreaStart, gatherRuleInfo.ListAreaEnd);
            string regexChannel = GatherUtility.GetRegexChannel(gatherRuleInfo.ContentChannelStart, gatherRuleInfo.ContentChannelEnd);
            string regexContent = GatherUtility.GetRegexContent(gatherRuleInfo.ContentContentStart, gatherRuleInfo.ContentContentEnd);
            string regexContent2 = string.Empty;
            if (!string.IsNullOrEmpty(gatherRuleInfo.Additional.ContentContentStart2) && !string.IsNullOrEmpty(gatherRuleInfo.Additional.ContentContentEnd2))
            {
                regexContent2 = GatherUtility.GetRegexContent(gatherRuleInfo.Additional.ContentContentStart2, gatherRuleInfo.Additional.ContentContentEnd2);
            }
            string regexContent3 = string.Empty;
            if (!string.IsNullOrEmpty(gatherRuleInfo.Additional.ContentContentStart3) && !string.IsNullOrEmpty(gatherRuleInfo.Additional.ContentContentEnd3))
            {
                regexContent3 = GatherUtility.GetRegexContent(gatherRuleInfo.Additional.ContentContentStart3, gatherRuleInfo.Additional.ContentContentEnd3);
            }
            string regexNextPage = GatherUtility.GetRegexUrl(gatherRuleInfo.ContentNextPageStart, gatherRuleInfo.ContentNextPageEnd);
            string regexTitle = GatherUtility.GetRegexTitle(gatherRuleInfo.ContentTitleStart, gatherRuleInfo.ContentTitleEnd);
            ArrayList contentAttributes = TranslateUtils.StringCollectionToArrayList(gatherRuleInfo.ContentAttributes);
            NameValueCollection contentAttributesXML = TranslateUtils.ToNameValueCollection(gatherRuleInfo.ContentAttributesXML);

            ArrayList contentUrls = GatherUtility.GetContentUrlArrayList(gatherRuleInfo, regexListArea, regexUrlInclude, isCache, cacheMessageKey, errorBuilder);

            if (gatherRuleInfo.Additional.GatherNum > 0)
            {
                totalCount = gatherRuleInfo.Additional.GatherNum;
            }
            else
            {
                totalCount = contentUrls.Count;
            }

            if (isCache)
            {
                CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                CacheUtils.Max(cacheMessageKey, "开始采集内容...");//存储消息
            }

            Hashtable contentTitleHashtable = new Hashtable();

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, gatherRuleInfo.NodeID);
            ArrayList nodeIDAndContentIDArrayList = new ArrayList();

            foreach (string contentUrl in contentUrls)
            {
                if (GatherUtility.GatherOneByUrl(publishmentSystemInfo, nodeInfo, gatherRuleInfo.Additional.IsSaveImage, gatherRuleInfo.Additional.IsSetFirstImageAsImageUrl, gatherRuleInfo.Additional.IsEmptyContentAllowed, gatherRuleInfo.Additional.IsSameTitleAllowed, gatherRuleInfo.Additional.IsChecked, gatherRuleInfo.Charset, contentUrl, gatherRuleInfo.CookieString, regexTitleInclude, regexContentExclude, gatherRuleInfo.ContentHtmlClearCollection, gatherRuleInfo.ContentHtmlClearTagCollection, gatherRuleInfo.Additional.ContentReplaceFrom, gatherRuleInfo.Additional.ContentReplaceTo, regexTitle, regexContent, regexContent2, regexContent3, regexNextPage, regexChannel, contentAttributes, contentAttributesXML, contentTitleHashtable, nodeIDAndContentIDArrayList, isCache, cacheMessageKey))
                {
                    currentCount++;
                    if (isCache)
                    {
                        CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                    }
                }
                if (currentCount == totalCount) break;
            }

            if (gatherRuleInfo.Additional.IsChecked)
            {
                for (int i = 0; i < nodeIDAndContentIDArrayList.Count; i++)
                {
                    try
                    {
                        int[] nodeIDAndContentID = (int[])nodeIDAndContentIDArrayList[i];

                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(publishmentSystemID, EChangedType.Add, ETemplateType.ContentTemplate, nodeIDAndContentID[0], nodeIDAndContentID[1], 0);
                        if (gatherRuleInfo.Additional.IsAutoCreate)
                            //采集属于自动处理，所以可以异步生成，不过需要判断是否启用异步生成配置
                            AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }
                    catch { }
                }
            }

            DataProvider.GatherRuleDAO.UpdateLastGatherDate(gatherRuleName, publishmentSystemID);

            resultBuilder.AppendFormat("任务完成，<strong> {1} </strong>栏目共采集内容<strong> {0} </strong>篇。请手动生成页面。<br/>", currentCount, nodeInfo.NodeName);

            if (isCache)
            {
                CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
                CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
                CacheUtils.Remove(cacheMessageKey);//取消存储消息
            }
        }

        public static void GatherDatabase(int publishmentSystemID, string gatherRuleName, StringBuilder resultBuilder, StringBuilder errorBuilder, bool isCache, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            if (isCache)
            {
                CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
                CacheUtils.Max(cacheMessageKey, "开始连接数据库...");//存储消息
            }

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            try
            {
                int totalCount;
                int currentCount = 0;

                GatherDatabaseRuleInfo gatherDatabaseRuleInfo = DataProvider.GatherDatabaseRuleDAO.GetGatherDatabaseRuleInfo(gatherRuleName, publishmentSystemID);
                TableMatchInfo tableMatchInfo = BaiRongDataProvider.TableMatchDAO.GetTableMatchInfo(gatherDatabaseRuleInfo.TableMatchID);

                if (!DataProvider.NodeDAO.IsExists(gatherDatabaseRuleInfo.NodeID))
                {
                    gatherDatabaseRuleInfo.NodeID = publishmentSystemID;
                }

                IDatabaseDAO databaseDAO = BaiRongDataProvider.CreateDatabaseDAO(gatherDatabaseRuleInfo.DatabaseType);
                if (gatherDatabaseRuleInfo.GatherNum > 0)
                {
                    totalCount = gatherDatabaseRuleInfo.GatherNum;
                }
                else
                {
                    totalCount = databaseDAO.GetIntResult(gatherDatabaseRuleInfo.ConnectionString, string.Format("SELECT COUNT(*) FROM {0}", gatherDatabaseRuleInfo.RelatedTableName));
                }

                if (isCache)
                {
                    CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                    CacheUtils.Max(cacheMessageKey, "开始采集内容...");//存储消息
                }

                ArrayList nodeIDAndContentIDArrayList = new ArrayList();

                string whereString = string.Empty;
                if (!string.IsNullOrEmpty(gatherDatabaseRuleInfo.WhereString))
                {
                    if (gatherDatabaseRuleInfo.WhereString.Trim().ToLower().StartsWith("where "))
                    {
                        whereString = gatherDatabaseRuleInfo.WhereString;
                    }
                    else
                    {
                        whereString = "WHERE " + gatherDatabaseRuleInfo.WhereString;
                    }
                }
                string sqlString = string.Format("SELECT * FROM {0} {1}", gatherDatabaseRuleInfo.RelatedTableName, whereString);

                string tableName = NodeManager.GetTableName(publishmentSystemInfo, gatherDatabaseRuleInfo.NodeID);

                ArrayList titleArrayList = BaiRongDataProvider.ContentDAO.GetValueArrayList(tableName, gatherDatabaseRuleInfo.NodeID, ContentAttribute.Title);

                using (System.Data.IDataReader rdr = databaseDAO.GetDataReader(gatherDatabaseRuleInfo.ConnectionString, sqlString))
                {
                    while (rdr.Read())
                    {
                        try
                        {
                            NameValueCollection collection = new NameValueCollection();
                            databaseDAO.ReadResultsToNameValueCollection(rdr, collection);
                            BackgroundContentInfo contentInfo = Converter.ToBackgroundContentInfo(collection, tableMatchInfo.ColumnsMap);
                            if (contentInfo != null && !string.IsNullOrEmpty(contentInfo.Title) && !titleArrayList.Contains(contentInfo.Title))
                            {
                                contentInfo.PublishmentSystemID = publishmentSystemID;
                                contentInfo.NodeID = gatherDatabaseRuleInfo.NodeID;
                                if (contentInfo.AddDate == DateTime.MinValue || contentInfo.AddDate == DateUtils.SqlMinValue)
                                {
                                    contentInfo.AddDate = DateTime.Now;
                                }
                                contentInfo.AddUserName = AdminManager.Current.UserName;
                                contentInfo.LastEditDate = contentInfo.AddDate;
                                contentInfo.LastEditUserName = contentInfo.AddUserName;
                                contentInfo.IsChecked = gatherDatabaseRuleInfo.IsChecked;
                                contentInfo.CheckedLevel = 0;

                                contentInfo.SourceID = SourceManager.CaiJi;

                                int theContentID = DataProvider.ContentDAO.Insert(tableName, publishmentSystemInfo, contentInfo);
                                nodeIDAndContentIDArrayList.Add(new int[] { contentInfo.NodeID, theContentID });

                                currentCount++;
                                if (isCache)
                                {
                                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                }
                            }
                        }
                        catch { }
                        if (currentCount == totalCount) break;
                    }
                    rdr.Close();
                }

                if (gatherDatabaseRuleInfo.IsChecked)
                {
                    for (int i = 0; i < nodeIDAndContentIDArrayList.Count; i++)
                    {
                        int[] nodeIDAndContentID = (int[])nodeIDAndContentIDArrayList[i];

                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(publishmentSystemID, EChangedType.Add, ETemplateType.ContentTemplate, nodeIDAndContentID[0], nodeIDAndContentID[1], 0);
                        if (gatherDatabaseRuleInfo.IsAutoCreate)
                            //采集属于自动处理，所以可以异步生成，不过需要判断是否启用异步生成配置
                            AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }
                }

                DataProvider.GatherRuleDAO.UpdateLastGatherDate(gatherRuleName, publishmentSystemID);
                string nodeName = NodeManager.GetNodeName(gatherDatabaseRuleInfo.PublishmentSystemID, gatherDatabaseRuleInfo.NodeID);
                resultBuilder.AppendFormat("任务完成，<strong> {1} </strong>栏目共采集内容<strong> {0} </strong>篇。请手动生成页面。<br />", currentCount, nodeName);
            }
            catch (Exception ex)
            {
                errorBuilder.Append(ex.Message);
                LogUtils.AddErrorLog(ex);
            }

            if (isCache)
            {
                CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
                CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
                CacheUtils.Remove(cacheMessageKey);//取消存储消息
            }
        }


        public static void GatherFile(int publishmentSystemID, string gatherRuleName, StringBuilder resultBuilder, StringBuilder errorBuilder, bool isCache, string userKeyPrefix)
        {
            string cacheTotalCountKey = userKeyPrefix + CACHE_TOTAL_COUNT;
            string cacheCurrentCountKey = userKeyPrefix + CACHE_CURRENT_COUNT;
            string cacheMessageKey = userKeyPrefix + CACHE_MESSAGE;

            if (isCache)
            {
                CacheUtils.Max(cacheTotalCountKey, "0");//存储需要的页面总数
                CacheUtils.Max(cacheCurrentCountKey, "0");//存储当前的页面总数
                CacheUtils.Max(cacheMessageKey, "开始获取内容...");//存储消息
            }

            try
            {
                int totalCount;
                int currentCount = 0;

                GatherFileRuleInfo gatherFileRuleInfo = DataProvider.GatherFileRuleDAO.GetGatherFileRuleInfo(gatherRuleName, publishmentSystemID);
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

                if (gatherFileRuleInfo.IsToFile)
                {
                    string gatherFilePath = PathUtility.MapPath(publishmentSystemInfo, gatherFileRuleInfo.FilePath);
                    string publishmentSystemPath = PathUtility.GetPublishmentSystemPath(publishmentSystemInfo);
                    int level = StringUtils.GetCount(PathUtils.SeparatorChar.ToString(), PathUtils.GetPathDifference(publishmentSystemPath, gatherFilePath));
                    string fileContent = WebClientUtils.GetRemoteFileSource(gatherFileRuleInfo.GatherUrl, gatherFileRuleInfo.Charset);
                    if (gatherFileRuleInfo.IsRemoveScripts)
                    {
                        fileContent = RegexUtils.RemoveScripts(fileContent);
                    }
                    if (gatherFileRuleInfo.IsSaveRelatedFiles)
                    {
                        string styleDirectoryPath = PathUtility.MapPath(publishmentSystemInfo, gatherFileRuleInfo.StyleDirectoryPath);
                        string scriptDirectoryPath = PathUtility.MapPath(publishmentSystemInfo, gatherFileRuleInfo.ScriptDirectoryPath);
                        string imageDirectoryPath = PathUtility.MapPath(publishmentSystemInfo, gatherFileRuleInfo.ImageDirectoryPath);
                        DirectoryUtils.CreateDirectoryIfNotExists(styleDirectoryPath);
                        DirectoryUtils.CreateDirectoryIfNotExists(scriptDirectoryPath);
                        DirectoryUtils.CreateDirectoryIfNotExists(imageDirectoryPath);

                        ArrayList originalCssHrefs = RegexUtils.GetOriginalCssHrefs(fileContent);
                        ArrayList originalScriptSrcs = RegexUtils.GetOriginalScriptSrcs(fileContent);
                        ArrayList originalImageSrcs = RegexUtils.GetOriginalImageSrcs(fileContent);
                        ArrayList originalFlashSrcs = RegexUtils.GetOriginalFlashSrcs(fileContent);
                        ArrayList originalStyleImageUrls = RegexUtils.GetOriginalStyleImageUrls(fileContent);
                        ArrayList originalBackgroundImageSrcs = RegexUtils.GetOriginalBackgroundImageSrcs(fileContent);

                        totalCount = originalCssHrefs.Count + originalScriptSrcs.Count + originalImageSrcs.Count + originalFlashSrcs.Count + originalStyleImageUrls.Count + originalBackgroundImageSrcs.Count;
                        if (isCache)
                        {
                            CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                            CacheUtils.Max(cacheMessageKey, "保存文件：" + gatherFileRuleInfo.FilePath);//存储消息
                        }

                        ArrayList cssHrefs = RegexUtils.GetCssHrefs(gatherFileRuleInfo.GatherUrl, fileContent);
                        for (int i = 0; i < originalCssHrefs.Count; i++)
                        {
                            try
                            {
                                string originalLinkHref = (string)originalCssHrefs[i];
                                string cssHref = (string)cssHrefs[i];

                                string fileUrl = GatherUtility.GatherCss(publishmentSystemInfo, publishmentSystemPath, imageDirectoryPath, styleDirectoryPath, level, gatherFileRuleInfo.Charset, cssHref);
                                fileContent = fileContent.Replace(originalLinkHref, fileUrl);
                                currentCount++;
                                if (isCache)
                                {
                                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                    CacheUtils.Max(cacheMessageKey, "保存Css样式文件：" + PathUtils.GetFileName(cssHref));//存储消息
                                }

                                //string originalLinkHref = (string)originalCssHrefs[i];
                                //string cssHref = (string)cssHrefs[i];

                                //string fileName = PageUtils.UrlDecode(PathUtils.GetFileName(cssHref));
                                //string filePath = PathUtils.Combine(styleDirectoryPath, fileName);

                                //string styleContent = WebClientUtils.GetRemoteFileSource(cssHref, gatherFileRuleInfo.Charset);
                                //ArrayList originalStyleImageUrls_i = RegexUtils.GetOriginalStyleImageUrls(styleContent);
                                //ArrayList styleImageUrls_i = RegexUtils.GetStyleImageUrls(PageUtils.GetUrlWithoutFileName(cssHref), styleContent);
                                //for (int j = 0; j < originalStyleImageUrls_i.Count; j++)
                                //{
                                //    string originalStyleImageUrl = (string)originalStyleImageUrls_i[j];
                                //    string styleImageUrl = (string)styleImageUrls_i[j];
                                //    string fileName_j = PageUtils.UrlDecode(PathUtils.GetFileName(styleImageUrl));
                                //    string filePath_j = PathUtils.Combine(imageDirectoryPath, fileName_j);
                                //    try
                                //    {
                                //        WebClientUtils.SaveRemoteFileToLocal(styleImageUrl, filePath_j);
                                //        string fileUrl_j = PageUtility.GetPublishmentSystemUrlOfRelatedByPhysicalPath(publishmentSystemInfo, filePath_j, level);
                                //        styleContent = styleContent.Replace(originalStyleImageUrl, fileUrl_j);
                                //    }
                                //    catch { }
                                //}

                                //FileUtils.WriteText(filePath, gatherFileRuleInfo.Charset, styleContent);

                                //string fileUrl = PageUtility.GetPublishmentSystemUrlOfRelatedByPhysicalPath(publishmentSystemInfo, filePath, level);
                                //fileContent = fileContent.Replace(originalLinkHref, fileUrl);
                                //currentCount++;
                                //if (isCache)
                                //{
                                //    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                //    CacheUtils.Max(cacheMessageKey, "保存Css样式文件：" + fileName);//存储消息
                                //}
                            }
                            catch { }
                        }

                        ArrayList scriptSrcs = RegexUtils.GetScriptSrcs(gatherFileRuleInfo.GatherUrl, fileContent);
                        for (int i = 0; i < originalScriptSrcs.Count; i++)
                        {
                            try
                            {
                                string originalScriptSrc = (string)originalScriptSrcs[i];
                                string scriptSrc = (string)scriptSrcs[i];
                                string fileName = PageUtils.UrlDecode(PathUtils.GetFileName(scriptSrc));
                                string filePath = PathUtils.Combine(scriptDirectoryPath, fileName);

                                WebClientUtils.SaveRemoteFileToLocal(scriptSrc, filePath);
                                string fileUrl = PageUtility.GetPublishmentSystemUrlOfRelatedByPhysicalPath(publishmentSystemInfo, filePath, level);
                                fileContent = fileContent.Replace(originalScriptSrc, fileUrl);
                                currentCount++;
                                if (isCache)
                                {
                                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                    CacheUtils.Max(cacheMessageKey, "保存Js脚本文件：" + fileName);//存储消息
                                }
                            }
                            catch { }
                        }

                        ArrayList imageSrcs = RegexUtils.GetImageSrcs(gatherFileRuleInfo.GatherUrl, fileContent);
                        for (int i = 0; i < originalImageSrcs.Count; i++)
                        {
                            try
                            {
                                string originalImageSrc = (string)originalImageSrcs[i];
                                string imageSrc = (string)imageSrcs[i];
                                string fileName = PageUtils.UrlDecode(PathUtils.GetFileName(imageSrc));
                                string filePath = PathUtils.Combine(imageDirectoryPath, fileName);

                                WebClientUtils.SaveRemoteFileToLocal(imageSrc, filePath);
                                string fileUrl = PageUtility.GetPublishmentSystemUrlOfRelatedByPhysicalPath(publishmentSystemInfo, filePath, level);
                                fileContent = fileContent.Replace(originalImageSrc, fileUrl);
                                currentCount++;
                                if (isCache)
                                {
                                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                    CacheUtils.Max(cacheMessageKey, "保存图片文件：" + fileName);//存储消息
                                }
                            }
                            catch { }
                        }

                        ArrayList flashSrcs = RegexUtils.GetFlashSrcs(gatherFileRuleInfo.GatherUrl, fileContent);
                        for (int i = 0; i < originalFlashSrcs.Count; i++)
                        {
                            try
                            {
                                string originalFlashSrc = (string)originalFlashSrcs[i];
                                string flashSrc = (string)flashSrcs[i];
                                string fileName = PageUtils.UrlDecode(PathUtils.GetFileName(flashSrc));
                                string filePath = PathUtils.Combine(imageDirectoryPath, fileName);

                                WebClientUtils.SaveRemoteFileToLocal(flashSrc, filePath);
                                string fileUrl = PageUtility.GetPublishmentSystemUrlOfRelatedByPhysicalPath(publishmentSystemInfo, filePath, level);
                                fileContent = fileContent.Replace(originalFlashSrc, fileUrl);
                                currentCount++;
                                if (isCache)
                                {
                                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                    CacheUtils.Max(cacheMessageKey, "保存Flash文件：" + fileName);//存储消息
                                }
                            }
                            catch { }
                        }

                        ArrayList styleImageUrls = RegexUtils.GetStyleImageUrls(gatherFileRuleInfo.GatherUrl, fileContent);
                        for (int j = 0; j < originalStyleImageUrls.Count; j++)
                        {
                            try
                            {
                                string originalStyleImageUrl = (string)originalStyleImageUrls[j];
                                string styleImageUrl = (string)styleImageUrls[j];
                                string fileName_j = PageUtils.UrlDecode(PathUtils.GetFileName(styleImageUrl));
                                string filePath_j = PathUtils.Combine(imageDirectoryPath, fileName_j);

                                WebClientUtils.SaveRemoteFileToLocal(styleImageUrl, filePath_j);
                                string fileUrl_j = PageUtility.GetPublishmentSystemUrlOfRelatedByPhysicalPath(publishmentSystemInfo, filePath_j, level);
                                fileContent = fileContent.Replace(originalStyleImageUrl, fileUrl_j);
                                currentCount++;
                                if (isCache)
                                {
                                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                    CacheUtils.Max(cacheMessageKey, "保存图片文件：" + fileName_j);//存储消息
                                }
                            }
                            catch { }
                        }

                        ArrayList backgroundImageSrcs = RegexUtils.GetBackgroundImageSrcs(gatherFileRuleInfo.GatherUrl, fileContent);
                        for (int j = 0; j < originalBackgroundImageSrcs.Count; j++)
                        {
                            try
                            {
                                string originalBackgroundImageSrc = (string)originalBackgroundImageSrcs[j];
                                string backgroundImageSrc = (string)backgroundImageSrcs[j];
                                string fileName_j = PageUtils.UrlDecode(PathUtils.GetFileName(backgroundImageSrc));
                                string filePath_j = PathUtils.Combine(imageDirectoryPath, fileName_j);

                                WebClientUtils.SaveRemoteFileToLocal(backgroundImageSrc, filePath_j);
                                string fileUrl_j = PageUtility.GetPublishmentSystemUrlOfRelatedByPhysicalPath(publishmentSystemInfo, filePath_j, level);
                                fileContent = fileContent.Replace(originalBackgroundImageSrc, fileUrl_j);
                                currentCount++;
                                if (isCache)
                                {
                                    CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                                    CacheUtils.Max(cacheMessageKey, "保存图片文件：" + fileName_j);//存储消息
                                }
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        totalCount = 1;
                        if (isCache)
                        {
                            CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                        }
                    }

                    FileUtils.WriteText(gatherFilePath, gatherFileRuleInfo.Charset, fileContent);

                    resultBuilder.AppendFormat("任务完成。");
                }
                else
                {

                    if (!DataProvider.NodeDAO.IsExists(gatherFileRuleInfo.NodeID))
                    {
                        gatherFileRuleInfo.NodeID = publishmentSystemID;
                    }

                    string regexContentExclude = GatherUtility.GetRegexString(gatherFileRuleInfo.ContentExclude);
                    string regexContent = GatherUtility.GetRegexContent(gatherFileRuleInfo.ContentContentStart, gatherFileRuleInfo.ContentContentEnd);
                    string regexTitle = GatherUtility.GetRegexTitle(gatherFileRuleInfo.ContentTitleStart, gatherFileRuleInfo.ContentTitleEnd);
                    ArrayList contentAttributes = TranslateUtils.StringCollectionToArrayList(gatherFileRuleInfo.ContentAttributes);
                    NameValueCollection contentAttributesXML = TranslateUtils.ToNameValueCollection(gatherFileRuleInfo.ContentAttributesXML);

                    totalCount = 1;

                    if (isCache)
                    {
                        CacheUtils.Max(cacheTotalCountKey, totalCount.ToString());//存储需要的页面总数
                        CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        CacheUtils.Max(cacheMessageKey, "开始采集内容...");//存储消息
                    }

                    Hashtable contentTitleHashtable = new Hashtable();
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, gatherFileRuleInfo.NodeID);
                    ArrayList nodeIDAndContentIDArrayList = new ArrayList();

                    if (GatherUtility.GatherOneByUrl(publishmentSystemInfo, nodeInfo, gatherFileRuleInfo.IsSaveImage, false, true, true, gatherFileRuleInfo.IsChecked, gatherFileRuleInfo.Charset, gatherFileRuleInfo.GatherUrl, string.Empty, string.Empty, regexContentExclude, gatherFileRuleInfo.ContentHtmlClearCollection, gatherFileRuleInfo.ContentHtmlClearTagCollection, string.Empty, string.Empty, regexTitle, regexContent, string.Empty, string.Empty, string.Empty, string.Empty, contentAttributes, contentAttributesXML, contentTitleHashtable, nodeIDAndContentIDArrayList, isCache, cacheMessageKey))
                    {
                        currentCount++;
                        if (isCache)
                        {
                            CacheUtils.Max(cacheCurrentCountKey, currentCount.ToString());//存储当前的页面总数
                        }
                    }

                    if (gatherFileRuleInfo.IsChecked)
                    {
                        for (int i = 0; i < nodeIDAndContentIDArrayList.Count; i++)
                        {
                            int[] nodeIDAndContentID = (int[])nodeIDAndContentIDArrayList[i];

                            string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(publishmentSystemID, EChangedType.Add, ETemplateType.ContentTemplate, nodeIDAndContentID[0], nodeIDAndContentID[1], 0);
                            if (gatherFileRuleInfo.IsAutoCreate)
                                //采集属于自动处理，所以可以异步生成，不过需要判断是否启用异步生成配置
                                AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                        }
                    }

                    resultBuilder.AppendFormat("任务完成，<strong> {1} </strong>栏目共采集内容<strong> {0} </strong>篇。请手动生成页面。<br />", currentCount, nodeInfo.NodeName);
                }
                DataProvider.GatherFileRuleDAO.UpdateLastGatherDate(gatherRuleName, publishmentSystemID);
            }
            catch (Exception ex)
            {
                errorBuilder.Append(ex.Message);
                LogUtils.AddErrorLog(ex);
            }

            if (isCache)
            {
                CacheUtils.Remove(cacheTotalCountKey);//取消存储需要的页面总数
                CacheUtils.Remove(cacheCurrentCountKey);//取消存储当前的页面总数
                CacheUtils.Remove(cacheMessageKey);//取消存储消息
            }
        }

        private static string GatherCss(PublishmentSystemInfo publishmentSystemInfo, string publishmentSystemPath, string imageDirectoryPath, string styleDirectoryPath, int topLevel, ECharset charset, string cssUrl)
        {
            string fileUrl = cssUrl;
            try
            {
                string fileName = PageUtils.UrlDecode(PathUtils.GetFileName(cssUrl));
                string filePath = PathUtils.Combine(styleDirectoryPath, fileName);

                int level = StringUtils.GetCount(PathUtils.SeparatorChar.ToString(), PathUtils.GetPathDifference(publishmentSystemPath, filePath));

                string styleContent = WebClientUtils.GetRemoteFileSource(cssUrl, charset);

                //开始采集CSS内部导入的CSS
                ArrayList originalCssHrefs = RegexUtils.GetOriginalCssHrefs(styleContent);
                ArrayList cssHrefs = RegexUtils.GetCssHrefs(cssUrl, styleContent);
                for (int i = 0; i < originalCssHrefs.Count; i++)
                {
                    try
                    {
                        string originalLinkHref = (string)originalCssHrefs[i];
                        string cssHref = (string)cssHrefs[i];

                        string fileUrl_i = GatherCss(publishmentSystemInfo, publishmentSystemPath, imageDirectoryPath, styleDirectoryPath, level, charset, cssHref);
                        styleContent = styleContent.Replace(originalLinkHref, fileUrl_i);
                    }
                    catch { }
                }

                //开始采集CSS内部图片
                ArrayList originalStyleImageUrls = RegexUtils.GetOriginalStyleImageUrls(styleContent);
                ArrayList styleImageUrls = RegexUtils.GetStyleImageUrls(cssUrl, styleContent);
                for (int j = 0; j < originalStyleImageUrls.Count; j++)
                {
                    string originalStyleImageUrl = (string)originalStyleImageUrls[j];
                    string styleImageUrl = (string)styleImageUrls[j];
                    string fileName_j = PageUtils.UrlDecode(PathUtils.GetFileName(styleImageUrl));
                    string filePath_j = PathUtils.Combine(imageDirectoryPath, fileName_j);
                    try
                    {
                        WebClientUtils.SaveRemoteFileToLocal(styleImageUrl, filePath_j);
                        string fileUrl_j = PageUtility.GetPublishmentSystemUrlOfRelatedByPhysicalPath(publishmentSystemInfo, filePath_j, level);
                        styleContent = styleContent.Replace(originalStyleImageUrl, fileUrl_j);
                    }
                    catch { }
                }

                FileUtils.WriteText(filePath, charset, styleContent);

                fileUrl = PageUtility.GetPublishmentSystemUrlOfRelatedByPhysicalPath(publishmentSystemInfo, filePath, topLevel);
            }
            catch { }
            return fileUrl;
        }

        #endregion

    }
}
