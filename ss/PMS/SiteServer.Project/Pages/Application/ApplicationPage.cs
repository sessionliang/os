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
    public class ApplicationPage : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            Hashtable attributes = new Hashtable();
            string callback = PageUtils.FilterSql(base.Request.QueryString["callback"]);

            try
            {
                EApplicationType applicationType = EApplicationTypeUtils.GetEnumType(base.Request.QueryString["applicationType"]);
                string applyResource = PageUtils.FilterSql(base.Request.QueryString["applyResource"]);
                string ipAddress = PageUtils.GetIPAddress();
                DateTime addDate = DateTime.Now;
                string contactPerson = PageUtils.FilterSql(base.Request.QueryString["contactPerson"]);
                string email = PageUtils.FilterSql(base.Request.QueryString["email"]);
                string mobile = PageUtils.FilterSql(base.Request.QueryString["mobile"]);
                string qq = PageUtils.FilterSql(base.Request.QueryString["qq"]);
                string telephone = PageUtils.FilterSql(base.Request.QueryString["telephone"]);
                string location = PageUtils.FilterSql(base.Request.QueryString["location"]);
                string address = PageUtils.FilterSql(base.Request.QueryString["address"]);
                string orgType = PageUtils.FilterSql(base.Request.QueryString["orgType"]);
                string orgName = PageUtils.FilterSql(base.Request.QueryString["orgName"]);
                bool isITDepartment = TranslateUtils.ToBool(base.Request.QueryString["isITDepartment"]);
                string comment = PageUtils.FilterSql(base.Request.QueryString["comment"]);

                if (!string.IsNullOrEmpty(contactPerson))
                {
                    ApplicationInfo applicationInfo = new ApplicationInfo(0, applicationType, applyResource, ipAddress, addDate, contactPerson, email, mobile, qq, telephone, location, address, orgType, orgName, isITDepartment, comment, false, DateUtils.SqlMinValue, string.Empty, string.Empty);

                    DataProvider.ApplicationDAO.Insert(applicationInfo);

                    attributes.Add("success", "true");
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
