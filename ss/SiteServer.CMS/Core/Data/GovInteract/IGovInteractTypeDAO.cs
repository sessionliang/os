using System.Collections;
using SiteServer.CMS.Model;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
	public interface IGovInteractTypeDAO
	{
        void Insert(GovInteractTypeInfo typeInfo);

        void Update(GovInteractTypeInfo typeInfo);

        void Delete(int typeID);

        GovInteractTypeInfo GetTypeInfo(int typeID);

        string GetTypeName(int typeID);

        ArrayList GetTypeInfoArrayList(int nodeID);

        IEnumerable GetDataSource(int nodeID);

        ArrayList GetTypeNameArrayList(int nodeID);

        bool UpdateTaxisToUp(int typeID, int nodeID);

        bool UpdateTaxisToDown(int typeID, int nodeID);
	}
}
