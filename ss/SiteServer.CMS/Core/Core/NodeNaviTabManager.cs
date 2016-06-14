using System.Collections;
using System.Web;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Collections.Specialized;

using BaiRong.Model;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CMS.Core
{
    public class NodeNaviTabManager
    {
        public const string SiteManagement = "SiteManagement";

        //public const string Content = "Content";
        //public const string InputManagement = "InputManagement";
        //public const string ResumeManagement = "ResumeManagement";
        //public const string GovInteract = "GovInteract";

        public static bool IsValid(Tab tab, ArrayList permissionArrayList)
        {
            if (StringUtils.StartsWithIgnoreCase(tab.ID, AppManager.CMS.LeftMenu.Function.ID_Input))
            {
                if (ProductPermissionsManager.Current.IsSystemAdministrator)
                {
                    return true;
                }
                else
                {
                    return IsValid(tab, permissionArrayList);
                }
            }
            else
            {
                return IsValid(tab, permissionArrayList);
            }
        }

        private static bool IsValid(Tab tab, IList permissionList)
        {
            if (tab.HasPermissions)
            {
                if (permissionList != null && permissionList.Count > 0)
                {
                    string[] tabPermissions = tab.Permissions.Split(',');
                    foreach (string tabPermission in tabPermissions)
                    {
                        if (permissionList.Contains(tabPermission))
                            return true;
                    }
                }

                //ITab valid, but invalid role set
                return false;
            }

            //ITab valid, but no roles
            return true;
        }

        public static TabCollection GetTabCollection(Tab parent, int publishmentSystemID)
        {
            TabCollection tabCollection = null;
            /***
            ****   by 20151028 sofuny 取消表单显示在菜单内 
           
            if (StringUtils.EqualsIgnoreCase(parent.ID, AppManager.CMS.LeftMenu.Function.ID_Input))
            {
                Tab[] tabs = null;
                ArrayList inputNameArrayList = null;
                try
                {
                    inputNameArrayList = DataProvider.InputDAO.GetInputNameArrayList(publishmentSystemID);
                }
                catch { }
                if (inputNameArrayList != null && inputNameArrayList.Count > 0)
                {
                    tabs = new Tab[parent.Children.Length + inputNameArrayList.Count];
                    int i = 0;
                    foreach (string inputName in inputNameArrayList)
                    {
                        Tab tab = new Tab();
                        tab.Text = inputName;
                        tab.ID = AppManager.CMS.LeftMenu.Function.ID_Input + "_" + inputName;
                        //tab.Href = PageUtils.GetCMSUrl("background_inputContent.aspx?InputName=" + inputName);
                        tab.Href = "cms/background_inputContent.aspx?InputName=" + inputName;
                        tab.KeepQueryString = true;
                        tab.Target = "right";
                        tab.Permissions = AppManager.CMS.Permission.WebSite.InputContentView + "," + AppManager.CMS.Permission.WebSite.InputContentEdit + "_" + inputName;
                        tabs[i++] = tab;
                    }

                    for (int j = 0; j < parent.Children.Length; j++)
                    {
                        tabs[j + i] = parent.Children[j];
                    }
                }
                else
                {
                    tabs = parent.Children;
                }

                tabCollection = new TabCollection(tabs);
            }
            else 
     **/
            if (StringUtils.EqualsIgnoreCase(parent.ID, AppManager.CMS.LeftMenu.Function.ID_Resume))
            {
                Tab[] tabs = null;
                ArrayList styleNameArrayList = null;
                try
                {
                    styleNameArrayList = DataProvider.TagStyleDAO.GetStyleNameArrayList(publishmentSystemID, Constants.STLElementName.StlResume);
                }
                catch { }
                if (styleNameArrayList != null && styleNameArrayList.Count > 0)
                {
                    tabs = new Tab[parent.Children.Length + styleNameArrayList.Count];
                    int i = 0;
                    foreach (string styleName in styleNameArrayList)
                    {
                        Tab tab = new Tab();
                        tab.Text = styleName;
                        tab.ID = AppManager.CMS.LeftMenu.Function.ID_Resume + "_" + styleName;
                        tab.Href = PageUtils.GetCMSUrl("background_resumeContent.aspx?StyleName=" + styleName);
                        tab.KeepQueryString = true;
                        tab.Target = "right";
                        tabs[i++] = tab;
                    }

                    for (int j = 0; j < parent.Children.Length; j++)
                    {
                        tabs[j + i] = parent.Children[j];
                    }
                }
                else
                {
                    tabs = parent.Children;
                }

                tabCollection = new TabCollection(tabs);
            }
            else if (StringUtils.EqualsIgnoreCase(parent.ID, AppManager.CMS.LeftMenu.ID_GovInteract))
            {
                Tab[] tabs = null;
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                ArrayList nodeInfoArrayList = GovInteractManager.GetNodeInfoArrayList(publishmentSystemInfo);
                if (nodeInfoArrayList.Count > 0)
                {
                    ArrayList govInteractPermissionArrayListOfPublishmentSystemID = ProductPermissionsManager.Current.GovInteractPermissionSortedList[publishmentSystemID] as ArrayList;
                    ArrayList govInteractPermissionArrayList = govInteractPermissionArrayListOfPublishmentSystemID;
                    ArrayList tabArrayList = new ArrayList();
                    foreach (NodeInfo nodeInfo in nodeInfoArrayList)
                    {
                        if (govInteractPermissionArrayListOfPublishmentSystemID == null || govInteractPermissionArrayListOfPublishmentSystemID.Count == 0)
                        {
                            govInteractPermissionArrayList = ProductPermissionsManager.Current.GovInteractPermissionSortedList[nodeInfo.NodeID] as ArrayList;
                        }

                        if (govInteractPermissionArrayList != null && govInteractPermissionArrayList.Count > 0)
                        {
                            Tab tab = new Tab();
                            tab.Text = nodeInfo.NodeName;
                            tab.ID = AppManager.CMS.LeftMenu.ID_GovInteract + "_" + nodeInfo.NodeID;

                            ArrayList childArrayList = new ArrayList();

                            if (govInteractPermissionArrayList.Contains(AppManager.CMS.Permission.GovInteract.GovInteractAccept))
                            {
                                Tab child = new Tab();
                                child.Text = "待受理办件";
                                child.Href = PageUtils.GetWCMUrl("background_govInteractListAccept.aspx?NodeID=" + nodeInfo.NodeID);
                                child.KeepQueryString = true;
                                child.Target = "right";
                                childArrayList.Add(child);
                            }
                            if (govInteractPermissionArrayList.Contains(AppManager.CMS.Permission.GovInteract.GovInteractReply))
                            {
                                Tab child = new Tab();
                                child.Text = "待办理办件";
                                child.Href = PageUtils.GetWCMUrl("background_govInteractListReply.aspx?NodeID=" + nodeInfo.NodeID);
                                child.KeepQueryString = true;
                                child.Target = "right";
                                childArrayList.Add(child);
                            }
                            if (govInteractPermissionArrayList.Contains(AppManager.CMS.Permission.GovInteract.GovInteractCheck))
                            {
                                Tab child = new Tab();
                                child.Text = "待审核办件";
                                child.Href = PageUtils.GetWCMUrl("background_govInteractListCheck.aspx?NodeID=" + nodeInfo.NodeID);
                                child.KeepQueryString = true;
                                child.Target = "right";
                                childArrayList.Add(child);
                            }
                            if (govInteractPermissionArrayList.Contains(AppManager.CMS.Permission.GovInteract.GovInteractView) || govInteractPermissionArrayList.Contains(AppManager.CMS.Permission.GovInteract.GovInteractDelete))
                            {
                                Tab child = new Tab();
                                child.Text = "所有办件";
                                child.Href = PageUtils.GetWCMUrl("background_govInteractListAll.aspx?NodeID=" + nodeInfo.NodeID);
                                child.KeepQueryString = true;
                                child.Target = "right";
                                childArrayList.Add(child);
                            }
                            if (govInteractPermissionArrayList.Contains(AppManager.CMS.Permission.GovInteract.GovInteractAdd))
                            {
                                Tab child = new Tab();
                                child.Text = "新增办件";
                                string redirectUrl = PageUtils.GetWCMUrl(string.Format("background_govInteractListAccept.aspx?PublishmentSystemID={0}&NodeID={1}", nodeInfo.PublishmentSystemID, nodeInfo.NodeID));
                                child.Href = WebUtils.GetContentAddAddUrl(publishmentSystemID, nodeInfo, redirectUrl);
                                child.KeepQueryString = false;
                                child.Target = "right";
                                childArrayList.Add(child);
                            }

                            tab.Children = new Tab[childArrayList.Count];
                            for (int i = 0; i < childArrayList.Count; i++)
                            {
                                tab.Children[i] = childArrayList[i] as Tab;
                            }

                            tabArrayList.Add(tab);
                        }
                    }

                    int k = 0;
                    tabs = new Tab[parent.Children.Length + tabArrayList.Count];
                    for (; k < tabArrayList.Count; k++)
                    {
                        tabs[k] = tabArrayList[k] as Tab;
                    }

                    for (int j = 0; j < parent.Children.Length; j++)
                    {
                        tabs[j + k] = parent.Children[j];
                    }
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
    }
}
