using System;
using System.Collections;
using System.Text;
using System.Data;
using SiteServer.BBS.Model;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.BBS.Core
{
    public class TemplateManager
    {
        private int publishmentSystemID;
        private readonly string rootPath;

        private TemplateManager(int publishmentSystemID, string rootPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.rootPath = rootPath;
        }

        public static TemplateManager GetInstance(int publishmentSystemID, string rootPath)
        {
            return new TemplateManager(publishmentSystemID, rootPath);
        }

        public static TemplateManager GetInstance(int publishmentSystemID)
        {
            return new TemplateManager(publishmentSystemID, PathUtilityBBS.GetTemplatesPath(publishmentSystemID));
        }

        public void DeleteTemplate(string templateDir)
        {
            string templatePath = PathUtils.Combine(this.rootPath, templateDir);
            DirectoryUtils.DeleteDirectoryIfExists(templatePath);
        }

        public bool IsTemplateDirectoryExists(string templateDir)
        {
            string templatePath = PathUtils.Combine(this.rootPath, templateDir);
            return DirectoryUtils.IsDirectoryExists(templatePath);
        }

        public int GetTemplateCount()
        {
            string[] directorys = DirectoryUtils.GetDirectoryPaths(this.rootPath);
            return directorys.Length;
        }

        public SortedList GetAllTemplateSortedList()
        {
            SortedList sortedlist = new SortedList();
            string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(this.rootPath);
            foreach (string templatePath in directoryPaths)
            {
                string xmlFilePath = PathUtilityBBS.GetTemplateXmlPath(templatePath);
                if (FileUtils.IsFileExists(xmlFilePath))
                {
                    TemplateInfo templateInfo = Serializer.ConvertFileToObject(xmlFilePath, typeof(TemplateInfo)) as TemplateInfo;
                    if (templateInfo != null)
                    {
                        string directoryName = PathUtils.GetDirectoryName(templatePath);
                        sortedlist.Add(directoryName, templateInfo);
                    }
                }
            }
            return sortedlist;
        }
    }
}
