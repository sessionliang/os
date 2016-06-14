using System;
using System.Data;
using System.Collections;

using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
	public interface ITableCollectionDAO
	{
		void Insert(AuxiliaryTableInfo info);

		void Update(AuxiliaryTableInfo info);

		void UpdateAttributeNum(string tableENName);

		void UpdateIsCreatedInDB(bool isCreatedInDB, string tableENName);

        void UpdateIsChangedAfterCreatedInDB(bool isChangedAfterCreatedInDB, string tableENName);

		void Delete(string tableENName);

		AuxiliaryTableInfo GetAuxiliaryTableInfo(string tableENName);

        EAuxiliaryTableType GetTableType(string tableENName);

        string GetTableCNName(string tableENName);

		IEnumerable GetDataSourceByAuxiliaryTableType();

		//IEnumerable GetDataSourceCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType type);

        ArrayList GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(params EAuxiliaryTableType[] eAuxiliaryTableTypeArray);

        ArrayList GetAuxiliaryTableArrayListCreatedInDB();

		ArrayList GetTableENNameCollection();

        ArrayList GetTableENNameCollectionCreatedInDB();

		int GetTableUsedNum(string tableENName, EAuxiliaryTableType tableType);

        bool IsTableExists(EAuxiliaryTableType tableType);

        bool IsTableExists(string tableName);

        string GetFirstTableNameByTableType(EAuxiliaryTableType tableType);

        void CreateAllAuxiliaryTableIfNotExists();
	}
}
