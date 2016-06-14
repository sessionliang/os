using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using System.Text;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Model.MLib;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class ContentReferenceSelect : BackgroundBasePage
    {
        public DropDownList ddlPublishmentSystemID;
        public Literal ltlChannelName;
        public Repeater rptChannel;
        public RadioButtonList rblThirdPlatform;

        private int targetPublishmentSystemID;
        private PublishmentSystemInfo targetPublishmentSystemInfo;
        private string extParam = "";

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, int contentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ID", contentID.ToString());

            return JsUtils.OpenWindow.GetOpenWindowString("选择目标栏目", "modal_contentReferenceSelect.aspx", arguments, 600, 580, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.targetPublishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["TargetPublishmentSystemID"]);
            if (this.targetPublishmentSystemID == 0)
            {
                this.targetPublishmentSystemID = base.PublishmentSystemID;
            }

            this.targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);

            if (!string.IsNullOrEmpty(Request.QueryString["NodeID"]))
            {
                extParam += "&NodeID=" + Request.QueryString["NodeID"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                extParam += "&ID=" + Request.QueryString["ID"];
            }

            if (!IsPostBack)
            {
                ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();

                ArrayList mySystemInfoArrayList = new ArrayList();
                Hashtable parentWithChildren = new Hashtable();
                foreach (int publishmentSystemID in publishmentSystemIDArrayList)
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

                if (!string.IsNullOrEmpty(base.Request.QueryString["TargetNodeID"]))
                {
                    int targetNodeID = TranslateUtils.ToInt(base.Request.QueryString["TargetNodeID"]);

                    string siteName = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID).PublishmentSystemName;
                    string nodeNames = NodeManager.GetNodeNameNavigation(targetPublishmentSystemID, targetNodeID);
                    if (targetPublishmentSystemID != base.PublishmentSystemID)
                    {
                        nodeNames = siteName + "：" + nodeNames;
                    }
                    string value = string.Format("{0}_{1}", targetPublishmentSystemID, targetNodeID);
                    string scripts = "alert('引用成功!');";
                    ContentUtility.Translate(base.PublishmentSystemInfo, string.Format("{0}_{1}", Request.QueryString["NodeID"], Request.QueryString["ID"]), value, ETranslateContentType.Copy);

                    #region 引用记录
                    int nodeID = TranslateUtils.ToInt(Request.QueryString["NodeID"]);
                    int contentID = TranslateUtils.ToInt(Request.QueryString["ID"]);

                    var nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                    var tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
                    var tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);

                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                    
                    ReferenceLog referenceLogInfo = new ReferenceLog();
                    referenceLogInfo.PublishmentSystemID = targetPublishmentSystemID;
                    referenceLogInfo.NodeID = targetNodeID;
                    referenceLogInfo.Operator = AdminManager.Current.UserName;
                    referenceLogInfo.OperateDate = DateTime.Now;
                    referenceLogInfo.SubmissionID = contentInfo.ReferenceID;
                    DataProvider.MlibDAO.InsertReferenceLogs(referenceLogInfo);

                    #endregion 



                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.targetPublishmentSystemID, this.targetPublishmentSystemID);
                    string linkUrl = string.Format("modal_contentReferenceSelect.aspx?PublishmentSystemID={0}&TargetPublishmentSystemID={1}&TargetNodeID={1}", base.PublishmentSystemID + extParam, this.targetPublishmentSystemID);
                    this.ltlChannelName.Text = string.Format("<a href='{0}'>{1}</a>", linkUrl, nodeInfo.NodeName);

                    NameValueCollection additional = new NameValueCollection();
                    additional["linkUrl"] = string.Format("modal_contentReferenceSelect.aspx?PublishmentSystemID={0}&TargetPublishmentSystemID={1}&TargetNodeID=", base.PublishmentSystemID + extParam, this.targetPublishmentSystemID);

                    JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(this.targetPublishmentSystemInfo, ELoadingType.ChannelSelect, additional));

                    this.rptChannel.DataSource = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(this.targetPublishmentSystemID, this.targetPublishmentSystemID);
                    this.rptChannel.ItemDataBound += new RepeaterItemEventHandler(rptChannel_ItemDataBound);
                    this.rptChannel.DataBind();
                }



                #region 第三方平台
                var rtDs = DataProvider.MlibDAO.GetReferenceTypeList();
                rblThirdPlatform.DataSource = rtDs.Tables[0];
                rblThirdPlatform.DataTextField = "RTName";
                rblThirdPlatform.DataValueField = "RTID";
                rblThirdPlatform.DataBind();
                rblThirdPlatform.SelectedIndex = 0;
                #endregion
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
            additional["linkUrl"] = string.Format("modal_contentReferenceSelect.aspx?PublishmentSystemID={0}&TargetPublishmentSystemID={1}&TargetNodeID=", base.PublishmentSystemID + extParam, this.targetPublishmentSystemID);

            ltlHtml.Text = ChannelLoading.GetChannelRowHtml(base.PublishmentSystemInfo, nodeInfo, enabled, ELoadingType.ChannelSelect, additional);
        }

        public void PublishmentSystemID_OnSelectedIndexChanged(object sender, EventArgs E)
        {
            string redirectUrl = string.Format("modal_contentReferenceSelect.aspx?PublishmentSystemID={0}&TargetPublishmentSystemID={1}", base.PublishmentSystemID + extParam, this.ddlPublishmentSystemID.SelectedValue);
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


        public override void Submit_OnClick(object sender, EventArgs E)
        {
            var rtid = rblThirdPlatform.SelectedValue;
            int nodeID = TranslateUtils.ToInt(Request.QueryString["NodeID"]);
            int contentID = TranslateUtils.ToInt(Request.QueryString["ID"]);

            var nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            var tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            var tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            
            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
            

            ReferenceLog referenceLogInfo = new ReferenceLog();
            referenceLogInfo.RTID =TranslateUtils.ToInt( rtid);
            referenceLogInfo.Operator = AdminManager.Current.UserName;
            referenceLogInfo.OperateDate = DateTime.Now;
            referenceLogInfo.SubmissionID = contentInfo.ReferenceID;
            DataProvider.MlibDAO.InsertReferenceLogs(referenceLogInfo);


            JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, string.Format("parent.location.href='ReviewShow.aspx?PublishmentSystemID={0}&nodeid={1}&id={2}'",base.PublishmentSystemID, nodeID,contentID));

        }
    }
}
