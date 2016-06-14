using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
    public interface IMLibScopeDAO
    {
        string TableName
        {
            get;
        }

        int Insert(MLibScopeInfo info);

        void Insert(ArrayList infoList);

        void Update(MLibScopeInfo info);         

        MLibScopeInfo GetMLibScopeInfo(int publishmentSystemID, int nodeID);

        ArrayList GetInfoList(int publishmentSystemID);

        ArrayList GetInfoList();
    }
}
