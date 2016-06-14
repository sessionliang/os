using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;
using BaiRong.Core.AuxiliaryTable;


namespace SiteServer.CMS.Core
{
	public class RelatedIdentities
	{
        private RelatedIdentities()
		{
		}

        public static ArrayList GetRelatedIdentities(ETableStyle tableStyle, int publishmentSystemID, int relatedIdentity)
        {
            ArrayList relatedIdentities = new ArrayList();

            if (tableStyle == ETableStyle.Channel || tableStyle == ETableStyle.BackgroundContent || tableStyle == ETableStyle.VoteContent || tableStyle == ETableStyle.JobContent || tableStyle == ETableStyle.UserDefined)
            {
                relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemID, relatedIdentity);
            }
            else if (tableStyle == ETableStyle.User)
            {
                relatedIdentities = null;
            }
            else
            {
                relatedIdentities = RelatedIdentities.GetRelatedIdentities(relatedIdentity);
            }

            return relatedIdentities;
        }

        private static ArrayList GetRelatedIdentities(int relatedIdentity)
        {
            if (relatedIdentity == 0)
            {
                return TranslateUtils.StringCollectionToIntArrayList("0");
            }
            return TranslateUtils.StringCollectionToIntArrayList(relatedIdentity + ",0");
        }

        public static ArrayList GetChannelRelatedIdentities(int publishmentSystemID, int nodeID)
        {
            ArrayList arraylist = new ArrayList();
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            if (nodeInfo != null)
            {
                string nodeIDCollection = "0," + nodeInfo.NodeID;
                if (nodeInfo.ParentsCount > 0)
                {
                    nodeIDCollection = "0," + nodeInfo.ParentsPath + "," + nodeInfo.NodeID;
                }

                arraylist = TranslateUtils.StringCollectionToIntArrayList(nodeIDCollection);
                arraylist.Reverse();
            }
            else
            {
                arraylist.Add(0);
            }
            return arraylist;
        }

        public static ArrayList GetTableStyleInfoArrayList(PublishmentSystemInfo publishmentSystemInfo, ETableStyle tableStyle, int relatedIdentity)
        {
            ArrayList relatedIdentities = null;
            if (tableStyle == ETableStyle.BackgroundContent || tableStyle == ETableStyle.Channel || tableStyle == ETableStyle.GovInteractContent || tableStyle == ETableStyle.GovPublicContent || tableStyle == ETableStyle.VoteContent || tableStyle == ETableStyle.JobContent || tableStyle == ETableStyle.UserDefined)
            {
                relatedIdentities = GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, relatedIdentity);
            }
            else
            {
                relatedIdentities = GetRelatedIdentities(relatedIdentity);
            }

            string tableName = GetTableName(publishmentSystemInfo, tableStyle, relatedIdentity);

            return TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
        }

        public static string GetTableName(PublishmentSystemInfo publishmentSystemInfo, ETableStyle tableStyle, int relatedIdentity)
        {
            string tableName = publishmentSystemInfo.AuxiliaryTableForContent;

            if (tableStyle == ETableStyle.BackgroundContent || tableStyle == ETableStyle.GovInteractContent || tableStyle == ETableStyle.GovPublicContent || tableStyle == ETableStyle.VoteContent || tableStyle == ETableStyle.JobContent || tableStyle == ETableStyle.UserDefined)
            {
                tableName = NodeManager.GetTableName(publishmentSystemInfo, relatedIdentity);
            }
            else if (tableStyle == ETableStyle.Channel)
            {
                tableName = DataProvider.NodeDAO.TableName;
            }
            else if (tableStyle == ETableStyle.InputContent)
            {
                tableName = DataProvider.InputContentDAO.TableName;
            }

            return tableName;
        }
	}
}
