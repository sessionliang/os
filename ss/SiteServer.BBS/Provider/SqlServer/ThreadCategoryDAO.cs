using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Core;
using BaiRong.Model;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class ThreadCategoryDAO : DataProviderBase, IThreadCategoryDAO
    {
        public void Insert(ThreadCategoryInfo info)
        {

            string sqlString = "INSERT INTO bbs_ThreadCategory(PublishmentSystemID, ForumID, CategoryName, Summary, Taxis) VALUES (@PublishmentSystemID, @ForumID, @CategoryName, @Summary, @Taxis)";

            IDbDataParameter[] param = new IDbDataParameter[]
            { 
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter("@ForumID", EDataType.Integer, info.ForumID),
                this.GetParameter("@CategoryName", EDataType.NVarChar, 50, info.CategoryName), 
                this.GetParameter("@Summary", EDataType.NVarChar, 500, info.Summary),
                this.GetParameter("@Taxis", EDataType.Integer, info.Taxis)
            };

            this.ExecuteNonQuery(sqlString, param);

            ThreadCategoryManager.RemoveCache(info.PublishmentSystemID);
        }

        public void Update(ThreadCategoryInfo info)
        {

            string sqlString = "UPDATE bbs_ThreadCategory SET ForumID = @ForumID, CategoryName = @CategoryName, Summary = @Summary, Taxis = @Taxis WHERE CategoryID = @CategoryID";

            IDbDataParameter[] param = new IDbDataParameter[]
            {
                this.GetParameter("@ForumID", EDataType.Integer, info.ForumID),
                this.GetParameter("@CategoryName", EDataType.NVarChar, 50, info.CategoryName), 
                this.GetParameter("@Summary", EDataType.NVarChar, 500, info.Summary),
                this.GetParameter("@Taxis", EDataType.Integer, info.Taxis),
                this.GetParameter("@CategoryID", EDataType.Integer, info.CategoryID)
            };

            this.ExecuteNonQuery(sqlString, param);

            ThreadCategoryManager.RemoveCache(info.PublishmentSystemID);
        }

        public void Delete(int publishmentSystemID, int categoryID)
        {
            string sqlString = "DELETE FROM bbs_ThreadCategory WHERE CategoryID = @CategoryID";

            IDbDataParameter[] param = new IDbDataParameter[]
            {
                this.GetParameter("@CategoryID", EDataType.Integer, categoryID)
            };

            this.ExecuteNonQuery(sqlString, param);

            ThreadCategoryManager.RemoveCache(publishmentSystemID);
        }

        public ThreadCategoryInfo GetThreadCategoryInfo(int categoryID)
        {
            ThreadCategoryInfo info = null;

            string sqlString = "SELECT CategoryID, PublishmentSystemID, ForumID, CategoryName, Summary, Taxis FROM bbs_ThreadCategory WHERE CategoryID = " + categoryID;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    info = new ThreadCategoryInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5));
                }
                rdr.Close();
            }

            return info;
        }

        public bool IsExists(int publishmentSystemID, string categoryName, int forumID)
        {

            bool exists = false;
            string sqlString = "SELECT CategoryID FROM bbs_ThreadCategory WHERE PublishmentSystemID = @PublishmentSystemID AND CategoryName = @CategoryName And ForumID = @ForumID";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, publishmentSystemID),
                this.GetParameter("@CategoryName", EDataType.NVarChar, 50, categoryName),
                this.GetParameter("@ForumID", EDataType.Integer, forumID)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public DataSet GetDataSource(int publishmentSystemID, string strWhere)
        {

            StringBuilder sqlString = new StringBuilder();

            sqlString.AppendFormat("SELECT CategoryID, PublishmentSystemID, ForumID, CategoryName, Summary, Taxis FROM bbs_ThreadCategory WHERE PublishmentSystemID = {0}", publishmentSystemID);

            if (!string.IsNullOrEmpty(strWhere))
            {
                sqlString.Append(" AND " + strWhere);
            }

            sqlString.Append(" ORDER BY ForumID, Taxis DESC");

            return this.ExecuteDataset(sqlString.ToString());;
        }

        public ArrayList GetCategoryInfoArrayList(int publishmentSystemID, int forumID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT CategoryID, PublishmentSystemID, ForumID, CategoryName, Summary, Taxis FROM bbs_ThreadCategory WHERE PublishmentSystemID = {0} AND ForumID = {1} ORDER BY Taxis DESC", publishmentSystemID, forumID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    ThreadCategoryInfo info = new ThreadCategoryInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5));

                    arraylist.Add(info);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetForumIDArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT CategoryID FROM bbs_ThreadCategory WHERE PublishmentSystemID = {0} ORDER BY ForumID,Taxis DESC", publishmentSystemID);
            int categoryID = 0;
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    categoryID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(categoryID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetThreadCategoryInfoPairArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT CategoryID, PublishmentSystemID, ForumID, CategoryName, Summary, Taxis FROM bbs_ThreadCategory WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    ThreadCategoryInfo info = new ThreadCategoryInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5));

                    KeyValuePair<int, ThreadCategoryInfo> kvp = new KeyValuePair<int, ThreadCategoryInfo>(info.CategoryID, info);

                    arraylist.Add(kvp);
                }
                rdr.Close();
            }

            return arraylist;
        }

        /// <summary>
        /// 把某分类的排序上升addNum位
        /// </summary>
        /// <param name="categoryID"></param>
        /// <param name="forumID"></param>
        /// <param name="addNum"></param>
        public void TaxisAdd(int publishmentSystemID, int categoryID, int forumID, int addNum)
        {

            // 按Taxis升序排序，把大于本分类的前addNum个分类的排序值-1
            string sqlString1 = string.Format("update bbs_ThreadCategory set Taxis=Taxis-1 where CategoryID in(select top {0} CategoryID from bbs_ThreadCategory where Taxis>=(Select Taxis from bbs_ThreadCategory where CategoryID={1} and ForumID={2}) AND PublishmentSystemID = {3} and ForumID={2} and CategoryID<>{1} order by Taxis)", addNum, categoryID, forumID, publishmentSystemID);
            // 按Taxis升序排序，把大于本分类的前addNum个分类的最大排序值+1赋给本分类
            string sqlString2 = string.Format(@"
update bbs_ThreadCategory set Taxis = (
    select isnull(max(Taxis)+1,(select Taxis from bbs_ThreadCategory where CategoryID={1})) from (select top {0} Taxis from bbs_ThreadCategory where Taxis>=(Select Taxis from bbs_ThreadCategory where CategoryID={1} and ForumID={2}) and ForumID={2} and CategoryID<>{1} order by Taxis) as tmp)
where CategoryID={1}", addNum, categoryID, forumID);
            if (addNum <= 0)
            {
                // 按Taxis升序排序，把大于本分类的分类的排序值-1
                sqlString1 = string.Format("update bbs_ThreadCategory set Taxis=Taxis-1 where CategoryID in(select CategoryID from bbs_ThreadCategory where Taxis>=(Select Taxis from bbs_ThreadCategory where CategoryID={0} and ForumID={1}) and ForumID={1} and CategoryID<>{0})", categoryID, forumID);
                // 按Taxis升序排序，把大于本分类的分类的最大排序值+1赋给本分类
                sqlString2 = string.Format("update bbs_ThreadCategory set Taxis=(select isnull(max(Taxis)+1,(select Taxis from bbs_ThreadCategory where CategoryID={0})) from (select Taxis from bbs_ThreadCategory where Taxis>=(Select Taxis from bbs_ThreadCategory where CategoryID={0} and ForumID={1}) and ForumID={1} and CategoryID<>{0}) as tmp) where CategoryID={0}", categoryID, forumID);

            }
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString1);
                        this.ExecuteNonQuery(trans, sqlString2);
                        trans.Commit();
                    }
                    catch
                    {
                        throw new Exception(sqlString1);
                    }
                }
            }
             
            ThreadCategoryManager.RemoveCache(publishmentSystemID);
        }
        public void TaxisSubtract(int publishmentSystemID, int categoryID, int forumID, int subtractNum)
        {

            // 按Taxis降序排序，把小于本分类的前subtractNum个分类的排序值+1
            string sqlString1 = string.Format("update bbs_ThreadCategory set Taxis=Taxis+1 where CategoryID in(select top {0} CategoryID from bbs_ThreadCategory where Taxis<=(Select Taxis from bbs_ThreadCategory where CategoryID={1} and ForumID={2}) and ForumID={2} and CategoryID<>{1} order by Taxis desc)", subtractNum, categoryID, forumID);
            // 按Taxis降序排序，把小于本分类的前subtractNum个分类的最小排序值-1赋给本分类
            string sqlString2 = string.Format("update bbs_ThreadCategory set Taxis=(select isnull(min(Taxis)-1,(select Taxis from bbs_ThreadCategory where CategoryID={1})) from (select top {0} Taxis from bbs_ThreadCategory where Taxis<=(Select Taxis from bbs_ThreadCategory where CategoryID={1} and ForumID={2}) and ForumID={2} and CategoryID<>{1} order by Taxis desc) as tmp) where CategoryID={1}", subtractNum, categoryID, forumID);
            if (subtractNum <= 0)
            {
                // 按Taxis降序排序，把小于本分类的分类的排序值+1
                sqlString1 = string.Format("update bbs_ThreadCategory set Taxis=Taxis+1 where CategoryID in(select CategoryID from bbs_ThreadCategory where Taxis<=(Select Taxis from bbs_ThreadCategory where CategoryID={0} and ForumID={1}) and ForumID={1} and CategoryID<>{0})", categoryID, forumID);
                // 按Taxis降序排序，把小于本分类的分类的最小排序值-1赋给本分类
                sqlString2 = string.Format("update bbs_ThreadCategory set Taxis=(select isnull(min(Taxis)-1,(select Taxis from bbs_ThreadCategory where CategoryID={0})) from (select Taxis from bbs_ThreadCategory where Taxis<=(Select Taxis from bbs_ThreadCategory where CategoryID={0} and ForumID={1}) and ForumID={1} and CategoryID<>{0}) as tmp) where CategoryID={0}", categoryID, forumID);

            }
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    this.ExecuteNonQuery(trans, sqlString1);
                    this.ExecuteNonQuery(trans, sqlString2);
                    trans.Commit();
                }
            }

            ThreadCategoryManager.RemoveCache(publishmentSystemID);
        }

        public int GetMaxTaxisByCategoryID(int publishmentSystemID, int categoryID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) AS MaxTaxis FROM bbs_ThreadCategory where CategoryID={0} AND PublishmentSystemID = {1} and ForumID=(select ForumID from bbs_ThreadCategory where CategoryID = {0} AND PublishmentSystemID = {1})", categoryID, publishmentSystemID);
            int maxTaxis = 0;
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    maxTaxis = rdr.GetInt32(0);
                }
                rdr.Close();
            }
            return maxTaxis;
        }

        public int GetMaxTaxisByForumID(int publishmentSystemID, int forumID)
        {

            string sqlString = string.Format("SELECT MAX(Taxis) AS MaxTaxis FROM bbs_ThreadCategory where PublishmentSystemID = {0} AND ForumID = {1}", publishmentSystemID, forumID);
            int maxTaxis = 0;
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    maxTaxis = rdr.GetInt32(0);
                }                    
                rdr.Close();
            }
            return maxTaxis;
        }
    }
}