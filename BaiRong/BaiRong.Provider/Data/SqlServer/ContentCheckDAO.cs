using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Provider.Data.SqlServer
{
	public class ContentCheckDAO : DataProviderBase, IContentCheckDAO
	{
        private const string SQL_SELECT = "SELECT CheckID, TableName, PublishmentSystemID, NodeID, ContentID, IsAdmin, UserName, IsChecked, CheckedLevel, CheckDate, Reasons FROM bairong_ContentCheck WHERE CheckID = @CheckID";

        private const string SQL_SELECT_BY_LAST_ID = "SELECT TOP 1 CheckID, TableName, PublishmentSystemID, NodeID, ContentID, IsAdmin, UserName, IsChecked, CheckedLevel, CheckDate, Reasons FROM bairong_ContentCheck WHERE TableName = @TableName AND ContentID = @ContentID ORDER BY CheckID DESC";

        private const string SQL_SELECT_ALL = "SELECT CheckID, TableName, PublishmentSystemID, NodeID, ContentID, IsAdmin, UserName, IsChecked, CheckedLevel, CheckDate, Reasons FROM bairong_ContentCheck WHERE TableName = @TableName AND ContentID = @ContentID ORDER BY CheckID DESC";

        private const string SQL_DELETE = "DELETE FROM bairong_ContentCheck WHERE CheckID = @CheckID";

        private const string PARM_CHECKID = "@CheckID";
        private const string PARM_TABLE_NAME = "@TableName";
		private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_NODEID = "@NodeID";
        private const string PARM_CONTENTID = "@ContentID";
        private const string PARM_IS_ADMIN = "@IsAdmin";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_IS_CHECKED = "@IsChecked";
        private const string PARM_CHECKED_LEVEL = "@CheckedLevel";
        private const string PARM_CHECK_DATE = "@CheckDate";
        private const string PARM_REASONS = "@Reasons";

		public void Insert(ContentCheckInfo checkInfo)
		{
            string sqlString = "INSERT INTO bairong_ContentCheck (TableName, PublishmentSystemID, NodeID, ContentID, IsAdmin, UserName, IsChecked, CheckedLevel, CheckDate, Reasons) VALUES (@TableName, @PublishmentSystemID, @NodeID, @ContentID, @IsAdmin, @UserName, @IsChecked, @CheckedLevel, @CheckDate, @Reasons)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_ContentCheck (CheckID, TableName, PublishmentSystemID, NodeID, ContentID, IsAdmin, UserName, IsChecked, CheckedLevel, CheckDate, Reasons) VALUES (bairong_ContentCheck_SEQ.NEXTVAL, @TableName, @PublishmentSystemID, @NodeID, @ContentID, @IsAdmin, @UserName, @IsChecked, @CheckedLevel, @CheckDate, @Reasons)";
            }

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 50, checkInfo.TableName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, checkInfo.PublishmentSystemID),
                this.GetParameter(PARM_NODEID, EDataType.Integer, checkInfo.NodeID),
                this.GetParameter(PARM_CONTENTID, EDataType.Integer, checkInfo.ContentID),
                this.GetParameter(PARM_IS_ADMIN, EDataType.VarChar, 18, checkInfo.IsAdmin.ToString()),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, checkInfo.UserName),
                this.GetParameter(PARM_IS_CHECKED, EDataType.VarChar, 18, checkInfo.IsChecked.ToString()),
                this.GetParameter(PARM_CHECKED_LEVEL, EDataType.Integer, checkInfo.CheckedLevel),
                this.GetParameter(PARM_CHECK_DATE, EDataType.DateTime, checkInfo.CheckDate),
                this.GetParameter(PARM_REASONS, EDataType.NVarChar, 255, checkInfo.Reasons),
			};

            this.ExecuteNonQuery(sqlString, parms);
		}

		public void Delete(int checkID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CHECKID, EDataType.Integer, checkID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
		}

		public ContentCheckInfo GetCheckInfo(int checkID)
		{
			ContentCheckInfo checkInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CHECKID, EDataType.Integer, checkID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms)) 
			{
				if (rdr.Read()) 
				{
                    checkInfo = new ContentCheckInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetInt32(8), rdr.GetDateTime(9), rdr.GetValue(10).ToString());
				}
				rdr.Close();
			}

			return checkInfo;
		}

        public ContentCheckInfo GetCheckInfoByLastID(string tableName, int contentID)
        {
            ContentCheckInfo checkInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 50, tableName),
                this.GetParameter(PARM_CONTENTID, EDataType.Integer, contentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_LAST_ID, parms))
            {
                if (rdr.Read())
                {
                    checkInfo = new ContentCheckInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetInt32(8), rdr.GetDateTime(9), rdr.GetValue(10).ToString());
                }
                rdr.Close();
            }

            return checkInfo;
        }

		public ArrayList GetCheckInfoArrayList(string tableName, int contentID)
		{
			ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 50, tableName),
                this.GetParameter(PARM_CONTENTID, EDataType.Integer, contentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms)) 
			{
				while (rdr.Read()) 
				{
                    arraylist.Add(new ContentCheckInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetInt32(8), rdr.GetDateTime(9), rdr.GetValue(10).ToString()));
				}
				rdr.Close();
			}

			return arraylist;
		}
	}
}