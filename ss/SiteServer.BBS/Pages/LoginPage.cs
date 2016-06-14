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
    public class LoginPage : BasePage
    {
        public Literal ltlTips;
        public PlaceHolder phValidateCode;
        public Literal ltlValidateCode;

        private VCManager vcManager;
        protected string tips;
        protected string returnUrl;

        protected override void OnInit(EventArgs e)
        {
            
        }

        public static string GetLoginUrl(int publishmentSystemID, string tips, string returnUrl)
        {
            return PageUtilityBBS.GetBBSUrl(publishmentSystemID, string.Format("login.aspx?tips={0}&returnUrl={1}", RuntimeUtils.EncryptStringByTranslate(tips), RuntimeUtils.EncryptStringByTranslate(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            this.vcManager = VCManager.GetInstanceOfLogin();

            if (!IsPostBack)
            {
                this.tips = RuntimeUtils.DecryptStringByTranslate(base.Request.QueryString["tips"]);
                this.returnUrl = RuntimeUtils.DecryptStringByTranslate(base.Request.QueryString["returnUrl"]);
                if (string.IsNullOrEmpty(this.returnUrl))
                {
                    this.returnUrl = PageUtilityBBS.GetIndexPageUrl(base.PublishmentSystemID);
                }

                //if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                //{
                //    PageUtils.Redirect(PageUtilityBBS.GetIndexPageUrl());
                //    return;
                //}

                if (!string.IsNullOrEmpty(this.tips))
                {
                    this.ltlTips.Text = string.Format(@"<div class=""tips"">{0}</div>", this.tips);
                }
                if (FileConfigManager.Instance.IsValidateCode)
                {
                    this.ltlValidateCode.Text = string.Format(@"<img id=""imgVerify"" name=""imgVerify"" src=""{0}"" align=""absmiddle"" />", this.vcManager.GetImageUrl(true));
                }
                else
                {
                    this.phValidateCode.Visible = false;
                }
            }
        }
    }
}
