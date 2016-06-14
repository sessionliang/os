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
    public class LotteryLogDAO : DataProviderBase, ILotteryLogDAO
    {
        private const string TABLE_NAME = "wx_LotteryLog";
        public int Insert(LotteryLogInfo lotteryLogInfo)
        {
            int lotteryLogID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(lotteryLogInfo.ToNameValueCollection(), this.ConnectionString, LotteryLogDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        lotteryLogID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, LotteryLogDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return lotteryLogID;
        }

        public void AddCount(int publishmentSystemID, int lotteryID, string cookieSN, string wxOpenID, string userName, int maxCount, int maxDailyCount, out bool isMaxCount, out bool isMaxDailyCount)
        {
            isMaxCount = false;
            isMaxDailyCount = false;

            LotteryLogInfo logInfo = this.GetLogInfo(lotteryID, cookieSN, wxOpenID, userName);
            if (logInfo == null)
            {
                logInfo = new LotteryLogInfo { PublishmentSystemID = publishmentSystemID, LotteryID = lotteryID, CookieSN = cookieSN, WXOpenID = wxOpenID, UserName = userName, LotteryCount = 1, LotteryDailyCount = 1, LastLotteryDate = DateTime.Now };

                IDbDataParameter[] parms = null;

                string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(logInfo.ToNameValueCollection(), this.ConnectionString, LotteryLogDAO.TABLE_NAME, out parms);

                this.ExecuteNonQuery(SQL_INSERT, parms);
            }
            else
            {
                if (maxCount > 0 && logInfo.LotteryCount >= maxCount)
                {
                    isMaxCount = true;
                }
                else
                {
                    bool theSameDay = DateUtils.IsTheSameDay(DateTime.Now, logInfo.LastLotteryDate);
                    if (theSameDay)
                    {
                        if (maxDailyCount > 0 && logInfo.LotteryDailyCount >= maxDailyCount)
                        {
                            isMaxDailyCount = true;
                        }
                    }

                    if (!isMaxDailyCount)
                    {
                        string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1, {2} = 1, {3} = getdate() WHERE ID = {4}", TABLE_NAME, LotteryLogAttribute.LotteryCount, LotteryLogAttribute.LotteryDailyCount, LotteryLogAttribute.LastLotteryDate, logInfo.ID);
                        if (theSameDay)
                        {
                            sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1, {2} = {2} + 1, {3} = getdate() WHERE ID = {4}", TABLE_NAME, LotteryLogAttribute.LotteryCount, LotteryLogAttribute.LotteryDailyCount, LotteryLogAttribute.LastLotteryDate, logInfo.ID);
                        }
                        this.ExecuteNonQuery(sqlString);
                    }
                }
            }
        }

        public void DeleteAll(int lotteryID)
        {
            if (lotteryID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2}", LotteryLogDAO.TABLE_NAME, LotteryLogAttribute.LotteryID, lotteryID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(List<int> logIDList)
        {
            if (logIDList != null && logIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", LotteryLogDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(logIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public int GetCount(int lotteryID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2}", LotteryLogDAO.TABLE_NAME, LotteryLogAttribute.LotteryID, lotteryID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private LotteryLogInfo GetLogInfo(int lotteryID, string cookieSN, string wxOpenID, string userName)
        {
            LotteryLogInfo logInfo = null;

            string SQL_WHERE = string.Format("WHERE {1} = {2} AND {3} = '{4}'", LotteryLogDAO.TABLE_NAME, LotteryLogAttribute.LotteryID, lotteryID, LotteryLogAttribute.CookieSN, cookieSN);
            //if (!string.IsNullOrEmpty(wxOpenID))
            //{
            //    SQL_WHERE += string.Format(" AND {0} = '{1}'", LotteryWinnerAttribute.WXOpenID, wxOpenID);
            //}
            //else if (!string.IsNullOrEmpty(userName))
            //{
            //    SQL_WHERE += string.Format(" AND {0} = '{1}'", LotteryWinnerAttribute.UserName, userName);
            //}

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    logInfo = new LotteryLogInfo(rdr);
                }
                rdr.Close();
            }

            return logInfo;
        }

        public string GetSelectString(int lotteryID)
        {
            string whereString = string.Format("WHERE {0} = {1}", LotteryLogAttribute.LotteryID, lotteryID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(LotteryLogDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<LotteryLogInfo> GetLotteryLogInfoList(int publishmentSystemID, int lotteryID)
        {
            List<LotteryLogInfo> lotteryLogInfoList = new List<LotteryLogInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}", LotteryLogAttribute.PublishmentSystemID, publishmentSystemID, LotteryLogAttribute.LotteryID, lotteryID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryLogDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    LotteryLogInfo lotteryLogInfo = new LotteryLogInfo(rdr);
                    lotteryLogInfoList.Add(lotteryLogInfo);
                }
                rdr.Close();
            }

            return lotteryLogInfoList;
        }

    }
}
