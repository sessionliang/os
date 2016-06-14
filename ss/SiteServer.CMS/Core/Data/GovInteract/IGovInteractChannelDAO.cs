using System;
using System.Data;
using System.Collections;

using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IGovInteractChannelDAO
	{
        void Insert(GovInteractChannelInfo channelInfo);

        void Update(GovInteractChannelInfo channelInfo);

        void Delete(int nodeID);

        GovInteractChannelInfo GetChannelInfo(int publishmentSystemID, int nodeID);

        string GetSummary(int nodeID);

        int GetApplyStyleID(int publishmentSystemID, int nodeID);

        int GetQueryStyleID(int publishmentSystemID, int nodeID);

        int GetNodeIDByInteractName(int publishmentSystemID, string interactName);

        int GetNodeIDByApplyStyleID(int applyStyleID);

        int GetNodeIDByQueryStyleID(int queryStyleID);

        bool IsExists(int nodeID);
	}
}
