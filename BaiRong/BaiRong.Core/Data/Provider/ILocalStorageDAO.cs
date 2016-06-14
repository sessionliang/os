using System.Collections;
using BaiRong.Model;
using BaiRong.Model.Service;

namespace BaiRong.Core.Data.Provider
{
	public interface ILocalStorageDAO
	{
        void Insert(LocalStorageInfo localStorageInfo);

        void Update(LocalStorageInfo localStorageInfo);

        LocalStorageInfo GetLocalStorageInfo(int storageID);

        string GetDirectoryPath(int storageID);
	}
}
