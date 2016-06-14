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
        /// ��ʼ��Ĭ���ܱ�����
        /// </summary>
        void SetDefaultQuestion();
    }
}
