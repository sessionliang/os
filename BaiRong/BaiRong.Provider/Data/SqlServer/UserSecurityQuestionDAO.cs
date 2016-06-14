using System;
using System.Data;
using System.Collections;

using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using System.Text;
using System.Collections.Generic;

namespace BaiRong.Provider.Data.SqlServer
{
    public class UserSecurityQuestionDAO : DataProviderBase, IUserSecurityQuestionDAO
    {
        private const string SQL_SELECT_SECURITY_QUESTION = "SELECT ID, Question, IsEnable FROM bairong_UserSecurityQuestion WHERE ID = @ID";

        private const string SQL_UPDATE_NOTICE_TEMPLATE = "UPDATE bairong_UserSecurityQuestion SET  Question=@Question, IsEnable=@IsEnable WHERE ID = @ID";

        private const string SQL_INSERT_NOTICE_TEMPLATE = "INSERT INTO bairong_UserSecurityQuestion ( Question, IsEnable) VALUES ( @Question, @IsEnable)";

        private const string PARM_ID = "@ID";
        private const string PARM_QUESTION = "@Question";
        private const string PARM_IS_ENABLE = "@IsEnable";


        public void Insert(UserSecurityQuestionInfo info)
        {

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
				this.GetParameter(PARM_QUESTION, EDataType.NVarChar, 255, info.Question),
				this.GetParameter(PARM_IS_ENABLE, EDataType.VarChar, 18, info.IsEnable.ToString())
			};

            this.ExecuteNonQuery(SQL_INSERT_NOTICE_TEMPLATE, parms);

        }

        public void Update(UserSecurityQuestionInfo info)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
				this.GetParameter(PARM_QUESTION, EDataType.NVarChar, 255, info.Question),
				this.GetParameter(PARM_IS_ENABLE, EDataType.VarChar, 18, info.IsEnable.ToString()),
                this.GetParameter(PARM_ID, EDataType.Integer, info.ID)
			};
            this.ExecuteNonQuery(SQL_UPDATE_NOTICE_TEMPLATE, parms);
        }



        public void Delete(int SecurityQuestionID)
        {

            string deleteSqlString = string.Format("DELETE bairong_UserSecurityQuestion WHERE ID = {0}", SecurityQuestionID);
            this.ExecuteNonQuery(deleteSqlString);
        }

        public UserSecurityQuestionInfo GetSecurityQuestionInfo(int SecurityQuestionID)
        {
            UserSecurityQuestionInfo info = null;

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
				this.GetParameter(PARM_ID, EDataType.Integer, SecurityQuestionID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SECURITY_QUESTION, selectParms))
            {
                if (rdr.Read())
                {
                    info = new UserSecurityQuestionInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), TranslateUtils.ToBool(rdr.GetValue(2).ToString()));
                }
                rdr.Close();
            }

            return info;
        }

        public string GetSqlString(string keywords)
        {
            string sql = string.Format("SELECT  ID, Question, IsEnable  FROM bairong_UserSecurityQuestion Where 1=1 ");

            if (!string.IsNullOrEmpty(keywords))
            {
                sql += string.Format(" and Question like '%{0}%' ", PageUtils.FilterSql(keywords));
            }

            return sql;
        }

        public DataSet GetSecurityQuestionDS(string keywords)
        {
            DataSet ds = new DataSet();

            string sql = GetSqlString(keywords);
            ds = this.ExecuteDataset(sql);

            return ds;
        }

        public List<UserSecurityQuestionInfo> GetSecurityQuestionInfoList()
        {
            List<UserSecurityQuestionInfo> list = new List<UserSecurityQuestionInfo>();
            string sql = "SELECT ID, Question, IsEnable FROM bairong_UserSecurityQuestion WHERE IsEnable = 'true'";
            using (IDataReader rdr = this.ExecuteReader(sql))
            {
                while (rdr.Read())
                {
                    UserSecurityQuestionInfo info = new UserSecurityQuestionInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), TranslateUtils.ToBool(rdr.GetValue(2).ToString()));
                    list.Add(info);
                }
            }
            return list;
        }

        public void SetDefaultQuestion()
        {
            this.ExecuteNonQuery("DELETE FROM bairong_UserSecurityQuestion;");
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat("INSERT INTO bairong_UserSecurityQuestion (Question, IsEnable) VALUES ('您的名字', 'True');");
            sbSql.AppendFormat("INSERT INTO bairong_UserSecurityQuestion (Question, IsEnable) VALUES ('您的生日', 'True');");
            sbSql.AppendFormat("INSERT INTO bairong_UserSecurityQuestion (Question, IsEnable) VALUES ('您父亲的名字', 'True');");
            sbSql.AppendFormat("INSERT INTO bairong_UserSecurityQuestion (Question, IsEnable) VALUES ('您母亲的名字', 'True');");
            sbSql.AppendFormat("INSERT INTO bairong_UserSecurityQuestion (Question, IsEnable) VALUES ('您配偶的名字', 'True');");
            sbSql.AppendFormat("INSERT INTO bairong_UserSecurityQuestion (Question, IsEnable) VALUES ('您配偶的生日', 'True');");
            sbSql.AppendFormat("INSERT INTO bairong_UserSecurityQuestion (Question, IsEnable) VALUES ('您初中在哪个学校', 'True');");
            sbSql.AppendFormat("INSERT INTO bairong_UserSecurityQuestion (Question, IsEnable) VALUES ('您高中在哪个学校', 'True');");
            sbSql.AppendFormat("INSERT INTO bairong_UserSecurityQuestion (Question, IsEnable) VALUES ('您大学在哪个学校', 'True');");
            this.ExecuteNonQuery(sbSql.ToString());
        }
    }
}