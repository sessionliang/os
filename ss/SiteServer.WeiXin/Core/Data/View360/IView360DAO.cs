using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface IView360DAO
	{
        int Insert(View360Info view360Info);

        void Update(View360Info view360Info);

        void Delete(int view360ID);

        void Delete(List<int> view360IDList);

        void AddPVCount(int view360ID);

        View360Info GetView360Info(int view360ID);

        List<View360Info> GetView360InfoListByKeywordID(int publishmentSystemID, int keywordID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        string GetTitle(int view360ID);

        string GetSelectString(int publishmentSystemID);

        List<View360Info> GetView360InfoList(int publishmentSystemID);
	}
}
