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

namespace SiteServer.CMS.Provider.Data.Oracle
{
    public class CommentDAO : SiteServer.CMS.Provider.Data.SqlServer.CommentDAO
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

        public override int GetCountChecked(int publishmentSystemID, DateTime begin, DateTime end)
        {
            string whereString = string.Format("WHERE PublishmentSystemID = {0} AND (AddDate BETWEEN {1} AND {2}) AND IsChecked = 'True'", publishmentSystemID, SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end));
            string sqlString = string.Format("SELECT COUNT(*) FROM [{0}] {1}", "siteserver_Comment", whereString);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public override int GetCountChecked(int publishmentSystemID, int channelID, DateTime begin, DateTime end)
        {
            string whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID = {1} AND (AddDate BETWEEN {2} AND {3}) AND IsChecked = 'True'", publishmentSystemID, channelID, SqlUtils.ParseToOracleDateTime(begin), SqlUtils.ParseToOracleDateTime(end));
            string sqlString = string.Format("SELECT COUNT(*) FROM [{0}] {1}", "siteserver_Comment", whereString);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }
	}
}
