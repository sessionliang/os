using System;
using System.Data;
using System.Collections;
using SiteServer.Project.Model;

namespace SiteServer.Project.Core
{
    public interface IUrlActivityDAO
    {
        void Insert(UrlActivityInfo info);
    }
}
