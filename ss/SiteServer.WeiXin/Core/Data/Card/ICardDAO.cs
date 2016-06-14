using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface ICardDAO
	{
        int Insert(CardInfo cardInfo);

        void Update(CardInfo cardInfo);

        void Delete(int publishmentSystemID, int cardID);

        void Delete(int publishmentSystemID, List<int> cardIDList);
  
        void AddPVCount(int cardID);

        CardInfo GetCardInfo(int cardID);

        List<CardInfo> GetCardInfoList(int publishmentSystemID);

        List<CardInfo> GetCardInfoListByKeywordID(int publishmentSystemID, int keywordID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        string GetTitle(int cardID);

        string GetSelectString(int publishmentSystemID);
	}
}
