using SiteServer.CRM.Model;
using System.Collections;

namespace SiteServer.CRM.Core
{
	public interface IRemarkDAO
	{
        void Insert(RemarkInfo remarkInfo);

        void Delete(int remarkID);

        RemarkInfo GetRemarkInfo(int remarkID);

        ArrayList GetRemarkInfoArrayList(int applyID);

        IEnumerable GetDataSourceByApplyID(int applyID);
	}
}
