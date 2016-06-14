using System.Collections;
using SiteServer.B2C.Model;
using System.Collections.Generic;

namespace SiteServer.B2C.Core
{
	public interface IFilterDAO
	{
		int Insert(FilterInfo filterInfo);

        void UpdateIsDefaultValues(bool isDefaultValues, int filterID);

        void Update(FilterInfo filterInfo);

        void Delete(int publishmentSystemID, int nodeID, int filterID);

        void DeleteAll(int publishmentSystemID, int nodeID);

        FilterInfo GetFilterInfo(int filterID);

        bool IsExists(int nodeID, string attributeName);

        int GetCount(int publishmentSystemID, int nodeID);

        IEnumerable GetDataSource(int publishmentSystemID, int nodeID);

        List<FilterInfo> GetFilterInfoList(int publishmentSystemID, int nodeID);

        List<string> GetAttributeNameList(int publishmentSystemID, int nodeID);

        FilterInfo GetFilterInfo(int nodeID, string attributeName);

        bool UpdateTaxisToUp(int nodeID, int filterID);

        bool UpdateTaxisToDown(int nodeID, int filterID);

        IEnumerable GetStlDataSource(int publishmentSystemID, int nodeID, int startNum, int totalNum);
    }
}
