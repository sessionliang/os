using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface IMapDAO
	{
        int Insert(MapInfo mapInfo);

        void Update(MapInfo mapInfo);

        void Delete(int mapID);

        void Delete(List<int> mapIDList);

        void AddPVCount(int mapID);

        MapInfo GetMapInfo(int mapID);

        List<MapInfo> GetMapInfoListByKeywordID(int publishmentSystemID, int keywordID);

        List<MapInfo> GetMapInfoList(int publishmentSystemID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        string GetTitle(int mapID);

        string GetSelectString(int publishmentSystemID);
	}
}
