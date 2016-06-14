using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using System.Collections.Generic;

namespace BaiRong.Provider.Data.SqlServer
{
    public class ContentDAO : DataProviderBase, IContentDAO
    {
        public const int TAXIS_MAX_VALUE = 2147483647;
        public const int TAXIS_ISTOP_START_VALUE = 2147480000;

        public int Insert(string tableName, ContentInfo contentInfo)
        {
            int contentID = 0;

            if (!string.IsNullOrEmpty(tableName))
            {
                IDbDataParameter[] parms = null;

                contentInfo.IsTop = contentInfo.IsTop;

                string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(contentInfo.Attributes, tableName, out parms);

                using (IDbConnection conn = this.GetConnection())
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                            contentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, tableName);

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }

            return contentID;
        }

        public void Update(string tableName, ContentInfo contentInfo)
        {
            IDbDataParameter[] parms = null;
            string sqlString = string.Empty;

            contentInfo.IsTop = contentInfo.IsTop;

            //出现IsTop与Taxis不同步情况
            if (contentInfo.IsTop == false && contentInfo.Taxis >= ContentDAO.TAXIS_ISTOP_START_VALUE)
            {
                contentInfo.Taxis = BaiRongDataProvider.ContentDAO.GetMaxTaxis(tableName, contentInfo.NodeID, false) + 1;
            }
            else if (contentInfo.IsTop && contentInfo.Taxis < ContentDAO.TAXIS_ISTOP_START_VALUE)
            {
                contentInfo.Taxis = BaiRongDataProvider.ContentDAO.GetMaxTaxis(tableName, contentInfo.NodeID, true) + 1;
            }
            contentInfo.LastEditDate = DateTime.Now;
            if (!string.IsNullOrEmpty(tableName))
            {
                contentInfo.BeforeExecuteNonQuery();
                sqlString = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(contentInfo.Attributes, tableName, out parms);
            }

            if (!string.IsNullOrEmpty(sqlString))
            {
                this.ExecuteNonQuery(sqlString, parms);
            }
        }

