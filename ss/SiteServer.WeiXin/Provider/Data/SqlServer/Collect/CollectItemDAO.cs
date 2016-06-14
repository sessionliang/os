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
    public class CollectItemDAO : DataProviderBase, ICollectItemDAO
    {
        private const string TABLE_NAME = "wx_CollectItem";

        public int Insert(CollectItemInfo itemInfo)
        {
            int itemID = 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        IDbDataParameter[] parms = null;
                        string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(itemInfo.ToNameValueCollection(), this.ConnectionString, TABLE_NAME, out parms);


                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        itemID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return itemID;
        }

        public void Update(CollectItemInfo itemInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(itemInfo.ToNameValueCollection(), this.ConnectionString, CollectItemDAO.TABLE_NAME, out parms);


            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int publishmentSystemID, List<int> collectItemIDList)
        {
            if (collectItemIDList != null && collectItemIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", CollectItemDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(collectItemIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll(int publishmentSystemID, int collectID)
        {
            if (collectID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2} AND {3} = {4}", CollectItemDAO.TABLE_NAME, CollectItemAttribute.PublishmentSystemID, publishmentSystemID, CollectItemAttribute.CollectID, collectID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public CollectItemInfo GetCollectItemInfo(int itemID)
        {
            CollectItemInfo collectItemInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", itemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CollectItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    collectItemInfo = new CollectItemInfo(rdr);
                }
                rdr.Close();
            }

            return collectItemInfo;
        }

        public List<CollectItemInfo> GetCollectItemInfoList(int collectID)
        {
            List<CollectItemInfo> list = new List<CollectItemInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND IsChecked = 'True' order by id desc", CollectItemAttribute.CollectID, collectID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, CollectItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    CollectItemInfo itemInfo = new CollectItemInfo(rdr);
                    list.Add(itemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public void AddVoteNum(int collectID, int itemID)
        {
            if (collectID > 0 && itemID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2} AND CollectID = {3}", CollectItemDAO.TABLE_NAME, CollectItemAttribute.VoteNum, itemID, collectID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public Dictionary<string, int> GetItemIDCollectionWithRank(int collectID)
        {
            Dictionary<int, string> rankWithItemIDCollection = new Dictionary<int, string>();

            string sqlString = string.Format("SELECT ID, VoteNum FROM {0} WHERE {1} = {2} ORDER BY VoteNum DESC", TABLE_NAME, CollectLogAttribute.CollectID, collectID);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                int rank = 1;
                while (rdr.Read())
                {
                    int itemID = rdr.GetInt32(0);
                    int voteNum = rdr.GetInt32(1);

                    if (rankWithItemIDCollection.ContainsKey(rank))
                    {
                        string itemIDCollection = rankWithItemIDCollection[rank];
                        itemIDCollection += "," + itemID;
                        rankWithItemIDCollection[rank] = itemIDCollection;
                    }
                    else
                    {
                        rankWithItemIDCollection.Add(rank, itemID.ToString());
                    }

                    rank++;
                }
                rdr.Close();
            }

            Dictionary<string, int> itemIDCollectionWithRank = new Dictionary<string, int>();
            foreach (var item in rankWithItemIDCollection)
            {
                itemIDCollectionWithRank.Add(item.Value, item.Key);
            }

            return itemIDCollectionWithRank;
        }

        public string GetSelectString(int publishmentSystemID, int collectID)
        {
            string whereString = string.Format("WHERE {0} = {1} AND {2} = {3}", CollectItemAttribute.PublishmentSystemID, publishmentSystemID, CollectItemAttribute.CollectID, collectID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(CollectItemDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public void Audit(int publishmentSystemID, int collectItemID)
        {
            if (collectItemID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET IsChecked='True' WHERE PublishmentSystemID = {1} AND ID = {2}", CollectItemDAO.TABLE_NAME, publishmentSystemID, collectItemID);
                this.ExecuteNonQuery(sqlString);
            }
        }

    }
}
