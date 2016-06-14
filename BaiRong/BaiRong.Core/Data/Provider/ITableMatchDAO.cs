using System;
using System.Data;
using System.Collections;

using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
	public interface ITableMatchDAO
	{
		int Insert(TableMatchInfo tableMatchInfo);
		void Update(TableMatchInfo tableMatchInfo);
		void Delete(int tableMatchID);
		TableMatchInfo GetTableMatchInfo(int tableMatchID);
	}
}
