using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Model;
using System.Text;
using System.Collections.Generic;

namespace BaiRong.Core.Data.Provider
{
	public interface IDatabaseDAO
	{
        string GetString(string connectionString, string sqlString);

        string GetString(string sqlString);

        DateTime GetDateTime(string sqlString);

        DateTime GetDateTime(string sqlString, IDataParameter[] parms);

        DataSet GetDataSet(string connectionString, string sql);

		DataSet GetDataSet(string sql);

		DataSet GetDataSetByWhereString(string tableENName, string whereString);

		IEnumerable GetDataSource(string connectionString, string sql);

        IEnumerable GetDataSource(string sql);

        IDataReader GetDataReader(string connectionString, string sqlString);

        ArrayList GetIntArrayList(string sqlString);

        ArrayList GetIntArrayList(string connectionString, string sqlString);

        List<int> GetIntList(string sqlString);

        List<int> GetIntList(string connectionString, string sqlString);

        ArrayList GetStringArrayList(string sqlString);

        List<string> GetStringList(string sqlString);

        ArrayList GetStringArrayList(string connectionString, string sqlString);

        void ExecuteSql(string sqlString);

        void ExecuteSql(string connectionString, string sqlString);

		void ExecuteSql(ArrayList sqlArrayList);

        void ExecuteSql(string connectionString, ArrayList sqlArrayList);

        void ExecuteSqlInFile(string pathToScriptFile, StringBuilder errorBuilder);

        void ExecuteSqlInFile(string pathToScriptFile, string tableName, StringBuilder errorBuilder);

		void DeleteDBLog();

        int GetIntResult(string connectionString, string sqlString);

		int GetIntResult(string sqlString);

        int GetIntResult(string sqlString, IDataParameter[] parms);

        int GetSequence(IDbTransaction trans, string tableName);

        void ReadResultsToExtendedAttributes(IDataRecord rdr, ExtendedAttributes attributes);

        void ReadResultsToNameValueCollection(IDataRecord rdr, NameValueCollection attributes);

        int GetPageTotalCount(string sqlString);

        string GetPageSqlString(string sqlString, string orderByString, int recordCount, int itemsPerPage, int currentPageIndex);

        #region 辅助表数据操作

        int DataInsert(NameValueCollection attributes, string tableName);

        void DataUpdate(NameValueCollection attributes, string tableName);

        void DataDelete(string tableName, NameValueCollection whereMap);

        bool DataIsExists(string tableName, NameValueCollection whereMap);

        NameValueCollection DataGetAttributes(string tableName, NameValueCollection whereMap);

        #endregion

        void ClearDatabase(string connectionString);
	}
}
