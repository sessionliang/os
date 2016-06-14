using System;
using System.Text;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using System.Collections.Generic;

namespace BaiRong.Provider.Data.SqlServer
{
    public class TableMetadataDAO : DataProviderBase, ITableMetadataDAO
    {
        private const string SQL_SELECT_TABLE_METADATA = "SELECT TableMetadataID, AuxiliaryTableENName, AttributeName, DataType, DataLength, CanBeNull, DBDefaultValue, Taxis, IsSystem FROM bairong_TableMetadata WHERE TableMetadataID = @TableMetadataID";

        private const string SQL_SELECT_ALL_TABLE_METADATA_BY_ENNAME = "SELECT TableMetadataID, AuxiliaryTableENName, AttributeName, DataType, DataLength, CanBeNull, DBDefaultValue, Taxis, IsSystem FROM bairong_TableMetadata WHERE AuxiliaryTableENName = @AuxiliaryTableENName ORDER BY IsSystem DESC, Taxis";

        private const string SQL_SELECT_TABLE_METADATA_COUNT_BY_ENNAME = "SELECT COUNT(*) FROM bairong_TableMetadata WHERE AuxiliaryTableENName = @AuxiliaryTableENName";

        private const string SQL_SELECT_TABLE_METADATA_BY_TABLE_ENNAME_AND_ATTRIBUTE_NAME = "SELECT TableMetadataID, AuxiliaryTableENName, AttributeName, DataType, DataLength, CanBeNull, DBDefaultValue, Taxis, IsSystem FROM bairong_TableMetadata WHERE AuxiliaryTableENName = @AuxiliaryTableENName AND AttributeName = @AttributeName";

        private const string SQL_SELECT_TABLE_METADATA_ID_BY_TABLE_ENNAME_AND_ATTRIBUTE_NAME = "SELECT TableMetadataID FROM bairong_TableMetadata WHERE AuxiliaryTableENName = @AuxiliaryTableENName AND AttributeName = @AttributeName";

        private const string SQL_SELECT_TABLE_METADATA_ALL_ATTRIBUTE_NAME = "SELECT AttributeName FROM bairong_TableMetadata WHERE AuxiliaryTableENName = @AuxiliaryTableENName ORDER BY Taxis";

        private const string SQL_UPDATE_TABLE_METADATA = "UPDATE bairong_TableMetadata SET AuxiliaryTableENName = @AuxiliaryTableENName, AttributeName = @AttributeName, DataType = @DataType, DataLength = @DataLength, CanBeNull = @CanBeNull, DBDefaultValue = @DBDefaultValue, IsSystem = @IsSystem WHERE  TableMetadataID = @TableMetadataID";

        private const string SQL_DELETE_TABLE_METADATA = "DELETE FROM bairong_TableMetadata WHERE  TableMetadataID = @TableMetadataID";

        private const string SQL_DELETE_TABLE_METADATA_BY_TABLE_NAME = "DELETE FROM bairong_TableMetadata WHERE  AuxiliaryTableENName = @AuxiliaryTableENName";

        private const string SQL_UPDATE_TABLE_METADATA_TAXIS = "UPDATE bairong_TableMetadata SET Taxis = @Taxis WHERE  TableMetadataID = @TableMetadataID";

        private const string PARM_TABLE_METADATA_ID = "@TableMetadataID";
        private const string PARM_AUXILIARY_TABLE_ENNAME = "@AuxiliaryTableENName";
        private const string PARM_ATTRIBUTE_NAME = "@AttributeName";
        private const string PARM_DATA_TYPE = "@DataType";
        private const string PARM_DATA_LENGTH = "@DataLength";
        private const string PARM_CAN_BE_NULL = "@CanBeNull";
        private const string PARM_DB_DEFAULT_VALUE = "@DBDefaultValue";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_IS_SYSTEM = "@IsSystem";

