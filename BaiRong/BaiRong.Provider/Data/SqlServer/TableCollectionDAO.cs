using System;
using System.Data;
using System.Collections;
using System.Text;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;

namespace BaiRong.Provider.Data.SqlServer
{
    public class TableCollectionDAO : DataProviderBase, ITableCollectionDAO
	{
		// Static constants
        private const string SQL_SELECT_TABLE = "SELECT TableENName, TableCNName, AttributeNum, AuxiliaryTableType, IsCreatedInDB, IsChangedAfterCreatedInDB, IsDefault, Description FROM bairong_TableCollection WHERE TableENName = @TableENName";
		private const string SQL_SELECT_TABLE_TYPE = "SELECT AuxiliaryTableType FROM bairong_TableCollection WHERE TableENName = @TableENName";
        private const string SQL_SELECT_TABLE_CNNAME = "SELECT TableCNName FROM bairong_TableCollection WHERE TableENName = @TableENName";
        private const string SQL_SELECT_ALL_TABLE = "SELECT TableENName, TableCNName, AttributeNum, AuxiliaryTableType, IsCreatedInDB, IsChangedAfterCreatedInDB, IsDefault, Description FROM bairong_TableCollection ORDER BY AuxiliaryTableType DESC, IsCreatedInDB DESC, TableENName";
        private const string SQL_SELECT_ALL_TABLE_BY_AUXILIARY_TYPE = "SELECT TableENName, TableCNName, AttributeNum, AuxiliaryTableType, IsCreatedInDB, IsChangedAfterCreatedInDB, IsDefault, Description FROM bairong_TableCollection WHERE AuxiliaryTableType = @AuxiliaryTableType ORDER BY IsCreatedInDB DESC, TableENName";
        private const string SQL_SELECT_ALL_TABLE_CREATED_IN_DB_BY_AUXILIARY_TYPE = "SELECT TableENName, TableCNName, AttributeNum, AuxiliaryTableType, IsCreatedInDB, IsChangedAfterCreatedInDB, IsDefault, Description FROM bairong_TableCollection WHERE AuxiliaryTableType = @AuxiliaryTableType AND IsCreatedInDB = @IsCreatedInDB ORDER BY IsCreatedInDB DESC, TableENName";
        private const string SQL_SELECT_ALL_TABLE_CREATED_IN_DB = "SELECT TableENName, TableCNName, AttributeNum, AuxiliaryTableType, IsCreatedInDB, IsChangedAfterCreatedInDB, IsDefault, Description FROM bairong_TableCollection WHERE IsCreatedInDB = @IsCreatedInDB ORDER BY IsCreatedInDB DESC, TableENName";
		private const string SQL_SELECT_TABLE_COUNT = "SELECT COUNT(*) FROM bairong_TableCollection";
		private const string SQL_SELECT_TABLE_ENNAME = "SELECT TableENName FROM bairong_TableCollection";

        private const string SQL_INSERT_TABLE = "INSERT INTO bairong_TableCollection (TableENName, TableCNName, AttributeNum, AuxiliaryTableType, IsCreatedInDB, IsChangedAfterCreatedInDB, IsDefault, Description) VALUES (@TableENName, @TableCNName, @AttributeNum, @AuxiliaryTableType, @IsCreatedInDB, @IsChangedAfterCreatedInDB, @IsDefault, @Description)";
        private const string SQL_UPDATE_TABLE = "UPDATE bairong_TableCollection SET TableCNName = @TableCNName, AttributeNum = @AttributeNum, AuxiliaryTableType = @AuxiliaryTableType, IsCreatedInDB = @IsCreatedInDB, IsChangedAfterCreatedInDB = @IsChangedAfterCreatedInDB, IsDefault = @IsDefault, Description = @Description WHERE  TableENName = @TableENName";
		private const string SQL_UPDATE_TABLE_ATTRIBUTE_NUM = "UPDATE bairong_TableCollection SET AttributeNum = @AttributeNum WHERE  TableENName = @TableENName";
		private const string SQL_UPDATE_TABLE_IS_CREATED_IN_DB = "UPDATE bairong_TableCollection SET IsCreatedInDB = @IsCreatedInDB WHERE  TableENName = @TableENName";
		private const string SQL_UPDATE_TABLE_IS_CHANGED_AFTER_CREATED_IN_DB = "UPDATE bairong_TableCollection SET IsChangedAfterCreatedInDB = @IsChangedAfterCreatedInDB WHERE  TableENName = @TableENName";
		private const string SQL_DELETE_TABLE = "DELETE FROM bairong_TableCollection WHERE TableENName = @TableENName";

