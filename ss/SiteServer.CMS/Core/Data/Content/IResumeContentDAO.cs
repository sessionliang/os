using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
	public interface IResumeContentDAO
	{
        string TableName
        {
            get;
        }

        int Insert(ResumeContentInfo info);

        void Update(ResumeContentInfo info);

        void SetIsView(ArrayList idCollection, bool isView);

        void Delete(ArrayList deleteIDArrayList);

        void Delete(int styleID);

        ResumeContentInfo GetContentInfo(int publishmentSystemID, int styleID, NameValueCollection form);

        ResumeContentInfo GetContentInfo(int contentID);

        int GetCount(int styleID);

        int GetCount(int publishmentSystemID, int jobContentID);

        string GetSelectStringOfID(int styleID, string whereString);

        string GetSelectStringOfID(int publishmentSystemID, int jobContentID, string whereString);

        string GetSelectSqlString(int styleID, int startNum, int totalNum, string whereString, string orderByString);

        string GetSortFieldName();
	}
}
