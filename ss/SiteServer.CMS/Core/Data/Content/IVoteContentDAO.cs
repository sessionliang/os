using System;
using System.Collections;
using System.Data;
using BaiRong.Model;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IVoteContentDAO
	{
        VoteContentInfo GetContentInfo(PublishmentSystemInfo publishmentSystemInfo, int contentID);

        int GetContentNum(PublishmentSystemInfo publishmentSystemInfo);

        string GetSelectCommendByNodeID(PublishmentSystemInfo publishmentSystemInfo, int nodeID);
	}
}
