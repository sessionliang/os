using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface ILotteryDAO
    {
        int Insert(LotteryInfo lotteryInfo);

        void Update(LotteryInfo lotteryInfo);

        void UpdateUserCount(int publishmentSystemID, Dictionary<int, int> lotteryIDWithCount);

        void Delete(int publishmentSystemID, List<int> idList);

        LotteryInfo GetLotteryInfo(int lotteryID);

        List<int> GetLotteryIDList(int publishmentSystemID, ELotteryType lotteryType);

        List<LotteryInfo> GetLotteryInfoListByKeywordID(int publishmentSystemID, ELotteryType lotteryType, int keywordID);

        int GetFirstIDByKeywordID(int publishmentSystemID, ELotteryType lotteryType, int keywordID);

        int GetKeywordID(int lotteryID);

        string GetSelectString(int publishmentSystemID, ELotteryType lotteryType);

        void AddUserCount(int lotteryID);

        void AddPVCount(int lotteryID);

        List<LotteryInfo> GetLotteryInfoList(int publishmentSystemID);
    }
}
