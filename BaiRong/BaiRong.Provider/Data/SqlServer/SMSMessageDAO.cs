using System;
using System.Data;
using System.Collections.Generic;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;
namespace BaiRong.Provider.Data.SqlServer
{
    public class SMSMessageDAO : DataProviderBase,ISMSMessageDAO
    {
        private const string SQL_INSERT = "INSERT INTO bairong_SMSMessages (MobilesList,SMSContent,SendDate,SMSUserName) VALUES (@MobilesList,@SMSContent,@SendDate,@SMSUserName)";

        private const string SQL_UPDATE = "UPDATE bairong_SMSMessages SET MobilesList=@MobilesList,SMSContent=@SMSContent,SendDate=@SendDate,SMSUserName=@SMSUserName WHERE ID=@ID";

        private const string SQL_DELETE = "DELETE FROM bairong_SMSMessages WHERE ID=@ID";

        private const string SQL_DELETEALL = "DELETE FROM bairong_SMSMessages";

        private const string SQL_SELECT = "SELECT * FROM bairong_SMSMessages WHERE ID=@ID";

        public void Insert(SMSMessageInfo smsMessageInfo)
        {
            IDataParameter[] parms = new IDataParameter[] { 
                    this.GetParameter("@MobilesList",EDataType.NText,smsMessageInfo.MobilesList),
                    this.GetParameter("@SMSContent",EDataType.NVarChar,1000,smsMessageInfo.SMSContent),
                    this.GetParameter("@SendDate",EDataType.DateTime,smsMessageInfo.SendDate),
                    this.GetParameter("@SMSUserName",EDataType.NVarChar,255,smsMessageInfo.SMSUserName)
            };
            this.ExecuteNonQuery(SQL_INSERT, parms);
        }

        public void Update(SMSMessageInfo smsMessageInfo)
        {
            IDataParameter[] parms = new IDataParameter[] { 
                    this.GetParameter("@ID",EDataType.NText,smsMessageInfo.ID),
                    this.GetParameter("@MobilesList",EDataType.NText,smsMessageInfo.MobilesList),
                    this.GetParameter("@SMSContent",EDataType.NVarChar,1000,smsMessageInfo.SMSContent),
                    this.GetParameter("@SendDate",EDataType.DateTime,smsMessageInfo.SendDate),
                    this.GetParameter("@SMSUserName",EDataType.NVarChar,255,smsMessageInfo.SMSUserName)
            };
            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int id)
        {
            IDataParameter[] parms = new IDataParameter[]
            {
                this.GetParameter("@ID", EDataType.NText, id)
            };
            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        public void DeleteAll()
        {
            this.ExecuteNonQuery(SQL_DELETEALL);
        }

        public SMSMessageInfo GetSMSMessageInfo(int id)
        {
            SMSMessageInfo smsMessageInfo = null;

            IDataParameter[] parms = new IDataParameter[]
            {
                this.GetParameter("@ID", EDataType.NText, id)
            };

            using(IDataReader rdr=this.ExecuteReader(SQL_SELECT,parms))
            {
                if (rdr.Read())
                {
                    smsMessageInfo = new SMSMessageInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetDateTime(3), rdr.GetValue(4).ToString());
                }
                rdr.Close();
            }

            return smsMessageInfo;
        }

        public string GetSelectCommand()
        {
            return string.Format("SELECT * FROM bairong_SMSMessages WHERE SMSUserName = '{0}'", PageUtils.FilterSql(ConfigManager.Additional.SMSAccount));
        }
    }
}
