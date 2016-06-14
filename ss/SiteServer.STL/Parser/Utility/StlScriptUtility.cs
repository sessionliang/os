using System.Web.UI;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Services;
using SiteServer.CMS.Model;
using SiteServer.STL.BackgroundPages.Modal.StlTemplate;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser
{
	public class StlScriptUtility
	{
        public class PageScript
        {
            public static string GetStlLogin(string redirectUrl)
            {
                return string.Format("stlLogin('{0}');", StringUtils.ValueToUrl(redirectUrl));
            }

            public static string GetStlOpenWindow(string pageUrl, int width, int height)
            {
                return string.Format("stlOpenWindow('{0}', {1}, {2});", pageUrl, width, height);
            }

            public static string GetStlRefresh()
            {
                return "stlRefresh();";
            }

            public static string GetStlInputReplaceTextarea(string attributeName, string editorUrl, int height, int width)
            {
                return string.Format(@"stlInputReplaceTextarea('{0}', '{1}', {2}, {3});", attributeName, editorUrl, height, width);
            }

            public static string GetStlInputSubmit(string resultsPageUrl, string ajaxDivID, bool isSuccessHide, bool isSuccessReload, string checkMethod, string successTemplate, string failureTemplate)
            {
                if (string.IsNullOrEmpty(checkMethod))
                {
                    return string.Format("stlInputSubmit('{0}', '{1}', {2}, {3}, '{4}', '{5}');return false;", resultsPageUrl, ajaxDivID, isSuccessHide.ToString().ToLower(), isSuccessReload.ToString().ToLower(), successTemplate, failureTemplate);
                }
                else
                {
                    checkMethod = checkMethod.Trim().TrimEnd(';');
                    if (!checkMethod.EndsWith(")"))
                    {
                        checkMethod = checkMethod + "()";
                    }
                    return string.Format("if ({0})stlInputSubmit('{1}', '{2}', {3}, {4}, '{5}', '{6}');return false;", checkMethod, resultsPageUrl, ajaxDivID, isSuccessHide.ToString().ToLower(), isSuccessReload.ToString().ToLower(), successTemplate, failureTemplate);
                }
            }

            public static string GetStlInputSubmitWithUpdate(string resultsPageUrl, string ajaxDivID, bool isSuccessHide, bool isSuccessReload, string checkMethod, string successTemplate, string failureTemplate, string successCallback, string successArgument)
            {
                if (string.IsNullOrEmpty(checkMethod))
                {
                    return string.Format("stlInputSubmit('{0}', '{1}', {2}, {3}, '{4}', '{5}', '{6}', '{7}');return false;", resultsPageUrl, ajaxDivID, isSuccessHide.ToString().ToLower(), isSuccessReload.ToString().ToLower(), successTemplate, failureTemplate, successCallback, successArgument);
                }
                else
                {
                    checkMethod = checkMethod.Trim().TrimEnd(';');
                    if (!checkMethod.EndsWith(")"))
                    {
                        checkMethod = checkMethod + "()";
                    }
                    return string.Format("if ({0})stlInputSubmit('{1}', '{2}', {3}, {4}, '{5}', '{6}', '{7}', '{8}');return false;", checkMethod, resultsPageUrl, ajaxDivID, isSuccessHide.ToString().ToLower(), isSuccessReload.ToString().ToLower(), successTemplate, failureTemplate, successCallback, successArgument);
                }
            }
        }

        //public static void RegisteScript(PageInfo pageInfo, string elementName)
        //{
        //    if (StringUtils.EqualsIgnoreCase(elementName, StlComments.ElementName))
        //    {
        //        pageInfo.AddPageScriptsIfNotExists(StlComments.ElementName, string.Format(@"<script type=""text/javascript"" charset=""utf-8"" src=""{0}""></script>", StlTemplateManager.Comments.ScriptUrl));
        //    }
        //}
	}
}
