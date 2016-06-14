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
    public class MessageDAO : DataProviderBase, IMessageDAO
    {
        private const string TABLE_NAME = "wx_Message";
         
        public int Insert(MessageInfo messageInfo)
        {
            int messageID = 0;

            IDbDataParameter[] parms = null;

            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(messageInfo.ToNameValueCollection(), this.ConnectionString, MessageDAO.TABLE_NAME, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        messageID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, MessageDAO.TABLE_NAME);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return messageID;
        }

        public void Update(MessageInfo messageInfo)
        {
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(messageInfo.ToNameValueCollection(), this.ConnectionString, MessageDAO.TABLE_NAME, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        private List<int> GetMessageIDList(int publishmentSystemID)
        {
            List<int> messageIDList = new List<int>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", MessageAttribute.PublishmentSystemID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MessageDAO.TABLE_NAME, 0, MessageAttribute.ID, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    messageIDList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return messageIDList;
        }

        public void UpdateUserCount(int publishmentSystemID, Dictionary<int, int> messageIDWithCount)
        {
            if (messageIDWithCount.Count == 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = 0 WHERE {2} = {3}", TABLE_NAME, MessageAttribute.UserCount, MessageAttribute.PublishmentSystemID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
            else
            {
                List<int> messageIDList = this.GetMessageIDList(publishmentSystemID);
                foreach (int messageID in messageIDList)
                {
                    if (messageIDWithCount.ContainsKey(messageID))
                    {
                        string sqlString = string.Format("UPDATE {0} SET {1} = {2} WHERE ID = {3}", TABLE_NAME, MessageAttribute.UserCount, messageIDWithCount[messageID], messageID);
                        this.ExecuteNonQuery(sqlString);
                    }
                    else
                    {
                        string sqlString = string.Format("UPDATE {0} SET {1} = 0 WHERE ID = {2}", TABLE_NAME, MessageAttribute.UserCount, messageID);
                        this.ExecuteNonQuery(sqlString);
                    }
                }
            }
        }

        public void AddUserCount(int messageID)
        {
            if (messageID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", MessageDAO.TABLE_NAME, MessageAttribute.UserCount, messageID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void AddPVCount(int messageID)
        {
            if (messageID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET {1} = {1} + 1 WHERE ID = {2}", MessageDAO.TABLE_NAME, MessageAttribute.PVCount, messageID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(int messageID)
        {
            if (messageID > 0)
            {
                List<int> messageIDList = new List<int>();
                messageIDList.Add(messageID);

                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(messageIDList));

                DataProviderWX.MessageContentDAO.DeleteAll(messageID);

                string sqlString = string.Format("DELETE FROM {0} WHERE ID = {1}", MessageDAO.TABLE_NAME, messageID);
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void Delete(List<int> messageIDList)
        {
            if (messageIDList != null  && messageIDList.Count > 0)
            {
                DataProviderWX.KeywordDAO.Delete(this.GetKeywordIDList(messageIDList));

                foreach (int messageID in messageIDList)
                {
                    DataProviderWX.MessageContentDAO.DeleteAll(messageID);
                }

                string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", MessageDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(messageIDList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        private List<int> GetKeywordIDList(List<int> messageIDList)
        {
            List<int> keywordIDList = new List<int>();

            if (messageIDList != null && messageIDList.Count > 0)
            {
                string sqlString = string.Format("SELECT {0} FROM {1} WHERE ID IN ({2})", MessageAttribute.KeywordID, MessageDAO.TABLE_NAME, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(messageIDList));

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        keywordIDList.Add(rdr.GetInt32(0));
                    }
                    rdr.Close();
                }
            }

            return keywordIDList;
        }

        public MessageInfo GetMessageInfo(int messageID)
        {
            MessageInfo messageInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", messageID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MessageDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    messageInfo = new MessageInfo(rdr);
                }
                rdr.Close();
            }

            return messageInfo;
        }

        public List<MessageInfo> GetMessageInfoListByKeywordID(int publishmentSystemID, int keywordID)
        {
            List<MessageInfo> messageInfoList = new List<MessageInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1} AND {2} <> '{3}'", MessageAttribute.PublishmentSystemID, publishmentSystemID, MessageAttribute.IsDisabled, true);
            if (keywordID > 0)
            {
                SQL_WHERE += string.Format(" AND {0} = {1}", MessageAttribute.KeywordID, keywordID);
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MessageDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    MessageInfo messageInfo = new MessageInfo(rdr);
                    messageInfoList.Add(messageInfo);
                }
                rdr.Close();
            }

            return messageInfoList;
        }

        public string GetTitle(int messageID)
        {
            string title = string.Empty;

            string SQL_WHERE = string.Format("WHERE ID = {0}", messageID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MessageDAO.TABLE_NAME, 0, MessageAttribute.Title, SQL_WHERE, null);

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

        public int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID)
        {
            string sqlString = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = {2} AND {3} <> '{4}' AND {5} = {6}", TABLE_NAME, MessageAttribute.PublishmentSystemID, publishmentSystemID, MessageAttribute.IsDisabled, true, MessageAttribute.KeywordID, keywordID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectString(int publishmentSystemID)
        {
            string whereString = string.Format("WHERE {0} = {1}", MessageAttribute.PublishmentSystemID, publishmentSystemID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(MessageDAO.TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public List<MessageInfo> GetMessageInfoList(int publishmentSystemID)
        {
            List<MessageInfo> messageInfoList = new List<MessageInfo>();

            string SQL_WHERE = string.Format("WHERE {0} = {1}", MessageAttribute.PublishmentSystemID, publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, MessageDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    MessageInfo messageInfo = new MessageInfo(rdr);
                    messageInfoList.Add(messageInfo);
                }
                rdr.Close();
            }

            return messageInfoList;
        }
    }
}
