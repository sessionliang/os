using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
    public interface ISubscribePushRecordDAO
    {
        string TableName
        {
            get;
        }


        int Insert(SubscribePushRecordInfo info);
         
        void Update(SubscribePushRecordInfo info);

        string GetAllString(int publishmentSystemID, string whereString);


        string GetSelectCommend(int publishmentSystemID, string mobile, string email, string dateFrom, string dateTo, ETriState checkedState);

        ArrayList GetSubscribeUserList(int publishmentSystemID, ArrayList arrayList);
	}
}
