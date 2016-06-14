using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;



namespace BaiRong.BackgroundPages.Modal
{
	public class ConfigurationMenuAdd : BackgroundBasePage
	{
        public TextBox tbID;
        public TextBox tbText;
        public TextBox tbHref;
        public TextBox tbTarget;
        public CheckBoxList cblPermissions;

        private string appID;
        private string menuID;
        private int level;
        private string menuText;
        private string parentMenuText;
        private bool isEdit;

        public static string GetOpenWindowStringToAdd(string appID, string menuID, int level, string parentMenuText)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("AppID", appID);
            arguments.Add("MenuID", menuID);
            arguments.Add("Level", level.ToString());
            arguments.Add("ParentMenuText", parentMenuText);
            arguments.Add("IsEdit", false.ToString());
            return PageUtilityPF.GetOpenWindowString("添加菜单", "modal_configurationMenuAdd.aspx", arguments, 500, 500);
        }

        public static string GetOpenWindowStringToEdit(string appID, string menuID, int level, string menuText)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("AppID", appID);
            arguments.Add("MenuID", menuID);
            arguments.Add("Level", level.ToString());
            arguments.Add("MenuText", menuText);
            arguments.Add("IsEdit", true.ToString());
            return PageUtilityPF.GetOpenWindowString("修改菜单", "modal_configurationMenuAdd.aspx", arguments, 500, 500);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.appID = base.GetQueryString("AppID");
            this.menuID = base.GetQueryString("menuID");
            this.level = TranslateUtils.ToInt(base.GetQueryString("Level"));
            this.menuText = base.GetQueryString("MenuText");
            this.parentMenuText = base.GetQueryString("ParentMenuText");
            this.isEdit = base.GetBoolQueryString("IsEdit");

