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

namespace SiteServer.Project.Yun.Pages
{
    public class Hotfix : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(base.Request.QueryString["hotfixID"]))
                {
                    int hotfixID = TranslateUtils.ToInt(base.Request.QueryString["hotfixID"]);
                    if (hotfixID > 0)
                    {
                        HotfixDownloadInfo downloadInfo = new HotfixDownloadInfo();
                        if (!string.IsNullOrEmpty(base.Request.QueryString["version"]))
                        {
                            string version = PageUtils.FilterSql(base.Request.QueryString["version"]);
                            bool isBeta = TranslateUtils.ToBool(base.Request.QueryString["isBeta"]);
                            string domain = PageUtils.FilterSql(base.Request.QueryString["domain"]);
                            if (string.IsNullOrEmpty(domain))
                            {
                                domain = PageUtils.GetUrlWithoutPathInfo(PageUtils.GetRefererUrl());
                            }
                            downloadInfo = new HotfixDownloadInfo(0, hotfixID, version, isBeta, domain, DateTime.Now);
                        }
                        else
                        {
                            downloadInfo.HotfixID = hotfixID;
                            downloadInfo.Domain = PageUtils.GetUrlWithoutPathInfo(PageUtils.GetRefererUrl());
                            downloadInfo.DownloadDate = DateTime.Now;
                        }
                        DataProvider.HotfixDownloadDAO.Insert(downloadInfo);

                        DataProvider.HotfixDAO.AddDownloadCount(hotfixID);
                        HotfixInfo hotfixInfo = HotfixManager.GetHotfixInfo(hotfixID);
                        string fileUrl = string.Empty;
                        if (hotfixInfo != null)
                        {
                            fileUrl = hotfixInfo.FileUrl;
                        }
                        if (!string.IsNullOrEmpty(fileUrl))
                        {
                            PageUtils.Redirect(fileUrl);
                        }
                    }
                }
                else
                {
                    string callback = PageUtils.FilterSql(base.Request.QueryString["callback"]);
                    string guid = PageUtils.FilterSql(base.Request.QueryString["guid"]);
                    int site = TranslateUtils.ToInt(base.Request.QueryString["site"]);
                    string version = PageUtils.FilterSql(base.Request.QueryString["version"]);
                    bool isBeta = TranslateUtils.ToBool(base.Request.QueryString["isBeta"]);
                    string databaseType = PageUtils.FilterSql(base.Request.QueryString["databaseType"]);
                    string dotnet = PageUtils.FilterSql(base.Request.QueryString["dotnet"]);
                    string domain = PageUtils.FilterSql(base.Request.QueryString["domain"]);
                    if (string.IsNullOrEmpty(domain))
                    {
                        domain = PageUtils.GetUrlWithoutPathInfo(PageUtils.GetRefererUrl());
                    }

                    if (!string.IsNullOrEmpty(domain))
                    {
                        domain = domain.ToLower().Trim();
                        domain = StringUtils.ReplaceStartsWith(domain, "http://", string.Empty);
                        domain = domain.Replace("/siteserver", string.Empty);

                        UrlInfo urlInfo = new UrlInfo(0, guid, site, version, isBeta, databaseType, dotnet, domain, DateTime.Now, DateTime.Now.AddDays(-1), 1, false, DateTime.Now.AddYears(1), string.Empty, string.Empty);

                        DateTime lastActivityDate = DateTime.MinValue;
                        urlInfo.ID = DataProvider.UrlDAO.GetUrlID(guid, domain, out lastActivityDate);

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

                    Hashtable attributes = this.GetHashtable(version, isBeta, domain);
                    string json = JsonMapper.ToJson(attributes);
                    base.Response.ContentType = "text/javascript";
                    base.Response.Write(callback + "(" + json + ")");
                    base.Response.End();
                }
            }
        }

        //{success:'true',fullVersion:'3.6 SP1',pubDate:'2013年3月11日',pageUrl:'http://bbs.siteserver.cn/thread-3.aspx',licenseID:'0'}
        public Hashtable GetHashtable(string version, bool isBeta, string domain)
        {
            Hashtable attributes = new Hashtable();

            DateTime expireDate = DateTime.Now;
            bool isExpire = DataProvider.UrlDAO.IsExpire(domain, out expireDate);
            attributes.Add("isExpire", isExpire.ToString().ToLower());
            attributes.Add("expireDate", expireDate.ToString("yyyy年MM月dd日"));
            if (isExpire && expireDate < DateTime.Now)
            {
                attributes.Add("isForbidden", "true");
            }

            return attributes;
        }
    }
}
