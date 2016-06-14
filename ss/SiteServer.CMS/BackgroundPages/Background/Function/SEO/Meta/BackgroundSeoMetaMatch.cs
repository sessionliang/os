using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;



namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundSeoMetaMatch : BackgroundBasePage
	{
		public ListBox NodeIDCollectionToMatch;
		public ListBox ChannelSeoMetaID;
		public ListBox ContentSeoMetaID;
		
		bool[] IsLastNodeArray;
		string defaultSeoMetaName;

		public string GetTitle(int nodeID, string nodeName, ENodeType nodeType, int parentsCount, bool isLastNode)
		{
			string str = "";
            if (nodeID == base.PublishmentSystemID)
			{
				isLastNode = true;
			}
			if (isLastNode == false)
			{
				this.IsLastNodeArray[parentsCount] = false;
			}
			else
			{
				this.IsLastNodeArray[parentsCount] = true;
			}
			for (int i = 0; i < parentsCount; i++)
			{
				if (this.IsLastNodeArray[i])
				{
					str = String.Concat(str, "　");
				}
				else
				{
					str = String.Concat(str, "│");
				}
			}
			if (isLastNode)
			{
				str = String.Concat(str, "└");
			}
			else
			{
				str = String.Concat(str, "├");
			}
			str = String.Concat(str, StringUtils.MaxLengthText(nodeName, 8));

			int channelSeoMetaID = DataProvider.SeoMetaDAO.GetSeoMetaIDByNodeID(nodeID, true);
			string channelSeoMetaName = string.Empty;
			if (channelSeoMetaID != 0)
			{
				channelSeoMetaName = DataProvider.SeoMetaDAO.GetSeoMetaName(channelSeoMetaID);
			}
			if (string.IsNullOrEmpty(channelSeoMetaName))
			{
				channelSeoMetaName = this.defaultSeoMetaName;
			}
			str = string.Concat(str, string.Format(" ({0})", channelSeoMetaName));

			int contentSeoMetaID = DataProvider.SeoMetaDAO.GetSeoMetaIDByNodeID(nodeID, false);
			string contentSeoMetaName = string.Empty;
			if (contentSeoMetaID != 0)
			{
				contentSeoMetaName = DataProvider.SeoMetaDAO.GetSeoMetaName(contentSeoMetaID);
			}
			if (string.IsNullOrEmpty(contentSeoMetaName))
			{
				contentSeoMetaName = this.defaultSeoMetaName;
			}
			str = string.Concat(str, string.Format(" ({0})", contentSeoMetaName));
			
			return str;
		}
		

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            int defaultSeoMetaID = DataProvider.SeoMetaDAO.GetDefaultSeoMetaID(base.PublishmentSystemID);
			if (defaultSeoMetaID == 0)
			{
				this.defaultSeoMetaName = "<无>";
			}
			else
			{
				this.defaultSeoMetaName = DataProvider.SeoMetaDAO.GetSeoMetaName(defaultSeoMetaID);
			}

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_SEO, "页面元数据匹配", AppManager.CMS.Permission.WebSite.SEO);

				BindListBox();
			}
		}


		public void BindListBox()
		{
			ArrayList selectedNodeIDArrayList = new ArrayList();
			foreach (ListItem listitem in NodeIDCollectionToMatch.Items)
			{
				if (listitem.Selected) selectedNodeIDArrayList.Add(listitem.Value);
			}
			string selectedChannelSeoMetaID = this.ChannelSeoMetaID.SelectedValue;
			string selectedContentSeoMetaID = this.ContentSeoMetaID.SelectedValue;

			NodeIDCollectionToMatch.Items.Clear();
			this.ChannelSeoMetaID.Items.Clear();
			this.ContentSeoMetaID.Items.Clear();
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
            int nodeCount = nodeIDArrayList.Count;
			this.IsLastNodeArray = new bool[nodeCount];
            foreach (int theNodeID in nodeIDArrayList)
			{
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, theNodeID);
				ListItem listitem = new ListItem(this.GetTitle(nodeInfo.NodeID, nodeInfo.NodeName, nodeInfo.NodeType, nodeInfo.ParentsCount, nodeInfo.IsLastNode), nodeInfo.NodeID.ToString());
				NodeIDCollectionToMatch.Items.Add(listitem);
			}

            ArrayList seoMetaInfoArrayList = DataProvider.SeoMetaDAO.GetSeoMetaInfoArrayListByPublishmentSystemID(base.PublishmentSystemID);
			foreach (SeoMetaInfo seoMetaInfo in seoMetaInfoArrayList)
			{
				ListItem listitem = new ListItem(seoMetaInfo.SeoMetaName, seoMetaInfo.SeoMetaID.ToString());
				this.ChannelSeoMetaID.Items.Add(listitem);
				this.ContentSeoMetaID.Items.Add(listitem);
			}

			string[] stringArray = new string[selectedNodeIDArrayList.Count];
			selectedNodeIDArrayList.CopyTo(stringArray);
			ControlUtils.SelectListItems(NodeIDCollectionToMatch, stringArray);
			ControlUtils.SelectListItems(this.ChannelSeoMetaID, selectedChannelSeoMetaID);
			ControlUtils.SelectListItems(this.ContentSeoMetaID, selectedContentSeoMetaID);
		}


		public void MatchChannelSeoMetaButton_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				if (Validate(true, true))
				{
					ArrayList nodeIDArrayList = new ArrayList();
					foreach (ListItem item in NodeIDCollectionToMatch.Items)
					{
						if (item.Selected)
						{
							int nodeID = int.Parse(item.Value);
							nodeIDArrayList.Add(nodeID);
						}
					}
					int channelSeoMetaID = int.Parse(this.ChannelSeoMetaID.SelectedValue);
					this.Process(nodeIDArrayList, channelSeoMetaID, true);
				}
			}
		}

		public void RemoveChannelSeoMetaButton_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				if (Validate(false, true))
				{
					ArrayList nodeIDArrayList = new ArrayList();
					foreach (ListItem item in NodeIDCollectionToMatch.Items)
					{
						if (item.Selected)
						{
							int nodeID = int.Parse(item.Value);
							nodeIDArrayList.Add(nodeID);
						}
					}
					int channelSeoMetaID = 0;
					this.Process(nodeIDArrayList, channelSeoMetaID, true);
				}
			}
		}

		public void MatchContentSeoMetaButton_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				if (Validate(true, false))
				{
					ArrayList nodeIDArrayList = new ArrayList();
					foreach (ListItem item in NodeIDCollectionToMatch.Items)
					{
						if (item.Selected)
						{
							int nodeID = int.Parse(item.Value);
							nodeIDArrayList.Add(nodeID);
						}
					}
					int contentSeoMetaID = int.Parse(this.ContentSeoMetaID.SelectedValue);
					this.Process(nodeIDArrayList, contentSeoMetaID, false);
				}
			}
		}

		public void RemoveContentSeoMetaButton_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				if (Validate(false, false))
				{
					ArrayList nodeIDArrayList = new ArrayList();
					foreach (ListItem item in NodeIDCollectionToMatch.Items)
					{
						if (item.Selected)
						{
							int nodeID = int.Parse(item.Value);
							nodeIDArrayList.Add(nodeID);
						}
					}
					int contentSeoMetaID = 0;
					this.Process(nodeIDArrayList, contentSeoMetaID, false);
				}
			}
		}

		private bool Validate(bool isMatch, bool isChannelSeoMeta)
		{
			if (this.NodeIDCollectionToMatch.SelectedIndex < 0)
			{
				base.FailMessage("请选择栏目！");
				return false;
			}
			if (isMatch)
			{
				if (isChannelSeoMeta)
				{
					if (this.ChannelSeoMetaID.SelectedIndex < 0)
					{
                        base.FailMessage("请选择栏目页元数据！");
						return false;
					}
				}
				else
				{
					if (this.ContentSeoMetaID.SelectedIndex < 0)
					{
                        base.FailMessage("请选择内容页元数据！");
						return false;
					}
				}
			}
			return true;
		}

		private void Process(ArrayList nodeIDArrayList, int seoMetaID, bool isChannel)
		{
			if (nodeIDArrayList != null && nodeIDArrayList.Count > 0)
			{
				foreach (int nodeID in nodeIDArrayList)
				{
					if (seoMetaID == 0)
					{
						DataProvider.SeoMetaDAO.DeleteMatch(base.PublishmentSystemID, nodeID, isChannel);
					}
					else
					{
                        DataProvider.SeoMetaDAO.InsertMatch(base.PublishmentSystemID, nodeID, seoMetaID, isChannel);
					}
				}
			}
			BindListBox();
			if (seoMetaID == 0)
			{
				base.SuccessMessage("取消匹配成功！");
			}
			else
			{
				base.SuccessMessage("模板匹配成功！");
			}
		}

	}
}
