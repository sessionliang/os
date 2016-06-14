using System.Collections;
using SiteServer.CMS.Model;
using System.Collections.Generic;

namespace SiteServer.CMS.Core
{
    public interface IPhotoDAO
    {
        void Insert(PhotoInfo photoInfo);

        void Update(PhotoInfo photoInfo);

        void Delete(int id);

        void Delete(List<int> idList);

        void Delete(int publishmentSystemID, int contentID);

        PhotoInfo GetPhotoInfo(int id);

        PhotoInfo GetFirstPhotoInfo(int publishmentSystemID, int contentID);

        int GetCount(int publishmentSystemID, int contentID);

        string GetSortFieldName();

        string GetSelectSqlString(int publishmentSystemID, int contentID);

        IEnumerable GetStlDataSource(int publishmentSystemID, int contentID, int startNum, int totalNum);

        List<int> GetPhotoContentIDList(int publishmentSystemID, int contentID);

        List<PhotoInfo> GetPhotoInfoList(int publishmentSystemID, int contentID);

        bool UpdateTaxisToUp(int publishmentSystemID, int contentID, int id);

        bool UpdateTaxisToDown(int publishmentSystemID, int contentID, int id);
    }
}
