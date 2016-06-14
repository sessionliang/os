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
    public class AppointmentItemDAO : DataProviderBase, IAppointmentItemDAO
    {
        private const string TABLE_NAME = "wx_AppointmentItem";

         
        public int Insert(AppointmentItemInfo appointmentItemInfo)
        {
            int appointmentItemID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(appointmentItemInfo.ToNameValueCollection(), this.ConnectionString, AppointmentItemDAO.TABLE_NAME, out parms);
             
            
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        appointmentItemID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, AppointmentItemDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return appointmentItemID;
        }

        public void Update(AppointmentItemInfo appointmentItemInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(appointmentItemInfo.ToNameValueCollection(), this.ConnectionString, AppointmentItemDAO.TABLE_NAME, out parms);
             

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateAppointmentID(int publishmentSystemID, int appointmentID)
        {
            if (appointmentID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE {1} = 0 AND {3} = {4}", AppointmentItemDAO.TABLE_NAME, AppointmentItemAttribute.AppointmentID, appointmentID,AppointmentItemAttribute.PublishmentSystemID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, int appointmentItemID)
        {
            if (appointmentItemID > 0)
            {  
                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", AppointmentItemDAO.TABLE_NAME, appointmentItemID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> appointmentItemIDList)
        {
            if (appointmentItemIDList != null && appointmentItemIDList.Count > 0)
            { 
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", AppointmentItemDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(appointmentItemIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll(int appointmentID)
        {
            if (appointmentID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2}", TABLE_NAME, AppointmentItemAttribute.AppointmentID, appointmentID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void AddUserCount(int itemID)
        {
            if (itemID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", TABLE_NAME, AppointmentItemAttribute.UserCount, itemID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void UpdateUserCount(int publishmentSystemID, Dictionary<int, int> itemIDWithCount)
        {
            if (itemIDWithCount.Count == 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = 0 WHERE {2} = {3}", TABLE_NAME, AppointmentItemAttribute.UserCount, AppointmentItemAttribute.PublishmentSystemID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
            else
            {
                List<int> itemIDList = this.GetItemIDList(publishmentSystemID);
                foreach (int itemID in itemIDList)
                {
                    if (itemIDWithCount.ContainsKey(itemID))
                    {
                        string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE ID = {3}", TABLE_NAME, AppointmentItemAttribute.UserCount, itemIDWithCount[itemID], itemID);
                        this.ExecuteNonQuery(sqlString);
                    }
                    else
                    {
                        string sqlString = string.Format("UPDATE {0} SET {1} = 0 WHERE ID = {2}", TABLE_NAME, AppointmentItemAttribute.UserCount, itemID);
                        this.ExecuteNonQuery(sqlString);
                    }
                }
            }
        }
 
        public AppointmentItemInfo GetItemInfo(int appointmentItemID)
        {
            AppointmentItemInfo appointmentItemInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", appointmentItemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    appointmentItemInfo = new AppointmentItemInfo(rdr);
                }
                rdr.Close();
            }

            return appointmentItemInfo;
        }

        public AppointmentItemInfo GetItemInfo(int publishmentSystemID, int appointmentID)
        {
            AppointmentItemInfo appointmentItemInfo = null;

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}",AppointmentItemAttribute.PublishmentSystemID,publishmentSystemID,AppointmentItemAttribute.AppointmentID, appointmentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    appointmentItemInfo = new AppointmentItemInfo(rdr);
                }
                rdr.Close();
            }

            return appointmentItemInfo;
        }

        public int GetItemID(int publishmentSystemID, int appointmentID)
        {
            int itemID = 0;

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}", AppointmentItemAttribute.PublishmentSystemID, publishmentSystemID, AppointmentItemAttribute.AppointmentID, appointmentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentItemDAO.TABLE_NAME, 0, AppointmentItemAttribute.ID, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    itemID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return itemID;
        }

        private List<int> GetItemIDList(int publishmentSystemID)
        {
            List<int> itemIDList = new List<int>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", AppointmentItemAttribute.PublishmentSystemID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentItemDAO.TABLE_NAME, 0, AppointmentItemAttribute.ID, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    itemIDList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return itemIDList;
        }

        public List<AppointmentItemInfo> GetItemInfoList(int publishmentSystemID, int appointmentID)
        {
            List<AppointmentItemInfo> list = new List<AppointmentItemInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}", AppointmentItemAttribute.PublishmentSystemID, publishmentSystemID, AppointmentItemAttribute.AppointmentID, appointmentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AppointmentItemInfo appointmentItemInfo = new AppointmentItemInfo(rdr);
                    list.Add(appointmentItemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<AppointmentItemInfo> GetItemInfoList(string wxOpenID,string userName)
        {
            List<AppointmentItemInfo> list = new List<AppointmentItemInfo>();

            string SQL_WHERE = string.Format("WHERE  {0} IN (SELECT AppointmentItemID FROM wx_AppointmentContent WHERE UserName = '{1}')", AppointmentItemAttribute.ID,PageUtils.FilterSql(userName));
            if (!string.IsNullOrEmpty(wxOpenID))
            {
                SQL_WHERE = string.Format("WHERE  {0} IN (SELECT AppointmentItemID FROM wx_AppointmentContent WHERE WXOpenID = '{1}')", AppointmentItemAttribute.ID, PageUtils.FilterSql(wxOpenID));
            }
            
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AppointmentItemInfo appointmentItemInfo = new AppointmentItemInfo(rdr);
                    list.Add(appointmentItemInfo);
                }
                rdr.Close();
            }

            return list;
        }

         
        public string GetTitle(int appointmentItemID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", appointmentItemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentItemDAO.TABLE_NAME, 0, AppointmentItemAttribute.Title, SQL_WHERE, null);

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
         
        public string GetSelectString(int publishmentSystemID,int appointmentID)
        {
            string whereString = string.Format("WHERE {0} = {1} AND {2} = {3}", AppointmentItemAttribute.PublishmentSystemID, publishmentSystemID,AppointmentItemAttribute.AppointmentID,appointmentID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(AppointmentItemDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<AppointmentItemInfo> GetAppointmentItemInfoList(int publishmentSystemID)
        {
            List<AppointmentItemInfo> list = new List<AppointmentItemInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", AppointmentItemAttribute.PublishmentSystemID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AppointmentItemInfo appointmentItemInfo = new AppointmentItemInfo(rdr);
                    list.Add(appointmentItemInfo);
                }
                rdr.Close();
            }

            return list;
        }
    }
}
