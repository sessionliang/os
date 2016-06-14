using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface ILotteryLogDAO
    {
        int Insert(LotteryLogInfo lotteryLogInfo);

        void AddCount(int publishmentSystemID, int lotteryID, string cookieSN, string wxOpenID, string userName, int maxCount, int maxDailyCount, out bool isMaxCount, out bool isMaxDailyCount);

        void DeleteAll(int lotteryID);

        void Delete(List<int> logIDList);

        int GetCount(int lotteryID);

        string GetSelectString(int lotteryID);

        List<LotteryLogInfo> GetLotteryLogInfoList(int publishmentSystemID, int lotteryID);
    }
}
