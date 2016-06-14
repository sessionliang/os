using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface IMessageDAO
	{
        int Insert(MessageInfo messageInfo);

        void Update(MessageInfo messageInfo);

        void UpdateUserCount(int publishmentSystemID, Dictionary<int, int> messageIDWithCount);

        void Delete(int messageID);

        void Delete(List<int> messageIDList);

        void AddUserCount(int messageID);

        void AddPVCount(int messageID);

        MessageInfo GetMessageInfo(int messageID);

        List<MessageInfo> GetMessageInfoListByKeywordID(int publishmentSystemID, int keywordID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        string GetTitle(int messageID);

        string GetSelectString(int publishmentSystemID);

        List<MessageInfo> GetMessageInfoList(int publishmentSystemID);
        
	}
}
