using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
    public interface IOrganizationAreaDAO : ITreeDAO
    {
        int Insert(int publishmentSystemID, int parentID, string itemName, string itemIndexName);

        int Insert(OrganizationAreaInfo info);

        void Update(OrganizationAreaInfo info);

        OrganizationAreaInfo GetInfo(int itemID);

        OrganizationAreaInfo GetInfoByNew(int itemID);

        void Delete(int deleteID);


        void DeleteByClassifyID(int classifyID);

        /// <summary>
        /// 设置默认分类
        /// </summary>
        /// <returns></returns>
        int SetDefaultInfo(int publishmentSystemID);


        OrganizationAreaInfo GetFirstInfo();

        /// <summary>
        /// 某个分类下的区域是否重复
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="classifyID"></param>
        /// <returns></returns>
        bool IsExists(string itemName, int classifyID);

        /// <summary>
        /// 加载某个分类下的最上级区域 
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="parentID"></param>
        /// <param name="classifyID"></param>
        /// <returns></returns>
        ArrayList GetItemIDArrayListByParentID(int publishmentSystemID, int parentID, int classifyID);

        /// <summary>
        /// 获取区域全称
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        string getArea(int publishmentSystemID, int itemID);

        void UpdateTaxis(int publishmentSystemID, int classifyID, int itemID, bool isSubtract);

        #region 前台方法
        ArrayList getParentArea(int classifyID);

        ArrayList getChildArea(int parenID);

        #endregion
    }
}
