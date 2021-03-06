using System;
using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundRoleAddPublishmentSystemPermissions : BackgroundBasePage
    {
        public CheckBoxList WebsitePermissions;
        public CheckBoxList ChannelPermissions;
        public Literal NodeTree;

        public PlaceHolder WebsitePermissionsPlaceHolder;
        public PlaceHolder ChannelPermissionsPlaceHolder;

        private string GetNodeTreeHtml()
        {
            StringBuilder htmlBuilder = new StringBuilder();
            ArrayList systemPermissionsInfoArrayList = base.Session[BackgroundRoleAdd.SystemPermissionsInfoArrayListKey] as ArrayList;
            if (systemPermissionsInfoArrayList == null)
            {
                PageUtils.RedirectToErrorPage("超出时间范围，请重新进入！");
                return string.Empty;
            }
            ArrayList nodeIDArrayList = new ArrayList();
            foreach (SystemPermissionsInfo systemPermissionsInfo in systemPermissionsInfoArrayList)
            {
                nodeIDArrayList.AddRange(TranslateUtils.StringCollectionToIntArrayList(systemPermissionsInfo.NodeIDCollection));
            }

            string treeDirectoryUrl = PageUtils.GetIconUrl("tree");

            htmlBuilder.Append("<span id='ChannelSelectControl'>");
            ArrayList theNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
            bool[] IsLastNodeArray = new bool[theNodeIDArrayList.Count];
            foreach (int theNodeID in theNodeIDArrayList)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, theNodeID);
                int NodeID = nodeInfo.NodeID;
                string NodeName = nodeInfo.NodeName;
                int ParentsCount = nodeInfo.ParentsCount;
                int ChildrenCount = nodeInfo.ChildrenCount;
                bool IsLastNode = nodeInfo.IsLastNode;
                int ContentNum = nodeInfo.ContentNum;
                htmlBuilder.Append(GetTitle(nodeInfo, treeDirectoryUrl, IsLastNodeArray, nodeIDArrayList));
                htmlBuilder.Append("<br/>");
            }
            htmlBuilder.Append("</span>");
            return htmlBuilder.ToString();
        }

        private string GetTitle(NodeInfo nodeInfo, string treeDirectoryUrl, bool[] IsLastNodeArray, IList nodeIDArrayList)
        {
            StringBuilder itemBuilder = new StringBuilder();
            if (nodeInfo.NodeID == base.PublishmentSystemID)
            {
                nodeInfo.IsLastNode = true;
            }
            if (nodeInfo.IsLastNode == false)
            {
                IsLastNodeArray[nodeInfo.ParentsCount] = false;
            }
            else
            {
                IsLastNodeArray[nodeInfo.ParentsCount] = true;
            }
            for (int i = 0; i < nodeInfo.ParentsCount; i++)
            {
                if (IsLastNodeArray[i])
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_empty.gif\"/>", treeDirectoryUrl);
                }
                else
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_line.gif\"/>", treeDirectoryUrl);
                }
            }
            if (nodeInfo.IsLastNode)
            {
                if (nodeInfo.ChildrenCount > 0)
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_plusbottom.gif\"/>", treeDirectoryUrl);
                }
                else
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_minusbottom.gif\"/>", treeDirectoryUrl);
                }
            }
            else
            {
                if (nodeInfo.ChildrenCount > 0)
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_plusmiddle.gif\"/>", treeDirectoryUrl);
                }
                else
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_minusmiddle.gif\"/>", treeDirectoryUrl);
                }
            }

            string check = "";
            if (nodeIDArrayList.Contains(nodeInfo.NodeID))
            {
                check = "checked";
            }

            string disabled = "";
            if (!base.IsOwningNodeID(nodeInfo.NodeID))
            {
                disabled = "disabled";
                check = "";
            }

            itemBuilder.AppendFormat(@"<label class=""checkbox inline""><input type=""checkbox"" name=""NodeIDCollection"" value=""{0}"" {1} {2}/> {3} {4}</label>", nodeInfo.NodeID, check, disabled, nodeInfo.NodeName, string.Format("&nbsp;<span style=\"font-size:8pt;font-family:arial\" class=\"gray\">({0})</span>", nodeInfo.ContentNum));

            return itemBuilder.ToString();
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.NodeTree.Text = GetNodeTreeHtml();

            if (!base.IsPostBack)
            {
                AdminManager.VerifyPermissions(AppManager.Platform.Permission.Platform_Administrator);

                if (PermissionsManager.Current.IsSystemAdministrator)
                {
                    ArrayList channelPermissions = PermissionConfigManager.GetChannelPermissionsOfApp(AppManager.CMS.AppID);
                    foreach (PermissionConfig permission in channelPermissions)
                    {
                        if (permission.Name == AppManager.CMS.Permission.Channel.ContentCheckLevel1)
                        {
                            if (base.PublishmentSystemInfo.IsCheckContentUseLevel)
                            {
                                if (base.PublishmentSystemInfo.CheckContentLevel < 1)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (permission.Name == AppManager.CMS.Permission.Channel.ContentCheckLevel2)
                        {
                            if (base.PublishmentSystemInfo.IsCheckContentUseLevel)
                            {
                                if (base.PublishmentSystemInfo.CheckContentLevel < 2)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (permission.Name == AppManager.CMS.Permission.Channel.ContentCheckLevel3)
                        {
                            if (base.PublishmentSystemInfo.IsCheckContentUseLevel)
                            {
                                if (base.PublishmentSystemInfo.CheckContentLevel < 3)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (permission.Name == AppManager.CMS.Permission.Channel.ContentCheckLevel4)
                        {
                            if (base.PublishmentSystemInfo.IsCheckContentUseLevel)
                            {
                                if (base.PublishmentSystemInfo.CheckContentLevel < 4)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (permission.Name == AppManager.CMS.Permission.Channel.ContentCheckLevel5)
                        {
                            if (base.PublishmentSystemInfo.IsCheckContentUseLevel)
                            {
                                if (base.PublishmentSystemInfo.CheckContentLevel < 5)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        ListItem listItem = new ListItem(permission.Text, permission.Name);
                        this.ChannelPermissions.Items.Add(listItem);
                    }
                }
                else
                {
                    ChannelPermissionsPlaceHolder.Visible = false;
                    if (ProductPermissionsManager.Current.ChannelPermissionSortedList[base.PublishmentSystemID] != null)
                    {
                        ArrayList channelPermissions = (ArrayList)ProductPermissionsManager.Current.ChannelPermissionSortedList[base.PublishmentSystemID];
                        foreach (string channelPermission in channelPermissions)
                        {
                            foreach (PermissionConfig permission in PermissionConfigManager.Instance.ChannelPermissions)
                            {
                                if (permission.Name == channelPermission)
                                {
                                    if (channelPermission == AppManager.CMS.Permission.Channel.ContentCheck)
                                    {
                                        if (base.PublishmentSystemInfo.IsCheckContentUseLevel) continue;
                                    }
                                    else if (channelPermission == AppManager.CMS.Permission.Channel.ContentCheckLevel1)
                                    {
                                        if (base.PublishmentSystemInfo.IsCheckContentUseLevel == false || base.PublishmentSystemInfo.CheckContentLevel < 1) continue;
                                    }
                                    else if (channelPermission == AppManager.CMS.Permission.Channel.ContentCheckLevel2)
                                    {
                                        if (base.PublishmentSystemInfo.IsCheckContentUseLevel == false || base.PublishmentSystemInfo.CheckContentLevel < 2) continue;
                                    }
                                    else if (channelPermission == AppManager.CMS.Permission.Channel.ContentCheckLevel3)
                                    {
                                        if (base.PublishmentSystemInfo.IsCheckContentUseLevel == false || base.PublishmentSystemInfo.CheckContentLevel < 3) continue;
                                    }
                                    else if (channelPermission == AppManager.CMS.Permission.Channel.ContentCheckLevel4)
                                    {
                                        if (base.PublishmentSystemInfo.IsCheckContentUseLevel == false || base.PublishmentSystemInfo.CheckContentLevel < 4) continue;
                                    }
                                    else if (channelPermission == AppManager.CMS.Permission.Channel.ContentCheckLevel5)
                                    {
                                        if (base.PublishmentSystemInfo.IsCheckContentUseLevel == false || base.PublishmentSystemInfo.CheckContentLevel < 5) continue;
                                    }

                                    ChannelPermissionsPlaceHolder.Visible = true;
                                    ListItem listItem = new ListItem(permission.Text, permission.Name);
                                    this.ChannelPermissions.Items.Add(listItem);
                                }
                            }
                        }
                    }
                }

                if (PermissionsManager.Current.IsSystemAdministrator)
                {
                    if (base.PublishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.BBS || base.PublishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.CRM)
                    {
                        string appID = EPublishmentSystemTypeUtils.GetAppID(base.PublishmentSystemInfo.PublishmentSystemType);
                        ArrayList websitePermissions = PermissionConfigManager.GetWebsitePermissionsOfApp(appID);
                        foreach (PermissionConfig permission in websitePermissions)
                        {
                            ListItem listItem = new ListItem(permission.Text, permission.Name);
                            this.WebsitePermissions.Items.Add(listItem);
                        }

                        this.ChannelPermissionsPlaceHolder.Visible = false;
                    }
                    else if (base.PublishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.UserCenter)
                    {
                        ArrayList websitePermissions = PermissionConfigManager.GetWebsitePermissionsOfApp(AppManager.Platform.AppID);
                        foreach (PermissionConfig permission in websitePermissions)
                        {
                            ListItem listItem = new ListItem(permission.Text, permission.Name);
                            this.WebsitePermissions.Items.Add(listItem);
                        }

                        #region by 20151205 sofuny 因改为表单分类权限，将表单权限取消
                        //try
                        //{
                        //    ArrayList inputNameArrayList = DataProvider.InputDAO.GetInputNameArrayList(base.PublishmentSystemID);
                        //    foreach (string inputName in inputNameArrayList)
                        //    {
                        //        ListItem listItem = new ListItem(inputName, AppManager.CMS.Permission.WebSite.InputContentEdit + "_" + inputName);
                        //        this.WebsitePermissions.Items.Add(listItem);
                        //    }
                        //}
                        //catch { }
                        #endregion
                    }
                    else
                    {

                        ArrayList websitePermissions = PermissionConfigManager.GetWebsitePermissionsOfApp(AppManager.CMS.AppID);
                        foreach (PermissionConfig permission in websitePermissions)
                        {
                            ListItem listItem = new ListItem(permission.Text, permission.Name);
                            this.WebsitePermissions.Items.Add(listItem);
                        }

                        string appID = EPublishmentSystemTypeUtils.GetAppID(base.PublishmentSystemInfo.PublishmentSystemType);
                        if (!string.IsNullOrEmpty(appID) && !EPublishmentSystemTypeUtils.Equals(base.PublishmentSystemInfo.PublishmentSystemType, AppManager.CMS.AppID))
                        {
                            ArrayList websitePermissions2 = PermissionConfigManager.GetWebsitePermissionsOfApp(appID);
                            foreach (PermissionConfig permission in websitePermissions2)
                            {
                                ListItem listItem = new ListItem(permission.Text, permission.Name);
                                this.WebsitePermissions.Items.Add(listItem);
                            }
                        }

                        #region by 20151205 sofuny 因改为表单分类权限，将表单权限取消
                        //try
                        //{
                        //    ArrayList inputNameArrayList = DataProvider.InputDAO.GetInputNameArrayList(base.PublishmentSystemID);
                        //    foreach (string inputName in inputNameArrayList)
                        //    {
                        //        ListItem listItem = new ListItem(inputName, AppManager.CMS.Permission.WebSite.InputContentEdit + "_" + inputName);
                        //        this.WebsitePermissions.Items.Add(listItem);
                        //    }
                        //}
                        //catch { }
                        #endregion
                    }
                }
                else
                {
                    WebsitePermissionsPlaceHolder.Visible = false;
                    if (ProductPermissionsManager.Current.WebsitePermissionSortedList[base.PublishmentSystemID] != null)
                    {
                        ArrayList websitePermissionArrayList = (ArrayList)ProductPermissionsManager.Current.WebsitePermissionSortedList[base.PublishmentSystemID];
                        foreach (string websitePermission in websitePermissionArrayList)
                        {
                            foreach (PermissionConfig permission in PermissionConfigManager.Instance.WebsitePermissions)
                            {
                                if (permission.Name == websitePermission)
                                {
                                    WebsitePermissionsPlaceHolder.Visible = true;
                                    ListItem listItem = new ListItem(permission.Text, permission.Name);
                                    this.WebsitePermissions.Items.Add(listItem);
                                }
                            }
                        }
                    }
                }

                ArrayList systemPermissionsInfoArrayList = (ArrayList)base.Session[BackgroundRoleAdd.SystemPermissionsInfoArrayListKey];
                if (systemPermissionsInfoArrayList != null)
                {
                    SystemPermissionsInfo systemPermissionsInfo = null;
                    foreach (SystemPermissionsInfo publishmentSystemPermissionsInfo in systemPermissionsInfoArrayList)
                    {
                        if (publishmentSystemPermissionsInfo.PublishmentSystemID == base.PublishmentSystemID)
                        {
                            systemPermissionsInfo = publishmentSystemPermissionsInfo;
                            break;
                        }
                    }
                    if (systemPermissionsInfo != null)
                    {
                        foreach (ListItem item in ChannelPermissions.Items)
                        {
                            if (CompareUtils.Contains(systemPermissionsInfo.ChannelPermissions, item.Value))
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                        }
                        foreach (ListItem item in WebsitePermissions.Items)
                        {
                            if (CompareUtils.Contains(systemPermissionsInfo.WebsitePermissions, item.Value))
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                        }
                    }
                }
            }


        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ArrayList systemPermissionsInfoArrayList = (ArrayList)base.Session[BackgroundRoleAdd.SystemPermissionsInfoArrayListKey];
                if (systemPermissionsInfoArrayList != null)
                {
                    ArrayList arraylist = new ArrayList();
                    foreach (SystemPermissionsInfo systemPermissionsInfo in systemPermissionsInfoArrayList)
                    {
                        if (systemPermissionsInfo.PublishmentSystemID == base.PublishmentSystemID)
                        {
                            continue;
                        }
                        arraylist.Add(systemPermissionsInfo);
                    }

                    ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToArrayList(Request.Form["NodeIDCollection"]);
                    if ((nodeIDArrayList.Count > 0 && ChannelPermissions.SelectedItem != null) || WebsitePermissions.SelectedItem != null)
                    {
                        SystemPermissionsInfo systemPermissionsInfo = new SystemPermissionsInfo();

                        systemPermissionsInfo.PublishmentSystemID = base.PublishmentSystemID;
                        systemPermissionsInfo.NodeIDCollection = TranslateUtils.ObjectCollectionToString(nodeIDArrayList);
                        systemPermissionsInfo.ChannelPermissions = ControlUtils.SelectedItemsValueToStringCollection(ChannelPermissions.Items);
                        systemPermissionsInfo.WebsitePermissions = ControlUtils.SelectedItemsValueToStringCollection(WebsitePermissions.Items);


                        #region by 20151205 sofuny 因改为表单分类权限，将表单权限取消
                        //if (systemPermissionsInfo.WebsitePermissions.Contains(AppManager.CMS.Permission.WebSite.InputContentEdit + "_"))
                        //if (StringUtils.Contains(systemPermissionsInfo.WebsitePermissions, AppManager.CMS.Permission.WebSite.InputContentEdit + "_"))
                        //{

                        //    systemPermissionsInfo.WebsitePermissions += "," + AppManager.CMS.Permission.WebSite.InputContentEdit;
                        //}
                        #endregion

                        arraylist.Add(systemPermissionsInfo);
                    }

                    base.Session[BackgroundRoleAdd.SystemPermissionsInfoArrayListKey] = arraylist;
                }
                if (base.GetQueryString("RoleName") == null)
                {
                    PageUtils.Redirect(PageUtils.GetCMSUrl("background_roleAdd.aspx?Return=True"));
                }
                else
                {
                    string roleName = base.GetQueryString("RoleName");
                    PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_roleAdd.aspx?Return=True&RoleName={0}", roleName)));
                }
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            if (base.GetQueryString("RoleName") == null)
            {
                PageUtils.Redirect(PageUtils.GetCMSUrl("background_roleAdd.aspx?Return=True"));
            }
            else
            {
                string roleName = base.GetQueryString("RoleName");
                PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_roleAdd.aspx?Return=True&RoleName={0}", roleName)));
            }
        }

    }
}
