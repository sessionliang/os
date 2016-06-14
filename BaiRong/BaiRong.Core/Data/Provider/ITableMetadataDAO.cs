using System;
using System.Data;
using System.Collections;

using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    /// <summary>
    /// Inteface for the TableMetadata Data Access Object
    /// </summary>
    public interface ITableMetadataDAO
    {

        void Insert(TableMetadataInfo info);

        void InsertSystemItems(string tableENName, EAuxiliaryTableType tableType, IDbTransaction trans);

        void Update(TableMetadataInfo info);

        void Delete(int tableMetadataID);
        void Delete(string tableENName, IDbTransaction trans);

        void SyncTable(string tableENName);

        TableMetadataInfo GetTableMetadataInfo(int tableMetadataID);

        IEnumerable GetDataSource(string tableENName);

        IEnumerable GetDataSorceMinusAttributes(string tableENName, ArrayList attributeNameArrayList);

        Hashtable GetTableENNameAndTableMetadataInfoArrayListHashtable();

        int GetTableMetadataCountByENName(string tableENName);

        int GetMaxTaxis(string TableENName);

        void TaxisUp(int TableMetadataID, string tableENName);

        void TaxisDown(int TableMetadataID, string tableENName);

        void CreateAuxiliaryTable(string tableENName);

        void CreateAuxiliaryTableOfArchive(string tableENName);

        void DeleteAuxiliaryTable(string tableENName);

        void ReCreateAuxiliaryTable(string tableENName, EAuxiliaryTableType tableType);

        int GetTableMetadataID(string tableENName, string attributeName);
    }
}
