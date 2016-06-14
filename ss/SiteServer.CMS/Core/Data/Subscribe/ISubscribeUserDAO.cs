using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
    public interface ISubscribeUserDAO
    {
        string TableName
        {
            get;
        }

        string GetSelectCommend(int publishmentSystemID, string itemId, string mobile, string email, string dateFrom, string dateTo, ETriState checkedState);


        int Insert(SubscribeUserInfo info);

        void Update(SubscribeUserInfo info);

        void Delete(int publishmentSystemID, ArrayList deleteIDArrayList);

        void Delete(int publishmentSystemID, int subscribeID);

        void DeleteByWhereStr(int publishmentSystemID, string whereStr);

        void Delete(int publishmentSystemID, string subscribeUserIDs);
        void ChangeSubscribeStatu(int publishmentSystemID, int subscribeUserID, EBoolean subscribeStatu);
        void ChangeSubscribeStatu(int publishmentSystemID, ArrayList arrayList, EBoolean subscribeStatu);

        ArrayList GetSubscribeUserList(int publishmentSystemID, ArrayList arrayList);

        /// <summary>
        /// ��ȡ���������µĻ�Ա�б�
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="arrayList"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        ArrayList GetSubscribeUserList(int publishmentSystemID, ArrayList arrayList, string whereStr);

        SubscribeUserInfo GetContentInfo(int contentID);

        SubscribeUserInfo GetContentInfo(int publishmentSystemID, string email);

        string GetSortFieldName();

        bool IsExists(string email);


        void UpdatePushNum(int publishmentSystemID, int subscribeUserID);

        void UpdatePushNumByEmail(int publishmentSystemID, string email);


        ArrayList GetSubscribeUserList(int publishmentSystemID, EBoolean state);

        /// <summary>
        /// ȡ�����ж�����ĳЩ���ݵĻ�Ա����
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="subscribeIDs"></param>
        void UpdateHasSubscribe(int publishmentSystemID, ArrayList subscribeIDs);


        /// <summary>
        /// �޸Ķ�����ĳЩ���ݵ�ĳЩ��Ա�Ķ��������ֶ�
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="subscribeIDs"></param>
        /// <param name="userIDs"></param>
        /// <param name="state"></param>
        void UpdateHasSubscribe(int publishmentSystemID, ArrayList subscribeIDs, ArrayList userIDs, EBoolean state);

        /// <summary>
        /// ��ȡ�������ݵĻ�Ա����
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        int GetCount(int publishmentSystemID, string whereStr);


        /// <summary>
        /// ����ĳЩ�����µĻ�Ա״̬ 
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        void UpdateSubscribeStatu(int publishmentSystemID, string whereStr, EBoolean subscribeStatu);

        /// <summary>
        /// ���ĳЩ�����»�Ա�Ķ������� 
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        void ClearSubscribeName(int publishmentSystemID, ArrayList userList, string whereStr);

        /// <summary>
        /// ���ӵ��޸�
        /// </summary>
        /// <param name="info"></param>
        /// <param name="oldSubID"></param>
        void Update(SubscribeUserInfo info, string oldSubID);

        /// <summary>
        /// ����Ա���¶�������
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="subscribeIDs"></param>
        /// <param name="userIDs"></param> 
        void UpdateUserSubscribe(int publishmentSystemID, ArrayList subscribeIDs, ArrayList userIDs);
    }
}
