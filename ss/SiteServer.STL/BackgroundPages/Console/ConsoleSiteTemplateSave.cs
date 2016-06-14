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

namespace SiteServer.STL.BackgroundPages
{
    public class ConsoleSiteTemplateSave : BackgroundBasePage
    {
        public PlaceHolder phWelcome;
        public TextBox SiteTemplateName;
        public TextBox SiteTemplateDir;
        public TextBox WebSiteUrl;
        public TextBox Description;

        public PlaceHolder phSaveFiles;
        public RadioButtonList rblIsSaveAllFiles;
        public PlaceHolder phDirectoriesAndFiles;
        public CheckBoxList cblDirectoriesAndFiles;

        public PlaceHolder phSaveSiteContents;
        public RadioButtonList rblIsSaveContents;
        public RadioButtonList rblIsSaveAllChannels;
        public PlaceHolder phChannels;
        public Literal ltlChannelTree;

        public PlaceHolder phSaveSiteStyles;

        public PlaceHolder phUploadImageFile;
        public HtmlInputFile SamplePicFile;

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
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "保存应用模板", AppManager.Platform.Permission.Platform_Site);

                if (base.PublishmentSystemInfo.IsHeadquarters)
                {
                    this.SiteTemplateDir.Text = "T_" + base.PublishmentSystemInfo.PublishmentSystemName;
                }
                else
                {
                    this.SiteTemplateDir.Text = "T_" + base.PublishmentSystemInfo.PublishmentSystemDir.Replace("\\", "_");
                }
                this.SiteTemplateName.Text = base.PublishmentSystemInfo.PublishmentSystemName;

