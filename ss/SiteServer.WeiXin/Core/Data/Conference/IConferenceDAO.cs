using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface IConferenceDAO
	{
        int Insert(ConferenceInfo conferenceInfo);

        void Update(ConferenceInfo conferenceInfo);

        void Delete(int publishmentSystemID, int conferenceID);

        void Delete(int publishmentSystemID, List<int> conferenceIDList);

        void AddUserCount(int conferenceID);

        void AddPVCount(int conferenceID);

        ConferenceInfo GetConferenceInfo(int conferenceID);

        List<ConferenceInfo> GetConferenceInfoListByKeywordID(int publishmentSystemID, int keywordID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        string GetTitle(int conferenceID);

        string GetSelectString(int publishmentSystemID);

        void UpdateUserCount(int publishmentSystemID, Dictionary<int, int> conferenceIDWithCount);

        List<ConferenceInfo> GetConferenceInfoList(int publishmentSystemID);
	}
}
