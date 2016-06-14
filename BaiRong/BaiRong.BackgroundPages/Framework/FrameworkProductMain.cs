using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;

using System.Collections;
using System.Text;
using BaiRong.Controls;
using System.Collections.Generic;
using BaiRong.BackgroundPages;

namespace BaiRong.BackgroundPages
{
    public class FrameworkProductMain : BackgroundBasePage
    {
        public Literal ltlTitle;
        public Literal ltlLogo;
        public NavigationTree leftMenuSystem;

        public Repeater rptTopMenu;
        public Repeater rptTopMenuExternal;
        public Literal ltlUserName;
        public Literal ltlTopApps;

        public Literal ltlRight;
        public Literal ltlScript;

        private string productID = string.Empty;
        private string menuID = string.Empty;

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

            this.productID = base.GetQueryString("productID");
            if (!string.IsNullOrEmpty(this.productID))
            {
                if (!ProductFileUtils.IsExists(this.productID))
                {
                    PageUtils.RedirectToLoginPage();
                    return;
                }
            }
            this.menuID = base.GetQueryString("menuID");

            ModuleFileInfo moduleFileInfo = ProductFileUtils.GetModuleFileInfo(this.productID);

            this.ltlTitle.Text = moduleFileInfo.ModuleName;
            this.ltlLogo.Text = @"<a href=""http://www.siteserver.cn"" target=""_blank"" id=""siteserver""></a>";

            this.ltlUserName.Text = AdminManager.GetDisplayName(AdminManager.Current.UserName, true);

            this.leftMenuSystem.ProductID = this.productID;
            if (!string.IsNullOrEmpty(this.menuID))
            {
                this.leftMenuSystem.FileName = string.Format("~/SiteFiles/Products/{0}/Menu/{1}.config", this.productID, this.menuID);
            }
            else
            {
                ArrayList tabArrayList = ProductFileUtils.GetMenuTopTabArrayList(this.productID);
                if (tabArrayList.Count > 0)
                {
                    Tab tab = tabArrayList[0] as Tab;
                    this.leftMenuSystem.FileName = string.Format("~/SiteFiles/Products/{0}/Menu/{1}.config", this.productID, tab.ID);
                }
            }
            this.leftMenuSystem.PermissionArrayList = PermissionsManager.Current.PermissionArrayList;

            ltlScript.Text = NavigationTreeItem.GetNavigationBarScript();

            ArrayList menuExternalArrayList = ProductFileUtils.GetMenuTopTabArrayList(this.productID);

            this.rptTopMenuExternal.DataSource = menuExternalArrayList;
            this.rptTopMenuExternal.ItemDataBound += rptTopMenuExternal_ItemDataBound;
            this.rptTopMenuExternal.DataBind();

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

                foreach (string productID in productIDList)
                {
                    if (StringUtils.EqualsIgnoreCase(productID, ProductManager.Apps.ProductID))
                    {
                        builder.AppendFormat(@"<li><a href=""{0}"" target=""_self"">SiteServer 管理平台</a></li>", PageUtils.GetCMSUrl("background_initialization.aspx"));
                    }
                    else
                    {
                        if (StringUtils.EqualsIgnoreCase(productID, this.productID))
                        {
                            builder.AppendFormat(@"<li><a href=""productMain.aspx?productID={0}"" target=""_self""><i class=""icon-asterisk""></i> {1}</a></li>", productID, ProductFileUtils.GetModuleFileInfo(productID).FullName);
                        }
                        else
                        {
                            builder.AppendFormat(@"<li><a href=""productMain.aspx?productID={0}"" target=""_self"">{1}</a></li>", productID, ProductFileUtils.GetModuleFileInfo(productID).FullName);
                        }
                    }
                }

                builder.Append(@"
    </ul>
</li>
");
                this.ltlTopApps.Text = builder.ToString();
            }

            BaiRongDataProvider.AdministratorDAO.UpdateLastActivityDateAndProductID(AdminManager.Current.UserName, this.productID);

            string rightUrl = PageUtils.GetPlatformUrl("framework_right.aspx?productID=" + this.productID);
            ltlRight.Text = string.Format(@"<iframe frameborder=""0"" id=""right"" name=""right"" src=""{0}""></iframe>", rightUrl);
        }

        void rptTopMenuExternal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Literal ltlMenu = e.Item.FindControl("ltlMenu") as Literal;
            int index = e.Item.ItemIndex;
            Tab tab = e.Item.DataItem as Tab;

            string loadingUrl = this.GetRedirectUrl(this.productID, tab.ID);

            bool active = false;
            if (!string.IsNullOrEmpty(this.menuID))
            {
                if (StringUtils.EqualsIgnoreCase(tab.ID, this.menuID))
                {
                    active = true;
                }
            }
            else
            {
                active = index == 0;
            }


            ltlMenu.Text = this.GetTopMenuHtml(loadingUrl, active, tab.Text);
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

        private string GetRedirectUrl(string productID, string menuID)
        {
            if (string.IsNullOrEmpty(menuID))
            {
                return string.Format("productMain.aspx?productID={0}", productID);
            }
            else
            {
                return string.Format("productMain.aspx?productID={0}&menuID={1}", productID, menuID);
            }
        }
    }
}