                EBooleanUtils.AddListItems(this.rblIsSaveAllFiles, "全部文件", "指定文件");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSaveAllFiles, true.ToString());

                ArrayList publishmentSystemDirArrayList = DataProvider.PublishmentSystemDAO.GetLowerPublishmentSystemDirArrayListThatNotIsHeadquarters();
                FileSystemInfoExtendCollection fileSystems = FileManager.GetFileSystemInfoExtendCollection(PathUtility.GetPublishmentSystemPath(base.PublishmentSystemInfo), true);
                foreach (FileSystemInfoExtend fileSystem in fileSystems)
                {
                    if (fileSystem.IsDirectory)
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
                foreach (FileSystemInfoExtend fileSystem in fileSystems)
                {
                    if (!fileSystem.IsDirectory)
                    {
                        if (!StringUtils.EqualsIgnoreCase(fileSystem.Name, "web.config"))
                        {
                            if (!PathUtility.IsSystemFile(fileSystem.Name))
                            {
                                this.cblDirectoriesAndFiles.Items.Add(new ListItem(fileSystem.Name, fileSystem.Name.ToLower()));
                            }
                        }
                    }
                }

                EBooleanUtils.AddListItems(this.rblIsSaveContents, "保存内容数据", "不保存内容数据");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSaveContents, true.ToString());

                EBooleanUtils.AddListItems(this.rblIsSaveAllChannels, "全部栏目", "指定栏目");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsSaveAllChannels, true.ToString());

                this.ltlChannelTree.Text = this.GetChannelTreeHtml();

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

        public void rblIsSaveAllFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phDirectoriesAndFiles.Visible = !TranslateUtils.ToBool(this.rblIsSaveAllFiles.SelectedValue);
        }

        public void rblIsSaveAllChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phChannels.Visible = !TranslateUtils.ToBool(this.rblIsSaveAllChannels.SelectedValue);
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
            SaveFiles,
            SaveSiteContents,
            SaveSiteStyles,
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

        public bool Validate_UploadImageFile(out string errorMessage, out string samplePicPath)
        {
            if (this.SamplePicFile.PostedFile != null && "" != this.SamplePicFile.PostedFile.FileName)
            {
                string filePath = this.SamplePicFile.PostedFile.FileName;
                string fileExtName = filePath.ToLower().Substring(filePath.LastIndexOf("."));
                if (fileExtName == ".jpg" || fileExtName == ".jpeg" || fileExtName == ".gif" || fileExtName == ".bmp" || fileExtName == ".png" || fileExtName == ".tif")
                {
                    try
                    {
                        string siteTemplatePath = PathUtility.GetSiteTemplatesPath(this.SiteTemplateDir.Text);
                        string localFilePath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, this.SiteTemplateDir.Text + fileExtName);

                        SamplePicFile.PostedFile.SaveAs(localFilePath);
                        samplePicPath = this.SiteTemplateDir.Text + fileExtName;
                        errorMessage = "";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                        samplePicPath = "";
                        return false;
                    }
                }
                else
                {
                    errorMessage = "网站样图不是有效的图片文件";
                    samplePicPath = "";
                    return false;
                }
            }
            else
            {
                errorMessage = "";
                samplePicPath = "";
                return true;
            }
        }

        public bool Validate_SaveFiles(out string errorMessage)
        {
            try
            {
                string siteTemplatePath = PathUtility.GetSiteTemplatesPath(this.SiteTemplateDir.Text);
                bool isSaveAll = TranslateUtils.ToBool(this.rblIsSaveAllFiles.SelectedValue);
                ArrayList lowerFileSystemArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.cblDirectoriesAndFiles);
                this.exportObject.ExportFilesToSite(siteTemplatePath, isSaveAll, lowerFileSystemArrayList, true);
                errorMessage = "";
                return true;
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
                string siteTemplatePath = PathUtility.GetSiteTemplatesPath(this.SiteTemplateDir.Text);
                string siteContentDirectoryPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.SiteContent);

                bool isSaveContents = TranslateUtils.ToBool(this.rblIsSaveContents.SelectedValue);
                bool isSaveAllChannels = TranslateUtils.ToBool(this.rblIsSaveAllChannels.SelectedValue);

                ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(Request.Form["NodeIDCollection"]);
                this.exportObject.ExportSiteContent(siteContentDirectoryPath, isSaveContents, isSaveAllChannels, nodeIDArrayList);

                errorMessage = "";
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }


        public bool Validate_SaveSiteStyles(out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                SiteTemplateManager.ExportPublishmentSystemToSiteTemplate(base.PublishmentSystemInfo, this.SiteTemplateDir.Text);

                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }


        public void NextPanel(Object sender, EventArgs e)
        {
            string errorMessage = "";
            switch (CurrentWizardPanel)
            {
                case WizardPanel.Welcome:

                    if (SiteTemplateManager.Instance.IsSiteTemplateDirectoryExists(this.SiteTemplateDir.Text))
                    {
                        this.ltlErrorMessage.Text = "应用模板保存失败，应用模板已存在！";
                        this.SetActivePlaceHolder(WizardPanel.OperatingError, this.phOperatingError);
                    }
                    else
                    {
                        this.SetActivePlaceHolder(WizardPanel.SaveFiles, this.phSaveFiles);
                    }
                    break;

                case WizardPanel.SaveFiles:
                    if (Validate_SaveFiles(out errorMessage))
                    {
                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "保存应用模板", string.Format("应用:{0}", base.PublishmentSystemInfo.PublishmentSystemName));

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
                        this.SetActivePlaceHolder(WizardPanel.SaveSiteStyles, this.phSaveSiteStyles);
                    }
                    else
                    {
                        ltlErrorMessage.Text = errorMessage;
                        this.SetActivePlaceHolder(WizardPanel.OperatingError, this.phOperatingError);
                    }
                    break;

                case WizardPanel.SaveSiteStyles:
                    if (Validate_SaveSiteStyles(out errorMessage))
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

                    string samplePicPath = "";
                    if (Validate_UploadImageFile(out errorMessage, out samplePicPath))
                    {
                        try
                        {
                            SiteTemplateInfo siteTemplateInfo = new SiteTemplateInfo();
                            siteTemplateInfo.SiteTemplateName = this.SiteTemplateName.Text;
                            siteTemplateInfo.PublishmentSystemType = EPublishmentSystemTypeUtils.GetValue(base.PublishmentSystemInfo.PublishmentSystemType);
                            siteTemplateInfo.PicFileName = samplePicPath;
                            siteTemplateInfo.WebSiteUrl = this.WebSiteUrl.Text;
                            siteTemplateInfo.Description = this.Description.Text;

                            string siteTemplatePath = PathUtility.GetSiteTemplatesPath(this.SiteTemplateDir.Text);
                            string xmlPath = PathUtility.GetSiteTemplateMetadataPath(siteTemplatePath, DirectoryUtils.SiteTemplates.File_Metadata);
                            Serializer.SaveAsXML(siteTemplateInfo, xmlPath);

                            this.SetActivePlaceHolder(WizardPanel.Done, this.phDone);
                        }
                        catch (Exception ex)
                        {
                            ltlErrorMessage.Text = ex.Message;
                            this.SetActivePlaceHolder(WizardPanel.OperatingError, this.phOperatingError);
                        }

                    }
                    else
                    {
                        ltlErrorMessage.Text = errorMessage;
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

                case WizardPanel.SaveFiles:
                    this.SetActivePlaceHolder(WizardPanel.Welcome, this.phWelcome);
                    break;

                case WizardPanel.SaveSiteContents:
                    this.SetActivePlaceHolder(WizardPanel.SaveFiles, this.phSaveFiles);
                    break;

                case WizardPanel.SaveSiteStyles:
                    this.SetActivePlaceHolder(WizardPanel.SaveSiteContents, this.phSaveSiteContents);
                    break;

                case WizardPanel.UploadImageFile:
                    this.SetActivePlaceHolder(WizardPanel.SaveSiteStyles, this.phSaveSiteStyles);
                    break;
            }
        }

    }
}
