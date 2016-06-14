using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
	public interface IAccountDAO
	{
        int Insert(AccountInfo couponInfo);

        void Update(AccountInfo couponInfo);

        AccountInfo GetAccountInfo(int publishmentSystemID);

        List<AccountInfo> GetAccountInfoList(int publishmentSystemID);
	}
}
