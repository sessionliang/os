using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;

using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using System.Collections;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Controls;
using System.Collections.Generic;
using BaiRong.Core.Cryptography;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundMain : BackgroundBasePage
    {
        public Literal ltlTitle;
        public Literal ltlFavicon;
        public Literal ltlLogo;
        public NodeNaviTree leftMenuSite;
        public NavigationTree leftMenuSystem;

        public Repeater rptTopMenu;
        public Repeater rptTopMenuExternal;
        public Literal ltlUserName;
        public Literal ltlTopApps;

        public Literal ltlRight;

        private string menuID = string.Empty;
        private PublishmentSystemInfo publishmentSystemInfo = new PublishmentSystemInfo();
        private PublishmentSystemInfo hqPublishmentSystemInfo;
        private readonly ArrayList addedSiteIDArrayList = new ArrayList();

        public string t;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            //防止csrf，update by sessionliang at 20151214
            DESEncryptor encryptor = new DESEncryptor();
            encryptor.InputString = AdminManager.Current.UserName;
            encryptor.EncryptKey = "csrf_filter_xxxx";
            encryptor.DesEncrypt();
            t = encryptor.OutString;

            if (FileConfigManager.Instance.IsSaas)
            {
                if (FileConfigManager.Instance.OEMConfig.IsOEM)
                {
                    this.ltlTitle.Text = "微信公众号管理平台 ";
                    this.ltlLogo.Text = @"<a href=""javascript:;"" id=""gexia"" style=""background: url(images/wx.png) no-repeat;""></a>";
                    this.ltlFavicon.Text = string.Empty;
                }
                else
                {
                    this.ltlTitle.Text = "GEXIA ";
                    this.ltlLogo.Text = @"<a href=""http://www.gexia.com"" target=""_blank"" id=""gexia""></a>";
                    this.ltlFavicon.Text = @"<link rel=""icon"" type=""image/png"" href=""images/gexia_icon.png"">";
                }
            }
            else
            {
                this.ltlTitle.Text = "SiteServer ";
                this.ltlLogo.Text = @"<a href=""http://www.siteserver.cn"" target=""_blank"" id=""siteserver""></a>";
                this.ltlFavicon.Text = @"<link rel=""icon"" type=""image/png"" href=""images/siteserver_icon.png"">";
            }
            this.ltlTitle.Text += ProductManager.GetFullVersion();

            this.ltlUserName.Text = AdminManager.GetDisplayName(AdminManager.Current.UserName, true);

            this.menuID = base.GetQueryString("menuID");

            if (string.IsNullOrEmpty(this.menuID))
            {
                int publishmentSystemID = base.PublishmentSystemID;

                if (publishmentSystemID == 0)
                {
                    publishmentSystemID = AdminManager.Current.PublishmentSystemID;
                }

                List<int> publishmentSystemIDList = ProductPermissionsManager.Current.PublishmentSystemIDList;

                //排除用户中心
                publishmentSystemIDList = PublishmentSystemManager.RemoveUserCenter(publishmentSystemIDList);

                publishmentSystemIDList = PublishmentSystemManager.RemoveMLib(publishmentSystemIDList);

                //站点要判断是否存在，是否有权限，update by sessionliang 20160104
                if (publishmentSystemID == 0 || !PublishmentSystemManager.IsExists(publishmentSystemID) || !publishmentSystemIDList.Contains(publishmentSystemID))
                {
                    //ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayListOrderByLevel();
                    // List<int> publishmentSystemIDList = ProductPermissionsManager.Current.PublishmentSystemIDList;
                    if (publishmentSystemIDList != null && publishmentSystemIDList.Count > 0)
                    {
                        publishmentSystemID = publishmentSystemIDList[0];
                    }
                }

                this.publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

                if (this.publishmentSystemInfo != null && this.publishmentSystemInfo.PublishmentSystemID > 0)
                {
                    if (base.PublishmentSystemID == 0)
                    {
                        PageUtils.Redirect(this.GetRedirectUrl(this.publishmentSystemInfo.PublishmentSystemID, string.Empty, string.Empty));
                        return;
                    }

                    bool showPublishmentSystem = false;

                    ArrayList permissionArrayList = new ArrayList();
                    ArrayList websitePermissionArrayList = (ArrayList)ProductPermissionsManager.Current.WebsitePermissionSortedList[this.publishmentSystemInfo.PublishmentSystemID];
                    if (websitePermissionArrayList != null)
                    {
                        showPublishmentSystem = true;
                        permissionArrayList.AddRange(websitePermissionArrayList);
                    }

                    ICollection nodeIDCollection = ProductPermissionsManager.Current.ChannelPermissionSortedList.Keys;
                    foreach (int nodeID in nodeIDCollection)
                    {
                        if (ChannelUtility.IsAncestorOrSelf(this.publishmentSystemInfo.PublishmentSystemID, this.publishmentSystemInfo.PublishmentSystemID, nodeID))
                        {
                            showPublishmentSystem = true;
                            ArrayList arraylist = (ArrayList)ProductPermissionsManager.Current.ChannelPermissionSortedList[nodeID];
                            permissionArrayList.AddRange(arraylist);
                        }
                    }

                    Hashtable publishmentSystemIDHashtable = new Hashtable();
                    foreach (int thePublishmentSystemID in publishmentSystemIDList)
                    {
                        publishmentSystemIDHashtable.Add(thePublishmentSystemID, thePublishmentSystemID);
                    }

                    if (!publishmentSystemIDHashtable.Contains(base.PublishmentSystemID))
                    {
                        showPublishmentSystem = false;
                    }

                    if (!showPublishmentSystem)
                    {
                        PageUtils.RedirectToErrorPage("您没有此发布系统的操作权限！");
                        return;
                    }

                    string appID = EPublishmentSystemTypeUtils.GetValue(this.publishmentSystemInfo.PublishmentSystemType);

                    this.leftMenuSite.FileName = string.Format("~/SiteFiles/Products/Apps/{0}", PathUtils.Combine(appID, DirectoryUtils.Module.Menu.DirectoryName, "Management.config"));
                    this.leftMenuSite.PublishmentSystemID = this.publishmentSystemInfo.PublishmentSystemID;
                    this.leftMenuSite.PermissionArrayList = permissionArrayList;

                    JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", NodeNaviTreeItem.GetNavigationBarScript());
                }
                else
                {
                    if (PermissionsManager.Current.IsSystemAdministrator)
                    {
                        PageUtils.Redirect(PageUtils.GetSTLUrl("console_appAdd.aspx"));
                        return;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(this.menuID) || PermissionsManager.Current.PermissionArrayList.Contains(AppManager.Platform.Permission.Platform_Users))
            {
                //如果是用户中心并且没有开启过，那么左边菜单不显示
                PublishmentSystemInfo uniqueUserCenter = PublishmentSystemManager.GetUniqueUserCenter();
                if (EPublishmentSystemTypeUtils.IsUserCenter(EPublishmentSystemTypeUtils.GetEnumType(this.menuID)) && uniqueUserCenter == null)
                {

                }
                else if (EPublishmentSystemTypeUtils.IsMLib(EPublishmentSystemTypeUtils.GetEnumType(this.menuID)) && PublishmentSystemManager.GetUniqueMLib() == null)
                {

                }
                else
                {

                    ArrayList permissionArrayList = new ArrayList();
                    ArrayList websitePermissionArrayList = (ArrayList)ProductPermissionsManager.Current.WebsitePermissionSortedList[base.PublishmentSystemID];
                    if (websitePermissionArrayList != null)
                    {
                        permissionArrayList.AddRange(websitePermissionArrayList);
                    }
                    permissionArrayList.AddRange(PermissionsManager.Current.PermissionArrayList);
                    this.leftMenuSystem.ProductID = ProductManager.Apps.ProductID;
                    this.leftMenuSystem.FileName = string.Format("~/SiteFiles/Products/Apps/{0}/{1}/{2}.config", AppManager.Platform.AppID, DirectoryUtils.Module.Menu.DirectoryName, this.menuID);
                    this.leftMenuSystem.PermissionArrayList = permissionArrayList;

                    JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", NavigationTreeItem.GetNavigationBarScript());

                }
            }

            List<int> topMenuList = new List<int>();
            topMenuList.Add(1);
            topMenuList.Add(2);
            //cms超管和有权限的管理员
            if ((PermissionsManager.Current.IsConsoleAdministrator || PermissionsManager.Current.PermissionArrayList.Count > 0) && !FileConfigManager.Instance.IsSaas)
            {
                topMenuList.Add(3);
            }
            //gexia超管
            else if (FileConfigManager.Instance.IsSaas && PermissionsManager.Current.IsConsoleAdministrator)
            {
                topMenuList.Add(3);
            }
            this.rptTopMenu.DataSource = topMenuList;
            this.rptTopMenu.ItemDataBound += rptTopMenu_ItemDataBound;
            this.rptTopMenu.DataBind();

            List<string> productIDList = new List<string>();
            List<string> ownedProductIDList = RoleManager.GetProductIDList(PermissionsManager.Current.Roles);
            foreach (string productID in ProductManager.GetExternalInstalledProductIDArrayList())
            {
                if (ownedProductIDList.Contains(productID))
                {
                    productIDList.Add(productID);
                }
            }

            if (productIDList.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(@"
<li class=""dropdown"">
    <a href=""javascript:;"" class='dropdown-toggle' data-toggle=""dropdown"" title=""系统切换""><i class=""icon-retweet""></i></a>
    <ul class=""dropdown-menu pull-right"">
");
                builder.Append(@"<li><a href=""main.aspx"" target=""_self""><i class=""icon-asterisk""></i> SiteServer 管理平台</a></li>");

                foreach (string productID in productIDList)
                {
                    builder.AppendFormat(@"<li><a href=""productMain.aspx?productID={0}"" target=""_self"">{1}</a></li>", productID, ProductFileUtils.GetModuleFileInfo(productID).FullName);
                }

                builder.Append(@"
    </ul>
</li>
");
                this.ltlTopApps.Text = builder.ToString();
            }

            //update at 20141106，避免空引用异常
            if (this.publishmentSystemInfo != null && this.publishmentSystemInfo.PublishmentSystemID > 0)
            {
                BaiRongDataProvider.AdministratorDAO.UpdateLastActivityDateAndPublishmentSystemID(AdminManager.Current.UserName, this.publishmentSystemInfo.PublishmentSystemID);
            }

            string rightUrl = PageUtils.GetCMSUrl("background_right.aspx");
            if (base.PublishmentSystemInfo != null && EPublishmentSystemTypeUtils.IsWeixin(base.PublishmentSystemInfo.PublishmentSystemType))
            {
                rightUrl = PageUtils.GetWXUrl(string.Format("background_right.aspx?publishmentSystemID={0}", base.PublishmentSystemID));
            }
            if (EPublishmentSystemTypeUtils.IsUserCenter(EPublishmentSystemTypeUtils.GetEnumType(this.menuID)))
            {
                //如果是用户中心，首先需要判断是否开启；如果没有开启，那么先开启
                PublishmentSystemInfo uniqueUserCenter = PublishmentSystemManager.GetUniqueUserCenter();
                if (uniqueUserCenter == null)
                {
                    rightUrl = PageUtils.GetSTLUrl("console_UserCenterOpen.aspx");
                }
            }
            if (EPublishmentSystemTypeUtils.IsMLib(EPublishmentSystemTypeUtils.GetEnumType(this.menuID)))
            {
                //如果是用户中心，首先需要判断是否开启；如果没有开启，那么先开启
                PublishmentSystemInfo uniqueMLib = PublishmentSystemManager.GetUniqueMLib();
                if (uniqueMLib == null)
                {
                    rightUrl = PageUtils.GetSTLUrl("console_MLibOpen.aspx");
                }
            }
            ltlRight.Text = string.Format(@"<iframe frameborder=""0"" id=""right"" name=""right"" src=""{0}""></iframe>", rightUrl);
        }

        void rptTopMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal ltlMenuLi = e.Item.FindControl("ltlMenuLi") as Literal;
            Literal ltlMenuName = e.Item.FindControl("ltlMenuName") as Literal;
            Literal ltlMenues = e.Item.FindControl("ltlMenues") as Literal;
            int index = e.Item.ItemIndex;

            #region 菜单li的展现形式（已注释）
            //if (index == 0)
            //{
            //    if (this.publishmentSystemInfo != null && this.publishmentSystemInfo.PublishmentSystemID > 0)
            //    {
            //        ltlMenuLi.Text = @"<li class=""active"">";
            //        ltlMenuName.Text = string.Format(@"应用管理&nbsp;<i class=""icon-angle-right""></i> {0}&nbsp;{1}", EPublishmentSystemTypeUtils.GetIconHtml(publishmentSystemInfo.PublishmentSystemType), publishmentSystemInfo.PublishmentSystemName);
            //    }
            //    else
            //    {
            //        ltlMenuLi.Text = @"<li>";
            //        ltlMenuName.Text = "应用管理";
            //    }

            //    StringBuilder builder = new StringBuilder();

            //    bool isPublishmentSystemList = this.GetPublishmentSystemListHtml(builder);
            //    if (isPublishmentSystemList)
            //    {
            //        ltlMenues.Text = builder.ToString();
            //    }
            //    else
            //    {
            //        e.Item.Visible = false;
            //    }
            //}
            //else if (index == 1)
            //{
            //    PublishmentSystemInfo uniqueUserCenter = PublishmentSystemManager.GetUniqueUserCenter();
            //    PublishmentSystemInfo uniqueMlib = PublishmentSystemManager.GetUniqueMLib();

            //    if (this.publishmentSystemInfo != null && this.publishmentSystemInfo.PublishmentSystemID > 0)
            //    {
            //        ltlMenuLi.Text = @"<li>";
            //        ltlMenuName.Text = "应用地址";

            //        StringBuilder builder = new StringBuilder();
            //        if (this.publishmentSystemInfo.Additional.IsMultiDeployment)
            //        {
            //            builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开外网地址</a></li>", this.publishmentSystemInfo.Additional.OuterUrl);
            //            builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开内网地址</a></li>", this.publishmentSystemInfo.Additional.InnerUrl);
            //        }
            //        else
            //        {
            //            string publishmentSystemUrl = PageUtility.GetPublishmentSystemUrl(this.publishmentSystemInfo, string.Empty);

            //            builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 新窗口打开应用</a></li>", publishmentSystemUrl);
            //        }

            //        ltlMenues.Text = builder.ToString();
            //    }
            //    else if (uniqueUserCenter != null && this.menuID == "UserCenter")
            //    {
            //        ltlMenuLi.Text = @"<li>";
            //        ltlMenuName.Text = "应用地址";

            //        StringBuilder builder = new StringBuilder();
            //        if (uniqueUserCenter.Additional.IsMultiDeployment)
            //        {
            //            builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开外网地址</a></li>", uniqueUserCenter.Additional.OuterUrl);
            //            builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开内网地址</a></li>", uniqueUserCenter.Additional.InnerUrl);
            //        }
            //        else
            //        {
            //            string publishmentSystemUrl = PageUtility.GetPublishmentSystemUrl(uniqueUserCenter, string.Empty);

            //            builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 新窗口打开应用</a></li>", publishmentSystemUrl);
            //        }

            //        ltlMenues.Text = builder.ToString();
            //    }

            //    else if (uniqueMlib != null && uniqueUserCenter != null && this.menuID == "MLib")
            //    {
            //        ltlMenuLi.Text = @"<li>";
            //        ltlMenuName.Text = "应用地址";

            //        StringBuilder builder = new StringBuilder();
            //        if (uniqueMlib.Additional.IsMultiDeployment)
            //        {
            //            builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开外网地址</a></li>", uniqueMlib.Additional.OuterUrl);
            //            builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开内网地址</a></li>", uniqueMlib.Additional.InnerUrl);
            //        }
            //        else
            //        {
            //            string publishmentSystemUrl = PageUtility.GetPublishmentSystemUrl(uniqueUserCenter, string.Empty);

            //            builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 新窗口打开应用</a></li>", publishmentSystemUrl + "/contents.aspx");
            //        }

            //        ltlMenues.Text = builder.ToString();
            //    }
            //    else
            //    {
            //        e.Item.Visible = false;
            //    }
            //}
            //else if (index == 2)
            //{
            //    if (string.IsNullOrEmpty(this.menuID))
            //    {
            //        ltlMenuLi.Text = @"<li>";
            //        ltlMenuName.Text = "系统设置";
            //    }
            //    else
            //    {
            //        ltlMenuLi.Text = @"<li class=""active"">";
            //    }

            //    ArrayList tabArrayList = ProductFileUtils.GetMenuTopArrayList();

            //    StringBuilder builder = new StringBuilder();
            //    foreach (Tab tab in tabArrayList)
            //    {
            //        if (TabManager.IsValid(tab, PermissionsManager.Current.PermissionArrayList))
            //        {
            //            string loadingUrl = this.GetRedirectUrl(0, string.Empty, tab.ID);
            //            if (!string.IsNullOrEmpty(tab.Href))
            //            {
            //                loadingUrl = PageUtils.ParseNavigationUrl(tab.Href);
            //            }

            //            //如果是用户中心，那么需要publishmentSystemID
            //            if (EPublishmentSystemTypeUtils.IsUserCenter(EPublishmentSystemTypeUtils.GetEnumType(tab.ID)))
            //            {
            //                PublishmentSystemInfo uniqueUserCenter = PublishmentSystemManager.GetUniqueUserCenter();
            //                if (uniqueUserCenter != null)
            //                {
            //                    loadingUrl += string.Format("&PublishmentSystemID={0}", uniqueUserCenter.PublishmentSystemID);
            //                }
            //                //else
            //                //{
            //                //    //没有用户中心，跳转到开启用户中心
            //                //    string returnUrl = this.GetRedirectUrl(0, string.Empty, tab.ID);
            //                //    PageUtils.Redirect(PageUtils.GetSTLUrl(string.Format("console_publishmentSystemAdd.aspx?publishmentSystemType={0}", tab.ID)));
            //                //}
            //            }
            //            //如果是投稿系统，那么需要publishmentSystemID
            //            if (EPublishmentSystemTypeUtils.IsMLib(EPublishmentSystemTypeUtils.GetEnumType(tab.ID)))
            //            {
            //                PublishmentSystemInfo uniqueMLib = PublishmentSystemManager.GetUniqueMLib();
            //                if (uniqueMLib != null)
            //                {
            //                    loadingUrl += string.Format("&PublishmentSystemID={0}", uniqueMLib.PublishmentSystemID);
            //                }
            //            }

            //            string target = "_self";
            //            if (!string.IsNullOrEmpty(tab.Target))
            //            {
            //                target = tab.Target;
            //            }

            //            if (!string.IsNullOrEmpty(this.menuID) && StringUtils.EqualsIgnoreCase(tab.ID, this.menuID))
            //            {
            //                ltlMenuName.Text = string.Format(@"系统设置&nbsp;<i class=""icon-angle-right""></i> {0}", tab.Text);
            //            }
            //            //li的展现形式
            //            builder.AppendFormat(@"<li><a href=""{0}"" target=""{1}"">{2}</a></li>", loadingUrl, target, tab.Text);
            //        }
            //    }

            //    if (builder.Length == 0)
            //    {
            //        e.Item.Visible = false;
            //    }
            //    else
            //    {
            //        ltlMenues.Text = builder.ToString();
            //    }
            //} 
            #endregion

            #region 菜单平铺
            if (index == 0)
            {
                if (this.publishmentSystemInfo != null && this.publishmentSystemInfo.PublishmentSystemID > 0)
                {
                    ltlMenuLi.Text = @"<li class=""active"">";
                    ltlMenuName.Text = string.Format(@"应用管理&nbsp;<i class=""icon-angle-right""></i> {0}&nbsp;{1}", EPublishmentSystemTypeUtils.GetIconHtml(publishmentSystemInfo.PublishmentSystemType), publishmentSystemInfo.PublishmentSystemName);
                }
                else
                {
                    ltlMenuLi.Text = @"<li>";
                    ltlMenuName.Text = "应用管理";
                }

                StringBuilder builder = new StringBuilder();

                builder.Append(@"<ul class=""dropdown-menu"">");

                bool isPublishmentSystemList = this.GetPublishmentSystemListHtml(builder);

                builder.Append(@"</ul>");

                if (isPublishmentSystemList)
                {
                    ltlMenues.Text = builder.ToString();
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
            else if (index == 1)
            {
                PublishmentSystemInfo uniqueUserCenter = PublishmentSystemManager.GetUniqueUserCenter();
                PublishmentSystemInfo uniqueMlib = PublishmentSystemManager.GetUniqueMLib();

                if (this.publishmentSystemInfo != null && this.publishmentSystemInfo.PublishmentSystemID > 0)
                {
                    ltlMenuLi.Text = @"<li>";
                    ltlMenuName.Text = "应用地址";

                    StringBuilder builder = new StringBuilder();

                    builder.Append(@"<ul class=""dropdown-menu"">");

                    if (this.publishmentSystemInfo.Additional.IsMultiDeployment)
                    {
                        builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开外网地址</a></li>", this.publishmentSystemInfo.Additional.OuterUrl);
                        builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开内网地址</a></li>", this.publishmentSystemInfo.Additional.InnerUrl);
                    }
                    else
                    {
                        string publishmentSystemUrl = PageUtility.GetPublishmentSystemUrl(this.publishmentSystemInfo, string.Empty);

                        builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 新窗口打开应用</a></li>", publishmentSystemUrl);
                    }

                    builder.Append(@"</ul>");

                    ltlMenues.Text = builder.ToString();
                }
                else if (uniqueUserCenter != null && this.menuID == "UserCenter")
                {
                    ltlMenuLi.Text = @"<li>";
                    ltlMenuName.Text = "应用地址";

                    StringBuilder builder = new StringBuilder();
                    builder.Append(@"<ul class=""dropdown-menu"">");

                    if (uniqueUserCenter.Additional.IsMultiDeployment)
                    {
                        builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开外网地址</a></li>", uniqueUserCenter.Additional.OuterUrl);
                        builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开内网地址</a></li>", uniqueUserCenter.Additional.InnerUrl);
                    }
                    else
                    {
                        string publishmentSystemUrl = PageUtility.GetPublishmentSystemUrl(uniqueUserCenter, string.Empty);

                        builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 新窗口打开应用</a></li>", publishmentSystemUrl);
                    }

                    builder.Append(@"</ul>");
                    ltlMenues.Text = builder.ToString();
                }
                else if (uniqueMlib != null && uniqueUserCenter != null && this.menuID == "MLib")
                {
                    ltlMenuLi.Text = @"<li>";
                    ltlMenuName.Text = "应用地址";

                    StringBuilder builder = new StringBuilder();
                    builder.Append(@"<ul class=""dropdown-menu"">");
                    if (uniqueMlib.Additional.IsMultiDeployment)
                    {
                        builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开外网地址</a></li>", uniqueMlib.Additional.OuterUrl);
                        builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 打开内网地址</a></li>", uniqueMlib.Additional.InnerUrl);
                    }
                    else
                    {
                        string publishmentSystemUrl = PageUtility.GetPublishmentSystemUrl(uniqueUserCenter, string.Empty);

                        builder.AppendFormat(@"<li><a href=""{0}"" target=""_blank""><i class=""icon-external-link""></i> 新窗口打开应用</a></li>", publishmentSystemUrl + "/contents.aspx");
                    }
                    builder.Append(@"</ul>");
                    ltlMenues.Text = builder.ToString();
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
            else if (index == 2)
            {

                ltlMenuLi.Text = @"<li>";

                ArrayList tabArrayList = ProductFileUtils.GetMenuTopArrayList();

                StringBuilder builder = new StringBuilder();
                foreach (Tab tab in tabArrayList)
                {
                    if (TabManager.IsValid(tab, PermissionsManager.Current.PermissionArrayList))
                    {
                        string loadingUrl = this.GetRedirectUrl(0, string.Empty, tab.ID);
                        if (!string.IsNullOrEmpty(tab.Href))
                        {
                            loadingUrl = PageUtils.ParseNavigationUrl(tab.Href);
                        }

                        //如果是用户中心，那么需要publishmentSystemID
                        if (EPublishmentSystemTypeUtils.IsUserCenter(EPublishmentSystemTypeUtils.GetEnumType(tab.ID)))
                        {
                            PublishmentSystemInfo uniqueUserCenter = PublishmentSystemManager.GetUniqueUserCenter();
                            if (uniqueUserCenter != null)
                            {
                                loadingUrl += string.Format("&PublishmentSystemID={0}", uniqueUserCenter.PublishmentSystemID);
                            }
                        }
                        //如果是投稿系统，那么需要publishmentSystemID
                        if (EPublishmentSystemTypeUtils.IsMLib(EPublishmentSystemTypeUtils.GetEnumType(tab.ID)))
                        {
                            PublishmentSystemInfo uniqueMLib = PublishmentSystemManager.GetUniqueMLib();
                            if (uniqueMLib != null)
                            {
                                loadingUrl += string.Format("&PublishmentSystemID={0}", uniqueMLib.PublishmentSystemID);
                            }
                        }

                        string target = "_self";
                        if (!string.IsNullOrEmpty(tab.Target))
                        {
                            target = tab.Target;
                        }

                        //菜单平铺，update by sessionliang at 20151207
                        builder.AppendFormat(@"<span><a href=""{0}"" target=""{1}"" style=""padding: 10px 15px;color: #fff;"">{2}</a></span>", loadingUrl, target, tab.Text);
                    }
                }

                if (builder.Length == 0)
                {
                    e.Item.Visible = false;
                }
                else
                {
                    ltlMenues.Text = builder.ToString();
                }
            }
            #endregion
        }

        private string GetTopMenuHtml(string url, bool active, string menuText)
        {
            if (active)
            {
                return string.Format(@"<li class=""active""><a href=""{0}"" target=""_self"">{1}</a></li>", url, menuText);
            }
            else
            {
                return string.Format(@"<li><a href=""{0}"" target=""_self"">{1}</a></li>", url, menuText);
            }
        }

        private bool GetPublishmentSystemListHtml(StringBuilder builder)
        {

            List<int> publishmentSystemIDList = ProductPermissionsManager.Current.PublishmentSystemIDList;

            //排除用户中心
            publishmentSystemIDList = PublishmentSystemManager.RemoveUserCenter(publishmentSystemIDList);


            publishmentSystemIDList = PublishmentSystemManager.RemoveMLib(publishmentSystemIDList);

            //操作者拥有的应用列表
            ArrayList mySystemInfoArrayList = new ArrayList();

            Hashtable parentWithChildren = new Hashtable();

            if (ProductPermissionsManager.Current.IsSystemAdministrator)
            {
                foreach (int publishmentSystemID in publishmentSystemIDList)
                {
                    this.AddToMySystemInfoArrayList(mySystemInfoArrayList, parentWithChildren, publishmentSystemID);
                }
            }
            else
            {
                ICollection nodeIDCollection = ProductPermissionsManager.Current.ChannelPermissionSortedList.Keys;
                ICollection publishmentSystemIDCollection = ProductPermissionsManager.Current.WebsitePermissionSortedList.Keys;
                foreach (int publishmentSystemID in publishmentSystemIDList)
                {
                    bool showPublishmentSystem = IsShowPublishmentSystem(publishmentSystemID, publishmentSystemIDCollection, nodeIDCollection);
                    if (showPublishmentSystem)
                    {
                        this.AddToMySystemInfoArrayList(mySystemInfoArrayList, parentWithChildren, publishmentSystemID);
                    }
                }
            }

            if (PermissionsManager.Current.IsSystemAdministrator)
            {
                builder.AppendFormat(@"<li style=""background:#eee;""><a href=""{0}""><i class=""icon-plus icon-large
""></i> 创建新应用</a></li>", PageUtils.GetLoadingUrl(PageUtils.Combine("stl", "console_appAdd.aspx")));

                builder.Append(@"<li class=""divider""></li>");
            }

            if (this.hqPublishmentSystemInfo != null || mySystemInfoArrayList.Count > 0)
            {
                if (this.hqPublishmentSystemInfo != null)
                {
                    AddSite(builder, this.hqPublishmentSystemInfo, parentWithChildren, 0);
                }

                if (mySystemInfoArrayList.Count > 0)
                {
                    int count = 0;
                    foreach (PublishmentSystemInfo publishmentSystemInfo in mySystemInfoArrayList)
                    {
                        if (publishmentSystemInfo.IsHeadquarters == false)
                        {
                            count++;
                            AddSite(builder, publishmentSystemInfo, parentWithChildren, 0);
                        }
                        if (count == 13)
                        {
                            builder.Append(@"<li class=""divider""></li>");
                            builder.AppendFormat(@"<li style=""background:#eee;""><a href=""javascript:;"" onclick=""{0}""><i class=""icon-search icon-large
                    ""></i> 列出全部应用...</a></li>", Modal.PublishmentSystemSelect.GetOpenLayerString());
                            break;
                        }
                    }
                }
            }

            //if (builder.Length > 0)
            //{
            //    builder.Append(@"<li class=""divider""></li>");
            //}

            bool isPublishmentSystemList = false;
            if (PermissionsManager.Current.IsSystemAdministrator || publishmentSystemIDList.Count > 0)
            {
                isPublishmentSystemList = true;
            }

            return isPublishmentSystemList;
        }

        private static bool IsShowPublishmentSystem(int publishmentSystemID, ICollection publishmentSystemIDCollection, ICollection nodeIDCollection)
        {
            foreach (int psID in publishmentSystemIDCollection)
            {
                if (psID == publishmentSystemID)
                {
                    return true;
                }
            }
            foreach (int nodeID in nodeIDCollection)
            {
                if (ChannelUtility.IsAncestorOrSelf(publishmentSystemID, publishmentSystemID, nodeID))
                {
                    return true;
                }
            }
            return false;
        }

        private void AddToMySystemInfoArrayList(ArrayList mySystemInfoArrayList, Hashtable parentWithChildren, int publishmentSystemID)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo != null)
            {
                if (publishmentSystemInfo.IsHeadquarters)
                {
                    this.hqPublishmentSystemInfo = publishmentSystemInfo;
                }
                else if (publishmentSystemInfo.ParentPublishmentSystemID > 0)
                {
                    ArrayList children = new ArrayList();
                    if (parentWithChildren.Contains(publishmentSystemInfo.ParentPublishmentSystemID))
                    {
                        children = (ArrayList)parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID];
                    }
                    children.Add(publishmentSystemInfo);
                    parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID] = children;
                }
                mySystemInfoArrayList.Add(publishmentSystemInfo);
            }

        }

        private void AddSite(StringBuilder builder, PublishmentSystemInfo publishmentSystemInfo, Hashtable parentWithChildren, int level)
        {
            if (addedSiteIDArrayList.Contains(publishmentSystemInfo.PublishmentSystemID)) return;

            string loadingUrl = PageUtils.GetLoadingUrl(string.Format("main.aspx?PublishmentSystemID={0}", publishmentSystemInfo.PublishmentSystemID));

            if (parentWithChildren[publishmentSystemInfo.PublishmentSystemID] != null)
            {
                ArrayList children = (ArrayList)parentWithChildren[publishmentSystemInfo.PublishmentSystemID];

                builder.AppendFormat(@"
<li class=""dropdown-submenu"">
    <a tabindex=""-1"" href=""{0}"" target=""_self"">{1}&nbsp;{2}</a>
    <ul class=""dropdown-menu"">
", loadingUrl, EPublishmentSystemTypeUtils.GetIconHtml(publishmentSystemInfo.PublishmentSystemType), publishmentSystemInfo.PublishmentSystemName);

                level++;
                foreach (PublishmentSystemInfo subSiteInfo in children)
                {
                    AddSite(builder, subSiteInfo, parentWithChildren, level);
                }

                builder.Append(@"
    </ul>
</li>");
            }
            else
            {
                builder.AppendFormat(@"<li><a href=""{0}"" target=""_self"">{1}&nbsp;{2}</a></li>", loadingUrl, EPublishmentSystemTypeUtils.GetIconHtml(publishmentSystemInfo.PublishmentSystemType), publishmentSystemInfo.PublishmentSystemName);
            }

            addedSiteIDArrayList.Add(publishmentSystemInfo.PublishmentSystemID);
        }

        private string GetRedirectUrl(int publishmentSystemID, string appID, string menuID)
        {
            if (publishmentSystemID > 0)
            {
                return string.Format("main.aspx?publishmentSystemID={0}", publishmentSystemID);
            }
            else if (!string.IsNullOrEmpty(appID))
            {
                return string.Format("main.aspx?appID={0}", appID);
            }
            else if (!string.IsNullOrEmpty(menuID))
            {
                return string.Format("main.aspx?menuID={0}", menuID);
            }
            return string.Empty;
        }
    }
}
