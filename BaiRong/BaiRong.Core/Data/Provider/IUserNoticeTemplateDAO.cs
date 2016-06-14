using System.Collections;
using BaiRong.Model;
using System.Data;

namespace BaiRong.Core.Data.Provider
{
    public interface IUserNoticeTemplateDAO
    {
        int Insert(UserNoticeTemplateInfo info);

        void Update(UserNoticeTemplateInfo info);

        void Delete(int NoticeTemplateID);

        void SetIsEnable(int NoticeTemplateID);

        UserNoticeTemplateInfo GetNoticeTemplateInfo(string userNoticeType, string userNoticeTemplateType);

        UserNoticeTemplateInfo GetNoticeTemplateInfo(int NoticeTemplateID);

        string GetSqlString(string userNoticeType, string userNoticeTemplateType, string keywords);

        string GetSortFieldName();

        DataSet GetUserNoticeTemplateDS(string userNoticeType, string userNoticeTemplateType, string keywords);

    }
}
