using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlAd
    {
        private StlAd() { }
        public const string ElementName = "stl:ad";//固定广告

        public const string Attribute_Area = "area";			//广告位名称

        public const string Attribute_adName = "adname";        //4.0版本之前（不包含4.0）使用

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_Area, "广告位名称");
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                string adAreaName = string.Empty;
                string adName = string.Empty;
                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Attribute_Area))
                    {
                        adAreaName = attr.Value;
                    }
                    else if (attributeName.Equals(Attribute_adName))
                    {
                        adName = attr.Value;
                    }
                }

                //兼容4.0版本之前（不包含4.0）
                if (string.IsNullOrEmpty(adAreaName) && !string.IsNullOrEmpty(adName))
                    adAreaName = adName;

                AdAreaInfo adAreaInfo = DataProvider.AdAreaDAO.GetAdAreaInfo(adAreaName, pageInfo.PublishmentSystemID);
                if (adAreaInfo != null)
                {
                    if (adAreaInfo.IsEnabled)
                    {
                        pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ac_SWFObject);

                        ArrayList adMaterialInfoList = new ArrayList();
                        AdvInfo advInfo = AdvManager.GetAdvInfoByAdAreaName(pageInfo.TemplateInfo.TemplateType, adAreaInfo.AdAreaName, pageInfo.PublishmentSystemID, pageInfo.PageNodeID, pageInfo.TemplateInfo.TemplateID);
                        if (advInfo != null)
                        {
                            if (advInfo.RotateType == EAdvRotateType.Equality || advInfo.RotateType == EAdvRotateType.HandWeight)
                            {
                                ETemplateType templateType = pageInfo.TemplateInfo.TemplateType;
                                if (templateType == ETemplateType.IndexPageTemplate || templateType == ETemplateType.ChannelTemplate || templateType == ETemplateType.ContentTemplate)
                                {
                                    //parsedContent = string.Format("<script src='{0}' language='javascript'></script>", PageUtility.Services.GetUrl(string.Format("adv/js.aspx?publishmentSystemID={0}&uniqueID={1}&adAreaName={2}&channelID={3}&templateType={4}", pageInfo.PublishmentSystemID, pageInfo.UniqueID, adAreaName, pageInfo.PageNodeID, pageInfo.TemplateInfo.TemplateType)));
                                    parsedContent = string.Format("<script src='{0}' language='javascript'></script>", PageUtility.Services.GetUrlOfAdHtml(pageInfo.PublishmentSystemInfo, pageInfo.UniqueID, adAreaName, pageInfo.PageNodeID, 0, pageInfo.TemplateInfo.TemplateType.ToString()));
                                }
                                else if (templateType == ETemplateType.FileTemplate)
                                {
                                    //parsedContent = string.Format("<script src='{0}' language='javascript'></script>", PageUtility.Services.GetUrl(string.Format("adv/js.aspx?publishmentSystemID={0}&uniqueID={1}&adAreaName={2}&fileTemplateID={3}&templateType={4}", pageInfo.PublishmentSystemID, pageInfo.UniqueID, adAreaName, pageInfo.TemplateInfo.TemplateID, pageInfo.TemplateInfo.TemplateType)));
                                    parsedContent = string.Format("<script src='{0}' language='javascript'></script>", PageUtility.Services.GetUrlOfAdHtml(pageInfo.PublishmentSystemInfo, pageInfo.UniqueID, adAreaName, 0, pageInfo.TemplateInfo.TemplateID, pageInfo.TemplateInfo.TemplateType.ToString()));
                                }
                            }
                            else if (advInfo.RotateType == EAdvRotateType.SlideRotate)
                            {
                                AdMaterialInfo adMaterialInfo = AdvManager.GetShowAdMaterialInfo(pageInfo.PublishmentSystemID, advInfo, out adMaterialInfoList);
                                parsedContent = string.Format(@"{0}", AdvManager.GetSlideAdvHtml(pageInfo.PublishmentSystemInfo, adAreaInfo, advInfo, adMaterialInfoList));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }
    }
}
