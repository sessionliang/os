using System.Collections;
using SiteServer.CRM.Model;
using BaiRong.Model;

namespace SiteServer.CRM.Core
{
	public interface IDocTypeDAO
	{
        int Insert(DocTypeInfo typeInfo);

        void Update(DocTypeInfo typeInfo);

        void Delete(int typeID);

        DocTypeInfo GetTypeInfo(int typeID);

        string GetTypeName(int typeID);

        ArrayList GetTypeInfoArrayList(int parentID);

        int GetCount(int parentID);

        IEnumerable GetDataSource(int parentID);

        ArrayList GetTypeNameArrayList(int parentID);

        bool UpdateTaxisToUp(int parentID, int typeID);

        bool UpdateTaxisToDown(int parentID, int typeID);
	}
}
