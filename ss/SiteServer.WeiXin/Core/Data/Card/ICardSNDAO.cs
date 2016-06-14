using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface ICardSNDAO
	{
        int Insert(CardSNInfo cardSNInfo);

        void Update(CardSNInfo cardSNInfo);

        void UpdateStatus(int cardID, bool isDisabled, List<int> cardSNIDList);

        void Delete(int publishmentSystemID, int cardSNID);

        void Delete(int publishmentSystemID, List<int> cardSNIDList);

        void Recharge(int cardSNID, string userName, decimal amount);

        void Consume(int cardSNID, string userName, decimal amount);

        void Exchange(int cardSNID, string userName, decimal amount);

        CardSNInfo GetCardSNInfo(int cardSNID);

        CardSNInfo GetCardSNInfo(int publishmentSystemID, int cardID, string cardSN, string userName);

        List<CardSNInfo> GetCardSNInfoList(int publishmentSystemID, int cardID);

        ArrayList GetUserNameArrayList(int publishmentSystemID, int cardID, string cardSN, string userName);

        bool isExists(int publishmentSystemID, int cardID, string userName);

        decimal GetAmount(int cardSNID, string userName);

        string GetNextCardSN(int publishmentSystemID, int cardID);

        string GetSelectString(int publishmentSystemID, int cardID, string cardSN, string userName, string mobile);
	}
}
