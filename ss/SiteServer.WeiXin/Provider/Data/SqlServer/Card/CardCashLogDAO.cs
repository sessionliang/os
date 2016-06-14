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
    public class CardCashLogDAO : DataProviderBase, ICardCashLogDAO
    {
        private const string TABLE_NAME = "wx_CardCashLog";
         
        public int Insert(CardCashLogInfo cardCashLogInfo)
        {
            int cardCashLogID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(cardCashLogInfo.ToNameValueCollection(), this.ConnectionString, CardCashLogDAO.TABLE_NAME, out parms);
             
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                         cardCashLogID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, CardCashLogDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return cardCashLogID;
        }

        public void Update(CardCashLogInfo cardCashLogInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(cardCashLogInfo.ToNameValueCollection(), this.ConnectionString, CardCashLogDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }
   
        public void Delete(int publishmentSystemID, int cardCashLogID)
        {
            if (cardCashLogID > 0)
            { 
                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", CardCashLogDAO.TABLE_NAME, cardCashLogID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> cardCashLogIDList)
        {
            if (cardCashLogIDList != null && cardCashLogIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", CardCashLogDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cardCashLogIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }
         
        public CardCashLogInfo GetCardCashLogInfo(int  cardCashLogID)
        {
            CardCashLogInfo cardCashLogInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", cardCashLogID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardCashLogDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    cardCashLogInfo = new CardCashLogInfo(rdr);
                }
                rdr.Close();
            }

            return cardCashLogInfo;
        }

        public List<CardCashLogInfo> GetCardCashLogInfoList(int cardID,int cardSNID,string userName,string startDate,string endDate)
        {
            List<CardCashLogInfo> cardCashLogInfoList = new List<CardCashLogInfo>();

            string SQL_WHERE = string.Format("WHERE CardID = {0} AND CardSNID={1} AND UserName='{2}'",cardID,cardSNID,PageUtils.FilterSql(userName));
            if (!string.IsNullOrEmpty(startDate))
            {
                SQL_WHERE += string.Format(" AND AddDate >='{0}' AND AddDate < '{1}'",startDate,endDate);
            }
            string SQL_ORDER = string.Format(" ORDER BY {0} DESC", CardCashLogAttribute.AddDate);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardCashLogDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, SQL_ORDER);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CardCashLogInfo cardCashLogInfo = new CardCashLogInfo(rdr);
                    cardCashLogInfoList.Add(cardCashLogInfo);
                }
                rdr.Close();
            }

            return cardCashLogInfoList;
        }

        public List<CardCashYearCountInfo> GetCardCashYearCountInfoList(int cardID, int cardSNID,string userName)
        {
            List<CardCashYearCountInfo> cardCashYearCountInfoList = new List<CardCashYearCountInfo>();

            string sqlString = string.Format("select datepart(year,addDate)as 年份,sum( case CashType when 'Consume' then Amount else 0 end)as '消费',sum(case CashType when 'Recharge' then Amount else 0 end )as '充值' ,sum(case CashType when 'Exchange' then Amount else 0 end )as '积分兑换' from wx_CardCashLog  where CardID= {0} and CardSNID={1} and UserName = '{2}'  group by  datepart(year,addDate) order by datepart(year,addDate) desc ",cardID,cardSNID,PageUtils.FilterSql(userName));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    CardCashYearCountInfo cardCashYearCountInfo = new CardCashYearCountInfo();
                    cardCashYearCountInfo.Year =ConvertHelper.GetString( rdr.GetValue(0));
                    cardCashYearCountInfo.TotalConsume = ConvertHelper.GetDecimal(rdr.GetValue(1));
                    cardCashYearCountInfo.TotalRecharge = ConvertHelper.GetDecimal(rdr.GetValue(2));
                    cardCashYearCountInfo.TotalExchange = ConvertHelper.GetDecimal(rdr.GetValue(3));
                    cardCashYearCountInfoList.Add(cardCashYearCountInfo);
                }
                rdr.Close();
            }

            return cardCashYearCountInfoList;
        }

        public List<CardCashMonthCountInfo> GetCardCashMonthCountInfoList(int cardID, int cardSNID, string userName,string year)
        {
            List<CardCashMonthCountInfo> cardCashMonthCountInfoList = new List<CardCashMonthCountInfo>();

            string sqlString = string.Format("select datepart(year,addDate)as 年份, datepart(month,addDate)as 月份,sum( case CashType when 'Consume' then Amount else 0 end)as '消费',sum(case CashType when 'Recharge' then Amount else 0 end )as '充值' ,sum(case CashType when 'Exchange' then Amount else 0 end )as '积分兑换' from wx_CardCashLog where CardID= {0} and  CardSNID= {1} and  UserName='{2}' and AddDate like '%{3}%'  group by datepart(month,addDate), datepart(year,addDate) order by datepart(year,addDate) desc,datepart(month,addDate) desc ", cardID, cardSNID,PageUtils.FilterSql(userName),year);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    CardCashMonthCountInfo cardCashMonthCountInfo = new CardCashMonthCountInfo();
                    cardCashMonthCountInfo.Year = ConvertHelper.GetString(rdr.GetValue(0));
                    cardCashMonthCountInfo.Month = ConvertHelper.GetString(rdr.GetValue(1));
                    cardCashMonthCountInfo.TotalConsume = ConvertHelper.GetDecimal(rdr.GetValue(2));
                    cardCashMonthCountInfo.TotalRecharge = ConvertHelper.GetDecimal(rdr.GetValue(3));
                    cardCashMonthCountInfo.TotalExchange = ConvertHelper.GetDecimal(rdr.GetValue(4));
                    cardCashMonthCountInfoList.Add(cardCashMonthCountInfo);
                }
                rdr.Close();
            }

            return cardCashMonthCountInfoList;
        }

        public string GetSelectString(int publishmentSystemID, ECashType cashType, int cardID, string cardSN, string userName, string mobile)
        {
            string whereString = string.Format("WHERE {0} = {1} AND {2}='{3}'", CardCashLogAttribute.PublishmentSystemID, publishmentSystemID, CardCashLogAttribute.CashType, cashType);
            if (cardID > 0)
            {
                whereString += string.Format(" AND {0}={1}", CardCashLogAttribute.CardID, cardID);
            }
            if (!string.IsNullOrEmpty(cardSN))
            {
                whereString += string.Format(" AND {0} IN (SELECT ID FROM wx_CardSN WHERE SN='{1}')", CardCashLogAttribute.CardSNID, cardSN);
            }
            if (!string.IsNullOrEmpty(userName))
            {
                whereString += string.Format(" AND {0}='{1}'", CardCashLogAttribute.UserName,PageUtils.FilterSql(userName));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                whereString += string.Format(" AND {0} IN (SELECT UserName FROM bairong_Users WHERE Mobile='{1}')", CardCashLogAttribute.UserName, PageUtils.FilterSql(mobile));
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CardCashLogDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<CardCashLogInfo> GetCardCashLogInfoList(int publishmentSystemID, int cardID, int cardSNID)
        {
            List<CardCashLogInfo> cardCashLogInfoList = new List<CardCashLogInfo>();

            string SQL_WHERE = string.Format("WHERE PublishmentSystemID={0} AND CardID = {1} AND CardSNID = {2}", publishmentSystemID, cardID, cardSNID);
            
            string SQL_ORDER = string.Format(" ORDER BY {0} DESC", CardCashLogAttribute.AddDate);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardCashLogDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, SQL_ORDER);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CardCashLogInfo cardCashLogInfo = new CardCashLogInfo(rdr);
                    cardCashLogInfoList.Add(cardCashLogInfo);
                }
                rdr.Close();
            }

            return cardCashLogInfoList;
        }
    }
}