        public bool UpdateTaxisToUp(string tableName, int nodeID, int contentID, bool isTop)
        {
            //Get Higher Taxis and ID
            string sqlString;
            if (isTop)
            {
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE (Taxis > (SELECT Taxis FROM {0} WHERE ID = {1}) AND Taxis >= {2} AND NodeID = {3}) ORDER BY Taxis", tableName, contentID, ContentDAO.TAXIS_ISTOP_START_VALUE, nodeID);
            }
            else
            {
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE (Taxis > (SELECT Taxis FROM {0} WHERE ID = {1}) AND Taxis < {2} AND NodeID = {3}) ORDER BY Taxis", tableName, contentID, ContentDAO.TAXIS_ISTOP_START_VALUE, nodeID);
            }
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            if (higherID != 0)
            {
                //Get Taxis Of Selected ID
                int selectedTaxis = this.GetTaxis(contentID, tableName);

                //Set The Selected Class Taxis To Higher Level
                this.SetTaxis(contentID, higherTaxis, tableName);
                //Set The Higher Class Taxis To Lower Level
                this.SetTaxis(higherID, selectedTaxis, tableName);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(string tableName, int nodeID, int contentID, bool isTop)
        {
            //Get Lower Taxis and ID
            string sqlString;
            if (isTop)
            {
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE (Taxis < (SELECT Taxis FROM {0} WHERE ID = {1}) AND Taxis >= {2} AND NodeID = {3}) ORDER BY Taxis DESC", tableName, contentID, ContentDAO.TAXIS_ISTOP_START_VALUE, nodeID);
            }
            else
            {
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE (Taxis < (SELECT Taxis FROM {0} WHERE ID = {1}) AND Taxis < {2} AND NodeID = {3}) ORDER BY Taxis DESC", tableName, contentID, ContentDAO.TAXIS_ISTOP_START_VALUE, nodeID);
            }
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            if (lowerID != 0)
            {
                //Get Taxis Of Selected Class
                int selectedTaxis = this.GetTaxis(contentID, tableName);

                //Set The Selected Class Taxis To Lower Level
                this.SetTaxis(contentID, lowerTaxis, tableName);
                //Set The Lower Class Taxis To Higher Level
                this.SetTaxis(lowerID, selectedTaxis, tableName);
                return true;
            }
            return false;
        }

        public int GetMaxTaxis(string tableName, int nodeID, bool isTop)
        {
            int maxTaxis = 0;
            if (isTop)
            {
                maxTaxis = ContentDAO.TAXIS_ISTOP_START_VALUE;

                string sqlString = string.Format("SELECT MAX(Taxis) FROM {0} WHERE NodeID = {1} AND Taxis >= {2}", tableName, nodeID, ContentDAO.TAXIS_ISTOP_START_VALUE);

                using (IDbConnection conn = this.GetConnection())
                {
                    conn.Open();
                    using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                    {
                        if (rdr.Read())
                        {
                            if (!rdr.IsDBNull(0))
                            {
                                maxTaxis = Convert.ToInt32(rdr[0]);
                            }
                        }
                        rdr.Close();
                    }
                }
                if (maxTaxis == ContentDAO.TAXIS_MAX_VALUE)
                {
                    maxTaxis = ContentDAO.TAXIS_MAX_VALUE - 1;
                }
            }
            else
            {
                string sqlString = string.Format("SELECT MAX(Taxis) FROM {0} WHERE NodeID = {1} AND Taxis < {2}", tableName, nodeID, ContentDAO.TAXIS_ISTOP_START_VALUE);
                using (IDbConnection conn = this.GetConnection())
                {
                    conn.Open();
                    using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                    {
                        if (rdr.Read())
                        {
                            if (!rdr.IsDBNull(0))
                            {
                                maxTaxis = Convert.ToInt32(rdr[0]);
                            }
                        }
                        rdr.Close();
                    }
                }
            }
            return maxTaxis;
        }

        public int GetTaxis(int selectedID, string tableName)
        {
            string sqlString = string.Format("SELECT Taxis FROM {0} WHERE (ID = {1})", tableName, selectedID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public void SetTaxis(int id, int taxis, string tableName)
        {
            string sqlString = string.Format("UPDATE {0} SET Taxis = {1} WHERE ID = {2}", tableName, taxis, id);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateIsChecked(string tableName, int publishmentSystemID, int nodeID, ArrayList contentIDArrayList, int translateNodeID, bool isAdmin, string userName, bool isChecked, int checkedLevel, string reasons)
        {
            if (isChecked)
            {
                checkedLevel = 0;
            }

            //string sqlString = string.Format("UPDATE {0} SET IsChecked = '{1}', CheckedLevel = {2} WHERE ID IN ({3})", tableName, isChecked.ToString(), checkedLevel, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
            //this.ExecuteNonQuery(sqlString);

            DateTime checkDate = DateTime.Now;

            foreach (int contentID in contentIDArrayList)
            {
                string settingsXML = BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.SettingsXML);
                NameValueCollection attributes = TranslateUtils.ToNameValueCollection(settingsXML);
                attributes[ContentAttribute.Check_IsAdmin] = isAdmin.ToString();
                attributes[ContentAttribute.Check_UserName] = userName;
                attributes[ContentAttribute.Check_CheckDate] = checkDate.ToString();
                attributes[ContentAttribute.Check_Reasons] = reasons;

                string sqlString = string.Format("UPDATE {0} SET IsChecked = '{1}', CheckedLevel = {2}, SettingsXML = '{3}' WHERE ID = {4}", tableName, isChecked.ToString(), checkedLevel, TranslateUtils.NameValueCollectionToString(attributes), contentID);
                if (translateNodeID > 0)
                {
                    sqlString = string.Format("UPDATE {0} SET IsChecked = '{1}', CheckedLevel = {2}, SettingsXML = '{3}', NodeID = {4} WHERE ID = {5}", tableName, isChecked.ToString(), checkedLevel, TranslateUtils.NameValueCollectionToString(attributes), translateNodeID, contentID);
                }
                this.ExecuteNonQuery(sqlString);

                ContentCheckInfo checkInfo = new ContentCheckInfo(0, tableName, publishmentSystemID, nodeID, contentID, isAdmin, userName, isChecked, checkedLevel, checkDate, reasons);
                BaiRongDataProvider.ContentCheckDAO.Insert(checkInfo);
            }
        }

        public void UpdateIsChecked(string tableName, int publishmentSystemID, int nodeID, ArrayList contentIDArrayList, int translateNodeID, bool isAdmin, string userName, bool isChecked, int checkedLevel, string reasons, bool isCheck)
        {
            if (isChecked)
            {
                checkedLevel = 0;
            }

            //string sqlString = string.Format("UPDATE {0} SET IsChecked = '{1}', CheckedLevel = {2} WHERE ID IN ({3})", tableName, isChecked.ToString(), checkedLevel, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
            //this.ExecuteNonQuery(sqlString);

            DateTime checkDate = DateTime.Now;

            foreach (int contentID in contentIDArrayList)
            {
                string settingsXML = BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.SettingsXML);
                NameValueCollection attributes = TranslateUtils.ToNameValueCollection(settingsXML);
                attributes[ContentAttribute.Check_IsAdmin] = isAdmin.ToString();
                attributes[ContentAttribute.Check_UserName] = userName;
                attributes[ContentAttribute.Check_CheckDate] = checkDate.ToString();
                attributes[ContentAttribute.Check_Reasons] = reasons;
                //把只执行一次的任务设置为false
                if (isCheck)
                {
                    attributes[ContentAttribute.Check_IsTask] = "False";
                    attributes[ContentAttribute.CheckTaskDate] = "";
                }
                else
                {
                    attributes[ContentAttribute.UnCheck_IsTask] = "False";
                    attributes[ContentAttribute.UnCheckTaskDate] = "";
                }

                string sqlString = string.Format("UPDATE {0} SET IsChecked = '{1}', CheckedLevel = {2}, SettingsXML = '{3}', {5} WHERE ID = {4}", tableName, isChecked.ToString(), checkedLevel, TranslateUtils.NameValueCollectionToString(attributes), contentID, isCheck ? " CheckTaskDate = NULL " : " UnCheckTaskDate = NULL ");
                if (translateNodeID > 0)
                {
                    sqlString = string.Format("UPDATE {0} SET IsChecked = '{1}', CheckedLevel = {2}, SettingsXML = '{3}', NodeID = {4}, {6} WHERE ID = {5}", tableName, isChecked.ToString(), checkedLevel, TranslateUtils.NameValueCollectionToString(attributes), translateNodeID, contentID, isCheck ? " CheckTaskDate = NULL " : " UnCheckTaskDate = NULL ");
                }
                this.ExecuteNonQuery(sqlString);

                ContentCheckInfo checkInfo = new ContentCheckInfo(0, tableName, publishmentSystemID, nodeID, contentID, isAdmin, userName, isChecked, checkedLevel, checkDate, reasons);
                BaiRongDataProvider.ContentCheckDAO.Insert(checkInfo);

                //删除只执行一次的任务
                int taskId = isCheck ? TranslateUtils.ToInt(attributes[ContentAttribute.Check_TaskID]) : TranslateUtils.ToInt(attributes[ContentAttribute.UnCheck_TaskID]);
                if (taskId > 0)
                    BaiRongDataProvider.TaskDAO.Delete(taskId);
            }
        }

        public void AddHits(string tableName, bool isCountHits, bool isCountHitsByDay, int contentID)
        {
            if (contentID > 0)
            {
                if (isCountHits)
                {
                    if (isCountHitsByDay)
                    {
                        int referenceID = 0;
                        int hitsByDay = 0;
                        int hitsByWeek = 0;
                        int hitsByMonth = 0;
                        DateTime lastHitsDate = DateTime.Now;

                        string sqlString = string.Format("SELECT ReferenceID, HitsByDay, HitsByWeek, HitsByMonth, LastHitsDate FROM {0} WHERE (ID = {1})", tableName, contentID);

                        using (IDataReader rdr = this.ExecuteReader(sqlString))
                        {
                            if (rdr.Read())
                            {
                                referenceID = rdr.GetInt32(0);
                                hitsByDay = rdr.GetInt32(1);
                                hitsByWeek = rdr.GetInt32(2);
                                hitsByMonth = rdr.GetInt32(3);
                                lastHitsDate = rdr.GetDateTime(4);
                            }
                            rdr.Close();
                        }

                        if (referenceID > 0)
                        {
                            contentID = referenceID;
                        }

                        DateTime now = DateTime.Now;

                        hitsByDay = (now.Day != lastHitsDate.Day || now.Month != lastHitsDate.Month || now.Year != lastHitsDate.Year) ? 1 : hitsByDay + 1;
                        hitsByWeek = (now.Month != lastHitsDate.Month || now.Year != lastHitsDate.Year || now.DayOfYear / 7 != lastHitsDate.DayOfYear / 7) ? 1 : hitsByWeek + 1;
                        hitsByMonth = (now.Month != lastHitsDate.Month || now.Year != lastHitsDate.Year) ? 1 : hitsByMonth + 1;

                        sqlString = string.Format("UPDATE {0} SET Hits = Hits + 1, HitsByDay = {1}, HitsByWeek = {2}, HitsByMonth = {3}, LastHitsDate = getdate() WHERE ID = {4}  AND ReferenceID = 0", tableName, hitsByDay, hitsByWeek, hitsByMonth, contentID);
                        if (this.DataBaseType == EDatabaseType.Oracle)
                        {
                            sqlString = string.Format("UPDATE {0} SET Hits = Hits + 1, HitsByDay = {1}, HitsByWeek = {2}, HitsByMonth = {3}, LastHitsDate = sysdate WHERE ID = {4}  AND ReferenceID = 0", tableName, hitsByDay, hitsByWeek, hitsByMonth, contentID);
                        }
                        this.ExecuteNonQuery(sqlString);
                    }
                    else
                    {
                        string sqlString = string.Format("UPDATE {0} SET Hits = Hits + 1, LastHitsDate = getdate() WHERE ID = {1} AND ReferenceID = 0", tableName, contentID);
                        if (this.DataBaseType == EDatabaseType.Oracle)
                        {
                            sqlString = string.Format("UPDATE {0} SET Hits = Hits + 1, LastHitsDate = sysdate WHERE ID = {1}  AND ReferenceID = 0", tableName, contentID);
                        }
                        int count = this.ExecuteNonQuery(sqlString);
                        if (count < 1)
                        {
                            int referenceID = 0;

                            sqlString = string.Format("SELECT ReferenceID FROM {0} WHERE (ID = {1})", tableName, contentID);

                            using (IDataReader rdr = this.ExecuteReader(sqlString))
                            {
                                if (rdr.Read())
                                {
                                    referenceID = rdr.GetInt32(0);
                                }
                                rdr.Close();
                            }

                            if (referenceID > 0)
                            {
                                sqlString = string.Format("UPDATE {0} SET Hits = Hits + 1, LastHitsDate = getdate() WHERE ID = {1} AND ReferenceID = 0", tableName, referenceID);
                                if (this.DataBaseType == EDatabaseType.Oracle)
                                {
                                    sqlString = string.Format("UPDATE {0} SET Hits = Hits + 1, LastHitsDate = sysdate WHERE ID = {1}  AND ReferenceID = 0", tableName, referenceID);
                                }
                                this.ExecuteNonQuery(sqlString);
                            }
                        }
                    }
                }
            }
        }

        public void UpdateComments(string tableName, int contentID, int comments)
        {
            string sqlString = string.Format("UPDATE {0} SET Comments = {1} WHERE ID = {2}", tableName, comments, contentID);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdatePhotos(string tableName, int contentID, int photos)
        {
            string sqlString = string.Format("UPDATE {0} SET Photos = {1} WHERE ID = {2}", tableName, photos, contentID);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateTeleplays(string tableName, int contentID, int teleplays)
        {
            string sqlString = string.Format("UPDATE {0} SET Teleplays = {1} WHERE ID = {2}", tableName, teleplays, contentID);
            this.ExecuteNonQuery(sqlString);
        }

        public int GetReferenceID(ETableStyle tableStyle, string tableName, int contentID, out string linkUrl, out int nodeID)
        {
            int referenceID = 0;
            linkUrl = string.Empty;
            nodeID = 0;
            try
            {
                string sqlString = string.Format("SELECT ReferenceID, NodeID, LinkUrl FROM {0} WHERE ID = {1}", tableName, contentID);
                if (tableStyle == ETableStyle.BackgroundContent)
                {
                    sqlString = string.Format("SELECT ReferenceID, NodeID FROM {0} WHERE ID = {1}", tableName, contentID);
                }

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        referenceID = Convert.ToInt32(rdr[0]);
                        nodeID = Convert.ToInt32(rdr[1]);
                        if (tableStyle == ETableStyle.BackgroundContent)
                        {
                            linkUrl = rdr.GetValue(2).ToString();
                        }
                    }
                    rdr.Close();
                }
            }
            catch { }
            return referenceID;
        }

        public int GetReferenceID(ETableStyle tableStyle, string tableName, int contentID, out string linkUrl)
        {
            int referenceID = 0;
            linkUrl = string.Empty;
            try
            {
                string sqlString = string.Format("SELECT ReferenceID, LinkUrl FROM {0} WHERE ID = {1}", tableName, contentID);
                if (tableStyle == ETableStyle.BackgroundContent)
                {
                    sqlString = string.Format("SELECT ReferenceID FROM {0} WHERE ID = {1}", tableName, contentID);
                }

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        referenceID = Convert.ToInt32(rdr[0]);
                        if (tableStyle == ETableStyle.BackgroundContent)
                        {
                            linkUrl = rdr.GetValue(1).ToString();
                        }
                    }
                    rdr.Close();
                }
            }
            catch { }
            return referenceID;
        }

        public virtual int GetCountOfContentAdd(string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, DateTime begin, DateTime end, string userName)
        {
            string sqlString = string.Empty;
            if (string.IsNullOrEmpty(userName))
            {
                if (nodeIDArrayList.Count == 1)
                    sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID = {2} AND (AddDate BETWEEN '{3}' AND '{4}')", tableName, publishmentSystemID, nodeIDArrayList[0], begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
                else
                    sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID IN ({2}) AND (AddDate BETWEEN '{3}' AND '{4}')", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            else
            {
                if (nodeIDArrayList.Count == 1)
                    sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID = {2} AND (AddDate BETWEEN '{3}' AND '{4}') AND (AddUserName = '{5}')", tableName, publishmentSystemID, nodeIDArrayList[0], begin.ToShortDateString(), end.AddDays(1).ToShortDateString(), userName);
                else
                    sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID IN ({2}) AND (AddDate BETWEEN '{3}' AND '{4}') AND (AddUserName = '{5}')", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), begin.ToShortDateString(), end.AddDays(1).ToShortDateString(), userName);
            }

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public virtual int GetCountOfContentUpdate(string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, DateTime begin, DateTime end, string userName)
        {
            string sqlString = string.Empty;
            if (string.IsNullOrEmpty(userName))
            {
                if (nodeIDArrayList.Count == 1)
                    sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID = {2} AND (LastEditDate BETWEEN '{3}' AND '{4}') AND (LastEditDate <> AddDate)", tableName, publishmentSystemID, nodeIDArrayList[0], begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
                else
                    sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID IN ({2}) AND (LastEditDate BETWEEN '{3}' AND '{4}') AND (LastEditDate <> AddDate)", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            else
            {
                if (nodeIDArrayList.Count == 1)
                    sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID = {2} AND (LastEditDate BETWEEN '{3}' AND '{4}') AND (LastEditDate <> AddDate) AND (AddUserName = '{5}')", tableName, publishmentSystemID, nodeIDArrayList[0], begin.ToShortDateString(), end.AddDays(1).ToShortDateString(), userName);
                else
                    sqlString = string.Format("SELECT COUNT(ID) AS Num FROM {0} WHERE PublishmentSystemID = {1} AND NodeID IN ({2}) AND (LastEditDate BETWEEN '{3}' AND '{4}') AND (LastEditDate <> AddDate) AND (AddUserName = '{5}')", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), begin.ToShortDateString(), end.AddDays(1).ToShortDateString(), userName);
            }

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectCommendByCondition(ETableStyle tableStyle, string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, ETriState checkedState, bool isNoDup, bool isTrashContent)
        {
            return GetSelectCommendByCondition(tableStyle, tableName, publishmentSystemID, nodeIDArrayList, searchType, keyword, dateFrom, dateTo, checkedState, isNoDup, isTrashContent, false);
        }

        public string GetSelectCommendByCondition(ETableStyle tableStyle, string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, ETriState checkedState, bool isNoDup, bool isTrashContent, bool isViewContentOnlySelf)
        {
            if (nodeIDArrayList == null || nodeIDArrayList.Count == 0)
            {
                return null;
            }

            string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", dateFrom);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString = string.Format(" AND to_char(AddDate,'YYYY-MM-DD') >= '{0}' ", dateFrom);
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", dateTo);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString += string.Format(" AND to_char(AddDate,'YYYY-MM-DD') <= '{0}' ", dateTo);
                }
            }
            StringBuilder whereString = new StringBuilder("WHERE ");

            if (isTrashContent)
            {
                for (int i = 0; i < nodeIDArrayList.Count; i++)
                {
                    int theNodeID = (int)nodeIDArrayList[i];
                    nodeIDArrayList[i] = -theNodeID;
                }
            }

            if (nodeIDArrayList.Count == 1)
                whereString.AppendFormat("PublishmentSystemID = {0} AND (NodeID = {1}) ", publishmentSystemID, nodeIDArrayList[0]);
            else
                whereString.AppendFormat("PublishmentSystemID = {0} AND (NodeID IN ({1})) ", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList));


            if (string.IsNullOrEmpty(keyword))
            {
                whereString.Append(dateString);
            }
            else
            {
                bool columnExists = false;
                ArrayList columnNameArrayList = BaiRongDataProvider.TableStructureDAO.GetColumnNameArrayList(tableName);
                foreach (string columnName in columnNameArrayList)
                {
                    if (StringUtils.EqualsIgnoreCase(columnName, searchType))
                    {
                        columnExists = true;
                        whereString.AppendFormat("AND ([{0}] LIKE '%{1}%') {2} ", searchType, keyword, dateString);
                        break;
                    }
                }
                if (!columnExists)
                {
                    whereString.AppendFormat("AND (SettingsXML LIKE '%{0}={1}%') {2} ", searchType, keyword, dateString);
                }

                //if (TableManager.IsAttributeNameExists(tableStyle, tableName, searchType))
                //{
                //    whereString.AppendFormat("AND ([{0}] LIKE '%{1}%') {2} ", searchType, keyword, dateString);
                //}
                //else
                //{
                //    whereString.AppendFormat("AND (SettingsXML LIKE '%{0}={1}%') {2} ", searchType, keyword, dateString);
                //}
            }

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False' ");
            }

            if (isNoDup)
            {
                string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, "MIN(ID)", whereString.ToString() + " GROUP BY Title");
                whereString.AppendFormat("AND ID IN ({0})", sqlString);
            }

