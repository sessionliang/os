using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using System.Text;
using SiteServer.CMS.Core.Security;
using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class ChannelMultipleSelect : BackgroundBasePage
	{
        public PlaceHolder phPublishmentSystemID;
        public DropDownList ddlPublishmentSystemID;
        public Literal ltlChannelName;
        public Repeater rptChannel;

        private int targetPublishmentSystemID;
        private bool isPublishmentSystemSelect;
        private string jsMethod;

        public static string GetOpenWindowString(int publishmentSystemID, bool isPublishmentSystemSelect, string jsMethod)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("isPublishmentSystemSelect", isPublishmentSystemSelect.ToString());
            arguments.Add("jsMethod", jsMethod);
            return PageUtility.GetOpenWindowString("选择目标栏目", "modal_channelMultipleSelect.aspx", arguments, 600, 580, true);
        }

        public static string GetOpenWindowString(int publishmentSystemID, bool isPublishmentSystemSelect)
        {
            return GetOpenWindowString(publishmentSystemID, isPublishmentSystemSelect, "translateNodeAdd");
        }

        public string GetRedirectUrl(string targetPublishmentSystemID, string targetNodeID)
        {
            return PageUtils.GetCMSUrl(string.Format("modal_channelMultipleSelect.aspx?publishmentSystemID={0}&isPublishmentSystemSelect={1}&jsMethod={2}&targetPublishmentSystemID={3}&targetNodeID={4}", base.PublishmentSystemID, this.isPublishmentSystemSelect, this.jsMethod, targetPublishmentSystemID, targetNodeID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.isPublishmentSystemSelect = TranslateUtils.ToBool(base.GetQueryString("isPublishmentSystemSelect"));
            this.jsMethod = base.GetQueryString("jsMethod");

            this.targetPublishmentSystemID = TranslateUtils.ToInt(base.GetQueryString("TargetPublishmentSystemID"));
            if (this.targetPublishmentSystemID == 0)
            {
                this.targetPublishmentSystemID = base.PublishmentSystemID;
            }

            if (!IsPostBack)
            {
                this.phPublishmentSystemID.Visible = this.isPublishmentSystemSelect;

                List<int> publishmentSystemIDList = ProductPermissionsManager.Current.PublishmentSystemIDList;

                ArrayList mySystemInfoArrayList = new ArrayList();
                Hashtable parentWithChildren = new Hashtable();
                foreach (int publishmentSystemID in publishmentSystemIDList)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    if (publishmentSystemInfo.ParentPublishmentSystemID == 0)
                    {
                        mySystemInfoArrayList.Add(publishmentSystemInfo);
                    }
                    else
                    {
                        ArrayList children = new ArrayList();
                        if (parentWithChildren.Contains(publishmentSystemInfo.ParentPublishmentSystemID))
                        {
                            children = (ArrayList)parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID];
                        }
                        children.Add(publishmentSystemInfo);
                        parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID] = children;
                    }
                }
                foreach (PublishmentSystemInfo publishmentSystemInfo in mySystemInfoArrayList)
                {
                    AddSite(this.ddlPublishmentSystemID, publishmentSystemInfo, parentWithChildren, 0);
                }
                ControlUtils.SelectListItems(this.ddlPublishmentSystemID, this.targetPublishmentSystemID.ToString());

                if (!string.IsNullOrEmpty(base.GetQueryString("TargetNodeID")))
                {
                    int targetNodeID = TranslateUtils.ToInt(base.GetQueryString("TargetNodeID"));

                    string siteName = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID).PublishmentSystemName;
                    string nodeNames = NodeManager.GetNodeNameNavigation(targetPublishmentSystemID, targetNodeID);
                    if (targetPublishmentSystemID != base.PublishmentSystemID)
                    {
                        nodeNames = siteName + "：" + nodeNames;
                    }
                    string value = string.Format("{0}_{1}", targetPublishmentSystemID, targetNodeID);
                    if (!this.isPublishmentSystemSelect)
                    {
                        value = targetNodeID.ToString();
                    }
                    string scripts = string.Format("window.parent.{0}('{1}', '{2}');", this.jsMethod, nodeNames, value);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.targetPublishmentSystemID, this.targetPublishmentSystemID);
                    //string linkUrl = PageUtils.GetCMSUrl(string.Format("modal_channelMultipleSelect.aspx?PublishmentSystemID={0}&TargetPublishmentSystemID={1}&TargetNodeID={1}", base.PublishmentSystemID, this.targetPublishmentSystemID));
                    string linkUrl = this.GetRedirectUrl(this.targetPublishmentSystemID.ToString(), this.targetPublishmentSystemID.ToString());
                    this.ltlChannelName.Text = string.Format("<a href='{0}'>{1}</a>", linkUrl, nodeInfo.NodeName);

                    NameValueCollection additional = new NameValueCollection();
                    additional["linkUrl"] = this.GetRedirectUrl(this.targetPublishmentSystemID.ToString(), string.Empty);
                    JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(PublishmentSystemManager.GetPublishmentSystemInfo(this.targetPublishmentSystemID), ELoadingType.ChannelSelect, additional));

                    this.rptChannel.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(this.targetPublishmentSystemID, this.targetPublishmentSystemID);
                    this.rptChannel.ItemDataBound += new RepeaterItemEventHandler(rptChannel_ItemDataBound);
                    this.rptChannel.DataBind();
                }
            }
        }

        void rptChannel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int nodeID = (int)e.Item.DataItem;
            bool enabled = (base.IsOwningNodeID(nodeID)) ? true : false;
            if (!enabled)
            {
                if (!base.IsHasChildOwningNodeID(nodeID)) e.Item.Visible = false;
            }
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.targetPublishmentSystemID, nodeID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            NameValueCollection additional = new NameValueCollection();
            additional["linkUrl"] = this.GetRedirectUrl(this.targetPublishmentSystemID.ToString(), string.Empty);

            ltlHtml.Text = ChannelLoading.GetChannelRowHtml(base.PublishmentSystemInfo, nodeInfo, enabled, ELoadingType.ChannelSelect, additional);
        }

        public void PublishmentSystemID_OnSelectedIndexChanged(object sender, EventArgs E)
        {
            string redirectUrl = this.GetRedirectUrl(this.ddlPublishmentSystemID.SelectedValue, string.Empty);
            PageUtils.Redirect(redirectUrl);
        }

        private void AddSite(ListControl listControl, PublishmentSystemInfo publishmentSystemInfo, Hashtable parentWithChildren, int level)
        {
            string padding = string.Empty;
            for (int i = 0; i < level; i++)
            {
                padding += "　";
            }
            if (level > 0)
            {
                padding += "└ ";
            }

            if (parentWithChildren[publishmentSystemInfo.PublishmentSystemID] != null)
            {
                ArrayList children = (ArrayList)parentWithChildren[publishmentSystemInfo.PublishmentSystemID];

                ListItem listitem = new ListItem(padding + publishmentSystemInfo.PublishmentSystemName + string.Format("({0})", children.Count), publishmentSystemInfo.PublishmentSystemID.ToString());
                if (publishmentSystemInfo.PublishmentSystemID == base.PublishmentSystemID) listitem.Selected = true;

                listControl.Items.Add(listitem);
                level++;
                foreach (PublishmentSystemInfo subSiteInfo in children)
                {
                    AddSite(listControl, subSiteInfo, parentWithChildren, level);
                }
            }
            else
            {
                ListItem listitem = new ListItem(padding + publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemID.ToString());
                if (publishmentSystemInfo.PublishmentSystemID == base.PublishmentSystemID) listitem.Selected = true;

                listControl.Items.Add(listitem);
            }
        }
	}
}
