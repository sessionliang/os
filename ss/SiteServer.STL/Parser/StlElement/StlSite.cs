using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using SiteServer.STL.Parser.StlEntity;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlSite
	{
        private StlSite() { }
		public const string ElementName = "stl:site";       //��ʾָ��Ӧ�õ�����

        public const string Attribute_SiteName = "sitename";				//Ӧ������
        public const string Attribute_Directory = "directory";				//Ӧ���ļ���
        public const string Attribute_IsDynamic = "isdynamic";              //�Ƿ�̬��ʾ

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_SiteName, "Ӧ������");
                attributes.Add(Attribute_Directory, "Ӧ���ļ���");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");
				return attributes;
			}
		}


        //ѭ�������ͱ�ǩ
        internal static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfoRef)
		{
			string parsedContent = string.Empty;
            
			try
			{
                if (!string.IsNullOrEmpty(node.InnerXml))
                {
                    ContextInfo contextInfo = contextInfoRef.Clone();
                    IEnumerator ie = node.Attributes.GetEnumerator();
                    string siteName = string.Empty;
                    string directory = string.Empty;
                    bool isDynamic = false;

                    while (ie.MoveNext())
                    {
                        XmlAttribute attr = (XmlAttribute)ie.Current;
                        string attributeName = attr.Name.ToLower();
                        if (attributeName.Equals(StlSite.Attribute_SiteName))
                        {
                            siteName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        }
                        else if (attributeName.Equals(StlSite.Attribute_Directory))
                        {
                            directory = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        }
                        else if (attributeName.Equals(StlSite.Attribute_IsDynamic))
                        {
                            isDynamic = TranslateUtils.ToBool(attr.Value);
                        }
                    }

                    if (isDynamic)
                    {
                        parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                    }
                    else
                    {
                        parsedContent = ParseImpl(node, pageInfo, contextInfo, siteName, directory);
                    }
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, string siteName, string directory)
        {
            string parsedContent = string.Empty;

            PublishmentSystemInfo publishmentSystemInfo = null;

            if (!string.IsNullOrEmpty(siteName))
            {
                publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfoBySiteName(siteName);
            }
            else if (!string.IsNullOrEmpty(directory))
            {
                publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfoByDirectory(directory);
            }
            else
            {
                int siteID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();
                if (siteID > 0)
                {
                    publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(siteID);
                }
            }

            if (publishmentSystemInfo != null)
            {
                PublishmentSystemInfo prePublishmentSystemInfo = pageInfo.PublishmentSystemInfo;
                int prePageNodeID = pageInfo.PageNodeID;
                int prePageContentID = pageInfo.PageContentID;
                EVisualType visualType = pageInfo.VisualType;

                pageInfo.ChangeSite(publishmentSystemInfo, publishmentSystemInfo.PublishmentSystemID, 0, contextInfo, publishmentSystemInfo.Additional.VisualType);

                StringBuilder innerBuilder = new StringBuilder(node.InnerXml);
                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                parsedContent = innerBuilder.ToString();

                pageInfo.ChangeSite(prePublishmentSystemInfo, prePageNodeID, prePageContentID, contextInfo, visualType);
            }

            return parsedContent;
        }
	}
}
