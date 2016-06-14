using System.Collections;
using System.Collections.Specialized;
using System.Text;
using BaiRong.Core;
using SiteServer.BBS.Core.TemplateParser.Model;
using System;
using SiteServer.BBS.Pages.Dialog;
using SiteServer.BBS.Model;

namespace SiteServer.BBS.Core.TemplateParser.Entity
{
    public class BBSEntities
	{
        private BBSEntities()
		{
		}

        public static string PoweredBy = "{BBS.PoweredBy}";         //SiteServer CMS 支持信息
        public static string RootUrl = "{BBS.RootUrl}";             //系统根目录地址
        public static string BBSUrl = "{BBS.BBSUrl}";             //系统根目录地址
        public static string FormUrl = "{BBS.FormUrl}";
        public static string EditUploadUrl = "{BBS.EditUploadUrl}";
        public static string BBSName = "{BBS.BBSName}";           //频道名称
        public static string PublishmentSystemID = "{BBS.PublishmentSystemID}";       //站点ID
        public static string CurrentUrl = "{BBS.CurrentUrl}";       //当前页地址
        public static string AbsoluteUrl = "{BBS.AbsoluteUrl}";       //当前页绝对地址        


        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
           
            try
            {
                string entityName = ParserUtility.GetNameFromEntity(stlEntity);

                if (StringUtils.EqualsIgnoreCase(BBSEntities.PoweredBy, entityName))//SiteServer BBS 支持信息
                {
                    parsedContent = string.Format(@"Powered by <strong><a href=""http://bbs.siteserver.cn"">SiteServer BBS</a></strong> V{0} © 2003-{1} <a href=""http://www.siteserver.cn"">Bairongsoft Corporation</a>.", ProductManager.GetFullVersion(), DateTime.Now.Year);

                    ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(pageInfo.PublishmentSystemID);

                    if (!string.IsNullOrEmpty(additional.CountCode))
                    {
                        parsedContent += additional.CountCode;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(BBSEntities.RootUrl, entityName))//系统根目录地址
                {
                    parsedContent = PageUtils.ParseConfigRootUrl("~");
                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = parsedContent.TrimEnd('/');
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(BBSEntities.BBSUrl, entityName))//系统根目录地址
                {
                    parsedContent = PageUtilityBBS.GetBBSUrl(pageInfo.PublishmentSystemID, string.Empty);
                    if (!string.IsNullOrEmpty(parsedContent))
                    {
                        parsedContent = parsedContent.TrimEnd('/');
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(BBSEntities.FormUrl, entityName))
                {
                    parsedContent = PageUtilityBBS.GetBBSUrl(pageInfo.PublishmentSystemID, string.Format("/ajax/form.aspx?publishmentSystemID={0}&action=", pageInfo.PublishmentSystemID));
                }
                else if (StringUtils.EqualsIgnoreCase(BBSEntities.EditUploadUrl, entityName))
                {
                    parsedContent = PageUtilityBBS.GetBBSUrl(pageInfo.PublishmentSystemID, string.Format("/editor/upload_json.ashx?publishmentSystemID={0}", pageInfo.PublishmentSystemID));
                }
                else if (StringUtils.EqualsIgnoreCase(BBSEntities.BBSName, entityName))//频道名称
                {
                    ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(pageInfo.PublishmentSystemID);

                    parsedContent = additional.BBSName;
                }
                else if (StringUtils.EqualsIgnoreCase(BBSEntities.PublishmentSystemID, entityName))
                {
                    parsedContent = pageInfo.PublishmentSystemID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(BBSEntities.CurrentUrl, entityName) || StringUtils.EqualsIgnoreCase(BBSEntities.AbsoluteUrl, entityName))//当前页地址
                {
                    if (pageInfo.ForumID > 0 && pageInfo.ThreadID > 0)
                    {
                        parsedContent = PageUtilityBBS.GetThreadUrl(pageInfo.PublishmentSystemID, pageInfo.ForumID, pageInfo.ThreadID);
                    }
                    else if (pageInfo.ForumID > 0)
                    {
                        ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, pageInfo.ForumID);
                        parsedContent = PageUtilityBBS.GetForumUrl(pageInfo.PublishmentSystemID, forumInfo);
                    }
                    else
                    {
                        parsedContent = PageUtils.Combine(PageUtilityBBS.GetBBSUrl(pageInfo.PublishmentSystemID, pageInfo.DirectoryName), pageInfo.FileName);
                    }

                    if (StringUtils.EqualsIgnoreCase(BBSEntities.AbsoluteUrl, entityName))
                    {
                        parsedContent = PageUtils.AddProtocolToUrl(parsedContent);
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(BBSEntities.CurrentUrl, entityName))//当前页地址
                {
                    if (pageInfo.ForumID > 0 && pageInfo.ThreadID > 0)
                    {
                        parsedContent = PageUtilityBBS.GetThreadUrl(pageInfo.PublishmentSystemID, pageInfo.ForumID, pageInfo.ThreadID);
                    }
                    else if (pageInfo.ForumID > 0)
                    {
                        ForumInfo forumInfo = ForumManager.GetForumInfo(pageInfo.PublishmentSystemID, pageInfo.ForumID);
                        parsedContent = PageUtilityBBS.GetForumUrl(pageInfo.PublishmentSystemID, forumInfo);
                    }
                    else
                    {
                        parsedContent = PageUtils.Combine(PageUtilityBBS.GetBBSUrl(pageInfo.PublishmentSystemID, pageInfo.DirectoryName), pageInfo.FileName);
                    }
                }
                else
                {
                    if (StringUtils.StartsWithIgnoreCase(entityName, "{bbs.item."))
                    {
                        string name = entityName.Substring(10, entityName.Length - 11);
                        parsedContent = string.Format(@"<%#DataBinder.Eval(Container.DataItem,""{0}"")%>", name);
                    }
                    else if (StringUtils.StartsWithIgnoreCase(entityName, "{bbs.dialog."))
                    {
                        string name = entityName.Substring(12, entityName.Length - 13);
                        //threadManager.html包含文件
                        if (StringUtils.EqualsIgnoreCase(name, "deleteThread"))
                        {
                            parsedContent = Delete.GetOpenWindowStringDeleteThread(pageInfo.PublishmentSystemID, pageInfo.ForumID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "translate"))
                        {
                            parsedContent = Translate.GetOpenWindowString(pageInfo.PublishmentSystemID, pageInfo.ForumID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "category"))
                        {
                            parsedContent = Category.GetOpenWindowString(pageInfo.PublishmentSystemID, pageInfo.ForumID);
                        }
                        else if (StringUtils.StartsWithIgnoreCase(name, "highlight."))
                        {
                            string action = name.Substring(10);
                            parsedContent = Highlight.GetOpenWindowString(pageInfo.PublishmentSystemID, action, pageInfo.ForumID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "upDown"))
                        {
                            parsedContent = UpDown.GetOpenWindowStringUpDownThread(pageInfo.PublishmentSystemID, pageInfo.ForumID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "lock"))
                        {
                            parsedContent = Lock.GetOpenWindowString(pageInfo.PublishmentSystemID, pageInfo.ForumID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "banThreads"))
                        {
                            parsedContent = Ban.GetOpenWindowStringBanThreads(pageInfo.PublishmentSystemID, pageInfo.ForumID);
                        }
                        //thread.aspx模板页
                        else if (StringUtils.EqualsIgnoreCase(name, "replyThread"))
                        {
                            parsedContent = Post.GetOpenWindowStringByReplyThread(pageInfo.PublishmentSystemID, pageInfo.ForumID, pageInfo.ThreadID);
                        }
                        //postManager.html包含文件
                        else if (StringUtils.EqualsIgnoreCase(name, "deletePost"))
                        {
                            parsedContent = Delete.GetOpenWindowStringDeletePost(pageInfo.PublishmentSystemID, pageInfo.ForumID, pageInfo.ThreadID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "banPost"))
                        {
                            parsedContent = Ban.GetOpenWindowStringBanPost(pageInfo.PublishmentSystemID, pageInfo.ForumID, pageInfo.ThreadID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "upDownPost"))
                        {
                            parsedContent = UpDown.GetOpenWindowStringUpDownPost(pageInfo.PublishmentSystemID, pageInfo.ForumID, pageInfo.ThreadID);
                        }
                        //thread.aspx模板页（管理）
                        else if (StringUtils.EqualsIgnoreCase(name, "deleteThreadSingle"))
                        {
                            parsedContent = Delete.GetOpenWindowStringDeleteThreadSingle(pageInfo.PublishmentSystemID, pageInfo.ForumID, pageInfo.ThreadID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "translateSingle"))
                        {
                            parsedContent = Translate.GetOpenWindowString(pageInfo.ForumID, pageInfo.ThreadID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "categorySingle"))
                        {
                            parsedContent = Category.GetOpenWindowString(pageInfo.ForumID, pageInfo.ThreadID);
                        }
                        else if (StringUtils.StartsWithIgnoreCase(name, "highlightSingle."))
                        {
                            string action = name.Substring(16);
                            parsedContent = Highlight.GetOpenWindowString(pageInfo.PublishmentSystemID, action, pageInfo.ForumID, pageInfo.ThreadID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "lockSingle"))
                        {
                            parsedContent = Lock.GetOpenWindowString(pageInfo.ForumID, pageInfo.ThreadID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "banThread"))
                        {
                            parsedContent = Ban.GetOpenWindowStringBanThread(pageInfo.PublishmentSystemID, pageInfo.ForumID, pageInfo.ThreadID);
                        }
                        else if (StringUtils.EqualsIgnoreCase(name, "identifySingle"))
                        {
                            parsedContent = Identify.GetOpenWindowString(pageInfo.ForumID, pageInfo.ThreadID);
                        }
                    }
                }
            }
            catch { }

            return parsedContent;
        }
	}
}
