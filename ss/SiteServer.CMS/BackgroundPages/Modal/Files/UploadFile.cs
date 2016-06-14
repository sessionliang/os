using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using BaiRong.Model.Service;
using System.Web.UI.WebControls;
using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class UploadFile : BackgroundBasePage
	{
        public HtmlInputFile hifUpload;
        public RadioButtonList rblIsFileUploadChangeFileName;
        public Literal ltlScript;

        private EUploadType uploadType;
        private string realtedPath;
        private string textBoxClientID;

        public static string GetOpenWindowStringToTextBox(int publishmentSystemID, EUploadType uploadType, string textBoxClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("uploadType", EUploadTypeUtils.GetValue(uploadType));
            arguments.Add("TextBoxClientID", textBoxClientID);
            return PageUtility.GetOpenWindowString("上传附件", "modal_uploadFile.aspx", arguments, 480, 300);
        }

        public static string GetOpenWindowStringToList(int publishmentSystemID, EUploadType uploadType, string realtedPath)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("uploadType", EUploadTypeUtils.GetValue(uploadType));
            arguments.Add("realtedPath", realtedPath);
            return PageUtility.GetOpenWindowString("上传附件", "modal_uploadFile.aspx", arguments, 480, 300);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.uploadType = EUploadTypeUtils.GetEnumType(this.GetQueryString("uploadType"));
            this.realtedPath = base.GetQueryString("realtedPath");
            this.textBoxClientID = base.GetQueryString("TextBoxClientID");

            if (!base.IsPostBack)
            {
                EBooleanUtils.AddListItems(this.rblIsFileUploadChangeFileName, "采用系统生成文件名", "采用原有文件名");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsFileUploadChangeFileName, base.PublishmentSystemInfo.Additional.IsFileUploadChangeFileName.ToString());
            }
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (this.hifUpload.PostedFile != null && "" != this.hifUpload.PostedFile.FileName)
            {
                string filePath = this.hifUpload.PostedFile.FileName;
                
				try
				{
                    string fileExtName = PathUtils.GetExtension(filePath).ToLower();
                    string localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                    if (!string.IsNullOrEmpty(this.realtedPath))
					{
                        localDirectoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, this.realtedPath);
                        DirectoryUtils.CreateDirectoryIfNotExists(localDirectoryPath);
					}
                    string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath, DateTime.Now, TranslateUtils.ToBool(this.rblIsFileUploadChangeFileName.SelectedValue));

                    string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                    if (this.uploadType == EUploadType.Image && !EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                    {
                        base.FailMessage("此格式不允许上传，此文件夹只允许上传图片以及音视频文件！");
                        return;
                    }
                    if (this.uploadType == EUploadType.Video && !EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                    {
                        base.FailMessage("此格式不允许上传，此文件夹只允许上传图片以及音视频文件！");
                        return;
                    }
                    if (this.uploadType == EUploadType.File && !PathUtility.IsFileExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                    {
                        base.FailMessage("此格式不允许上传，请选择有效的文件！");
                        return;
                    }
                    
                    if (!PathUtility.IsFileSizeAllowed(base.PublishmentSystemInfo, this.hifUpload.PostedFile.ContentLength))
                    {
                        base.FailMessage("上传失败，上传文件超出规定文件大小！");
                        return;
                    }

                    this.hifUpload.PostedFile.SaveAs(localFilePath);

					FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);

                    string fileUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                    string textBoxUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, fileUrl);

                    if (string.IsNullOrEmpty(this.textBoxClientID))
                    {
                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                    else
                    {
                        this.ltlScript.Text += string.Format(@"
if (parent.document.getElementById('{0}') != null)
{{
    parent.document.getElementById('{0}').value = '{1}';
}}
", this.textBoxClientID, textBoxUrl);

                        this.ltlScript.Text += JsUtils.OpenWindow.HIDE_POP_WIN;
                    }
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "文件上传失败");
				}
			}
		}

	}
}
