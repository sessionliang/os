using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IVoteLogDAO
    {
        void Insert(VoteLogInfo logInfo);

        void DeleteAll(int voteID);

        void Delete(List<int> logIDList);

        int GetCount(int voteID);

        bool IsVoted(int voteID, string cookieSN, string wxOpenID);

        string GetSelectString(int voteID);

        List<VoteLogInfo> GetVoteLogInfoListByVoteID(int publishmentSystemID, int voteID);

        List<VoteLogInfo> GetVoteLogInfoList(int publishmentSystemID);

        int GetCount(int voteID, string iPAddress);
    }
}
