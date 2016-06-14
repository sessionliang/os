using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.JWT;
using BaiRong.Model;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace SiteServer.API.Core
{

    /// <summary>
    /// 与内容有关的函数
    /// </summary>
    public class ContentUtils
    {

        #region 权限
        /// <summary>
        /// 是否审核通过
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="nodeID"></param>
        /// <param name="userCheckedLevel"></param>
        /// <returns></returns>
        public static bool ContentIsChecked(string userName, PublishmentSystemInfo publishmentSystemInfo, int nodeID, out int userCheckedLevel)
        {
            int checkContentLevel = publishmentSystemInfo.CheckContentLevel;

            object[] pair = GetUserCheckLevel(userName, publishmentSystemInfo, nodeID);
            bool isChecked = (bool)pair[0];
            int checkedLevel = (int)pair[1];
            if (isChecked)
            {
                checkedLevel = checkContentLevel;
            }
            userCheckedLevel = checkedLevel;
            return isChecked;
        }

        public static object[] GetUserCheckLevel(string userName, PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            int publishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
            if (AdminManager.HasChannelPermissionIsConsoleAdministrator(userName) || AdminManager.HasChannelPermissionIsSystemAdministrator(userName))//如果是超级管理员或站点管理员
            {
                return new object[] { true, publishmentSystemInfo.CheckContentLevel };
            }

            bool isChecked = false;
            int checkedLevel = 0;
            if (publishmentSystemInfo.IsCheckContentUseLevel == false)
            {
                if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheck))
                {
                    isChecked = true;
                }
                else
                {
                    isChecked = false;
                }
            }
            else
            {
                if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel5))
                {
                    isChecked = true;
                }
                else if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel4))
                {
                    if (publishmentSystemInfo.CheckContentLevel <= 4)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                        checkedLevel = 4;
                    }
                }
                else if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel3))
                {
                    if (publishmentSystemInfo.CheckContentLevel <= 3)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                        checkedLevel = 3;
                    }
                }
                else if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel2))
                {
                    if (publishmentSystemInfo.CheckContentLevel <= 2)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                        checkedLevel = 2;
                    }
                }
                else if (HasChannelPermissions(userName, publishmentSystemID, nodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel1))
                {
                    if (publishmentSystemInfo.CheckContentLevel <= 1)
                    {
                        isChecked = true;
                    }
                    else
                    {
                        isChecked = false;
                        checkedLevel = 1;
                    }
                }
                else
                {
                    isChecked = false;
                    checkedLevel = 0;
                }
            }
            return new object[] { isChecked, checkedLevel };
        }


        /// <summary>
        /// 是否有审核权限
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="channelPermissionArray"></param>
        /// <returns></returns>
        public static bool HasChannelPermissions(string userName, int publishmentSystemID, int nodeID, string channelPermission)
        {
            if (string.IsNullOrEmpty(userName)) return false;
            if (publishmentSystemID == 0) return false;
            if (nodeID == 0) return false;

            bool returnval = false;
            string[] roles = RoleManager.GetRolesForUser(userName);
            ProductAdministratorWithPermissions ps = new ProductAdministratorWithPermissions(userName, true);
            foreach (int itemForPSID in ps.WebsitePermissionSortedList.Keys)
            {
                ArrayList nodeIDCollections = DataProvider.SystemPermissionsDAO.GetAllPermissionArrayList(roles, itemForPSID, true);
                foreach (SystemPermissionsInfo info in nodeIDCollections)
                {
                    ArrayList nodeIDCollection = TranslateUtils.StringCollectionToArrayList(info.NodeIDCollection);
                    ArrayList channelPermissions = TranslateUtils.StringCollectionToArrayList(info.ChannelPermissions);
                    if (nodeIDCollection.Contains(nodeID.ToString()) && channelPermissions.Contains(channelPermission))
                    {
                        returnval = true;
                    }
                }
            }
            return returnval;
        }

        #endregion

        public static void CreateExcel(ArrayList mLibScopeInfoArrayList, string httpHost, string docFileName)
        {

            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";
            StringBuilder createBuilder = new StringBuilder();

            createBuilder.Append("CREATE TABLE tb_mlib ( ");

            createBuilder.AppendFormat(" [{0}] NTEXT, ", "站点栏目");
            createBuilder.AppendFormat(" [{0}] NTEXT, ", "标题");
            createBuilder.AppendFormat(" [{0}] NTEXT, ", "链接");
            createBuilder.Length = createBuilder.Length - 2;
            createBuilder.Append(" )");

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.Append("INSERT INTO tb_mlib (");

            preInsertBuilder.AppendFormat("[{0}], ", "站点栏目");
            preInsertBuilder.AppendFormat("[{0}], ", "标题");
            preInsertBuilder.AppendFormat("[{0}], ", "链接");
            preInsertBuilder.Length = preInsertBuilder.Length - 2;
            preInsertBuilder.Append(" ) VALUES (");


            ArrayList insertSqlArrayList = new ArrayList();

            foreach (PublishmentSystemInfo pinfo in mLibScopeInfoArrayList)
            {
                if (pinfo == null)
                    break;
                string tableName = pinfo.AuxiliaryTableForContent;
                DataSet ds = new DataSet();
                try
                {
                    ds = DataProvider.ContentDAO.GetDateSet(tableName, pinfo.PublishmentSystemID, string.Empty, string.Empty, string.Empty, true, true, UserManager.Current.UserName);


                    foreach (DataTable dt in ds.Tables)
                    {
                        //定义表对象与行对象，同时用DataSet对其值进行初始化  
                        DataRow[] myRow = dt.Select();//可以类似dt.Select("id>10")之形式达到数据筛选目的 
                        int cl = dt.Columns.Count;

                        //向HTTP输出流中写入取得的数据信息 

                        //逐行处理数据   
                        foreach (DataRow row in myRow)
                        {
                            if (pinfo == null)
                                break;
                            string nodeName = NodeManager.GetNodeNameNavigation(TranslateUtils.ToInt(row[0].ToString()), TranslateUtils.ToInt(row[1].ToString()));

                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(ETableStyle.BackgroundContent,
                           tableName, TranslateUtils.ToInt(row[3].ToString()));


                            StringBuilder insertBuilder = new StringBuilder();
                            insertBuilder.Append(preInsertBuilder.ToString());

                            insertBuilder.AppendFormat("'{0}', ", pinfo.PublishmentSystemName + " > " + nodeName);
                            insertBuilder.AppendFormat("'{0}', ", new System.Web.UI.Page().Server.HtmlDecode(row[2].ToString()));
                            insertBuilder.AppendFormat("'{0}', ", PageUtility.GetContentUrl(pinfo, contentInfo, true, pinfo.Additional.VisualType));

                            insertBuilder.Length = insertBuilder.Length - 2;
                            insertBuilder.Append(") ");

                            insertSqlArrayList.Add(insertBuilder.ToString());

                        }
                    }
                }
                catch
                {

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


        #region 显示字段
        /// <summary>
        /// 获取投稿默认字段
        /// </summary>
        private static string mLibDraftContentAttributeNames;
        public static string MLibDraftContentAttributeNames(string tableName)
        {
            ArrayList arraylist = BaiRongDataProvider.AuxiliaryTableDataDAO.GetDefaultTableMetadataInfoArrayList(tableName, EAuxiliaryTableType.ManuscriptContent);

            ArrayList attributeNames = new ArrayList();
            foreach (TableMetadataInfo metadataInfo in arraylist)
            {
                attributeNames.Add(metadataInfo.AttributeName);
            }
            mLibDraftContentAttributeNames = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(attributeNames);

            return mLibDraftContentAttributeNames;
        }


        public static string GetMLibFileds(NameValueCollection formCollection, PublishmentSystemInfo publishmentSystemInfo, int nodeID, ArrayList relatedIdentities, ETableStyle tableStyle, string tableName, bool isEdit, bool isPostBack, string hasFiles)
        {
            ArrayList excludeAttributeNames = new ArrayList();

            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(tableName))
            {
                if (formCollection == null)
                {
                    if (HttpContext.Current.Request.Form != null && HttpContext.Current.Request.Form.Count > 0)
                    {
                        formCollection = HttpContext.Current.Request.Form;
                    }
                    else
                    {
                        formCollection = new NameValueCollection();
                    }
                }

                ArrayList styleInfoArrayListOld = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);

                ArrayList styleInfoArrayList = new ArrayList();
                if (string.IsNullOrEmpty(hasFiles))
                {
                    styleInfoArrayList = styleInfoArrayListOld;
                }
                foreach (TableStyleInfo styleInfo in styleInfoArrayListOld)
                {
                    ArrayList files = TranslateUtils.StringCollectionToArrayList(hasFiles);
                    foreach (string flesName in files)
                    {
                        if (flesName == styleInfo.AttributeName)
                            styleInfoArrayList.Add(styleInfo);
                    }
                }


                NameValueCollection pageScripts = new NameValueCollection();

                if (styleInfoArrayList != null)
                {
                    bool isPreviousSingleLine = true;
                    bool isPreviousLeftColumn = false;
                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.Title))
                        {
                            styleInfo.Additional.IsFormatString = false;
                        }
                        else if (StringUtils.EqualsIgnoreCase(styleInfo.AttributeName, ContentAttribute.AddDate))
                        {
                            styleInfo.IsVisible = false;
                        }
                        if (styleInfo.IsVisible && !excludeAttributeNames.Contains(styleInfo.AttributeName.ToLower()))
                        {
                            string text = StringUtility.GetHelpHtml(styleInfo.DisplayName, styleInfo.HelpText);
                            string value = InputTypeParser.Parse(publishmentSystemInfo, nodeID, styleInfo, tableStyle, styleInfo.AttributeName, formCollection, isEdit, isPostBack, null, pageScripts, false, true);

                            if (builder.Length > 0)
                            {
                                if (isPreviousSingleLine)
                                {
                                    builder.Append("</div>");
                                }
                                else
                                {
                                    if (!isPreviousLeftColumn)
                                    {
                                        builder.Append("</div>");
                                    }
                                    else if (styleInfo.IsSingleLine)
                                    {
                                        builder.Append(@"</div>");
                                    }
                                }
                            }

                            //this line

                            if (styleInfo.IsSingleLine || isPreviousSingleLine || !isPreviousLeftColumn)
                            {
                                builder.Append(@"<div class=""form-row"">");
                            }

                            if (styleInfo.InputType == EInputType.TextEditor)
                            {
                                string commands = WebUtils.GetTextEditorCommands(publishmentSystemInfo, ETextEditorTypeUtils.GetEnumType(styleInfo.Additional.EditorTypeString), styleInfo.AttributeName);
                                //builder.AppendFormat(@"<label for=""title"" class=""form-field""></label><div class=""form-cont"">{0}</div>", commands);
                                builder.AppendFormat(@"<label for=""title"" class=""form-field"">{0}</label><div class=""form-cont"" style=""width:85%"">{1}<br /><br />{2}</div>", text, commands, value);
                                //builder.AppendFormat(@"<td colspan=""4"" align=""left"">{0}</td></tr><tr><td colspan=""4"" align=""left"">{1}</td>", text, value);
                                //自动检测敏感词
                                builder.AppendFormat(@"{0}", WebUtils.GetAutoCheckKeywordsCommands(publishmentSystemInfo, ETextEditorTypeUtils.GetEnumType(styleInfo.Additional.EditorTypeString), styleInfo.AttributeName));
                            }
                            else
                            {
                                #region by 20160215 sofuny 标题和副标题增加敏感词监测
                                if (styleInfo.AttributeName == "Title" || styleInfo.AttributeName == "SubTitle")
                                {
                                    builder.AppendFormat(@"<label for=""title"" class=""form-field"">{0}</label><div class=""form-cont"">{1}</div>", text, value);
                                    //builder.AppendFormat(@"<td>{0}</td><td {1}>{2}</td>", text, styleInfo.IsSingleLine ? @"colspan=""3""" : string.Empty, value);
                                    //自动检测敏感词
                                    builder.AppendFormat(@"{0}", WebUtils.GetAutoCheckKeywordsCommandsByInput(publishmentSystemInfo, styleInfo.AttributeName));
                                }
                                #endregion
                                else
                                    builder.AppendFormat(@"<label for=""title"" class=""form-field"">{0}</label><div class=""form-cont"">{1}</div>", text, value);
                                //builder.AppendFormat(@"<td align=""left"">{0}</td><td {1} align=""left"">{2}</td>", text, styleInfo.IsSingleLine ? @"colspan=""3""" : string.Empty, value);
                            }

                            if (styleInfo.IsSingleLine)
                            {
                                isPreviousSingleLine = true;
                                isPreviousLeftColumn = false;
                            }
                            else
                            {
                                isPreviousSingleLine = false;
                                isPreviousLeftColumn = !isPreviousLeftColumn;
                            }
                        }
                    }

                    if (builder.Length > 0)
                    {
                        if (isPreviousSingleLine || !isPreviousLeftColumn)
                        {
                            builder.Append("</div>");
                        }
                        else
                        {
                            //builder.Append(@"<td></td><td></td></tr>");
                            builder.Append("</div>");
                        }
                    }


                    foreach (string key in pageScripts.Keys)
                    {
                        builder.Append(pageScripts[key]);
                    }
                }
            }
            return builder.ToString();
        }

        #endregion


        #region 内容功能字段

        public static ArrayList GetFunctionFileds(int publishmentSystemID, int nodeID, int contentID, ETableStyle tableStyle, string tableName)
        {

            ArrayList ftsList = DataProvider.FunctionTableStylesDAO.GetInfoList(publishmentSystemID, nodeID, contentID, tableStyle.ToString(), "files");

            ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(tableStyle, publishmentSystemID, nodeID);

            ArrayList tableStyleList = BaiRongDataProvider.TableStyleDAO.GetFunctionTableStyle(tableName, nodeID, publishmentSystemID, contentID, tableStyle.ToString());

            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);


            ArrayList tslist = new ArrayList();
            if (ftsList.Count > 0)
            {
                foreach (TableStyleInfo info in tableStyleList)
                {
                    if (ftsList.Contains(info.TableStyleID))
                    {
                        tslist.Add(info);
                    }
                }
            }
            else
            {
                foreach (TableStyleInfo info in tableStyleInfoArrayList)
                {
                    if (info.IsVisible)
                    {
                        tslist.Add(info);
                    }
                }
            }
            ArrayList list = new ArrayList();
            foreach (TableStyleInfo info in tslist)
            {

                var tsinfo = new
                {
                    TableStyleID = info.TableStyleID,
                    AttributeName = info.AttributeName,
                    DefaultValue = info.DefaultValue,
                    DisplayName = info.DisplayName,
                    ExtendValues = info.ExtendValues,
                    HelpText = info.HelpText,
                    InputType = info.InputType.ToString(),
                    IsHorizontal = info.IsHorizontal,
                    IsSingleLine = info.IsSingleLine,
                    IsVisible = info.IsVisible,
                    IsVisibleInList = info.IsVisibleInList,
                    TableName = info.TableName,
                    Taxis = info.Taxis,
                    Additional = info.Additional,
                    StyleItems = info.StyleItems
                };
                list.Add(tsinfo);
            }
            return list;
        }
        #endregion

    }
}