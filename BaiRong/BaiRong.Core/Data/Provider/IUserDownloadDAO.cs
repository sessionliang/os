using System;
using System.Data;
using System.Collections;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections.Specialized;

namespace BaiRong.Core.Data.Provider
{
	public interface IUserDownloadDAO
	{
        void Insert(UserDownloadInfo downloadInfo);

        void Update(UserDownloadInfo downloadInfo);

        void Delete(ArrayList deleteIDArrayList);

        void Delete(int contactID);

        UserDownloadInfo GetDownloadInfo(string createUserName, int taxis, int downloads, NameValueCollection form);

        UserDownloadInfo GetDownloadInfo(int downloadID);

        string GetSelectString();

        string GetSelectString(string createUserName);

        string GetSortFieldName();
	}
}
