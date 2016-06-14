using BaiRong.Model;
using BaiRong.Model.Service;
using System.Collections;

namespace BaiRong.Core.Data.Provider
{
	public interface ITaskLogDAO
	{
        void Insert(TaskLogInfo log);

        void Delete(ArrayList idArrayList);

        void DeleteAll();

        string GetSelectCommend();

        string GetSelectCommend(ETriState successState, string keyword, string dateFrom, string dateTo);
	}
}
