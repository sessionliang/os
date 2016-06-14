using BaiRong.Core.Data;
using BaiRong.Model;
using System.Collections;
using System.Data;

namespace SiteServer.CMS.Provider.Data.Oracle
{
	public class PublishmentSystemDAO : SiteServer.CMS.Provider.Data.SqlServer.PublishmentSystemDAO
	{
		protected override string ADOType
		{
			get
			{
				return SqlUtils.ORACLE;
			}
		}

		protected override EDatabaseType DataBaseType
		{
			get
			{
                return EDatabaseType.Oracle;
			}
		}

        protected override System.Collections.ArrayList GetPublishmentSystemIDArrayList(System.DateTime sinceDate)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT p.PublishmentSystemID FROM siteserver_PublishmentSystem p INNER JOIN siteserver_Node n ON (p.PublishmentSystemID = n.NodeID AND (n.AddDate BETWEEN {0} AND sysdate)) ORDER BY p.IsHeadquarters DESC, p.ParentPublishmentSystemID, p.Taxis DESC, n.NodeID", SqlUtils.ParseToOracleDateTime(sinceDate));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }
            return arraylist;
        }
	}
}
