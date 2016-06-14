using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Model;
using System.Data;
using BaiRong.Model;
using BaiRong.Core;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class KeywordsFilterDAO : DataProviderBase, IKeywordsFilterDAO
    {

        //根据ID对敏感词汇表中的数据进行删除
        public void Delete(int publishmentSystemID, int id)
        {
            string sqlString = string.Format("DELETE FROM bbs_KeywordsFilter WHERE ID = {0}", id);
            this.ExecuteNonQuery(sqlString);

            IdentifyManager.RemoveCache(publishmentSystemID);
        }

        //根据分类表中ID删除敏感词汇
        public void DelByCategoryID(int publishmentSystemID, int categoryID)
        {
            string sqlString = string.Format("DELETE FROM bbs_KeywordsFilter WHERE CategoryID = {0}", categoryID);
            this.ExecuteNonQuery(sqlString);

            IdentifyManager.RemoveCache(publishmentSystemID);
        }

        //取最大的Taxis
        private int GetMaxTaxis(int publishmentSystemID)
        {
            int maxTaxis = 0;

            string sqlString = string.Format("SELECT MAX(Taxis) FROM bbs_KeywordsFilter WHERE PublishmentSystemID = {0}", publishmentSystemID);

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

        //往敏感词汇表中添加数据
        public void Insert(int publishmentSystemID, KeywordsFilterInfo info)
        {
            int maxTaxis = this.GetMaxTaxis(info.PublishmentSystemID);
            info.Taxis = maxTaxis + 1;

            string sqlString = "INSERT INTO bbs_KeywordsFilter(PublishmentSystemID, CategoryID, Grade, Name, Replacement, Taxis) VALUES(@PublishmentSystemID, @CategoryID, @Grade, @Name, @Replacement, @Taxis)";
            IDbDataParameter[] param = new IDbDataParameter[]
            {
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter("@CategoryID", EDataType.Integer, info.CategoryID),
                this.GetParameter("@Grade", EDataType.Integer, info.Grade),
                this.GetParameter("@Name", EDataType.NVarChar, 50, info.Name),
                this.GetParameter("@Replacement",EDataType.NVarChar,50,info.Replacement),
                this.GetParameter("@Taxis", EDataType.Integer, info.Taxis)
            };

            this.ExecuteNonQuery(sqlString, param);

            IdentifyManager.RemoveCache(publishmentSystemID);
        }

        //修改敏感词汇表中的信息
        public void Update(int publishmentSystemID, KeywordsFilterInfo info)
        {
            string sqlString = "UPDATE bbs_KeywordsFilter SET CategoryID=@CategoryID, Grade=@Grade,Name=@Name,Replacement=@Replacement WHERE ID=@ID";

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@CategoryID", EDataType.Integer, info.CategoryID),
                this.GetParameter("@Grade", EDataType.Integer, info.Grade),
                this.GetParameter("@Name", EDataType.NVarChar,50, info.Name),
                this.GetParameter("@Replacement",EDataType.NVarChar,50,info.Replacement),
                this.GetParameter("@ID",EDataType.Integer, info.ID)
            };

            this.ExecuteNonQuery(sqlString, param);

            IdentifyManager.RemoveCache(publishmentSystemID);
        }

        public string GetSelectCommend(int publishmentSystemID, int grade, int categoryid, string keyword)
        {
            StringBuilder sqlString = new StringBuilder();
            sqlString.AppendFormat("SELECT ID, PublishmentSystemID, CategoryID, Grade, Name, Replacement, Taxis FROM bbs_KeywordsFilter WHERE PublishmentSystemID = {0} AND CategoryID !=0 ", publishmentSystemID);

            if (grade != 0)
            {
                sqlString.Append(" AND Grade=" + grade + "");
            }
            if (categoryid != 0)
            {
                sqlString.Append(" AND CategoryID=" + categoryid + "");
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sqlString.Append(" AND Name like '%" + keyword + "%'");
            }
            sqlString.Append(" ORDER BY Taxis DESC");
            return sqlString.ToString();
        }

        //根据ID取信息
        public KeywordsFilterInfo GetKeywordsFilterInfo(int id)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, CategoryID, Grade, Name, Replacement, Taxis FROM bbs_KeywordsFilter WHERE ID = {0}", id);

            KeywordsFilterInfo Info = null;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    Info = new KeywordsFilterInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetString(5),rdr.GetInt32(6));
                }
                rdr.Close();
            }
            return Info;
        }

        //取敏感词分类表中的信息
        public List<KeywordsFilterInfo> GetKeywordsFilterList(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, CategoryID, Grade, Name, Replacement, Taxis FROM bbs_KeywordsFilter WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC", publishmentSystemID);

            List<KeywordsFilterInfo> list = new List<KeywordsFilterInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    KeywordsFilterInfo info = new KeywordsFilterInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetString(5), rdr.GetInt32(6));
                    list.Add(info);
                }
                rdr.Close();
            }
            return list;
        }

        //根据敏感级别取敏感词汇
        public List<KeywordsFilterInfo> GetKeywordsByGrade(int publishmentSystemID, int grade)
        {
            string sqlString;
            if (grade == 0)
            {
                sqlString = string.Format("SELECT ID, PublishmentSystemID, CategoryID, Grade, Name, Replacement, Taxis FROM bbs_KeywordsFilter WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC", publishmentSystemID);
            }
            else
            {
                sqlString = string.Format("SELECT ID, PublishmentSystemID, CategoryID, Grade, Name, Replacement, Taxis FROM bbs_KeywordsFilter WHERE PublishmentSystemID = {0} AND Grade={1} ORDER BY Taxis DESC", publishmentSystemID, grade);
            }

            List<KeywordsFilterInfo> list = new List<KeywordsFilterInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    KeywordsFilterInfo info = new KeywordsFilterInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetString(5), rdr.GetInt32(6));
                    list.Add(info);
                }
                rdr.Close();
            }
            return list;
        }
    }
}
