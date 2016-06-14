using System.Collections;
using SiteServer.Project.Model;
using BaiRong.Model;
using System.Collections.Generic;

namespace SiteServer.Project.Core
{
	public interface IProjectDAO
	{
        int Insert(ProjectInfo projectInfo);

        void Update(ProjectInfo projectInfo);

        void Delete(int projectID);

        ProjectInfo GetProjectInfo(int projectID);

        ProjectInfo GetProjectInfo(string projectName);

        string GetProjectName(int projectID);

        ArrayList GetProjectInfoArrayList(bool isClosed);

        Dictionary<int, string> GetProjectIDWithNameDictionary(string userName);

        int GetCount();

        IEnumerable GetDataSource(bool isClosed);

        IEnumerable GetDataSource(string type);

        ArrayList GetProjectNameArrayList();

        int GetAmountNet(int year, int month);

        Dictionary<int, int> GetProjectIDAmountNetDictionary(int year, int month);
	}
}
