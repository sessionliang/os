using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using BaiRong.Model;
using System;

namespace SiteServer.CMS.Core
{
    public interface ITrialApplyDAO
    {
        string TableName
        {
            get;
        }

        int Insert(TrialApplyInfo info);

        void Inserts(ArrayList infoList);

        void Delete(int publishmentSystemID, int nodeID, int contentID, ArrayList infoList);

        ArrayList GetInfoList(int publishmentSystemID, int nodeID, int contentID, ArrayList infoList);

        string GetSelectSqlString(int publishmentSystemID, int nodeID, int contentID);


        int GetCountChecked(int publishmentSystemID, int nodeID, int contentID);


        List<TrialApplyInfo> GetInfoListChecked(int publishmentSystemID, int nodeID, int contentID);

        string GetSelectSqlString(int publishmentSystemID, List<int> channelIDList, int searchDate, ETriState checkedState, ETriState channelState);


        string GetSortFieldName();


        int GetCount(int publishmentSystemID, int nodeID, int contentID);


        void ApplyChecked(int publishmentSystemID, int nodeID, int contentID, ArrayList ids, bool isChecked, bool isReport, bool isMobile, string checkAdmin, DateTime CheckDate);

        TrialApplyInfo GetInfo(int taid);

        int Update(TrialApplyInfo info);

        int UpdateWhithTrans(TrialApplyInfo info, IDbTransaction trans);

        bool IsExists(int publishmentSystemID, int nodeID, int contentID, string userName);

        void ApplyChecked(int publishmentSystemID, int nodeID, ArrayList ids, bool isChecked,string applystatus, bool isReport, bool isMobile, string checkAdmin, DateTime CheckDate);
         
    }
}
