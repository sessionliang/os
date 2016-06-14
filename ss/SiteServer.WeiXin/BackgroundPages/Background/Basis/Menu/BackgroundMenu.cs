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
using SiteServer.CMS.Model;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundMenu : BackgroundBasePageWX
    {
        public Literal ltlMenu;
        public Literal ltlIFrame;

        private int parentID;
        private int menuID;

        public static string GetRedirectUrl(int publishmentSystemID, int parentID, int menuID)
        {
            return PageUtils.GetWXUrl(string.Format("background_menu.aspx?publishmentSystemID={0}&parentID={1}&menuID={2}", publishmentSystemID, parentID, menuID));
        }

        public static string GetDeleteRedirectUrl(int publishmentSystemID, int parentID, int menuID)
        {
            return PageUtils.GetWXUrl(string.Format("background_menu.aspx?publishmentSystemID={0}&parentID={1}&menuID={2}&Delete=True", publishmentSystemID, parentID, menuID));
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
                DataProviderWX.MenuDAO.Delete(this.menuID);
                base.SuccessMessage("菜单删除成功！");
            }
            if (base.Request.QueryString["Subtract"] != null && this.menuID > 0)
            {
                DataProviderWX.MenuDAO.UpdateTaxisToUp(this.parentID, this.menuID);
                base.SuccessMessage("菜单排序成功！");
            }
            if (!IsPostBack)
            {

                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Accounts, AppManager.WeiXin.LeftMenu.Function.ID_Menu, string.Empty, AppManager.WeiXin.Permission.WebSite.Menu);
                AccountInfo accountInfo = WeiXinManager.GetAccountInfo(base.PublishmentSystemID);
                if (EWXAccountTypeUtils.Equals(accountInfo.AccountType, EWXAccountType.Subscribe))
                {
                    PageUtils.RedirectToErrorPage(@"您的微信公众账号类型为订阅号（非认证），微信目前不支持订阅号自定义菜单。如果您的公众账号类型不是订阅号，请到账户信息中设置微信绑定账号。");
                    return;
                }

                this.ltlIFrame.Text = string.Format(@"<iframe frameborder=""0"" id=""menu"" name=""menu"" width=""100%"" height=""500""></iframe>");

                List<MenuInfo> menuInfoList = DataProviderWX.MenuDAO.GetMenuInfoList(base.PublishmentSystemID, 0);

                StringBuilder builder = new StringBuilder();

                foreach (MenuInfo menuInfo in menuInfoList)
                {
                    List<MenuInfo> subMenuInfoList = DataProviderWX.MenuDAO.GetMenuInfoList(base.PublishmentSystemID, menuInfo.MenuID);

                    StringBuilder subBuilder = new StringBuilder();

                    if (subMenuInfoList.Count < 5)
                    {
                        string addSubUrl = BackgroundMenuAdd.GetRedirectUrl(base.PublishmentSystemID, menuInfo.MenuID, 0);
                        subBuilder.AppendFormat(@"
                            <dd class=""add"">
                              <a href=""{0}"" target=""menu""><font>新增菜单</font></a>
                            </dd>", addSubUrl);
                    }

                    int i = 0;
                    foreach (MenuInfo subMenuInfo in subMenuInfoList)
                    {
                        i++;

                        string ddClass = i == subMenuInfoList.Count ? "last" : string.Empty;
                        string editSubUrl = BackgroundMenuAdd.GetRedirectUrl(base.PublishmentSystemID, subMenuInfo.ParentID, subMenuInfo.MenuID);
                        string deleteSubUrl = BackgroundMenu.GetDeleteRedirectUrl(base.PublishmentSystemID, subMenuInfo.ParentID, subMenuInfo.MenuID);
                        string subtractSubUrl = BackgroundMenu.GetSubtractRedirectUrl(base.PublishmentSystemID, subMenuInfo.ParentID, subMenuInfo.MenuID);


                        subBuilder.AppendFormat(@"                                                                   
                            <dd class=""{0}"">
                              <a href=""{1}"" target=""menu""><font>{2}</font></a>
                              <a href=""{3}"" onclick=""javascript:return confirm('此操作将删除子菜单“{2}”，确认吗？');"" class=""delete""><img src=""images/iphone-btn-delete.png""></a>
                              <a href=""{4}"" style='top:20px;' class=""delete""><img src=""images/iphone-btn-up.png""></a>
                            </dd>", ddClass, editSubUrl, subMenuInfo.MenuName, deleteSubUrl, subtractSubUrl);
                    }

                    string editUrl = BackgroundMenuAdd.GetRedirectUrl(base.PublishmentSystemID, menuInfo.ParentID, menuInfo.MenuID);
                    string subMenuStyle = this.parentID == menuInfo.MenuID ? string.Empty : "display:none";
                    string deleteUrl = BackgroundMenu.GetDeleteRedirectUrl(base.PublishmentSystemID, menuInfo.ParentID, menuInfo.MenuID);
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
                    string addUrl = BackgroundMenuAdd.GetRedirectUrl(base.PublishmentSystemID, 0, 0);
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
                    bool isSync = false;
                    string errorMessage = string.Empty;

                    AccountInfo accountInfo = WeiXinManager.GetAccountInfo(base.PublishmentSystemID);

                    if (WeiXinManager.IsBinding(accountInfo))
                    {
                        GetMenuResultFull resultFull = new GetMenuResultFull();
                        resultFull.menu = new MenuFull_ButtonGroup();
                        resultFull.menu.button = new List<MenuFull_RootButton>();

                        string publishmentSystemUrl = PageUtils.AddProtocolToUrl(PageUtility.GetPublishmentSystemUrl(base.PublishmentSystemInfo, string.Empty));

                        List<MenuInfo> menuInfoList = DataProviderWX.MenuDAO.GetMenuInfoList(base.PublishmentSystemID, 0);
                        foreach (MenuInfo menuInfo in menuInfoList)
                        {
                            MenuFull_RootButton rootButton = new MenuFull_RootButton();
                            rootButton.name = menuInfo.MenuName;

                            rootButton.sub_button = new List<MenuFull_RootButton>();
                            List<MenuInfo> subMenuInfoList = DataProviderWX.MenuDAO.GetMenuInfoList(base.PublishmentSystemID, menuInfo.MenuID);

                            if (subMenuInfoList.Count > 0)
                            {
                                foreach (MenuInfo subMenuInfo in subMenuInfoList)
                                {
                                    MenuFull_RootButton subButton = new MenuFull_RootButton();

                                    bool isExists = false;

                                    subButton.name = subMenuInfo.MenuName;
                                    if (subMenuInfo.MenuType == EMenuType.Site)
                                    {
                                        string pageUrl = string.Empty;
                                        if (subMenuInfo.ContentID > 0)
                                        {
                                            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, subMenuInfo.ChannelID);
                                            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, subMenuInfo.ChannelID);

                                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoNotTrash(tableStyle, tableName, subMenuInfo.ContentID);
                                            if (contentInfo != null)
                                            {
                                                pageUrl = PageUtility.GetContentUrl(base.PublishmentSystemInfo, contentInfo, true, base.PublishmentSystemInfo.Additional.VisualType);
                                            }
                                        }
                                        else
                                        {
                                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, subMenuInfo.ChannelID);
                                            if (nodeInfo != null)
                                            {
                                                pageUrl = PageUtility.GetChannelUrl(base.PublishmentSystemInfo, nodeInfo, base.PublishmentSystemInfo.Additional.VisualType);
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(pageUrl))
                                        {
                                            isExists = true;
                                            subButton.type = "view";
                                            subButton.url = PageUtils.AddProtocolToUrl(pageUrl);
                                        }
                                    }
                                    else if (subMenuInfo.MenuType == EMenuType.Keyword)
                                    {
                                        if (KeywordManager.IsExists(base.PublishmentSystemID, subMenuInfo.Keyword))
                                        {
                                            isExists = true;
                                            subButton.type = "click";
                                            subButton.key = subMenuInfo.Keyword;
                                        }
                                    }
                                    else if (subMenuInfo.MenuType == EMenuType.Url)
                                    {
                                        if (!string.IsNullOrEmpty(subMenuInfo.Url))
                                        {
                                            isExists = true;
                                            subButton.type = "view";
                                            subButton.url = subMenuInfo.Url;
                                        }
                                    }

                                    if (!isExists)
                                    {
                                        subButton.type = "view";
                                        subButton.url = publishmentSystemUrl;
                                    }

                                    rootButton.sub_button.Add(subButton);
                                }
                            }
                            else
                            {
                                bool isExists = false;

                                if (menuInfo.MenuType == EMenuType.Site)
                                {
                                    string pageUrl = string.Empty;
                                    if (menuInfo.ContentID > 0)
                                    {
                                        ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, menuInfo.ChannelID);
                                        string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, menuInfo.ChannelID);

                                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfoNotTrash(tableStyle, tableName, menuInfo.ContentID);
                                        if (contentInfo != null)
                                        {
                                            pageUrl = PageUtility.GetContentUrl(base.PublishmentSystemInfo, contentInfo, true, base.PublishmentSystemInfo.Additional.VisualType);
                                        }
                                    }
                                    else
                                    {
                                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, menuInfo.ChannelID);
                                        if (nodeInfo != null)
                                        {
                                            pageUrl = PageUtility.GetChannelUrl(base.PublishmentSystemInfo, nodeInfo, base.PublishmentSystemInfo.Additional.VisualType);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(pageUrl))
                                    {
                                        isExists = true;
                                        rootButton.type = "view";
                                        rootButton.url = PageUtils.AddProtocolToUrl(pageUrl);
                                    }
                                }
                                else if (menuInfo.MenuType == EMenuType.Keyword)
                                {
                                    if (KeywordManager.IsExists(base.PublishmentSystemID, menuInfo.Keyword))
                                    {
                                        isExists = true;
                                        rootButton.type = "click";
                                        rootButton.key = menuInfo.Keyword;
                                    }
                                }
                                else if (menuInfo.MenuType == EMenuType.Url)
                                {
                                    if (!string.IsNullOrEmpty(menuInfo.Url))
                                    {
                                        isExists = true;
                                        rootButton.type = "view";
                                        rootButton.url = menuInfo.Url;
                                    }
                                }

                                if (!isExists)
                                {
                                    rootButton.type = "view";
                                    rootButton.url = publishmentSystemUrl;
                                }
                            }

                            resultFull.menu.button.Add(rootButton);
                        }

                        isSync = this.SyncMenu(resultFull, accountInfo, out errorMessage);
                    }
                    else
                    {
                        errorMessage = "您的微信公众号未绑定，请先绑定之后同步菜单";
                    }

                    if (isSync)
                    {
                        base.SuccessMessage("菜单同步成功，取消关注公众账号后再次关注，可以立即看到创建后的效果");
                    }
                    else
                    {
                        base.FailMessage(string.Format("菜单同步失败：{0}", errorMessage));
                        ErrorLogInfo logInfo = new ErrorLogInfo(0, DateTime.Now, errorMessage, string.Empty, "微信同步菜单错误");
                        LogUtils.AddErrorLog(logInfo);
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(string.Format("菜单同步失败：{0}", ex.Message));

                    ErrorLogInfo logInfo = new ErrorLogInfo(0, DateTime.Now, ex.Message, ex.StackTrace, "微信同步菜单错误");
                    LogUtils.AddErrorLog(logInfo);
                }
            }
        }

        private bool SyncMenu(GetMenuResultFull resultFull, AccountInfo accountInfo, out string errorMessage)
        {
            bool isSync = false;
            errorMessage = string.Empty;

            var bg = CommonApi.GetMenuFromJsonResult(resultFull).menu;
            string accessToken = MPUtils.GetAccessToken(accountInfo);
            var result = CommonApi.CreateMenu(accessToken, bg);

            if (result.errmsg == "ok")
            {
                isSync = true;
            }
            else
            {
                isSync = false;
                errorMessage = result.errmsg;
            }

            return isSync;
        }

        public void Delete_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    AccountInfo accountInfo = WeiXinManager.GetAccountInfo(base.PublishmentSystemID);

                    string accessToken = MPUtils.GetAccessToken(accountInfo);

                    var result = CommonApi.DeleteMenu(accessToken);

                    if (result.errmsg == "ok")
                    {
                        base.SuccessMessage("菜单禁用成功，取消关注公众账号后再次关注，可以立即看到禁用后的效果");
                    }
                    else
                    {
                        base.FailMessage(string.Format("菜单禁用失败：{0}", result.errmsg));
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(string.Format("菜单禁用失败：{0}", ex.Message));
                }
            }
        }
    }
}
