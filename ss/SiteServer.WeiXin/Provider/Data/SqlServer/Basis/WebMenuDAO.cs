using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using ECountType = SiteServer.WeiXin.Model.ECountType;
using ECountTypeUtils = SiteServer.WeiXin.Model.ECountTypeUtils;
using BaiRong.Model;
using System.Text;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class WebMenuDAO : DataProviderBase, IWebMenuDAO
    {
        private const string TABLE_NAME = "wx_WebMenu";

        public int Insert(WebMenuInfo menuInfo)
        {
            int menuID = 0;

            menuInfo.Taxis = this.GetMaxTaxis(menuInfo.ParentID) + 1;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(menuInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        menuID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return menuID;
        }

        public void Update(WebMenuInfo menuInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(menuInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int menuID)
        {
            if (menuID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", TABLE_NAME, menuID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll(int publishmentSystemID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2}", TABLE_NAME, WebMenuAttribute.PublishmentSystemID, publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
        }

        public WebMenuInfo GetMenuInfo(int menuID)
        {
            WebMenuInfo menuInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", menuID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    menuInfo = new WebMenuInfo(rdr);
                }
                rdr.Close();
            }

            return menuInfo;
        }

        public List<WebMenuInfo> GetMenuInfoList(int publishmentSystemID, int parentID)
        {
            List<WebMenuInfo> menuInfoList = new List<WebMenuInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}", WebMenuAttribute.PublishmentSystemID, publishmentSystemID, WebMenuAttribute.ParentID, parentID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    WebMenuInfo menuInfo = new WebMenuInfo(rdr);
                    menuInfoList.Add(menuInfo);
                }
                rdr.Close();
            }

            return menuInfoList;
        }

        public IEnumerable GetDataSource(int publishmentSystemID, int parentID)
        {
            string whereString = string.Format("WHERE {0} = {1} AND {2} = {3}", WebMenuAttribute.PublishmentSystemID, publishmentSystemID, WebMenuAttribute.ParentID, parentID);
            string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, whereString);

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public int GetCount(int publishmentSystemID, int parentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = {4}", TABLE_NAME, WebMenuAttribute.PublishmentSystemID, publishmentSystemID, WebMenuAttribute.ParentID, parentID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public bool UpdateTaxisToUp(int publishmentSystemID, int parentID, int menuID)
        {
            string sqlString = string.Format("SELECT TOP 1 MenuID, Taxis FROM wx_WebMenu WHERE (Taxis > (SELECT Taxis FROM wx_WebMenu WHERE MenuID = {0} AND ParentID = {1} AND PublishmentSystemID = {2})) AND ParentID = {1} AND PublishmentSystemID = {2} ORDER BY Taxis", menuID, parentID, publishmentSystemID);
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

            int selectedTaxis = GetTaxis(menuID);

            if (higherID > 0)
            {
                SetTaxis(menuID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, int parentID, int menuID)
        {
            string sqlString = string.Format("SELECT TOP 1 MenuID, Taxis FROM wx_WebMenu WHERE (Taxis < (SELECT Taxis FROM wx_WebMenu WHERE MenuID = {0} AND ParentID = {1} AND PublishmentSystemID = {2})) AND ParentID = {1} AND PublishmentSystemID = {2} ORDER BY Taxis DESC", menuID, parentID, publishmentSystemID);
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

            int selectedTaxis = GetTaxis(menuID);

            if (lowerID > 0)
            {
                SetTaxis(menuID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int parentID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM wx_WebMenu WHERE ParentID = {0}", parentID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int menuID)
        {
            string sqlString = string.Format("SELECT Taxis FROM wx_WebMenu WHERE MenuID = {0}", menuID);
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

        private void SetTaxis(int menuID, int taxis)
        {
            string sqlString = string.Format("UPDATE wx_WebMenu SET Taxis = {0} WHERE MenuID = {1}", taxis, menuID);
            this.ExecuteNonQuery(sqlString);
        }

        public void Sync(int publishmentSystemID)
        {
            this.DeleteAll(publishmentSystemID);

            List<MenuInfo> menuInfoList = DataProviderWX.MenuDAO.GetMenuInfoList(publishmentSystemID, 0);
            foreach (MenuInfo menuInfo in menuInfoList)
            {
                ENavigationType navigationType = ENavigationType.Url;
                if (menuInfo.MenuType == EMenuType.Site)
                {
                    navigationType = ENavigationType.Site;
                }
                else if (menuInfo.MenuType == EMenuType.Keyword)
                {
                    navigationType = ENavigationType.Function;
                }
                EKeywordType keywordType = EKeywordType.Text;
                int functionID = 0;
                if (menuInfo.MenuType == EMenuType.Keyword && !string.IsNullOrEmpty(menuInfo.Keyword))
                {
                    int keywordID = DataProviderWX.KeywordMatchDAO.GetKeywordIDByMPController(publishmentSystemID, menuInfo.Keyword);
                    if (keywordID > 0)
                    {
                        KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(keywordID);
                        functionID = KeywordManager.GetFunctionID(keywordInfo);
                    }
                }

                WebMenuInfo webMenuInfo = new WebMenuInfo { PublishmentSystemID = publishmentSystemID, MenuName = menuInfo.MenuName, NavigationType = ENavigationTypeUtils.GetValue(navigationType), Url = menuInfo.Url, ChannelID = menuInfo.ChannelID, ContentID = menuInfo.ContentID, KeywordType = EKeywordTypeUtils.GetValue(keywordType), FunctionID = functionID, ParentID = 0 };

                int menuID = this.Insert(webMenuInfo);

                List<MenuInfo> subMenuInfoList = DataProviderWX.MenuDAO.GetMenuInfoList(publishmentSystemID, menuInfo.MenuID);
                if (subMenuInfoList != null && subMenuInfoList.Count > 0)
                {
                    foreach (MenuInfo subMenuInfo in subMenuInfoList)
                    {
                        navigationType = ENavigationType.Url;
                        if (subMenuInfo.MenuType == EMenuType.Site)
                        {
                            navigationType = ENavigationType.Site;
                        }
                        else if (subMenuInfo.MenuType == EMenuType.Keyword)
                        {
                            navigationType = ENavigationType.Function;
                        }
                        keywordType = EKeywordType.Text;
                        functionID = 0;
                        if (subMenuInfo.MenuType == EMenuType.Keyword && !string.IsNullOrEmpty(subMenuInfo.Keyword))
                        {
                            int keywordID = DataProviderWX.KeywordMatchDAO.GetKeywordIDByMPController(publishmentSystemID, subMenuInfo.Keyword);
                            if (keywordID > 0)
                            {
                                KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(keywordID);
                                functionID = KeywordManager.GetFunctionID(keywordInfo);
                            }
                        }

                        WebMenuInfo subWebMenuInfo = new WebMenuInfo { PublishmentSystemID = publishmentSystemID, MenuName = subMenuInfo.MenuName, NavigationType = ENavigationTypeUtils.GetValue(navigationType), Url = subMenuInfo.Url, ChannelID = subMenuInfo.ChannelID, ContentID = subMenuInfo.ContentID, KeywordType = EKeywordTypeUtils.GetValue(keywordType), FunctionID = functionID, ParentID = menuID };

                        this.Insert(subWebMenuInfo);
                    }
                }
            }
        }

        public List<WebMenuInfo> GetWebMenuInfoList(int publishmentSystemID)
        {
            List<WebMenuInfo> menuInfoList = new List<WebMenuInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND ParentID = 0", WebMenuAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    WebMenuInfo menuInfo = new WebMenuInfo(rdr);
                    menuInfoList.Add(menuInfo);
                }
                rdr.Close();
            }

            return menuInfoList;
        }

    }
}
