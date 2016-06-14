using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface ILotteryAwardDAO
    {
        int Insert(LotteryAwardInfo awardInfo);

        void Update(LotteryAwardInfo awardInfo);

        void UpdateTotalNum(int awardID, int totalNum);

        void UpdateWonNum(int awardID);

        void Delete(int awardID);

        void Delete(List<int> awardIDList);

        void DeleteAll(int lotteryID);

        void DeleteAllNotInIDList(int publishmentSystemID, int lotteryID, List<int> idList);

        void UpdateLotteryID(int publishmentSystemID, int lotteryID);

        LotteryAwardInfo GetAwardInfo(int awardID);

        List<LotteryAwardInfo> GetLotteryAwardInfoList(int publishmentSystemID, int lotteryID);

        string GetSelectString(int publishmentSystemID);

        Dictionary<string, int> GetLotteryAwardDictionary(int lotteryID);

        void GetCount(int publishmentSystemID, ELotteryType lotteryType, int lotteryID, out int totalNum, out int wonNum);

        List<LotteryAwardInfo> GetLotteryAwardInfoList(int publishmentSystemID);
    }
}
