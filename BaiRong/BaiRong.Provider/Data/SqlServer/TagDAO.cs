using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;
using System.Collections.Generic;

namespace BaiRong.Provider.Data.SqlServer
{
    public class TagDAO : DataProviderBase, ITagDAO
	{
        private const string PARM_TAG_ID = "@TagID";
        private const string PARM_PRODUCT_ID = "@ProductID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_CONTENT_ID_COLLECTION = "@ContentIDCollection";
        private const string PARM_TAG = "@Tag";
        private const string PARM_USE_NUM = "@UseNum";
        
        public int Insert(TagInfo tagInfo)
        {
            int tagID = 0;

            string sqlString = "INSERT INTO bairong_Tags (ProductID, PublishmentSystemID, ContentIDCollection, Tag, UseNum) VALUES (@ProductID, @PublishmentSystemID, @ContentIDCollection, @Tag, @UseNum)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_Tags (TagID, ProductID, PublishmentSystemID, ContentIDCollection, Tag, UseNum) VALUES (bairong_Tags_SEQ.NEXTVAL, @ProductID, @PublishmentSystemID, @ContentIDCollection, @Tag, @UseNum)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, tagInfo.ProductID),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, tagInfo.PublishmentSystemID),
				this.GetParameter(PARM_CONTENT_ID_COLLECTION, EDataType.NVarChar, 255, tagInfo.ContentIDCollection),
                this.GetParameter(PARM_TAG, EDataType.NVarChar, 255, tagInfo.Tag),
                this.GetParameter(PARM_USE_NUM, EDataType.Integer, tagInfo.UseNum)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        tagID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_Tags");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return tagID;
        }

