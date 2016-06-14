using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;


namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class ChangePublishmentSystemType : BackgroundBasePage
    {
        protected PlaceHolder HeadquartersExists;

        protected PlaceHolder ChangeToSite;
        protected TextBox PublishmentSystemDir;
        protected RegularExpressionValidator PublishmentSystemDirValidator;
        protected CheckBoxList FilesToSite;

        protected PlaceHolder ChangeToHeadquarters;
        protected RadioButtonList IsMoveFiles;

        private bool isHeadquarters = false;

        public string SiteName;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            string title = string.Empty;
            if (publishmentSystemInfo.IsHeadquarters)
            {
                title = "转移到子目录";
            }
            else
            {
                title = "转移到根目录";
            }
            return PageUtility.GetOpenWindowString(title, "modal_changePublishmentSystemType.aspx", arguments, 550, 500);
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.isHeadquarters = base.PublishmentSystemInfo.IsHeadquarters;

            ArrayList selectedList = new ArrayList();

            if (!Page.IsPostBack)
            {
                if (isHeadquarters)
                {
                    this.HeadquartersExists.Visible = false;
                    this.ChangeToSite.Visible = true;
                    this.ChangeToHeadquarters.Visible = false;
                    FileSystemInfoExtendCollection fileSystems = FileManager.GetFileSystemInfoExtendCollection(ConfigUtils.Instance.PhysicalApplicationPath, true);
                    ArrayList publishmentSystemDirArrayList = DataProvider.PublishmentSystemDAO.GetLowerPublishmentSystemDirArrayListThatNotIsHeadquarters();
                    foreach (FileSystemInfoExtend fileSystem in fileSystems)
                    {
                        if (fileSystem.IsDirectory)
                        {
                            if (!DirectoryUtils.IsSystemDirectory(fileSystem.Name) && !publishmentSystemDirArrayList.Contains(fileSystem.Name.ToLower()))
                            {
                                this.FilesToSite.Items.Add(new ListItem(fileSystem.Name, fileSystem.Name));
                            }
                        }
                        else
                        {
                            if (!PathUtility.IsSystemFileForChangePublishmentSystemType(fileSystem.Name))
                            {
                                this.FilesToSite.Items.Add(new ListItem(fileSystem.Name, fileSystem.Name));
                            }
                        }

                        if (PathUtility.IsWebSiteFile(fileSystem.Name) || DirectoryUtils.IsWebSiteDirectory(fileSystem.Name))
                        {
                            selectedList.Add(fileSystem.Name);
                        }

                    }

                    //主站下的单页模板
                    ArrayList fileTemplateInfoList = DataProvider.TemplateDAO.GetTemplateInfoArrayListByType(base.PublishmentSystemID, ETemplateType.FileTemplate);
                    foreach (TemplateInfo fileT in fileTemplateInfoList)
                    {
                        if (fileT.CreatedFileFullName.StartsWith("@/") || fileT.CreatedFileFullName.StartsWith("~/"))
                        {
                            string[] arr = fileT.CreatedFileFullName.Substring(2).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                            if (arr.Length > 0)
                                selectedList.Add(arr[0]);
                        }
                    }
                }
                else
                {
                    bool headquartersExists = false;
                    ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
                    foreach (int psID in publishmentSystemIDArrayList)
                    {
                        PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(psID);
                        if (psInfo.IsHeadquarters)
                        {
                            headquartersExists = true;
                            break;
                        }
                    }
                    if (headquartersExists)
                    {
                        this.HeadquartersExists.Visible = true;
                        this.ChangeToSite.Visible = false;
                        this.ChangeToHeadquarters.Visible = false;
                    }
                    else
                    {
                        this.HeadquartersExists.Visible = false;
                        this.ChangeToSite.Visible = false;
                        this.ChangeToHeadquarters.Visible = true;
                    }
                }

                //设置选中的文件以及文件夹
                ControlUtils.SelectListItems(FilesToSite, selectedList);
            }
        }

        public string GetSiteName()
        {
            return base.PublishmentSystemInfo.PublishmentSystemName;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;
            if (this.HeadquartersExists.Visible)
            {
                Page.Response.Clear();
                Page.Response.Write("<script language=\"javascript\">" + JsUtils.OpenWindow.HIDE_POP_WIN + "</script>");
                Page.Response.End();
            }
            else
            {
                try
                {
                    if (this.isHeadquarters)
                    {
                        ArrayList arraylist = DataProvider.NodeDAO.GetLowerSystemDirArrayList(base.PublishmentSystemInfo.ParentPublishmentSystemID);
                        if (arraylist.IndexOf(this.PublishmentSystemDir.Text.Trim().ToLower()) != -1)
                        {
                            this.PublishmentSystemDirValidator.IsValid = false;
                            this.PublishmentSystemDirValidator.ErrorMessage = "已存在相同的发布路径";
                        }
                        else
                        {
                            if (!DirectoryUtils.IsDirectoryNameCompliant(this.PublishmentSystemDir.Text))
                            {
                                this.PublishmentSystemDirValidator.IsValid = false;
                                this.PublishmentSystemDirValidator.ErrorMessage = "文件夹名称不符合要求";
                            }
                            else
                            {
                                ArrayList filesToSite = new ArrayList();
                                foreach (ListItem item in this.FilesToSite.Items)
                                {
                                    if (item.Selected)
                                    {
                                        filesToSite.Add(item.Value);
                                    }
                                }
                                DirectoryUtility.ChangeToSubSite(base.PublishmentSystemInfo, this.PublishmentSystemDir.Text, filesToSite);
                                isChanged = true;
                            }
                        }
                    }
                    else
                    {
                        DirectoryUtility.ChangeToHeadquarters(base.PublishmentSystemInfo, TranslateUtils.ToBool(this.IsMoveFiles.SelectedValue));

                        isChanged = true;
                    }
                }
                catch (Exception ex)
                {
                    PageUtils.RedirectToErrorPage(string.Format("修改失败：{0}", ex.Message));
                    return;
                }
            }

            if (isChanged)
            {
                if (this.isHeadquarters)
                {
                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "转移到子目录", string.Format("应用:{0}", base.PublishmentSystemInfo.PublishmentSystemName));
                }
                else
                {
                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "转移到根目录", string.Format("应用:{0}", base.PublishmentSystemInfo.PublishmentSystemName));
                }
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
    }
}
