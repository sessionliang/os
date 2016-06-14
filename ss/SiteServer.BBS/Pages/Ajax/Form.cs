using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Text;

using SiteServer.BBS.Model;
using BaiRong.Core;
using SiteServer.BBS.Core;
using BaiRong.Text.LitJson;
using BaiRong.Model;
using SiteServer.CMS.Core;

namespace SiteServer.BBS.Pages.Ajax
{
    public class Form : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
                string action = PageUtils.FilterSqlAndXss(Request.QueryString["action"]);

                Hashtable attributes = null;

                ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);
                if (RestrictionManager.IsVisitAllowed(additional.RestrictionType, additional.RestrictionBlackList, additional.RestrictionWhiteList))
                {
                    if (action == "login")
                    {
                        attributes = Login(publishmentSystemID);
                    }
                    else if (action == "logout")
                    {
                        attributes = Logout(publishmentSystemID);
                    }
                    else if (action == "register")
                    {
                        attributes = new Hashtable();

                        bool success = false;
                        string message = string.Empty;

                        string userName = PageUtils.FilterSqlAndXss(base.Request.Form["userName"]);
                        string displayName = PageUtils.FilterSqlAndXss(base.Request.Form["displayName"]);
                        string email = PageUtils.FilterSqlAndXss(base.Request.Form["email"]);
                        string password = PageUtils.FilterSqlAndXss(base.Request.Form["password"]);
                        string verifyCode = PageUtils.FilterSqlAndXss(base.Request.Form["verifyCode"]);

                        string groupSN = PublishmentSystemManager.GetGroupSN(publishmentSystemID);
                        success = UserManager.Register(groupSN, userName, password, displayName, email, string.Empty, null, verifyCode, out message);
                        BBSUserManager.AddCredits(publishmentSystemID, userName, ECreditRule.Login);

                        attributes.Add("success", success.ToString().ToLower());
                        if (success)
                        {
                            attributes.Add("successMessage", message);
                        }
                        else
                        {
                            attributes.Add("errorMessage", message);
                        }
                    }
                    else if (action == "addThread")
                    {
                        attributes = AddThread(publishmentSystemID);
                    }
                    else if (action == "addPost")
                    {
                        attributes = AddPost(publishmentSystemID, false, string.Empty);
                    }
                    else if (action == "postAllInOne")
                    {
                        attributes = PostAllInOne(publishmentSystemID);
                    }
                    else if (action == "poll")
                    {
                        attributes = Poll(publishmentSystemID);
                    }
                    else if (action == "report")
                    {
                        attributes = Report(publishmentSystemID);
                    }
                }

                string json = JsonMapper.ToJson(attributes);
                base.Response.Write(json);
                base.Response.End();
            }
        }

        private Hashtable Login(int publishmentSystemID)
        {
            Hashtable attributes = new Hashtable();

            bool success = false;
            string errorMessage = string.Empty;

            try
            {
                string userName = PageUtils.FilterSqlAndXss(base.Request.Form["userName"]);
                string password = PageUtils.FilterSqlAndXss(base.Request.Form["password"]);
                bool persistent = TranslateUtils.ToBool(base.Request.Form["persistent"]);
                bool isHide = TranslateUtils.ToBool(base.Request.Form["isHide"]);
                string verifyCode = PageUtils.FilterSqlAndXss(base.Request.Form["verifyCode"]);

                if (!string.IsNullOrEmpty(verifyCode))
                {
                    if (FileConfigManager.Instance.IsValidateCode)
                    {
                        VCManager vcManager = VCManager.GetInstanceOfLogin();
                        if (!vcManager.IsCodeValid(verifyCode))
                        {
                            errorMessage = "验证码不正确，请重新输入！";
                        }
                    }
                }

                string groupSN = PublishmentSystemManager.GetGroupSN(publishmentSystemID);

                if (string.IsNullOrEmpty(errorMessage))
                {
                    success = BaiRongDataProvider.UserDAO.Validate(groupSN, userName, password, out errorMessage);
                }
                if (success)
                {
                    DataProvider.OnlineDAO.DeleteBySessionID(publishmentSystemID, PageUtils.SessionID);
                    DataProvider.OnlineDAO.RemoveTimeOutUsers(publishmentSystemID);
                    if (isHide)
                    {
                        int groupID = UserManager.GetGroupID(groupSN, userName);
                        UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(groupSN, groupID);
                        if (groupInfo.Additional.IsAllowHide)
                        {
                            OnlineManager.AddHideOnlineUser(publishmentSystemID, userName);
                        }
                    }

                    BBSUserManager.AddCredits(publishmentSystemID, userName, ECreditRule.Login);

                    BaiRongDataProvider.UserDAO.Login(groupSN, userName, persistent);
                    //string urls = UserManager.Login(userName, persistent);
                    //if (!string.IsNullOrEmpty(urls))
                    //{
                    //    attributes.Add("isSSO", true.ToString().ToLower());
                    //    attributes.Add("ssoUrls", urls);
                    //}
                }
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            attributes.Add("success", success.ToString().ToLower());
            attributes.Add("errorMessage", "登录失败，" + errorMessage);

            return attributes;
        }

        private Hashtable Logout(int publishmentSystemID)
        {
            Hashtable attributes = new Hashtable();

            BaiRongDataProvider.UserDAO.Logout();
            //string urls = UserManager.Logout();
            //if (!string.IsNullOrEmpty(urls))
            //{
            //    attributes.Add("isSSO", true.ToString().ToLower());
            //    attributes.Add("ssoUrls", urls);
            //}

            return attributes;
        }

        private Hashtable AddThread(int publishmentSystemID)
        {
            Hashtable attributes = new Hashtable();

            bool success = false;
            string errorMessage = string.Empty;
            string url = string.Empty;

            try
            {
                int forumID = TranslateUtils.ToInt(base.Request.Form["forumID"]);

                int areaID = ForumManager.GetAreaID(publishmentSystemID, forumID);
                int categoryID = TranslateUtils.ToInt(base.Request.Form["categoryID"]);
                int fileCount = TranslateUtils.ToInt(base.Request.Form["fileCount"]);
                string title = PageUtils.FilterSqlAndXss(base.Request.Form["title"]);
                string content = PageUtils.FilterSql(base.Request.Form["content"]);

                string groupSN = PublishmentSystemManager.GetGroupSN(publishmentSystemID);

                SortedList list = DataProvider.PermissionsDAO.GetUserGroupIDWithForbiddenSortedList(groupSN, forumID);
                UserInfo userInfo = UserManager.GetUserInfo(groupSN, BaiRongDataProvider.UserDAO.CurrentUserName);
                int userGroup = userInfo.GroupID;
                if (userGroup == 0)
                {
                    userGroup = UserGroupManager.GetGroupInfoByCredits(groupSN, userInfo.Credits).GroupID;
                }

                if (list.Count > 0 && userGroup != 0)
                {
                    if (list[userGroup] != null && (list[userGroup].ToString().Contains("AddThread") || string.Equals("AddThread", list[userGroup].ToString())))
                    {
                        UserGroupInfo groupInfo = UserGroupManager.GetGroupInfo(groupSN, userGroup);
                        errorMessage = string.Format("本版块{0}禁止发帖，请使用对应账号登录", groupInfo != null ? groupInfo.GroupName : "");
                    }
                }

                ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);
                BBSUserInfo userExtendInfo = BBSUserManager.GetCurrentUserInfo();

                int maxPostPerday = BaiRongDataProvider.UserGroupDAO.GetUserGroupInfo(userGroup).Additional.MaxPostPerDay;
                DateTime fromTime = System.DateTime.Now.Date;
                DateTime toTime = System.DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                int threadCount = DataProvider.ThreadDAO.GetToadyThreadCount(publishmentSystemID, BaiRongDataProvider.UserDAO.CurrentUserName, fromTime, toTime);
                if (threadCount >= maxPostPerday && maxPostPerday != 0)
                {
                    errorMessage = string.Format("对不起，你今天的发帖量已达系统设置最大值{0}条。", maxPostPerday);
                }

                if (additional.PostInterval > 0)
                {
                    if (userInfo != null)
                    {
                        TimeSpan ts = DateTime.Now - userExtendInfo.LastPostDate;
                        if (ts.TotalSeconds < additional.PostInterval)
                        {
                            errorMessage = string.Format("两次发帖间隔时间必须大于{0}秒，请稍后！", additional.PostInterval);
                        }
                    }
                }

                if (UserUtils.GetInstance(publishmentSystemID).IsVerifyCodeThread)
                {
                    string verifyCode = PageUtils.FilterSqlAndXss(base.Request.Form["verifyCode"]);
                    VCManager vcManager = VCManager.GetInstanceOfLogin();
                    if (!vcManager.IsCodeValid(verifyCode))
                    {
                        errorMessage = "验证码不正确，请重新输入！";
                    }
                }

                if (string.IsNullOrEmpty(errorMessage))
                {
                    content = StringUtilityBBS.TextEditorContentEncode(publishmentSystemID, content);
                    bool isSignature = TranslateUtils.ToBool(base.Request.Form["isSignature"]);
                    bool isChecked = true;

                    UserGroupInfo groupInfo = UserGroupManager.GetCurrent(groupSN);
                    bool isAddable = AccessManager.IsThreadAddable(publishmentSystemID, groupInfo, forumID, string.Empty, out url);
                    if (!isAddable)
                    {
                        errorMessage = AccessManager.GetErrorMessage(EPermission.AddThread, groupInfo);
                    }
                    else
                    {
                        #region 标题
                        if (string.IsNullOrEmpty(title))
                        {
                            errorMessage = "请填写标题";
                        }
                        else
                        {
                            int titleGrade = 0;//所有级别
                            IList<KeywordsFilterInfo> titleList = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(publishmentSystemID, titleGrade);
                            StringBuilder sbTitle = new StringBuilder();
                            if (titleList.Count > 0)
                            {
                                foreach (KeywordsFilterInfo titleInfo in titleList)
                                {
                                    if (title.Contains(titleInfo.Name))
                                    {
                                        sbTitle.Append("“");
                                        sbTitle.Append(titleInfo.Name);
                                        sbTitle.Append("”，");
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(sbTitle.ToString()))
                            {
                                errorMessage += sbTitle.ToString();
                                errorMessage += "标题中含有这些敏感词，暂不能发表。";
                            }
                            #endregion
                            else
                            {
                                if (string.IsNullOrEmpty(content))
                                {
                                    errorMessage = "请填写内容";
                                }
                                else
                                {
                                    #region 禁用
                                    int forbidGrade = 1;
                                    IList<KeywordsFilterInfo> forbidList = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(publishmentSystemID, forbidGrade);
                                    StringBuilder sbForbid = new StringBuilder();
                                    if (forbidList.Count > 0)
                                    {
                                        foreach (KeywordsFilterInfo forbidInfo in forbidList)
                                        {
                                            if (content.Contains(forbidInfo.Name))
                                            {
                                                sbForbid.Append("“");
                                                sbForbid.Append(forbidInfo.Name);
                                                sbForbid.Append("”，");
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(sbForbid.ToString()))
                                    {
                                        errorMessage += sbForbid.ToString();
                                        errorMessage += "内容中含有这些敏感词，暂不能发表。";
                                    }
                                    #endregion
                                    else
                                    {
                                        #region 审核
                                        int auditGrade = 2;
                                        IList<KeywordsFilterInfo> auditList = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(publishmentSystemID, auditGrade);
                                        StringBuilder sbAudit = new StringBuilder();
                                        if (auditList.Count > 0)
                                        {
                                            foreach (KeywordsFilterInfo auditInfo in auditList)
                                            {
                                                if (content.Contains(auditInfo.Name))
                                                {
                                                    sbAudit.Append("“");
                                                    sbAudit.Append(content.Contains(auditInfo.Name));
                                                    sbAudit.Append("”,");
                                                }
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(sbAudit.ToString()))
                                        {
                                            isChecked = false;
                                        }
                                        #endregion
                                        else
                                        {
                                            #region 替换
                                            int replaceGrade = 3;
                                            IList<KeywordsFilterInfo> replaceList = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(publishmentSystemID, replaceGrade);
                                            //StringBuilder sbReplace = new StringBuilder();
                                            if (replaceList.Count > 0)
                                            {
                                                foreach (KeywordsFilterInfo replaceInfo in replaceList)
                                                {
                                                    if (content.Contains(replaceInfo.Name))
                                                    {
                                                        content = content.Replace(replaceInfo.Name, replaceInfo.Replacement);
                                                        //sbReplace.Append("“");
                                                        //sbReplace.Append(replaceInfo.Name);
                                                        //sbReplace.Append("”,");
                                                    }
                                                }
                                            }
                                            //if (!string.IsNullOrEmpty(sbReplace.ToString()))
                                            //{
                                            //errorMessage += sbReplace.ToString();
                                            //errorMessage += "内容中含有这些敏感词，相关内容将被替换。";
                                            //}
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            try
                            {
                                int postID = 0;
                                bool isAttachment = fileCount > 0;
                                EThreadType threadType = EThreadType.Post;

                                ArrayList pollItems = new ArrayList();
                                if (!string.IsNullOrEmpty(base.Request.Form["PollItems"]))//投票
                                {
                                    string[] array = PageUtils.FilterSqlAndXss(base.Request.Form["PollItems"]).Split(',');
                                    foreach (string str in array)
                                    {
                                        string val = str.Trim();
                                        if (!string.IsNullOrEmpty(val))
                                        {
                                            pollItems.Add(val);
                                        }
                                    }
                                }
                                if (pollItems.Count > 0)
                                {
                                    threadType = EThreadType.Poll;
                                }

                                int threadID = DataProvider.ThreadDAO.Insert(publishmentSystemID, areaID, forumID, categoryID, title, content, isChecked, isSignature, isAttachment, threadType, out postID);

                                if (pollItems.Count > 0)
                                {
                                    bool isVoteFirst = TranslateUtils.ToBool(base.Request.Form["IsVoteFirst"]);
                                    int maxNum = TranslateUtils.ToInt(base.Request.Form["MaxNum"]);
                                    DateTime deadline = TranslateUtils.ToDateTime(base.Request.Form["Deadline"]);
                                    PollInfo pollInfo = new PollInfo(0, publishmentSystemID, threadID, isVoteFirst, maxNum, EPollRestrictType.NoRestrict, DateTime.Now, deadline);
                                    DataProvider.PollDAO.Insert(pollInfo, pollItems);
                                }

                                this.UpdateAttachments(publishmentSystemID, threadID, fileCount, postID, content, null);

                                additional.StatPostCount += 1;
                                additional.StatTodayPostCount += 1;

                                ConfigurationManager.Update(publishmentSystemID);

                                success = true;
                                url = PageUtilityBBS.GetThreadUrl(publishmentSystemID, forumID, threadID);
                            }
                            catch (Exception ex)
                            {
                                success = false;
                                errorMessage = "发表主题失败，" + ex.Message;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            attributes.Add("success", success.ToString().ToLower());
            attributes.Add("errorMessage", errorMessage);
            attributes.Add("url", url);

            return attributes;
        }

        private Hashtable AddPost(int publishmentSystemID, bool isPostPage, string postType)
        {
            Hashtable attributes = new Hashtable();

            bool success = false;
            string errorMessage = string.Empty;
            string url = string.Empty;

            try
            {
                int forumID = TranslateUtils.ToInt(base.Request.Form["forumID"]);
                int threadID = TranslateUtils.ToInt(base.Request.Form["threadID"]);
                int replies = TranslateUtils.ToInt(base.Request.Form["replies"]);
                int fileCount = TranslateUtils.ToInt(base.Request.Form["fileCount"]);
                bool isChecked = true;
                if (isPostPage)
                {
                    replies = DataProvider.ThreadDAO.GetReplies(threadID);
                }
                string title = PageUtils.FilterSqlAndXss(base.Request.Form["title"]);
                string content = PageUtils.FilterSql(base.Request.Form["content"]);
                content = StringUtilityBBS.TextEditorContentEncode(publishmentSystemID, content);
                bool isSignature = TranslateUtils.ToBool(base.Request.Form["isSignature"]);

                string groupSN = PublishmentSystemManager.GetGroupSN(publishmentSystemID);

                ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);
                BBSUserInfo userInfo = BBSUserManager.GetCurrentUserInfo();

                if (additional.PostInterval > 0)
                {
                    TimeSpan ts = DateTime.Now - userInfo.LastPostDate;
                    if (ts.TotalSeconds < additional.PostInterval)
                    {
                        errorMessage = string.Format("两次发帖间隔时间必须大于{0}秒，请稍后！", additional.PostInterval);
                    }
                }

                if (UserUtils.GetInstance(publishmentSystemID).IsVerifyCodePost)
                {
                    string verifyCode = PageUtils.FilterSqlAndXss(base.Request.Form["verifyCode"]);
                    VCManager vcManager = VCManager.GetInstanceOfLogin();
                    if (!vcManager.IsCodeValid(verifyCode))
                    {
                        errorMessage = "验证码不正确，请重新输入！";
                    }
                }

                if (string.IsNullOrEmpty(errorMessage))
                {
                    UserGroupInfo groupInfo = UserGroupManager.GetCurrent(groupSN);
                    bool isAddable = AccessManager.IsPostAddable(publishmentSystemID, groupInfo, forumID, string.Empty, out url);
                    if (!isAddable)
                    {
                        errorMessage = AccessManager.GetErrorMessage(EPermission.AddPost, groupInfo);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(content))
                        {
                            errorMessage += "请填写内容";
                        }
                        else
                        {
                            #region 禁用
                            int forbidGrade = 1;
                            IList<KeywordsFilterInfo> forbidList = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(publishmentSystemID, forbidGrade);
                            StringBuilder sbForbid = new StringBuilder();
                            if (forbidList.Count > 0)
                            {
                                foreach (KeywordsFilterInfo forbidInfo in forbidList)
                                {
                                    if (content.Contains(forbidInfo.Name))
                                    {
                                        sbForbid.Append("“");
                                        sbForbid.Append(forbidInfo.Name);
                                        sbForbid.Append("”，");
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(sbForbid.ToString()))
                            {
                                errorMessage += sbForbid.ToString();
                                errorMessage += "内容中含有这些敏感词，暂不能发表。";
                            }
                            #endregion
                            else
                            {
                                #region 审核
                                int auditGrade = 2;
                                IList<KeywordsFilterInfo> auditList = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(publishmentSystemID, auditGrade);
                                StringBuilder sbAudit = new StringBuilder();
                                if (auditList.Count > 0)
                                {
                                    foreach (KeywordsFilterInfo auditInfo in auditList)
                                    {
                                        if (content.Contains(auditInfo.Name))
                                        {
                                            sbAudit.Append("“");
                                            sbAudit.Append(content.Contains(auditInfo.Name));
                                            sbAudit.Append("”,");
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(sbAudit.ToString()))
                                {
                                    isChecked = false;
                                }
                                #endregion
                                else
                                {
                                    #region 替换
                                    int replaceGrade = 3;
                                    IList<KeywordsFilterInfo> replaceList = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(publishmentSystemID, replaceGrade);
                                    if (replaceList.Count > 0)
                                    {
                                        foreach (KeywordsFilterInfo replaceInfo in replaceList)
                                        {
                                            if (content.Contains(replaceInfo.Name))
                                            {
                                                content = content.Replace(replaceInfo.Name, replaceInfo.Replacement);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            try
                            {
                                PostInfo postInfo = new PostInfo(0, publishmentSystemID, threadID, forumID, UserUtils.GetInstance(publishmentSystemID).UserName, title, content, isChecked, DateTime.Now, PageUtils.GetIPAddress(), false, false, false, false, false, false, false, isSignature, false, false, string.Empty);

                                if (StringUtils.EqualsIgnoreCase(postType, "Reference"))
                                {
                                    int threadPostID = TranslateUtils.ToInt(base.Request.Form["postID"]);
                                    PostInfo threadPostInfo = DataProvider.PostDAO.GetPostInfo(publishmentSystemID, threadPostID);
                                    if (threadPostInfo != null)
                                    {
                                        postInfo.Content = ThreadManager.GetPostReferenceString(threadPostInfo, false) + ThreadManager.PostReferenceSeparator + postInfo.Content;
                                    }
                                }

                                if (fileCount > 0)
                                {
                                    postInfo.IsAttachment = true;
                                }

                                int postID = DataProvider.PostDAO.InsertPostOnly(publishmentSystemID, postInfo);

                                this.UpdateAttachments(publishmentSystemID, threadID, fileCount, postID, content, null);

                                additional.StatPostCount += 1;
                                additional.StatTodayPostCount += 1;

                                ConfigurationManager.Update(publishmentSystemID);

                                success = true;
                                if (isPostPage)
                                {
                                    int page = ThreadManager.GetPostPage(publishmentSystemID, postInfo.Taxis);
                                    url = PageUtilityBBS.GetPostUrl(publishmentSystemID, forumID, threadID, page, postID);
                                    if (page <= 1)
                                    {
                                        attributes.Add("page", false.ToString().ToLower());
                                    }
                                    else
                                    {
                                        attributes.Add("page", true.ToString().ToLower());
                                    }
                                }
                                else
                                {
                                    UserUtils userUtils = UserUtils.GetInstance(publishmentSystemID);

                                    attributes.Add("postID", postID.ToString());
                                    attributes.Add("userUrl", userUtils.GetUserUrl(postInfo.UserName));
                                    attributes.Add("userImageUrl", userUtils.GetUserAvatarMiddleUrl(postInfo.UserName));
                                    attributes.Add("userName", postInfo.UserName);

                                    attributes.Add("groupName", userUtils.GetGroupName(postInfo.UserName));
                                    attributes.Add("credits", userUtils.GetCredits(postInfo.UserName));
                                    attributes.Add("postCount", userUtils.GetPostCount(postInfo.UserName));
                                    attributes.Add("prestige", userUtils.GetPrestige(postInfo.UserName));
                                    attributes.Add("contribution", userUtils.GetContribution(postInfo.UserName));
                                    attributes.Add("currency", userUtils.GetCurrency(postInfo.UserName));
                                    attributes.Add("onlineTotal", userUtils.GetOnlineTotal(postInfo.UserName));
                                    attributes.Add("creationDate", DateUtils.GetDateString(UserManager.GetCreateDate(groupSN, postInfo.UserName)));

                                    attributes.Add("addDate", DateUtils.GetDateAndTimeString(postInfo.AddDate));
                                    attributes.Add("title", title.StartsWith("Re:") ? string.Empty : title);
                                    if (!postInfo.IsChecked)
                                    {
                                        attributes.Add("content", ThreadManager.GetCheckedContent());
                                    }
                                    else
                                    {
                                        attributes.Add("content", ThreadManager.GetPostContent(publishmentSystemID, postInfo, null));
                                    }

                                    attributes.Add("floor", StringUtilityBBS.GetFloor(replies + 2));
                                    attributes.Add("editUrl", PostPage.GetUrl(publishmentSystemID, forumID, threadID, postID, string.Empty));

                                    if (userUtils.IsSignature(postInfo.IsSignature, postInfo.UserName) && isSignature)
                                    {
                                        attributes.Add("signature", string.Format(@"<div class=""sign"">{0}</div>", userUtils.GetSignature(postInfo.UserName)));
                                    }
                                    else
                                    {
                                        attributes.Add("signature", string.Empty);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                success = false;
                                errorMessage = "发表回复失败，" + ex.Message;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            attributes.Add("success", success.ToString().ToLower());
            attributes.Add("errorMessage", errorMessage);
            attributes.Add("url", url);

            return attributes;
        }

        private Hashtable EditPost(int publishmentSystemID)
        {
            Hashtable attributes = new Hashtable();

            bool success = false;
            string errorMessage = string.Empty;
            string url = string.Empty;

            try
            {
                bool isChecked = true;
                int forumID = TranslateUtils.ToInt(base.Request.Form["forumID"]);
                int threadID = TranslateUtils.ToInt(base.Request.Form["threadID"]);
                int postID = TranslateUtils.ToInt(base.Request.Form["postID"]);
                int categoryID = TranslateUtils.ToInt(base.Request.Form["categoryID"]);
                int fileCount = TranslateUtils.ToInt(base.Request.Form["fileCount"]);

                string lastEditUsername = BaiRongDataProvider.UserDAO.CurrentUserName;

                string title = PageUtils.FilterSqlAndXss(base.Request.Form["title"]);
                string content = PageUtils.FilterSql(base.Request.Form["content"]);
                content = StringUtilityBBS.TextEditorContentEncode(publishmentSystemID, content);
                bool isSignature = TranslateUtils.ToBool(base.Request.Form["isSignature"]);


                #region 标题
                if (string.IsNullOrEmpty(title))
                {
                    errorMessage = "请填写标题";
                }
                else
                {
                    int titleGrade = 0;//所有级别
                    IList<KeywordsFilterInfo> titleList = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(publishmentSystemID, titleGrade);
                    StringBuilder sbTitle = new StringBuilder();
                    if (titleList.Count > 0)
                    {
                        foreach (KeywordsFilterInfo titleInfo in titleList)
                        {
                            if (title.Contains(titleInfo.Name))
                            {
                                sbTitle.Append("“");
                                sbTitle.Append(titleInfo.Name);
                                sbTitle.Append("”，");
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(sbTitle.ToString()))
                    {
                        errorMessage += sbTitle.ToString();
                        errorMessage += "标题中含有这些敏感词，暂不能发表。";
                    }
                    #endregion
                    else
                    {
                        if (string.IsNullOrEmpty(content))
                        {
                            errorMessage = "请填写内容";
                        }
                        else
                        {
                            #region 禁用
                            int forbidGrade = 1;
                            IList<KeywordsFilterInfo> forbidList = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(publishmentSystemID, forbidGrade);
                            StringBuilder sbForbid = new StringBuilder();
                            if (forbidList.Count > 0)
                            {
                                foreach (KeywordsFilterInfo forbidInfo in forbidList)
                                {
                                    if (content.Contains(forbidInfo.Name))
                                    {
                                        sbForbid.Append("“");
                                        sbForbid.Append(forbidInfo.Name);
                                        sbForbid.Append("”，");
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(sbForbid.ToString()))
                            {
                                errorMessage += sbForbid.ToString();
                                errorMessage += "内容中含有这些敏感词，暂不能发表。";
                            }
                            #endregion
                            else
                            {
                                #region 审核
                                int auditGrade = 2;
                                IList<KeywordsFilterInfo> auditList = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(publishmentSystemID, auditGrade);
                                StringBuilder sbAudit = new StringBuilder();
                                if (auditList.Count > 0)
                                {
                                    foreach (KeywordsFilterInfo auditInfo in auditList)
                                    {
                                        if (content.Contains(auditInfo.Name))
                                        {
                                            sbAudit.Append("“");
                                            sbAudit.Append(content.Contains(auditInfo.Name));
                                            sbAudit.Append("”,");
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(sbAudit.ToString()))
                                {
                                    isChecked = false;
                                }
                                #endregion
                                else
                                {
                                    #region 替换
                                    int replaceGrade = 3;
                                    IList<KeywordsFilterInfo> replaceList = DataProvider.KeywordsFilterDAO.GetKeywordsByGrade(publishmentSystemID, replaceGrade);
                                    if (replaceList.Count > 0)
                                    {
                                        foreach (KeywordsFilterInfo replaceInfo in replaceList)
                                        {
                                            if (content.Contains(replaceInfo.Name))
                                            {
                                                content = content.Replace(replaceInfo.Name, replaceInfo.Replacement);
                                            }
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(errorMessage))
                {
                    try
                    {
                        PostInfo postInfo = DataProvider.PostDAO.GetPostInfo(publishmentSystemID, postID);
                        if (postInfo.IsThread)
                        {
                            ThreadInfo threadInfo = DataProvider.ThreadDAO.GetThreadInfo(publishmentSystemID, threadID);

                            ArrayList pollItems = new ArrayList();
                            if (!string.IsNullOrEmpty(base.Request.Form["PollItems"]))//投票
                            {
                                string[] array = PageUtils.FilterSqlAndXss(base.Request.Form["PollItems"]).Split(',');
                                foreach (string str in array)
                                {
                                    string val = str.Trim();
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        pollItems.Add(val);
                                    }
                                }
                            }
                            if (pollItems.Count > 0)
                            {
                                threadInfo.ThreadType = EThreadType.Poll;
                                PollInfo pollInfo = DataProvider.PollDAO.GetPollInfo(threadInfo.ID);
                                pollInfo.IsVoteFirst = TranslateUtils.ToBool(base.Request.Form["IsVoteFirst"]);
                                pollInfo.MaxNum = TranslateUtils.ToInt(base.Request.Form["MaxNum"]);
                                pollInfo.Deadline = TranslateUtils.ToDateTime(base.Request.Form["Deadline"]);
                                DataProvider.PollDAO.Update(pollInfo, pollItems);
                            }
                            else
                            {
                                threadInfo.ThreadType = EThreadType.Post;
                            }

                            threadInfo.Title = title;
                            if (ThreadCategoryManager.IsCategoryExists(publishmentSystemID, forumID))
                            {
                                threadInfo.CategoryID = categoryID;
                            }
                            DataProvider.ThreadDAO.Update(threadInfo);
                        }
                        else
                        {
                            string reference = ThreadManager.GetPostReference(postInfo.Content);
                            if (!string.IsNullOrEmpty(reference))
                            {
                                content = reference + ThreadManager.PostReferenceSeparator + content;
                            }
                        }
                        postInfo.LastEditUserName = lastEditUsername;
                        postInfo.LastEditDate = DateTime.Now;
                        postInfo.Title = title;
                        postInfo.Content = content;
                        postInfo.IsSignature = isSignature;
                        postInfo.IsChecked = isChecked;

                        List<int> attachIDListOrinigal = null;
                        if (postInfo.IsAttachment)
                        {
                            attachIDListOrinigal = DataProvider.AttachmentDAO.GetAttachIDList(publishmentSystemID, threadID, postID);
                        }
                        this.UpdateAttachments(publishmentSystemID, threadID, fileCount, postID, content, attachIDListOrinigal);

                        if (fileCount > 0)
                        {
                            postInfo.IsAttachment = true;
                        }

                        DataProvider.PostDAO.Update(publishmentSystemID, postInfo);

                        success = true;
                        int page = ThreadManager.GetPostPage(publishmentSystemID, postInfo.Taxis);
                        url = PageUtilityBBS.GetPostUrl(publishmentSystemID, forumID, threadID, page, postID);
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        errorMessage = "编辑回复失败，" + ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            attributes.Add("success", success.ToString().ToLower());
            attributes.Add("errorMessage", errorMessage);
            attributes.Add("url", url);

            return attributes;
        }

        private void UpdateAttachments(int publishmentSystemID, int threadID, int fileCount, int postID, string content, List<int> attachIDListOrinigal)
        {
            //附件
            if (fileCount > 0)
            {
                string fileIDs = PageUtils.FilterSqlAndXss(base.Request.Form["fileID"]);
                string fileDescriptions = PageUtils.FilterSqlAndXss(base.Request.Form["fileDescription"]);
                string filePrices = PageUtils.FilterSqlAndXss(base.Request.Form["filePrice"]);
                ArrayList fileIDArrayList = TranslateUtils.StringCollectionToIntArrayList(fileIDs.Substring(1));
                ArrayList fileDescriptionArrayList = TranslateUtils.StringCollectionToArrayList(fileDescriptions.Substring(1));
                if (fileDescriptionArrayList.Count < fileCount)
                {
                    for (int i = 0; i < fileCount - fileDescriptionArrayList.Count; i++)
                    {
                        fileDescriptionArrayList.Add(string.Empty);
                    }
                }
                ArrayList filePriceArray = TranslateUtils.StringCollectionToIntArrayList(filePrices.Substring(2));
                if (filePriceArray.Count < fileCount)
                {
                    for (int i = 0; i < fileCount - filePriceArray.Count; i++)
                    {
                        filePriceArray.Add(0);
                    }
                }

                if (fileCount == fileIDArrayList.Count)
                {
                    for (int index = 0; index < fileCount; index++)
                    {
                        int fileID = (int)fileIDArrayList[index];
                        if (fileID > 0)
                        {
                            string description = PageUtils.FilterXSS((string)fileDescriptionArrayList[index]);
                            int price = (int)filePriceArray[index];
                            string ubb = UBBUtility.GetUBB_Attachment(fileID);
                            bool isInContent = StringUtils.Contains(content, ubb);

                            DataProvider.AttachmentDAO.Update(fileID, threadID, postID, isInContent, price, description);
                        }
                    }
                }

                if (attachIDListOrinigal != null && attachIDListOrinigal.Count > 0)
                {
                    List<int> idsToDelete = new List<int>();
                    foreach (int attachID in attachIDListOrinigal)
                    {
                        if (!fileIDArrayList.Contains(attachID))
                        {
                            idsToDelete.Add(attachID);
                        }
                    }
                    if (idsToDelete.Count > 0)
                    {
                        DataProvider.AttachmentDAO.Delete(publishmentSystemID, idsToDelete);
                    }
                }
            }
        }

        private Hashtable PostAllInOne(int publishmentSystemID)
        {
            Hashtable attributes = new Hashtable();

            int forumID = TranslateUtils.ToInt(base.Request.Form["forumID"]);
            int threadID = TranslateUtils.ToInt(base.Request.Form["threadID"]);
            int postID = TranslateUtils.ToInt(base.Request.Form["postID"]);
            string postType = PageUtils.FilterSqlAndXss(base.Request.Form["postType"]);

            if (forumID > 0)
            {
                if (threadID == 0 && postID == 0)//发表帖子
                {
                    attributes = this.AddThread(publishmentSystemID);
                }
                else if (postID == 0)//回复主题
                {
                    attributes = this.AddPost(publishmentSystemID, true, string.Empty);
                }
                else
                {
                    if (string.IsNullOrEmpty(postType))//修改帖子
                    {
                        attributes = this.EditPost(publishmentSystemID);
                    }
                    else
                    {
                        attributes = this.AddPost(publishmentSystemID, true, postType);
                    }
                }
            }
            else
            {
                attributes.Add("success", false.ToString().ToLower());
                attributes.Add("errorMessage", "请指定论坛版块");
            }

            return attributes;
        }

        private Hashtable Poll(int publishmentSystemID)
        {
            Hashtable attributes = new Hashtable();

            bool success = false;
            string errorMessage = string.Empty;

            try
            {
                int threadID = TranslateUtils.ToInt(base.Request.Form["threadID"]);
                ArrayList pollItemIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.Form["pollItemID"]);

                PollInfo pollInfo = DataProvider.PollDAO.GetPollInfo(threadID);

                if (pollInfo == null)
                {
                    errorMessage = "对应的投票已被删除";
                }
                if (pollInfo.IsOverTime)
                {
                    errorMessage = "对应的投票已过期";
                }
                if (pollItemIDArrayList.Count == 0)
                {
                    errorMessage = "必须选择一项进行投票";
                }
                if (pollInfo.MaxNum > 0 && pollItemIDArrayList.Count > pollInfo.MaxNum)
                {
                    errorMessage = string.Format("最后只能选择{0}项", pollInfo.MaxNum);
                }
                if (BaiRongDataProvider.UserDAO.IsAnonymous)
                {
                    errorMessage = "投票前请先登录";
                }
                if (DataProvider.PollDAO.IsUserExists(pollInfo.ID, BaiRongDataProvider.UserDAO.CurrentUserName))
                {
                    errorMessage = "一个用户只能投一次票";
                }

                if (string.IsNullOrEmpty(errorMessage))
                {
                    try
                    {
                        DataProvider.PollDAO.AddPollNum(pollItemIDArrayList);
                        PollUserInfo userInfo = new PollUserInfo(0, publishmentSystemID, pollInfo.ID, TranslateUtils.ObjectCollectionToString(pollItemIDArrayList), PageUtils.GetIPAddress(), BaiRongDataProvider.UserDAO.CurrentUserName, DateTime.Now);
                        DataProvider.PollDAO.InsertPollUser(userInfo);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        errorMessage = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }

            attributes.Add("success", success.ToString().ToLower());
            attributes.Add("errorMessage", "投票失败，" + errorMessage);

            return attributes;
        }

        private Hashtable Report(int publishmentSystemID)
        {
            Hashtable attributes = new Hashtable();

            bool success = false;
            string errorMessage = string.Empty;
            try
            {
                int forumID = TranslateUtils.ToInt(base.Request.Form["forumID"]);
                int threadID = TranslateUtils.ToInt(base.Request.Form["threadID"]);
                int postID = TranslateUtils.ToInt(base.Request.Form["postID"]);
                string txtCon = PageUtils.FilterSqlAndXss(base.Request.Form["txtCon"]);
                PostInfo postInfo = new PostInfo(publishmentSystemID);

                if (string.IsNullOrEmpty(txtCon))
                {
                    errorMessage = "请填写内容";
                }
                if (string.IsNullOrEmpty(errorMessage))
                {
                    try
                    {
                        string ipAddress = Request.UserHostAddress.ToString();
                        string name = BaiRongDataProvider.UserDAO.CurrentUserName;
                        ReportInfo reportInfo = new ReportInfo(0, publishmentSystemID, forumID, threadID, postID, name, ipAddress, DateTime.Now, txtCon);

                        DataProvider.ReportDAO.InsertWithReport(reportInfo);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        errorMessage = "举报失败，" + ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                errorMessage = ex.Message;
            }
            attributes.Add("success", success.ToString().ToLower());
            attributes.Add("errorMessage", errorMessage);

            return attributes;
        }
    }
}
