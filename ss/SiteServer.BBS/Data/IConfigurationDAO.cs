using System;
using System.Collections.Generic;
using System.Data;
using SiteServer.BBS.Model;

namespace SiteServer.BBS
{
    public interface IConfigurationDAO
    {
        void Update(ConfigurationInfo info);

        ConfigurationInfo GetConfigurationInfo(int publishmentSystemID);
    }
}