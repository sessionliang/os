using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECountType = SiteServer.WeiXin.Model.ECountType;
using ECountTypeUtils = SiteServer.WeiXin.Model.ECountTypeUtils;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class ScenceDAO : DataProviderBase, IScenceDAO
    {
        private const string TABLE_NAME = "wx_Scence";

        public ScenceInfo GetScenceInfo(int scenceID)
        {
            ScenceInfo scenceInfo = null;

            string SQL_WHERE = string.Format("WHERE ID = {0}", scenceID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(this.ConnectionString, ScenceDAO.TABLE_NAME, 0, SqlUtils.Asterisk, SQL_WHERE, null);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    scenceInfo = new ScenceInfo(rdr);
                }
                rdr.Close();
            }

            return scenceInfo;
        }

        // Mr.wu begin
        public void UpdateClickNum(int scenceID, int publishmentSystemID)
        {
            if (scenceID > 0)
            {
                string sqlString = string.Format("UPDATE {0} set ClickNum= ClickNum+1 WHERE ID = {1} AND publishmentSystemID = {2}", TABLE_NAME, scenceID, publishmentSystemID);
                this.ExecuteNonQuery(sqlString);
            }
        }
        // Mr.wu end
    }
}
