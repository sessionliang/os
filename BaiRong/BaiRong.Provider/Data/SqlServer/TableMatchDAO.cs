using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;

namespace BaiRong.Provider.Data.SqlServer
{
	public class TableMatchDAO : DataProviderBase, ITableMatchDAO
	{
		private const string SQL_SELECT_TABLE_MATCH = "SELECT TableMatchID, ConnectionString, TableName, ConnectionStringToMatch, TableNameToMatch, ColumnsMap FROM bairong_TableMatch WHERE TableMatchID = @TableMatchID";

		private const string SQL_UPDATE_TABLE_MATCH = "UPDATE bairong_TableMatch SET ConnectionString = @ConnectionString, TableName = @TableName, ConnectionStringToMatch = @ConnectionStringToMatch, TableNameToMatch = @TableNameToMatch, ColumnsMap = @ColumnsMap WHERE TableMatchID = @TableMatchID";

		private const string SQL_DELETE_TABLE_MATCH = "DELETE FROM bairong_TableMatch WHERE TableMatchID = @TableMatchID";

		private const string PARM_TABLE_MATCH_ID = "@TableMatchID";
		private const string PARM_CONNECTION_STRING = "@ConnectionString";
		private const string PARM_TABLE_NAME = "@TableName";
		private const string PARM_CONNECTION_STRING_TO_MATCH = "@ConnectionStringToMatch";
		private const string PARM_TABLE_NAME_TO_MATCH = "@TableNameToMatch";
		private const string PARM_COLUMNS_MAP = "@ColumnsMap";		

		public int Insert(TableMatchInfo tableMatchInfo)
		{
			int tableMatchID = 0;

            string sqlString = "INSERT INTO bairong_TableMatch (ConnectionString, TableName, ConnectionStringToMatch, TableNameToMatch, ColumnsMap) VALUES (@ConnectionString, @TableName, @ConnectionStringToMatch, @TableNameToMatch, @ColumnsMap)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_TableMatch (TableMatchID, ConnectionString, TableName, ConnectionStringToMatch, TableNameToMatch, ColumnsMap) VALUES (bairong_TableMatch_SEQ.NEXTVAL, @ConnectionString, @TableName, @ConnectionStringToMatch, @TableNameToMatch, @ColumnsMap)"; 
            }

			IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CONNECTION_STRING, EDataType.VarChar, 200, tableMatchInfo.ConnectionString),
				this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 200, tableMatchInfo.TableName),
				this.GetParameter(PARM_CONNECTION_STRING_TO_MATCH, EDataType.VarChar, 200, tableMatchInfo.ConnectionStringToMatch),
				this.GetParameter(PARM_TABLE_NAME_TO_MATCH, EDataType.VarChar, 200, tableMatchInfo.TableNameToMatch),
				this.GetParameter(PARM_COLUMNS_MAP, EDataType.NText, TranslateUtils.NameValueCollectionToString(tableMatchInfo.ColumnsMap))
			};

			using (IDbConnection conn = this.GetConnection()) 
			{
				conn.Open();
				using (IDbTransaction trans = conn.BeginTransaction()) 
				{
					try 
					{
                        this.ExecuteNonQuery(trans, sqlString, insertParms);

                        tableMatchID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_TableMatch");

                        //string SELECT_CMD = "SELECT @@IDENTITY AS 'TableMatchID'";
                        //tableMatchID = Convert.ToInt32(this.ExecuteScalar(trans, SELECT_CMD));

						trans.Commit();
					}
					catch
					{
						trans.Rollback();
						throw;
					}
				}
			}

			return tableMatchID;
		}

		public void Update(TableMatchInfo tableMatchInfo)
		{
			IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CONNECTION_STRING, EDataType.VarChar, 200, tableMatchInfo.ConnectionString),
				this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 200, tableMatchInfo.TableName),
				this.GetParameter(PARM_CONNECTION_STRING_TO_MATCH, EDataType.VarChar, 200, tableMatchInfo.ConnectionStringToMatch),
				this.GetParameter(PARM_TABLE_NAME_TO_MATCH, EDataType.VarChar, 200, tableMatchInfo.TableNameToMatch),
				this.GetParameter(PARM_COLUMNS_MAP, EDataType.NText, TranslateUtils.NameValueCollectionToString(tableMatchInfo.ColumnsMap)),
				this.GetParameter(PARM_TABLE_MATCH_ID, EDataType.Integer, tableMatchInfo.TableMatchID)
			};

			using (IDbConnection conn = this.GetConnection()) 
			{
				conn.Open();
				try 
				{
					this.ExecuteNonQuery(conn, SQL_UPDATE_TABLE_MATCH, updateParms);
				}
				catch 
				{
					throw;
				}
			}
		}

		public void Delete(int tableMatchID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_MATCH_ID, EDataType.Integer, tableMatchID)
			};
							
			using (IDbConnection conn = this.GetConnection()) 
			{
				conn.Open();
				try 
				{
					this.ExecuteNonQuery(conn, SQL_DELETE_TABLE_MATCH, parms);
				}
				catch 
				{
					throw;
				}
			}
		}

		public TableMatchInfo GetTableMatchInfo(int tableMatchID)
		{
			TableMatchInfo tableMatchInfo = null;

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_MATCH_ID, EDataType.Integer, tableMatchID)
			};

			using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_MATCH, parms))
			{
				if (rdr.Read())
				{
                    tableMatchInfo = new TableMatchInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), TranslateUtils.ToNameValueCollection(rdr.GetValue(5).ToString()));
				}
				rdr.Close();
			}

			return tableMatchInfo;
		}
	}
}
