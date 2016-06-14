using System;
using System.Collections;
using System.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.Core;
using BaiRong.Model.Service;
using BaiRong.Core.Service;

namespace BaiRong.Provider.Data.SqlServer
{
    public class SMSServerDAO : DataProviderBase, ISMSServerDAO
    {
        private const string SQL_SELECT_BY_ID = "SELECT ID, SmsServerName, SmsServerEName, ParamCollection, IsEnable, ExtendValues  FROM bairong_SMSServer WHERE ID = @ID";

        private const string SQL_SELECT_BY_ENAME = "SELECT ID, SmsServerName, SmsServerEName, ParamCollection, IsEnable, ExtendValues FROM bairong_SMSServer WHERE SmsServerEName = @SmsServerEName";

        private const string SQL_UPDATE_SMS_SERVER = "UPDATE bairong_SMSServer SET SmsServerName = @SmsServerName, ParamCollection = @ParamCollection, IsEnable = @IsEnable, ExtendValues = @ExtendValues WHERE ID = @ID";

        private const string SQL_UPDATE_SMS_SERVER_STATE = "UPDATE bairong_SMSServer SET IsEnabled = @IsEnabled WHERE ID = @ID";

        private const string SQL_DELETE = "DELETE FROM bairong_SMSServer WHERE ID = @ID";

        private const string SQL_DELETE_BY_ENAME = "DELETE FROM bairong_SMSServer WHERE SmsServerEName = @SmsServerEName";

        private const string PARM_ID = "@ID";
        private const string PARM_SMS_SERVER_NAME = "@SmsServerName";
        private const string PARM_SMS_SERVER_ENAME = "@SmsServerEName";
        private const string PARM_PARAM_COLLECTION = "@ParamCollection";
        private const string PARM_IS_ENABLE = "@IsEnable";
        private const string PARM_EXTEND_VALUES = "@ExtendValues";


        public int Insert(SMSServerInfo info)
        {
            int id = 0;
            string sqlString = "INSERT INTO bairong_SMSServer (SmsServerName, SmsServerEName, ParamCollection, IsEnable, ExtendValues) output @@identity VALUES (@SmsServerName, @SmsServerEName, @ParamCollection, @IsEnable, @ExtendValues)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_SMSServer(SmsServerName, SmsServerEName, ParamCollection, IsEnable, ExtendValues) VALUES (bairong_SMSServer_SEQ.NEXTVAL, @SmsServerName, @SmsServerEName, @ParamCollection, @IsEnable, @ExtendValues)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_SMS_SERVER_NAME, EDataType.NVarChar, 50, info.SMSServerName),
                this.GetParameter(PARM_SMS_SERVER_ENAME, EDataType.VarChar, 100, info.SMSServerEName),
                this.GetParameter(PARM_PARAM_COLLECTION, EDataType.NText, TranslateUtils.NameValueCollectionToString( info.ParamCollection )),
                this.GetParameter(PARM_IS_ENABLE, EDataType.VarChar,18, info.IsEnable.ToString()),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, info.Additional.ToString())
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        id = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_SMSServer");
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

        public void Update(SMSServerInfo info)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
               this.GetParameter(PARM_SMS_SERVER_NAME, EDataType.NVarChar, 50, info.SMSServerName),
                this.GetParameter(PARM_SMS_SERVER_ENAME, EDataType.VarChar, 100, info.SMSServerEName),
                this.GetParameter(PARM_PARAM_COLLECTION, EDataType.NText, TranslateUtils.NameValueCollectionToString( info.ParamCollection )),
                this.GetParameter(PARM_IS_ENABLE, EDataType.VarChar,18, info.IsEnable.ToString()),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, info.Additional.ToString()),
                this.GetParameter(PARM_ID,EDataType.Integer,info.SMSServerID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_SMS_SERVER, updateParms);
        }

        public void UpdateState(int SMSServerID, bool isEnabled)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_IS_ENABLE, EDataType.VarChar, 18, isEnabled.ToString()),
                this.GetParameter(PARM_ID, EDataType.Integer, SMSServerID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_SMS_SERVER_STATE, updateParms);
        }

        public void Delete(int SMSServerID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, SMSServerID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        public void Delete(string SMSServerEName)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SMS_SERVER_ENAME, EDataType.VarChar,100, SMSServerEName)
			};

            this.ExecuteNonQuery(SQL_DELETE_BY_ENAME, parms);
        }

        public SMSServerInfo GetSMSServerInfo(int SMSServerID)
        {
            SMSServerInfo info = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, SMSServerID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_ID, parms))
            {
                if (rdr.Read())
                {
                    info = new SMSServerInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToNameValueCollection(rdr.GetValue(3).ToString()), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString());
                }
                rdr.Close();
            }

            return info;
        }

        public SMSServerInfo GetSMSServerInfo(string SMSServerEName)
        {
            SMSServerInfo info = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SMS_SERVER_ENAME, EDataType.VarChar,100, SMSServerEName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_ENAME, parms))
            {
                if (rdr.Read())
                {
                    info = new SMSServerInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToNameValueCollection(rdr.GetValue(3).ToString()), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString());
                }
                rdr.Close();
            }

            return info;
        }

        public ArrayList GetSMSServerInfoArrayList()
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = "SELECT ID, SmsServerName, SmsServerEName, ParamCollection, IsEnable, ExtendValues FROM bairong_SMSServer";

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    SMSServerInfo info = new SMSServerInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToNameValueCollection(rdr.GetValue(3).ToString()), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString());
                    arraylist.Add(info);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public bool IsExists(string SMSServerEName)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SMS_SERVER_ENAME, EDataType.VarChar, 100, SMSServerEName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_ENAME, parms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }
    }
}
