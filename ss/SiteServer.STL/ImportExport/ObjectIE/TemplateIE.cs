using System.Collections;
using Atom.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using BaiRong.Core;
using SiteServer.STL.IO;
using SiteServer.CMS.Core;
using System.Collections.Generic;

namespace SiteServer.STL.ImportExport
{
	internal class TemplateIE
	{
		private readonly int publishmentSystemID;
		private readonly string filePath;

		public TemplateIE(int publishmentSystemID, string filePath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.filePath = filePath;
		}


		public void ExportTemplates()
		{
			AtomFeed feed = AtomUtility.GetEmptyFeed();

			ArrayList templateInfoArrayList = DataProvider.TemplateDAO.GetTemplateInfoArrayListByPublishmentSystemID(publishmentSystemID);

			foreach (TemplateInfo templateInfo in templateInfoArrayList)
			{
				AtomEntry entry = ExportTemplateInfo(templateInfo);
				feed.Entries.Add(entry);
			}
			feed.Save(filePath);
		}

        public void ExportTemplates(List<int> templateIDList)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            ArrayList templateInfoArrayList = DataProvider.TemplateDAO.GetTemplateInfoArrayListByPublishmentSystemID(publishmentSystemID);

            foreach (TemplateInfo templateInfo in templateInfoArrayList)
            {
                if (templateIDList.Contains(templateInfo.TemplateID))
                {
                    AtomEntry entry = ExportTemplateInfo(templateInfo);
                    feed.Entries.Add(entry);
                }
            }
            feed.Save(filePath);
        }

		private AtomEntry ExportTemplateInfo(TemplateInfo templateInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID);

			AtomUtility.AddDcElement(entry.AdditionalElements, "TemplateID", templateInfo.TemplateID.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", templateInfo.PublishmentSystemID.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "TemplateName", templateInfo.TemplateName);
			AtomUtility.AddDcElement(entry.AdditionalElements, "TemplateType", ETemplateTypeUtils.GetValue(templateInfo.TemplateType));
            AtomUtility.AddDcElement(entry.AdditionalElements, "RelatedFileName", templateInfo.RelatedFileName);
			AtomUtility.AddDcElement(entry.AdditionalElements, "CreatedFileFullName", templateInfo.CreatedFileFullName);
			AtomUtility.AddDcElement(entry.AdditionalElements, "CreatedFileExtName", templateInfo.CreatedFileExtName);
			AtomUtility.AddDcElement(entry.AdditionalElements, "Charset", ECharsetUtils.GetValue(templateInfo.Charset));
            AtomUtility.AddDcElement(entry.AdditionalElements, "IsDefault", templateInfo.IsDefault.ToString());

            string templateContent = CreateCacheManager.FileContent.GetTemplateContent(publishmentSystemInfo, templateInfo);
			AtomUtility.AddDcElement(entry.AdditionalElements, "Content", AtomUtility.Encrypt(templateContent));

			return entry;
		}

		public void ImportTemplates(bool overwrite)
		{
			if (!FileUtils.IsFileExists(this.filePath)) return;
            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

			FileSystemObject FSO = new FileSystemObject(this.publishmentSystemID);
			foreach (AtomEntry entry in feed.Entries)
			{
				string templateName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "TemplateName");
				if (templateName != null && templateName.Length > 0)
				{
					TemplateInfo templateInfo = new TemplateInfo();
					templateInfo.PublishmentSystemID = this.publishmentSystemID;
					templateInfo.TemplateName = templateName;
					templateInfo.TemplateType = ETemplateTypeUtils.GetEnumType(AtomUtility.GetDcElementContent(entry.AdditionalElements, "TemplateType"));
                    templateInfo.RelatedFileName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "RelatedFileName");
					templateInfo.CreatedFileFullName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "CreatedFileFullName");
					templateInfo.CreatedFileExtName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "CreatedFileExtName");
					templateInfo.Charset = ECharsetUtils.GetEnumType(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Charset"));
					templateInfo.IsDefault = false;

					string templateContent = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Content"));
					
					TemplateInfo srcTemplateInfo = TemplateManager.GetTemplateInfoByTemplateName(this.publishmentSystemID, templateInfo.TemplateType, templateInfo.TemplateName);

					int templateID;

					if (srcTemplateInfo != null)
					{
						if (overwrite)
						{
                            srcTemplateInfo.RelatedFileName = templateInfo.RelatedFileName;
							srcTemplateInfo.TemplateType = templateInfo.TemplateType;
							srcTemplateInfo.CreatedFileFullName = templateInfo.CreatedFileFullName;
							srcTemplateInfo.CreatedFileExtName = templateInfo.CreatedFileExtName;
							srcTemplateInfo.Charset = templateInfo.Charset;
							DataProvider.TemplateDAO.Update(FSO.PublishmentSystemInfo, srcTemplateInfo, templateContent);
							templateID = srcTemplateInfo.TemplateID;
						}
						else
						{
							templateInfo.TemplateName = DataProvider.TemplateDAO.GetImportTemplateName(this.publishmentSystemID, templateInfo.TemplateName);
							templateID = DataProvider.TemplateDAO.Insert(templateInfo, templateContent);
						}
					}
					else
					{
						templateID = DataProvider.TemplateDAO.Insert(templateInfo, templateContent);
					}

					if (templateInfo.TemplateType == ETemplateType.FileTemplate)
					{
						FSO.CreateFile(templateID);
					}
				}
			}
		}

	}
}
