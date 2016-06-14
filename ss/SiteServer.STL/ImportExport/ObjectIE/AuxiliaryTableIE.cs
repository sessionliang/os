using System.Collections;
using System.Collections.Specialized;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.ImportExport
{
	internal class AuxiliaryTableIE
	{
		private readonly string directoryPath;

		public AuxiliaryTableIE(string directoryPath)
		{
			this.directoryPath = directoryPath;
		}

		public void ExportAuxiliaryTable(string tableName)
		{
            AuxiliaryTableInfo tableInfo = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableInfo(tableName);
			if (tableInfo != null)
			{
                ArrayList metaInfoArrayList = TableManager.GetTableMetadataInfoArrayList(tableInfo.TableENName);
				string filePath = this.directoryPath + PathUtils.SeparatorChar + tableInfo.TableENName + ".xml";

				AtomFeed feed = GetAtomFeed(tableInfo);

				foreach (TableMetadataInfo metaInfo in metaInfoArrayList)
				{
					AtomEntry entry = GetAtomEntry(metaInfo);
					feed.Entries.Add(entry);
				}
				feed.Save(filePath);
			}
		}

		private static AtomFeed GetAtomFeed(AuxiliaryTableInfo tableInfo)
		{
			AtomFeed feed = AtomUtility.GetEmptyFeed();

			AtomUtility.AddDcElement(feed.AdditionalElements, "TableENName", tableInfo.TableENName);
			AtomUtility.AddDcElement(feed.AdditionalElements, "TableCNName", tableInfo.TableCNName);
			AtomUtility.AddDcElement(feed.AdditionalElements, "AttributeNum", tableInfo.AttributeNum.ToString());
			AtomUtility.AddDcElement(feed.AdditionalElements, "AuxiliaryTableType", EAuxiliaryTableTypeUtils.GetValue(tableInfo.AuxiliaryTableType));
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsCreatedInDB", tableInfo.IsCreatedInDB.ToString());
            AtomUtility.AddDcElement(feed.AdditionalElements, "IsChangedAfterCreatedInDB", tableInfo.IsChangedAfterCreatedInDB.ToString());
			AtomUtility.AddDcElement(feed.AdditionalElements, "Description", tableInfo.Description);
            //表唯一序列号
            AtomUtility.AddDcElement(feed.AdditionalElements, "SerializedString", TableManager.GetSerializedString(tableInfo.TableENName));

			return feed;
		}

		private static AtomEntry GetAtomEntry(TableMetadataInfo metaInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

			AtomUtility.AddDcElement(entry.AdditionalElements, "TableMetadataID", metaInfo.TableMetadataID.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "AuxiliaryTableENName", metaInfo.AuxiliaryTableENName);
			AtomUtility.AddDcElement(entry.AdditionalElements, "AttributeName", metaInfo.AttributeName);
			AtomUtility.AddDcElement(entry.AdditionalElements, "DataType", EDataTypeUtils.GetValue(metaInfo.DataType));
			AtomUtility.AddDcElement(entry.AdditionalElements, "DataLength", metaInfo.DataLength.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "CanBeNull", metaInfo.CanBeNull.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "DBDefaultValue", metaInfo.DBDefaultValue);
			AtomUtility.AddDcElement(entry.AdditionalElements, "Taxis", metaInfo.Taxis.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "IsSystem", metaInfo.IsSystem.ToString());

			return entry;
		}


        /// <summary>
        /// 将频道模板中的辅助表导入发布系统中，返回修改了的表名对照
        /// 在导入辅助表的同时检查发布系统辅助表并替换对应表
        /// </summary>
        public NameValueCollection ImportAuxiliaryTables(int publishmentSystemID, bool isUserTables)
		{
			if (!DirectoryUtils.IsDirectoryExists(this.directoryPath)) return null;

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            NameValueCollection nameValueCollection = new NameValueCollection();
            string tableNamePrefix = publishmentSystemInfo.PublishmentSystemDir + "_";

			string[] filePaths = DirectoryUtils.GetFilePaths(this.directoryPath);

            foreach (string filePath in filePaths)
            {
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                string tableName = AtomUtility.GetDcElementContent(feed.AdditionalElements, "TableENName");
                EAuxiliaryTableType tableType = EAuxiliaryTableTypeUtils.GetEnumType(AtomUtility.GetDcElementContent(feed.AdditionalElements, "AuxiliaryTableType"));

                if (!isUserTables)
                {
                    nameValueCollection[tableName] = NodeManager.GetTableName(publishmentSystemInfo, tableType);
                    continue;
                }

                string tableCNName = AtomUtility.GetDcElementContent(feed.AdditionalElements, "TableCNName");
                string serializedString = AtomUtility.GetDcElementContent(feed.AdditionalElements, "SerializedString");

                string tableNameToInsert = string.Empty;//需要增加的表名，空代表不需要添加辅助表

                AuxiliaryTableInfo tableInfo = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableInfo(tableName);
                if (tableInfo == null)//如果当前系统无此表名
                {
                    tableNameToInsert = tableName;
                }
                else
                {
                    string serializedStringForExistTable = TableManager.GetSerializedString(tableName);

                    if (!string.IsNullOrEmpty(serializedString))
                    {
                        if (serializedString != serializedStringForExistTable)//仅有此时，导入表需要修改表名
                        {
                            tableNameToInsert = tableNamePrefix + tableName;
                            tableCNName = tableNamePrefix + tableCNName;
                            nameValueCollection[tableName] = tableNameToInsert;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(tableNameToInsert))//需要添加
                {
                    if (!BaiRongDataProvider.TableStructureDAO.IsTableExists(tableNameToInsert))
                    {
                        tableInfo = new AuxiliaryTableInfo();
                        tableInfo.TableENName = tableNameToInsert;
                        tableInfo.TableCNName = tableCNName;
                        tableInfo.AttributeNum = 0;
                        tableInfo.AuxiliaryTableType = tableType;
                        tableInfo.IsCreatedInDB = false;
                        tableInfo.IsChangedAfterCreatedInDB = false;
                        tableInfo.Description = AtomUtility.GetDcElementContent(feed.AdditionalElements, "Description");

                        BaiRongDataProvider.TableCollectionDAO.Insert(tableInfo);

                        ETableStyle tableStyle = EAuxiliaryTableTypeUtils.GetTableStyle(tableInfo.AuxiliaryTableType);

                        ArrayList attributeNameArrayList =
                            TableManager.GetAttributeNameArrayList(tableStyle, tableInfo.TableENName, true);
                        attributeNameArrayList.AddRange(
                            TableManager.GetHiddenAttributeNameArrayList(tableStyle));

                        foreach (AtomEntry entry in feed.Entries)
                        {
                            TableMetadataInfo metaInfo = new TableMetadataInfo();
                            metaInfo.AuxiliaryTableENName = tableNameToInsert;
                            metaInfo.AttributeName =
                                AtomUtility.GetDcElementContent(entry.AdditionalElements, "AttributeName");
                            metaInfo.DataType =
                                EDataTypeUtils.GetEnumType(
                                    AtomUtility.GetDcElementContent(entry.AdditionalElements, "DataType"));
                            metaInfo.DataLength =
                                TranslateUtils.ToInt(
                                    AtomUtility.GetDcElementContent(entry.AdditionalElements, "DataLength"));
                            metaInfo.CanBeNull =
                                TranslateUtils.ToBool(
                                    AtomUtility.GetDcElementContent(entry.AdditionalElements, "CanBeNull"));
                            metaInfo.DBDefaultValue =
                                AtomUtility.GetDcElementContent(entry.AdditionalElements, "DBDefaultValue");
                            metaInfo.Taxis =
                                TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Taxis"));
                            metaInfo.IsSystem =
                                TranslateUtils.ToBool(
                                    AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsSystem"));

                            if (attributeNameArrayList.IndexOf(metaInfo.AttributeName.Trim().ToLower()) != -1) continue;

                            if (metaInfo.IsSystem) continue;

                            BaiRongDataProvider.TableMetadataDAO.Insert(metaInfo);
                        }

                        BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTable(tableNameToInsert);
                    }
                }

                string tableNameToChange = (!string.IsNullOrEmpty(tableNameToInsert)) ? tableNameToInsert : tableName;
                //更新发布系统后台内容表及栏目表
                if (tableType == EAuxiliaryTableType.BackgroundContent)
                {
                    publishmentSystemInfo.AuxiliaryTableForContent = tableNameToChange;
                    DataProvider.PublishmentSystemDAO.Update(publishmentSystemInfo);
                }
                else if (tableType == EAuxiliaryTableType.GovPublicContent)
                {
                    publishmentSystemInfo.AuxiliaryTableForGovPublic = tableNameToChange;
                    DataProvider.PublishmentSystemDAO.Update(publishmentSystemInfo);
                }
                else if (tableType == EAuxiliaryTableType.GovInteractContent)
                {
                    publishmentSystemInfo.AuxiliaryTableForGovInteract = tableNameToChange;
                    DataProvider.PublishmentSystemDAO.Update(publishmentSystemInfo);
                }
                else if (tableType == EAuxiliaryTableType.JobContent)
                {
                    publishmentSystemInfo.AuxiliaryTableForJob = tableNameToChange;
                    DataProvider.PublishmentSystemDAO.Update(publishmentSystemInfo);
                }
                else if (tableType == EAuxiliaryTableType.VoteContent)
                {
                    publishmentSystemInfo.AuxiliaryTableForVote = tableNameToChange;
                    DataProvider.PublishmentSystemDAO.Update(publishmentSystemInfo);
                }
            }

            return nameValueCollection;
		}

	}
}
