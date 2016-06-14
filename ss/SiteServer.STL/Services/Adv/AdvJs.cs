using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using SiteServer.CMS.Core.Security;

using SiteServer.CMS.Core;
using SiteServer.STL.Parser.Model;

namespace SiteServer.CMS.Services
{
    public class AdvJs : BasePage
    {
        public Literal ltlScript;

        public void Page_Load(object sender, System.EventArgs e)
        {  
            string adAreaName = PageUtils.FilterSqlAndXss(base.Request.QueryString["adAreaName"]);

            ETemplateType templateType = ETemplateTypeUtils.GetEnumType(base.Request.QueryString["templateType"]);
            string uniqueID = PageUtils.FilterSqlAndXss(base.Request.QueryString["uniqueID"]);
          
            if (!string.IsNullOrEmpty(adAreaName)&&!string.IsNullOrEmpty(templateType.ToString()))
            {
                AdvInfo advInfo = null;
                if (templateType == ETemplateType.IndexPageTemplate || templateType == ETemplateType.ChannelTemplate || templateType == ETemplateType.ContentTemplate)
                {
                    int channelID = TranslateUtils.ToInt(base.Request.QueryString["channelID"]);

                    advInfo=AdvManager.GetAdvInfoByAdAreaName(templateType,adAreaName, base.PublishmentSystemID, channelID,0);
                }
                else if (templateType == ETemplateType.FileTemplate)
                {
                    int fileTemplateID = TranslateUtils.ToInt(base.Request.QueryString["fileTemplateID"]);

                    advInfo = AdvManager.GetAdvInfoByAdAreaName(templateType, adAreaName, base.PublishmentSystemID,0,fileTemplateID);
                }
                if (advInfo != null)
                {
                    ArrayList adMaterialInfoList = new ArrayList();
                    AdMaterialInfo adMaterialInfo = AdvManager.GetShowAdMaterialInfo(base.PublishmentSystemID, advInfo, out adMaterialInfoList);
                    if (advInfo.RotateType == EAdvRotateType.Equality || advInfo.RotateType == EAdvRotateType.HandWeight)
                    {
                        if (adMaterialInfo.AdMaterialType == EAdvType.HtmlCode)
                        {
                            this.ltlScript.Text = string.Format(@"<!--
document.write('{0}');
-->
", StringUtils.ToJsString(adMaterialInfo.Code));
                        }
                        else if (adMaterialInfo.AdMaterialType == EAdvType.Text)
                        {
                            string style = string.Empty;
                            if (!string.IsNullOrEmpty(adMaterialInfo.TextColor))
                            {
                                style += string.Format("color:{0};", adMaterialInfo.TextColor);
                            }
                            if (adMaterialInfo.TextFontSize > 0)
                            {
                                style += string.Format("font-size:{0}px;", adMaterialInfo.TextFontSize);
                            }
                            this.ltlScript.Text = string.Format(@"<!--
document.write('<a href=""{0}"" target=""_blank"" style=""{1}"">{2}</a>\r\n');
-->
", PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, adMaterialInfo.TextLink)), style, StringUtils.ToJsString(adMaterialInfo.TextWord));
                        }
                        else if (adMaterialInfo.AdMaterialType == EAdvType.Image)
                        {
                            string attribute = string.Empty;
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
                                attribute += string.Format(@" title=""{0}""", adMaterialInfo.ImageAlt);
                            }
                            this.ltlScript.Text = string.Format(@"<!--
document.write('<a href=""{0}"" target=""_blank""><img src=""{1}"" {2} border=""0"" /></a>\r\n');
-->
", PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, adMaterialInfo.ImageLink)), PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, adMaterialInfo.ImageUrl), attribute);
                        }
                        else if (adMaterialInfo.AdMaterialType == EAdvType.Flash)
                        {
                            this.ltlScript.Text = string.Format(@"<!--
document.write('<div id=""flashcontent_{0}""></div>');
var so_{0} = new SWFObject(""{1}"", ""flash_{0}"", ""{2}"", ""{3}"", ""7"", """");

so_{0}.addParam(""quality"", ""high"");
so_{0}.addParam(""wmode"", ""transparent"");

so_{0}.write(""flashcontent_{0}"");
-->
", uniqueID, PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, adMaterialInfo.ImageUrl), adMaterialInfo.ImageWidth.ToString(), adMaterialInfo.ImageHeight.ToString());
                        }
                    }
                }
            }
        }
    }
}
