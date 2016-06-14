using System;

using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using System.Collections;
using System.Data;
using System.Text;
using BaiRong.Model;

namespace SiteServer.BBS.Pages
{
    public class DefaultPage : BasePage
    {
        protected Literal ltlStatistics;
        protected Literal ltlUserAnalysis;
        protected Literal ltlUserList;
        protected Image iocoImg1;

        public void Page_Load(object sender, EventArgs e)
        {
            bool isClosed = this.isClosed();
            if (isClosed)
            {
                Response.Write("<script>alert('对不起，论坛已关闭！');window.opener = null;window.open('','_self');window.close();</script>");
            }
            else
            {
                
                this.ltlStatistics.Text = StringUtilityBBS.Statistics(base.PublishmentSystemID);

                int onlineCount = OnlineManager.GetOnlineCount(base.PublishmentSystemID);
                ArrayList userNameArrayList = OnlineManager.GetUserNameArrayList(base.PublishmentSystemID);
                int userCount = userNameArrayList.Count;
                int onlineAnonymousCount = onlineCount - userCount;
                for (int i = 0; i < onlineAnonymousCount; i++)
                {
                    userNameArrayList.Add(string.Empty);
                }

                if (onlineCount <= base.Additional.OnlineMaxInIndexPage)
                {
                    StringBuilder builder = new StringBuilder();
                    bool isUserOnly = base.Additional.IsOnlineUserOnly;
                    int guestGroupID = UserGroupManager.GetGroupIDByGroupType(base.PublishmentSystemInfo.GroupSN, EUserGroupType.Guest);
                    foreach (string userName in userNameArrayList)
                    {
                        if (!string.IsNullOrEmpty(userName) && isUserOnly) continue;

                        if (string.IsNullOrEmpty(userName))
                        {
                            builder.AppendFormat(@"<li><img src=""images/{0}""/>游客</li>", UserGroupManager.GetIconUrl(base.PublishmentSystemInfo.GroupSN, guestGroupID));
                        }
                        else
                        {
                            int groupID = UserManager.GetGroupID(base.PublishmentSystemInfo.GroupSN, userName);
                            builder.AppendFormat(@"<li><img src=""images/{0}""/><a href=""{1}"" target=""_blank"">{2}</a></li>", UserGroupManager.GetIconUrl(base.PublishmentSystemInfo.GroupSN, groupID), base.UserUtils.GetUserUrl(userName), userName);
                        }
                    }
                    this.ltlUserList.Text = builder.ToString();
                }

                if (onlineCount > base.Additional.OnlineMaxCount)
                {
                    base.Additional.OnlineMaxCount = onlineCount;
                    base.Additional.OnlineMaxDateTime = DateUtils.GetDateAndTimeString(DateTime.Now);

                    ConfigurationManager.Update(base.PublishmentSystemID);
                }

                string maxString = string.Empty;
                if (!string.IsNullOrEmpty(base.Additional.OnlineMaxDateTime) && base.Additional.OnlineMaxCount > 0)
                {
                    maxString = string.Format(@",最多 {0} 人发生在 {1} ", base.Additional.OnlineMaxCount, base.Additional.OnlineMaxDateTime);
                }
                this.ltlUserAnalysis.Text = string.Format(@"共 {0} 人在线,{1} 位会员,{2} 位访客{3}", onlineCount, userCount, onlineAnonymousCount, maxString);
            }
        }

        public bool isClosed()
        {
            ConfigurationInfo conInfo = DataProvider.ConfigurationDAO.GetConfigurationInfo(base.PublishmentSystemID);
            string str = conInfo.SettingsXML;
            return  str.Contains("isclosebbs=True") ? true : false;
        }

        
    }
}
