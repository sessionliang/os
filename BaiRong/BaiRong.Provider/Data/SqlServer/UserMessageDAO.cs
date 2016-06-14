using System;
using System.Data;
using System.Collections;

using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using System.Text;
using System.Collections.Generic;

namespace BaiRong.Provider.Data.SqlServer
{
    public class UserMessageDAO : DataProviderBase, IUserMessageDAO
    {
        private const string SQL_SELECT_MESSAGE = "SELECT ID, MessageFrom, MessageTo, MessageType, ParentID, IsViewed, AddDate, Content, LastAddDate, LastContent,Title FROM bairong_UserMessage WHERE ID = @ID";

        private const string SQL_SELECT_RECIVE_MESSAGE_BY_USERNAME_TYPE = "SELECT ID, MessageFrom, MessageTo, MessageType, ParentID, IsViewed, AddDate, Content, LastAddDate, LastContent,Title FROM bairong_UserMessage WHERE MessageTo=@MessageTo AND MessageType=@MessageType";

        private const string SQL_SELECT_SEND_MESSAGE_BY_USERNAME_TYPE = "SELECT ID, MessageFrom, MessageTo, MessageType, ParentID, IsViewed, AddDate, Content, LastAddDate, LastContent,Title FROM bairong_UserMessage WHERE MessageFrom=@MessageFrom";

        private const string SQL_UPDATE_MESSAGE_IS_VIEWED = "UPDATE bairong_UserMessage SET IsViewed = @IsViewed WHERE ID = @ID";

        private const string SQL_UPDATE_USERMESSAGE = "UPDATE bairong_UserMessage SET  MessageFrom=@MessageFrom, MessageTo=@MessageTo, MessageType=@MessageType, ParentID=@ParentID, IsViewed=@IsViewed, AddDate=@AddDate, Content=@Content, LastAddDate=@LastAddDate, LastContent=@LastContent, Title=@Title WHERE ID = @ID";

        private const string PARM_ID = "@ID";
        private const string PARM_MESSAGE_FROM = "@MessageFrom";
        private const string PARM_MESSAGE_TO = "@MessageTo";
        private const string PARM_MESSAGE_TYPE = "@MessageType";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_IS_VIEWED = "@IsViewed";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_CONTENT = "@Content";
        private const string PARM_LAST_ADD_DATE = "@LastAddDate";
        private const string PARM_LAST_CONTENT = "@LastContent";
        private const string PARM_TITLE = "@Title";

