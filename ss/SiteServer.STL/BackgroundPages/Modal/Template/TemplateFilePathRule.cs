using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.IO;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.STL.BackgroundPages.Modal
{
    public class TemplateFilePathRule : BackgroundBasePage
	{
        public PlaceHolder phFilePath;
        public TextBox tbFilePath;
        public TextBox tbChannelFilePathRule;
        public TextBox tbContentFilePathRule;

        public Button btnCreateChannelRule;
        public Button btnCreateContentRule;

		private int nodeID;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID)
        {
            return ChannelLoading.GetTemplateFilePathRuleOpenWindowString(publishmentSystemID, nodeID);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID");
            this.nodeID = base.GetIntQueryString("NodeID");

			if (!IsPostBack)
			{
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                if (nodeInfo.NodeType == ENodeType.BackgroundPublishNode || nodeInfo.LinkType == ELinkType.LinkToFirstChannel || nodeInfo.LinkType == ELinkType.LinkToFirstContent || nodeInfo.LinkType == ELinkType.LinkToLastAddChannel || nodeInfo.LinkType == ELinkType.NoLink)
                {
                    this.phFilePath.Visible = false;
                }

                string showPopWinString = FilePathRule.GetOpenWindowString(base.PublishmentSystemID, this.nodeID, true, this.tbChannelFilePathRule.ClientID);
                this.btnCreateChannelRule.Attributes.Add("onclick", showPopWinString);

                showPopWinString = FilePathRule.GetOpenWindowString(base.PublishmentSystemID, this.nodeID, false, this.tbContentFilePathRule.ClientID);
                this.btnCreateContentRule.Attributes.Add("onclick", showPopWinString);

                if (string.IsNullOrEmpty(nodeInfo.FilePath))
                {
                    this.tbFilePath.Text = PageUtility.GetInputChannelUrl(base.PublishmentSystemInfo, nodeInfo, base.PublishmentSystemInfo.Additional.VisualType);
                }
                else
                {
                    this.tbFilePath.Text = nodeInfo.FilePath;
                }

                if (string.IsNullOrEmpty(nodeInfo.ChannelFilePathRule))
                {
                    this.tbChannelFilePathRule.Text = PathUtility.GetChannelFilePathRule(base.PublishmentSystemInfo, this.nodeID);
                }
                else
                {
                    this.tbChannelFilePathRule.Text = nodeInfo.ChannelFilePathRule;
                }

                if (string.IsNullOrEmpty(nodeInfo.ContentFilePathRule))
                {
                    this.tbContentFilePathRule.Text = PathUtility.GetContentFilePathRule(base.PublishmentSystemInfo, this.nodeID);
                }
                else
                {
                    this.tbContentFilePathRule.Text = nodeInfo.ContentFilePathRule;
                }
            }
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            try
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                string filePath = nodeInfo.FilePath;

                if (this.phFilePath.Visible)
                {
                    this.tbFilePath.Text = this.tbFilePath.Text.Trim();
                    if (!string.IsNullOrEmpty(this.tbFilePath.Text) && !StringUtils.EqualsIgnoreCase(filePath, this.tbFilePath.Text))
                    {
                        if (!DirectoryUtils.IsDirectoryNameCompliant(this.tbFilePath.Text))
                        {
                            base.FailMessage("栏目页面路径不符合系统要求！");
                            return;
                        }

                        if (PathUtils.IsDirectoryPath(this.tbFilePath.Text))
                        {
                            this.tbFilePath.Text = PageUtils.Combine(this.tbFilePath.Text, "index.html");
                        }

                        ArrayList filePathArrayList = DataProvider.NodeDAO.GetAllFilePathByPublishmentSystemID(base.PublishmentSystemID);
                        filePathArrayList.AddRange(DataProvider.TemplateMatchDAO.GetAllFilePathByPublishmentSystemID(base.PublishmentSystemID));
                        if (filePathArrayList.IndexOf(this.tbFilePath.Text) != -1)
                        {
                            base.FailMessage("栏目修改失败，栏目页面路径已存在！");
                            return;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(this.tbChannelFilePathRule.Text))
                {
                    string filePathRule = this.tbChannelFilePathRule.Text.Replace("|", string.Empty);
                    if (!DirectoryUtils.IsDirectoryNameCompliant(filePathRule))
                    {
                        base.FailMessage("栏目页面命名规则不符合系统要求！");
                        return;
                    }
                    if (PathUtils.IsDirectoryPath(filePathRule))
                    {
                        base.FailMessage("栏目页面命名规则必须包含生成文件的后缀！");
                        return;
                    }
                    
                    //if (!StringUtils.Contains(this.tbChannelFilePathRule.Text.ToLower(), PathUtility.ChannelFilePathRules.ChannelID.ToLower()))
                    //{
                    //    base.FailMessage(string.Format("栏目页面命名规则必须包含<strong>{0}</strong>！", PathUtility.ChannelFilePathRules.ChannelID));
                    //    return;
                    //}
                }

                if (!string.IsNullOrEmpty(this.tbContentFilePathRule.Text))
                {
                    string filePathRule = this.tbContentFilePathRule.Text.Replace("|", string.Empty);
                    if (!DirectoryUtils.IsDirectoryNameCompliant(filePathRule))
                    {
                        base.FailMessage("内容页面命名规则不符合系统要求！");
                        return;
                    }
                    if (PathUtils.IsDirectoryPath(filePathRule))
                    {
                        base.FailMessage("内容页面命名规则必须包含生成文件的后缀！");
                        return;
                    }
                    
                    //if (!StringUtils.Contains(this.tbContentFilePathRule.Text.ToLower(), PathUtility.ContentFilePathRules.ContentID.ToLower()))
                    //{
                    //    base.FailMessage(string.Format("内容页面命名规则必须包含<strong>{0}</strong>！", PathUtility.ContentFilePathRules.ContentID));
                    //    return;
                    //}
                }

                if (this.tbFilePath.Text != PageUtility.GetInputChannelUrl(base.PublishmentSystemInfo, nodeInfo, base.PublishmentSystemInfo.Additional.VisualType))
                {
                    nodeInfo.FilePath = this.tbFilePath.Text;
                }
                if (this.tbChannelFilePathRule.Text != PathUtility.GetChannelFilePathRule(base.PublishmentSystemInfo, this.nodeID))
                {
                    nodeInfo.ChannelFilePathRule = this.tbChannelFilePathRule.Text;
                }
                if (this.tbContentFilePathRule.Text != PathUtility.GetContentFilePathRule(base.PublishmentSystemInfo, this.nodeID))
                {
                    nodeInfo.ContentFilePathRule = this.tbContentFilePathRule.Text;
                }

                DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);
                
                FileSystemObject FSO = new FileSystemObject(base.PublishmentSystemID);
                FSO.CreateImmediately(EChangedType.Edit, ETemplateType.ChannelTemplate, this.nodeID, 0, 0);

                StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "设置页面命名规则", string.Format("栏目:{0}", nodeInfo.NodeName));

                isSuccess = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }

            if (isSuccess)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetSTLUrl(string.Format("background_templateFilePathRule.aspx?PublishmentSystemID={0}&CurrentNodeID={1}", base.PublishmentSystemID, this.nodeID)));
            }
        }
	}
}
