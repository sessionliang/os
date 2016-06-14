using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Core.Office;


using System.Collections.Generic;


namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicContentTree : BackgroundGovPublicBasePage
    {
        public Repeater rptCategoryChannel;
        public Repeater rptCategoryClass;

        public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.GovPublicChannelTree, null));

                NameValueCollection additional = new NameValueCollection();
                additional.Add("PublishmentSystemID", base.PublishmentSystemID.ToString());
                additional.Add("DepartmentIDCollection", TranslateUtils.ObjectCollectionToString(GovPublicManager.GetFirstDepartmentIDArrayList(base.PublishmentSystemInfo)));

                JsManager.RegisterClientScriptBlock(Page, "DepartmentTreeScript", DepartmentTreeItem.GetScript(EDepartmentLoadingType.ContentTree, additional));

                ArrayList categoryClassInfoArrayList = DataProvider.GovPublicCategoryClassDAO.GetCategoryClassInfoArrayList(base.PublishmentSystemID, ETriState.False, ETriState.True);
                foreach (GovPublicCategoryClassInfo categoryClassInfo in categoryClassInfoArrayList)
                {
                    JsManager.RegisterClientScriptBlock(Page, "CategoryTreeScript_" + categoryClassInfo.ClassCode, GovPublicCategoryTreeItem.GetScript(categoryClassInfo.ClassCode, base.PublishmentSystemID, EGovPublicCategoryLoadingType.Tree, null));
                }

                this.BindGrid(categoryClassInfoArrayList);
            }
        }

        public void BindGrid(ArrayList categoryClassInfoArrayList)
        {
            try
            {
                this.rptCategoryChannel.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, base.PublishmentSystemInfo.Additional.GovPublicNodeID);
                this.rptCategoryChannel.ItemDataBound += new RepeaterItemEventHandler(rptCategoryChannel_ItemDataBound);
                this.rptCategoryChannel.DataBind();

                this.rptCategoryClass.DataSource = categoryClassInfoArrayList;
                this.rptCategoryClass.ItemDataBound += new RepeaterItemEventHandler(rptCategoryClass_ItemDataBound);
                this.rptCategoryClass.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        private void rptCategoryClass_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            GovPublicCategoryClassInfo categoryClassInfo = e.Item.DataItem as GovPublicCategoryClassInfo;

            Literal ltlClassName = e.Item.FindControl("ltlClassName") as Literal;
            Literal ltlPlusIcon = e.Item.FindControl("ltlPlusIcon") as Literal;

            ltlClassName.Text = categoryClassInfo.ClassName;
            ltlPlusIcon.Text = string.Format(@"<img align=""absmiddle"" style=""cursor:pointer"" onClick=""displayChildren_{0}(this);"" isAjax=""true"" isOpen=""false"" id=""0"" src=""../../sitefiles/bairong/icons/tree/plus.gif"" />", categoryClassInfo.ClassCode);
        }

        private void rptCategoryChannel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int nodeID = (int)e.Item.DataItem;
            bool enabled = (base.IsOwningNodeID(nodeID)) ? true : false;
            if (!enabled)
            {
                if (!base.IsHasChildOwningNodeID(nodeID)) e.Item.Visible = false;
            }
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = ChannelLoading.GetChannelRowHtml(base.PublishmentSystemInfo, nodeInfo, enabled, ELoadingType.GovPublicChannelTree, null);
        }
    }
}
