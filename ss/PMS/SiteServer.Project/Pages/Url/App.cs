using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core;
using BaiRong.Text.LitJson;
using BaiRong.Model;
using BaiRong.Core.Data;

namespace SiteServer.Project.Pages
{
    public class App : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(base.Request.QueryString["productID"]))
                {
                    string guid = PageUtils.FilterSql(base.Request.QueryString["guid"]);
                    string version = PageUtils.FilterSql(base.Request.QueryString["version"]);
                    string dotnet = PageUtils.FilterSql(base.Request.QueryString["dotnet"]);

                    UrlInfo urlInfo = new UrlInfo(0, guid, 0, version, false, string.Empty, dotnet, string.Empty, DateTime.Now, DateTime.Now.AddDays(-1), 1, false, DateTime.Now.AddYears(1), string.Empty, string.Empty);

                    DateTime lastActivityDate = DateTime.MinValue;
                    urlInfo.ID = DataProvider.UrlDAO.GetUrlID(guid, string.Empty, out lastActivityDate);

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

                    base.Response.End();
                }
            }
        }

        //{success:'true',fullVersion:'3.6 SP1',pubDate:'2013年3月11日',pageUrl:'http://bbs.siteserver.cn/thread-3.aspx'}
        public Hashtable GetHashtable(string version, bool isBeta, string domain, string productID)
        {
            Hashtable attributes = new Hashtable();

            HotfixInfo selectedHotfixInfo = null;
            ArrayList hotfixIDArrayList = HotfixManager.GetHotfixIDArrayList();
            foreach (int hotfixID in hotfixIDArrayList)
            {
                HotfixInfo hotfixInfo = HotfixManager.GetHotfixInfo(hotfixID);
                if (hotfixInfo.IsRestrict)
                {
                    if (!string.IsNullOrEmpty(hotfixInfo.RestrictDomain))
                    {
                        if (!StringUtils.StartsWithIgnoreCase(domain, hotfixInfo.RestrictDomain))
                        {
                            continue;
                        }
                    }
                    if (!string.IsNullOrEmpty(hotfixInfo.RestrictProductIDCollection))
                    {
                        if (!StringUtils.In(hotfixInfo.RestrictProductIDCollection.ToLower(), productID.ToLower()))
                        {
                            continue;
                        }
                    }
                }
                if (ProductManager.GetVersionDouble(hotfixInfo.Version) > ProductManager.GetVersionDouble(version))
                {
                    selectedHotfixInfo = hotfixInfo;
                    break;
                }
                else if (ProductManager.GetVersionDouble(hotfixInfo.Version) == ProductManager.GetVersionDouble(version))
                {
                    if (hotfixInfo.IsBeta == false && isBeta == true)
                    {
                        selectedHotfixInfo = hotfixInfo;
                        break;
                    }
                }
            }

            if (selectedHotfixInfo != null)
            {
                string fullVersion = ProductManager.GetFullVersion(selectedHotfixInfo.Version, selectedHotfixInfo.IsBeta);
                string pubDate = DateUtils.GetDateString(selectedHotfixInfo.PubDate, EDateFormatType.Chinese);

                attributes.Add("success", "true");
                attributes.Add("fullVersion", fullVersion);
                attributes.Add("pubDate", pubDate);
                attributes.Add("message", selectedHotfixInfo.Message);
                attributes.Add("hotfixID", selectedHotfixInfo.ID.ToString());
                attributes.Add("pageUrl", selectedHotfixInfo.PageUrl);
            }
            else
            {
                attributes.Add("success", "false");
                attributes.Add("message", "当前版本为最新版本，请继续关注产品动态");
            }

            return attributes;
        }
    }
}
