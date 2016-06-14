using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using System;

namespace SiteServer.WeiXin.Core
{
    public interface ICardSignLogDAO
	{
        int Insert(CardSignLogInfo cardSignLogInfo);

        void Update(CardSignLogInfo cardSignLogInfo);

        void Delete(int publishmentSystemID, int cardSignLogID);

        void Delete(int publishmentSystemID, List<int> cardSignLogIDList);

        bool IsSign(int publishmentSystemID, string userName);

        CardSignLogInfo GetCardSignLogInfo(int cardSignLogID);

        List<CardSignLogInfo> GetCardSignLogInfoList(int publishmentSystemID, string userName);

        List<DateTime> GetSignDateList(int publishmentSystemID, string userName);

        string GetSelectString(int publishmentSystemID);

        string GetSignAction();
        List<CardSignLogInfo> GetCardSignLogInfoList(int publishmentSystemID);
        
	}
}
