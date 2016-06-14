using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public interface ISearchwordSettingDAO
    {
        void Insert(SearchwordSettingInfo searchwordSettingInfo);

        void Update(SearchwordSettingInfo searchwordSettingInfo);

        SearchwordSettingInfo GetSearchwordSettingInfo(int publishmentSystemID);

        bool IsExists(int publishmentSystemID);
    }
}