            if (!IsPostBack)
            {
                ArrayList permissions = PermissionConfigManager.GetGeneralPermissionsOfApp(this.appID);
                foreach (PermissionConfig config in permissions)
                {
                    ListItem listItem = new ListItem(config.Text, config.Name);
                    this.cblPermissions.Items.Add(listItem);
                }

                if (this.isEdit)
                {
                    ArrayList tabArrayList = this.GetTabArrayListByLevel();
                    foreach (Tab tab in tabArrayList)
                    {
                        if (tab.Text == this.menuText)
                        {
                            this.tbID.Text = tab.ID;
                            this.tbText.Text = tab.Text;
                            this.tbHref.Text = tab.Href;
                            this.tbTarget.Text = tab.Target;
                            ControlUtils.SelectListItems(this.cblPermissions, TranslateUtils.StringCollectionToArrayList(tab.Permissions));
                            break;
                        }
                    }
                }
            }
        }

        private ArrayList GetTabArrayListByLevel()
        {
            ArrayList tabArrayList = new ArrayList();
            ArrayList topArrayList = ProductFileUtils.GetMenuTopTabArrayList(this.appID);
            foreach (Tab topTab in topArrayList)
            {
                if (this.level == 1)
                {
                    tabArrayList.Add(topTab);
                }
                else if (this.level == 2)
                {
                    ArrayList groupArrayList = ProductFileUtils.GetMenuGroupTabArrayListOfApp(appID, topTab.ID);
                    tabArrayList.AddRange(groupArrayList);
                }
                else if (this.level == 3)
                {
                    ArrayList groupArrayList = ProductFileUtils.GetMenuGroupTabArrayListOfApp(appID, topTab.ID);
                    if (groupArrayList.Count > 0)
                    {
                        foreach (Tab groupTab in groupArrayList)
                        {
                            if (groupTab.Children != null)
                            {
                                foreach (Tab tab in groupTab.Children)
                                {
                                    tabArrayList.Add(tab);
                                }
                            }
                        }
                    }
                }
            }
            return tabArrayList;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (this.level > 1)
                {
                    if (!this.isEdit)
                    {
                        Tab tab = new Tab();
                        tab.ID = this.tbID.Text;
                        tab.Text = this.tbText.Text;
                        tab.Href = this.tbHref.Text;
                        tab.Target = this.tbTarget.Text;
                        tab.Permissions = ControlUtils.GetSelectedListControlValueCollection(this.cblPermissions);

                        ArrayList levelTabArrayList = this.GetTabArrayListByLevel();
                        foreach (Tab theTab in levelTabArrayList)
                        {
                            if (theTab.ID == tab.ID)
                            {
                                base.FailMessage("菜单添加失败，菜单标识已存在！");
                                return;
                            }
                            else if (theTab.Text == tab.Text)
                            {
                                base.FailMessage("菜单添加失败，菜单名称已存在！");
                                return;
                            }
                        }

                        ArrayList tabArrayList = ProductFileUtils.GetMenuGroupTabArrayListOfApp(appID, this.menuID);
                        if (this.level == 2)
                        {
                            tabArrayList.Add(tab);
                        }
                        else if (this.level == 3)
                        {
                            foreach (Tab theTab in tabArrayList)
                            {
                                if (this.parentMenuText == theTab.Text)
                                {
                                    if (theTab.Children != null)
                                    {
                                        Tab[] children = new Tab[theTab.Children.Length + 1];
                                        for (int i = 0; i < theTab.Children.Length; i++)
                                        {
                                            children[i] = theTab.Children[i];
                                        }
                                        children[theTab.Children.Length] = tab;
                                        theTab.Children = children;
                                    }
                                    else
                                    {
                                        Tab[] children = new Tab[] { tab };
                                        theTab.Children = children;
                                    }
                                }
                            }
                        }

                        ProductFileUtils.SaveMenuTabArrayList(this.appID, this.menuID, tabArrayList);
                    }
                    else
                    {
                        ArrayList tabArrayList = ProductFileUtils.GetMenuGroupTabArrayListOfApp(appID, this.menuID);
                        if (this.level == 2)
                        {
                            foreach (Tab tab in tabArrayList)
                            {
                                if (this.menuText == tab.Text)
                                {
                                    tab.ID = this.tbID.Text;
                                    tab.Text = this.tbText.Text;
                                    tab.Href = this.tbHref.Text;
                                    tab.Target = this.tbTarget.Text;
                                    tab.Permissions = ControlUtils.GetSelectedListControlValueCollection(this.cblPermissions);
                                    break;
                                }
                            }
                        }
                        else if (this.level == 3)
                        {
                            foreach (Tab theTab in tabArrayList)
                            {
                                if (theTab.Children != null)
                                {
                                    foreach (Tab tab in theTab.Children)
                                    {
                                        if (this.menuText == tab.Text)
                                        {
                                            tab.ID = this.tbID.Text;
                                            tab.Text = this.tbText.Text;
                                            tab.Href = this.tbHref.Text;
                                            tab.Target = this.tbTarget.Text;
                                            tab.Permissions = ControlUtils.GetSelectedListControlValueCollection(this.cblPermissions);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        ProductFileUtils.SaveMenuTabArrayList(this.appID, this.menuID, tabArrayList);
                    }
                }
                else
                {
                    if (this.isEdit)
                    {
                        ArrayList topArrayList = ProductFileUtils.GetMenuTopTabArrayList(this.appID);
                        foreach (Tab tab in topArrayList)
                        {
                            if (tab.ID == this.menuID)
                            {
                                tab.Text = this.tbText.Text;
                                tab.Href = this.tbHref.Text;
                                tab.Target = this.tbTarget.Text;
                                tab.Permissions = ControlUtils.GetSelectedListControlValueCollection(this.cblPermissions);
                                break;
                            }
                        }
                        ProductFileUtils.SaveMenuTabArrayList(this.appID, this.menuID, topArrayList);
                    }
                }

                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
	}
}
