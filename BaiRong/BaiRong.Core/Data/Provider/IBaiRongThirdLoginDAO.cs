using BaiRong.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiRong.Core.Data.Provider
{
    public interface IBaiRongThirdLoginDAO
    {
        int Insert(BaiRongThirdLoginInfo thirdLoginInfo);

        void Update(BaiRongThirdLoginInfo thirdLoginInfo);

        void Delete(int thirdLoginID);

        BaiRongThirdLoginInfo GetSiteserverThirdLoginInfo(int thirdLoginID);

        bool IsExists(string thirdLoginName);

        bool IsExists(ESiteserverThirdLoginType thirdLoginType);

        IEnumerable GetDataSource();

        List<BaiRongThirdLoginInfo> GetSiteserverThirdLoginInfoList();

        bool UpdateTaxisToUp(int thirdLoginID);

        bool UpdateTaxisToDown(int thirdLoginID);

        void InsertUserBinding(int userID, string thirdLoginType, string thirdLoginUserID);

        int GetUserBindingCount(string ThirdLoginUserID);
        /// <summary>
        /// 设置默认登录方式
        /// </summary>
        void SetDefaultThirdLogin();
    }
}
