using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using System.Collections;
using SiteServer.BBS.Core;
using System.Text;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class ForumDAO : DataProviderBase, IForumDAO
    {
        private const string SQL_SELECT = "SELECT ForumID, PublishmentSystemID, ForumName, IndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, IconUrl, Color, Columns, MetaKeywords, MetaDescription, Summary, Content, FilePath, FilePathRule, TemplateID, LinkUrl, ThreadCount, TodayThreadCount, PostCount, TodayPostCount, LastThreadID, LastPostID, LastTitle, LastUserName, LastDate, State, ExtendValues FROM bbs_Forum WHERE ForumID = @ForumID";

        private const string SQL_SELECT_ID = "SELECT ForumID FROM bbs_Forum WHERE ForumID = @ForumID";

        private const string SQL_SELECT_PARENT_ID = "SELECT ParentID FROM bbs_Forum WHERE ForumID = @ForumID";

        private const string SQL_SELECT_COUNT = "SELECT COUNT(*) FROM bbs_Forum WHERE PublishmentSystemID = @PublishmentSystemID AND ParentID = @ParentID";

        private const string SQL_SELECT_INDEX_NAME_COLLECTION = "SELECT DISTINCT IndexName FROM bbs_Forum WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ID_BY_INDEX = "SELECT ForumID FROM bbs_Forum WHERE PublishmentSystemID = @PublishmentSystemID AND IndexName = @IndexName";

        private const string SQL_UPDATE = "UPDATE bbs_Forum SET ForumName = @ForumName, IndexName = @IndexName, IconUrl = @IconUrl, Color = @Color, Columns = @Columns, MetaKeywords = @MetaKeywords, MetaDescription = @MetaDescription, Summary = @Summary, Content = @Content, FilePath = @FilePath, FilePathRule = @FilePathRule, TemplateID = @TemplateID, LinkUrl = @LinkUrl, ThreadCount = @ThreadCount, TodayThreadCount = @TodayThreadCount, PostCount = @PostCount, TodayPostCount = @TodayPostCount, LastThreadID = @LastThreadID, LastPostID = @LastPostID, LastTitle = @LastTitle, LastUserName = @LastUserName, LastDate = @LastDate, State = @State, ExtendValues = @ExtendValues WHERE ForumID = @ForumID";

        private const string SQL_UPDATE_WITH_POST = "UPDATE bbs_Forum SET ThreadCount = @ThreadCount, TodayThreadCount = @TodayThreadCount, PostCount = @PostCount, TodayPostCount = @TodayPostCount, LastThreadID = @LastThreadID, LastPostID = @LastPostID, LastTitle = @LastTitle, LastUserName = @LastUserName, LastDate = @LastDate WHERE ForumID = @ForumID";

        private const string SQL_UPDATE_COUNT = "UPDATE bbs_Forum SET ThreadCount = @ThreadCount, PostCount = @PostCount WHERE ForumID = @ForumID";

        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_FORUM_ID = "@ForumID";
        private const string PARM_FORUM_NAME = "@ForumName";
        private const string PARM_INDEX_NAME = "@IndexName";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_PARENTS_PATH = "@ParentsPath";
        private const string PARM_PARENTS_COUNT = "@ParentsCount";
        private const string PARM_CHILDREN_COUNT = "@ChildrenCount";
        private const string PARM_IS_LAST_NODE = "@IsLastNode";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_ICON_URL = "@IconUrl";
        private const string PARM_COLOR = "@Color";
        private const string PARM_COLUMNS = "@Columns";
        private const string PARM_META_KEYWORDS = "@MetaKeywords";
        private const string PARM_META_DESCRIPTION = "@MetaDescription";
        private const string PARM_SUMMARY = "@Summary";
        private const string PARM_CONTENT = "@Content";
        private const string PARM_FILE_PATH = "@FilePath";
        private const string PARM_FILE_PATH_RULE = "@FilePathRule";
        private const string PARM_TEMPLATE_ID = "@TemplateID";
        private const string PARM_LINK_URL = "@LinkUrl";
        private const string PARM_THREAD_COUNT = "@ThreadCount";
        private const string PARM_TODAY_THREAD_COUNT = "@TodayThreadCount";
        private const string PARM_POST_COUNT = "@PostCount";
        private const string PARM_TODAY_POST_COUNT = "@TodayPostCount";
        private const string PARM_LAST_THREAD_ID = "@LastThreadID";
        private const string PARM_LAST_POST_ID = "@LastPostID";
        private const string PARM_LAST_TITLE = "@LastTitle";
        private const string PARM_LAST_USERNAME = "@LastUserName";
        private const string PARM_LAST_DATE = "@LastDate";
        private const string PARM_STATE = "@State";
        private const string PARM_EXTEND_VALUES = "@ExtendValues";

        private void InsertForumInfoWithTrans(int publishmentSystemID, ForumInfo parentForumInfo, ForumInfo forumInfo, IDbTransaction trans)
        {
            if (parentForumInfo == null)
            {
                parentForumInfo = new ForumInfo(publishmentSystemID);
            }

            if (parentForumInfo.ParentsPath.Length == 0)
            {
                forumInfo.ParentsPath = parentForumInfo.ForumID.ToString();
            }
            else
            {
                forumInfo.ParentsPath = parentForumInfo.ParentsPath + "," + parentForumInfo.ForumID;
            }
            forumInfo.ParentsCount = parentForumInfo.ParentsCount + 1;

            int maxTaxis = this.GetMaxTaxisByParentPath(forumInfo.PublishmentSystemID, forumInfo.ParentsPath);
            if (maxTaxis == 0)
            {
                maxTaxis = parentForumInfo.Taxis;
            }
            forumInfo.Taxis = maxTaxis + 1;
            forumInfo.ExtendValues = forumInfo.Additional.ToString();

            string SQL_INSERT = "INSERT INTO bbs_Forum (PublishmentSystemID, ForumName, IndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, IconUrl, Color, Columns, MetaKeywords, MetaDescription, Summary, Content, FilePath, FilePathRule, TemplateID, LinkUrl, ThreadCount, TodayThreadCount, PostCount, TodayPostCount, LastThreadID, LastPostID, LastTitle, LastUserName, LastDate, State, ExtendValues) VALUES (@PublishmentSystemID, @ForumName, @IndexName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @IsLastNode, @Taxis, @AddDate, @IconUrl, @Color, @Columns, @MetaKeywords, @MetaDescription, @Summary, @Content, @FilePath, @FilePathRule, @TemplateID, @LinkUrl, @ThreadCount, @TodayThreadCount, @PostCount, @TodayPostCount, @LastThreadID, @LastPostID, @LastTitle, @LastUserName, @LastDate, @State, @ExtendValues)";

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, forumInfo.PublishmentSystemID),
				this.GetParameter(PARM_FORUM_NAME, EDataType.NVarChar, 255, forumInfo.ForumName),
				this.GetParameter(PARM_INDEX_NAME, EDataType.NVarChar, 255, forumInfo.IndexName),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, forumInfo.ParentID),
				this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, forumInfo.ParentsPath),
				this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, forumInfo.ParentsCount),
				this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, forumInfo.ChildrenCount),
				this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, true.ToString()),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, forumInfo.Taxis),
				this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, forumInfo.AddDate),
				this.GetParameter(PARM_ICON_URL, EDataType.VarChar, 200, forumInfo.IconUrl),
				this.GetParameter(PARM_COLOR, EDataType.VarChar, 50, forumInfo.Color),
				this.GetParameter(PARM_COLUMNS, EDataType.Integer, forumInfo.Columns),
                this.GetParameter(PARM_META_KEYWORDS, EDataType.NVarChar, 255, forumInfo.MetaKeywords),
                this.GetParameter(PARM_META_DESCRIPTION, EDataType.NVarChar, 255, forumInfo.MetaDescription),
                this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, forumInfo.Summary),
                this.GetParameter(PARM_CONTENT, EDataType.NText, forumInfo.Content),
                this.GetParameter(PARM_FILE_PATH, EDataType.VarChar, 200, forumInfo.FilePath),
                this.GetParameter(PARM_FILE_PATH_RULE, EDataType.VarChar, 200, forumInfo.FilePathRule),
                this.GetParameter(PARM_TEMPLATE_ID, EDataType.Integer, forumInfo.TemplateID),
                this.GetParameter(PARM_LINK_URL, EDataType.VarChar, 200, forumInfo.LinkUrl),
				this.GetParameter(PARM_THREAD_COUNT, EDataType.Integer, forumInfo.ThreadCount),
                this.GetParameter(PARM_TODAY_THREAD_COUNT, EDataType.Integer, forumInfo.TodayThreadCount),
                this.GetParameter(PARM_POST_COUNT, EDataType.Integer, forumInfo.PostCount),
                this.GetParameter(PARM_TODAY_POST_COUNT, EDataType.Integer, forumInfo.TodayPostCount),
                this.GetParameter(PARM_LAST_THREAD_ID, EDataType.Integer, forumInfo.LastThreadID),
                this.GetParameter(PARM_LAST_POST_ID, EDataType.Integer, forumInfo.LastPostID),
                this.GetParameter(PARM_LAST_TITLE, EDataType.NVarChar, 200, forumInfo.LastTitle),
                this.GetParameter(PARM_LAST_USERNAME, EDataType.NVarChar, 200, forumInfo.LastUserName),
                this.GetParameter(PARM_LAST_DATE, EDataType.DateTime, forumInfo.LastDate),
                this.GetParameter(PARM_STATE, EDataType.NVarChar, 50, forumInfo.State),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, forumInfo.ExtendValues)
			};

            string sqlString = string.Empty;

            if (forumInfo.ParentID != 0)
            {
                sqlString = string.Format("UPDATE bbs_Forum SET Taxis = Taxis + 1 WHERE Taxis >= {0} AND PublishmentSystemID = {1}", forumInfo.Taxis, forumInfo.PublishmentSystemID);
                this.ExecuteNonQuery(trans, sqlString);
            }

            this.ExecuteNonQuery(trans, SQL_INSERT, insertParms);

            forumInfo.ForumID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bbs_Forum");

            sqlString = string.Empty;

            if (!string.IsNullOrEmpty(forumInfo.ParentsPath))
            {
                sqlString = string.Concat("UPDATE bbs_Forum SET ChildrenCount = ChildrenCount + 1 WHERE ForumID in (", forumInfo.ParentsPath, ")");

                this.ExecuteNonQuery(trans, sqlString);
            }

            if (forumInfo.ParentID > 0)
            {
                sqlString = "UPDATE bbs_Forum SET IsLastNode = 'Flase' WHERE ParentID = " + forumInfo.ParentID;
                this.ExecuteNonQuery(trans, sqlString);
            }

            sqlString = string.Format("UPDATE bbs_Forum SET IsLastNode = 'True' WHERE (ForumID IN (SELECT TOP 1 ForumID FROM bbs_Forum WHERE PublishmentSystemID = {0} AND ParentID = {1} ORDER BY Taxis DESC))", publishmentSystemID, forumInfo.ParentID);

            this.ExecuteNonQuery(trans, sqlString);

            ForumManager.RemoveCache(publishmentSystemID);
        }

        private void UpdateSubtractChildrenCount(int publishmentSystemID, string parentsPath, int subtractNum)
        {
            if (!string.IsNullOrEmpty(parentsPath))
            {
                string sqlString = string.Format("UPDATE bbs_Forum SET ChildrenCount = ChildrenCount - {0} WHERE PublishmentSystemID = {1} AND ForumID IN ({2})", subtractNum, publishmentSystemID, parentsPath);
                this.ExecuteNonQuery(sqlString);
            }
        }

        private void UpdateIsLastNode(int publishmentSystemID, int parentID)
        {
            if (parentID > 0)
            {
                string sqlString = "UPDATE bbs_Forum SET IsLastNode = @IsLastNode WHERE PublishmentSystemID = @PublishmentSystemID AND ParentID = @ParentID";

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, false.ToString()),
				    this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                    this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
			    };

                this.ExecuteNonQuery(sqlString, parms);

                sqlString = string.Format("UPDATE bbs_Forum SET IsLastNode = '{0}' WHERE (ForumID IN (SELECT TOP 1 ForumID FROM bbs_Forum WHERE PublishmentSystemID = {1} AND ParentID = {2} ORDER BY Taxis DESC))", true.ToString(), publishmentSystemID, parentID);

                this.ExecuteNonQuery(sqlString);
            }
        }

        private int GetParentID(int publishmentSystemID, int forumID)
        {
            int parentID = 0;

            IDbDataParameter[] forumParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, forumID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PARENT_ID, forumParms))
            {
                if (rdr.Read())
                {
                    parentID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }
            return parentID;
        }

        private void TaxisSubtract(int publishmentSystemID, int selectedForumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, selectedForumID);
            if (forumInfo == null) return;
            
            int lowerNodeID = 0;
            int lowerChildrenCount = 0;
            string lowerParentsPath = "";
            string sqlString = @"
