using System.Collections;
using BaiRong.Model;
using System.Collections.Generic;
using System;

namespace BaiRong.Core.Data.Provider
{
    public interface IUserMessageDAO
    {
        void Insert(UserMessageInfo info);

        void Update(UserMessageInfo info);

        void Delete(string userName, ArrayList idArrayList);

        void Delete(int messageID);

        void SetIsViewed(string userName, int messageID);

        UserMessageInfo GetMessageInfo(int messageID);

        int GetUnReadedMessageCount(string toUserName);

        int GetCount(string where);

        int GetMessageCount(string toUserName);

        string GetSqlString(string toUserName, EUserMessageType messageType);

        string GetSqlString(string toUserName, EUserMessageType messageType, int daysToCurrent, string keyWords);

        string GetSortFieldName();

        int GetViewdCount(string userName, string messageType);

        List<UserMessageInfo> GetReciveMessageInfoList(string messageTo, EUserMessageType messageType);

        List<UserMessageInfo> GetReciveMessageInfoList(string messageTo, EUserMessageType messageType, int pageIndex, int prePageNum);

        List<UserMessageInfo> GetSendMessageInfoList(string messageFrom);

        DateTime GetLastMessagePublishDate(EUserMessageType messageType);
    }
}
