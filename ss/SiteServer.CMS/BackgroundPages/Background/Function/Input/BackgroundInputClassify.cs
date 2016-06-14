using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundInputClassify : BackgroundBasePage
    {
        public Repeater rptContents;

        public PlaceHolder PlaceHolder_AddChannel;
        public Button AddChannel1;
        public Button AddChannel2;
        public PlaceHolder PlaceHolder_Delete;
        public Button Delete;
        public PlaceHolder PlaceHolder_Permission;
        public Button btnPermission;

        private int currentItemID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("ItemID") != null && (base.GetQueryString("Subtract") != null || base.GetQueryString("Add") != null))
            {
                int itemID = base.GetIntQueryString("ItemID");
                if (base.PublishmentSystemID != itemID)
                {
                    bool isSubtract = (base.GetQueryString("Subtract") != null) ? true : false;
                    DataProvider.InputClassifyDAO.UpdateTaxis(base.PublishmentSystemID, itemID, isSubtract);

                    StringUtility.AddLog(base.PublishmentSystemID, itemID, 0, "分类排序" + (isSubtract ? "上升" : "下降"), string.Format("分类:{0}", DataProvider.InputClassifyDAO.GetItemName(base.PublishmentSystemID, itemID)));

                    PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_inputClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                }
            }

            if (base.GetQueryString("ItemIDCollection") != null && base.GetQueryString("Delete") != null)
            {
                ArrayList itemIDList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ItemIDCollection"));
                foreach (int deleteID in itemIDList)
                {
                    DataProvider.InputClassifyDAO.Delete(deleteID);
                }
                //修改全部分类下表单的数量
                InputClissifyInfo pinfo = DataProvider.InputClassifyDAO.GetDefaultInfo(base.PublishmentSystemID);
                DataProvider.InputClassifyDAO.UpdateCountByAll(base.PublishmentSystemID, pinfo.ItemID);

            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Input, "表单分类管理", AppManager.CMS.Permission.WebSite.Input);

                #region 默认创建一个全部分类和默认分类，表单分类
                DataProvider.InputClassifyDAO.SetDefaultInfo(base.PublishmentSystemID);
                #endregion

                NameValueCollection additional = new NameValueCollection();
                additional.Add("returnUrl", PageUtils.GetCMSUrl(string.Format("background_inputClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                additional.Add("upLink", "background_inputClassify.aspx");
                additional.Add("downLink", "background_inputClassify.aspx");
                additional.Add("editLink", "background_inputClassifyAdd.aspx");
                JsManager.RegisterClientScriptBlock(Page, "TreeScript", Tree.GetScript(base.PublishmentSystemInfo, additional, "InputClassify", "ClassifyManage"));

                if (!string.IsNullOrEmpty(base.GetQueryString("CurrentItemID")))
                {
                    this.currentItemID = TranslateUtils.ToInt(base.GetQueryString("CurrentItemID"));
                    string onLoadScript = string.Empty;// Tree.GetScriptOnLoad(base.PublishmentSystemID, this.currentItemID);
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
                this.AddChannel1.Attributes.Add("onclick", Modal.InputClassifyAdd.GetOpenWindowString(base.PublishmentSystemID, base.PublishmentSystemID, string.Format("background_inputClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                this.AddChannel2.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", BackgroundInputClassifyAdd.GetRedirectUrl(base.PublishmentSystemID, 0, 0, PageUtils.GetCMSUrl(string.Format("background_inputClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)))));
            }

            this.PlaceHolder_Delete.Visible = true;
            if (this.PlaceHolder_Delete.Visible)
            {
                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_inputClassify.aspx?PublishmentSystemID={0}&Delete=true", base.PublishmentSystemID)), "ItemIDCollection", "ItemIDCollection", "请选择需要删除的分类！", "删除该分类将会删除该分类下的表单，确定删除吗？"));
            }

            this.PlaceHolder_Permission.Visible = false;

            if (base.HasWebsitePermissions(AppManager.CMS.Permission.WebSite.InputPermission))
            {
                this.PlaceHolder_Permission.Visible = true;

                this.btnPermission.Attributes.Add("onclick", Modal.InputClassifyPermission.GetOpenWindowString(base.PublishmentSystemID, string.Format("background_inputClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
            }
        }

        public void BindGrid()
        {
            try
            {
                InputClissifyInfo pinfo = DataProvider.InputClassifyDAO.GetDefaultInfo(base.PublishmentSystemID);
                this.rptContents.DataSource = DataProvider.InputClassifyDAO.GetItemIDArrayListByParentID(base.PublishmentSystemID, pinfo.ItemID);
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
            InputClissifyInfo itemInfo = DataProvider.InputClassifyDAO.GetInputClassifyInfo(itemID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;
            string redirectUrl = string.Empty;
            NameValueCollection additional = new NameValueCollection();
            //additional.Add("LinkUrl", linkUrl);
            additional.Add("RedirectUrl", redirectUrl);
            additional.Add("returnUrl", PageUtils.GetCMSUrl(string.Format("background_inputClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
            additional.Add("upLink", "background_inputClassify.aspx");
            additional.Add("downLink", "background_inputClassify.aspx");
            additional.Add("editLink", "background_inputClassifyAdd.aspx");
            ltlHtml.Text = Tree.GetItemRowHtmlForManage(base.PublishmentSystemInfo, itemInfo, "InputClassify", additional);
        }
    }
}
