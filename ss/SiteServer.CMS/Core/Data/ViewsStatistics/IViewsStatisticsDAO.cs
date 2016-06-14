using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
    public interface IViewsStatisticsDAO
    {
        string TableName
        {
            get;
        }


        int Insert(ViewsStatisticsInfo info);

        void Update(ViewsStatisticsInfo info);

        string GetAllString(int publishmentSystemID, string whereString);

        ViewsStatisticsInfo IsExists(ViewsStatisticsInfo info);

        ArrayList GetMaxNode(int publishmentSystemID, int userID, string whereStr);

        ArrayList GetMaxNode(int publishmentSystemID, int userID,EIntelligentPushType type, string whereStr);


        string GetAllString(int publishmentSystemID, string userName, string dateFrom, string dateTo);
	}
}
