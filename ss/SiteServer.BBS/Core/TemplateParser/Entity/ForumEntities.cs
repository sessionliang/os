using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Model;
using SiteServer.BBS.Pages;

namespace SiteServer.BBS.Core.TemplateParser.Entity
{
    public class ForumEntities
	{
        private ForumEntities()
		{
		}

        public static string ForumID = "ForumID";//板块ID
        public static string ForumName = "ForumName";//板块名称
        public static string ForumIndex = "ForumIndex";//板块索引
        public static string Title = "Title";//板块名称
        public static string Content = "Content";//板块正文
        public static string NavigationUrl = "NavigationUrl";//板块链接地址
        public static string IconUrl = "IconUrl";//板块图片地址
        public static string AddDate = "AddDate";//板块添加日期
        public static string DirectoryName = "DirectoryName";//生成文件夹名称
        public static string ItemIndex = "ItemIndex";//板块排序

        internal static string Parse(string entity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            try
            {
                string entityName = ParserUtility.GetNameFromEntity(entity);
                string forumIndex = ParserUtility.GetForumIndexFromEntity(entity);
                string attributeName = entityName.Substring(11, entityName.Length - 12);

                int upLevel = 0;
                int topLevel = -1;
                int forumID = contextInfo.ForumID;
                if (!string.IsNullOrEmpty(forumIndex))
                {
                    forumID = DataProvider.ForumDAO.GetForumIDByIndexName(pageInfo.PublishmentSystemID, forumIndex);
                    if (forumID == 0)
                    {
                        forumID = contextInfo.ForumID;
                    }
                }
                
                if (attributeName.ToLower().StartsWith("up") && attributeName.IndexOf(".") != -1)
                {
                    if (attributeName.ToLower().StartsWith("up."))
                    {
                        upLevel = 1;
                    }
                    else
                    {
                        string upLevelStr = attributeName.Substring(2, attributeName.IndexOf(".") - 2);
                        upLevel = TranslateUtils.ToInt(upLevelStr);
                    }
                    topLevel = -1;
                    attributeName = attributeName.Substring(attributeName.IndexOf(".") + 1);
                }
                else if (attributeName.ToLower().StartsWith("top") && attributeName.IndexOf(".") != -1)
                {
                    if (attributeName.ToLower().StartsWith("top."))
                    {
                        topLevel = 1;
                    }
                    else
                    {
                        string topLevelStr = attributeName.Substring(3, attributeName.IndexOf(".") - 3);
                        topLevel = TranslateUtils.ToInt(topLevelStr);
                    }
                    upLevel = 0;
                    attributeName = attributeName.Substring(attributeName.IndexOf(".") + 1);
                }

                ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, DataUtility.GetForumIDByLevel(pageInfo.PublishmentSystemID, forumID, upLevel, topLevel));

                if (StringUtils.EqualsIgnoreCase(ForumEntities.ForumID, attributeName))//板块ID
                {
                    parsedContent = forumInfo.ForumID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.Title, attributeName) || StringUtils.EqualsIgnoreCase(ForumEntities.ForumName, attributeName))//板块名称
                {
                    parsedContent = forumInfo.ForumName;
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.ForumIndex, attributeName))//板块索引
                {
                    parsedContent = forumInfo.IndexName;
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.Content, attributeName))//板块正文
                {
                    parsedContent = forumInfo.Summary;
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.NavigationUrl, attributeName))//板块链接地址
                {
                    parsedContent = PageUtilityBBS.GetForumUrl(pageInfo.PublishmentSystemID, forumInfo);
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.IconUrl, attributeName))//板块图片地址
                {
                    parsedContent = forumInfo.IconUrl;

                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = PageUtils.ParseNavigationUrl(parsedContent);
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.AddDate, attributeName))//板块添加日期
                {
                    parsedContent = DateUtils.Format(forumInfo.AddDate, string.Empty);
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.DirectoryName, attributeName))//生成文件夹名称
                {
                    parsedContent = PathUtils.GetDirectoryName(forumInfo.FilePath);
                }
                else if (StringUtils.StartsWithIgnoreCase(attributeName, ParserUtility.ItemIndex) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.ForumItem != null)
                {
                    parsedContent = ParserUtility.ParseItemIndex(contextInfo.ItemContainer.ForumItem.ItemIndex, attributeName, contextInfo).ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.ThreadCount, attributeName))
                {
                    parsedContent = forumInfo.ThreadCount.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.MetaKeywords, attributeName))//板块组别
                {
                    parsedContent = forumInfo.MetaKeywords;
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.MetaDescription, attributeName))//板块组别
                {
                    parsedContent = forumInfo.MetaDescription;
                }
                else if (StringUtils.EqualsIgnoreCase("addPostUrl", attributeName))
                {
                    parsedContent = PostPage.GetAddUrl(pageInfo.PublishmentSystemID, forumID, false);
                }
                else if (StringUtils.EqualsIgnoreCase("addPollUrl", attributeName))
                {
                    parsedContent = PostPage.GetAddUrl(pageInfo.PublishmentSystemID, forumID, true);
                }
            }
            catch { }

            return parsedContent;
        }
	}
}
