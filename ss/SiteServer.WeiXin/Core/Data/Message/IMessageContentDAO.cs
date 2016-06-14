using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface IMessageContentDAO
	{
        int Insert(MessageContentInfo contentInfo);

        void DeleteAll(int messageID);

        void Delete(int publishmentSystemID, List<int> contentIDList);

        bool AddLikeCount(int contentID, string cookieSN, string wxOpenID);

        void AddReplyCount(int contentID);

        int GetCount(int messageID, bool isReply);

        string GetSelectString(int publishmentSystemID, int messageID);

        List<MessageContentInfo> GetContentInfoList(int messageID, int index, int count);

        List<MessageContentInfo> GetReplyContentInfoList(int messageID, int replyID);

        List<MessageContentInfo> GetMessageContentInfoList(int publishmentSystemID, int messageID);
        
	}
}
