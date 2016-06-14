using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using BaiRong.Model;
using System.Text;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class SearchDAO : DataProviderBase, ISearchDAO
    {
        private const string TABLE_NAME = "wx_Search";

        public int Insert(SearchInfo searchInfo)
        {
            int searchID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(searchInfo.ToNameValueCollection(), this.ConnectionString, SearchDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        searchID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, SearchDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return searchID;
        }

        public void Update(SearchInfo searchInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(searchInfo.ToNameValueCollection(), this.ConnectionString, SearchDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }
  
        public void AddPVCount(int searchID)
        {
            if (searchID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", SearchDAO.TABLE_NAME, SearchAttribute.PVCount, searchID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, int searchID)
        {
            if (searchID > 0)
            {
                List<int> SearchIDList = new List<int>();
                SearchIDList.Add(searchID);
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(SearchIDList));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", SearchDAO.TABLE_NAME, searchID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> searchIDList)
        {
            if (searchIDList != null && searchIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(searchIDList));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", SearchDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(searchIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> searchIDList)
        {
            List<int> keywordIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", SearchAttribute.KeywordID, SearchDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(searchIDList));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    keywordIDList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return keywordIDList;
        }

        public SearchInfo GetSearchInfo(int SearchID)
        {
            SearchInfo searchInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", SearchID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, SearchDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    searchInfo = new SearchInfo(rdr);
                }
                rdr.Close();
            }

            return searchInfo;
        }

        public List<SearchInfo> GetSearchInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<SearchInfo> searchInfoList = new List<SearchInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", SearchAttribute.PublishmentSystemID, publishmentSystemID, SearchAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", SearchAttribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    SearchInfo searchInfo = new SearchInfo(rdr);
                    searchInfoList.Add(searchInfo);
                }
                rdr.Close();
            }

            return searchInfoList;
        }

        public string GetTitle(int searchID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", searchID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, SearchDAO.TABLE_NAME, 0, SearchAttribute.Title, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    title = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return title;
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", SearchAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(SearchDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, SearchAttribute.PublishmentSystemID, publishmentSystemID, SearchAttribute.IsDisabled, true, SearchAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public List<SearchInfo> GetSearchInfoList(int publishmentSystemID)
        {
            List<SearchInfo> searchInfoList = new List<SearchInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", SearchAttribute.PublishmentSystemID, publishmentSystemID);
            
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    SearchInfo searchInfo = new SearchInfo(rdr);
                    searchInfoList.Add(searchInfo);
                }
                rdr.Close();
            }

            return searchInfoList;
        }
        
    }
}
