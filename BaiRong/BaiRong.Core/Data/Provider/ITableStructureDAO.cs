using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface ITableStructureDAO
    {
        ArrayList GetDatabaseNameArrayList();

        ArrayList GetDatabaseNameArrayList(string connectionString);

        IDictionary GetTablesAndViewsDictionary(string databaseName);

        IDictionary GetTablesAndViewsDictionary(string connectionString, string databaseName);

        ArrayList GetColumnNameArrayList(string tableName);

        ArrayList GetColumnNameArrayList(string tableName, bool isLower);

        ArrayList GetColumnNameArrayList(string connectionString, string tableName);

        ArrayList GetTableColumnInfoArrayList(string databaseName, string tableID);

        ArrayList GetTableColumnInfoArrayList(string connectionString, string databaseName, string tableID);

        DataSet GetTableColumnsDataSet(string databaseName, string tableID);

        bool IsColumnEquals(TableMetadataInfo metadataInfo, TableColumnInfo columnInfo);

        bool IsTableExists(string tableName);

        string GetTableID(string connectionString, string databaseName, string tableName);

        string GetTableName(string databaseName, string tableID);

        string GetTableName(string connectionString, string databaseName, string tableID);

        string GetTableOwner(string databaseName, string tableID);

        string GetDefaultConstraintName(string tableName, string columnName);

        string GetInsertSqlString(NameValueCollection attributes, string tableName, out IDbDataParameter[] parms);

        string GetInsertSqlString(NameValueCollection attributes, string connectionString, string tableName, out IDbDataParameter[] parms);

        string GetUpdateSqlString(NameValueCollection attributes, string tableName, out IDbDataParameter[] parms);

        string GetUpdateSqlString(NameValueCollection attributes, string connectionString, string tableName, out IDbDataParameter[] parms);

        string GetSelectSqlString(string tableName, string columns, string whereString);

        string GetSelectSqlString(string tableName, string columns, string whereString, string orderByString);

        string GetSelectSqlString(string tableName, int contentNum, string columns, string whereString, string orderByString);

        string GetSelectSqlString(string tableName, int startNum, int totalNum, string columns, string whereString, string orderByString);

        string GetSelectSqlString(string connectionString, string tableName, int totalNum, string columns, string whereString, string orderByString);

        string GetSelectSqlString(string connectionString, string tableName, int totalNum, string columns, string whereString, string orderByString, string joinString);

        string GetSelectSqlString(string connectionString, string tableName, int startNum, int totalNum, string columns, string whereString, string orderByString);

        string GetSelectSqlStringByQueryString(string connectionString, string queryString, int totalNum, string orderByString);

        string GetSelectSqlStringByQueryString(string connectionString, string queryString, int startNum, int totalNum, string orderByString);
    }
}
