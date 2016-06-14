using System;
using System.Collections;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Collections.Specialized;
using SiteServer.CMS.Core;

namespace SiteServer.STL.ImportExport
{
	internal class InputIE
	{
		private readonly int publishmentSystemID;
		private readonly string directoryPath;

        public InputIE(int publishmentSystemID, string directoryPath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.directoryPath = directoryPath;
		}

        public void ExportInput(int inputID)
		{
            InputInfo inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);
            string filePath = this.directoryPath + PathUtils.SeparatorChar + inputInfo.InputID + ".xml";

            AtomFeed feed = ExportInputInfo(inputInfo);

            string styleDirectoryPath = PathUtils.Combine(this.directoryPath, inputInfo.InputID.ToString());
            TableStyleIE.SingleExportTableStyles(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, this.publishmentSystemID, inputInfo.InputID, styleDirectoryPath);

            ArrayList contentIDArrayList = DataProvider.InputContentDAO.GetContentIDArrayListWithChecked(inputInfo.InputID);
            foreach (int contentID in contentIDArrayList)
			{
                InputContentInfo contentInfo = DataProvider.InputContentDAO.GetContentInfo(contentID);
                AtomEntry entry = GetAtomEntry(contentInfo);
				feed.Entries.Add(entry);
			}
			feed.Save(filePath);
		}

        private static AtomFeed ExportInputInfo(InputInfo inputInfo)
		{
			AtomFeed feed = AtomUtility.GetEmptyFeed();

            AtomUtility.AddDcElement(feed.AdditionalElements, "InputID", inputInfo.InputID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "InputName", inputInfo.InputName);
            AtomUtility.AddDcElement(feed.AdditionalElements, "PublishmentSystemID", inputInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "AddDate", inputInfo.AddDate.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsChecked", inputInfo.IsChecked.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsReply", inputInfo.IsReply.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "Taxis", inputInfo.Taxis.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsTemplate", inputInfo.IsTemplate.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "StyleTemplate", AtomUtility.Encrypt(inputInfo.StyleTemplate));
            AtomUtility.AddDcElement(feed.AdditionalElements, "ScriptTemplate", AtomUtility.Encrypt(inputInfo.ScriptTemplate));
            AtomUtility.AddDcElement(feed.AdditionalElements, "ContentTemplate", AtomUtility.Encrypt(inputInfo.ContentTemplate));
            AtomUtility.AddDcElement(feed.AdditionalElements, "SettingsXML", AtomUtility.Encrypt(inputInfo.SettingsXML));

			return feed;
		}

        private static AtomEntry GetAtomEntry(InputContentInfo contentInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in contentInfo.Attributes)
            {
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, contentInfo.Attributes[attributeName]);
            }

