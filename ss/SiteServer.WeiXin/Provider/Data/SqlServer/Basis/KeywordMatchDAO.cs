using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class KeywordMatchDAO : DataProviderBase, IKeywordMatchDAO
    {
        private const string TABLE_NAME = "wx_KeywordMatch";

        private const string SQL_DELETE_BY_KEYWORD_ID = "DELETE FROM wx_KeywordMatch WHERE KeywordID = @KeywordID";

        private const string SQL_SELECT_KEYOWRD_BY_TYPE = "SELECT Keyword FROM wx_KeywordMatch WHERE PublishmentSystemID = @PublishmentSystemID AND KeywordType = @KeywordType ORDER BY Keyword";

        private const string SQL_SELECT_KEYOWRD_ENABLED = "SELECT Keyword FROM wx_KeywordMatch WHERE PublishmentSystemID = @PublishmentSystemID AND IsDisabled = @IsDisabled ORDER BY Keyword";

        private const string PARM_MATCH_ID = "@MatchID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_KEYWORD = "@Keyword";
        private const string PARM_KEYWORD_ID = "@KeywordID";
        private const string PARM_IS_DISABLED = "@IsDisabled";
        private const string PARM_KEYWORD_TYPE = "@KeywordType";
        private const string PARM_MATCH_TYPE = "@MatchType";

        public void Insert(KeywordMatchInfo matchInfo)
        {
            string sqlString = "INSERT INTO wx_KeywordMatch (PublishmentSystemID, Keyword, KeywordID, IsDisabled, KeywordType, MatchType) VALUES (@PublishmentSystemID, @Keyword, @KeywordID, @IsDisabled, @KeywordType, @MatchType)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO wx_KeywordMatch(MatchID, PublishmentSystemID, Keyword, KeywordID, IsDisabled, KeywordType, MatchType) VALUES (wx_KeywordMatch_SEQ.NEXTVAL, @PublishmentSystemID, @Keyword, @KeywordID, @IsDisabled, @KeywordType, @MatchType)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, matchInfo.PublishmentSystemID),
                this.GetParameter(PARM_KEYWORD, EDataType.NVarChar, 255, matchInfo.Keyword),
                this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, matchInfo.KeywordID),
                this.GetParameter(PARM_IS_DISABLED, EDataType.VarChar, 18, matchInfo.IsDisabled.ToString()),
                this.GetParameter(PARM_KEYWORD_TYPE, EDataType.VarChar, 50, EKeywordTypeUtils.GetValue(matchInfo.KeywordType)),
                this.GetParameter(PARM_MATCH_TYPE, EDataType.VarChar, 50, EMatchTypeUtils.GetValue(matchInfo.MatchType))
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void DeleteByKeywordID(int keywordID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
			};

            this.ExecuteNonQuery(SQL_DELETE_BY_KEYWORD_ID, parms);
        }

        public List<string> GetKeywordList(int publishmentSystemID, EKeywordType keywordType)
        {
            List<string> list = new List<string>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_KEYWORD_TYPE, EDataType.VarChar, 50, EKeywordTypeUtils.GetValue(keywordType))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_KEYOWRD_BY_TYPE, parms))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return list;
        }

        public List<string> GetKeywordListEnabled(int publishmentSystemID)
        {
            List<string> list = new List<string>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_IS_DISABLED, EDataType.VarChar, 18, false.ToString())
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_KEYOWRD_ENABLED, parms))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return list;
        }

        public int GetKeywordIDByMPController(int publishmentSystemID, string keyword)
        {
            int keywordID = 0;

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim();
                //string sqlString = string.Format(@"SELECT TOP 1 KeywordID FROM wx_KeywordMatch WHERE PublishmentSystemID = {0} AND IsDisabled <> 'True' AND ( (MatchType = '{1}' AND Keyword = '{2}') OR (MatchType = '{3}' AND Keyword LIKE '%{2}%') ) ORDER BY MatchID", publishmentSystemID, EMatchTypeUtils.GetValue(EMatchType.Exact), keyword, EMatchTypeUtils.GetValue(EMatchType.Contains));
                //string sqlString = string.Format(@"SELECT TOP 1 KeywordID FROM wx_KeywordMatch WHERE PublishmentSystemID = {0} AND IsDisabled <> 'True' AND ( (MatchType = '{1}' AND Keyword = '{2}') OR (MatchType = '{3}' AND CHARINDEX(Keyword, '{2}') <> 0) ) ORDER BY MatchID", publishmentSystemID, EMatchTypeUtils.GetValue(EMatchType.Exact), keyword, EMatchTypeUtils.GetValue(EMatchType.Contains));

                #region 张浩然 2014-8-25 修改关键词触发规则（全字匹配优先）
                string sqlString = string.Format(@"SELECT TOP 1 KeywordID FROM ( SELECT wx_KeywordMatch.*,'Exact' AS TypeName FROM wx_KeywordMatch WHERE PublishmentSystemID ={0} AND IsDisabled <> 'True' AND MatchType = '{1}' AND Keyword = '{2}' union SELECT wx_KeywordMatch.*,'Contains' AS TypeName FROM wx_KeywordMatch WHERE PublishmentSystemID ={0} AND IsDisabled <> 'True' AND MatchType = '{3}' AND CHARINDEX(Keyword, '{2}') <> 0 ) AS wx_KM", publishmentSystemID, EMatchTypeUtils.GetValue(EMatchType.Exact), keyword, EMatchTypeUtils.GetValue(EMatchType.Contains));
                #endregion

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        keywordID = rdr.GetInt32(0);
                    }
                    rdr.Close();
                }
            }

            return keywordID;
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE PublishmentSystemID = {0}", publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString("wx_KeywordMatch", SqlUtils.Asterisk, whereString);
        }

        public string GetSelectString(int publishmentSystemID, string keywordType, string keyword)
        {
            string whereString = string.Format("WHERE PublishmentSystemID = {0}", publishmentSystemID);
            if (!string.IsNullOrEmpty(keywordType))
            {
                whereString += string.Format(" AND KeywordType = '{0}'", keywordType);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereString += string.Format(" AND Keyword LIKE '%{0}%'", keyword);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString("wx_KeywordMatch", SqlUtils.Asterisk, whereString);
        }

        public string GetSortField()
        {
            return "MatchID";
        }

        public bool IsExists(int publishmentSystemID, string keyword)
        {
            bool isExists = false;

            string sqlString = string.Format("SELECT MatchID FROM wx_KeywordMatch WHERE PublishmentSystemID = {0} AND Keyword = '{1}'", publishmentSystemID, keyword);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }
            return isExists;
        }

        public List<KeywordMatchInfo> GetKeywordMatchInfoList(int publishmentSystemID, int keyWordID)
        {

            List<KeywordMatchInfo> keywordMatchInfoList = new List<KeywordMatchInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}", KeywordMatchAttribute.PublishmentSystemID, publishmentSystemID, KeywordMatchAttribute.KeywordID, keyWordID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, KeywordMatchDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    KeywordMatchInfo keywordMatchInfo = new KeywordMatchInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), EKeywordTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), EMatchTypeUtils.GetEnumType(rdr.GetValue(5).ToString()));
                    keywordMatchInfoList.Add(keywordMatchInfo);
                }
                rdr.Close();
            }

            return keywordMatchInfoList;
        }
    }
}