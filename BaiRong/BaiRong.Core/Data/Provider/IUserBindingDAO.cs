using System;
using System.Data;
using System.Collections;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface IUserBindingDAO
    {
        //void Insert(UserBindingInfo info);

        //void Update(UserBindingInfo info);

        //void Delete(int bindingID);

        //UserBindingInfo GetUserBindingInfo(string bindingName);

        //UserBindingInfo GetUserBindingInfo(string bindingType, string userName);

        //UserBindingInfo GetUserBindingInfo(string bindingType, int bindingID);

        //UserBindingInfo GetUserBindingInfoByBindIDAndType(string bindingType, int bindingID);

        //int GetBindingCount(string userName);

        //IEnumerable GetDataSource(string userName);

        //IEnumerable GetBindNameDataSource(string userName);


        void DeleteByUserID(int userID, string thirdLoginType);

        void DeleteByThirdUserID(string thirdUserID);

        string GetUserBindByUserAndType(int userID, string thirdLoginType);
    }
}
