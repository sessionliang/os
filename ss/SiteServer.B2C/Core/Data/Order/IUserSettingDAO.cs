using System.Collections;
using SiteServer.B2C.Model;
using System.Collections.Generic;

namespace SiteServer.B2C.Core
{
	public interface IUserSettingDAO
	{
        UserSettingInfo GetSettingInfo(string userName, string sessionID);

        void Update(UserSettingInfo settingInfo);
	}
}
