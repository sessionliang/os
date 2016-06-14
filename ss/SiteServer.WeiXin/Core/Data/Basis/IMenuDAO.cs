using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface IMenuDAO
	{
        int Insert(MenuInfo menuInfo);

        void Update(MenuInfo menuInfo);

        void Delete(int menuID);

        MenuInfo GetMenuInfo(int menuID);

        IEnumerable GetDataSource(int publishmentSystemID, int parentID);

        int GetCount(int parentID);

        List<MenuInfo> GetMenuInfoList(int publishmentSystemID, int parentID);

        bool UpdateTaxisToUp(int parentID, int menuID);

        bool UpdateTaxisToDown(int parentID, int menuID);

        List<MenuInfo> GetMenuInfoList(int publishmentSystemID);
	}
}
