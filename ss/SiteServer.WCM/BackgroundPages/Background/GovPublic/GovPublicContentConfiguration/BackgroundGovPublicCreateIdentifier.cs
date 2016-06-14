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
    public class BackgroundGovPublicCreateIdentifier : BackgroundGovPublicBasePage
	{
        public DropDownList ddlNodeID;
        public RadioButtonList rblCreateType;

		public void Page_Load(object sender, EventArgs E)
		{
            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicContentConfiguration, "重新生成索引号", AppManager.CMS.Permission.WebSite.GovPublicContentConfiguration);

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, base.PublishmentSystemInfo.Additional.GovPublicNodeID);
                ListItem listItem = new ListItem("└" + nodeInfo.NodeName, base.PublishmentSystemInfo.Additional.GovPublicNodeID.ToString());
                this.ddlNodeID.Items.Add(listItem);

                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, base.PublishmentSystemInfo.Additional.GovPublicNodeID);
                int index = 1;
                foreach (int nodeID in nodeIDArrayList)
                {
                    nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                    listItem = new ListItem("　├" + nodeInfo.NodeName, nodeID.ToString());
                    if (index++ == nodeIDArrayList.Count)
                    {
                        listItem = new ListItem("　└" + nodeInfo.NodeName, nodeID.ToString());
                    }
                    if (!EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.GovPublic))
                    {
                        listItem.Attributes.Add("style", "color:gray;");
                        listItem.Value = "";
                    }
                    this.ddlNodeID.Items.Add(listItem);
                }

                listItem = new ListItem("索引号为空的信息", "Empty");
                listItem.Selected = true;
                this.rblCreateType.Items.Add(listItem);
                listItem = new ListItem("全部信息", "All");
                listItem.Selected = false;
                this.rblCreateType.Items.Add(listItem);
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
		{
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                int nodeID = TranslateUtils.ToInt(this.ddlNodeID.SelectedValue);
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                if (nodeInfo == null || !EContentModelTypeUtils.Equals(EContentModelType.GovPublic, nodeInfo.ContentModelID))
                {
                    base.FailMessage("索引号生成失败，所选栏目必须为信息公开类型栏目！");
                    return;
                }

                try
                {
                    bool isAll = StringUtils.EqualsIgnoreCase(this.rblCreateType.SelectedValue, "All");
                    DataProvider.GovPublicContentDAO.CreateIdentifier(base.PublishmentSystemInfo, nodeID, isAll);

                    StringUtility.AddLog(base.PublishmentSystemID, "重新生成索引号");

                    base.SuccessMessage("索引号重新生成成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "索引号重新生成失败！");
                }
            }
		}
	}
}
