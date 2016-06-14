using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
    public interface IMLibDraftContentDAO
    {
        string TableName
        {
            get;
        }

        int Insert(MLibDraftContentInfo info);

        void Insert(ArrayList infoList);

        void Update(MLibDraftContentInfo info);

        void Delete(ArrayList infoList);

        MLibDraftContentInfo GetMLibDraftContentInfo(int id);

        ArrayList GetInfoList(int publishmentSystemID);

        ArrayList GetInfoList();



        ArrayList GetUserMLibDraftContentList(string addUserName, string title, string startdate, string enddate, int pageIndex, int prePageNum);
    }
}
