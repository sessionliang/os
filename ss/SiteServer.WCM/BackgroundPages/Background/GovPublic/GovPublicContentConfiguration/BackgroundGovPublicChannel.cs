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



namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicChannel : BackgroundGovPublicBasePage
    {
        public Repeater rptContents;

        public Button AddChannel;
        public Button Delete;

        private int currentNodeID;

        public string GetRedirectUrl(int publishmentSystemID, int currentNodeID)
        {
            if (currentNodeID != 0 && currentNodeID != publishmentSystemID)
            {
                return PageUtils.GetWCMUrl(string.Format("background_govPublicChannel.aspx?PublishmentSystemID={0}&CurrentNodeID={1}", publishmentSystemID, currentNodeID));
            }
            return PageUtils.GetWCMUrl(string.Format("background_govPublicChannel.aspx?PublishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.Request.QueryString["NodeID"] != null && (base.Request.QueryString["Subtract"] != null || base.Request.QueryString["Add"] != null))
            {
                int nodeID = int.Parse(base.Request.QueryString["NodeID"]);
                if (base.PublishmentSystemID != nodeID)
                {
                    bool isSubtract = (base.Request.QueryString["Subtract"] != null) ? true : false;
                    DataProvider.NodeDAO.UpdateTaxis(base.PublishmentSystemID, nodeID, isSubtract);

                    StringUtility.AddLog(base.PublishmentSystemID, nodeID, 0, "栏目排序" + (isSubtract ? "上升" : "下降"), string.Format("栏目:{0}", NodeManager.GetNodeName(base.PublishmentSystemID, nodeID)));

                    PageUtils.Redirect(this.GetRedirectUrl(base.PublishmentSystemID, nodeID));
                    return;
                }
            }
            else if (base.Request.QueryString["Delete"] != null && base.Request.QueryString["ChannelIDCollection"] != null)
            {
                ArrayList channelIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["ChannelIDCollection"]);
                if (channelIDArrayList.Count > 0)
                {
                    foreach (int nodeID in channelIDArrayList)
                    {
                        DataProvider.NodeDAO.Delete(nodeID);
                    }
                }
                PageUtils.Redirect(this.GetRedirectUrl(base.PublishmentSystemID, 0));
                return;
            }

            if (!IsPostBack)
            {
                base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicContentConfiguration, "分类法管理", "主题分类", AppManager.CMS.Permission.WebSite.GovPublicContentConfiguration);

                JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.GovPublicChannel, null));

                if (!string.IsNullOrEmpty(Request.QueryString["CurrentNodeID"]))
                {
                    this.currentNodeID = TranslateUtils.ToInt(base.Request.QueryString["CurrentNodeID"]);
                    string onLoadScript = ChannelLoading.GetScriptOnLoad(base.PublishmentSystemID, this.currentNodeID);
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        JsManager.RegisterClientScriptBlock(Page, "NodeTreeScriptOnLoad", onLoadScript);
                    }
                }

                this.AddChannel.Attributes.Add("onclick", Modal.GovPublicChannelAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, string.Empty));

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(PageUtils.GetWCMUrl(string.Format("background_govPublicChannel.aspx?PublishmentSystemID={0}&Delete=True", base.PublishmentSystemID)), "ChannelIDCollection", "ChannelIDCollection", "请选择需要删除的节点！", "此操作将删除对应节点以及所有下级节点，确认删除吗？"));

                BindGrid();
            }
        }

        public void BindGrid()
        {
            try
            {
                rptContents.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, base.PublishmentSystemInfo.Additional.GovPublicNodeID);
                rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                rptContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int nodeID = (int)e.Item.DataItem;
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            bool enabled = false;
            if (EContentModelTypeUtils.Equals(EContentModelType.GovPublic, nodeInfo.ContentModelID))
            {
                enabled = true;
            }

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = ChannelLoading.GetChannelRowHtml(base.PublishmentSystemInfo, nodeInfo, enabled, ELoadingType.GovPublicChannel, null);
        }
    }
}
