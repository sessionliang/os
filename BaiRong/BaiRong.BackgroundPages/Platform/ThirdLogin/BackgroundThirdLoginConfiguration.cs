using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace BaiRong.BackgroundPages
{
    public class BackgroundThirdLoginConfiguration : BackgroundBasePage
    {
        public TextBox tbThirdLoginName;
        public Literal ltlThirdLoginType;

        public PlaceHolder phLoginAuth;
        public TextBox tbLoginAuthAppKey;
        public TextBox tbLoginAuthAppSercet;
        public TextBox tbLoginAuthCallBackUrl;
        public Literal ltlLoginAuthCallBackUrl;

        public BREditor breDescription;

        private int thirdLoginID;

        public static string GetRedirectUrl(int publishmentSystemID, int thirdLoginID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_thirdLoginConfiguration.aspx?publishmentSystemID={0}&thirdLoginID={1}", publishmentSystemID, thirdLoginID));
        }

        public static string GetRedirectUrl(int thirdLoginID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_thirdLoginConfiguration.aspx?publishmentSystemID={0}&thirdLoginID={1}", 0, thirdLoginID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.thirdLoginID = base.GetIntQueryString("thirdLoginID");

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserBasicSetting, "授权登录管理", AppManager.User.Permission.Usercenter_Setting);

                string publishmentSystemUrl = HttpContext.Current.Request.Url.Host.ToString() + "/SiteFiles/Services/Platform/iframe/authLogin.html";
                this.ltlLoginAuthCallBackUrl.Text = "<span style='color:red;'>" + publishmentSystemUrl + "</span>";
                this.tbLoginAuthCallBackUrl.Text = publishmentSystemUrl;
                if (this.thirdLoginID > 0)
                {
                    BaiRongThirdLoginInfo siteserverThirdLoginInfo = BaiRongDataProvider.BaiRongThirdLoginDAO.GetSiteserverThirdLoginInfo(this.thirdLoginID);
                    if (siteserverThirdLoginInfo != null)
                    {
                        this.tbThirdLoginName.Text = siteserverThirdLoginInfo.ThirdLoginName;
                        this.ltlThirdLoginType.Text = ESiteserverThirdLoginTypeUtils.GetText(siteserverThirdLoginInfo.ThirdLoginType);

                        ThirdLoginAuthInfo authInfo = new ThirdLoginAuthInfo(siteserverThirdLoginInfo.SettingsXML);

                        this.phLoginAuth.Visible = true;
                        this.tbLoginAuthAppKey.Text = authInfo.AppKey;
                        this.tbLoginAuthAppSercet.Text = authInfo.AppSercet;
                        this.tbLoginAuthCallBackUrl.Text = authInfo.CallBackUrl;

                        this.breDescription.Text = siteserverThirdLoginInfo.Description;
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                BaiRongThirdLoginInfo siteserverThirdLoginInfo = new BaiRongThirdLoginInfo();
                if (this.thirdLoginID > 0)
                {
                    siteserverThirdLoginInfo = BaiRongDataProvider.BaiRongThirdLoginDAO.GetSiteserverThirdLoginInfo(this.thirdLoginID);
                }

                siteserverThirdLoginInfo.ThirdLoginName = this.tbThirdLoginName.Text;

                ThirdLoginAuthInfo authInfo = new ThirdLoginAuthInfo(siteserverThirdLoginInfo.SettingsXML);


                authInfo.AppKey = this.tbLoginAuthAppKey.Text;
                authInfo.AppSercet = this.tbLoginAuthAppSercet.Text; ;
                authInfo.CallBackUrl = this.tbLoginAuthCallBackUrl.Text; ;

                siteserverThirdLoginInfo.SettingsXML = authInfo.ToString();

                siteserverThirdLoginInfo.Description = this.breDescription.Text;
                siteserverThirdLoginInfo.IsEnabled = true;//设置成功之后，启用

                if (this.thirdLoginID > 0)
                {
                    BaiRongDataProvider.BaiRongThirdLoginDAO.Update(siteserverThirdLoginInfo);
                }
                else
                {
                    BaiRongDataProvider.BaiRongThirdLoginDAO.Insert(siteserverThirdLoginInfo);
                }

                base.SuccessMessage("配置登录方式成功！");

                base.AddWaitAndRedirectScript(BackgroundThirdLogin.GetRedirectUrl());
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
