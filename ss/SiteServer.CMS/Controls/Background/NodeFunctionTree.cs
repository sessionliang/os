using System;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;
using System.Collections;

namespace SiteServer.CMS.Controls
{
    public class NodeFunctionTree : Control
    {
        private PublishmentSystemInfo publishmentSystemInfo;
        ELoadingType loadingType = ELoadingType.ContentTree;
        string rightPageURL = string.Empty;

        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();

            int publishmentSystemID = int.Parse(base.Page.Request.QueryString["PublishmentSystemID"]);
            this.loadingType = ELoadingTypeUtils.GetEnumType(base.Page.Request.QueryString["LoadingType"]);
            this.publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            string scripts = ChannelLoading.GetScript(this.publishmentSystemInfo, this.loadingType, null);
            builder.Append(scripts);
            if (base.Page.Request.QueryString["PublishmentSystemID"] != null)
            {
                this.rightPageURL = base.Page.Request.QueryString["RightPageURL"];
                try
                {
                    ArrayList nodeIDArrayList = new ArrayList();
                    if (ELoadingTypeUtils.Equals(ELoadingType.EvaluationNodeTree, loadingType))
                    {
                        ArrayList nodeList = DataProvider.NodeDAO.GetNodeIDByFunction(publishmentSystemID, "IsUseEvaluation".ToLower());
                        nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(this.publishmentSystemInfo.PublishmentSystemID, 0, nodeList);
                    }
                    else if (ELoadingTypeUtils.Equals(ELoadingType.TrialApplyNodeTree, loadingType) || ELoadingTypeUtils.Equals(ELoadingType.TrialReportNodeTree, loadingType) || ELoadingTypeUtils.Equals(ELoadingType.TrialAnalysisNodeTree, loadingType))
                    {
                        ArrayList nodeList = DataProvider.NodeDAO.GetNodeIDByFunction(publishmentSystemID, "IsUseTrial".ToLower());
                        nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(this.publishmentSystemInfo.PublishmentSystemID, 0, nodeList);
                    }
                    else if (ELoadingTypeUtils.Equals(ELoadingType.SurveyNodeTree, loadingType) || ELoadingTypeUtils.Equals(ELoadingType.SurveyAnalysisNodeTree, loadingType))
                    {
                        ArrayList nodeList = DataProvider.NodeDAO.GetNodeIDByFunction(publishmentSystemID, "IsUseSurvey".ToLower());
                        nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(this.publishmentSystemInfo.PublishmentSystemID, 0, nodeList);
                    }
                    else if (ELoadingTypeUtils.Equals(ELoadingType.CompareNodeTree, loadingType))
                    {
                        ArrayList nodeList = DataProvider.NodeDAO.GetNodeIDByFunction(publishmentSystemID, "IsUseCompare".ToLower());
                        nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(this.publishmentSystemInfo.PublishmentSystemID, 0, nodeList);
                    }
                    else
                    {
                        nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(this.publishmentSystemInfo.PublishmentSystemID, 0);
                    }
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.publishmentSystemInfo.PublishmentSystemID, nodeID);
                        bool enabled = (AdminUtility.IsOwningNodeID(nodeInfo.NodeID)) ? true : false;
                        if (!enabled)
                        {
                            if (!AdminUtility.IsHasChildOwningNodeID(nodeInfo.NodeID)) continue;
                        }

                        builder.Append(ChannelLoading.GetChannelRowHtml(this.publishmentSystemInfo, nodeInfo, enabled, this.loadingType, null));
                    }
                }
                catch (Exception ex)
                {
                    PageUtils.RedirectToErrorPage(ex.Message);
                }
            }
            writer.Write(builder);
        }
    }
}