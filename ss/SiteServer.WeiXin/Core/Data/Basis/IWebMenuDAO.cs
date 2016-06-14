using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface IWebMenuDAO
	{
        int Insert(WebMenuInfo menuInfo);

        void Update(WebMenuInfo menuInfo);

        void Delete(int menuID);

        WebMenuInfo GetMenuInfo(int menuID);

        List<WebMenuInfo> GetMenuInfoList(int publishmentSystemID, int parentID);

        IEnumerable GetDataSource(int publishmentSystemID, int parentID);

        int GetCount(int publishmentSystemID, int parentID);

        bool UpdateTaxisToUp(int publishmentSystemID, int parentID, int menuID);

        bool UpdateTaxisToDown(int publishmentSystemID, int parentID, int menuID);

        void Sync(int publishmentSystemID);
        List<WebMenuInfo> GetWebMenuInfoList(int publishmentSystemID);
        
	}
}
