using System.Text;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Xml;

using BaiRong.Model;
using BaiRong.Core;
using BaiRong.Core.Cryptography;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser;
using SiteServer.CMS.Services;
using SiteServer.CMS.Core;

namespace SiteServer.STL.StlTemplate
{
    public class StlTemplateManager
    {
        private StlTemplateManager() { }

        public class Input
        {
            public static string ScriptUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("input/script.js");
                }
            }
        }

        public class WebsiteMessage
        {
            public static string ScriptUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("websiteMessage/script.js");
                }
            }
        }

        public class Vote
        {
            public static string ScriptUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("vote/script.js");
                }
            }

            public static string StyleUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("vote/css/vote.css");
                }
            }

            public static string GetInnerPageUrl(PublishmentSystemInfo publishmentSystemInfo, int voteID, string ajaxDivID, string width, string itemWidth, int columns, string openWindowHeight, string openWindowWidth, string titleStyle, string itemTextStyle, string itemImageStyle, string theme)
            {
                string innerPageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("vote/default.aspx?publishmentSystemID={0}&voteID={1}&ajaxDivID={2}&width={3}&itemWidth={4}&columns={5}&openWindowHeight={6}&openWindowWidth={7}&titleStyle={8}&itemTextStyle={9}&itemImageStyle={10}&theme={11}", publishmentSystemInfo.PublishmentSystemID, voteID, ajaxDivID, PageUtils.UrlEncode(width), PageUtils.UrlEncode(itemWidth), columns, openWindowHeight, openWindowWidth, titleStyle, itemTextStyle, itemImageStyle, theme));
                return innerPageUrl;
            }

            public static string GetContentVoteInnerPageUrl(PublishmentSystemInfo publishmentSystemInfo, int voteID, string ajaxDivID, string width, string itemWidth, int columns, string openWindowHeight, string openWindowWidth, string titleStyle, string itemTextStyle, string itemImageStyle, string theme)
            {
                string innerPageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("ContentVote/default.aspx?publishmentSystemID={0}&voteID={1}&ajaxDivID={2}&width={3}&itemWidth={4}&columns={5}&openWindowHeight={6}&openWindowWidth={7}&titleStyle={8}&itemTextStyle={9}&itemImageStyle={10}&theme={11}", publishmentSystemInfo.PublishmentSystemID, voteID, ajaxDivID, PageUtils.UrlEncode(width), PageUtils.UrlEncode(itemWidth), columns, openWindowHeight, openWindowWidth, titleStyle, itemTextStyle, itemImageStyle, theme));
                return innerPageUrl;
            }

            public static string GetViewPageUrl(PublishmentSystemInfo publishmentSystemInfo, int voteID, string titleStyle, string itemTextStyle, string itemImageStyle)
            {
                return PageUtils.GetRootUrl(string.Format("SiteFiles/Inner/vote/results.aspx?publishmentSystemID={0}&voteID={1}&titleStyle={2}&itemTextStyle={3}&itemImageStyle={4}", publishmentSystemInfo.PublishmentSystemID, voteID, titleStyle, itemTextStyle, itemImageStyle));
            }

            public static string GetContentVoteViewPageUrl(PublishmentSystemInfo publishmentSystemInfo, int voteID, string titleStyle, string itemTextStyle, string itemImageStyle)
            {
                return PageUtils.GetRootUrl(string.Format("SiteFiles/Inner/ContentVote/results.aspx?publishmentSystemID={0}&voteID={1}&titleStyle={2}&itemTextStyle={3}&itemImageStyle={4}", publishmentSystemInfo.PublishmentSystemID, voteID, titleStyle, itemTextStyle, itemImageStyle));
            }

            public static string GetClickString(bool isVote, string viewPageUrl, string ajaxDivID, int maxVoteItemNum, string cookieName, string openWindowHeight, string openWindowWidth)
            {
                return string.Format("stlVoteOpenWin({0}, '{1}', '{2}', {3}, '{4}', {5}, {6});return false;", isVote.ToString().ToLower(), viewPageUrl, ajaxDivID, maxVoteItemNum, cookieName, openWindowHeight, openWindowWidth);
            }
        }

        public class Digg
        {
            public static string ScriptUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("digg/script.js");
                }
            }

            public static string GetStyleUrl(string theme)
            {
                return PageUtility.Services.GetUrl(string.Format("digg/{0}.css", theme));
            }

            public static string GetInnerPageUrl(PublishmentSystemInfo publishmentSystemInfo, int relatedIdentity, int updaterID, EDiggType diggType, string goodText, string badText, string theme)
            {
                string innerPageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("digg/{0}.aspx?publishmentSystemID={1}&relatedIdentity={2}&updaterID={3}&type={4}&goodText={5}&badText={6}", theme, publishmentSystemInfo.PublishmentSystemID, relatedIdentity, updaterID, EDiggTypeUtils.GetValue(diggType), RuntimeUtils.EncryptStringByTranslate(goodText), RuntimeUtils.EncryptStringByTranslate(badText)));
                return innerPageUrl;
            }

            public static string GetInnerPageUrlWithAction(PublishmentSystemInfo publishmentSystemInfo, int relatedIdentity, int updaterID, EDiggType diggType, string goodText, string badText, string theme, bool isGood)
            {
                string innerPageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("digg/{0}.aspx?publishmentSystemID={1}&relatedIdentity={2}&updaterID={3}&type={4}&goodText={5}&badText={6}&isDigg=True&isGood={7}", theme, publishmentSystemInfo.PublishmentSystemID, relatedIdentity, updaterID, EDiggTypeUtils.GetValue(diggType), RuntimeUtils.EncryptStringByTranslate(goodText), RuntimeUtils.EncryptStringByTranslate(badText), isGood));
                return innerPageUrl;
            }

            public static string GetClickString(bool isGood, int updaterID)
            {
                return string.Format("stlDiggSet_{0}({1});return false;", updaterID, isGood.ToString().ToLower());
            }
        }

        public class Star
        {
            public static string ScriptUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("star/script.js");
                }
            }

            public static string GetStyleUrl(string theme)
            {
                return PageUtility.Services.GetUrl(string.Format("star/{0}.css", theme));
            }

            public static string GetInnerPageUrl(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID, int updaterID, int totalStar, int initStar, string theme)
            {
                string innerPageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("star/{0}.aspx?publishmentSystemID={1}&channelID={2}&contentID={3}&updaterID={4}&totalStar={5}&initStar={6}&theme={0}", theme, publishmentSystemInfo.PublishmentSystemID, channelID, contentID, updaterID, totalStar, initStar));
                return innerPageUrl;
            }

            public static string GetInnerPageUrlWithAction(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID, int updaterID, int totalStar, int initStar, string theme)
            {
                string innerPageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("star/{0}.aspx?publishmentSystemID={1}&channelID={2}&contentID={3}&updaterID={4}&totalStar={5}&initStar={6}&theme={0}&isStar=True&point=", theme, publishmentSystemInfo.PublishmentSystemID, channelID, contentID, updaterID, totalStar, initStar));
                return innerPageUrl;
            }

            public static string GetClickString(int point, int updaterID)
            {
                return string.Format("stlStarPoint_{0}({1});return false;", updaterID, point);
            }
        }

        public class Resume
        {
            public static string ScriptUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("resume/js/resume.js");
                }
            }

            public static string StyleUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("resume/css/resume.css");
                }
            }

            public static string GetAjaxUploadUrl(int publishmentSystemID)
            {
                return PageUtility.Services.GetUrl(string.Format(@"ajaxUpload.aspx?publishmentSystemID={0}&isResume=True", publishmentSystemID));
            }
        }

        //public class Comments
        //{
        //    public static string ScriptUrl
        //    {
        //        get
        //        {
        //            return PageUtility.Services.GetUrl("comments/script.js");
        //        }
        //    }

        //    //public static string GetCommentDiggClickString(PublishmentSystemInfo publishmentSystemInfo, int commentID, bool isGood)
        //    //{
        //    //    return string.Format("commentDigg('{0}',{1},{2},'{3}');return false;", PageService.GetAddCommentDiggUrl(publishmentSystemInfo), publishmentSystemInfo.PublishmentSystemID, commentID, isGood.ToString());
        //    //}

        //    //public static string GetAjaxUrl(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID, int pageNum)
        //    //{
        //    //    return PageUtility.GetStlInnerUrl(publishmentSystemInfo, string.Format("comments/default.aspx?PublishmentSystemID={0}&channelID={1}&contentID={2}&pageNum={3}", publishmentSystemInfo.PublishmentSystemID, channelID, contentID, pageNum));
        //    //}

        //    //public static string GetDynamicAjaxUrl(PublishmentSystemInfo publishmentSystemInfo, int pageNum)
        //    //{
        //    //    return PageUtility.GetStlInnerUrl(publishmentSystemInfo, string.Format("comments/default.aspx?PublishmentSystemID={0}&pageNum={1}&isDynamic=true", publishmentSystemInfo.PublishmentSystemID, pageNum));
        //    //}

        //    public static string GetDynamicUpdateScriptName()
        //    {
        //        return "stlDynamic_Comments(0)";
        //    }

        //    public static string GetDynamicUpdateScriptName2()
        //    {
        //        return "stlDynamic_ajaxElement_1(0)";
        //    }

        //    //public static string GetCommentsPageUrl(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID)
        //    //{
        //    //    string url = PageUtility.ParseNavigationUrl(publishmentSystemInfo, "@/utils/comments.html");
        //    //    NameValueCollection attributes = new NameValueCollection();
        //    //    attributes.Add("channelID", channelID.ToString());
        //    //    attributes.Add("contentID", contentID.ToString());
        //    //    url = PageUtils.AddQueryString(url, attributes);

        //    //    return url;
        //    //}
        //}

        public class Search
        {
            public static string ScriptUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("search/script.js");
                }
            }

            public static string SuccessTemplatePath
            {
                get
                {
                    return PageUtility.Services.GetPath("search/successTemplate.html");
                }
            }

            public static string FailureTemplatePath
            {
                get
                {
                    return PageUtility.Services.GetPath("search/failureTemplate.html");
                }
            }

            //public static string GetPageUrlInputSimple(PublishmentSystemInfo publishmentSystemInfo, string ajaxDivID, string type, bool isLoadValues, string width, string inputWidth, bool openWin)
            //{
            //    string pageUrl = string.Empty;
            //    if (StringUtils.EqualsIgnoreCase(type, StlSearchInput.Type_Advanced))
            //    {
            //        pageUrl = PageUtility.GetStlInnerUrl(publishmentSystemInfo, string.Format("search/inputAdvanced.aspx?publishmentSystemID={0}&ajaxDivID={1}&isLoadValues={2}&width={3}&inputWidth={4}&openWin={5}", publishmentSystemInfo.PublishmentSystemID, ajaxDivID, isLoadValues, PageUtils.UrlEncode(width), PageUtils.UrlEncode(inputWidth), openWin));
            //    }
            //    else
            //    {
            //        pageUrl = PageUtility.GetStlInnerUrl(publishmentSystemInfo, string.Format("search/inputSimple.aspx?publishmentSystemID={0}&ajaxDivID={1}&isLoadValues={2}&width={3}&inputWidth={4}&openWin={5}", publishmentSystemInfo.PublishmentSystemID, ajaxDivID, isLoadValues, PageUtils.UrlEncode(width), PageUtils.UrlEncode(inputWidth), openWin));
            //    }

            //    return pageUrl;
            //}

            public static string GetPageUrlOutput(PublishmentSystemInfo publishmentSystemInfo)
            {

                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    string pageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("services/search/output?publishmentSystemID={0}", publishmentSystemInfo.PublishmentSystemID));
                    return pageUrl;
                }
                else
                {
                    string pageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("search/output.aspx?publishmentSystemID={0}", publishmentSystemInfo.PublishmentSystemID));
                    return pageUrl;
                }

            }

            public static string GetOutputParameters(string ajaxDivID, int pageNum, bool isHighlight, bool isRedirectSingle, bool isDefaultDisplay, string dateAttribute, ECharset charset, string successTemplateString, string failureTemplateString)
            {
                return string.Format("ajaxDivID={0}&pageNum={1}&isHighlight={2}&isRedirectSingle={3}&isDefaultDisplay={4}&dateAttribute={5}&charset={6}&successTemplateString={7}&failureTemplateString={8}", ajaxDivID, pageNum, isHighlight, isRedirectSingle, isDefaultDisplay, dateAttribute, ECharsetUtils.GetValue(charset), RuntimeUtils.EncryptStringByTranslate(successTemplateString), RuntimeUtils.EncryptStringByTranslate(failureTemplateString));
            }

            /// <summary>
            /// 获得站内搜索联想词地址
            /// </summary>
            /// <param name="publishmentSystemInfo"></param>
            /// <returns></returns>
            public static string GetSearchwordUrl(PublishmentSystemInfo publishmentSystemInfo)
            {
                string pageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("search/output.aspx?publishmentSystemID={0}&action=GetSearchwords", publishmentSystemInfo.PublishmentSystemID));
                return pageUrl;
            }
        }

        public class Register
        {
            public static string ScriptUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("register/script.js");
                }
            }

            public static string GetStyleUrl()
            {
                return PageUtility.Services.GetUrl("register/style.css");
            }

            public static string GetInnerPageUrl(PublishmentSystemInfo publishmentSystemInfo, string returnUrl)
            {
                string innerPageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("register/default.aspx?publishmentSystemID={0}&returnUrl={1}", publishmentSystemInfo.PublishmentSystemID, RuntimeUtils.EncryptStringByTranslate(returnUrl)));
                return innerPageUrl;
            }

            public static string GetInnerPageParameters(string ajaxDivID)
            {
                return string.Format("ajaxDivID={0}", ajaxDivID);
            }

            public static string GetClickString(PublishmentSystemInfo publishmentSystemInfo, string userType, string ajaxDivID, bool isSessionExists, bool isTypeChanged, int current)
            {
                string pageUrl = StlTemplateManager.Register.GetInnerPageUrl(publishmentSystemInfo, userType);
                return string.Format("stlUserRegister('{0}', '{1}', '{2}', '{3}', '{4}')", pageUrl, ajaxDivID, isSessionExists.ToString(), isTypeChanged.ToString(), current.ToString());
            }
        }

        public class Dynamic
        {
            public static string GetPageUrlOutput(PublishmentSystemInfo publishmentSystemInfo)
            {
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    string pageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("services/dynamic/output?publishmentSystemID={0}", publishmentSystemInfo.PublishmentSystemID));
                    return pageUrl;
                }
                else
                {
                    string pageUrl = PageUtility.Services.GetUrl(publishmentSystemInfo, string.Format("dynamic/output.aspx?publishmentSystemID={0}", publishmentSystemInfo.PublishmentSystemID));
                    return pageUrl;
                }
            }

            public static string GetOutputParameters(int pageNodeID, int pageContentID, int pageTemplateID, string pageUrl, string ajaxDivID, bool isPageRefresh, string templateContent)
            {
                return string.Format("pageNodeID={0}&pageContentID={1}&pageTemplateID={2}&isPageRefresh={3}&pageUrl={4}&ajaxDivID={5}&templateContent={6}", pageNodeID, pageContentID, pageTemplateID, isPageRefresh, RuntimeUtils.EncryptStringByTranslate(pageUrl), ajaxDivID, RuntimeUtils.EncryptStringByTranslate(templateContent));
            }
        }

        public class Tags
        {
            public static string GetStyleUrl(string theme)
            {
                return PageUtility.Services.GetUrl(string.Format("tags/{0}.css", theme));
            }
        }

        public class CommentInput
        {
            public static string StyleUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("commentInput/css/comment.css");
                }
            }
        }

        public class GovPublicApply
        {
            public static string ScriptUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("govpublicapply/js/apply.js");
                }
            }

            public static string StyleUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("govpublicapply/css/apply.css");
                }
            }

            public static string GetAjaxUploadUrl(int publishmentSystemID)
            {
                return PageUtility.Services.GetUrl(string.Format(@"ajaxUpload.aspx?publishmentSystemID={0}&isGovPublicApply=True", publishmentSystemID));
            }
        }

        public class GovPublicQuery
        {
            public static string ScriptUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("govpublicquery/js/query.js");
                }
            }

            public static string StyleUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("govpublicquery/css/query.css");
                }
            }
        }

        public class GovInteractApply
        {
            public static string StyleUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("govinteractapply/css/apply.css");
                }
            }
        }

        public class GovInteractQuery
        {
            public static string StyleUrl
            {
                get
                {
                    return PageUtility.Services.GetUrl("govinteractquery/css/query.css");
                }
            }
        }
    }
}
