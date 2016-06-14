using System;
using System.Data;
using System.Collections;
using BaiRong.Model;
using System.Collections.Generic;

namespace BaiRong.Core.Data.Provider
{
    public interface IUserDAO
    {
        string TABLE_NAME { get; }

        bool Insert(UserInfo userInfo, out string errorMessage);

        void InsertWithoutValidation(UserInfo userInfo);

        UserInfo GetUserInfo(string groupSN, string userName);

        UserInfo GetUserInfoByNameOrEmailOrMobile(string groupSN, string userName);

        UserInfo GetUserInfo(int userID);

        UserInfo GetUserInfoByLoginName(string groupSN, string loginName);

        UserInfo GetUserInfoByCreateIPAddress(string ipAddress);

        void Update(UserInfo userInfo);

        bool UpdateBasic(UserInfo userInfo, out string errorMessage);

        void AddOnlineSeconds(string groupSN, string userName, int seconds);

        bool ChangePassword(int userID, string password);

        string GetPassword(int userID);

        void SetGroupID(string groupSN, string userName, int groupID);

        void SetGroupID(string groupSN, ArrayList userNameArrayList, int groupID);

        void AddCredits(string groupSN, string userName, int credits);

        void Check(List<int> userIDList);

        void Check(int userID);

        void Lock(List<int> userIDList, bool isLockOut);

        void Delete(int userID);

        bool IsUserExists(string groupSN, string userName);

        bool IsExists(string groupSN, string user);

        bool IsUserNameCompliant(string userName);

        bool IsEmailExists(string groupSN, string email);

        bool IsMobileExists(string groupSN, string mobile);

        ArrayList GetUserInfoArrayList(ETriState checkedState);

        ArrayList GetUserNameArrayList(bool isChecked);

        List<int> GetUserIDList(bool isChecked);

        int GetUserID(string groupSN, string userName);

        int GetUserIDByEmailOrMobile(string groupSN, string email, string mobile);

        ArrayList GetUserNameArrayListByGroupIDCollection(string groupIDCollection);

        List<int> GetUserIDListByGroupIDCollection(string groupIDCollection);

        ArrayList GetUserNameArrayList(string searchWord, int dayOfCreate, int dayOfLastActivity, bool isChecked);

        string GetSelectCommand(string groupSN, bool isChecked);

        string GetSelectCommand(string groupSN, string searchWord, int dayOfCreate, int dayOfLastActivity, bool isChecked, int groupID);

        string GetSelectCommand(string groupSN, int levelID, string searchWord, int dayOfCreate, int dayOfLastActivity, bool isChecked);

        string GetSelectCommand(string groupSN, int levelID, string searchWord, int dayOfCreate, int dayOfLastActivity, bool isChecked, int loginNum);

        string GetSelectCommand(string groupSN, int levelID, string searchWord, int dayOfCreate, int dayOfLastActivity, bool isChecked, int loginNum, string searchType);

        string GetSelectCommandByNewGroup(string groupSN, string searchWord, bool isChecked, int newGroupID);

        string GetSortFieldName();

        string GetUserName(int userID);

        string GetEmail(int userID);

        string GetMobile(int userID);

        int GetTotalCount();

        bool Import(UserInfo userInfo);

        IEnumerable GetStlDataSource(int startNum, int totalNum, string orderByString, string whereString);

        #region 验证相关

        string EncodePassword(string password, EPasswordFormat passwordFormat, string passwordSalt);

        string DecodePassword(string password, EPasswordFormat passwordFormat, string passwordSalt);

        bool Validate(string groupSN, string userName, string password, out string errorMessage);

        bool ValidateByLoginName(string groupSN, string loginName, string password, out string userName, out string errorMessage);

        void Login(string groupSN, string userName, bool persistent);

        void Logout();

        string CurrentGroupSN { get; }

        string CurrentUserName { get; }

        bool IsAnonymous { get; }

        #endregion

        /// <summary>
        /// 用户添加量统计
        /// add by sessionliang at 20151224
        /// </summary>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        Hashtable GetTrackingHashtable(DateTime dataFrom, DateTime dataTo, string xType);

        /// <summary>
        /// 修改用户投稿有效期
        /// </summary>
        /// <param name="userIDs"></param>
        /// <param name="validityDate"></param>
        void UpdateMLibValidityDate(ArrayList userIDs, DateTime validityDate);
    }
}
