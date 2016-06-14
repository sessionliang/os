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

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class WebsiteMessageReplayTemplateDAO : DataProviderBase, IWebsiteMessageReplayTemplateDAO
    {
        public void Insert(WebsiteMessageReplayTemplateInfo websiteMessageReplayTemplateInfo)
        {
            websiteMessageReplayTemplateInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(websiteMessageReplayTemplateInfo.Attributes, WebsiteMessageReplayTemplateInfo.TableName, out parms);
            this.ExecuteNonQuery(SQL_INSERT, parms);
        }

        public void Update(WebsiteMessageReplayTemplateInfo websiteMessageReplayTemplateInfo)
        {
            websiteMessageReplayTemplateInfo.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(websiteMessageReplayTemplateInfo.Attributes, WebsiteMessageReplayTemplateInfo.TableName, out parms);
            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", WebsiteMessageReplayTemplateInfo.TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int websiteMessageReplayTemplateID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", WebsiteMessageReplayTemplateInfo.TableName, websiteMessageReplayTemplateID);
            this.ExecuteNonQuery(sqlString);
        }

        public WebsiteMessageReplayTemplateInfo GetWebsiteMessageReplayTemplateInfo(int websiteMessageReplayTemplateID)
        {
            WebsiteMessageReplayTemplateInfo websiteMessageReplayTemplateInfo = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", websiteMessageReplayTemplateID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(WebsiteMessageReplayTemplateInfo.TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    websiteMessageReplayTemplateInfo = new WebsiteMessageReplayTemplateInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, websiteMessageReplayTemplateInfo);
                }
                rdr.Close();
            }

            if (websiteMessageReplayTemplateInfo != null) websiteMessageReplayTemplateInfo.AfterExecuteReader();
            return websiteMessageReplayTemplateInfo;
        }

        public string GetSelectString(string where)
        {
            StringBuilder whereBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(where))
                whereBuilder.AppendFormat(" WHERE {0} ", where);
            string orderString = String.Format("ORDER BY {0} DESC", this.GetSortFieldName());
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(WebsiteMessageReplayTemplateInfo.TableName, SqlUtils.Asterisk, whereBuilder.ToString(), orderString);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }

        public ArrayList GetWebsiteMessageReplayTemplateInfoArrayList(string where)
        {
            WebsiteMessageReplayTemplateInfo websiteMessageReplayTemplateInfo = null;
            ArrayList arrayList = new ArrayList();
            string SQL_SELECT = this.GetSelectString(where);
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    websiteMessageReplayTemplateInfo = new WebsiteMessageReplayTemplateInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, websiteMessageReplayTemplateInfo);
                    arrayList.Add(websiteMessageReplayTemplateInfo);
                }
                rdr.Close();
            }
            return arrayList;
        }
    }
}
