using System.Collections;
using SiteServer.B2C.Model;
using System.Collections.Generic;

namespace SiteServer.B2C.Core
{
	public interface IFilterItemDAO
	{
		void Insert(FilterItemInfo itemInfo);

        void Delete(int filterID);

        IEnumerable GetDataSource(int filterID);

        List<FilterItemInfo> GetFilterItemInfoList(int filterID);

        IEnumerable GetStlDataSource(int filterID, int startNum, int totalNum);

    }
}
