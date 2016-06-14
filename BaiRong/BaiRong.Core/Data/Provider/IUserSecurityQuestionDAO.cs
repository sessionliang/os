using System.Collections;
using BaiRong.Model;
using System.Data;
using System.Collections.Generic;

namespace BaiRong.Core.Data.Provider
{
    public interface IUserSecurityQuestionDAO
    {
        void Insert(UserSecurityQuestionInfo info);

        void Update(UserSecurityQuestionInfo info);

        void Delete(int SecurityQuestionID);


        UserSecurityQuestionInfo GetSecurityQuestionInfo(int SecurityQuestionID);

        string GetSqlString(string keywords);

        DataSet GetSecurityQuestionDS(string keywords);

        List<UserSecurityQuestionInfo> GetSecurityQuestionInfoList();

        /// <summary>
        /// 初始化默认密保问题
        /// </summary>
        void SetDefaultQuestion();
    }
}
