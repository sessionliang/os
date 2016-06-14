using System.Collections;
using SiteServer.Project.Model;
using BaiRong.Model;

namespace SiteServer.Project.Core
{
	public interface ITypeDAO
	{
        void Insert(TypeInfo typeInfo);

        void Update(TypeInfo typeInfo);

        void Delete(int typeID);

        TypeInfo GetTypeInfo(int typeID);

        string GetTypeName(int typeID);

        ArrayList GetTypeInfoArrayList(int projectID);

        IEnumerable GetDataSource(int projectID);

        ArrayList GetTypeNameArrayList(int projectID);

        bool UpdateTaxisToUp(int typeID, int projectID);

        bool UpdateTaxisToDown(int typeID, int projectID);
	}
}
