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
    public class KeywordDAO : DataProviderBase, IKeywordDAO
    {

        private const string SQL_UPDATE = "UPDATE wx_Keyword SET PublishmentSystemID = @PublishmentSystemID, Keywords = @Keywords, IsDisabled = @IsDisabled, KeywordType = @KeywordType, MatchType = @MatchType, Reply = @Reply, AddDate = @AddDate, Taxis = @Taxis WHERE KeywordID = @KeywordID";

        private const string SQL_UPDATE_KEYWRODS_AND_ISDISABLED = "UPDATE wx_Keyword SET Keywords = @Keywords, IsDisabled = @IsDisabled WHERE KeywordID = @KeywordID";

        private const string SQL_UPDATE_KEYWRODS = "UPDATE wx_Keyword SET Keywords = @Keywords, KeywordType = @KeywordType WHERE KeywordID = @KeywordID";

        private const string SQL_DELETE = "DELETE FROM wx_Keyword WHERE KeywordID = @KeywordID";

        private const string SQL_SELECT = "SELECT KeywordID, PublishmentSystemID, Keywords, IsDisabled, KeywordType, MatchType, Reply, AddDate, Taxis FROM wx_Keyword WHERE KeywordID = @KeywordID";

        private const string SQL_SELECT_AVALIABLE = "SELECT TOP 1 KeywordID, PublishmentSystemID, Keywords, IsDisabled, KeywordType, MatchType, Reply, AddDate, Taxis FROM wx_Keyword WHERE PublishmentSystemID = @PublishmentSystemID AND IsDisabled = @IsDisabled AND KeywordType = @KeywordType";

        private const string SQL_SELECT_KEYWRODS = "SELECT Keywords FROM wx_Keyword WHERE KeywordID = @KeywordID";

        private const string SQL_SELECT_ALL = "SELECT KeywordID, PublishmentSystemID, Keywords, IsDisabled, KeywordType, MatchType, Reply, AddDate, Taxis FROM wx_Keyword WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC";

        private const string SQL_SELECT_ALL_BY_TYPE = "SELECT KeywordID, PublishmentSystemID, Keywords, IsDisabled, KeywordType, MatchType, Reply, AddDate, Taxis FROM wx_Keyword WHERE PublishmentSystemID = @PublishmentSystemID AND KeywordType = @KeywordType ORDER BY Taxis DESC";

        private const string PARM_KEYWORD_ID = "@KeywordID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_KEYWORDS = "@Keywords";
        private const string PARM_IS_DISABLED = "@IsDisabled";
        private const string PARM_KEYWORD_TYPE = "@KeywordType";
        private const string PARM_MATCH_TYPE = "@MatchType";
        private const string PARM_REPLY = "@Reply";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_TAXIS = "@Taxis";

        public int Insert(KeywordInfo keywordInfo)
        {
            int keywordID = 0;

            string sqlString = "INSERT INTO wx_Keyword (PublishmentSystemID, Keywords, IsDisabled, KeywordType, MatchType, Reply, AddDate, Taxis) VALUES (@PublishmentSystemID, @Keywords, @IsDisabled, @KeywordType, @MatchType, @Reply, @AddDate, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO wx_Keyword(KeywordID, PublishmentSystemID, Keywords, IsDisabled, KeywordType, MatchType, Reply, AddDate, Taxis) VALUES (wx_Keyword_SEQ.NEXTVAL, @PublishmentSystemID, @Keywords, @IsDisabled, @KeywordType, @MatchType, @Reply, @AddDate, @Taxis)";
            }

            int taxis = this.GetMaxTaxis(keywordInfo.PublishmentSystemID, keywordInfo.KeywordType) + 1;
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, keywordInfo.PublishmentSystemID),
                this.GetParameter(PARM_KEYWORDS, EDataType.NVarChar, 255, keywordInfo.Keywords),
                this.GetParameter(PARM_IS_DISABLED, EDataType.VarChar, 18, keywordInfo.IsDisabled.ToString()),
                this.GetParameter(PARM_KEYWORD_TYPE, EDataType.VarChar, 50, EKeywordTypeUtils.GetValue(keywordInfo.KeywordType)),
                this.GetParameter(PARM_MATCH_TYPE, EDataType.VarChar, 50, EMatchTypeUtils.GetValue(keywordInfo.MatchType)),
                this.GetParameter(PARM_REPLY, EDataType.NText, keywordInfo.Reply),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, keywordInfo.AddDate),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        keywordID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "wx_Keyword");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            foreach (string str in TranslateUtils.StringCollectionToStringList(keywordInfo.Keywords, ' '))
            {
                string keyword = str.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    DataProviderWX.KeywordMatchDAO.Insert(new KeywordMatchInfo(0, keywordInfo.PublishmentSystemID, keyword, keywordID, keywordInfo.IsDisabled, keywordInfo.KeywordType, keywordInfo.MatchType));
                }
            }

            return keywordID;
        }

        public void Update(KeywordInfo keywordInfo)
        {
            if (keywordInfo != null && keywordInfo.KeywordID > 0)
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
                    this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, keywordInfo.PublishmentSystemID),
                    this.GetParameter(PARM_KEYWORDS, EDataType.NVarChar, 255, keywordInfo.Keywords),
                    this.GetParameter(PARM_IS_DISABLED, EDataType.VarChar, 18, keywordInfo.IsDisabled.ToString()),
                    this.GetParameter(PARM_KEYWORD_TYPE, EDataType.VarChar, 50, EKeywordTypeUtils.GetValue(keywordInfo.KeywordType)),
                    this.GetParameter(PARM_MATCH_TYPE, EDataType.VarChar, 50, EMatchTypeUtils.GetValue(keywordInfo.MatchType)),
                    this.GetParameter(PARM_REPLY, EDataType.NText, keywordInfo.Reply),
                    this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, keywordInfo.AddDate),
                    this.GetParameter(PARM_TAXIS, EDataType.Integer, keywordInfo.Taxis),
                    this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordInfo.KeywordID)
			    };

                this.ExecuteNonQuery(SQL_UPDATE, parms);

                DataProviderWX.KeywordMatchDAO.DeleteByKeywordID(keywordInfo.KeywordID);

                foreach (string str in TranslateUtils.StringCollectionToStringList(keywordInfo.Keywords, ' '))
                {
                    string keyword = str.Trim();
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        DataProviderWX.KeywordMatchDAO.Insert(new KeywordMatchInfo(0, keywordInfo.PublishmentSystemID, keyword, keywordInfo.KeywordID, keywordInfo.IsDisabled, keywordInfo.KeywordType, keywordInfo.MatchType));
                    }
                }
            }
        }

        public void Update(int publishmentSystemID, int keywordID, EKeywordType keywordType, EMatchType matchType, string keywords, bool isDisabled)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_KEYWORDS, EDataType.NVarChar, 255, keywords),
                this.GetParameter(PARM_IS_DISABLED, EDataType.VarChar, 18, isDisabled.ToString()),
                this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_KEYWRODS_AND_ISDISABLED, parms);

            DataProviderWX.KeywordMatchDAO.DeleteByKeywordID(keywordID);

            foreach (string str in TranslateUtils.StringCollectionToStringList(keywords, ' '))
            {
                string keyword = str.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    DataProviderWX.KeywordMatchDAO.Insert(new KeywordMatchInfo(0, publishmentSystemID, keyword, keywordID, isDisabled, keywordType, matchType));
                }
            }
        }

        public void Update(int publishmentSystemID, int keywordID, EKeywordType keywordType, EMatchType matchType, string keywords)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_KEYWORDS, EDataType.NVarChar, 255, keywords),
                this.GetParameter(PARM_KEYWORD_TYPE, EDataType.VarChar, 50, EKeywordTypeUtils.GetValue(keywordType)),
                this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_KEYWRODS, parms);

            DataProviderWX.KeywordMatchDAO.DeleteByKeywordID(keywordID);

            foreach (string str in TranslateUtils.StringCollectionToStringList(keywords, ' '))
            {
                string keyword = str.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    DataProviderWX.KeywordMatchDAO.Insert(new KeywordMatchInfo(0, publishmentSystemID, keyword, keywordID, false, keywordType, matchType));
                }
            }
        }

        public void Delete(int keywordID)
        {
            if (keywordID > 0)
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
			    };

                this.ExecuteNonQuery(SQL_DELETE, parms);
            }
        }

        public void Delete(List<int> keywordIDList)
        {
            if (keywordIDList != null && keywordIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM wx_Keyword WHERE KeywordID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(keywordIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public string GetKeywords(int keywordID)
        {
            string keywords = string.Empty;

            if (keywordID > 0)
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
			    };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_KEYWRODS, parms))
                {
                    if (rdr.Read())
                    {
                        keywords = rdr.GetValue(0).ToString();
                    }
                    rdr.Close();
                }
            }

            return keywords;
        }

        public KeywordInfo GetKeywordInfo(int keywordID)
        {
            KeywordInfo keywordInfo = null;

            if (keywordID > 0)
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
			    };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
                {
                    if (rdr.Read())
                    {
                        keywordInfo = new KeywordInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), EKeywordTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), EMatchTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), rdr.GetDateTime(7), rdr.GetInt32(8));
                    }
                    rdr.Close();
                }
            }
            else
            {
                keywordInfo = new KeywordInfo();
                keywordInfo.KeywordID = 0;
                keywordInfo.Keywords = "";
            }

            if (keywordInfo == null)
            {
                keywordInfo = new KeywordInfo();
                keywordInfo.KeywordID = 0;
                keywordInfo.Keywords = "";
            }
            return keywordInfo;
        }

        public int GetKeywordID(int publishmentSystemID, bool isExists, string keywords, EKeywordType keywordType, int existKeywordID)
        {
            int keywordID = existKeywordID;

            if (isExists)
            {
                if (!string.IsNullOrEmpty(keywords))
                {
                    if (existKeywordID > 0)
                    {
                        DataProviderWX.KeywordDAO.Update(publishmentSystemID, existKeywordID, keywordType, EMatchType.Exact, keywords);
                    }
                    else
                    {
                        KeywordInfo keywordInfo = new KeywordInfo(0, publishmentSystemID, keywords, false, keywordType, EMatchType.Exact, string.Empty, DateTime.Now, 0);
                        keywordID = DataProviderWX.KeywordDAO.Insert(keywordInfo);
                    }
                }
                else
                {
                    if (existKeywordID > 0)
                    {
                        DataProviderWX.KeywordDAO.Delete(existKeywordID);
                        keywordID = 0;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keywords))
                {
                    KeywordInfo keywordInfo = new KeywordInfo(0, publishmentSystemID, keywords, false, keywordType, EMatchType.Exact, string.Empty, DateTime.Now, 0);
                    keywordID = DataProviderWX.KeywordDAO.Insert(keywordInfo);
                }
            }

            return keywordID;
        }

        public KeywordInfo GetAvaliableKeywordInfo(int publishmentSystemID, EKeywordType keywordType)
        {
            KeywordInfo keywordInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_IS_DISABLED, EDataType.VarChar, 18, false.ToString()),
                this.GetParameter(PARM_KEYWORD_TYPE, EDataType.VarChar, 50, EKeywordTypeUtils.GetValue(keywordType))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_AVALIABLE, parms))
            {
                if (rdr.Read())
                {
                    keywordInfo = new KeywordInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), EKeywordTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), EMatchTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), rdr.GetDateTime(7), rdr.GetInt32(8));
                }
                rdr.Close();
            }

            return keywordInfo;
        }

        public IEnumerable GetDataSource(int publishmentSystemID, EKeywordType keywordType)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_KEYWORD_TYPE, EDataType.VarChar, 50, EKeywordTypeUtils.GetValue(keywordType))
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_BY_TYPE, parms);
            return enumerable;
        }

        public int GetCount(int publishmentSystemID, EKeywordType keywordType)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM wx_Keyword WHERE PublishmentSystemID = {0} AND KeywordType = '{1}'", publishmentSystemID, EKeywordTypeUtils.GetValue(keywordType));
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public List<KeywordInfo> GetKeywordInfoList(int publishmentSystemID)
        {
            List<KeywordInfo> list = new List<KeywordInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    KeywordInfo keywordInfo = new KeywordInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), EKeywordTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), EMatchTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), rdr.GetDateTime(7), rdr.GetInt32(8));
                    list.Add(keywordInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<KeywordInfo> GetKeywordInfoList(int publishmentSystemID, EKeywordType keywordType)
        {
            List<KeywordInfo> list = new List<KeywordInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_KEYWORD_TYPE, EDataType.VarChar, 50, EKeywordTypeUtils.GetValue(keywordType))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_BY_TYPE, parms))
            {
                while (rdr.Read())
                {
                    KeywordInfo keywordInfo = new KeywordInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), EKeywordTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), EMatchTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), rdr.GetDateTime(7), rdr.GetInt32(8));
                    list.Add(keywordInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public bool UpdateTaxisToUp(int publishmentSystemID, EKeywordType keywordType, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 KeywordID, Taxis FROM wx_Keyword WHERE (Taxis > (SELECT Taxis FROM wx_Keyword WHERE KeywordID = {0})) AND PublishmentSystemID = {1} AND KeywordType = '{2}' ORDER BY Taxis", keywordID, publishmentSystemID, EKeywordTypeUtils.GetValue(keywordType));
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

            int selectedTaxis = GetTaxis(keywordID);

            if (higherID > 0)
            {
                SetTaxis(keywordID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, EKeywordType keywordType, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 KeywordID, Taxis FROM wx_Keyword WHERE (Taxis < (SELECT Taxis FROM wx_Keyword WHERE KeywordID = {0})) AND PublishmentSystemID = {1} AND KeywordType = '{2}' ORDER BY Taxis DESC", keywordID, publishmentSystemID, EKeywordTypeUtils.GetValue(keywordType));
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

            int selectedTaxis = GetTaxis(keywordID);

            if (lowerID > 0)
            {
                SetTaxis(keywordID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int publishmentSystemID, EKeywordType keywordType)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM wx_Keyword WHERE PublishmentSystemID = {0} AND KeywordType = '{1}'", publishmentSystemID, EKeywordTypeUtils.GetValue(keywordType));
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int keywordID)
        {
            string sqlString = string.Format("SELECT Taxis FROM wx_Keyword WHERE KeywordID = {0}", keywordID);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int keywordID, int taxis)
        {
            string sqlString = string.Format("UPDATE wx_Keyword SET Taxis = {0} WHERE KeywordID = {1}", taxis, keywordID);
            this.ExecuteNonQuery(sqlString);
        }


        public int GetKeywordsIDbyName(int publishmentSystemID, string keywords)
        {
            string sqlString = string.Format("SELECT KeywordID FROM wx_Keyword WHERE Keywords = '{0}' AND PublishmentSystemID = {1}", keywords, publishmentSystemID);
            int keywordID = 0;
            if (!string.IsNullOrEmpty(keywords))
            {
                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read())
                    {
                        keywordID = Convert.ToInt32(rdr[0]);
                    }
                    rdr.Close();
                }
            }
            return keywordID;
        }
    }
}