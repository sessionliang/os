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
    public class ConferenceContentDAO : DataProviderBase, IConferenceContentDAO
    {
        private const string TABLE_NAME = "wx_ConferenceContent";


        public void Insert(ConferenceContentInfo contentInfo)
        {
            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(contentInfo.ToNameValueCollection(), this.ConnectionString, ConferenceContentDAO.TABLE_NAME, out parms);


            this.ExecuteNonQuery(SQL_INSERT, parms);
        }

        public void DeleteAll(int conferenceID)
        {
            if (conferenceID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2}", ConferenceContentDAO.TABLE_NAME, ConferenceContentAttribute.ConferenceID, conferenceID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> contentIDList)
        {
            if (contentIDList != null && contentIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", ConferenceContentDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDList));
                this.ExecuteNonQuery(sqlString);

                this.UpdateUserCount(publishmentSystemID);
            }
        }

        private void UpdateUserCount(int publishmentSystemID)
        {
            Dictionary<int, int> conferenceIDWithCount = new Dictionary<int, int>();

            string sqlString = string.Format("SELECT {0}, COUNT(*) FROM {1} WHERE {2} = {3} GROUP BY {0}", ConferenceContentAttribute.ConferenceID, TABLE_NAME, ConferenceContentAttribute.PublishmentSystemID, publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    conferenceIDWithCount.Add(rdr.GetInt32(0), rdr.GetInt32(1));
                }
                rdr.Close();
            }

            DataProviderWX.ConferenceDAO.UpdateUserCount(publishmentSystemID, conferenceIDWithCount);
        }

        public int GetCount(int conferenceID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = '{4}'", ConferenceContentDAO.TABLE_NAME, ConferenceContentAttribute.ConferenceID, conferenceID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public bool IsApplied(int conferenceID, string cookieSN, string wxOpenID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2}", ConferenceContentDAO.TABLE_NAME, ConferenceContentAttribute.ConferenceID, conferenceID);

            sqlString += string.Format(" AND ({0} = '{1}'", ConferenceContentAttribute.CookieSN, cookieSN);
            //if (!string.IsNullOrEmpty(wxOpenID))
            //{
            //sqlString += string.Format(" OR {0} = '{1}'", ConferenceContentAttribute.WXOpenID, wxOpenID);

            //sqlString += string.Format(" AND {0} = '{1}'", ConferenceContentAttribute.WXOpenID, wxOpenID);
            //}
            sqlString += ")";

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString) > 0;
        }

        public string GetSelectString(int publishmentSystemID, int conferenceID)
        {
            string whereString = string.Format("WHERE {0} = {1}", ConferenceContentAttribute.PublishmentSystemID, publishmentSystemID);
            if (conferenceID > 0)
            {
                whereString += string.Format(" AND {0} = {1}", ConferenceContentAttribute.ConferenceID, conferenceID);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(ConferenceContentDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<ConferenceContentInfo> GetConferenceContentInfoList(int publishmentSystemID, int conferenceID)
        {
            List<ConferenceContentInfo> conferenceContentInfoList = new List<ConferenceContentInfo>();

            string SQL_WHERE = string.Format(" AND {0} = {1} AND {2} = {3}", ConferenceContentAttribute.PublishmentSystemID, publishmentSystemID, ConferenceContentAttribute.ConferenceID, conferenceID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ConferenceContentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    ConferenceContentInfo conferenceContentInfo = new ConferenceContentInfo(rdr);
                    conferenceContentInfoList.Add(conferenceContentInfo);
                }
                rdr.Close();
            }

            return conferenceContentInfoList;
        }


    }
}
