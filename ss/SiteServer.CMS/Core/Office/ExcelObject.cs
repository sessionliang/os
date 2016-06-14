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
using System.Collections.Generic;

namespace SiteServer.CMS.Core.Office
{
    public class ExcelObject
    {
        public static void CreateExcelFileForInputContents(string filePath, int publishmentSystemID, InputInfo inputInfo)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";
            StringBuilder createBuilder = new StringBuilder();

            createBuilder.AppendFormat("CREATE TABLE {0} ( ", DataProvider.InputContentDAO.TableName);

            ArrayList relatedidentityes = RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, publishmentSystemID, inputInfo.InputID);
            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, relatedidentityes);

            if (tableStyleInfoArrayList.Count == 0)
            {
                throw new Exception("表单无字段，无法导出");
            }

            foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
            {
                createBuilder.AppendFormat(" [{0}] NTEXT, ", tableStyleInfo.DisplayName);
            }

            if (inputInfo.IsReply)
            {
                createBuilder.AppendFormat(" [{0}] NTEXT, ", "回复");
            }
            createBuilder.AppendFormat(" [{0}] NTEXT, ", "添加时间");
            createBuilder.Length = createBuilder.Length - 2;
            createBuilder.Append(" )");

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.AppendFormat("INSERT INTO {0} (", DataProvider.InputContentDAO.TableName);
            foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
            {
                preInsertBuilder.AppendFormat("[{0}], ", tableStyleInfo.DisplayName);
            }

            if (inputInfo.IsReply)
            {
                preInsertBuilder.AppendFormat("[{0}], ", "回复");
            }
            preInsertBuilder.AppendFormat("[{0}], ", "添加时间");
            preInsertBuilder.Length = preInsertBuilder.Length - 2;
            preInsertBuilder.Append(") VALUES (");

            ArrayList insertSqlArrayList = new ArrayList();
            ArrayList contentIDArrayList = DataProvider.InputContentDAO.GetContentIDArrayListWithChecked(inputInfo.InputID);
            foreach (int contentID in contentIDArrayList)
            {
                InputContentInfo contentInfo = DataProvider.InputContentDAO.GetContentInfo(contentID);
                if (contentInfo != null)
                {
                    StringBuilder insertBuilder = new StringBuilder();
                    insertBuilder.Append(preInsertBuilder.ToString());

                    foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
                    {
                        string value = contentInfo.GetExtendedAttribute(tableStyleInfo.AttributeName);
                        insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(value)));
                    }

                    if (inputInfo.IsReply)
                    {
                        insertBuilder.AppendFormat("'{0}', ", StringUtils.StripTags(contentInfo.Reply));
                    }
                    insertBuilder.AppendFormat("'{0}', ", contentInfo.AddDate.ToString());
                    insertBuilder.Length = insertBuilder.Length - 2;
                    insertBuilder.Append(") ");

                    insertSqlArrayList.Add(insertBuilder.ToString());
                }
            }

            OleDbConnection oConn = new OleDbConnection();

            oConn.ConnectionString = OLEDBConnStr;
            OleDbCommand oCreateComm = new OleDbCommand();
            oCreateComm.Connection = oConn;
            oCreateComm.CommandText = createBuilder.ToString();

            oConn.Open();
            oCreateComm.ExecuteNonQuery();
            foreach (string insertSql in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();
        }

        public static void CreateExcelFileForWebsiteMessageContents(string filePath, int publishmentSystemID, WebsiteMessageInfo websiteMessageInfo, int classifyID)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";
            StringBuilder createBuilder = new StringBuilder();

            createBuilder.AppendFormat("CREATE TABLE {0} ( ", DataProvider.WebsiteMessageContentDAO.TableName);

            ArrayList relatedidentityes = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, publishmentSystemID, websiteMessageInfo.WebsiteMessageID);
            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, relatedidentityes);

            if (tableStyleInfoArrayList.Count == 0)
            {
                throw new Exception("表单无字段，无法导出");
            }

            foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
            {
                createBuilder.AppendFormat(" [{0}] NTEXT, ", tableStyleInfo.DisplayName);
            }

            if (websiteMessageInfo.IsReply)
            {
                createBuilder.AppendFormat(" [{0}] NTEXT, ", "回复");
            }
            createBuilder.AppendFormat(" [{0}] NTEXT, ", "添加时间");
            createBuilder.Length = createBuilder.Length - 2;
            createBuilder.Append(" )");

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.AppendFormat("INSERT INTO {0} (", DataProvider.WebsiteMessageContentDAO.TableName);
            foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
            {
                preInsertBuilder.AppendFormat("[{0}], ", tableStyleInfo.DisplayName);
            }

            if (websiteMessageInfo.IsReply)
            {
                preInsertBuilder.AppendFormat("[{0}], ", "回复");
            }
            preInsertBuilder.AppendFormat("[{0}], ", "添加时间");
            preInsertBuilder.Length = preInsertBuilder.Length - 2;
            preInsertBuilder.Append(") VALUES (");

            ArrayList insertSqlArrayList = new ArrayList();
            ArrayList contentIDArrayList = DataProvider.WebsiteMessageContentDAO.GetContentIDArrayListWithChecked(websiteMessageInfo.WebsiteMessageID, classifyID);
            foreach (int contentID in contentIDArrayList)
            {
                WebsiteMessageContentInfo contentInfo = DataProvider.WebsiteMessageContentDAO.GetContentInfo(contentID);
                if (contentInfo != null)
                {
                    StringBuilder insertBuilder = new StringBuilder();
                    insertBuilder.Append(preInsertBuilder.ToString());

                    foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
                    {
                        string value = contentInfo.GetExtendedAttribute(tableStyleInfo.AttributeName);
                        insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(value)));
                    }

                    if (websiteMessageInfo.IsReply)
                    {
                        insertBuilder.AppendFormat("'{0}', ", StringUtils.StripTags(contentInfo.Reply));
                    }
                    insertBuilder.AppendFormat("'{0}', ", contentInfo.AddDate.ToString());
                    insertBuilder.Length = insertBuilder.Length - 2;
                    insertBuilder.Append(") ");

                    insertSqlArrayList.Add(insertBuilder.ToString());
                }
            }

            OleDbConnection oConn = new OleDbConnection();

            oConn.ConnectionString = OLEDBConnStr;
            OleDbCommand oCreateComm = new OleDbCommand();
            oCreateComm.Connection = oConn;
            oCreateComm.CommandText = createBuilder.ToString();

            oConn.Open();
            oCreateComm.ExecuteNonQuery();
            foreach (string insertSql in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();
        }

        public static void CreateExcelFileForMailSubscribe(string filePath, int publishmentSystemID)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";
            StringBuilder createBuilder = new StringBuilder();

            createBuilder.Append("CREATE TABLE 邮件订阅 ( [邮箱] NTEXT, [姓名] NTEXT, [IP地址] NTEXT, [订阅时间] NTEXT )");

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.Append("INSERT INTO 邮件订阅 ([邮箱], [姓名], [IP地址], [订阅时间]) VALUES (");

            ArrayList insertSqlArrayList = new ArrayList();
            ArrayList mailSubscribeInfoArrayList = DataProvider.MailSubscribeDAO.GetMailSubscribeInfoArrayList(publishmentSystemID);
            foreach (MailSubscribeInfo msInfo in mailSubscribeInfoArrayList)
            {
                StringBuilder insertBuilder = new StringBuilder();
                insertBuilder.Append(preInsertBuilder.ToString());

                insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(msInfo.Mail)));
                insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(msInfo.Receiver)));
                insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(msInfo.IPAddress)));
                insertBuilder.AppendFormat("'{0}') ", SqlUtils.ToSqlString(DateUtils.GetDateAndTimeString(msInfo.AddDate)));

                insertSqlArrayList.Add(insertBuilder.ToString());
            }

            OleDbConnection oConn = new OleDbConnection();

            oConn.ConnectionString = OLEDBConnStr;
            OleDbCommand oCreateComm = new OleDbCommand();
            oCreateComm.Connection = oConn;
            oCreateComm.CommandText = createBuilder.ToString();

            oConn.Open();
            oCreateComm.ExecuteNonQuery();
            foreach (string insertSql in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();
        }

        public static bool CreateExcelFileForContents(string filePath, PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo, ArrayList contentIDArrayList, ArrayList displayAttributes, bool isPeriods, string startDate, string endDate, ETriState checkedState)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";
            StringBuilder createBuilder = new StringBuilder();

            createBuilder.AppendFormat("CREATE TABLE {0} ( ", nodeInfo.NodeName);

            ArrayList relatedidentityes = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID);
            ContentModelInfo modelInfo = ContentModelManager.GetContentModelInfo(publishmentSystemInfo, nodeInfo.ContentModelID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, modelInfo.TableName, relatedidentityes);
            tableStyleInfoArrayList = ContentUtility.GetAllTableStyleInfoArrayList(publishmentSystemInfo, tableStyle, tableStyleInfoArrayList);

            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

            foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
            {
                if (displayAttributes.Contains(tableStyleInfo.AttributeName))
                {
                    createBuilder.AppendFormat(" [{0}] NTEXT, ", tableStyleInfo.DisplayName);
                }
            }

            createBuilder.Length = createBuilder.Length - 2;
            createBuilder.Append(" )");

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.AppendFormat("INSERT INTO {0} (", nodeInfo.NodeName);

            foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
            {
                if (displayAttributes.Contains(tableStyleInfo.AttributeName))
                {
                    preInsertBuilder.AppendFormat("[{0}], ", tableStyleInfo.DisplayName);
                }
            }

            preInsertBuilder.Length = preInsertBuilder.Length - 2;
            preInsertBuilder.Append(") VALUES (");

            if (contentIDArrayList == null || contentIDArrayList.Count == 0)
            {
                contentIDArrayList = BaiRongDataProvider.ContentDAO.GetContentIDArrayList(tableName, nodeInfo.NodeID, isPeriods, startDate, endDate, checkedState);
            }
            if (contentIDArrayList.Count == 0) return false;

            ArrayList insertSqlArrayList = new ArrayList();
            foreach (int contentID in contentIDArrayList)
            {
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                if (contentInfo != null)
                {
                    StringBuilder insertBuilder = new StringBuilder();
                    insertBuilder.Append(preInsertBuilder.ToString());

                    foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
                    {
                        if (displayAttributes.Contains(tableStyleInfo.AttributeName))
                        {
                            string value = contentInfo.GetExtendedAttribute(tableStyleInfo.AttributeName);
                            insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(value)));
                        }
                    }

                    insertBuilder.Length = insertBuilder.Length - 2;
                    insertBuilder.Append(") ");

                    insertSqlArrayList.Add(insertBuilder.ToString());
                }
            }

            OleDbConnection oConn = new OleDbConnection();

            oConn.ConnectionString = OLEDBConnStr;
            OleDbCommand oCreateComm = new OleDbCommand();
            oCreateComm.Connection = oConn;
            oCreateComm.CommandText = createBuilder.ToString();

            oConn.Open();
            oCreateComm.ExecuteNonQuery();
            foreach (string insertSql in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();

            return true;
        }


        public static void CreateExcelFileForSurveyContents(string filePath, int publishmentSystemID, int nodeID, int contentID)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";
            StringBuilder createBuilder = new StringBuilder();

            createBuilder.AppendFormat("CREATE TABLE {0} ( ", SurveyQuestionnaireInfo.TableName);

            ETableStyle tableStyle = ETableStyle.SurveyContent;

            ArrayList relatedidentityes = RelatedIdentities.GetRelatedIdentities(tableStyle, publishmentSystemID, nodeID);
            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, SurveyQuestionnaireInfo.TableName, relatedidentityes);

            ArrayList trianApplyTableStyle = DataProvider.FunctionTableStylesDAO.GetInfoList(publishmentSystemID, nodeID,  contentID, tableStyle.ToString(), "files");

            ArrayList myStyleInfoArrayList = new ArrayList();
            if (tableStyleInfoArrayList != null)
            {
                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    if (trianApplyTableStyle.Count > 0)
                    {
                        if (trianApplyTableStyle.Contains(styleInfo.TableStyleID))
                        {
                            myStyleInfoArrayList.Add(styleInfo);
                        }
                    }
                    else
                        if (styleInfo.IsVisible)
                        {
                            myStyleInfoArrayList.Add(styleInfo);
                        }
                }
            }

            if (myStyleInfoArrayList.Count == 0)
            {
                throw new Exception("当前内容的调查问卷无字段，无法导出");
            }

            foreach (TableStyleInfo tableStyleInfo in myStyleInfoArrayList)
            {
                createBuilder.AppendFormat(" [{0}] NTEXT, ", tableStyleInfo.DisplayName);
            }

            createBuilder.AppendFormat(" [{0}] NTEXT, ", "添加时间");
            createBuilder.AppendFormat(" [{0}] NTEXT, ", "会员");
            createBuilder.Length = createBuilder.Length - 2;
            createBuilder.Append(" )");

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.AppendFormat("INSERT INTO {0} (", DataProvider.SurveyQuestionnaireDAO.TableName);
            foreach (TableStyleInfo tableStyleInfo in myStyleInfoArrayList)
            {
                preInsertBuilder.AppendFormat("[{0}], ", tableStyleInfo.DisplayName);
            }

            preInsertBuilder.AppendFormat("[{0}], ", "添加时间");
            preInsertBuilder.AppendFormat("[{0}], ", "会员");
            preInsertBuilder.Length = preInsertBuilder.Length - 2;
            preInsertBuilder.Append(") VALUES (");

            ArrayList insertSqlArrayList = new ArrayList();
            ArrayList contentIDArrayList = DataProvider.SurveyQuestionnaireDAO.GetInfoList(publishmentSystemID, nodeID, contentID);
            foreach (SurveyQuestionnaireInfo contentInfo in contentIDArrayList)
            {
                StringBuilder insertBuilder = new StringBuilder();
                insertBuilder.Append(preInsertBuilder.ToString());

                foreach (TableStyleInfo tableStyleInfo in myStyleInfoArrayList)
                {
                    string value = contentInfo.GetExtendedAttribute(tableStyleInfo.AttributeName);
                    insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(value)));
                }

                insertBuilder.AppendFormat("'{0}', ", contentInfo.AddDate.ToString());
                insertBuilder.AppendFormat("'{0}', ", contentInfo.UserName.ToString());
                insertBuilder.Length = insertBuilder.Length - 2;
                insertBuilder.Append(") ");

                insertSqlArrayList.Add(insertBuilder.ToString());

            }

            OleDbConnection oConn = new OleDbConnection();

            oConn.ConnectionString = OLEDBConnStr;
            OleDbCommand oCreateComm = new OleDbCommand();
            oCreateComm.Connection = oConn;
            oCreateComm.CommandText = createBuilder.ToString();

            oConn.Open();
            oCreateComm.ExecuteNonQuery();
            foreach (string insertSql in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();
        }


        //public static string CreateExcelFileForTestReport(PublishmentSystemInfo publishmentSystemInfo)
        //{
        //    string filePath = PathUtils.GetTemporaryFilesPath("TestReport.zip");
        //    DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
        //    FileUtils.DeleteFileIfExists(filePath);

        //    string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
        //    OLEDBConnStr += " Extended Properties=Excel 8.0;";
        //    StringBuilder createBuilder = new StringBuilder();

        //    createBuilder.AppendFormat("CREATE TABLE {0} ( ", "测试");

        //    createBuilder.Append(" [序号] NTEXT, ");
        //    createBuilder.Append(" [栏目名称] NTEXT, ");
        //    createBuilder.Append(" [页面地址] NTEXT, ");
        //    createBuilder.Append(" [测试未通过原因] NTEXT, ");

        //    createBuilder.Length = createBuilder.Length - 2;
        //    createBuilder.Append(" )");

        //    StringBuilder preInsertBuilder = new StringBuilder();
        //    preInsertBuilder.AppendFormat("INSERT INTO {0} (", "测试");

        //    preInsertBuilder.Append("[序号], ");
        //    preInsertBuilder.Append("[栏目名称], ");
        //    preInsertBuilder.Append("[页面地址], ");
        //    preInsertBuilder.Append("[测试未通过原因], ");

        //    preInsertBuilder.Length = preInsertBuilder.Length - 2;
        //    preInsertBuilder.Append(") VALUES (");

        //    ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);

        //    ArrayList insertSqlArrayList = new ArrayList();
        //    int index = 0;
        //    foreach (int nodeID in nodeIDArrayList)
        //    {
        //        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
        //        StringBuilder insertBuilder = new StringBuilder();
        //        insertBuilder.Append(preInsertBuilder.ToString());

        //        insertBuilder.AppendFormat("'序号', ", ++index);
        //        insertBuilder.AppendFormat("'栏目名称', ", nodeInfo.NodeName);
        //        insertBuilder.AppendFormat("'页面地址', ", PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, EVisualType.Static));
        //        insertBuilder.AppendFormat("'测试未通过原因', ", string.Empty);

        //        insertBuilder.Length = insertBuilder.Length - 2;
        //        insertBuilder.Append(") ");

        //        insertSqlArrayList.Add(insertBuilder.ToString());
        //    }

        //    OleDbConnection oConn = new OleDbConnection();

        //    oConn.ConnectionString = OLEDBConnStr;
        //    OleDbCommand oCreateComm = new OleDbCommand();
        //    oCreateComm.Connection = oConn;
        //    oCreateComm.CommandText = createBuilder.ToString();

        //    oConn.Open();
        //    oCreateComm.ExecuteNonQuery();
        //    foreach (string insertSql in insertSqlArrayList)
        //    {
        //        OleDbCommand oInsertComm = new OleDbCommand();
        //        oInsertComm.Connection = oConn;
        //        oInsertComm.CommandText = insertSql;
        //        oInsertComm.ExecuteNonQuery();
        //    }
        //    oConn.Close();

        //    return PathUtils.GetFileName(filePath);
        //}

        public static void CreateExcelFileForComments(string filePath, PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";
            StringBuilder createBuilder = new StringBuilder();

            createBuilder.AppendFormat("CREATE TABLE {0} ( ", DataProvider.InputContentDAO.TableName);
            createBuilder.Append(" [评论] NTEXT )");

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.AppendFormat("INSERT INTO {0} (", DataProvider.InputContentDAO.TableName);
            preInsertBuilder.Append("[评论]) VALUES (");

            ArrayList insertSqlArrayList = new ArrayList();
            List<int> commentIDList = DataProvider.CommentDAO.GetCommentIDListWithChecked(publishmentSystemInfo.PublishmentSystemID, contentID);
            foreach (int commentID in commentIDList)
            {
                CommentInfo commentInfo = DataProvider.CommentDAO.GetCommentInfo(commentID);
                if (commentInfo != null)
                {
                    StringBuilder insertBuilder = new StringBuilder();
                    insertBuilder.Append(preInsertBuilder.ToString());
                    insertBuilder.AppendFormat("'{0}') ", SqlUtils.ToSqlString(StringUtils.StripTags(commentInfo.Content)));
                    insertSqlArrayList.Add(insertBuilder.ToString());
                }
            }

            OleDbConnection oConn = new OleDbConnection();

            oConn.ConnectionString = OLEDBConnStr;
            OleDbCommand oCreateComm = new OleDbCommand();
            oCreateComm.Connection = oConn;
            oCreateComm.CommandText = createBuilder.ToString();

            oConn.Open();
            oCreateComm.ExecuteNonQuery();
            foreach (string insertSql in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();
        }

        public static void CreateExcelFileForTrackingHours(string filePath, int publishmentSystemID)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            Hashtable trackingHourHashtable = DataProvider.TrackingDAO.GetTrackingHourHashtable(publishmentSystemID);
            Hashtable uniqueTrackingHourHashtable = DataProvider.TrackingDAO.GetUniqueTrackingHourHashtable(publishmentSystemID);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";

            string createSqlString1 = "CREATE TABLE [访问量24小时分配图表] ( [时间段] INT, [访问量] INT )";
            string createSqlString2 = "CREATE TABLE [访客24小时分配图表] ( [时间段] INT, [访问量] INT )";

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.Append("INSERT INTO [访问量24小时分配图表] ( [时间段], [访问量] ) VALUES (");
            ArrayList insertSqlArrayList = new ArrayList();

            StringBuilder preInsertBuilder2 = new StringBuilder();
            preInsertBuilder2.Append("INSERT INTO [访客24小时分配图表] ( [时间段], [访问量] ) VALUES (");
            ArrayList insertSqlArrayList2 = new ArrayList();

            Hashtable accessNumHashtable = new Hashtable();
            int maxAccessNum = 0;
            Hashtable uniqueAccessNumHashtable = new Hashtable();
            int uniqueMaxAccessNum = 0;

            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            for (int i = 0; i < 24; i++)
            {
                DateTime datetime = now.AddHours(-i);
                int accessNum = 0;
                if (trackingHourHashtable[datetime] != null)
                {
                    accessNum = (int)trackingHourHashtable[datetime];
                }
                accessNumHashtable.Add(24 - i, accessNum);
                if (accessNum > maxAccessNum)
                {
                    maxAccessNum = accessNum;
                }

                int uniqueAccessNum = 0;
                if (uniqueTrackingHourHashtable[datetime] != null)
                {
                    uniqueAccessNum = (int)uniqueTrackingHourHashtable[datetime];
                }
                uniqueAccessNumHashtable.Add(24 - i, uniqueAccessNum);
                if (uniqueAccessNum > uniqueMaxAccessNum)
                {
                    uniqueMaxAccessNum = uniqueAccessNum;
                }

                insertSqlArrayList.Add(string.Format(preInsertBuilder.ToString() + " {0}, {1} )", datetime.Hour, accessNum));
                insertSqlArrayList2.Add(string.Format(preInsertBuilder2.ToString() + " {0}, {1} )", datetime.Hour, uniqueAccessNum));
            }

            OleDbConnection oConn = new OleDbConnection();
            oConn.ConnectionString = OLEDBConnStr;
            oConn.Open();

            OleDbCommand createComm1 = new OleDbCommand();
            createComm1.Connection = oConn;
            createComm1.CommandText = createSqlString1;
            createComm1.ExecuteNonQuery();

            OleDbCommand createComm2 = new OleDbCommand();
            createComm2.Connection = oConn;
            createComm2.CommandText = createSqlString2;
            createComm2.ExecuteNonQuery();

            foreach (string insertSql1 in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql1;
                oInsertComm.ExecuteNonQuery();
            }
            foreach (string insertSql2 in insertSqlArrayList2)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql2;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();
        }

        public static void CreateExcelFileForTrackingDays(string filePath, int publishmentSystemID)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            Hashtable trackingDayHashtable = DataProvider.TrackingDAO.GetTrackingDayHashtable(publishmentSystemID);
            Hashtable uniqueTrackingDayHashtable = DataProvider.TrackingDAO.GetUniqueTrackingDayHashtable(publishmentSystemID);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";

            string createSqlString1 = "CREATE TABLE [访问量最近30日分配图表] ( [时间段] INT, [访问量] INT )";
            string createSqlString2 = "CREATE TABLE [访客最近30日分配图表] ( [时间段] INT, [访问量] INT )";

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.Append("INSERT INTO [访问量最近30日分配图表] ( [时间段], [访问量] ) VALUES (");
            ArrayList insertSqlArrayList = new ArrayList();

            StringBuilder preInsertBuilder2 = new StringBuilder();
            preInsertBuilder2.Append("INSERT INTO [访客最近30日分配图表] ( [时间段], [访问量] ) VALUES (");
            ArrayList insertSqlArrayList2 = new ArrayList();

            Hashtable accessNumHashtable = new Hashtable();
            Hashtable uniqueAccessNumHashtable = new Hashtable();
            int maxAccessNum = 0;
            int uniqueMaxAccessNum = 0;

            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            for (int i = 0; i < 30; i++)
            {
                DateTime datetime = now.AddDays(-i);
                int accessNum = 0;
                if (trackingDayHashtable[datetime] != null)
                {
                    accessNum = (int)trackingDayHashtable[datetime];
                }
                accessNumHashtable.Add(30 - i, accessNum);
                if (accessNum > maxAccessNum)
                {
                    maxAccessNum = accessNum;
                }

                int uniqueAccessNum = 0;
                if (uniqueTrackingDayHashtable[datetime] != null)
                {
                    uniqueAccessNum = (int)uniqueTrackingDayHashtable[datetime];
                }
                uniqueAccessNumHashtable.Add(30 - i, uniqueAccessNum);
                if (uniqueAccessNum > uniqueMaxAccessNum)
                {
                    uniqueMaxAccessNum = uniqueAccessNum;
                }

                insertSqlArrayList.Add(string.Format(preInsertBuilder.ToString() + " {0}, {1} )", datetime.Day, accessNum));
                insertSqlArrayList2.Add(string.Format(preInsertBuilder2.ToString() + " {0}, {1} )", datetime.Day, uniqueAccessNum));
            }

            OleDbConnection oConn = new OleDbConnection();
            oConn.ConnectionString = OLEDBConnStr;
            oConn.Open();

            OleDbCommand createComm1 = new OleDbCommand();
            createComm1.Connection = oConn;
            createComm1.CommandText = createSqlString1;
            createComm1.ExecuteNonQuery();

            OleDbCommand createComm2 = new OleDbCommand();
            createComm2.Connection = oConn;
            createComm2.CommandText = createSqlString2;
            createComm2.ExecuteNonQuery();

            foreach (string insertSql1 in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql1;
                oInsertComm.ExecuteNonQuery();
            }
            foreach (string insertSql2 in insertSqlArrayList2)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql2;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();
        }

        public static void CreateExcelFileForTrackingMonths(string filePath, int publishmentSystemID)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            Hashtable trackingMonthHashtable = DataProvider.TrackingDAO.GetTrackingMonthHashtable(publishmentSystemID);
            Hashtable uniqueTrackingMonthHashtable = DataProvider.TrackingDAO.GetUniqueTrackingMonthHashtable(publishmentSystemID);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";

            string createSqlString1 = "CREATE TABLE [访问量最近12月分配图表] ( [时间段] INT, [访问量] INT )";
            string createSqlString2 = "CREATE TABLE [访客最近12月分配图表] ( [时间段] INT, [访问量] INT )";

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.Append("INSERT INTO [访问量最近12月分配图表] ( [时间段], [访问量] ) VALUES (");
            ArrayList insertSqlArrayList = new ArrayList();

            StringBuilder preInsertBuilder2 = new StringBuilder();
            preInsertBuilder2.Append("INSERT INTO [访客最近12月分配图表] ( [时间段], [访问量] ) VALUES (");
            ArrayList insertSqlArrayList2 = new ArrayList();

            Hashtable accessNumHashtable = new Hashtable();
            Hashtable uniqueAccessNumHashtable = new Hashtable();
            int maxAccessNum = 0;
            int uniqueMaxAccessNum = 0;

            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            for (int i = 0; i < 12; i++)
            {
                DateTime datetime = now.AddMonths(-i);
                int accessNum = 0;
                if (trackingMonthHashtable[datetime] != null)
                {
                    accessNum = (int)trackingMonthHashtable[datetime];
                }
                accessNumHashtable.Add(12 - i, accessNum);
                if (accessNum > maxAccessNum)
                {
                    maxAccessNum = accessNum;
                }

                int uniqueAccessNum = 0;
                if (uniqueTrackingMonthHashtable[datetime] != null)
                {
                    uniqueAccessNum = (int)uniqueTrackingMonthHashtable[datetime];
                }
                uniqueAccessNumHashtable.Add(12 - i, uniqueAccessNum);
                if (uniqueAccessNum > uniqueMaxAccessNum)
                {
                    uniqueMaxAccessNum = uniqueAccessNum;
                }

                insertSqlArrayList.Add(string.Format(preInsertBuilder.ToString() + " {0}, {1} )", datetime.Month, accessNum));
                insertSqlArrayList2.Add(string.Format(preInsertBuilder2.ToString() + " {0}, {1} )", datetime.Month, uniqueAccessNum));
            }

            OleDbConnection oConn = new OleDbConnection();
            oConn.ConnectionString = OLEDBConnStr;
            oConn.Open();

            OleDbCommand createComm1 = new OleDbCommand();
            createComm1.Connection = oConn;
            createComm1.CommandText = createSqlString1;
            createComm1.ExecuteNonQuery();

            OleDbCommand createComm2 = new OleDbCommand();
            createComm2.Connection = oConn;
            createComm2.CommandText = createSqlString2;
            createComm2.ExecuteNonQuery();

            foreach (string insertSql1 in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql1;
                oInsertComm.ExecuteNonQuery();
            }
            foreach (string insertSql2 in insertSqlArrayList2)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql2;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();
        }

        public static void CreateExcelFileForTrackingYears(string filePath, int publishmentSystemID)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            Hashtable trackingYearHashtable = DataProvider.TrackingDAO.GetTrackingYearHashtable(publishmentSystemID);
            Hashtable uniqueTrackingYearHashtable = DataProvider.TrackingDAO.GetUniqueTrackingYearHashtable(publishmentSystemID);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";

            string createSqlString1 = "CREATE TABLE [访问量年分配图表] ( [时间段] INT, [访问量] INT )";
            string createSqlString2 = "CREATE TABLE [访客年分配图表] ( [时间段] INT, [访问量] INT )";

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.Append("INSERT INTO [访问量年分配图表] ( [时间段], [访问量] ) VALUES (");
            ArrayList insertSqlArrayList = new ArrayList();

            StringBuilder preInsertBuilder2 = new StringBuilder();
            preInsertBuilder2.Append("INSERT INTO [访客年分配图表] ( [时间段], [访问量] ) VALUES (");
            ArrayList insertSqlArrayList2 = new ArrayList();

            Hashtable accessNumHashtable = new Hashtable();
            Hashtable uniqueAccessNumHashtable = new Hashtable();
            int maxAccessNum = 0;
            int uniqueMaxAccessNum = 0;

            DateTime now = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
            for (int i = 0; i < 10; i++)
            {
                DateTime datetime = now.AddYears(-i);
                int accessNum = 0;
                if (trackingYearHashtable[datetime] != null)
                {
                    accessNum = (int)trackingYearHashtable[datetime];
                }
                accessNumHashtable.Add(10 - i, accessNum);
                if (accessNum > maxAccessNum)
                {
                    maxAccessNum = accessNum;
                }

                int uniqueAccessNum = 0;
                if (uniqueTrackingYearHashtable[datetime] != null)
                {
                    uniqueAccessNum = (int)uniqueTrackingYearHashtable[datetime];
                }
                uniqueAccessNumHashtable.Add(10 - i, uniqueAccessNum);
                if (uniqueAccessNum > uniqueMaxAccessNum)
                {
                    uniqueMaxAccessNum = uniqueAccessNum;
                }
                insertSqlArrayList.Add(string.Format(preInsertBuilder.ToString() + " {0}, {1} )", datetime.Year, accessNum));
                insertSqlArrayList2.Add(string.Format(preInsertBuilder2.ToString() + " {0}, {1} )", datetime.Year, uniqueAccessNum));
            }

            OleDbConnection oConn = new OleDbConnection();
            oConn.ConnectionString = OLEDBConnStr;
            oConn.Open();

            OleDbCommand createComm1 = new OleDbCommand();
            createComm1.Connection = oConn;
            createComm1.CommandText = createSqlString1;
            createComm1.ExecuteNonQuery();

            OleDbCommand createComm2 = new OleDbCommand();
            createComm2.Connection = oConn;
            createComm2.CommandText = createSqlString2;
            createComm2.ExecuteNonQuery();

            foreach (string insertSql1 in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql1;
                oInsertComm.ExecuteNonQuery();
            }
            foreach (string insertSql2 in insertSqlArrayList2)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql2;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();
        }

        public static void CreateExcelFileForTrackingContents(string filePath, string startDateString, string endDateString, PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID, int totalNum, bool isDelete)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string target = string.Empty;
            string upChannel = string.Empty;
            string upupChannel = string.Empty;
            if (contentID != 0)
            {
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                target = BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title);
                upChannel = NodeManager.GetNodeName(publishmentSystemInfo.PublishmentSystemID, nodeID);
                if (nodeID != publishmentSystemInfo.PublishmentSystemID)
                {
                    upupChannel = NodeManager.GetNodeName(publishmentSystemInfo.PublishmentSystemID, NodeManager.GetParentID(publishmentSystemInfo.PublishmentSystemID, nodeID));
                }
            }

            DateTime begin = DateUtils.SqlMinValue;
            if (!string.IsNullOrEmpty(startDateString))
            {
                begin = TranslateUtils.ToDateTime(startDateString);
            }
            DateTime end = TranslateUtils.ToDateTime(endDateString);

            ArrayList ipAddresses = DataProvider.TrackingDAO.GetContentIPAddressArrayList(publishmentSystemInfo.PublishmentSystemID, nodeID, contentID, begin, end);
            ArrayList trackingInfoArrayList = DataProvider.TrackingDAO.GetTrackingInfoArrayList(publishmentSystemInfo.PublishmentSystemID, nodeID, contentID, begin, end);

            SortedList cityWithNumSortedList = BaiRongDataProvider.IP2CityDAO.GetCityWithNumSortedList(ipAddresses);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";

            string createSqlString = "CREATE TABLE [访问详细] ( [目标页面] VARCHAR(200), [上级栏目] VARCHAR(200), [上上级栏目] VARCHAR(200), [IP地址] VARCHAR(50), [所属区域] VARCHAR(50), [访问时间] VARCHAR(50), [访问来源] VARCHAR(200) )";

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.Append("INSERT INTO [访问详细] ( [目标页面], [上级栏目], [上上级栏目], [IP地址], [所属区域], [访问时间], [访问来源] ) VALUES (");
            ArrayList insertSqlArrayList = new ArrayList();
            foreach (TrackingInfo trackingInfo in trackingInfoArrayList)
            {
                if (contentID == 0)
                {
                    if (trackingInfo.PageContentID != 0)
                    {
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, trackingInfo.PageNodeID);
                        target = BaiRongDataProvider.ContentDAO.GetValue(tableName, trackingInfo.PageContentID, ContentAttribute.Title);
                        upChannel = NodeManager.GetNodeName(publishmentSystemInfo.PublishmentSystemID, trackingInfo.PageNodeID);
                        if (trackingInfo.PageNodeID != publishmentSystemInfo.PublishmentSystemID)
                        {
                            upupChannel = NodeManager.GetNodeName(publishmentSystemInfo.PublishmentSystemID, NodeManager.GetParentID(publishmentSystemInfo.PublishmentSystemID, trackingInfo.PageNodeID));
                        }
                    }
                    else if (trackingInfo.PageNodeID != 0)
                    {
                        target = NodeManager.GetNodeName(publishmentSystemInfo.PublishmentSystemID, trackingInfo.PageNodeID);
                        if (trackingInfo.PageNodeID != publishmentSystemInfo.PublishmentSystemID)
                        {
                            int upChannelID = NodeManager.GetParentID(publishmentSystemInfo.PublishmentSystemID, trackingInfo.PageNodeID);
                            upChannel = NodeManager.GetNodeName(publishmentSystemInfo.PublishmentSystemID, upChannelID);
                            if (upChannelID != publishmentSystemInfo.PublishmentSystemID)
                            {
                                upupChannel = NodeManager.GetNodeName(publishmentSystemInfo.PublishmentSystemID, NodeManager.GetParentID(publishmentSystemInfo.PublishmentSystemID, upChannelID));
                            }
                        }
                    }
                }
                string ipAddress = trackingInfo.IPAddress;
                string city = BaiRongDataProvider.IP2CityDAO.GetCity(trackingInfo.IPAddress);
                string accessDate = trackingInfo.AccessDateTime.ToString(DateUtils.FormatStringDateTime);
                string referrer = trackingInfo.Referrer;
                insertSqlArrayList.Add(string.Format(preInsertBuilder.ToString() + " '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}' )", target, upChannel, upupChannel, ipAddress, city, accessDate, referrer));
            }

            OleDbConnection oConn = new OleDbConnection();
            oConn.ConnectionString = OLEDBConnStr;
            oConn.Open();

            OleDbCommand createComm = new OleDbCommand();
            createComm.Connection = oConn;
            createComm.CommandText = createSqlString;
            createComm.ExecuteNonQuery();

            foreach (string insertSql1 in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql1;
                oInsertComm.ExecuteNonQuery();
            }

            oConn.Close();

            if (isDelete)
            {
                DataProvider.TrackingDAO.DeleteAll(publishmentSystemInfo.PublishmentSystemID);
            }
        }

        //public static void CreateExcelFileForTrackingContents(string filePath, string startDateString, string endDateString, int publishmentSystemID, int nodeID, int contentID, int totalNum)
        //{
        //    DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
        //    FileUtils.DeleteFileIfExists(filePath);

        //    string target = NodeManager.GetNodeNameNavigation(publishmentSystemID, nodeID);
        //    if (contentID != 0)
        //    {
        //        target += " - " + DataProvider.BackgroundContentDAO.GetValue(publishmentSystemID, contentID, BackgroundContentAttribute.Title);
        //    }

        //    DateTime begin = DateUtils.SqlMinValue;
        //    if (!string.IsNullOrEmpty(startDateString))
        //    {
        //        begin = TranslateUtils.ToDateTime(startDateString);
        //    }
        //    DateTime end = TranslateUtils.ToDateTime(endDateString);

        //    ArrayList ipAddresses = DataProvider.TrackingDAO.GetContentIPAddressArrayList(publishmentSystemID, nodeID, contentID, begin, end);
        //    ArrayList trackingInfoArrayList = DataProvider.TrackingDAO.GetTrackingInfoArrayList(publishmentSystemID, nodeID, contentID, begin, end);

        //    SortedList cityWithNumSortedList = BaiRongDataProvider.IP2CityDAO.GetCityWithNumSortedList(ipAddresses);

        //    string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
        //    OLEDBConnStr += " Extended Properties=Excel 8.0;";

        //    string createSqlString1 = "CREATE TABLE [访问目标] ( [目标] VARCHAR(200), [开始时间] VARCHAR(50), [结束时间] VARCHAR(50), [总访问量] INT )";
        //    string createSqlString2 = "CREATE TABLE [访问量区域] ( [区域] VARCHAR(50), [访问量] INT )";
        //    string createSqlString3 = "CREATE TABLE [详细访问IP] ( [IP地址] VARCHAR(50), [所属区域] VARCHAR(50), [访问时间] VARCHAR(50), [访问目标] VARCHAR(200), [访问来源] VARCHAR(200) )";

        //    StringBuilder insertBuilder = new StringBuilder();
        //    insertBuilder.AppendFormat("INSERT INTO [访问目标] ( [目标], [开始时间], [结束时间], [总访问量] ) VALUES ( '{0}', '{1}', '{2}', {3} )", target, startDateString, endDateString, totalNum);

        //    StringBuilder preInsertBuilder1 = new StringBuilder();
        //    preInsertBuilder1.Append("INSERT INTO [访问量区域] ( [区域], [访问量] ) VALUES (");
        //    ArrayList insertSqlArrayList1 = new ArrayList();
        //    foreach (string city in cityWithNumSortedList.Keys)
        //    {
        //        insertSqlArrayList1.Add(string.Format(preInsertBuilder1.ToString() + " '{0}', {1} )", city, cityWithNumSortedList[city]));
        //    }

        //    StringBuilder preInsertBuilder2 = new StringBuilder();
        //    preInsertBuilder2.Append("INSERT INTO [详细访问IP] ( [IP地址], [所属区域], [访问时间], [访问目标], [访问来源] ) VALUES (");
        //    ArrayList insertSqlArrayList2 = new ArrayList();
        //    foreach (TrackingInfo trackingInfo in trackingInfoArrayList)
        //    {
        //        string theTarget = target;
        //        if (contentID == 0 && trackingInfo.PageContentID != 0)
        //        {
        //            theTarget += " - " + DataProvider.BackgroundContentDAO.GetValue(publishmentSystemID, trackingInfo.PageContentID, BackgroundContentAttribute.Title);
        //        }
        //        string city = BaiRongDataProvider.IP2CityDAO.GetCity(trackingInfo.IPAddress);
        //        insertSqlArrayList2.Add(string.Format(preInsertBuilder2.ToString() + " '{0}', '{1}', '{2}', '{3}', '{4}' )", trackingInfo.IPAddress, city, trackingInfo.AccessDateTime.ToString(DateUtils.FormatStringDateTime), theTarget, trackingInfo.Referrer));
        //    }

        //    OleDbConnection oConn = new OleDbConnection();
        //    oConn.ConnectionString = OLEDBConnStr;
        //    oConn.Open();

        //    OleDbCommand createComm1 = new OleDbCommand();
        //    createComm1.Connection = oConn;
        //    createComm1.CommandText = createSqlString1;
        //    createComm1.ExecuteNonQuery();

        //    OleDbCommand createComm2 = new OleDbCommand();
        //    createComm2.Connection = oConn;
        //    createComm2.CommandText = createSqlString2;
        //    createComm2.ExecuteNonQuery();

        //    OleDbCommand createComm3 = new OleDbCommand();
        //    createComm3.Connection = oConn;
        //    createComm3.CommandText = createSqlString3;
        //    createComm3.ExecuteNonQuery();

        //    OleDbCommand oInsertComm1 = new OleDbCommand();
        //    oInsertComm1.Connection = oConn;
        //    oInsertComm1.CommandText = insertBuilder.ToString();
        //    oInsertComm1.ExecuteNonQuery();

        //    foreach (string insertSql1 in insertSqlArrayList1)
        //    {
        //        OleDbCommand oInsertComm = new OleDbCommand();
        //        oInsertComm.Connection = oConn;
        //        oInsertComm.CommandText = insertSql1;
        //        oInsertComm.ExecuteNonQuery();
        //    }

        //    foreach (string insertSql2 in insertSqlArrayList2)
        //    {
        //        OleDbCommand oInsertComm = new OleDbCommand();
        //        oInsertComm.Connection = oConn;
        //        oInsertComm.CommandText = insertSql2;
        //        oInsertComm.ExecuteNonQuery();
        //    }

        //    oConn.Close();
        //}


        public static ArrayList GetContentsByExcelFile(string filePath, PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            ArrayList contentInfoArrayList = new ArrayList();

            ArrayList al = new ArrayList();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;";
            OleDbConnection oleDbConn = new OleDbConnection(strConn);
            OleDbCommand oleDbCmd;
            OleDbDataAdapter oleDbAdp;
            DataTable oleDt = new DataTable();
            DataTable dtTableName = new DataTable();

            oleDbConn.Open();
            dtTableName = oleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string[] tableNames = new string[dtTableName.Rows.Count];
            for (int j = 0; j < dtTableName.Rows.Count; j++)
            {
                tableNames[j] = dtTableName.Rows[j]["TABLE_NAME"].ToString();
            }

            foreach (string tableName in tableNames)
            {
                if (!tableName.EndsWith("$"))
                    continue;

                try
                {
                    oleDbCmd = new OleDbCommand("SELECT * FROM [" + tableName + "]", oleDbConn);
                    oleDbAdp = new OleDbDataAdapter(oleDbCmd);
                    oleDbAdp.Fill(oleDt);
                    oleDbConn.Close();

                    if (oleDt.Rows.Count > 0)
                    {
                        ArrayList relatedidentityes = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID);
                        ContentModelInfo modelInfo = ContentModelManager.GetContentModelInfo(publishmentSystemInfo, nodeInfo.ContentModelID);
                        ETableStyle tableStyle = EAuxiliaryTableTypeUtils.GetTableStyle(modelInfo.TableType);
                        // ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.BackgroundContent, publishmentSystemInfo.AuxiliaryTableForContent, relatedidentityes);

                        ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, modelInfo.TableName, relatedidentityes);
                        tableStyleInfoArrayList = ContentUtility.GetAllTableStyleInfoArrayList(publishmentSystemInfo, tableStyle, tableStyleInfoArrayList);
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
                catch { }
            }

            return contentInfoArrayList;
        }

        public static ArrayList GetInputContentsByExcelFile(string filePath, PublishmentSystemInfo publishmentSystemInfo, InputInfo inputInfo)
        {
            ArrayList contentInfoArrayList = new ArrayList();

            ArrayList al = new ArrayList();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;";
            OleDbConnection oleDbConn = new OleDbConnection(strConn);
            OleDbCommand oleDbCmd;
            OleDbDataAdapter oleDbAdp;
            DataTable oleDt = new DataTable();
            DataTable dtTableName = new DataTable();

            oleDbConn.Open();
            dtTableName = oleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string[] tableNames = new string[dtTableName.Rows.Count];
            for (int j = 0; j < dtTableName.Rows.Count; j++)
            {
                tableNames[j] = dtTableName.Rows[j]["TABLE_NAME"].ToString();
            }
            oleDbCmd = new OleDbCommand("SELECT * FROM [" + tableNames[0] + "]", oleDbConn);
            oleDbAdp = new OleDbDataAdapter(oleDbCmd);
            oleDbAdp.Fill(oleDt);
            oleDbConn.Close();

            if (oleDt.Rows.Count > 0)
            {
                ArrayList relatedidentityes = RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, publishmentSystemInfo.PublishmentSystemID, inputInfo.InputID);
                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, relatedidentityes);

                NameValueCollection nameValueCollection = new NameValueCollection();

                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    nameValueCollection[styleInfo.DisplayName] = styleInfo.AttributeName.ToLower();
                }

                nameValueCollection["回复"] = InputContentAttribute.Reply.ToLower();
                nameValueCollection["添加时间"] = InputContentAttribute.AddDate.ToLower();

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
                    InputContentInfo contentInfo = new InputContentInfo(0, inputInfo.InputID, 0, true, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty);

                    for (int i = 0; i < oleDt.Columns.Count; i++)
                    {
                        string attributeName = attributeNames[i] as string;
                        if (!string.IsNullOrEmpty(attributeName))
                        {
                            string value = row[i].ToString();
                            contentInfo.SetExtendedAttribute(attributeName, value);
                        }
                    }

                    contentInfoArrayList.Add(contentInfo);
                }
            }

            contentInfoArrayList.Reverse();

            return contentInfoArrayList;
        }
    }
}
