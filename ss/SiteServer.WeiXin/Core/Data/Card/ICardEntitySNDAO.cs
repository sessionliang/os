using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface ICardEntitySNDAO
    {
        int Insert(CardEntitySNInfo cardEntitySNInfo);

        void Update(CardEntitySNInfo cardEntitySNInfo);

        void UpdateStatus(bool isBinding, List<int> cardSNIDList);

        void Delete(int publishmentSystemID, int cardEntitySNID);

        void Delete(int publishmentSystemID, List<int> cardEntitySNIDList);

        CardEntitySNInfo GetCardEntitySNInfo(int cardEntitySNID);

        CardEntitySNInfo GetCardEntitySNInfo(int cardID, string cardSN, string mobile);

        bool IsExist(int publishmentSystemID, int cardID, string cardSN);

        bool IsExistMobile(int publishmentSystemID, int cardID, string mobile);

        string GetSelectString(int publishmentSystemID, int cardID, string cardSN, string userName, string mobile);

        List<CardEntitySNInfo> GetCardEntitySNInfoList(int publishmentSystemID, int cardID);


    }
}
