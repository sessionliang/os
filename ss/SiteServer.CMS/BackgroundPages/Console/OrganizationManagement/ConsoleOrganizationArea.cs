using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;


using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Controls;
using System.Text;
using System.Web.Script.Serialization;

namespace SiteServer.CMS.BackgroundPages
{
    public class ConsoleOrganizationArea : BackgroundBasePage
    {
        public Repeater rptContents;

        public PlaceHolder PlaceHolder_AddChannel;
        public Button AddChannel2;
        public PlaceHolder PlaceHolder_Delete;
        public Button Delete;

        private int currentItemID;
        private int classifyID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.classifyID = base.GetIntQueryString("ItemID");
            if (base.GetQueryString("ClassifyID") != null)
                this.classifyID = base.GetIntQueryString("ClassifyID");

            if (base.GetQueryString("ItemID") != null && (base.GetQueryString("Subtract") != null || base.GetQueryString("Add") != null))
            {
                int itemID = int.Parse(base.GetQueryString("ItemID"));
                if (base.PublishmentSystemID != itemID)
                {
                    bool isSubtract = (base.GetQueryString("Subtract") != null) ? true : false;
                    DataProvider.OrganizationAreaDAO.UpdateTaxis(base.PublishmentSystemID, this.classifyID, itemID, isSubtract);

                    StringUtility.AddLog(base.PublishmentSystemID, itemID, 0, "区域排序" + (isSubtract ? "上升" : "下降"), string.Format("区域:{0}", DataProvider.OrganizationAreaDAO.GetItemName(base.PublishmentSystemID, itemID)));

                    PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("console_organizationArea.aspx?PublishmentSystemID={0}&ClassifyID={1}", base.PublishmentSystemID, this.classifyID)));
                }
            }

            if (base.GetQueryString("ItemIDCollection") != null && base.GetQueryString("Delete") != null)
            {
                ArrayList itemIDList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ItemIDCollection"));
                foreach (int deleteID in itemIDList)
                {
                    DataProvider.OrganizationAreaDAO.Delete(deleteID);
                }
                base.SuccessMessage("区域删除成功！");
            }

            if (!IsPostBack)
            {
                OrganizationClassifyInfo selInfo = DataProvider.OrganizationClassifyDAO.GetInfo(this.classifyID);
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Organization, "区域管理 / " + selInfo.ItemName, AppManager.Platform.Permission.Platform_Organization);

                NameValueCollection additional = new NameValueCollection();
                additional.Add("returnUrl", PageUtils.GetCMSUrl(string.Format("console_organizationArea.aspx?PublishmentSystemID={0}&ClassifyID={1}", base.PublishmentSystemID, this.classifyID)));
                additional.Add("upLink", "console_organizationArea.aspx");
                additional.Add("downLink", "console_organizationArea.aspx");
                additional.Add("editLink", "console_organizationAreaAdd.aspx");
                additional.Add("showAttribute", OrganizationAreaAttribute.ContentNum);
                additional.Add("linkParam", "&ClassifyID=" + this.classifyID.ToString());

                JsManager.RegisterClientScriptBlock(Page, "TreeScript", Tree.GetScript(base.PublishmentSystemInfo, additional, "OrganizationArea", "ClassifyManage"));

                if (!string.IsNullOrEmpty(base.GetQueryString("CurrentItemID")))
                {
                    this.currentItemID = TranslateUtils.ToInt(base.GetQueryString("CurrentItemID"));
                    string onLoadScript = string.Empty;
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        JsManager.RegisterClientScriptBlock(Page, "TreeScriptOnLoad", onLoadScript);
                    }
                }

                ButtonPreLoad();

                BindGrid();
            }
        }

        private void ButtonPreLoad()
        {
            NameValueCollection arguments = new NameValueCollection();
            string showPopWinString = string.Empty;

            this.PlaceHolder_AddChannel.Visible = true;
            OrganizationClassifyInfo info = DataProvider.OrganizationClassifyDAO.GetInfo(this.classifyID);
            OrganizationClassifyInfo pinfo = DataProvider.OrganizationClassifyDAO.GetFirstInfo();
            if (info != null)
            {
                if (this.classifyID != pinfo.ItemID)
                {
                    this.PlaceHolder_AddChannel.Visible = true;
                    this.PlaceHolder_Delete.Visible = true;
                }
                else
                {
                    this.PlaceHolder_AddChannel.Visible = false;
                    this.PlaceHolder_Delete.Visible = false;
                }
            }
            else
            {
                this.PlaceHolder_AddChannel.Visible = false;
                this.PlaceHolder_Delete.Visible = false;
            }
            if (this.PlaceHolder_AddChannel.Visible)
            {
                this.AddChannel2.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", ConsoleOrganizationAreaAdd.GetRedirectUrl(base.PublishmentSystemID, 0, this.classifyID, PageUtils.GetCMSUrl(string.Format("console_organizationArea.aspx?PublishmentSystemID={0}&ClassifyID={1}", base.PublishmentSystemID, this.classifyID)))));
            }

            this.PlaceHolder_Delete.Visible = true;
            if (this.PlaceHolder_Delete.Visible)
            {
                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("console_organizationArea.aspx?PublishmentSystemID={0}&ClassifyID={1}&Delete=true", base.PublishmentSystemID, this.classifyID)), "ItemIDCollection", "ItemIDCollection", "请选择需要删除的区域！", "删除该区域将会删除该区域下的子区域，确定删除吗？"));
            }
        }

        public void BindGrid()
        {
            try
            {
                this.rptContents.DataSource = DataProvider.OrganizationAreaDAO.GetItemIDArrayListByParentID(base.PublishmentSystemID, 0, this.classifyID);
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int itemID = (int)e.Item.DataItem;
            OrganizationAreaInfo itemInfo = DataProvider.OrganizationAreaDAO.GetInfoByNew(itemID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;
            string linkUrl = string.Empty;
            string redirectUrl = string.Empty;
            NameValueCollection additional = new NameValueCollection();
            additional.Add("LinkUrl", linkUrl);
            additional.Add("RedirectUrl", redirectUrl);
            additional.Add("returnUrl", PageUtils.GetCMSUrl(string.Format("console_organizationArea.aspx?PublishmentSystemID={0}&ClassifyID={1}", base.PublishmentSystemID, this.classifyID)));
            additional.Add("upLink", "console_organizationArea.aspx");
            additional.Add("downLink", "console_organizationArea.aspx");
            additional.Add("editLink", "console_organizationAreaAdd.aspx");
            additional.Add("showAttribute", OrganizationAreaAttribute.ContentNum);
            additional.Add("linkParam", "&ClassifyID=" + this.classifyID.ToString());


            ltlHtml.Text = Tree.GetItemRowHtmlForManage(base.PublishmentSystemInfo, itemInfo, "OrganizationArea", additional);
        }
    }
}
