using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IAlbumContentDAO
	{
        int Insert(AlbumContentInfo albumContentInfo);

        void Update(AlbumContentInfo albumContentInfo);

        void Delete(int publishmentSystemID, int albumContentID);

        void Delete(int publishmentSystemID, List<int> albumContentIDList);

        void DeleteByParentID(int publishmentSystemID, int parentID);

        AlbumContentInfo GetAlbumContentInfo(int albumContentID);

        List<AlbumContentInfo> GetAlbumContentInfoList(int publishmentSystemID, int albumID, int parentID);

        List<int> GetAlbumContentIDList(int publishmentSystemID, int albumID, int parentID);

        string GetTitle(int albumContentID);

        int GetCount(int publishmentSystemID, int parentID);
         
        IEnumerable GetDataSource(int publishmentSystemID, int albumID);

        IEnumerable GetDataSource(int publishmentSystemID, int albumID, int parentID);

        List<AlbumContentInfo> GetAlbumContentInfoList(int publishmentSystemID);
	}
}
