using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;


using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using SiteServer.BBS.Core;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using BaiRong.Model;

namespace SiteServer.BBS.Pages
{
    public class UserPage : BasePage
    {
        private string userFrom;
        private string userName;

        public Literal ltlUserName;
        public Literal ltlDisplayName;
        public Literal ltlAvatar;
        public Literal ltlSignature;

        public void Page_Load(object sender, EventArgs e)
        {
            this.userFrom = PageUtils.FilterSqlAndXss(base.Request.QueryString["userFrom"]);
            this.userName = PageUtils.FilterSqlAndXss(base.Request.QueryString["userName"]);

            if (!string.IsNullOrEmpty(this.userName) && BaiRongDataProvider.UserDAO.IsUserExists(base.PublishmentSystemInfo.GroupSN, this.userName))
            {
                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(this.userFrom, this.userName);

                this.ltlUserName.Text = userInfo.UserName;
                this.ltlDisplayName.Text = userInfo.DisplayName;
                this.ltlAvatar.Text = string.Format(@"<img src=""{0}"" />", PageUtils.GetUserAvatarUrl(this.userFrom, this.userName, EAvatarSize.Large));
                this.ltlSignature.Text = base.UserUtils.GetSignature(this.userName);
            }
        }
    }
}
