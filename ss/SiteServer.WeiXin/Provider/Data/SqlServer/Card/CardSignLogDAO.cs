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
    public class CardSignLogDAO : DataProviderBase, ICardSignLogDAO
    {
        private const string TABLE_NAME = "wx_CardSignLog";

        public int Insert(CardSignLogInfo cardSignLogInfo)
        {
            int cardSignLogID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(cardSignLogInfo.ToNameValueCollection(), this.ConnectionString, CardSignLogDAO.TABLE_NAME, out parms);


            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        cardSignLogID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, CardSignLogDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return cardSignLogID;
        }

        public void Update(CardSignLogInfo cardSignLogInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(cardSignLogInfo.ToNameValueCollection(), this.ConnectionString, CardSignLogDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int publishmentSystemID, int cardSignLogID)
        {
            if (cardSignLogID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", CardSignLogDAO.TABLE_NAME, cardSignLogID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> cardSignLogIDList)
        {
            if (cardSignLogIDList != null && cardSignLogIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", CardSignLogDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cardSignLogIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public bool IsSign(int publishmentSystemID, string userName)
        {
            bool isSign = false;

            string SQL_WHERE = string.Format("WHERE PublishmentSystemID ={0} AND UserName = '{1}' AND SignDate > '{2}'", publishmentSystemID, PageUtils.FilterSql(userName), string.Format("{0}-{1}-{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSignLogDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    isSign = true;
                }
                rdr.Close();
            }

            return isSign;
        }

        public CardSignLogInfo GetCardSignLogInfo(int cardSignLogID)
        {
            CardSignLogInfo cardSignLogInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", cardSignLogID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSignLogDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    cardSignLogInfo = new CardSignLogInfo(rdr);
                }
                rdr.Close();
            }

            return cardSignLogInfo;
        }

        public List<CardSignLogInfo> GetCardSignLogInfoList(int publishmentSystemID, string userName)
        {
            List<CardSignLogInfo> cardSignLogInfoList = new List<CardSignLogInfo>();

            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} AND UserName='{1}'", publishmentSystemID, PageUtils.FilterSql(userName));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSignLogDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CardSignLogInfo cardSignLogInfo = new CardSignLogInfo(rdr);
                    cardSignLogInfoList.Add(cardSignLogInfo);
                }
                rdr.Close();
            }

            return cardSignLogInfoList;
        }

        public List<DateTime> GetSignDateList(int publishmentSystemID, string userName)
        {
            List<DateTime> signDateList = new List<DateTime>();

            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} AND UserName='{1}'", publishmentSystemID, PageUtils.FilterSql(userName));
            string SQL_ORDER = string.Format(" ORDER BY SignDate DESC ");
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSignLogDAO.TABLE_NAME, 0, CardSignLogAttribute.SignDate, SQL_WHERE, SQL_ORDER);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    signDateList.Add(ConvertHelper.GetDateTime(rdr.GetValue(0)));
                }
                rdr.Close();
            }

            return signDateList;
        }
        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", CardSignLogAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CardSignLogDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public string GetSignAction()
        {
            return "签到领取积分";
        }
        public List<CardSignLogInfo> GetCardSignLogInfoList(int publishmentSystemID)
        {
            List<CardSignLogInfo> cardSignLogInfoList = new List<CardSignLogInfo>();

            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0}", publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSignLogDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CardSignLogInfo cardSignLogInfo = new CardSignLogInfo(rdr);
                    cardSignLogInfoList.Add(cardSignLogInfo);
                }
                rdr.Close();
            }

            return cardSignLogInfoList;
        }

    }
}
