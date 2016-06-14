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
    public class LotteryAwardDAO : DataProviderBase, ILotteryAwardDAO
    {
        private const string TABLE_NAME = "wx_LotteryAward";
          
        public int Insert(LotteryAwardInfo awardInfo)
        {
            int awardID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(awardInfo.ToNameValueCollection(), this.ConnectionString, LotteryAwardDAO.TABLE_NAME, out parms);
             
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        awardID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, LotteryAwardDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return awardID;
        }

        public void Update(LotteryAwardInfo awardInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(awardInfo.ToNameValueCollection(), this.ConnectionString, LotteryAwardDAO.TABLE_NAME, out parms);


            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateTotalNum(int awardID, int totalNum)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE ID = {3}", TABLE_NAME, LotteryAwardAttribute.TotalNum, totalNum, awardID);

            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateWonNum(int awardID)
        {
            int wonNum = DataProviderWX.LotteryWinnerDAO.GetTotalNum(awardID);
            string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE ID = {3}", TABLE_NAME, LotteryAwardAttribute.WonNum, wonNum, awardID);

            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateLotteryID(int publishmentSystemID, int lotteryID)
        {
            if (lotteryID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE {1} = 0 AND {3} = {4}", TABLE_NAME, LotteryAwardAttribute.LotteryID, lotteryID, LotteryAwardAttribute.PublishmentSystemID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int awardID)
        {
            if (awardID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", LotteryAwardDAO.TABLE_NAME, awardID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(List<int> awardIDList)
        {
            if (awardIDList != null  && awardIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", LotteryAwardDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(awardIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll(int lotteryID)
        {
            if (lotteryID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2}", TABLE_NAME, LotteryAwardAttribute.LotteryID, lotteryID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAllNotInIDList(int publishmentSystemID, int lotteryID, List<int> idList)
        {
            if (lotteryID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2} AND {3} = {4}", LotteryAwardDAO.TABLE_NAME, LotteryAwardAttribute.PublishmentSystemID, publishmentSystemID, LotteryAwardAttribute.LotteryID, lotteryID);
                if (idList != null && idList.Count > 0)
                {
                    sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2} AND {3} = {4} AND ID NOT IN ({5})", LotteryAwardDAO.TABLE_NAME, LotteryAwardAttribute.PublishmentSystemID, publishmentSystemID, LotteryAwardAttribute.LotteryID, lotteryID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idList));
                }
                this.ExecuteNonQuery(sqlString);
            }
        }

        public LotteryAwardInfo GetAwardInfo(int awardID)
        {
            LotteryAwardInfo awardInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", awardID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryAwardDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    awardInfo = new LotteryAwardInfo(rdr);
                }
                rdr.Close();
            }

            return awardInfo;
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", LotteryAwardAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(LotteryAwardDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public Dictionary<string, int> GetLotteryAwardDictionary(int lotteryID)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", LotteryAwardAttribute.LotteryID, lotteryID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, LotteryAwardAttribute.Title + "," + LotteryAwardAttribute.TotalNum, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    dictionary.Add(rdr.GetValue(0).ToString(), rdr.GetInt32(1));
                }
                rdr.Close();
            }

            return dictionary;
        }

        public List<LotteryAwardInfo> GetLotteryAwardInfoList(int publishmentSystemID, int lotteryID)
        {
            List<LotteryAwardInfo> list = new List<LotteryAwardInfo>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = {1} AND {2} = {3}", LotteryAwardAttribute.PublishmentSystemID, publishmentSystemID, LotteryAwardAttribute.LotteryID, lotteryID));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryAwardDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    LotteryAwardInfo awardInfo = new LotteryAwardInfo(rdr);
                    list.Add(awardInfo);
                }
                rdr.Close();
            }

            return list;
        }        

        public void GetCount(int publishmentSystemID, ELotteryType lotteryType, int lotteryID, out int totalNum, out int wonNum)
        {
            totalNum = 0;
            wonNum = 0;

            List<int> lotteryIDList = new List<int>();
            if (lotteryID == 0)
            {
                lotteryIDList = DataProviderWX.LotteryDAO.GetLotteryIDList(publishmentSystemID, lotteryType);
            }
            else
            {
                lotteryIDList.Add(lotteryID);
            }

            string sqlString = string.Format("SELECT SUM(TotalNum), SUM(WonNum) FROM {0} WHERE {1} = {2} AND {3} IN ({4})", TABLE_NAME, LotteryWinnerAttribute.PublishmentSystemID, publishmentSystemID, LotteryWinnerAttribute.LotteryID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(lotteryIDList));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    totalNum = rdr.GetInt32(0);
                    wonNum = rdr.GetInt32(1);
                }
                rdr.Close();
            }
        }

        public List<LotteryAwardInfo> GetLotteryAwardInfoList(int publishmentSystemID)
        {
            List<LotteryAwardInfo> list = new List<LotteryAwardInfo>();

            StringBuilder builder = new StringBuilder(string.Format("WHERE {0} = {1}", LotteryAwardAttribute.PublishmentSystemID, publishmentSystemID));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryAwardDAO.TABLE_NAME, 0, SqlUtils.Asterisk, builder.ToString(), "ORDER BY ID");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    LotteryAwardInfo awardInfo = new LotteryAwardInfo(rdr);
                    list.Add(awardInfo);
                }
                rdr.Close();
            }

            return list;
        }
    }
}
