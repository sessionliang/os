using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class CrossSiteTransEdit : BackgroundBasePage
    {
        protected DropDownList TransType;

        protected PlaceHolder PlaceHolder_PublishmentSystem;
        protected DropDownList PublishmentSystemIDCollection;
        protected ListBox NodeIDCollection;

        protected PlaceHolder PlaceHolder_NodeNames;
        protected TextBox NodeNames;

        protected PlaceHolder PlaceHolder_IsAutomatic;
        protected RadioButtonList IsAutomatic;

        private NodeInfo nodeInfo;

        protected RadioButtonList ddlTranslateDoneType;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID)
        {
            return ChannelLoading.GetCrossSiteTransEditOpenWindowString(publishmentSystemID, nodeID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID");
            int nodeID = int.Parse(base.GetQueryString("NodeID"));
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

            if (!IsPostBack)
            {
                ECrossSiteTransTypeUtils.AddAllListItems(this.TransType);

                ControlUtils.SelectListItems(this.TransType, ECrossSiteTransTypeUtils.GetValue(this.nodeInfo.Additional.TransType));

                TransType_OnSelectedIndexChanged(null, EventArgs.Empty);
                ControlUtils.SelectListItems(this.PublishmentSystemIDCollection, this.nodeInfo.Additional.TransPublishmentSystemID.ToString());


                PublishmentSystemIDCollection_OnSelectedIndexChanged(null, EventArgs.Empty);
                ControlUtils.SelectListItems(this.NodeIDCollection, TranslateUtils.StringCollectionToArrayList(this.nodeInfo.Additional.TransNodeIDs));
                this.NodeNames.Text = this.nodeInfo.Additional.TransNodeNames;

                EBooleanUtils.AddListItems(this.IsAutomatic, "自动", "提示");
                ControlUtils.SelectListItemsIgnoreCase(this.IsAutomatic, this.nodeInfo.Additional.TransIsAutomatic.ToString());

                ETranslateContentTypeUtils.AddListItems(this.ddlTranslateDoneType, false);
                ControlUtils.SelectListItems(this.ddlTranslateDoneType, ETranslateContentTypeUtils.GetValue(this.nodeInfo.Additional.TransDoneType));
            }
        }

        protected void TransType_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.PublishmentSystemIDCollection.Items.Clear();
            this.PublishmentSystemIDCollection.Enabled = true;

            this.PlaceHolder_IsAutomatic.Visible = false;

            ECrossSiteTransType contributeType = ECrossSiteTransTypeUtils.GetEnumType(this.TransType.SelectedValue);
            if (contributeType == ECrossSiteTransType.None)
            {
                this.PlaceHolder_PublishmentSystem.Visible = this.PlaceHolder_NodeNames.Visible = false;
            }
            else if (contributeType == ECrossSiteTransType.SelfSite || contributeType == ECrossSiteTransType.SpecifiedSite)
            {
                this.PlaceHolder_PublishmentSystem.Visible = true;
                this.PlaceHolder_NodeNames.Visible = false;

                this.PlaceHolder_IsAutomatic.Visible = true;
            }
            else if (contributeType == ECrossSiteTransType.ParentSite)
            {
                this.PlaceHolder_PublishmentSystem.Visible = true;
                this.PlaceHolder_NodeNames.Visible = false;
                this.PublishmentSystemIDCollection.Enabled = false;

                this.PlaceHolder_IsAutomatic.Visible = true;
            }
            else if (contributeType == ECrossSiteTransType.AllParentSite || contributeType == ECrossSiteTransType.AllSite)
            {
                this.PlaceHolder_PublishmentSystem.Visible = false;
                this.PlaceHolder_NodeNames.Visible = true;
            }

            if (this.PlaceHolder_PublishmentSystem.Visible)
            {
                ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();

                ArrayList allParentPublishmentSystemIDArrayList = new ArrayList();
                if (contributeType == ECrossSiteTransType.AllParentSite)
                {
                    PublishmentSystemManager.GetAllParentPublishmentSystemIDArrayList(allParentPublishmentSystemIDArrayList, publishmentSystemIDArrayList, base.PublishmentSystemID);
                }
                else if (contributeType == ECrossSiteTransType.SelfSite)
                {
                    publishmentSystemIDArrayList = new ArrayList();
                    publishmentSystemIDArrayList.Add(base.PublishmentSystemID);
                }

                foreach (int psID in publishmentSystemIDArrayList)
                {
                    PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(psID);
                    bool show = false;
                    if (contributeType == ECrossSiteTransType.SpecifiedSite)
                    {
                        show = true;
                    }
                    else if (contributeType == ECrossSiteTransType.SelfSite)
                    {
                        if (psID == base.PublishmentSystemID)
                        {
                            show = true;
                        }
                    }
                    else if (contributeType == ECrossSiteTransType.ParentSite)
                    {
                        if (psInfo.PublishmentSystemID == base.PublishmentSystemInfo.ParentPublishmentSystemID || (base.PublishmentSystemInfo.ParentPublishmentSystemID == 0 && psInfo.IsHeadquarters))
                        {
                            show = true;
                        }
                    }
                    if (show)
                    {
                        ListItem listitem = new ListItem(psInfo.PublishmentSystemName, psID.ToString());
                        if (psInfo.IsHeadquarters) listitem.Selected = true;
                        this.PublishmentSystemIDCollection.Items.Add(listitem);
                    }
                }
            }
            this.PublishmentSystemIDCollection_OnSelectedIndexChanged(sender, e);
        }

        protected void PublishmentSystemIDCollection_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.NodeIDCollection.Items.Clear();
            if (this.PlaceHolder_PublishmentSystem.Visible && this.PublishmentSystemIDCollection.Items.Count > 0)
            {
                NodeManager.AddListItemsForAddContent(this.NodeIDCollection.Items, PublishmentSystemManager.GetPublishmentSystemInfo(int.Parse(this.PublishmentSystemIDCollection.SelectedValue)), false);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            try
            {
                this.nodeInfo.Additional.TransType = ECrossSiteTransTypeUtils.GetEnumType(this.TransType.SelectedValue);
                if (this.nodeInfo.Additional.TransType == ECrossSiteTransType.SpecifiedSite)
                {
                    this.nodeInfo.Additional.TransPublishmentSystemID = TranslateUtils.ToInt(this.PublishmentSystemIDCollection.SelectedValue);
                }
                else
                {
                    this.nodeInfo.Additional.TransPublishmentSystemID = 0;
                }
                this.nodeInfo.Additional.TransNodeIDs = ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollection);
                this.nodeInfo.Additional.TransNodeNames = this.NodeNames.Text;

                this.nodeInfo.Additional.TransIsAutomatic = TranslateUtils.ToBool(this.IsAutomatic.SelectedValue);


                ETranslateContentType translateDoneType = ETranslateContentTypeUtils.GetEnumType(this.ddlTranslateDoneType.SelectedValue);
                this.nodeInfo.Additional.TransDoneType = translateDoneType;

                DataProvider.NodeDAO.UpdateNodeInfo(this.nodeInfo);

                StringUtility.AddLog(base.PublishmentSystemID, "修改跨站转发设置");

                isSuccess = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }

            if (isSuccess)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl(string.Format("background_configurationCrossSiteTrans.aspx?PublishmentSystemID={0}&CurrentNodeID={1}", base.PublishmentSystemID, this.nodeInfo.NodeID)));
            }
        }
    }
}
