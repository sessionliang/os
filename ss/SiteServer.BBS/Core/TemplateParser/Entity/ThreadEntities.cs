using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.BBS.Core.TemplateParser.Model;
using SiteServer.BBS.Model;
using System;
using SiteServer.BBS.Pages;

namespace SiteServer.BBS.Core.TemplateParser.Entity
{
    public class ThreadEntities
	{
        private ThreadEntities()
		{
		}

        public static string ItemIndex = "ItemIndex";//°å¿éÅÅÐò

        internal static string Parse(string entity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            try
            {
                string entityName = ParserUtility.GetNameFromEntity(entity);
                string attributeName = entityName.Substring(12, entityName.Length - 13);

                if (StringUtils.EqualsIgnoreCase(ThreadAttribute.ThreadID, attributeName))//°å¿éID
                {
                    parsedContent = contextInfo.ThreadID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(ThreadAttribute.ForumID, attributeName))//°å¿éID
                {
                    parsedContent = contextInfo.ForumID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(ThreadAttribute.Content, attributeName))//°å¿éÕýÎÄ
                {
                    parsedContent = DataProvider.PostDAO.GetThreadValue(pageInfo.PublishmentSystemID, contextInfo.ThreadID, PostAttribute.Content);
                }
                else if (StringUtils.EqualsIgnoreCase(ThreadAttribute.Replies, attributeName))
                {
                    parsedContent = contextInfo.ThreadInfo.Replies.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(ThreadAttribute.NavigationUrl, attributeName))//°å¿éÁ´½ÓµØÖ·
                {
                    parsedContent = PageUtilityBBS.GetThreadUrl(pageInfo.PublishmentSystemID, contextInfo.ForumID, contextInfo.ThreadID);
                }
                else if (StringUtils.EqualsIgnoreCase(ThreadAttribute.AddDate, attributeName))//°å¿éÌí¼ÓÈÕÆÚ
                {
                    DateTime addDate = DataProvider.ThreadDAO.GetAddDate(contextInfo.ThreadID);
                    parsedContent = DateUtils.Format(addDate, string.Empty);
                }
                else if (StringUtils.EqualsIgnoreCase("replyUrl", attributeName))
                {
                    parsedContent = PostPage.GetUrl(pageInfo.PublishmentSystemID, contextInfo.ForumID, contextInfo.ThreadID, 0, string.Empty);
                }
                else if (StringUtils.StartsWithIgnoreCase(attributeName, ParserUtility.ItemIndex) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.ThreadItem != null)
                {
                    parsedContent = ParserUtility.ParseItemIndex(contextInfo.ItemContainer.ThreadItem.ItemIndex, attributeName, contextInfo).ToString();
                }
                else
                {
                    parsedContent = DataProvider.ThreadDAO.GetValue(contextInfo.ThreadID, attributeName);
                }
            }
            catch { }

            return parsedContent;
        }
	}
}
