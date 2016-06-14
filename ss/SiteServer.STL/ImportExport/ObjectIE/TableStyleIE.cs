using System.Collections;
using System.Collections.Specialized;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.IO;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.ImportExport
{
	internal class TableStyleIE
	{
		private readonly string directoryPath;

        public TableStyleIE(string directoryPath)
		{
			this.directoryPath = directoryPath;
		}

		public void ExportTableStyles(int publishmentSystemID, string tableName)
		{
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemID);
            Hashtable tableStyleInfoWithItemsHashtable = TableStyleManager.GetTableStyleInfoWithItemsHashtable(tableName, nodeIDArrayList);
            if (tableStyleInfoWithItemsHashtable != null && tableStyleInfoWithItemsHashtable.Count > 0)
            {
                string styleDirectoryPath = PathUtils.Combine(this.directoryPath, tableName);
                DirectoryUtils.CreateDirectoryIfNotExists(styleDirectoryPath);

                foreach (string attributeName in tableStyleInfoWithItemsHashtable.Keys)
                {
                    ArrayList tableStyleInfoWithItemsArrayList = tableStyleInfoWithItemsHashtable[attributeName] as ArrayList;
                    if (tableStyleInfoWithItemsArrayList != null && tableStyleInfoWithItemsArrayList.Count > 0)
                    {
                        string attributeNameDirectoryPath = PathUtils.Combine(styleDirectoryPath, attributeName);
                        DirectoryUtils.CreateDirectoryIfNotExists(attributeNameDirectoryPath);

                        foreach (TableStyleInfo tableStyleInfo in tableStyleInfoWithItemsArrayList)
                        {
                            //仅导出当前系统内的表样式
                            if (tableStyleInfo.RelatedIdentity != 0)
                            {
                                if (!ChannelUtility.IsAncestorOrSelf(publishmentSystemID, publishmentSystemID, tableStyleInfo.RelatedIdentity))
                                {
                                    continue;
                                }
                            }
                            string filePath = attributeNameDirectoryPath + PathUtils.SeparatorChar + tableStyleInfo.TableStyleID + ".xml";
                            AtomFeed feed = ExportTableStyleInfo(tableStyleInfo);
                            if (tableStyleInfo.StyleItems != null && tableStyleInfo.StyleItems.Count > 0)
                            {
                                foreach (TableStyleItemInfo styleItemInfo in tableStyleInfo.StyleItems)
                                {
                                    AtomEntry entry = ExportTableStyleItemInfo(styleItemInfo);
                                    feed.Entries.Add(entry);
                                }
                            }
                            feed.Save(filePath);
                        }
                    }
                }
            }
		}

        private static AtomFeed ExportTableStyleInfo(TableStyleInfo tableStyleInfo)
		{
			AtomFeed feed = AtomUtility.GetEmptyFeed();

            AtomUtility.AddDcElement(feed.AdditionalElements, "TableStyleID", tableStyleInfo.TableStyleID.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "RelatedIdentity", tableStyleInfo.RelatedIdentity.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "TableName", tableStyleInfo.TableName);
            AtomUtility.AddDcElement(feed.AdditionalElements, "AttributeName", tableStyleInfo.AttributeName);
            AtomUtility.AddDcElement(feed.AdditionalElements, "Taxis", tableStyleInfo.Taxis.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "DisplayName", tableStyleInfo.DisplayName);
            AtomUtility.AddDcElement(feed.AdditionalElements, "HelpText", tableStyleInfo.HelpText);
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsVisible", tableStyleInfo.IsVisible.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsVisibleInList", tableStyleInfo.IsVisibleInList.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsSingleLine", tableStyleInfo.IsSingleLine.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "InputType", EInputTypeUtils.GetValue(tableStyleInfo.InputType));
            AtomUtility.AddDcElement(feed.AdditionalElements, "DefaultValue", tableStyleInfo.DefaultValue);
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsHorizontal", tableStyleInfo.IsHorizontal.ToString());
            //SettingsXML
            AtomUtility.AddDcElement(feed.AdditionalElements, "ExtendValues", tableStyleInfo.ExtendValues);

            //保存此栏目样式在系统中的排序号
            string orderString = string.Empty;
            if (tableStyleInfo.RelatedIdentity != 0)
            {
                orderString = DataProvider.NodeDAO.GetOrderStringInPublishmentSystem(tableStyleInfo.RelatedIdentity);
            }

            AtomUtility.AddDcElement(feed.AdditionalElements, "OrderString", orderString);

			return feed;
		}

        private static AtomEntry ExportTableStyleItemInfo(TableStyleItemInfo styleItemInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, "TableStyleItemID", styleItemInfo.TableStyleItemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "TableStyleID", styleItemInfo.TableStyleID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "ItemTitle", styleItemInfo.ItemTitle);
            AtomUtility.AddDcElement(entry.AdditionalElements, "ItemValue", styleItemInfo.ItemValue);
            AtomUtility.AddDcElement(entry.AdditionalElements, "IsSelected", styleItemInfo.IsSelected.ToString());

			return entry;
		}

        public static void SingleExportTableStyles(ETableStyle tableStyle, string tableName, int publishmentSystemID, int relatedIdentity, string styleDirectoryPath)
        {
            ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(tableStyle, publishmentSystemID, relatedIdentity);

            DirectoryUtils.DeleteDirectoryIfExists(styleDirectoryPath);
            DirectoryUtils.CreateDirectoryIfNotExists(styleDirectoryPath);

            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
            foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
            {
                string filePath = PathUtils.Combine(styleDirectoryPath, tableStyleInfo.AttributeName + ".xml");
                AtomFeed feed = ExportTableStyleInfo(tableStyleInfo);
                ArrayList styleItems = TableStyleManager.GetStyleItemArrayList(tableStyleInfo.TableStyleID);
                if (styleItems != null && styleItems.Count > 0)
                {
                    foreach (TableStyleItemInfo styleItemInfo in styleItems)
                    {
                        AtomEntry entry = ExportTableStyleItemInfo(styleItemInfo);
                        feed.Entries.Add(entry);
                    }
                }
                feed.Save(filePath);
            }
        }

        public static void SingleImportTableStyle(ETableStyle tableStyle, string tableName, string styleDirectoryPath, int publishmentSystemID, int relatedIdentity)
        {
            if (!DirectoryUtils.IsDirectoryExists(styleDirectoryPath)) return;

            string[] filePaths = DirectoryUtils.GetFilePaths(styleDirectoryPath);
            foreach (string filePath in filePaths)
            {
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                string attributeName = AtomUtility.GetDcElementContent(feed.AdditionalElements, "AttributeName");
                int taxis = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "Taxis"), 0);
                string displayName = AtomUtility.GetDcElementContent(feed.AdditionalElements, "DisplayName");
                string helpText = AtomUtility.GetDcElementContent(feed.AdditionalElements, "HelpText");
                bool isVisible = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsVisible"));
                bool isVisibleInList = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsVisibleInList"));
                bool isSingleLine = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsSingleLine"));
                EInputType inputType = EInputTypeUtils.GetEnumType(AtomUtility.GetDcElementContent(feed.AdditionalElements, "InputType"));
                string defaultValue = AtomUtility.GetDcElementContent(feed.AdditionalElements, "DefaultValue");
                bool isHorizontal = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsHorizontal"));
                //SettingsXML
                string extendValues = AtomUtility.GetDcElementContent(feed.AdditionalElements, "ExtendValues");

                TableStyleInfo styleInfo = new TableStyleInfo(0, relatedIdentity, tableName, attributeName, taxis, displayName, helpText, isVisible, isVisibleInList, isSingleLine, inputType, defaultValue, isHorizontal, extendValues);

                ArrayList styleItems = new ArrayList();
                foreach (AtomEntry entry in feed.Entries)
                {
                    string itemTitle = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ItemTitle");
                    string itemValue = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ItemValue");
                    bool IsSelected = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsSelected"));

                    styleItems.Add(new TableStyleItemInfo(0, 0, itemTitle, itemValue, IsSelected));
                }

                if (styleItems.Count > 0)
                {
                    styleInfo.StyleItems = styleItems;
                }

                if (TableStyleManager.IsExists(relatedIdentity, tableName, attributeName))
                {
                    TableStyleManager.Delete(relatedIdentity, tableName, attributeName);
                }
                TableStyleManager.InsertWithTaxis(styleInfo, tableStyle);
            }
        }

        public void ImportTableStyles(int publishmentSystemID)
		{
			if (!DirectoryUtils.IsDirectoryExists(this.directoryPath)) return;

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            ImportObject importObject = new ImportObject(publishmentSystemID);
            NameValueCollection tableNameCollection = importObject.GetTableNameCache();

			string[] styleDirectoryPaths = DirectoryUtils.GetDirectoryPaths(this.directoryPath);

            foreach (string styleDirectoryPath in styleDirectoryPaths)
            {
                string tableName = PathUtils.GetDirectoryName(styleDirectoryPath);
                if (tableNameCollection != null && !string.IsNullOrEmpty(tableNameCollection[tableName]))
                {
                    tableName = tableNameCollection[tableName];
                }

                ETableStyle tableStyle = ETableStyle.UserDefined;

                if (BaiRongDataProvider.TableCollectionDAO.IsTableExists(tableName))
                {
                    EAuxiliaryTableType tableType = BaiRongDataProvider.TableCollectionDAO.GetTableType(tableName);
                    tableStyle = EAuxiliaryTableTypeUtils.GetTableStyle(tableType);
                }
                else
                {
                    tableStyle = PublishmentSystemManager.GetTableStyle(publishmentSystemInfo, tableName);
                }

                string[] attributeNamePaths = DirectoryUtils.GetDirectoryPaths(styleDirectoryPath);
                foreach (string attributeNamePath in attributeNamePaths)
                {
                    string attributeName = PathUtils.GetDirectoryName(attributeNamePath);
                    string[] filePaths = DirectoryUtils.GetFilePaths(attributeNamePath);
                    foreach (string filePath in filePaths)
                    {
                        AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                        int taxis = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "Taxis"), 0);
                        string displayName = AtomUtility.GetDcElementContent(feed.AdditionalElements, "DisplayName");
                        string helpText = AtomUtility.GetDcElementContent(feed.AdditionalElements, "HelpText");
                        bool isVisible = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsVisible"));
                        bool isVisibleInList = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsVisibleInList"));
                        bool isSingleLine = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsSingleLine"));
                        EInputType inputType = EInputTypeUtils.GetEnumType(AtomUtility.GetDcElementContent(feed.AdditionalElements, "InputType"));
                        string defaultValue = AtomUtility.GetDcElementContent(feed.AdditionalElements, "DefaultValue");
                        bool isHorizontal = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(feed.AdditionalElements, "IsHorizontal"));
                        string extendValues = AtomUtility.GetDcElementContent(feed.AdditionalElements, "ExtendValues");

                        int relatedIdentity = 0;
                        if (tableStyle == ETableStyle.Site)
                        {
                            relatedIdentity = publishmentSystemID;
                        }
                        else
                        {
                            string orderString = AtomUtility.GetDcElementContent(feed.AdditionalElements, "OrderString");

                            if (!string.IsNullOrEmpty(orderString))
                            {
                                relatedIdentity = DataProvider.NodeDAO.GetNodeID(publishmentSystemID, orderString);
                            }
                        }                        

                        if (!TableStyleManager.IsExists(relatedIdentity, tableName, attributeName))
                        {
                            TableStyleInfo styleInfo = new TableStyleInfo(0, relatedIdentity, tableName, attributeName, taxis, displayName, helpText, isVisible, isVisibleInList, isSingleLine, inputType, defaultValue, isHorizontal, extendValues);

                            ArrayList styleItems = new ArrayList();
                            foreach (AtomEntry entry in feed.Entries)
                            {
                                string itemTitle = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ItemTitle");
                                string itemValue = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ItemValue");
                                bool IsSelected = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsSelected"));

                                styleItems.Add(new TableStyleItemInfo(0, 0, itemTitle, itemValue, IsSelected));
                            }

                            if (styleItems.Count > 0)
                            {
                                styleInfo.StyleItems = styleItems;
                            }

                            TableStyleManager.InsertWithTaxis(styleInfo, tableStyle);
                        }
                    }
                }
            }
		}

	}
}
