using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
    public interface IWebsiteMessageContentDAO
    {
        string TableName
        {
            get;
        }

        int Insert(WebsiteMessageContentInfo info);

        void Update(WebsiteMessageContentInfo info);

        void UpdateIsChecked(ArrayList contentIDArrayList);

        bool UpdateTaxisToUp(int websiteMessageID, int classifyID, int contentID);

        bool UpdateTaxisToDown(int websiteMessageID, int classifyID, int contentID);

        void Delete(int websiteMessageID, ArrayList deleteIDArrayList);

        void Delete(int websiteMessageID, int classifyID);

        void Check(int websiteMessageID, ArrayList contentIDArrayList);

        WebsiteMessageContentInfo GetContentInfo(int contentID);

        int GetCountChecked(int websiteMessageID, int classifyID);

        int GetCountUnChecked(int websiteMessageID, int classifyID);

        DataSet GetDataSetNotChecked(int websiteMessageID, int classifyID);

        DataSet GetDataSetWithChecked(int websiteMessageID, int classifyID);

        IEnumerable GetStlDataSourceChecked(int websiteMessageID, int classifyID, int totalNum, string orderByString, string whereString);

        ArrayList GetContentIDArrayListWithChecked(int websiteMessageID, int classifyID);

        ArrayList GetContentIDArrayListWithChecked(int websiteMessageID, int classifyID, ArrayList searchFields, string keyword);

        ArrayList GetContentIDArrayListByUserName(string userName);

        string GetValue(int contentID, string attributeName);

        string GetSelectStringOfContentID(int websiteMessageID, int classifyID, string searchType, string searchWord, string whereString);

        string GetSelectSqlStringWithChecked(int publishmentSystemID, int websiteMessageID, int classifyID, bool isReplyExists, bool isReply, int startNum, int totalNum, string whereString, string orderByString, NameValueCollection otherAttributes);

        string GetSortFieldName();

        /// <summary>
        /// ×ªÒÆÄÚÈÝ
        /// </summary>
        /// <param name="contentIDArrayList"></param>
        /// <param name="v"></param>
        void TranslateContent(ArrayList contentIDArrayList, int classifyID);

        string GetSelectSqlStringWithChecked(int publishmentSystemID, int websiteMessageID, bool isReplyExists, bool isReply, int startNum, int totalNum, string where, string orderByString, NameValueCollection otherAttributes);
    }
}
