using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Web.UI;

using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;

using SiteServer.STL.Parser.StlElement;

using System.Collections.Specialized;
using SiteServer.STL.Parser;
using System.Web.UI.WebControls;
using BaiRong.Core.Data.Provider;
using BaiRong.Text.LitJson;

namespace SiteServer.CMS.Services
{
    public class PageService : Page
    {
        private const string Type_AddTrackerCount = "AddTrackerCount";
        private const string Type_AddCountHits = "AddCountHits";
        private const string Type_IsVisible = "IsVisible";
        private const string Type_LoadingChannels = "LoadingChannels";
        private const string Type_IsVisitAllowed = "IsVisitAllowed";
        //private const string Type_AddCommentDigg = "AddCommentDigg";
        //private const string Type_RelatedField = "RelatedField";
        private const string Type_GetChannels = "GetChannels";
        private const string Type_GetVote = "GetVote";
        //private const string Type_GetTitles = "GetTitles";
        //private const string Type_GetTags = "GetTags";
        //private const string Type_GetWordSpliter = "GetWordSpliter";
        //private const string Type_GetDetection = "GetDetection";

        private const string Type_GetSearchwords = "Type_GetSearchwords";

        private const string Type_AddIntelligentPushCount = "AddIntelligentPushCount";//by 20151125 sofuny 培生智能推送--会员浏览内容统计内容上一级栏目


        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request["type"];
            NameValueCollection retval = new NameValueCollection();
            string retString = null;
            string[] retStrings = null;

