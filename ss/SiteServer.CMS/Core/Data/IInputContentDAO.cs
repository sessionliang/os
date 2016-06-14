using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
    public interface IInputContentDAO
    {
        string TableName
        {
            get;
        }

        int Insert(InputContentInfo info);

        void Update(InputContentInfo info);

        void UpdateIsChecked(ArrayList contentIDArrayList);

        bool UpdateTaxisToUp(int inputID, int contentID);

        bool UpdateTaxisToDown(int inputID, int contentID);

        void Delete(int inputID, ArrayList deleteIDArrayList);

        void Delete(int inputID);

        void Check(int inputID, ArrayList contentIDArrayList);

        InputContentInfo GetContentInfo(int contentID);

        int GetCountChecked(int inputID);

        int GetCountUnChecked(int inputID);

        DataSet GetDataSetNotChecked(int inputID);

        DataSet GetDataSetWithChecked(int inputID);

        IEnumerable GetStlDataSourceChecked(int inputID, int totalNum, string orderByString, string whereString);

        ArrayList GetContentIDArrayListWithChecked(int inputID);

        ArrayList GetContentIDArrayListWithChecked(int inputID, ArrayList searchFields, string keyword);

        ArrayList GetContentIDArrayListByUserName(string userName);

        string GetValue(int contentID, string attributeName);

        string GetSelectStringOfContentID(int inputID, string whereString);

        string GetSelectSqlStringWithChecked(int publishmentSystemID, int inputID, bool isReplyExists, bool isReply, int startNum, int totalNum, string whereString, string orderByString, NameValueCollection otherAttributes);

        string GetSortFieldName();

        /// <summary>
        /// 校验是否重复数据
        /// </summary>
        /// <param name="uniquePro">校验字段</param>
        /// <param name="value">校验值</param>
        /// <returns></returns>
        bool IsExistsPro(int inputId, string uniquePro, string value);
    }
}
