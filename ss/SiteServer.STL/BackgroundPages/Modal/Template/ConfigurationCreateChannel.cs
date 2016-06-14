using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
    public class ConfigurationCreateChannel : BackgroundBasePage
	{
        public RadioButtonList IsCreateChannelIfContentChanged;

        protected ListBox NodeIDCollection;

		private int nodeID;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID)
        {
            return ChannelLoading.GetConfigurationCreateChannelOpenWindowString(publishmentSystemID, nodeID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID");
            this.nodeID = base.GetIntQueryString("NodeID");

			if (!IsPostBack)
			{
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                EBooleanUtils.AddListItems(this.IsCreateChannelIfContentChanged, "生成", "不生成");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateChannelIfContentChanged, nodeInfo.Additional.IsCreateChannelIfContentChanged.ToString());

                //NodeManager.AddListItemsForAddContent(this.NodeIDCollection.Items, base.PublishmentSystemInfo, false);
                NodeManager.AddListItemsForCreateChannel(this.NodeIDCollection.Items, base.PublishmentSystemInfo, false);
                ControlUtils.SelectListItems(this.NodeIDCollection, TranslateUtils.StringCollectionToArrayList(nodeInfo.Additional.CreateChannelIDsIfContentChanged));
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            try
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                nodeInfo.Additional.IsCreateChannelIfContentChanged = TranslateUtils.ToBool(this.IsCreateChannelIfContentChanged.SelectedValue);
                nodeInfo.Additional.CreateChannelIDsIfContentChanged = ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollection);

                DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

                StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "设置栏目变动生成页面", string.Format("栏目:{0}", nodeInfo.NodeName));
                isSuccess = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }

            if (isSuccess)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetCMSUrl(string.Format("background_configurationCreateTrigger.aspx?PublishmentSystemID={0}&CurrentNodeID={1}", base.PublishmentSystemID, this.nodeID)));
            }
        }
	}
}
