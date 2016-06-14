using BaiRong.Model.Service;
using System.Collections;

namespace BaiRong.Core.Data.Provider
{
    public interface ITaskDAO
	{
        int Insert(TaskInfo info);

        void Update(TaskInfo info);

        void UpdateState(int taskID, bool isEnabled);

        void UpdateLastExecuteDate(int taskID);

        void Delete(int taskID);

        TaskInfo GetTaskInfo(int taskID);

        ArrayList GetTaskIDArrayList(string productID, EServiceType serviceType);

        ArrayList GetTaskInfoArrayList();

        ArrayList GetTaskInfoArrayList(string productID, EServiceType serviceType);

        ArrayList GetTaskInfoArrayList(string productID, EServiceType serviceType, int publishmentSystemID);

        SortedList GetTaskInfoSortedList();

        IEnumerable GetDataSource(string productID, EServiceType serviceType, int publishmentSystemID);

        bool IsExists(string taskName);

        bool IsSystemTaskExists(string productID, int publishmentSystemID, EServiceType serviceType);

        void DeleteSystemTask(string productID, int publishmentSystemID, EServiceType serviceType);

        string GetPublishmentSystemName(int publishmentSystemID);
	}
}
