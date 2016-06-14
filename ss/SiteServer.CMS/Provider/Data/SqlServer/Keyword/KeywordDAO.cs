using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;
using System.Data;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.Data;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class KeywordDAO : DataProviderBase, IKeywordDAO
    {
        private const string PARM_KEYWORD_ID = "@KeywordID";
        private const string PARM_KEYWORD = "@Keyword";
        private const string PARM_ALTERNATIVE = "@Alternative";
        private const string PARM_GRADE = "@Grade";
        private const string PARM_CLASSIFY_ID = "@ClassifyID";

        private const string SQL_UPDATE = "UPDATE siteserver_Keyword SET Keyword=@Keyword,Alternative=@Alternative,Grade=@Grade,ClassifyID=@ClassifyID WHERE KeywordID=@KeywordID";

        private const string SQL_DELETE = "DELETE FROM siteserver_Keyword WHERE KeywordID=@KeywordID";

        private const string SQL_DELETE_CLASSIFY_ID = "DELETE FROM siteserver_Keyword WHERE ClassifyID=@ClassifyID";

        private const string SQL_SELECT = "SELECT KeywordID,Keyword,Alternative,Grade,ClassifyID FROM siteserver_Keyword WHERE KeywordID=@KeywordID";

        private const string SQL_SELECT_ALL = "SELECT KeywordID,Keyword,Alternative,Grade,ClassifyID FROM siteserver_Keyword";

        private const string SQL_SELECT_KEYWORD = "SELECT KeywordID,Keyword,Alternative,Grade,ClassifyID FROM siteserver_Keyword WHERE Keyword = @Keyword";

        public void Insert(KeywordInfo keywordInfo)
        {
            string sqlString = "INSERT INTO siteserver_Keyword(Keyword,Alternative,Grade,ClassifyID) VALUES(@Keyword,@Alternative,@Grade,@ClassifyID)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_Keyword(KeywordID,Keyword,Alternative,Grade,ClassifyID) VALUES(siteserver_Keyword_SEQ.NEXTVAL,@Keyword,@Alternative,@Grade,@ClassifyID)";
            }
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_KEYWORD, EDataType.NVarChar,50, keywordInfo.Keyword),
                this.GetParameter(PARM_ALTERNATIVE, EDataType.NVarChar,50, keywordInfo.Alternative),
                this.GetParameter(PARM_GRADE, EDataType.NVarChar, 50, EKeywordGradeUtils.GetValue(keywordInfo.Grade)),
                this.GetParameter(PARM_CLASSIFY_ID, EDataType.Integer, keywordInfo.ClassifyID)
            };

            this.ExecuteNonQuery(sqlString, parms);

            int contentNum = this.GetCount(keywordInfo.ClassifyID);

            DataProvider.KeywordClassifyDAO.UpdateContentNum(0, keywordInfo.ClassifyID, contentNum);
        }

        public int GetCount(int classifyID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM siteserver_Keyword WHERE (ClassifyID = {0})", classifyID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public void Update(KeywordInfo keywordInfo)
        {
            IDataParameter[] parms = new IDataParameter[]
            {
                this.GetParameter(PARM_KEYWORD, EDataType.NVarChar,50, keywordInfo.Keyword),
                this.GetParameter(PARM_ALTERNATIVE, EDataType.NVarChar,50, keywordInfo.Alternative),
                this.GetParameter(PARM_GRADE, EDataType.NVarChar, 50, EKeywordGradeUtils.GetValue(keywordInfo.Grade)),
                this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordInfo.KeywordID),
                this.GetParameter(PARM_CLASSIFY_ID, EDataType.Integer, keywordInfo.ClassifyID)
            };
            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public KeywordInfo GetKeywordInfo(int keywordID)
        {
            KeywordInfo keywordInfo = new KeywordInfo();

            IDataParameter[] parms = new IDataParameter[]
            {
                this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    keywordInfo = new KeywordInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EKeywordGradeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4));
                }
                rdr.Close();
            }
            return keywordInfo;
        }

        public void Delete(int keywordID)
        {
            KeywordInfo keywordInfo = GetKeywordInfo(keywordID);

            IDataParameter[] parms = new IDataParameter[]
            {
                this.GetParameter(PARM_KEYWORD_ID, EDataType.Integer, keywordID)
            };
            this.ExecuteNonQuery(SQL_DELETE, parms);

            int contentNum = this.GetCount(keywordInfo.ClassifyID);

            DataProvider.KeywordClassifyDAO.UpdateContentNum(0, keywordInfo.ClassifyID, contentNum);
        }

        public void DeleteByClassifyID(int classifyID)
        {
            KeywordInfo keywordInfo = GetKeywordInfo(classifyID);

            IDataParameter[] parms = new IDataParameter[]
            {
                this.GetParameter(PARM_CLASSIFY_ID, EDataType.Integer, classifyID)
            };
            this.ExecuteNonQuery(SQL_DELETE_CLASSIFY_ID, parms);

            int contentNum = this.GetCount(keywordInfo.ClassifyID);

            DataProvider.KeywordClassifyDAO.UpdateContentNum(0, keywordInfo.ClassifyID, contentNum);
        }

        public void Delete(ArrayList idArrayList)
        {
            KeywordInfo keywordInfo = GetKeywordInfo(TranslateUtils.ToInt(idArrayList[0].ToString()));

            string sqlString = string.Format(@"DELETE FROM siteserver_Keyword WHERE KeywordID IN ({0})", TranslateUtils.ObjectCollectionToString(idArrayList));
            this.ExecuteNonQuery(sqlString);

            int contentNum = this.GetCount(keywordInfo.ClassifyID);

            DataProvider.KeywordClassifyDAO.UpdateContentNum(0, keywordInfo.ClassifyID, contentNum);
        }

        public string GetSelectCommand(int classifyID)
        {
            if (classifyID > 0)
                return string.Format("SELECT KeywordID,Keyword,Alternative,Grade,ClassifyID FROM siteserver_Keyword WHERE ClassifyID = {0}", classifyID);
            else
                return SQL_SELECT_ALL;
        }

        public bool IsExists(string keyword)
        {
            bool isExists = false;

            IDataParameter[] parms = new IDataParameter[]
            {
                this.GetParameter(PARM_KEYWORD, EDataType.NVarChar, 50, keyword)
            };
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_KEYWORD, parms))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }
            return isExists;
        }

        public List<KeywordInfo> GetKeywordInfoList(int classifyID)
        {
            List<KeywordInfo> list = new List<KeywordInfo>();
            string sql = string.Empty;
            if (classifyID > 0)
                sql = string.Format("SELECT KeywordID,Keyword,Alternative,Grade,ClassifyID FROM siteserver_Keyword WHERE ClassifyID = {0}", classifyID);
            else
                sql = SQL_SELECT_ALL;
            using (IDataReader rdr = this.ExecuteReader(sql))
            {
                while (rdr.Read())
                {
                    KeywordInfo keywordInfo = new KeywordInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EKeywordGradeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4));
                    list.Add(keywordInfo);
                }
                rdr.Close();
            }
            return list;
        }

        public List<KeywordInfo> GetKeywordInfoList(ArrayList keywords)
        {
            List<KeywordInfo> list = new List<KeywordInfo>();
            string SQL_SELECT_KEYWORDS = string.Format("SELECT KeywordID,Keyword,Alternative,Grade,ClassifyID FROM siteserver_Keyword WHERE Keyword in ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(keywords));
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_KEYWORDS))
            {
                while (rdr.Read())
                {
                    KeywordInfo keywordInfo = new KeywordInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EKeywordGradeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4));
                    list.Add(keywordInfo);
                }
                rdr.Close();
            }
            return list;
        }

        public ArrayList GetKeywordArrayListByContent(string content)
        {
            string sqlString = string.Format("SELECT Keyword FROM siteserver_Keyword WHERE CHARINDEX(Keyword, '{0}') > 0", PageUtils.FilterSql(content));
            return BaiRongDataProvider.DatabaseDAO.GetStringArrayList(sqlString);
        }
    }
}
