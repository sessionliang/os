using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IStlTagDAO
	{
        void Insert(StlTagInfo info);

        void Update(StlTagInfo info);

        void Delete(int publishmentSystemID, string tagName);

        StlTagInfo GetStlTagInfo(int publishmentSystemID, string tagName);

        ArrayList GetStlTagInfoArrayListByPublishmentSystemID(int publishmentSystemID);

        ArrayList GetStlTagNameArrayList(int publishmentSystemID);

        IEnumerable GetDataSource(int publishmentSystemID);

        bool IsExactExists(int publishmentSystemID, string tagName);

        bool IsExists(int publishmentSystemID, string tagName);
	}
}
