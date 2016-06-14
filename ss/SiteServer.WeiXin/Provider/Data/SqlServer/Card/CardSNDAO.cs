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
    public class CardSNDAO : DataProviderBase, ICardSNDAO
    {
        private const string TABLE_NAME = "wx_CardSN";

        public int Insert(CardSNInfo cardSNInfo)
        {
            int cardSNID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(cardSNInfo.ToNameValueCollection(), this.ConnectionString, CardSNDAO.TABLE_NAME, out parms);


            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        cardSNID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, CardSNDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return cardSNID;
        }

        public void Update(CardSNInfo cardSNInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(cardSNInfo.ToNameValueCollection(), this.ConnectionString, CardSNDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateStatus(int cardID, bool isDisabled, List<int> cardSNIDList)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE CardID={3} AND ID IN ({4})  ", TABLE_NAME, CardSNAttribute.IsDisabled, isDisabled, cardID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cardSNIDList));

            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int publishmentSystemID, int cardSNID)
        {
            if (cardSNID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", CardSNDAO.TABLE_NAME, cardSNID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> cardSNIDList)
        {
            if (cardSNIDList != null && cardSNIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", CardSNDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cardSNIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Recharge(int cardSNID, string userName, decimal amount)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = {1}+{2} WHERE ID = {3} AND UserName='{4}' ", TABLE_NAME, CardSNAttribute.Amount, amount, cardSNID, PageUtils.FilterSql(userName));

            this.ExecuteNonQuery(sqlString);
        }

        public void Recharge(int cardSNID, string userName, decimal amount, CardCashLogInfo cardCashInfo, IDbTransaction trans)
        {
            DataProviderWX.CardCashLogDAO.Insert(cardCashInfo);

            string sqlString = string.Format("UPDATE {0} SET {1} = {1}+{2} WHERE ID = {3} AND UserName='{4}' ", TABLE_NAME, CardSNAttribute.Amount, amount, cardSNID, PageUtils.FilterSql(userName));

            this.ExecuteNonQuery(trans, sqlString);

        }

        public void Consume(int cardSNID, string userName, decimal amount)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = {1}-{2} WHERE ID = {3} AND UserName='{4}' ", TABLE_NAME, CardSNAttribute.Amount, amount, cardSNID, PageUtils.FilterSql(userName));

            this.ExecuteNonQuery(sqlString);
        }

        public void Exchange(int cardSNID, string userName, decimal amount)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = {1}+{2} WHERE ID = {3} AND UserName='{4}' ", TABLE_NAME, CardSNAttribute.Amount, amount, cardSNID, PageUtils.FilterSql(userName));

            this.ExecuteNonQuery(sqlString);
        }
        public CardSNInfo GetCardSNInfo(int cardSNID)
        {
            CardSNInfo cardSNInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", cardSNID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    cardSNInfo = new CardSNInfo(rdr);
                }
                rdr.Close();
            }

            return cardSNInfo;
        }

        public CardSNInfo GetCardSNInfo(int publishmentSystemID, int cardID, string cardSN, string userName)
        {
            CardSNInfo cardSNInfo = null;

            string SQL_WHERE = string.Format("WHERE {0} = {1} ", CardSNAttribute.PublishmentSystemID, publishmentSystemID);
            if (cardID > 0)
            {
                SQL_WHERE += string.Format(" AND {0}='{1}'", CardSNAttribute.CardID, cardID);
            }
            if (!string.IsNullOrEmpty(cardSN))
            {
                SQL_WHERE += string.Format(" AND {0}='{1}'", CardSNAttribute.SN, PageUtils.FilterSql(cardSN));
            }
            if (!string.IsNullOrEmpty(userName))
            {
                SQL_WHERE += string.Format(" AND {0}='{1}'", CardSNAttribute.UserName, PageUtils.FilterSql(userName));
            }
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    cardSNInfo = new CardSNInfo(rdr);
                }
                rdr.Close();
            }
            return cardSNInfo;
        }

        public List<CardSNInfo> GetCardSNInfoList(int publishmentSystemID, int cardID)
        {
            List<CardSNInfo> cardSNInfoList = new List<CardSNInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}", CardSNAttribute.PublishmentSystemID, publishmentSystemID, CardSNAttribute.CardID, cardID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CardSNInfo cardSNInfo = new CardSNInfo(rdr);
                    cardSNInfoList.Add(cardSNInfo);
                }
                rdr.Close();
            }
            return cardSNInfoList;
        }

        public ArrayList GetUserNameArrayList(int publishmentSystemID, int cardID, string cardSN, string userName)
        {
            ArrayList userNameArrayList = new ArrayList();

            string SQL_WHERE = string.Format("WHERE {0} = {1} ", CardSNAttribute.PublishmentSystemID, publishmentSystemID);

            if (cardID > 0)
            {
                SQL_WHERE += string.Format(" AND CardID = {0}", cardID);
            }
            if (!string.IsNullOrEmpty(cardSN))
            {
                SQL_WHERE += string.Format(" AND {0}='{1}'", CardSNAttribute.SN, PageUtils.FilterSql(cardSN));
            }
            if (!string.IsNullOrEmpty(userName))
            {
                SQL_WHERE += string.Format(" AND {0} ='{1}'", CardSNAttribute.UserName, PageUtils.FilterSql(userName));
            }
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CardSNInfo cardSNInfo = new CardSNInfo(rdr);
                    if (!userNameArrayList.Contains(cardSNInfo.UserName))
                    {
                        userNameArrayList.Add(cardSNInfo.UserName);
                    }
                }
                rdr.Close();
            }
            return userNameArrayList;
        }

        public bool isExists(int publishmentSystemID, int cardID, string userName)
        {
            bool isExist = false;

            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0}", publishmentSystemID);
            if (cardID > 0)
            {
                SQL_WHERE += string.Format(" AND CardID={0}", cardID);
            }
            if (!string.IsNullOrEmpty(userName))
            {
                SQL_WHERE += string.Format(" AND UserName='{0}'", PageUtils.FilterSql(userName));
            }
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

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

        public decimal GetAmount(int cardSNID, string userName)
        {
            decimal amount = 0;

            string SQL_WHERE = string.Format("WHERE ID = {0} AND userName='{1}'", cardSNID, PageUtils.FilterSql(userName));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    CardSNInfo cardSNInfo = new CardSNInfo(rdr);
                    amount = cardSNInfo.Amount;
                }
                rdr.Close();
            }
            return amount;
        }

        public string GetNextCardSN(int publishmentSystemID, int cardID)
        {
            string cardSN = string.Empty;
            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2}={3}", CardSNAttribute.PublishmentSystemID, publishmentSystemID, CardSNAttribute.CardID, cardID);
            string SQL_ORDER = string.Format(" ORDER BY AddDate {0}", "DESC");
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardSNDAO.TABLE_NAME, 0, "SN", SQL_WHERE, SQL_ORDER);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    cardSN = ConvertHelper.GetString(rdr.GetValue(0));
                }
                rdr.Close();
            }
            if (string.IsNullOrEmpty(cardSN))
            {
                cardSN = "100001";
            }
            else
            {
                int curCardSN = Convert.ToInt32(cardSN);
                string nextCardSN = (curCardSN + 1).ToString();

                int len = cardSN.Length;
                int i = nextCardSN.Length;
                while (i < len)
                {
                    nextCardSN = "0" + nextCardSN;
                    i++;
                }
                return nextCardSN;
            }
            return cardSN;
        }


        public string GetSelectString(int publishmentSystemID, int cardID, string cardSN, string userName, string mobile)
        {
            string whereString = string.Format("WHERE {0} = {1} AND {2}={3}", CardSNAttribute.PublishmentSystemID, publishmentSystemID, CardSNAttribute.CardID, cardID);
            if (!string.IsNullOrEmpty(cardSN))
            {
                whereString += string.Format(" AND {0}='{1}'", CardSNAttribute.SN, PageUtils.FilterSql(cardSN));
            }
            if (!string.IsNullOrEmpty(userName))
            {
                whereString += string.Format(" AND {0}='{1}'", CardSNAttribute.UserName, PageUtils.FilterSql(userName));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                whereString += string.Format(" AND {0} IN (SELECT UserName FROM bairong_Users WHERE Mobile='{1}')", CardSNAttribute.UserName, PageUtils.FilterSql(mobile));
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CardSNDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

    }
}
