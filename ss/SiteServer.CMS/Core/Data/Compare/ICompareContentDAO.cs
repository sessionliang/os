using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
    public interface ICompareContentDAO
    {
        string TableName
        {
            get;
        }

        int Insert(CompareContentInfo info);

        void Insert(ArrayList infoList, bool deleteAll);

        void Delete(int publishmentSystemID, int nodeID, int contentID, ArrayList infoList);

        ArrayList GetInfoList(int publishmentSystemID, int nodeID, int contentID);

        string GetSelectSqlString(int publishmentSystemID, int nodeID, int contentID);


        int GetCountChecked(int publishmentSystemID, int nodeID, int contentID);


        List<CompareContentInfo> GetInfoListChecked(int publishmentSystemID, int nodeID, int contentID);

        string GetSelectSqlString(int publishmentSystemID, List<int> channelIDList, string keyword, int searchDate, ETriState checkedState, ETriState channelState);


        string GetSortFieldName();

        /// <summary>
        /// ��ȡվ����Ŀ�ۺ���������
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        string GetSelectCommendOfAnalysis(int publishmentSystemID, string begin, string end);

        /// <summary>
        /// ��ȡĳ����Ŀ���ۺ�ƽ����
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        object GetSelectCommendOfAnalysisByNode(int publishmentSystemID, int nodeID, string begin, string end);


        CompareContentInfo GetInfo(int publishmentSystemID, int nodeID, int contentID, int id);


        bool IsExists(int publishmentSystemID, int nodeID, int contentID, string userName);


        void UpdateStatus(int publishmentSystemID, int nodeID, int contentID, int id, string adminName);
    }
}
