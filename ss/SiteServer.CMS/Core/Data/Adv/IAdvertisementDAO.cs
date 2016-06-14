using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IAdvertisementDAO
	{
        void Insert(AdvertisementInfo adInfo);

        void Update(AdvertisementInfo adInfo);

        void Delete(string advertisementName, int publishmentSystemID);

        AdvertisementInfo GetAdvertisementInfo(string advertisementName, int publishmentSystemID);

        EAdvertisementType GetAdvertisementType(string advertisementName, int publishmentSystemID);

        bool IsExists(string advertisementName, int publishmentSystemID);

        IEnumerable GetDataSource(int publishmentSystemID);

        IEnumerable GetDataSourceByType(EAdvertisementType advertisementType, int publishmentSystemID);

        ArrayList GetAdvertisementNameArrayList(int publishmentSystemID);

        ArrayList[] GetAdvertisementArrayLists(int publishmentSystemID);

	}
}
