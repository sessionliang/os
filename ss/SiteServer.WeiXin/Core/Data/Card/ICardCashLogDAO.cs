using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface ICardCashLogDAO
	{
        int Insert(CardCashLogInfo cardCashLogInfo);

        void Update(CardCashLogInfo cardCashLogInfo);

        void Delete(int publishmentSystemID, int cardCashLogID);

        void Delete(int publishmentSystemID, List<int> cardCashLogIDList);
   
        CardCashLogInfo GetCardCashLogInfo(int cardCashLogID);

        List<CardCashLogInfo> GetCardCashLogInfoList(int cardID, int cardSNID, string userName, string startDate, string endDate);

        List<CardCashYearCountInfo> GetCardCashYearCountInfoList(int cardID, int cardSNID, string userName);

        List<CardCashMonthCountInfo> GetCardCashMonthCountInfoList(int cardID, int cardSNID, string userName, string year);

        string GetSelectString(int publishmentSystemID, ECashType cashType, int cardID, string cardSN, string userName, string mobile);

        List<CardCashLogInfo> GetCardCashLogInfoList(int publishmentSystemID, int cardID, int cardSNID);
	}
}
