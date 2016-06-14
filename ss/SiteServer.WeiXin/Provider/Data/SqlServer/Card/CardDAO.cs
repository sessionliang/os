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
    public class CardDAO : DataProviderBase, ICardDAO
    {
        private const string TABLE_NAME = "wx_Card";
         
        public int Insert(CardInfo cardInfo)
        {
            int cardID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(cardInfo.ToNameValueCollection(), this.ConnectionString, CardDAO.TABLE_NAME, out parms);
              
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        cardID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, CardDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return cardID;
        }

        public void Update(CardInfo cardInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(cardInfo.ToNameValueCollection(), this.ConnectionString, CardDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }
  
        public void AddPVCount(int cardID)
        {
            if (cardID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", CardDAO.TABLE_NAME, CardAttribute.PVCount, cardID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, int cardID)
        {
            if (cardID > 0)
            {
                List<int> CardIDList = new List<int>();
                CardIDList.Add(cardID);
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(CardIDList));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", CardDAO.TABLE_NAME, cardID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> cardIDList)
        {
            if (cardIDList != null && cardIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(cardIDList));

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", CardDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cardIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> cardIDList)
        {
            List<int> keywordIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", CardAttribute.KeywordID, CardDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cardIDList));

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

        public CardInfo GetCardInfo(int cardID)
        {
            CardInfo CardInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", cardID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    CardInfo = new CardInfo(rdr);
                }
                rdr.Close();
            }

            return CardInfo;
        }

        public List<CardInfo> GetCardInfoList(int publishmentSystemID)
        {
            List<CardInfo> cardInfoList = new List<CardInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", CardAttribute.PublishmentSystemID,publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CardInfo cardInfo = new CardInfo(rdr);
                    cardInfoList.Add(cardInfo);
                }
                rdr.Close();
            }

            return cardInfoList;
        }

        public List<CardInfo> GetCardInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<CardInfo> CardInfoList = new List<CardInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", CardAttribute.PublishmentSystemID, publishmentSystemID, CardAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", CardAttribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CardInfo CardInfo = new CardInfo(rdr);
                    CardInfoList.Add(CardInfo);
                }
                rdr.Close();
            }

            return CardInfoList;
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, CardAttribute.PublishmentSystemID, publishmentSystemID, CardAttribute.IsDisabled, true, CardAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetTitle(int cardID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", cardID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardDAO.TABLE_NAME, 0, CardAttribute.Title, SQL_WHERE, null);

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
            string whereString = string.Format("WHERE {0} = {1}", CardAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CardDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }
    }
}
