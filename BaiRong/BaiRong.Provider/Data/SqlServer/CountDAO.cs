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
    public class CountDAO : DataProviderBase, ICountDAO
    {
        // Static constants
        private const string SQL_SELECT_COUNT_NUM = "SELECT CountNum FROM bairong_Count WHERE ApplicationName = @ApplicationName AND RelatedTableName = @RelatedTableName AND RelatedIdentity = @RelatedIdentity AND CountType = @CountType";

        private const string SQL_ADD_COUNT_NUM = "UPDATE bairong_Count SET CountNum = (CountNum + 1) WHERE ApplicationName = @ApplicationName AND RelatedTableName = @RelatedTableName AND RelatedIdentity = @RelatedIdentity AND CountType = @CountType";

        private const string SQL_DELETE_BY_RELATED_TABLE_NAME = "DELETE FROM bairong_Count WHERE ApplicationName = @ApplicationName AND RelatedTableName = @RelatedTableName";

        private const string SQL_DELETE_BY_IDENTITY = "DELETE FROM bairong_Count WHERE ApplicationName = @ApplicationName AND RelatedTableName = @RelatedTableName AND RelatedIdentity = @RelatedIdentity";

        private const string PARM_COUNT_ID = "@CountID";
        private const string PARM_APPLICATION_NAME = "@ApplicationName";
        private const string PARM_RELATED_TABLE_NAME = "@RelatedTableName";
        private const string PARM_RELATED_IDENTITY = "@RelatedIdentity";
        private const string PARM_COUNT_TYPE = "@CountType";
        private const string PARM_COUNT_NUM = "@CountNum";

        public void Insert(string applicationName, string relatedTableName, string relatedIdentity, ECountType countType, int countNum)
        {
            string sqlString = "INSERT INTO bairong_Count (ApplicationName, RelatedTableName, RelatedIdentity, CountType, CountNum) VALUES (@ApplicationName, @RelatedTableName, @RelatedIdentity, @CountType, @CountNum)";

            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_Count (CountID, ApplicationName, RelatedTableName, RelatedIdentity, CountType, CountNum) VALUES (bairong_Count_SEQ.NEXTVAL, @ApplicationName, @RelatedTableName, @RelatedIdentity, @CountType, @CountNum)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_APPLICATION_NAME, EDataType.VarChar, 50, applicationName),
				this.GetParameter(PARM_RELATED_TABLE_NAME, EDataType.NVarChar, 255, relatedTableName),
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.NVarChar, 255, relatedIdentity),
				this.GetParameter(PARM_COUNT_TYPE, EDataType.VarChar, 50, ECountTypeUtils.GetValue(countType)),
				this.GetParameter(PARM_COUNT_NUM, EDataType.Integer, countNum)
			};

            this.ExecuteNonQuery(sqlString, insertParms);
        }

        public void AddCountNum(string applicationName, string relatedTableName, string relatedIdentity, ECountType countType)
        {
            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_APPLICATION_NAME, EDataType.VarChar, 50, applicationName),
				this.GetParameter(PARM_RELATED_TABLE_NAME, EDataType.NVarChar, 255, relatedTableName),
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.NVarChar, 255, relatedIdentity),
				this.GetParameter(PARM_COUNT_TYPE, EDataType.VarChar, 50, ECountTypeUtils.GetValue(countType)),
			};

            this.ExecuteNonQuery(SQL_ADD_COUNT_NUM, insertParms);
        }

        public void DeleteByRelatedTableName(string applicationName, string relatedTableName)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_APPLICATION_NAME, EDataType.VarChar, 50, applicationName),
				this.GetParameter(PARM_RELATED_TABLE_NAME, EDataType.NVarChar, 255, relatedTableName)
			};

            this.ExecuteNonQuery(SQL_DELETE_BY_RELATED_TABLE_NAME, parms);
        }

        public void DeleteByIdentity(string applicationName, string relatedTableName, string relatedIdentity)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_APPLICATION_NAME, EDataType.VarChar, 50, applicationName),
				this.GetParameter(PARM_RELATED_TABLE_NAME, EDataType.NVarChar, 255, relatedTableName),
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.NVarChar, 255, relatedIdentity),
			};

            this.ExecuteNonQuery(SQL_DELETE_BY_IDENTITY, parms);
        }

        public bool IsExists(string applicationName, string relatedTableName, string relatedIdentity, ECountType countType)
        {
            if (this.GetCountNum(applicationName, relatedTableName, relatedIdentity, countType) == 0)
            {
                return false;
            }
            return true;
        }

        public int GetCountNum(string applicationName, string relatedTableName, string relatedIdentity, ECountType countType)
        {
            int countNum = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_APPLICATION_NAME, EDataType.VarChar, 50, applicationName),
				this.GetParameter(PARM_RELATED_TABLE_NAME, EDataType.NVarChar, 255, relatedTableName),
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.NVarChar, 255, relatedIdentity),
				this.GetParameter(PARM_COUNT_TYPE, EDataType.VarChar, 50, ECountTypeUtils.GetValue(countType))
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_COUNT_NUM, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        countNum = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }

            return countNum;
        }

        /// <summary>
        /// 获取站点的统计数据
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="relatedTableName"></param>
        /// <param name="publishmentSystemID"></param>
        /// <param name="countType"></param>
        /// <returns></returns>
        public int GetCountNum(string applicationName, string relatedTableName, int publishmentSystemID, ECountType countType)
        {
            int countNum = 0;

            string sqlString = string.Format(@"select sum(cou.CountNum) from bairong_Count cou left join {0} con on cou.RelatedIdentity = con.ID
where cou.ApplicationName = '{3}'
and cou.RelatedTableName = '{0}'
and con.PublishmentSystemID = {1}
and cou.CountType = '{2}'", relatedTableName, publishmentSystemID, ECountTypeUtils.GetValue(countType), applicationName);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        countNum = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return countNum;
        }
    }
}