SELECT TOP 1 ForumID, ChildrenCount, ParentsPath
FROM bbs_Forum
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (ParentID = @ParentID) AND (ForumID <> @ForumID) AND (Taxis < @Taxis)
ORDER BY Taxis DESC";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, forumInfo.PublishmentSystemID),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, forumInfo.ParentID),
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, forumInfo.ForumID),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, forumInfo.Taxis)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    lowerNodeID = Convert.ToInt32(rdr[0]);
                    lowerChildrenCount = Convert.ToInt32(rdr[1]);
                    lowerParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string lowerNodePath = "";
            if (lowerParentsPath == "")
            {
                lowerNodePath = lowerNodeID.ToString();
            }
            else
            {
                lowerNodePath = String.Concat(lowerParentsPath, ",", lowerNodeID);
            }
            string selectedNodePath = "";
            if (forumInfo.ParentsPath == "")
            {
                selectedNodePath = forumInfo.ForumID.ToString();
            }
            else
            {
                selectedNodePath = String.Concat(forumInfo.ParentsPath, ",", forumInfo.ForumID);
            }

            this.SetTaxisSubtract(publishmentSystemID, selectedForumID, selectedNodePath, lowerChildrenCount + 1);
            this.SetTaxisAdd(publishmentSystemID, lowerNodeID, lowerNodePath, forumInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(forumInfo.PublishmentSystemID, forumInfo.ParentID);

        }

        /// <summary>
        /// Change The Texis To Higher Level
        /// </summary>
        private void TaxisAdd(int publishmentSystemID, int selectedForumID)
        {
            ForumInfo forumInfo = ForumManager.GetForumInfo(publishmentSystemID, selectedForumID);
            if (forumInfo == null) return;

            int higherNodeID = 0;
            int higherChildrenCount = 0;
            string higherParentsPath = "";
            string sqlString = @"
SELECT TOP 1 ForumID, ChildrenCount, ParentsPath
FROM bbs_Forum
WHERE (PublishmentSystemID = @PublishmentSystemID) AND (ParentID = @ParentID) AND (ForumID <> @ForumID) AND (Taxis > @Taxis)
ORDER BY Taxis";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, forumInfo.PublishmentSystemID),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, forumInfo.ParentID),
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, forumInfo.ForumID),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, forumInfo.Taxis)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    higherNodeID = Convert.ToInt32(rdr[0]);
                    higherChildrenCount = Convert.ToInt32(rdr[1]);
                    higherParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }

            string higherNodePath = string.Empty;
            if (higherParentsPath == string.Empty)
            {
                higherNodePath = higherNodeID.ToString();
            }
            else
            {
                higherNodePath = String.Concat(higherParentsPath, ",", higherNodeID);
            }
            string selectedNodePath = string.Empty;
            if (forumInfo.ParentsPath == string.Empty)
            {
                selectedNodePath = forumInfo.ForumID.ToString();
            }
            else
            {
                selectedNodePath = String.Concat(forumInfo.ParentsPath, ",", forumInfo.ForumID);
            }

            this.SetTaxisAdd(forumInfo.PublishmentSystemID, selectedForumID, selectedNodePath, higherChildrenCount + 1);
            this.SetTaxisSubtract(forumInfo.PublishmentSystemID, higherNodeID, higherNodePath, forumInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(forumInfo.PublishmentSystemID, forumInfo.ParentID);
        }

        private void SetTaxisAdd(int publishmentSystemID, int forumID, string parentsPath, int addNum)
        {
            string sqlString = string.Format("UPDATE bbs_Forum SET Taxis = Taxis + {0} WHERE PublishmentSystemID = {1} AND ForumID = {2} OR ParentsPath = '{3}' OR ParentsPath like '{3},%'", addNum, publishmentSystemID, forumID, parentsPath);

            this.ExecuteNonQuery(sqlString);
        }

        private void SetTaxisSubtract(int publishmentSystemID, int forumID, string parentsPath, int subtractNum)
        {
            string sqlString = string.Format("UPDATE bbs_Forum SET Taxis = Taxis - {0} WHERE PublishmentSystemID = {1} AND ForumID = {2} OR ParentsPath = '{3}' OR ParentsPath like '{3},%'", subtractNum, publishmentSystemID, forumID, parentsPath);

            this.ExecuteNonQuery(sqlString);
        }

        private int GetMaxTaxisByParentPath(int publishmentSystemID, string parentPath)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) AS MaxTaxis FROM bbs_Forum WHERE PublishmentSystemID = {0} AND(ParentsPath = '{1}' OR ParentsPath like '{1},%')", publishmentSystemID, parentPath);
            int maxTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    maxTaxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }
            return maxTaxis;
        }

        public int InsertForumInfo(int publishmentSystemID, int parentID, string forumName, string indexName)
        {
            ForumInfo forumInfo = new ForumInfo(publishmentSystemID);
            forumInfo.PublishmentSystemID = publishmentSystemID;
            forumInfo.ParentID = parentID;
            forumInfo.ForumName = forumName;
            forumInfo.IndexName = indexName;
            forumInfo.AddDate = DateTime.Now;

            ForumInfo parentForumInfo = ForumManager.GetForumInfo(publishmentSystemID, parentID);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    this.InsertForumInfoWithTrans(publishmentSystemID, parentForumInfo, forumInfo, trans);
                    trans.Commit();
                }
            }

            return forumInfo.ForumID;
        }

        public int InsertForumInfo(int publishmentSystemID, ForumInfo forumInfo)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    ForumInfo parentForumInfo = this.GetForumInfo(publishmentSystemID, forumInfo.ParentID);
                    this.InsertForumInfoWithTrans(publishmentSystemID, parentForumInfo, forumInfo, trans);
                    trans.Commit();
                }
            }

            return forumInfo.ForumID;
        }

        public void UpdateForumInfo(int publishmentSystemID, ForumInfo forumInfo)
        {
            forumInfo.ExtendValues = forumInfo.Additional.ToString();

            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_FORUM_NAME, EDataType.NVarChar, 255, forumInfo.ForumName),
				this.GetParameter(PARM_INDEX_NAME, EDataType.NVarChar, 255, forumInfo.IndexName),
				this.GetParameter(PARM_ICON_URL, EDataType.VarChar, 200, forumInfo.IconUrl),
				this.GetParameter(PARM_COLOR, EDataType.VarChar, 50, forumInfo.Color),
				this.GetParameter(PARM_COLUMNS, EDataType.Integer, forumInfo.Columns),
                this.GetParameter(PARM_META_KEYWORDS, EDataType.NVarChar, 255, forumInfo.MetaKeywords),
                this.GetParameter(PARM_META_DESCRIPTION, EDataType.NVarChar, 255, forumInfo.MetaDescription),
                this.GetParameter(PARM_SUMMARY, EDataType.NVarChar, 255, forumInfo.Summary),
                this.GetParameter(PARM_CONTENT, EDataType.NText, forumInfo.Content),
                this.GetParameter(PARM_FILE_PATH, EDataType.VarChar, 200, forumInfo.FilePath),
                this.GetParameter(PARM_FILE_PATH_RULE, EDataType.VarChar, 200, forumInfo.FilePathRule),
                this.GetParameter(PARM_TEMPLATE_ID, EDataType.Integer, forumInfo.TemplateID),
                this.GetParameter(PARM_LINK_URL, EDataType.VarChar, 200, forumInfo.LinkUrl),
				this.GetParameter(PARM_THREAD_COUNT, EDataType.Integer, forumInfo.ThreadCount),
                this.GetParameter(PARM_TODAY_THREAD_COUNT, EDataType.Integer, forumInfo.TodayThreadCount),
                this.GetParameter(PARM_POST_COUNT, EDataType.Integer, forumInfo.PostCount),
                this.GetParameter(PARM_TODAY_POST_COUNT, EDataType.Integer, forumInfo.TodayPostCount),
                this.GetParameter(PARM_LAST_THREAD_ID, EDataType.Integer, forumInfo.LastThreadID),
                this.GetParameter(PARM_LAST_POST_ID, EDataType.Integer, forumInfo.LastPostID),
                this.GetParameter(PARM_LAST_TITLE, EDataType.NVarChar, 200, forumInfo.LastTitle),
                this.GetParameter(PARM_LAST_USERNAME, EDataType.NVarChar, 200, forumInfo.LastUserName),
                this.GetParameter(PARM_LAST_DATE, EDataType.DateTime, forumInfo.LastDate),
                this.GetParameter(PARM_STATE, EDataType.NVarChar, 50, forumInfo.State),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, forumInfo.ExtendValues),
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, forumInfo.ForumID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);

            ForumManager.UpdateForumCache(publishmentSystemID, forumInfo);
        }

        public void UpdateForumByInsertPost(int publishmentSystemID, ForumInfo forumInfo, IDbTransaction trans)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_THREAD_COUNT, EDataType.Integer, forumInfo.ThreadCount),
                this.GetParameter(PARM_TODAY_THREAD_COUNT, EDataType.Integer, forumInfo.TodayThreadCount),
                this.GetParameter(PARM_POST_COUNT, EDataType.Integer, forumInfo.PostCount),
                this.GetParameter(PARM_TODAY_POST_COUNT, EDataType.Integer, forumInfo.TodayPostCount),
                this.GetParameter(PARM_LAST_THREAD_ID, EDataType.Integer, forumInfo.LastThreadID),
                this.GetParameter(PARM_LAST_POST_ID, EDataType.Integer, forumInfo.LastPostID),
                this.GetParameter(PARM_LAST_TITLE, EDataType.NVarChar, 200, forumInfo.LastTitle),
                this.GetParameter(PARM_LAST_USERNAME, EDataType.NVarChar, 200, forumInfo.LastUserName),
                this.GetParameter(PARM_LAST_DATE, EDataType.DateTime, forumInfo.LastDate),
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, forumInfo.ForumID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_WITH_POST, updateParms);

            ForumManager.UpdateForumCache(publishmentSystemID, forumInfo);
        }

        public void UpdateCount(int publishmentSystemID, int forumID)
        {
            int threadCount = DataProvider.ThreadDAO.GetThreadCount(publishmentSystemID, forumID);
            int postCount = DataProvider.PostDAO.GetPostCount(publishmentSystemID, forumID);

            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_THREAD_COUNT, EDataType.Integer, threadCount),
                this.GetParameter(PARM_POST_COUNT, EDataType.Integer, postCount),
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, forumID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_COUNT, updateParms);

            ForumManager.RemoveCache(publishmentSystemID);
        }

        public void Delete(int publishmentSystemID, int forumID)
        {
            ForumInfo forumInfo = this.GetForumInfo(publishmentSystemID, forumID);

            //by 20151201 sofuny ,下面删除语句是根据ParentsPath进行删除，下级再次删除时就查询不到了
            if (forumInfo == null)
                return;
            string parentsPath = "";
            if (forumInfo.ParentsPath.Length == 0)
            {
                parentsPath = forumInfo.ForumID.ToString();
            }
            else
            {
                parentsPath = forumInfo.ParentsPath + "," + forumInfo.ForumID;
            }
            string DELETE_CMD = string.Format("DELETE FROM bbs_Forum WHERE PublishmentSystemID = {0} AND (ForumID = {1} OR ParentsPath = '{2}' OR ParentsPath LIKE '{2},%')", publishmentSystemID, forumID, parentsPath);

            string sqlString1 = string.Format("DELETE FROM bbs_Thread WHERE PublishmentSystemID = {0} AND ForumID = {1}", publishmentSystemID, forumID);
            string sqlString2 = string.Format("DELETE FROM bbs_Post WHERE PublishmentSystemID = {0} AND ForumID = {1}", publishmentSystemID, forumID);

            this.ExecuteNonQuery(sqlString2);
            this.ExecuteNonQuery(sqlString1);
            int deletedNum = this.ExecuteNonQuery(DELETE_CMD);

            this.UpdateIsLastNode(publishmentSystemID, forumInfo.ParentID);
            this.UpdateSubtractChildrenCount(publishmentSystemID, forumInfo.ParentsPath, deletedNum);

            ForumManager.RemoveCache(publishmentSystemID);
        }

        public void UpdateTaxis(int publishmentSystemID, int selectedForumID, bool isSubtract)
        {
            if (isSubtract)
            {
                this.TaxisSubtract(publishmentSystemID, selectedForumID);
            }
            else
            {
                this.TaxisAdd(publishmentSystemID, selectedForumID);
            }
            ForumManager.RemoveCache(publishmentSystemID);
        }

        public ForumInfo GetForumInfo(int publishmentSystemID, int forumID)
        {
            ForumInfo forum = null;

            IDbDataParameter[] forumParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, forumID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, forumParms))
            {
                if (rdr.Read())
                {
                    forum = new ForumInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), rdr.GetString(5), rdr.GetInt32(6), rdr.GetInt32(7), TranslateUtils.ToBool(rdr.GetString(8)), rdr.GetInt32(9), rdr.GetDateTime(10), rdr.GetString(11), rdr.GetString(12), rdr.GetInt32(13), rdr.GetString(14), rdr.GetString(15), rdr.GetString(16), rdr.GetString(17), rdr.GetString(18), rdr.GetString(19), rdr.GetInt32(20), rdr.GetString(21), rdr.GetInt32(22), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetInt32(25), rdr.GetInt32(26), rdr.GetInt32(27), rdr.GetString(28), rdr.GetString(29), rdr.GetDateTime(30), rdr.GetString(31), rdr.GetString(32));
                }
                rdr.Close();
            }
            return forum;
        }

        public ArrayList GetIndexNameArrayList(int publishmentSystemID)
        {
            ArrayList list = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INDEX_NAME_COLLECTION, parms))
            {
                while (rdr.Read())
                {
                    string indexName = rdr.GetValue(0).ToString();
                    list.Add(indexName);
                }
                rdr.Close();
            }

            return list;
        }

        public int GetForumCount(int publishmentSystemID, int forumID)
        {
            int forumCount = 0;

            IDbDataParameter[] forumParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, forumID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_COUNT, forumParms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        forumCount = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return forumCount;
        }

        public int GetForumIDByIndexName(int publishmentSystemID, string indexName)
        {
            int forumID = 0;
            if (string.IsNullOrEmpty(indexName)) return forumID;

            IDbDataParameter[] forumParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_INDEX_NAME, EDataType.NVarChar, 255, indexName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ID_BY_INDEX, forumParms))
            {
                if (rdr.Read())
                {
                    forumID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return forumID;
        }

        public int GetForumIDByParentIDAndForumName(int publishmentSystemID, int parentID, string forumName, bool recursive)
        {
            int forumID = 0;
            string sqlString = string.Empty;

            if (recursive)
            {
                if (parentID == 0)
                {
                    sqlString = string.Format("SELECT ForumID FROM bbs_Forum WHERE PublishmentSystemID = {0} AND ForumName = '{1}' ORDER BY Taxis", publishmentSystemID, forumName);
                }
                else
                {
                    sqlString = string.Format(@"SELECT ForumID
FROM bbs_Forum 
WHERE ((ParentID = {0}) OR
      (ParentsPath = '{0}') OR
      (ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}')) AND PublishmentSystemID = {1} AND ForumName = '{2}'
ORDER BY Taxis", parentID, publishmentSystemID, forumName);
                }
            }
            else
            {
                sqlString = string.Format("SELECT ForumID FROM bbs_Forum WHERE PublishmentSystemID = {0} AND ParentID = {1} AND ForumName = '{2}' ORDER BY Taxis", publishmentSystemID, parentID, forumName);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    forumID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }
            return forumID;
        }

        public int GetForumIDByParentIDAndTaxis(int publishmentSystemID, int parentID, int taxis, bool isNextForum)
        {
            int forumID = 0;

            string sqlString = string.Empty;
            if (isNextForum)
            {
                sqlString = string.Format("SELECT TOP 1 ForumID FROM bbs_Forum WHERE PublishmentSystemID = {0} AND ParentID = {1} AND Taxis > {2} ORDER BY Taxis", publishmentSystemID, parentID, taxis);
            }
            else
            {
                sqlString = string.Format("SELECT TOP 1 ForumID FROM bbs_Forum WHERE PublishmentSystemID = {0} AND ParentID = {1} AND Taxis < {2} ORDER BY Taxis DESC", publishmentSystemID, parentID, taxis);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    forumID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }
            return forumID;
        }

        public bool IsExists(int publishmentSystemID, int forumID)
        {
            bool exists = false;

            IDbDataParameter[] forumParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, forumID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ID, forumParms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        exists = true;
                    }
                }
                rdr.Close();
            }
            return exists;
        }

        public ArrayList GetForumIDArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT ForumID FROM bbs_Forum WHERE PublishmentSystemID = {0} ORDER BY Taxis", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int forumIDToArrayList = Convert.ToInt32(rdr[0]);
                    arraylist.Add(forumIDToArrayList);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetForumIDArrayListForDescendant(int publishmentSystemID, int forumID)
        {
            string sqlString = string.Format(@"SELECT ForumID
FROM bbs_Forum
WHERE ((ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}') OR
      (ParentID = {0})) AND PublishmentSystemID = {1}
", forumID, publishmentSystemID);
            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(Convert.ToInt32(rdr[0]));
                }
                rdr.Close();
            }

            return list;
        }

        public ArrayList GetForumIDArrayListByParentID(int publishmentSystemID, int parentID)
        {
            string sqlString = string.Format(@"SELECT ForumID FROM bbs_Forum WHERE PublishmentSystemID = {0} AND ParentID = {1} ORDER BY Taxis", publishmentSystemID, parentID);

            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int forumID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(forumID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public Hashtable GetForumInfoHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();
            string sqlString = string.Format("SELECT ForumID, PublishmentSystemID, ForumName, IndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis, AddDate, IconUrl, Color, Columns, MetaKeywords, MetaDescription, Summary, Content, FilePath, FilePathRule, TemplateID, LinkUrl, ThreadCount, TodayThreadCount, PostCount, TodayPostCount, LastThreadID, LastPostID, LastTitle, LastUserName, LastDate,State, ExtendValues FROM bbs_Forum WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    ForumInfo forumInfo = new ForumInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), rdr.GetString(5), rdr.GetInt32(6), rdr.GetInt32(7), TranslateUtils.ToBool(rdr.GetString(8)), rdr.GetInt32(9), rdr.GetDateTime(10), rdr.GetString(11), rdr.GetString(12), rdr.GetInt32(13), rdr.GetString(14), rdr.GetString(15), rdr.GetString(16), rdr.GetString(17), rdr.GetString(18), rdr.GetString(19), rdr.GetInt32(20), rdr.GetString(21), rdr.GetInt32(22), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetInt32(25), rdr.GetInt32(26), rdr.GetInt32(27), rdr.GetString(28), rdr.GetString(29), rdr.GetDateTime(30), rdr.GetString(31), rdr.GetString(32));
                    hashtable.Add(forumInfo.ForumID, forumInfo);
                }
                rdr.Close();
            }

            return hashtable;
        }

        public ArrayList GetAllFilePath(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT FilePath FROM bbs_Forum WHERE FilePath <> '' AND PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return arraylist;
        }

        private string GetGroupWhereString(int publishmentSystemID, string group, string groupNot)
        {
            StringBuilder whereStringBuilder = new StringBuilder();
            //if (!string.IsNullOrEmpty(group))
            //{
            //    //whereStringBuilder.AppendFormat(" AND (bbs_Forum.NodeGroupNameCollection = '{0}' OR bbs_Forum.NodeGroupNameCollection LIKE '{0},%' OR bbs_Forum.NodeGroupNameCollection LIKE '%,{0},%' OR bbs_Forum.NodeGroupNameCollection LIKE '%,{0}') ", group);

            //    group = group.Trim().Trim(',');
            //    string[] groupArr = group.Split(',');
            //    if (groupArr != null && groupArr.Length > 0)
            //    {
            //        whereStringBuilder.Append(" AND (");
            //        foreach (string theGroup in groupArr)
            //        {
            //            whereStringBuilder.AppendFormat(" (bbs_Forum.NodeGroupNameCollection = '{0}' OR bbs_Forum.NodeGroupNameCollection LIKE '{0},%' OR bbs_Forum.NodeGroupNameCollection LIKE '%,{0},%' OR bbs_Forum.NodeGroupNameCollection LIKE '%,{0}') OR ", theGroup.Trim());
            //        }
            //        if (groupArr.Length > 0)
            //        {
            //            whereStringBuilder.Length = whereStringBuilder.Length - 3;
            //        }
            //        whereStringBuilder.Append(") ");
            //    }

            //}

            //if (!string.IsNullOrEmpty(groupNot))
            //{
            //    //whereStringBuilder.AppendFormat(" AND (bbs_Forum.NodeGroupNameCollection <> '{0}' AND bbs_Forum.NodeGroupNameCollection NOT LIKE '{0},%' AND bbs_Forum.NodeGroupNameCollection NOT LIKE '%,{0},%' AND bbs_Forum.NodeGroupNameCollection NOT LIKE '%,{0}') ", groupNot);

            //    groupNot = groupNot.Trim().Trim(',');
            //    string[] groupNotArr = groupNot.Split(',');
            //    if (groupNotArr != null && groupNotArr.Length > 0)
            //    {
            //        whereStringBuilder.Append(" AND (");
            //        foreach (string theGroupNot in groupNotArr)
            //        {
            //            whereStringBuilder.AppendFormat(" (bbs_Forum.NodeGroupNameCollection <> '{0}' AND bbs_Forum.NodeGroupNameCollection NOT LIKE '{0},%' AND bbs_Forum.NodeGroupNameCollection NOT LIKE '%,{0},%' AND bbs_Forum.NodeGroupNameCollection NOT LIKE '%,{0}') AND ", theGroupNot.Trim());
            //        }
            //        if (groupNotArr.Length > 0)
            //        {
            //            whereStringBuilder.Length = whereStringBuilder.Length - 4;
            //        }
            //        whereStringBuilder.Append(") ");
            //    }
            //}
            return whereStringBuilder.ToString();
        }

        public string GetWhereString(int publishmentSystemID, string group, string groupNot, string where)
        {
            StringBuilder whereStringBuilder = new StringBuilder();

            whereStringBuilder.Append(GetGroupWhereString(publishmentSystemID, group, groupNot));

            if (!string.IsNullOrEmpty(where))
            {
                whereStringBuilder.Append(" AND ");
                whereStringBuilder.Append(where);
            }

            return whereStringBuilder.ToString();
        }

        public IEnumerable GetParserDataSource(int publishmentSystemID, int forumID, int startNum, int totalNum, string whereString, string orderByString)
        {
            string tableName = "bbs_Forum";

            ArrayList forumIDArrayList = DataProvider.ForumDAO.GetForumIDArrayListByParentID(publishmentSystemID, forumID);

            if (forumIDArrayList == null || forumIDArrayList.Count == 0)
            {
                return null;
            }

            string sqlWhereString = string.Format("WHERE PublishmentSystemID = {0} AND (ForumID IN ({1}) {2})", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(forumIDArrayList), whereString);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, "ForumID, AddDate, Taxis", sqlWhereString, orderByString);

            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }
    }
}