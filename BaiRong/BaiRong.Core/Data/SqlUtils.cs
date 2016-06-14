using System;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
using StringReader = System.IO.StringReader;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data.Helper;
using System.IO;

namespace BaiRong.Core.Data
{
    public sealed class SqlUtils
    {
        private SqlUtils() { }

        public const string SQL_SERVER = "SqlServer";
        public const string OLE_DB = "OleDb";
        public const string ODBC = "Odbc";
        public const string ORACLE = "Oracle";

        public const string Asterisk = "*";

        public static AdoHelper GetAdoHelper(string adoType)
        {
            string assembly = null;
            string type = null;

            switch (adoType)
            {
                case SQL_SERVER:
                    assembly = "BaiRong.Provider";
                    type = "BaiRong.Provider.Data.Helper.SqlServer";
                    break;
                case OLE_DB:
                    assembly = "BaiRong.Provider";
                    type = "BaiRong.Provider.Data.Helper.OleDb";
                    break;
                case ODBC:
                    assembly = "BaiRong.Provider";
                    type = "BaiRong.Provider.Data.Helper.Odbc";
                    break;
                case ORACLE:
                    assembly = "BaiRong.Provider";
                    type = "BaiRong.Provider.Data.Helper.Oracle";
                    break;
            }

            return AdoHelper.CreateHelper(assembly, type);
        }

        public static IDbConnection GetIDbConnection(string adoType)
        {
            return GetIDbConnection(adoType, BaiRongDataProvider.ConnectionString);
        }

        public static IDbConnection GetIDbConnection(string adoType, string connectionString)
        {
            IDbConnection conn = null;
            switch (adoType)
            {
                case SQL_SERVER:
                    conn = new SqlConnection(connectionString);
                    break;
                case OLE_DB:
                    conn = new OleDbConnection(connectionString);
                    break;
                case ODBC:
                    conn = new OdbcConnection(connectionString);
                    break;
                case ORACLE:
                    conn = new OracleConnection(connectionString);
                    break;
            }

            return conn;
        }

        public static IDbCommand GetIDbCommand(string adoType)
        {
            IDbCommand command = null;
            switch (adoType)
            {
                case SQL_SERVER:
                    command = new SqlCommand();
                    break;
                case OLE_DB:
                    command = new OleDbCommand();
                    break;
                case ODBC:
                    command = new OdbcCommand();
                    break;
                case ORACLE:
                    command = new OracleCommand();
                    break;
            }

            return command;
        }

        public static IDbDataAdapter GetIDbDataAdapter(string adoType, string text, string connectionString)
        {
            IDbDataAdapter adapter = null;
            switch (adoType)
            {
                case SQL_SERVER:
                    adapter = new SqlDataAdapter(text, connectionString);
                    break;
                case OLE_DB:
                    adapter = new OleDbDataAdapter(text, connectionString);
                    break;
                case ODBC:
                    adapter = new OdbcDataAdapter(text, connectionString);
                    break;
                case ORACLE:
                    adapter = new OracleDataAdapter(text, connectionString);
                    break;
            }

            return adapter;
        }

        public static IDbDataAdapter GetIDbDataAdapter(string adoType)
        {
            IDbDataAdapter adapter = null;
            switch (adoType)
            {
                case SQL_SERVER:
                    adapter = new SqlDataAdapter();
                    break;
                case OLE_DB:
                    adapter = new OleDbDataAdapter();
                    break;
                case ODBC:
                    adapter = new OdbcDataAdapter();
                    break;
                case ORACLE:
                    adapter = new OracleDataAdapter();
                    break;
            }

            return adapter;
        }

        public static void FillDataAdapterWithDataTable(string adoType, IDbDataAdapter adapter, DataTable table)
        {
            switch (adoType)
            {
                case SQL_SERVER:
                    ((SqlDataAdapter)adapter).Fill(table);
                    break;
                case OLE_DB:
                    ((OleDbDataAdapter)adapter).Fill(table);
                    break;
                case ODBC:
                    ((OdbcDataAdapter)adapter).Fill(table);
                    break;
                case ORACLE:
                    ((OracleDataAdapter)adapter).Fill(table);
                    break;
            }
        }

