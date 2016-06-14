using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Collections.Specialized;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class InputClassifyPermission : BackgroundBasePage
    {
        protected ListBox ClassifyCollection;
        protected DropDownList ddlRole;
        protected DropDownList ddlPermission;

        private string returnUrl;
        private int publishmentSystemID;
        private InputClissifyInfo pinfo;

        public static string GetOpenWindowString(int publishmentSystemID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtility.GetOpenWindowString("分类权限", "modal_inputClassifyPermission.aspx", arguments, 600, 550);
        }

        public static string GetRedirectUrl(int publishmentSystemID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtils.AddQueryString(PageUtils.GetCMSUrl("modal_inputClassifyPermission.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ReturnUrl");

            this.publishmentSystemID = base.GetIntQueryString("PublishmentSystemID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            pinfo = DataProvider.InputClassifyDAO.GetDefaultInfo(base.PublishmentSystemID);
            if (!IsPostBack)
            {

                //有本站点表单权限的角色
                ArrayList roles = DataProvider.SystemPermissionsDAO.GetSystemPermissionsInfoArrayListByPublishmentSystemID(this.publishmentSystemID, string.Format(" and WebsitePermissions  like '%{0}%' ", AppManager.CMS.Permission.WebSite.Input));

                foreach (SystemPermissionsInfo role in roles)
                {
                    if (!EPredefinedRoleUtils.IsPredefinedRole(role.RoleName))
                    {
                        this.ddlRole.Items.Add(new ListItem(role.RoleName, role.RoleName));
                    }
                }
                ArrayList cAll = DataProvider.InputClassifyDAO.GetItemIDArrayListByParentID(base.PublishmentSystemID, pinfo.ItemID);

                //分类              
                TreeManager.AddListItems(this.ClassifyCollection.Items, this.publishmentSystemID, pinfo.ItemID, true, true, "InputClassify");
                this.ClassifyCollection.Items.RemoveAt(0);

                //加载第一个角色的分类权限
                if (roles.Count > 0)
                {
                    this.ddlRole.SelectedIndex = 0;
                    //判断角色是否有表单管理权限，有则分类权限有查看和编辑 ，没有则判断是否有查看权限，有则有分类查看权限
                    if (HasWebsitePermissionsByRole(this.ddlRole.SelectedValue, this.publishmentSystemID, AppManager.CMS.Permission.WebSite.Input))
                    {
                        //分类权限
                        this.ddlPermission.Items.Clear();
                        ListItem item = new ListItem("查看", AppManager.CMS.Permission.WebSite.InputClassifyView);
                        this.ddlPermission.Items.Add(item);
                        item = new ListItem("编辑", AppManager.CMS.Permission.WebSite.InputClassifyEdit);
                        this.ddlPermission.Items.Add(item);
                        this.ddlPermission.SelectedIndex = 0;

                        foreach (ListItem itemInfo in this.ClassifyCollection.Items)
                        {
                            if (HasWebsitePermissionsByRole(this.ddlRole.SelectedValue, this.publishmentSystemID, this.ddlPermission.SelectedValue + "_" + itemInfo.Value))
                            {
                                itemInfo.Selected = true;
                            }
                        }
                    }
                    else
                    {
                        //分类权限
                        this.ddlPermission.Items.Clear();
                        ListItem item = new ListItem("查看", AppManager.CMS.Permission.WebSite.InputClassifyView);
                        this.ddlPermission.Items.Add(item);
                        this.ddlPermission.SelectedIndex = 0;

                        foreach (ListItem itemInfo in this.ClassifyCollection.Items)
                        {
                            if (HasWebsitePermissionsByRole(this.ddlRole.SelectedValue, this.publishmentSystemID, this.ddlPermission.SelectedValue + "_" + itemInfo.Value))
                            {
                                itemInfo.Selected = true;
                            }
                        }
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {

            bool isChanged = false;

            try
            {
                SystemPermissionsInfo info = DataProvider.SystemPermissionsDAO.GetSystemPermissionsInfoByRP(this.ddlRole.SelectedValue, this.publishmentSystemID);

                //获取角色原有权限  
                ArrayList websitePermissionArrayListOld = TranslateUtils.StringCollectionToArrayList(info.WebsitePermissions);

                //拼接现在的表单分类权限
                ArrayList cPermissionArrayListNew = new ArrayList();
                foreach (ListItem item in this.ClassifyCollection.Items)
                {
                    if (item.Selected)
                    {
                        //如果权限为编辑权限，则有查看和编辑的权限
                        if (this.ddlPermission.SelectedValue == AppManager.CMS.Permission.WebSite.InputClassifyEdit)
                        {
                            cPermissionArrayListNew.Add(AppManager.CMS.Permission.WebSite.InputClassifyEdit + "_" + item.Value);
                            cPermissionArrayListNew.Add(AppManager.CMS.Permission.WebSite.InputClassifyView + "_" + item.Value);
                            //如果只有下级的编辑权限，将上级的查看权限加上（因为加载分类树时是加载有查看权限的树）
                            InputClissifyInfo ccinfo = DataProvider.InputClassifyDAO.GetInputClassifyInfo(TranslateUtils.ToInt(item.Value));
                            if (ccinfo != null)
                            {
                                if (ccinfo.ParentID != pinfo.ItemID && !cPermissionArrayListNew.Contains(AppManager.CMS.Permission.WebSite.InputClassifyView + "_" + ccinfo.ParentID))//上级不是全部分类的ID，且不在新的表单分类的权限里
                                {
                                    cPermissionArrayListNew.Add(AppManager.CMS.Permission.WebSite.InputClassifyView + "_" + ccinfo.ParentID);
                                }
                            }
                        }
                        else
                        {
                            cPermissionArrayListNew.Add(AppManager.CMS.Permission.WebSite.InputClassifyView + "_" + item.Value);
                            //如果只有下级的编辑权限，将上级的查看权限加上（因为加载分类树时是加载有查看权限的树）
                            InputClissifyInfo ccinfo = DataProvider.InputClassifyDAO.GetInputClassifyInfo(TranslateUtils.ToInt(item.Value));
                            if (ccinfo != null)
                            {
                                if (ccinfo.ParentID != pinfo.ItemID && !cPermissionArrayListNew.Contains(AppManager.CMS.Permission.WebSite.InputClassifyView + "_" + ccinfo.ParentID))//上级不是全部分类的ID，且不在新的表单分类的权限里
                                {
                                    cPermissionArrayListNew.Add(AppManager.CMS.Permission.WebSite.InputClassifyView + "_" + ccinfo.ParentID);
                                }
                            }
                        }
                    }
                }

                if (cPermissionArrayListNew.Count == 0)
                {
                    base.FailMessage("表单分类权限设置失败：请选择分类！");
                    return;
                }

                //去掉重新设置前的表单分类权限
                ArrayList websitePermissionArrayListOld2 = TranslateUtils.StringCollectionToArrayList(info.WebsitePermissions);
                foreach (string value in websitePermissionArrayListOld2)
                {
                    if (value.StartsWith(this.ddlPermission.SelectedValue + "_"))
                        websitePermissionArrayListOld.Remove(value);
                }

                string websitePermissionArrayListNew = (websitePermissionArrayListOld.Count > 0 ? TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(websitePermissionArrayListOld) + "," : "") + TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(cPermissionArrayListNew);
                info.WebsitePermissions = websitePermissionArrayListNew;
                DataProvider.SystemPermissionsDAO.Update(info);

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "设置表单分类权限", string.Format("角色:{0}", this.ddlRole.SelectedValue));

                base.SuccessMessage("表单分类权限设置成功！");
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "表单分类权限设置失败！");
            }

            if (isChanged)
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);

        }

        public void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlPermission.Items.Clear();
            this.ClassifyCollection.Items.Clear(); 
            TreeManager.AddListItems(this.ClassifyCollection.Items, this.publishmentSystemID, pinfo.ItemID, true, true, "InputClassify");
            this.ClassifyCollection.Items.RemoveAt(0);
            //判断角色是否有表单管理权限，有则分类权限有查看和编辑 ，没有则判断是否有查看权限，有则有分类查看权限
            if (HasWebsitePermissionsByRole(this.ddlRole.SelectedValue, this.publishmentSystemID, AppManager.CMS.Permission.WebSite.Input))
            {
                //分类权限 
                ListItem item = new ListItem("查看", AppManager.CMS.Permission.WebSite.InputClassifyView);
                this.ddlPermission.Items.Add(item);
                item = new ListItem("编辑", AppManager.CMS.Permission.WebSite.InputClassifyEdit);
                this.ddlPermission.Items.Add(item);
                this.ddlPermission.SelectedIndex = 0;

                foreach (ListItem itemInfo in this.ClassifyCollection.Items)
                {
                    if (HasWebsitePermissionsByRole(this.ddlRole.SelectedValue, this.publishmentSystemID, this.ddlPermission.SelectedValue + "_" + itemInfo.Value))
                    {
                        itemInfo.Selected = true;
                    }
                }
            }
            else
            {
                //分类权限
                ListItem item = new ListItem("查看", AppManager.CMS.Permission.WebSite.InputClassifyView);
                this.ddlPermission.Items.Add(item);
                this.ddlPermission.SelectedIndex = 0;

                foreach (ListItem itemInfo in this.ClassifyCollection.Items)
                {
                    if (HasWebsitePermissionsByRole(this.ddlRole.SelectedValue, this.publishmentSystemID, this.ddlPermission.SelectedValue + "_" + itemInfo.Value))
                    {
                        itemInfo.Selected = true;
                    }
                }
            }
        }


        public void ddlPermission_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ClassifyCollection.Items.Clear();
            TreeManager.AddListItems(this.ClassifyCollection.Items, this.publishmentSystemID, pinfo.ItemID, true, true, "InputClassify");
            this.ClassifyCollection.Items.RemoveAt(0);
            foreach (ListItem itemInfo in this.ClassifyCollection.Items)
            {
                if (HasWebsitePermissionsByRole(this.ddlRole.SelectedValue, this.publishmentSystemID, this.ddlPermission.SelectedValue + "_" + itemInfo.Value))
                {
                    itemInfo.Selected = true;
                }
            }
        }


        public static bool HasWebsitePermissionsByRole(string roleName, int publishmentSystemID, params string[] websitePermissionArray)
        {
            ArrayList websitePermissionArrayList = DataProvider.SystemPermissionsDAO.GetWebsitePermissionListByRP(roleName, publishmentSystemID);
            foreach (string websitePermission in websitePermissionArray)
            {
                if (websitePermissionArrayList.Contains(websitePermission))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
