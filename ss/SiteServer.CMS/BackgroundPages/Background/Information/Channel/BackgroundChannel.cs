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

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundChannel : BackgroundBasePage
    {
        public Repeater rptContents;

        public PlaceHolder PlaceHolder_AddChannel;
        public Button AddChannel1;
        public Button AddChannel2;
        public PlaceHolder PlaceHolder_ChannelEdit;
        public Button AddToGroup;
        public Button SelectEditColumns;
        public PlaceHolder PlaceHolder_Translate;
        public Button Translate;
        public PlaceHolder PlaceHolder_Import;
        public Button Import;
        public Button Export;
        public PlaceHolder PlaceHolder_Delete;
        public Button Delete;
        public PlaceHolder PlaceHolder_Create;
        public Button Create;
        public PlaceHolder PlaceHolder_Publish;
        public Button Publish;

        private int currentNodeID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("NodeID") != null && (base.GetQueryString("Subtract") != null || base.GetQueryString("Add") != null))
            {
                int nodeID = int.Parse(base.GetQueryString("NodeID"));
                if (base.PublishmentSystemID != nodeID)
                {
                    bool isSubtract = (base.GetQueryString("Subtract") != null) ? true : false;
                    DataProvider.NodeDAO.UpdateTaxis(base.PublishmentSystemID, nodeID, isSubtract);

                    StringUtility.AddLog(base.PublishmentSystemID, nodeID, 0, "栏目排序" + (isSubtract ? "上升" : "下降"), string.Format("栏目:{0}", NodeManager.GetNodeName(base.PublishmentSystemID, nodeID)));

                    PageUtils.Redirect(ChannelLoading.GetRedirectUrl(base.PublishmentSystemID, nodeID));
                    return;
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "栏目管理", string.Empty);

                JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.Channel, null));

                if (!string.IsNullOrEmpty(base.GetQueryString("CurrentNodeID")))
                {
                    this.currentNodeID = TranslateUtils.ToInt(base.GetQueryString("CurrentNodeID"));
                    string onLoadScript = ChannelLoading.GetScriptOnLoad(base.PublishmentSystemID, this.currentNodeID);
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        JsManager.RegisterClientScriptBlock(Page, "NodeTreeScriptOnLoad", onLoadScript);
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

            this.PlaceHolder_AddChannel.Visible = base.HasChannelPermissionsIgnoreNodeID(AppManager.CMS.Permission.Channel.ChannelAdd);
            if (this.PlaceHolder_AddChannel.Visible)
            {
                this.AddChannel1.Attributes.Add("onclick", Modal.ChannelAdd.GetOpenWindowString(base.PublishmentSystemID, base.PublishmentSystemID, ChannelLoading.GetRedirectUrl(base.PublishmentSystemID, base.PublishmentSystemID)));
                this.AddChannel2.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", BackgroundChannelAdd.GetRedirectUrl(base.PublishmentSystemID, base.PublishmentSystemID, PageUtils.GetCMSUrl(string.Format("background_channel.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)))));
            }

            this.PlaceHolder_ChannelEdit.Visible = base.HasChannelPermissionsIgnoreNodeID(AppManager.CMS.Permission.Channel.ChannelEdit);
            if (this.PlaceHolder_ChannelEdit.Visible)
            {
                showPopWinString = Modal.AddToGroup.GetOpenWindowStringToChannel(base.PublishmentSystemID);
                this.AddToGroup.Attributes.Add("onclick", showPopWinString);

                this.SelectEditColumns.Attributes.Add("onclick", Modal.SelectColumns.GetOpenWindowStringToChannel(base.PublishmentSystemID, false));
            }

            this.PlaceHolder_Translate.Visible = base.HasChannelPermissionsIgnoreNodeID(AppManager.CMS.Permission.Channel.ChannelTranslate);
            if (this.PlaceHolder_Translate.Visible)
            {
                this.Translate.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("background_channelTranslate.aspx?PublishmentSystemID={0}&ReturnUrl={1}", base.PublishmentSystemID, StringUtils.ValueToUrl(ChannelLoading.GetRedirectUrl(base.PublishmentSystemID, this.currentNodeID)))), "ChannelIDCollection", "ChannelIDCollection", "请选择需要转移的栏目！"));
            }

            this.PlaceHolder_Delete.Visible = base.HasChannelPermissionsIgnoreNodeID(AppManager.CMS.Permission.Channel.ChannelDelete);
            if (this.PlaceHolder_Delete.Visible)
            {
                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("background_channelDelete.aspx?PublishmentSystemID={0}&ReturnUrl={1}", base.PublishmentSystemID, StringUtils.ValueToUrl(ChannelLoading.GetRedirectUrl(base.PublishmentSystemID, base.PublishmentSystemID)))), "ChannelIDCollection", "ChannelIDCollection", "请选择需要删除的栏目！"));
            }

            this.PlaceHolder_Create.Visible = AdminUtility.HasWebsitePermissions(base.PublishmentSystemID, AppManager.CMS.Permission.WebSite.Create) 
 || base.HasChannelPermissionsIgnoreNodeID(AppManager.CMS.Permission.Channel.CreatePage);
            if (this.PlaceHolder_Create.Visible)
            {
                this.Create.Attributes.Add("onclick", Modal.CreateChannels.GetOpenWindowString(base.PublishmentSystemID));
            }

            this.PlaceHolder_Publish.Visible = base.PublishmentSystemInfo.Additional.IsSiteStorage && base.HasChannelPermissionsIgnoreNodeID(AppManager.CMS.Permission.Channel.PublishPage);

            if (this.PlaceHolder_Publish.Visible)
            {
                this.Publish.Attributes.Add("onclick", Modal.PublishPages.GetOpenWindowStringByChannels(base.PublishmentSystemID));
            }

            this.PlaceHolder_Import.Visible = this.PlaceHolder_AddChannel.Visible;
            if (this.PlaceHolder_Import.Visible)
            {
                this.Import.Attributes.Add("onclick", PageUtility.ModalSTL.ChannelImport_GetOpenWindowString(base.PublishmentSystemID, base.PublishmentSystemID));
            }
            this.Export.Attributes.Add("onclick", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToChannel(base.PublishmentSystemID, "ChannelIDCollection", "请选择需要导出的栏目！"));
        }

        public void BindGrid()
        {
            try
            {
                this.rptContents.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, 0);
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
            int nodeID = (int)e.Item.DataItem;
            bool enabled = (base.IsOwningNodeID(nodeID)) ? true : false;
            if (!enabled)
            {
                if (!base.IsHasChildOwningNodeID(nodeID)) e.Item.Visible = false;
            }
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = ChannelLoading.GetChannelRowHtml(base.PublishmentSystemInfo, nodeInfo, enabled, ELoadingType.Channel, null);
        }
    }
}
