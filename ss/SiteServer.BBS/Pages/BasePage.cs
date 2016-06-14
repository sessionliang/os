using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using BaiRong.Core.Diagnostics;
using System.Collections.Generic;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Model;

namespace SiteServer.BBS.Pages
{
    public class BasePage : Page
    {
        protected Literal ltlScripts;

        protected virtual bool IsAccessable
        {
            get { return true; }
        }

        private int publishmentSystemID;

        public int PublishmentSystemID
        {
            get
            {
                if (this.publishmentSystemID == 0)
                {
                    this.publishmentSystemID = this.GetIntQueryString("publishmentSystemID");
                }
                return this.publishmentSystemID;
            }
            set
            {
                this.publishmentSystemID = value;
            }
        }

        private PublishmentSystemInfo publishmentSystemInfo;
        public PublishmentSystemInfo PublishmentSystemInfo
        {
            get
            {
                if (publishmentSystemInfo == null)
                {
                    publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.PublishmentSystemID);
                }
                return publishmentSystemInfo;
            }
        }

        private UserUtils userUtis;
        public UserUtils UserUtils
        {
            get
            {
                if (this.userUtis == null)
                {
                    this.userUtis = UserUtils.GetInstance(this.publishmentSystemID);
                }
                return this.userUtis;
            }
        }

        private ForumUtils forumUtils;
        public ForumUtils ForumUtils
        {
            get
            {
                if (this.forumUtils == null)
                {
                    this.forumUtils = ForumUtils.GetInstance(this.publishmentSystemID);
                }
                return this.forumUtils;
            }
        }

