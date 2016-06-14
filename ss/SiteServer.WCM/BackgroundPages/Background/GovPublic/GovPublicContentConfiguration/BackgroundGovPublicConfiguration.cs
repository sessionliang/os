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
    public class BackgroundGovPublicConfiguration : BackgroundGovPublicBasePage
	{
        public DropDownList ddlGovPublicNodeID;
        public RadioButtonList rblIsPublisherRelatedDepartmentID;

		public void Page_Load(object sender, EventArgs E)
		{
            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicContentConfiguration, "信息公开设置", AppManager.CMS.Permission.WebSite.GovPublicContentConfiguration);

                this.AddListItemsForGovPublic(this.ddlGovPublicNodeID.Items);
                ControlUtils.SelectListItems(this.ddlGovPublicNodeID, base.PublishmentSystemInfo.Additional.GovPublicNodeID.ToString());

                ControlUtils.SelectListItems(this.rblIsPublisherRelatedDepartmentID, base.PublishmentSystemInfo.Additional.GovPublicIsPublisherRelatedDepartmentID.ToString());
			}
		}

        private void AddListItemsForGovPublic(ListItemCollection listItemCollection)
        {
            ArrayList arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByScopeType(base.PublishmentSystemID, EScopeType.SelfAndChildren, string.Empty, string.Empty);
            int nodeCount = arraylist.Count;
            bool[] isLastNodeArray = new bool[nodeCount];
            foreach (int nodeID in arraylist)
            {
                bool enabled = true;
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                if (!EContentModelTypeUtils.Equals(EContentModelType.GovPublic, nodeInfo.ContentModelID))
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
                int nodeID = TranslateUtils.ToInt(this.ddlGovPublicNodeID.SelectedValue);
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                if (nodeInfo == null || !EContentModelTypeUtils.Equals(EContentModelType.GovPublic, nodeInfo.ContentModelID))
                {
                    this.ddlGovPublicNodeID.Items.Clear();
                    this.AddListItemsForGovPublic(this.ddlGovPublicNodeID.Items);
                    ControlUtils.SelectListItems(this.ddlGovPublicNodeID, base.PublishmentSystemInfo.Additional.GovPublicNodeID.ToString());

                    base.FailMessage("信息公开设置修改失败，主题分类根栏目必须选择信息公开类型栏目！");
                    return;
                }
                base.PublishmentSystemInfo.Additional.GovPublicNodeID = nodeID;
                base.PublishmentSystemInfo.Additional.GovPublicIsPublisherRelatedDepartmentID = TranslateUtils.ToBool(this.rblIsPublisherRelatedDepartmentID.SelectedValue);
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改信息公开设置");

                    base.SuccessMessage("信息公开设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "信息公开设置修改失败！");
				}
			}
		}
	}
}
