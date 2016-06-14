using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using SiteServer.CMS.Core.Security;
using System.Collections;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Model;
using System.Text;
using System;

namespace SiteServer.CMS.Core
{
    public class AdvManager
	{ 
        public static AdvInfo GetAdvInfoByAdAreaName(ETemplateType templateType, string adAreaName, int publishmentSystemID, int channelID,int fileTemplateID)
        {
            AdvInfo advInfo=null;
            AdAreaInfo adAreaInfo = DataProvider.AdAreaDAO.GetAdAreaInfo(adAreaName, publishmentSystemID);
            if (adAreaInfo != null)
            {
                if (adAreaInfo.IsEnabled == false)
                {
                    return null;
                }
                ArrayList advInfoList = DataProvider.AdvDAO.GetAdvInfoArrayList(templateType, adAreaInfo.AdAreaID, publishmentSystemID, channelID, fileTemplateID);
                if (advInfoList.Count <= 0)
                {
                    return null;
                }
                advInfo =AdvManager.GetShowAdvInfo(advInfoList);
             }
            return advInfo;
        }
         

        #region 获得轮换广告的HTML

        public static string GetSlideAdvHtml(PublishmentSystemInfo publishmentSystemInfo, AdAreaInfo adAreaInfo, AdvInfo advInfo, ArrayList adMaterialInfoList)
        {
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append(@"<link href=""/SiteFiles/bairong/Styles/Css/slideAdv.css"" rel=""stylesheet"" />");
            strHtml.Append(@"<script src=""/SiteFiles/bairong/JQuery/jquery-1.4.3.min.js""></script>");
            strHtml.AppendFormat(@"
<script type=""text/javascript"">
    var t = n = 0, count;
    $(document).ready(function () {{
        count = $(""#banner_list a"").length;
        $(""#banner_list a:not(:first-child)"").hide();
        $(""#banner_info"").html($(""#banner_list a:first-child"").find(""img"").attr('alt'));
        $(""#banner_info"").click(function () {{
            window.open($(""#banner_list a:first-child"").attr('href'), ""_blank"")
        }});
        $(""#banner li"").click(function () {{
            var i = $(this).text() - 1; 
            n = i;
            if (i >= count) return;
            $(""#banner_info"").html($(""#banner_list a"").eq(i).find(""img"").attr('alt'));
            $(""#banner_info"").unbind().click(function () {{
                window.open($(""#banner_list a"").eq(i).attr('href'),
               ""_blank"")
            }})
            $(""#banner_list a"").filter("":visible"").fadeOut(500).parent().children().eq(i).fadeIn(1000);
            document.getElementById(""banner"").style.background = """";
            $(this).toggleClass(""on"");
            $(this).siblings().removeAttr(""class"");
        }});
        t = setInterval(""showAuto()"", {0}000);
        $(""#banner"").hover(function () {{ clearInterval(t) }}, function () {{
            t = setInterval(""showAuto()"", {0}000);
        }});
    }})
   function showAuto() {{
        n = n >= (count - 1) ? 0 : ++n;
        $(""#banner li"").eq(n).trigger('click');
    }}
</script>
", advInfo.RotateInterval);

            strHtml.AppendFormat(@"<div id=""banner"" style=""width:{0}px;height:{1}px;"">", adAreaInfo.Width,adAreaInfo.Height);
            strHtml.AppendFormat(@"<div id=""banner_bg""></div> <!--标题背景-->");
            strHtml.AppendFormat(@"<div id=""banner_info""></div><ul> <!--标题-->");
            if (adMaterialInfoList.Count == 1)
            {
                AdMaterialInfo info = adMaterialInfoList[0] as AdMaterialInfo;
                strHtml.AppendFormat(@"<li class=""on"">1</li></ul><div id=""banner_list"">");
                strHtml.AppendFormat(AdvManager.GetImageHtml(publishmentSystemInfo,info));
                strHtml.AppendFormat(@"</div></div>");
            }
            else if (adMaterialInfoList.Count > 1)
            {
                int index = 0;
                foreach (AdMaterialInfo info in adMaterialInfoList)
                {
                    index++;
                    if (index == 1)
                    {
                        strHtml.AppendFormat(@"<li class=""on"">1</li>");
                    }
                    else
                    {
                        if (index != adMaterialInfoList.Count)
                        {
                            strHtml.AppendFormat(@"<li>{0}</li>", index);
                        }
                        else
                        {
                            strHtml.AppendFormat(@"<li>{0}</li>", index);
                            strHtml.AppendFormat(@"</ul><div id=""banner_list"">");
                        }
                    }
                }
                foreach (AdMaterialInfo info in adMaterialInfoList)
                {
                    strHtml.AppendFormat(AdvManager.GetImageHtml(publishmentSystemInfo, info));
                }
                strHtml.AppendFormat(@"</div></div>");
            }
           
            return strHtml.ToString();

        }
        #endregion

        #region 获得图片的HTML
        public static string GetImageHtml(PublishmentSystemInfo publishmentSystemInfo ,AdMaterialInfo adMaterialInfo)
        {
            string attribute = string.Empty;
            string imgeHtml = string.Empty;
            if (adMaterialInfo.ImageWidth > 0)
            {
                attribute += string.Format(@" width=""{0}""", adMaterialInfo.ImageWidth);
            }
            if (adMaterialInfo.ImageHeight > 0)
            {
                attribute += string.Format(@" height=""{0}""", adMaterialInfo.ImageHeight);
            }
            if (!string.IsNullOrEmpty(adMaterialInfo.ImageAlt))
            {
                attribute += string.Format(@" alt=""{0}""", adMaterialInfo.ImageAlt);
                attribute += string.Format(@" title=""{0}""" , adMaterialInfo.ImageAlt);
            }
            imgeHtml = string.Format(@"
<a href=""{0}"" target=""_blank""><img src=""{1}"" {2} border=""0"" /></a>

", PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, adMaterialInfo.ImageLink)), PageUtility.ParseNavigationUrl(publishmentSystemInfo, adMaterialInfo.ImageUrl), attribute);
            return imgeHtml;
        }

        #endregion

        #region 获得展示的广告
        /*
         优先级：独占大于标准
         独占：不设置权重，按开始时间正序取第一条；设置权重，取权重最大的一条
         标准：一个等级相当于10个权重值，随机获取，等级和权重高的获取的几率比较大
         */
        public static AdvInfo GetShowAdvInfo(ArrayList advInfoList)
        {
            int index = 0;
            int maxWeight  = 0;
            bool isSetWeight = false;
            AdvInfo advInfo = null;
            ArrayList advIDList = new ArrayList();
            Hashtable advWeights = new Hashtable();
            ArrayList holdAdvInfoList = new ArrayList();
            ArrayList standardAdvInfoList = new ArrayList();
            foreach (AdvInfo info in advInfoList)
            {
                index++;
                if (info.IsDateLimited)
                {
                    if (info.StartDate > DateTime.Now || info.EndDate < DateTime.Now)
                    {
                        continue;
                    }
                }
                if (info.LevelType == EAdvLevelType.Hold)
                {
                    if (info.IsWeight == true)
                    {
                        isSetWeight = true;
                        if (info.Weight  > maxWeight )
                        {
                            maxWeight  = info.Weight ;
                            advWeights.Add(maxWeight , index);
                        }
                    }
                    holdAdvInfoList.Add(info);
                }
                else if (info.LevelType == EAdvLevelType.Standard)
                {
                    int k = 0;
                    if (info.IsWeight == false)
                    {
                        k = info.Level * 10 + info.Weight ;
                    }
                    else
                    {
                        k = info.Level * 10;
                    }
                    for (int j = 0; j < k; j++)
                    {
                        advIDList.Add(info.AdvID);
                    }
                    standardAdvInfoList.Add(info);
                }
            }
            if (holdAdvInfoList.Count > 0)
            {
                if (isSetWeight == false)
                {
                    advInfo = holdAdvInfoList[0] as AdvInfo;
                }
                else
                {
                    advInfo = holdAdvInfoList[(int)advWeights[maxWeight ] - 1] as AdvInfo;
                }
            }
            else
            {
                if (standardAdvInfoList.Count > 0)
                {
                    Random rd = new Random();
                    int index1 = rd.Next(0, advIDList.Count);
                    foreach (AdvInfo info in standardAdvInfoList)
                    {
                        if (info.AdvID == (int)advIDList[index1])
                        {
                            advInfo = info;
                        }
                    }
                }
            }
            return advInfo;
        }
        #endregion

        #region 获得展示的广告物料
        /*
         广告物料轮换
         均匀：随机获取广告物料，几率相等
         手动权重：去权重值最大的，权重相等则取最新的一条
         幻灯片轮换：图片大于1时有效,
        */
        public static AdMaterialInfo GetShowAdMaterialInfo(int publishmentSystemID, AdvInfo advInfo, out ArrayList adMaterialInfoList)
        {
            AdMaterialInfo adMaterialInfo = null;
            adMaterialInfoList = new ArrayList();
            ArrayList adMaterialInfoArrayList = DataProvider.AdMaterialDAO.GetAdMaterialInfoArrayList(advInfo.AdvID, publishmentSystemID);
            if (adMaterialInfoArrayList.Count > 0)
            {
                if (advInfo.RotateType == EAdvRotateType.Equality)
                {
                    Random rd = new Random();
                    int index = rd.Next(0, adMaterialInfoArrayList.Count);
                    adMaterialInfo = adMaterialInfoArrayList[index] as AdMaterialInfo;
                }
                else if (advInfo.RotateType == EAdvRotateType.HandWeight)
                {
                    int index = 0;
                    int maxWeight  = 0;
                    Hashtable adMaterialIDList = new Hashtable();
                    foreach (AdMaterialInfo info in adMaterialInfoArrayList)
                    {
                        index++;
                        if (info.Weight  > maxWeight )
                        {
                            maxWeight  = info.Weight ;
                            adMaterialIDList.Add(maxWeight , index);
                        }
                    }
                    adMaterialInfo = adMaterialInfoArrayList[(int)adMaterialIDList[maxWeight ] - 1] as AdMaterialInfo;
                }
                else if (advInfo.RotateType == EAdvRotateType.SlideRotate)
                {
                    foreach (AdMaterialInfo info in adMaterialInfoArrayList)
                    {
                        if (info.AdMaterialType == EAdvType.Image)
                        {
                            adMaterialInfoList.Add(info);
                        }
                    }
                }
            }
            return adMaterialInfo;
        }
        #endregion
         
	}
}
