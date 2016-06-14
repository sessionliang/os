using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
	public class ChannelUtility
	{
        private ChannelUtility()
		{
		}

		public static bool IsAncestorOrSelf(int publishmentSystemID, int parentID, int childID)
		{
			if (parentID == childID)
			{
				return true;
			}
			NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, childID);
			if (nodeInfo == null)
			{
				return false;
			}
			if (CompareUtils.Contains(nodeInfo.ParentsPath, parentID.ToString()))
			{
				return true;
			}
			return false;
		}

		public static ArrayList GetParentsNodeIDArrayList(string parentsPath)
		{
			ArrayList arraylist = new ArrayList();
			if (!string.IsNullOrEmpty(parentsPath))
			{
				string[] nodeIDStrArray = parentsPath.Split(',');
				if (nodeIDStrArray != null && nodeIDStrArray.Length > 0)
				{
					foreach (string nodeIDStr in nodeIDStrArray)
					{
						int nodeID = TranslateUtils.ToInt(nodeIDStr.Trim());
						if (nodeID != 0)
						{
							arraylist.Add(nodeID);
						}
					}
				}
			}
			return arraylist;
		}
	}
}
