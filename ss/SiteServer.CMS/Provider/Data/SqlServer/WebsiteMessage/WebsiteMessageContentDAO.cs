using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class WebsiteMessageContentDAO : DataProviderBase, IWebsiteMessageContentDAO
    {
        public string TableName
        {
            get
            {
                return "siteserver_WebsiteMessageContent";
            }
        }

        public int Insert(WebsiteMessageContentInfo info)
        {
            int contentID = 0;

            info.Taxis = this.GetMaxTaxis(info.WebsiteMessageID, info.ClassifyID) + 1;
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        contentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TableName);

                        //using (IDataReader rdr = this.ExecuteReader(trans, "SELECT @@IDENTITY AS 'ContentID'"))
                        //{
                        //    rdr.Read();
                        //    contentID = Convert.ToInt32(rdr[0].ToString());
                        //    rdr.Close();
                        //}

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }

                int contentNum = this.GetCount(info.ClassifyID);
                WebsiteMessageClassifyInfo classifyInfo = DataProvider.WebsiteMessageClassifyDAO.GetWebsiteMessageClassifyInfo(info.ClassifyID);
                DataProvider.WebsiteMessageClassifyDAO.UpdateContentNum(classifyInfo.PublishmentSystemID, info.ClassifyID, contentNum);
            }

            return contentID;
        }

        public int GetCount(int classifyID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM siteserver_WebsiteMessageContent WHERE (ClassifyID = {0})", classifyID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public void Update(WebsiteMessageContentInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateIsChecked(ArrayList contentIDArrayList)
        {
            string sqlString = string.Format("UPDATE {0} SET IsChecked = '{1}' WHERE ID IN ({2})", TableName, true.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));

            this.ExecuteNonQuery(sqlString);
        }

        public bool UpdateTaxisToUp(int websiteMessageID, int classifyID, int contentID)
        {
            string sqlString = string.Empty;
            if (classifyID > 0)
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE ((Taxis > (SELECT Taxis FROM {0} WHERE ID = {1})) AND WebsiteMessageID ={2} AND ClassifyID = {3}) ORDER BY Taxis", TableName, contentID, websiteMessageID, classifyID);
            else
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE ((Taxis > (SELECT Taxis FROM {0} WHERE ID = {1})) AND WebsiteMessageID ={2}) ORDER BY Taxis", TableName, contentID, websiteMessageID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(contentID);

            if (higherID != 0)
            {
                SetTaxis(contentID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int websiteMessageID, int classifyID, int contentID)
        {
            string sqlString = string.Empty;
            if (classifyID > 0)
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE ((Taxis < (SELECT Taxis FROM {0} WHERE (ID = {1}))) AND WebsiteMessageID = {2} AND ClassifyID = {3}) ORDER BY Taxis DESC", TableName, contentID, websiteMessageID, classifyID);
            else
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE ((Taxis < (SELECT Taxis FROM {0} WHERE (ID = {1}))) AND WebsiteMessageID = {2}) ORDER BY Taxis DESC", TableName, contentID, websiteMessageID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(contentID);

            if (lowerID != 0)
            {
                SetTaxis(contentID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        public void Delete(int websiteMessageID, ArrayList deleteIDArrayList)
        {
            WebsiteMessageContentInfo info = new WebsiteMessageContentInfo();
            if (deleteIDArrayList.Count > 0)
            {
                info = this.GetContentInfo(TranslateUtils.ToInt(deleteIDArrayList[0].ToString()));
            }

            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);

            int contentNum = this.GetCount(info.ClassifyID);
            WebsiteMessageClassifyInfo classifyInfo = DataProvider.WebsiteMessageClassifyDAO.GetWebsiteMessageClassifyInfo(info.ClassifyID);
            DataProvider.WebsiteMessageClassifyDAO.UpdateContentNum(classifyInfo.PublishmentSystemID, info.ClassifyID, contentNum);
        }

        public void Check(int websiteMessageID, ArrayList contentIDArrayList)
        {
            string sqlString = string.Format("UPDATE {0} SET IsChecked = '{1}' WHERE ID IN ({2})", TableName, true.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int websiteMessageID, int classifyID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE WebsiteMessageID ={1} AND ClassifyID = {2}", TableName, websiteMessageID, classifyID);
            this.ExecuteNonQuery(sqlString);

            int contentNum = this.GetCount(classifyID);
            WebsiteMessageClassifyInfo classifyInfo = DataProvider.WebsiteMessageClassifyDAO.GetWebsiteMessageClassifyInfo(classifyID);
            DataProvider.WebsiteMessageClassifyDAO.UpdateContentNum(classifyInfo.PublishmentSystemID, classifyID, contentNum);
        }

        public WebsiteMessageContentInfo GetContentInfo(int contentID)
        {
            WebsiteMessageContentInfo info = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", contentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new WebsiteMessageContentInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public int GetCountChecked(int websiteMessageID, int classifyID)
        {
            string sqlString = string.Empty;
            if (classifyID > 0)
                sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (WebsiteMessageID = {1} AND ClassifyID = {4} AND {2} = '{3}')", TableName, websiteMessageID, WebsiteMessageContentAttribute.IsChecked, true.ToString(), classifyID);
            else
                sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (WebsiteMessageID = {1} AND {2} = '{3}')", TableName, websiteMessageID, WebsiteMessageContentAttribute.IsChecked, true.ToString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountUnChecked(int websiteMessageID, int classifyID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (WebsiteMessageID = {1} AND {2} = '{3}')", TableName, websiteMessageID, WebsiteMessageContentAttribute.IsChecked, false.ToString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public DataSet GetDataSetNotChecked(int websiteMessageID, int classifyID)
        {
            string whereString = string.Empty;
            if (classifyID > 0)
                whereString = string.Format("WHERE (WebsiteMessageID = {0} AND ClassifyID = {2} AND IsChecked = '{1}') ORDER BY Taxis DESC", websiteMessageID, false.ToString(), classifyID);
            else
                whereString = string.Format("WHERE (WebsiteMessageID = {0} AND IsChecked = '{1}') ORDER BY Taxis DESC", websiteMessageID, false.ToString());
            return GetDataSetByWhereString(whereString);
        }

        public DataSet GetDataSetWithChecked(int websiteMessageID, int classifyID)
        {
            return this.GetDataSetWithChecked(websiteMessageID, classifyID, 0, ETaxisTypeUtils.GetWebsiteMessageContentOrderByString(ETaxisType.OrderByTaxisDesc), "", EScopeType.Self);
        }

        private DataSet GetDataSetByWhereString(string whereString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString);
            return this.ExecuteDataset(SQL_SELECT);
        }

        private DataSet GetDataSetWithChecked(int websiteMessageID, int classifyID, int contentNum, string orderByString, string whereString, EScopeType scopeType)
        {
            string where = string.Empty;
            if (classifyID > 0)
                where = string.Format("WHERE (WebsiteMessageID = {0} AND ClassifyID = {4} AND IsChecked = '{1}' {2}) {3}", websiteMessageID, true.ToString(), whereString, orderByString, classifyID);
            else
                where = string.Format("WHERE (WebsiteMessageID = {0} AND IsChecked = '{1}' {2}) {3}", websiteMessageID, true.ToString(), whereString, orderByString);
            return this.GetDataSetByWhereString(where);
        }

        public IEnumerable GetStlDataSourceChecked(int websiteMessageID, int classifyID, int totalNum, string orderByString, string whereString)
        {
            string sqlWhereString = string.Empty;
            if (classifyID > 0)
                string.Format("WHERE (WebsiteMessageID = {0} AND ClassifyID = {3} AND IsChecked = '{1}' {2})", websiteMessageID, true.ToString(), whereString, classifyID);
            else
                string.Format("WHERE (WebsiteMessageID = {0} AND IsChecked = '{1}' {2})", websiteMessageID, true.ToString(), whereString);
            return GetDataSourceByContentNumAndWhereString(totalNum, sqlWhereString, orderByString);
        }

        private IEnumerable GetDataSourceByContentNumAndWhereString(int totalNum, string whereString, string orderByString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, totalNum, SqlUtils.Asterisk, whereString, orderByString);
            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        public ArrayList GetContentIDArrayListWithChecked(int websiteMessageID, int classifyID)
        {
            ArrayList arraylist = new ArrayList();
            string taxisString = ETaxisTypeUtils.GetWebsiteMessageContentOrderByString(ETaxisType.OrderByTaxisDesc);
            string sqlString = string.Empty;
            if (classifyID > 0)
                sqlString = string.Format("SELECT ID FROM {0} WHERE (WebsiteMessageID = {1} AND ClassifyID = {4} AND IsChecked = '{2}') {3}", TableName, websiteMessageID, true.ToString(), taxisString, classifyID);
            else
                sqlString = string.Format("SELECT ID FROM {0} WHERE (WebsiteMessageID = {1} AND IsChecked = '{2}') {3}", TableName, websiteMessageID, true.ToString(), taxisString);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(contentID);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetContentIDArrayListWithChecked(int websiteMessageID, int classifyID, ArrayList searchFields, string keyword)
        {
            ArrayList contentIDArrayList = new ArrayList();
            string TaxisString = ETaxisTypeUtils.GetWebsiteMessageContentOrderByString(ETaxisType.OrderByTaxisDesc);
            StringBuilder whereStringBuilder = new StringBuilder();
            foreach (string field in searchFields)
            {
                if (!string.IsNullOrEmpty(field))
                {
                    whereStringBuilder.AppendFormat(" {0} LIKE '%{1}%' OR", field, PageUtils.FilterSql(keyword));
                }
            }
            if (whereStringBuilder.Length > 0)
            {
                whereStringBuilder.Remove(whereStringBuilder.Length - 3, 3);
            }

            string sqlString = string.Empty;
            if (classifyID > 0)
                sqlString = string.Format("SELECT ID FROM {0} WHERE (WebsiteMessageID = {1} AND ClassifyID = {5} AND IsChecked = '{2}' AND ({3})) {4}", TableName, websiteMessageID, true.ToString(), whereStringBuilder.ToString(), TaxisString, classifyID);
            else
                sqlString = string.Format("SELECT ID FROM {0} WHERE (WebsiteMessageID = {1} AND IsChecked = '{2}' AND ({3})) {4}", TableName, websiteMessageID, true.ToString(), whereStringBuilder.ToString(), TaxisString);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    contentIDArrayList.Add(contentID);
                }
                rdr.Close();
            }
            return contentIDArrayList;
        }

        public ArrayList GetContentIDArrayListByUserName(string userName)
        {
            ArrayList contentIDArrayList = new ArrayList();

            string sqlString = string.Format("SELECT ID FROM {0} WHERE UserName = @UserName ORDER BY AddDate DESC, ID DESC", TableName);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter("@UserName", EDataType.NVarChar, 255,userName)
            };
            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                while (rdr.Read())
                {
                    int contentID = Convert.ToInt32(rdr[0]);
                    contentIDArrayList.Add(contentID);
                }
                rdr.Close();
            }
            return contentIDArrayList;
        }

        public string GetValue(int contentID, string attributeName)
        {
            string sqlString = string.Format("SELECT [{0}] FROM [{1}] WHERE ([ID] = {2})", attributeName, TableName, contentID);
            return BaiRongDataProvider.DatabaseDAO.GetString(sqlString);
        }

        private int GetTaxis(int contentID)
        {
            string sqlString = string.Format("SELECT Taxis FROM {0} WHERE (ID = {1})", TableName, contentID);
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

        private void SetTaxis(int contentID, int taxis)
        {
            string sqlString = string.Format("UPDATE {0} SET Taxis = {1} WHERE  ID = {2}", TableName, taxis, contentID);
            this.ExecuteNonQuery(sqlString);
        }

        private int GetMaxTaxis(int websiteMessageID, int classifyID)
        {
            string sqlString = string.Empty;
            if (classifyID > 0)
                sqlString = string.Format("SELECT MAX(Taxis) FROM {0} WHERE WebsiteMessageID = {1} AND ClassifyID = {2}", TableName, websiteMessageID, classifyID);
            else
                sqlString = string.Format("SELECT MAX(Taxis) FROM {0} WHERE WebsiteMessageID = {1}", TableName, websiteMessageID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectStringOfContentID(int websiteMessageID, int classifyID, string searchType, string searchWord, string whereString)
        {
            string orderByString = ETaxisTypeUtils.GetWebsiteMessageContentOrderByString(ETaxisType.OrderByTaxisDesc);
            string where = string.Empty;

            StringBuilder whereBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(searchType))
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat("(UserName LIKE '%{0}%' OR EMAIL LIKE '%{0}%')", PageUtils.FilterSql(searchWord));
            }
            else
            {
                bool columnExists = false;
                ArrayList columnNameArrayList = BaiRongDataProvider.TableStructureDAO.GetColumnNameArrayList(TableName);
                foreach (string columnName in columnNameArrayList)
                {
                    if (StringUtils.EqualsIgnoreCase(columnName, searchType))
                    {
                        columnExists = true;
                        whereBuilder.AppendFormat("AND ([{0}] LIKE '%{1}%') ", searchType, searchWord);
                        break;
                    }
                }
                if (!columnExists)
                {
                    whereBuilder.AppendFormat("AND (SettingsXML LIKE '%{0}={1}%') ", searchType, searchWord);
                }
            }

            whereString += whereBuilder.ToString();

            if (classifyID > 0)
                where = string.Format("WHERE (WebsiteMessageID = {0} AND ClassifyID = {3} {1}) {2}", websiteMessageID, string.IsNullOrEmpty(whereString) ? string.Empty : " AND " + whereString, orderByString, classifyID);
            else
                where = string.Format("WHERE (WebsiteMessageID = {0} {1}) {2}", websiteMessageID, string.IsNullOrEmpty(whereString) ? string.Empty : " AND " + whereString, orderByString);


            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, "ID, Taxis", where);
        }

        public string GetSelectSqlStringWithChecked(int publishmentSystemID, int websiteMessageID, int classifyID, bool isReplyExists, bool isReply, int startNum, int totalNum, string whereString, string orderByString, NameValueCollection otherAttributes)
        {
            if (!string.IsNullOrEmpty(whereString) && !StringUtils.StartsWithIgnoreCase(whereString.Trim(), "AND "))
            {
                whereString = "AND " + whereString.Trim();
            }
            string sqlWhereString = string.Empty;

            if (classifyID > 0)
                sqlWhereString = string.Format("WHERE WebsiteMessageID = {0} AND IsChecked = '{1}' AND ClassifyID = {3} {2}", websiteMessageID, true.ToString(), whereString, classifyID);
            else
                sqlWhereString = string.Format("WHERE WebsiteMessageID = {0} AND IsChecked = '{1}' {2}", websiteMessageID, true.ToString(), whereString);
            if (isReplyExists)
            {
                if (isReply)
                {
                    sqlWhereString += string.Format(" AND datalength(Reply) > 0");
                }
                else
                {
                    sqlWhereString += string.Format(" AND datalength(Reply) = 0");
                }
            }
            if (otherAttributes != null && otherAttributes.Count > 0)
            {
                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, publishmentSystemID, websiteMessageID);
                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, TableName, relatedIdentities);
                foreach (TableStyleInfo tableStyleInfo in styleInfoArrayList)
                {
                    if (!string.IsNullOrEmpty(otherAttributes[tableStyleInfo.AttributeName.ToLower()]))
                    {
                        sqlWhereString += string.Format(" AND ({0} like '%{1}={2}%')", WebsiteMessageContentAttribute.SettingsXML, tableStyleInfo.AttributeName, otherAttributes[tableStyleInfo.AttributeName.ToLower()]);
                    }
                }
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);
        }

        public string GetSortFieldName()
        {
            return "Taxis";
        }

        /// <summary>
        /// ×ªÒÆÄÚÈÝ
        /// </summary>
        /// <param name="contentIDArrayList"></param>
        /// <param name="classifyID"></param>
        public void TranslateContent(ArrayList contentIDArrayList, int classifyID)
        {
            WebsiteMessageContentInfo info = new WebsiteMessageContentInfo();
            if (contentIDArrayList.Count > 0)
            {
                info = this.GetContentInfo(TranslateUtils.ToInt(contentIDArrayList[0].ToString()));
            }
            string sqlString = string.Format("UPDATE {0} SET ClassifyID = '{1}' WHERE ID IN ({2})", TableName, classifyID.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDArrayList));

            this.ExecuteNonQuery(sqlString);

            int contentNum = this.GetCount(classifyID);
            WebsiteMessageClassifyInfo classifyInfo = DataProvider.WebsiteMessageClassifyDAO.GetWebsiteMessageClassifyInfo(classifyID);
            DataProvider.WebsiteMessageClassifyDAO.UpdateContentNum(classifyInfo.PublishmentSystemID, classifyID, contentNum);

            if (contentIDArrayList.Count > 0)
            {
                classifyInfo = DataProvider.WebsiteMessageClassifyDAO.GetWebsiteMessageClassifyInfo(info.ClassifyID);
                if (info != null && classifyInfo != null)
                {
                    contentNum = this.GetCount(info.ClassifyID);
                    DataProvider.WebsiteMessageClassifyDAO.UpdateContentNum(classifyInfo.PublishmentSystemID, info.ClassifyID, contentNum);
                }
            }

        }

        public string GetSelectSqlStringWithChecked(int publishmentSystemID, int websiteMessageID, bool isReplyExists, bool isReply, int startNum, int totalNum, string whereString, string orderByString, NameValueCollection otherAttributes)
        {
            if (!string.IsNullOrEmpty(whereString) && !StringUtils.StartsWithIgnoreCase(whereString.Trim(), "AND "))
            {
                whereString = "AND " + whereString.Trim();
            }
            string sqlWhereString = string.Format("WHERE WebsiteMessageID = {0} AND IsChecked = '{1}' {2}", websiteMessageID, true.ToString(), whereString);
            if (isReplyExists)
            {
                if (isReply)
                {
                    sqlWhereString += string.Format(" AND datalength(Reply) > 0");
                }
                else
                {
                    sqlWhereString += string.Format(" AND datalength(Reply) = 0");
                }
            }
            if (otherAttributes != null && otherAttributes.Count > 0)
            {
                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, publishmentSystemID, websiteMessageID);
                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, TableName, relatedIdentities);
                foreach (TableStyleInfo tableStyleInfo in styleInfoArrayList)
                {
                    if (!string.IsNullOrEmpty(otherAttributes[tableStyleInfo.AttributeName.ToLower()]))
                    {
                        sqlWhereString += string.Format(" AND ({0} like '%{1}={2}%')", WebsiteMessageContentAttribute.SettingsXML, tableStyleInfo.AttributeName, otherAttributes[tableStyleInfo.AttributeName.ToLower()]);
                    }
                }
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);
        }
    }
}
