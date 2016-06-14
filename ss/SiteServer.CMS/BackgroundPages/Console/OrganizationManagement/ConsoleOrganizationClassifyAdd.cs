using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;



namespace SiteServer.CMS.BackgroundPages
{
    public class ConsoleOrganizationClassifyAdd : BackgroundBasePage
    {
        public DropDownList ParentItemID;
        public TextBox ItemName;
        public TextBox ItemIndexName;

        private int itemID;
        private string returnUrl;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ItemID", "ReturnUrl");
            this.itemID = base.GetIntQueryString("ItemID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            string parentItemID = base.GetQueryString("parentItemID");

            OrganizationClassifyInfo itemInfo = DataProvider.OrganizationClassifyDAO.GetInfo(this.itemID);

            if (!base.IsPostBack)
            {

                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Organization, "添加分类", AppManager.Platform.Permission.Platform_Organization);

                TreeManager.AddListItems(this.ParentItemID.Items, base.PublishmentSystemID, this.itemID, true, true, "OrganizationClassify", 1);
                ControlUtils.SelectListItems(this.ParentItemID, parentItemID);

                if (this.itemID > 0)
                {
                    this.ItemName.Text = itemInfo.ItemName;
                    this.ItemIndexName.Text = itemInfo.ItemIndexName;
                    ControlUtils.SelectListItems(this.ParentItemID, string.IsNullOrEmpty(parentItemID) ? itemInfo.ParentID.ToString() : parentItemID);
                }
            }
        }

        public void ParentItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int theItemID = TranslateUtils.ToInt(this.ParentItemID.SelectedValue);
            if (theItemID == 0)
            {
                theItemID = this.itemID;
            }
            PageUtils.Redirect(GetRedirectUrl(base.PublishmentSystemID, TranslateUtils.ToInt(base.GetQueryString("itemID")), theItemID, base.GetQueryString("ReturnUrl")));
        }

        public static string GetRedirectUrl(int publishmentSystemID, int itemID, int parentID, string returnUrl)
        {
            return PageUtils.GetCMSUrl(string.Format("console_organizationClassifyAdd.aspx?PublishmentSystemID={0}&ItemID={1}&parentItemID={2}&ReturnUrl={3}", publishmentSystemID, itemID, parentID, StringUtils.ValueToUrl(returnUrl)));
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                int insertItemID = 0;

                try
                {
                    if (this.itemID == 0)
                    {
                        int parentItemID = TranslateUtils.ToInt(base.Request.Form["parentItemID"]);
                        OrganizationClassifyInfo itemInfo = new OrganizationClassifyInfo();
                        itemInfo.ParentID = parentItemID;
                        itemInfo.PublishmentSystemID = base.PublishmentSystemID;
                        itemInfo.ItemName = PageUtils.FilterXSS(this.ItemName.Text);
                        itemInfo.ItemIndexName = PageUtils.FilterXSS(this.ItemIndexName.Text);
                        itemInfo.Enabled = true;

                        if (ItemIndexName.Text.Length != 0)
                        {
                            ArrayList itemIndexNameArrayList = DataProvider.OrganizationClassifyDAO.GetItemIndexNameArrayList(base.PublishmentSystemID);
                            if (itemIndexNameArrayList.IndexOf(ItemIndexName.Text) != -1)
                            {
                                base.FailMessage("分类添加失败，分类编码已存在！");
                                return;
                            }
                        }

                        itemInfo.AddDate = DateTime.Now;

                        insertItemID = DataProvider.OrganizationClassifyDAO.Insert(itemInfo);
                        base.SuccessMessage("分类添加成功！");
                    }
                    else
                    {
                        if (this.ParentItemID.SelectedValue == this.itemID.ToString())
                        {
                            base.FailMessage("分类修改失败，不成为自己的下级！");
                            return;
                        }
                        int parentItemID = TranslateUtils.ToInt(base.Request.Form["parentItemID"]);
                        OrganizationClassifyInfo itemInfo =DataProvider.OrganizationClassifyDAO.GetInfo(this.itemID);
                        if (itemInfo.ParentID > 0)
                            itemInfo.ParentID = parentItemID;
                        itemInfo.ItemName = PageUtils.FilterXSS(this.ItemName.Text);


                        if (ItemIndexName.Text.Length != 0 && ItemIndexName.Text != itemInfo.ItemIndexName)
                        {
                            ArrayList itemIndexNameArrayList = DataProvider.OrganizationClassifyDAO.GetItemIndexNameArrayList(base.PublishmentSystemID);
                            if (itemIndexNameArrayList.IndexOf(ItemIndexName.Text) != -1)
                            {
                                base.FailMessage("分类修改失败，分类编码已存在！");
                                return;
                            }
                        }

                        itemInfo.ItemIndexName = PageUtils.FilterXSS(this.ItemIndexName.Text);
                        DataProvider.OrganizationClassifyDAO.Update(itemInfo);
                        base.SuccessMessage("分类修改成功！");
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("分类添加失败：{0}", ex.Message));
                    LogUtils.AddErrorLog(ex);
                    return;
                }

                base.AddWaitAndRedirectScript(this.returnUrl);
            }
        }

        public string ReturnUrl { get { return this.returnUrl; } }
    }
}
