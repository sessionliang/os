using System;
using System.Collections;
using System.Text;
using System.Data;
using SiteServer.BBS.Model;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.BBS.Core
{
    //public class ArrayListCompare : IComparer
    //{
    //    Random r = new Random();

    //    int Compare(object x, object y)
    //    {
    //        if (x.Equals(y)) return 0;
    //        return r.Next(-1, 1);
    //    }
    //}

    public class AdManager
    {
        public static void RemoveCache(int publishmentSystemID)
        {
            string cacheKey = GetCacheKey(publishmentSystemID);
            CacheUtils.Remove(cacheKey);
        }

        private static readonly object lockObject = new object();
        private static string GetCacheKey(int publishmentSystemID)
        {
            return string.Format("SiteServer.BBS.Core.AdManager.{0}", publishmentSystemID);
        }

        public static ArrayList GetAdInfoArrayList(int publishmentSystemID)
        {
            lock (lockObject)
            {
                string cacheKey = GetCacheKey(publishmentSystemID);
                if (CacheUtils.Get(cacheKey) == null)
                {
                    ArrayList arraylist = DataProvider.AdDAO.GetAdInfoArrayList(publishmentSystemID);
                    CacheUtils.Insert(cacheKey, arraylist, 3600);
                    return arraylist;
                }
                return CacheUtils.Get(cacheKey) as ArrayList;
            }
        }

        public static string GetAdContents(int publishmentSystemID)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                ArrayList arraylist = AdManager.GetAdInfoArrayList(publishmentSystemID);
                //IComparer sort = new ArrayListCompare();
                //arraylist.Sort(sort);

                Hashtable hashtable = new Hashtable();

                foreach (AdInfo adInfo in arraylist)
                {
                    if (adInfo.IsEnabled == false)
                    {
                        continue;
                    }
                    if (adInfo.IsDateLimited)
                    {
                        if (adInfo.StartDate > DateTime.Now || adInfo.EndDate < DateTime.Now)
                        {
                            continue;
                        }
                    }
                    if (hashtable[adInfo.AdLocation] == null)
                    {
                        string adContent = AdManager.GetAdContent(adInfo);
                        builder.AppendFormat(@"<div id=""ad_{0}_nodisplay"" style=""display:none"">{1}</div><script type=""text/javascript"">try{{
document.getElementById('ad_{0}').innerHTML = document.getElementById('ad_{0}_nodisplay').innerHTML;
document.getElementById('ad_{0}_nodisplay').parentNode.removeChild(document.getElementById('ad_{0}_nodisplay'));
}}catch(e){{}}</script>", EAdLocationUtils.GetValue(adInfo.AdLocation).ToLower(), adContent);

                        hashtable[adInfo.AdLocation] = adInfo.AdName;
                    }
                }
            }
            catch { }

            return builder.ToString();
        }

        private static string GetAdContent(AdInfo adInfo)
        {
            string retval = string.Empty;

            if (adInfo != null)
            {
                if (adInfo.AdType == EAdType.HtmlCode)
                {
                    retval = adInfo.Code;
                }
                else if (adInfo.AdType == EAdType.Text)
                {
                    string style = string.Empty;
                    if (!string.IsNullOrEmpty(adInfo.TextColor))
                    {
                        style += string.Format("color:{0};", adInfo.TextColor);
                    }
                    if (adInfo.TextFontSize > 0)
                    {
                        style += string.Format("font-size:{0}px;", adInfo.TextFontSize);
                    }
                    retval = string.Format(@"
<a href=""{0}"" target=""_blank"" style=""{1}"">{2}</a>
", PageUtils.AddProtocolToUrl(PageUtils.ParseNavigationUrl(adInfo.TextLink)), style, StringUtils.ToJsString(adInfo.TextWord));
                }
                else if (adInfo.AdType == EAdType.Image)
                {
                    string attribute = string.Empty;
                    if (adInfo.ImageWidth > 0)
                    {
                        attribute += string.Format(@" width=""{0}""", adInfo.ImageWidth);
                    }
                    if (adInfo.ImageHeight > 0)
                    {
                        attribute += string.Format(@" height=""{0}""", adInfo.ImageHeight);
                    }
                    if (!string.IsNullOrEmpty(adInfo.ImageAlt))
                    {
                        attribute += string.Format(@" alt=""{0}""", adInfo.ImageAlt);
                    }
                    retval = string.Format(@"
<a href=""{0}"" target=""_blank""><img src=""{1}"" {2} border=""0"" /></a>
", PageUtils.AddProtocolToUrl(PageUtils.ParseNavigationUrl(adInfo.ImageLink)), PageUtils.ParseNavigationUrl(adInfo.ImageUrl), attribute);
                }
                else if (adInfo.AdType == EAdType.Flash)
                {
                    retval = string.Format(@"
<div id=""flashcontent_{0}""></div>
<script>
var so_{0} = new SWFObject(""{1}"", ""flash_{0}"", ""{2}"", ""{3}"", ""7"", """");

so_{0}.addParam(""quality"", ""high"");
so_{0}.addParam(""wmode"", ""transparent"");

so_{0}.write(""flashcontent_{0}"");
</script>
-->
", adInfo.AdName, PageUtils.ParseNavigationUrl(adInfo.ImageUrl), adInfo.ImageWidth.ToString(), adInfo.ImageHeight.ToString());
                }
            }

            return retval;
        }
    }
}
