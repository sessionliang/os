using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.CMS.Core.Security;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class VoteContentDAO : DataProviderBase, IVoteContentDAO
	{
        public VoteContentInfo GetContentInfo(PublishmentSystemInfo publishmentSystemInfo, int contentID)
        {
            VoteContentInfo info = null;
            if (contentID > 0)
            {
                if (!string.IsNullOrEmpty(publishmentSystemInfo.AuxiliaryTableForVote))
                {
                    string SQL_WHERE = string.Format("WHERE ID = {0}", contentID);
                    string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(publishmentSystemInfo.AuxiliaryTableForVote, SqlUtils.Asterisk, SQL_WHERE);

                    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                    {
                        if (rdr.Read())
                        {
                            info = new VoteContentInfo();
                            BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                        }
                        rdr.Close();
                    }
                }
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public int GetContentNum(PublishmentSystemInfo publishmentSystemInfo)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1}", publishmentSystemInfo.AuxiliaryTableForVote, publishmentSystemInfo.PublishmentSystemID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectCommendByNodeID(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            return BaiRongDataProvider.ContentDAO.GetSelectCommend(publishmentSystemInfo.AuxiliaryTableForVote, nodeID, ETriState.All);
        }
	}
}