		private const string PARM_TABLE_ENNAME = "@TableENName";
		private const string PARM_TABLE_CNNAME = "@TableCNName";
		private const string PARM_ATTRIBUTE_NUM = "@AttributeNum";
		private const string PARM_TABLE_TYPE = "@AuxiliaryTableType";
		private const string PARM_IS_CREATED_IN_DB = "@IsCreatedInDB";
		private const string PARM_IS_CHANGED_AFTER_CREATED_IN_DB = "@IsChangedAfterCreatedInDB";
        private const string PARM_IS_DEFAULT = "@IsDefault";
		private const string PARM_DESCRIPTION = "@Description";

		public void Insert(AuxiliaryTableInfo info) 
		{
			IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_ENNAME, EDataType.VarChar, 50, info.TableENName),
				this.GetParameter(PARM_TABLE_CNNAME, EDataType.NVarChar, 50, info.TableCNName),
				this.GetParameter(PARM_ATTRIBUTE_NUM, EDataType.Integer, info.AttributeNum),
				this.GetParameter(PARM_TABLE_TYPE, EDataType.VarChar, 50, EAuxiliaryTableTypeUtils.GetValue(info.AuxiliaryTableType)),
				this.GetParameter(PARM_IS_CREATED_IN_DB, EDataType.VarChar, 18, false.ToString()),
				this.GetParameter(PARM_IS_CHANGED_AFTER_CREATED_IN_DB, EDataType.VarChar, 18, false.ToString()),
                this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, info.IsDefault.ToString()),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NText, info.Description)
			};
							
			using (IDbConnection conn = this.GetConnection()) 
			{
				conn.Open();
				using (IDbTransaction trans = conn.BeginTransaction()) 
				{
					try 
					{
						this.ExecuteNonQuery(trans, SQL_INSERT_TABLE, insertParms);
                        BaiRongDataProvider.TableMetadataDAO.InsertSystemItems(info.TableENName, info.AuxiliaryTableType, trans);
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

		public void Update(AuxiliaryTableInfo info) 
		{
			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_CNNAME, EDataType.NVarChar, 50, info.TableCNName),
				this.GetParameter(PARM_ATTRIBUTE_NUM, EDataType.Integer, info.AttributeNum),
				this.GetParameter(PARM_TABLE_TYPE, EDataType.VarChar, 50, EAuxiliaryTableTypeUtils.GetValue(info.AuxiliaryTableType)),
				this.GetParameter(PARM_IS_CREATED_IN_DB, EDataType.VarChar, 18, info.IsCreatedInDB.ToString()),
				this.GetParameter(PARM_IS_CHANGED_AFTER_CREATED_IN_DB, EDataType.VarChar, 18, info.IsChangedAfterCreatedInDB.ToString()),
                this.GetParameter(PARM_IS_DEFAULT, EDataType.VarChar, 18, info.IsDefault.ToString()),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NText, info.Description),
				this.GetParameter(PARM_TABLE_ENNAME, EDataType.VarChar, 50, info.TableENName)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TABLE, updateParms);
		}

		public void UpdateAttributeNum(string tableENName)
		{

            int fieldNum = BaiRongDataProvider.TableMetadataDAO.GetTableMetadataCountByENName(tableENName);
			this.UpdateAttributeNum(fieldNum, tableENName);
		}

		private void UpdateAttributeNum(int attributeNum, string tableENName)
		{

			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ATTRIBUTE_NUM, EDataType.Integer, attributeNum),
				this.GetParameter(PARM_TABLE_ENNAME, EDataType.VarChar, 50, tableENName)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TABLE_ATTRIBUTE_NUM, updateParms);
		}

		public void UpdateIsCreatedInDB(bool isCreatedInDB, string tableENName)
		{
			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_CREATED_IN_DB, EDataType.VarChar, 18, isCreatedInDB.ToString()),
				this.GetParameter(PARM_TABLE_ENNAME, EDataType.VarChar, 50, tableENName)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TABLE_IS_CREATED_IN_DB, updateParms);
            TableManager.IsChanged = true;
		}

        public void UpdateIsChangedAfterCreatedInDB(bool isChangedAfterCreatedInDB, string tableENName)
		{
			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_IS_CHANGED_AFTER_CREATED_IN_DB, EDataType.VarChar, 18, isChangedAfterCreatedInDB.ToString()),
				this.GetParameter(PARM_TABLE_ENNAME, EDataType.VarChar, 50, tableENName)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TABLE_IS_CHANGED_AFTER_CREATED_IN_DB, updateParms);
            TableManager.IsChanged = true;
		}

		public void Delete(string tableENName)
		{
            bool isTableExists = BaiRongDataProvider.TableStructureDAO.IsTableExists(tableENName);
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_ENNAME, EDataType.VarChar, 50, tableENName),
			};
							
			using (IDbConnection conn = this.GetConnection()) 
			{
				conn.Open();
				using (IDbTransaction trans = conn.BeginTransaction()) 
				{
					try 
					{
						if (isTableExists)
						{
							this.ExecuteNonQuery(trans, string.Format("DROP TABLE [{0}]", tableENName));
						}
						
						this.ExecuteNonQuery(trans, SQL_DELETE_TABLE, parms);
                        BaiRongDataProvider.TableMetadataDAO.Delete(tableENName, trans);
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

		public AuxiliaryTableInfo GetAuxiliaryTableInfo(string tableENName)
		{
			AuxiliaryTableInfo info = null;
			
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_ENNAME, EDataType.VarChar, 50, tableENName)
			};
			
			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE, parms)) 
			{
				if (rdr.Read()) 
				{
                    info = new AuxiliaryTableInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetInt32(2), EAuxiliaryTableTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetValue(7).ToString());
				}
				rdr.Close();
			}

			return info;
		}

		public EAuxiliaryTableType GetTableType(string tableENName)
		{
			EAuxiliaryTableType type = EAuxiliaryTableType.BackgroundContent;
			
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_ENNAME, EDataType.VarChar, 50, tableENName),
			};
			
			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_TYPE, parms)) 
			{
				if (rdr.Read()) 
				{
                    type = EAuxiliaryTableTypeUtils.GetEnumType(rdr.GetValue(0).ToString());
				}
				rdr.Close();
			}

			return type;
		}

        public string GetTableCNName(string tableENName)
        {
            string tableCNName = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_ENNAME, EDataType.VarChar, 50, tableENName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_CNNAME, parms))
            {
                if (rdr.Read())
                {
                    tableCNName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return tableCNName;
        }

		public IEnumerable GetDataSourceByAuxiliaryTableType()
		{
            string SQL_SELECT = string.Format("SELECT TableENName, TableCNName, AttributeNum, AuxiliaryTableType, IsCreatedInDB, IsChangedAfterCreatedInDB, IsDefault, Description FROM bairong_TableCollection ORDER BY IsCreatedInDB DESC, TableENName");

			IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT);
			return enumerable;
		}

		public IEnumerable GetDataSourceCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType type)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_TYPE, EDataType.VarChar, 50, EAuxiliaryTableTypeUtils.GetValue(type)),
				this.GetParameter(PARM_IS_CREATED_IN_DB, EDataType.VarChar, 18, true.ToString())
			};

			IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_TABLE_CREATED_IN_DB_BY_AUXILIARY_TYPE, parms);
			return enumerable;
		}

		/// <summary>
		/// Get Total AuxiliaryTable Count
		/// </summary>
		public int GetAuxiliaryTableCount()
		{
            return Convert.ToInt32(this.ExecuteScalar(SQL_SELECT_TABLE_COUNT));
		}

		public ArrayList GetTableENNameCollection()
		{
			ArrayList list = new ArrayList();
			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_ENNAME)) 
			{
				while (rdr.Read()) 
				{
                    list.Add(rdr.GetValue(0).ToString());
				}
				rdr.Close();
			}

			return list;
		}

        public ArrayList GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(params EAuxiliaryTableType[] eAuxiliaryTableTypeArray)
		{
			ArrayList arraylist = new ArrayList();
            foreach (EAuxiliaryTableType eAuxiliaryTableType in eAuxiliaryTableTypeArray)
            {
                arraylist.Add(EAuxiliaryTableTypeUtils.GetValue(eAuxiliaryTableType));
            }

            string sqlString = string.Format("SELECT TableENName, TableCNName, AttributeNum, AuxiliaryTableType, IsCreatedInDB, IsChangedAfterCreatedInDB, IsDefault, Description FROM bairong_TableCollection WHERE AuxiliaryTableType IN ({0}) AND IsCreatedInDB = '{1}' ORDER BY IsCreatedInDB DESC, TableENName", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(arraylist), true.ToString());

            arraylist.Clear();

            using (IDataReader rdr = this.ExecuteReader(sqlString)) 
			{
				while (rdr.Read()) 
				{
                    AuxiliaryTableInfo info = new AuxiliaryTableInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetInt32(2), EAuxiliaryTableTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetValue(7).ToString());
					arraylist.Add(info);
				}
				rdr.Close();
			}

			return arraylist;
		}

        public ArrayList GetAuxiliaryTableArrayListCreatedInDB()
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT TableENName, TableCNName, AttributeNum, AuxiliaryTableType, IsCreatedInDB, IsChangedAfterCreatedInDB, IsDefault, Description FROM bairong_TableCollection WHERE IsCreatedInDB = '{0}' ORDER BY IsCreatedInDB DESC, TableENName", true.ToString());

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    AuxiliaryTableInfo info = new AuxiliaryTableInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetInt32(2), EAuxiliaryTableTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), rdr.GetValue(7).ToString());
                    arraylist.Add(info);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetTableENNameCollectionCreatedInDB()
        {
            ArrayList list = new ArrayList();

            string sqlString = string.Format("SELECT TableENName FROM bairong_TableCollection WHERE IsCreatedInDB = '{0}'", true.ToString());

            using (IDataReader rdr = this.ExecuteReader(sqlString))
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
		/// 获取表的使用次数，不抛出错误
		/// </summary>
		/// <param name="tableENName"></param>
		/// <param name="tableType"></param>
		/// <returns></returns>
		public int GetTableUsedNum(string tableENName, EAuxiliaryTableType tableType)
		{
			int count = 0;

            if (tableType == EAuxiliaryTableType.BackgroundContent)
            {
                string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_PublishmentSystem WHERE (AuxiliaryTableForContent = '{0}')", PageUtils.FilterSql(tableENName));
                try
                {
                    count += BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
                }
                catch { }
            }
            else if (tableType == EAuxiliaryTableType.GoodsContent)
            {
                string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_PublishmentSystem WHERE (AuxiliaryTableForGoods = '{0}')", PageUtils.FilterSql(tableENName));
                try
                {
                    count += BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
                }
                catch { }
            }
            else if (tableType == EAuxiliaryTableType.BrandContent)
            {
                string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_PublishmentSystem WHERE (AuxiliaryTableForBrand = '{0}')", PageUtils.FilterSql(tableENName));
                try
                {
                    count += BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
                }
                catch { }
            }
            else if (tableType == EAuxiliaryTableType.GovPublicContent)
            {
                string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_PublishmentSystem WHERE (AuxiliaryTableForGovPublic = '{0}')", PageUtils.FilterSql(tableENName));
                try
                {
                    count += BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
                }
                catch { }
            }
            else if (tableType == EAuxiliaryTableType.GovInteractContent)
            {
                string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_PublishmentSystem WHERE (AuxiliaryTableForGovInteract = '{0}')", PageUtils.FilterSql(tableENName));
                try
                {
                    count += BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
                }
                catch { }
            }
            else if (tableType == EAuxiliaryTableType.VoteContent)
            {
                string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_PublishmentSystem WHERE (AuxiliaryTableForVote = '{0}')", PageUtils.FilterSql(tableENName));
                try
                {
                    count += BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
                }
                catch { }
            }
            else if (tableType == EAuxiliaryTableType.JobContent)
            {
                string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_PublishmentSystem WHERE (AuxiliaryTableForJob = '{0}')", PageUtils.FilterSql(tableENName));
                try
                {
                    count += BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
                }
                catch { }
            }

            string sqlString2 = string.Format("SELECT COUNT(*) FROM bairong_ContentModel WHERE (TableName = '{0}')", PageUtils.FilterSql(tableENName));
            try
            {
                count += BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString2);
            }
            catch { }

			return count;
		}

        public bool IsTableExists(EAuxiliaryTableType tableType)
        {
            bool isExists = false;

            string sqlString = string.Format("SELECT TableENName FROM bairong_TableCollection WHERE AuxiliaryTableType = '{0}' AND IsCreatedInDB = 'True'", EAuxiliaryTableTypeUtils.GetValue(tableType));
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    isExists = true;
                }
                rdr.Close();
            }

            return isExists;
        }

        public bool IsTableExists(string tableName)
        {
            bool isExists = false;

            string sqlString = string.Format("SELECT TableENName FROM bairong_TableCollection WHERE TableENName = '{0}' AND IsCreatedInDB = 'True'", PageUtils.FilterSql(tableName));
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    isExists = true;
                }
                rdr.Close();
            }

            return isExists;
        }

        public string GetFirstTableNameByTableType(EAuxiliaryTableType tableType)
        {
            string tableName = string.Empty;

            string sqlString = string.Format("SELECT TableENName FROM bairong_TableCollection WHERE AuxiliaryTableType = '{0}' AND IsCreatedInDB = 'True'", EAuxiliaryTableTypeUtils.GetValue(tableType));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    tableName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return tableName;
        }

        public void CreateAllAuxiliaryTableIfNotExists()
        {
            //添加后台内容表
            if (!IsTableExists(EAuxiliaryTableType.BackgroundContent))
            {
                string tableName = EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.BackgroundContent);
                AuxiliaryTableInfo tableInfo = new AuxiliaryTableInfo(tableName, "后台内容表", 0, EAuxiliaryTableType.BackgroundContent, false, false, true, string.Empty);
                BaiRongDataProvider.TableCollectionDAO.Insert(tableInfo);
                BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTable(tableInfo.TableENName);
            }

            //添加招聘内容表
            if (!IsTableExists(EAuxiliaryTableType.JobContent))
            {
                string tableName = EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.JobContent);
                AuxiliaryTableInfo tableInfo = new AuxiliaryTableInfo(tableName, "招聘内容表", 0, EAuxiliaryTableType.JobContent, false, false, true, string.Empty);
                BaiRongDataProvider.TableCollectionDAO.Insert(tableInfo);
                BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTable(tableInfo.TableENName);
            }

            //添加投票内容表
            if (!IsTableExists(EAuxiliaryTableType.VoteContent))
            {
                string tableName = EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.VoteContent);
                AuxiliaryTableInfo tableInfo = new AuxiliaryTableInfo(tableName, "投票内容表", 0, EAuxiliaryTableType.VoteContent, false, false, true, string.Empty);
                BaiRongDataProvider.TableCollectionDAO.Insert(tableInfo);
                BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTable(tableInfo.TableENName);
            }

            //添加自定义内容表
            if (!IsTableExists(EAuxiliaryTableType.UserDefined))
            {
                string tableName = EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.UserDefined);
                AuxiliaryTableInfo tableInfo = new AuxiliaryTableInfo(tableName, "自定义内容表", 0, EAuxiliaryTableType.UserDefined, false, false, true, string.Empty);
                BaiRongDataProvider.TableCollectionDAO.Insert(tableInfo);
                BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTable(tableInfo.TableENName);
            }

            #region B2C

            //添加商品内容表
            if (!IsTableExists(EAuxiliaryTableType.GoodsContent))
            {
                string tableName = EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.GoodsContent);
                AuxiliaryTableInfo tableInfo = new AuxiliaryTableInfo(tableName, "商品内容表", 0, EAuxiliaryTableType.GoodsContent, false, false, true, string.Empty);
                BaiRongDataProvider.TableCollectionDAO.Insert(tableInfo);
                BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTable(tableInfo.TableENName);
            }

            //添加品牌内容表
            if (!IsTableExists(EAuxiliaryTableType.BrandContent))
            {
                string tableName = EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.BrandContent);
                AuxiliaryTableInfo tableInfo = new AuxiliaryTableInfo(tableName, "品牌内容表", 0, EAuxiliaryTableType.BrandContent, false, false, true, string.Empty);
                BaiRongDataProvider.TableCollectionDAO.Insert(tableInfo);
                BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTable(tableInfo.TableENName);
            }

            #endregion

            #region WCM

            //添加信息公开内容表
            if (!IsTableExists(EAuxiliaryTableType.GovPublicContent))
            {
                string tableName = EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.GovPublicContent);
                AuxiliaryTableInfo tableInfo = new AuxiliaryTableInfo(tableName, "信息公开内容表", 0, EAuxiliaryTableType.GovPublicContent, false, false, true, string.Empty);
                BaiRongDataProvider.TableCollectionDAO.Insert(tableInfo);
                BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTable(tableInfo.TableENName);
            }

            //添加互动交流内容表
            if (!IsTableExists(EAuxiliaryTableType.GovInteractContent))
            {
                string tableName = EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.GovInteractContent);
                AuxiliaryTableInfo tableInfo = new AuxiliaryTableInfo(tableName, "互动交流内容表", 0, EAuxiliaryTableType.GovInteractContent, false, false, true, string.Empty);
                BaiRongDataProvider.TableCollectionDAO.Insert(tableInfo);
                BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTable(tableInfo.TableENName);
            }

            #endregion
        }
	}
}
