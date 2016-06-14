using System.Collections;
using BaiRong.Model;
using BaiRong.Model.Service;

namespace BaiRong.Core.Data.Provider
{
	public interface IStorageDAO
	{
        int Insert(StorageInfo storageInfo);

        void Update(StorageInfo storageInfo);

        void UpdateState(int storageID, bool isEnabled);

        void Delete(int storageID);

        StorageInfo GetStorageInfo(int storageID);

        bool IsExists(string storageName);

        IEnumerable GetDataSource();

        SortedList GetStorageInfoSortedList();

        ArrayList GetStorageInfoArrayListEnabled();
	}
}
