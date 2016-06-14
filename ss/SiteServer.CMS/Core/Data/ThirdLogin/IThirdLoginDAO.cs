using BaiRong.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.CMS.Core
{
    public interface ISiteserverThirdLoginDAO
    {
        int Insert(SiteserverThirdLoginInfo thirdLoginInfo);

        void Update(SiteserverThirdLoginInfo thirdLoginInfo);

        void Delete(int thirdLoginID);

        SiteserverThirdLoginInfo GetSiteserverThirdLoginInfo(int thirdLoginID);

        bool IsExists(int publishmentSystemID, string thirdLoginName);

        bool IsExists(int publishmentSystemID, SiteserverEThirdLoginType thirdLoginType);

        IEnumerable GetDataSource(int publishmentSystemID);

        List<SiteserverThirdLoginInfo> GetSiteserverThirdLoginInfoList(int publishmentSystemID);

        bool UpdateTaxisToUp(int publishmentSystemID, int thirdLoginID);

        bool UpdateTaxisToDown(int publishmentSystemID, int thirdLoginID);

        void InsertUserBinding(int userID, string thirdLoginType, string thirdLoginUserID);

        int GetUserBindingCount(string ThirdLoginUserID);
    }
}
