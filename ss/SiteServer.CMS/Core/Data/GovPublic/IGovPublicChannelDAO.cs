using System;
using System.Data;
using System.Collections;

using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IGovPublicChannelDAO
	{
        void Insert(GovPublicChannelInfo channelInfo);

        void Update(GovPublicChannelInfo channelInfo);

        void Delete(int nodeID);

        GovPublicChannelInfo GetChannelInfo(int nodeID);

        string GetCode(int nodeID);

        bool IsExists(int nodeID);
	}
}
