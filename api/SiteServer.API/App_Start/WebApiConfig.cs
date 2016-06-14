using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiContrib.Formatting.Jsonp;

namespace SiteServer.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}/{format}",
                defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional, format = RouteParameter.Optional }
            );
        }
    }
}
