using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages
{
   public class BackgroundShareRegister : BackgroundBasePage
    {
       public TextBox email;
       public TextBox password;
       public TextBox domain;

       public void Page_Load(object sender, System.EventArgs e)
       {
           if (base.IsForbidden) return;

           PageUtils.CheckRequestParameter("PublishmentSystemID");

           if (!IsPostBack)
           {
               base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_BShare, "插件设置", AppManager.CMS.Permission.WebSite.BShare);
           }
       }
       public void btnOpenShare_Click(object sender, EventArgs e)
       {
           try
           {
               string uuid = "";
               string email = this.email.Text;
               string password = this.password.Text;
               string domain = this.domain.Text;
               string url = "http://api.bshare.cn/analytics/reguuid.json?email=" + email + "&password=" + password + "&domain=" + domain + "";
               ECharset charset = ECharset.utf_8;
               string json = BaiRong.Core.Net.WebClientUtils.GetRemoteFileSource(url, charset);
               NameValueCollection attributes = TranslateUtils.ParseJsonStringToNameValueCollection(json);
               foreach (string key in attributes.Keys)
               {
                   uuid += attributes[key] + ",";

               }

               string[] array = uuid.Split(',');
               base.PublishmentSystemInfo.Additional.BshareUUID = array[0];
               base.PublishmentSystemInfo.Additional.BshareSecret = array[1];
               base.PublishmentSystemInfo.Additional.BshareUserName = this.email.Text;
               base.PublishmentSystemInfo.Additional.BsharePassword = this.password.Text;
               DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
               string PublishmentSystemID = Request["PublishmentSystemID"];
               Response.Redirect(PageUtils.GetCMSUrl("background_shareSet.aspx?PublishmentSystemID=" + PublishmentSystemID));
           }
           catch(Exception ex)
           {
               base.FailMessage(ex, "您输入的网站域名已注册,但密码错误");
           }
       }
   
    }
}
