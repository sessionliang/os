using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Model;
using System.Data;
using BaiRong.Model;

using BaiRong.Core;


namespace SiteServer.BBS.Provider.SqlServer
{
    public class KeywordsCategoryDAO : DataProviderBase, IKeywordsCategoryDAO
    {
        //取敏感词分类表中的信息
        public List<KeywordsCategoryInfo> GetKeywordsCategoryList(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT CategoryID, PublishmentSystemID, CategoryName, IsOpen, Taxis FROM bbs_KeywordsCategory WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC", publishmentSystemID);

            List<KeywordsCategoryInfo> list = new List<KeywordsCategoryInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    KeywordsCategoryInfo info = new KeywordsCategoryInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), TranslateUtils.ToBool(rdr.GetString(3)), rdr.GetInt32(4));
                    list.Add(info);
                }
                rdr.Close();
            }
            return list;
        }

        //根据ID取敏感词分类表中的信息
        public KeywordsCategoryInfo GetKeywordsCategoryInfo(int categoryID)
        {
            string sqlString = string.Format("SELECT CategoryID, PublishmentSystemID, CategoryName, IsOpen, Taxis FROM bbs_KeywordsCategory WHERE CategoryID = {0}", categoryID);

            KeywordsCategoryInfo KeywordsCategoryInfo = null;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    KeywordsCategoryInfo = new KeywordsCategoryInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetString(2), TranslateUtils.ToBool(rdr.GetString(3)), rdr.GetInt32(4));
                }
                rdr.Close();
            }
            return KeywordsCategoryInfo;
        }

        //根据ID进行删除
        public void Delete(int categoryID)
        {
            string sqlString = string.Format("DELETE FROM bbs_KeywordsCategory WHERE CategoryID = {0}", categoryID);
            this.ExecuteNonQuery(sqlString);
        }

        //取最大的Taxis
        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM bbs_KeywordsCategory WHERE PublishmentSystemID = {0}", publishmentSystemID);
            int maxTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
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
            return maxTaxis;
        }

        //插入记录
        public void Insert(KeywordsCategoryInfo info)
        {
            int maxTaxis = this.GetMaxTaxis(info.PublishmentSystemID);
            info.Taxis = maxTaxis + 1;

            string sqlString = "INSERT INTO bbs_KeywordsCategory(PublishmentSystemID, CategoryName, IsOpen, Taxis) VALUES (@PublishmentSystemID, @CategoryName, @IsOpen, @Taxis)";

            IDbDataParameter[] param = new IDbDataParameter[]
            {
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter("@CategoryName", EDataType.NVarChar, 50, info.CategoryName),
                this.GetParameter("@IsOpen", EDataType.VarChar, 18, info.IsOpen.ToString()),
                this.GetParameter("@Taxis", EDataType.Integer, info.Taxis)
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        //插入记录返回主键
        public int Add(KeywordsCategoryInfo info)
        {
            int attachmentID = 0;
            int maxTaxis = this.GetMaxTaxis(info.PublishmentSystemID);
            info.Taxis = maxTaxis + 1;

            string sqlString = "INSERT INTO bbs_KeywordsCategory(PublishmentSystemID, CategoryName, IsOpen, Taxis) VALUES(@PublishmentSystemID, @CategoryName, @IsOpen, @Taxis)";

            IDbDataParameter[] param = new IDbDataParameter[]
            {
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter("@CategoryName", EDataType.NVarChar, 50, info.CategoryName),
                this.GetParameter("@IsOpen", EDataType.VarChar, 18, info.IsOpen.ToString()),
                this.GetParameter("@Taxis", EDataType.Integer, info.Taxis)
            };
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, param);

                        attachmentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bbs_KeywordsCategory");

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return attachmentID;
        }

        //对敏感词分类表中的信息进行修改
        public void Update(KeywordsCategoryInfo info)
        {
            string sqlString = "UPDATE bbs_KeywordsCategory SET CategoryName=@CategoryName, IsOpen=@IsOpen WHERE CategoryID=@CategoryID";

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@CategoryName", EDataType.NVarChar, 50, info.CategoryName),
                this.GetParameter("@IsOpen", EDataType.VarChar, 18, info.IsOpen.ToString()),
                this.GetParameter("@CategoryID",EDataType.Integer, info.CategoryID)
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        //搜索此分类中的敏感词汇的数量
        public int KeyWordsFiltersCount(int categoryID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM bbs_KeywordsFilter WHERE CategoryID = {0}", categoryID);
            int count = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
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
            return count; 
        }

        //取分类表中记录数量
        public int KeywordsCategoryCount(int publishmentSystemID)
        {
            int count = 0;

            string sqlString = string.Format("SELECT COUNT(*) FROM bbs_KeywordsCategory WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
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

            return count; 
        }

        //创建默认的分类
        public void CreateDefaultKeywordsCategory(int publishmentSystemID)
        {
            bool isExists = false;

            string sqlString = string.Format("SELECT CategoryID FROM bbs_KeywordsCategory WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }

            if (!isExists)
            {
                KeywordsCategoryInfo info = new KeywordsCategoryInfo(0, publishmentSystemID, "默认分类", true, 0);
                Insert(info);
            }
        }
    }
}
