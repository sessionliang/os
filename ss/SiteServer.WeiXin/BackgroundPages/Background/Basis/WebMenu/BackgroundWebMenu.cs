using System.Web.UI.WebControls;
using BaiRong.Core;
using System;


using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using System.Text;
using BaiRong.Core.WebService;
using BaiRong.Core.Net;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using System.Collections.Generic;
using SiteServer.WeiXin.Model;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP;
using SiteServer.CMS.Core;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundWebMenu : BackgroundBasePage
    {
        public Literal ltlMenu;
        public Literal ltlIFrame;
        public Button btnStatus;

        private int parentID;
        private int menuID;

        public static string GetRedirectUrl(int publishmentSystemID, int parentID, int menuID)
        {
            return PageUtils.GetWXUrl(string.Format("background_webMenu.aspx?publishmentSystemID={0}&parentID={1}&menuID={2}", publishmentSystemID, parentID, menuID));
        }

        public static string GetDeleteRedirectUrl(int publishmentSystemID, int parentID, int menuID)
        {
            return PageUtils.GetWXUrl(string.Format("background_webMenu.aspx?publishmentSystemID={0}&parentID={1}&menuID={2}&Delete=True", publishmentSystemID, parentID, menuID));
        }

        public static string GetSubtractRedirectUrl(int publishmentSystemID, int parentID, int menuID)
        {
            return PageUtils.GetWXUrl(string.Format("background_menu.aspx?publishmentSystemID={0}&parentID={1}&menuID={2}&Subtract=True", publishmentSystemID, parentID, menuID));
        }
        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            this.menuID = TranslateUtils.ToInt(base.GetQueryString("menuID"));
            this.parentID = TranslateUtils.ToInt(base.GetQueryString("parentID"));

            if (base.Request.QueryString["Delete"] != null && this.menuID > 0)
            {
                DataProviderWX.WebMenuDAO.Delete(this.menuID);
                base.SuccessMessage("菜单删除成功！");
            }
            if (base.Request.QueryString["Subtract"] != null && this.menuID > 0)
            {
                DataProviderWX.WebMenuDAO.UpdateTaxisToUp(this.PublishmentSystemID, this.parentID, this.menuID);
                base.SuccessMessage("菜单排序成功！");
            }
            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "底部导航菜单", AppManager.Platform.Permission.Platform_Site);

                if (base.PublishmentSystemInfo.Additional.WX_IsWebMenu)
                {
                    this.btnStatus.Text = "禁用底部导航菜单";
                    this.btnStatus.CssClass = "btn btn-danger";
                }
                else
                {
                    this.btnStatus.Text = "启用底部导航菜单";
                    this.btnStatus.CssClass = "btn btn-success";
                }

                this.ltlIFrame.Text = string.Format(@"<iframe frameborder=""0"" id=""menu"" name=""menu"" width=""100%"" height=""500""></iframe>");

                List<WebMenuInfo> menuInfoList = DataProviderWX.WebMenuDAO.GetMenuInfoList(base.PublishmentSystemID, 0);

                StringBuilder builder = new StringBuilder();

                foreach (WebMenuInfo menuInfo in menuInfoList)
                {
                    List<WebMenuInfo> subMenuInfoList = DataProviderWX.WebMenuDAO.GetMenuInfoList(base.PublishmentSystemID, menuInfo.ID);

                    StringBuilder subBuilder = new StringBuilder();

                    string addSubUrl = BackgroundWebMenuAdd.GetRedirectUrl(base.PublishmentSystemID, menuInfo.ID, 0);
                    subBuilder.AppendFormat(@"
                            <dd class=""add"">
                              <a href=""{0}"" target=""menu""><font>新增菜单</font></a>
                            </dd>", addSubUrl);

                    int i = 0;
                    foreach (WebMenuInfo subMenuInfo in subMenuInfoList)
                    {
                        i++;

                        string ddClass = i == subMenuInfoList.Count ? "last" : string.Empty;
                        string editSubUrl = BackgroundWebMenuAdd.GetRedirectUrl(base.PublishmentSystemID, subMenuInfo.ParentID, subMenuInfo.ID);
                        string deleteSubUrl = BackgroundWebMenu.GetDeleteRedirectUrl(base.PublishmentSystemID, subMenuInfo.ParentID, subMenuInfo.ID);
                        string subtractSubUrl = BackgroundWebMenu.GetSubtractRedirectUrl(base.PublishmentSystemID, subMenuInfo.ParentID, subMenuInfo.ID);

                        subBuilder.AppendFormat(@"                                                                   
                            <dd class=""{0}"">
                              <a href=""{1}"" target=""menu""><font>{2}</font></a>
                              <a href=""{3}"" onclick=""javascript:return confirm('此操作将删除子菜单“{2}”，确认吗？');"" class=""delete""><img src=""images/iphone-btn-delete.png""></a>
                              <a href=""{4}"" style='top:20px;' class=""delete""><img src=""images/iphone-btn-up.png""></a>                           
</dd>", ddClass, editSubUrl, subMenuInfo.MenuName, deleteSubUrl, subtractSubUrl);
                    }

                    string editUrl = BackgroundWebMenuAdd.GetRedirectUrl(base.PublishmentSystemID, menuInfo.ParentID, menuInfo.ID);
                    string subMenuStyle = this.parentID == menuInfo.ID ? string.Empty : "display:none";
                    string deleteUrl = BackgroundWebMenu.GetDeleteRedirectUrl(base.PublishmentSystemID, menuInfo.ParentID, menuInfo.ID);
                    builder.AppendFormat(@"
                    <li class=""secondMenu"">
                        <a href=""{0}"" class=""mainMenu"" target=""menu""><font>{1}</font></a>
                        <dl class=""subMenus"" style=""{2}"">
                            <span>
                                <img width=""9"" height=""6"" src=""images/iphone-btn-tri.png"">
                            </span>
                            {3}
                        </dl>
                        <a href=""{4}"" onclick=""javascript:return confirm('此操作将删除主菜单“{1}”，确认吗？');"" class=""delete""><img src=""images/iphone-btn-delete.png""></a>
                    </li>", editUrl, menuInfo.MenuName, subMenuStyle, subBuilder.ToString(), deleteUrl);
                }

                if (menuInfoList.Count < 3)
                {
                    string addUrl = BackgroundWebMenuAdd.GetRedirectUrl(base.PublishmentSystemID, 0, 0);
                    builder.AppendFormat(@"
                    <li class=""secondMenu addMain"">
                        <a href=""{0}"" class=""mainMenu"" target=""menu""><font>新增菜单</font></a>
                    </li>", addUrl);
                }

                this.ltlMenu.Text = builder.ToString();
            }
        }

        public void Sync_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    DataProviderWX.WebMenuDAO.Sync(base.PublishmentSystemID);

                    base.SuccessMessage("成功复制微信菜单到底部导航菜单！");
                    base.AddWaitAndRedirectScript(BackgroundWebMenu.GetRedirectUrl(base.PublishmentSystemID, this.parentID, this.menuID));
                }
                catch (Exception ex)
                {
                    base.FailMessage(string.Format("菜单同步失败：{0}", ex.Message));
                }
            }
        }

        public void Status_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    base.PublishmentSystemInfo.Additional.WX_IsWebMenu = !base.PublishmentSystemInfo.Additional.WX_IsWebMenu;
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    if (base.PublishmentSystemInfo.Additional.WX_IsWebMenu)
                    {
                        base.SuccessMessage("底部导航菜单启用成功");
                    }
                    else
                    {
                        base.SuccessMessage("底部导航菜单禁用成功");
                    }

                    base.AddWaitAndRedirectScript(BackgroundWebMenu.GetRedirectUrl(base.PublishmentSystemID, this.parentID, this.menuID));
                }
                catch (Exception ex)
                {
                    base.FailMessage(string.Format("失败：{0}", ex.Message));
                }
            }
        }
    }
}
