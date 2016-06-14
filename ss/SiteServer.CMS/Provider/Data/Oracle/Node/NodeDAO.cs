using System;
using System.Text;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.Oracle
{
	public class NodeDAO : SiteServer.CMS.Provider.Data.SqlServer.NodeDAO
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

        public override void UpdateContentNum(PublishmentSystemInfo publishmentSystemInfo, int nodeID, bool isRemoveCache)
		{
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
			string sqlString = string.Empty;
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            if (!string.IsNullOrEmpty(tableName))
            {
                int contentNum = BaiRongDataProvider.ContentDAO.GetCount(tableName, nodeID);
                sqlString = string.Format("UPDATE siteserver_Node SET ContentNum = {0} WHERE (NodeID = {1})", contentNum, nodeID);
            }
            if (!string.IsNullOrEmpty(sqlString))
			{
                this.ExecuteNonQuery(sqlString);
			}

            if (isRemoveCache)
            {
                NodeManager.RemoveCache(publishmentSystemInfo.PublishmentSystemID);
            }
		}
	}
}
