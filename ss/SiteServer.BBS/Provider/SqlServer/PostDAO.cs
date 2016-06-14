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
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class PostDAO : DataProviderBase, IPostDAO
    {
        public int InsertWithThread(int publishmentSystemID, PostInfo info, IDbTransaction trans)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, PostInfo.TableName, out parms);

            this.ExecuteNonQuery(trans, SQL_INSERT, parms);

            int postID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, PostInfo.TableName);

            int creditMultiplierPostCount = ConfigurationManager.GetAdditional(publishmentSystemID).CreditMultiplierPostCount;

            DataProvider.BBSUserDAO.AddPostCount(PublishmentSystemManager.GetGroupSN(publishmentSystemID), info.UserName, info.AddDate, creditMultiplierPostCount, trans);

            return postID;
        }

        public int InsertPostOnly(int publishmentSystemID, PostInfo info)
        {
            int postID = 0;

            info.Taxis = this.GetMaxTaxis(info.PublishmentSystemID, info.ThreadID) + 1;

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, PostInfo.TableName, out parms);

            ForumInfo forumInfo = ForumManager.GetForumInfo(info.PublishmentSystemID, info.ForumID);
            forumInfo.PostCount += 1;
            if (forumInfo.LastDate.DayOfYear != DateTime.Now.DayOfYear)//一天前
            {
                forumInfo.TodayThreadCount = 0;
                forumInfo.TodayPostCount = 1;
            }
            else
            {
                forumInfo.TodayPostCount += 1;
            }
            forumInfo.LastTitle = info.Title;
            forumInfo.LastUserName = info.UserName;
            forumInfo.LastDate = DateTime.Now;
            forumInfo.LastThreadID = info.ThreadID;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        postID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, PostInfo.TableName);

                        int creditMultiplierPostCount = ConfigurationManager.GetAdditional(publishmentSystemID).CreditMultiplierPostCount;
                        DataProvider.BBSUserDAO.AddPostCount(PublishmentSystemManager.GetGroupSN(publishmentSystemID), info.UserName, info.AddDate, creditMultiplierPostCount, trans);

                        DataProvider.ThreadDAO.UpdateThreadByInsertPost(info.ThreadID, postID, info.UserName, trans);

                        forumInfo.LastPostID = postID;
                        DataProvider.ForumDAO.UpdateForumByInsertPost(publishmentSystemID, forumInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return postID;
        }

        public int ImportPost(int publishmentSystemID, PostInfo info)
        {
            int postID = 0;
            info.Taxis = this.GetMaxTaxis(info.PublishmentSystemID, info.ThreadID) + 1;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    info.BeforeExecuteNonQuery();
                    IDbDataParameter[] parms = null;
                    string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, PostInfo.TableName, out parms);

                    this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                    postID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, PostInfo.TableName);

                    trans.Commit();
                }
            }
            return postID;
        }

        public void Update(int publishmentSystemID, PostInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, PostInfo.TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateContent(int publishmentSystemID, int postID, string content)
        {
            string sqlString = string.Format("UPDATE {0} SET Content = @Content WHERE ID = {1}", PostInfo.TableName, postID);
            IDbDataParameter[] param = new IDbDataParameter[] { 
                this.GetParameter("@Content", EDataType.NText, content)
            };
            this.ExecuteNonQuery(sqlString, param);
        }

        public void Handle(int publishmentSystemID, List<int> threadIDList)
        {
            if (threadIDList != null && threadIDList.Count > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET isHandled = '{1}' WHERE ThreadID IN ({2})", PostInfo.TableName, true.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));

                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, int forumID, int threadID, List<int> postIDList)
        {
            if (postIDList == null || postIDList.Count <= 0) return;

            DataProvider.AttachmentDAO.DeleteByPostIDList(publishmentSystemID, postIDList);

            string sqlString = string.Format("DELETE bbs_Post WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(postIDList));
            this.ExecuteNonQuery(sqlString);
        }

        public void DeleteAll(int publishmentSystemID)
        {
            string sqlString = string.Format("DELETE bbs_Post WHERE PublishmentSystemID = {0} AND ForumID < 0", publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
        }

        public void DeletePostTrash(int publishmentSystemID, int forumID, int threadID, List<int> postIDList)
        {
            if (postIDList == null || postIDList.Count <= 0) return;
            string sqlString = string.Format("UPDATE bbs_Post SET ForumID = -ForumID WHERE ID IN ({0}) AND ForumID > 0", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(postIDList));
            this.ExecuteNonQuery(sqlString);

            ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(publishmentSystemID, threadID);
            if (threadInfo != null)
            {
                threadInfo.Replies -= postIDList.Count;
                if (postIDList.Contains(threadInfo.LastPostID))
                {
                    threadInfo.LastPostID = 0;
                }
                DataProvider.ThreadDAO.Update(threadInfo);
            }

            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                forumInfo.PostCount -= postIDList.Count;
                if (postIDList.Contains(forumInfo.LastPostID))
                {
                    forumInfo.LastPostID = 0;
                }
                DataProvider.ForumDAO.UpdateForumInfo(publishmentSystemID, forumInfo);
            }
            
        }

        public void DeleteByThreadIDList(int publishmentSystemID, List<int> threadIDList)
        {
            if (threadIDList != null && threadIDList.Count > 0)
            {
                DataProvider.AttachmentDAO.DeleteByThreadIDList(publishmentSystemID, threadIDList);
                string sqlString = string.Format("DELETE bbs_Post WHERE ThreadID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteSingleThreadPostTrash(int publishmentSystemID, int forumID, List<int> threadIDList)
        {
            if (threadIDList != null && threadIDList.Count > 0)
            {
                string sqlStringPost = string.Format("UPDATE bbs_Post SET ForumID = -ForumID WHERE threadID IN ({0}) AND ForumID > 0 AND IsThread = 'True'", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));
                int delPostCount = this.ExecuteNonQuery(sqlStringPost);
                ThreadManager.RemoveCacheOfTop(publishmentSystemID);
            }
        }

        public void Restore(int publishmentSystemID, List<int> postIDList)
        {
            if (postIDList != null && postIDList.Count > 0)
            {
                ArrayList threadList = new ArrayList();
                ArrayList forumList = new ArrayList();
                string sqlPostString = string.Format("SELECT ThreadID, ForumID FROM bbs_Post WHERE PublishmentSystemID = {0} AND ID IN ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(postIDList));
                using (IDataReader rdr = this.ExecuteReader(sqlPostString))
                {
                    while (rdr.Read())
                    {
                        int threadID = (int)rdr.GetValue(0);
                        int forumID = (int)rdr.GetValue(1);
                        threadList.Add(threadID);
                        forumList.Add(forumID);
                    }
                    rdr.Close();
                }

                string sqlCountPostString = string.Format("SELECT COUNT(*) AS ThreadCount,ThreadID FROM bbs_Post WHERE PublishmentSystemID = {0} AND ForumID < 0 AND ThreadID IN ({1}) AND IsThread = 'False' GROUP BY ThreadID", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadList));
                using (IDataReader rdr = this.ExecuteReader(sqlCountPostString))
                {
                    while (rdr.Read())
                    {
                        int threadCount = (int)rdr.GetValue(0);
                        int threadID = (int)rdr.GetValue(1);
                        ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(publishmentSystemID, threadID);
                        if (threadInfo != null)
                        {
                            threadInfo.Replies = threadInfo.Replies + threadCount;
                            DataProvider.ThreadDAO.Update(threadInfo);
                        }
                    }
                    rdr.Close();
                }

                string sqlCountForumString = string.Format("SELECT COUNT(*) AS ForumCount, ForumID FROM bbs_Post WHERE PublishmentSystemID = {0} AND ForumID < 0 AND ForumID IN ({1}) AND IsThread = 'False' GROUP BY ForumID", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(forumList));
                using (IDataReader rdr = this.ExecuteReader(sqlCountForumString))
                {
                    while (rdr.Read())
                    {
                        int forumCount = (int)rdr.GetValue(0);
                        int forumID = (int)rdr.GetValue(1);
                        ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, -forumID);
                        if (forumInfo != null)
                        {
                            forumInfo.PostCount = forumInfo.PostCount + forumCount;
                            DataProvider.ForumDAO.UpdateForumInfo(publishmentSystemID, forumInfo);
                        }
                    }
                    rdr.Close();
                }

                string sqlString = string.Format("UPDATE bbs_Post SET ForumID = -ForumID WHERE ID IN ({0}) AND ForumID < 0", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(postIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void AllRestore(int publishmentSystemID)
        {
            string sqlForumString = string.Format("SELECT COUNT(*) AS FourmCount, ForumID FROM bbs_Post WHERE PublishmentSystemID = {0} AND ForumID < 0 AND IsThread = 'False' GROUP BY ForumID", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlForumString))
            {
                while (rdr.Read())
                {
                    int postCount = (int)rdr.GetValue(0);
                    int forumID = (int)rdr.GetValue(1);
                    ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, -forumID);
                    if (forumInfo != null)
                    {
                        forumInfo.PostCount = forumInfo.PostCount + postCount;
                        DataProvider.ForumDAO.UpdateForumInfo(publishmentSystemID, forumInfo);
                    }
                }
                rdr.Close();
            }

            string sqlThreadString = string.Format("SELECT COUNT(*) AS ThreadCount,ThreadID FROM bbs_Post WHERE PublishmentSystemID = {0} AND ForumID < 0 AND IsThread = 'False' GROUP BY ThreadID", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlThreadString))
            {
                while (rdr.Read())
                {
                    int postCount = (int)rdr.GetValue(0);
                    int threadID = (int)rdr.GetValue(1);
                    ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(publishmentSystemID, threadID);
                    if (threadInfo != null)
                    {
                        threadInfo.Replies = threadInfo.Replies + postCount;
                        DataProvider.ThreadDAO.Update(threadInfo);
                    }

                }
                rdr.Close();
            }

            string sqlString = string.Format("UPDATE bbs_Post SET ForumID=-ForumID WHERE PublishmentSystemID = {0} AND ForumID < 0", publishmentSystemID);

            this.ExecuteNonQuery(sqlString);
        }

        public void Ban(int publishmentSystemID, int forumID, List<int> postIDList, bool isBanned)
        {
            if (postIDList == null || postIDList.Count <= 0) return;

            string sqlString = string.Format("UPDATE bbs_Post SET IsBanned = '{0}' WHERE ID IN ({1})", isBanned.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(postIDList));
            this.ExecuteNonQuery(sqlString);
        }

        public void BanThreadIDList(int publishmentSystemID, int forumID, List<int> threadIDList, bool isBanned)
        {
            if (threadIDList == null || threadIDList.Count <= 0) return;

            string sqlString = string.Format("UPDATE bbs_Post SET IsBanned = '{0}' WHERE ThreadID IN ({1}) AND IsThread = 'True'", isBanned.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDList));
            this.ExecuteNonQuery(sqlString);
        }

        public PostInfo GetPostInfo(int publishmentSystemID, int postID)
        {
            PostInfo info = null;
            if (postID > 0)
            {
                string SQL_WHERE = string.Format("WHERE ID = {0}", postID);
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(PostInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                {
                    if (rdr.Read())
                    {
                        info = new PostInfo(publishmentSystemID);
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    }
                    rdr.Close();
                }
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public int GetPostIDByTaxis(int publishmentSystemID, int forumID, int threadID, int taxis)
        {
            string sqlString = string.Format("SELECT ID FROM [{0}] WHERE (ForumID = {1} AND ThreadID = {2} AND Taxis = {3})", PostInfo.TableName, forumID, threadID, taxis);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetValue(int publishmentSystemID, int postID, string name)
        {
            string sqlString = string.Format("SELECT [{0}] FROM [{1}] WHERE ([ID] = {2})", name, PostInfo.TableName, postID);
            return BaiRongDataProvider.DatabaseDAO.GetString(sqlString);
        }

        public string GetThreadValue(int publishmentSystemID, int threadID, string name)
        {
            string sqlString = string.Format("SELECT [{0}] FROM [{1}] WHERE ([ThreadID] = {2} AND IsThread = 'True')", name, PostInfo.TableName, threadID);
            return BaiRongDataProvider.DatabaseDAO.GetString(sqlString);
        }

        public string GetSqlString(int publishmentSystemID, string userName, string title, string dateFrom, string dateTo,string forumID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT ID, PublishmentSystemID, ThreadID, ForumID, UserName, LastEditUserName, LastEditDate, Title, Content, Taxis, IsChecked, Assessor, CheckDate, AddDate, IPAddress, IsThread, IsBanned, IsAnonymous, IsHtml, IsBBCodeOff, IsSmileyOff, IsUrlOff, IsSignature, IsAttachment, IsHandled, State FROM bbs_Post WHERE ForumID > 0 AND PublishmentSystemID = {0}", publishmentSystemID);

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
            if (!string.IsNullOrEmpty(forumID) && forumID!="0")
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

        public string GetSqlStringTrash(int publishmentSystemID, string userName, string title, string dateFrom, string dateTo)
        {
            StringBuilder sb = new StringBuilder();
            ArrayList list = new ArrayList();
            ArrayList listpost = new ArrayList();

            string sqlString = string.Format("SELECT ThreadID FROM bbs_Post WHERE ForumID < 0 AND PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int threadid =(int)rdr.GetValue(0);
                    list.Add(threadid);
                }
                rdr.Close();
            }

            string sqlPostString = string.Format("SELECT ID,ForumID FROM bbs_Thread WHERE PublishmentSystemID = {0} AND ID IN ({1}) ", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(list));
            using (IDataReader rdr = this.ExecuteReader(sqlPostString))
            {
                while (rdr.Read())
                {
                    int id = (int)rdr.GetValue(0);
                    int forumID = (int)rdr.GetValue(1);
                    
                    if (forumID > 0)
                    {
                        listpost.Add(id);
                    }
                }
                rdr.Close();
            }

            sb.Append(string.Format("SELECT ID, PublishmentSystemID, ThreadID, ForumID, UserName, LastEditUserName, LastEditDate, Title, Content, Taxis, IsChecked, Assessor, CheckDate, AddDate, IPAddress, IsThread, IsBanned, IsAnonymous, IsHtml, IsBBCodeOff, IsSmileyOff, IsUrlOff, IsSignature, IsAttachment, IsHandled, State FROM bbs_Post WHERE PublishmentSystemID = {0} AND ForumID < 0 AND ThreadID IN ({1})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(listpost)));

            if (!string.IsNullOrEmpty(userName))
            {
                sb.Append(" AND UserName='" + userName + "'");
            }
            if (!string.IsNullOrEmpty(title))
            {
                sb.Append(" AND Title like '%" + title + "%'");
            }
            if (!string.IsNullOrEmpty(dateFrom))
            {
                DateTime dateFromDate = ConvertHelper.GetDateTime(dateFrom);
                sb.Append(" AND AddDate >='" + dateFromDate + "'");
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                DateTime dateToDate = ConvertHelper.GetDateTime(dateTo);
                sb.Append(" AND AddDate<='" + dateToDate + "'");
            }
            sb.Append(" ORDER BY ID DESC");

            return sb.ToString();
        }

        public bool IsReply(int publishmentSystemID, int threadID, string userName)
        {
            bool isReply = false;

            string sqlString = string.Format("SELECT ID FROM [{0}] WHERE PublishmentSystemID = {1} AND [ThreadID] = {2} AND UserName = '{3}'", PostInfo.TableName, publishmentSystemID, threadID, userName);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    isReply = true;
                }
                rdr.Close();
            }

            return isReply;
        }

        public IEnumerable GetParserDataSource(int publishmentSystemID, int threadID, int startNum, int totalNum, bool isThreadExists, bool isThread, string orderByString, string whereString)
        {
            string sqlWhereString = string.Format("WHERE (ThreadID = {0} {1})", threadID, whereString);
           
            if (isThreadExists)
            {
                sqlWhereString += string.Format(" AND IsThread = '{0}'", isThread);
            }

            if (startNum <= 1)
            {
                return GetDataSourceByContentNumAndWhereString(publishmentSystemID, totalNum, sqlWhereString, orderByString);
            }
            else
            {
                return GetDataSourceByStartNum(publishmentSystemID, startNum, totalNum, sqlWhereString, orderByString);
            }
        }

        private IEnumerable GetDataSourceByContentNumAndWhereString(int publishmentSystemID, int totalNum, string whereString, string orderByString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(PostInfo.TableName, totalNum, SqlUtils.Asterisk, whereString, orderByString);
            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        private IEnumerable GetDataSourceByStartNum(int publishmentSystemID, int startNum, int totalNum, string whereString, string orderByString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(PostInfo.TableName, startNum, totalNum, SqlUtils.Asterisk, whereString, orderByString);
            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        public IEnumerable GetDataSourceByIsHandled(int publishmentSystemID, int startNum, int totalNum, int days)
        {
            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            string SQL_SELECT = string.Empty;
            string orderByString = string.Empty;
            string whereString = string.Empty;
            if (startNum == 1)
            {
                orderByString = EBBSTaxisTypeUtils.GetOrderByString(EContextType.Post, EBBSTaxisType.OrderByIDDesc);
                whereString = string.Format("WHERE (IsHandled != 'True') AND DATEDIFF(day, AddDate, getdate()) <= {0}", days);
                if (!string.IsNullOrEmpty(additional.NotHandleForumIDCollection))
                {
                    whereString += string.Format(" AND (ForumID NOT IN ({0}))", additional.NotHandleForumIDCollection);
                }
            }
            else
            {
                string str = string.Format("SELECT MAX(ID) AS ID FROM bbs_Post WHERE PublishmentSystemID = {0} AND ID < (SELECT MIN(ID) AS ID FROM bbs_Post WHERE PublishmentSystemID = {0} AND ID IN (SELECT TOP {1} ID FROM bbs_Post WHERE PublishmentSystemID = {0} AND (IsHandled != 'True') AND DATEDIFF(day, AddDate, getdate()) <= {2} ORDER BY ID DESC))", publishmentSystemID, startNum - 2, days);

                orderByString = EBBSTaxisTypeUtils.GetOrderByString(EContextType.Post, EBBSTaxisType.OrderByIDDesc);
                whereString = string.Format("WHERE (IsHandled != 'True') AND DATEDIFF(day, AddDate, getdate()) <= {0} AND ID < ({1})", days, str);
                if (!string.IsNullOrEmpty(additional.NotHandleForumIDCollection))
                {
                    whereString += string.Format(" AND (ForumID NOT IN ({0}))", additional.NotHandleForumIDCollection);
                }
            }

            whereString += " AND ForumID > 0";

            SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(PostInfo.TableName, totalNum, SqlUtils.Asterisk, whereString, orderByString);
            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        public int GetDataSourceCountByIsHandled(int publishmentSystemID, int days)
        {
            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE PublishmentSystemID = {1} AND IsHandled != 'True' AND DATEDIFF(day, AddDate, getdate()) <= {2} AND ForumID > 0", PostInfo.TableName, publishmentSystemID, days);

            if (!string.IsNullOrEmpty(additional.NotHandleForumIDCollection))
            {
                sqlString += string.Format(" AND (ForumID NOT IN ({0}))", additional.NotHandleForumIDCollection);
            }

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public IEnumerable GetDataSourceByIsMy(int publishmentSystemID, int startNum, int totalNum, string userName)
        {
            string SQL_SELECT = string.Empty;
            string orderByString = string.Empty;
            string whereString = string.Empty;
            if (startNum == 1)
            {
                orderByString = EBBSTaxisTypeUtils.GetOrderByString(EContextType.Post, EBBSTaxisType.OrderByIDDesc);
                whereString = string.Format("WHERE PublishmentSystemID = {0} AND UserName = '{1}'", publishmentSystemID, userName);
                SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(PostInfo.TableName, totalNum, SqlUtils.Asterisk, whereString, orderByString);
            }
            else
            {
                string str = string.Format("SELECT MAX(ID) AS ID FROM bbs_Post WHERE ID < (SELECT MIN(ID) AS ID FROM bbs_Post WHERE ID IN (SELECT TOP {0} ID FROM bbs_Post WHERE PublishmentSystemID = {1} AND UserName = '{2}' ORDER BY ID DESC)) AND PublishmentSystemID = {1}", startNum - 2, publishmentSystemID, userName);

                orderByString = EBBSTaxisTypeUtils.GetOrderByString(EContextType.Post, EBBSTaxisType.OrderByIDDesc);
                whereString = string.Format("WHERE (UserName ='{0}' AND PublishmentSystemID = {1}) AND ID < ({2})", userName, publishmentSystemID, str);
                SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(PostInfo.TableName, totalNum, SqlUtils.Asterisk, whereString, orderByString);
            }
            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        public int GetDataSourceCountByIsMy(int publishmentSystemID, string userName)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE PublishmentSystemID = {1} AND UserName = '{2}'", PostInfo.TableName, publishmentSystemID, userName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetMaxTaxis(int publishmentSystemID, int threadID)
        {
            int maxTaxis = 0;

            string sqlString = string.Format("SELECT MAX(Taxis) FROM {0} WHERE ThreadID = {1} AND PublishmentSystemID = {2}", PostInfo.TableName, threadID, publishmentSystemID);

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

        public int GetTaxis(int publishmentSystemID, int selectedID)
        {
            string sqlString = string.Format("SELECT Taxis FROM {0} WHERE ID = {1}", PostInfo.TableName, selectedID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public void SetTaxis(int publishmentSystemID, int id, int taxis)
        {
            string sqlString = string.Format("UPDATE {0} SET Taxis = {1} WHERE ID = {2}", PostInfo.TableName, taxis, id);
            this.ExecuteNonQuery(sqlString);
        }

        public int GetPostCount(int publishmentSystemID, int forumID)
        {
            int count = 0;

            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE PublishmentSystemID = {1} AND ForumID = {2}", PostInfo.TableName, publishmentSystemID, forumID);

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

        public int GetPostCount(int publishmentSystemID)
        {
            int count = 0;

            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1}", PostInfo.TableName, publishmentSystemID);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            count = Convert.ToInt32(rdr[0]);
                        }
                    }
                    rdr.Close();
                }
            }

            return count;
        }

        public int GetPostCount(int publishmentSystemID, DateTime date)
        {
            int count = 0;

            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND year(AddDate) = {2} AND month(AddDate) = {3} AND day(AddDate) = {4} ", PostInfo.TableName, publishmentSystemID, date.Year, date.Month, date.Day);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            count = Convert.ToInt32(rdr[0]);
                        }
                    }
                    rdr.Close();
                }
            }

            return count;
        }

        public void UpDownPost(int publishmentSystemID, string postIDList, bool isUp, int threadID)
        {
            if (!string.IsNullOrEmpty(postIDList))
            {
                foreach (int postID in TranslateUtils.StringCollectionToIntArrayList(postIDList))
                {
                    if (isUp)
                    {
                        this.UpdateTaxisToDown(publishmentSystemID, postID, threadID);
                    }
                    else
                    {
                        this.UpdateTaxisToUp(publishmentSystemID, postID, threadID);
                    }
                }
            }
        }

        public void UpdateTaxisToUp(int publishmentSystemID, int id, int threadID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Post WHERE (Taxis > (SELECT Taxis FROM bbs_Post WHERE ID = {0}) AND PublishmentSystemID = {1} AND ThreadID = {2} AND IsThread = '{3}') ORDER BY Taxis", id, publishmentSystemID, threadID, false.ToString());

            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = rdr.GetInt32(0);
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            if (higherID != 0)
            {
                int selectedTaxis = GetTaxis(publishmentSystemID, id);
                SetTaxis(publishmentSystemID, id, higherTaxis);
                SetTaxis(publishmentSystemID, higherID, selectedTaxis);
            }
        }

        public void UpdateTaxisToDown(int publishmentSystemID, int id, int threadID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM bbs_Post WHERE (Taxis < (SELECT Taxis FROM bbs_Post WHERE (ID = {0})) AND PublishmentSystemID = {1} AND ThreadID = {2} AND IsThread = '{3}') ORDER BY Taxis DESC", id, publishmentSystemID, threadID, false.ToString());

            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = rdr.GetInt32(0);
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            if (lowerID != 0)
            {
                int selectedTaxis = GetTaxis(publishmentSystemID, id);
                SetTaxis(publishmentSystemID, id, lowerTaxis);
                SetTaxis(publishmentSystemID, lowerID, selectedTaxis);
            }
        }

        public List<PostInfo> GetNotPassedPost(int publishmentSystemID)
        {
            string isChecked = "False";

            string sqlString = string.Format("SELECT ID, Content FROM bbs_Post WHERE PublishmentSystemID = {0} AND IsChecked='{1}' ORDER BY Taxis DESC", publishmentSystemID, isChecked);

            List<PostInfo> list = new List<PostInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    PostInfo info = new PostInfo(publishmentSystemID);
                    info.ID = rdr.GetInt32(0);
                    info.Content = rdr.GetString(1);
                    list.Add(info);
                }
                rdr.Close();
            }
            return list;
        }

        public void Delete(int publishmentSystemID, int postID)
        {
            string sqlString = "DELETE FROM bbs_Post WHERE ID = @ID";

            PostInfo postInfo = this.GetPostInfo(publishmentSystemID, postID);
            int threadID = postInfo.ThreadID;
            int forumID = postInfo.ForumID;

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@ID", EDataType.Integer, postID)
            };

            this.ExecuteNonQuery(sqlString, param);

            ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(publishmentSystemID, threadID);
            if (threadInfo != null)
            {
                threadInfo.Replies -= 1;
                if (postID == threadInfo.LastPostID)
                {
                    threadInfo.LastPostID = 0;
                }
                DataProvider.ThreadDAO.Update(threadInfo);
            }

            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, forumID);
            if (forumInfo != null)
            {
                forumInfo.PostCount -= 1;
                if (postID == forumInfo.LastPostID)
                {
                    forumInfo.LastPostID = 0;
                }
                DataProvider.ForumDAO.UpdateForumInfo(publishmentSystemID, forumInfo);
            }
        }

        public void Pass(int publishmentSystemID, int ID)
        {
            string userName = BaiRongDataProvider.AdministratorDAO.UserName;
            System.DateTime dt = System.DateTime.Now;
            string isChecked = "True";
            string sqlString = string.Format("UPDATE bbs_Post SET IsChecked=@IsChecked,Assessor=@Assessor,CheckDate=@CheckDate WHERE ID = @ID");
            IDbDataParameter[] param = new IDbDataParameter[]
            {
                this.GetParameter("@IsChecked",EDataType.VarChar,18,isChecked),
                this.GetParameter("@Assessor",EDataType.NVarChar,50,userName),
                this.GetParameter("@CheckDate",EDataType.DateTime,dt),
                this.GetParameter("ID",EDataType.Integer,ID)
            };
            this.ExecuteNonQuery(sqlString,param);
        }
    }
}
