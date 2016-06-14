using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public interface IWebsiteMessageReplayTemplateDAO
    {
        void Insert(WebsiteMessageReplayTemplateInfo websiteMessageReplayTemplateInfo);

        void Update(WebsiteMessageReplayTemplateInfo websiteMessageReplayTemplateInfo);

        void Delete(ArrayList deleteIDArrayList);

        void Delete(int websiteMessageReplayTemplateID);

        WebsiteMessageReplayTemplateInfo GetWebsiteMessageReplayTemplateInfo(int websiteMessageReplayTemplateID);

        string GetSelectString(string where);

        string GetSortFieldName();

        ArrayList GetWebsiteMessageReplayTemplateInfoArrayList(string where);
    }
}
