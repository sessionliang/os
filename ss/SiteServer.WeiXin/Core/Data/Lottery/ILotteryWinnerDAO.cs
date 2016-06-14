using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface ILotteryWinnerDAO
	{
        int Insert(LotteryWinnerInfo winnerInfo);

        void UpdateStatus(EWinStatus status, List<int> winnerIDList);

        void Update(LotteryWinnerInfo winnerInfo);

        void Delete(int publishmentSystemID, List<int> winnerIDList);

        void DeleteAll(int lotteryID);

        LotteryWinnerInfo GetWinnerInfo(int winnerID);

        LotteryWinnerInfo GetWinnerInfo(int publishmentSystemID, int lotteryID, string cookieSN, string wxOpenID, string userName);

        List<LotteryWinnerInfo> GetWinnerInfoList(int publishmentSystemID, ELotteryType lotteryType, int lotteryID);

        int GetTotalNum(int publishmentSystemID, ELotteryType lotteryType, int lotteryID);

        int GetTotalNum(int awardID);

        int GetCashNum(int publishmentSystemID, int lotteryID);

        string GetSelectString(int publishmentSystemID, ELotteryType lotteryType, int lotteryID, int awardID);

        List<LotteryWinnerInfo> GetLotteryWinnerInfoList(int publishmentSystemID, int lotteryID, int awardID);
	}
}
