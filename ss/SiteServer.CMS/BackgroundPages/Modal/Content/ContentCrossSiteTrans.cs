using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class ContentCrossSiteTrans : BackgroundBasePage
	{
	    protected DropDownList PublishmentSystemIDDropDownList;
        protected ListBox NodeIDListBox;

        private int nodeID;
	    private ArrayList contentIDArrayList;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("转发所选内容", "modal_contentCrossSiteTrans.aspx", arguments, "ContentIDCollection", "请选择需要转发的内容！", 400, 390);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ContentIDCollection");

            this.nodeID = base.GetIntQueryString("NodeID");
            this.contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));

			if (!IsPostBack)
			{
                CrossSiteTransUtility.LoadPublishmentSystemIDDropDownList(this.PublishmentSystemIDDropDownList, base.PublishmentSystemInfo, this.nodeID);

                if (this.PublishmentSystemIDDropDownList.Items.Count > 0)
                {
                    PublishmentSystemID_SelectedIndexChanged(null, EventArgs.Empty);
                }

				
			}
		}

        public void PublishmentSystemID_SelectedIndexChanged(object sender, EventArgs E)
        {
            int psID = int.Parse(this.PublishmentSystemIDDropDownList.SelectedValue);
            CrossSiteTransUtility.LoadNodeIDListBox(this.NodeIDListBox, base.PublishmentSystemInfo, psID,
                                                     NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID));
        }

        public override void Submit_OnClick(object sender, EventArgs e)
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
                            ETableStyle targetTableStyle = NodeManager.GetTableStyle(targetPublishmentSystemInfo, targetNodeID);
                            string targetTableName = NodeManager.GetTableName(targetPublishmentSystemInfo, targetNodeID);
                            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeID);
                            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeID);
                            foreach (int contentID in contentIDArrayList)
                            {
                                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                                FileUtility.MoveFileByContentInfo(base.PublishmentSystemInfo, targetPublishmentSystemInfo, contentInfo as BackgroundContentInfo);
                                contentInfo.PublishmentSystemID = targetPublishmentSystemID;
                                contentInfo.SourceID = contentInfo.NodeID;
                                contentInfo.NodeID = targetNodeID;
                                
                                if (targetPublishmentSystemInfo.Additional.IsCrossSiteTransChecked)
                                {
                                    contentInfo.IsChecked = true;
                                }
                                else
                                {
                                    contentInfo.IsChecked = false;
                                }
                                contentInfo.CheckedLevel = 0;

                                DataProvider.ContentDAO.Insert(targetTableName, targetPublishmentSystemInfo, contentInfo);
                            }
                        }
                    }
                }

                StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "跨站转发内容", string.Empty);

                base.SuccessMessage("内容转发成功，请选择后续操作。");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "内容转发失败！");
            }
            
            JsUtils.OpenWindow.CloseModalPage(Page);
		}

	}
}
