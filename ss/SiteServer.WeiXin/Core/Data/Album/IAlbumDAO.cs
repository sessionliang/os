using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IAlbumDAO
	{
        int Insert(AlbumInfo albumInfo);

        void Update(AlbumInfo albumInfo);

        void Delete(int publishmentSystemID, int albumID);

        void Delete(int publishmentSystemID, List<int> albumIDList);
  
        void AddPVCount(int albumID);

        AlbumInfo GetAlbumInfo(int albumID);

        List<AlbumInfo> GetAlbumInfoListByKeywordID(int publishmentSystemID, int keywordID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        string GetTitle(int albumID);

        string GetSelectString(int publishmentSystemID);

        List<AlbumInfo> GetAlbumInfoList(int publishmentSystemID);
	}
}