        public void Update(TagInfo tagInfo)
        {
            string sqlString = "UPDATE bairong_Tags SET ContentIDCollection = @ContentIDCollection, UseNum = @UseNum WHERE TagID = @TagID";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CONTENT_ID_COLLECTION, EDataType.NVarChar, 255, tagInfo.ContentIDCollection),
                this.GetParameter(PARM_USE_NUM, EDataType.Integer, tagInfo.UseNum),
                this.GetParameter(PARM_TAG_ID, EDataType.Integer, tagInfo.TagID)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public TagInfo GetTagInfo(string productID, int publishmentSystemID, string tag)
        {
            TagInfo tagInfo = null;

            string sqlString = "SELECT TagID, ProductID, PublishmentSystemID, ContentIDCollection, Tag, UseNum FROM bairong_Tags WHERE ProductID = @ProductID AND PublishmentSystemID = @PublishmentSystemID AND Tag = @Tag";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, productID),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_TAG, EDataType.NVarChar, 255, tag)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    tagInfo = new TagInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5));
                }
                rdr.Close();
            }
            return tagInfo;
        }

        public ArrayList GetTagInfoArrayList(string productID, int publishmentSystemID, int contentID)
        {
            ArrayList arraylist = new ArrayList();

            string whereString = this.GetWhereString(null, productID, publishmentSystemID, contentID);
            string sqlString = string.Format("SELECT TagID, ProductID, PublishmentSystemID, ContentIDCollection, Tag, UseNum FROM bairong_Tags {0}", whereString);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    TagInfo tagInfo = new TagInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5));
                    arraylist.Add(tagInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetTagInfoArrayList(string productID, int publishmentSystemID, int contentID, bool isOrderByCount, int totalNum)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Empty;

            string whereString = this.GetWhereString(null, productID, publishmentSystemID, contentID);
            string orderString = string.Empty;
            if (isOrderByCount)
            {
                orderString = "ORDER BY UseNum DESC";
            }

            if (totalNum > 0)
            {
                if (this.DataBaseType != EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"
SELECT TOP {0} TagID, ProductID, PublishmentSystemID, ContentIDCollection, Tag, UseNum FROM bairong_Tags {1} {2}
            ", totalNum, whereString, orderString);
                }
                else
                {
                    sqlString = string.Format(@"
SELECT * FROM (
SELECT TagID, ProductID, PublishmentSystemID, ContentIDCollection, Tag, UseNum FROM bairong_Tags {0} {1}
) WHERE ROWNUM <= {2}
            ", whereString, orderString, totalNum);
                }
            }
            else
            {
                sqlString = string.Format(@"
SELECT TagID, ProductID, PublishmentSystemID, ContentIDCollection, Tag, UseNum FROM bairong_Tags {0} {1}
            ", whereString, orderString);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    TagInfo tagInfo = new TagInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), rdr.GetInt32(5));
                    arraylist.Add(tagInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetTagArrayListByStartString(string productID, int publishmentSystemID, string startString, int totalNum)
        {
            string totalString = string.Empty;
            if (totalNum > 0)
            {
                totalString = " TOP " + totalNum + " ";
            }

            string sqlString = string.Format("SELECT DISTINCT {0} Tag, UseNum FROM bairong_Tags WHERE ProductID = '{1}' AND PublishmentSystemID = {2} AND Tag LIKE '{3}%' ORDER BY UseNum DESC", totalString, productID, publishmentSystemID, PageUtils.FilterSql(startString));
            if (this.DataBaseType == EDatabaseType.SqlServer)
            {
                sqlString = string.Format("SELECT DISTINCT {0} Tag, UseNum FROM bairong_Tags WHERE ProductID = '{1}' AND PublishmentSystemID = {2} AND CHARINDEX('{3}',Tag) > 0  ORDER BY UseNum DESC", totalString, productID, publishmentSystemID, PageUtils.FilterSql(startString));
            }
            else if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = string.Format("SELECT DISTINCT {0} Tag, UseNum FROM bairong_Tags WHERE ProductID = '{1}' AND PublishmentSystemID = {2} AND instr(Tag, '{3}') > 0  ORDER BY UseNum DESC", totalString, productID, publishmentSystemID, PageUtils.FilterSql(startString));
            }
            return BaiRongDataProvider.DatabaseDAO.GetStringArrayList(sqlString);
        }

        public ArrayList GetTagArrayList(string productID, int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT Tag FROM bairong_Tags WHERE ProductID = '{0}' AND PublishmentSystemID = {1} ORDER BY UseNum DESC", PageUtils.FilterSql(productID), publishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetStringArrayList(sqlString);
        }

        public void DeleteTags(string productID, int publishmentSystemID)
        {
            string whereString = this.GetWhereString(null, productID, publishmentSystemID, 0);
            string sqlString = string.Format("DELETE FROM bairong_Tags {0}", whereString);
            this.ExecuteNonQuery(sqlString);
        }

        public void DeleteTag(string tag, string productID, int publishmentSystemID)
        {
            string whereString = this.GetWhereString(tag, productID, publishmentSystemID, 0);
            string sqlString = string.Format("DELETE FROM bairong_Tags {0}", whereString);
            this.ExecuteNonQuery(sqlString);
        }

        public int GetTagCount(string tag, string productID, int publishmentSystemID)
        {
            ArrayList contentIDArrayList = this.GetContentIDArrayListByTag(tag, productID, publishmentSystemID);
            return contentIDArrayList.Count;
        }

        private string GetWhereString(string tag, string productID, int publishmentSystemID, int contentID)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" WHERE ProductID = '{0}' AND PublishmentSystemID = {1} ", PageUtils.FilterSql(productID), publishmentSystemID);
            if (!string.IsNullOrEmpty(tag))
            {
                builder.AppendFormat("AND Tag = '{0}' ", PageUtils.FilterSql(tag));
            }
            if (contentID > 0)
            {
                builder.AppendFormat("AND (ContentIDCollection = '{0}' OR ContentIDCollection LIKE '{0},%' OR ContentIDCollection LIKE '%,{0},%' OR ContentIDCollection LIKE '%,{0}')", contentID);
            }

            return builder.ToString();
        }

        public ArrayList GetContentIDArrayListByTag(string tag, string productID, int publishmentSystemID)
        {
            ArrayList contentIDArrayList = new ArrayList();
            if (!string.IsNullOrEmpty(tag))
            {
                string whereString = this.GetWhereString(tag, productID, publishmentSystemID, 0);
                string sqlString = "SELECT ContentIDCollection FROM bairong_Tags" + whereString;

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read())
                    {
                        string contentIDCollection = rdr.GetValue(0).ToString();
                        contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(contentIDCollection);
                    }
                    rdr.Close();
                }
            }
            return contentIDArrayList;
        }

        public ArrayList GetContentIDArrayListByTagCollection(StringCollection tagCollection, string productID, int publishmentSystemID)
        {
            ArrayList contentIDArrayList = new ArrayList();
            if (tagCollection.Count > 0)
            {
                string parameterNameList = string.Empty;
                List<IDbDataParameter> parameterList = this.GetINParameterList(PARM_TAG, EDataType.NVarChar, 255, tagCollection, out parameterNameList);

                string sqlString = string.Format("SELECT ContentIDCollection FROM bairong_Tags WHERE Tag IN ({0}) AND ProductID = @ProductID AND PublishmentSystemID = @PublishmentSystemID", parameterNameList);

                List<IDbDataParameter> paramList = new List<IDbDataParameter>();
                paramList.AddRange(parameterList);
                paramList.Add(this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, productID));
                paramList.Add(this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID));

                //string sqlString = string.Format("SELECT ContentIDCollection FROM bairong_Tags WHERE Tag IN ({0}) AND ProductID = '{1}' AND PublishmentSystemID = {2}", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(tagCollection), productID, publishmentSystemID);

                using (IDataReader rdr = this.ExecuteReader(sqlString, paramList.ToArray()))
                {
                    while (rdr.Read())
                    {
                        string contentIDCollection = rdr.GetValue(0).ToString();
                        ArrayList arraylist = TranslateUtils.StringCollectionToIntArrayList(contentIDCollection);
                        foreach (int contentID in arraylist)
                        {
                            if (!contentIDArrayList.Contains(contentID))
                            {
                                contentIDArrayList.Add(contentID);
                            }
                        }
                    }
                    rdr.Close();
                }
            }
            return contentIDArrayList;
        }
	}
}
