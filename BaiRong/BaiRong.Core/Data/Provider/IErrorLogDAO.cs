using System.Collections;
using System.Data;
using System;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
	public interface IErrorLogDAO
	{
        void Insert(ErrorLogInfo logInfo);

		void Delete(ArrayList logIDArrayList);

        void Delete(int days, int counter);

        void DeleteAll();

        string GetSelectCommend();

        string GetSelectCommend(string keyword, string dateFrom, string dateTo);

		int GetCount();
    }
}
