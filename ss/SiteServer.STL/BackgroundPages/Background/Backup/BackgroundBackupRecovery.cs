using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;


using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundBackupRecovery : BackgroundBasePage
	{
		public DropDownList BackupType;
        public PlaceHolder PlaceHolder_Delete;
        public RadioButtonList IsDeleteChannels;
        public RadioButtonList IsDeleteTemplates;
        public RadioButtonList IsDeleteFiles;
        public RadioButtonList IsOverride;
        public RadioButtonList IsRecoveryByUpload;

        public PlaceHolder PlaceHolderByUpload;
        public HtmlInputFile myFile;

		public Button RecoveryButton;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Backup, "数据恢复", AppManager.CMS.Permission.WebSite.Backup);

                EBackupTypeUtils.AddListItems(this.BackupType);
                ControlUtils.SelectListItems(this.BackupType, EBackupTypeUtils.GetValue(EBackupType.Templates));

                EBooleanUtils.AddListItems(this.IsRecoveryByUpload, "从上传文件中恢复", "从服务器备份文件中恢复");
                ControlUtils.SelectListItems(this.IsRecoveryByUpload, true.ToString());

                this.Options_SelectedIndexChanged(null, EventArgs.Empty);
			}
		}

        public void Options_SelectedIndexChanged(object sender, EventArgs e)
        {
            EBackupType backupType = EBackupTypeUtils.GetEnumType(this.BackupType.SelectedValue);
            if (backupType == EBackupType.Site)
            {
                this.PlaceHolder_Delete.Visible = true;
            }
            else
            {
                this.PlaceHolder_Delete.Visible = false;
            }

            this.PlaceHolderByUpload.Visible = TranslateUtils.ToBool(this.IsRecoveryByUpload.SelectedValue);
        }


		public void RecoveryButton_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                if (TranslateUtils.ToBool(this.IsRecoveryByUpload.SelectedValue))
                {
                    if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
                    {
                        string filePath = myFile.PostedFile.FileName;
                        if (EBackupTypeUtils.Equals(EBackupType.Templates, this.BackupType.SelectedValue))
                        {
                            if (EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(filePath)) != EFileSystemType.Xml)
                            {
                                base.FailMessage("必须上传Xml文件");
                                return;
                            }
                        }
                        else
                        {
                            if (!EFileSystemTypeUtils.IsCompressionFile(PathUtils.GetExtension(filePath)))
                            {
                                base.FailMessage("必须上传压缩文件");
                                return;
                            }
                        }

                        try
                        {
                            string localFilePath = PathUtils.GetTemporaryFilesPath(PathUtils.GetFileName(filePath));

                            myFile.PostedFile.SaveAs(localFilePath);

                            ImportObject importObject = new ImportObject(base.PublishmentSystemID);

                            if (EBackupTypeUtils.Equals(EBackupType.Templates, this.BackupType.SelectedValue))
                            {
                                importObject.ImportTemplates(localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue));
                                base.SuccessMessage("恢复模板成功!");
                            }
                            else if (EBackupTypeUtils.Equals(EBackupType.ChannelsAndContents, this.BackupType.SelectedValue))
                            {
                                importObject.ImportChannelsAndContentsByZipFile(0, localFilePath, TranslateUtils.ToBool(this.IsOverride.SelectedValue));
                                base.SuccessMessage("恢复栏目及内容成功!");
                            }
                            else if (EBackupTypeUtils.Equals(EBackupType.Files, this.BackupType.SelectedValue))
                            {
                                string filesDirectoryPath = PathUtils.GetTemporaryFilesPath(EBackupTypeUtils.GetValue(EBackupType.Files));
                                DirectoryUtils.DeleteDirectoryIfExists(filesDirectoryPath);
                                DirectoryUtils.CreateDirectoryIfNotExists(filesDirectoryPath);

                                ZipUtils.UnpackFiles(localFilePath, filesDirectoryPath);

                                importObject.ImportFiles(filesDirectoryPath, TranslateUtils.ToBool(this.IsOverride.SelectedValue));
                                base.SuccessMessage("恢复文件成功!");
                            }
                            else if (EBackupTypeUtils.Equals(EBackupType.Site, this.BackupType.SelectedValue))
                            {
                                string userKeyPrefix = StringUtils.GUID();
                                PageUtils.Redirect(BackgroundProgressBar.GetRecoveryUrl(base.PublishmentSystemID, this.IsDeleteChannels.SelectedValue, this.IsDeleteTemplates.SelectedValue, this.IsDeleteFiles.SelectedValue, true, localFilePath, this.IsOverride.SelectedValue, this.IsOverride.SelectedValue, userKeyPrefix));
                            }
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "数据恢复失败！");
                        }
                    }
                }
			}
		}
	}
}