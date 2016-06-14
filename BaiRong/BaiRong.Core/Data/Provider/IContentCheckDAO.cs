using System;
using System.Collections;
using System.Data;
using BaiRong.Model;
using System.Text;
using System.Collections.Specialized;

namespace BaiRong.Core.Data.Provider
{
	public interface IContentCheckDAO
	{
        void Insert(ContentCheckInfo checkInfo);

        void Delete(int checkID);

        ContentCheckInfo GetCheckInfo(int checkID);

        ContentCheckInfo GetCheckInfoByLastID(string tableName, int contentID);

        ArrayList GetCheckInfoArrayList(string tableName, int contentID);
	}
}