			return entry;
		}

		public void ImportInput(bool overwrite)
		{
			if (!DirectoryUtils.IsDirectoryExists(this.directoryPath)) return;
			string[] filePaths = DirectoryUtils.GetFilePaths(this.directoryPath);

			foreach (string filePath in filePaths)
			{
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                InputInfo inputInfo = new InputInfo();
                inputInfo.InputName = AtomUtility.GetDcElementContent(feed.AdditionalElements, "InputName");
                inputInfo.PublishmentSystemID = this.publishmentSystemID;
                inputInfo.AddDate = DateTime.Now;
                inputInfo.IsChecked = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsChecked"));
                inputInfo.IsReply = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsReply"));
                inputInfo.Taxis = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "Taxis"));
                inputInfo.IsTemplate = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsTemplate"));
                inputInfo.StyleTemplate = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "StyleTemplate"));
                inputInfo.ScriptTemplate = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "ScriptTemplate"));
                inputInfo.ContentTemplate = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "ContentTemplate"));

                inputInfo.SettingsXML = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "SettingsXML"));

                InputInfo srcInputInfo = DataProvider.InputDAO.GetInputInfo(inputInfo.InputName, this.publishmentSystemID);
                if (srcInputInfo != null)
				{
					if (overwrite)
					{
                        DataProvider.InputDAO.Delete(srcInputInfo.InputID);
					}
					else
					{
                        inputInfo.InputName = DataProvider.InputDAO.GetImportInputName(inputInfo.InputName, this.publishmentSystemID);
					}
				}

                int inputID = DataProvider.InputDAO.Insert(inputInfo);

                string styleDirectoryPath = PathUtils.Combine(this.directoryPath, AtomUtility.GetDcElementContent(feed.AdditionalElements, "InputID"));
                TableStyleIE.SingleImportTableStyle(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, styleDirectoryPath, this.publishmentSystemID, inputID);

                foreach (AtomEntry entry in feed.Entries)
                {
                    InputContentInfo contentInfo = new InputContentInfo();
                    contentInfo.InputID = inputID;
                    contentInfo.IsChecked = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, InputContentAttribute.IsChecked));
                    contentInfo.UserName = AtomUtility.GetDcElementContent(entry.AdditionalElements, InputContentAttribute.UserName);
                    contentInfo.AddDate = DateTime.Now;
                    contentInfo.Reply = AtomUtility.GetDcElementContent(entry.AdditionalElements, InputContentAttribute.Reply);
                    NameValueCollection attributes = AtomUtility.GetDcElementNameValueCollection(entry.AdditionalElements);
                    foreach (string entryName in attributes.Keys)
                    {
                        if (!InputContentAttribute.AllAttributes.Contains(entryName.ToLower()))
                        {
                            contentInfo.SetExtendedAttribute(entryName, attributes[entryName]);
                        }
                    }
                    DataProvider.InputContentDAO.Insert(contentInfo);
                }
			}
		}

        /// <summary>
        /// by 20151029 sofuny
        /// </summary>
        /// <param name="overwrite"></param>
        /// <param name="itemID"></param>
        public void ImportInput(bool overwrite, int itemID)
        {
            if (!DirectoryUtils.IsDirectoryExists(this.directoryPath)) return;
            string[] filePaths = DirectoryUtils.GetFilePaths(this.directoryPath);

            foreach (string filePath in filePaths)
            {
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                InputInfo inputInfo = new InputInfo();
                inputInfo.InputName = AtomUtility.GetDcElementContent(feed.AdditionalElements, "InputName");
                inputInfo.PublishmentSystemID = this.publishmentSystemID;
                inputInfo.AddDate = DateTime.Now;
                inputInfo.IsChecked = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsChecked"));
                inputInfo.IsReply = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsReply"));
                inputInfo.Taxis = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "Taxis"));
                inputInfo.IsTemplate = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsTemplate"));
                inputInfo.StyleTemplate = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "StyleTemplate"));
                inputInfo.ScriptTemplate = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "ScriptTemplate"));
                inputInfo.ContentTemplate = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "ContentTemplate"));

                inputInfo.SettingsXML = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "SettingsXML"));

                inputInfo.ClassifyID = itemID;

                 InputInfo srcInputInfo = DataProvider.InputDAO.GetInputInfo(inputInfo.InputName, this.publishmentSystemID);
              //  InputInfo srcInputInfo = DataProvider.InputDAO.GetInputInfo(inputInfo.InputName, this.publishmentSystemID,itemID);
                if (srcInputInfo != null)
                {
                    if (overwrite)
                    {
                        DataProvider.InputDAO.Delete(srcInputInfo.InputID);
                    }
                    else
                    {
                        inputInfo.InputName = DataProvider.InputDAO.GetImportInputName(inputInfo.InputName, this.publishmentSystemID);
                        DataProvider.InputClassifyDAO.UpdateInputCount(this.publishmentSystemID, itemID, 1);//更新分类下表单数量 
                    }
                }

                int inputID = DataProvider.InputDAO.Insert(inputInfo);

                string styleDirectoryPath = PathUtils.Combine(this.directoryPath, AtomUtility.GetDcElementContent(feed.AdditionalElements, "InputID"));
                TableStyleIE.SingleImportTableStyle(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, styleDirectoryPath, this.publishmentSystemID, inputID);

                foreach (AtomEntry entry in feed.Entries)
                {
                    InputContentInfo contentInfo = new InputContentInfo();
                    contentInfo.InputID = inputID;
                    contentInfo.IsChecked = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, InputContentAttribute.IsChecked));
                    contentInfo.UserName = AtomUtility.GetDcElementContent(entry.AdditionalElements, InputContentAttribute.UserName);
                    contentInfo.AddDate = DateTime.Now;
                    contentInfo.Reply = AtomUtility.GetDcElementContent(entry.AdditionalElements, InputContentAttribute.Reply);
                    NameValueCollection attributes = AtomUtility.GetDcElementNameValueCollection(entry.AdditionalElements);
                    foreach (string entryName in attributes.Keys)
                    {
                        if (!InputContentAttribute.AllAttributes.Contains(entryName.ToLower()))
                        {
                            contentInfo.SetExtendedAttribute(entryName, attributes[entryName]);
                        }
                    }
                    DataProvider.InputContentDAO.Insert(contentInfo);
                }
            }
        }

    }
}
