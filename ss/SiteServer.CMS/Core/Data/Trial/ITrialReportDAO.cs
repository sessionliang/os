using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
    public interface ITrialReportDAO
    {
        string TableName
        {
            get;
        }

        int Insert(TrialReportInfo info);

        void Insert(ArrayList infoList, bool deleteAll);

        void Delete(int publishmentSystemID, int nodeID, int contentID, ArrayList infoList); 

        ArrayList GetInfoList(int publishmentSystemID, int nodeID, int contentID, string tableStyle );

        string GetSelectSqlString(int publishmentSystemID, int nodeID, int contentID);


        int GetCountChecked(int publishmentSystemID, int nodeID, int contentID);


        List<TrialReportInfo> GetInfoListChecked(int publishmentSystemID, int nodeID, int contentID);

        string GetSelectSqlString(int publishmentSystemID, List<int> channelIDList, string keyword, int searchDate, ETriState checkedState, ETriState channelState);


        string GetSortFieldName();

        TrialReportInfo GetInfo(int publishmentSystemID, int nodeID, int contentID, int trid);

        void UpdateStatus(int publishmentSystemID, int nodeID, int contentID, int trid, string
            adminName);

        /// <summary>
        /// 获取某个栏目的综合平均分
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        object GetSelectCommendOfAnalysisByNode(int publishmentSystemID, int nodeID, string begin, string end);

        int GetReportCountByApplyID(int publishmentSystemID, int nodeID, int contentID, int taid);
    }
}
