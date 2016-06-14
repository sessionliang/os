using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface INodeGroupDAO
	{
		void Insert(NodeGroupInfo group);

		void Update(NodeGroupInfo group);

		void Delete(int publishmentSystemID, string groupName);

		NodeGroupInfo GetNodeGroupInfo(int publishmentSystemID, string groupName);

		bool IsExists(int publishmentSystemID, string groupName);

		IEnumerable GetDataSource(int publishmentSystemID);

		ArrayList GetNodeGroupInfoArrayList(int publishmentSystemID);

		ArrayList GetNodeGroupNameArrayList(int publishmentSystemID);

		ArrayList GetNodeInfoArrayListChecked(int publishmentSystemID, string groupName);

        bool UpdateTaxisToUp(int publishmentSystemID, string groupName);

        bool UpdateTaxisToDown(int publishmentSystemID, string groupName);
	}
}
