using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BaiRong.BackgroundPages
{
    public class BackgroundThirdLogin : BackgroundBasePage
    {
        public Repeater rptInstalled;
        public Repeater rptUnInstalled;

        private List<BaiRongThirdLoginInfo> SiteserverThirdLoginInfoList;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_thirdLogin.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public static string GetRedirectUrl()
        {
            return PageUtils.GetPlatformUrl(string.Format("background_thirdLogin.aspx?publishmentSystemID={0}", 0));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("isInstall") != null && base.GetQueryString("siteserverThirdLoginType") != null)
            {
                ESiteserverThirdLoginType siteserverThirdLoginType = ESiteserverThirdLoginTypeUtils.GetEnumType(base.GetQueryString("siteserverThirdLoginType"));

                if (!BaiRongDataProvider.BaiRongThirdLoginDAO.IsExists(siteserverThirdLoginType))
                {
                    //安装之后，默认不可用
                    BaiRongThirdLoginInfo SiteserverThirdLoginInfo = new BaiRongThirdLoginInfo(0, siteserverThirdLoginType, ESiteserverThirdLoginTypeUtils.GetText(siteserverThirdLoginType), false, 0, ESiteserverThirdLoginTypeUtils.GetDescription(siteserverThirdLoginType), string.Empty);

                    BaiRongDataProvider.BaiRongThirdLoginDAO.Insert(SiteserverThirdLoginInfo);
                    //安装之后，直接跳转到设置页面
                    base.Response.Redirect(BaiRong.BackgroundPages.BackgroundThirdLoginConfiguration.GetRedirectUrl((int)siteserverThirdLoginType));
                    //base.SuccessMessage("登录方式安装成功");
                }
            }
            else if (base.GetQueryString("isDelete") != null && base.GetQueryString("thirdLoginID") != null)
            {
                int thirdLoginID = base.GetIntQueryString("thirdLoginID");
                if (thirdLoginID > 0)
                {
                    BaiRongDataProvider.BaiRongThirdLoginDAO.Delete(thirdLoginID);
                    base.SuccessMessage("登录方式删除成功");
                }
            }
            else if (base.GetQueryString("isEnable") != null && base.GetQueryString("thirdLoginID") != null)
            {
                int thirdLoginID = base.GetIntQueryString("thirdLoginID");
                if (thirdLoginID > 0)
                {
                    BaiRongThirdLoginInfo SiteserverThirdLoginInfo = BaiRongDataProvider.BaiRongThirdLoginDAO.GetSiteserverThirdLoginInfo(thirdLoginID);
                    if (SiteserverThirdLoginInfo != null)
                    {
                        ThirdLoginAuthInfo authInfo = new ThirdLoginAuthInfo(SiteserverThirdLoginInfo.SettingsXML);
                        if (string.IsNullOrEmpty(authInfo.AppKey) || string.IsNullOrEmpty(authInfo.AppSercet) || string.IsNullOrEmpty(authInfo.CallBackUrl))
                        {
                            base.FailMessage("请先对第三方登录方式进行设置，设置之后才能启用！");
                        }
                        else
                        {
                            string action = SiteserverThirdLoginInfo.IsEnabled ? "禁用" : "启用";
                            SiteserverThirdLoginInfo.IsEnabled = !SiteserverThirdLoginInfo.IsEnabled;
                            BaiRongDataProvider.BaiRongThirdLoginDAO.Update(SiteserverThirdLoginInfo);
                            base.SuccessMessage(string.Format("成功{0}登录方式", action));
                        }
                    }
                }
            }
            else if (base.GetQueryString("setTaxis") != null)
            {
                int thirdLoginID = base.GetIntQueryString("thirdLoginID");
                string direction = base.GetQueryString("direction");
                if (thirdLoginID > 0)
                {

                    switch (direction.ToUpper())
                    {
                        case "UP":
                            BaiRongDataProvider.BaiRongThirdLoginDAO.UpdateTaxisToUp(thirdLoginID);
                            break;
                        case "DOWN":
                            BaiRongDataProvider.BaiRongThirdLoginDAO.UpdateTaxisToDown(thirdLoginID);
                            break;
                        default:
                            break;
                    }
                    base.SuccessMessage("排序成功！");
                    base.AddWaitAndRedirectScript(BackgroundThirdLogin.GetRedirectUrl(0));
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserBasicSetting, "授权登录管理", AppManager.User.Permission.Usercenter_Setting);

                this.SiteserverThirdLoginInfoList = BaiRongDataProvider.BaiRongThirdLoginDAO.GetSiteserverThirdLoginInfoList();

                this.rptInstalled.DataSource = this.SiteserverThirdLoginInfoList;
                this.rptInstalled.ItemDataBound += new RepeaterItemEventHandler(rptInstalled_ItemDataBound);
                this.rptInstalled.DataBind();

                this.rptUnInstalled.DataSource = ESiteserverThirdLoginTypeUtils.GetESiteserverThirdLoginTypeList();
                this.rptUnInstalled.ItemDataBound += new RepeaterItemEventHandler(rptUnInstalled_ItemDataBound);
                this.rptUnInstalled.DataBind();
            }
        }

        private void rptInstalled_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                BaiRongThirdLoginInfo SiteserverThirdLoginInfo = e.Item.DataItem as BaiRongThirdLoginInfo;

                Literal ltlThirdLoginName = e.Item.FindControl("ltlThirdLoginName") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                HyperLink hlUpLink = e.Item.FindControl("hlUpLink") as HyperLink;
                HyperLink hlDownLink = e.Item.FindControl("hlDownLink") as HyperLink;
                Literal ltlConfigUrl = e.Item.FindControl("ltlConfigUrl") as Literal;
                Literal ltlIsEnabledUrl = e.Item.FindControl("ltlIsEnabledUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlThirdLoginName.Text = SiteserverThirdLoginInfo.ThirdLoginName;
                ltlDescription.Text = StringUtils.MaxLengthText(SiteserverThirdLoginInfo.Description, 200);
                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(SiteserverThirdLoginInfo.IsEnabled);
                hlUpLink.NavigateUrl = BackgroundThirdLogin.GetRedirectUrl(0) + string.Format("&setTaxis=True&thirdLoginID={0}&direction=UP", SiteserverThirdLoginInfo.ID);
                hlDownLink.NavigateUrl = BackgroundThirdLogin.GetRedirectUrl(0) + string.Format("&setTaxis=True&thirdLoginID={0}&direction=DOWN", SiteserverThirdLoginInfo.ID);

                string urlConfig = BackgroundThirdLoginConfiguration.GetRedirectUrl(0, SiteserverThirdLoginInfo.ID);
                ltlConfigUrl.Text = string.Format(@"<a href=""{0}"">设置</a>", urlConfig);

                string action = SiteserverThirdLoginInfo.IsEnabled ? "禁用" : "启用";
                string urlIsEnabled = BackgroundThirdLogin.GetRedirectUrl(0) + string.Format("&isEnable=True&thirdLoginID={0}", SiteserverThirdLoginInfo.ID);
                ltlIsEnabledUrl.Text = string.Format(@"<a href=""{0}"">{1}</a>", urlIsEnabled, action);

                string urlDelete = BackgroundThirdLogin.GetRedirectUrl(0) + string.Format("&isDelete=True&thirdLoginID={0}", SiteserverThirdLoginInfo.ID);
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onclick=""javascript:return confirm('此操作将删除选定的登录方式，确认吗？');"">删除</a>", urlDelete);
            }
        }

        private void rptUnInstalled_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ESiteserverThirdLoginType siteserverThirdLoginType = (ESiteserverThirdLoginType)e.Item.DataItem;

                foreach (BaiRongThirdLoginInfo SiteserverThirdLoginInfo in this.SiteserverThirdLoginInfoList)
                {
                    if (SiteserverThirdLoginInfo.ThirdLoginType == siteserverThirdLoginType)
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                Literal ltlThirdLoginName = e.Item.FindControl("ltlThirdLoginName") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlInstallUrl = e.Item.FindControl("ltlInstallUrl") as Literal;

                ltlThirdLoginName.Text = ESiteserverThirdLoginTypeUtils.GetText(siteserverThirdLoginType);
                ltlDescription.Text = ESiteserverThirdLoginTypeUtils.GetDescription(siteserverThirdLoginType);

                string urlInstall = BackgroundThirdLogin.GetRedirectUrl(0) + string.Format("&isInstall=True&siteserverThirdLoginType={0}", ESiteserverThirdLoginTypeUtils.GetValue(siteserverThirdLoginType));
                ltlInstallUrl.Text = string.Format(@"<a href=""{0}"">安装</a>", urlInstall);
            }
        }
    }
}
