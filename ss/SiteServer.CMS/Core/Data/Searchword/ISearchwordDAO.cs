using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public interface ISearchwordDAO
    {
        void Insert(SearchwordInfo searchwordInfo);

        void Update(SearchwordInfo searchwordInfo);

        void Delete(ArrayList deleteIDArrayList);

        void Delete(int searchwordID);

        SearchwordInfo GetSearchwordInfo(int searchwordID);

        SearchwordInfo GetSearchwordInfo(int publishmentSystemID, string searchword);

        string GetSelectString(int publishmentSystemID, string where);

        string GetSortFieldName(int publishmentSystemID);

        ArrayList GetSearchwordInfoArrayList(int publishmentSystemID, string where);

        /// <summary>
        /// 前台搜索联想词
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        ArrayList GetSearchwordInfoArrayListForRelated(int publishmentSystemID, string searchword);

        bool IsExists(int publishmentSystemID, string searchword);

        /// <summary>
        /// 更新搜索关键词的搜索结果数
        /// </summary>
        /// <param name="id"></param>
        void UpdateSearchResultCount(int id);

        /// <summary>
        /// 更新一组搜索关键词的搜索结果数
        /// </summary>
        /// <param name="id"></param>
        void UpdateSearchResultCount(ArrayList arraylist);

        /// <summary>
        /// 更新全部搜索关键词的搜索结果数
        /// </summary>
        /// <param name="id"></param>
        void UpdateSearchResultCountAll(int publishmentSystemID);
    }
}
