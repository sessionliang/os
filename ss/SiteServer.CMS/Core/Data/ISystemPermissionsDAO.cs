using System.Collections;
using System.Data;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public interface ISystemPermissionsDAO
    {
        void InsertWithTrans(SystemPermissionsInfo info, IDbTransaction trans);

        void DeleteWithTrans(string roleName, IDbTransaction trans);

        ArrayList GetSystemPermissionsInfoArrayList(string roleName);

        SortedList GetWebsitePermissionSortedList(string[] roles);

        SortedList GetChannelPermissionSortedList(string[] roles);

        ArrayList GetChannelPermissionArrayListIgnoreNodeID(string[] roles);

        #region by 20151206 sofuny  վ��Ȩ�޽�ɫ

        /// <summary>
        /// ��ȡ��ĳ��վ��Ȩ�޵Ľ�ɫ
        /// </summary>
        /// <param name="publishmentSystemID">վ��ID</param>
        /// <param name="whereStr">��������</param>
        /// <returns></returns>
        ArrayList GetSystemPermissionsInfoArrayListByPublishmentSystemID(int publishmentSystemID, string whereStr);

        /// <summary>
        /// ���ݽ�ɫ����վ��ID��ȡ��ɫȨ����Ϣ
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        SystemPermissionsInfo GetSystemPermissionsInfoByRP(string roleName, int publishmentSystemID);


        /// <summary>
        /// ���ݽ�ɫ����վ��ID��ȡ��ɫ��վ��Ȩ����Ϣ
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        ArrayList GetWebsitePermissionListByRP(string roleName, int publishmentSystemID);

        void Update(SystemPermissionsInfo info);

        #endregion

        #region Ͷ�����

        /// <summary>
        /// ��ȡ��ɫ��վ��Ȩ��
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        ArrayList GetAllPermissionArrayList(string[] roles, int publishmentSystemID,bool iscc);
        #endregion
    }
}
