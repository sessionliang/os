using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;
using System.Data.OracleClient;

namespace BaiRong.Provider.Data.Oracle
{
	public class TableStructureDAO : BaiRong.Provider.Data.SqlServer.TableStructureDAO
	{
		protected override string ADOType
		{
			get
			{
				return SqlUtils.ORACLE;
			}
		}

		protected override EDatabaseType DataBaseType
		{
			get
			{
                return EDatabaseType.Oracle;
			}
		}


		public override ArrayList GetDatabaseNameArrayList()
		{
			ArrayList arraylist = new ArrayList();
			string databaseName = SqlUtils.GetDatabaseNameFormConnectionString(BaiRongDataProvider.ADOType, BaiRongDataProvider.ConnectionString);
			arraylist.Add(databaseName);
			return arraylist;
		}

        public override ArrayList GetDatabaseNameArrayList(string connectionString)
        {
            ArrayList arraylist = new ArrayList();
            string databaseName = SqlUtils.GetDatabaseNameFormConnectionString(BaiRongDataProvider.ADOType, connectionString);
            arraylist.Add(databaseName);
            return arraylist;
        }

		public override IDictionary GetTablesAndViewsDictionary(string databaseName)
		{
            string sqlString = "select table_name from user_tables";

			SortedList sortedlist = new SortedList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
			{
				while (rdr.Read())
				{
                    string tableName = rdr.GetValue(0).ToString();
                    sortedlist.Add(tableName, tableName);
				}
				rdr.Close();
			}
			return sortedlist;
		}

