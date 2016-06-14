using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using BaiRong.Core;
using BaiRong.Model;
using Word.Plugin;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core.Office
{
    public class WordUtils
    {
        public WordUtils() { }

        public static string GetWordFilePath(string fileName)
        {
            return PathUtils.GetTemporaryFilesPath(fileName);
        }

        public static string Parse(int publishmentSystemID, string filePath, bool isClearFormat, bool isFirstLineIndent, bool isClearFontSize, bool isClearFontFamily, bool isClearImages)
        {
            string parsedContent = string.Empty;
            if (!string.IsNullOrEmpty(filePath))
            {
                string filename = PathUtils.GetFileNameWithoutExtension(filePath);

                //被转换的html文档保存的位置
                try
                {
                    string saveFilePath = PathUtils.GetTemporaryFilesPath(filename + ".html");
                    WordDntb.buildWord(filePath, saveFilePath);

                    parsedContent = FileUtils.ReadText(saveFilePath, System.Text.Encoding.Default);
                    parsedContent = RegexUtils.GetInnerContent("body", parsedContent);

                    //try
                    //{
                    //    parsedContent = HtmlClearUtils.ClearElementAttributes(parsedContent, "p");
                    //}
                    //catch { }

                    if (isClearFormat)
                    {
                        parsedContent = HtmlClearUtils.ClearFormat(parsedContent);
                    }

                    if (isFirstLineIndent)
                    {
                        parsedContent = HtmlClearUtils.FirstLineIndent(parsedContent);
                    }

                    if (isClearFontSize)
                    {
                        parsedContent = HtmlClearUtils.ClearFontSize(parsedContent);
                    }

                    if (isClearFontFamily)
                    {
                        parsedContent = HtmlClearUtils.ClearFontFamily(parsedContent);
                    }

                    if (isClearImages)
                    {
                        parsedContent = StringUtils.StripTags(parsedContent, "img");
                    }
                    else
                    {
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                        ArrayList imageFileNameArrayList = RegexUtils.GetOriginalImageSrcs(parsedContent);
                        if (imageFileNameArrayList != null && imageFileNameArrayList.Count > 0)
                        {
                            DateTime now = DateTime.Now;
                            foreach (string imageFileName in imageFileNameArrayList)
                            {
                                now = now.AddMilliseconds(10);
                                string imageFilePath = PathUtils.GetTemporaryFilesPath(imageFileName);
                                string fileExtension = PathUtils.GetExtension(imageFilePath);
                                string uploadDirectoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, fileExtension);
                                string uploadDirectoryUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, uploadDirectoryPath);
                                if (FileUtils.IsFileExists(imageFilePath))
                                {
                                    string uploadFileName = PathUtility.GetUploadFileName(publishmentSystemInfo, imageFilePath, now);
                                    string destFilePath = PathUtils.Combine(uploadDirectoryPath, uploadFileName);
                                    FileUtils.MoveFile(imageFilePath, destFilePath, false);
                                    parsedContent = parsedContent.Replace(imageFileName, PageUtils.Combine(uploadDirectoryUrl, uploadFileName));

                                    FileUtils.DeleteFileIfExists(imageFilePath);
                                }
                            }
                        }
                    }

                    FileUtils.DeleteFileIfExists(filePath);
                    FileUtils.DeleteFileIfExists(saveFilePath);
                    return parsedContent;
                }
                catch 
                {
                   
                }
            }

            return parsedContent;
        }

        public static NameValueCollection GetWordNameValueCollection(int publishmentSystemID, string contentModelID, bool isFirstLineTitle, bool isFirstLineRemove, bool isClearFormat, bool isFirstLineIndent, bool isClearFontSize, bool isClearFontFamily, bool isClearImages, int contentLevel, string fileName)
        {
            NameValueCollection formCollection = new NameValueCollection();
            string wordContent = WordUtils.Parse(publishmentSystemID, WordUtils.GetWordFilePath(fileName), isClearFormat, isFirstLineIndent, isClearFontSize, isClearFontFamily, isClearImages);
            if (!string.IsNullOrEmpty(wordContent))
            {
                string title = string.Empty;
                if (isFirstLineTitle)
                {
                    title = RegexUtils.GetInnerContent("p", wordContent);
                    title = StringUtils.StripTags(title);
                    if (!string.IsNullOrEmpty(title) && isFirstLineRemove)
                    {
                        wordContent = StringUtils.ReplaceFirst(title, wordContent, string.Empty);
                    }
                    if (!string.IsNullOrEmpty(title))
                    {
                        title = title.Trim('　', ' ');
                        title = StringUtils.StripEntities(title);
                    }
                }
                if (string.IsNullOrEmpty(title))
                {
                    title = PathUtils.GetFileNameWithoutExtension(fileName);
                }
                formCollection[ContentAttribute.Title] = title;

                wordContent = StringUtils.ReplaceFirst("<p></p>", wordContent, string.Empty);

                if (EContentModelTypeUtils.Equals(contentModelID, EContentModelType.Job))
                {
                    formCollection[JobContentAttribute.Responsibility] = wordContent;
                }
                else
                {
                    formCollection[BackgroundContentAttribute.Content] = wordContent;
                }
            }
            return formCollection;
        }
    }
}
