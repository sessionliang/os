using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
    public interface IConsoleLogDAO
    {
        string TableName
        {
            get;
        }

        int Insert(ConsoleLogInfo info);

        void Insert(ArrayList infoList);

        ArrayList GetInfoList(int publishmentSystemID, int nodeID, int contentID, string actionType, string type);

        ArrayList GetInfoList(string actionType, string userName);

        ArrayList GetInfoList(string actionType, string userName, string startdate, string enddate, int pageIndex, int prePageNum);

        int GetCount(string actionType, string userName);

    }
}
