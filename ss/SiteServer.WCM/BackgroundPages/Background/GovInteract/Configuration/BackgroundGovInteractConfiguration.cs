using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using System.Web.UI.HtmlControls;


namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovInteractConfiguration : BackgroundGovInteractBasePage
	{
        public DropDownList ddlGovInteractNodeID;
        public RadioButtonList rblGovInteractApplyIsOpenWindow;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWCMUrl(string.Format("background_govInteractConfiguration.aspx?PublishmentSystemID={0}", publishmentSystemID));
        }

		public void Page_Load(object sender, EventArgs E)
		{
            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovInteract, AppManager.CMS.LeftMenu.GovInteract.ID_GovInteractConfiguration, "互动交流设置", AppManager.CMS.Permission.WebSite.GovInteractConfiguration);

                this.AddListItemsForGovInteract(this.ddlGovInteractNodeID.Items);
                ControlUtils.SelectListItems(this.ddlGovInteractNodeID, base.PublishmentSystemInfo.Additional.GovInteractNodeID.ToString());

                EBooleanUtils.AddListItems(this.rblGovInteractApplyIsOpenWindow);
                ControlUtils.SelectListItems(this.rblGovInteractApplyIsOpenWindow, base.PublishmentSystemInfo.Additional.GovInteractApplyIsOpenWindow.ToString());
			}
		}

        private void AddListItemsForGovInteract(ListItemCollection listItemCollection)
        {
            ArrayList arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(base.PublishmentSystemID, EScopeType.SelfAndChildren, string.Empty, string.Empty);
            int nodeCount = arraylist.Count;
            bool[] isLastNodeArray = new bool[nodeCount];
            foreach (int nodeID in arraylist)
            {
                bool enabled = true;
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                if (!EContentModelTypeUtils.Equals(EContentModelType.GovInteract, nodeInfo.ContentModelID))
                {
                    enabled = false;
                }

                ListItem listitem = new ListItem(NodeManager.GetSelectText(base.PublishmentSystemInfo, nodeInfo, isLastNodeArray, false, true), nodeInfo.NodeID.ToString());
                if (!enabled)
                {
                    listitem.Attributes.Add("style", "color:gray;");
                }
                listItemCollection.Add(listitem);
            }
        }

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                int nodeID = TranslateUtils.ToInt(this.ddlGovInteractNodeID.SelectedValue);
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                if (nodeInfo == null || !EContentModelTypeUtils.Equals(EContentModelType.GovInteract, nodeInfo.ContentModelID))
                {
                    this.ddlGovInteractNodeID.Items.Clear();
                    this.AddListItemsForGovInteract(this.ddlGovInteractNodeID.Items);
                    ControlUtils.SelectListItems(this.ddlGovInteractNodeID, base.PublishmentSystemInfo.Additional.GovInteractNodeID.ToString());

                    base.FailMessage("互动交流设置修改失败，根栏目必须选择互动交流类型栏目！");
                    return;
                }
                base.PublishmentSystemInfo.Additional.GovInteractNodeID = nodeID;
                base.PublishmentSystemInfo.Additional.GovInteractApplyIsOpenWindow = TranslateUtils.ToBool(this.rblGovInteractApplyIsOpenWindow.SelectedValue);
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改互动交流设置");

                    base.SuccessMessage("互动交流设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "互动交流设置修改失败！");
				}
			}
		}
	}
}
