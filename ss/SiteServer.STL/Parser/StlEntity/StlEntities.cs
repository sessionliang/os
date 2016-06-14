using System.Collections;
using System.Collections.Specialized;
using System.Text;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Model;

namespace SiteServer.STL.Parser.StlEntity
{
    public class StlEntities
	{
		private StlEntities()
		{
		}

        public const string EntityName = "Stl";                  //ͨ��ʵ��

        public static string PoweredBy = "PoweredBy";         //PoweredBy ����
        public static string SiteName = "SiteName";           //Ӧ������
        public static string SiteID = "SiteID";               //Ӧ��ID
        public static string SiteDir = "SiteDir";             //Ӧ���ļ���
        public static string SiteUrl = "SiteUrl";             //Ӧ�ø�Ŀ¼��ַ
		public static string RootUrl = "RootUrl";             //ϵͳ��Ŀ¼��ַ
        public static string HomeUrl = "HomeUrl";             //�û����ĵ�ַ
        public static string CurrentUrl = "CurrentUrl";       //��ǰҳ��ַ
        public static string ChannelUrl = "ChannelUrl";       //��Ŀҳ��ַ

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(PoweredBy, "PoweredBy ����");
                attributes.Add(SiteName, "Ӧ������");
                attributes.Add(SiteID, "Ӧ��ID");
                attributes.Add(SiteDir, "Ӧ���ļ���");
                attributes.Add(SiteUrl, "Ӧ�ø�Ŀ¼��ַ");
                attributes.Add(RootUrl, "ϵͳ��Ŀ¼��ַ");
                attributes.Add(HomeUrl, "�û����ĵ�ַ");
                attributes.Add(CurrentUrl, "��ǰҳ��ַ");
                attributes.Add(ChannelUrl, "��Ŀҳ��ַ");
                return attributes;
            }
        }

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                string entityName = StlParserUtility.GetNameFromEntity(stlEntity);
                string attributeName = entityName.Substring(5, entityName.Length - 6);

                if (StringUtils.EqualsIgnoreCase(StlEntities.PoweredBy, attributeName))//֧����Ϣ
                {
                    parsedContent = string.Format(@"Powered by <a href=""http://www.siteserver.cn"" target=""_blank"">{0}</a>", EPublishmentSystemTypeUtils.GetAppName(pageInfo.PublishmentSystemInfo.PublishmentSystemType));
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.RootUrl, attributeName))//ϵͳ��Ŀ¼��ַ
                {
                    parsedContent = PageUtils.ParseConfigRootUrl("~");
                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = parsedContent.TrimEnd('/');
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.HomeUrl, attributeName))//�û����ĵ�ַ
                {
                    parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, pageInfo.PublishmentSystemInfo.Additional.HomeUrl).TrimEnd('/');
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.SiteID, attributeName))//Ƶ��ID
                {
                    parsedContent = pageInfo.PublishmentSystemID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.SiteName, attributeName))//Ƶ������
                {
                    parsedContent = pageInfo.PublishmentSystemInfo.PublishmentSystemName;
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.SiteUrl, attributeName))//Ƶ��������ַ
                {
                    if (!string.IsNullOrEmpty(pageInfo.PublishmentSystemInfo.PublishmentSystemUrl))
                    {
                        parsedContent = pageInfo.PublishmentSystemInfo.PublishmentSystemUrl.TrimEnd('/');
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.SiteDir, attributeName))//Ƶ���ļ���
                {
                    parsedContent = pageInfo.PublishmentSystemInfo.PublishmentSystemDir;
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.CurrentUrl, attributeName))//��ǰҳ��ַ
                {
                    parsedContent = StlUtility.GetStlCurrentUrl(pageInfo, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ContentInfo);
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.ChannelUrl, attributeName))//��Ŀҳ��ַ
                {
                    parsedContent = PageUtility.GetChannelUrl(pageInfo.PublishmentSystemInfo, NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contextInfo.ChannelID), pageInfo.VisualType);
                }
                else if (StringUtils.StartsWithIgnoreCase(attributeName, "TableFor"))//
                {
                    if (StringUtils.EqualsIgnoreCase(attributeName, "TableForContent"))
                    {
                        parsedContent = pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent;
                    }
                    else if (StringUtils.EqualsIgnoreCase(attributeName, "TableForGovInteract"))
                    {
                        parsedContent = pageInfo.PublishmentSystemInfo.AuxiliaryTableForGovInteract;
                    }
                    else if (StringUtils.EqualsIgnoreCase(attributeName, "TableForGovPublic"))
                    {
                        parsedContent = pageInfo.PublishmentSystemInfo.AuxiliaryTableForGovPublic;
                    }
                    else if (StringUtils.EqualsIgnoreCase(attributeName, "TableForJob"))
                    {
                        parsedContent = pageInfo.PublishmentSystemInfo.AuxiliaryTableForJob;
                    }
                    else if (StringUtils.EqualsIgnoreCase(attributeName, "TableForVote"))
                    {
                        parsedContent = pageInfo.PublishmentSystemInfo.AuxiliaryTableForVote;
                    }
                }
                else if (StringUtils.StartsWithIgnoreCase(attributeName, "Site"))//
                {
                    parsedContent = pageInfo.PublishmentSystemInfo.Additional.GetExtendedAttribute(attributeName.Substring(4));
                }
                else if (StringUtils.StartsWithIgnoreCase(attributeName, "template."))//
                {
                    parsedContent = string.Format("<%={0}%>", attributeName.Substring(9));
                }
                else
                {
                    if (pageInfo.PublishmentSystemInfo.Additional.ContainsKey(attributeName))
                    {
                        parsedContent = pageInfo.PublishmentSystemInfo.Additional.GetExtendedAttribute(attributeName);
                        if (parsedContent.StartsWith("@"))
                        {
                            parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemID, parsedContent);
                        }
                    }
                    else
                    {
                        StlTagInfo stlTagInfo = DataProvider.StlTagDAO.GetStlTagInfo(pageInfo.PublishmentSystemID, attributeName);
                        if (stlTagInfo == null)
                        {
                            stlTagInfo = DataProvider.StlTagDAO.GetStlTagInfo(0, attributeName);
                        }
                        if (stlTagInfo != null && !string.IsNullOrEmpty(stlTagInfo.TagContent))
                        {
                            StringBuilder innerBuilder = new StringBuilder(stlTagInfo.TagContent);
                            StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                            parsedContent = innerBuilder.ToString();
                        }
                    }
                }
            }
            catch { }

            return parsedContent;
        }
	}
}
