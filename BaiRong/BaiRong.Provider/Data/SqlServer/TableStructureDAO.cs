using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;
using System.Collections.Generic;

namespace BaiRong.Provider.Data.SqlServer
{
    public class TableStructureDAO : DataProviderBase, ITableStructureDAO
    {
        public virtual ArrayList GetDatabaseNameArrayList()
        {
            string sqlString = "select name from master.dbo.sysdatabases Order by name";

            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return arraylist;
        }

        public virtual ArrayList GetDatabaseNameArrayList(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            string sqlString = "select name from master.dbo.sysdatabases Order by name";

            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return arraylist;
        }

        public virtual IDictionary GetTablesAndViewsDictionary(string databaseName)
        {
            string sqlString = string.Format("select name, id from [{0}].dbo.sysobjects where type in ('U','V') and category<>2 Order By Name", databaseName);

            SortedList sortedlist = new SortedList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    sortedlist.Add(rdr.GetValue(0).ToString(), rdr.GetInt32(1));
                }
                rdr.Close();
            }
            return sortedlist;
        }

        public virtual IDictionary GetTablesAndViewsDictionary(string connectionString, string databaseName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            string sqlString = string.Format("select name, id from [{0}].dbo.sysobjects where type in ('U','V') and category<>2 Order By Name", databaseName);

            SortedList sortedlist = new SortedList();

            using (IDataReader rdr = this.ExecuteReader(connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    sortedlist.Add(rdr.GetValue(0).ToString(), rdr.GetInt32(1));
                }
                rdr.Close();
            }
            return sortedlist;
        }

        public virtual bool IsTableExists(string tableName)
        {
            bool exists = false;
            string sqlString = string.Format("select * from dbo.sysobjects where id = object_id(N'[{0}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1", tableName);

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

        public virtual string GetTableID(string connectionString, string databaseName, string tableName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            string tableID = SqlUtils.Cache_GetTableIDCache(databaseName, tableName);

            if (string.IsNullOrEmpty(tableID))
            {
                string sqlString = string.Format("select id from [{0}].dbo.sysobjects where type in ('U','V') and category<>2 and name='{1}'", databaseName, tableName);

                using (IDataReader rdr = this.ExecuteReader(connectionString, sqlString))
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            tableID = rdr.GetValue(0).ToString();
                            SqlUtils.Cache_CacheTableID(databaseName, tableName, tableID);
                        }
                    }
                    rdr.Close();
                }
            }

            return tableID;
        }

        public virtual string GetTableName(string databaseName, string tableID)
        {
            string tableName = string.Empty;
            string CMD = string.Format("select O.name from [{0}].dbo.sysobjects O, [{0}].dbo.sysusers U where O.id={1} and U.uid=O.uid", databaseName, tableID);

            using (IDataReader rdr = this.ExecuteReader(CMD))
            {
                if (rdr.Read())
                {
                    tableName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return tableName;
        }

        public virtual string GetTableName(string connectionString, string databaseName, string tableID)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            string tableName = string.Empty;
            string sqlString = string.Format("select O.name from [{0}].dbo.sysobjects O, [{0}].dbo.sysusers U where O.id={1} and U.uid=O.uid", databaseName, tableID);

            using (IDataReader rdr = this.ExecuteReader(connectionString, sqlString))
            {
                if (rdr.Read())
                {
                    tableName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return tableName;
        }

        public virtual string GetTableOwner(string databaseName, string tableID)
        {
            string tableOwner = string.Empty;
            string CMD = string.Format("select U.name from [{0}].dbo.sysobjects O, [{0}].dbo.sysusers U where O.id={1} and U.uid=O.uid", databaseName, tableID);

            using (IDataReader rdr = this.ExecuteReader(CMD))
            {
                if (rdr.Read())
                {
                    tableOwner = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return tableOwner;
        }

        public virtual string GetDefaultConstraintName(string tableName, string columnName)
        {
            string defaultConstraintName = string.Empty;
            string sqlString = string.Format("select b.name from syscolumns a,sysobjects b where a.id=object_id('{0}') and b.id=a.cdefault and a.name='{1}' and b.name like 'DF%'", tableName, columnName);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    defaultConstraintName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return defaultConstraintName;
        }

        public ArrayList GetColumnNameArrayList(string tableName)
        {
            return GetColumnNameArrayList(tableName, false);
        }

        public ArrayList GetColumnNameArrayList(string tableName, bool isLower)
        {
            string databaseName = SqlUtils.GetDatabaseNameFormConnectionString(this.ADOType, BaiRongDataProvider.ConnectionString);
            string tableID = GetTableID(BaiRongDataProvider.ConnectionString, databaseName, tableName);
            ArrayList allTableColumnInfoArrayList = GetTableColumnInfoArrayList(databaseName, tableID);

            ArrayList columnNameArrayList = new ArrayList();

            foreach (TableColumnInfo tableColumnInfo in allTableColumnInfoArrayList)
            {
                if (isLower)
                {
                    columnNameArrayList.Add(tableColumnInfo.ColumnName.ToLower());
                }
                else
                {
                    columnNameArrayList.Add(tableColumnInfo.ColumnName);
                }
            }

            return columnNameArrayList;
        }

        public ArrayList GetColumnNameArrayList(string connectionString, string tableName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            string databaseName = SqlUtils.GetDatabaseNameFormConnectionString(this.ADOType, connectionString);
            string tableID = GetTableID(connectionString, databaseName, tableName);
            ArrayList allTableColumnInfoArrayList = GetTableColumnInfoArrayList(connectionString, databaseName, tableID);

            ArrayList columnNameArrayList = new ArrayList();

            foreach (TableColumnInfo tableColumnInfo in allTableColumnInfoArrayList)
            {
                columnNameArrayList.Add(tableColumnInfo.ColumnName);
            }

            return columnNameArrayList;
        }

        public ArrayList GetTableColumnInfoArrayList(string databaseName, string tableID)
        {
            return GetTableColumnInfoArrayList(BaiRongDataProvider.ConnectionString, databaseName, tableID);
        }

        public virtual ArrayList GetTableColumnInfoArrayList(string connectionString, string databaseName, string tableID)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            ArrayList cacheArrayList = SqlUtils.Cache_GetTableColumnInfoArrayListCache(connectionString, databaseName, tableID);
            if (cacheArrayList != null && cacheArrayList.Count > 0)
            {
                return cacheArrayList;
            }

            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("select C.name, T.name, C.length, C.xprec, C.xscale, C.colstat, C.isnullable, case when C.autoval is null then 0 else 1 end, SC.text, (select CForgin.name from [{0}].dbo.sysreferences Sr,[{0}].dbo.sysobjects O,[{0}].dbo.syscolumns CForgin where Sr.fkeyid={1} and Sr.fkey1=C.colid and Sr.rkeyid=O.id and CForgin.id=O.id and CForgin.colid=Sr.rkey1), (select O.name from [{0}].dbo.sysreferences Sr,[{0}].dbo.sysobjects O,[{0}].dbo.syscolumns CForgin where Sr.fkeyid={1} and Sr.fkey1=C.colid and Sr.rkeyid=O.id and CForgin.id=O.id and CForgin.colid=Sr.rkey1), (select Sr.rkeyid from [{0}].dbo.sysreferences Sr,[{0}].dbo.sysobjects O,[{0}].dbo.syscolumns CForgin where Sr.fkeyid={1} and Sr.fkey1=C.colid and Sr.rkeyid=O.id and CForgin.id=O.id and CForgin.colid=Sr.rkey1) from [{0}].dbo.systypes T, [{0}].dbo.syscolumns C left join [{0}].dbo.syscomments SC on C.cdefault=SC.id where C.id={1} and C.xtype=T.xusertype order by C.colid", databaseName, tableID);

            using (IDataReader rdr = this.ExecuteReader(connectionString, sqlString))
            {
                while (rdr.Read())
                {
                    string columnName = Convert.ToString(rdr.GetValue(0));
                    if (columnName == "msrepl_tran_version")//sqlserver 发布订阅字段，忽略
                    {
                        continue;
                    }
                    EDataType dataType = EDataTypeUtils.FromSqlServer(Convert.ToString(rdr.GetValue(1)));
                    int length = this.GetDataLength(dataType, Convert.ToInt32(rdr.GetValue(2)));
                    int precision = Convert.ToInt32(rdr.GetValue(3));
                    int scale = Convert.ToInt32(rdr.GetValue(4));
                    int isPrimaryKeyInt = Convert.ToInt32(rdr.GetValue(5));
                    int isNullableInt = Convert.ToInt32(rdr.GetValue(6));
                    int isIdentityInt = Convert.ToInt32(rdr.GetValue(7));
                    string defaultValue = Convert.ToString(rdr.GetValue(8));
                    string foreignColumnName = Convert.ToString(rdr.GetValue(9));
                    string foreignTableName = Convert.ToString(rdr.GetValue(10));
                    int foreignTableID = (rdr.IsDBNull(11)) ? 0 : Convert.ToInt32(rdr.GetValue(11));

                    bool isPrimaryKey = (isPrimaryKeyInt == 1) ? true : false;
                    bool isNullable = (isNullableInt == 1) ? true : false;
                    bool isIdentity = (isIdentityInt == 1) ? true : false;
                    //sqlserver 2005 返回isIdentity结果不正确,so 在此假设所有ID字段为Idenity字段
                    if (StringUtils.EqualsIgnoreCase(columnName, "ID")) isIdentity = true;

                    TableColumnInfo info = new TableColumnInfo(databaseName, tableID, columnName, dataType, length, precision, scale, isPrimaryKey, isNullable, isIdentity, defaultValue, foreignColumnName, foreignTableName, foreignTableID);
                    arraylist.Add(info);
                }
                rdr.Close();
            }

            SqlUtils.Cache_CacheTableColumnInfoArrayList(connectionString, databaseName, tableID, arraylist);

            return arraylist;
        }

        //lengthFromDb:数据库元数据查询获取的长度
        protected int GetDataLength(EDataType dataType, int lengthFromDb)
        {
            if (dataType == EDataType.NChar || dataType == EDataType.NVarChar)
            {
                return Convert.ToInt32(lengthFromDb / 2);
            }

            return lengthFromDb;
        }

        public bool IsColumnEquals(TableMetadataInfo metadataInfo, TableColumnInfo columnInfo)
        {
            if (!StringUtils.EqualsIgnoreCase(metadataInfo.AttributeName, columnInfo.ColumnName)) return false;
            if (!(metadataInfo.DataType == columnInfo.DataType)) return false;
            if (metadataInfo.DataLength != columnInfo.Length) return false;
            if (metadataInfo.CanBeNull != columnInfo.IsNullable) return false;
            string metaDefaultValue = (string.IsNullOrEmpty(metadataInfo.DBDefaultValue)) ? string.Empty : metadataInfo.DBDefaultValue;
            string columnDefaultValue = (string.IsNullOrEmpty(columnInfo.DefaultValue)) ? string.Empty : columnInfo.DefaultValue.Trim('(', ')');
            if (!string.Equals(metaDefaultValue, columnDefaultValue)) return false;
            return true;
        }

        public DataSet GetTableColumnsDataSet(string databaseName, string tableID)
        {
            ArrayList tableColumnInfoArrayList = GetTableColumnInfoArrayList(databaseName, tableID);
            DataSet dataset = new DataSet();
            DataTable dataTable = MakeTableColumnsTable();

            foreach (TableColumnInfo tableColumnInfo in tableColumnInfoArrayList)
            {
                DataRow dataRow = dataTable.NewRow();

                dataRow["ColumnName"] = tableColumnInfo.ColumnName;
                dataRow["DataType"] = EDataTypeUtils.GetValue(tableColumnInfo.DataType);
                dataRow["Length"] = tableColumnInfo.Length;
                dataRow["Precision"] = tableColumnInfo.Precision;
                dataRow["Scale"] = tableColumnInfo.Scale;
                dataRow["IsPrimaryKey"] = tableColumnInfo.IsPrimaryKey;
                dataRow["IsNullable"] = tableColumnInfo.IsNullable;
                dataRow["IsIdentity"] = tableColumnInfo.IsIdentity;
                dataRow["DefaultValue"] = tableColumnInfo.DefaultValue;
                dataRow["ForeignColumnName"] = tableColumnInfo.ForeignColumnName;
                dataRow["ForeignTableName"] = tableColumnInfo.ForeignTableName;
                dataRow["ForeignTableID"] = tableColumnInfo.ForeignTableID;

                dataTable.Rows.Add(dataRow);
            }
            dataset.Tables.Add(dataTable);
            return dataset;
        }

        private DataTable MakeTableColumnsTable()
        {
            TableColumnInfo info = new TableColumnInfo();

            DataTable tableColumnsTable = new DataTable("TableColumns");

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "AutoIncrement";
            column.AutoIncrementSeed = 1;
            column.AutoIncrement = true;
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ColumnName";
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "DataType";
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "Length";
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "Precision";
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "Scale";
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "IsPrimaryKey";
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "IsNullable";
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "IsIdentity";
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "DefaultValue";
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ForeignColumnName";
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ForeignTableName";
            tableColumnsTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ForeignTableID";
            tableColumnsTable.Columns.Add(column);

            return tableColumnsTable;
        }

        public string GetInsertSqlString(NameValueCollection attributes, string tableName, out IDbDataParameter[] parms)
        {
            return GetInsertSqlString(attributes, this.ConnectionString, tableName, out parms);
        }

        public virtual string GetInsertSqlString(NameValueCollection attributes, string connectionString, string tableName, out IDbDataParameter[] parms)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }
            //by 20151030 sofuny 获取自动增长列
            string sqlIdentity = "select[name] from syscolumns where id = object_id(N'" + tableName + "') and COLUMNPROPERTY(id, name,'IsIdentity')= 1";
            string clName = "";
            using (IDataReader rdr = this.ExecuteReader(sqlIdentity))
            {
                if (rdr.Read())
                {
                    clName = rdr[0].ToString();
                }
                rdr.Close();
            }
            parms = null;
            string databaseName = SqlUtils.GetDatabaseNameFormConnectionString(this.ADOType, connectionString);
            string tableID = GetTableID(connectionString, databaseName, tableName);
            ArrayList allTableColumnInfoArrayList = GetTableColumnInfoArrayList(connectionString, databaseName, tableID);

            ArrayList columnNameArrayList = new ArrayList();
            ArrayList columnValueArrayList = new ArrayList();

            List<IDbDataParameter> parameterList = new List<IDbDataParameter>();

            foreach (TableColumnInfo tableColumnInfo in allTableColumnInfoArrayList)
            {
                if (!tableColumnInfo.IsIdentity && tableColumnInfo.ColumnName != clName)
                {
                    if (attributes[tableColumnInfo.ColumnName] == null)
                    {
                        if (!tableColumnInfo.IsNullable && string.IsNullOrEmpty(tableColumnInfo.DefaultValue))
                        {
                            columnNameArrayList.Add(tableColumnInfo.ColumnName);
                            string valueStr = string.Empty;
                            columnValueArrayList.Add(SqlUtils.Parse(this.DataBaseType, tableColumnInfo.DataType, valueStr, tableColumnInfo.Length));

                            if (tableColumnInfo.DataType == EDataType.DateTime)
                            {
                                parameterList.Add(this.GetParameter("@" + tableColumnInfo.ColumnName, tableColumnInfo.DataType, TranslateUtils.ToDateTime(valueStr)));
                            }
                            else
                            {
                                parameterList.Add(this.GetParameter("@" + tableColumnInfo.ColumnName, tableColumnInfo.DataType, valueStr));
                            }


                        }
                    }
                    else
                    {
                        columnNameArrayList.Add(tableColumnInfo.ColumnName);
                        string valueStr = attributes[tableColumnInfo.ColumnName];
                        columnValueArrayList.Add(SqlUtils.Parse(this.DataBaseType, tableColumnInfo.DataType, valueStr, tableColumnInfo.Length));

                        if (tableColumnInfo.DataType == EDataType.DateTime)
                        {
                            parameterList.Add(this.GetParameter("@" + tableColumnInfo.ColumnName, tableColumnInfo.DataType, TranslateUtils.ToDateTime(valueStr)));
                        }
                        else
                        {
                            parameterList.Add(this.GetParameter("@" + tableColumnInfo.ColumnName, tableColumnInfo.DataType, valueStr));
                        }
                    }
                }
            }

            parms = parameterList.ToArray();

            string returnSqlString = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2})", tableName, TranslateUtils.ObjectCollectionToString(columnNameArrayList, " ,", "[", "]"), TranslateUtils.ObjectCollectionToString(columnNameArrayList, " ,", "@"));


            return returnSqlString;
        }

        public string GetUpdateSqlString(NameValueCollection attributes, string tableName, out IDbDataParameter[] parms)
        {
            return GetUpdateSqlString(attributes, this.ConnectionString, tableName, out parms);
        }

        public virtual string GetUpdateSqlString(NameValueCollection attributes, string connectionString, string tableName, out IDbDataParameter[] parms)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            parms = null;
            string databaseName = SqlUtils.GetDatabaseNameFormConnectionString(this.ADOType, connectionString);
            string tableID = GetTableID(connectionString, databaseName, tableName);
            ArrayList allTableColumnInfoArrayList = GetTableColumnInfoArrayList(connectionString, databaseName, tableID);

            ArrayList setArrayList = new ArrayList();
            ArrayList whereArrayList = new ArrayList();

            List<IDbDataParameter> parmsList = new List<IDbDataParameter>();

            foreach (TableColumnInfo tableColumnInfo in allTableColumnInfoArrayList)
            {
                if (attributes[tableColumnInfo.ColumnName] != null)
                {
                    if (!tableColumnInfo.IsPrimaryKey && tableColumnInfo.ColumnName != "ID")
                    {
                        string valueStr = attributes[tableColumnInfo.ColumnName];
                        string sqlValue = SqlUtils.Parse(this.DataBaseType, tableColumnInfo.DataType, valueStr, tableColumnInfo.Length);
                        if (!string.IsNullOrEmpty(sqlValue))
                        {
                            setArrayList.Add(string.Format("[{0}] = {1}", tableColumnInfo.ColumnName, "@" + tableColumnInfo.ColumnName));

                            if (tableColumnInfo.DataType == EDataType.DateTime)
                            {
                                parmsList.Add(this.GetParameter("@" + tableColumnInfo.ColumnName, tableColumnInfo.DataType, TranslateUtils.ToDateTime(valueStr)));
                            }
                            else
                            {
                                parmsList.Add(this.GetParameter("@" + tableColumnInfo.ColumnName, tableColumnInfo.DataType, valueStr));
                            }
                        }

                    }
                    else
                    {
                        string valueStr = attributes[tableColumnInfo.ColumnName];
                        string sqlValue = SqlUtils.Parse(this.DataBaseType, tableColumnInfo.DataType, valueStr, tableColumnInfo.Length);
                        whereArrayList.Add(string.Format("[{0}] = {1}", tableColumnInfo.ColumnName, "@" + tableColumnInfo.ColumnName));
                        parmsList.Add(this.GetParameter("@" + tableColumnInfo.ColumnName, tableColumnInfo.DataType, valueStr));
                    }
                }
            }

            if (whereArrayList.Count == 0 && !string.IsNullOrEmpty(attributes["ID"]))
            {
                string valueStr = attributes["ID"];
                string sqlValue = SqlUtils.Parse(this.DataBaseType, EDataType.Integer, valueStr, 0);
                whereArrayList.Add(string.Format("[ID] = @ID"));
            }

            if (whereArrayList.Count == 0)
            {
                throw new System.Data.MissingPrimaryKeyException();
            }
            if (setArrayList.Count == 0)
            {
                throw new System.Data.SyntaxErrorException();
            }

            parms = parmsList.ToArray();

            string returnSqlString = string.Format("UPDATE {0} SET {1} WHERE {2}", tableName, TranslateUtils.ObjectCollectionToString(setArrayList, " ,"), TranslateUtils.ObjectCollectionToString(whereArrayList, " AND "));

            return returnSqlString;
        }

        public string GetSelectSqlString(string tableName, string columns, string whereString)
        {
            return GetSelectSqlString(tableName, 0, columns, whereString, null);
        }

        public string GetSelectSqlString(string tableName, string columns, string whereString, string orderByString)
        {
            return GetSelectSqlString(tableName, 0, columns, whereString, orderByString);
        }

        public string GetSelectSqlString(string tableName, int totalNum, string columns, string whereString, string orderByString)
        {
            return GetSelectSqlString(this.ConnectionString, tableName, totalNum, columns, whereString, orderByString);
        }

        public string GetSelectSqlString(string connectionString, string tableName, int totalNum, string columns, string whereString, string orderByString)
        {
            return GetSelectSqlString(this.ConnectionString, tableName, totalNum, columns, whereString, orderByString, string.Empty);
        }

        public string GetSelectSqlString(string connectionString, string tableName, int totalNum, string columns, string whereString, string orderByString, string joinString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            string sqlString = string.Empty;

            if (!string.IsNullOrEmpty(whereString))
            {
                whereString = StringUtils.ReplaceStartsWith(whereString.Trim(), "AND", string.Empty);
                if (!StringUtils.StartsWithIgnoreCase(whereString, "WHERE "))
                {
                    whereString = "WHERE " + whereString;
                }
            }

            if (!string.IsNullOrEmpty(joinString))
            {
                whereString = joinString + " " + whereString;
            }

            if (totalNum > 0)
            {
                if (this.DataBaseType != EDatabaseType.Oracle)
                {
                    sqlString = string.Format("SELECT TOP {0} {1} FROM [{2}] {3} {4}", totalNum, columns, tableName, whereString, orderByString);
                }
                else
                {
                    sqlString = string.Format(@"
SELECT * FROM (
    SELECT {0} FROM [{1}] {2} {3}
) WHERE ROWNUM <= {4}
", columns, tableName, whereString, orderByString, totalNum);
                }
            }
            else
            {
                sqlString = string.Format("SELECT {0} FROM [{1}] {2} {3}", columns, tableName, whereString, orderByString);
            }

            return sqlString;
        }

        public string GetSelectSqlString(string tableName, int startNum, int totalNum, string columns, string whereString, string orderByString)
        {
            return GetSelectSqlString(this.ConnectionString, tableName, startNum, totalNum, columns, whereString, orderByString);
        }

        public string GetSelectSqlString(string connectionString, string tableName, int startNum, int totalNum, string columns, string whereString, string orderByString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            if (startNum <= 1)
            {
                return GetSelectSqlString(connectionString, tableName, totalNum, columns, whereString, orderByString);
            }

            string countSqlString = string.Format("SELECT Count(*) FROM [{0}] {1}", tableName, whereString);
            int count = BaiRongDataProvider.DatabaseDAO.GetIntResult(connectionString, countSqlString);
            if (totalNum == 0)
            {
                totalNum = count;
            }

            if (startNum > count) return string.Empty;

            int topNum = startNum + totalNum - 1;

            if (count < topNum)
            {
                totalNum = count - startNum + 1;
                if (totalNum < 1)
                {
                    return GetSelectSqlString(connectionString, tableName, totalNum, columns, whereString, orderByString);
                }
            }

            string orderByStringOpposite = this.GetOrderByStringOpposite(orderByString);

            string sqlString = string.Empty;
            if (this.DataBaseType != EDatabaseType.Oracle)
            {
                sqlString = string.Format(@"
SELECT {0}
FROM (SELECT TOP {1} {0}
        FROM (SELECT TOP {2} {0}
                FROM [{3}] {4} {5}) tmp
        {6}) tmp
{5}
", columns, totalNum, topNum, tableName, whereString, orderByString, orderByStringOpposite);
            }
            else
            {
                sqlString = string.Format(@"
SELECT {0} FROM (
    SELECT {0} FROM (
        SELECT {0} FROM [{3}] {4} {5}
    ) WHERE ROWNUM <= {2} {6}
) WHERE ROWNUM <= {1} {5}
", columns, totalNum, topNum, tableName, whereString, orderByString, orderByStringOpposite);
            }

            return sqlString;
        }

        public string GetSelectSqlStringByQueryString(string connectionString, string queryString, int totalNum, string orderByString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            if (totalNum == 0 && string.IsNullOrEmpty(orderByString))
            {
                return queryString;
            }
            string sqlString = string.Empty;
            if (totalNum > 0)
            {
                //TODO: 当queryString包含top 2语句时排序有问题
                if (this.DataBaseType != EDatabaseType.Oracle)
                {
                    sqlString = string.Format("SELECT TOP {0} * FROM ({1}) tmp {2}", totalNum, queryString, orderByString);
                }
                else
                {
                    sqlString = string.Format("SELECT * FROM ({0}) WHERE ROWNUM <= {1} {2}", queryString, totalNum, orderByString);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(orderByString))
                {
                    sqlString = queryString;
                }
                else
                {
                    sqlString = string.Format("SELECT * FROM ({0}) tmp {1}", queryString, orderByString);
                }
            }
            return sqlString;
        }


        public string GetSelectSqlStringByQueryString(string connectionString, string queryString, int startNum, int totalNum, string orderByString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            if (startNum == 1 && totalNum == 0 && string.IsNullOrEmpty(orderByString))
            {
                return queryString;
            }
            //queryString = queryString.Trim().ToUpper();
            if (queryString.LastIndexOf(" ORDER ") != -1)
            {
                if (string.IsNullOrEmpty(orderByString))
                {
                    orderByString = queryString.Substring(queryString.LastIndexOf(" ORDER ") + 1);
                }
                queryString = queryString.Substring(0, queryString.LastIndexOf(" ORDER "));
            }
            orderByString = this.ParseOrderByString(orderByString);

            if (startNum <= 1)
            {
                return GetSelectSqlStringByQueryString(connectionString, queryString, totalNum, orderByString);
            }

            string countSqlString = string.Format("SELECT Count(*) FROM ({0}) tmp", queryString);
            int count = BaiRongDataProvider.DatabaseDAO.GetIntResult(connectionString, countSqlString);
            if (totalNum == 0)
            {
                totalNum = count;
            }

            if (startNum > count) return string.Empty;

            int topNum = startNum + totalNum - 1;

            if (count < topNum)
            {
                totalNum = count - startNum + 1;
                if (totalNum < 1)
                {
                    return GetSelectSqlStringByQueryString(connectionString, queryString, totalNum, orderByString);
                }
            }

            string orderByStringOpposite = this.GetOrderByStringOpposite(orderByString);

            string sqlString = string.Empty;

            if (this.DataBaseType != EDatabaseType.Oracle)
            {
                sqlString = string.Format(@"
SELECT *
FROM (SELECT TOP {0} *
        FROM (SELECT TOP {1} *
                FROM ({2}) tmp {3}) tmp
        {4}) tmp
{3}
", totalNum, topNum, queryString, orderByString, orderByStringOpposite);
            }
            else
            {
                sqlString = string.Format(@"
SELECT * FROM (
    SELECT * FROM (
        {2}
    ) WHERE ROWNUM <= {1} {4}
) WHERE ROWNUM <= {0} {3}
", totalNum, topNum, queryString, orderByString, orderByStringOpposite);
            }

            return sqlString;
        }

        private string ParseOrderByString(string orderByString)
        {
            if (!string.IsNullOrEmpty(orderByString))
            {
                orderByString = orderByString.ToUpper().Trim();
                if (!orderByString.StartsWith("ORDER BY"))
                {
                    orderByString = "ORDER BY " + orderByString;
                }
                if (!orderByString.EndsWith("DESC") && !orderByString.EndsWith("ASC"))
                {
                    orderByString = orderByString + " ASC";
                }
            }
            return orderByString;
        }

        private string GetOrderByStringOpposite(string orderByString)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(orderByString))
            {
                retval = orderByString.Replace(" DESC", " DESC_OPPOSITE").Replace(" ASC", " DESC").Replace(" DESC_OPPOSITE", " ASC");
            }
            return retval;
        }
    }
}
