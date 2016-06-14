using SiteServer.Project.Model;
using System.Collections;

namespace SiteServer.Project.Core
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
