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
    public class SearchwordSettingDAO : DataProviderBase, ISearchwordSettingDAO
    {
        public void Insert(SearchwordSettingInfo searchwordSettingInfo)
        {
            searchwordSettingInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(searchwordSettingInfo.Attributes, SearchwordSettingInfo.TableName, out parms);
            this.ExecuteNonQuery(SQL_INSERT, parms);
        }

        public void Update(SearchwordSettingInfo searchwordSettingInfo)
        {
            searchwordSettingInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(searchwordSettingInfo.Attributes, SearchwordSettingInfo.TableName, out parms);
            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public bool IsExists(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE PublishmentSystemID = {1}", SearchwordSettingInfo.TableName, publishmentSystemID);
            object result = this.ExecuteScalar(sqlString);
            int count = 0;
            if (result == null)
                return false;
            else
                return int.TryParse(result.ToString(), out count) && count > 0;
        }

        public SearchwordSettingInfo GetSearchwordSettingInfo(int pulishmentSystemID)
        {
            SearchwordSettingInfo searchwordSettingInfo = null;
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0}", pulishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(SearchwordSettingInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    searchwordSettingInfo = new SearchwordSettingInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, searchwordSettingInfo);
                }
                rdr.Close();
            }

            if (searchwordSettingInfo != null)
            {
                searchwordSettingInfo.AfterExecuteReader();
            }
            else
            {
                searchwordSettingInfo = new SearchwordSettingInfo();
                searchwordSettingInfo.InNode = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(pulishmentSystemID));
                searchwordSettingInfo.SearchSort = SearchwordAttribute.SearchResultCount;
            }
            if (string.IsNullOrEmpty(searchwordSettingInfo.InNode) && string.IsNullOrEmpty(searchwordSettingInfo.NotInNode))
            {
                searchwordSettingInfo.InNode = pulishmentSystemID.ToString();
            }
            if (string.IsNullOrEmpty(searchwordSettingInfo.SearchSort))
            {
                searchwordSettingInfo.SearchSort = SearchwordAttribute.SearchResultCount;
            }
            return searchwordSettingInfo;
        }

    }
}
