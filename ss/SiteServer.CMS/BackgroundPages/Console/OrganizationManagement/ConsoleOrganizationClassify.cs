﻿using System;
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

namespace SiteServer.CMS.BackgroundPages
{
    public class ConsoleOrganizationClassify : BackgroundBasePage
    {
        public Repeater rptContents;

        public PlaceHolder PlaceHolder_AddChannel;
        public Button AddChannel2;
        public PlaceHolder PlaceHolder_Delete;
        public Button Delete;

        private int currentItemID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("ItemID") != null && (base.GetQueryString("Subtract") != null || base.GetQueryString("Add") != null))
            {
                int itemID = int.Parse(base.GetQueryString("ItemID"));
                if (base.PublishmentSystemID != itemID)
                {
                    bool isSubtract = (base.GetQueryString("Subtract") != null) ? true : false;
                    DataProvider.OrganizationClassifyDAO.UpdateTaxis(base.PublishmentSystemID, itemID, isSubtract);

                    StringUtility.AddLog(base.PublishmentSystemID, itemID, 0, "分类排序" + (isSubtract ? "上升" : "下降"), string.Format("分类:{0}", DataProvider.OrganizationClassifyDAO.GetItemName(base.PublishmentSystemID, itemID)));

                    PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("console_organizationClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                }
            }

            if (base.GetQueryString("ItemIDCollection") != null && base.GetQueryString("Delete") != null)
            {
                ArrayList itemIDList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ItemIDCollection"));
                foreach (int deleteID in itemIDList)
                {
                    DataProvider.OrganizationClassifyDAO.Delete(deleteID);
                }
                //重新统计全部分类的数量 
                int count = DataProvider.OrganizationInfoDAO.GetCount();
                OrganizationClassifyInfo pinfo = DataProvider.OrganizationClassifyDAO.GetFirstInfo();
                DataProvider.OrganizationClassifyDAO.UpdateContentNum(base.PublishmentSystemID, pinfo.ItemID, count);
                base.SuccessMessage("分类删除成功！");
            }

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Organization, "分类管理", AppManager.Platform.Permission.Platform_Organization);

                #region 默认创建一个全部分类
                DataProvider.OrganizationClassifyDAO.SetDefaultInfo(base.PublishmentSystemID); 
                #endregion

                NameValueCollection additional = new NameValueCollection();
                additional.Add("returnUrl", PageUtils.GetCMSUrl(string.Format("console_organizationClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                additional.Add("upLink", "console_organizationClassify.aspx");
                additional.Add("downLink", "console_organizationClassify.aspx");
                additional.Add("editLink", "console_organizationClassifyAdd.aspx");
                JsManager.RegisterClientScriptBlock(Page, "TreeScript", Tree.GetScript(base.PublishmentSystemInfo, additional, "OrganizationClassify", "ClassifyManage"));

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
            if (this.PlaceHolder_AddChannel.Visible)
            {
                this.AddChannel2.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", ConsoleOrganizationClassifyAdd.GetRedirectUrl(base.PublishmentSystemID, 0, 0, PageUtils.GetCMSUrl(string.Format("console_organizationClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)))));
            }

            this.PlaceHolder_Delete.Visible = true;
            if (this.PlaceHolder_Delete.Visible)
            {
                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("console_organizationClassify.aspx?PublishmentSystemID={0}&Delete=true", base.PublishmentSystemID)), "ItemIDCollection", "ItemIDCollection", "请选择需要删除的分类！", "删除该分类将会删除该分类下的区域和机构，确定删除吗？"));
            }
        }

        public void BindGrid()
        {
            try
            {
                OrganizationClassifyInfo pinfo = DataProvider.OrganizationClassifyDAO.GetFirstInfo();
                this.rptContents.DataSource = DataProvider.OrganizationClassifyDAO.GetItemIDArrayListByParentID(base.PublishmentSystemID, pinfo.ItemID);
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
            OrganizationClassifyInfo itemInfo = DataProvider.OrganizationClassifyDAO.GetInfoByNew(itemID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;
            string linkUrl = string.Empty;
            string redirectUrl = string.Empty;
            NameValueCollection additional = new NameValueCollection();
            additional.Add("LinkUrl", linkUrl);
            additional.Add("RedirectUrl", redirectUrl);
            additional.Add("returnUrl", PageUtils.GetCMSUrl(string.Format("console_organizationClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
            additional.Add("upLink", "console_organizationClassify.aspx");
            additional.Add("downLink", "console_organizationClassify.aspx");
            additional.Add("editLink", "console_organizationClassifyAdd.aspx");
            ltlHtml.Text = Tree.GetItemRowHtmlForManage(base.PublishmentSystemInfo, itemInfo, "OrganizationClassify", additional);
        }
    }
}
