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
    public class ConferenceDAO : DataProviderBase, IConferenceDAO
    {
        private const string TABLE_NAME = "wx_Conference";

        public int Insert(ConferenceInfo conferenceInfo)
        {
            int conferenceID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(conferenceInfo.ToNameValueCollection(), this.ConnectionString, ConferenceDAO.TABLE_NAME, out parms);


            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        conferenceID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, ConferenceDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return conferenceID;
        }

        public void Update(ConferenceInfo conferenceInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(conferenceInfo.ToNameValueCollection(), this.ConnectionString, ConferenceDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void AddUserCount(int conferenceID)
        {
            if (conferenceID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", ConferenceDAO.TABLE_NAME, ConferenceAttribute.UserCount, conferenceID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void AddPVCount(int conferenceID)
        {
            if (conferenceID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", ConferenceDAO.TABLE_NAME, ConferenceAttribute.PVCount, conferenceID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, int conferenceID)
        {
            if (conferenceID > 0)
            {
                List<int> conferenceIDList = new List<int>();
                conferenceIDList.Add(conferenceID);
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(conferenceIDList));

                DataProviderWX.ConferenceContentDAO.DeleteAll(conferenceID);

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", ConferenceDAO.TABLE_NAME, conferenceID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> conferenceIDList)
        {
            if (conferenceIDList != null && conferenceIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(conferenceIDList));

                foreach (int conferenceID in conferenceIDList)
                {
                    DataProviderWX.ConferenceContentDAO.DeleteAll(conferenceID);
                }

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", ConferenceDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(conferenceIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetConferenceIDList(int publishmentSystemID)
        {
            List<int> conferenceIDList = new List<int>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", ConferenceAttribute.PublishmentSystemID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ConferenceDAO.TABLE_NAME, 0, ConferenceAttribute.ID, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    conferenceIDList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return conferenceIDList;
        }

        public void UpdateUserCount(int publishmentSystemID, Dictionary<int, int> conferenceIDWithCount)
        {
            if (conferenceIDWithCount.Count == 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = 0 WHERE {2} = {3}", TABLE_NAME, ConferenceAttribute.UserCount, ConferenceAttribute.PublishmentSystemID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
            else
            {
                List<int> conferenceIDList = this.GetConferenceIDList(publishmentSystemID);
                foreach (int conferenceID in conferenceIDList)
                {
                    if (conferenceIDWithCount.ContainsKey(conferenceID))
                    {
                        string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE ID = {3}", TABLE_NAME, ConferenceAttribute.UserCount, conferenceIDWithCount[conferenceID], conferenceID);
                        this.ExecuteNonQuery(sqlString);
                    }
                    else
                    {
                        string sqlString = string.Format("UPDATE {0} SET {1} = 0 WHERE ID = {2}", TABLE_NAME, ConferenceAttribute.UserCount, conferenceID);
                        this.ExecuteNonQuery(sqlString);
                    }
                }
            }
        }

        private List<int> GetKeywordIDList(List<int> conferenceIDList)
        {
            List<int> keywordIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", ConferenceAttribute.KeywordID, ConferenceDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(conferenceIDList));

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

        public ConferenceInfo GetConferenceInfo(int conferenceID)
        {
            ConferenceInfo conferenceInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", conferenceID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ConferenceDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    conferenceInfo = new ConferenceInfo(rdr);
                }
                rdr.Close();
            }

            return conferenceInfo;
        }

        public List<ConferenceInfo> GetConferenceInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<ConferenceInfo> conferenceInfoList = new List<ConferenceInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", ConferenceAttribute.PublishmentSystemID, publishmentSystemID, ConferenceAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", ConferenceAttribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ConferenceDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    ConferenceInfo conferenceInfo = new ConferenceInfo(rdr);
                    conferenceInfoList.Add(conferenceInfo);
                }
                rdr.Close();
            }

            return conferenceInfoList;
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, ConferenceAttribute.PublishmentSystemID, publishmentSystemID, ConferenceAttribute.IsDisabled, true, ConferenceAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetTitle(int conferenceID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", conferenceID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ConferenceDAO.TABLE_NAME, 0, ConferenceAttribute.Title, SQL_WHERE, null);

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
            string whereString = string.Format("WHERE {0} = {1}", ConferenceAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ConferenceDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<ConferenceInfo> GetConferenceInfoList(int publishmentSystemID)
        {
            List<ConferenceInfo> conferenceInfoList = new List<ConferenceInfo>();

            string SQL_WHERE = string.Format(" AND {0} = {1}", ConferenceAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ConferenceDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    ConferenceInfo conferenceInfo = new ConferenceInfo(rdr);
                    conferenceInfoList.Add(conferenceInfo);
                }
                rdr.Close();
            }

            return conferenceInfoList;
        }

    }
}