        public static IDbDataParameter GetIDbDataParameter(string adoType, string parameterName, EDataType dataType, int size)
        {
            IDbDataParameter parameter = null;
            switch (adoType)
            {
                case SQL_SERVER:
                    parameter = new SqlParameter(parameterName, EDataTypeUtils.ToSqlDbType(dataType), size);
                    break;
                case OLE_DB:
                    parameter = new OleDbParameter(parameterName, EDataTypeUtils.ToOleDbType(dataType), size);
                    break;
                case ORACLE:
                    parameter = new OracleParameter(parameterName, EDataTypeUtils.ToOracleDbType(dataType), size);
                    break;
                case ODBC:
                    parameter = new SqlParameter();
                    break;
            }

            return parameter;
        }

        public static IDbDataParameter GetIDbDataParameter(string adoType, string parameterName, EDataType dataType)
        {
            IDbDataParameter parameter = null;
            switch (adoType)
            {
                case SQL_SERVER:
                    parameter = new SqlParameter(parameterName, EDataTypeUtils.ToSqlDbType(dataType));
                    break;
                case OLE_DB:
                    parameter = new OleDbParameter(parameterName, EDataTypeUtils.ToOleDbType(dataType));
                    break;
                case ORACLE:
                    parameter = new OracleParameter(parameterName, EDataTypeUtils.ToOracleDbType(dataType));
                    break;
                case ODBC:
                    parameter = new SqlParameter();
                    break;
            }

            return parameter;
        }

        public static string ParseSqlString(string sqlString)
        {
            if (BaiRongDataProvider.DatabaseType == EDatabaseType.Oracle)
            {
                if (sqlString.IndexOf(" WHERE ") != -1)
                {
                    int index = sqlString.IndexOf(" WHERE ");
                    string first = sqlString.Substring(0, index);
                    string second = sqlString.Substring(index);
                    sqlString = first.Replace("[", string.Empty).Replace("]", string.Empty) + second.Replace("[", string.Empty).Replace("]", string.Empty).Replace("<> ''", "IS NOT NULL").Replace("= ''", "IS NULL");
                }
                else
                {
                    sqlString = sqlString.Replace("[", string.Empty).Replace("]", string.Empty);
                }

                if (sqlString.StartsWith("SELECT TOP 1 "))
                {
                    sqlString = string.Format(@"SELECT * FROM ( {0} ) WHERE ROWNUM <= 1", StringUtils.ReplaceStartsWith(sqlString, "SELECT TOP 1 ", "SELECT "));
                }

                sqlString = sqlString.Replace("@/", "_at_/");
                sqlString = sqlString.Replace("@", ":");
                sqlString = sqlString.Replace("_at_/", "@/");
            }

            return sqlString;
        }

        public static string GetFullTableName(string adoType, string databaseName, string tableOwner, string tableName)
        {
            string retval = tableName;
            switch (adoType)
            {
                case SQL_SERVER:
                    retval = string.Format("[{0}].[{1}].[{2}]", databaseName, tableOwner, tableName);
                    break;
                case OLE_DB:
                    retval = string.Format("[{0}]", tableName);
                    break;
                case ODBC:
                    retval = string.Format("[{0}].[{1}].[{2}]", databaseName, tableOwner, tableName);
                    break;
                case ORACLE:
                    retval = string.Format("[{0}].[{1}].[{2}]", databaseName, tableOwner, tableName);
                    break;
            }

            return retval;
        }

        public static string GetDatabaseNameFormConnectionString(string adoType, string connectionString)
        {
            string attribute = null;

            switch (adoType)
            {
                case SQL_SERVER:
                    attribute = "database";
                    break;
                case OLE_DB:
                    attribute = "data source";
                    break;
                case ORACLE:
                    attribute = "data source";
                    break;
                case ODBC:
                    attribute = "database";
                    break;
            }

            string databaseName = GetValueFromConnectionString(connectionString, attribute);
            if (string.IsNullOrEmpty(databaseName) && adoType == SQL_SERVER)
            {
                databaseName = GetValueFromConnectionString(connectionString, "Initial Catalog");
            }

            return databaseName;
        }


