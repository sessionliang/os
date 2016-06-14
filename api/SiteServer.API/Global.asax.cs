using BaiRong.Core;
using SiteServer.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace SiteServer.API
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/fwlink/?LinkId=301868
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FormatterConfig.RegisterFormatters(GlobalConfiguration.Configuration.Formatters);

            if (FileConfigManager.Instance.Cors)
                GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler());
        }
    }
}
