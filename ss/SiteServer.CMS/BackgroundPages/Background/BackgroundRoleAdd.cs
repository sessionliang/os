using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;


using BaiRong.Model;
using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundRoleAdd : BackgroundBasePage
    {
        public TextBox tbRoleName;
        public TextBox tbDescription;

        public Repeater rptProducts;

        public const string SystemPermissionsInfoArrayListKey = "SystemPermissionsInfoArrayListKey";

        private string theRoleName;
        private ArrayList generalPermissionArrayList;

        public string GetPublishmentSystemsHtml(ArrayList allPublishmentSystemIDArrayList, ArrayList managedPublishmentSystemIDArrayList)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<table width='100%' cellpadding='4' cellspacing='0' border='0'>");
            int count = 1;
            foreach (int publishmentSystemID in allPublishmentSystemIDArrayList)
            {
                PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                string imageName = "cantedit";
                if (managedPublishmentSystemIDArrayList.Contains(publishmentSystemID))
                {
                    imageName = "canedit";
                }

                string space = "";
                if (count % 4 == 0)
                {
                    space = "<TR>";
                }

                string withRoleName = "";
                if (base.GetQueryString("RoleName") != null)
                {
                    withRoleName = "&RoleName=" + base.GetQueryString("RoleName");
                }

                string pageUrl = PageUtils.GetCMSUrl(string.Format("background_roleAddPublishmentSystemPermissions.aspx?PublishmentSystemID={0}{1}", publishmentSystemID, withRoleName));
                string content = string.Format(@"
					<td height=20>
                        <img id='PublishmentSystemImage_{0}' align='absmiddle' border='0' src='../pic/{1}.gif'/>
					    <a href='{2}'>{3}&nbsp;{4}</a>{5}
                    </td>
				", publishmentSystemID, imageName, pageUrl, psInfo.PublishmentSystemName, EPublishmentSystemTypeUtils.GetIconHtml(psInfo.PublishmentSystemType), space);
                htmlBuilder.Append(content);
                count++;
            }
            htmlBuilder.Append("</TABLE>");
            return htmlBuilder.ToString();
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.theRoleName = base.GetQueryString("RoleName");
            this.generalPermissionArrayList = PermissionsManager.Current.PermissionArrayList;

            if (!IsPostBack)
            {
                AdminManager.VerifyPermissions(AppManager.Platform.Permission.Platform_Administrator);

                if (!string.IsNullOrEmpty(this.theRoleName))
                {
                    tbRoleName.Text = this.theRoleName;
                    tbRoleName.Enabled = false;
                    tbDescription.Text = RoleManager.GetRoleDescription(this.theRoleName);

                    if (base.GetQueryString("Return") == null)
                    {
                        ArrayList systemPermissionsInfoArrayList = DataProvider.SystemPermissionsDAO.GetSystemPermissionsInfoArrayList(this.theRoleName);
                        base.Session[SystemPermissionsInfoArrayListKey] = systemPermissionsInfoArrayList;
                    }
                }
                else
                {
                    if (base.GetQueryString("Return") == null)
                    {
                        base.Session[SystemPermissionsInfoArrayListKey] = new ArrayList();
                    }
                }

                this.rptProducts.DataSource = RoleManager.GetProductIDList(PermissionsManager.Current.Roles);
                this.rptProducts.ItemDataBound += new RepeaterItemEventHandler(rptProducts_ItemDataBound);
                this.rptProducts.DataBind();
            }
        }

        private int cblIndex = 0;
        private void rptProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string theProductID = (string)e.Item.DataItem;

                Literal ltlModuleName = e.Item.FindControl("ltlModuleName") as Literal;
                Literal ltlModulePermissions = e.Item.FindControl("ltlModulePermissions") as Literal;

                PlaceHolder phPublishmentSystemPermissions = e.Item.FindControl("phPublishmentSystemPermissions") as PlaceHolder;
                Literal ltlPublishmentSystems = e.Item.FindControl("ltlPublishmentSystems") as Literal;

                Literal ltlUserCenter = e.Item.FindControl("ltlUserCenter") as Literal;

                CheckBoxList cblModulePermissions = new CheckBoxList();

                ArrayList permissions = PermissionConfigManager.GetGeneralPermissionsOfProduct(theProductID);
                if (permissions.Count > 0)
                {
                    foreach (PermissionConfig permission in permissions)
                    {
                        if (this.generalPermissionArrayList.Contains(permission.Name))
                        {
                            ListItem listItem = new ListItem(permission.Text, permission.Name);
                            cblModulePermissions.Items.Add(listItem);
                        }
                    }

                    if (!string.IsNullOrEmpty(this.theRoleName))
                    {
                        ArrayList permissionArrayList = BaiRongDataProvider.PermissionsInRolesDAO.GetGeneralPermissionArrayList(new string[] { this.theRoleName });
                        if (permissionArrayList != null && permissionArrayList.Count > 0)
                        {
                            string[] permissionArray = new string[permissionArrayList.Count];
                            permissionArrayList.CopyTo(permissionArray);
                            ControlUtils.SelectListItems(cblModulePermissions, permissionArray);
                        }
                    }

                    StringBuilder permissionBuilder = new StringBuilder();
                    permissionBuilder.Append(@"<table border=""0""><tr>");
                    int i = 1;
                    foreach (ListItem listItem in cblModulePermissions.Items)
                    {
                        permissionBuilder.AppendFormat(@"<td><label class=""checkbox""><input type=""checkbox"" id=""cblModulePermissions{0}"" name=""{1}ModulePermissions"" {2} value=""{3}"" /> {4}</label></td>", cblIndex++, theProductID, listItem.Selected ? "checked" : string.Empty, listItem.Value, listItem.Text);
                        if (i++ % 8 == 0)
                        {
                            permissionBuilder.Append("</tr><tr>");
                        }
                    }
                    permissionBuilder.Append(@"</tr></table>");
                    ltlModulePermissions.Text = permissionBuilder.ToString();

                    //if (!StringUtils.EqualsIgnoreCase(theProductID, ProductManager.Apps.ProductID))
                    //{
                    //    ModuleFileInfo fileInfo = ProductFileUtils.GetProductFileInfo(theProductID);
                    //    ltlModuleName.Text = fileInfo.ProductName;
                    //}
                }

                phPublishmentSystemPermissions.Visible = false;
                PublishmentSystemInfo userCenter = PublishmentSystemManager.GetUniqueUserCenter();
                if (StringUtils.EqualsIgnoreCase(theProductID, ProductManager.Apps.ProductID))
                {
                    ArrayList psPermissionsInRolesInfoArrayList = (ArrayList)base.Session[SystemPermissionsInfoArrayListKey];
                    if (psPermissionsInRolesInfoArrayList != null)
                    {
                        #region 其他站点
                        ArrayList allPublishmentSystemIDArrayList = new ArrayList();
                        foreach (int itemForPSID in ProductPermissionsManager.Current.WebsitePermissionSortedList.Keys)
                        {
                            ArrayList arraylistOne = (ArrayList)ProductPermissionsManager.Current.ChannelPermissionSortedList[itemForPSID];
                            ArrayList arraylistTwo = (ArrayList)ProductPermissionsManager.Current.WebsitePermissionSortedList[itemForPSID];
                            if ((arraylistOne != null && arraylistOne.Count > 0) || (arraylistTwo != null && arraylistTwo.Count > 0))
                            {
                                phPublishmentSystemPermissions.Visible = true;
                                if (userCenter == null || userCenter.PublishmentSystemID != itemForPSID)
                                    allPublishmentSystemIDArrayList.Add(itemForPSID);
                            }
                        }
                        ArrayList managedPublishmentSystemIDArrayList = new ArrayList();
                        foreach (SystemPermissionsInfo systemPermissionsInfo in psPermissionsInRolesInfoArrayList)
                        {
                            managedPublishmentSystemIDArrayList.Add(systemPermissionsInfo.PublishmentSystemID);
                        }
                        ltlPublishmentSystems.Text = this.GetPublishmentSystemsHtml(allPublishmentSystemIDArrayList, managedPublishmentSystemIDArrayList); 
                        #endregion

                        #region 用户中心
                        ArrayList userCenterArrayList = new ArrayList();
                        if (userCenter != null)
                        {
                            userCenterArrayList.Add(userCenter.PublishmentSystemID);
                            ltlUserCenter.Text = this.GetPublishmentSystemsHtml(userCenterArrayList, managedPublishmentSystemIDArrayList);
                        } 
                        #endregion
                    }
                    else
                    {
                        PageUtils.RedirectToErrorPage("页面超时，请重新进入。");
                    }
                }

                if (cblModulePermissions.Items.Count == 0 && phPublishmentSystemPermissions.Visible == false)
                {
                    e.Item.Visible = false;
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (!string.IsNullOrEmpty(this.theRoleName))
                {
                    try
                    {
                        ArrayList modules = new ArrayList();
                        ArrayList publishmentSystemPermissionsInRolesInfoArrayList = new ArrayList();
                        ArrayList generalPermissionArrayList = new ArrayList();

                        publishmentSystemPermissionsInRolesInfoArrayList = (ArrayList)base.Session[SystemPermissionsInfoArrayListKey];
                        if (publishmentSystemPermissionsInRolesInfoArrayList.Count > 0 || generalPermissionArrayList.Count > 0)
                        {
                            modules.Add(AppManager.CMS.AppID);
                        }

                        List<string> productIDList = ProductManager.GetProductIDList();
                        foreach (string theProductID in productIDList)
                        {
                            string productPermissions = base.Request[theProductID + "ModulePermissions"];
                            if (!string.IsNullOrEmpty(productPermissions))
                            {
                                generalPermissionArrayList.AddRange(TranslateUtils.StringCollectionToArrayList(productPermissions));
                                modules.Add(theProductID);
                            }
                        }

                        BaiRongDataProvider.PermissionsInRolesDAO.UpdateRoleAndGeneralPermissions(this.theRoleName, modules, this.tbDescription.Text, generalPermissionArrayList);

                        DataProvider.PermissionsDAO.UpdatePublishmentPermissions(this.theRoleName, publishmentSystemPermissionsInRolesInfoArrayList);

                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改管理员角色", string.Format("角色名称:{0}", this.theRoleName));
                        base.SuccessMessage("角色修改成功！");
                        base.AddWaitAndRedirectScript(PageUtils.GetCMSUrl("background_role.aspx?module=" + AppManager.CMS.AppID));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "角色修改失败！");
                    }
                }
                else
                {
                    if (RoleManager.IsRoleExists(this.tbRoleName.Text))
                    {
                        base.FailMessage("角色添加失败，角色标识已存在！");
                    }
                    else
                    {
                        ArrayList modules = new ArrayList();
                        ArrayList publishmentSystemPermissionsInRolesInfoArrayList = new ArrayList();
                        ArrayList generalPermissionArrayList = new ArrayList();

                        publishmentSystemPermissionsInRolesInfoArrayList = (ArrayList)base.Session[SystemPermissionsInfoArrayListKey];
                        if (publishmentSystemPermissionsInRolesInfoArrayList.Count > 0 || generalPermissionArrayList.Count > 0)
                        {
                            modules.Add(AppManager.CMS.AppID);
                        }

                        List<string> productIDList = ProductManager.GetProductIDList();
                        foreach (string theProductID in productIDList)
                        {
                            string modulePermissions = base.Request[theProductID + "ModulePermissions"];
                            if (!string.IsNullOrEmpty(modulePermissions))
                            {
                                generalPermissionArrayList.AddRange(TranslateUtils.StringCollectionToArrayList(modulePermissions));
                                modules.Add(theProductID);
                            }
                        }

                        try
                        {
                            DataProvider.PermissionsDAO.InsertRoleAndPermissions(this.tbRoleName.Text, modules, AdminManager.Current.UserName, this.tbDescription.Text, generalPermissionArrayList, publishmentSystemPermissionsInRolesInfoArrayList);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "新增管理员角色", string.Format("角色名称:{0}", this.tbRoleName.Text));

                            base.SuccessMessage("角色添加成功！");
                            base.AddWaitAndRedirectScript(PageUtils.GetCMSUrl("background_role.aspx?module=" + AppManager.CMS.AppID));
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, string.Format("角色添加失败，{0}", ex.Message));
                        }
                    }
                }

            }
        }

    }
}