        public static string GetValueFromConnectionString(string connectionString, string attribute)
        {
            //server=(local);uid=sa;pwd=bairong;Trusted_Connection=no;database=V1
            //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=/SiteFiles/Data/ASPNetDB.mdb;User Id=;Password=;
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(attribute))
            {
                string[] pairs = connectionString.Split(';');
                foreach (string pair in pairs)
                {
                    if (pair.IndexOf("=") != -1)
                    {
                        if (StringUtils.EqualsIgnoreCase(attribute, pair.Trim().Split('=')[0]))
                        {
                            retval = pair.Trim().Split('=')[1];
                            break;
                        }
                    }
                }
            }
            return retval;
        }

        public static string ReadNextStatementFromStream(StringReader _reader)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string lineOfText;

                while (true)
                {
                    lineOfText = _reader.ReadLine();
                    if (lineOfText == null)
                    {

                        if (sb.Length > 0)
                        {
                            return sb.ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }

                    if (lineOfText.TrimEnd().ToUpper() == "GO")
                    {
                        break;
                    }

                    sb.Append(lineOfText + Environment.NewLine);
                }

                return sb.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static string GetColumnSqlString(string adoType, EDataType dataType, string attributeName, int length, bool canBeNull, string defaultValue)
        {
            switch (adoType)
            {
                case SQL_SERVER:
                    return GetSqlServerColumnSqlString(dataType, attributeName, length, canBeNull, defaultValue);
                case ORACLE:
                    return GetOracleColumnSqlString(dataType, attributeName, length, canBeNull, defaultValue);
                case ODBC:
                    return GetSqlServerColumnSqlString(dataType, attributeName, length, canBeNull, defaultValue);
            }

            return null;
        }

        public static string GetSqlServerColumnSqlString(EDataType dataType, string attributeName, int length, bool canBeNull, string defaultValue)
        {
            string retval = string.Empty;
            SqlDbType sqlDbType = EDataTypeUtils.ToSqlDbType(dataType);
            switch (sqlDbType)
            {
                case SqlDbType.BigInt:
                    retval = string.Format("[{0}] [bigint]", attributeName);
                    break;
                case SqlDbType.Binary:
                    retval = string.Format("[{0}] [binary] ({1})", attributeName, length);
                    break;
                case SqlDbType.Bit:
                    retval = string.Format("[{0}] [bit]", attributeName);
                    break;
                case SqlDbType.Char:
                    retval = string.Format("[{0}] [char] ({1})", attributeName, length);
                    break;
                case SqlDbType.DateTime:
                    retval = string.Format("[{0}] [datetime]", attributeName);
                    break;
                case SqlDbType.Decimal:
                    retval = string.Format("[{0}] [decimal] (18, 2)", attributeName);
                    break;
                case SqlDbType.Float:
                    retval = string.Format("[{0}] [float]", attributeName);
                    break;
                case SqlDbType.Image:
                    retval = string.Format("[{0}] [image]", attributeName);
                    break;
                case SqlDbType.Int:
                    retval = string.Format("[{0}] [int]", attributeName);
                    break;
                case SqlDbType.Money:
                    retval = string.Format("[{0}] [money]", attributeName);
                    break;
                case SqlDbType.NChar:
                    retval = string.Format("[{0}] [nchar] ({1})", attributeName, length);
                    break;
                case SqlDbType.NText:
                    retval = string.Format("[{0}] [ntext]", attributeName);
                    break;
                case SqlDbType.NVarChar:
                    retval = string.Format("[{0}] [nvarchar] ({1})", attributeName, length);
                    break;
                case SqlDbType.Real:
                    retval = string.Format("[{0}] [real]", attributeName);
                    break;
                case SqlDbType.SmallDateTime:
                    retval = string.Format("[{0}] [smalldatetime]", attributeName);
                    break;
                case SqlDbType.SmallInt:
                    retval = string.Format("[{0}] [smallint]", attributeName);
                    break;
                case SqlDbType.SmallMoney:
                    retval = string.Format("[{0}] [smallmoney]", attributeName);
                    break;
                case SqlDbType.Text:
                    retval = string.Format("[{0}] [text]", attributeName);
                    break;
                case SqlDbType.Timestamp:
                    retval = string.Format("[{0}] [timestamp]", attributeName);
                    break;
                case SqlDbType.TinyInt:
                    retval = string.Format("[{0}] [tinyint]", attributeName);
                    break;
                case SqlDbType.VarBinary:
                    retval = string.Format("[{0}] [varbinary] ({1})", attributeName, length);
                    break;
                case SqlDbType.VarChar:
                    retval = string.Format("[{0}] [varchar] ({1})", attributeName, length);
                    break;
                default:
                    break;
            }
            if (canBeNull == false)
            {
                retval += " NOT NULL";
                if (string.IsNullOrEmpty(defaultValue))
                {
                    defaultValue = EDataTypeUtils.GetDefaultString(EDatabaseType.SqlServer, dataType);
                }
                retval += string.Format(" DEFAULT ({0})", defaultValue);
            }
            else
            {
                retval += " NULL";
            }
            return retval;
        }

        public static string GetOracleColumnSqlString(EDataType dataType, string attributeName, int length, bool canBeNull, string defaultValue)
        {
            string retval = string.Empty;
            OracleType oracleType = EDataTypeUtils.ToOracleDbType(dataType);
            switch (oracleType)
            {
                case OracleType.Char:
                    retval = string.Format("{0} CHAR({1})", attributeName, length);
                    break;
                case OracleType.Clob:
                    retval = string.Format("{0} CLOB", attributeName);
                    break;
                case OracleType.DateTime:
                    retval = string.Format("{0} DATE", attributeName);
                    break;
                case OracleType.Double:
                    retval = string.Format("{0} DOUBLE PRECISION", attributeName);
                    break;
                case OracleType.Float:
                    retval = string.Format("{0} FLOAT({0})", attributeName, length);
                    break;
                case OracleType.Int16:
                    retval = string.Format("{0} NUMBER(38, 0)", attributeName);
                    break;
                case OracleType.Int32:
                    retval = string.Format("{0} NUMBER(38, 0)", attributeName);
                    break;
                case OracleType.NChar:
                    retval = string.Format("{0} NCHAR({1})", attributeName, length);
                    break;
                case OracleType.NClob:
                    retval = string.Format("{0} NCLOB", attributeName);
                    break;
                case OracleType.Number:
                    retval = string.Format("{0} NUMBER({1})", attributeName, length);
                    break;
                case OracleType.NVarChar:
                    retval = string.Format("{0} NVARCHAR2({1})", attributeName, length);
                    break;
                case OracleType.Timestamp:
                    retval = string.Format("{0} TIMESTAMP({1})", attributeName, length);
                    break;
                case OracleType.VarChar:
                    retval = string.Format("{0} VARCHAR2({1})", attributeName, length);
                    break;
                default:
                    break;
            }
            bool isNotNull = false;
            if (canBeNull == false && oracleType != OracleType.NChar && oracleType != OracleType.Char && oracleType != OracleType.VarChar && oracleType != OracleType.NVarChar && oracleType != OracleType.Clob && oracleType != OracleType.NClob)
            {
                isNotNull = true;
                if (string.IsNullOrEmpty(defaultValue))
                {
                    defaultValue = EDataTypeUtils.GetDefaultString(EDatabaseType.Oracle, dataType);
                }
            }

            if (!string.IsNullOrEmpty(defaultValue))
            {
                retval += string.Format(" DEFAULT {0}", defaultValue);
            }

            if (isNotNull)
            {
                retval += " NOT NULL";
            }

            return retval;
        }

        public static string GetDefaultDateString(EDatabaseType databaseType)
        {
            return EDataTypeUtils.GetDefaultString(databaseType, EDataType.DateTime);
        }

        public static string Parse(EDatabaseType databaseType, EDataType dataType, string valueStr, int length)
        {
            string retval;

            switch (dataType)
            {
                case EDataType.Bit:
                    retval = SqlUtils.ParseToIntString(valueStr);
                    break;
                case EDataType.Char:
                    retval = SqlUtils.ParseToSqlStringWithQuote(valueStr, length);
                    break;
                case EDataType.DateTime:
                    retval = SqlUtils.ParseToDateTimeString(databaseType, valueStr);
                    break;
                case EDataType.Decimal:
                    retval = SqlUtils.ParseToDoubleString(valueStr);
                    break;
                case EDataType.Float:
                    retval = SqlUtils.ParseToDoubleString(valueStr);
                    break;
                case EDataType.Integer:
                    retval = SqlUtils.ParseToIntString(valueStr);
                    break;
                case EDataType.NChar:
                    retval = SqlUtils.ParseToSqlStringWithNAndQuote(databaseType, valueStr, length);
                    break;
                case EDataType.NText:
                    retval = SqlUtils.ParseToSqlStringWithNAndQuote(databaseType, valueStr);
                    break;
                case EDataType.NVarChar:
                    retval = SqlUtils.ParseToSqlStringWithNAndQuote(databaseType, valueStr, length);
                    break;
                case EDataType.Text:
                    retval = SqlUtils.ParseToSqlStringWithQuote(valueStr);
                    break;
                case EDataType.VarChar:
                    retval = SqlUtils.ParseToSqlStringWithQuote(valueStr, length);
                    break;
                default:
                    retval = "NULL";
                    break;
            }
            return retval;
        }

        public static string ParseToIntString(string str)
        {
            int ValueInt = 0;
            try
            {
                ValueInt = int.Parse(str);
            }
            catch { }
            return ValueInt.ToString();
        }


        public static string ParseToDoubleString(string str)
        {
            double ValueDouble = 0;
            try
            {
                ValueDouble = double.Parse(str);
            }
            catch { }
            return ValueDouble.ToString();
        }


        public static string ParseToSqlStringWithQuote(string str)
        {
            return ParseToSqlStringWithQuote(str, 0);
        }


        public static string ParseToSqlStringWithQuote(string str, int length)
        {
            str = ToSqlString(str);
            if (string.IsNullOrEmpty(str))
            {
                return "''";
            }
            if (length == 0)
            {
                return "'" + str + "'";
            }
            else
            {
                length = Math.Min(str.Length, length);
                return "'" + str.Substring(0, length) + "'";
            }
        }

        public static string ParseToSqlStringWithNAndQuote(EDatabaseType databaseType, string str)
        {
            return ParseToSqlStringWithNAndQuote(databaseType, str, 0);
        }


        public static string ParseToSqlStringWithNAndQuote(EDatabaseType databaseType, string str, int length)
        {
            string retval = string.Empty;
            str = ToSqlString(str);
            if (string.IsNullOrEmpty(str))
            {
                retval = "''";
            }
            if (length == 0)
            {
                retval = "'" + str + "'";
            }
            else
            {
                length = Math.Min(str.Length, length);
                retval = "'" + str.Substring(0, length) + "'";
            }
            if (databaseType == EDatabaseType.SqlServer)
            {
                retval = "N" + retval;
            }
            return retval;
        }


        public static string ParseToDateTimeString(EDatabaseType databaseType, string datetimeStr)
        {
            if (databaseType == EDatabaseType.Oracle)
            {
                DateTime datetime = TranslateUtils.ToDateTime(datetimeStr, DateUtils.SqlMinValue);
                return ParseToOracleDateTime(datetime);
            }
            else
            {
                DateTime dateTime = TranslateUtils.ToDateTime(datetimeStr, DateUtils.SqlMinValue);
                if (dateTime == DateTime.MinValue)
                {
                    dateTime = DateUtils.SqlMinValue;
                }
                return "'" + dateTime + "'";
            }
        }

        public static string ParseToOracleDateTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                dateTime = DateUtils.SqlMinValue;
            }
            return string.Format("TO_DATE('{0}', 'yyyy-mm-dd hh24:mi:ss')", dateTime.ToString("yyyy-MM-dd hh:mm:ss"));
        }


        public static int GetMaxLengthForNVarChar(EDatabaseType databaseType)
        {
            int maxValue = 4000;
            if (databaseType == EDatabaseType.SqlServer)
            {
                maxValue = 4000;
            }
            return maxValue;
        }

        public static string ToSqlString(string inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                return inputString.Replace("'", "''");
            }
            return string.Empty;
        }


        public static string ToSqlString(string inputString, int maxLength)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                if (maxLength > 0 && inputString.Length > maxLength)
                {
                    inputString = inputString.Substring(0, maxLength);
                }
                return inputString.Replace("'", "''");
            }
            return string.Empty;
        }

        /// <summary>
        /// 验证此字符串是否合作作为字段名称
        /// </summary>
        public static bool IsAttributeNameCompliant(string attributeName)
        {
            if (string.IsNullOrEmpty(attributeName) || attributeName.IndexOf(" ") != -1) return false;
            if (-1 != attributeName.IndexOfAny(PathUtils.InvalidPathChars))
            {
                return false;
            }
            for (int i = 0; i < attributeName.Length; i++)
            {
                if (StringUtils.IsTwoBytesChar(attributeName[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static string ReadNextSqlString(StreamReader _reader)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string lineOfText;

                while (true)
                {
                    lineOfText = _reader.ReadLine();
                    if (lineOfText == null)
                    {

                        if (sb.Length > 0)
                        {
                            return sb.ToString();
                        }
                        else
                        {
                            return null;
                        }
                    }

                    if (lineOfText.TrimEnd().ToUpper() == "GO")
                    {
                        break;
                    }

                    sb.Append(lineOfText + Environment.NewLine);
                }

                return sb.ToString();
            }
            catch
            {
                return null;
            }
        }

        #region 数据库结构缓存

        private static string Cache_GetTableStructureCacheString(string connectionString, string databaseName, string tableID)
        {
            return string.Format("BaiRong.Core.Data.SqlUtils.GetTableStructureCacheString.{0}.{1}.{2}", connectionString, databaseName, tableID);
        }

        public static void Cache_CacheTableColumnInfoArrayList(string connectionString, string databaseName, string tableID, ArrayList tableColumnInfoArrayList)
        {
            string cacheKey = Cache_GetTableStructureCacheString(connectionString, databaseName, tableID);
            CacheUtils.Max(cacheKey, tableColumnInfoArrayList);
        }

        public static ArrayList Cache_GetTableColumnInfoArrayListCache(string connectionString, string databaseName, string tableID)
        {
            string cacheKey = Cache_GetTableStructureCacheString(connectionString, databaseName, tableID);
            return CacheUtils.Get(cacheKey) as ArrayList;
        }

        private static string Cache_GetTableIDCacheString(string databaseName, string tableName)
        {
            return string.Format("BaiRong.Core.Data.SqlUtils.GetTableStructureCacheString.tableID_{0}_{1}", databaseName, tableName);
        }

        public static void Cache_CacheTableID(string databaseName, string tableName, string tableID)
        {
            string cacheKey = Cache_GetTableIDCacheString(databaseName, tableName);
            CacheUtils.Max(cacheKey, tableID);
        }

        public static string Cache_GetTableIDCache(string databaseName, string tableName)
        {
            string cacheKey = Cache_GetTableIDCacheString(databaseName, tableName);
            return CacheUtils.Get(cacheKey) as string;
        }

        public static void Cache_RemoveTableColumnInfoArrayListCache()
        {
            CacheUtils.RemoveByStartString("BaiRong.Core.Data.SqlUtils.GetTableStructureCacheString");
        }

        #endregion
    }
}
