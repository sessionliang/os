using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IInnerLinkDAO
	{
        void Insert(InnerLinkInfo innerLinkInfo);

        void Update(InnerLinkInfo innerLinkInfo);

        void Delete(string innerLinkName, int publishmentSystemID);

        InnerLinkInfo GetInnerLinkInfo(string innerLinkName, int publishmentSystemID);

        bool IsExists(string innerLinkName, int publishmentSystemID);

        bool IsExactExists(string innerLinkName, int publishmentSystemID);

		IEnumerable GetDataSource(int publishmentSystemID);

		ArrayList GetInnerLinkInfoArrayList(int publishmentSystemID);

		ArrayList GetInnerLinkNameArrayList(int publishmentSystemID);
	}
}
