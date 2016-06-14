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
    public class VoteItemDAO : DataProviderBase, IVoteItemDAO
    {
        private const string TABLE_NAME = "wx_VoteItem";

        
        public int Insert(VoteItemInfo itemInfo)
        {
            int voteItemID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(itemInfo.ToNameValueCollection(), this.ConnectionString, VoteItemDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        voteItemID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, VoteItemDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return voteItemID;
        }

        public void Update(VoteItemInfo itemInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(itemInfo.ToNameValueCollection(), this.ConnectionString, VoteItemDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void UpdateVoteID(int publishmentSystemID, int voteID)
        {
            if (voteID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE {1} = 0 AND {3} = {4}", VoteItemDAO.TABLE_NAME, VoteItemAttribute.VoteID, voteID, VoteItemAttribute.PublishmentSystemID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void DeleteAll(int publishmentSystemID, int voteID)
        {
            if (voteID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2} AND {3} = {4}", VoteItemDAO.TABLE_NAME, VoteItemAttribute.PublishmentSystemID, publishmentSystemID, VoteItemAttribute.VoteID, voteID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public VoteItemInfo GetVoteItemInfo(int itemID)
        {
            VoteItemInfo voteItemInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", itemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, VoteItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    voteItemInfo = new VoteItemInfo(rdr);
                }
                rdr.Close();
            }

            return voteItemInfo;
        }


        public List<VoteItemInfo> GetVoteItemInfoList(int voteID)
        {
            List<VoteItemInfo> list = new List<VoteItemInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", VoteItemAttribute.VoteID, voteID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, VoteItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    VoteItemInfo itemInfo = new VoteItemInfo(rdr);
                    list.Add(itemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public void AddVoteNum(int voteID, List<int> itemIDList)
        {
            if (voteID > 0 && itemIDList != null && itemIDList.Count > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID IN ({2}) AND VoteID = {3}", VoteItemDAO.TABLE_NAME, VoteItemAttribute.VoteNum, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(itemIDList), voteID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void UpdateVoteNumByID(int VNum, int voteItemID)
        {
            if (voteItemID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE ID = {3} ", VoteItemDAO.TABLE_NAME, VoteItemAttribute.VoteNum, VNum, voteItemID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void UpdateAllVoteNumByVoteID(int VNum, int voteID)
        {
            if (voteID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE VoteID = {3} ", VoteItemDAO.TABLE_NAME, VoteItemAttribute.VoteNum, VNum, voteID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void UpdateOtherVoteNumByIDList(List<int> logIDList, int VNum, int VoteID)
        {
            if (logIDList != null && logIDList.Count > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE VoteID = {3} AND ID NOT IN ({4}) ", VoteItemDAO.TABLE_NAME, VoteItemAttribute.VoteNum, VNum, VoteID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(logIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public List<VoteItemInfo> GetVoteItemInfoList(int publishmentSystemID, int voteID)
        {
            List<VoteItemInfo> list = new List<VoteItemInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3}", VoteItemAttribute.PublishmentSystemID, publishmentSystemID, VoteItemAttribute.VoteID, voteID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, VoteItemDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    VoteItemInfo itemInfo = new VoteItemInfo(rdr);
                    list.Add(itemInfo);
                }
                rdr.Close();
            }

            return list;
        }         
    }
}
