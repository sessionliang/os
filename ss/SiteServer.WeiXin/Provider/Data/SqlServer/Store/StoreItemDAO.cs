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
    public class StoreItemDAO : DataProviderBase, IStoreItemDAO
    {
        private const string TABLE_NAME = "wx_StoreItem";

        public int Insert(StoreItemInfo storeItemInfo)
        {
            int storeItemID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(storeItemInfo.ToNameValueCollection(), this.ConnectionString, StoreItemDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        storeItemID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, StoreItemDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return storeItemID;
        }

        public int Insert(int publishmentSystemID, StoreItemInfo storeItemInfo)
        {
            int storeItemID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(storeItemInfo.ToNameValueCollection(), this.ConnectionString, StoreItemDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        storeItemID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, StoreItemDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            DataProviderWX.StoreCategoryDAO.UpdateStoreItemCount(publishmentSystemID);

            return storeItemID;
        }

        public void Update(int publishmentSystemID, StoreItemInfo storeItemInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(storeItemInfo.ToNameValueCollection(), this.ConnectionString, StoreItemDAO.TABLE_NAME, out parms);
            this.ExecuteNonQuery(SQL_UPDATE, parms);

            DataProviderWX.StoreCategoryDAO.UpdateStoreItemCount(publishmentSystemID);
        }

        public void Delete(int publishmentSystemID, int storeItemID)
        {
            if (storeItemID > 0)
            {
                List<int> categoryIDList = this.GetCategoryIDList(TranslateUtils.ToIntList(storeItemID));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", StoreItemDAO.TABLE_NAME, storeItemID);
                this.ExecuteNonQuery(sqlString);

                DataProviderWX.StoreCategoryDAO.UpdateStoreItemCount(publishmentSystemID);
            }
        }

        public void Delete(int publishmentSystemID, List<int> storeItemIDList)
        {
            if (storeItemIDList != null && storeItemIDList.Count > 0)
            {
                List<int> categoryIDList = this.GetCategoryIDList(storeItemIDList);

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", StoreItemDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(storeItemIDList));
                this.ExecuteNonQuery(sqlString);

                DataProviderWX.StoreCategoryDAO.UpdateStoreItemCount(publishmentSystemID);
            }
        }

        private List<int> GetCategoryIDList(List<int> storeItemIDList)
        {
            List<int> categoryIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", StoreItemAttribute.CategoryID, TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(storeItemIDList));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    categoryIDList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return categoryIDList;
        }

        public StoreItemInfo GetStoreItemInfo(int storeItemID)
        {
            StoreItemInfo storeItemInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", storeItemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    storeItemInfo = new StoreItemInfo(rdr);
                }
                rdr.Close();
            }

            return storeItemInfo;
        }

        public StoreItemInfo GetStoreItemInfoByParentID(int publishmentSystemID, int parentID)
        {
            StoreItemInfo storeItemInfo = null;

            string SQL_WHERE = string.Format("WHERE publishmentSystemID = {0} AND ParentID = {1}", publishmentSystemID, parentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    storeItemInfo = new StoreItemInfo(rdr);
                }
                rdr.Close();
            }

            return storeItemInfo;
        }

        public string GetSelectString(int storeID)
        {
            string whereString = string.Format("WHERE {0} = {1} ", StoreItemAttribute.StoreID, storeID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(StoreItemDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<StoreItemInfo> GetStoreItemInfoListByCategoryID(int publishmentSystemID, int categoryID)
        {
            List<StoreItemInfo> list = new List<StoreItemInfo>();
            StringBuilder builder;
            if (categoryID == 0)
            {
                builder = new StringBuilder(string.Format("WHERE {0} = {1} ", StoreItemAttribute.PublishmentSystemID, publishmentSystemID));
            }
            else
            {
                builder = new StringBuilder(string.Format("WHERE {0} = {1} AND {2} = {3} ", StoreItemAttribute.PublishmentSystemID, publishmentSystemID, StoreItemAttribute.CategoryID, categoryID));
            }
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    StoreItemInfo storeItemInfo = new StoreItemInfo(rdr);
                    list.Add(storeItemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<StoreItemInfo> GetAllStoreItemInfoList(int publishmentSystemID)
        {
            List<StoreItemInfo> list = new List<StoreItemInfo>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = {1}", StoreItemAttribute.PublishmentSystemID, publishmentSystemID));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    StoreItemInfo storeItemInfo = new StoreItemInfo(rdr);
                    list.Add(storeItemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public void DeleteAll(int publishmentSystemID, int storeID)
        {
            if (storeID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2} AND {3} = {4}", StoreItemDAO.TABLE_NAME, StoreItemAttribute.PublishmentSystemID, publishmentSystemID, StoreItemAttribute.StoreID, storeID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public List<StoreItemInfo> GetAllStoreItemInfoListByLocation(int publishmentSystemID, string location_X)
        {
            List<StoreItemInfo> list = new List<StoreItemInfo>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = {1} AND {2} BETWEEN '{3}' AND '{4}'", StoreItemAttribute.PublishmentSystemID, publishmentSystemID, StoreItemAttribute.Latitude, Convert.ToDouble(location_X) - 0.5, Convert.ToDouble(location_X) + 0.5));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    StoreItemInfo storeItemInfo = new StoreItemInfo(rdr);
                    list.Add(storeItemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public int GetCount(int publishmentSystemID, int categoryID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = {4}", TABLE_NAME, StoreItemAttribute.PublishmentSystemID, publishmentSystemID, StoreItemAttribute.CategoryID, categoryID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetAllCount(int publishmentSystemID, int categoryID)
        {
            List<int> categoryIDList = DataProviderWX.StoreCategoryDAO.GetCategoryIDListForLastNode(publishmentSystemID, categoryID);

            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} IN ({4})", TABLE_NAME, StoreItemAttribute.PublishmentSystemID, publishmentSystemID, StoreItemAttribute.CategoryID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(categoryIDList));

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public List<StoreItemInfo> GetStoreItemInfoList(int publishmentSystemID, int storeID)
        {
            List<StoreItemInfo> list = new List<StoreItemInfo>();
            StringBuilder builder;
            builder = new StringBuilder(string.Format("WHERE {0} = {1} AND {2} = {3} ", StoreItemAttribute.PublishmentSystemID, publishmentSystemID, StoreItemAttribute.StoreID, storeID));

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, StoreItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    StoreItemInfo storeItemInfo = new StoreItemInfo(rdr);
                    list.Add(storeItemInfo);
                }
                rdr.Close();
            }

            return list;
        }
    }
}
