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
    public class AppointmentDAO : DataProviderBase, IAppointmentDAO
    {
        private const string TABLE_NAME = "wx_Appointment";
         
        public int Insert(AppointmentInfo appointmentInfo)
        {
            int appointmentID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(appointmentInfo.ToNameValueCollection(), this.ConnectionString, AppointmentDAO.TABLE_NAME, out parms);
             
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        appointmentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, AppointmentDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return appointmentID;
        }

        public void Update(AppointmentInfo appointmentInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(appointmentInfo.ToNameValueCollection(), this.ConnectionString, AppointmentDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void AddUserCount(int appointmentID)
        {
            if (appointmentID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", AppointmentDAO.TABLE_NAME, AppointmentAttribute.UserCount, appointmentID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void UpdateUserCount(int publishmentSystemID, Dictionary<int, int> appointmentIDWithCount)
        {
            if (appointmentIDWithCount.Count == 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = 0 WHERE {2} = {3}", TABLE_NAME, AppointmentAttribute.UserCount, AppointmentAttribute.PublishmentSystemID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
            else
            {
                List<int> appointmentIDList = this.GetAppointmentIDList(publishmentSystemID);
                foreach (int appointmentID in appointmentIDList)
                {
                    if (appointmentIDWithCount.ContainsKey(appointmentID))
                    {
                        string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE ID = {3}", AppointmentDAO.TABLE_NAME, AppointmentAttribute.UserCount, appointmentIDWithCount[appointmentID], appointmentID);
                        this.ExecuteNonQuery(sqlString);
                    }
                    else
                    {
                        string sqlString = string.Format("UPDATE {0} SET {1} = 0 WHERE ID = {2}", TABLE_NAME, AppointmentAttribute.UserCount, appointmentID);
                        this.ExecuteNonQuery(sqlString);
                    }
                }
            }
        }
  
        public void AddPVCount(int appointmentID)
        {
            if (appointmentID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", AppointmentDAO.TABLE_NAME, AppointmentAttribute.PVCount, appointmentID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, int appointmentID)
        {
            if (appointmentID > 0)
            {
                List<int> appointmentIDList = new List<int>();
                appointmentIDList.Add(appointmentID);
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(appointmentIDList));

                DataProviderWX.AppointmentContentDAO.DeleteAll(appointmentID);
                DataProviderWX.AppointmentItemDAO.DeleteAll(appointmentID);

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", AppointmentDAO.TABLE_NAME, appointmentID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> appointmentIDList)
        {
            if (appointmentIDList != null && appointmentIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(appointmentIDList));

                foreach (int appointmentID in appointmentIDList)
                {
                    DataProviderWX.AppointmentContentDAO.DeleteAll(appointmentID);
                    DataProviderWX.AppointmentItemDAO.DeleteAll(appointmentID);
                }

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", AppointmentDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(appointmentIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> appointmentIDList)
        {
            List<int> keywordIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", AppointmentAttribute.KeywordID, AppointmentDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(appointmentIDList));

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

        private List<int> GetAppointmentIDList(int publishmentSystemID)
        {
            List<int> idList = new List<int>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", AppointmentAttribute.PublishmentSystemID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, AppointmentAttribute.ID, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    idList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return idList;
        }

        public AppointmentInfo GetAppointmentInfo(int appointmentID)
        {
            AppointmentInfo appointmentInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", appointmentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    appointmentInfo = new AppointmentInfo(rdr);
                }
                rdr.Close();
            }

            return appointmentInfo;
        }

        public List<AppointmentInfo> GetAppointmentInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<AppointmentInfo> appointmentInfoList = new List<AppointmentInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", AppointmentAttribute.PublishmentSystemID, publishmentSystemID, AppointmentAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", AppointmentAttribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AppointmentInfo appointmentInfo = new AppointmentInfo(rdr);
                    appointmentInfoList.Add(appointmentInfo);
                }
                rdr.Close();
            }

            return appointmentInfoList;
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, AppointmentAttribute.PublishmentSystemID, publishmentSystemID, AppointmentAttribute.IsDisabled, true, AppointmentAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetTitle(int appointmentID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", appointmentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, AppointmentDAO.TABLE_NAME, 0, AppointmentAttribute.Title, SQL_WHERE, null);

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
            string whereString = string.Format("WHERE {0} = {1}", AppointmentAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(AppointmentDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<AppointmentInfo> GetAppointmentInfoList(int publishmentSystemID)
        {
            List<AppointmentInfo> appointmentInfoList = new List<AppointmentInfo>();

            string SQL_WHERE = string.Format(" AND {0} = {1}", AppointmentAttribute.PublishmentSystemID, publishmentSystemID);         

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    AppointmentInfo appointmentInfo = new AppointmentInfo(rdr);
                    appointmentInfoList.Add(appointmentInfo);
                }
                rdr.Close();
            }

            return appointmentInfoList;
        }
    }
}
