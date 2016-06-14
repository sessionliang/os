using System.Collections;
using System.Data;
using System;
using BaiRong.Model;
using System.Collections.Generic;

namespace BaiRong.Core.Data.Provider
{
    public interface IUserLogDAO
    {
        void Insert(UserLogInfo userLog);

        void Delete(ArrayList userLogIDArrayList);

        void DeleteAll();

        string GetSelectCommend();

        string GetSelectCommend(string userName, string keyword, string dateFrom, string dateTo);

        int GetCount();

        int GetCount(string where);

        DateTime GetLastUserLoginDate(string userName);

        DateTime GetLastRemoveUserLogDate(string userName);

        List<UserLogInfo> GetUserLoginLog(string userName);


        List<UserLogInfo> GetUserLoginLogByPage(string userName, int pageIndex, int prePageNum);
    }
}
