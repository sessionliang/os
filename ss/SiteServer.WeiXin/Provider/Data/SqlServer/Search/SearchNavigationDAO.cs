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
    public class SearchNavigationDAO : DataProviderBase, ISearchNavigationDAO
    {
        private const string TABLE_NAME = "wx_SearchNavigation";

        public int Insert(SearchNavigationInfo searchNavigationInfo)
        {
            int searchNavigationID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(searchNavigationInfo.ToNameValueCollection(), this.ConnectionString, SearchNavigationDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        searchNavigationID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, SearchNavigationDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return searchNavigationID;
        }

        public void Update(SearchNavigationInfo searchNavigationInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(searchNavigationInfo.ToNameValueCollection(), this.ConnectionString, SearchNavigationDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateSearchID(int publishmentSystemID, int searchID)
        {
            if (searchID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE {1} = 0 AND {3} = {4}", TABLE_NAME, SearchNavigationAttribute.SearchID, searchID, SearchNavigationAttribute.PublishmentSystemID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
        }


        public void Delete(int publishmentSystemID, int searchNavigationID)
        {
            if (searchNavigationID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", SearchNavigationDAO.TABLE_NAME, searchNavigationID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> searchNavigationIDList)
        {
            if (searchNavigationIDList != null && searchNavigationIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", SearchNavigationDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(searchNavigationIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAllNotInIDList(int publishmentSystemID, int searchID, List<int> idList)
        {
            if (searchID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2} AND {3} = {4}", SearchNavigationDAO.TABLE_NAME, SearchNavigationAttribute.PublishmentSystemID, publishmentSystemID, SearchNavigationAttribute.SearchID, searchID);
                if (idList != null && idList.Count > 0)
                {
                    sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2} AND {3} = {4} AND ID NOT IN ({5})", SearchNavigationDAO.TABLE_NAME, SearchNavigationAttribute.PublishmentSystemID, publishmentSystemID, SearchNavigationAttribute.SearchID, searchID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idList));
                }
                this.ExecuteNonQuery(sqlString);
            }
        }

        public SearchNavigationInfo GetSearchNavigationInfo(int SearchNavigationID)
        {
            SearchNavigationInfo searchNavigationInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", SearchNavigationID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, SearchNavigationDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    searchNavigationInfo = new SearchNavigationInfo(rdr);
                }
                rdr.Close();
            }

            return searchNavigationInfo;
        }

        public List<SearchNavigationInfo> GetSearchNavigationInfoList(int publishmentSystemID, int searchID)
        {
            List<SearchNavigationInfo> searchNavigationInfoList = new List<SearchNavigationInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}", SearchNavigationAttribute.PublishmentSystemID, publishmentSystemID, SearchNavigationAttribute.SearchID, searchID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    SearchNavigationInfo searchNavigationInfo = new SearchNavigationInfo(rdr);
                    searchNavigationInfoList.Add(searchNavigationInfo);
                }
                rdr.Close();
            }

            return searchNavigationInfoList;
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", SearchNavigationAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(SearchNavigationDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<SearchNavigationInfo> GetSearchNavigationInfoList(int publishmentSystemID)
        {
            List<SearchNavigationInfo> searchNavigationInfoList = new List<SearchNavigationInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", SearchNavigationAttribute.PublishmentSystemID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    SearchNavigationInfo searchNavigationInfo = new SearchNavigationInfo(rdr);
                    searchNavigationInfoList.Add(searchNavigationInfo);
                }
                rdr.Close();
            }

            return searchNavigationInfoList;
        }

    }
}
