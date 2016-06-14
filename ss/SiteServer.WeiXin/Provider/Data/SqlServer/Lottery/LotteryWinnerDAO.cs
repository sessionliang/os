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
    public class LotteryWinnerDAO : DataProviderBase, ILotteryWinnerDAO
    {
        private const string TABLE_NAME = "wx_LotteryWinner";

        public int Insert(LotteryWinnerInfo winnerInfo)
        {
            int winnerID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(winnerInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);


            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        winnerID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return winnerID;
        }

        public void UpdateStatus(EWinStatus status, List<int> winnerIDList)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID IN ({3})", TABLE_NAME, LotteryWinnerAttribute.Status, EWinStatusUtils.GetValue(status), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(winnerIDList));

            if (status == EWinStatus.Cashed)
            {
                sqlString = string.Format("UPDATE {0} SET {1} = '{2}', {3} = getdate() WHERE ID IN ({4})", TABLE_NAME, LotteryWinnerAttribute.Status, EWinStatusUtils.GetValue(status), LotteryWinnerAttribute.CashDate, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(winnerIDList));
            }

            this.ExecuteNonQuery(sqlString);
        }

        public void Update(LotteryWinnerInfo winnerInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(winnerInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        private void UpdateUserCount(int publishmentSystemID)
        {
            Dictionary<int, int> lotteryIDWithCount = new Dictionary<int, int>();

            string sqlString = string.Format("SELECT {0}, COUNT(*) FROM {1} WHERE {2} = {3} GROUP BY {0}", LotteryWinnerAttribute.LotteryID, TABLE_NAME, LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    lotteryIDWithCount.Add(rdr.GetInt32(0), rdr.GetInt32(1));
                }
                rdr.Close();
            }

            DataProviderWX.LotteryDAO.UpdateUserCount(publishmentSystemID, lotteryIDWithCount);

        }

        public void Delete(int publishmentSystemID, List<int> winnerIDList)
        {

            List<int> awardIDList = new List<int>();

            if (winnerIDList != null && winnerIDList.Count > 0)
            {

                for (int i = 0; i < winnerIDList.Count; i++)
                {
                    int getAwardID = this.GetWinnerInfo(winnerIDList[i]).AwardID;
                    awardIDList.Add(getAwardID);
                }

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", LotteryWinnerDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(winnerIDList));
                this.ExecuteNonQuery(sqlString);

                this.UpdateUserCount(publishmentSystemID);

                for (int j = 0; j < awardIDList.Count; j++)
                {
                    DataProviderWX.LotteryAwardDAO.UpdateWonNum(awardIDList[j]);
                }
            }

        }

        public void DeleteAll(int lotteryID)
        {
            if (lotteryID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2}", TABLE_NAME, LotteryWinnerAttribute.LotteryID, lotteryID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public LotteryWinnerInfo GetWinnerInfo(int winnerID)
        {
            LotteryWinnerInfo winnerInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", winnerID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryWinnerDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    winnerInfo = new LotteryWinnerInfo(rdr);
                }
                rdr.Close();
            }

            return winnerInfo;
        }

        public LotteryWinnerInfo GetWinnerInfo(int publishmentSystemID, int lotteryID, string cookieSN, string wxOpenID, string userName)
        {

            ///改成 wxopenID

            string SQL_WHERE = "";

            LotteryWinnerInfo winnerInfo = null;

            if (string.IsNullOrEmpty(wxOpenID))
            {
                SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3} AND {4}= '{5}'", LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID, LotteryWinnerAttribute.LotteryID, lotteryID, LotteryWinnerAttribute.CookieSN, cookieSN);
            }
            else
            {
                SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3} AND {4}= '{5}'", LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID, LotteryWinnerAttribute.LotteryID, lotteryID, LotteryWinnerAttribute.WXOpenID, wxOpenID);
            }
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryWinnerDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    winnerInfo = new LotteryWinnerInfo(rdr);
                }
                rdr.Close();
            }

            if (winnerInfo == null)
            {
                //6月30日需要删除
                if (!string.IsNullOrEmpty(wxOpenID))
                {
                    SQL_WHERE += string.Format(" AND {0} = '{1}'", LotteryWinnerAttribute.WXOpenID, wxOpenID);
                }
                else if (!string.IsNullOrEmpty(userName))
                {
                    SQL_WHERE += string.Format(" AND {0} = '{1}'", LotteryWinnerAttribute.UserName, userName);
                }

                SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryWinnerDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                {
                    if (rdr.Read())
                    {
                        winnerInfo = new LotteryWinnerInfo(rdr);
                    }
                    rdr.Close();
                }
            }

            return winnerInfo;
        }

        public List<LotteryWinnerInfo> GetWinnerInfoList(int publishmentSystemID, ELotteryType lotteryType, int lotteryID)
        {
            List<LotteryWinnerInfo> winnerInfoList = new List<LotteryWinnerInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = '{3}'", LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID, LotteryWinnerAttribute.LotteryType, ELotteryTypeUtils.GetValue(lotteryType));
            if (lotteryID > 0)
            {
                SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}", LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID, LotteryWinnerAttribute.LotteryID, lotteryID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryWinnerDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    LotteryWinnerInfo winnerInfo = new LotteryWinnerInfo(rdr);
                    winnerInfoList.Add(winnerInfo);
                }
                rdr.Close();
            }

            return winnerInfoList;
        }

        public int GetTotalNum(int publishmentSystemID, ELotteryType lotteryType, int lotteryID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = '{4}'", TABLE_NAME, LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID, LotteryWinnerAttribute.LotteryType, ELotteryTypeUtils.GetValue(lotteryType));

            if (lotteryID > 0)
            {
                sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = {4}", TABLE_NAME, LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID, LotteryWinnerAttribute.LotteryID, lotteryID);
            }

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetTotalNum(int awardID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2}", TABLE_NAME, LotteryWinnerAttribute.AwardID, awardID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCashNum(int publishmentSystemID, int lotteryID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = {4} AND {5} = '{6}'", TABLE_NAME, LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID, LotteryWinnerAttribute.LotteryID, lotteryID, LotteryWinnerAttribute.Status, EWinStatusUtils.GetValue(EWinStatus.Cashed));
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectString(int publishmentSystemID, ELotteryType lotteryType, int lotteryID, int awardID)
        {
            string whereString = string.Format("WHERE {0} = {1} AND {2} = '{3}'", LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID, LotteryWinnerAttribute.LotteryType, ELotteryTypeUtils.GetValue(lotteryType));
            if (lotteryID > 0)
            {
                whereString = string.Format("WHERE {0} = {1} AND {2} = {3}", LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID, LotteryWinnerAttribute.LotteryID, lotteryID);
            }
            if (awardID > 0)
            {
                whereString = string.Format(" AND {0} = {1}", LotteryWinnerAttribute.AwardID, awardID);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(LotteryWinnerDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<LotteryWinnerInfo> GetLotteryWinnerInfoList(int publishmentSystemID, int lotteryID, int awardID)
        {
            List<LotteryWinnerInfo> lotteryWinnerInfoList = new List<LotteryWinnerInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3} AND {4} = {5}", LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID, LotteryWinnerAttribute.LotteryID, lotteryID, LotteryWinnerAttribute.AwardID, awardID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryWinnerDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    LotteryWinnerInfo lotteryWinnerInfo = new LotteryWinnerInfo(rdr);
                    lotteryWinnerInfoList.Add(lotteryWinnerInfo);
                }
                rdr.Close();
            }

            return lotteryWinnerInfoList;
        }

    }
}
