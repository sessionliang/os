using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public interface IWebsiteMessageDAO
    {
        int Insert(WebsiteMessageInfo websiteMessageInfo);

        void Update(WebsiteMessageInfo websiteMessageInfo);

        void Delete(int websiteMessageID);

        WebsiteMessageInfo GetWebsiteMessageInfo(string websiteMessageName, int publishmentSystemID);

        WebsiteMessageInfo GetWebsiteMessageInfo(int websiteMessageID);

        WebsiteMessageInfo GetWebsiteMessageInfoAsPossible(int websiteMessageID, int publishmentSystemID);

        int GetWebsiteMessageIDAsPossible(string websiteMessageName, int publishmentSystemID);

        WebsiteMessageInfo GetLastAddWebsiteMessageInfo(int publishmentSystemID);

        bool IsExists(string websiteMessageName, int publishmentSystemID);

        IEnumerable GetDataSource(int publishmentSystemID);

        ArrayList GetWebsiteMessageIDArrayList(int publishmentSystemID);

        ArrayList GetWebsiteMessageNameArrayList(int publishmentSystemID);

        string GetImportWebsiteMessageName(string websiteMessageName, int publishmentSystemID);

        bool UpdateTaxisToUp(int publishmentSystemID, int websiteMessageID);

        bool UpdateTaxisToDown(int publishmentSystemID, int websiteMessageID);

        /// <summary>
        /// …Ë÷√ƒ¨»œÕ¯’æ¡Ù—‘
        /// </summary>
        /// <returns></returns>
        int SetDefaultWebsiteMessageInfo(int publishmentSystemID);
    }
}
