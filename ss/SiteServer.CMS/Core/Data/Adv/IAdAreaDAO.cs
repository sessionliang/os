using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IAdAreaDAO
	{
        void Insert(AdAreaInfo adAreaInfo);

        void Update(AdAreaInfo adAreaInfo);

        void Delete(string adAreaName, int publishmentSystemID);

        AdAreaInfo GetAdAreaInfo(string adAreaName, int publishmentSystemID);

        AdAreaInfo GetAdAreaInfo(int adAreaID, int publishmentSystemID);

        bool IsExists(string adAreaName, int publishmentSystemID);

        IEnumerable GetDataSource(int publishmentSystemID);

        IEnumerable GetDataSourceByName(string adAreaName, int publishmentSystemID);
         
        ArrayList GetAdAreaNameArrayList(int publishmentSystemID);

        ArrayList GetAdAreaInfoArrayList(int publishmentSystemID);
	}
}
