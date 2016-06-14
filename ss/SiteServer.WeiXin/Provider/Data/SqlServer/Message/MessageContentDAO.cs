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
    public class MessageContentDAO : DataProviderBase, IMessageContentDAO
    {
        private const string TABLE_NAME = "wx_MessageContent";
        
        public int Insert(MessageContentInfo contentInfo)
        {
            int messageContentID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(contentInfo.ToNameValueCollection(), this.ConnectionString, MessageContentDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        messageContentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, MessageContentDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return messageContentID;
        }

        public void DeleteAll(int messageID)
        {
            if (messageID > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE {1} = {2}", MessageContentDAO.TABLE_NAME, MessageContentAttribute.MessageID, messageID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        private void UpdateUserCount(int publishmentSystemID)
        {
            Dictionary<int, int> messageIDWithCount = new Dictionary<int, int>();

            string sqlString = string.Format("SELECT {0}, COUNT(*) FROM {1} WHERE {2} = {3} GROUP BY {0}", MessageContentAttribute.MessageID, TABLE_NAME, MessageContentAttribute.PublishmentSystemID, publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    messageIDWithCount.Add(rdr.GetInt32(0), rdr.GetInt32(1));
                }
                rdr.Close();
            }

            DataProviderWX.MessageDAO.UpdateUserCount(publishmentSystemID, messageIDWithCount);

        }

        public void Delete(int publishmentSystemID, List<int> contentIDList)
        {
            if (contentIDList != null && contentIDList.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", MessageContentDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(contentIDList));
                this.ExecuteNonQuery(sqlString);

                this.UpdateUserCount(publishmentSystemID);
            }
        }

        public bool AddLikeCount(int contentID, string cookieSN, string wxOpenID)
        {
            bool isAdd = false;
            if (contentID > 0)
            {
                string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID = {2}", MessageContentAttribute.LikeCookieSNCollection, MessageContentDAO.TABLE_NAME, contentID);

                List<string> cookieSNList = TranslateUtils.StringCollectionToStringList(BaiRongDataProvider.DatabaseDAO.GetString(sqlString));
                if (!cookieSNList.Contains(cookieSN))
                {
                    cookieSNList.Add(cookieSN);

                    sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1, {2} = '{3}' WHERE ID = {4}", MessageContentDAO.TABLE_NAME, MessageContentAttribute.LikeCount, MessageContentAttribute.LikeCookieSNCollection, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cookieSNList), contentID);
                    this.ExecuteNonQuery(sqlString);

                    isAdd = true;
                }
            }
            return isAdd;
        }

        public void AddReplyCount(int contentID)
        {
            if (contentID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", MessageContentDAO.TABLE_NAME, MessageContentAttribute.ReplyCount, contentID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public int GetCount(int messageID, bool isReply)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM {0} WHERE {1} = {2} AND {3} = '{4}'", MessageContentDAO.TABLE_NAME, MessageContentAttribute.MessageID, messageID, MessageContentAttribute.IsReply, isReply);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectString(int publishmentSystemID, int messageID)
        {
            string whereString = string.Format("WHERE {0} = {1}", MessageContentAttribute.PublishmentSystemID, publishmentSystemID);
            if (messageID > 0)
            {
                whereString += string.Format(" AND {0} = {1}", MessageContentAttribute.MessageID, messageID);
            }
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(MessageContentDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<MessageContentInfo> GetContentInfoList(int messageID, int startNum, int totalNum)
        {
            List<MessageContentInfo> list = new List<MessageContentInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = '{1}' AND {2} = {3}", MessageContentAttribute.IsReply, false, MessageContentAttribute.MessageID, messageID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MessageContentDAO.TABLE_NAME, startNum, totalNum, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    MessageContentInfo itemInfo = new MessageContentInfo(rdr);
                    list.Add(itemInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<MessageContentInfo> GetReplyContentInfoList(int messageID, int replyID)
        {
            List<MessageContentInfo> list = new List<MessageContentInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = '{1}' AND {2} = {3} AND {4} = {5}", MessageContentAttribute.IsReply, true, MessageContentAttribute.MessageID, messageID, MessageContentAttribute.ReplyID, replyID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MessageContentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, "ORDER BY ID DESC");

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    MessageContentInfo itemInfo = new MessageContentInfo(rdr);
                    list.Add(itemInfo);
                }
                rdr.Close();
            }

            return list;
        }
        public List<MessageContentInfo> GetMessageContentInfoList(int publishmentSystemID, int messageID)
        {
            List<MessageContentInfo> messageContentInfoList = new List<MessageContentInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} = {3} AND ReplyID = 0", MessageContentAttribute.PublishmentSystemID, publishmentSystemID, MessageContentAttribute.MessageID, messageID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MessageContentDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    MessageContentInfo messageContentInfo = new MessageContentInfo(rdr);
                    messageContentInfoList.Add(messageContentInfo);
                }
                rdr.Close();
            }

            return messageContentInfoList;
        }
    }
}
