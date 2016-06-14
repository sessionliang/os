using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using System.Collections;
using System.Web.UI.HtmlControls;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundConfiguration : BackgroundBasePage
    {
        public DropDownList ddlIsVirtualGoods;

        //品牌
        public PlaceHolder phBrandNode;
        public DropDownList ddlIsUseBrandNode;
        public DropDownList ddlBrandNode;
        private bool[] isLastNodeArray;

        private int nodeID;
        private NodeInfo currentNode;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.nodeID = base.GetIntQueryString("NodeID");

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID");
            currentNode = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
            base.BreadCrumb(AppManager.B2C.LeftMenu.ID_ConfigrationB2C, "商品设置", AppManager.B2C.Permission.WebSite.Configration);

            if (!IsPostBack)
            {
                EBooleanUtils.AddListItems(this.ddlIsVirtualGoods, "虚拟商品", "实体商品");
                ControlUtils.SelectListItems(this.ddlIsVirtualGoods, B2CConfigurationManager.GetInstance(this.nodeID).IsVirtualGoods.ToString());



                EBooleanUtils.AddListItems(this.ddlIsUseBrandNode, "使用品牌栏目", "不使用品牌栏目");
                ControlUtils.SelectListItems(this.ddlIsUseBrandNode, currentNode.Additional.IsBrandSpecified.ToString());

                this.phBrandNode.Visible = currentNode.Additional.IsBrandSpecified;

                //品牌
                ddlBrandNode.Items.Add(new ListItem("<<请选择>>", string.Empty));
                int nodeID = DataProvider.NodeDAO.GetNodeIDByContentModelType(base.PublishmentSystemID, EContentModelType.Brand);
                string parentNodeIDPaths = NodeManager.GetParentsPath(base.PublishmentSystemID, nodeID);
                ArrayList nodeInfoArrayList = DataProvider.NodeDAO.GetNodeInfoArrayListByParentID(base.PublishmentSystemID, nodeID);
                ArrayList nodeIDArrayList = new ArrayList();
                nodeIDArrayList.AddRange(TranslateUtils.StringCollectionToIntArrayList(parentNodeIDPaths));
                nodeIDArrayList.Add(nodeID);
                //nodeInfoArrayList.Add(NodeManager.GetNodeInfo(base.PublishmentSystemID, base.PublishmentSystemID));
                //nodeInfoArrayList.Add(NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID));
                foreach (NodeInfo nodeInfo in nodeInfoArrayList)
                {
                    if (nodeInfo == null)
                        continue;
                    if (EContentModelTypeUtils.Equals(nodeInfo.ContentModelID, EContentModelType.Brand) && !nodeIDArrayList.Contains(nodeInfo.NodeID))
                    {
                        nodeIDArrayList.Add(nodeInfo.NodeID);
                    }
                    else if (nodeIDArrayList.Contains(nodeInfo.NodeID))
                    {
                        nodeIDArrayList.Remove(nodeInfo.NodeID);
                    }
                }
                int nodeCount = nodeIDArrayList.Count;
                this.isLastNodeArray = new bool[nodeCount];

                foreach (int theNodeID in nodeIDArrayList)
                {
                    bool enabled = base.IsOwningNodeID(theNodeID);
                    if (!enabled)
                    {
                        if (!base.IsHasChildOwningNodeID(theNodeID)) continue;
                    }
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, theNodeID);

                    string value = enabled ? nodeInfo.NodeID.ToString() : string.Empty;
                    value = (nodeInfo.Additional.IsContentAddable) ? value : string.Empty;

                    string text = this.GetTitle(nodeInfo);
                    ListItem listItem = new ListItem(text, value);
                    if (currentNode.Additional.IsBrandSpecified && currentNode.Additional.BrandNodeID.ToString() == value)
                    {
                        listItem.Selected = true;
                    }
                    ddlBrandNode.Items.Add(listItem);
                }
            }
        }

        public string GetTitle(NodeInfo nodeInfo)
        {
            string str = "";
            if (nodeInfo.NodeID == base.PublishmentSystemID)
            {
                nodeInfo.IsLastNode = true;
            }
            if (nodeInfo.IsLastNode == false)
            {
                this.isLastNodeArray[nodeInfo.ParentsCount] = false;
            }
            else
            {
                this.isLastNodeArray[nodeInfo.ParentsCount] = true;
            }
            for (int i = 0; i < nodeInfo.ParentsCount; i++)
            {
                if (this.isLastNodeArray[i])
                {
                    str = String.Concat(str, "　");
                }
                else
                {
                    str = String.Concat(str, "│");
                }
            }
            if (nodeInfo.IsLastNode)
            {
                str = String.Concat(str, "└");
            }
            else
            {
                str = String.Concat(str, "├");
            }
            str = String.Concat(str, nodeInfo.NodeName);
            if (nodeInfo.ContentNum != 0)
            {
                str = string.Format("{0} ({1})", str, nodeInfo.ContentNum);
            }
            return str;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                B2CConfigurationInfo configurationInfo = B2CConfigurationManager.GetInstance(this.nodeID);
                configurationInfo.IsVirtualGoods = TranslateUtils.ToBool(this.ddlIsVirtualGoods.SelectedValue);

                NodeInfo currentNodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                if (!string.IsNullOrEmpty(this.ddlBrandNode.SelectedValue))
                {
                    currentNodeInfo.Additional.BrandNodeID = TranslateUtils.ToInt(this.ddlBrandNode.SelectedValue);
                }

                currentNodeInfo.Additional.IsBrandSpecified = TranslateUtils.ToBool(this.ddlIsUseBrandNode.SelectedValue);

                try
                {
                    DataProviderB2C.B2CConfigurationDAO.Update(configurationInfo);
                    DataProvider.NodeDAO.UpdateNodeInfo(currentNodeInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改商品设置");

                    base.SuccessMessage("商品设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "商品设置修改失败！");
                }
            }
        }

        protected void ddlIsUseBrandNode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TranslateUtils.ToBool(this.ddlIsUseBrandNode.SelectedValue))
            {
                this.phBrandNode.Visible = true;
            }
            else
            {
                this.phBrandNode.Visible = false;
            }
        }
    }
}
