using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class SubscribePushRecordDAO : DataProviderBase, ISubscribePushRecordDAO
    {
        public string TableName
        {
            get
            {
                return "siteserver_SubscribePushRecord";
            }
        }

        private const string PARM_EMAIL = "@Email";
        private const string TABLE_NAME = "@siteserver_SubscribePushRecord";

        private const string SQL_SELECT_SUBSCRIBEUSER = "SELECT * FROM " + TABLE_NAME + " WHERE Email = @Email";


        public int Insert(SubscribePushRecordInfo info)
        {
            int contentID = 0;

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);
            contentID = this.ExecuteNonQuery(SQL_INSERT, parms);

            return contentID;
        }
        public void Update(SubscribePushRecordInfo info)
        {  
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms);
             this.ExecuteNonQuery(SQL_INSERT, parms); 
        }
        
        public string GetAllString(int publishmentSystemID, string whereString)
        {
            string orderByString = ETaxisTypeUtils.GetInputContentOrderByString(ETaxisType.OrderByTaxisDesc);
            string where = string.Format("WHERE (PublishmentSystemID = {0} {1}) {2}", publishmentSystemID, whereString, orderByString);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, where);
        }

        public string GetSelectCommend(int publishmentSystemID,  string mobile, string email, string dateFrom, string dateTo, ETriState checkedState)
        {


            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", dateFrom);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString = string.Format(" AND to_char(AddDate,'YYYY-MM-DD') >= '{0}' ", dateFrom);
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", dateTo);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString += string.Format(" AND to_char(AddDate,'YYYY-MM-DD') <= '{0}' ", dateTo);
                }
            }
            StringBuilder whereString = new StringBuilder("WHERE 1=1");


            whereString.AppendFormat(" and PublishmentSystemID = {0} ", publishmentSystemID);
 

            whereString.Append(dateString);

            if (!string.IsNullOrEmpty(mobile))
            {
                whereString.AppendFormat("AND (Mobile LIKE '%{0}%')  ", mobile);
            }

            if (!string.IsNullOrEmpty(email))
            {
                whereString.AppendFormat("AND (Email LIKE '%{0}%')  ", email);
            }

            if (!ETriStateUtils.Equals(ETriState.All, checkedState))
            {
                whereString.AppendFormat("AND PushStatu='{0}' ", checkedState);
            }



            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString.ToString());
        }



        public ArrayList GetSubscribeUserList(int publishmentSystemID, ArrayList arrayList)
        {
            ArrayList subscribeUserList = new ArrayList();

            string sqlString = string.Format("SELECT * FROM {0} WHERE PublishmentSystemID = {1} and RecordID in ({2}) ", TableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(arrayList));
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    SubscribePushRecordInfo info = new SubscribePushRecordInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    subscribeUserList.Add(info);
                }
                rdr.Close();
            }
            return subscribeUserList;
        }

    }
}
