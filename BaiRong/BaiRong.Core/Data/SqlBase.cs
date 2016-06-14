using System;
using System.Data;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Helper;
using BaiRong.Core.Data.Provider;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Collections;

namespace BaiRong.Core.Data
{
	public abstract class SqlBase
	{
		#region Ë½ÓÐÊý¾Ý

		private AdoHelper _helper;

		private AdoHelper Helper
		{
			get
			{
				if (this._helper == null)
				{
                    this._helper = SqlUtils.GetAdoHelper(SqlUtils.SQL_SERVER);
				}
				return this._helper;
				
			}
		}

		#endregion

        protected abstract string ConnectionString { get; }

		protected IDbConnection GetConnection()
		{
			return SqlUtils.GetIDbConnection(SqlUtils.SQL_SERVER, this.ConnectionString);
		}

        protected SqlParameter GetParameter(string parameterName, SqlDbType dataType, int value)
        {
            return this.GetParameterInner(parameterName, dataType, 0, value);
        }

        protected SqlParameter GetParameter(string parameterName, SqlDbType dataType, DateTime value)
        {
            return this.GetParameterInner(parameterName, dataType, 0, value);
        }

        protected SqlParameter GetParameter(string parameterName, SqlDbType dataType, string value)
        {
            return this.GetParameterInner(parameterName, dataType, 0, value);
        }

        protected SqlParameter GetParameter(string parameterName, SqlDbType dataType, int size, string value)
        {
            return this.GetParameterInner(parameterName, dataType, size, value);
        }

        private SqlParameter GetParameterInner(string parameterName, SqlDbType dataType, int size, object value)
        {
            if (size == 0)
            {
                SqlParameter parameter = new SqlParameter(parameterName, dataType);
                parameter.Value = value;
                return parameter;
            }
            else
            {
                SqlParameter parameter = new SqlParameter(parameterName, dataType, size);
                parameter.Value = value;
                return parameter;
            }

        }

