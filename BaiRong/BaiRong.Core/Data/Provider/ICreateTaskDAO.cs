using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface ICreateTaskDAO
    {
        int Insert(CreateTaskInfo info);

        void Update(CreateTaskInfo info);

        void Delete(ArrayList deleteIDArrayList);

        void Delete(int id);

        CreateTaskInfo GetCreateTaskInfo(int createTaskID);

        string GetSelectSqlString(int startNum, int totalNum, string whereString, string orderByString);

        void GetQueuingTaskCount(int id, out int totalCount, out int currentCount, out int errorCount, out int queuingCount);

        void UpdateState(int createTaskID, ECreateTaskType eCreateTaskType);

        void UpdateState(int createTaskID);

        void UpdateState();

        ArrayList GetCreateTaskInfoArrayList(string where);

        void UpdateStartTime(int createTaskID);

        void UpdateEndTime(int createTaskID);
    }
}
