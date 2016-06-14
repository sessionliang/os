using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IVoteDAO
    {
        int Insert(VoteInfo voteInfo);

        void Update(VoteInfo voteInfo);

        void Delete(int publishmentSystemID, int voteID);

        void Delete(int publishmentSystemID, List<int> voteIDList);

        void AddUserCount(int voteID);

        void AddPVCount(int voteID);

        VoteInfo GetVoteInfo(int voteID);

        List<VoteInfo> GetVoteInfoListByKeywordID(int publishmentSystemID, int keywordID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        string GetTitle(int voteID);

        string GetSelectString(int publishmentSystemID);

        void UpdateUserCountByID(int CNum, int voteID);

        List<VoteInfo> GetVoteInfoList(int publishmentSystemID);

    }
}
