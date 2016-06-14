using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Web.UI;
using BaiRong.Core.Data.Provider;


namespace SiteServer.Project.Controls
{
    public class NavigationTree : TabDrivenTemplatedWebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            BuildNavigationTree(builder, base.GetTabs(), 0, true);
            writer.Write(builder);
        }

        /// <summary>
        /// Creates the markup for the current TabCollection
        /// </summary>
        /// <returns></returns>
        protected void BuildNavigationTree(StringBuilder builder, TabCollection tc, int parentsCount, bool isDisplay)
        {
            if (tc != null && tc.Tabs != null)
            {
                foreach (Tab parent in tc.Tabs)
                {
                    if (TabManager.IsValid(parent, this.PermissionArrayList))
                    {
                        string linkUrl = base.FormatLink(parent);
                        if (!StringUtils.IsNullorEmpty(linkUrl) && !StringUtils.EqualsIgnoreCase(linkUrl, PageUtils.UNCLICKED_URL))
                        {
                            linkUrl = PageUtils.GetLoadingUrl(linkUrl);
                        }
                        bool hasChildren = (parent.Children != null && parent.Children.Length > 0) ? true : false;
                        bool openWindow = hasChildren ? false : StringUtils.EndsWithIgnoreCase(parent.Href, "Main.aspx");

                        NavigationTreeItem item = NavigationTreeItem.CreateNavigationBarItem(isDisplay, parent.Selected, parentsCount, hasChildren, openWindow, parent.Text, linkUrl, parent.Target, parent.Enabled, parent.IconUrl);

                        builder.Append(item.GetTrHtml());
                        if (parent.Children != null && parent.Children.Length > 0)
                        {
                            TabCollection tc2 = NavigationTree.GetTabCollection(parent);
                            BuildNavigationTree(builder, tc2, parentsCount + 1, parent.Selected);
                        }
                    }
                }
            }
        }

        public static TabCollection GetTabCollection(Tab parent)
        {
            TabCollection tabCollection = null;

            if (StringUtils.EqualsIgnoreCase(parent.ID, "Projects"))
            {
                parent.Children = new Tab[0];
                Tab[] tabs = null;
                Dictionary<int, string> dictioanry = DataProvider.ProjectDAO.GetProjectIDWithNameDictionary(AdminManager.Current.UserName);

                if (dictioanry.Count > 0)
                {
                    tabs = new Tab[parent.Children.Length + dictioanry.Count];
                    int i = 0;
                    foreach (var value in dictioanry)
                    {
                        int projectID = value.Key;
                        string projectName = value.Value;

                        Tab tab = new Tab();
                        tab.Text = projectName;
                        tab.ID = "Projects" + "_" + projectID;

                        Tab tab1 = new Tab();
                        tab1.Text = "处理中的办件";
                        tab1.Href = "project/background_applyToWork.aspx?ProjectID=" + projectID;
                        tab1.KeepQueryString = true;
                        tab1.Target = "right";

                        Tab tab2 = new Tab();
                        tab2.Text = "所有办件";
                        tab2.Href = "project/background_applyToAll.aspx?ProjectID=" + projectID;
                        tab2.KeepQueryString = true;
                        tab2.Target = "right";

                        Tab tab3 = new Tab();
                        tab3.Text = "项目文档";
                        tab3.Href = "project/background_projectDocument.aspx?ProjectID=" + projectID;
                        tab3.KeepQueryString = true;
                        tab3.Target = "right";

                        Tab tab4 = new Tab();
                        tab4.Text = "新增办件";
                        tab4.Href = "project/background_applyAdd.aspx?ProjectID=" + projectID;
                        tab4.KeepQueryString = true;
                        tab4.Target = "right";

                        tab.Children = new Tab[] { tab1, tab2, tab3, tab4};

                        tabs[i++] = tab;
                    }

                    for (int j = 0; j < parent.Children.Length; j++)
                    {
                        tabs[j + i] = parent.Children[j];
                    }

                    parent.Children = tabs;
                }
                else
                {
                    tabs = parent.Children;
                }

                tabCollection = new TabCollection(tabs);
            }
            else if (StringUtils.EqualsIgnoreCase(parent.ID, "Documents"))
            {
                parent.Children = new Tab[0];
                Tab[] tabs = null;
                ArrayList typeInfoArrayList = null;
                try
                {
                    typeInfoArrayList = DataProvider.DocTypeDAO.GetTypeInfoArrayList(0);
                }
                catch { }
                if (typeInfoArrayList != null && typeInfoArrayList.Count > 0)
                {
                    tabs = new Tab[parent.Children.Length + typeInfoArrayList.Count];
                    int i = 0;
                    foreach (DocTypeInfo typeInfo in typeInfoArrayList)
                    {
                        Tab tab = new Tab();
                        tab.Text = typeInfo.TypeName;
                        tab.ID = "Documents" + "_" + typeInfo.TypeID;

                        ArrayList subTypeInfoArrayList = DataProvider.DocTypeDAO.GetTypeInfoArrayList(typeInfo.TypeID);
                        tab.Children = new Tab[subTypeInfoArrayList.Count];

                        int x = 0;
                        foreach (DocTypeInfo subTypeInfo in subTypeInfoArrayList)
                        {
                            Tab subTab = new Tab();
                            subTab.Text = subTypeInfo.TypeName;
                            subTab.Href = "project/background_document.aspx?TypeID=" + subTypeInfo.TypeID;
                            subTab.KeepQueryString = true;
                            subTab.Target = "right";
                            tab.Children[x++] = subTab;
                        }

                        tabs[i++] = tab;
                    }

                    for (int j = 0; j < parent.Children.Length; j++)
                    {
                        tabs[j + i] = parent.Children[j];
                    }

                    parent.Children = tabs;
                }
                else
                {
                    tabs = parent.Children;
                }

                tabCollection = new TabCollection(tabs);
            }
            else
            {
                tabCollection = new TabCollection(parent.Children);
            }
            return tabCollection;
        }

        private string _selected = null;
        public override string Selected
        {
            get
            {
                if (_selected == null)
                    _selected = Context.Items["ControlPanelSelectedNavItem"] as string;

                return _selected;
            }
            set
            {
                _selected = value;
            }
        }
    }
}