            //只查看自己的内容, update by sessionliang at 20151217
            if (isViewContentOnlySelf)
            {
                whereString.AppendFormat(" AND AddUserName = '{0}' ", AdminManager.Current.UserName);
            }

            whereString.Append(" ").Append(orderByString);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString());
        }

        public string GetSelectCommendByWhere(string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, string where, ETriState checkedState)
        {
            if (nodeIDArrayList == null || nodeIDArrayList.Count == 0)
            {
                return null;
            }

            string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            StringBuilder whereString = new StringBuilder("WHERE ");

            if (nodeIDArrayList.Count == 1)
                whereString.AppendFormat("PublishmentSystemID = {0} AND (NodeID = {1}) AND ({2}) ", publishmentSystemID, nodeIDArrayList[0], where);
            else
                whereString.AppendFormat("PublishmentSystemID = {0} AND (NodeID IN ({1})) AND ({2}) ", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), where);

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False' ");
            }

            whereString.Append(orderByString);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString());
        }

        public string GetSelectCommend(string tableName, int nodeID, ETriState checkedState)
        {
            string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            StringBuilder whereString = new StringBuilder();
            whereString.AppendFormat("WHERE (NodeID = {0}) ", nodeID);

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False'");
            }

            //whereString.Append(orderByString);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString(), orderByString);
        }

        public string GetSelectCommend(string tableName, int nodeID, ETriState checkedState, bool isViewContentOnlySelf)
        {
            string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            StringBuilder whereString = new StringBuilder();
            whereString.AppendFormat("WHERE (NodeID = {0}) ", nodeID);

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False'");
            }

            //只查看自己的内容, update by sessionliang at 20151217
            if (isViewContentOnlySelf)
            {
                whereString.AppendFormat(" AND AddUserName = '{0}' ", AdminManager.Current.UserName);
            }

            //whereString.Append(orderByString);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString(), orderByString);
        }

        public string GetSelectCommend(string tableName, ArrayList nodeIDArrayList, ETriState checkedState)
        {
            string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            StringBuilder whereString = new StringBuilder();

            if (nodeIDArrayList.Count == 1)
                whereString.AppendFormat("WHERE (NodeID = {0}) ", nodeIDArrayList[0]);
            else
                whereString.AppendFormat("WHERE (NodeID IN ({0})) ", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList));

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False'");
            }

            whereString.Append(orderByString);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString());
        }

        public string GetValue(string tableName, int contentID, string name)
        {
            string sqlString = string.Format("SELECT [{0}] FROM [{1}] WHERE ([ID] = {2})", name, tableName, contentID);
            return BaiRongDataProvider.DatabaseDAO.GetString(sqlString);
        }

        public void SetValue(string tableName, int contentID, string name, string value)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID = {3}", tableName, name, value, contentID);

            this.ExecuteNonQuery(sqlString);
        }

        public void AddContentGroupArrayList(string tableName, int contentID, ArrayList contentGroupArrayList)
        {
            ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(this.GetValue(tableName, contentID, ContentAttribute.ContentGroupNameCollection));
            foreach (string groupName in contentGroupArrayList)
            {
                if (!arraylist.Contains(groupName)) arraylist.Add(groupName);
            }
            this.SetValue(tableName, contentID, ContentAttribute.ContentGroupNameCollection, TranslateUtils.ObjectCollectionToString(arraylist));
        }

        public ArrayList GetReferenceIDArrayList(string tableName, ArrayList contentIDArrayList)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT ID FROM {0} WHERE NodeID > 0 AND ReferenceID IN ({1})", tableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    arraylist.Add(Convert.ToInt32(rdr[0]));
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetContentIDArrayList(string tableName, int nodeID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT ID FROM {0} WHERE NodeID = {1}", tableName, nodeID);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(contentID);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetContentIDArrayList(string tableName, int nodeID, bool isPeriods, string dateFrom, string dateTo, ETriState checkedState)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT ID FROM {0} WHERE NodeID = {1}", tableName, nodeID);
            if (isPeriods)
            {
                string dateString = string.Empty;
                if (!string.IsNullOrEmpty(dateFrom))
                {
                    dateString = string.Format(" AND AddDate >= '{0}' ", dateFrom);
                    if (this.DataBaseType == EDatabaseType.Oracle)
                    {
                        dateString = string.Format(" AND to_char(AddDate,'YYYY-MM-DD') >= '{0}' ", dateFrom);
                    }
                }
                if (!string.IsNullOrEmpty(dateTo))
                {
                    dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                    dateString += string.Format(" AND AddDate <= '{0}' ", dateTo);
                    if (this.DataBaseType == EDatabaseType.Oracle)
                    {
                        dateString += string.Format(" AND to_char(AddDate,'YYYY-MM-DD') <= '{0}' ", dateTo);
                    }
                }
                sqlString += dateString;
            }

            if (checkedState != ETriState.All)
            {
                sqlString += string.Format(" AND IsChecked = '{0}'", ETriStateUtils.GetValue(checkedState));
            }

            sqlString += string.Format(" ORDER BY Taxis DESC, ID DESC");

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(contentID);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetContentIDArrayListByPublishmentSystemID(string tableName, int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT ID FROM {0} WHERE PublishmentSystemID = {1}", tableName, publishmentSystemID);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(contentID);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetContentIDArrayListChecked(string tableName, ArrayList nodeIDArrayList, int totalNum, string orderByString, string whereString)
        {
            ArrayList arraylist = new ArrayList();

            if (nodeIDArrayList == null || nodeIDArrayList.Count == 0)
            {
                return arraylist;
            }

            string sqlString = string.Empty;

            if (totalNum > 0)
            {
                if (this.DataBaseType != EDatabaseType.Oracle)
                {
                    if (nodeIDArrayList.Count == 1)
                        sqlString = string.Format("SELECT TOP {0} ID FROM {1} WHERE (NodeID = {2} AND IsChecked = '{3}' {4}) {5}", totalNum, tableName, nodeIDArrayList[0], true.ToString(), whereString, orderByString);
                    else
                        sqlString = string.Format("SELECT TOP {0} ID FROM {1} WHERE (NodeID IN ({2}) AND IsChecked = '{3}' {4}) {5}", totalNum, tableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), true.ToString(), whereString, orderByString);
                }
                else
                {
                    if (nodeIDArrayList.Count == 1)
                        sqlString = string.Format(@"
SELECT * FROM (
    SELECT ID FROM {0} WHERE (NodeID = {1} AND IsChecked = '{2}' {3}) {4}
) WHERE ROWNUM <= {5}
", tableName, nodeIDArrayList[0], true.ToString(), whereString, orderByString, totalNum);
                    else
                        sqlString = string.Format(@"
SELECT * FROM (
    SELECT ID FROM {0} WHERE (NodeID IN ({1}) AND IsChecked = '{2}' {3}) {4}
) WHERE ROWNUM <= {5}
", tableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), true.ToString(), whereString, orderByString, totalNum);
                }
            }
            else
            {
                if (nodeIDArrayList.Count == 1)
                    sqlString = string.Format("SELECT ID FROM {0} WHERE (NodeID = {1} AND IsChecked = '{2}' {3}) {4}", tableName, nodeIDArrayList[0], true.ToString(), whereString, orderByString);
                else
                    sqlString = string.Format("SELECT ID FROM {0} WHERE (NodeID IN ({1}) AND IsChecked = '{2}' {3}) {4}", tableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), true.ToString(), whereString, orderByString);

            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(contentID);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetContentIDArrayListByTrash(int publishmentSystemID, string tableName)
        {
            string sqlString = string.Format("SELECT ID FROM {0} WHERE PublishmentSystemID = {1} AND NodeID < 0", tableName, publishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetIntArrayList(sqlString);
        }

        public int TrashContents(int publishmentSystemID, string tableName, ArrayList contentIDArrayList)
        {
            if (!string.IsNullOrEmpty(tableName) && contentIDArrayList != null && contentIDArrayList.Count > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET NodeID = -NodeID, LastEditDate = {1} WHERE PublishmentSystemID = {2} AND ID IN ({3})", tableName, SqlUtils.GetDefaultDateString(BaiRongDataProvider.DatabaseType), publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
                return this.ExecuteNonQuery(sqlString);
            }
            return 0;
        }

        public int TrashContentsByNodeID(int publishmentSystemID, string tableName, int nodeID)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                string sqlString = string.Format("UPDATE {0} SET NodeID = -NodeID, LastEditDate = {1} WHERE PublishmentSystemID = {2} AND NodeID = {2}", tableName, SqlUtils.GetDefaultDateString(BaiRongDataProvider.DatabaseType), publishmentSystemID, nodeID);
                return this.ExecuteNonQuery(sqlString);
            }
            return 0;
        }

        public int DeleteContents(string productID, int publishmentSystemID, string tableName, ArrayList contentIDArrayList)
        {
            if (!string.IsNullOrEmpty(tableName) && contentIDArrayList != null && contentIDArrayList.Count > 0)
            {
                TagUtils.RemoveTags(productID, publishmentSystemID, contentIDArrayList);

                string sqlString = string.Format("DELETE FROM {0} WHERE PublishmentSystemID = {1} AND ID IN ({2})", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
                return this.ExecuteNonQuery(sqlString);
            }
            return 0;
        }

        public int DeleteContentsByNodeID(string productID, int publishmentSystemID, string tableName, int nodeID, ArrayList contentIDArrayList)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                TagUtils.RemoveTags(productID, publishmentSystemID, contentIDArrayList);

                string sqlString = string.Format("DELETE FROM {0} WHERE PublishmentSystemID = {1} AND NodeID = {2}", tableName, publishmentSystemID, nodeID);
                return this.ExecuteNonQuery(sqlString);
            }
            return 0;
        }

        public void DeleteContentsArchive(int publishmentSystemID, string tableName, ArrayList contentIDArrayList)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE PublishmentSystemID = {1} AND ID IN ({2})", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteContentsByTrash(string productID, int publishmentSystemID, string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                ArrayList contentIDArrayList = BaiRongDataProvider.ContentDAO.GetContentIDArrayListByTrash(publishmentSystemID, tableName);
                TagUtils.RemoveTags(productID, publishmentSystemID, contentIDArrayList);

                string sqlString = string.Format("DELETE FROM {0} WHERE PublishmentSystemID = {1} AND NodeID < 0", tableName, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public int RestoreContentsByTrash(int publishmentSystemID, string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                string sqlString = string.Format("UPDATE {0} SET NodeID = -NodeID, LastEditDate = {1} WHERE PublishmentSystemID = {2} AND NodeID < 0", tableName, SqlUtils.GetDefaultDateString(BaiRongDataProvider.DatabaseType), publishmentSystemID);
                return this.ExecuteNonQuery(sqlString);
            }
            return 0;
        }

        public int GetContentID(string tableName, int nodeID, int taxis, bool isNextContent)
        {
            int contentID = 0;
            string sqlString = string.Empty;
            if (isNextContent)
            {
                sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE (NodeID = {1} AND Taxis < {2} AND IsChecked = 'True') ORDER BY Taxis DESC", tableName, nodeID, taxis);
            }
            else
            {
                sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE (NodeID = {1} AND Taxis > {2} AND IsChecked = 'True') ORDER BY Taxis", tableName, nodeID, taxis);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        contentID = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return contentID;
        }

        //根据排序规则获得第一条内容的ID
        public int GetContentID(string tableName, int nodeID, string orderByString)
        {
            int contentID = 0;
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE (NodeID = {1}) {2}", tableName, nodeID, orderByString);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        contentID = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return contentID;
        }

        public int GetContentID(string tableName, int nodeID, string attributeName, string value)
        {
            int contentID = 0;
            string sqlString = string.Format("SELECT ID FROM {0} WHERE (NodeID = {1} AND [{2}] = '{3}')", tableName, nodeID, attributeName, value);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        contentID = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return contentID;
        }

        public ArrayList GetValueArrayList(string tableName, int nodeID, string name)
        {
            string sqlString = string.Format("SELECT [{0}] From [{1}] WHERE [NodeID] = {2}", name, tableName, nodeID);
            return BaiRongDataProvider.DatabaseDAO.GetStringArrayList(sqlString);
        }

        public ArrayList GetValueArrayListByStartString(string tableName, int nodeID, string name, string startString, int totalNum)
        {
            string totalString = string.Empty;
            if (totalNum > 0)
            {
                totalString = " TOP " + totalNum + " ";
            }
            string sqlString = string.Format("SELECT DISTINCT {0} {1}, Taxis FROM {2} WHERE NodeID = {3} AND {0} LIKE '{4}%' ORDER BY Taxis DESC", totalString, name, tableName, nodeID, startString);
            if (this.DataBaseType == EDatabaseType.SqlServer)
            {
                sqlString = string.Format("SELECT DISTINCT {0} {1}, Taxis FROM {2} WHERE NodeID = {3} AND CHARINDEX('{4}',{1}) > 0  ORDER BY Taxis DESC", totalString, name, tableName, nodeID, startString);
            }
            else if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = string.Format("SELECT DISTINCT {0} {1}, Taxis FROM {2} WHERE NodeID = {3} AND instr({1}, '{4}') > 0  ORDER BY Taxis DESC", totalString, name, tableName, nodeID, startString);
            }
            return BaiRongDataProvider.DatabaseDAO.GetStringArrayList(sqlString);
        }

        public int GetNodeID(string tableName, int contentID)
        {
            int nodeID = 0;
            string sqlString = string.Format("SELECT {0} FROM {1} WHERE (ID = {2})", ContentAttribute.NodeID, tableName, contentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        nodeID = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return nodeID;
        }

        public DateTime GetAddDate(string tableName, int contentID)
        {
            DateTime addDate = DateTime.Now;
            string sqlString = string.Format("SELECT [{0}] FROM [{1}] WHERE ([ID] = {2})", ContentAttribute.AddDate, tableName, contentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    addDate = rdr.GetDateTime(0);
                }
                rdr.Close();
            }
            return addDate;
        }

        public DateTime GetLastEditDate(string tableName, int contentID)
        {
            DateTime lastEditDate = DateTime.Now;
            string sqlString = string.Format("SELECT [{0}] FROM [{1}] WHERE ([ID] = {2})", ContentAttribute.LastEditDate, tableName, contentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lastEditDate = rdr.GetDateTime(0);
                }
                rdr.Close();
            }
            return lastEditDate;
        }

        public int GetCount(string tableName, int nodeID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (NodeID = {1})", tableName, nodeID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public virtual int GetCountChecked(string tableName, int nodeID, int days)
        {
            string whereString = string.Empty;
            if (days > 0)
            {
                whereString = string.Format("AND (DATEDIFF([Day], AddDate, getdate()) < {0})", days);
            }
            return GetCountChecked(tableName, nodeID, whereString);
        }

        public int GetCountChecked(string tableName, int nodeID, string whereString)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (NodeID = {1} AND IsChecked = '{2}' {3})", tableName, nodeID, true.ToString(), whereString);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetSequence(string tableName, int nodeID, int contentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE NodeID = {1} AND IsChecked = '{2}' AND Taxis < (SELECT Taxis FROM {0} WHERE (ID = {3}))", tableName, nodeID, true.ToString(), contentID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString) + 1;
        }

        public string GetSelectCommendOfAdminExcludeRecycle(string tableName, int publishmentSystemID, DateTime begin, DateTime end)
        {
            string sqlString = string.Empty;
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                //sqlString = string.Format("SELECT AddUserName, Count(AddUserName) AS TotalNum FROM {0} INNER JOIN bairong_Administrator ON AddUserName = bairong_Administrator.UserName WHERE {0}.PublishmentSystemID = {1} AND ({0}.AddDate BETWEEN {2} AND {3} AND ({0}.NodeID > 0)) GROUP BY AddUserName", tableName, publishmentSystemID, SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)));
                sqlString = string.Format(@"select userName,SUM(addCount) as addCount, SUM(updateCount) as updateCount, SUM(commentCount) as commentCount from( 
SELECT AddUserName as userName, Count(AddUserName) as addCount, 0 as updateCount, 0 as commentCount FROM {0} 
INNER JOIN bairong_Administrator ON AddUserName = bairong_Administrator.UserName 
WHERE {0}.PublishmentSystemID = {1} AND (({0}.NodeID > 0)) 
AND LastEditDate BETWEEN '{2}' AND '{3}'
GROUP BY AddUserName
Union
SELECT LastEditUserName as userName,0 as addCount, Count(LastEditUserName) as updateCount, 0 as commentCount FROM {0} 
INNER JOIN bairong_Administrator ON LastEditUserName = bairong_Administrator.UserName 
WHERE {0}.PublishmentSystemID = 1 AND (({0}.NodeID > 0)) 
AND LastEditDate BETWEEN '{2}' AND '{3}'
AND LastEditDate != AddDate
GROUP BY LastEditUserName
Union
SELECT siteserver_Comment.UserName as userName,0 as addCount,0 as updateCount, COUNT(siteserver_Comment.UserName) as commentCount FROM siteserver_Comment
INNER JOIN bairong_Administrator ON siteserver_Comment.UserName = bairong_Administrator.UserName 
WHERE siteserver_Comment.PublishmentSystemID = 1 AND ((siteserver_Comment.NodeID > 0)) 
AND AddDate BETWEEN '{2}' AND '{3}'
GROUP BY siteserver_Comment.UserName
) as tmp
group by tmp.userName", tableName, publishmentSystemID, SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)));
            }
            else
            {
                //sqlString = string.Format("SELECT AddUserName, Count(AddUserName) AS TotalNum FROM {0} INNER JOIN bairong_Administrator ON AddUserName = bairong_Administrator.UserName WHERE {0}.PublishmentSystemID = {1} AND ({0}.AddDate BETWEEN '{2}' AND '{3}' AND ({0}.NodeID > 0)) GROUP BY AddUserName", tableName, publishmentSystemID, DateUtils.GetDateString(begin), DateUtils.GetDateString(end.AddDays(1)));

                sqlString = string.Format(@"select userName,SUM(addCount) as addCount, SUM(updateCount) as updateCount, SUM(commentCount) as commentCount from( 
SELECT AddUserName as userName, Count(AddUserName) as addCount, 0 as updateCount, 0 as commentCount FROM {0} 
INNER JOIN bairong_Administrator ON AddUserName = bairong_Administrator.UserName 
WHERE {0}.PublishmentSystemID = {1} AND (({0}.NodeID > 0)) 
AND LastEditDate BETWEEN '{2}' AND '{3}'
GROUP BY AddUserName
Union
SELECT LastEditUserName as userName,0 as addCount, Count(LastEditUserName) as updateCount, 0 as commentCount FROM {0} 
INNER JOIN bairong_Administrator ON LastEditUserName = bairong_Administrator.UserName 
WHERE {0}.PublishmentSystemID = {1} AND (({0}.NodeID > 0)) 
AND LastEditDate BETWEEN '{2}' AND '{3}'
AND LastEditDate != AddDate
GROUP BY LastEditUserName
Union
SELECT siteserver_Comment.AdminName as userName,0 as addCount,0 as updateCount, COUNT(siteserver_Comment.AdminName) as commentCount FROM siteserver_Comment
INNER JOIN bairong_Administrator ON siteserver_Comment.AdminName = bairong_Administrator.UserName 
WHERE siteserver_Comment.PublishmentSystemID = {1} AND ((siteserver_Comment.NodeID > 0)) 
AND AddDate BETWEEN '{2}' AND '{3}'
GROUP BY siteserver_Comment.AdminName
) as tmp
group by tmp.userName", tableName, publishmentSystemID, DateUtils.GetDateString(begin), DateUtils.GetDateString(end.AddDays(1)));

            }

            return sqlString;
        }

        public virtual ArrayList GetNodeIDArrayListCheckedByLastEditDateHour(string tableName, int publishmentSystemID, int hour)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT DISTINCT NodeID FROM [{0}] WHERE (PublishmentSystemID = {1}) AND (IsChecked = '{2}') AND (LastEditDate BETWEEN DATEADD([hour] , -{3} , GETDATE()) AND GETDATE())", tableName, publishmentSystemID, true.ToString(), hour);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int nodeID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(nodeID);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public string GetSelectedCommendByCheck(string tableName, int publishmentSystemID, bool isSystemAdministrator, ArrayList owningNodeIDArrayList, ArrayList checkLevelArrayList)
        {
            string whereString = string.Empty;

            if (isSystemAdministrator)
            {
                whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID > 0 AND IsChecked='{1}' AND CheckedLevel IN ({2}) ", publishmentSystemID, false.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(checkLevelArrayList));
            }
            else
            {
                if (owningNodeIDArrayList.Count == 1)
                    whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID = {1} AND IsChecked='{2}' AND CheckedLevel IN ({3}) ", publishmentSystemID, owningNodeIDArrayList[0], false.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(checkLevelArrayList));
                else
                    whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID IN ({1}) AND IsChecked='{2}' AND CheckedLevel IN ({3}) ", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(owningNodeIDArrayList), false.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(checkLevelArrayList));
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString);
        }

        public IEnumerable GetStlDataSourceChecked(string tableName, ArrayList nodeIDArrayList, int startNum, int totalNum, string orderByString, string whereString, bool isNoDup, NameValueCollection otherAttributes)
        {
            if (nodeIDArrayList == null || nodeIDArrayList.Count == 0)
            {
                return null;
            }
            string sqlWhereString = string.Empty;
            if (nodeIDArrayList.Count == 1)
                sqlWhereString = string.Format("WHERE (NodeID = {0} AND IsChecked = '{1}' {2})", nodeIDArrayList[0], true.ToString(), whereString);
            else
                sqlWhereString = string.Format("WHERE (NodeID IN ({0}) AND IsChecked = '{1}' {2})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), true.ToString(), whereString);

            if (isNoDup)
            {
                string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, "MIN(ID)", sqlWhereString.ToString() + " GROUP BY Title");
                sqlWhereString += string.Format(" AND ID IN ({0})", sqlString);
            }

            if (otherAttributes != null && otherAttributes.Count > 0)
            {
                ArrayList columnNameArrayList = BaiRongDataProvider.TableStructureDAO.GetColumnNameArrayList(tableName, true);
                foreach (string attributeName in otherAttributes)
                {
                    if (columnNameArrayList.Contains(attributeName))
                    {
                        string value = otherAttributes[attributeName];
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = value.Trim();
                            if (StringUtils.StartsWithIgnoreCase(value, "not:"))
                            {
                                value = value.Substring("not:".Length);
                                if (value.IndexOf(',') == -1)
                                {
                                    sqlWhereString += string.Format(" AND ({0} <> '{1}')", attributeName, value);
                                }
                                else
                                {
                                    List<string> collection = TranslateUtils.StringCollectionToStringList(value);
                                    foreach (string val in collection)
                                    {
                                        sqlWhereString += string.Format(" AND ({0} <> '{1}')", attributeName, val);
                                    }
                                }
                            }
                            else if (StringUtils.StartsWithIgnoreCase(value, "contains:"))
                            {
                                value = value.Substring("contains:".Length);
                                if (value.IndexOf(',') == -1)
                                {
                                    sqlWhereString += string.Format(" AND ({0} LIKE '%{1}%')", attributeName, value);
                                }
                                else
                                {
                                    StringBuilder builder = new StringBuilder(" AND (");
                                    List<string> collection = TranslateUtils.StringCollectionToStringList(value);
                                    foreach (string val in collection)
                                    {
                                        builder.AppendFormat(" {0} LIKE '%{1}%' OR ", attributeName, val);
                                    }
                                    builder.Length -= 3;

                                    builder.Append(")");

                                    sqlWhereString += builder.ToString();
                                }
                            }
                            else if (StringUtils.StartsWithIgnoreCase(value, "start:"))
                            {
                                value = value.Substring("start:".Length);
                                if (value.IndexOf(',') == -1)
                                {
                                    sqlWhereString += string.Format(" AND ({0} LIKE '{1}%')", attributeName, value);
                                }
                                else
                                {
                                    StringBuilder builder = new StringBuilder(" AND (");
                                    List<string> collection = TranslateUtils.StringCollectionToStringList(value);
                                    foreach (string val in collection)
                                    {
                                        builder.AppendFormat(" {0} LIKE '{1}%' OR ", attributeName, val);
                                    }
                                    builder.Length -= 3;

                                    builder.Append(")");

                                    sqlWhereString += builder.ToString();
                                }
                            }
                            else if (StringUtils.StartsWithIgnoreCase(value, "end:"))
                            {
                                value = value.Substring("end:".Length);
                                if (value.IndexOf(',') == -1)
                                {
                                    sqlWhereString += string.Format(" AND ({0} LIKE '%{1}')", attributeName, value);
                                }
                                else
                                {
                                    StringBuilder builder = new StringBuilder(" AND (");
                                    List<string> collection = TranslateUtils.StringCollectionToStringList(value);
                                    foreach (string val in collection)
                                    {
                                        builder.AppendFormat(" {0} LIKE '%{1}' OR ", attributeName, val);
                                    }
                                    builder.Length -= 3;

                                    builder.Append(")");

                                    sqlWhereString += builder.ToString();
                                }
                            }
                            else
                            {
                                if (value.IndexOf(',') == -1)
                                {
                                    sqlWhereString += string.Format(" AND ({0} = '{1}')", attributeName, value);
                                }
                                else
                                {
                                    StringBuilder builder = new StringBuilder(" AND (");
                                    List<string> collection = TranslateUtils.StringCollectionToStringList(value);
                                    foreach (string val in collection)
                                    {
                                        builder.AppendFormat(" {0} = '{1}' OR ", attributeName, val);
                                    }
                                    builder.Length -= 3;

                                    builder.Append(")");

                                    sqlWhereString += builder.ToString();
                                }
                            }
                        }
                    }
                }
            }

            if (startNum <= 1)
            {
                return GetDataSourceByContentNumAndWhereString(tableName, totalNum, sqlWhereString, orderByString);
            }
            else
            {
                return GetDataSourceByStartNum(tableName, startNum, totalNum, sqlWhereString, orderByString);
            }
        }

        public int GetStlCountChecked(string tableName, ArrayList nodeIDArrayList, string whereString)
        {
            if (nodeIDArrayList == null || nodeIDArrayList.Count == 0)
            {
                return 0;
            }
            string sqlWhereString = string.Empty;
            if (nodeIDArrayList.Count == 1)
                sqlWhereString = string.Format("WHERE (NodeID ={0} AND IsChecked = '{1}' {2})", nodeIDArrayList[0], true.ToString(), whereString);
            else
                sqlWhereString = string.Format("WHERE (NodeID IN ({0}) AND IsChecked = '{1}' {2})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), true.ToString(), whereString);

            string sqlString = null;
            if (BaiRongDataProvider.DatabaseType != EDatabaseType.Oracle)
            {
                sqlString = string.Format("SELECT COUNT(*) FROM {0} {1}", tableName, sqlWhereString);
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(1) FROM {0} {1}", tableName, sqlWhereString);
            }

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private IEnumerable GetDataSourceByContentNumAndWhereString(string tableName, int totalNum, string whereString, string orderByString)
        {
            IEnumerable enumerable = null;
            if (!string.IsNullOrEmpty(tableName))
            {
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, totalNum, SqlUtils.Asterisk, whereString, orderByString);
                enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT);
            }
            return enumerable;
        }

        private IEnumerable GetDataSourceByStartNum(string tableName, int startNum, int totalNum, string whereString, string orderByString)
        {
            IEnumerable enumerable = null;
            if (!string.IsNullOrEmpty(tableName))
            {
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, SqlUtils.Asterisk, whereString, orderByString);
                enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT);
            }
            return enumerable;
        }

        public string GetStlWhereString(string productID, int publishmentSystemID, string group, string groupNot, string tags, bool isTopExists, bool isTop, string where)
        {
            StringBuilder whereStringBuilder = new StringBuilder();

            if (isTopExists)
            {
                whereStringBuilder.AppendFormat(" AND IsTop = '{0}' ", isTop.ToString());
            }

            if (!string.IsNullOrEmpty(group))
            {
                group = group.Trim().Trim(',');
                string[] groupArr = group.Split(',');
                if (groupArr != null && groupArr.Length > 0)
                {
                    whereStringBuilder.Append(" AND (");
                    foreach (string theGroup in groupArr)
                    {
                        if (this.DataBaseType == EDatabaseType.SqlServer)
                        {
                            whereStringBuilder.AppendFormat(" ({0} = '{1}' OR CHARINDEX('{1},',{0}) > 0 OR CHARINDEX(',{1},',{0}) > 0 OR CHARINDEX(',{1}',{0}) > 0) OR ", ContentAttribute.ContentGroupNameCollection, theGroup.Trim());
                        }
                        else if (this.DataBaseType == EDatabaseType.Oracle)
                        {
                            whereStringBuilder.AppendFormat(" ({0} = '{1}' OR instr({0}, '{1},') > 0 OR instr({0}, ',{1},') > 0 OR instr({0}, ',{1}') > 0) OR ", ContentAttribute.ContentGroupNameCollection, theGroup.Trim());
                        }
                        else
                        {
                            whereStringBuilder.AppendFormat(" ({0} = '{1}' OR {0} LIKE '{1},%' OR {0} LIKE '%,{1},%' OR {0} LIKE '%,{1}') OR ", ContentAttribute.ContentGroupNameCollection, theGroup.Trim());
                        }
                    }
                    if (groupArr.Length > 0)
                    {
                        whereStringBuilder.Length = whereStringBuilder.Length - 3;
                    }
                    whereStringBuilder.Append(") ");
                }
            }

            if (!string.IsNullOrEmpty(groupNot))
            {
                groupNot = groupNot.Trim().Trim(',');
                string[] groupNotArr = groupNot.Split(',');
                if (groupNotArr != null && groupNotArr.Length > 0)
                {
                    whereStringBuilder.Append(" AND (");
                    foreach (string theGroupNot in groupNotArr)
                    {
                        if (this.DataBaseType == EDatabaseType.SqlServer)
                        {
                            whereStringBuilder.AppendFormat(" ({0} <> '{1}' AND CHARINDEX('{1},',{0}) = 0 AND CHARINDEX(',{1},',{0}) = 0 AND CHARINDEX(',{1}',{0}) = 0) AND ", ContentAttribute.ContentGroupNameCollection, theGroupNot.Trim());
                        }
                        else if (this.DataBaseType == EDatabaseType.Oracle)
                        {
                            whereStringBuilder.AppendFormat(" ({0} <> '{1}' AND instr({0}, '{1},') = 0 AND instr({0}, ',{1},') = 0 AND instr({0}, ',{1}') = 0) AND ", ContentAttribute.ContentGroupNameCollection, theGroupNot.Trim());
                        }
                        else
                        {
                            whereStringBuilder.AppendFormat(" ({0} <> '{1}' AND {0} NOT LIKE '{1},%' AND {0} NOT LIKE '%,{1},%' AND {0} NOT LIKE '%,{1}') AND ", ContentAttribute.ContentGroupNameCollection, theGroupNot.Trim());
                        }
                    }
                    if (groupNotArr.Length > 0)
                    {
                        whereStringBuilder.Length = whereStringBuilder.Length - 4;
                    }
                    whereStringBuilder.Append(") ");
                }
            }

            if (!string.IsNullOrEmpty(tags))
            {
                StringCollection tagCollection = TagUtils.ParseTagsString(tags);
                ArrayList contentIDArrayList = BaiRongDataProvider.TagDAO.GetContentIDArrayListByTagCollection(tagCollection, productID, publishmentSystemID);
                if (contentIDArrayList.Count > 0)
                {
                    whereStringBuilder.AppendFormat(" AND (ID IN ({0}))", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
                }
            }

            if (!string.IsNullOrEmpty(where))
            {
                whereStringBuilder.AppendFormat(" AND ({0}) ", where);
            }

            return whereStringBuilder.ToString();
        }

        public string GetSortFieldName()
        {
            return "Taxis";
        }

        public ArrayList GetContentIDArrayListCheck(int publishmentSystemID, int nodeID, string tableName)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT ID FROM {0} WHERE PublishmentSystemID = {1} AND NodeID = {2} AND IsChecked = '{3}' AND CheckTaskDate IS NOT NULL AND CheckTaskDate <= getdate()", tableName, publishmentSystemID, nodeID, false.ToString());
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(contentID);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetContentIDArrayListUnCheck(int publishmentSystemID, int nodeID, string tableName)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT ID FROM {0} WHERE PublishmentSystemID = {1} AND NodeID = {2} AND IsChecked = '{3}' AND UnCheckTaskDate IS NOT NULL AND UnCheckTaskDate <= getdate()", tableName, publishmentSystemID, nodeID, true.ToString());
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(contentID);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public DataSet GetDataSetOfAdminExcludeRecycle(string tableName, int publishmentSystemID, DateTime begin, DateTime end)
        {
            string sqlString = this.GetSelectCommendOfAdminExcludeRecycle(tableName, publishmentSystemID, begin, end);

            return this.ExecuteDataset(sqlString); ;
        }

        #region 投稿管理

        /// <summary>
        /// 投稿内容
        /// </summary>
        /// <param name="tableStyle"></param>
        /// <param name="tableName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeIDArrayList"></param>
        /// <param name="searchType"></param>
        /// <param name="keyword"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="checkedState"></param>
        /// <param name="isNoDup"></param>
        /// <param name="isTrashContent"></param>
        /// <param name="isViewContentOnlySelf"></param>
        /// <returns></returns>
        public string GetSelectCommendByMLib(ETableStyle tableStyle, string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, ETriState checkedState, bool isNoDup, bool isTrashContent, bool isViewContentOnlySelf, string memberName, bool isMLib)
        {
            if (nodeIDArrayList == null || nodeIDArrayList.Count == 0)
            {
                return null;
            }

            string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", dateFrom);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString = string.Format(" AND to_char(AddDate,'YYYY-MM-DD') >= '{0}' ", dateFrom);
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", dateTo);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString += string.Format(" AND to_char(AddDate,'YYYY-MM-DD') <= '{0}' ", dateTo);
                }
            }
            StringBuilder whereString = new StringBuilder("WHERE ");

            if (isTrashContent)
            {
                for (int i = 0; i < nodeIDArrayList.Count; i++)
                {
                    int theNodeID = (int)nodeIDArrayList[i];
                    nodeIDArrayList[i] = -theNodeID;
                }
            }

            if (nodeIDArrayList.Count == 1)
                whereString.AppendFormat("PublishmentSystemID = {0} AND (NodeID = {1}) ", publishmentSystemID, nodeIDArrayList[0]);
            else
                whereString.AppendFormat("PublishmentSystemID = {0} AND (NodeID IN ({1})) ", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList));

            if (string.IsNullOrEmpty(keyword))
            {
                whereString.Append(dateString);
            }
            else
            {
                bool columnExists = false;
                ArrayList columnNameArrayList = BaiRongDataProvider.TableStructureDAO.GetColumnNameArrayList(tableName);
                foreach (string columnName in columnNameArrayList)
                {
                    if (StringUtils.EqualsIgnoreCase(columnName, searchType))
                    {
                        columnExists = true;
                        whereString.AppendFormat("AND ([{0}] LIKE '%{1}%') {2} ", searchType, keyword, dateString);
                        break;
                    }
                }
                if (!columnExists)
                {
                    whereString.AppendFormat("AND (SettingsXML LIKE '%{0}={1}%') {2} ", searchType, keyword, dateString);
                }
            }

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False' ");
            }

            if (isNoDup)
            {
                string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, "MIN(ID)", whereString.ToString() + " GROUP BY Title");
                whereString.AppendFormat("AND ID IN ({0})", sqlString);
            }

            //只查看自己的内容, update by sessionliang at 20151217
            if (isViewContentOnlySelf)
            {
                whereString.AppendFormat(" AND AddUserName = '{0}' ", AdminManager.Current.UserName);
            }
            //投稿内容
            if (isMLib)
            {
                whereString.AppendFormat(" AND MemberName != '' ");
            }
            //投稿用户
            if (!string.IsNullOrEmpty(memberName))
            {
                whereString.AppendFormat(" AND MemberName like '%{0}%' ", memberName);
            }
            whereString.Append(" ").Append(orderByString);

            ArrayList columns=new ArrayList ();
            columns.Add(ContentAttribute.ID);
            columns.Add(ContentAttribute.Title);
            columns.Add(ContentAttribute.NodeID);
            columns.Add(ContentAttribute.CheckedLevel);
            columns.Add(ContentAttribute.IsChecked);
            columns.Add(ContentAttribute.PublishmentSystemID);
            columns.Add(ContentAttribute.MemberName);
            columns.Add(ContentAttribute.AddDate);
            columns.Add(ContentAttribute.IsTop);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(columns), whereString.ToString());
        }


        /// <summary>
        /// 获取用户的投稿信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="whereString"></param>
        /// <returns></returns>
        public ArrayList GetUserContent(string tableName, string whereString, int pageIndex, int prePageNum)
        {
            string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            StringBuilder SQL_SELECT = new StringBuilder();
            SQL_SELECT.AppendFormat(" SELECT tmp.* from ( ");
            SQL_SELECT.AppendFormat(" SELECT *, ROW_NUMBER() OVER(ORDER BY IsViewed, AddDate DESC) as rowNum FROM {0} WHERE {1} {2} ", tableName, whereString, orderByString);
            SQL_SELECT.AppendFormat(" ) as tmp ");
            SQL_SELECT.AppendFormat(" WHERE tmp.rowNum >= {0} and tmp.rowNum <= {1} ", (pageIndex - 1) * prePageNum + 1, pageIndex * prePageNum);

            ArrayList list = new ArrayList();
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT.ToString()))
            {
                while (rdr.Read())
                {
                    ContentInfo info = new ContentInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    list.Add(info);
                }
                rdr.Close();
            }
            return list;
        }

        #endregion


        public string GetSelectCommendOfDeptExcludeRecycle(string tableName, int publishmentSystemID, DateTime begin, DateTime end, int deptID, string userName)
        {
            string sqlString = string.Empty;
            string deptStr = string.Empty;

            if (deptID > 0)
            {
                deptStr = string.Format(@" and bairong_Administrator.UserName in (select UserName from dbo.bairong_Administrator
where DepartmentID ={0}) ", deptID);
                if (!string.IsNullOrEmpty(userName))
                    deptStr = string.Format(@" and (bairong_Administrator.UserName like '%{0}%' or bairong_Administrator.DisplayName like '%{0}%') ", userName);
            }
            else
            {
                if (!string.IsNullOrEmpty(userName))
                    deptStr = string.Format(@" and (bairong_Administrator.UserName like '%{0}%' or bairong_Administrator.DisplayName like '%{0}%') ", userName);
            }

            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = string.Format(@"select userName,SUM(addCount) as addCount, SUM(updateCount) as updateCount,DepartmentID from( 
SELECT AddUserName as userName, Count(AddUserName) as addCount, 0 as updateCount, 0 as commentCount,DepartmentID FROM {0} 
INNER JOIN bairong_Administrator ON AddUserName = bairong_Administrator.UserName  {4}
WHERE {0}.PublishmentSystemID = {1} AND (({0}.NodeID > 0)) 
AND LastEditDate BETWEEN '{2}' AND '{3}'
GROUP BY DepartmentID,AddUserName
Union
SELECT LastEditUserName as userName,0 as addCount, Count(LastEditUserName) as updateCount, 0 as commentCount,DepartmentID FROM {0} 
INNER JOIN bairong_Administrator ON LastEditUserName = bairong_Administrator.UserName  {4}
WHERE {0}.PublishmentSystemID = 1 AND (({0}.NodeID > 0)) 
AND LastEditDate BETWEEN '{2}' AND '{3}'
AND LastEditDate != AddDate
GROUP BY DepartmentID,LastEditUserName 
) as tmp
group by tmp.DepartmentID,tmp.userName", tableName, publishmentSystemID, SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end.AddDays(1)), deptStr);
            }
            else
            {
                sqlString = string.Format(@"select userName,SUM(addCount) as addCount, SUM(updateCount) as updateCount,DepartmentID from( 
SELECT AddUserName as userName, Count(AddUserName) as addCount, 0 as updateCount, 0 as commentCount,DepartmentID FROM {0} 
INNER JOIN bairong_Administrator ON AddUserName = bairong_Administrator.UserName  {4}
WHERE {0}.PublishmentSystemID = {1} AND (({0}.NodeID > 0)) 
AND LastEditDate BETWEEN '{2}' AND '{3}' 
GROUP BY DepartmentID,AddUserName
Union
SELECT LastEditUserName as userName,0 as addCount, Count(LastEditUserName) as updateCount, 0 as commentCount,DepartmentID FROM {0} 
INNER JOIN bairong_Administrator ON LastEditUserName = bairong_Administrator.UserName {4}
WHERE {0}.PublishmentSystemID = {1} AND (({0}.NodeID > 0)) 
AND LastEditDate BETWEEN '{2}' AND '{3}'
AND LastEditDate != AddDate 
GROUP BY DepartmentID,LastEditUserName 
) as tmp
group by tmp.DepartmentID,tmp.userName", tableName, publishmentSystemID, DateUtils.GetDateString(begin), DateUtils.GetDateString(end.AddDays(1)), deptStr);

            }

            return sqlString;
        }



        public string GetSelectCommendBySelectType(ETableStyle tableStyle, string tableName, int publishmentSystemID, ArrayList nodeIDArrayList, string searchType, string keyword, string dateFrom, string dateTo, ETriState checkedState, bool isNoDup, bool isTrashContent, string selectType, string adminName)
        {
            if (nodeIDArrayList == null || nodeIDArrayList.Count == 0)
            {
                return null;
            }

            string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);

            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", dateFrom);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString = string.Format(" AND to_char(AddDate,'YYYY-MM-DD') >= '{0}' ", dateFrom);
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", dateTo);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString += string.Format(" AND to_char(AddDate,'YYYY-MM-DD') <= '{0}' ", dateTo);
                }
            }
            StringBuilder whereString = new StringBuilder("WHERE ");

            if (isTrashContent)
            {
                for (int i = 0; i < nodeIDArrayList.Count; i++)
                {
                    int theNodeID = (int)nodeIDArrayList[i];
                    nodeIDArrayList[i] = -theNodeID;
                }
            }

            if (nodeIDArrayList.Count == 1)
                whereString.AppendFormat("PublishmentSystemID = {0} AND (NodeID = {1}) ", publishmentSystemID, nodeIDArrayList[0]);
            else
                whereString.AppendFormat("PublishmentSystemID = {0} AND (NodeID IN ({1})) ", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList));


            if (string.IsNullOrEmpty(keyword))
            {
                whereString.Append(dateString);
            }
            else
            {
                bool columnExists = false;
                ArrayList columnNameArrayList = BaiRongDataProvider.TableStructureDAO.GetColumnNameArrayList(tableName);
                foreach (string columnName in columnNameArrayList)
                {
                    if (StringUtils.EqualsIgnoreCase(columnName, searchType))
                    {
                        columnExists = true;
                        whereString.AppendFormat("AND ([{0}] LIKE '%{1}%') {2} ", searchType, keyword, dateString);
                        break;
                    }
                }
                if (!columnExists)
                {
                    whereString.AppendFormat("AND (SettingsXML LIKE '%{0}={1}%') {2} ", searchType, keyword, dateString);
                }
                 
            }

            if (checkedState == ETriState.True)
            {
                whereString.Append("AND IsChecked='True' ");
            }
            else if (checkedState == ETriState.False)
            {
                whereString.Append("AND IsChecked='False' ");
            }

            if (isNoDup)
            {
                string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, "MIN(ID)", whereString.ToString() + " GROUP BY Title");
                whereString.AppendFormat("AND ID IN ({0})", sqlString);
            }

            //只查看自己的内容, update by sessionliang at 20151217
            if (selectType=="add")
            {
                whereString.AppendFormat(" AND AddUserName = '{0}' ", adminName);
            }

            if (selectType == "edit")
            {
                whereString.AppendFormat(" AND LastEditUserName = '{0}' ", adminName);
            }
            whereString.Append(" ").Append(orderByString);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereString.ToString());
        }
    }
}
