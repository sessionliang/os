using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core;

using BaiRong.Core.IO.FileManagement;
using System.Collections;
using System.Text;
using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;
using System.Collections.Generic;

namespace SiteServer.STL.BackgroundPages
{
	public class ConsoleIndependentTemplateSave : BackgroundBasePage
	{
		public PlaceHolder phWelcome;
		public TextBox IndependentTemplateName;
		public TextBox IndependentTemplateDir;
		public TextBox WebSiteUrl;
		public TextBox Description;

        public PlaceHolder phSaveTemplates;
        public CheckBoxList cblIndexTemplates;
        public CheckBoxList cblChannelTemplates;
        public CheckBoxList cblContentTemplates;
        public CheckBoxList cblFileTemplates;

        public PlaceHolder phSaveSiteContents;
        public Literal ltlChannelTree;
        public RadioButtonList rblIsSaveContents;

        public PlaceHolder phSaveFiles;
        public CheckBoxList cblDirectoriesAndFiles;

        public PlaceHolder phUploadImageFile;

        public PlaceHolder phDone;

        public PlaceHolder phOperatingError;
		public Literal ltlErrorMessage;

		public Button Previous;
		public Button Next;

		ExportObject exportObject;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			this.exportObject = new ExportObject(base.PublishmentSystemID);

			if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "保存独立模板", AppManager.Platform.Permission.Platform_Site);

                if (base.PublishmentSystemInfo.IsHeadquarters)
				{
                    this.IndependentTemplateDir.Text = "IN_" + base.PublishmentSystemInfo.PublishmentSystemName;
				}
				else
				{
                    this.IndependentTemplateDir.Text = "IN_" + base.PublishmentSystemInfo.PublishmentSystemDir.Replace("\\", "_");
				}
                this.IndependentTemplateName.Text = base.PublishmentSystemInfo.PublishmentSystemName;

                ArrayList templateInfoArrayList = DataProvider.TemplateDAO.GetTemplateInfoArrayListByPublishmentSystemID(base.PublishmentSystemID);
                foreach (TemplateInfo templateInfo in templateInfoArrayList)
                {
                    ListItem listItem = new ListItem(string.Format("{0}（{1}）", templateInfo.TemplateName, templateInfo.RelatedFileName), templateInfo.TemplateID.ToString());
                    if (templateInfo.TemplateType == ETemplateType.IndexPageTemplate)
                    {
                        this.cblIndexTemplates.Items.Add(listItem);
                    }
                    else if (templateInfo.TemplateType == ETemplateType.ChannelTemplate)
                    {
                        this.cblChannelTemplates.Items.Add(listItem);
                    }
                    else if (templateInfo.TemplateType == ETemplateType.ContentTemplate)
                    {
                        this.cblContentTemplates.Items.Add(listItem);
                    }
                    else if (templateInfo.TemplateType == ETemplateType.FileTemplate)
                    {
                        this.cblFileTemplates.Items.Add(listItem);
                    }
                }

