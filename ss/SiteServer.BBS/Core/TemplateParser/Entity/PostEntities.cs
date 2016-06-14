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
    public class PostEntities
	{
        private PostEntities()
		{
		}

        public static string ItemIndex = "ItemIndex";//°å¿éÅÅÐò

        internal static string Parse(string entity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            PostInfo postInfo = new PostInfo(contextInfo.ItemContainer.PostItem.DataItem);

            try
            {
                string entityName = ParserUtility.GetNameFromEntity(entity);
                string attributeName = entityName.Substring(10, entityName.Length - 11);

                if (StringUtils.EqualsIgnoreCase(PostAttribute.ForumID, attributeName))
                {
                    parsedContent = postInfo.ForumID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(PostAttribute.ThreadID, attributeName))
                {
                    parsedContent = postInfo.ThreadID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(PostAttribute.PostID, attributeName))
                {
                    parsedContent = postInfo.ID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(PostAttribute.UserName, attributeName))
                {
                    parsedContent = postInfo.UserName;
                }
                else if (StringUtils.EqualsIgnoreCase(PostAttribute.IsThread, attributeName))
                {
                    parsedContent = postInfo.IsThread.ToString().ToLower();
                }
                else if (StringUtils.EqualsIgnoreCase(PostAttribute.IsSignature, attributeName))
                {
                    parsedContent = postInfo.IsSignature.ToString().ToLower();
                }
                else if (StringUtils.EqualsIgnoreCase("replyUrl", attributeName))
                {
                    parsedContent = SiteServer.BBS.Pages.Dialog.Post.GetOpenWindowStringByReplyPost(pageInfo.PublishmentSystemID, postInfo.ForumID, postInfo.ThreadID, postInfo.ID);
                }
                else if (StringUtils.EqualsIgnoreCase("referenceUrl", attributeName))
                {
                    parsedContent = SiteServer.BBS.Pages.Dialog.Post.GetOpenWindowStringByReferencePost(pageInfo.PublishmentSystemID, postInfo.ForumID, postInfo.ThreadID, postInfo.ID);
                }
                else if (StringUtils.EqualsIgnoreCase("reportUrl", attributeName))
                {
                    parsedContent = SiteServer.BBS.Pages.Dialog.Report.GetOpenWindowStringByReportPost(pageInfo.PublishmentSystemID, postInfo.ForumID, postInfo.ThreadID, postInfo.ID);
                }
                else if (StringUtils.EqualsIgnoreCase("editUrl", attributeName))
                {
                    parsedContent = PostPage.GetUrl(pageInfo.PublishmentSystemID, postInfo.ForumID, postInfo.ThreadID, postInfo.ID, string.Empty);
                }
                else if (StringUtils.StartsWithIgnoreCase(attributeName, ParserUtility.ItemIndex) && contextInfo.ItemContainer != null && contextInfo.ItemContainer.PostItem != null)
                {
                    parsedContent = ParserUtility.ParseItemIndex(contextInfo.ItemContainer.PostItem.ItemIndex, attributeName, contextInfo).ToString();
                }
            }
            catch { }

            return parsedContent;
        }
	}
}
