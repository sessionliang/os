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
    public class AppointmentContentDAO : DataProviderBase, IAppointmentContentDAO
    {
        private const string TABLE_NAME = "wx_AppointmentContent";

        public int Insert(AppointmentContentInfo contentInfo)
        {
            int contentID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(contentInfo.ToNameValueCollection(), this.ConnectionString, AppointmentContentDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        contentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, AppointmentContentDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            DataProviderWX.AppointmentItemDAO.AddUserCount(contentInfo.AppointmentItemID);
            DataProviderWX.AppointmentDAO.AddUserCount(contentInfo.AppointmentID);

            return contentID;
        }

        public void Update(AppointmentContentInfo contentInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(contentInfo.ToNameValueCollection(), this.ConnectionString, AppointmentContentDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        private void UpdateUserCount(int publishmentSystemID)
        {
            Dictionary<int, int> itemIDWithCount = new Dictionary<int, int>();

            string sqlString = string.Format("SELECT {0}, COUNT(*) FROM {1} WHERE {2} = {3} GROUP BY {0}", AppointmentContentAttribute.AppointmentItemID, TABLE_NAME, AppointmentContentAttribute.PublishmentSystemID, publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    itemIDWithCount.Add(rdr.GetInt32(0), rdr.GetInt32(1));
                }
                rdr.Close();
            }

            DataProviderWX.AppointmentItemDAO.UpdateUserCount(publishmentSystemID, itemIDWithCount);

            Dictionary<int, int> appointmentIDWithCount = new Dictionary<int, int>();

            sqlString = string.Format("SELECT {0}, COUNT(*) FROM {1} WHERE {2} = {3} GROUP BY {0}", AppointmentContentAttribute.AppointmentID, TABLE_NAME, AppointmentContentAttribute.PublishmentSystemID, publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    appointmentIDWithCount.Add(rdr.GetInt32(0), rdr.GetInt32(1));
                }
                rdr.Close();
            }

            DataProviderWX.AppointmentDAO.UpdateUserCount(publishmentSystemID, appointmentIDWithCount);
        }

        public void Delete(int publishmentSystemID, int contentID)
        {
            if (contentID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", AppointmentContentDAO.TABLE_NAME, contentID);
                this.ExecuteNonQuery(sqlString);

                this.UpdateUserCount(publishmentSystemID);
            }
        }

        public void Delete(int publishmentSystemID, List<int> contentIDList)
        {
            if (contentIDList != null && contentIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", AppointmentContentDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDList));
                this.ExecuteNonQuery(sqlString);

                this.UpdateUserCount(publishmentSystemID);
            }
        }

        public void DeleteAll(int appointmentID)
        {
            if (appointmentID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2}", TABLE_NAME, AppointmentContentAttribute.AppointmentID, appointmentID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public bool IsExist(int itemID, string cookieSN, string wxOpenID, string userName)
        {
            bool isExist = false;

            List<string> statusList = new List<string>();
            statusList.Add(EAppointmentStatusUtils.GetValue(EAppointmentStatus.Handling));
            statusList.Add(EAppointmentStatusUtils.GetValue(EAppointmentStatus.Agree));

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} IN ({3})", AppointmentContentAttribute.AppointmentItemID, itemID, AppointmentContentAttribute.Status, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(statusList));

            SQL_WHERE += string.Format(" AND ({0} = '{1}'", AppointmentContentAttribute.CookieSN, PageUtils.FilterSql(cookieSN));

            if (!string.IsNullOrEmpty(wxOpenID))
            {
                SQL_WHERE += string.Format(" OR {0} = '{1}'", AppointmentContentAttribute.WXOpenID, PageUtils.FilterSql(wxOpenID));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                SQL_WHERE += string.Format(" OR {0} = '{1}'", AppointmentContentAttribute.UserName, PageUtils.FilterSql(userName));
            }

            SQL_WHERE += ")";

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentContentDAO.TABLE_NAME, 0, AppointmentContentAttribute.ID, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    isExist = true;
                }
                rdr.Close();
            }

            return isExist;
        }

        public AppointmentContentInfo GetContentInfo(int contentID)
        {
            AppointmentContentInfo contentInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", contentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentContentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    contentInfo = new AppointmentContentInfo(rdr);
                }
                rdr.Close();
            }

            return contentInfo;
        }

        public AppointmentContentInfo GetLatestContentInfo(int itemID, string cookieSN, string wxOpenID, string userName)
        {
            AppointmentContentInfo contentInfo = null;

            string SQL_WHERE = string.Format("WHERE {0} = {1}", AppointmentContentAttribute.AppointmentItemID, itemID);

            SQL_WHERE += string.Format(" AND ({0} = '{1}'", AppointmentContentAttribute.CookieSN, PageUtils.FilterSql(cookieSN));

            if (!string.IsNullOrEmpty(wxOpenID))
            {
                SQL_WHERE += string.Format(" AND {0} = '{1}'", AppointmentContentAttribute.WXOpenID, PageUtils.FilterSql(wxOpenID));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                SQL_WHERE += string.Format(" AND {0} = '{1}'", AppointmentContentAttribute.UserName, PageUtils.FilterSql(userName));
            }

            SQL_WHERE += ")";

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentContentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    contentInfo = new AppointmentContentInfo(rdr);
                }
                rdr.Close();
            }

            return contentInfo;
        }

        public List<AppointmentContentInfo> GetLatestContentInfoList(int appointmentID, string cookieSN, string wxOpenID, string userName)
        {
            List<AppointmentContentInfo> list = new List<AppointmentContentInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", AppointmentContentAttribute.AppointmentID, appointmentID);

            SQL_WHERE += string.Format(" AND ({0} = '{1}'", AppointmentContentAttribute.CookieSN, PageUtils.FilterSql(cookieSN));

            if (!string.IsNullOrEmpty(wxOpenID))
            {
                SQL_WHERE += string.Format(" AND {0} = '{1}'", AppointmentContentAttribute.WXOpenID, PageUtils.FilterSql(wxOpenID));
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                SQL_WHERE += string.Format(" AND {0} = '{1}'", AppointmentContentAttribute.UserName, PageUtils.FilterSql(userName));
            }

            SQL_WHERE += ")";

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentContentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AppointmentContentInfo contentInfo = new AppointmentContentInfo(rdr);

                    bool isExists = false;
                    foreach (AppointmentContentInfo theContentInfo in list)
                    {
                        if (theContentInfo.AppointmentItemID == contentInfo.AppointmentItemID)
                        {
                            isExists = true;
                        }
                    }

                    if (!isExists)
                    {
                        list.Add(contentInfo);
                    }
                }
                rdr.Close();
            }

            return list;
        }

        public string GetSelectString(int publishmentSystemID, int appointmentID)
        {
            string whereString = string.Format("WHERE {0} = {1}", AppointmentContentAttribute.PublishmentSystemID, publishmentSystemID);
            if (appointmentID > 0)
            {
                whereString += string.Format(" AND {0} = {1}", AppointmentContentAttribute.AppointmentID, appointmentID);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(AppointmentContentDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<AppointmentContentInfo> GetAppointmentContentInfoList(int publishmentSystemID, int appointmentID)
        {
            List<AppointmentContentInfo> appointmentContentInfolList = new List<AppointmentContentInfo>();


            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}", AppointmentContentAttribute.PublishmentSystemID, publishmentSystemID, AppointmentContentAttribute.AppointmentID, appointmentID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentContentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AppointmentContentInfo appointmentContentInfo = new AppointmentContentInfo(rdr);
                    appointmentContentInfolList.Add(appointmentContentInfo);
                }
                rdr.Close();
            }

            return appointmentContentInfolList;
        }

        public List<AppointmentContentInfo> GetAppointmentContentInfoList(int publishmentSystemID, int appointmentID, int appointmentItemID)
        {
            List<AppointmentContentInfo> appointmentContentInfolList = new List<AppointmentContentInfo>();


            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3} AND {4} = {5}", AppointmentContentAttribute.PublishmentSystemID, publishmentSystemID, AppointmentContentAttribute.AppointmentID, appointmentID, AppointmentContentAttribute.AppointmentItemID, appointmentItemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentContentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AppointmentContentInfo appointmentContentInfo = new AppointmentContentInfo(rdr);
                    appointmentContentInfolList.Add(appointmentContentInfo);
                }
                rdr.Close();
            }

            return appointmentContentInfolList;
        }
    }
}
