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

        #region by 20151206 sofuny  站点权限角色

        /// <summary>
        /// 获取有某个站点权限的角色
        /// </summary>
        /// <param name="publishmentSystemID">站点ID</param>
        /// <param name="whereStr">增加条件</param>
        /// <returns></returns>
        ArrayList GetSystemPermissionsInfoArrayListByPublishmentSystemID(int publishmentSystemID, string whereStr);

        /// <summary>
        /// 根据角色名和站点ID获取角色权限信息
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        SystemPermissionsInfo GetSystemPermissionsInfoByRP(string roleName, int publishmentSystemID);


        /// <summary>
        /// 根据角色名和站点ID获取角色的站点权限信息
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        ArrayList GetWebsitePermissionListByRP(string roleName, int publishmentSystemID);

        void Update(SystemPermissionsInfo info);

        #endregion

        #region 投稿管理

        /// <summary>
        /// 获取角色的站点权限
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        ArrayList GetAllPermissionArrayList(string[] roles, int publishmentSystemID,bool iscc);
        #endregion
    }
}
