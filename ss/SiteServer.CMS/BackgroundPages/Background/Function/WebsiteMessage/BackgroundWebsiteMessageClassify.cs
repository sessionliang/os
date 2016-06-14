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

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundWebsiteMessageClassify : BackgroundBasePage
    {
        public Repeater rptContents;

        public PlaceHolder PlaceHolder_AddChannel;
        public Button AddChannel1;
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
                    DataProvider.WebsiteMessageClassifyDAO.UpdateTaxis(base.PublishmentSystemID, itemID, isSubtract);

                    StringUtility.AddLog(base.PublishmentSystemID, itemID, 0, "分类排序" + (isSubtract ? "上升" : "下降"), string.Format("分类:{0}", DataProvider.WebsiteMessageClassifyDAO.GetItemName(base.PublishmentSystemID, itemID)));

                    PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_websiteMessageClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                }
            }

            if (base.GetQueryString("ItemIDCollection") != null && base.GetQueryString("Delete") != null)
            {
                ArrayList itemIDList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ItemIDCollection"));
                foreach (int deleteID in itemIDList)
                {
                    DataProvider.WebsiteMessageClassifyDAO.Delete(deleteID);
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_WebsiteMessage, "分类管理", AppManager.CMS.Permission.WebSite.WebsiteMessage);

                #region 默认创建一个网站留言，网站留言分类
                DataProvider.WebsiteMessageClassifyDAO.SetDefaultWebsiteMessageClassifyInfo(base.PublishmentSystemID);
                DataProvider.WebsiteMessageDAO.SetDefaultWebsiteMessageInfo(base.PublishmentSystemID);
                #endregion

                NameValueCollection additional = new NameValueCollection();
                additional.Add("returnUrl", PageUtils.GetCMSUrl(string.Format("background_websiteMessageClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                additional.Add("upLink", "background_websiteMessageClassify.aspx");
                additional.Add("downLink", "background_websiteMessageClassify.aspx");
                additional.Add("editLink", "background_websiteMessageClassifyAdd.aspx");
                additional.Add("showLayer", "2");//两级
                JsManager.RegisterClientScriptBlock(Page, "TreeScript", Tree.GetScript(base.PublishmentSystemInfo, additional, "WebsiteMessageClassify", "ClassifyManage"));

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
                this.AddChannel1.Attributes.Add("onclick", Modal.WebsiteMessageClassifyAdd.GetOpenWindowString(base.PublishmentSystemID, 0, string.Format("background_websiteMessageClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
                this.AddChannel2.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", BackgroundWebsiteMessageClassifyAdd.GetRedirectUrl(base.PublishmentSystemID, 0, 0, PageUtils.GetCMSUrl(string.Format("background_websiteMessageClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)))));
            }

            this.PlaceHolder_Delete.Visible = true;
            if (this.PlaceHolder_Delete.Visible)
            {
                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetCMSUrl(string.Format("background_websiteMessageClassify.aspx?PublishmentSystemID={0}&Delete=true", base.PublishmentSystemID)), "ItemIDCollection", "ItemIDCollection", "请选择需要删除的分类！", "删除该分类将会删除该分类下的关键字，确定删除吗？"));
            }
        }

        public void BindGrid()
        {
            try
            {
                this.rptContents.DataSource = DataProvider.WebsiteMessageClassifyDAO.GetItemIDArrayListByParentID(base.PublishmentSystemID, 0);
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
            WebsiteMessageClassifyInfo itemInfo = DataProvider.WebsiteMessageClassifyDAO.GetWebsiteMessageClassifyInfo(itemID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;
            string linkUrl = "background_websiteMessageContent.aspx";
            string redirectUrl = string.Empty;
            NameValueCollection additional = new NameValueCollection();
            //additional.Add("LinkUrl", linkUrl);
            additional.Add("RedirectUrl", redirectUrl);
            additional.Add("returnUrl", PageUtils.GetCMSUrl(string.Format("background_websiteMessageClassify.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
            additional.Add("upLink", "background_websiteMessageClassify.aspx");
            additional.Add("downLink", "background_websiteMessageClassify.aspx");
            additional.Add("editLink", "background_websiteMessageClassifyAdd.aspx");
            additional.Add("showLayer", "2");//两级
            ltlHtml.Text = Tree.GetItemRowHtmlForManage(base.PublishmentSystemInfo, itemInfo, "WebsiteMessageClassify", additional);
        }
    }
}
