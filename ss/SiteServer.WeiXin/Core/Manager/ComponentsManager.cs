using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using SiteServer.WeiXin.Model;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using Senparc.Weixin.MP.Entities;

namespace SiteServer.WeiXin.Core
{
    public class ComponentsManager
    {
        public static string GetBackgroundImageUrl(PublishmentSystemInfo publishmentSystemInfo, string backgroundImageUrl)
        {
            if (string.IsNullOrEmpty(backgroundImageUrl))
            {
                backgroundImageUrl = "1.jpg";
            }
            if (!backgroundImageUrl.StartsWith("@"))
            {
                return APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl(string.Format("services/weixin/components/background/{0}", backgroundImageUrl))));
            }
            else
            {
                return PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, backgroundImageUrl));
            }
        }

        public static string GetBackgroundImageSelectHtml(string backgroundImageUrl)
        {
            StringBuilder builder = new StringBuilder(@"<select id=""backgroundSelect"" onchange=""if ($(this).val()){$('#preview_backgroundImageUrl').attr('src', $(this).val());$('#backgroundImageUrl').val($(this).find('option:selected').attr('url'));}""><option value="""">选择预设背景</option>");
            if (string.IsNullOrEmpty(backgroundImageUrl))
            {
                backgroundImageUrl = "0.jpg";
            }
            for (int i = 0; i <= 20; i++)
            {
                string fileName = i + ".jpg";
                string selected = string.Empty;
                if (fileName == backgroundImageUrl)
                {
                    selected = "selected";
                }
                builder.AppendFormat(@"<option value=""{0}"" url=""{1}"" {2}>{3}</option>", APIPageUtils.ParseUrl(PageUtils.AddProtocolToUrl(PageUtils.GetSiteFilesUrl(string.Format("services/weixin/components/background/{0}.jpg", i)))), fileName, selected, "预设背景" + (i + 1));
            }
            builder.Append("</select>");
            return builder.ToString();
        }
	}
}
