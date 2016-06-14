using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IAdMaterialDAO
	{
        void Insert(AdMaterialInfo adMaterialInfo);

        void Update(AdMaterialInfo adMaterialInfo);

        void Delete(int adMaterialD, int publishmentSystemID);

        void Delete(ArrayList adMaterialIDArrarList, int publishmentSystemID);

        AdMaterialInfo GetAdMaterialInfo(int adMaterialD, int publishmentSystemID);

        bool IsExists(string adMaterialName, int publishmentSystemID);

        IEnumerable GetDataSource(int advertID, int publishmentSystemID);

        IEnumerable GetDataSourceByType(EAdvType adMaterialType, int publishmentSystemID);

        ArrayList GetAdMaterialNameArrayList(int publishmentSystemID);

        ArrayList GetAdMaterialIDArrayList(int advertID,int publishmentSystemID);

        ArrayList GetAdMaterialInfoArrayList(int advertID, int publishmentSystemID);
	}
}