        public void Insert(TableMetadataInfo info)
        {
            string sqlString = "INSERT INTO bairong_TableMetadata (AuxiliaryTableENName, AttributeName, DataType, DataLength, CanBeNull, DBDefaultValue, Taxis, IsSystem) VALUES (@AuxiliaryTableENName, @AttributeName, @DataType, @DataLength, @CanBeNull, @DBDefaultValue, @Taxis, @IsSystem)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_TableMetadata (TableMetadataID, AuxiliaryTableENName, AttributeName, DataType, DataLength, CanBeNull, DBDefaultValue, Taxis, IsSystem) VALUES (bairong_TableMetadata_SEQ.NEXTVAL, @AuxiliaryTableENName, @AttributeName, @DataType, @DataLength, @CanBeNull, @DBDefaultValue, @Taxis, @IsSystem)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar, 50, info.AuxiliaryTableENName),
				this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, info.AttributeName),
				this.GetParameter(PARM_DATA_TYPE, EDataType.VarChar, 50, EDataTypeUtils.GetValue(info.DataType)),
				this.GetParameter(PARM_DATA_LENGTH, EDataType.Integer, info.DataLength),
				this.GetParameter(PARM_CAN_BE_NULL, EDataType.VarChar, 18, info.CanBeNull.ToString()),
				this.GetParameter(PARM_DB_DEFAULT_VALUE, EDataType.NVarChar, 255, info.DBDefaultValue),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, GetMaxTaxis(info.AuxiliaryTableENName) + 1),
				this.GetParameter(PARM_IS_SYSTEM, EDataType.VarChar, 18, info.IsSystem.ToString())
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                try
                {
                    this.ExecuteNonQuery(conn, sqlString, insertParms);

                    BaiRongDataProvider.TableCollectionDAO.UpdateAttributeNum(info.AuxiliaryTableENName);
                    BaiRongDataProvider.TableCollectionDAO.UpdateIsChangedAfterCreatedInDB(true, info.AuxiliaryTableENName);
                    TableManager.IsChanged = true;
                }
                catch
                {
                    throw;
                }
            }
        }


        internal void InsertWithTransaction(TableMetadataInfo info, EAuxiliaryTableType tableType, int taxis, IDbTransaction trans)
        {
            string sqlString = "INSERT INTO bairong_TableMetadata (AuxiliaryTableENName, AttributeName, DataType, DataLength, CanBeNull, DBDefaultValue, Taxis, IsSystem) VALUES (@AuxiliaryTableENName, @AttributeName, @DataType, @DataLength, @CanBeNull, @DBDefaultValue, @Taxis, @IsSystem)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_TableMetadata (TableMetadataID, AuxiliaryTableENName, AttributeName, DataType, DataLength, CanBeNull, DBDefaultValue, Taxis, IsSystem) VALUES (bairong_TableMetadata_SEQ.NEXTVAL, @AuxiliaryTableENName, @AttributeName, @DataType, @DataLength, @CanBeNull, @DBDefaultValue, @Taxis, @IsSystem)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
		    {
			    this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar, 50, info.AuxiliaryTableENName),
			    this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, info.AttributeName),
			    this.GetParameter(PARM_DATA_TYPE, EDataType.VarChar, 50, EDataTypeUtils.GetValue(info.DataType)),
			    this.GetParameter(PARM_DATA_LENGTH, EDataType.Integer, info.DataLength),
			    this.GetParameter(PARM_CAN_BE_NULL, EDataType.VarChar, 18, info.CanBeNull.ToString()),
			    this.GetParameter(PARM_DB_DEFAULT_VALUE, EDataType.NVarChar, 255, info.DBDefaultValue),
			    this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
			    this.GetParameter(PARM_IS_SYSTEM, EDataType.VarChar, 18, info.IsSystem.ToString())
		    };

            this.ExecuteNonQuery(trans, sqlString, insertParms);
            if (info.StyleInfo != null)
            {
                info.StyleInfo.TableName = info.AuxiliaryTableENName;
                info.StyleInfo.AttributeName = info.AttributeName;
                BaiRongDataProvider.TableStyleDAO.InsertWithTransaction(info.StyleInfo, EAuxiliaryTableTypeUtils.GetTableStyle(tableType), trans);
                TableStyleManager.IsChanged = true;
            }
            TableManager.IsChanged = true;
        }

        public void InsertSystemItems(string tableENName, EAuxiliaryTableType tableType, IDbTransaction trans)
        {
            ArrayList arraylist = BaiRongDataProvider.AuxiliaryTableDataDAO.GetDefaultTableMetadataInfoArrayList(tableENName, tableType);
            if (arraylist != null && arraylist.Count > 0)
            {
                int taxis = 1;
                foreach (TableMetadataInfo info in arraylist)
                {
                    this.InsertWithTransaction(info, tableType, taxis++, trans);
                }
            }
        }


        public void Update(TableMetadataInfo info)
        {
            bool isSqlChanged = true;
            TableMetadataInfo originalInfo = this.GetTableMetadataInfo(info.TableMetadataID);
            if (originalInfo != null)
            {
                if (info.AuxiliaryTableENName == originalInfo.AuxiliaryTableENName
                 && info.AttributeName == originalInfo.AttributeName
                 && info.DataType == originalInfo.DataType
                 && info.DataLength == originalInfo.DataLength
                 && info.CanBeNull == originalInfo.CanBeNull
                 && info.DBDefaultValue == originalInfo.DBDefaultValue
                 && info.Taxis == originalInfo.Taxis)
                {
                    isSqlChanged = false;
                }
            }

            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar, 50, info.AuxiliaryTableENName),
				this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, info.AttributeName),
				this.GetParameter(PARM_DATA_TYPE, EDataType.VarChar, 50, EDataTypeUtils.GetValue(info.DataType)),
				this.GetParameter(PARM_DATA_LENGTH, EDataType.Integer, info.DataLength),
				this.GetParameter(PARM_CAN_BE_NULL, EDataType.VarChar, 18, info.CanBeNull.ToString()),
				this.GetParameter(PARM_DB_DEFAULT_VALUE, EDataType.NVarChar, 255, info.DBDefaultValue),
				this.GetParameter(PARM_IS_SYSTEM, EDataType.VarChar, 18, info.IsSystem.ToString()),
				this.GetParameter(PARM_TABLE_METADATA_ID, EDataType.Integer, info.TableMetadataID)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                try
                {
                    this.ExecuteNonQuery(conn, SQL_UPDATE_TABLE_METADATA, updateParms);
                    if (isSqlChanged)
                    {
                        BaiRongDataProvider.TableCollectionDAO.UpdateIsChangedAfterCreatedInDB(true, info.AuxiliaryTableENName);
                    }
                    TableManager.IsChanged = true;
                }
                catch
                {
                    throw;
                }
            }
        }

        public void Delete(int tableMetadataID)
        {
            if (FileConfigManager.Instance.IsSaas) return;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_METADATA_ID, EDataType.Integer, tableMetadataID)
			};

            TableMetadataInfo metadataInfo = this.GetTableMetadataInfo(tableMetadataID);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                try
                {
                    this.ExecuteNonQuery(conn, SQL_DELETE_TABLE_METADATA, parms);

                    BaiRongDataProvider.TableCollectionDAO.UpdateAttributeNum(metadataInfo.AuxiliaryTableENName);
                    BaiRongDataProvider.TableCollectionDAO.UpdateIsChangedAfterCreatedInDB(true, metadataInfo.AuxiliaryTableENName);
                    TableManager.IsChanged = true;
                }
                catch
                {
                    throw;
                }
            }
        }

        public void Delete(string tableENName, IDbTransaction trans)
        {
            if (FileConfigManager.Instance.IsSaas) return;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar,50, tableENName)
			};
            if (trans == null)
            {
                using (IDbConnection conn = this.GetConnection())
                {
                    conn.Open();
                    try
                    {
                        this.ExecuteNonQuery(conn, SQL_DELETE_TABLE_METADATA_BY_TABLE_NAME, parms);
                        TableManager.IsChanged = true;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            else
            {
                try
                {
                    this.ExecuteNonQuery(trans, SQL_DELETE_TABLE_METADATA_BY_TABLE_NAME, parms);
                    TableManager.IsChanged = true;
                }
                catch
                {
                    throw;
                }

            }
        }

        public TableMetadataInfo GetTableMetadataInfo(int tableMetadataID)
        {
            TableMetadataInfo info = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_METADATA_ID, EDataType.Integer, tableMetadataID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_METADATA, parms))
            {
                if (rdr.Read())
                {
                    info = new TableMetadataInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EDataTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), rdr.GetInt32(7), TranslateUtils.ToBool(rdr.GetValue(8).ToString()));
                }
                rdr.Close();
            }

            return info;
        }

        public TableMetadataInfo GetTableMetadataInfo(string tableENName, string attributeName)
        {
            TableMetadataInfo info = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar, 50, tableENName),
				this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, attributeName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_METADATA_BY_TABLE_ENNAME_AND_ATTRIBUTE_NAME, parms))
            {
                if (rdr.Read())
                {
                    info = new TableMetadataInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EDataTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), rdr.GetInt32(7), TranslateUtils.ToBool(rdr.GetValue(8).ToString()));
                }
                rdr.Close();
            }

            return info;
        }

        public int GetTableMetadataID(string tableENName, string attributeName)
        {
            int tableMetadataID = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar, 50, tableENName),
				this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, attributeName)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, SQL_SELECT_TABLE_METADATA_ID_BY_TABLE_ENNAME_AND_ATTRIBUTE_NAME, parms))
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            tableMetadataID = Convert.ToInt32(rdr[0]);
                        }
                    }
                    rdr.Close();
                }
            }

            return tableMetadataID;
        }

        public IEnumerable GetDataSource(string tableENName)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar, 50, tableENName)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_TABLE_METADATA_BY_ENNAME, parms);
            return enumerable;
        }

        public IEnumerable GetDataSorceMinusAttributes(string tableENName, ArrayList attributeNameArrayList)
        {
            string parameterNameList = string.Empty;
            List<IDbDataParameter> parameterList = this.GetINParameterList(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, attributeNameArrayList, out parameterNameList);
            string sqlString = string.Format("SELECT TableMetadataID, AuxiliaryTableENName, AttributeName, DataType, DataLength, DataScale, CanBeNull, DBDefaultValue, Taxis, IsSystem FROM bairong_TableMetadata WHERE AuxiliaryTableENName = @AuxiliaryTableENName AND AttributeName NOT IN ({0}) ORDER BY Taxis", parameterNameList);

            List<IDbDataParameter> paramList = new List<IDbDataParameter>();
            paramList.Add(this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar, 50, tableENName));
            paramList.AddRange(parameterList);

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString, paramList.ToArray());
            return enumerable;
        }

        public ArrayList GetTableMetadataInfoArrayList(string tableENName)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar, 50, tableENName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_TABLE_METADATA_BY_ENNAME, parms))
            {
                while (rdr.Read())
                {
                    TableMetadataInfo info = new TableMetadataInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EDataTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), rdr.GetInt32(7), TranslateUtils.ToBool(rdr.GetValue(8).ToString()));
                    arraylist.Add(info);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public Hashtable GetTableENNameAndTableMetadataInfoArrayListHashtable()
        {
            Hashtable hashtable = new Hashtable();
            ArrayList tableNameList = BaiRongDataProvider.TableCollectionDAO.GetTableENNameCollection();
            foreach (string tableName in tableNameList)
            {
                ArrayList arraylist = this.GetTableMetadataInfoArrayList(tableName);
                hashtable.Add(tableName, arraylist);
            }
            return hashtable;
        }

        /// <summary>
        /// Get Total AuxiliaryTable Count
        /// </summary>
        public int GetTableMetadataCountByENName(string tableENName)
        {
            int count = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar, 50, tableENName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_METADATA_COUNT_BY_ENNAME, parms))
            {
                if (rdr.Read())
                {
                    count = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return count;
        }

        public ArrayList GetAttributeNameArrayList(string tableENName)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar, 50, tableENName)
			};

            ArrayList list = new ArrayList();
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_METADATA_ALL_ATTRIBUTE_NAME, parms))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return list;
        }

        /// <summary>
        /// Get max Taxis in the database
        /// </summary>
        public int GetMaxTaxis(string tableENName)
        {
            string sqlString = "SELECT MAX(Taxis) FROM bairong_TableMetadata WHERE AuxiliaryTableENName = @AuxiliaryTableENName";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_AUXILIARY_TABLE_ENNAME, EDataType.VarChar, 50, tableENName)
			};

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString, parms);
        }


        /// <summary>
        /// Change The Texis To Higher Level
        /// </summary>
        public void TaxisUp(int SelectedID, string tableENName)
        {
            //Get Higher Taxis and ClassID
            string cmd = "SELECT TOP 1 TableMetadataID, Taxis FROM bairong_TableMetadata WHERE ((Taxis > (SELECT Taxis FROM bairong_TableMetadata WHERE (TableMetadataID = @TableMetadataID AND AuxiliaryTableENName = @AuxiliaryTableENName1))) AND AuxiliaryTableENName=@AuxiliaryTableENName2) ORDER BY Taxis";
            int HigherID = 0;
            int HigherTaxis = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_METADATA_ID, EDataType.Integer, SelectedID),
				this.GetParameter("@AuxiliaryTableENName1", EDataType.VarChar, 50, tableENName),
				this.GetParameter("@AuxiliaryTableENName2", EDataType.VarChar, 50, tableENName)
			};

            try
            {
                using (IDataReader rdr = this.ExecuteReader(cmd, parms))
                {
                    if (rdr.Read())
                    {
                        HigherID = Convert.ToInt32(rdr[0]);
                        HigherTaxis = Convert.ToInt32(rdr[1]);
                    }
                    rdr.Close();
                }
            }
            catch
            {
                throw;
            }

            //Get Taxis Of Selected Class
            int SelectedTaxis = GetTaxis(SelectedID);

            if (HigherID != 0)
            {
                //Set The Selected Class Taxis To Higher Level
                SetTaxis(SelectedID, HigherTaxis);
                //Set The Higher Class Taxis To Lower Level
                SetTaxis(HigherID, SelectedTaxis);
                TableManager.IsChanged = true;
                //BaiRongDataProvider.CreateAuxiliaryTableDAO().UpdateIsChangedAfterCreatedInDB(EBoolean.True, tableENName);
            }

        }

        /// <summary>
        /// Change The Texis To Lower Level
        /// </summary>
        public void TaxisDown(int SelectedID, string tableENName)
        {
            //Get Lower Taxis and ClassID
            string cmd = "SELECT TOP 1 TableMetadataID, Taxis FROM bairong_TableMetadata WHERE ((Taxis < (SELECT Taxis FROM bairong_TableMetadata WHERE (TableMetadataID = @TableMetadataID AND AuxiliaryTableENName = @AuxiliaryTableENName1))) AND AuxiliaryTableENName = @AuxiliaryTableENName2) ORDER BY Taxis DESC";
            int LowerID = 0;
            int LowerTaxis = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_METADATA_ID, EDataType.Integer, SelectedID),
				this.GetParameter("@AuxiliaryTableENName1", EDataType.VarChar, 50, tableENName),
				this.GetParameter("@AuxiliaryTableENName2", EDataType.VarChar, 50, tableENName)
			};

            try
            {
                using (IDataReader rdr = this.ExecuteReader(cmd, parms))
                {
                    if (rdr.Read())
                    {
                        LowerID = Convert.ToInt32(rdr[0]);
                        LowerTaxis = Convert.ToInt32(rdr[1]);
                    }
                    rdr.Close();
                }
            }
            catch
            {
                throw;
            }

            //Get Taxis Of Selected Class
            int SelectedTaxis = GetTaxis(SelectedID);

            if (LowerID != 0)
            {
                //Set The Selected Class Taxis To Lower Level
                SetTaxis(SelectedID, LowerTaxis);
                //Set The Lower Class Taxis To Higher Level
                SetTaxis(LowerID, SelectedTaxis);
                TableManager.IsChanged = true;
                //BaiRongDataProvider.CreateAuxiliaryTableDAO().UpdateIsChangedAfterCreatedInDB(EBoolean.True, tableENName);
            }
        }

        private int GetTaxis(int SelectedID)
        {
            string cmd = "SELECT Taxis FROM bairong_TableMetadata WHERE (TableMetadataID = @TableMetadataID)";
            int taxis = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_METADATA_ID, EDataType.Integer, SelectedID)
			};

            using (IDataReader rdr = this.ExecuteReader(cmd, parms))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int id, int taxis)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
				this.GetParameter(PARM_TABLE_METADATA_ID, EDataType.Integer, id)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TABLE_METADATA_TAXIS, parms);
            TableManager.IsChanged = true;
        }

        public void CreateAuxiliaryTable(string tableENName)
        {
            string createTableSqlString = BaiRongDataProvider.AuxiliaryTableDataDAO.GetCreateAuxiliaryTableSqlString(tableENName);

            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter("@IsCreatedInDB", EDataType.VarChar, 18, true.ToString()),
				this.GetParameter("@IsChangedAfterCreatedInDB", EDataType.VarChar, 18, false.ToString()),
				this.GetParameter("@TableENName", EDataType.VarChar, 50, tableENName)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        System.IO.StringReader reader = new System.IO.StringReader(createTableSqlString);
                        string sql = null;
                        while (null != (sql = SqlUtils.ReadNextStatementFromStream(reader)))
                        {
                            this.ExecuteNonQuery(trans, sql.Trim());
                        }

                        this.ExecuteNonQuery(trans, "UPDATE bairong_TableCollection SET IsCreatedInDB = @IsCreatedInDB, IsChangedAfterCreatedInDB = @IsChangedAfterCreatedInDB WHERE TableENName = @TableENName", updateParms);
                        TableManager.IsChanged = true;
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void CreateAuxiliaryTableOfArchive(string tableENName)
        {
            string createTableSqlString = BaiRongDataProvider.AuxiliaryTableDataDAO.GetCreateAuxiliaryTableSqlString(tableENName);

            string archiveTableName = TableManager.GetTableNameOfArchive(tableENName);

            createTableSqlString = createTableSqlString.Replace(tableENName, archiveTableName);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        System.IO.StringReader reader = new System.IO.StringReader(createTableSqlString);
                        string sql = null;

                        while (null != (sql = SqlUtils.ReadNextStatementFromStream(reader)))
                        {
                            this.ExecuteNonQuery(trans, sql.Trim());
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DeleteAuxiliaryTable(string tableENName)
        {
            if (FileConfigManager.Instance.IsSaas) return;

            if (BaiRongDataProvider.TableStructureDAO.IsTableExists(tableENName))
            {
                string dropTableSqlString = string.Format("DROP TABLE [{0}]", tableENName);

                IDbDataParameter[] updateParms = new IDbDataParameter[]
				{
					this.GetParameter("@IsCreatedInDB", EDataType.VarChar, 18, false.ToString()),
					this.GetParameter("@IsChangedAfterCreatedInDB", EDataType.VarChar, 18, false.ToString()),
					this.GetParameter("@TableENName", EDataType.VarChar, 50, tableENName)
				};

                using (IDbConnection conn = this.GetConnection())
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            this.ExecuteNonQuery(trans, dropTableSqlString);
                            this.ExecuteNonQuery(trans, "UPDATE bairong_TableCollection SET IsCreatedInDB = @IsCreatedInDB, IsChangedAfterCreatedInDB = @IsChangedAfterCreatedInDB WHERE TableENName = @TableENName", updateParms);
                            TableManager.IsChanged = true;
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }


        public void ReCreateAuxiliaryTable(string tableENName, EAuxiliaryTableType tableType)
        {
            if (FileConfigManager.Instance.IsSaas) return;

            ArrayList defaultTableMetadataInfoArrayList = BaiRongDataProvider.AuxiliaryTableDataDAO.GetDefaultTableMetadataInfoArrayList(tableENName, tableType);

            if (BaiRongDataProvider.TableStructureDAO.IsTableExists(tableENName))
            {
                IDbDataParameter[] updateParms = new IDbDataParameter[]
				{
					this.GetParameter("@IsCreatedInDB", EDataType.VarChar, 18, true.ToString()),
					this.GetParameter("@IsChangedAfterCreatedInDB", EDataType.VarChar, 18, false.ToString()),
					this.GetParameter("@TableENName", EDataType.VarChar, 50, tableENName)
				};

                int taxis = this.GetMaxTaxis(tableENName) + 1;

                using (IDbConnection conn = this.GetConnection())
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (TableMetadataInfo tableMetadataInfo in defaultTableMetadataInfoArrayList)
                            {
                                if (this.GetTableMetadataID(tableENName, tableMetadataInfo.AttributeName) == 0)
                                {
                                    this.InsertWithTransaction(tableMetadataInfo, tableType, taxis++, trans);
                                }
                            }

                            string dropTableSqlString = string.Format("DROP TABLE [{0}]", tableENName);
                            string createTableSqlString = BaiRongDataProvider.AuxiliaryTableDataDAO.GetCreateAuxiliaryTableSqlString(tableENName);

                            this.ExecuteNonQuery(trans, dropTableSqlString);

                            System.IO.StringReader reader = new System.IO.StringReader(createTableSqlString);
                            string sql = null;
                            while (null != (sql = SqlUtils.ReadNextStatementFromStream(reader)))
                            {
                                this.ExecuteNonQuery(trans, sql.Trim());
                            }

                            this.ExecuteNonQuery(trans, "UPDATE bairong_TableCollection SET IsCreatedInDB = @IsCreatedInDB, IsChangedAfterCreatedInDB = @IsChangedAfterCreatedInDB WHERE  TableENName = @TableENName", updateParms);
                            TableManager.IsChanged = true;
                            SqlUtils.Cache_RemoveTableColumnInfoArrayListCache();
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        protected const string ERROR_COMMAND_MESSAGE = "此辅助表无字段，无法在数据库中生成表！";

        protected ArrayList GetAlterDropColumnSqls(string tableENName, string attributeName, bool isHasDefaultValue)
        {
            ArrayList sqlArrayList = new ArrayList();
            string defaultConstraintName = BaiRongDataProvider.TableStructureDAO.GetDefaultConstraintName(tableENName, attributeName);
            if (!string.IsNullOrEmpty(defaultConstraintName))
            {
                sqlArrayList.Add(string.Format("ALTER TABLE [{0}] DROP CONSTRAINT [{1}]", tableENName, defaultConstraintName));
            }
            sqlArrayList.Add(string.Format("ALTER TABLE [{0}] DROP COLUMN [{1}]", tableENName, attributeName));
            return sqlArrayList;
        }

        protected ArrayList GetAlterAddColumnSqls(string tableENName, TableMetadataInfo metadataInfo)
        {
            ArrayList sqlArrayList = new ArrayList();
            string columnSqlString = SqlUtils.GetColumnSqlString(this.ADOType, metadataInfo.DataType, metadataInfo.AttributeName, metadataInfo.DataLength, metadataInfo.CanBeNull, metadataInfo.DBDefaultValue);
            string alterSqlString = string.Format("ALTER TABLE [{0}] ADD {1}", tableENName, columnSqlString);
            sqlArrayList.Add(alterSqlString);
            return sqlArrayList;
        }

        public void SyncTable(string tableENName)
        {
            if (FileConfigManager.Instance.IsSaas) return;

            ITableStructureDAO strucDAO = BaiRongDataProvider.TableStructureDAO;
            ArrayList arraylist = this.GetTableMetadataInfoArrayList(tableENName);
            string databaseName = SqlUtils.GetDatabaseNameFormConnectionString(BaiRongDataProvider.ADOType, BaiRongDataProvider.ConnectionString);
            string tableID = strucDAO.GetTableID(BaiRongDataProvider.ConnectionString, databaseName, tableENName);
            ArrayList columnArraylist = strucDAO.GetTableColumnInfoArrayList(BaiRongDataProvider.ConnectionString, databaseName, tableID);

            ArrayList sqlArrayList = new ArrayList();

            //添加新增/修改字段SQL语句
            foreach (TableMetadataInfo metadataInfo in arraylist)
            {
                if (metadataInfo.IsSystem) continue;
                bool columnExists = false;
                foreach (TableColumnInfo columnInfo in columnArraylist)
                {
                    if (StringUtils.EqualsIgnoreCase(columnInfo.ColumnName, metadataInfo.AttributeName))
                    {
                        columnExists = true;
                        if (!strucDAO.IsColumnEquals(metadataInfo, columnInfo))
                        {
                            bool isHasDefaultValue = !string.IsNullOrEmpty(columnInfo.DefaultValue);
                            ArrayList alterSqlArraylist = this.GetAlterDropColumnSqls(tableENName, columnInfo.ColumnName, isHasDefaultValue);
                            foreach (string sql in alterSqlArraylist)
                            {
                                sqlArrayList.Add(sql);
                            }
                            alterSqlArraylist = this.GetAlterAddColumnSqls(tableENName, metadataInfo);
                            foreach (string sql in alterSqlArraylist)
                            {
                                sqlArrayList.Add(sql);
                            }
                        }
                        break;
                    }
                }
                if (!columnExists)
                {
                    ArrayList alterSqlArraylist = this.GetAlterAddColumnSqls(tableENName, metadataInfo);
                    foreach (string sql in alterSqlArraylist)
                    {
                        sqlArrayList.Add(sql);
                    }
                }
            }

            //添加删除字段SQL语句
            EAuxiliaryTableType tableType = BaiRongDataProvider.TableCollectionDAO.GetTableType(tableENName);
            ArrayList hiddenAttributeNameArrayList = TableManager.GetHiddenAttributeNameArrayList(EAuxiliaryTableTypeUtils.GetTableStyle(tableType));
            foreach (TableColumnInfo columnInfo in columnArraylist)
            {
                if (hiddenAttributeNameArrayList.Contains(columnInfo.ColumnName.ToLower())) continue;
                bool isNeedDelete = true;
                foreach (TableMetadataInfo metadataInfo in arraylist)
                {
                    if (StringUtils.EqualsIgnoreCase(columnInfo.ColumnName, metadataInfo.AttributeName))
                    {
                        isNeedDelete = false;
                        break;
                    }
                }
                if (isNeedDelete)
                {
                    bool isHasDefaultValue = !string.IsNullOrEmpty(columnInfo.DefaultValue);
                    ArrayList alterSqlArraylist = this.GetAlterDropColumnSqls(tableENName, columnInfo.ColumnName, isHasDefaultValue);
                    foreach (string sql in alterSqlArraylist)
                    {
                        sqlArrayList.Add(sql);
                    }
                }
            }
            BaiRongDataProvider.DatabaseDAO.ExecuteSql(sqlArrayList);
            BaiRongDataProvider.TableCollectionDAO.UpdateIsChangedAfterCreatedInDB(false, tableENName);
        }

        //public string[] GetCreateAuxiliaryTableSqlString(string tableENName)
        //{
        //    StringBuilder createSqlString = new StringBuilder("");
        //    StringBuilder alterSqlString = new StringBuilder("");

        //    AuxiliaryTableInfo tableInfo = BaiRongDataProvider.CreateAuxiliaryTableDAO().GetAuxiliaryTableInfo(tableENName);
        //    if (tableInfo == null) throw new Exception(ERROR_COMMAND_MESSAGE);

        //    ArrayList tableMetadataInfoArrayList = this.GetTableMetadataInfoArrayList(tableENName);
        //    if (tableMetadataInfoArrayList.Count == 0) throw new Exception(ERROR_COMMAND_MESSAGE);

        //    ArrayList columnSqlStringArrayList = new ArrayList();

        //    IAuxiliaryTableDataDAO systemData = BaiRongDataProvider.CreateAuxiliaryTableDataDAO();

        //    foreach (TableMetadataInfo metadataInfo in tableMetadataInfoArrayList)
        //    {
        //        string columnSql = SqlUtils.GetColumnSqlString(this.ADOType, metadataInfo.DataType, metadataInfo.AttributeName, metadataInfo.DataLength, metadataInfo.CanBeNull, metadataInfo.DBDefaultValue);
        //        if (!string.IsNullOrEmpty(columnSql))
        //        {
        //            columnSqlStringArrayList.Add(columnSql);
        //        }
        //    }

        //    EAuxiliaryTableType tableType = BaiRongDataProvider.CreateAuxiliaryTableDAO().GetAuxiliaryTableType(tableENName);

        //    createSqlString.Append(systemData.GetCreateAuxiliaryTableSqlStringFirst(tableENName, tableType));
        //    columnSqlStringArrayList[columnSqlStringArrayList.Count - 1] = ((string)columnSqlStringArrayList[columnSqlStringArrayList.Count - 1]).Replace("\t","");
        //    createSqlString.Append(ArrayListToString(columnSqlStringArrayList));
        //    createSqlString.Append("\r\n)  \r\n\r\n");

        //    alterSqlString.Append(systemData.GetDefaultMetadataAlterCommand(tableENName, tableType));

        //    return new string[]{createSqlString.ToString(), alterSqlString.ToString()};
        //}

        //protected string ArrayListToString(ArrayList collection)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    if (collection != null)
        //    {
        //        foreach (string str in collection)
        //        {
        //            builder.Append(str).Append(" ,\r\n\t");
        //        }
        //        if (builder.Length != 0) builder.Remove(builder.Length - 4, 4);
        //    }
        //    return builder.ToString();
        //}


    }
}
