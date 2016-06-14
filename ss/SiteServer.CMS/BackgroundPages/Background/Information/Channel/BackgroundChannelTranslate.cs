using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Text;

using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundChannelTranslate : BackgroundBasePage
	{
        public ListBox NodeIDFrom;
		public DropDownList NodeIDTo;
		public DropDownList TranslateType;
        public RadioButtonList IsIncludeDesendents;
		public RadioButtonList IsDeleteAfterTranslate;

		public DropDownList PublishmentSystemIDDropDownList;

        public PlaceHolder phReturn;
        public Button Submit;

		private bool[] isLastNodeArray;
		public string SystemKeyword;
        private string returnUrl;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));

            if (!base.HasChannelPermissions(base.PublishmentSystemID, AppManager.CMS.Permission.Channel.ContentDelete))
			{
				IsDeleteAfterTranslate.Visible = false;
			}
			
			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "批量转移", string.Empty);

                this.phReturn.Visible = !string.IsNullOrEmpty(this.returnUrl);
				ETranslateTypeUtils.AddListItems(TranslateType);
                if (!string.IsNullOrEmpty(base.GetQueryString("ChannelIDCollection")))
                {
                    ControlUtils.SelectListItems(TranslateType, ETranslateTypeUtils.GetValue(ETranslateType.All));
                }
                else
                {
                    ControlUtils.SelectListItems(TranslateType, ETranslateTypeUtils.GetValue(ETranslateType.Content));
                }

				IsDeleteAfterTranslate.Items[0].Value = true.ToString();
				IsDeleteAfterTranslate.Items[1].Value = false.ToString();

                List<int> publishmentSystemIDList = ProductPermissionsManager.Current.PublishmentSystemIDList;
                foreach (int psID in publishmentSystemIDList)
				{
					PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(psID);
                    ListItem listitem = new ListItem(psInfo.PublishmentSystemName, psID.ToString());
                    if (psID == base.PublishmentSystemID) listitem.Selected = true;
                    PublishmentSystemIDDropDownList.Items.Add(listitem);
				}

                ArrayList nodeIDStrArrayList = new ArrayList();
                if (!string.IsNullOrEmpty(base.GetQueryString("ChannelIDCollection")))
                {
                    nodeIDStrArrayList = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ChannelIDCollection"));
                }

				ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
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
                    if (nodeIDStrArrayList.Contains(value))
                    {
                        listItem.Selected = true;
                    }
                    NodeIDFrom.Items.Add(listItem);
                    listItem = new ListItem(text, value);
                    NodeIDTo.Items.Add(listItem);
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
			if (base.Page.IsPostBack)
			{
				int targetNodeID = int.Parse(NodeIDTo.SelectedValue);

				int targetPublishmentSystemID = int.Parse(this.PublishmentSystemIDDropDownList.SelectedValue);
				PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);
				bool isChecked = false;
				int checkedLevel = 0;
                if (targetPublishmentSystemInfo.CheckContentLevel == 0 || AdminUtility.HasChannelPermissions(targetPublishmentSystemID, targetNodeID, AppManager.CMS.Permission.Channel.ContentAdd, AppManager.CMS.Permission.Channel.ContentCheck))
				{
					isChecked = true;
					checkedLevel = 0;
				}
				else
				{
					int UserCheckLevel = 0;
					bool OwnHighestLevel = false;

                    if (AdminUtility.HasChannelPermissions(targetPublishmentSystemID, targetNodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel1))
                    {
                        UserCheckLevel = 1;
                        if (AdminUtility.HasChannelPermissions(targetPublishmentSystemID, targetNodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel2))
                        {
                            UserCheckLevel = 2;
                            if (AdminUtility.HasChannelPermissions(targetPublishmentSystemID, targetNodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel3))
                            {
                                UserCheckLevel = 3;
                                if (AdminUtility.HasChannelPermissions(targetPublishmentSystemID, targetNodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel4))
                                {
                                    UserCheckLevel = 4;
                                    if (AdminUtility.HasChannelPermissions(targetPublishmentSystemID, targetNodeID, AppManager.CMS.Permission.Channel.ContentCheckLevel5))
                                    {
                                        UserCheckLevel = 5;
                                    }
                                }
                            }
                        }
                    }

                    if (UserCheckLevel >= targetPublishmentSystemInfo.CheckContentLevel)
					{
						OwnHighestLevel = true;
					}
					if (OwnHighestLevel)
					{
						isChecked = true;
						checkedLevel = 0;
					}
					else
					{
						isChecked = false;
						checkedLevel = UserCheckLevel;
					}
				}

				try
				{
                    ETranslateType translateType = ETranslateTypeUtils.GetEnumType(this.TranslateType.SelectedValue);

                    ArrayList nodeIDStrArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.NodeIDFrom);

                    ArrayList nodeIDArrayList = new ArrayList();//需要转移的栏目ID
                    foreach (string nodeIDStr in nodeIDStrArrayList)
                    {
                        int nodeID = int.Parse(nodeIDStr);
                        if (translateType != ETranslateType.Content)//需要转移栏目
                        {
                            if (!ChannelUtility.IsAncestorOrSelf(base.PublishmentSystemID, nodeID, targetNodeID))
                            {
                                nodeIDArrayList.Add(nodeID);
                            }
                        }

                        if (translateType == ETranslateType.Content)//转移内容
                        {
                            this.TranslateContent(targetPublishmentSystemInfo, nodeID, targetNodeID, isChecked, checkedLevel);
                        }
                    }

                    if (translateType != ETranslateType.Content)//需要转移栏目
                    {
                        ArrayList nodeIDArrayListToTranslate = new ArrayList(nodeIDArrayList);
                        foreach (int nodeID in nodeIDArrayList)
                        {
                            ArrayList subNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(nodeID);
                            if (subNodeIDArrayList != null && subNodeIDArrayList.Count > 0)
                            {
                                foreach (int nodeIDToDelete in subNodeIDArrayList)
                                {
                                    if (nodeIDArrayListToTranslate.Contains(nodeIDToDelete))
                                    {
                                        nodeIDArrayListToTranslate.Remove(nodeIDToDelete);
                                    }
                                }
                            }
                        }

                        ArrayList nodeInfoArrayList = new ArrayList();
                        foreach (int nodeID in nodeIDArrayListToTranslate)
                        {
                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                            nodeInfoArrayList.Add(nodeInfo);
                        }

                        this.TranslateChannelAndContent(nodeInfoArrayList, targetPublishmentSystemID, targetNodeID, translateType, isChecked, checkedLevel, null, null);

                        if (IsDeleteAfterTranslate.Visible && EBooleanUtils.Equals(IsDeleteAfterTranslate.SelectedValue, EBoolean.True))
                        {
                            foreach (int nodeID in nodeIDArrayListToTranslate)
                            {
                                try
                                {
                                    DataProvider.NodeDAO.Delete(nodeID);
                                }
                                catch { }
                            }
                        }
                    }
                    this.Submit.Enabled = false;

                    StringBuilder builder = new StringBuilder();
                    foreach (ListItem listItem in this.NodeIDFrom.Items)
                    {
                        if (listItem.Selected)
                        {
                            builder.Append(listItem.Text).Append(",");
                        }
                    }
                    if (builder.Length > 0)
                    {
                        builder.Length = builder.Length - 1;
                    }
                    StringUtility.AddLog(base.PublishmentSystemID, "批量转移", string.Format("栏目:{0},转移后删除:{1}", builder.ToString(), this.IsDeleteAfterTranslate.SelectedValue));

					base.SuccessMessage("批量转移成功！");
                    if (!string.IsNullOrEmpty(base.GetQueryString("ChannelIDCollection")))
                    {
                        PageUtils.Redirect(this.returnUrl);
                    }
                    else
                    {
                        PageUtils.Redirect(PageUtils.GetCMSUrl("background_channelTranslate.aspx?PublishmentSystemID=" + base.PublishmentSystemID));
                    }
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "批量转移失败！");
                    LogUtils.AddErrorLog(ex);
				}

			}
		}

		private void TranslateChannelAndContent(ArrayList nodeInfoArrayList, int targetPublishmentSystemID, int parentID, ETranslateType translateType, bool isChecked, int checkedLevel, ArrayList nodeIndexNameArrayList, ArrayList filePathArrayList)
		{
			if (nodeInfoArrayList == null || nodeInfoArrayList.Count == 0)
			{
				return;
			}

			if (nodeIndexNameArrayList == null)
			{
				nodeIndexNameArrayList = DataProvider.NodeDAO.GetNodeIndexNameArrayList(targetPublishmentSystemID);
			}

            if (filePathArrayList == null)
			{
                filePathArrayList = DataProvider.NodeDAO.GetAllFilePathByPublishmentSystemID(targetPublishmentSystemID);
			}

            PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);

			foreach (NodeInfo oldNodeInfo in nodeInfoArrayList)
			{
                NodeInfo nodeInfo = new NodeInfo(oldNodeInfo);
				nodeInfo.PublishmentSystemID = targetPublishmentSystemID;
				nodeInfo.ParentID = parentID;
				nodeInfo.ContentNum = 0;
				nodeInfo.ChildrenCount = 0;
                
				nodeInfo.AddDate = DateTime.Now;
                if (IsDeleteAfterTranslate.Visible && EBooleanUtils.Equals(IsDeleteAfterTranslate.SelectedValue, EBoolean.True))
                {
                    nodeIndexNameArrayList.Add(nodeInfo.NodeIndexName);
                }
               
                else if (!string.IsNullOrEmpty(nodeInfo.NodeIndexName) && nodeIndexNameArrayList.IndexOf(nodeInfo.NodeIndexName) == -1)
                {
                    nodeIndexNameArrayList.Add(nodeInfo.NodeIndexName);
                }
                else
                {
                    nodeInfo.NodeIndexName = string.Empty;
                }

                if (!string.IsNullOrEmpty(nodeInfo.FilePath) && filePathArrayList.IndexOf(nodeInfo.FilePath) == -1)
                {
                    filePathArrayList.Add(nodeInfo.FilePath);
                }
                else
                {
                    nodeInfo.FilePath = string.Empty;
                }

                int insertedNodeID = DataProvider.NodeDAO.InsertNodeInfo(nodeInfo);

                if (translateType == ETranslateType.All)
                {
                    TranslateContent(targetPublishmentSystemInfo, oldNodeInfo.NodeID, insertedNodeID, isChecked, checkedLevel);
                }

                if (insertedNodeID != 0)
                {
                    string orderByString = ETaxisTypeUtils.GetChannelOrderByString(ETaxisType.OrderByTaxis);
                    ArrayList childrenNodeInfoArrayList = DataProvider.NodeDAO.GetNodeInfoArrayList(oldNodeInfo, 0, "", EScopeType.Children, orderByString);
                    if (childrenNodeInfoArrayList != null && childrenNodeInfoArrayList.Count > 0)
                    {
                        this.TranslateChannelAndContent(childrenNodeInfoArrayList, targetPublishmentSystemID, insertedNodeID, translateType, isChecked, checkedLevel, nodeIndexNameArrayList, filePathArrayList);
                    }

                    if (isChecked)
                    {
                        //this.FSO.CreateRedirectChannel(insertedNodeID);

                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateChannel(targetPublishmentSystemID, insertedNodeID);
                        //栏目转移，内容较多，不执行异步生成
                        //AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }
                }
			}
		}

		private void TranslateContent(PublishmentSystemInfo targetPublishmentSystemInfo, int nodeID, int targetNodeID, bool isChecked, int checkedLevel)
		{
            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);

            string orderByString = ETaxisTypeUtils.GetOrderByString(tableStyle, ETaxisType.OrderByTaxis);

            ETableStyle targetTableStyle = NodeManager.GetTableStyle(targetPublishmentSystemInfo, targetNodeID);
            string targetTableName = NodeManager.GetTableName(targetPublishmentSystemInfo, targetNodeID);
            ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
			foreach (int contentID in contentIDArrayList)
			{
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                FileUtility.MoveFileByContentInfo(base.PublishmentSystemInfo, targetPublishmentSystemInfo, contentInfo);
                contentInfo.PublishmentSystemID = TranslateUtils.ToInt(this.PublishmentSystemIDDropDownList.SelectedValue);
                contentInfo.SourceID = contentInfo.NodeID;
				contentInfo.NodeID = targetNodeID;
				contentInfo.IsChecked = isChecked;
				contentInfo.CheckedLevel = checkedLevel;

                int theContentID = DataProvider.ContentDAO.Insert(targetTableName, targetPublishmentSystemInfo, contentInfo);
				if (contentInfo.IsChecked)
				{
					//this.FSO.CreateRedirectContent(contentInfo.NodeID, theContentID);

                    string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateContent(targetPublishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID, theContentID);
                    AjaxUrlManager.AddAjaxUrl(ajaxUrl);
				}
			}

			if (IsDeleteAfterTranslate.Visible && EBooleanUtils.Equals(IsDeleteAfterTranslate.SelectedValue, EBoolean.True))
			{
                try
                {
                    DataProvider.ContentDAO.TrashContents(base.PublishmentSystemID, tableName, contentIDArrayList, nodeID);
                }
                catch { }
			}
		}


		public void PublishmentSystemID_OnSelectedIndexChanged(object sender, EventArgs E)
		{
			int psID = int.Parse(this.PublishmentSystemIDDropDownList.SelectedValue);

			NodeIDTo.Items.Clear();

			ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(psID);
            int nodeCount = nodeIDArrayList.Count;
			this.isLastNodeArray = new bool[nodeCount];
            foreach (int theNodeID in nodeIDArrayList)
			{
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(psID, theNodeID);
                string value = base.IsOwningNodeID(nodeInfo.NodeID) ? nodeInfo.NodeID.ToString() : "";
                value = (nodeInfo.Additional.IsContentAddable) ? value : "";
                ListItem listitem = new ListItem(this.GetTitle(nodeInfo), value);
				NodeIDTo.Items.Add(listitem);
			}
		}

        public string ReturnUrl { get { return this.returnUrl; } }
	}
}