		protected IDataReader ExecuteReader(string commandText, params IDataParameter[] commandParameters)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(this.ConnectionString, CommandType.Text, commandText, commandParameters);
            }
            return null;
		}

        protected IDataReader ExecuteReader(string commandText, CommandType commandType, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(this.ConnectionString, commandType, commandText, commandParameters);
            }
            return null;
        }


		protected IDataReader ExecuteReader(string commandText)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(this.ConnectionString, CommandType.Text, commandText);
            }
            return null;
		}


		protected IDataReader ExecuteReader(IDbConnection conn, string commandText, params IDataParameter[] commandParameters)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(conn, CommandType.Text, commandText, commandParameters);
            }
            return null;
		}

		protected IDataReader ExecuteReader(IDbConnection conn, string commandText)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(conn, CommandType.Text, commandText);
            }
            return null;
		}


		protected IDataReader ExecuteReader(IDbTransaction trans, string commandText, params IDataParameter[] commandParameters)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(trans, CommandType.Text, commandText, commandParameters);
            }
            return null;
		}


		protected IDataReader ExecuteReader(IDbTransaction trans, string commandText)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(trans, CommandType.Text, commandText);
            }
            return null;
		}


		protected IDataReader ExecuteReader(string connectionString, string commandText)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(connectionString, CommandType.Text, commandText);
            }
            return null;
		}


		protected DataSet ExecuteDataset(string commandText, params IDataParameter[] commandParameters)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteDataset(this.ConnectionString, CommandType.Text, commandText, commandParameters);
            }
            return null;
		}


		protected DataSet ExecuteDataset(string commandText)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteDataset(this.ConnectionString, CommandType.Text, commandText);
            }
            return null;
		}

        protected DataSet ExecuteDataset(string connectionString, string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteDataset(connectionString, CommandType.Text, commandText);
            }
            return null;
        }

		protected int ExecuteNonQuery(IDbConnection conn, CommandType commandType, string commandText, params IDataParameter[] commandParameters)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteNonQuery(conn, commandType, commandText, commandParameters);
            }
            return 0;
		}


		protected int ExecuteNonQuery(IDbConnection conn, string commandText, params IDataParameter[] commandParameters)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteNonQuery(conn, CommandType.Text, commandText, commandParameters);
            }
            return 0;
		}


		protected int ExecuteNonQuery(IDbConnection conn, string commandText)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteNonQuery(conn, CommandType.Text, commandText);
            }
            return 0;
		}


		protected int ExecuteNonQuery(IDbTransaction trans, string commandText, params IDataParameter[] commandParameters)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteNonQuery(trans, CommandType.Text, commandText, commandParameters);
            }
            return 0;
		}


		protected int ExecuteNonQuery(IDbTransaction trans, string commandText)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteNonQuery(trans, CommandType.Text, commandText);
            }
            return 0;
		}


		protected int ExecuteNonQuery(string commandText, params IDataParameter[] commandParameters)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, commandText, commandParameters);
            }
            return 0;
		}

		protected int ExecuteNonQuery(string commandText)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, commandText);
            }
            return 0;
		}

		protected object ExecuteScalar(IDbConnection conn, string commandText, params IDataParameter[] commandParameters)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteScalar(conn, CommandType.Text, commandText, commandParameters);
            }
            return null;
		}

		protected object ExecuteScalar(IDbConnection conn, string commandText)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteScalar(conn, CommandType.Text, commandText);
            }
            return null;
		}

		protected object ExecuteScalar(IDbTransaction trans, string commandText, params IDataParameter[] commandParameters)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteScalar(trans, CommandType.Text, commandText, commandParameters);
            }
            return null;
		}

		protected object ExecuteScalar(IDbTransaction trans, string commandText)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteScalar(trans, CommandType.Text, commandText);
            }
            return null;
		}

		protected object ExecuteScalar(string commandText, params IDataParameter[] commandParameters)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteScalar(this.ConnectionString, CommandType.Text, commandText, commandParameters);
            }
            return null;
		}

		protected object ExecuteScalar(string commandText)
		{
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteScalar(this.ConnectionString, CommandType.Text, commandText);
            }
            return null;
		}

        protected string GetInsertSqlString(NameValueCollection attributes, string tableName, out IDbDataParameter[] parms)
        {
            return BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(attributes, this.ConnectionString, tableName, out parms);
        }

        protected string GetUpdateSqlString(NameValueCollection attributes, string tableName, out IDbDataParameter[] parms)
        {
            return BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(attributes, this.ConnectionString, tableName, out parms);
        }

        protected int GetIntResult(string sqlString)
        {
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(this.ConnectionString, sqlString);
        }

        protected string GetString(string sqlString)
        {
            return BaiRongDataProvider.DatabaseDAO.GetString(this.ConnectionString, sqlString);
        }

        protected ArrayList GetIntArrayList(string sqlString)
        {
            return BaiRongDataProvider.DatabaseDAO.GetIntArrayList(sqlString);
        }

        protected ArrayList GetIntArrayList(string connectionString, string sqlString)
        {
            return BaiRongDataProvider.DatabaseDAO.GetIntArrayList(connectionString, sqlString);
        }

        protected ArrayList GetStringArrayList(string sqlString)
        {
            return BaiRongDataProvider.DatabaseDAO.GetStringArrayList(sqlString);
        }

        protected ArrayList GetStringArrayList(string connectionString, string sqlString)
        {
            return BaiRongDataProvider.DatabaseDAO.GetStringArrayList(connectionString, sqlString);
        }

        protected string GetSelectSqlString(string tableName, string columns, string where)
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, tableName, 0, columns, where, null);
        }

        protected string GetSelectSqlString(string connectionString, string tableName, int totalNum, string columns, string whereString, string orderByString)
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(connectionString, tableName, totalNum, columns, whereString, orderByString);
        }

        protected string GetSelectSqlString(string connectionString, string tableName, int startNum, int totalNum, string columns, string whereString, string orderByString)
        {
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(connectionString, tableName, startNum, totalNum, columns, whereString, orderByString);
        }

        protected void ReadResultsToNameValueCollection(IDataRecord rdr, NameValueCollection attributes)
        {
            BaiRongDataProvider.DatabaseDAO.ReadResultsToNameValueCollection(rdr, attributes);
        }

        protected void ReadResultsToExtendedAttributes(IDataRecord rdr, ExtendedAttributes attributes)
        {
            BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, attributes);
        }
	}
}
