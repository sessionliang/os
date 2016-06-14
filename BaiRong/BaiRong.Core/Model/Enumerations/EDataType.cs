using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;
using BaiRong.Core;

namespace BaiRong.Model
{
	public enum EDataType
	{
        Bit,            //boolean
        Char,           //string
        DateTime,       //system.datetime
        Decimal,        //system.decimal
        Float,          //system.double
        Integer,        //int32
        NChar,          //string
        NText,          //string
        NVarChar,       //string
        Text,           //string
        VarChar,        //string
		Unknown
	}

	public class EDataTypeUtils
	{
		public static string GetValue(EDataType type)
		{
			if (type == EDataType.Bit)
			{
				return "Bit";
			}
			else if (type == EDataType.Char)
			{
				return "Char";
			}
			else if (type == EDataType.DateTime)
			{
				return "DateTime";
			}
			else if (type == EDataType.Decimal)
			{
				return "Decimal";
			}
			else if (type == EDataType.Float)
			{
				return "Float";
			}
			else if (type == EDataType.Integer)
			{
				return "Integer";
			}
			else if (type == EDataType.NChar)
			{
				return "NChar";
			}
			else if (type == EDataType.NText)
			{
				return "NText";
			}
			else if (type == EDataType.NVarChar)
			{
				return "NVarChar";
			}
			else if (type == EDataType.Text)
			{
				return "Text";
			}
			else if (type == EDataType.VarChar)
			{
				return "VarChar";
			}
			else if (type == EDataType.Unknown)
			{
				return "Unknown";
			}
			else
			{
				throw new Exception();
			}
		}

		public static EDataType GetEnumType(string typeStr)
		{
			EDataType retval = EDataType.Unknown;

			if (Equals(EDataType.Bit, typeStr))
			{
				retval = EDataType.Bit;
			}
			else if (Equals(EDataType.Char, typeStr))
			{
				retval = EDataType.Char;
			}
			else if (Equals(EDataType.DateTime, typeStr))
			{
				retval = EDataType.DateTime;
			}
			else if (Equals(EDataType.Decimal, typeStr))
			{
				retval = EDataType.Decimal;
			}
			else if (Equals(EDataType.Float, typeStr))
			{
				retval = EDataType.Float;
			}
			else if (Equals(EDataType.Integer, typeStr))
			{
				retval = EDataType.Integer;
			}
			else if (Equals(EDataType.NChar, typeStr))
			{
				retval = EDataType.NChar;
			}
			else if (Equals(EDataType.NText, typeStr))
			{
				retval = EDataType.NText;
			}
			else if (Equals(EDataType.NVarChar, typeStr))
			{
				retval = EDataType.NVarChar;
			}
			else if (Equals(EDataType.Text, typeStr))
			{
				retval = EDataType.Text;
			}
			else if (Equals(EDataType.VarChar, typeStr))
			{
				retval = EDataType.VarChar;
			}
			else if (Equals(EDataType.Unknown, typeStr))
			{
				retval = EDataType.Unknown;
			}

			return retval;
		}

		public static bool Equals(EDataType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EDataType type)
        {
            return Equals(type, typeStr);
        }

		public static EDataType FromSqlServer(string typeStr)
		{
			EDataType dataType = EDataType.Unknown;
			if (!string.IsNullOrEmpty(typeStr))
			{
				typeStr = typeStr.ToLower().Trim();
				switch (typeStr)
				{
					case "bit":
						dataType = EDataType.Bit;
						break;
					case "char":
						dataType = EDataType.Char;
						break;
					case "datetime":
						dataType = EDataType.DateTime;
						break;
					case "decimal":
						dataType = EDataType.Decimal;
						break;
					case "float":
						dataType = EDataType.Float;
						break;
					case "int":
						dataType = EDataType.Integer;
						break;
					case "nchar":
						dataType = EDataType.NChar;
						break;
					case "ntext":
						dataType = EDataType.NText;
						break;
					case "nvarchar":
						dataType = EDataType.NVarChar;
						break;
					case "text":
						dataType = EDataType.Text;
						break;
					case "varchar":
						dataType = EDataType.VarChar;
						break;
				}
			}
			return dataType;
		}

