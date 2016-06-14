using System;
using System.Data;
using System.Collections;

using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using System.Text;

namespace BaiRong.Provider.Data.SqlServer
{
    public class UserNoticeTemplateDAO : DataProviderBase, IUserNoticeTemplateDAO
    {
        private const string SQL_SELECT_NOTICE_TEMPLATE = "SELECT ID, RelatedIdentity, Name, Title, Content, Type, IsEnable, IsSystem, RemoteTemplateID, RemoteType FROM bairong_UserNoticeTemplate WHERE ID = @ID";

        private const string SQL_UPDATE_NOTICE_TEMPLATE_IS_ENABLE = "UPDATE bairong_UserNoticeTemplate SET IsEnable = @IsEnable WHERE ID = @ID";

        private const string SQL_UPDATE_NOTICE_TEMPLATE = "UPDATE bairong_UserNoticeTemplate SET  RelatedIdentity=@RelatedIdentity, Name=@Name, Title=@Title, Content=@Content, Type=@Type, IsEnable=@IsEnable, IsSystem=@IsSystem, RemoteTemplateID = @RemoteTemplateID, RemoteType = @RemoteType WHERE ID = @ID";

        private const string SQL_INSERT_NOTICE_TEMPLATE = "INSERT INTO bairong_UserNoticeTemplate (RelatedIdentity, Name, Title, Content, Type, IsEnable, IsSystem, RemoteTemplateID, RemoteType) VALUES (@RelatedIdentity, @Name, @Title, @Content, @Type, @IsEnable, @IsSystem, @RemoteTemplateID, @RemoteType)";

        private const string PARM_ID = "@ID";
        private const string PARM_RELATED_IDENTITY = "@RelatedIdentity";
        private const string PARM_NAME = "@Name";
        private const string PARM_TITLE = "@Title";
        private const string PARM_CONTENT = "@Content";
        private const string PARM_TYPE = "@Type";
        private const string PARM_IS_ENABLE = "@IsEnable";
        private const string PARM_IS_SYSTEM = "@IsSystem";
        private const string PARM_REMOTE_TEMPLATE_ID = "@RemoteTemplateID";
        private const string PARM_REMOTE_TYPE = "@RemoteType";

