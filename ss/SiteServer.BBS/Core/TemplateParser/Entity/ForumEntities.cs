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

        public static string ForumID = "ForumID";//���ID
        public static string ForumName = "ForumName";//�������
        public static string ForumIndex = "ForumIndex";//�������
        public static string Title = "Title";//�������
        public static string Content = "Content";//�������
        public static string NavigationUrl = "NavigationUrl";//������ӵ�ַ
        public static string IconUrl = "IconUrl";//���ͼƬ��ַ
        public static string AddDate = "AddDate";//����������
        public static string DirectoryName = "DirectoryName";//�����ļ�������
        public static string ItemIndex = "ItemIndex";//�������

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

                if (StringUtils.EqualsIgnoreCase(ForumEntities.ForumID, attributeName))//���ID
                {
                    parsedContent = forumInfo.ForumID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.Title, attributeName) || StringUtils.EqualsIgnoreCase(ForumEntities.ForumName, attributeName))//�������
                {
                    parsedContent = forumInfo.ForumName;
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.ForumIndex, attributeName))//�������
                {
                    parsedContent = forumInfo.IndexName;
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.Content, attributeName))//�������
                {
                    parsedContent = forumInfo.Summary;
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.NavigationUrl, attributeName))//������ӵ�ַ
                {
                    parsedContent = PageUtilityBBS.GetForumUrl(pageInfo.PublishmentSystemID, forumInfo);
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.IconUrl, attributeName))//���ͼƬ��ַ
                {
                    parsedContent = forumInfo.IconUrl;

                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = PageUtils.ParseNavigationUrl(parsedContent);
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.AddDate, attributeName))//����������
                {
                    parsedContent = DateUtils.Format(forumInfo.AddDate, string.Empty);
                }
                else if (StringUtils.EqualsIgnoreCase(ForumEntities.DirectoryName, attributeName))//�����ļ�������
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
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.MetaKeywords, attributeName))//������
                {
                    parsedContent = forumInfo.MetaKeywords;
                }
                else if (StringUtils.EqualsIgnoreCase(ForumAttribute.MetaDescription, attributeName))//������
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
