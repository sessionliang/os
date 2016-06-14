using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using BaiRong.Model;
using System.Web.UI;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using SiteServer.BBS.Core.TemplateParser;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundUtils : Page
    {
        private const string Type_CreatePage = "CreatePage";
        private const string Type_Dynamic = "Dynamic";

        public static string GetRedirectUrl(int publishmentSystemID, string type)
        {
            return PageUtils.GetBBSUrl(string.Format("background_utils.aspx?publishmentSystemID={0}&type={1}", publishmentSystemID, type));
        }

        public static string GetCreatePageUrl(int publishmentSystemID, string directoryName, string fileName, int forumID)
        {
            string ajaxUrl = string.Format("{0}&directoryName={0}&fileName={1}&forumID={2}", BackgroundUtils.GetRedirectUrl(publishmentSystemID, Type_CreatePage), directoryName, fileName, forumID);
            return PageUtils.GetAdminDirectoryUrlByPage(ajaxUrl);
        }

        public static string GetDynamicUrl(int publishmentSystemID)
        {
            string ajaxUrl = BackgroundUtils.GetRedirectUrl(publishmentSystemID, Type_Dynamic);
            return PageUtils.GetAdminDirectoryUrlByPage(ajaxUrl);
        }

        public static string GetDynamicParms(string directoryName, string fileName, int forumID, int threadID, ETemplateType templateType, bool isPageRefresh, string pageUrl, string ajaxDivID, string templateContent)
        {
            return string.Format("directoryName={0}&fileName={1}&forumID={2}&threadID={3}&templateType={4}&isPageRefresh={5}&pageUrl={6}&ajaxDivID={7}&templateContent={8}", directoryName, fileName, forumID, threadID, ETemplateTypeUtils.GetValue(templateType), isPageRefresh, RuntimeUtils.EncryptStringByTranslate(pageUrl), ajaxDivID, RuntimeUtils.EncryptStringByTranslate(templateContent));
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request.QueryString["type"];
            if (type == Type_CreatePage)
            {
                CreatePage();
            }
            else if (type == Type_Dynamic)
            {
                Dynamic();
            }
        }

        private void Dynamic()
        {
            int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]); 
            string directoryName = base.Request["directoryName"];
            string fileName = base.Request["fileName"];
            int forumID = TranslateUtils.ToInt(base.Request["forumID"]);
            int threadID = TranslateUtils.ToInt(base.Request["threadID"]);
            ETemplateType templateType = ETemplateTypeUtils.GetEnumType(base.Request["templateType"]);
            bool isPageRefresh = TranslateUtils.ToBool(base.Request["isPageRefresh"]);
            string templateContent = RuntimeUtils.DecryptStringByTranslate(Request["templateContent"]);
            string ajaxDivID = base.Request["ajaxDivID"];

            string pageUrl = RuntimeUtils.DecryptStringByTranslate(base.Request["pageUrl"]);
            int pageIndex = TranslateUtils.ToInt(base.Request["pageNum"]);
            if (pageIndex > 0)
            {
                pageIndex--;
            }

            NameValueCollection queryString = PageUtils.GetQueryString(base.Request.RawUrl);

            string content = ParserManager.ParseDynamicContent(publishmentSystemID, directoryName, fileName, forumID, threadID, templateType, isPageRefresh, templateContent, pageUrl, pageIndex, ajaxDivID, queryString);

            base.Response.Write(content);
            base.Response.End();
        }

        private void CreatePage()
        {
            int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
            string directoryName = base.Request.QueryString["directoryName"];
            string fileName = base.Request.QueryString["fileName"];
            int forumID = TranslateUtils.ToInt(base.Request.QueryString["forumID"]);
            string redirectUrl = string.Empty;

            FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
            if (forumID > 0)
            {
                FSO.CreateForum(forumID);
                redirectUrl = PageUtilityBBS.GetForumUrl(publishmentSystemID, ForumManager.GetForumInfo(publishmentSystemID, forumID));
            }
            else
            {
                FSO.CreateFile(directoryName, fileName);
                redirectUrl = PageUtilityBBS.GetBBSUrl(publishmentSystemID, PageUtils.Combine(directoryName, fileName));
            }

            redirectUrl = PageUtils.AddQueryString(redirectUrl, "__r", StringUtils.GetRandomInt(1, 10000).ToString());
            base.Response.Redirect(redirectUrl, true);
            return;
        }
    }
}
