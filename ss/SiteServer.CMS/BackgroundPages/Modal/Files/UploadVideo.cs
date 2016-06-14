using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Drawing;
using SiteServer.CMS.Core;
using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class UploadVideo : BackgroundBasePage
    {
        public HtmlInputFile hifUpload;
        public Literal ltlScript;

        string currentRootPath;
        string textBoxClientID;

        public static string GetOpenWindowStringToTextBox(int publishmentSystemID, string textBoxClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("TextBoxClientID", textBoxClientID);
            return PageUtility.GetOpenWindowString("上传视频", "modal_uploadVideo.aspx", arguments, 480, 220);
        }

        public static string GetOpenWindowStringToList(int publishmentSystemID, string currentRootPath)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("CurrentRootPath", currentRootPath);
            return PageUtility.GetOpenWindowString("上传视频", "modal_uploadVideo.aspx", arguments, 480, 220);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.currentRootPath = base.GetQueryString("CurrentRootPath");
            if (!string.IsNullOrEmpty(this.currentRootPath) && !this.currentRootPath.StartsWith("@"))
            {
                this.currentRootPath = "@/" + this.currentRootPath;
            }
            this.textBoxClientID = base.GetQueryString("TextBoxClientID");
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
                    if (!string.IsNullOrEmpty(this.currentRootPath))
                    {
                        localDirectoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, this.currentRootPath);
                        DirectoryUtils.CreateDirectoryIfNotExists(localDirectoryPath);
                    }
                    string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);
                    string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                    if (!PathUtility.IsVideoExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                    {
                        base.FailMessage("上传失败，上传视频格式不正确！");
                        return;
                    }
                    if (!PathUtility.IsVideoSizeAllowed(base.PublishmentSystemInfo, this.hifUpload.PostedFile.ContentLength))
                    {
                        base.FailMessage("上传失败，上传视频超出规定文件大小！");
                        return;
                    }

                    this.hifUpload.PostedFile.SaveAs(localFilePath);

                    string videoUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                    string textBoxUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, videoUrl);

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
                catch (Exception ex)
                {
                    base.FailMessage(ex, "视频上传失败！");
                }
            }
        }

    }
}
