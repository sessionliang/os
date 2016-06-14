using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
    public interface IUserNewGroupDAO : ITreeDAO
    {
        string TableName
        {
            get;
        }

        int Insert(SiteServer.CMS.Model.UserNewGroupInfo info);

        void Update(SiteServer.CMS.Model.UserNewGroupInfo info);

        bool IsExists(string subscribeName);

        void Delete(ArrayList deleteIDArrayList);

        void Delete(int subscribeID);


        SiteServer.CMS.Model.UserNewGroupInfo GetContentInfo(int contentID);


        ArrayList GetInfoList(string whrerStr);


        string GetAllString(string whereString);

        string GetSortFieldName();


        string GetName(string itemIDs);
         

        /// <summary>
        /// 设置默认分类
        /// </summary>
        /// <returns></returns>
        int SetDefaultInfo();

        SiteServer.CMS.Model.UserNewGroupInfo GetDefaultInfo();


        void UpdateEnabled(ArrayList itemIDs, EBoolean type);


        /// <summary>
        /// 修改分类内容数量
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="itemID"></param>
        /// <param name="contentNum"></param>
        void UpdateContentNum( int itemID, int contentNum);


        string GetAllNewGroupString(string whereString);
    }
}
