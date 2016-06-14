using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IContentGroupDAO
	{
		void Insert(ContentGroupInfo group);

		void Update(ContentGroupInfo group);

		void Delete(string groupName, int publishmentSystemID);

		ContentGroupInfo GetContentGroupInfo(string groupName, int publishmentSystemID);

		bool IsExists(string groupName, int publishmentSystemID);

		IEnumerable GetDataSource(int publishmentSystemID);

		ArrayList GetContentGroupInfoArrayList(int publishmentSystemID);

		ArrayList GetContentGroupNameArrayList(int publishmentSystemID);

        bool UpdateTaxisToUp(int publishmentSystemID, string groupName);

        bool UpdateTaxisToDown(int publishmentSystemID, string groupName);
	}
}
