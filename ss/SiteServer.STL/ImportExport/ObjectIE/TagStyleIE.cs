using System;
using System.Collections;
using Atom.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using BaiRong.Core;
using SiteServer.CMS.Core;

namespace SiteServer.STL.ImportExport
{
	internal class TagStyleIE
	{
		private readonly int publishmentSystemID;
		private readonly string filePath;

        public TagStyleIE(int publishmentSystemID, string filePath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.filePath = filePath;
		}

		public void ExportTagStyle()
		{
			AtomFeed feed = AtomUtility.GetEmptyFeed();

			ArrayList tagStyleInfoArrayList = DataProvider.TagStyleDAO.GetTagStyleInfoArrayList(this.publishmentSystemID);

            foreach (TagStyleInfo tagStyleInfo in tagStyleInfoArrayList)
			{
                AtomEntry entry = ExportTagStyleInfo(tagStyleInfo);
				feed.Entries.Add(entry);
			}

			feed.Save(filePath);
		}

        public void ExportTagStyle(TagStyleInfo tagStyleInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            AtomEntry entry = ExportTagStyleInfo(tagStyleInfo);
            feed.Entries.Add(entry);

            feed.Save(filePath);
        }

        private static AtomEntry ExportTagStyleInfo(TagStyleInfo tagStyleInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, "StyleID", tagStyleInfo.StyleID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "StyleName", tagStyleInfo.StyleName);
            AtomUtility.AddDcElement(entry.AdditionalElements, "ElementName", tagStyleInfo.ElementName);
            AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", tagStyleInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "IsTemplate", tagStyleInfo.IsTemplate.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "StyleTemplate", AtomUtility.Encrypt(tagStyleInfo.StyleTemplate));
            AtomUtility.AddDcElement(entry.AdditionalElements, "ScriptTemplate", AtomUtility.Encrypt(tagStyleInfo.ScriptTemplate));
            AtomUtility.AddDcElement(entry.AdditionalElements, "ContentTemplate", AtomUtility.Encrypt(tagStyleInfo.ContentTemplate));
            AtomUtility.AddDcElement(entry.AdditionalElements, "SettingsXML", AtomUtility.Encrypt(tagStyleInfo.SettingsXML));

			return entry;
		}


		public void ImportTagStyle(bool overwrite)
		{
			if (!FileUtils.IsFileExists(this.filePath)) return;
            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

			foreach (AtomEntry entry in feed.Entries)
			{
                string styleName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "StyleName");

				if (!string.IsNullOrEmpty(styleName))
				{
                    TagStyleInfo tagStyleInfo = new TagStyleInfo();
                    tagStyleInfo.StyleName = styleName;
                    tagStyleInfo.ElementName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ElementName");
					tagStyleInfo.PublishmentSystemID = this.publishmentSystemID;
                    tagStyleInfo.IsTemplate = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsTemplate"));
                    tagStyleInfo.StyleTemplate = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "StyleTemplate"));
                    tagStyleInfo.ScriptTemplate = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ScriptTemplate"));
                    tagStyleInfo.ContentTemplate = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentTemplate"));
                    tagStyleInfo.SettingsXML = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "SettingsXML"));

                    TagStyleInfo srcTagStyleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(this.publishmentSystemID, tagStyleInfo.ElementName, tagStyleInfo.StyleName);
                    if (srcTagStyleInfo != null)
					{
						if (overwrite)
						{
                            tagStyleInfo.StyleID = srcTagStyleInfo.StyleID;
                            DataProvider.TagStyleDAO.Update(tagStyleInfo);
						}
						else
						{
                            tagStyleInfo.StyleName = DataProvider.TagStyleDAO.GetImportStyleName(this.publishmentSystemID, tagStyleInfo.ElementName, tagStyleInfo.StyleName);
                            DataProvider.TagStyleDAO.Insert(tagStyleInfo);
						}
					}
					else
					{
                        DataProvider.TagStyleDAO.Insert(tagStyleInfo);
					}
				}
			}
		}

	}
}
