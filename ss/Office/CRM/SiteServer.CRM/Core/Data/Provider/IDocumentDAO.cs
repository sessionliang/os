using System.Collections;
using SiteServer.CRM.Model;
using BaiRong.Model;

namespace SiteServer.CRM.Core
{
	public interface IDocumentDAO
	{
        void Insert(DocumentInfo documentInfo);

        void Delete(int documentID);

        DocumentInfo GetDocumentInfo(int documentID);

        int GetCountByCategory(int typeID);

        int GetCountByContract(int contractID);

        IEnumerable GetDataSourceByCategory(int typeID);

        IEnumerable GetDataSourceByContract(int contractID);

        IEnumerable GetDataSource(string userName);

        IEnumerable GetDataSource();
	}
}
