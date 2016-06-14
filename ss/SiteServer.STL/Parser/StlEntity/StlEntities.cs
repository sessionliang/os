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

        public const string EntityName = "Stl";                  //通用实体

        public static string PoweredBy = "PoweredBy";         //PoweredBy 链接
        public static string SiteName = "SiteName";           //应用名称
        public static string SiteID = "SiteID";               //应用ID
        public static string SiteDir = "SiteDir";             //应用文件夹
        public static string SiteUrl = "SiteUrl";             //应用根目录地址
		public static string RootUrl = "RootUrl";             //系统根目录地址
        public static string HomeUrl = "HomeUrl";             //用户中心地址
        public static string CurrentUrl = "CurrentUrl";       //当前页地址
        public static string ChannelUrl = "ChannelUrl";       //栏目页地址

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(PoweredBy, "PoweredBy 链接");
                attributes.Add(SiteName, "应用名称");
                attributes.Add(SiteID, "应用ID");
                attributes.Add(SiteDir, "应用文件夹");
                attributes.Add(SiteUrl, "应用根目录地址");
                attributes.Add(RootUrl, "系统根目录地址");
                attributes.Add(HomeUrl, "用户中心地址");
                attributes.Add(CurrentUrl, "当前页地址");
                attributes.Add(ChannelUrl, "栏目页地址");
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

                if (StringUtils.EqualsIgnoreCase(StlEntities.PoweredBy, attributeName))//支持信息
                {
                    parsedContent = string.Format(@"Powered by <a href=""http://www.siteserver.cn"" target=""_blank"">{0}</a>", EPublishmentSystemTypeUtils.GetAppName(pageInfo.PublishmentSystemInfo.PublishmentSystemType));
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.RootUrl, attributeName))//系统根目录地址
                {
                    parsedContent = PageUtils.ParseConfigRootUrl("~");
                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = parsedContent.TrimEnd('/');
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.HomeUrl, attributeName))//用户中心地址
                {
                    parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, pageInfo.PublishmentSystemInfo.Additional.HomeUrl).TrimEnd('/');
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.SiteID, attributeName))//频道ID
                {
                    parsedContent = pageInfo.PublishmentSystemID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.SiteName, attributeName))//频道名称
                {
                    parsedContent = pageInfo.PublishmentSystemInfo.PublishmentSystemName;
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.SiteUrl, attributeName))//频道域名地址
                {
                    if (!string.IsNullOrEmpty(pageInfo.PublishmentSystemInfo.PublishmentSystemUrl))
                    {
                        parsedContent = pageInfo.PublishmentSystemInfo.PublishmentSystemUrl.TrimEnd('/');
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.SiteDir, attributeName))//频道文件夹
                {
                    parsedContent = pageInfo.PublishmentSystemInfo.PublishmentSystemDir;
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.CurrentUrl, attributeName))//当前页地址
                {
                    parsedContent = StlUtility.GetStlCurrentUrl(pageInfo, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ContentInfo);
                }
                else if (StringUtils.EqualsIgnoreCase(StlEntities.ChannelUrl, attributeName))//栏目页地址
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
