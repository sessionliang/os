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

namespace SiteServer.Project.Pages
{
    public class LicensePage : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            Hashtable attributes = new Hashtable();
            string callback = PageUtils.FilterSql(base.Request.QueryString["callback"]);
            string domain = PageUtils.FilterSql(base.Request.QueryString["domain"]);

            try
            {
                if (!string.IsNullOrEmpty(domain))
                {
                    domain = domain.Replace('\'', ' ');
                    StringBuilder builder = new StringBuilder();
                    ArrayList licenseInfoArrayList = DataProvider.DBLicenseDAO.GetLicenseInfoArrayList(domain);
                    foreach (DBLicenseInfo licenseInfo in licenseInfoArrayList)
                    {
                        string product = string.Empty;

                        if (licenseInfo.MaxSiteNumber == 1)
                        {
                            product = " 单站点版本";
                        }
                        else if (licenseInfo.MaxSiteNumber > 0)
                        {
                            product = string.Format(" 多站点版本（{0}个站点）", licenseInfo.MaxSiteNumber);
                        }
                        else if (licenseInfo.MaxSiteNumber == 0)
                        {
                            product = " 网站群版";
                        }
                        builder.Append(product).Append("<br />");
                    }
                    if (licenseInfoArrayList.Count > 0)
                    {
                        attributes.Add("success", "true");
                        attributes.Add("domain", domain);
                        attributes.Add("products", builder.ToString());
                    }
                }
            }
            catch { }

            if (attributes.Count == 0)
            {
                attributes.Add("success", "false");
            }

            string json = JsonMapper.ToJson(attributes);
            base.Response.ContentType = "text/javascript";
            base.Response.Write(callback + "(" + json + ")");
            base.Response.End();
        }
    }
}
