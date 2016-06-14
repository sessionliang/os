using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IConferenceContentDAO
    {
        void Insert(ConferenceContentInfo contentInfo);

        void DeleteAll(int conferenceID);

        void Delete(int publishmentSystemID, List<int> contentIDList);

        int GetCount(int conferenceID);

        bool IsApplied(int conferenceID, string cookieSN, string wxOpenID);

        string GetSelectString(int publishmentSystemID, int conferenceID);

        List<ConferenceContentInfo> GetConferenceContentInfoList(int publishmentSystemID, int conferenceID);
    }
}
