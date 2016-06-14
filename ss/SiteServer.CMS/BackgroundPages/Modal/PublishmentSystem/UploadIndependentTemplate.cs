using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using System.Web.UI.WebControls;
using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class UploadIndependentTemplate : BackgroundBasePage
	{
        public RadioButtonList rblImportType;

        public PlaceHolder phUpload;
		public HtmlInputFile myFile;

        public PlaceHolder phDownload;
        public TextBox tbDownloadUrl;

        public static string GetOpenWindowString()
        {
            NameValueCollection arguments = new NameValueCollection();
            return PageUtility.GetOpenWindowString("导入独立模板", "modal_uploadIndependentTemplate.aspx", arguments, 460, 300);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!base.Page.IsPostBack)
            {
                EBooleanUtils.AddListItems(this.rblImportType, "上传压缩包并导入", "从指定地址下载压缩包并导入");
                ControlUtils.SelectListItemsIgnoreCase(this.rblImportType, true.ToString());

                this.phUpload.Visible = true;
                this.phDownload.Visible = false;
            }
		}

        public void rblImportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TranslateUtils.ToBool(this.rblImportType.SelectedValue))
            {
                this.phUpload.Visible = true;
                this.phDownload.Visible = false;
            }
            else
            {
                this.phUpload.Visible = false;
                this.phDownload.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isUpload = TranslateUtils.ToBool(this.rblImportType.SelectedValue);
            if (isUpload)
            {
                if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
                {
                    string filePath = myFile.PostedFile.FileName;
                    string sExt = PathUtils.GetExtension(filePath);
                    if (!StringUtils.EqualsIgnoreCase(sExt, ".zip"))
                    {
                        base.FailMessage("独立模板压缩包为zip格式，请选择有效的文件上传");
                        return;
                    }
                    try
                    {
                        string directoryName = PathUtils.GetFileNameWithoutExtension(filePath);
                        string directoryPath = PathUtility.GetIndependentTemplatesPath(directoryName);
                        if (DirectoryUtils.IsDirectoryExists(directoryPath))
                        {
                            base.FailMessage(string.Format("独立模板导入失败，文件夹{0}已存在", directoryName));
                            return;
                        }
                        string localFilePath = PathUtility.GetIndependentTemplatesPath(directoryName + ".zip");
                        FileUtils.DeleteFileIfExists(localFilePath);

                        myFile.PostedFile.SaveAs(localFilePath);

                        ZipUtils.UnpackFiles(localFilePath, directoryPath);

                        JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, PageUtils.GetSTLUrl("console_independentTemplate.aspx"));
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "文件上传失败！");
                    }
                }
            }
            else
            {
                string sExt = PathUtils.GetExtension(this.tbDownloadUrl.Text);
                if (!StringUtils.EqualsIgnoreCase(sExt, ".zip"))
                {
                    base.FailMessage("独立模板压缩包为zip格式，请输入有效文件地址");
                    return;
                }
                string directoryName = PathUtils.GetFileNameWithoutExtension(this.tbDownloadUrl.Text);
                PageUtils.Redirect(Modal.ProgressBar.GetRedirectUrlStringWithIndependentTemplateDownload(this.tbDownloadUrl.Text, directoryName));
            }
		}

	}
}