        private ConfigurationInfoExtend additional;
        public ConfigurationInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = ConfigurationManager.GetAdditional(this.PublishmentSystemID);
                }
                return this.additional;
            }
        }

        public BasePage() { }

        protected override void OnLoad(EventArgs e)
        {
            OnlineManager.UpdateOnlineUser(this.PublishmentSystemID);

            UserGroupInfo groupInfo = UserGroupManager.GetCurrent(this.PublishmentSystemInfo.GroupSN);

            if (groupInfo.Additional.IsAllowVisit)
            {
                if (RestrictionManager.IsVisitAllowed(this.Additional.RestrictionType, additional.RestrictionBlackList, additional.RestrictionWhiteList))
                {
                    if (!IsAccessable && BaiRongDataProvider.UserDAO.IsAnonymous && !groupInfo.Additional.IsAllowVisit)
                    {
                        PageUtils.Redirect(LoginPage.GetLoginUrl(this.PublishmentSystemID, string.Empty, Page.Request.RawUrl));
                    }

                    base.OnInit(e);

                    if (this.ltlScripts != null)
                    {
                        this.ltlScripts.Text = AdManager.GetAdContents(this.PublishmentSystemID);
                    }
                }
                else
                {
                    Page.Response.Write("<h1>此页面禁止访问.</h1>");
                    Page.Response.Write(string.Format("<p>IP地址：{0}<br />需要访问此页面请与网站管理员联系开通相关权限.</p>", PageUtils.GetIPAddress()));
                    Page.Response.End();
                }
            }
            else
            {
                string redirectUrl = LoginPage.GetLoginUrl(this.PublishmentSystemID, "您没有论坛访问权限，请使用对应账号登录。", base.Request.RawUrl);
                PageUtils.Redirect(redirectUrl);
            }

            base.OnLoad(e);
        }

        private void ShowDig(string functionName, string message, string callback)
        {
            if (this.ltlScripts != null)
            {
                if (string.IsNullOrEmpty(callback))
                {
                    this.ltlScripts.Text += string.Format(@"
<script type=""text/javascript"" language=""javascript"">{0}('{1}');</script>
", functionName, message);
                }
                else
                {
                    this.ltlScripts.Text += string.Format(@"
<script type=""text/javascript"" language=""javascript"">{0}('{1}', {2});</script>
", functionName, message, callback);
                }
            }
        }

        public void SuccessMessage(string message)
        {
            this.ShowDig("successMessage", message, string.Empty);
        }

        public void SuccessMessage(string message, string redirectUrl)
        {
            string callback = string.Format("function() {{location.href='{0}';}}", redirectUrl);
            this.ShowDig("successMessage", message, callback);
        }

        public void FailureMessage(string message)
        {
            this.ShowDig("failureMessage", message, string.Empty);
        }

        public void InfoMessage(string message)
        {
            this.ShowDig("infoMessage", message, string.Empty);
        }

        //public string GetNameUserMessage()
        //{
        //    string[] counts = UserMessageManager.GetMessageCounts(BaiRongDataProvider.UserDAO.CurrentUserName);
        //    string messageUnReadedCount = counts[0];

        //    string messageCountHtml = (messageUnReadedCount != "0") ? string.Format("<img src='{0}' align='absmiddle' border='0' />({1})", PageUtils.GetIconUrl("message.gif"), messageUnReadedCount) : string.Format("<img src='{0}' align='absmiddle' border='0' />(0)", PageUtils.GetIconUrl("messageEmpty.gif"));
        //    string messageHtml = string.Format("<a href='{0}' style='text-decoration:none;background-image:none;padding:0px;'; target='_usercenter'>{1}</a>", UserPageUtils.GetUserCenterIndexUrl("0,4"), messageCountHtml);
        //    return messageHtml;
        //}

        public bool IsFullScreen
        {
            get
            {
                return this.Additional.IsFullScreen;
            }
        }

        public bool IsRegisterAllowed
        {
            get
            {
                return UserConfigManager.Additional.IsRegisterAllowed;
            }
        }

        private Dictionary<string, PagerInfo> m_Pagers = null;

        public PagerInfo GetPagerInfo(string pagerID)
        {
            if (m_Pagers == null)
                return new PagerInfo();

            PagerInfo pager;

            if (m_Pagers.TryGetValue(pagerID, out pager))
                return pager;

            return new PagerInfo();

            //PagerInfo pagerInfo = new PagerInfo();
            //pagerInfo.ButtonCount = 10;
            //pagerInfo.TotalRecords = 20;
            //pagerInfo.PageSize = 4;
            //pagerInfo.PageNumber = 3;
            //return pagerInfo;
        }

        public void SetPager(string id, string urlFormat, int pageNumber, int pageSize, int totalRecords, int reduceCount, int buttonCount)
        {
            PagerInfo pager = new PagerInfo();
            pager.ID = id;

            pager.ReduceCount = reduceCount;

            if (pageNumber > 0)
            {
                pager.PageNumber = pageNumber;
            }

            if (pageSize > 0)
            {
                pager.PageSize = pageSize;
            }

            if (totalRecords >= 0)
            {
                pager.TotalRecords = totalRecords;
            }

            pager.UrlFormat = urlFormat;
            if (buttonCount > 0)
            {
                pager.ButtonCount = buttonCount;
            }

            if (m_Pagers == null)
            {
                m_Pagers = new Dictionary<string, PagerInfo>(StringComparer.OrdinalIgnoreCase);
            }

            m_Pagers[id] = pager;
        }

        protected string GetQueryString(string name)
        {
            return GetQueryStringNoSql(name);
        }

        protected int GetIntQueryString(string name)
        {
            return TranslateUtils.ToInt(base.Request.QueryString[name]);
        }

        protected bool GetBoolQueryString(string name)
        {
            return TranslateUtils.ToBool(base.Request.QueryString[name]);
        }

        protected string GetQueryStringNoSql(string name)
        {
            string value = base.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterSql(value);
        }

        protected string GetQueryStringNoXss(string name)
        {
            string value = base.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterXSS(value);
        }

        protected string GetQueryStringNoSqlAndXss(string name)
        {
            string value = base.Request.QueryString[name];
            if (value == null) return null;
            return PageUtils.FilterSqlAndXss(value);
        }
    }
}