            if (type == PageService.Type_IsVisible)
            {
                string test = base.Request["test"];
                string userGroup = RuntimeUtils.DecryptStringByTranslate(base.Request["userGroup"]);
                int userRank = TranslateUtils.ToInt(base.Request["userRank"]);
                int userCredits = TranslateUtils.ToInt(base.Request["userCredits"]);
                string redirectUrl = RuntimeUtils.DecryptStringByTranslate(base.Request["redirectUrl"]);
                string successTemplateString = RuntimeUtils.DecryptStringByTranslate(base.Request["successTemplateString"]);
                string failureTemplateString = RuntimeUtils.DecryptStringByTranslate(base.Request["failureTemplateString"]);

                string html = IsVisible(test, userGroup, userRank, userCredits, redirectUrl, successTemplateString, failureTemplateString);
                Response.Write(html);
                Page.Response.End();
                return;
            }
            else if (type == PageService.Type_AddTrackerCount)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int channelID = TranslateUtils.ToInt(base.Request["channelID"]);
                int contentID = TranslateUtils.ToInt(base.Request["contentID"]);
                bool isFirstAccess = TranslateUtils.ToBool(base.Request["isFirstAccess"]);
                string location = base.Request["location"];
                string referrer = base.Request["referrer"];
                string lastAccessDateTime = base.Request["lastAccessDateTime"];
                AddTrackerCount(publishmentSystemID, channelID, contentID, isFirstAccess, location, referrer, lastAccessDateTime);
                return;
            }
            else if (type == PageService.Type_AddCountHits)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int channelID = TranslateUtils.ToInt(base.Request["channelID"]);
                int contentID = TranslateUtils.ToInt(base.Request["contentID"]);
                AddCountHits(publishmentSystemID, channelID, contentID);
                return;
            }
            /**
             * by 20151125 sofuny
             * 培生智能推送--会员浏览内容统计内容的上一级栏目
             * */
            else if (type == PageService.Type_AddIntelligentPushCount)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int channelID = TranslateUtils.ToInt(base.Request["channelID"]);
                int contentID = TranslateUtils.ToInt(base.Request["contentID"]);
                AddIntelligentPushCount(publishmentSystemID, channelID, contentID);
                return;
            }
            else if (type == PageService.Type_LoadingChannels)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
                string target = base.Request["target"];
                bool isShowTreeLine = TranslateUtils.ToBool(base.Request["isShowTreeLine"]);
                bool isShowContentNum = TranslateUtils.ToBool(base.Request["isShowContentNum"]);
                string currentFormatString = base.Request["currentFormatString"];
                int topNodeID = TranslateUtils.ToInt(base.Request["topNodeID"]);
                int topParentsCount = TranslateUtils.ToInt(base.Request["topParentsCount"]);
                int currentNodeID = TranslateUtils.ToInt(base.Request["currentNodeID"]);
                string html = GetLoadingChannels(publishmentSystemID, parentID, target, isShowTreeLine, isShowContentNum, currentFormatString, topNodeID, topParentsCount, currentNodeID);

                Response.Write(html);
                Page.Response.End();
            }
            else if (type == PageService.Type_IsVisitAllowed)
            {
                int siteID = TranslateUtils.ToInt(base.Request["siteID"]);
                int channelID = TranslateUtils.ToInt(base.Request["channelID"]);
                bool isChannel = TranslateUtils.ToBool(base.Request["isChannel"]);

                bool isVisitAllowed = IsVisitAllowed(siteID, channelID, isChannel);
                retval.Add("isVisitAllowed", isVisitAllowed.ToString().ToLower());
            }
            //else if (type == Type_AddCommentDigg)
            //{
            //    int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
            //    int commentID = TranslateUtils.ToInt(base.Request["commentID"]);
            //    bool isGood = TranslateUtils.ToBool(base.Request["isGood"]);
            //    AddCommentDigg(publishmentSystemID, commentID, isGood);
            //    return;
            //}
            //else if (type == Type_RelatedField)
            //{
            //    string callback = base.Request["callback"];
            //    int relatedFieldID = TranslateUtils.ToInt(base.Request["relatedFieldID"]);
            //    int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
            //    string jsonString = GetRelatedField(relatedFieldID, parentID);
            //    string call = callback + "(" + jsonString + ")";

            //    Page.Response.Write(call);
            //    Page.Response.End();

            //    return;
            //}
            else if (type == Type_GetChannels)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                string jsonString = GetChannels(publishmentSystemID);

                Page.Response.Write(jsonString);
                Page.Response.End();

                return;
            }
            else if (type == Type_GetVote)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int nodeID = TranslateUtils.ToInt(base.Request["nodeID"]);
                int contentID = TranslateUtils.ToInt(base.Request["contentID"]);

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

                string jsonString = PageService.Vote.GetJsonString(publishmentSystemInfo, nodeID, contentID, true, string.Empty);

                Page.Response.Write(jsonString);
                Page.Response.End();

                return;
            }
            else if (type == Type_GetSearchwords)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                string searchword = PageUtils.FilterSqlAndXss(base.Request["searchword"]);
                string jsonString = GetSearchwords(publishmentSystemID, searchword);

                Page.Response.Write(jsonString);
                Page.Response.End();

                return;
            }
            //else if (type == Type_GetTitles)
            //{
            //    int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
            //    int channelID = TranslateUtils.ToInt(base.Request["channelID"]);
            //    int nodeID = TranslateUtils.ToInt(base.Request["nodeID"]);
            //    if (channelID > 0)
            //    {
            //        nodeID = channelID;
            //    }
            //    string title = base.Request["title"];
            //    string titles = GetTitles(publishmentSystemID, nodeID, title);

            //    Page.Response.Write(titles);
            //    Page.Response.End();

            //    return;
            //}
            //else if (type == Type_GetTags)
            //{
            //    int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
            //    string tag = base.Request["tag"];
            //    string tags = GetTags(publishmentSystemID, tag);

            //    Page.Response.Write(tags);
            //    Page.Response.End();

            //    return;
            //}
            //else if (type == Type_GetWordSpliter)
            //{
            //    int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
            //    string contents = base.Request.Form["content"];
            //    string tags = WordSpliter.GetKeywords(contents, AppManager.CMS.AppID, publishmentSystemID, 10);

            //    Page.Response.Write(tags);
            //    Page.Response.End();

            //    return;
            //}
            //else if (type == Type_GetDetection)
            //{
            //    int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
            //    string content = base.Request.Form["content"];
            //    ArrayList arraylist = DataProvider.KeywordDAO.GetKeywordArrayListByContent(content);
            //    string keywords = TranslateUtils.ObjectCollectionToString(arraylist);

            //    Page.Response.Write(keywords);
            //    Page.Response.End();
            //}
            else if (type == "Login")
            {
                string userFrom = base.Request["userFrom"];
                string userName = base.Request["userName"];
                string password = base.Request["password"];
                string validateCode = base.Request["validateCode"];
                bool isRemember = TranslateUtils.ToBool(base.Request["isRemember"]);
                bool isAdmin = TranslateUtils.ToBool(base.Request["isAdmin"]);
                bool isCheckCode = TranslateUtils.ToBool(base.Request["isCheckCode"]);
                if (isCheckCode)
                {
                    isCheckCode = FileConfigManager.Instance.IsValidateCode;
                }
                bool isCrossDomain = TranslateUtils.ToBool(base.Request["isCrossDomain"]);
                retStrings = Login(userFrom, userName, password, validateCode, isRemember, isAdmin, isCheckCode, isCrossDomain);
            }
            else if (type == "GetTotalNum")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int channelID = TranslateUtils.ToInt(base.Request["channelID"]);
                string style = base.Request["style"];
                string since = base.Request["since"];
                retString = GetTotalNum(publishmentSystemID, channelID, style, since);
            }
            else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_SubscribeQuery))
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                string jsonString = this.SubscribeQuery(publishmentSystemID);

                Page.Response.Write(jsonString);
                Page.Response.End();

                return;
            }

            if (retval.Count > 0)
            {
                string jsonString = TranslateUtils.NameValueCollectionToJsonString(retval);
                Page.Response.Write(jsonString);
                Page.Response.End();
            }
            else
            {
                if (retString != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
                    builder.AppendFormat("<result>{0}</result>", retString);
                    ResponseXML(builder);
                }
                else if (retStrings != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<result>");
                    foreach (string str in retStrings)
                    {
                        builder.Append(string.Format("<string>{0}</string>", StringUtils.ToXmlContent(str)));
                    }
                    builder.Append("</result>");
                    ResponseXML(builder);
                }
            }
        }

        public static string GetAddTrackerCountUrl(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID)
        {
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                string serviceUrl = string.Format("?type={0}&publishmentSystemID={1}&channelID={2}&contentID={3}", Constants.StlTemplateManagerActionType.Type_AddTrackerCount, publishmentSystemInfo.PublishmentSystemID, channelID, contentID);
                //by 20151130 sofuny to api
                return PageUtility.Services.GetUrl(publishmentSystemInfo, PageUtility.Services.API_SERVICES_PAGE_ACTION + serviceUrl);
            }
            else
            {
                string serviceUrl = string.Format("PageService.aspx?type={0}&publishmentSystemID={1}&channelID={2}&contentID={3}", Type_AddTrackerCount, publishmentSystemInfo.PublishmentSystemID, channelID, contentID);
                return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
            }
        }

        public static string GetAddCountHitsUrl(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID)
        {
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                string serviceUrl = string.Format("?type={0}&publishmentSystemID={1}&channelID={2}&contentID={3}", Constants.StlTemplateManagerActionType.Type_AddCountHits, publishmentSystemInfo.PublishmentSystemID, channelID, contentID);
                //by 20151130 sofuny to api
                return PageUtility.Services.GetUrl(publishmentSystemInfo, PageUtility.Services.API_SERVICES_PAGE_ACTION + serviceUrl);
            }
            else
            {
                string serviceUrl = string.Format("PageService.aspx?type={0}&publishmentSystemID={1}&channelID={2}&contentID={3}", Type_AddCountHits, publishmentSystemInfo.PublishmentSystemID, channelID, contentID);
                return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
            }
        }

        /// <summary>
        /// by 20151125 sofuny
        /// 培生智能推送--会员浏览内容统计内容的上一级栏目
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="channelID"></param>
        /// <param name="contentID"></param>
        /// <returns></returns>
        public static string GetAddIntelligentPushCountUrl(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID)
        {
            //string serviceUrl = string.Format("PageService.aspx?type={0}&publishmentSystemID={1}&channelID={2}&contentID={3}", Type_AddIntelligentPushCount, publishmentSystemInfo.PublishmentSystemID, channelID, contentID);
            //return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
            string serviceUrl = string.Format("?type={0}&publishmentSystemID={1}&channelID={2}&contentID={3}", Constants.StlTemplateManagerActionType.Type_AddIntelligentPushCount, publishmentSystemInfo.PublishmentSystemID, channelID, contentID);
            //by 20151127 sofuny to api
            return PageUtility.API.GetAPIUrl(publishmentSystemInfo, PageUtility.Services.API_SERVICES_PAGE_ACTION + serviceUrl);
        }

        public static string GetIsVisibleUrl(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                string serviceUrl = string.Format("?type={0}&publishmentSystemID={1}", Constants.StlTemplateManagerActionType.Type_IsVisible, publishmentSystemInfo.PublishmentSystemID);
                //by 20151130 sofuny to api
                return PageUtility.Services.GetUrl(publishmentSystemInfo, PageUtility.Services.API_SERVICES_PAGE_ACTION + serviceUrl);
            }
            else
            {
                string serviceUrl = string.Format("PageService.aspx?type={0}", Type_IsVisible);
                return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
            }
        }

        public static string GetLoadingChannelsUrl(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                string serviceUrl = string.Format("?type={0}", Constants.StlTemplateManagerActionType.Type_LoadingChannels);
                //by 20151130 sofuny to api
                return PageUtility.Services.GetUrl(publishmentSystemInfo, PageUtility.Services.API_SERVICES_PAGE_ACTION + serviceUrl);
            }
            else
            {
                string serviceUrl = string.Format("PageService.aspx?type={0}", Type_LoadingChannels);
                return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
            }
        }

        public static string GetSearchwordsUrl(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                string serviceUrl = string.Format("?type={0}&publishmentSystemID={1}", Constants.StlTemplateManagerActionType.Type_GetSearchwords, publishmentSystemInfo.PublishmentSystemID);
                //by 20151130 sofuny to api
                return PageUtility.Services.GetUrl(publishmentSystemInfo, PageUtility.Services.API_SERVICES_PAGE_ACTION + serviceUrl);
            }
            else
            {
                string serviceUrl = string.Format("PageService.aspx?type={0}&publishmentSystemID={1}", Type_GetSearchwords, publishmentSystemInfo.PublishmentSystemID);
                return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
            }
        }

        //public static string GetTitlesUrl(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        //{
        //    string serviceUrl = string.Format("PageService.aspx?type={0}&publishmentSystemID={1}&nodeID={2}", Type_GetTitles, publishmentSystemInfo.PublishmentSystemID, nodeID);
        //    return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
        //}

        //public static string GetTagsUrl(PublishmentSystemInfo publishmentSystemInfo)
        //{
        //    string serviceUrl = string.Format("PageService.aspx?type={0}&publishmentSystemID={1}", Type_GetTags, publishmentSystemInfo.PublishmentSystemID);
        //    return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
        //}

        //public static string GetWordSpliterUrl(PublishmentSystemInfo publishmentSystemInfo)
        //{
        //    string serviceUrl = string.Format("PageService.aspx?type={0}&publishmentSystemID={1}", Type_GetWordSpliter, publishmentSystemInfo.PublishmentSystemID);
        //    return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
        //}

        //public static string GetDetectionUrl(PublishmentSystemInfo publishmentSystemInfo)
        //{
        //    string serviceUrl = string.Format("PageService.aspx?type={0}&publishmentSystemID={1}", Type_GetDetection, publishmentSystemInfo.PublishmentSystemID);
        //    return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
        //}

        public static string GetIsVisitAllowedUrl(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
            {
                string serviceUrl = string.Format("?type={0}", Constants.StlTemplateManagerActionType.Type_IsVisitAllowed);
                //by 20151130 sofuny to api
                return PageUtility.Services.GetUrl(publishmentSystemInfo, PageUtility.Services.API_SERVICES_PAGE_ACTION + serviceUrl);
            }
            else
            {
                string serviceUrl = string.Format("PageService.aspx?type={0}", Type_IsVisitAllowed);
                return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
            }
        }

        //public static string GetAddCommentDiggUrl(PublishmentSystemInfo publishmentSystemInfo)
        //{
        //    string serviceUrl = string.Format("PageService.aspx?type={0}", Type_AddCommentDigg);
        //    return PageUtility.Services.GetUrl(publishmentSystemInfo, serviceUrl, ECharset.utf_8, false);
        //}

        //public static string GetRelatedFieldUrlPrefix(PublishmentSystemInfo publishmentSystemInfo, int relatedFieldID)
        //{
        //    string serviceUrl = string.Format("PageService.aspx?type={0}&relatedFieldID={1}&parentID=", Type_RelatedField, relatedFieldID);
        //    return PageUtility.Services.GetUrl(serviceUrl);
        //}

        public static string RegisterIsVisible(string test, PublishmentSystemInfo publishmentSystemInfo, string userGroup, int userRank, int userCredits, string ajaxDivID, string redirectUrl, string successTemplateString, string failureTemplateString)
        {
            string redirectScript = string.Empty;
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                redirectScript = string.Format("location.href = '{0}';", PageUtils.ParseNavigationUrl(redirectUrl));
            }

            return string.Format(@"
<script type=""text/javascript"" language=""javascript"">
jQuery(document).ready(function(){{
try{{
    var url = '{0}';
    var pars = 'test={1}&userGroup={2}&userRank={3}&userCredits={4}&redirectUrl={5}&successTemplateString={6}&failureTemplateString={7}';
   // jQuery.post(url, pars, function(data, textStatus){{
       // jQuery(""#{8}"").replaceWith(data);
   // }});
    $.ajax({{
                url: url,
                type: ""POST"",
                //mimeType:""multipart/form-data"",
                //contentType: false,
                //processData: false,
                cache: false,
                xhrFields: {{   
                    withCredentials: true   
                }},
                data: pars,
                success: function(json, textStatus, jqXHR){{
                    jQuery(""#{8}"").replaceWith(json);
                }}
    }});
}}catch(e){{}}
}});
</script>", PageService.GetIsVisibleUrl(publishmentSystemInfo), test, RuntimeUtils.EncryptStringByTranslate(userGroup), userRank, userCredits, RuntimeUtils.EncryptStringByTranslate(redirectUrl), RuntimeUtils.EncryptStringByTranslate(successTemplateString), RuntimeUtils.EncryptStringByTranslate(failureTemplateString), ajaxDivID);
        }

        public static string RegisterIsVisitAllowed(string content, PublishmentSystemInfo publishmentSystemInfo, int channelID, bool isChannel)
        {
            string escapedHtml = TranslateUtils.EscapeHtml(content);
            return string.Format(@"
<script type=""text/javascript"" language=""javascript"">
var url = '{0}';
var pars = 'siteID={1}&channelID={2}&isChannel={3}';
var content = ""{4}"";
    $.ajax({{
                url: url,
                type: ""POST"",
                //mimeType:""multipart/form-data"",
                //contentType: false,
                //processData: false,
                cache: false,
                xhrFields: {{   
                    withCredentials: true   
                }},
                data: pars,
                success: function(data, textStatus){{
    data = eval('(' + data + ')');
    if (data.isVisitAllowed == 'true')
    {{
		document.write(unescape(content));
    }}
                }}
}});
</script>", PageService.GetIsVisitAllowedUrl(publishmentSystemInfo), publishmentSystemInfo.PublishmentSystemID, channelID, isChannel, escapedHtml);
        }

        private bool IsVisible(string test, string userGroup, int userRank, int userCredits)
        {
            if (BaiRongDataProvider.UserDAO.IsAnonymous && !BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
            {
                return false;
            }
            else
            {
                if (StringUtils.EqualsIgnoreCase(test, StlVisible.Test_IsAdmin))
                {
                    return BaiRongDataProvider.AdministratorDAO.IsAuthenticated;
                }
                else if (StringUtils.EqualsIgnoreCase(test, StlVisible.Test_IsUserOrAdmin))
                {
                    if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                    {
                        return true;
                    }
                }
            }

            if (BaiRongDataProvider.UserDAO.IsAnonymous)
            {
                return false;
            }

            //判断会员权限

            if (string.IsNullOrEmpty(userGroup) && userRank == 0 && userCredits == 0)
            {
                return true;
            }
            else
            {
                //UserExtendInfo userExtendInfo = UserExtendManager.Current;
                //if (userExtendInfo != null)
                //{
                //    if (!string.IsNullOrEmpty(userGroup))
                //    {
                //        ArrayList groupNameArrayList = TranslateUtils.StringCollectionToArrayList(userGroup);
                //        UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(userExtendInfo.GroupID);
                //        if (UserGroupManager.IsGroupIDInGroupNames(groupInfo.GroupID, groupNameArrayList))
                //        {
                //            return true;
                //        }
                //    }
                //    else if (userRank > 0)
                //    {
                //        UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(userExtendInfo.GroupID);
                //        if (groupInfo.Rank >= userRank)
                //        {
                //            return true;
                //        }
                //    }
                //    else if (userCredits > 0)
                //    {
                //        if (productUserInfo.Credits >= userCredits)
                //        {
                //            return true;
                //        }
                //    }
                //}

                UserInfo userInfo = UserManager.Current;
                if (userInfo != null)
                {
                    if (userInfo.GroupID > 0)
                    {
                        ArrayList groupNameArrayList = TranslateUtils.StringCollectionToArrayList(userGroup);
                        if (UserGroupManager.IsGroupIDInGroupNames(userInfo.GroupSN, userInfo.GroupID, groupNameArrayList))
                        {
                            return true;
                        }
                    }
                    else if (userRank > 0)
                    {
                        UserGroupInfo groupInfo = UserGroupManager.GetCurrent(userInfo.GroupSN);
                        if (((int)groupInfo.GroupType) >= userRank)
                        {
                            return true;
                        }
                    }
                    else if (userCredits > 0)
                    {
                        if (userInfo.Credits >= userCredits)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public string IsVisible(string test, string userGroup, int userRank, int userCredits, string redirectUrl, string successTemplateString, string failureTemplateString)
        {
            bool isSuccess = IsVisible(test, userGroup, userRank, userCredits);

            if (isSuccess)
            {
                UserInfo userInfo = UserManager.Current;
                return StlEntityParser.ReplaceStlUserEntities(successTemplateString, userInfo);
            }
            else
            {
                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    return string.Format("<script>location.href = '{0}';</script>", redirectUrl);
                }
                else
                {
                    return failureTemplateString;
                }
            }
        }

        public void AddTrackerCount(int publishmentSystemID, int channelID, int contentID, bool isFirstAccess, string location, string referrer, string lastAccessDateTime)
        {
            PublishmentSystemInfo publishmentSystemInfo =
                PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            if (publishmentSystemInfo != null && publishmentSystemInfo.Additional.IsTracker)
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string operatingSystem = HttpContext.Current.Request.Browser.Platform;
                string browser = HttpContext.Current.Request.Browser.Browser + " " + HttpContext.Current.Request.Browser.MajorVersion;

                TrackingInfo trackingInfo = new TrackingInfo();

                trackingInfo.PublishmentSystemID = publishmentSystemID;
                trackingInfo.UserName = BaiRongDataProvider.UserDAO.CurrentUserName;
                if (isFirstAccess)
                {
                    trackingInfo.TrackerType = ETrackerType.Site;
                }
                else
                {
                    trackingInfo.TrackerType = ETrackerType.Page;
                }
                if (!string.IsNullOrEmpty(lastAccessDateTime))
                {
                    trackingInfo.LastAccessDateTime = Converter.ToDateTime(lastAccessDateTime);
                }
                else
                {
                    trackingInfo.LastAccessDateTime = DateTime.Now;
                }
                trackingInfo.PageUrl = location;
                trackingInfo.PageNodeID = channelID;
                trackingInfo.PageContentID = contentID;
                trackingInfo.Referrer = referrer;
                trackingInfo.IPAddress = ipAddress;
                trackingInfo.OperatingSystem = operatingSystem;
                trackingInfo.Browser = browser;
                trackingInfo.AccessDateTime = DateTime.Now;

                try
                {
                    DataProvider.TrackingDAO.Insert(trackingInfo);
                }
                catch { }
            }
        }

        public void AddCountHits(int publishmentSystemID, int channelID, int contentID)
        {
            try
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                string tableName = NodeManager.GetTableName(publishmentSystemInfo, channelID);
                BaiRongDataProvider.ContentDAO.AddHits(tableName, publishmentSystemInfo.Additional.IsCountHits, publishmentSystemInfo.Additional.IsCountHitsByDay, contentID);
            }
            catch { }

        }

        /// <summary>
        ///  by 20151125 sofuny
        /// 培生智能推送--会员浏览内容统计内容的上一级栏目
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="channelID"></param>
        /// <param name="contentID"></param>
        public void AddIntelligentPushCount(int publishmentSystemID, int channelID, int contentID)
        {
            PublishmentSystemInfo publishmentSystemInfo =
            PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo != null && publishmentSystemInfo.Additional.IsIntelligentPushCount)
            {

                UserInfo uinfo = UserManager.Current;
                //uinfo = BaiRongDataProvider.UserDAO.GetUserInfo(1);

                if (uinfo.UserID > 0 && channelID > 0 && contentID > 0)
                {
                    ViewsStatisticsInfo vinfo = new ViewsStatisticsInfo();
                    vinfo.PublishmentSystemID = publishmentSystemID;
                    vinfo.UserID = uinfo.UserID;
                    vinfo.NodeID = channelID;
                    DateTime dtnow = DateTime.Now; ;
                    string[] date = dtnow.ToString("yyyy-MM-dd").Split('-');
                    vinfo.StasYear = date[0];
                    vinfo.StasMonth = date[1];
                    vinfo.StasCount = 0;

                    ViewsStatisticsInfo vsinfo = DataProvider.ViewsStatisticsDAO.IsExists(vinfo);
                    vsinfo.StasCount = vsinfo.StasCount + 1;

                    try
                    {
                        //如果这个月存在记录，则+1,没有则插入数据 
                        if (vsinfo.ID > 0)
                            DataProvider.ViewsStatisticsDAO.Update(vsinfo);
                        else
                            DataProvider.ViewsStatisticsDAO.Insert(vsinfo);
                    }
                    catch { }
                }
            }
        }

        public string GetLoadingChannels(int publishmentSystemID, int parentID, string target, bool isShowTreeLine, bool isShowContentNum, string currentFormatString, int topNodeID, int topParentsCount, int currentNodeID)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(publishmentSystemID, parentID);
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            foreach (int nodeID in nodeIDArrayList)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);

                builder.Append(StlTree.GetChannelRowHtml(publishmentSystemID, nodeInfo, target, isShowTreeLine, isShowContentNum, RuntimeUtils.DecryptStringByTranslate(currentFormatString), topNodeID, topParentsCount, currentNodeID));
            }

            return builder.ToString();
        }

        public bool IsVisitAllowed(int siteID, int channelID, bool isChannel)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(siteID);
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(siteID, channelID);
            ERestrictionType restrictionType = ERestrictionType.NoRestriction;
            if (isChannel)
            {
                restrictionType = nodeInfo.Additional.RestrictionTypeOfChannel;
            }
            else
            {
                restrictionType = nodeInfo.Additional.RestrictionTypeOfContent;
            }

            return RestrictionManager.IsVisitAllowed(restrictionType, publishmentSystemInfo.Additional.RestrictionBlackList, publishmentSystemInfo.Additional.RestrictionWhiteList);
        }

        public string[] Login(string userFrom, string userName, string password, string validateCode, bool isRemember, bool isAdmin, bool isCheckCode, bool isCrossDomain)
        {
            bool isLoggedIn = false;
            string errorMessage = string.Empty;
            if (isAdmin)
            {
                if (isCheckCode)
                {
                    ValidateCodeManager codeManager = ValidateCodeManager.GetInstanceOfPageAdminLogin(isCrossDomain);

                    if (!codeManager.IsCodeValid(validateCode))
                    {
                        return new string[] { "false", "验证码不正确" };
                    }
                }
                isLoggedIn = BaiRongDataProvider.AdministratorDAO.ValidateUser(userName, password, out errorMessage);
            }
            else
            {
                if (isCheckCode)
                {
                    VCManager codeManager = ValidateCodeManager.GetInstanceOfPageLogin(isCrossDomain);

                    if (!codeManager.IsCodeValid(validateCode))
                    {
                        return new string[] { "false", "验证码不正确" };
                    }
                }
                isLoggedIn = BaiRongDataProvider.UserDAO.Validate(userFrom, userName, password, out errorMessage);
            }

            if (!isLoggedIn)
            {
                return new string[] { "false", errorMessage };
            }
            else
            {
                if (isAdmin)
                {
                    BaiRongDataProvider.AdministratorDAO.Login(userName, isRemember);
                }
                else
                {
                    BaiRongDataProvider.UserDAO.Login(string.Empty, userName, isRemember);
                }
                return new string[] { "true", string.Empty };
            }
        }

        //public string GetRelatedField(int relatedFieldID, int parentID)
        //{
        //    StringBuilder jsonString = new StringBuilder();

        //    jsonString.Append("[");

        //    ArrayList arraylist = DataProvider.RelatedFieldItemDAO.GetRelatedFieldItemInfoArrayList(relatedFieldID, parentID);
        //    if (arraylist.Count > 0)
        //    {
        //        foreach (RelatedFieldItemInfo itemInfo in arraylist)
        //        {
        //            jsonString.AppendFormat(@"{{""id"":""{0}"",""name"":""{1}"",""value"":""{2}""}},", itemInfo.ID, StringUtils.ToJsString(itemInfo.ItemName), StringUtils.ToJsString(itemInfo.ItemValue));
        //        }
        //        jsonString.Length -= 1;
        //    }

        //    jsonString.Append("]");
        //    return jsonString.ToString();
        //}

        public string GetChannels(int publishmentSystemID)
        {
            StringBuilder jsonString = new StringBuilder();

            jsonString.Append("[");

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            ListItemCollection listItemCollection = new System.Web.UI.WebControls.ListItemCollection();
            NodeManager.AddListItemsForAddContent(listItemCollection, publishmentSystemInfo, true);
            if (listItemCollection.Count > 0)
            {
                foreach (ListItem listItem in listItemCollection)
                {
                    jsonString.AppendFormat(@"{{text:'{0}',value:'{1}'}},", StringUtils.ToJsString(listItem.Text), StringUtils.ToJsString(listItem.Value));
                }
                jsonString.Length -= 1;
            }

            jsonString.Append("]");
            return jsonString.ToString();
        }

        /// <summary>
        /// 获取站内搜索关键词联想词
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="searchword"></param>
        /// <returns></returns>
        private string GetSearchwords(int publishmentSystemID, string searchword)
        {
            ArrayList list = DataProvider.SearchwordDAO.GetSearchwordInfoArrayListForRelated(publishmentSystemID, searchword);
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            foreach (SearchwordInfo searchwordInfo in list)
            {
                jsonString.AppendFormat(@"{{searchWord:'{0}',searchResultCount:'{1}',searchCount:'{2}'}},", StringUtils.ToJsString(searchwordInfo.Searchword), StringUtils.ToJsString(searchwordInfo.SearchResultCount.ToString()), StringUtils.ToJsString(searchwordInfo.SearchCount.ToString()));
            }
            jsonString.Append("]");
            return jsonString.ToString();
        }


        private string SubscribeQuery(int publishmentSystemID)
        {
            JsonData data = new JsonData();
            try
            {
                ArrayList alist = DataProvider.SubscribeDAO.GetInfoList(publishmentSystemID, string.Format(" and ParentID != 0  and Enabled='{0}' ", EBoolean.True));

                data["isSuccess"] = true;

                JsonData Subscribes = new JsonData();
                foreach (SubscribeInfo info in alist)
                {
                    JsonData option = new JsonData();
                    option["itemID"] = info.ItemID;
                    option["itemName"] = info.ItemName;

                    Subscribes.Add(option);
                }

                data["subscribe"] = Subscribes;
                data["currentEmail"] = UserManager.Current.Email;
                data["currentMobile"] = UserManager.Current.Mobile;

                return data.ToJson();

            }
            catch (Exception ex)
            {
                data["isSuccess"] = false;
                return data.ToJson();
            }
        }

        #region by 20160104 sofuny 分支机构


        private string OrganizationQuery(int classifyID, int areaPID, int areaID)
        {
            JsonData data = new JsonData();
            try
            {
                ArrayList alist = DataProvider.OrganizationInfoDAO.GetInfoList(classifyID, areaPID, areaID);

                JsonData Info = new JsonData(); 
                foreach (OrganizationInfo info in alist)
                { 
                    JsonData option = new JsonData();
                    option["ID"] = info.ID;
                    option["Name"] = info.OrganizationName;
                    option["Address"] = info.OrganizationAddress;
                    option["Explain"] = info.Explain;
                    option["Phone"] = info.Phone;
                    option["Longitude"] = info.Longitude;
                    option["Latitude"] = info.Latitude;
                    option["LogoUrl"] = info.LogoUrl;
                    option["ContentNum"] = info.ContentNum;
                    Info.Add(option);
                }
                data["IsSuccess"] = true;
                data["Info"] = Info;
                return data.ToJson();
            } 
            catch (Exception ex)
            {
                data["IsSuccess"] = false;
                data["ErrorMessage"] = ex.Message;
                return data.ToJson();
            }
        }

        private string OrganizationShark(int classifyID)
        {
            JsonData data = new JsonData();
            try
            {
                if (RequestUtils.GetRequestString("Lat") != null && RequestUtils.GetRequestString("Lon") != null && RequestUtils.GetRequestString("Raidus") != null)
                {
                    string slat = RequestUtils.GetRequestString("Lat");
                    double lat = Convert.ToDouble(slat);
                    double lon = Convert.ToDouble(RequestUtils.GetRequestString("Lon"));
                    double raidus = Convert.ToInt64(RequestUtils.GetRequestString("Raidus"));

                    double minLat, maxLat, minLng, maxLng;

                    getAround(lat, lon, raidus, out  minLat, out   maxLat, out   minLng, out   maxLng);

                    ArrayList alist = DataProvider.OrganizationInfoDAO.GetInfoList(classifyID, minLat, maxLat, minLng, maxLng);
                    //获取每个点的距离                    

                    JsonData Info = new JsonData();  
                    foreach (OrganizationInfo info in alist)
                    {
                        double distance = double.Parse(getDistance(lat, lon, double.Parse(info.Latitude), double.Parse(info.Longitude)).ToString());
                        string distanceStr = "距离" + Math.Round((distance / 1000), 1) + "千米";
                         
                        if (Math.Round((distance / 1000), 1) > 10)
                        { 
                            JsonData option = new JsonData();
                            option["ID"] = info.ID;
                            option["Name"] = info.OrganizationName;
                            option["Address"] = info.OrganizationAddress;
                            option["Explain"] = info.Explain;
                            option["Phone"] = info.Phone;
                            option["Longitude"] = info.Longitude;
                            option["Latitude"] = info.Latitude;
                            option["LogoUrl"] = info.LogoUrl;
                            option["ContentNum"] = info.ContentNum;
                            option["Distance"] = distanceStr;
                            Info.Add(option);
                        }
                    }
                    data["IsSuccess"] = true;
                    data["Info"] = Info;
                    return data.ToJson(); 
                }
            }
            catch (Exception ex)
            {
                data["IsSuccess"] = false;
                data["ErrorMessage"] = ex.Message;
                return data.ToJson();
            }
            data["IsSuccess"] = false; 
            return data.ToJson();
        }

        private string OrganizationClassifyQuery()
        {
            JsonData data = new JsonData();
            try
            {
                ArrayList alist = DataProvider.OrganizationClassifyDAO.GetInfoList(string.Empty);

                JsonData ClassifyInfo = new JsonData();  
                foreach (OrganizationClassifyInfo info in alist)
                { 
                    JsonData option = new JsonData();
                    option["ItemID"] = info.ItemID;
                    option["ItemName"] = info.ItemName;
                    option["ContentNum"] = info.ContentNum;
                    option["ParentID"] = info.ParentID; 
                    ClassifyInfo.Add(option);
                } 
                data["IsSuccess"] = true;
                data["ClassifyInfo"] = ClassifyInfo;
                return data.ToJson();

            }
            catch (Exception ex)
            {
                data["IsSuccess"] = false;
                data["ErrorMessage"] = ex.Message;
                return data.ToJson();
            }
        }
        private string OrganizationAreaByClassifyQuery(int classifyID)
        {
            JsonData data = new JsonData();
            try
            {
                ArrayList alist = DataProvider.OrganizationAreaDAO.getParentArea(classifyID);

                JsonData AreaInfo = new JsonData();  
                foreach (OrganizationAreaInfo info in alist)
                { 
                    JsonData option = new JsonData();
                    option["ItemID"] = info.ItemID;
                    option["ItemName"] = info.ItemName;
                    AreaInfo.Add(option);
                }
                data["IsSuccess"] = true;
                data["AreaInfo"] = AreaInfo;
                return data.ToJson();
            }
            catch (Exception ex)
            {
                data["IsSuccess"] = false;
                data["ErrorMessage"] = ex.Message;
                return data.ToJson();
            }
        }
        private string OrganizationAreaByParentIDQuery(int parentID)
        {
            JsonData data = new JsonData();
            try
            {
                ArrayList alist = DataProvider.OrganizationAreaDAO.getChildArea(parentID);

                JsonData AreaInfo = new JsonData();  
                foreach (OrganizationAreaInfo info in alist)
                {
                    JsonData option = new JsonData();
                    option["ItemID"] = info.ItemID;
                    option["ItemName"] = info.ItemName;
                    AreaInfo.Add(option);
                }
                data["IsSuccess"] = true;
                data["AreaInfo"] = AreaInfo;
                return data.ToJson();
            }
            catch (Exception ex)
            {
                data["IsSuccess"] = false;
                data["ErrorMessage"] = ex.Message;
                return data.ToJson();
            }
        }

        /// <summary>
        /// 查找一定范围内的经纬度值
        /// 传入值：纬度  经度  查找半径(m)
        /// 返回值：最小纬度、经度，最大纬度、经度 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="raidus"></param>
        public void getAround(double lat, double lon, double raidus, out double minLat, out double maxLat, out double minLng, out double maxLng)
        {
            double latitude = lat;
            double longitude = lon;

            double degree = (double)((24901 * 1609) / 360.0);
            double raidusMile = raidus;

            double dpmLat = 1 / degree;
            double radiusLat = dpmLat * raidusMile;
            minLat = latitude - radiusLat;
            maxLat = latitude + radiusLat;

            double mpdLng = (double)(degree * Math.Cos(latitude * (Math.PI / 180)));
            double dpmLng = 1 / mpdLng;
            double radiusLng = dpmLng * raidusMile;
            minLng = longitude - radiusLng;
            maxLng = longitude + radiusLng;
        }


        /// <summary>
        /// 两个坐标之间的实际距离
        /// </summary>
        /// <param name="lat_a"></param>
        /// <param name="lng_a"></param>
        /// <param name="lat_b"></param>
        /// <param name="lng_b"></param>
        /// <returns></returns>
        public double getDistance(double lat_a, double lng_a, double lat_b, double lng_b)
        {
            double pk = (double)(180 / 3.14169);
            double a1 = lat_a / pk;
            double a2 = lng_a / pk;
            double b1 = lat_b / pk;
            double b2 = lng_b / pk;
            double t1 = Math.Cos(a1) * Math.Cos(a2) * Math.Cos(b1) * Math.Cos(b2);
            double t2 = Math.Cos(a1) * Math.Sin(a2) * Math.Cos(b1) * Math.Sin(b2);
            double t3 = Math.Sin(a1) * Math.Sin(b1);
            double tt = Math.Acos(t1 + t2 + t3);
            return 6366000 * tt;
        }

        #endregion

        //public string GetTitles(int publishmentSystemID, int nodeID, string title)
        //{
        //    StringBuilder retval = new StringBuilder();

        //    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
        //    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
        //    ArrayList titleArrayList = BaiRongDataProvider.ContentDAO.GetValueArrayListByStartString(tableName, nodeID, ContentAttribute.Title, title, 10);
        //    if (titleArrayList.Count > 0)
        //    {
        //        foreach (string value in titleArrayList)
        //        {
        //            retval.Append(value);
        //            retval.Append("|");
        //        }
        //        retval.Length -= 1;
        //    }

        //    return retval.ToString();
        //}

        //public string GetTags(int publishmentSystemID, string tag)
        //{
        //    StringBuilder retval = new StringBuilder();

        //    ArrayList tagArrayList = BaiRongDataProvider.TagDAO.GetTagArrayListByStartString(AppManager.CMS.AppID, publishmentSystemID, tag, 10);
        //    if (tagArrayList.Count > 0)
        //    {
        //        foreach (string value in tagArrayList)
        //        {
        //            retval.Append(value);
        //            retval.Append("|");
        //        }
        //        retval.Length -= 1;
        //    }

        //    return retval.ToString();
        //}

        public class Vote
        {
            public static string GetServiceUrl(int publishmentSystemID, int nodeID, int contentID)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (PageUtility.IsCorsCrossDomain(publishmentSystemInfo))
                {
                    string serviceUrl = string.Format("?type={0}&publishmentSystemID={1}&nodeID={2}&contentID={3}", Constants.StlTemplateManagerActionType.Type_GetVote, publishmentSystemID, nodeID, contentID);
                    //by 20151130 sofuny to api
                    return PageUtility.Services.GetUrl(publishmentSystemInfo, PageUtility.Services.API_SERVICES_PAGE_ACTION + serviceUrl);
                }
                else
                {
                    string serviceUrl = string.Format("PageService.aspx?type={0}&publishmentSystemID={1}&nodeID={2}&contentID={3}", Type_GetVote, publishmentSystemID, nodeID, contentID);
                    return PageUtility.Services.GetUrl(serviceUrl);
                }
            }

            public static string GetJsonString(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID, bool isSuccess, string message)
            {
                JsonData data = new JsonData();

                data["isSuccess"] = isSuccess.ToString().ToLower();
                data["message"] = message;
                data["isOver"] = "";

                VoteContentInfo contentInfo = DataProvider.VoteContentDAO.GetContentInfo(publishmentSystemInfo, contentID);

                data["title"] = contentInfo.Title;

                string limitDate = "30天";
                TimeSpan ts = contentInfo.EndDate - DateTime.Now;
                limitDate = string.Format("{0}天{1}小时{2}分钟", ts.Days, ts.Hours, ts.Minutes);

                if ((contentInfo.EndDate - DateTime.Now).TotalSeconds <= 0)
                {
                    limitDate = "投票已经结束";
                    data["isOver"] = "none";
                }
                else
                    limitDate = "距离投票结束还有" + limitDate;

                data["limitDate"] = limitDate;
                data["maxSelectNum"] = contentInfo.MaxSelectNum.ToString();

                data["limitAllSeconds"] = ts.TotalSeconds;

                JsonData table = new JsonData();

                ArrayList voteOptionInfoArrayList = DataProvider.VoteOptionDAO.GetVoteOptionInfoArrayList(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID);
                StringBuilder optionBuilder = new StringBuilder();
                int itemIndex = 1;
                int totalVoteNum = 0;
                foreach (VoteOptionInfo optionInfo in voteOptionInfoArrayList)
                {
                    JsonData option = new JsonData();
                    option["itemIndex"] = itemIndex++;
                    option["optionID"] = optionInfo.OptionID;
                    option["title"] = optionInfo.Title;
                    option["voteNum"] = optionInfo.VoteNum;

                    table.Add(option);

                    totalVoteNum += optionInfo.VoteNum;
                }

                data["table"] = table;

                if (totalVoteNum == 0) totalVoteNum = 1;
                data["totalVoteNum"] = totalVoteNum.ToString();

                data["totalOperation"] = DataProvider.VoteOperationDAO.GetCount(publishmentSystemInfo.PublishmentSystemID, nodeID, contentID);

                return data.ToJson();
            }
        }

        public string GetTotalNum(int publishmentSystemID, int channelID, string style, string since)
        {
            string html = string.Empty;

            PublishmentSystemInfo publishmentSystemInfo =
                PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            if (publishmentSystemInfo != null)
            {
                ETrackerStyle etrackerStyle = publishmentSystemInfo.Additional.TrackerStyle;
                if (!string.IsNullOrEmpty(style))
                {
                    etrackerStyle = ETrackerStyleUtils.GetEnumType(style);
                }

                int accessNum = 0;

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                if (nodeInfo != null)
                {
                    if (!string.IsNullOrEmpty(since))
                    {
                        int hours = DateUtils.GetSinceHours(since);
                        int days = 0;
                        if (hours > 0)
                        {
                            days = hours / 24;
                        }
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                        accessNum = BaiRongDataProvider.ContentDAO.GetCountChecked(tableName, nodeInfo.NodeID, days);
                    }
                    else
                    {
                        accessNum = nodeInfo.ContentNum;
                    }
                }

                switch (etrackerStyle)
                {
                    case ETrackerStyle.None:
                        break;
                    case ETrackerStyle.Number:
                        html = accessNum.ToString();
                        break;
                    default:
                        string numString = accessNum.ToString();
                        StringBuilder htmlBuilder = new StringBuilder();
                        string imgFolder = string.Format("{0}/{1}", PageUtility.GetIconUrl(publishmentSystemInfo, "tracker"), ETrackerStyleUtils.GetValue(etrackerStyle));
                        for (int index = 0; index < numString.Length; index++)
                        {
                            string imgHtml = string.Format("<img src='{0}/{1}.gif' align=absmiddle border=0>", imgFolder, numString[index]);
                            htmlBuilder.Append(imgHtml);
                        }
                        html = htmlBuilder.ToString();
                        break;
                }
            }
            return html;
        }

        #region Helper

        private void ResponseText(string text)
        {
            base.Response.Clear();
            base.Response.Write(text);
            base.Response.End();
        }

        /// <summary>
        /// 向页面输出xml内容
        /// </summary>
        /// <param name="xmlnode">xml内容</param>
        private void ResponseXML(StringBuilder xmlnode)
        {
            base.Response.Headers.Add("content-type", "text/xml;charset=utf-8");
            base.Response.Write(xmlnode.ToString());
            base.Response.End();
        }

        /// <summary>
        /// 输出json内容
        /// </summary>
        /// <param name="json"></param>
        private void ResponseJSON(string json)
        {
            base.Response.Clear();
            base.Response.ContentType = "application/json";
            base.Response.Expires = 0;

            base.Response.Cache.SetNoStore();
            base.Response.Write(json);
            base.Response.End();
        }
        #endregion

    }
}
