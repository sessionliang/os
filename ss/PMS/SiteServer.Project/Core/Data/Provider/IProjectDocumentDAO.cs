using System.Collections;
using SiteServer.Project.Model;
using BaiRong.Model;

namespace SiteServer.Project.Core
{
	public interface IProjectDocumentDAO
	{
        void Insert(ProjectDocumentInfo documentInfo);

        void Delete(int documentID);

        ProjectDocumentInfo GetDocumentInfo(int documentID);

        int GetCount(int projectID);

        IEnumerable GetDataSource(int projectID);
	}
}
