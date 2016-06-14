using System.Collections.Specialized;
using BaiRong.Core;


namespace BaiRong.BackgroundPages
{
    public class FrameworkLoading : BackgroundBasePage
    {
        protected override bool IsAccessable { get { return true; } }
        protected override bool IsSinglePage { get { return true; } }

        public string GetRedirectUrl()
        {
            string redirectUrl = PageUtils.FilterXSS(StringUtils.ValueToUrl(base.GetQueryString("RedirectUrl"), true));
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                NameValueCollection queryStringOriginal = new NameValueCollection(base.Request.QueryString);
                queryStringOriginal.Remove("RedirectType");
                queryStringOriginal.Remove("RedirectUrl");

                queryStringOriginal.Add(PageUtils.GetQueryString(redirectUrl));

                NameValueCollection queryString = new NameValueCollection();
                foreach (string name in queryStringOriginal.Keys)
                {
                    //filter xss for load page, update by sessionliang 20160112
                    queryString[name] = PageUtils.FilterXSS(queryStringOriginal[name]);
                }

                redirectUrl = PageUtils.GetUrlWithoutQueryString(redirectUrl);
                if (!PageUtils.IsProtocolUrl(redirectUrl) && !redirectUrl.StartsWith("/"))
                {
                    redirectUrl = PageUtils.GetAdminDirectoryUrl(redirectUrl);
                }

                redirectUrl = StringUtils.ValueFromUrl(redirectUrl, true);

                //filter xss for preload page, update by sessionliang 20160112
                //1. get query string with filter xss
                NameValueCollection fxQueryString = PageUtils.GetQueryStringFilterXSS(redirectUrl);
                //2. get url without query string
                redirectUrl = PageUtils.GetUrlWithoutQueryString(redirectUrl);
                //3. combin
                redirectUrl = PageUtils.AddQueryString(redirectUrl, fxQueryString);

                return PageUtils.AddQueryString(redirectUrl, queryString).Replace('"', ' ').Replace('\n', ' ');
            }
            return string.Empty;
        }
    }
}
