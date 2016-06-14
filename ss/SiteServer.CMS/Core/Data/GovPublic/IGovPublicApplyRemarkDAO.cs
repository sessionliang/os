using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
	public interface IGovPublicApplyRemarkDAO
	{
        void Insert(GovPublicApplyRemarkInfo remarkInfo);

        void Delete(int remarkID);

        GovPublicApplyRemarkInfo GetRemarkInfo(int remarkID);

        ArrayList GetRemarkInfoArrayList(int applyID);

        IEnumerable GetDataSourceByApplyID(int applyID);
	}
}
