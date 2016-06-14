using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IVoteItemDAO
    {
        int Insert(VoteItemInfo itemInfo);

        void Update(VoteItemInfo itemInfo);

        void UpdateVoteID(int publishmentSystemID, int voteID);

        void DeleteAll(int publishmentSystemID, int voteID);

        VoteItemInfo GetVoteItemInfo(int itemID);

        List<VoteItemInfo> GetVoteItemInfoList(int voteID);

        void AddVoteNum(int voteID, List<int> itemIDList);

        void UpdateVoteNumByID(int VNum, int voteItemID);

        void UpdateAllVoteNumByVoteID(int VNum, int voteID);

        void UpdateOtherVoteNumByIDList(List<int> logIDList, int VNum, int VoteID);

        List<VoteItemInfo> GetVoteItemInfoList(int publishmentSystemID, int voteID);

    }
}
