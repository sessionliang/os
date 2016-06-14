using System;
using System.Data;
using System.Collections;
using BaiRong.Model;
using System.Collections.Generic;

namespace BaiRong.Core.Data.Provider
{
	public interface IUserCreditsLogDAO
	{
        void Insert(UserCreditsLogInfo info);

        void Delete(ArrayList idArrayList);

        List<UserCreditsLogInfo> GetUserCreditsLogInfoList(string userName, string productID, string action);

        string GetSqlString(string userName);

        string GetSqlString(string productID, ArrayList userNameArrayList);

        string GetSortFieldName();
	}
}