                EBooleanUtils.AddListItems(this.rblIsSaveContents, "保存内容数据", "不保存内容数据");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSaveContents, true.ToString());

                this.ltlChannelTree.Text = this.GetChannelTreeHtml();

                ArrayList publishmentSystemDirArrayList = DataProvider.PublishmentSystemDAO.GetLowerPublishmentSystemDirArrayListThatNotIsHeadquarters();
                FileSystemInfoExtendCollection fileSystems = FileManager.GetFileSystemInfoExtendCollection(PathUtility.GetPublishmentSystemPath(base.PublishmentSystemInfo), true);
                foreach (FileSystemInfoExtend fileSystem in fileSystems)
                {
                    if (fileSystem.IsDirectory)
                    {
                        if (!StringUtils.EqualsIgnoreCase(fileSystem.Name, "Template"))
                        {
                            bool isPublishmentSystemDirectory = false;
                            if (base.PublishmentSystemInfo.IsHeadquarters)
                            {
                                foreach (string publishmentSystemDir in publishmentSystemDirArrayList)
                                {
                                    if (StringUtils.EqualsIgnoreCase(publishmentSystemDir, fileSystem.Name))
                                    {
                                        isPublishmentSystemDirectory = true;
                                    }
                                }
                            }
                            if (!isPublishmentSystemDirectory && !DirectoryUtils.IsSystemDirectory(fileSystem.Name))
                            {
                                this.cblDirectoriesAndFiles.Items.Add(new ListItem(fileSystem.Name, fileSystem.Name.ToLower()));
                            }
                        }
                    }
                }
                foreach (FileSystemInfoExtend fileSystem in fileSystems)
                {
                    if (!fileSystem.IsDirectory)
                    {
                        if (!StringUtils.StartsWithIgnoreCase(fileSystem.Name, "T_") && !StringUtils.EqualsIgnoreCase(fileSystem.Name, "web.config"))
                        {
                            if (!PathUtility.IsSystemFile(fileSystem.Name))
                            {
                                this.cblDirectoriesAndFiles.Items.Add(new ListItem(fileSystem.Name, fileSystem.Name.ToLower()));
                            }
                        }
                    }
                }

                this.SetActivePlaceHolder(WizardPanel.Welcome, phWelcome);
			}
		}

        private string GetChannelTreeHtml()
        {
            StringBuilder htmlBuilder = new StringBuilder();

            string treeDirectoryUrl = PageUtils.GetIconUrl("tree");

            htmlBuilder.Append("<span id='ChannelSelectControl'>");
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
            bool[] isLastNodeArray = new bool[nodeIDArrayList.Count];
            foreach (int nodeID in nodeIDArrayList)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                int NodeID = nodeInfo.NodeID;
                string NodeName = nodeInfo.NodeName;
                int ParentsCount = nodeInfo.ParentsCount;
                int ChildrenCount = nodeInfo.ChildrenCount;
                bool IsLastNode = nodeInfo.IsLastNode;
                int ContentNum = nodeInfo.ContentNum;
                htmlBuilder.Append(this.GetChannelTreeTitle(nodeInfo, treeDirectoryUrl, isLastNodeArray));
                htmlBuilder.Append("<br/>");
            }
            htmlBuilder.Append("</span>");
            return htmlBuilder.ToString();
        }

        private string GetChannelTreeTitle(NodeInfo nodeInfo, string treeDirectoryUrl, bool[] isLastNodeArray)
        {
            StringBuilder itemBuilder = new StringBuilder();
            if (nodeInfo.NodeID == base.PublishmentSystemID)
            {
                nodeInfo.IsLastNode = true;
            }
            if (nodeInfo.IsLastNode == false)
            {
                isLastNodeArray[nodeInfo.ParentsCount] = false;
            }
            else
            {
                isLastNodeArray[nodeInfo.ParentsCount] = true;
            }
            for (int i = 0; i < nodeInfo.ParentsCount; i++)
            {
                if (isLastNodeArray[i])
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_empty.gif\"/>", treeDirectoryUrl);
                }
                else
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_line.gif\"/>", treeDirectoryUrl);
                }
            }
            if (nodeInfo.IsLastNode)
            {
                if (nodeInfo.ChildrenCount > 0)
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_plusbottom.gif\"/>", treeDirectoryUrl);
                }
                else
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_minusbottom.gif\"/>", treeDirectoryUrl);
                }
            }
            else
            {
                if (nodeInfo.ChildrenCount > 0)
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_plusmiddle.gif\"/>", treeDirectoryUrl);
                }
                else
                {
                    itemBuilder.AppendFormat("<img align=\"absmiddle\" src=\"{0}/tree_minusmiddle.gif\"/>", treeDirectoryUrl);
                }
            }

            itemBuilder.AppendFormat(@"<label class=""checkbox inline""><input type=""checkbox"" name=""NodeIDCollection"" value=""{0}""/> {1} {2}</label>", nodeInfo.NodeID, nodeInfo.NodeName, string.Format("&nbsp;<span style=\"font-size:8pt;font-family:arial\" class=\"gray\">({0})</span>", nodeInfo.ContentNum));

            return itemBuilder.ToString();
        }

		public WizardPanel CurrentWizardPanel
		{
			get
			{
				if (ViewState["WizardPanel"] != null)
					return (WizardPanel)ViewState["WizardPanel"];

				return WizardPanel.Welcome;
			}
			set
			{
				ViewState["WizardPanel"] = value;
			}
		}


		public enum WizardPanel
		{
			Welcome,
            SaveTemplates,
            SaveSiteContents,
			SaveFiles,
			UploadImageFile,
			Done,
			OperatingError,
		}

        void SetActivePlaceHolder(WizardPanel panel, PlaceHolder controlToShow)
		{
            PlaceHolder currentPanel = FindControl("ph" + CurrentWizardPanel.ToString()) as PlaceHolder;
			if (currentPanel != null)
				currentPanel.Visible = false;

			switch (panel)
			{
				case WizardPanel.Welcome:
					Previous.CssClass = "btn disabled";
                    Previous.Enabled = false;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
					break;
				case WizardPanel.Done:
                    Previous.CssClass = "btn disabled";
                    Previous.Enabled = false;
                    Next.CssClass = "btn btn-primary disabled";
                    Next.Enabled = false;
					break;
				case WizardPanel.OperatingError:
                    Previous.CssClass = "btn disabled";
                    Previous.Enabled = false;
                    Next.CssClass = "btn btn-primary disabled";
                    Next.Enabled = false;
					break;
				default:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
					break;
			}

			controlToShow.Visible = true;
			CurrentWizardPanel = panel;
		}

		public bool Validate_SaveFiles(out string errorMessage)
		{
			try
			{
                string independentTemplatePath = PathUtility.GetIndependentTemplatesPath(this.IndependentTemplateDir.Text);
                ArrayList lowerFileSystemArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.cblDirectoriesAndFiles);
                this.exportObject.ExportFilesToSite(independentTemplatePath, false, lowerFileSystemArrayList, false);
				errorMessage = "";
				return true;
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
				return false;
			}
		}

		public bool Validate_SaveTemplates(out string errorMessage)
		{
			try
			{
                List<int> templateIDList = new List<int>();
                templateIDList.AddRange(ControlUtils.GetSelectedListControlValueIntList(this.cblIndexTemplates));
                templateIDList.AddRange(ControlUtils.GetSelectedListControlValueIntList(this.cblChannelTemplates));
                templateIDList.AddRange(ControlUtils.GetSelectedListControlValueIntList(this.cblContentTemplates));
                templateIDList.AddRange(ControlUtils.GetSelectedListControlValueIntList(this.cblFileTemplates));

                if (templateIDList.Count > 0)
                {
                    string independentTemplatePath = PathUtility.GetIndependentTemplatesPath(this.IndependentTemplateDir.Text);
                    DirectoryUtils.CreateDirectoryIfNotExists(independentTemplatePath);

                    //导出模板
                    string templateFilePath = PathUtility.GetIndependentTemplateMetadataPath(independentTemplatePath, DirectoryUtils.IndependentTemplates.File_Template);
                    DirectoryUtils.CreateDirectoryIfNotExists(templateFilePath);
                    this.exportObject.ExportTemplates(templateFilePath, templateIDList);

                    errorMessage = "";
                    return true;
                }
                else
                {
                    errorMessage = "必须选择至少一个模板";
                    return false;
                }				
			}
			catch (Exception e)
			{
				errorMessage = e.Message;
				return false;
			}
		}

        public bool Validate_SaveSiteContents(out string errorMessage)
        {
            try
            {
                string independentTemplatePath = PathUtility.GetIndependentTemplatesPath(this.IndependentTemplateDir.Text);
                string siteContentDirectoryPath = PathUtility.GetIndependentTemplateMetadataPath(independentTemplatePath, DirectoryUtils.IndependentTemplates.SiteContent);

                bool isSaveContents = TranslateUtils.ToBool(this.rblIsSaveContents.SelectedValue);

                ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(Request.Form["NodeIDCollection"]);
                this.exportObject.ExportSiteContent(siteContentDirectoryPath, isSaveContents, false, nodeIDArrayList);

                errorMessage = "";
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

        public string GetUploadUrl()
        {
            return ConsoleAjaxUpload.GetIndependentTemplateImageUrlUploadUrl(this.IndependentTemplateDir.Text);
        }

		public void NextPanel(Object sender, EventArgs e)
		{
			string errorMessage = "";
			switch (CurrentWizardPanel)
			{
				case WizardPanel.Welcome:

					if (IndependentTemplateManager.Instance.IsIndependentTemplateDirectoryExists(this.IndependentTemplateDir.Text))
					{
						this.ltlErrorMessage.Text = "独立模板保存失败，独立模板已存在！";
                        this.SetActivePlaceHolder(WizardPanel.OperatingError, this.phOperatingError);
					}
					else
					{
                        this.SetActivePlaceHolder(WizardPanel.SaveTemplates, this.phSaveTemplates);
					}
					break;

                case WizardPanel.SaveTemplates:
                    if (Validate_SaveTemplates(out errorMessage))
					{
                        this.SetActivePlaceHolder(WizardPanel.SaveSiteContents, this.phSaveSiteContents);
					}
					else
					{
                        ltlErrorMessage.Text = errorMessage;
                        this.SetActivePlaceHolder(WizardPanel.OperatingError, this.phOperatingError);
					}
					break;

                case WizardPanel.SaveSiteContents:
                    if (Validate_SaveSiteContents(out errorMessage))
                    {
                        this.SetActivePlaceHolder(WizardPanel.SaveFiles, this.phSaveFiles);
                    }
                    else
                    {
                        ltlErrorMessage.Text = errorMessage;
                        this.SetActivePlaceHolder(WizardPanel.OperatingError, this.phOperatingError);
                    }
                    break;

                case WizardPanel.SaveFiles:
                    if (Validate_SaveFiles(out errorMessage))
                    {
                        this.SetActivePlaceHolder(WizardPanel.UploadImageFile, this.phUploadImageFile);
                    }
                    else
                    {
                        ltlErrorMessage.Text = errorMessage;
                        this.SetActivePlaceHolder(WizardPanel.OperatingError, this.phOperatingError);
                    }
                    break;

				case WizardPanel.UploadImageFile:

                    try
                    {
                        IndependentTemplateInfo independentTemplateInfo = new IndependentTemplateInfo();
                        independentTemplateInfo.IndependentTemplateName = this.IndependentTemplateName.Text;
                        independentTemplateInfo.PublishmentSystemType = EPublishmentSystemTypeUtils.GetValue(base.PublishmentSystemInfo.PublishmentSystemType);
                        List<string> templateList = new List<string>();
                        if (!string.IsNullOrEmpty(this.cblIndexTemplates.SelectedValue))
                        {
                            templateList.Add(ETemplateTypeUtils.GetValue(ETemplateType.IndexPageTemplate));
                        }
                        if (!string.IsNullOrEmpty(this.cblChannelTemplates.SelectedValue))
                        {
                            templateList.Add(ETemplateTypeUtils.GetValue(ETemplateType.ChannelTemplate));
                        }
                        if (!string.IsNullOrEmpty(this.cblContentTemplates.SelectedValue))
                        {
                            templateList.Add(ETemplateTypeUtils.GetValue(ETemplateType.ContentTemplate));
                        }
                        if (!string.IsNullOrEmpty(this.cblFileTemplates.SelectedValue))
                        {
                            templateList.Add(ETemplateTypeUtils.GetValue(ETemplateType.FileTemplate));
                        }
                        independentTemplateInfo.TemplateTypes = TranslateUtils.ObjectCollectionToString(templateList);
                        independentTemplateInfo.PicFileNames = base.Request.Form["imageUrls"];
                        independentTemplateInfo.WebSiteUrl = this.WebSiteUrl.Text;
                        independentTemplateInfo.Description = this.Description.Text;

                        string independentTemplatePath = PathUtility.GetIndependentTemplatesPath(this.IndependentTemplateDir.Text);
                        string xmlPath = PathUtility.GetIndependentTemplateMetadataPath(independentTemplatePath, DirectoryUtils.IndependentTemplates.File_Metadata);
                        Serializer.SaveAsXML(independentTemplateInfo, xmlPath);

                        this.SetActivePlaceHolder(WizardPanel.Done, this.phDone);
                    }
                    catch (Exception ex)
                    {
                        ltlErrorMessage.Text = ex.Message;
                        this.SetActivePlaceHolder(WizardPanel.OperatingError, this.phOperatingError);
                    }

					break;

				case WizardPanel.Done:
					break;
			}
		}

		public void PreviousPanel(Object sender, EventArgs e)
		{
			switch (CurrentWizardPanel)
			{
				case WizardPanel.Welcome:
					break;

                case WizardPanel.SaveTemplates:
                    this.SetActivePlaceHolder(WizardPanel.Welcome, this.phWelcome);
                    break;

                case WizardPanel.SaveSiteContents:
                    this.SetActivePlaceHolder(WizardPanel.SaveTemplates, this.phSaveTemplates);
                    break;

				case WizardPanel.SaveFiles:
                    this.SetActivePlaceHolder(WizardPanel.SaveSiteContents, this.phSaveSiteContents);
					break;

				case WizardPanel.UploadImageFile:
                    this.SetActivePlaceHolder(WizardPanel.SaveFiles, this.phSaveFiles);
					break;
			}
		}

	}
}
