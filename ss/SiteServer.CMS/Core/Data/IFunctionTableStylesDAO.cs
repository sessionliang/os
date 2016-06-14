using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
    public interface IFunctionTableStylesDAO
    {
        string TableName
        {
            get;
        }

        int Insert(FunctionTableStyles info);

        void Insert(ArrayList infoList, bool deleteAll);

        void Delete(int publishmentSystemID, int nodeID, int contentID, string tableStyle, ArrayList infoList); 

        ArrayList GetInfoList(int publishmentSystemID, int nodeID, int contentID, string tableStyle, string type);


        /// <summary>
        /// 获取对应功能下的数据结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable getContentAnalysis(string sql);

    }
}
