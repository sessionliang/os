using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Configuration;


using BaiRong.Model;

namespace BaiRong.BackgroundPages
{
    public class BackgroundConfigurationMenu : BackgroundBasePage
	{
        public Repeater rptModules;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null && base.GetQueryString("ModuleID") != null && base.GetQueryString("MenuID") != null && base.GetQueryString("Level") != null && base.GetQueryString("MenuText") != null)
            {
                string moduleID = base.GetQueryString("ModuleID");
                string menuID = base.GetQueryString("MenuID");
                int level = base.GetIntQueryString("Level");
                string menuText = base.GetQueryString("MenuText");
                try
                {
                    ArrayList tabArrayList = new ArrayList();
                    ArrayList tabArrayListAll = ProductFileUtils.GetMenuGroupTabArrayListOfApp(moduleID, menuID);
                    if (level == 2)
                    {
                        foreach (Tab tab in tabArrayListAll)
                        {
                            if (tab.Text != menuText)
                            {
                                tabArrayList.Add(tab);
                            }
                        }
                    }
                    else if (level == 3)
                    {
                        foreach (Tab theTab in tabArrayListAll)
                        {
                            if (theTab.Children != null)
                            {
                                bool isSelectTab = false;
                                foreach (Tab tab in theTab.Children)
                                {
                                    if (menuText == tab.Text)
                                    {
                                        isSelectTab = true;
                                    }
                                }
                                if (isSelectTab)
                                {
                                    Tab[] children = null;
                                    if (theTab.Children.Length > 1)
                                    {
                                        children = new Tab[theTab.Children.Length - 1];
                                        int i = 0;
                                        foreach (Tab tab in theTab.Children)
                                        {
                                            if (menuText != tab.Text)
                                            {
                                                children[i++] = tab;
                                            }
                                        }
                                    }
                                    theTab.Children = children;
                                }
                            }
                            tabArrayList.Add(theTab);
                        }
                    }

                    ProductFileUtils.SaveMenuTabArrayList(moduleID, menuID, tabArrayList);

                    base.SuccessMessage("成功删除菜单");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除菜单失败，{0}", ex.Message));
                }
            }

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Configuration, AppManager.Platform.LeftMenu.Configuration.ID_ConfigurationMenu, "菜单设置", AppManager.Platform.Permission.Platform_Configuration);

                this.rptModules.DataSource = AppManager.GetAppIDList(true);
                this.rptModules.ItemDataBound += new RepeaterItemEventHandler(rptModules_ItemDataBound);
                this.rptModules.DataBind();
			}
		}

        private string appID;
        private void rptModules_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                this.appID = (string)e.Item.DataItem;
                Literal ltlModuleName = e.Item.FindControl("ltlModuleName") as Literal;
                Literal ltlTips = e.Item.FindControl("ltlTips") as Literal;
                Repeater rptMenues = e.Item.FindControl("rptMenues") as Repeater;

                ArrayList tabArrayList = new ArrayList();

                ArrayList groupArrayList = ProductFileUtils.GetMenuGroupTabArrayListOfApp(this.appID, "Management");
                if (groupArrayList.Count > 0)
                {
                    foreach (Tab groupTab in groupArrayList)
                    {
                        groupTab.AddtionalString = "1";
                        tabArrayList.Add(groupTab);
                        if (groupTab.Children != null)
                        {
                            foreach (Tab tab in groupTab.Children)
                            {
                                tab.AddtionalString = "2";
                                tabArrayList.Add(tab);

                                if (tab.Children != null)
                                {
                                    foreach (Tab subTab in tab.Children)
                                    {
                                        subTab.AddtionalString = "3";
                                        tabArrayList.Add(subTab);


                                    }
                                }
                            }
                        }
                    }
                }

                if (tabArrayList.Count > 0)
                {
                    ModuleFileInfo fileInfo = ProductFileUtils.GetModuleFileInfoOfApp(this.appID);
                    ltlModuleName.Text = fileInfo.ModuleName;

                    ltlTips.Text = string.Format("{0}菜单存储在/SiteFiles/Products/Apps/{1}/Menu/文件夹中，可以手工修改菜单文件", fileInfo.FullName, this.appID);

                    rptMenues.DataSource = tabArrayList;
                    rptMenues.ItemDataBound += new RepeaterItemEventHandler(rptMenues_ItemDataBound);
                    rptMenues.DataBind();
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
        }

        void rptMenues_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Tab tab = e.Item.DataItem as Tab;
                string menuID = tab.AddtionalString.Split('_')[0];
                int level = TranslateUtils.ToInt(tab.AddtionalString);

                Literal ltlText = e.Item.FindControl("ltlText") as Literal;
                Literal ltlHref = e.Item.FindControl("ltlHref") as Literal;
                Literal ltlTarget = e.Item.FindControl("ltlTarget") as Literal;
                Literal ltlPermissions = e.Item.FindControl("ltlPermissions") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlAddUrl = e.Item.FindControl("ltlAddUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                string prefix = string.Empty;
                for (int i = 1; i < level; i++)
                {
                    prefix += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                if (level > 1)
                {
                    prefix += " - ";
                }
                ltlText.Text = prefix + tab.Text;
                if (level == 1)
                {
                    ltlText.Text += @"<br /><span class=""gray"">(一级菜单)</span>";
                }
                else if (level == 2)
                {
                    ltlText.Text += @"<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=""gray"">(二级菜单)</span>";
                }
                else if (level == 3)
                {
                    ltlText.Text += @"<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=""gray"">(三级菜单)</span>";
                }
                ltlHref.Text = tab.Href;
                ltlTarget.Text = tab.Target;
                ltlPermissions.Text = tab.Permissions;

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.ConfigurationMenuAdd.GetOpenWindowStringToEdit(this.appID, menuID, level, tab.Text));
                if (level < 3)
                {
                    ltlAddUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">添加下级</a>", Modal.ConfigurationMenuAdd.GetOpenWindowStringToAdd(this.appID, menuID, level + 1, tab.Text));
                }
                if (level > 1 || !StringUtils.EqualsIgnoreCase(menuID, "Top"))
                {
                    string urlMenu = PageUtils.GetPlatformUrl(string.Format("background_configurationMenu.aspx?Delete=True&ProductID={0}&MenuID={1}&Level={2}&MenuText={3}", this.appID, menuID, level, tab.Text));
                    ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除菜单“{1}”及其下级菜单，确认吗？');"">删除</a>", urlMenu, tab.Text);
                }
            }
        }
	}
}