        public int Insert(UserNoticeTemplateInfo info)
        {
            int id = 0;
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.VarChar, 50, EUserNoticeTypeUtils.GetValue( info.RelatedIdentity)),
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 50, info.Name),
                this.GetParameter(PARM_TITLE, EDataType.NVarChar,50, info.Title),
                this.GetParameter(PARM_CONTENT, EDataType.NText, info.Content),
                this.GetParameter(PARM_TYPE, EDataType.VarChar,50,EUserNoticeTemplateTypeUtils.GetValue( info.Type)),
                this.GetParameter(PARM_IS_ENABLE, EDataType.VarChar,18, info.IsEnable.ToString()),
                this.GetParameter(PARM_IS_SYSTEM, EDataType.VarChar,18, info.IsSystem.ToString()),
                this.GetParameter(PARM_REMOTE_TEMPLATE_ID, EDataType.VarChar,50, info.RemoteTemplateID),
                this.GetParameter(PARM_REMOTE_TYPE, EDataType.VarChar,50,ESMSServerTypeUtils.GetValue(info.RemoteType))
			};
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT_NOTICE_TEMPLATE, parms);
                        id = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_UserNoticeTemplate");
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }

                }
            }


            return id;

        }

        public void Update(UserNoticeTemplateInfo info)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.VarChar, 50, EUserNoticeTypeUtils.GetValue( info.RelatedIdentity)),
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 50, info.Name),
                this.GetParameter(PARM_TITLE, EDataType.NVarChar,50, info.Title),
                this.GetParameter(PARM_CONTENT, EDataType.NText, info.Content),
                this.GetParameter(PARM_TYPE, EDataType.VarChar,50,EUserNoticeTemplateTypeUtils.GetValue( info.Type)),
                this.GetParameter(PARM_IS_ENABLE, EDataType.VarChar,18, info.IsEnable.ToString()),
                this.GetParameter(PARM_IS_SYSTEM, EDataType.VarChar,18, info.IsSystem.ToString()),
                this.GetParameter(PARM_REMOTE_TEMPLATE_ID, EDataType.VarChar,50, info.RemoteTemplateID),
                this.GetParameter(PARM_REMOTE_TYPE, EDataType.VarChar,50,ESMSServerTypeUtils.GetValue(info.RemoteType)),
                this.GetParameter(PARM_ID, EDataType.Integer, info.ID)
			};
            this.ExecuteNonQuery(SQL_UPDATE_NOTICE_TEMPLATE, parms);
        }



        public void Delete(int NoticeTemplateID)
        {

            string deleteSqlString = string.Format("DELETE bairong_UserNoticeTemplate WHERE ID = {0}", NoticeTemplateID);
            this.ExecuteNonQuery(deleteSqlString);
        }

        public void SetIsEnable(int NoticeTemplateID)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[] 
            {
				this.GetParameter(PARM_IS_ENABLE, EDataType.VarChar, 18, true.ToString()),
				this.GetParameter(PARM_ID, EDataType.Integer, NoticeTemplateID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_NOTICE_TEMPLATE_IS_ENABLE, updateParms);

        }

        public void SetIsNotEnable(int NoticeTemplateID)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[] 
            {
				this.GetParameter(PARM_IS_ENABLE, EDataType.VarChar, 18, false.ToString()),
				this.GetParameter(PARM_ID, EDataType.Integer, NoticeTemplateID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_NOTICE_TEMPLATE_IS_ENABLE, updateParms);

        }

        public UserNoticeTemplateInfo GetNoticeTemplateInfo(int NoticeTemplateID)
        {
            UserNoticeTemplateInfo info = null;

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
				this.GetParameter(PARM_ID, EDataType.Integer, NoticeTemplateID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NOTICE_TEMPLATE, selectParms))
            {
                if (rdr.Read())
                {
                    info = new UserNoticeTemplateInfo(rdr.GetInt32(0), EUserNoticeTypeUtils.GetEnumType(rdr.GetValue(1).ToString()), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), EUserNoticeTemplateTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(),ESMSServerTypeUtils.GetEnumType(rdr.GetValue(9).ToString()));
                }
                rdr.Close();
            }

            return info;
        }

        public UserNoticeTemplateInfo GetNoticeTemplateInfo(string userNoticeType, string userNoticeTemplateType)
        {
            string sql = GetSqlString(userNoticeType, userNoticeTemplateType, string.Empty);

            UserNoticeTemplateInfo info = null;

            using (IDataReader rdr = this.ExecuteReader(sql))
            {
                if (rdr.Read())
                {
                    info = new UserNoticeTemplateInfo(rdr.GetInt32(0), EUserNoticeTypeUtils.GetEnumType(rdr.GetValue(1).ToString()), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetValue(4).ToString(), EUserNoticeTemplateTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), TranslateUtils.ToBool(rdr.GetValue(6).ToString()), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), ESMSServerTypeUtils.GetEnumType(rdr.GetValue(9).ToString()));
                }
                rdr.Close();
            }

            return info;
        }

        public string GetSqlString(string userNoticeType, string userNoticeTemplateType, string keywords)
        {
            string sql = string.Format("SELECT ID, RelatedIdentity, Name, Title, Content, Type, IsEnable, IsSystem, RemoteTemplateID, RemoteType FROM bairong_UserNoticeTemplate Where 1=1 ");

            if (!string.IsNullOrEmpty(userNoticeType))
            {
                sql += string.Format(" and RelatedIdentity = '{0}' ", PageUtils.FilterSql(userNoticeType));
            }

            if (!string.IsNullOrEmpty(userNoticeTemplateType))
            {
                sql += string.Format(" and type = '{0}' ", PageUtils.FilterSql(userNoticeTemplateType));
            }

            if (!string.IsNullOrEmpty(keywords))
            {
                sql += string.Format(" and name like '%{0}%'  ", PageUtils.FilterSql(keywords));
            }

            return sql;
        }

        public string GetSortFieldName()
        {
            return "ID";
        }

        public DataSet GetUserNoticeTemplateDS(string userNoticeType, string userNoticeTemplateType, string keywords)
        {
            DataSet ds = new DataSet();

            string sql = GetSqlString(userNoticeType, userNoticeTemplateType, keywords);
            ds = this.ExecuteDataset(sql);

            return ds;
        }
    }
}