using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicContentAddAfter : BackgroundGovPublicBasePage
	{
        public RadioButtonList Operation;
        public DropDownList PublishmentSystemIDDropDownList;
        public ListBox NodeIDListBox;
        public Control PublishmentSystemIDRow;
        public Control NodeIDDropDownListRow;
        public Button Submit;

        private NodeInfo nodeInfo;
        private int contentID;
        private string returnUrl;

        public enum EContentAddAfter
        {
            ContinueAdd,
            ManageContents,
            Contribute
        }

		public void Page_Load(object sender, EventArgs E)
		{
			PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ContentID", "ReturnUrl");
			int nodeID = int.Parse(base.Request.QueryString["NodeID"]);
            this.contentID = int.Parse(base.Request.QueryString["ContentID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundContent.GetRedirectUrl(base.PublishmentSystemID, nodeID);
            }

            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicContent, "后续操作", AppManager.CMS.Permission.WebSite.GovPublicContent);

                this.Operation.Items.Add(new ListItem("继续添加内容", EContentAddAfter.ContinueAdd.ToString()));
                this.Operation.Items.Add(new ListItem("返回管理界面", EContentAddAfter.ManageContents.ToString()));
                bool isContribute = CrossSiteTransUtility.IsTranslatable(base.PublishmentSystemInfo, this.nodeInfo);
                bool isTransOk = false;
                if (isContribute)
                {
                    bool isAutomatic = CrossSiteTransUtility.IsAutomatic(this.nodeInfo);
                    if (isAutomatic)
                    {
                        int targetPublishmentSystemID = 0;

                        if (this.nodeInfo.Additional.TransType == ECrossSiteTransType.SpecifiedSite)
                        {
                            targetPublishmentSystemID = this.nodeInfo.Additional.TransPublishmentSystemID;
                        }
                        else if (this.nodeInfo.Additional.TransType == ECrossSiteTransType.SelfSite)
                        {
                            targetPublishmentSystemID = base.PublishmentSystemID;
                        }
                        else if (this.nodeInfo.Additional.TransType == ECrossSiteTransType.ParentSite)
                        {
                            targetPublishmentSystemID = PublishmentSystemManager.GetParentPublishmentSystemID(base.PublishmentSystemID);
                        }

                        if (targetPublishmentSystemID > 0)
                        {
                            PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);
                            if (targetPublishmentSystemInfo != null)
                            {
                                ArrayList targetNodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(this.nodeInfo.Additional.TransNodeIDs);
                                if (targetNodeIDArrayList.Count > 0)
                                {
                                    foreach (int targetNodeID in targetNodeIDArrayList)
                                    {
                                        CrossSiteTransUtility.TransContentInfo(base.PublishmentSystemInfo, this.nodeInfo, this.contentID, targetPublishmentSystemInfo, targetNodeID);
                                        isTransOk = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        this.Operation.Items.Add(new ListItem("转发到其他应用", EContentAddAfter.Contribute.ToString()));
                    }
                }
                if (isTransOk)
                {
                    base.SuccessMessage("内容添加成功并已转发到指定应用，请选择后续操作。");
                }
                else
                {
                    base.SuccessMessage("内容添加成功，请选择后续操作。");
                }
                
                this.PublishmentSystemIDRow.Visible = this.NodeIDDropDownListRow.Visible = this.Submit.Visible = false;
			}
		}

        public void Operation_SelectedIndexChanged(object sender, EventArgs e)
		{
            EContentAddAfter after = (EContentAddAfter)TranslateUtils.ToEnum(typeof(EContentAddAfter), this.Operation.SelectedValue, EContentAddAfter.ContinueAdd);
            if (after == EContentAddAfter.ContinueAdd)
            {
                PageUtils.Redirect(BackgroundGovPublicContentAdd.GetRedirectUrlOfAdd(base.PublishmentSystemID, this.nodeInfo.NodeID, base.Request.QueryString["ReturnUrl"]));
            }
            else if (after == EContentAddAfter.ManageContents)
            {
                PageUtils.Redirect(this.returnUrl);
            }
            else if (after == EContentAddAfter.Contribute)
            {
                CrossSiteTransUtility.LoadPublishmentSystemIDDropDownList(this.PublishmentSystemIDDropDownList, base.PublishmentSystemInfo, this.nodeInfo.NodeID);

                if (this.PublishmentSystemIDDropDownList.Items.Count > 0)
                {
                    PublishmentSystemID_SelectedIndexChanged(sender, e);
                }
                this.PublishmentSystemIDRow.Visible = this.NodeIDDropDownListRow.Visible = this.Submit.Visible = true;
            }
		}

        public void PublishmentSystemID_SelectedIndexChanged(object sender, EventArgs E)
        {
            int psID = int.Parse(this.PublishmentSystemIDDropDownList.SelectedValue);
            CrossSiteTransUtility.LoadNodeIDListBox(this.NodeIDListBox, base.PublishmentSystemInfo, psID, this.nodeInfo);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                int targetPublishmentSystemID = int.Parse(this.PublishmentSystemIDDropDownList.SelectedValue);
                PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);
                try
                {
                    foreach (ListItem listItem in this.NodeIDListBox.Items)
                    {
                        if (listItem.Selected)
                        {
                            int targetNodeID = TranslateUtils.ToInt(listItem.Value);
                            if (targetNodeID != 0)
                            {
                                CrossSiteTransUtility.TransContentInfo(base.PublishmentSystemInfo, this.nodeInfo, this.contentID, targetPublishmentSystemInfo, targetNodeID);
                            }
                        }
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeInfo.NodeID, this.contentID, "内容跨站转发", string.Format("转发到应用:{0}", targetPublishmentSystemInfo.PublishmentSystemName));

                    base.SuccessMessage("内容跨站转发成功，请选择后续操作。");
                    this.Operation.Items.Clear();
                    this.Operation.Items.Add(new ListItem("继续添加内容", EContentAddAfter.ContinueAdd.ToString()));
                    this.Operation.Items.Add(new ListItem("返回管理界面", EContentAddAfter.ManageContents.ToString()));
                    this.Operation.Items.Add(new ListItem("转发到其他应用", EContentAddAfter.Contribute.ToString()));
                    this.PublishmentSystemIDRow.Visible = this.NodeIDDropDownListRow.Visible = this.Submit.Visible = false;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "内容跨站转发失败！");
                }
            }
        }
	}
}
