using BaiRong.Core;
using BaiRong.Model;
using Siteserver.Core.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundThirdLogin : BackgroundBasePage
    {
        public Repeater rptInstalled;
        public Repeater rptUnInstalled;

        private List<SiteserverThirdLoginInfo> SiteserverThirdLoginInfoList;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_thirdLogin.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("isInstall") != null && base.GetQueryString("siteserverThirdLoginType") != null)
            {
                SiteserverEThirdLoginType siteserverThirdLoginType = SiteserverEThirdLoginTypeUtils.GetEnumType(base.GetQueryString("siteserverThirdLoginType"));

                if (!DataProvider.SiteserverThirdLoginDAO.IsExists(base.PublishmentSystemID, siteserverThirdLoginType))
                {
                    SiteserverThirdLoginInfo SiteserverThirdLoginInfo = new SiteserverThirdLoginInfo(0, base.PublishmentSystemID, siteserverThirdLoginType, SiteserverEThirdLoginTypeUtils.GetText(siteserverThirdLoginType), true, 0, SiteserverEThirdLoginTypeUtils.GetDescription(siteserverThirdLoginType), string.Empty);

                    DataProvider.SiteserverThirdLoginDAO.Insert(SiteserverThirdLoginInfo);
                    base.SuccessMessage("登录方式安装成功");
                }
            }
            else if (base.GetQueryString("isDelete") != null && base.GetQueryString("thirdLoginID") != null)
            {
                int thirdLoginID = base.GetIntQueryString("thirdLoginID");
                if (thirdLoginID > 0)
                {
                    DataProvider.SiteserverThirdLoginDAO.Delete(thirdLoginID);
                    base.SuccessMessage("支付方式删除成功");
                }
            }
            else if (base.GetQueryString("isEnable") != null && base.GetQueryString("thirdLoginID") != null)
            {
                int thirdLoginID = base.GetIntQueryString("thirdLoginID");
                if (thirdLoginID > 0)
                {
                    SiteserverThirdLoginInfo SiteserverThirdLoginInfo = DataProvider.SiteserverThirdLoginDAO.GetSiteserverThirdLoginInfo(thirdLoginID);
                    if (SiteserverThirdLoginInfo != null)
                    {
                        string action = SiteserverThirdLoginInfo.IsEnabled ? "禁用" : "启用";
                        SiteserverThirdLoginInfo.IsEnabled = !SiteserverThirdLoginInfo.IsEnabled;
                        DataProvider.SiteserverThirdLoginDAO.Update(SiteserverThirdLoginInfo);
                        base.SuccessMessage(string.Format("成功{0}登录方式", action));
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
                            DataProvider.SiteserverThirdLoginDAO.UpdateTaxisToUp(base.PublishmentSystemID, thirdLoginID);
                            break;
                        case "DOWN":
                            DataProvider.SiteserverThirdLoginDAO.UpdateTaxisToDown(base.PublishmentSystemID, thirdLoginID);
                            break;
                        default:
                            break;
                    }
                    base.SuccessMessage("排序成功！");
                    base.AddWaitAndRedirectScript(BackgroundThirdLogin.GetRedirectUrl(base.PublishmentSystemID));
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_ThirdLog, "登录方式", string.Empty);

                this.SiteserverThirdLoginInfoList = DataProvider.SiteserverThirdLoginDAO.GetSiteserverThirdLoginInfoList(base.PublishmentSystemID);

                this.rptInstalled.DataSource = this.SiteserverThirdLoginInfoList;
                this.rptInstalled.ItemDataBound += new RepeaterItemEventHandler(rptInstalled_ItemDataBound);
                this.rptInstalled.DataBind();

                this.rptUnInstalled.DataSource = SiteserverEThirdLoginTypeUtils.GetSiteserverEThirdLoginTypeList();
                this.rptUnInstalled.ItemDataBound += new RepeaterItemEventHandler(rptUnInstalled_ItemDataBound);
                this.rptUnInstalled.DataBind();
            }
        }

        private void rptInstalled_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SiteserverThirdLoginInfo SiteserverThirdLoginInfo = e.Item.DataItem as SiteserverThirdLoginInfo;

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
                hlUpLink.NavigateUrl = BackgroundThirdLogin.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&setTaxis=True&thirdLoginID={0}&direction=UP", SiteserverThirdLoginInfo.ID);
                hlDownLink.NavigateUrl = BackgroundThirdLogin.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&setTaxis=True&thirdLoginID={0}&direction=DOWN", SiteserverThirdLoginInfo.ID);

                string urlConfig = BackgroundThirdLoginConfiguration.GetRedirectUrl(base.PublishmentSystemID, SiteserverThirdLoginInfo.ID);
                ltlConfigUrl.Text = string.Format(@"<a href=""{0}"">设置</a>", urlConfig);

                string action = SiteserverThirdLoginInfo.IsEnabled ? "禁用" : "启用";
                string urlIsEnabled = BackgroundThirdLogin.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&isEnable=True&thirdLoginID={0}", SiteserverThirdLoginInfo.ID);
                ltlIsEnabledUrl.Text = string.Format(@"<a href=""{0}"">{1}</a>", urlIsEnabled, action);

                string urlDelete = BackgroundThirdLogin.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&isDelete=True&thirdLoginID={0}", SiteserverThirdLoginInfo.ID);
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onclick=""javascript:return confirm('此操作将删除选定的登录方式，确认吗？');"">删除</a>", urlDelete);
            }
        }

        private void rptUnInstalled_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SiteserverEThirdLoginType siteserverThirdLoginType = (SiteserverEThirdLoginType)e.Item.DataItem;

                foreach (SiteserverThirdLoginInfo SiteserverThirdLoginInfo in this.SiteserverThirdLoginInfoList)
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

                ltlThirdLoginName.Text = SiteserverEThirdLoginTypeUtils.GetText(siteserverThirdLoginType);
                ltlDescription.Text = SiteserverEThirdLoginTypeUtils.GetDescription(siteserverThirdLoginType);

                string urlInstall = BackgroundThirdLogin.GetRedirectUrl(base.PublishmentSystemID) + string.Format("&isInstall=True&siteserverThirdLoginType={0}", SiteserverEThirdLoginTypeUtils.GetValue(siteserverThirdLoginType));
                ltlInstallUrl.Text = string.Format(@"<a href=""{0}"">安装</a>", urlInstall);
            }
        }
    }
}