        public void Insert(UserMessageInfo info)
        {
            string sqlString = "INSERT INTO bairong_UserMessage (MessageFrom, MessageTo, MessageType, ParentID, IsViewed, AddDate, Content, LastAddDate, LastContent, Title) VALUES (@MessageFrom, @MessageTo, @MessageType, @ParentID, @IsViewed, @AddDate, @Content, @LastAddDate, @LastContent, @Title)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_UserMessage (ID, MessageFrom, MessageTo, MessageType, ParentID, IsViewed, AddDate, Content, LastAddDate, LastContent, Title) VALUES (bairong_UserMessage_SEQ.NEXTVAL, @MessageFrom, @MessageTo, @MessageType, @ParentID, @IsViewed, @AddDate, @Content, @LastAddDate, @LastContent, @Title)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_MESSAGE_FROM, EDataType.NVarChar, 255, info.MessageFrom),
                this.GetParameter(PARM_MESSAGE_TO, EDataType.NVarChar, 255, info.MessageTo),
                this.GetParameter(PARM_MESSAGE_TYPE, EDataType.VarChar, 50, EUserMessageTypeUtils.GetValue(info.MessageType)),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, info.ParentID),
                this.GetParameter(PARM_IS_VIEWED, EDataType.VarChar, 18, info.IsViewed.ToString()),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, info.AddDate),
                this.GetParameter(PARM_CONTENT, EDataType.NText, info.Content),
                this.GetParameter(PARM_LAST_ADD_DATE, EDataType.DateTime, info.LastAddDate),
                this.GetParameter(PARM_LAST_CONTENT, EDataType.NText, info.LastContent),
                this.GetParameter(PARM_TITLE, EDataType.NVarChar,255    , info.Title)
            };

            this.ExecuteNonQuery(sqlString, parms);

            UserMessageManager.RemoveCache(info.MessageTo);
        }

        public void Update(UserMessageInfo info)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_MESSAGE_FROM, EDataType.NVarChar, 255, info.MessageFrom),
                this.GetParameter(PARM_MESSAGE_TO, EDataType.NVarChar, 255, info.MessageTo),
                this.GetParameter(PARM_MESSAGE_TYPE, EDataType.VarChar, 50, EUserMessageTypeUtils.GetValue(info.MessageType)),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, info.ParentID),
                this.GetParameter(PARM_IS_VIEWED, EDataType.VarChar, 18, info.IsViewed.ToString()),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, info.AddDate),
                this.GetParameter(PARM_CONTENT, EDataType.NText, info.Content),
                this.GetParameter(PARM_LAST_ADD_DATE, EDataType.DateTime, info.LastAddDate),
                this.GetParameter(PARM_LAST_CONTENT, EDataType.NText, info.LastContent),
                this.GetParameter(PARM_TITLE, EDataType.NVarChar,255    , info.Title),
                this.GetParameter(PARM_ID, EDataType.Integer, info.ID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_USERMESSAGE, parms);
        }

        public void Delete(string userName, ArrayList idArrayList)
        {
            if (idArrayList != null && idArrayList.Count > 0)
            {
                string deleteSqlString = string.Format("DELETE bairong_UserMessage WHERE MessageTo = '{0}' AND ID IN ({1})", PageUtils.FilterSql(userName), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));
                this.ExecuteNonQuery(deleteSqlString);

                UserMessageManager.RemoveCache(userName);
            }
        }

        public void Delete(int messageID)
        {

            string deleteSqlString = string.Format("DELETE bairong_UserMessage WHERE ID = {0}", messageID);
            this.ExecuteNonQuery(deleteSqlString);
        }

        public void SetIsViewed(string userName, int messageID)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_IS_VIEWED, EDataType.VarChar, 18, true.ToString()),
                this.GetParameter(PARM_ID, EDataType.Integer, messageID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_MESSAGE_IS_VIEWED, updateParms);

            UserMessageManager.RemoveCache(userName);
        }

        public UserMessageInfo GetMessageInfo(int messageID)
        {
            UserMessageInfo info = null;

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ID, EDataType.Integer, messageID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MESSAGE, selectParms))
            {
                if (rdr.Read())
                {
                    info = new UserMessageInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EUserMessageTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetDateTime(6), rdr.GetValue(7).ToString(), rdr.GetDateTime(8), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString());
                }
                rdr.Close();
            }

            return info;
        }

        public int GetUnReadedMessageCount(string messageTo)
        {
            string sqlString = "SELECT Count(ID) FROM bairong_UserMessage WHERE MessageTo = @MessageTo AND IsViewed = @IsViewed";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_MESSAGE_TO, EDataType.NVarChar, 255, messageTo),
                this.GetParameter(PARM_IS_VIEWED, EDataType.VarChar, 18, false.ToString())
            };

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString, parms);
        }

        public int GetMessageCount(string messageTo)
        {
            string sqlString = "SELECT Count(ID) FROM bairong_UserMessage WHERE MessageTo = @MessageTo";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_MESSAGE_TO, EDataType.NVarChar, 255, messageTo)
            };

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString, parms);
        }

        public int GetCount(string where)
        {
            string sqlString = "SELECT Count(ID) FROM bairong_UserMessage ";

            if (!string.IsNullOrEmpty(where))
            {
                sqlString += "WHERE " + where;
            }

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSqlString(string messageTo, EUserMessageType messageType)
        {
            if (messageType == EUserMessageType.New)
            {
                return string.Format("SELECT ID, MessageFrom, MessageTo, MessageType, ParentID, IsViewed, AddDate, Content, LastAddDate, LastContent, Title FROM bairong_UserMessage WHERE MessageTo = '{0}' AND IsViewed = '{1}'", PageUtils.FilterSql(messageTo), false.ToString());
            }
            else
            {
                return string.Format("SELECT ID, MessageFrom, MessageTo, MessageType, ParentID, IsViewed, AddDate, Content, LastAddDate, LastContent, Title FROM bairong_UserMessage WHERE MessageTo = '{0}' AND MessageType = '{1}'", PageUtils.FilterSql(messageTo), EUserMessageTypeUtils.GetValue(messageType));
            }
        }

        public string GetSqlString(string messageTo, EUserMessageType messageType, int daysToCurrent, string keyWords)
        {
            string dateString = string.Empty;
            if (daysToCurrent > 0)
            {
                DateTime dt = DateTime.Now.AddDays(-daysToCurrent);
                dateString += " AND ";
                dateString += string.Format("(AddDate >= '{0}')", dt.ToString("yyyy-MM-dd"));
            }

            if (messageType == EUserMessageType.New)
            {
                return string.Format("SELECT ID, MessageFrom, MessageTo, MessageType, ParentID, IsViewed, AddDate, Content, LastAddDate, LastContent, Title FROM bairong_UserMessage WHERE MessageTo = '{0}' AND IsViewed = '{1}' AND (Content like '%{2}%' OR Title like '%{2}%') {3}", PageUtils.FilterSql(messageTo), false.ToString(), keyWords, dateString);
            }
            else
            {
                return string.Format("SELECT ID, MessageFrom, MessageTo, MessageType, ParentID, IsViewed, AddDate, Content, LastAddDate, LastContent, Title FROM bairong_UserMessage WHERE MessageTo = '{0}' AND MessageType = '{1}' AND (Content like '%{2}%' OR Title like '%{2}%') {3}", PageUtils.FilterSql(messageTo), EUserMessageTypeUtils.GetValue(messageType), keyWords, dateString);
            }
        }

        public string GetSortFieldName()
        {
            return "AddDate";
        }

        public int GetViewdCount(string userName, string messageType)
        {
            int viewcount = 0;
            StringBuilder str = new StringBuilder();
            str.AppendFormat("SELECT COUNT(IsViewed) AS ViewedCount FROM bairong_UserMessage WHERE MessageTo='{0}' AND IsViewed='False'", PageUtils.FilterSql(userName));
            if (!string.IsNullOrEmpty(messageType))
            {
                str.AppendFormat(" AND MessageType='{0}'", PageUtils.FilterSql(messageType));
            }
            using (IDataReader rdr = this.ExecuteReader(str.ToString()))
            {
                if (rdr.Read())
                {
                    viewcount = rdr.GetInt32(0);
                }
                rdr.Close();
            }
            return viewcount;

        }

        public List<UserMessageInfo> GetReciveMessageInfoList(string messageTo, EUserMessageType messageType)
        {
            List<UserMessageInfo> list = new List<UserMessageInfo>();
            try
            {
                if (EUserMessageTypeUtils.Equals(messageType, EUserMessageType.SystemAnnouncement))
                {
                    messageTo = string.Empty;
                }

                IDataParameter[] paras = new IDataParameter[] {
                    this.GetParameter(PARM_MESSAGE_TO,EDataType.NVarChar,255,messageTo),
                    this.GetParameter(PARM_MESSAGE_TYPE,EDataType.VarChar,50,EUserMessageTypeUtils.GetValue(messageType))
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_RECIVE_MESSAGE_BY_USERNAME_TYPE, paras))
                {
                    while (rdr.Read())
                    {
                        UserMessageInfo info = new UserMessageInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EUserMessageTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetDateTime(6), rdr.GetValue(7).ToString(), rdr.GetDateTime(8), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString());
                        list.Add(info);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return list;
        }

        public List<UserMessageInfo> GetReciveMessageInfoList(string messageTo, EUserMessageType messageType, int pageIndex, int prePageNum)
        {
            List<UserMessageInfo> list = new List<UserMessageInfo>();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.AppendFormat(" SELECT tmp.* from ( ");
                sbSql.AppendFormat(" SELECT *, ROW_NUMBER() OVER(ORDER BY IsViewed, AddDate DESC) as rowNum FROM dbo.bairong_UserMessage WHERE MessageTo = @MessageTo and MessageType = @MessageType ");
                sbSql.AppendFormat(" ) as tmp ");
                sbSql.AppendFormat(" WHERE tmp.rowNum >= {0} and tmp.rowNum <= {1} ", (pageIndex - 1) * prePageNum + 1, pageIndex * prePageNum);

                if (EUserMessageTypeUtils.Equals(messageType, EUserMessageType.SystemAnnouncement))
                {
                    messageTo = string.Empty;
                }

                IDataParameter[] paras = new IDataParameter[] {
                    this.GetParameter(PARM_MESSAGE_TO,EDataType.NVarChar,255,messageTo),
                    this.GetParameter(PARM_MESSAGE_TYPE,EDataType.VarChar,50,EUserMessageTypeUtils.GetValue(messageType))
                };

                using (IDataReader rdr = this.ExecuteReader(sbSql.ToString(), paras))
                {
                    while (rdr.Read())
                    {
                        UserMessageInfo info = new UserMessageInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EUserMessageTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetDateTime(6), rdr.GetValue(7).ToString(), rdr.GetDateTime(8), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString());
                        list.Add(info);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return list;
        }

        public List<UserMessageInfo> GetSendMessageInfoList(string messageFrom)
        {
            List<UserMessageInfo> list = new List<UserMessageInfo>();
            try
            {
                IDataParameter[] paras = new IDataParameter[] {
                    this.GetParameter(PARM_MESSAGE_TO,EDataType.NVarChar,255,messageFrom)
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SEND_MESSAGE_BY_USERNAME_TYPE, paras))
                {
                    while (rdr.Read())
                    {
                        UserMessageInfo info = new UserMessageInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EUserMessageTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetDateTime(6), rdr.GetValue(7).ToString(), rdr.GetDateTime(8), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString());
                        list.Add(info);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return list;
        }

        public DateTime GetLastMessagePublishDate(EUserMessageType messageType)
        {
            string sqlString = "SELECT TOP 1 AddDate FROM bairong_UserMessage WHERE MessageType = '" + messageType.ToString() + "' ORDER BY AddDate DESC";
            return BaiRongDataProvider.DatabaseDAO.GetDateTime(sqlString);
        }
    }
}