		public static EDataType FromAccess(OleDbType oleDbType)
		{
			EDataType dataType = EDataType.Unknown;
			
			switch (oleDbType)
			{
				case OleDbType.Boolean:
					dataType = EDataType.Bit;
					break;
				case OleDbType.BSTR:
					dataType = EDataType.VarChar;
					break;
				case OleDbType.Char:
					dataType = EDataType.Char;
					break;
				case OleDbType.Date:
					dataType = EDataType.DateTime;
					break;
				case OleDbType.DBDate:
					dataType = EDataType.DateTime;
					break;
				case OleDbType.Decimal:
					dataType = EDataType.Decimal;
					break;
				case OleDbType.Filetime:
					dataType = EDataType.DateTime;
					break;
				case OleDbType.Integer:
					dataType = EDataType.Integer;
					break;
				case OleDbType.LongVarChar:
					dataType = EDataType.Text;
					break;
				case OleDbType.LongVarWChar:
					dataType = EDataType.NVarChar;
					break;
				case OleDbType.Single:
					dataType = EDataType.Float;
					break;
				case OleDbType.UnsignedInt:
					dataType = EDataType.Integer;
					break;
				case OleDbType.VarChar:
					dataType = EDataType.VarChar;
					break;
				case OleDbType.VarWChar:
					dataType = EDataType.NVarChar;
					break;
				case OleDbType.WChar:
					dataType = EDataType.NChar;
					break;
			}

			return dataType;
		}

        public static EDataType FromOracle(string typeStr)
        {
            EDataType dataType = EDataType.Unknown;

            if (!string.IsNullOrEmpty(typeStr))
            {
                typeStr = typeStr.ToUpper().Trim();
                switch (typeStr)
                {
                    case "CHAR":
                        dataType = EDataType.Char;
                        break;
                    case "TIMESTAMP(6)":
                        dataType = EDataType.DateTime;
                        break;
                    case "TIMESTAMP(8)":
                        dataType = EDataType.DateTime;
                        break;
                    case "NUMBER":
                        dataType = EDataType.Integer;
                        break;
                    case "NCHAR":
                        dataType = EDataType.NChar;
                        break;
                    case "NCLOB":
                        dataType = EDataType.NText;
                        break;
                    case "NVARCHAR2":
                        dataType = EDataType.NVarChar;
                        break;
                    case "CLOB":
                        dataType = EDataType.Text;
                        break;
                    case "VARCHAR2":
                        dataType = EDataType.VarChar;
                        break;
                }
            }

            return dataType;
        }

		public static SqlDbType ToSqlDbType(EDataType type)
		{
			if (type == EDataType.Bit)
			{
				return SqlDbType.Bit;
			}
			else if (type == EDataType.Char)
			{
				return SqlDbType.Char;
			}
			else if (type == EDataType.DateTime)
			{
				return SqlDbType.DateTime;
			}
			else if (type == EDataType.Decimal)
			{
				return SqlDbType.Decimal;
			}
			else if (type == EDataType.Float)
			{
				return SqlDbType.Float;
			}
			else if (type == EDataType.Integer)
			{
				return SqlDbType.Int;
			}
			else if (type == EDataType.NChar)
			{
				return SqlDbType.NChar;
			}
			else if (type == EDataType.NText)
			{
				return SqlDbType.NText;
			}
			else if (type == EDataType.NVarChar)
			{
				return SqlDbType.NVarChar;
			}
			else if (type == EDataType.Text)
			{
				return SqlDbType.Text;
			}
			else if (type == EDataType.VarChar)
			{
				return SqlDbType.VarChar;
			}
			else if (type == EDataType.Unknown)//未知类型转换为VarChar类型
			{
				return SqlDbType.VarChar;
			}
			else
			{
				throw new Exception();
			}
		}

        public static OracleType ToOracleDbType(EDataType type)
        {
            if (type == EDataType.Char)
            {
                return OracleType.Char;
            }
            else if (type == EDataType.DateTime)
            {
                return OracleType.Timestamp;
            }
            else if (type == EDataType.Decimal)
            {
                return OracleType.Number;
            }
            else if (type == EDataType.Float)
            {
                return OracleType.Float;
            }
            else if (type == EDataType.Integer)
            {
                return OracleType.Number;
            }
            else if (type == EDataType.NChar)
            {
                return OracleType.NChar;
            }
            else if (type == EDataType.NText)
            {
                return OracleType.NClob;
            }
            else if (type == EDataType.NVarChar)
            {
                return OracleType.NVarChar;
            }
            else if (type == EDataType.Text)
            {
                return OracleType.Clob;
            }
            else if (type == EDataType.VarChar)
            {
                return OracleType.VarChar;
            }
            else if (type == EDataType.Unknown)//未知类型转换为VarChar类型
            {
                return OracleType.VarChar;
            }
            else
            {
                throw new Exception();
            }
        }

