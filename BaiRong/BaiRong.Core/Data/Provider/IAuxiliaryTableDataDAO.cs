using System;
using System.Data;
using System.Collections;

using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
	public interface IAuxiliaryTableDataDAO
	{
        ArrayList GetDefaultTableMetadataInfoArrayList(string tableName, EAuxiliaryTableType tableType);

        string GetCreateAuxiliaryTableSqlString(string tableName);
	}
}
