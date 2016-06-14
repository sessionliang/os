using System.Collections;
using BaiRong.Model;
using BaiRong.Model.Service;

namespace BaiRong.Core.Data.Provider
{
	public interface IFTPStorageDAO
	{
        void Insert(FTPStorageInfo ftpStorageInfo);

        void Update(FTPStorageInfo ftpStorageInfo);

        FTPStorageInfo GetFTPStorageInfo(int storageID);
	}
}
