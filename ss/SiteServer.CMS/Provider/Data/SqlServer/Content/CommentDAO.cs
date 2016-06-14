using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Collections.Generic;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class CommentDAO : DataProviderBase, ICommentDAO
    {
        private const string SQL_UPDATE = "UPDATE siteserver_Comment SET Good = @Good, Content = @Content WHERE CommentID = @CommentID";

        private const string SQL_SELECT = "SELECT CommentID, PublishmentSystemID, NodeID, ContentID, Good, UserName, IPAddress, AddDate, Taxis, IsChecked, IsRecommend, Content, AdminName FROM siteserver_Comment WHERE CommentID = @CommentID";

        private const string SQL_SELECT_ALL_CHECKED = "SELECT CommentID, PublishmentSystemID, NodeID, ContentID, Good, UserName, IPAddress, AddDate, Taxis, IsChecked, IsRecommend, Content, AdminName FROM siteserver_Comment WHERE PublishmentSystemID = @PublishmentSystemID AND NodeID = @NodeID AND ContentID = @ContentID AND IsChecked = @IsChecked ORDER BY IsRecommend DESC, CommentID DESC";

        private const string PARM_COMMENT_ID = "@CommentID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_CONTENT_ID = "@ContentID";
        private const string PARM_GOOD = "@Good";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_IS_CHECKED = "@IsChecked";
        private const string PARM_IS_RECOMMEND = "@IsRecommend";
        private const string PARM_CONTENT = "@Content";

        //管理员名称
        private const string PARM_ADMIN_NAME = "@AdminName";

        public int Insert(int publishmentSystemID, CommentInfo commentInfo)
        {
            int commentID = 0;

            string sqlString = "INSERT INTO siteserver_Comment(PublishmentSystemID, NodeID, ContentID, Good, UserName, IPAddress, AddDate, Taxis, IsChecked, IsRecommend, Content, AdminName) VALUES (@PublishmentSystemID, @NodeID, @ContentID, @Good, @UserName, @IPAddress, @AddDate, @Taxis, @IsChecked, @IsRecommend, @Content, @AdminName)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_Comment(CommentID, PublishmentSystemID, NodeID, ContentID, Good, UserName, IPAddress, AddDate, Taxis, IsChecked, IsRecommend, Content, AdminName) VALUES (siteserver_Comment_SEQ.NEXTVAL, @PublishmentSystemID, @NodeID, @ContentID, @Good, @UserName, @IPAddress, @AddDate, @Taxis, @IsChecked, @IsRecommend, @Content, @AdminName)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, commentInfo.PublishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, commentInfo.NodeID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, commentInfo.ContentID),
                this.GetParameter(PARM_GOOD, EDataType.Integer, commentInfo.Good),
				this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 50, commentInfo.UserName),
				this.GetParameter(PARM_IP_ADDRESS, EDataType.VarChar, 50, commentInfo.IPAddress),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, commentInfo.AddDate),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, commentInfo.Taxis),
                this.GetParameter(PARM_IS_CHECKED, EDataType.VarChar, 18, commentInfo.IsChecked.ToString()),
                this.GetParameter(PARM_IS_RECOMMEND, EDataType.VarChar, 18, commentInfo.IsRecommend.ToString()),
				this.GetParameter(PARM_CONTENT, EDataType.NText, commentInfo.Content),
				this.GetParameter(PARM_ADMIN_NAME, EDataType.NVarChar,255, commentInfo.AdminName)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);

                        commentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_Comment");

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            UpdateCommentNum(publishmentSystemID, commentInfo.NodeID, commentInfo.ContentID);

            return commentID;
        }

        public void Delete(int publishmentSystemID, int nodeID, int contentID, List<int> commentIDList)
        {
            if (commentIDList != null && commentIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM siteserver_Comment WHERE CommentID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(commentIDList));
                this.ExecuteNonQuery(sqlString);

                UpdateCommentNum(publishmentSystemID, nodeID, contentID);
            }
        }

        public void Delete(List<int> commentIDList)
        {
            if (commentIDList != null && commentIDList.Count > 0)
            {
                List<CommentInfo> commentInfoList = new List<CommentInfo>();
                foreach (int commentID in commentIDList)
                {
                    CommentInfo commentInfo = GetCommentInfo(commentID);
                    if (commentInfo != null)
                    {
                        commentInfoList.Add(commentInfo);
                    }
                }

                string sqlString = string.Format("DELETE FROM siteserver_Comment WHERE CommentID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(commentIDList));
                this.ExecuteNonQuery(sqlString);

                foreach (CommentInfo commentInfo in commentInfoList)
                {
                    UpdateCommentNum(commentInfo.PublishmentSystemID, commentInfo.NodeID, commentInfo.ContentID);
                }
            }
        }

        private void UpdateCommentNum(int publishmentSystemID, int nodeID, int contentID)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo != null)
            {
                int comments = GetCount(publishmentSystemInfo.PublishmentSystemID, nodeID, contentID);
                if (contentID > 0)
                {
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                    BaiRongDataProvider.ContentDAO.UpdateComments(tableName, contentID, comments);
                }
                else
                {
                    DataProvider.NodeDAO.UpdateCommentNum(publishmentSystemInfo.PublishmentSystemID, nodeID, comments);
                }
            }
        }

        public void Update(CommentInfo commentInfo, int publishmentSystemID)
        {
            IDataParameter[] parms = new IDataParameter[]
            {
                this.GetParameter(PARM_GOOD, EDataType.Integer, commentInfo.Good),
                this.GetParameter(PARM_CONTENT, EDataType.NText, commentInfo.Content),
				this.GetParameter(PARM_COMMENT_ID, EDataType.Integer, commentInfo.CommentID)
            };
            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void AddGood(int publishmentSystemID, int commentID)
        {
            string sqlString = string.Format("UPDATE siteserver_Comment SET Good = Good + 1 WHERE CommentID = {0}", commentID);
            this.ExecuteNonQuery(sqlString);
        }

        public void Check(List<int> commentIDList, int publishmentSystemID)
        {
            if (commentIDList != null && commentIDList.Count > 0)
            {
                string sqlString = string.Format("UPDATE siteserver_Comment SET IsChecked = 'True' WHERE CommentID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(commentIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Recommend(List<int> commentIDList, bool isRecommend, int publishmentSystemID)
        {
            if (commentIDList != null && commentIDList.Count > 0)
            {
                string sqlString = string.Format("UPDATE siteserver_Comment SET IsRecommend = '{0}' WHERE CommentID IN ({1})", isRecommend.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(commentIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public CommentInfo GetCommentInfo(int commentID)
        {
            CommentInfo commentInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_COMMENT_ID, EDataType.Integer, commentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    commentInfo = new CommentInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetDateTime(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetValue(10).ToString()), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString());
                }
                rdr.Close();
            }

            return commentInfo;
        }

        public List<CommentInfo> GetCommentInfoListChecked(int publishmentSystemID, int nodeID, int contentID)
        {
            List<CommentInfo> commentInfoList = new List<CommentInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
                this.GetParameter(PARM_IS_CHECKED, EDataType.VarChar, 18, true.ToString())
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_CHECKED, parms))
            {
                while (rdr.Read())
                {
                    CommentInfo commentInfo = new CommentInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetDateTime(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetValue(10).ToString()), rdr.GetValue(11).ToString(), rdr.GetValue(12).ToString());

                    commentInfoList.Add(commentInfo);
                }
                rdr.Close();
            }

            return commentInfoList;
        }

        private int GetCount(int publishmentSystemID, int nodeID, int contentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_Comment WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2}", publishmentSystemID, nodeID, contentID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountChecked(int publishmentSystemID, int nodeID, int contentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_Comment WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} AND IsChecked = '{3}'", publishmentSystemID, nodeID, contentID, true.ToString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public virtual int GetCountChecked(int publishmentSystemID, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_Comment WHERE PublishmentSystemID = {0} AND (AddDate BETWEEN '{1}' AND '{2}') AND IsChecked = '{3}'", publishmentSystemID, begin.ToShortDateString(), end.ToShortDateString(), true.ToString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public virtual int GetCountChecked(int publishmentSystemID, int nodeID, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_Comment WHERE PublishmentSystemID = {0} AND NodeID = {1} AND (AddDate BETWEEN '{2}' AND '{3}') AND IsChecked = '{4}'", publishmentSystemID, nodeID, begin.ToShortDateString(), end.ToShortDateString(), true.ToString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSortFieldName()
        {
            return "AddDate";
        }

        public string GetSelectSqlString(int publishmentSystemID, int nodeID, int contentID)
        {
            string whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} ORDER BY AddDate DESC", publishmentSystemID, nodeID, contentID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString("siteserver_Comment", SqlUtils.Asterisk, whereString);
        }

        public string GetSelectSqlString(int publishmentSystemID, List<int> channelIDList, string keyword, int searchDate, ETriState checkedState, ETriState channelState)
        {
            string checkString = string.Empty;
            if (checkedState == ETriState.True)
            {
                checkString = "AND IsChecked = 'True'";
            }
            else if (checkedState == ETriState.False)
            {
                checkString = "AND IsChecked = 'False'";
            }
            string channelString = string.Empty;
            if (channelState == ETriState.True)
            {
                channelString = "AND ContentID = 0";
            }
            else if (checkedState == ETriState.False)
            {
                channelString = "AND ContentID > 0";
            }
            string dateString = string.Empty;
            if (searchDate > 0)
            {
                DateTime dateTime = DateTime.Now.AddDays(-searchDate);
                dateString = string.Format(" AND (AddDate >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
            }
            return GetSelectSqlString(publishmentSystemID, channelIDList, keyword, checkString, channelString, dateString);
        }

        private string GetSelectSqlString(int publishmentSystemID, List<int> channelIDList, string keyword, string checkString, string channelString, string dateString)
        {
            string whereString;
            if (string.IsNullOrEmpty(keyword))
            {
                whereString = whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID IN ({1}) {2} {3} ORDER BY AddDate DESC", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(channelIDList), dateString, checkString);
            }
            else
            {
                whereString = whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID IN ({1}) AND Content LIKE '%{2}%' {3} {4} ORDER BY AddDate DESC", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(channelIDList), keyword, dateString, checkString);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString("siteserver_Comment", SqlUtils.Asterisk, whereString);
        }

        public string GetSelectSqlStringWithChecked(int publishmentSystemID, int nodeID, int contentID, int startNum, int totalNum, bool isRecommend, string whereString, string orderByString)
        {
            if (!string.IsNullOrEmpty(whereString) && !StringUtils.StartsWithIgnoreCase(whereString.Trim(), "AND "))
            {
                whereString = "AND " + whereString.Trim();
            }
            if (isRecommend)
            {
                whereString += string.Format("AND IsRecommend = '{0}'", true.ToString());
            }
            string sqlWhereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} AND IsChecked = '{3}' {4}", publishmentSystemID, nodeID, contentID, true.ToString(), whereString);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString("siteserver_Comment", startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);
        }

        public string GetSelectIDStringWithChecked(int publishmentSystemID, int nodeID, int contentID)
        {
            string sqlWhereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID = {1} AND ContentID = {2} AND IsChecked = 'True'", publishmentSystemID, nodeID, contentID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString("siteserver_Comment", "CommentID, AddDate", sqlWhereString);
        }

        public List<int> GetCommentIDListWithChecked(int publishmentSystemID, int contentID)
        {
            string sqlString = string.Format("SELECT CommentID FROM [{0}] WHERE PublishmentSystemID = {1} AND ContentID = {2} AND IsChecked = 'True' ORDER BY AddDate DESC", "siteserver_Comment", publishmentSystemID, contentID);
            return BaiRongDataProvider.DatabaseDAO.GetIntList(sqlString);
        }

        public IEnumerable GetDataSourceByUserName(int publishmentSystemID, string userName)
        {
            string sqlString = string.Format("SELECT * FROM {0} WHERE PublishmentSystemID = {1} AND UserName = '{2}' ORDER BY AddDate DESC, CommentID DESC", "siteserver_Comment", publishmentSystemID, userName);
            return BaiRongDataProvider.DatabaseDAO.GetDataSource(sqlString);
        }

        public ArrayList GetContentIDArrayListByCount(int publishmentSystemID)
        {
            ArrayList list = new ArrayList();

            string sqlString = string.Format(@"
SELECT ContentID, COUNT(ContentID) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND ContentID > 0 GROUP BY ContentID ORDER BY TotalNum DESC
", "siteserver_Comment", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        list.Add(rdr.GetInt32(0));
                    }
                }
                rdr.Close();
            }

            return list;
        }
    }
}