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

namespace SiteServer.BBS.Pages
{
    public class AccessPage : BasePage
    {
        protected TextBox txtPassword;

        private int forumID;
        private string returnUrl;

        public static string GetAccessUrl(int publishmentSystemID, int forumID, string returnUrl)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("access.aspx?forumID={0}&returnUrl={1}", forumID, RuntimeUtils.EncryptStringByTranslate(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            this.forumID = base.GetIntQueryString("forumID");
            this.returnUrl = RuntimeUtils.DecryptStringByTranslate(base.Request.QueryString["returnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = PageUtilityBBS.GetIndexPageUrl(base.PublishmentSystemID);
            }
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);
                if (forumInfo != null)
                {
                    if (this.txtPassword.Text == forumInfo.Additional.AccessPassword)
                    {
                        AccessManager.SetViewPassword(this.forumID, this.txtPassword.Text);
                        CookieUtils.SetCookie("access_password_" + forumInfo.ForumID, this.txtPassword.Text, DateTime.Now.AddDays(1));
                        PageUtils.Redirect(this.returnUrl);
                    }
                    else
                    {
                        base.FailureMessage("密码不正确，请重新输入。");
                    }
                }
            }
        }

        
    }
}
