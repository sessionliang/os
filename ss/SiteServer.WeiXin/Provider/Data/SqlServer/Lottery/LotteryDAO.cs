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
    public class LotteryDAO : DataProviderBase, ILotteryDAO
    {
        private const string TABLE_NAME = "wx_Lottery";

        public int Insert(LotteryInfo lotteryInfo)
        {
            int lotteryID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(lotteryInfo.ToNameValueCollection(), this.ConnectionString, LotteryDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        lotteryID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, LotteryDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return lotteryID;
        }

        public void Update(LotteryInfo lotteryInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(lotteryInfo.ToNameValueCollection(), this.ConnectionString, LotteryDAO.TABLE_NAME, out parms);


            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public List<int> GetLotteryIDList(int publishmentSystemID, ELotteryType lotteryType)
        {
            List<int> lotteryIDList = new List<int>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = '{3}'", LotteryAttribute.PublishmentSystemID, publishmentSystemID, LotteryAttribute.LotteryType, ELotteryTypeUtils.GetValue(lotteryType));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryDAO.TABLE_NAME, 0, LotteryAttribute.ID, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    lotteryIDList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return lotteryIDList;
        }

        private List<int> GetLotteryIDList(int publishmentSystemID)
        {
            List<int> lotteryIDList = new List<int>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", LotteryAttribute.PublishmentSystemID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryDAO.TABLE_NAME, 0, LotteryAttribute.ID, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    lotteryIDList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return lotteryIDList;
        }

        public void UpdateUserCount(int publishmentSystemID, Dictionary<int, int> lotteryIDWithCount)
        {
            if (lotteryIDWithCount.Count == 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = 0 WHERE {2} = {3}", TABLE_NAME, LotteryAttribute.UserCount, LotteryAttribute.PublishmentSystemID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
            else
            {
                List<int> lotteryIDList = this.GetLotteryIDList(publishmentSystemID);
                foreach (int lotteryID in lotteryIDList)
                {
                    if (lotteryIDWithCount.ContainsKey(lotteryID))
                    {
                        string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE ID = {3}", TABLE_NAME, LotteryAttribute.UserCount, lotteryIDWithCount[lotteryID], lotteryID);
                        this.ExecuteNonQuery(sqlString);
                    }
                    else
                    {
                        string sqlString = string.Format("UPDATE {0} SET {1} = 0 WHERE ID = {2}", TABLE_NAME, LotteryAttribute.UserCount, lotteryID);
                        this.ExecuteNonQuery(sqlString);
                    }
                }
            }
        }

        public void Delete(int publishmentSystemID, List<int> lotteryIDList)
        {
            if (lotteryIDList != null && lotteryIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(lotteryIDList));

                foreach (int lotteryID in lotteryIDList)
                {
                    DataProviderWX.LotteryAwardDAO.DeleteAll(lotteryID);
                    DataProviderWX.LotteryWinnerDAO.DeleteAll(lotteryID);
                    DataProviderWX.LotteryLogDAO.DeleteAll(lotteryID);
                }

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(lotteryIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> lotteryIDList)
        {
            List<int> keywordIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", LotteryAttribute.KeywordID, TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(lotteryIDList));

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

        public LotteryInfo GetLotteryInfo(int lotteryID)
        {
            LotteryInfo lotteryInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", lotteryID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    lotteryInfo = new LotteryInfo(rdr);
                }
                rdr.Close();
            }

            return lotteryInfo;
        }

        public List<LotteryInfo> GetLotteryInfoListByKeywordID(int publishmentSystemID, ELotteryType lotteryType, int keywordID)
        {
            List<LotteryInfo> lotteryInfoList = new List<LotteryInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = '{3}' AND {4} <> '{5}'", LotteryAttribute.PublishmentSystemID, publishmentSystemID, LotteryAttribute.LotteryType, ELotteryTypeUtils.GetValue(lotteryType), LotteryAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", LotteryAttribute.KeywordID, keywordID);
            }
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    LotteryInfo lotteryInfo = new LotteryInfo(rdr);
                    lotteryInfoList.Add(lotteryInfo);
                }
                rdr.Close();
            }

            return lotteryInfoList;
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, ELotteryType lotteryType, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} = '{4}' AND {5} <> '{6}' AND {7} = {8}", TABLE_NAME, LotteryAttribute.PublishmentSystemID, publishmentSystemID, LotteryAttribute.LotteryType, ELotteryTypeUtils.GetValue(lotteryType), LotteryAttribute.IsDisabled, true, LotteryAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetKeywordID(int lotteryID)
        {
            int keywordID = 0;

            string SQL_WHERE = string.Format("WHERE ID = {0}", lotteryID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryDAO.TABLE_NAME, 0, LotteryAttribute.KeywordID, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    keywordID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return keywordID;
        }

        public string GetSelectString(int publishmentSystemID, ELotteryType lotteryType)
        {
            string whereString = string.Format("WHERE {0} = {1} AND {2} = '{3}'", LotteryAttribute.PublishmentSystemID, publishmentSystemID, LotteryAttribute.LotteryType, ELotteryTypeUtils.GetValue(lotteryType));
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(LotteryDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public void AddUserCount(int lotteryID)
        {
            if (lotteryID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", TABLE_NAME, LotteryAttribute.UserCount, lotteryID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void AddPVCount(int lotteryID)
        {
            if (lotteryID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", TABLE_NAME, LotteryAttribute.PVCount, lotteryID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public List<LotteryInfo> GetLotteryInfoList(int publishmentSystemID)
        {
            List<LotteryInfo> lotteryInfoList = new List<LotteryInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", LotteryAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, LotteryDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    LotteryInfo lotteryInfo = new LotteryInfo(rdr);
                    lotteryInfoList.Add(lotteryInfo);
                }
                rdr.Close();
            }

            return lotteryInfoList;
        }
    }
}
