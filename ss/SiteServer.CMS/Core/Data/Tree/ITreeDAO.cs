using SiteServer.CMS.Controls;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.CMS.Core
{
    public interface ITreeDAO
    {
        /// <summary>
        /// 获取自己和子集
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        ArrayList GetItemIDArrayListByItemID(int publishmentSystemID);

        /// <summary>
        /// 获取子集ID
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        ArrayList GetItemIDArrayListByParentID(int publishmentSystemID, int parentID);

        /// <summary>
        /// 获取子集ID
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        ArrayList GetItemIDArrayListByParentID(int publishmentSystemID, int parentID, string where);

        /// <summary>
        /// 获取子集模型
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        List<TreeBaseItem> GetItemInfoArrayListByParentID(int publishmentSystemID, int parentID);

        /// <summary>
        /// 获取子集模型
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        List<TreeBaseItem> GetItemInfoArrayListByParentID(int publishmentSystemID, int parentID, string where);

        /// <summary>
        /// 获取站点下的所有的item
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<TreeBaseItem> GetItemInfoArrayListByPublishmentSystemID(int publishmentSystemID, string where);

        /// <summary>
        /// 获取TopID
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        int GetTopID(int publishmentSystemID);

        TreeBaseItem GetItemInfo(int publishmentSystemID, int itemID);

        string GetItemName(int publishmentSystemID, int itemID);

        void UpdateTaxis(int publishmentSystemID, int itemID, bool isSubtract);

        int GetMaxTaxisByParentPath(string parentPath);

        /// <summary>
        /// 获取所有的indexName
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        ArrayList GetItemIndexNameArrayList(int publishmentSystemID);

        /// <summary>
        /// 修改分类内容数量
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="itemID"></param>
        /// <param name="contentNum"></param>
        void UpdateContentNum(int publishmentSystemID, int itemID, int contentNum);

        /// <summary>
        /// 修改栏目父级之后，修改节点数目
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="oldParentID"></param>
        /// <param name="newParentID"></param>
        void UpdateItemNum(TreeBaseItem oldParentInfo, TreeBaseItem newParentInfo);
         

        /// <summary>
        /// by 20151229 sofuny
        /// 
        /// 加载有分类的树
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="classifyID"></param>
        /// <returns></returns>
        ArrayList GetItemInfoByClassifyID(int publishmentSystemID, int classifyID);


        /// <summary>
        /// by 20151206 sofuny
        /// 
        /// 获取子集模型
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        List<TreeBaseItem> GetItemInfoArrayListByParentID(int publishmentSystemID, int parentID,ArrayList itemList);


        /// <summary>
        /// 获取子集ID
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        ArrayList GetItemIDArrayListByParentID(int publishmentSystemID, int parentID, ArrayList itemList);
    }
}