        public override IDictionary GetTablesAndViewsDictionary(string connectionString, string databaseName)
        {
            string sqlString = "select table_name from user_tables";

            SortedList sortedlist = new SortedList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    string tableName = rdr.GetValue(0).ToString();
                    sortedlist.Add(tableName, tableName);
                }
                rdr.Close();
            }
            return sortedlist;
        }

        public override bool IsTableExists(string tableName)
		{
			bool exists = false;

			string sqlString = string.Format("select * from user_tables where table_name = '{0}'", tableName.Trim().ToUpper());

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
		}


		public override string GetTableID(string collectionString, string databaseName, string tableName)
		{
            return tableName;
		}


        public override string GetTableName(string databaseName, string tableID)
		{
            return tableID;
		}


        public override string GetTableName(string connectionString, string databaseName, string tableID)
        {
            return tableID;
        }

        public override string GetTableOwner(string databaseName, string tableID)
		{
			return string.Empty;
		}

		public override string GetDefaultConstraintName(string tableName, string columnName)
		{
			return string.Empty;
		}

        public override ArrayList GetTableColumnInfoArrayList(string connectionString, string databaseName, string tableName)
        {
            ArrayList cacheArrayList = SqlUtils.Cache_GetTableColumnInfoArrayListCache(connectionString, databaseName, tableName);
            if (cacheArrayList != null && cacheArrayList.Count > 0)
            {
                return cacheArrayList;
            }

            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("select * from user_tab_columns where table_name = '{0}'", tableName.ToUpper());

            using (IDataReader rdr = this.ExecuteReader(connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    string columnName = rdr["COLUMN_NAME"].ToString();
                    EDataType dataType = EDataTypeUtils.FromOracle(rdr["DATA_TYPE"].ToString());
                    int length = TranslateUtils.ToInt(rdr["DATA_LENGTH"].ToString());
                    int precision = TranslateUtils.ToInt(rdr["DATA_PRECISION"].ToString());
                    int scale = TranslateUtils.ToInt(rdr["DATA_SCALE"].ToString());
                    bool isPrimaryKey = false;
                    if (StringUtils.EqualsIgnoreCase(columnName, "ID") && dataType == EDataType.Integer)
                    {
                        isPrimaryKey = true;
                    }
                    bool isNullable = rdr["NULLABLE"].ToString() == "Y";
                    bool isIdentity = isPrimaryKey;
                    string defaultValue = rdr["DATA_DEFAULT"].ToString();

                    TableColumnInfo info = new TableColumnInfo(databaseName, tableName, columnName, dataType, length, precision, scale, isPrimaryKey, isNullable, isIdentity, defaultValue, string.Empty, string.Empty, 0);
                    arraylist.Add(info);
                }
                rdr.Close();
            }

            SqlUtils.Cache_CacheTableColumnInfoArrayList(connectionString, databaseName, tableName, arraylist);

            return arraylist;
        }

        public override string GetInsertSqlString(NameValueCollection attributes, string connectionString, string tableName, out IDbDataParameter[] parms)
        {
            parms = null;
            string databaseName = SqlUtils.GetDatabaseNameFormConnectionString(this.ADOType, connectionString);
            string tableID = GetTableID(connectionString, databaseName, tableName);
            ArrayList allTableColumnInfoArrayList = GetTableColumnInfoArrayList(connectionString, databaseName, tableID);

            ArrayList paramArrayList = new ArrayList();

            ArrayList columnNameArrayList = new ArrayList();
            ArrayList columnValueArrayList = new ArrayList();
            foreach (TableColumnInfo tableColumnInfo in allTableColumnInfoArrayList)
            {
                if (!tableColumnInfo.IsIdentity)
                {
                    if (attributes[tableColumnInfo.ColumnName] == null)
                    {
                        if (!tableColumnInfo.IsNullable && string.IsNullOrEmpty(tableColumnInfo.DefaultValue))
                        {
                            columnNameArrayList.Add(tableColumnInfo.ColumnName);
                            string valueStr = string.Empty;
                            columnValueArrayList.Add(SqlUtils.Parse(this.DataBaseType, tableColumnInfo.DataType, valueStr, tableColumnInfo.Length));
                        }
                    }
                    else
                    {
                        if (tableColumnInfo.DataType == EDataType.NText || tableColumnInfo.DataType == EDataType.Text)
                        {
                            IDbDataParameter param = this.GetParameter("@" + tableColumnInfo.ColumnName, tableColumnInfo.DataType, attributes[tableColumnInfo.ColumnName]);
                            columnNameArrayList.Add(tableColumnInfo.ColumnName);
                            columnValueArrayList.Add("@" + tableColumnInfo.ColumnName);
                            paramArrayList.Add(param);
                        }
                        else if (tableColumnInfo.DataType == EDataType.NVarChar)
                        {
                            IDbDataParameter param = this.GetParameter("@" + tableColumnInfo.ColumnName, tableColumnInfo.DataType, tableColumnInfo.Length, attributes[tableColumnInfo.ColumnName]);
                            columnNameArrayList.Add(tableColumnInfo.ColumnName);
                            columnValueArrayList.Add("@" + tableColumnInfo.ColumnName);
                            paramArrayList.Add(param);
                        }
                        else
                        {
                            columnNameArrayList.Add(tableColumnInfo.ColumnName);
                            string valueStr = attributes[tableColumnInfo.ColumnName];
                            columnValueArrayList.Add(SqlUtils.Parse(this.DataBaseType, tableColumnInfo.DataType, valueStr, tableColumnInfo.Length));
                        }
                    }
                }
                else
                {
                    if (tableColumnInfo.DataType == EDataType.Integer)
                    {
                        columnNameArrayList.Add(tableColumnInfo.ColumnName);
                        columnValueArrayList.Add(tableName + "_SEQ.NEXTVAL");
                    }
                }
            }

            if (paramArrayList.Count > 0)
            {
                parms = new IDbDataParameter[paramArrayList.Count];
                for (int i = 0; i < paramArrayList.Count; i++)
                {
                    parms[i] = paramArrayList[i] as IDbDataParameter;
                }
            }

            return string.Format("INSERT INTO [{0}] ({1}) VALUES ({2})", tableName, TranslateUtils.ObjectCollectionToString(columnNameArrayList, " ,", "[", "]"), TranslateUtils.ObjectCollectionToString(columnValueArrayList, " ,"));
        }

        public override string GetUpdateSqlString(NameValueCollection attributes, string connectionString, string tableName, out IDbDataParameter[] parms)
        {
            parms = null;
            string databaseName = SqlUtils.GetDatabaseNameFormConnectionString(this.ADOType, connectionString);
            string tableID = GetTableID(connectionString, databaseName, tableName);
            ArrayList allTableColumnInfoArrayList = GetTableColumnInfoArrayList(connectionString, databaseName, tableID);

            ArrayList paramArrayList = new ArrayList();

            ArrayList setArrayList = new ArrayList();
            ArrayList whereArrayList = new ArrayList();
            foreach (TableColumnInfo tableColumnInfo in allTableColumnInfoArrayList)
            {
                if (attributes[tableColumnInfo.ColumnName] != null)
                {
                    if (!tableColumnInfo.IsPrimaryKey && tableColumnInfo.ColumnName != "ID")
                    {
                        if (tableColumnInfo.DataType == EDataType.NText || tableColumnInfo.DataType == EDataType.Text)
                        {
                            IDbDataParameter param = this.GetParameter("@" + tableColumnInfo.ColumnName, tableColumnInfo.DataType, attributes[tableColumnInfo.ColumnName]);
                            paramArrayList.Add(param);

                            setArrayList.Add(string.Format("[{0}] = @{0}", tableColumnInfo.ColumnName));
                        }
                        else if (tableColumnInfo.DataType == EDataType.NVarChar)
                        {
                            IDbDataParameter param = this.GetParameter("@" + tableColumnInfo.ColumnName, tableColumnInfo.DataType, tableColumnInfo.Length, attributes[tableColumnInfo.ColumnName]);
                            paramArrayList.Add(param);

                            setArrayList.Add(string.Format("[{0}] = @{0}", tableColumnInfo.ColumnName));
                        }
                        else
                        {
                            string valueStr = attributes[tableColumnInfo.ColumnName];
                            string sqlValue = SqlUtils.Parse(this.DataBaseType, tableColumnInfo.DataType, valueStr, tableColumnInfo.Length);
                            setArrayList.Add(string.Format("[{0}] = {1}", tableColumnInfo.ColumnName, sqlValue));
                        }
                    }
                    else
                    {
                        string valueStr = attributes[tableColumnInfo.ColumnName];
                        string sqlValue = SqlUtils.Parse(this.DataBaseType, tableColumnInfo.DataType, valueStr, tableColumnInfo.Length);
                        whereArrayList.Add(string.Format("[{0}] = {1}", tableColumnInfo.ColumnName, sqlValue));
                    }
                }
            }

            if (whereArrayList.Count == 0 && !string.IsNullOrEmpty(attributes["ID"]))
            {
                string valueStr = attributes["ID"];
                string sqlValue = SqlUtils.Parse(this.DataBaseType, EDataType.Integer, valueStr, 0);
                whereArrayList.Add(string.Format("[ID] = {0}", sqlValue));
            }

            if (whereArrayList.Count == 0)
            {
                throw new System.Data.MissingPrimaryKeyException();
            }
            if (setArrayList.Count == 0)
            {
                throw new System.Data.SyntaxErrorException();
            }

            if (paramArrayList.Count > 0)
            {
                parms = new IDbDataParameter[paramArrayList.Count];
                for (int i = 0; i < paramArrayList.Count; i++)
                {
                    parms[i] = paramArrayList[i] as IDbDataParameter;
                }
            }

            return string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, TranslateUtils.ObjectCollectionToString(setArrayList, " ,"), TranslateUtils.ObjectCollectionToString(whereArrayList, " AND "));
        }
	}
}
