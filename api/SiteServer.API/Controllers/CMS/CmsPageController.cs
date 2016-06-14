using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using BaiRong.Text.LitJson;
using SiteServer.API.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.IO;
using SiteServer.STL.Parser;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.StlTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Xml;

namespace SiteServer.API.Controllers.CMS
{
    [RoutePrefix("api/services/cmspage")]
    public class CmsPageController : ApiController
    {
        private int PublishmentSystemID;
        private PublishmentSystemInfo PublishmentSystemInfo;

        [HttpPost, HttpGet]
        [Route("action")]
        public HttpResponseMessage Action()
        {
            NameValueCollection retval = new NameValueCollection();

            PublishmentSystemID = TranslateUtils.ToInt(RequestUtils.GetRequestString("publishmentSystemID"));

            PublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(PublishmentSystemID);
            bool isCrossDomain = PageUtility.IsCrossDomain(PublishmentSystemInfo);
            string type = RequestUtils.GetRequestString("type");
            //如果类型为Type_IsVisitAllowed时，传递过来的站点ID的参数名称为siteID
            if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.Type_IsVisitAllowed))
                PublishmentSystemID = TranslateUtils.ToInt(RequestUtils.GetRequestString("siteID"));
            string retString = null;
            string[] retStrings = null;
            if (PublishmentSystemInfo != null && !string.IsNullOrEmpty(type))
            {
                if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.Type_IsVisible))
                {
                    string test = RequestUtils.GetRequestString("test");
                    string userGroup = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetRequestString("userGroup"));
                    int userRank = TranslateUtils.ToInt(RequestUtils.GetRequestString("userRank"));
                    int userCredits = TranslateUtils.ToInt(RequestUtils.GetRequestString("userCredits"));
                    string redirectUrl = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetRequestString("redirectUrl"));
                    string successTemplateString = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetRequestString("successTemplateString"));
                    string failureTemplateString = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetRequestString("failureTemplateString"));

                    string html = IsVisible(test, userGroup, userRank, userCredits, redirectUrl, successTemplateString, failureTemplateString);

                    retString = html;
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.Type_AddTrackerCount))
                {
                    int channelID = TranslateUtils.ToInt(RequestUtils.GetRequestString("channelID"));
                    int contentID = TranslateUtils.ToInt(RequestUtils.GetRequestString("contentID"));
                    bool isFirstAccess = TranslateUtils.ToBool(RequestUtils.GetRequestString("isFirstAccess"));
                    string location = RequestUtils.GetRequestString("location");
                    string referrer = RequestUtils.GetRequestString("referrer");
                    string lastAccessDateTime = RequestUtils.GetRequestString("lastAccessDateTime");
                    AddTrackerCount(PublishmentSystemID, channelID, contentID, isFirstAccess, location, referrer, lastAccessDateTime);

                    retString = null;
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.Type_AddCountHits))
                {
                    int channelID = TranslateUtils.ToInt(RequestUtils.GetRequestString("channelID"));
                    int contentID = TranslateUtils.ToInt(RequestUtils.GetRequestString("contentID"));
                    AddCountHits(PublishmentSystemID, channelID, contentID);
                    retString = null;
                }
                /**
                 * by 20151125 sofuny
                 * 培生智能推送--会员浏览内容统计内容的上一级栏目
                 * */
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.Type_AddIntelligentPushCount))
                {
                    int channelID = TranslateUtils.ToInt(RequestUtils.GetRequestString("channelID"));
                    int contentID = TranslateUtils.ToInt(RequestUtils.GetRequestString("contentID"));
                    AddIntelligentPushCount(PublishmentSystemID, channelID, contentID);
                    retString = null;
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.Type_LoadingChannels))
                {
                    int parentID = TranslateUtils.ToInt(RequestUtils.GetRequestString("parentID"));
                    string target = RequestUtils.GetRequestString("target");
                    bool isShowTreeLine = TranslateUtils.ToBool(RequestUtils.GetRequestString("isShowTreeLine"));
                    bool isShowContentNum = TranslateUtils.ToBool(RequestUtils.GetRequestString("isShowContentNum"));
                    string currentFormatString = RequestUtils.GetRequestString("currentFormatString");
                    int topNodeID = TranslateUtils.ToInt(RequestUtils.GetRequestString("topNodeID"));
                    int topParentsCount = TranslateUtils.ToInt(RequestUtils.GetRequestString("topParentsCount"));
                    int currentNodeID = TranslateUtils.ToInt(RequestUtils.GetRequestString("currentNodeID"));
                    string html = GetLoadingChannels(PublishmentSystemID, parentID, target, isShowTreeLine, isShowContentNum, currentFormatString, topNodeID, topParentsCount, currentNodeID);

                    retString = html;
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.Type_IsVisitAllowed))
                {
                    int channelID = TranslateUtils.ToInt(RequestUtils.GetRequestString("channelID"));
                    bool isChannel = TranslateUtils.ToBool(RequestUtils.GetRequestString("isChannel"));

                    bool isVisitAllowed = IsVisitAllowed(PublishmentSystemID, channelID, isChannel);
                    retval.Add("isVisitAllowed", isVisitAllowed.ToString().ToLower());
                }

                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.Type_GetChannels))
                {
                    string jsonString = GetChannels(PublishmentSystemID);

                    retString = jsonString;
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.Type_GetVote))
                {
                    int nodeID = TranslateUtils.ToInt(RequestUtils.GetRequestString("nodeID"));
                    int contentID = TranslateUtils.ToInt(RequestUtils.GetRequestString("contentID"));


                    string jsonString = Vote.GetJsonString(PublishmentSystemInfo, nodeID, contentID, true, string.Empty);

                    retString = jsonString;
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.Type_GetSearchwords))
                {
                    string searchword = PageUtils.FilterSqlAndXss(RequestUtils.GetRequestString("searchword"));
                    string jsonString = GetSearchwords(PublishmentSystemID, searchword);

                    retString = jsonString;
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_GetTotalNum))
                {
                    int publishmentSystemID = TranslateUtils.ToInt(RequestUtils.GetRequestString("publishmentSystemID"));
                    int channelID = TranslateUtils.ToInt(RequestUtils.GetRequestString("channelID"));
                    string style = RequestUtils.GetRequestString("style");
                    string since = RequestUtils.GetRequestString("since");
                    retString = GetTotalNum(publishmentSystemID, channelID, style, since);
                }
                #region add by sessionliang 20160315 siteserver.cms.services.utils
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Redirect))
                {
                    Redirect();
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_Download))
                {
                    Download();
                }
                else if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.ActionType_StlTrigger))
                {
                    StlTrigger();
                }
                #endregion
            }

            if (StringUtils.EqualsIgnoreCase(type, Constants.StlTemplateManagerActionType.Type_Login))
            {
                string userFrom = RequestUtils.GetRequestString("userFrom");
                string userName = RequestUtils.GetRequestString("userName");
                string password = RequestUtils.GetRequestString("password");
                string validateCode = RequestUtils.GetRequestString("validateCode");
                bool isRemember = TranslateUtils.ToBool(RequestUtils.GetRequestString("isRemember"));
                bool isAdmin = TranslateUtils.ToBool(RequestUtils.GetRequestString("isAdmin"));
                bool isCheckCode = TranslateUtils.ToBool(RequestUtils.GetRequestString("isCheckCode"));
                if (isCheckCode)
                {
                    isCheckCode = FileConfigManager.Instance.IsValidateCode;
                }
                bool isCrossDomainl = TranslateUtils.ToBool(RequestUtils.GetRequestString("isCrossDomain"));
                retStrings = Login(userFrom, userName, password, validateCode, isRemember, isAdmin, isCheckCode, isCrossDomainl);
            }
            //上传文件之后，需要重新设置一下Content-Type
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, retString, new MediaTypeHeaderValue("application/json"));
            return response;
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
                    DateTime dtnow = DateTime.Now;
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



        public class Vote
        {

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


        public void StlTrigger()
        {
            int publishmentSystemID = TranslateUtils.ToInt(HttpContext.Current.Request["publishmentSystemID"]);
            int channelID = 0;
            if (HttpContext.Current.Request["channelID"] != null)
            {
                channelID = TranslateUtils.ToInt(HttpContext.Current.Request["channelID"]);
            }
            else
            {
                channelID = publishmentSystemID;
            }
            int contentID = 0;
            if (HttpContext.Current.Request["contentID"] != null)
            {
                contentID = TranslateUtils.ToInt(HttpContext.Current.Request["contentID"]);
            }
            int fileTemplateID = 0;
            if (HttpContext.Current.Request["fileTemplateID"] != null)
            {
                fileTemplateID = TranslateUtils.ToInt(HttpContext.Current.Request["fileTemplateID"]);
            }
            bool isRedirect = false;
            if (HttpContext.Current.Request["isRedirect"] != null)
            {
                isRedirect = TranslateUtils.ToBool(HttpContext.Current.Request["isRedirect"]);
            }

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            if (fileTemplateID != 0)
            {
                FSO.CreateFile(fileTemplateID);
            }
            else if (contentID != 0)
            {
                FSO.CreateContent(tableStyle, tableName, channelID, contentID);
            }
            else if (channelID != 0)
            {
                FSO.CreateChannel(channelID);
            }
            else if (publishmentSystemID != 0)
            {
                FSO.CreateIndex();
            }

            if (isRedirect)
            {
                string redirectUrl = string.Empty;
                if (fileTemplateID != 0)
                {
                    redirectUrl = PageUtility.GetFileUrl(publishmentSystemInfo, fileTemplateID, publishmentSystemInfo.Additional.VisualType);
                }
                else if (contentID != 0)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                    redirectUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, publishmentSystemInfo.Additional.VisualType);
                }
                else if (channelID != 0)
                {
                    redirectUrl = PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, publishmentSystemInfo.Additional.VisualType);
                }
                else if (publishmentSystemID != 0)
                {
                    redirectUrl = PageUtility.GetIndexPageUrl(publishmentSystemInfo, publishmentSystemInfo.Additional.VisualType);
                }

                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    redirectUrl = PageUtils.AddQueryString(redirectUrl, "__r", StringUtils.GetRandomInt(1, 10000).ToString());
                    HttpContext.Current.Response.Redirect(redirectUrl, true);
                    return;
                }
            }

            HttpContext.Current.Response.Write(string.Empty);
            HttpContext.Current.Response.End();
        }

        public void Redirect()
        {
            string url;
            if (!string.IsNullOrEmpty(HttpContext.Current.Request["u"]))
            {
                url = HttpContext.Current.Request["u"];
                if (!url.ToLower().StartsWith("http://") && !url.ToLower().StartsWith("https://"))
                {
                    url = "http://" + url;
                }
                PageUtils.Redirect(url);
            }
            //else if (!string.IsNullOrEmpty(HttpContext.Current.Request["download"]))
            //{
            //    bool isSuccess = false;
            //    try
            //    {
            //        if (!string.IsNullOrEmpty(HttpContext.Current.Request["publishmentSystemID"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["fileUrl"]))
            //        {
            //            int publishmentSystemID = TranslateUtils.ToInt(HttpContext.Current.Request["publishmentSystemID"]);
            //            string fileUrl = RuntimeUtils.DecryptStringByTranslate(HttpContext.Current.Request["fileUrl"]);

            //            if (PageUtils.IsProtocolUrl(fileUrl))
            //            {
            //                isSuccess = true;
            //                PageUtils.Redirect(fileUrl);
            //            }
            //            else
            //            {
            //                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            //                string filePath = PathUtility.MapPath(publishmentSystemInfo, fileUrl);
            //                EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
            //                if (EFileSystemTypeUtils.IsDownload(fileType))
            //                {
            //                    if (FileUtils.IsFileExists(filePath))
            //                    {
            //                        isSuccess = true;
            //                        PageUtils.Download(HttpContext.Current.Response, filePath);
            //                    }
            //                }
            //                else
            //                {
            //                    isSuccess = true;
            //                    PageUtils.Redirect(PageUtility.ParseNavigationUrl(publishmentSystemInfo, fileUrl));
            //                }
            //            }
            //        }
            //        else if (!string.IsNullOrEmpty(HttpContext.Current.Request["filePath"]))
            //        {
            //            string filePath = RuntimeUtils.DecryptStringByTranslate(HttpContext.Current.Request["filePath"]);
            //            EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
            //            if (EFileSystemTypeUtils.IsDownload(fileType))
            //            {
            //                if (FileUtils.IsFileExists(filePath))
            //                {
            //                    isSuccess = true;
            //                    PageUtils.Download(HttpContext.Current.Response, filePath);
            //                }
            //            }
            //            else
            //            {
            //                isSuccess = true;
            //                string fileUrl = PageUtils.GetRootUrlByPhysicalPath(filePath);
            //                PageUtils.Redirect(PageUtils.ParseNavigationUrl(fileUrl));
            //            }
            //        }
            //        else if (!string.IsNullOrEmpty(HttpContext.Current.Request["publishmentSystemID"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["channelID"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["contentID"]))
            //        {
            //            int publishmentSystemID = TranslateUtils.ToInt(HttpContext.Current.Request["publishmentSystemID"]);
            //            int channelID = TranslateUtils.ToInt(HttpContext.Current.Request["channelID"]);
            //            int contentID = TranslateUtils.ToInt(HttpContext.Current.Request["contentID"]);
            //            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            //            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
            //            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            //            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            //            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

            //            if (contentInfo != null && !string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl)))
            //            {
            //                string fileUrl = contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl);
            //                if (publishmentSystemInfo.Additional.IsCountDownload)
            //                {
            //                    CountManager.AddCount(AppManager.CMS.AppID, tableName, contentID.ToString(), ECountType.Download);
            //                }

            //                if (PageUtils.IsProtocolUrl(fileUrl))
            //                {
            //                    isSuccess = true;
            //                    PageUtils.Redirect(fileUrl);
            //                }
            //                else
            //                {
            //                    string filePath = PathUtility.MapPath(publishmentSystemInfo, fileUrl);
            //                    EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
            //                    if (EFileSystemTypeUtils.IsDownload(fileType))
            //                    {
            //                        if (FileUtils.IsFileExists(filePath))
            //                        {
            //                            isSuccess = true;
            //                            PageUtils.Download(HttpContext.Current.Response, filePath);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        isSuccess = true;
            //                        PageUtils.Redirect(PageUtility.ParseNavigationUrl(publishmentSystemInfo, fileUrl));
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    catch { }
            //    if (!isSuccess)
            //    {
            //        HttpContext.Current.Response.Write("下载失败，不存在此文件！");
            //    }
            //}
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request["publishmentSystemID"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["channelID"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["contentID"]))
            {
                int publishmentSystemID = TranslateUtils.ToInt(HttpContext.Current.Request["publishmentSystemID"]);
                int channelID = TranslateUtils.ToInt(HttpContext.Current.Request["channelID"]);
                int contentID = TranslateUtils.ToInt(HttpContext.Current.Request["contentID"]);
                bool isInner = TranslateUtils.ToBool(HttpContext.Current.Request["isInner"]);

                if (contentID != 0)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                    url = PageUtility.GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, isInner, publishmentSystemInfo.Additional.VisualType);
                    if (!url.Equals(PageUtils.UNCLICKED_URL))
                    {
                        PageUtils.Redirect(url);
                    }
                    else
                    {
                        Redirect_DefaultDirection();
                    }
                }
                else
                {
                    Redirect_DefaultDirection();
                }
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request["channelID"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["contentID"]))
            {
                int channelID = TranslateUtils.ToInt(HttpContext.Current.Request["channelID"]);
                int contentID = TranslateUtils.ToInt(HttpContext.Current.Request["contentID"]);
                bool isInner = TranslateUtils.ToBool(HttpContext.Current.Request["isInner"]);

                if (contentID != 0)
                {
                    int publishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(channelID);
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                    url = PageUtility.GetContentUrl(publishmentSystemInfo, nodeInfo, contentID, isInner, publishmentSystemInfo.Additional.VisualType);
                    if (!url.Equals(PageUtils.UNCLICKED_URL))
                    {
                        PageUtils.Redirect(url);
                    }
                    else
                    {
                        Redirect_DefaultDirection();
                    }
                }
                else
                {
                    Redirect_DefaultDirection();
                }
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request["channelID"]))
            {
                int channelID = TranslateUtils.ToInt(HttpContext.Current.Request["channelID"]);
                bool isInner = TranslateUtils.ToBool(HttpContext.Current.Request["isInner"]);

                if (channelID != 0)
                {
                    int publishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(channelID);
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                    url = PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, isInner, publishmentSystemInfo.Additional.VisualType);
                    if (!url.Equals(PageUtils.UNCLICKED_URL))
                    {
                        PageUtils.Redirect(url);
                    }
                    else
                    {
                        Redirect_DefaultDirection();
                    }
                }
                else
                {
                    Redirect_DefaultDirection();
                }
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request["channelindex"]))
            {
                string channelIndex = HttpContext.Current.Request["channelindex"];
                int publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
                if (publishmentSystemID == 0)
                {
                    publishmentSystemID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();
                }
                if (publishmentSystemID != 0)
                {
                    int channelID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(publishmentSystemID, channelIndex);
                    if (channelID != 0)
                    {
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                        url = PageUtility.GetChannelUrl(publishmentSystemInfo, nodeInfo, publishmentSystemInfo.Additional.VisualType);
                        if (!url.Equals(PageUtils.UNCLICKED_URL))
                        {
                            PageUtils.Redirect(url);
                        }
                        else
                        {
                            Redirect_DefaultDirection();
                        }
                    }
                    else
                    {
                        Redirect_DefaultDirection();
                    }
                }
                else
                {
                    Redirect_DefaultDirection();
                }
            }
            else
            {
                Redirect_DefaultDirection();
            }
        }

        public void Download()
        {
            bool isSuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["publishmentSystemID"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["fileUrl"]) && string.IsNullOrEmpty(HttpContext.Current.Request["contentID"]))
                {
                    int publishmentSystemID = TranslateUtils.ToInt(HttpContext.Current.Request["publishmentSystemID"]);
                    string fileUrl = RuntimeUtils.DecryptStringByTranslate(HttpContext.Current.Request["fileUrl"]);

                    if (PageUtils.IsProtocolUrl(fileUrl))
                    {
                        isSuccess = true;
                        PageUtils.Redirect(fileUrl);
                    }
                    else
                    {
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                        string filePath = PathUtility.MapPath(publishmentSystemInfo, fileUrl);
                        EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
                        if (EFileSystemTypeUtils.IsDownload(fileType))
                        {
                            if (FileUtils.IsFileExists(filePath))
                            {
                                isSuccess = true;
                                PageUtils.Download(HttpContext.Current.Response, filePath);
                            }
                        }
                        else
                        {
                            isSuccess = true;
                            PageUtils.Redirect(PageUtility.ParseNavigationUrl(publishmentSystemInfo, fileUrl));
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(HttpContext.Current.Request["filePath"]))
                {
                    string filePath = RuntimeUtils.DecryptStringByTranslate(HttpContext.Current.Request["filePath"]);
                    EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
                    if (EFileSystemTypeUtils.IsDownload(fileType))
                    {
                        if (FileUtils.IsFileExists(filePath))
                        {
                            isSuccess = true;
                            PageUtils.Download(HttpContext.Current.Response, filePath);
                        }
                    }
                    else
                    {
                        isSuccess = true;
                        string fileUrl = PageUtils.GetRootUrlByPhysicalPath(filePath);
                        PageUtils.Redirect(PageUtils.ParseNavigationUrl(fileUrl));
                    }
                }
                else if (!string.IsNullOrEmpty(HttpContext.Current.Request["publishmentSystemID"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["channelID"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["contentID"]) && !string.IsNullOrEmpty(HttpContext.Current.Request["fileUrl"]))
                {
                    int publishmentSystemID = TranslateUtils.ToInt(HttpContext.Current.Request["publishmentSystemID"]);
                    int channelID = TranslateUtils.ToInt(HttpContext.Current.Request["channelID"]);
                    int contentID = TranslateUtils.ToInt(HttpContext.Current.Request["contentID"]);
                    string fileUrl = RuntimeUtils.DecryptStringByTranslate(HttpContext.Current.Request["fileUrl"]);
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                    if (contentInfo != null && !string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl)))
                    {
                        //string fileUrl = contentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl);
                        if (publishmentSystemInfo.Additional.IsCountDownload)
                        {
                            CountManager.AddCount(AppManager.CMS.AppID, tableName, contentID.ToString(), ECountType.Download);
                        }

                        if (PageUtils.IsProtocolUrl(fileUrl))
                        {
                            isSuccess = true;
                            PageUtils.Redirect(fileUrl);
                        }
                        else
                        {
                            string filePath = PathUtility.MapPath(publishmentSystemInfo, fileUrl, true);
                            EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath));
                            if (EFileSystemTypeUtils.IsDownload(fileType))
                            {
                                if (FileUtils.IsFileExists(filePath))
                                {
                                    isSuccess = true;
                                    PageUtils.Download(HttpContext.Current.Response, filePath);
                                }
                            }
                            else
                            {
                                isSuccess = true;
                                PageUtils.Redirect(PageUtility.ParseNavigationUrl(publishmentSystemInfo, fileUrl));
                            }
                        }
                    }
                }
            }
            catch { }
            if (!isSuccess)
            {
                HttpContext.Current.Response.Write("下载失败，不存在此文件！");
            }
        }

        private void Redirect_DefaultDirection()
        {
            string url;
            if (HttpContext.Current.Request["ErrorUrl"] != null)
            {
                url = HttpContext.Current.Request["ErrorUrl"];
                url = PageUtils.ParseNavigationUrl(url);
                PageUtils.Redirect(url);
            }
            int publishmentSystemID = PathUtility.GetCurrentPublishmentSystemID();
            if (publishmentSystemID == 0)
            {
                publishmentSystemID = DataProvider.PublishmentSystemDAO.GetPublishmentSystemIDByIsHeadquarters();
            }
            if (publishmentSystemID != 0)
            {
                url = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID).PublishmentSystemUrl;
                FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                string filePath = System.IO.Path.Combine(FSO.PublishmentSystemPath, "Utility\\RedirectError.aspx");
                if (System.IO.File.Exists(filePath))
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (string key in HttpContext.Current.Request.QueryString.Keys)
                    {
                        builder.Append(string.Format("&{0}={1}", key, HttpContext.Current.Request[key]));
                    }
                    if (builder.Length > 0)
                    {
                        builder.Remove(0, 1);
                    }
                    url = url + string.Format("/Utility/RedirectError.aspx?{0}", builder);
                }
                PageUtils.Redirect(url);
            }
            else
            {
                url = ConfigUtils.Instance.ApplicationPath;
                PageUtils.Redirect(url);
            }
        }


    }
}