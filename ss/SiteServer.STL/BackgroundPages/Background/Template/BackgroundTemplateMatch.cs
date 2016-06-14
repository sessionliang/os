using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTemplateMatch : BackgroundBasePage
	{
		public ListBox NodeIDCollectionToMatch;
		public ListBox ChannelTemplateID;
		public ListBox ContentTemplateID;
        public PlaceHolder phCreate;
		
		bool[] IsLastNodeArray;
		string defaultChannelTemplateName;
		string defaultContentTemplateName;

        public string GetTitle(NodeInfo nodeInfo)
		{
			string str = string.Empty;
			if (nodeInfo.NodeID == base.PublishmentSystemID)
			{
                nodeInfo.IsLastNode = true;
			}
            if (nodeInfo.IsLastNode == false)
			{
                IsLastNodeArray[nodeInfo.ParentsCount] = false;
			}
			else
			{
                IsLastNodeArray[nodeInfo.ParentsCount] = true;
			}
            for (int i = 0; i < nodeInfo.ParentsCount; i++)
			{
				if (IsLastNodeArray[i])
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
            str = String.Concat(str, StringUtils.MaxLengthText(nodeInfo.NodeName, 8));


            if (nodeInfo.NodeType == ENodeType.BackgroundPublishNode)
			{
                int indexTemplateID = TemplateManager.GetDefaultTemplateID(base.PublishmentSystemID, ETemplateType.IndexPageTemplate);
                string indexTemplateName = TemplateManager.GetTemplateName(base.PublishmentSystemID, indexTemplateID);
				str = string.Concat(str, string.Format(" ({0})", indexTemplateName));
            }
            else
            {
                int channelTemplateID = nodeInfo.ChannelTemplateID;
                int contentTemplateID = nodeInfo.ContentTemplateID;

                string channelTemplateName = string.Empty;
                if (channelTemplateID != 0)
                {
                    channelTemplateName = TemplateManager.GetTemplateName(base.PublishmentSystemID, channelTemplateID);
                }
                if (string.IsNullOrEmpty(channelTemplateName))
                {
                    channelTemplateName = this.defaultChannelTemplateName;
                }
                str = string.Concat(str, string.Format(" ({0})", channelTemplateName));

                string contentTemplateName = string.Empty;
                if (contentTemplateID != 0)
                {
                    contentTemplateName = TemplateManager.GetTemplateName(base.PublishmentSystemID, contentTemplateID);
                }
                if (string.IsNullOrEmpty(contentTemplateName))
                {
                    contentTemplateName = this.defaultContentTemplateName;
                }
                str = string.Concat(str, string.Format(" ({0})", contentTemplateName));
            }
			
			return str;
		}

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            int defaultChannelTemplateID = TemplateManager.GetDefaultTemplateID(base.PublishmentSystemID, ETemplateType.ChannelTemplate);
            this.defaultChannelTemplateName = TemplateManager.GetTemplateName(base.PublishmentSystemID, defaultChannelTemplateID);

            int defaultContentTemplateID = TemplateManager.GetDefaultTemplateID(base.PublishmentSystemID, ETemplateType.ContentTemplate);
            this.defaultContentTemplateName = TemplateManager.GetTemplateName(base.PublishmentSystemID, defaultContentTemplateID);

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Template, "匹配模板", AppManager.CMS.Permission.WebSite.Template);

                this.ChannelTemplateID.Attributes.Add("onfocus", "$('ContentTemplateID').selectedIndex = -1");
                this.ContentTemplateID.Attributes.Add("onfocus", "$('ChannelTemplateID').selectedIndex = -1");

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
			string selectedChannelTemplateID = ChannelTemplateID.SelectedValue;
			string selectedContentTemplateID = ContentTemplateID.SelectedValue;

			NodeIDCollectionToMatch.Items.Clear();
			ChannelTemplateID.Items.Clear();
			ContentTemplateID.Items.Clear();
			ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
            int nodeCount = nodeIDArrayList.Count;
			IsLastNodeArray = new bool[nodeCount];
            foreach (int theNodeID in nodeIDArrayList)
			{
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, theNodeID);
                ListItem listitem = new ListItem(this.GetTitle(nodeInfo), nodeInfo.NodeID.ToString());
				NodeIDCollectionToMatch.Items.Add(listitem);
			}

            ChannelTemplateID.DataSource = DataProvider.TemplateDAO.GetDataSourceByType(base.PublishmentSystemID, ETemplateType.ChannelTemplate);
            ContentTemplateID.DataSource = DataProvider.TemplateDAO.GetDataSourceByType(base.PublishmentSystemID, ETemplateType.ContentTemplate);
			base.DataBind();

			string[] stringArray = new string[selectedNodeIDArrayList.Count];
			selectedNodeIDArrayList.CopyTo(stringArray);
			ControlUtils.SelectListItems(NodeIDCollectionToMatch, stringArray);
			ControlUtils.SelectListItems(ChannelTemplateID, selectedChannelTemplateID);
			ControlUtils.SelectListItems(ContentTemplateID, selectedContentTemplateID);
		}


		public void MatchChannelTemplateButton_OnClick(object sender, EventArgs E)
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
					int channelTemplateID = int.Parse(this.ChannelTemplateID.SelectedValue);
					this.Process(nodeIDArrayList, channelTemplateID, true);
				}
			}
		}

		public void RemoveChannelTemplateButton_OnClick(object sender, EventArgs E)
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
					int channelTemplateID = 0;
					this.Process(nodeIDArrayList, channelTemplateID, true);
				}
			}
		}

		public void MatchContentTemplateButton_OnClick(object sender, EventArgs E)
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
					int contentTemplateID = int.Parse(this.ContentTemplateID.SelectedValue);
					this.Process(nodeIDArrayList, contentTemplateID, false);
				}
			}
		}

		public void RemoveContentTemplateButton_OnClick(object sender, EventArgs E)
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
					int contentTemplateID = 0;
					this.Process(nodeIDArrayList, contentTemplateID, false);
				}
			}
		}

		private bool Validate(bool isMatch, bool isChannelTemplate)
		{
			if (this.NodeIDCollectionToMatch.SelectedIndex < 0)
			{
                base.FailMessage("请选择栏目！");
				return false;
			}
			if (isMatch)
			{
				if (isChannelTemplate)
				{
					if (this.ChannelTemplateID.SelectedIndex < 0)
					{
                        base.FailMessage("请选择栏目模板！");
						return false;
					}
				}
				else
				{
					if (this.ContentTemplateID.SelectedIndex < 0)
					{
                        base.FailMessage("请选择内容模板！");
						return false;
					}
				}
			}
			return true;
		}

		private void Process(ArrayList nodeIDArrayList, int templateID, bool isChannelTemplate)
		{
			if (nodeIDArrayList != null && nodeIDArrayList.Count > 0)
			{
                if (isChannelTemplate)
                {
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        TemplateManager.UpdateChannelTemplateID(base.PublishmentSystemID, nodeID, templateID);
                    }
                }
                else
                {
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        TemplateManager.UpdateContentTemplateID(base.PublishmentSystemID, nodeID, templateID);
                    }
                }
			}
			
			if (templateID == 0)
			{
                StringUtility.AddLog(base.PublishmentSystemID, "取消模板匹配", string.Format("栏目:{0}", this.GetNodeNames()));
				base.SuccessMessage("取消匹配成功！");
			}
			else
			{
                StringUtility.AddLog(base.PublishmentSystemID, "模板匹配", string.Format("栏目:{0}", this.GetNodeNames()));
				base.SuccessMessage("模板匹配成功！");
			}
            
            BindListBox();
		}

        private string GetNodeNames()
        {
            StringBuilder builder = new StringBuilder();
            foreach (ListItem listItem in NodeIDCollectionToMatch.Items)
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

            return RegexUtils.Replace("\\(.*?\\)|│|├|　|└", builder.ToString(), string.Empty);
        }

		public void CreateChannelTemplate_Click(object sender, EventArgs e)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				if (Validate(false, false))
				{
                    int defaultChannelTemplateID = TemplateManager.GetDefaultTemplateID(base.PublishmentSystemID, ETemplateType.ChannelTemplate);
                    ArrayList relatedFileNameArrayList = DataProvider.TemplateDAO.GetLowerRelatedFileNameArrayList(base.PublishmentSystemID, ETemplateType.ChannelTemplate);
                    ArrayList templateNameArrayList = DataProvider.TemplateDAO.GetTemplateNameArrayList(base.PublishmentSystemID, ETemplateType.ChannelTemplate);
					foreach (ListItem item in NodeIDCollectionToMatch.Items)
					{
						if (item.Selected)
						{
							int nodeID = int.Parse(item.Value);
							int channelTemplateID = -1;

                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
							if (nodeInfo.NodeType != ENodeType.BackgroundPublishNode)
							{
                                channelTemplateID = nodeInfo.ChannelTemplateID;
							}

							if (channelTemplateID != -1 && channelTemplateID != 0 && channelTemplateID != defaultChannelTemplateID)
							{
                                if (TemplateManager.GetTemplateInfo(base.PublishmentSystemID, channelTemplateID) == null)
								{
									channelTemplateID = -1;
								}
							}

							if (channelTemplateID != -1)
							{
                                TemplateInfo templateInfo = new TemplateInfo(0, base.PublishmentSystemID, nodeInfo.NodeName, ETemplateType.ChannelTemplate, "T_" + nodeInfo.NodeName + ".html", "index.html", ".html", ECharsetUtils.GetEnumType(base.PublishmentSystemInfo.Additional.Charset), false);
								
								if (relatedFileNameArrayList.Contains(templateInfo.RelatedFileName.ToLower()))
								{
									continue;
								}
								else if (templateNameArrayList.Contains(templateInfo.TemplateName))
								{
									continue;
								}
								int insertedTemplateID = DataProvider.TemplateDAO.Insert(templateInfo, string.Empty);
                                if (nodeInfo.NodeType != ENodeType.BackgroundPublishNode)
                                {
                                    TemplateManager.UpdateChannelTemplateID(base.PublishmentSystemID, nodeID, insertedTemplateID);
                                    //DataProvider.BackgroundNodeDAO.UpdateChannelTemplateID(nodeID, insertedTemplateID);
                                }
								
							}
						}
					}

                    StringUtility.AddLog(base.PublishmentSystemID, "生成并匹配栏目模版", string.Format("栏目:{0}", this.GetNodeNames()));

					base.SuccessMessage("生成栏目模版并匹配成功！");

                    this.BindListBox();
				}
			}
		}

		public void CreateSubChannelTemplate_Click(object sender, EventArgs e)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				if (Validate(false, false))
				{
                    ArrayList relatedFileNameArrayList = DataProvider.TemplateDAO.GetLowerRelatedFileNameArrayList(base.PublishmentSystemID, ETemplateType.ChannelTemplate);
                    ArrayList templateNameArrayList = DataProvider.TemplateDAO.GetTemplateNameArrayList(base.PublishmentSystemID, ETemplateType.ChannelTemplate);
					foreach (ListItem item in NodeIDCollectionToMatch.Items)
					{
						if (item.Selected)
						{
							int nodeID = int.Parse(item.Value);
                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

                            TemplateInfo templateInfo = new TemplateInfo(0, base.PublishmentSystemID, nodeInfo.NodeName + "_下级", ETemplateType.ChannelTemplate, "T_" + nodeInfo.NodeName + "_下级.html", "index.html", ".html", ECharsetUtils.GetEnumType(base.PublishmentSystemInfo.Additional.Charset), false);
								
							if (relatedFileNameArrayList.Contains(templateInfo.RelatedFileName.ToLower()))
							{
								continue;
							}
							else if (templateNameArrayList.Contains(templateInfo.TemplateName))
							{
								continue;
							}
							int insertedTemplateID = DataProvider.TemplateDAO.Insert(templateInfo, string.Empty);
							ArrayList childNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(nodeID);
							foreach (int childNodeID in childNodeIDArrayList)
							{
                                TemplateManager.UpdateChannelTemplateID(base.PublishmentSystemID, childNodeID, insertedTemplateID);
								//DataProvider.BackgroundNodeDAO.UpdateChannelTemplateID(childNodeID, insertedTemplateID);
							}
						}
					}

                    StringUtility.AddLog(base.PublishmentSystemID, "生成并匹配下级栏目模版", string.Format("栏目:{0}", this.GetNodeNames()));

					base.SuccessMessage("生成下级栏目模版并匹配成功！");

                    this.BindListBox();
				}
			}
		}

		public void CreateContentTemplate_Click(object sender, EventArgs e)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				if (Validate(false, false))
				{
                    int defaultContentTemplateID = TemplateManager.GetDefaultTemplateID(base.PublishmentSystemID, ETemplateType.ContentTemplate);
                    ArrayList relatedFileNameArrayList = DataProvider.TemplateDAO.GetLowerRelatedFileNameArrayList(base.PublishmentSystemID, ETemplateType.ContentTemplate);
                    ArrayList templateNameArrayList = DataProvider.TemplateDAO.GetTemplateNameArrayList(base.PublishmentSystemID, ETemplateType.ContentTemplate);
					foreach (ListItem item in NodeIDCollectionToMatch.Items)
					{
						if (item.Selected)
						{
							int nodeID = int.Parse(item.Value);

                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

                            int contentTemplateID = nodeInfo.ContentTemplateID;                            

							if (contentTemplateID != 0 && contentTemplateID != defaultContentTemplateID)
							{
                                if (TemplateManager.GetTemplateInfo(base.PublishmentSystemID, contentTemplateID) == null)
								{
									contentTemplateID = -1;
								}
							}

							if (contentTemplateID != -1)
							{
                                TemplateInfo templateInfo = new TemplateInfo(0, base.PublishmentSystemID, nodeInfo.NodeName, ETemplateType.ContentTemplate, "T_" + nodeInfo.NodeName + ".html", "index.html", ".html", ECharsetUtils.GetEnumType(base.PublishmentSystemInfo.Additional.Charset), false);
								if (relatedFileNameArrayList.Contains(templateInfo.RelatedFileName.ToLower()))
								{
									continue;
								}
								else if (templateNameArrayList.Contains(templateInfo.TemplateName))
								{
									continue;
								}
								int insertedTemplateID = DataProvider.TemplateDAO.Insert(templateInfo, string.Empty);
                                TemplateManager.UpdateContentTemplateID(base.PublishmentSystemID, nodeID, insertedTemplateID);
								//DataProvider.BackgroundNodeDAO.UpdateContentTemplateID(nodeID, insertedTemplateID);
							}
						}
					}

                    StringUtility.AddLog(base.PublishmentSystemID, "生成并匹配内容模版", string.Format("栏目:{0}", this.GetNodeNames()));
					
					base.SuccessMessage("生成内容模版并匹配成功！");
                    
                    this.BindListBox();
				}
			}
		}

		public void CreateSubContentTemplate_Click(object sender, EventArgs e)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				if (Validate(false, false))
				{
                    ArrayList relatedFileNameArrayList = DataProvider.TemplateDAO.GetLowerRelatedFileNameArrayList(base.PublishmentSystemID, ETemplateType.ContentTemplate);
                    ArrayList templateNameArrayList = DataProvider.TemplateDAO.GetTemplateNameArrayList(base.PublishmentSystemID, ETemplateType.ContentTemplate);
					foreach (ListItem item in NodeIDCollectionToMatch.Items)
					{
						if (item.Selected)
						{
							int nodeID = int.Parse(item.Value);
                            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

                            TemplateInfo templateInfo = new TemplateInfo(0, base.PublishmentSystemID, nodeInfo.NodeName + "_下级", ETemplateType.ContentTemplate, "T_" + nodeInfo.NodeName + "_下级.html", "index.html", ".html", ECharsetUtils.GetEnumType(base.PublishmentSystemInfo.Additional.Charset), false);
								
							if (relatedFileNameArrayList.Contains(templateInfo.RelatedFileName.ToLower()))
							{
								continue;
							}
							else if (templateNameArrayList.Contains(templateInfo.TemplateName))
							{
								continue;
							}
							int insertedTemplateID = DataProvider.TemplateDAO.Insert(templateInfo, string.Empty);
							ArrayList childNodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(nodeID);
							foreach (int childNodeID in childNodeIDArrayList)
							{
                                TemplateManager.UpdateContentTemplateID(base.PublishmentSystemID, childNodeID, insertedTemplateID);
								//DataProvider.BackgroundNodeDAO.UpdateContentTemplateID(childNodeID, insertedTemplateID);
							}
						}
					}

                    StringUtility.AddLog(base.PublishmentSystemID, "生成并匹配下级内容模版", string.Format("栏目:{0}", this.GetNodeNames()));
					
					base.SuccessMessage("生成下级内容模版并匹配成功！");
                    
                    this.BindListBox();
				}
			}
		}
	}
}
