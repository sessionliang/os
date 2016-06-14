using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
    public interface ISubscribeDAO : ITreeDAO
    {
        string TableName
        {
            get;
        }

        int Insert(SubscribeInfo info);

        void Update(SubscribeInfo info);

        bool IsExists(string subscribeName);

        void Delete(int publishmentSystemID, ArrayList deleteIDArrayList);

        void Delete(int publishmentSystemID, int subscribeID);


        SubscribeInfo GetContentInfo(int contentID);

        SubscribeInfo GetContentInfo(int publishmentSystemID, int contentID);

        ArrayList GetInfoList(int publishmentSystemID,string whrerStr);


        string GetAllString(int publishmentSystemID, string whereString);

        string GetSortFieldName();

        void UpdateSubscribeNum(int publishmentSystemID, string classifyID, int type);

        string UpdateSubscribeNumStr(int publishmentSystemID, string classifyID, bool type);

        string GetName(int publishmentSystemID, string itemIDs);


        bool GetValueByUserID(int publishmentSystemID, string userID, out string columnValue, out  string cabelValue);


        /// <summary>
        /// 设置默认分类
        /// </summary>
        /// <returns></returns>
        int SetDefaultInfo(int publishmentSystemID);

        SubscribeInfo GetDefaultInfo(int publishmentSystemID);


        void UpdateEnabled(int publishmentSystemID, ArrayList itemIDs, EBoolean type);
         
    }
}