		public static OleDbType ToOleDbType(EDataType type)
		{		
			if (type == EDataType.Bit)
			{
				return OleDbType.Boolean;
			}
			else if (type == EDataType.Char)
			{
				return OleDbType.Char;
			}
			else if (type == EDataType.DateTime)
			{
				return OleDbType.Date;
			}
			else if (type == EDataType.Decimal)
			{
				return OleDbType.Decimal;
			}
			else if (type == EDataType.Float)
			{
				return OleDbType.Single;
			}
			else if (type == EDataType.Integer)
			{
				return OleDbType.Integer;
			}
			else if (type == EDataType.NChar)
			{
				return OleDbType.WChar;
			}
			else if (type == EDataType.NText)
			{
				return OleDbType.LongVarWChar;
			}
			else if (type == EDataType.NVarChar)
			{
				return OleDbType.VarWChar;
			}
			else if (type == EDataType.Text)
			{
				return OleDbType.LongVarChar;
			}
			else if (type == EDataType.VarChar)
			{
				return OleDbType.VarChar;
			}
			else if (type == EDataType.Unknown)//未知类型转换为VarChar类型
			{
				return OleDbType.VarChar;
			}
			else
			{
				throw new Exception();
			}
		}

		public static ListItem GetListItem(EDataType type, string text)
		{
			ListItem item = new ListItem(text, GetValue(type));
			return item;
		}

		public static void AddListItemsToAuxiliaryTable(ListControl listControl, bool isSqlServer)
		{
            if (listControl != null)
            {
                if (isSqlServer)
                {
                    listControl.Items.Add(GetListItem(EDataType.NVarChar, "文本"));
                    listControl.Items.Add(GetListItem(EDataType.NText, "备注"));
                    listControl.Items.Add(GetListItem(EDataType.Integer, "数字"));
                    listControl.Items.Add(GetListItem(EDataType.DateTime, "日期/时间"));
                }
                else
                {
                    listControl.Items.Add(GetListItem(EDataType.NChar, "文本"));
                    listControl.Items.Add(GetListItem(EDataType.NText, "备注"));
                    listControl.Items.Add(GetListItem(EDataType.Integer, "数字"));
                    listControl.Items.Add(GetListItem(EDataType.DateTime, "日期/时间"));
                }
            }
		}

        public static string GetTextByAuxiliaryTable(EDataType dataType, int dataLength)
        {
            string retval = string.Empty;
            if (dataType == EDataType.NVarChar)
            {
                retval = string.Format("文本({0})", dataLength);
            }
            else if (dataType == EDataType.VarChar)
            {
                retval = string.Format("文本({0})", dataLength);
            }
            else if (dataType == EDataType.NChar)
            {
                retval = string.Format("文本({0})", dataLength);
            }
            else if (dataType == EDataType.NText)
            {
                retval = "备注";
            }
            else if (dataType == EDataType.Text)
            {
                retval = "备注";
            }
            else if (dataType == EDataType.Integer)
            {
                retval = "数字";
            }
            else if (dataType == EDataType.DateTime)
            {
                retval = "日期/时间";
            }
            else if (dataType == EDataType.Decimal)
            {
                retval = "小数";
            }
            return retval;
        }

        public static string GetDefaultString(EDatabaseType databaseType, string stringValue)
        {
            string retval = string.Empty;
            if (databaseType == EDatabaseType.SqlServer)
            {
                return string.Format("'{0}'", stringValue);
            }
            return retval;
        }

		public static string GetDefaultString(EDatabaseType databaseType, EDataType dataType)
		{
			string retval = string.Empty;
			if (databaseType == EDatabaseType.SqlServer)
			{
				if (dataType == EDataType.Char || dataType == EDataType.NChar || dataType == EDataType.NText || dataType == EDataType.NVarChar || dataType == EDataType.Text || dataType == EDataType.VarChar)
				{
					return "''";
				}
				else if (dataType == EDataType.Decimal || dataType == EDataType.Decimal || dataType == EDataType.Float || dataType == EDataType.Integer)
				{
					return "0";
				}
				else if (dataType == EDataType.DateTime)
				{
					return "getdate()";
				}
			}
            else if (databaseType == EDatabaseType.Oracle)
            {
                if (dataType == EDataType.Char || dataType == EDataType.NChar || dataType == EDataType.NText || dataType == EDataType.NVarChar || dataType == EDataType.Text || dataType == EDataType.VarChar)
                {
                    return "''";
                }
                else if (dataType == EDataType.Decimal || dataType == EDataType.Decimal || dataType == EDataType.Float || dataType == EDataType.Integer)
                {
                    return "0";
                }
                else if (dataType == EDataType.DateTime)
                {
                    return "sysdate";
                }
            }
			return retval;
		}
	}
}
