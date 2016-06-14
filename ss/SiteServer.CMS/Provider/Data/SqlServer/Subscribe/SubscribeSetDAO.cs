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
    public class SubscribeSetDAO : DataProviderBase, ISubscribeSetDAO
    {
        public string TableName
        {
            get
            {
                return "siteserver_SubscribeSet";
            }
        }


        public SubscribeSetInfo GetSubscribeSetInfo(int publishmentSystemID)
        {
            SubscribeSetInfo info = null;
            string SQL_WHERE = string.Format("WHERE publishmentSystemID =0");

            if(publishmentSystemID > 0)
                SQL_WHERE = string.Format("WHERE publishmentSystemID ={0}", publishmentSystemID);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new SubscribeSetInfo(); BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);//new SubscribeSetInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetValue(4).ToString(), ESubscribePushDateTypeUtils.GetEnumType(rdr.GetValue(3).ToString()),rdr.GetDateTime(5),rdr.GetInt32(6),rdr.GetValue(7).ToString());  
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public int Insert(SubscribeSetInfo info)
        {
            int contentID = 0;
             
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);
            contentID=this.ExecuteNonQuery(SQL_INSERT, parms);

            return contentID;
        }

        public void Update(SubscribeSetInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }
    
    }
}
