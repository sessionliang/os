using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Core;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using SiteServer.BBS.Core.TemplateParser.Model;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class ThreadDAO : DataProviderBase, IThreadDAO
    {
        private int InsertWithTrans(ThreadInfo info, IDbTransaction trans)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, ThreadInfo.TableName, out parms);

            this.ExecuteNonQuery(trans, SQL_INSERT, parms);

            return BaiRongDataProvider.DatabaseDAO.GetSequence(trans, ThreadInfo.TableName);
        }

        public int Insert(int publishmentSystemID, int areaID, int forumID, int categoryID, string title, string content, bool isChecked, bool isSignature, bool isAttachment, EThreadType threadType, out int postID)
        {
            ThreadInfo threadInfo = new ThreadInfo(publishmentSystemID);
            threadInfo.PublishmentSystemID = publishmentSystemID;
            threadInfo.AreaID = areaID;
            threadInfo.ForumID = forumID;
            threadInfo.UserName = UserUtils.GetInstance(publishmentSystemID).UserName;
            threadInfo.CategoryID = categoryID;
            threadInfo.Title = title;
            threadInfo.AddDate = DateTime.Now;
            threadInfo.IsChecked = true;
            threadInfo.IsImage = StringUtilityBBS.IsImageExists(content);
            threadInfo.IsAttachment = isAttachment;
            threadInfo.Taxis = this.GetMaxTaxis(threadInfo.ForumID, threadInfo.TopLevel) + 1;
            threadInfo.LastDate = threadInfo.AddDate;
            threadInfo.LastUserName = threadInfo.UserName;
            threadInfo.LastPostID = 0;
            threadInfo.ThreadType = threadType;

            PostInfo postInfo = new PostInfo(publishmentSystemID);
            postInfo.PublishmentSystemID = publishmentSystemID;
            postInfo.ForumID = threadInfo.ForumID;
            postInfo.UserName = threadInfo.UserName;
            postInfo.Title = threadInfo.Title;
            postInfo.Content = content;
            postInfo.IsChecked = isChecked;
            postInfo.Taxis = 0;
            postInfo.AddDate = threadInfo.AddDate;
            postInfo.IPAddress = PageUtils.GetIPAddress();
            postInfo.IsSignature = isSignature;
            postInfo.IsAttachment = isAttachment;
            postInfo.IsThread = true;

            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            forumInfo.ThreadCount += 1;
            if (forumInfo.LastDate.DayOfYear != DateTime.Now.DayOfYear)//一天前
            {
                forumInfo.TodayThreadCount = 1;
                forumInfo.TodayPostCount = 0;
            }
            else
            {
                forumInfo.TodayThreadCount += 1;
            }
            forumInfo.LastTitle = title;
            forumInfo.LastUserName = threadInfo.UserName;
            forumInfo.LastDate = DateTime.Now;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    postInfo.ThreadID = this.InsertWithTrans(threadInfo, trans);

                    forumInfo.LastThreadID = postInfo.ThreadID;
                    forumInfo.LastPostID = DataProvider.PostDAO.InsertWithThread(publishmentSystemID, postInfo, trans);

                    DataProvider.ForumDAO.UpdateForumByInsertPost(publishmentSystemID, forumInfo, trans);

                    trans.Commit();
                }
            }

            postID = forumInfo.LastPostID;

            return postInfo.ThreadID;
        }

        public int ImportThread(ThreadInfo threadInfo)
        {
            int threadID = 0;

            threadInfo.Taxis = this.GetMaxTaxis(threadInfo.ForumID, threadInfo.TopLevel) + 1;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    threadInfo.BeforeExecuteNonQuery();
                    IDbDataParameter[] parms = null;
                    string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(threadInfo.Attributes, ThreadInfo.TableName, out parms);

                    this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                    threadID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, ThreadInfo.TableName);

                    trans.Commit();
                }
            }
            return threadID;
        }

        public void Update(ThreadInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, ThreadInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateIsLocked(string threadIDList, bool isLocked)
        {
            if (string.IsNullOrEmpty(threadIDList))
                return;
            string sqlString = string.Format("UPDATE bbs_Thread SET IsLocked='{0}' WHERE ID IN ({1})", isLocked.ToString(), threadIDList);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateIdentifyID(string threadIDList, int identifyID)
        {
            if (string.IsNullOrEmpty(threadIDList))
                return;
            string sqlString = string.Format("UPDATE bbs_Thread SET IdentifyID={0} WHERE ID IN ({1})", identifyID, threadIDList);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateThreadByInsertPost(int threadID, int lastPostID, string lastUserName, IDbTransaction trans)
        {
            string sqlString = string.Format("UPDATE {0} SET LastDate = {1}, LastPostID = {2}, LastUserName = '{3}',  Replies = Replies + 1 WHERE (ID = {4})", ThreadInfo.TableName, SqlUtils.GetDefaultDateString(base.DataBaseType), lastPostID, lastUserName, threadID);

            this.ExecuteNonQuery(trans, sqlString);
        }

        public void Delete(int publishmentSystemID, int forumID, List<int> threadIDList)
        {
            if (threadIDList == null || threadIDList.Count <= 0) return;

            //DataProvider.PostDAO.DeleteByThreadIDArrayList(threadIDArrayList);
            DataProvider.AttachmentDAO.DeleteByThreadIDList(publishmentSystemID, threadIDList);
            string sqlStringPost = string.Format("DELETE bbs_Post WHERE ThreadID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));
            int delPostCount = this.ExecuteNonQuery(sqlStringPost);

            string sqlString = string.Format("DELETE bbs_Thread WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));

            //this.ExecuteNonQuery(sqlString);
            int delThread = this.ExecuteNonQuery(sqlString);
            int relCount = delPostCount - delThread;
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                forumInfo.ThreadCount -= threadIDList.Count;
                forumInfo.PostCount -= relCount;
                if (threadIDList.Contains(forumInfo.LastThreadID))
                {
                    forumInfo.LastThreadID = 0;
                    forumInfo.LastPostID = 0;
                }
                DataProvider.ForumDAO.UpdateForumInfo(publishmentSystemID, forumInfo);
            }
            ThreadManager.RemoveCacheOfTop(publishmentSystemID);
        }

        public void DeleteAllByTrash(int publishmentSystemID)
        {
            DataProvider.AttachmentDAO.DeleteByThreadAll(publishmentSystemID);

            string sqlString = string.Format("DELETE FROM bbs_Thread WHERE ForumID < 0 AND PublishmentSystemID = {0}", publishmentSystemID);
            this.ExecuteNonQuery(sqlString);

            string postsqlString = string.Format("DELETE FROM bbs_Post WHERE ForumID < 0 AND PublishmentSystemID = {0}", publishmentSystemID);
            this.ExecuteNonQuery(postsqlString);
        }

        public void DeleteThreadTrash(int publishmentSystemID, int forumID, List<int> threadIDList)
        {
            if (threadIDList == null || threadIDList.Count <= 0) return;

            string sqlStringPost = string.Format("UPDATE bbs_Post SET ForumID=- ForumID WHERE ThreadID IN ({0}) AND ForumID > 0", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));
            int delPostCount = this.ExecuteNonQuery(sqlStringPost);

            string sqlString = string.Format("UPDATE bbs_Thread  SET ForumID=- ForumID  WHERE ID IN ({0})  AND ForumID > 0", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));

            int delThread = this.ExecuteNonQuery(sqlString);
            int relCount = delPostCount - delThread;

            ThreadManager.RemoveCacheOfTop(publishmentSystemID);

            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                forumInfo.ThreadCount -= threadIDList.Count;
                forumInfo.PostCount -= relCount;
                if (threadIDList.Contains(forumInfo.LastThreadID))
                {
                    forumInfo.LastThreadID = 0;
                    forumInfo.LastPostID = 0;
                }
                DataProvider.ForumDAO.UpdateForumInfo(publishmentSystemID, forumInfo);
            }
        }

        public void Restore(int publishmentSystemID, List<int> threadIDList)
        {
            if (threadIDList == null || threadIDList.Count <= 0) return;

            ArrayList froumList = new ArrayList();

            string sqlthreadString = string.Format("SELECT ForumID FROM bbs_Thread WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));
            using (IDataReader rdr = this.ExecuteReader(sqlthreadString))
            {
                while (rdr.Read())
                {
                    int forumID = (int)rdr.GetValue(0);
                    froumList.Add(forumID);
                }
                rdr.Close();
            }

            string sqlCountString = string.Format("SELECT COUNT(*) AS threadcount,forumID FROM bbs_Thread WHERE PublishmentSystemID = {0} AND ForumID < 0 AND ForumID IN ({1}) GROUP BY ForumID", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(froumList));
            using (IDataReader rdr = this.ExecuteReader(sqlCountString))
            {
                while (rdr.Read())
                {
                    int threadcount = (int)rdr.GetValue(0);
                    int forumID = (int)rdr.GetValue(1);
                    ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, -forumID);
                    if (forumInfo != null)
                    {
                        forumInfo.ThreadCount = forumInfo.ThreadCount + threadcount;
                        DataProvider.ForumDAO.UpdateForumInfo(publishmentSystemID, forumInfo);
                    }
                }
                rdr.Close();
            }

            ArrayList postCountList = new ArrayList();
            string sqlCountPostString = string.Format("SELECT COUNT(*) AS PostCount , ForumID FROM bbs_Post WHERE PublishmentSystemID = {0} AND  ThreadID IN ({1}) AND ForumID < 0   AND IsThread = 'False' GROUP BY ForumID", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));
            using (IDataReader rdr = this.ExecuteReader(sqlCountPostString))
            {
                while (rdr.Read())
                {
                    int postcount = (int)rdr.GetValue(0);
                    int forumID = (int)rdr.GetValue(1);
                    ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, -forumID);
                    if (forumInfo != null)
                    {
                        forumInfo.PostCount = forumInfo.PostCount + postcount;
                        DataProvider.ForumDAO.UpdateForumInfo(publishmentSystemID, forumInfo);
                    }
                }
                rdr.Close();
            }

            string sqlThreadString = string.Format("UPDATE bbs_Thread SET ForumID = -ForumID WHERE ID IN ({0}) AND ForumID < 0", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));
            int delPostCount = this.ExecuteNonQuery(sqlThreadString);
            string sqlPostString = string.Format("UPDATE bbs_Post SET ForumID = -ForumID WHERE ThreadID IN({0}) AND  ForumID < 0 ", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));
            this.ExecuteNonQuery(sqlPostString);

            ThreadManager.RemoveCacheOfTop(publishmentSystemID);
        }

        public void AllRestore(int publishmentSystemID)
        {
            ArrayList postList = new ArrayList();
            string sqlThread = string.Format("SELECT ID FROM bbs_Thread WHERE ForumID < 0 AND PublishmentSystemID = {0}", publishmentSystemID);
            using (IDataReader rdr = this.ExecuteReader(sqlThread))
            {
                while (rdr.Read())
                {
                    int threadID = (int)rdr.GetValue(0);
                    postList.Add(threadID);
                }
                rdr.Close();
            }

            string sqlthreadString = string.Format("SELECT COUNT(*) AS ThreadCount, ForumID FROM bbs_Thread WHERE ForumID < 0 AND PublishmentSystemID = {0} GROUP BY ForumID", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlthreadString))
            {
                while (rdr.Read())
                {
                    int count = (int)rdr.GetValue(0);
                    int forumID = (int)rdr.GetValue(1);
                    ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, -forumID);
                    if (forumInfo != null)
                    {
                        forumInfo.ThreadCount = forumInfo.ThreadCount + count;
                        DataProvider.ForumDAO.UpdateForumInfo(publishmentSystemID, forumInfo);
                    }
                }
                rdr.Close();
            }
            string postString = string.Format("SELECT COUNT(*) AS PostCount, ForumID FROM bbs_Post WHERE PublishmentSystemID = {0} AND ThreadID IN ({1}) AND IsThread = 'False' GROUP BY ForumID", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(postList));
            using (IDataReader rdr = this.ExecuteReader(postString))
            {
                while (rdr.Read())
                {
                    int count = (int)rdr.GetValue(0);
                    int forumID = (int)rdr.GetValue(1);
                    ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, -forumID);
                    if (forumInfo != null)
                    {
                        forumInfo.PostCount = forumInfo.PostCount + count;
                        DataProvider.ForumDAO.UpdateForumInfo(publishmentSystemID, forumInfo);
                    }
                }
                rdr.Close();
            }

            string sqlString = string.Format("UPDATE bbs_Thread SET ForumID = -ForumID WHERE ForumID < 0 AND PublishmentSystemID = {0}", publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
            string sqlPostString = string.Format("UPDATE bbs_Post SET ForumID = -ForumID WHERE ForumID < 0 AND PublishmentSystemID = {0}", publishmentSystemID);
            this.ExecuteNonQuery(sqlPostString);

            ThreadManager.RemoveCacheOfTop(publishmentSystemID);
        }

        public ThreadInfo GetThreadInfo(int publishmentSystemID, int threadID)
        {
            ThreadInfo info = null;
            if (threadID > 0)
            {
                string SQL_WHERE = string.Format("WHERE ID = {0}", threadID);
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ThreadInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                {
                    if (rdr.Read())
                    {
                        info = new ThreadInfo(publishmentSystemID);
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    }
                    rdr.Close();
                }
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public DateTime GetAddDate(int threadID)
        {
            DateTime addDate = DateTime.Now;
            string sqlString = "SELECT AddDate FROM bbs_Thread WHERE ID =" + threadID;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    addDate = rdr.GetDateTime(0);
                }
                rdr.Close();
            }

            return addDate;
        }

        public int GetReplies(int threadID)
        {
            int replies = 0;
            string sqlString = "SELECT Replies FROM bbs_Thread WHERE ID =" + threadID;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    replies = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return replies;
        }

        public string GetTitle(int threadID)
        {
            string title = string.Empty;
            string sqlString = "SELECT Title FROM bbs_Thread WHERE ID =" + threadID;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    title = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return title;
        }

        public int GetCategoryID(int threadID)
        {
            int categoryID = 0;
            string sqlString = "SELECT CategoryID FROM bbs_Thread WHERE ID =" + threadID;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    categoryID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return categoryID;
        }

        public int GetThreadCount(int publishmentSystemID, int forumID)
        {
            int count = 0;

            string sqlString = string.Format("SELECT COUNT(*) FROM bbs_Thread WHERE PublishmentSystemID = {0} AND ForumID = {1}", publishmentSystemID, forumID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    count = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return count;
        }

        public int GetForumID(int threadID)
        {
            int forumID = 0;
            string sqlString = "SELECT ForumID FROM bbs_Thread WHERE ID =" + threadID;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    forumID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return forumID;
        }

        public void AddHits(int threadID)
        {
            if (threadID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET Hits = Hits + 1 WHERE ID = {1}", ThreadInfo.TableName, threadID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public int GetThreadID(int publishmentSystemID, int forumID, int taxis, bool isNextThread)
        {
            int threadID = 0;
            string sqlString = string.Empty;
            if (isNextThread)
            {
                sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE PublishmentSystemID = {1} AND ForumID = {2} AND Taxis < {3} AND IsChecked = 'True' ORDER BY Taxis DESC", ThreadInfo.TableName, publishmentSystemID, forumID, taxis);
            }
            else
            {
                sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE PublishmentSystemID = {1} AND ForumID = {2} AND Taxis > {3} AND IsChecked = 'True' ORDER BY Taxis", ThreadInfo.TableName, publishmentSystemID, forumID, taxis);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        threadID = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return threadID;
        }

        public string GetValue(int threadID, string name)
        {
            string sqlString = string.Format("SELECT [{0}] FROM [{1}] WHERE ([ID] = {2})", name, ThreadInfo.TableName, threadID);
            return BaiRongDataProvider.DatabaseDAO.GetString(sqlString);
        }

        public ArrayList GetThreadIDArrayListChecked(int publishmentSystemID, int forumID, bool isAllChildren, int totalNum, string whereString)
        {
            ArrayList arraylist = new ArrayList();

            ArrayList forumIDArrayList = new ArrayList();
            forumIDArrayList.Add(forumID);
            if (isAllChildren)
            {
                forumIDArrayList.AddRange(DataProvider.ForumDAO.GetForumIDArrayListForDescendant(publishmentSystemID, forumID));
            }

            string sqlString = string.Empty;

            if (totalNum > 0)
            {
                sqlString = string.Format("SELECT TOP {0} ID FROM {1} WHERE PublishmentSystemID = {2} AND ForumID IN ({3}) AND IsChecked = '{4}' {5} ORDER BY Taxis DESC", totalNum, ThreadInfo.TableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(forumIDArrayList), true.ToString(), whereString);
            }
            else
            {
                sqlString = string.Format("SELECT ID FROM {0} WHERE PublishmentSystemID = {1} AND ForumID IN ({2}) AND IsChecked = '{3}' {4} ORDER BY Taxis DESC", ThreadInfo.TableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(forumIDArrayList), true.ToString(), whereString);
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

        public DataSet GetDataSource(int publishmentSystemID, string strWhere)
        {

            StringBuilder sqlString = new StringBuilder();
            sqlString.AppendFormat("select ID, PublishmentSystemID, AreaID, ForumID, IconID, UserName, Title, AddDate, LastDate, LastPostID, LastUserName, Hits, Replies, Taxis, IsChecked, IsLocked, IsImage, IsAttachment, CategoryID, TopLevel, TopLevelDate, DigestLevel, DigestDate, Highlight, HighlightDate, IdentifyID, ThreadType FROM bbs_Thread WHERE PublishmentSystemID = {0}", publishmentSystemID);
            if (!string.IsNullOrEmpty(strWhere))
            {
                sqlString.Append(" AND " + strWhere);
            }
            sqlString.Append(" order by Taxis desc");

            return this.ExecuteDataset(sqlString.ToString());
        }

        public string GetSqlString(int publishmentSystemID)
        {
            return string.Format("select ID, PublishmentSystemID, Title, ForumID, UserName, Replies, Hits, LastDate FROM bbs_Thread WHERE ForumID > 0 AND PublishmentSystemID = {0}", publishmentSystemID);
        }
        public string GetSqlString(int publishmentSystemID, string userName, string title, string dateFrom, string dateTo, string forumID)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("SELECT ID, PublishmentSystemID, Title, ForumID, UserName, Replies, Hits, LastDate FROM bbs_Thread WHERE ForumID > 0 AND PublishmentSystemID = {0}", publishmentSystemID);
            if (!string.IsNullOrEmpty(userName) && userName != "请输入用户名")
            {
                //sb.Append(" AND UserName='" + userName + "'");
                //by 20151201 sofuny
                sb.Append(" AND UserName like '%" + userName + "%'");
            }
            if (!string.IsNullOrEmpty(title) && title != "输入标题关键字")
            {
                sb.Append(" AND Title like '%" + title + "%'");
            }
            if (!string.IsNullOrEmpty(dateFrom))
            {
                sb.Append(" AND AddDate >='" + TranslateUtils.ToDateTime(dateFrom) + "'");
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                sb.Append(" AND AddDate<='" + TranslateUtils.ToDateTime(dateTo) + "'");
            }
            if (!string.IsNullOrEmpty(forumID) && forumID != "0")
            {
                sb.Append("AND ForumID=" + TranslateUtils.ToInt(forumID) + "");
                ArrayList array = DataProvider.ForumDAO.GetForumIDArrayListByParentID(publishmentSystemID, TranslateUtils.ToInt(forumID));
                foreach (int childForumID in array)
                {
                    sb.Append(" OR ForumID=" + childForumID + "");
                }
            }
            sb.Append(" ORDER BY ID DESC");
            return sb.ToString();
        }
        public string GetSqlTrashString(int publishmentSystemID)
        {
            return string.Format("select ID, PublishmentSystemID, Title, ForumID, UserName, LastDate FROM bbs_Thread WHERE ForumID < 0 AND PublishmentSystemID = {0} order by LastDate desc", publishmentSystemID);
        }

        /// <summary>
        /// 版块合并
        /// </summary>
        /// <param name="sourceForumID">需要合并版块</param>
        /// <param name="targetForumID">合并到的目标版块</param>
        public void TranslateThreadByForumID(int publishmentSystemID, int sourceForumID, int targetForumID)
        {
            if (targetForumID <= 0 || sourceForumID <= 0)
                return;
            string sqlString1 = string.Format("Update bbs_Thread set ForumID = {0} WHERE ForumID = {1}", targetForumID, sourceForumID);
            string sqlString2 = string.Format("Update bbs_Post set ForumID = {0} WHERE ForumID = {1}", targetForumID, sourceForumID);
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    this.ExecuteNonQuery(trans, sqlString2);
                    this.ExecuteNonQuery(trans, sqlString1);
                    trans.Commit();
                }
            }

            DataProvider.ForumDAO.UpdateCount(publishmentSystemID, sourceForumID);
            DataProvider.ForumDAO.UpdateCount(publishmentSystemID, targetForumID);
        }

        /// <summary>
        /// 主题转移
        /// </summary>
        /// <param name="sourceForumID">需要转移的主题版块</param>
        /// <param name="targetForumID">转移到的目标版块</param>
        public void TranslateThread(int publishmentSystemID, string threadIDList, int targetForumID)
        {

            if (targetForumID <= 0 || string.IsNullOrEmpty(threadIDList))
                return;
            string sqlString1 = string.Format("Update bbs_Thread set ForumID={0} WHERE ID IN ({1})", targetForumID, threadIDList);
            string sqlString2 = string.Format("Update bbs_Post set ForumID={0} WHERE ThreadID IN ({1})", targetForumID, threadIDList);
            // 缺少更新bbs_forums之threadcount数据
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    this.ExecuteNonQuery(trans, sqlString2);
                    this.ExecuteNonQuery(trans, sqlString1);
                    trans.Commit();
                }
            }
            ForumManager.RemoveCache(publishmentSystemID);
        }

        public void CategoryThread(string threadIDList, int categoryID)
        {
            if (string.IsNullOrEmpty(threadIDList))
                return;
            string sqlString = string.Format("UPDATE bbs_Thread SET CategoryID={0} WHERE ID IN ({1})", categoryID, threadIDList);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpDownThread(string threadIDList, bool isUp, int forumID)
        {
            if (string.IsNullOrEmpty(threadIDList))
                return;
            string sqlString = string.Empty;
            if (isUp == false)
            {
                DateTime lastDate = DateUtils.SqlMinValue;
                sqlString = string.Format("SELECT TOP 1 LastDate FROM bbs_Thread WHERE ForumID = {0} ORDER BY LastDate", forumID);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        lastDate = rdr.GetDateTime(0);
                    }
                    rdr.Close();
                }

                if (lastDate != DateUtils.SqlMinValue)
                {
                    lastDate = lastDate.AddHours(-1);
                    sqlString = string.Format("UPDATE bbs_Thread SET LastDate='{0}' WHERE ID IN ({1})", DateUtils.GetDateAndTimeString(lastDate.AddHours(-1)), threadIDList);
                    this.ExecuteNonQuery(sqlString);
                }
            }
            else
            {
                sqlString = string.Format("UPDATE bbs_Thread SET LastDate=getdate() WHERE ID IN ({0})", threadIDList);
                this.ExecuteNonQuery(sqlString);
            }
        }

        /// <summary>
        /// 设置主题置顶级别和时间
        /// </summary>
        /// <param name="threadIDList"></param>
        /// <param name="topLevelDate"></param>
        /// <param name="topLevel"></param>
        public void TopLevelThread(string threadIDList, DateTime topLevelDate, int topLevel)
        {

            if (topLevel < 0)
                topLevel = 1;
            if (string.IsNullOrEmpty(threadIDList))
                return;
            string sqlString = string.Format("UPDATE bbs_Thread SET TopLevel={0}, TopLevelDate='{1}' WHERE ID IN ({2})", topLevel, topLevelDate.ToString("yyyy-MM-dd HH:mm:ss"), threadIDList);

            this.ExecuteNonQuery(sqlString);
        }

        /// <summary>
        /// 设置主题精华级别和时间
        /// </summary>
        /// <param name="threadIDList"></param>
        /// <param name="topLevelDate"></param>
        /// <param name="topLevel"></param>
        public void DigestThread(string threadIDList, DateTime digestDate, int digest)
        {

            if (digest < 0)
                digest = 1;
            if (string.IsNullOrEmpty(threadIDList))
                return;
            string sqlString = string.Format("UPDATE bbs_Thread SET DigestLevel={0}, DigestDate='{1}' WHERE ID IN ({2})", digest, digestDate.ToString("yyyy-MM-dd HH:mm:ss"), threadIDList);

            this.ExecuteNonQuery(sqlString);
        }

        public void HighlightThreads(int publishmentSystemID, int areaID, int forumID, string threadIDList, bool isTopExists, int topLevel, string topLevelDate, bool isDigestExists, int digestLevel, string digestDate, bool isHighlightExists, string highlight, string highlightDate)
        {
            if (isTopExists || isDigestExists || isHighlightExists)
            {
                StringBuilder setBuilder = new StringBuilder();

                if (isTopExists)
                {
                    setBuilder.AppendFormat(" TopLevel = {0},", topLevel);
                    if (!string.IsNullOrEmpty(topLevelDate))
                    {
                        setBuilder.AppendFormat(" TopLevelDate = {0},", topLevelDate);
                    }
                }
                if (isDigestExists)
                {
                    setBuilder.AppendFormat(" DigestLevel = {0},", digestLevel);
                    if (!string.IsNullOrEmpty(digestDate))
                    {
                        setBuilder.AppendFormat(" DigestDate = {0},", digestDate);
                    }
                }
                if (isHighlightExists)
                {
                    setBuilder.AppendFormat(" Highlight = '{0}',", highlight);
                    if (!string.IsNullOrEmpty(highlightDate))
                    {
                        setBuilder.AppendFormat(" HighlightDate = {0},", highlightDate);
                    }
                }
                string sqlString = string.Format("UPDATE bbs_Thread SET {0} WHERE ID IN ({1}) AND PublishmentSystemID = {2}", setBuilder.ToString(0, setBuilder.Length - 1), threadIDList, publishmentSystemID);

                ThreadManager.RemoveCacheOfTop(publishmentSystemID);

                this.ExecuteNonQuery(sqlString);
            }
        }

        public ArrayList GetTopLevelThreadInfoArrayList(int publishmentSystemID, int topLevel, int areaID, int forumID)
        {
            string sqlWhereString = string.Empty;
            if (topLevel == 3)
            {
                sqlWhereString = "WHERE (IsChecked = 'True' AND TopLevel = 3 AND ForumID>0)";
            }
            else if (topLevel == 2)
            {
                sqlWhereString = string.Format("WHERE (ForumID>0  AND IsChecked = 'True' AND TopLevel = 2 AND AreaID = {0})", areaID);
            }
            else
            {
                sqlWhereString = string.Format("WHERE (ForumID>0  AND IsChecked = 'True' AND TopLevel = 1 AND ForumID = {0})", forumID);
            }

            sqlWhereString += string.Format(" AND PublishmentSystemID = {0}", publishmentSystemID);

            ArrayList arraylist = new ArrayList();
            string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ThreadInfo.TableName, 0, SqlUtils.Asterisk, sqlWhereString, EBBSTaxisTypeUtils.GetOrderByString(EContextType.Thread, EBBSTaxisType.OrderByLastDate));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    ThreadInfo info = new ThreadInfo(publishmentSystemID);
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    info.AfterExecuteReader();
                    arraylist.Add(info);

                }
                rdr.Close();
            }

            return arraylist;
        }

        public string GetParserWhereString(int publishmentSystemID, int categoryID, string type, bool isTopExists, bool isTop, string where)
        {
            StringBuilder whereStringBuilder = new StringBuilder();
            whereStringBuilder.AppendFormat(" AND ForumID > 0 AND PublishmentSystemID = {0} ", publishmentSystemID);
            if (isTopExists)
            {
                if (isTop)
                {
                    whereStringBuilder.Append(" AND TopLevel > 0 ");
                }
                else
                {
                    whereStringBuilder.Append(" AND TopLevel = 0 ");
                }
            }

            if (categoryID > 0)
            {
                whereStringBuilder.AppendFormat(" AND CategoryID = {0} ", categoryID);
            }

            if (!string.IsNullOrEmpty(type))
            {
                if (StringUtils.EqualsIgnoreCase(type, "digest"))
                {
                    whereStringBuilder.Append(" AND DigestLevel > 0 ");
                }
                else if (StringUtils.EqualsIgnoreCase(type, "image"))
                {
                    whereStringBuilder.Append(" AND IsImage = 'True' ");
                }
            }

            if (!string.IsNullOrEmpty(where))
            {
                whereStringBuilder.AppendFormat(" AND ({0}) ", where);
            }

            return whereStringBuilder.ToString();
        }

        public IEnumerable GetParserDataSourceChecked(int publishmentSystemID, int forumID, int startNum, int totalNum, string orderByString, string whereString, bool isAllChildren)
        {
            string sqlWhereString = string.Empty;
            if (forumID == 0 && isAllChildren)
            {
                sqlWhereString = string.Format("WHERE PublishmentSystemID = {0} AND IsChecked = '{1}' {2}", publishmentSystemID, true.ToString(), whereString);
            }
            else
            {
                ArrayList forumIDArrayList = new ArrayList();
                forumIDArrayList.Add(forumID);
                if (isAllChildren)
                {
                    forumIDArrayList.AddRange(DataProvider.ForumDAO.GetForumIDArrayListForDescendant(publishmentSystemID, forumID));
                }

                sqlWhereString = string.Format("WHERE (ForumID IN ({0}) AND IsChecked = '{1}' {2})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(forumIDArrayList), true.ToString(), whereString);
            }

            if (startNum <= 1)
            {
                return GetDataSourceByContentNumAndWhereString(totalNum, sqlWhereString, orderByString);
            }
            else
            {
                return GetDataSourceByStartNum(startNum, totalNum, sqlWhereString, orderByString);
            }
        }

        public int GetParserDataSourceCheckedCount(int publishmentSystemID, int forumID, string whereString, bool isAllChildren)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE PublishmentSystemID = {1} ", ThreadInfo.TableName, publishmentSystemID);
            if (forumID == 0 && isAllChildren)
            {
                sqlString += string.Format(" AND IsChecked = '{0}' {1}", true.ToString(), whereString);
            }
            else
            {
                ArrayList forumIDArrayList = new ArrayList();
                forumIDArrayList.Add(forumID);
                if (isAllChildren)
                {
                    forumIDArrayList.AddRange(DataProvider.ForumDAO.GetForumIDArrayListForDescendant(publishmentSystemID, forumID));
                }

                sqlString += string.Format(" AND (ForumID IN ({0}) AND IsChecked = '{1}' {2})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(forumIDArrayList), true.ToString(), whereString);
            }

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private IEnumerable GetDataSourceByContentNumAndWhereString(int totalNum, string whereString, string orderByString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ThreadInfo.TableName, totalNum, SqlUtils.Asterisk, whereString, orderByString);
            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        private IEnumerable GetDataSourceByStartNum(int startNum, int totalNum, string whereString, string orderByString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ThreadInfo.TableName, startNum, totalNum, SqlUtils.Asterisk, whereString, orderByString);
            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        public int GetMaxTaxis(int forumID, int topLevel)
        {
            int maxTaxis = 0;

            string sqlString = string.Format("SELECT MAX(Taxis) FROM {0} WHERE ForumID = {1} AND TopLevel = {2}", ThreadInfo.TableName, forumID, topLevel);
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

            return maxTaxis;
        }

        public int GetTaxis(int selectedID)
        {
            string sqlString = string.Format("SELECT Taxis FROM {0} WHERE (ID = {1})", ThreadInfo.TableName, selectedID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public void SetTaxis(int id, int taxis)
        {
            string sqlString = string.Format("UPDATE {0} SET Taxis = {1} WHERE ID = {2}", ThreadInfo.TableName, taxis, id);
            this.ExecuteNonQuery(sqlString);
        }

        public int GetToadyThreadCount(int publishmentSystemID, string userName,DateTime fromTime,DateTime toTime)
        {
            int count = 0;
            string sqlString = string.Format("SELECT COUNT(ID) FROM bbs_Thread WHERE PublishmentSystemID = {0} AND UserName = '{1}' AND AddDate >= '{2}' AND AddDate <= '{3}' AND ThreadType = '{4}'", publishmentSystemID, userName, fromTime, toTime, "Post");
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    count = rdr.GetInt32(0);
                }
                rdr.Close();
            }
            return count;
        }
    }
}
