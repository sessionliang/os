using System.Collections;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Model;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.STL.IO;

namespace SiteServer.STL.ImportExport
{
	public class IndependentTemplateManager
	{
	    private readonly string rootPath;
        private IndependentTemplateManager(string rootPath)
	    {
            this.rootPath = rootPath;
            DirectoryUtils.CreateDirectoryIfNotExists(this.rootPath);
	    }

        public static IndependentTemplateManager GetInstance(string rootPath)
        {
            return new IndependentTemplateManager(rootPath);
        }

        public static IndependentTemplateManager Instance
        {
            get
            {
                return new IndependentTemplateManager(PathUtility.GetIndependentTemplatesPath(string.Empty));
            }
        }


        public void DeleteIndependentTemplate(string independentTemplateDir)
        {
            string directoryPath = PathUtils.Combine(this.rootPath, independentTemplateDir);
            DirectoryUtils.DeleteDirectoryIfExists(directoryPath);

            string filePath = PathUtils.Combine(this.rootPath, independentTemplateDir + ".zip");
            FileUtils.DeleteFileIfExists(filePath);
        }

        public bool IsIndependentTemplateDirectoryExists(string independentTemplateDir)
        {
            string independentTemplatePath = PathUtils.Combine(this.rootPath, independentTemplateDir);
            return DirectoryUtils.IsDirectoryExists(independentTemplatePath);
        }

        public int GetIndependentTemplateCount()
        {
            string[] directorys = DirectoryUtils.GetDirectoryPaths(this.rootPath);
            return directorys.Length;
        }

        public ArrayList GetDirectoryNameLowerArrayList()
        {
            string[] directorys = DirectoryUtils.GetDirectoryNames(this.rootPath);
            ArrayList arraylist = new ArrayList();
            foreach (string directoryName in directorys)
            {
                arraylist.Add(directoryName.ToLower().Trim());
            }
            return arraylist;
        }

        public SortedList GetIndependentTemplateSortedList(EPublishmentSystemType publishmentSystemType, ETemplateType templateType)
        {
            SortedList sortedlist = new SortedList();
            string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(this.rootPath);
            foreach (string independentTemplatePath in directoryPaths)
            {
                string metadataXmlFilePath = PathUtility.GetIndependentTemplateMetadataPath(independentTemplatePath, DirectoryUtils.IndependentTemplates.File_Metadata);
                if (FileUtils.IsFileExists(metadataXmlFilePath))
                {
                    IndependentTemplateInfo independentTemplateInfo = Serializer.ConvertFileToObject(metadataXmlFilePath, typeof(IndependentTemplateInfo)) as IndependentTemplateInfo;
                    if (independentTemplateInfo != null)
                    {
                        if (publishmentSystemType == EPublishmentSystemTypeUtils.GetEnumType(independentTemplateInfo.PublishmentSystemType) && ETemplateTypeUtils.Equals(independentTemplateInfo.TemplateTypes, templateType))
                        {
                            string directoryName = PathUtils.GetDirectoryName(independentTemplatePath);
                            sortedlist.Add(directoryName, independentTemplateInfo);
                        }
                    }
                }
            }
            return sortedlist;
        }

        public SortedList GetAllIndependentTemplateSortedList()
        {
            SortedList sortedlist = new SortedList();
            string[] directoryPaths = DirectoryUtils.GetDirectoryPaths(this.rootPath);
            foreach (string independentTemplatePath in directoryPaths)
            {
                string metadataXmlFilePath = PathUtility.GetIndependentTemplateMetadataPath(independentTemplatePath, DirectoryUtils.IndependentTemplates.File_Metadata);
                if (FileUtils.IsFileExists(metadataXmlFilePath))
                {
                    IndependentTemplateInfo independentTemplateInfo = Serializer.ConvertFileToObject(metadataXmlFilePath, typeof(IndependentTemplateInfo)) as IndependentTemplateInfo;
                    if (independentTemplateInfo != null)
                    {
                        string directoryName = PathUtils.GetDirectoryName(independentTemplatePath);
                        sortedlist.Add(directoryName, independentTemplateInfo);
                    }
                }
            }
            return sortedlist;
        }

        public void LoadIndependentTemplateToPublishmentSystem(int publishmentSystemID, string independentTemplateDir)
        {
            string independentTemplatePath = PathUtility.GetIndependentTemplatesPath(independentTemplateDir);
            if (DirectoryUtils.IsDirectoryExists(independentTemplatePath))
            {
                string templateFilePath = PathUtility.GetIndependentTemplateMetadataPath(independentTemplatePath, DirectoryUtils.IndependentTemplates.File_Template);
                string siteContentDirectoryPath = PathUtility.GetIndependentTemplateMetadataPath(independentTemplatePath, DirectoryUtils.IndependentTemplates.SiteContent);

                ImportObject importObject = new ImportObject(publishmentSystemID);

                importObject.ImportFiles(independentTemplatePath, true);

                importObject.ImportTemplates(templateFilePath, true);

                if (DirectoryUtils.IsDirectoryExists(siteContentDirectoryPath))
                {
                    ArrayList filePathArrayList = ImportObject.GetSiteContentFilePathArrayList(siteContentDirectoryPath);
                    foreach (string filePath in filePathArrayList)
                    {
                        importObject.ImportSiteContent(siteContentDirectoryPath, filePath, true);
                    }
                    DataProvider.NodeDAO.UpdateContentNum(PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID));
                }

                importObject.RemoveDbCache();
            }

            FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
            FSO.CreateIndex();
        }
	}
}
