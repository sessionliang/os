using System;
using System.Security.Principal;
using System.Threading;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;

using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Core.UrlRewriting;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace BaiRong.Provider.HttpModule
{
    public class SiteServerHttpModule : IHttpModule
    {
        public String ModuleName
        {
            get { return "SiteServerHttpModule"; }
        }

        public SiteServerHttpModule()
        {
        }

        public void Init(HttpApplication app)
        {
            app.AuthorizeRequest += this.application_Rewriter;
            app.Error += this.application_Error;
            app.PreSendRequestHeaders += this.app_PreSendRequestHeaders;
        }

        /// <summary>
        /// 去除Response.Header的Server信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void app_PreSendRequestHeaders(object sender, EventArgs e)
        {
            if (HttpContext.Current != null && HttpContext.Current.Response != null)
                HttpContext.Current.Response.Headers.Remove("Server");
        }

        private void application_Error(object sender, EventArgs e)
        {
            try
            {
                Exception ex = HttpContext.Current.Server.GetLastError();
                if (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                LogUtils.AddErrorLog(ex, "Application Error");
                HttpContext.Current.Server.ClearError();

                string pageUrl = PageUtils.GetAdminDirectoryUrl(string.Format("error.aspx?ErrorMessage={0}", HttpUtility.UrlEncode(ex.Message)));
                if (FileConfigManager.Instance.IsSaas)
                {
                    pageUrl = PageUtils.GetAdminDirectoryUrl("error.html");
                }

                PageUtils.RedirectToErrorPage(ex.Message);
            }
            catch { }
        }

        public void Dispose()
        {
        }

        private void application_Rewriter(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            Rewrite(application);
        }

        protected void Rewrite(HttpApplication app)
        {
            UrlRewriteManager manager = UrlRewriteManager.Instance;
            if (manager != null && manager.IsRewriter)
            {
                string requestedPath = app.Request.Path;
                NameValueCollection queryString = app.Request.QueryString;
                requestedPath = requestedPath + ((queryString != null && queryString.Count > 0) ? "?" + queryString.ToString() : string.Empty);

                for (int i = 0; i < manager.RewriterRules.Count; i++)
                {
                    string lookFor = "^" + RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, manager.RewriterRules[i].LookFor) + "$";

                    Regex re = new Regex(lookFor, RegexOptions.IgnoreCase);

                    if (re.IsMatch(requestedPath))
                    {
                        if (manager.RewriterRules[i].IsRewriting)
                        {
                            string sendToUrl = RewriterUtils.ResolveUrl(app.Context.Request.ApplicationPath, re.Replace(requestedPath, manager.RewriterRules[i].SendTo));

                            RewriterUtils.RewriteUrl(app.Context, sendToUrl);
                        }
                        break;
                    }
                }
            }
        }
    }

}
