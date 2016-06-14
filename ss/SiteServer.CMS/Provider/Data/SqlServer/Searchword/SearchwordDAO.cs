using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class SearchwordDAO : DataProviderBase, ISearchwordDAO
    {
        public void Insert(SearchwordInfo searchwordInfo)
        {
            searchwordInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(searchwordInfo.Attributes, SearchwordInfo.TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    int id = 0;
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);
                        id = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, SearchwordInfo.TableName);
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                    }
                    #region 更新关键词的搜索次数
                    this.UpdateSearchResultCount(id);
                    #endregion
                }
            }
        }

        public void Update(SearchwordInfo searchwordInfo)
        {
            searchwordInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(searchwordInfo.Attributes, SearchwordInfo.TableName, out parms);
            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", SearchwordInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int searchwordID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", SearchwordInfo.TableName, searchwordID);
            this.ExecuteNonQuery(sqlString);
        }

        public bool IsExists(int publishmentSystemID, string searchword)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE Searchword = '{1}' AND PublishmentSystemID = {2}", SearchwordInfo.TableName, PageUtils.FilterSql(searchword), publishmentSystemID);
            object result = this.ExecuteScalar(sqlString);
            int count = 0;
            if (result == null)
                return false;
            else
                return int.TryParse(result.ToString(), out count) && count > 0;
        }

        public SearchwordInfo GetSearchwordInfo(int searchwordID)
        {
            SearchwordInfo searchwordInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", searchwordID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(SearchwordInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    searchwordInfo = new SearchwordInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, searchwordInfo);
                }
                rdr.Close();
            }

            if (searchwordInfo != null) searchwordInfo.AfterExecuteReader();
            return searchwordInfo;
        }

        public SearchwordInfo GetSearchwordInfo(int publishmentSystemID, string searchword)
        {
            SearchwordInfo searchwordInfo = null;
            string SQL_WHERE = string.Format("WHERE Searchword = '{0}' AND PublishmentSystemID = {1}", PageUtils.FilterSql(searchword), publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(SearchwordInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    searchwordInfo = new SearchwordInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, searchwordInfo);
                }
                rdr.Close();
            }

            if (searchwordInfo != null) searchwordInfo.AfterExecuteReader();
            return searchwordInfo;
        }

        public string GetSelectString(int publishmentSystemID, string where)
        {
            StringBuilder whereBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(where))
                whereBuilder.AppendFormat(" WHERE PublishmentSystemID = {1} AND {0} ", where, publishmentSystemID);
            string orderString = String.Format("ORDER BY {0} DESC", this.GetSortFieldName(publishmentSystemID));
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(SearchwordInfo.TableName, SqlUtils.Asterisk, whereBuilder.ToString(), orderString);
        }

        public string GetSortFieldName(int publishmentSystemID)
        {
            SearchwordSettingInfo settingInfo = DataProvider.SearchwordSettingDAO.GetSearchwordSettingInfo(publishmentSystemID);
            if (settingInfo == null)
                settingInfo = new SearchwordSettingInfo();
            return settingInfo.SearchSort;
        }

        public ArrayList GetSearchwordInfoArrayList(int publishmentSystemID, string where)
        {
            SearchwordInfo searchwordInfo = null;
            ArrayList arrayList = new ArrayList();
            string SQL_SELECT = this.GetSelectString(publishmentSystemID, where);
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    searchwordInfo = new SearchwordInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, searchwordInfo);
                    arrayList.Add(searchwordInfo);
                }
                rdr.Close();
            }
            return arrayList;
        }

        public ArrayList GetSearchwordInfoArrayListForRelated(int publishmentSystemID, string searchword)
        {
            SearchwordInfo searchwordInfo = null;
            SearchwordSettingInfo settingInfo = DataProvider.SearchwordSettingDAO.GetSearchwordSettingInfo(publishmentSystemID);
            if (settingInfo == null)
                settingInfo = new SearchwordSettingInfo();

            #region 搜索条件
            StringBuilder builder = new StringBuilder();
            //搜索结果数阈值
            int searchResultCountLimit = settingInfo.SearchResultCountLimit;
            //搜索次数阈值
            int searchCountLimit = settingInfo.SearchCountLimit;
            //输出个数
            int searchOutputLimit = settingInfo.SearchOutputLimit;
            //排序字段
            string sort = settingInfo.SearchSort;
            builder.AppendFormat(@" WHERE PublishmentSystemID = {0} AND Searchword like '%{1}%' ", publishmentSystemID, searchword);
            if (searchResultCountLimit > 0)
                builder.AppendFormat(@" AND SearchResultCount >= {0} ", searchResultCountLimit);
            if (searchCountLimit > 0)
                builder.AppendFormat(@" AND SearchCount >= {0} ", searchCountLimit);
            string orderString = String.Format("ORDER BY {0} DESC", sort);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(SearchwordInfo.TableName, searchOutputLimit, SqlUtils.Asterisk, builder.ToString(), orderString);
            #endregion

            ArrayList arrayList = new ArrayList();
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    searchwordInfo = new SearchwordInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, searchwordInfo);
                    arrayList.Add(searchwordInfo);
                }
                rdr.Close();
            }
            return arrayList;
        }

        public ArrayList GetSearchwordIDArrayList(int publishmentSystemID, string where)
        {
            StringBuilder whereBuilder = new StringBuilder();
            ArrayList arrayList = new ArrayList();
            if (!string.IsNullOrEmpty(where))
                whereBuilder.AppendFormat(" WHERE PublishmentSystemID = {1} AND {0} ", where, publishmentSystemID);
            string orderString = String.Format("ORDER BY {0} DESC", this.GetSortFieldName(publishmentSystemID));
            string SQL_SELECT_ID = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(SearchwordInfo.TableName, SearchwordAttribute.ID, whereBuilder.ToString(), orderString);
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ID))
            {
                while (rdr.Read())
                {
                    arrayList.Add(TranslateUtils.ToInt(rdr[0].ToString()));
                }
                rdr.Close();
            }
            return arrayList;
        }


        public void UpdateSearchResultCount(int id)
        {
            //搜索关键字
            SearchwordInfo searchwordInfo = this.GetSearchwordInfo(id);
            if (searchwordInfo == null)
                return;
            SearchwordSettingInfo settingInfo = DataProvider.SearchwordSettingDAO.GetSearchwordSettingInfo(searchwordInfo.PublishmentSystemID);
            if (settingInfo == null)
                settingInfo = new SearchwordSettingInfo();
            //搜索方式
            bool isAllow = settingInfo.IsAllow;

            //搜索范围InNode,NotInNode
            string inNodeStr = settingInfo.InNode;
            string notInNodeStr = settingInfo.NotInNode;

            //全部内容辅助表
            ArrayList contentTableArray = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.BackgroundContent);

            //更新sql
            StringBuilder builder = new StringBuilder(string.Format(@" UPDATE {0} SET SearchResultCount = ( ", SearchwordInfo.TableName));
            foreach (AuxiliaryTableInfo table in contentTableArray)
            {
                builder.Append(@"Convert(int,(");
                builder.AppendFormat(@" SELECT COUNT(*) FROM {0} WHERE Title like '%{1}%' OR Content like '%{1}%' ", table.TableENName, PageUtils.FilterSql(searchwordInfo.Searchword));
                if (isAllow)
                    builder.AppendFormat(@" AND NodeID IN ({0}) ", inNodeStr);
                else
                    builder.AppendFormat(@" AND NodeID NOT IN ({0}) ", notInNodeStr);
                builder.Append(@"))+");
            }
            if (contentTableArray.Count > 0)
                builder.Length = builder.Length - 1;

            builder.Append(@") ");
            builder.AppendFormat(@" WHERE ID = {0} ", id);

            this.ExecuteNonQuery(builder.ToString());
        }

        public void UpdateSearchResultCount(ArrayList arraylist)
        {
            foreach (int id in arraylist)
            {
                UpdateSearchResultCount(id);
            }
        }

        public void UpdateSearchResultCountAll(int publishmentSystemID)
        {
            ArrayList arraylist = this.GetSearchwordIDArrayList(publishmentSystemID, string.Empty);
            UpdateSearchResultCount(arraylist);
        }
    }
}
