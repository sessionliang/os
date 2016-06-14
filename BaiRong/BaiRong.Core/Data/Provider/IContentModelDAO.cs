using System.Collections;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface IContentModelDAO
    {
        void Insert(ContentModelInfo contentModelInfo);

        void Update(ContentModelInfo contentModelInfo);

        void Delete(string modelID, string productID, int siteID);

        ContentModelInfo GetContentModelInfo(string modelID, string productID, int siteID);

        IEnumerable GetDataSource(string productID, int siteID);

        ArrayList GetContentModelInfoArrayList(string productID, int siteID);

        /// <summary>
        /// 导入导出ContentModel
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="modelID"></param>
        /// <returns></returns>
        string GetImportContentModelID(int publishmentSystemID, string modelID, string productID);
    }
}
