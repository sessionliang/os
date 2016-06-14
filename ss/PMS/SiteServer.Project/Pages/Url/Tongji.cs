using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core;

namespace SiteServer.Project.Pages
{
    public class Tongji : Page
    {
        public void Page_Load(object sender, EventArgs E)
        {
            string url = base.Request.QueryString["url"];
            string version = base.Request.QueryString["version"];
            string databaseType = base.Request.QueryString["databaseType"];

            string domain = PageUtils.GetUrlWithoutPathInfo(url);

            if (!StringUtils.IsNullorEmpty(domain))
            {
                domain = domain.ToLower().Trim();
                domain = StringUtils.ReplaceStartsWith(domain, "http://", string.Empty);

                UrlInfo urlInfo = new UrlInfo(0, string.Empty, 0, version, false, databaseType, string.Empty, domain, DateTime.Now, DateTime.Now.AddDays(-1), 1, false, DateTime.Now, string.Empty, string.Empty);

                DateTime lastActivityDate = DateTime.MinValue;
                urlInfo.ID = DataProvider.UrlDAO.GetUrlID(string.Empty, domain, out lastActivityDate);
                if (urlInfo.ID > 0)
                {
                    TimeSpan ts = DateTime.Now - lastActivityDate;
                    if (ts.Minutes > 30)
                    {
                        DataProvider.UrlDAO.Update(urlInfo.GUID, urlInfo.Site, urlInfo.Version, urlInfo.IsBeta, urlInfo.DatabaseType, urlInfo.Dotnet, urlInfo.ID);
                        UrlActivityInfo activityInfo = new UrlActivityInfo(0, urlInfo.ID, DateTime.Now);
                        DataProvider.UrlActivityDAO.Insert(activityInfo);
                    }
                }
                else
                {
                    DataProvider.UrlDAO.Insert(urlInfo);
                }
            }
        }
    }
}
