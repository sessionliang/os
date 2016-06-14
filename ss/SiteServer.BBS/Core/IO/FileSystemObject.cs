using System;
using System.Collections;
using System.Text;
using System.Data;
using SiteServer.BBS.Model;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.BBS.Core.TemplateParser;
using SiteServer.BBS.BackgroundPages;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Core
{
    public class FileSystemObject
    {
        private int publishmentSystemID;

        public FileSystemObject(int publishmentSystemID)
        {
            this.publishmentSystemID = publishmentSystemID;
        }

        public void CreateIndex()
        {
            string directoryName = string.Empty;
            string fileName = "default.aspx";
            this.CreateFile(directoryName, fileName);
        }

        public void CreateForum(int forumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(this.publishmentSystemID, forumID);
            if (forumInfo == null || !string.IsNullOrEmpty(forumInfo.LinkUrl)) return;

            string directoryName = "template";
            string fileName = "forum.aspx";

            string filePath = PathUtilityBBS.GetForumFilePath(this.publishmentSystemID, forumID);

            PageInfo pageInfo = new PageInfo(this.publishmentSystemID, ETemplateType.Forum, directoryName, fileName, forumID, 0);
            ContextInfo contextInfo = new ContextInfo(pageInfo);

            StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(this.publishmentSystemID, directoryName, fileName));
            this.GeneratePage(filePath, contentBuilder, pageInfo, contextInfo);//生成页面
        }

        //void CreateThread(int forumID, int threadID)
        //{
        //    string directoryName = "template/content";
        //    string fileName = "thread.aspx";

        //    string filePath = PathUtilityBBS.GetThreadFilePath(forumID, threadID);

        //    PageInfo pageInfo = new PageInfo(ETemplateType.Thread, directoryName, fileName, forumID, threadID);
        //    ContextInfo contextInfo = new ContextInfo(pageInfo);

        //    StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(directoryName, fileName));
        //    this.GeneratePage(filePath, contentBuilder, pageInfo, contextInfo);//生成页面
        //}

        public void CreateFile(string directoryName, string fileName)
        {
            string filePath = PathUtility.GetPublishmentSystemPath(this.publishmentSystemID, directoryName, fileName);

            EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(fileName));
            if (EFileSystemTypeUtils.IsTextEditable(fileType))
            {
                PageInfo pageInfo = new PageInfo(this.publishmentSystemID, ETemplateType.File, directoryName, fileName, 0, 0);
                ContextInfo contextInfo = new ContextInfo(pageInfo);
                StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(this.publishmentSystemID, directoryName, fileName));
                this.GeneratePage(filePath, contentBuilder, pageInfo, contextInfo);//生成页面
            }
            else
            {
                string tempalteFilePath = PathUtils.Combine(PathUtilityBBS.GetTemplateDirectoryPath(this.publishmentSystemID), directoryName, fileName);
                FileUtils.CopyFile(tempalteFilePath, filePath);
            }
        }

        private void GeneratePage(string filePath, StringBuilder contentBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            if (contentBuilder.Length > 0)
            {
                string regex = "<%=(?<content>[^><]*)%>";
                ArrayList arraylist = RegexUtils.GetContents("content", regex, contentBuilder.ToString());
                foreach (string str in arraylist)
                {
                    contentBuilder.Replace(str, str.Replace("\"", "'"));
                }

                ParserManager.ParseTemplateContent(contentBuilder, pageInfo, contextInfo);

                arraylist = RegexUtils.GetContents("content", regex, contentBuilder.ToString());
                foreach (string str in arraylist)
                {
                    contentBuilder.Replace(str, str.Replace("'", "\""));
                }
            }

            if (EFileSystemTypeUtils.IsHtml(PathUtils.GetExtension(filePath)))
            {
                string poweredBy = " - Powered by SiteServer BBS";
                StringUtils.InsertBefore(new string[] { "</title>", "</TITLE>" }, contentBuilder, poweredBy);

                ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(this.publishmentSystemID);

                if (additional.IsCreateDoubleClick)
                {
                    string ajaxUrl = BackgroundUtils.GetCreatePageUrl(pageInfo.PublishmentSystemID, pageInfo.DirectoryName, pageInfo.FileName, pageInfo.ForumID);
                    contentBuilder.AppendFormat(@"
<script type=""text/javascript"" language=""javascript"">document.ondblclick=function(x){{location.href = '{0}';}}</script>", ajaxUrl);
                }
            }
            
            this.GenerateFile(filePath, contentBuilder.ToString());
        }

        /// <summary>
        /// 在操作系统中创建文件，如果文件存在，重新创建此文件
        /// </summary>
        /// <param name="filePath">需要创建文件的绝对路径，必须是完整的路径</param>
        /// <param name="charset">编码</param>
        /// <param name="content">需要创建文件的内容</param>
        private void GenerateFile(string filePath, string content)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    FileUtils.WriteText(filePath, ECharset.utf_8, content);
                }
                catch
                {
                    FileUtils.RemoveReadOnlyAndHiddenIfExists(filePath);
                    FileUtils.WriteText(filePath, ECharset.utf_8, content);
                }
            }
        }
    }
}
