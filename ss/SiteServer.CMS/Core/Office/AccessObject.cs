using System.Collections;
using System.Data.OleDb;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System;
using BaiRong.Core.Data.Provider;
using System.Data;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core.Office
{
	public class AccessObject
	{
        public static bool CreateAccessFileForContents(string filePath, PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, ArrayList contentIDArrayList, ArrayList displayAttributes, bool isPeriods, string dateFrom, string dateTo, ETriState checkedState)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string sourceFilePath = PathUtils.GetSiteFilesPath(SiteFiles.Default.AccessMDB);
            FileUtils.CopyFile(sourceFilePath, filePath);

            ArrayList relatedidentityes = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID);

            ContentModelInfo modelInfo = ContentModelManager.GetContentModelInfo(publishmentSystemInfo, nodeInfo.ContentModelID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, modelInfo.TableName, relatedidentityes);
            tableStyleInfoArrayList = ContentUtility.GetAllTableStyleInfoArrayList(publishmentSystemInfo, tableStyle, tableStyleInfoArrayList);
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

            AccessDAO accessDAO = new AccessDAO(filePath);

            string createTableSqlString = accessDAO.GetCreateTableSqlString(nodeInfo.NodeName, tableStyleInfoArrayList, displayAttributes);
            accessDAO.ExecuteSqlString(createTableSqlString);

            bool isExport = true;

            ArrayList insertSqlArrayList = accessDAO.GetInsertSqlStringArrayList(nodeInfo.NodeName, publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, tableStyle, tableName, tableStyleInfoArrayList, displayAttributes, contentIDArrayList, isPeriods, dateFrom, dateTo, checkedState, out isExport);

            foreach (string insertSql in insertSqlArrayList)
            {
                accessDAO.ExecuteSqlString(insertSql);
            }

            return isExport;
        }

        public static ArrayList GetContentsByAccessFile(string filePath, PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            ArrayList contentInfoArrayList = new ArrayList();

            AccessDAO accessDAO = new AccessDAO(filePath);
            string[] tableNames = accessDAO.GetTableNames();
            if (tableNames != null && tableNames.Length > 0)
            {
                foreach (string tableName in tableNames)
                {
                    string sqlString = string.Format("SELECT * FROM [{0}]", tableName);
                    DataSet dataset = accessDAO.ReturnDataSet(sqlString);

                    DataTable oleDt = dataset.Tables[0];

                    if (oleDt.Rows.Count > 0)
                    {
                        ArrayList relatedidentityes = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID);

                        ContentModelInfo modelInfo = ContentModelManager.GetContentModelInfo(publishmentSystemInfo, nodeInfo.ContentModelID);
                        ETableStyle tableStyle = EAuxiliaryTableTypeUtils.GetTableStyle(modelInfo.TableType);

                        ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, modelInfo.TableName, relatedidentityes);

                        NameValueCollection nameValueCollection = new NameValueCollection();

                        foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                        {
                            nameValueCollection[styleInfo.DisplayName] = styleInfo.AttributeName.ToLower();
                        }

                        ArrayList attributeNames = new ArrayList();
                        for (int i = 0; i < oleDt.Columns.Count; i++)
                        {
                            string columnName = oleDt.Columns[i].ColumnName;
                            if (!string.IsNullOrEmpty(nameValueCollection[columnName]))
                            {
                                attributeNames.Add(nameValueCollection[columnName]);
                            }
                            else
                            {
                                attributeNames.Add(columnName);
                            }
                        }

                        foreach (DataRow row in oleDt.Rows)
                        {
                            BackgroundContentInfo contentInfo = new BackgroundContentInfo();

                            for (int i = 0; i < oleDt.Columns.Count; i++)
                            {
                                string attributeName = attributeNames[i] as string;
                                if (!string.IsNullOrEmpty(attributeName))
                                {
                                    string value = row[i].ToString();
                                    contentInfo.SetExtendedAttribute(attributeName, value);
                                }
                            }

                            if (!string.IsNullOrEmpty(contentInfo.Title))
                            {
                                contentInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                                contentInfo.NodeID = nodeInfo.NodeID;
                                contentInfo.LastEditDate = DateTime.Now;

                                contentInfoArrayList.Add(contentInfo);
                            }
                        }
                    }
                }
            }

            return contentInfoArrayList;
        }
	}
}
