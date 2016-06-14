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
    public class CardEntitySNDAO : DataProviderBase, ICardEntitySNDAO
    {
        private const string TABLE_NAME = "wx_CardEntitySN";

        public int Insert(CardEntitySNInfo cardEntitySNInfo)
        {
            int cardEntitySNID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(cardEntitySNInfo.ToNameValueCollection(), this.ConnectionString, CardEntitySNDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        cardEntitySNID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, CardEntitySNDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return cardEntitySNID;
        }

        public void Update(CardEntitySNInfo cardEntitySNInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(cardEntitySNInfo.ToNameValueCollection(), this.ConnectionString, CardEntitySNDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateStatus(bool isBinding, List<int> cardSNIDList)
        {
            string sqlString = string.Format("UPDATE {0} SET {1} = '{2}' WHERE ID IN ({3})  ", TABLE_NAME, CardEntitySNAttribute.IsBinding, isBinding, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cardSNIDList));

            this.ExecuteNonQuery(sqlString);
        }
        public void Delete(int publishmentSystemID, int cardEntitySNID)
        {
            if (cardEntitySNID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", CardEntitySNDAO.TABLE_NAME, cardEntitySNID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> cardEntitySNIDList)
        {
            if (cardEntitySNIDList != null && cardEntitySNIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", CardEntitySNDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cardEntitySNIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public CardEntitySNInfo GetCardEntitySNInfo(int cardEntitySNID)
        {
            CardEntitySNInfo cardEntitySNInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", cardEntitySNID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardEntitySNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    cardEntitySNInfo = new CardEntitySNInfo(rdr);
                }
                rdr.Close();
            }

            return cardEntitySNInfo;
        }

        public CardEntitySNInfo GetCardEntitySNInfo(int cardID, string cardSN, string mobile)
        {
            CardEntitySNInfo cardEntitySNInfo = null;

            string SQL_WHERE = string.Format("WHERE {0}= {1}", CardEntitySNAttribute.CardID, cardID);
            if (!string.IsNullOrEmpty(cardSN))
            {
                SQL_WHERE += string.Format(" AND {0}='{1}'", CardEntitySNAttribute.SN, PageUtils.FilterSql(cardSN));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                SQL_WHERE += string.Format(" AND {0}='{1}'", CardEntitySNAttribute.Mobile, PageUtils.FilterSql(mobile));
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardEntitySNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    cardEntitySNInfo = new CardEntitySNInfo(rdr);
                }
                rdr.Close();
            }

            return cardEntitySNInfo;
        }

        public bool IsExist(int publishmentSystemID, int cardID, string cardSN)
        {
            bool isExists = false;

            string SQL_WHERE = string.Format("WHERE {0}= {1} AND {2}={3} AND {4}='{5}' ", CardEntitySNAttribute.PublishmentSystemID, publishmentSystemID, CardEntitySNAttribute.CardID, cardID, CardEntitySNAttribute.SN, PageUtils.FilterSql(cardSN));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardEntitySNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }

            return isExists;
        }

        public bool IsExistMobile(int publishmentSystemID, int cardID, string mobile)
        {
            bool isExists = false;

            string SQL_WHERE = string.Format("WHERE {0}= {1} AND {2}={3} AND {4}='{5}' ", CardEntitySNAttribute.PublishmentSystemID, publishmentSystemID, CardEntitySNAttribute.CardID, cardID, CardEntitySNAttribute.Mobile, PageUtils.FilterSql(mobile));
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardEntitySNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }

            return isExists;
        }

        public string GetSelectString(int publishmentSystemID, int cardID, string cardSN, string userName, string mobile)
        {
            string whereString = string.Format("WHERE {0} = {1} AND {2} = {3}", CardEntitySNAttribute.PublishmentSystemID, publishmentSystemID, CardEntitySNAttribute.CardID, cardID);
            if (!string.IsNullOrEmpty(cardSN))
            {
                whereString += string.Format(" AND {0} ='{1}')", CardEntitySNAttribute.SN, PageUtils.FilterSql(cardSN));
            }
            if (!string.IsNullOrEmpty(userName))
            {
                whereString += string.Format(" AND {0}='{1}'", CardEntitySNAttribute.UserName, PageUtils.FilterSql(userName));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                whereString += string.Format(" AND {0} ='{1}')", CardEntitySNAttribute.Mobile, PageUtils.FilterSql(mobile));
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CardEntitySNDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<CardEntitySNInfo> GetCardEntitySNInfoList(int publishmentSystemID, int cardID)
        {

            List<CardEntitySNInfo> cardEntitySNInfoList = new List<CardEntitySNInfo>();

            string SQL_WHERE = string.Format("WHERE PublishmentSystemID={0} AND CardID = {1}", publishmentSystemID, cardID);

            string SQL_ORDER = string.Format(" ORDER BY {0} DESC", CardEntitySNAttribute.AddDate);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CardEntitySNDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, SQL_ORDER);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CardEntitySNInfo cardEntitySNInfo = new CardEntitySNInfo(rdr);
                    cardEntitySNInfoList.Add(cardEntitySNInfo);
                }
                rdr.Close();
            }

            return cardEntitySNInfoList;
        }
    }
}
