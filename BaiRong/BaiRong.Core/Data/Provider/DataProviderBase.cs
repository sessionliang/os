using System;
using System.Data;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Helper;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace BaiRong.Core.Data.Provider
{
    public class DataProviderBase
    {
        #region Ë½ÓÐÊý¾Ý

        private AdoHelper _helper;

        private AdoHelper Helper
        {
            get
            {
                if (this._helper == null)
                {
                    this._helper = SqlUtils.GetAdoHelper(this.ADOType);
                }
                return this._helper;

            }
        }

        #endregion

        protected virtual string ADOType
        {
            get
            {
                return SqlUtils.SQL_SERVER;
            }
        }

        protected virtual EDatabaseType DataBaseType
        {
            get
            {
                return EDatabaseType.SqlServer;
            }
        }

        protected virtual string ConnectionString
        {
            get
            {
                return BaiRongDataProvider.ConnectionString;
            }
        }

        protected IDbConnection GetConnection()
        {
            return SqlUtils.GetIDbConnection(this.ADOType, this.ConnectionString);
        }

        protected IDbConnection GetConnection(string connectionString)
        {
            return SqlUtils.GetIDbConnection(this.ADOType, connectionString);
        }

        protected IDbDataParameter GetParameter(string parameterName, EDataType dataType, int value)
        {
            return this.GetParameterInner(parameterName, dataType, 0, value);
        }

        protected IDbDataParameter GetParameter(string parameterName, EDataType dataType, DateTime value)
        {
            return this.GetParameterInner(parameterName, dataType, 0, value);
        }

        protected IDbDataParameter GetParameter(string parameterName, EDataType dataType, string value)
        {
            return this.GetParameterInner(parameterName, dataType, 0, value);
        }

        protected IDbDataParameter GetParameter(string parameterName, EDataType dataType, int size, string value)
        {
            return this.GetParameterInner(parameterName, dataType, size, value);
        }

        protected IDbDataParameter GetParameter(string parameterName, EDataType dataType, int size, decimal value)
        {
            return this.GetParameterInner(parameterName, dataType, size, value);
        }

        protected List<IDbDataParameter> GetINParameterList(string parameterName, EDataType dataType, int dataLength, ICollection valueCollection, out string parameterNameList)
        {
            parameterNameList = string.Empty;
            List<IDbDataParameter> parameterList = new List<IDbDataParameter>();

            if (valueCollection != null && valueCollection.Count > 0)
            {
                StringBuilder sbCondition = new StringBuilder();
                int i = 0;
                foreach (Object obj in valueCollection)
                {
                    i++;

                    string value = obj.ToString();
                    string parmName = parameterName + "_" + i;

                    sbCondition.Append(parmName + ",");

                    if (dataType == EDataType.Integer)
                    {
                        parameterList.Add(this.GetParameter(parmName, dataType, value));
                    }
                    else
                    {
                        parameterList.Add(this.GetParameter(parmName, dataType, dataLength, value));
                    }
                }

                parameterNameList = sbCondition.ToString().TrimEnd(',');
            }

            return parameterList;
        }

        private IDbDataParameter GetParameterInner(string parameterName, EDataType dataType, int size, object value)
        {
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                //parameterName = SqlUtils.ParseSyntax(parameterName);
                if ((dataType == EDataType.NText || dataType == EDataType.Text) && string.IsNullOrEmpty(value as string))
                {
                    value = DBNull.Value;
                }
            }
            if (size == 0)
            {
                IDbDataParameter parameter = SqlUtils.GetIDbDataParameter(this.ADOType, parameterName, dataType);
                parameter.Value = value;
                return parameter;
            }
            else
            {
                IDbDataParameter parameter = SqlUtils.GetIDbDataParameter(this.ADOType, parameterName, dataType, size);
                parameter.Value = value;
                return parameter;
            }
        }


        protected IDataReader ExecuteReader(string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteReader(this.ConnectionString, CommandType.Text, commandText, commandParameters);
            }
            return null;
        }


        protected IDataReader ExecuteReader(string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(this.ConnectionString, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return null;
        }


        protected IDataReader ExecuteReader(IDbConnection conn, string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteReader(conn, CommandType.Text, commandText, commandParameters);
            }
            return null;
        }


        protected IDataReader ExecuteReader(IDbConnection conn, string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(conn, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return null;
        }


        protected IDataReader ExecuteReader(IDbTransaction trans, string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteReader(trans, CommandType.Text, commandText, commandParameters);
            }
            return null;
        }


        protected IDataReader ExecuteReader(IDbTransaction trans, string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(trans, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return null;
        }


        protected IDataReader ExecuteReader(string connectionString, string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(connectionString, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return null;
        }

        protected IDataReader ExecuteReader(string connectionString, string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteReader(connectionString, CommandType.Text, SqlUtils.ParseSqlString(commandText), commandParameters);
            }
            return null;
        }


        protected DataSet ExecuteDataset(string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteDataset(this.ConnectionString, CommandType.Text, commandText, commandParameters);
            }
            return null;
        }


        protected DataSet ExecuteDataset(string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteDataset(this.ConnectionString, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return null;
        }

        protected DataSet ExecuteDataset(string connectionString, string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteDataset(connectionString, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return null;
        }

        protected int ExecuteNonQuery(IDbConnection conn, CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteNonQuery(conn, commandType, commandText, commandParameters);
            }
            return 0;
        }


        protected int ExecuteNonQuery(IDbConnection conn, string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteNonQuery(conn, CommandType.Text, commandText, commandParameters);
            }
            return 0;
        }


        protected int ExecuteNonQuery(IDbConnection conn, string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteNonQuery(conn, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return 0;
        }


        protected int ExecuteNonQuery(IDbTransaction trans, string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteNonQuery(trans, CommandType.Text, commandText, commandParameters);
            }
            return 0;
        }


        protected int ExecuteNonQuery(IDbTransaction trans, string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteNonQuery(trans, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return 0;
        }


        protected int ExecuteNonQuery(string connectionString, string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteNonQuery(connectionString, CommandType.Text, commandText, commandParameters);
            }
            return 0;
        }


        protected int ExecuteNonQuery(string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, commandText, commandParameters);
            }
            return 0;
        }


        protected int ExecuteNonQuery(string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return 0;
        }


        protected object ExecuteScalar(IDbConnection conn, string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteScalar(conn, CommandType.Text, commandText, commandParameters);
            }
            return null;
        }


        protected object ExecuteScalar(IDbConnection conn, string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteScalar(conn, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return null;
        }


        protected object ExecuteScalar(IDbTransaction trans, string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteScalar(trans, CommandType.Text, commandText, commandParameters);
            }
            return null;
        }


        protected object ExecuteScalar(IDbTransaction trans, string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteScalar(trans, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return null;
        }


        protected object ExecuteScalar(string commandText, params IDataParameter[] commandParameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                commandText = SqlUtils.ParseSqlString(commandText);
                return this.Helper.ExecuteScalar(this.ConnectionString, CommandType.Text, commandText, commandParameters);
            }
            return null;
        }


        protected object ExecuteScalar(string commandText)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return this.Helper.ExecuteScalar(this.ConnectionString, CommandType.Text, SqlUtils.ParseSqlString(commandText));
            }
            return null;
        }
    }
}
