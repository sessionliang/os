using System;
using System.Data;
using System.Collections;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
	public interface IUserConfigDAO
	{
        void Update(UserConfigInfo info);

        bool IsExists();

        UserConfigInfo GetUserConfigInfo();

        void InitializeUserRole(string consoleUserName, string consolePassword);
	}
}
