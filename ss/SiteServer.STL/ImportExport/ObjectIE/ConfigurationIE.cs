using System.Collections;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.ImportExport
{
	public class ConfigurationIE
	{
		private readonly int publishmentSystemID;
		private readonly string filePath;

		private const string DEFAULT_INDEX_TEMPLATE_NAME = "DefaultIndexTemplateName";
		private const string DEFAULT_CHANNEL_TEMPLATE_NAME = "DefaultChannelTemplateName";
		private const string DEFAULT_CONTENT_TEMPLATE_NAME = "DefaultContentTemplateName";
		private const string DEFAULT_FILE_TEMPLATE_NAME = "DefaultFileTemplateName";

		public ConfigurationIE(int publishmentSystemID, string filePath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.filePath = filePath;
		}


		public void Export()
		{
			PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID);

			AtomFeed feed = AtomUtility.GetEmptyFeed();

            AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.PublishmentSystemID, psInfo.PublishmentSystemID.ToString());
			AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.PublishmentSystemName, psInfo.PublishmentSystemName);
			AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.AuxiliaryTableForContent, psInfo.AuxiliaryTableForContent);
            AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.AuxiliaryTableForGovPublic, psInfo.AuxiliaryTableForGovPublic);
            AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.AuxiliaryTableForGovInteract, psInfo.AuxiliaryTableForGovInteract);
            AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.AuxiliaryTableForJob, psInfo.AuxiliaryTableForJob);
            AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.AuxiliaryTableForVote, psInfo.AuxiliaryTableForVote);
            AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.IsCheckContentUseLevel, psInfo.IsCheckContentUseLevel.ToString());
			AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.CheckContentLevel, psInfo.CheckContentLevel.ToString());
			AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.PublishmentSystemDir, psInfo.PublishmentSystemDir);
			AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.PublishmentSystemUrl, psInfo.PublishmentSystemUrl);
            AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.IsHeadquarters, psInfo.IsHeadquarters.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.ParentPublishmentSystemID, psInfo.ParentPublishmentSystemID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.Taxis, psInfo.Taxis.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, PublishmentSystemAttribute.SettingsXML, psInfo.Additional.ToString());

			int indexTemplateID = TemplateManager.GetDefaultTemplateID(psInfo.PublishmentSystemID, ETemplateType.IndexPageTemplate);
			if (indexTemplateID != 0)
			{
                string indexTemplateName = TemplateManager.GetTemplateName(this.publishmentSystemID, indexTemplateID);
				AtomUtility.AddDcElement(feed.AdditionalElements, ConfigurationIE.DEFAULT_INDEX_TEMPLATE_NAME, indexTemplateName);
			}

            int channelTemplateID = TemplateManager.GetDefaultTemplateID(psInfo.PublishmentSystemID, ETemplateType.ChannelTemplate);
			if (channelTemplateID != 0)
			{
                string channelTemplateName = TemplateManager.GetTemplateName(this.publishmentSystemID, channelTemplateID);
				AtomUtility.AddDcElement(feed.AdditionalElements, ConfigurationIE.DEFAULT_CHANNEL_TEMPLATE_NAME, channelTemplateName);
			}

            int contentTemplateID = TemplateManager.GetDefaultTemplateID(psInfo.PublishmentSystemID, ETemplateType.ContentTemplate);
			if (contentTemplateID != 0)
			{
                string contentTemplateName = TemplateManager.GetTemplateName(this.publishmentSystemID, contentTemplateID);
				AtomUtility.AddDcElement(feed.AdditionalElements, ConfigurationIE.DEFAULT_CONTENT_TEMPLATE_NAME, contentTemplateName);
			}

            int fileTemplateID = TemplateManager.GetDefaultTemplateID(psInfo.PublishmentSystemID, ETemplateType.FileTemplate);
			if (fileTemplateID != 0)
			{
                string fileTemplateName = TemplateManager.GetTemplateName(psInfo.PublishmentSystemID, fileTemplateID);
				AtomUtility.AddDcElement(feed.AdditionalElements, ConfigurationIE.DEFAULT_FILE_TEMPLATE_NAME, fileTemplateName);
			}

			ArrayList nodeGroupInfoArrayList = DataProvider.NodeGroupDAO.GetNodeGroupInfoArrayList(psInfo.PublishmentSystemID);
            nodeGroupInfoArrayList.Reverse();

			foreach (NodeGroupInfo nodeGroupInfo in nodeGroupInfoArrayList)
			{
				AtomEntry entry = ExportNodeGroupInfo(nodeGroupInfo);
				feed.Entries.Add(entry);
			}

			ArrayList contentGroupInfoArrayList = DataProvider.ContentGroupDAO.GetContentGroupInfoArrayList(psInfo.PublishmentSystemID);
            contentGroupInfoArrayList.Reverse();

			foreach (ContentGroupInfo contentGroupInfo in contentGroupInfoArrayList)
			{
				AtomEntry entry = ExportContentGroupInfo(contentGroupInfo);
				feed.Entries.Add(entry);
			}

			feed.Save(filePath);
		}

		private static AtomEntry ExportNodeGroupInfo(NodeGroupInfo nodeGroupInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

			AtomUtility.AddDcElement(entry.AdditionalElements, "IsNodeGroup", true.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "NodeGroupName", nodeGroupInfo.NodeGroupName);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Taxis", nodeGroupInfo.Taxis.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "Description", nodeGroupInfo.Description);

			return entry;
		}

		private static AtomEntry ExportContentGroupInfo(ContentGroupInfo contentGroupInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

			AtomUtility.AddDcElement(entry.AdditionalElements, "IsContentGroup", true.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "ContentGroupName", contentGroupInfo.ContentGroupName);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Taxis", contentGroupInfo.Taxis.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "Description", contentGroupInfo.Description);

			return entry;
		}

        public static PublishmentSystemInfo GetPublishmentSytemInfo(string filePath)
        {
            PublishmentSystemInfo publishmentSystemInfo = new PublishmentSystemInfo();
            if (FileUtils.IsFileExists(filePath))
            {
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                publishmentSystemInfo.PublishmentSystemName = AtomUtility.GetDcElementContent(feed.AdditionalElements, PublishmentSystemAttribute.PublishmentSystemName);
                publishmentSystemInfo.PublishmentSystemDir = AtomUtility.GetDcElementContent(feed.AdditionalElements, PublishmentSystemAttribute.PublishmentSystemDir);
                if (publishmentSystemInfo.PublishmentSystemDir != null && publishmentSystemInfo.PublishmentSystemDir.IndexOf("\\") != -1)
                {
                    publishmentSystemInfo.PublishmentSystemDir = publishmentSystemInfo.PublishmentSystemDir.Substring(publishmentSystemInfo.PublishmentSystemDir.LastIndexOf("\\") + 1);
                }
                //publishmentSystemInfo.IsCheckContentUseLevel = EBooleanUtils.GetEnumType(AtomUtility.GetDcElementContent(feed.AdditionalElements, PublishmentSystemAttribute.IsCheckContentUseLevel));
                //publishmentSystemInfo.CheckContentLevel = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(feed.AdditionalElements, PublishmentSystemAttribute.CheckContentLevel));
                publishmentSystemInfo.SettingsXML = AtomUtility.GetDcElementContent(feed.AdditionalElements, PublishmentSystemAttribute.SettingsXML);
                publishmentSystemInfo.Additional.IsCreateDoubleClick = false;
            }
            return publishmentSystemInfo;
        }

		public void Import()
		{
			if (!FileUtils.IsFileExists(this.filePath)) return;

            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

			PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID);

            //psInfo.IsCheckContentUseLevel = EBooleanUtils.GetEnumType(AtomUtility.GetDcElementContent(feed.AdditionalElements, PublishmentSystemAttribute.IsCheckContentUseLevel, EBooleanUtils.GetValue(psInfo.IsCheckContentUseLevel)));
            //psInfo.CheckContentLevel = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(feed.AdditionalElements, PublishmentSystemAttribute.CheckContentLevel, psInfo.CheckContentLevel.ToString()));
            psInfo.SettingsXML = AtomUtility.GetDcElementContent(feed.AdditionalElements, PublishmentSystemAttribute.SettingsXML, psInfo.SettingsXML);

            psInfo.Additional.TextEditorType = ETextEditorType.UEditor;
            psInfo.Additional.HomeUrl = string.Empty;
            psInfo.Additional.IsMultiDeployment = false;
            psInfo.Additional.FuncFilesType = EFuncFilesType.Direct;

			DataProvider.PublishmentSystemDAO.Update(psInfo);

			string indexTemplateName = AtomUtility.GetDcElementContent(feed.AdditionalElements, ConfigurationIE.DEFAULT_INDEX_TEMPLATE_NAME);
			if (!string.IsNullOrEmpty(indexTemplateName))
			{
				int indexTemplateID = TemplateManager.GetTemplateIDByTemplateName(psInfo.PublishmentSystemID, ETemplateType.IndexPageTemplate, indexTemplateName);
				if (indexTemplateID != 0)
				{
					DataProvider.TemplateDAO.SetDefault(psInfo.PublishmentSystemID, indexTemplateID);
				}
			}

			string channelTemplateName = AtomUtility.GetDcElementContent(feed.AdditionalElements, ConfigurationIE.DEFAULT_CHANNEL_TEMPLATE_NAME);
			if (!string.IsNullOrEmpty(channelTemplateName))
			{
                int channelTemplateID = TemplateManager.GetTemplateIDByTemplateName(psInfo.PublishmentSystemID, ETemplateType.ChannelTemplate, channelTemplateName);
				if (channelTemplateID != 0)
				{
					DataProvider.TemplateDAO.SetDefault(psInfo.PublishmentSystemID, channelTemplateID);
				}
			}

			string contentTemplateName = AtomUtility.GetDcElementContent(feed.AdditionalElements, ConfigurationIE.DEFAULT_CONTENT_TEMPLATE_NAME);
			if (!string.IsNullOrEmpty(contentTemplateName))
			{
                int contentTemplateID = TemplateManager.GetTemplateIDByTemplateName(psInfo.PublishmentSystemID, ETemplateType.ContentTemplate, contentTemplateName);
				if (contentTemplateID != 0)
				{
					DataProvider.TemplateDAO.SetDefault(psInfo.PublishmentSystemID, contentTemplateID);
				}
			}

			string fileTemplateName = AtomUtility.GetDcElementContent(feed.AdditionalElements, ConfigurationIE.DEFAULT_FILE_TEMPLATE_NAME);
			if (!string.IsNullOrEmpty(fileTemplateName))
			{
                int fileTemplateID = TemplateManager.GetTemplateIDByTemplateName(psInfo.PublishmentSystemID, ETemplateType.FileTemplate, fileTemplateName);
				if (fileTemplateID != 0)
				{
					DataProvider.TemplateDAO.SetDefault(psInfo.PublishmentSystemID, fileTemplateID);
				}
			}

			foreach (AtomEntry entry in feed.Entries)
			{
				bool isNodeGroup = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsNodeGroup"));
				bool isContentGroup = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsContentGroup"));
				if (isNodeGroup)
				{
					string nodeGroupName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "NodeGroupName");
                    if (!DataProvider.NodeGroupDAO.IsExists(psInfo.PublishmentSystemID, nodeGroupName))
					{
                        int taxis = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Taxis"));
						string description = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Description");
                        DataProvider.NodeGroupDAO.Insert(new NodeGroupInfo(nodeGroupName, psInfo.PublishmentSystemID, taxis, description));
					}
				}
				else if (isContentGroup)
				{
					string contentGroupName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentGroupName");
                    if (!DataProvider.ContentGroupDAO.IsExists(contentGroupName, psInfo.PublishmentSystemID))
					{
                        int taxis = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Taxis"));
						string description = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Description");
                        DataProvider.ContentGroupDAO.Insert(new ContentGroupInfo(contentGroupName, psInfo.PublishmentSystemID, taxis, description));
					}
				}
			}
		}

	}
}
