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
    public class VoteDAO : DataProviderBase, IVoteDAO
    {
        private const string TABLE_NAME = "wx_Vote";

        public int Insert(VoteInfo voteInfo)
        {
            int voteID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(voteInfo.ToNameValueCollection(), this.ConnectionString, VoteDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        voteID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, VoteDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return voteID;
        }

        public void Update(VoteInfo voteInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(voteInfo.ToNameValueCollection(), this.ConnectionString, VoteDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void AddUserCount(int voteID)
        {
            if (voteID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", VoteDAO.TABLE_NAME, VoteAttribute.UserCount, voteID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void AddPVCount(int voteID)
        {
            if (voteID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", VoteDAO.TABLE_NAME, VoteAttribute.PVCount, voteID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, int voteID)
        {
            if (voteID > 0)
            {
                List<int> voteIDList = new List<int>();
                voteIDList.Add(voteID);
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(voteIDList));

                DataProviderWX.VoteLogDAO.DeleteAll(voteID);
                DataProviderWX.VoteItemDAO.DeleteAll(publishmentSystemID, voteID);

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", VoteDAO.TABLE_NAME, voteID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int publishmentSystemID, List<int> voteIDList)
        {
            if (voteIDList != null  && voteIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(voteIDList));

                foreach (int voteID in voteIDList)
                {
                    DataProviderWX.VoteLogDAO.DeleteAll(voteID);
                    DataProviderWX.VoteItemDAO.DeleteAll(publishmentSystemID, voteID);
                }

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", VoteDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(voteIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> voteIDList)
        {
            List<int> keywordIDList = new List<int>();

            string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", VoteAttribute.KeywordID, VoteDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(voteIDList));

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

        public VoteInfo GetVoteInfo(int voteID)
        {
            VoteInfo voteInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", voteID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, VoteDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    voteInfo = new VoteInfo(rdr);
                }
                rdr.Close();
            }

            return voteInfo;
        }

        public List<VoteInfo> GetVoteInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<VoteInfo> voteInfoList = new List<VoteInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", VoteAttribute.PublishmentSystemID, publishmentSystemID, VoteAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", VoteAttribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, VoteDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    VoteInfo voteInfo = new VoteInfo(rdr);
                    voteInfoList.Add(voteInfo);
                }
                rdr.Close();
            }

            return voteInfoList;
        }

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, VoteAttribute.PublishmentSystemID, publishmentSystemID, VoteAttribute.IsDisabled, true, VoteAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetTitle(int voteID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", voteID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, VoteDAO.TABLE_NAME, 0, VoteAttribute.Title, SQL_WHERE, null);

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
            string whereString = string.Format("WHERE {0} = {1}", VoteAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(VoteDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public void UpdateUserCountByID(int CNum, int voteID)
        {
            if (voteID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE ID = {3} ", VoteDAO.TABLE_NAME, VoteAttribute.UserCount, CNum, voteID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public List<VoteInfo> GetVoteInfoList(int publishmentSystemID)
        {
            List<VoteInfo> voteInfoList = new List<VoteInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", LotteryAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, VoteDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    VoteInfo voteInfo = new VoteInfo(rdr);
                    voteInfoList.Add(voteInfo);
                }
                rdr.Close();
            }

            return voteInfoList;
        }

    }
}
