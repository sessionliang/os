using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Drawing;
using SiteServer.CMS.Core;
using BaiRong.Model;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser.TemplateDesign;

namespace SiteServer.STL.BackgroundPages.Modal.StlTemplate
{
	public class StlUploadImageSingle : BackgroundBasePage
	{
		public HtmlInputFile hifUpload;

        string currentRootPath;
        private string imageUrl;
        private int templateID;
        private string includeUrl;

        protected override bool IsSinglePage { get { return true; } }

        public static string GetOpenWindowStringToImageUrl(int publishmentSystemID, string imageUrl, int templateID, string includeUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("imageUrl", imageUrl);
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            return JsUtils.Layer.GetOpenLayerString("上传图片", PageUtils.GetSTLUrl("modal_stlUploadImageSingle.aspx"), arguments, 480, 220);
        }

        public static string GetOpenWindowStringToList(int publishmentSystemID, string currentRootPath)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("CurrentRootPath", currentRootPath);
            return JsUtils.Layer.GetOpenLayerString("上传图片", PageUtils.GetSTLUrl("modal_stlUploadImageSingle.aspx"), arguments, 480, 220);
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

            this.imageUrl = base.GetQueryString("imageUrl");
            this.templateID = TranslateUtils.ToInt(base.GetQueryString("templateID"));
            this.includeUrl = base.GetQueryString("includeUrl");
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

                    if (!PathUtility.IsImageExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                    {
                        base.FailMessage("上传失败，上传图片格式不正确！");
                        return;
                    }
                    if (!PathUtility.IsImageSizeAllowed(base.PublishmentSystemInfo, this.hifUpload.PostedFile.ContentLength))
                    {
                        base.FailMessage("上传失败，上传图片超出规定文件大小！");
                        return;
                    }

                    this.hifUpload.PostedFile.SaveAs(localFilePath);

                    bool isImage = EFileSystemTypeUtils.IsImage(fileExtName);

                    if (isImage)
                    {
                        FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);
                    }

                    if (string.IsNullOrEmpty(this.imageUrl))
                    {
                        JsUtils.Layer.CloseModalLayer(Page);
                    }
                    else
                    {
                        string uploadImageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);

                        TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, this.templateID);

                        string templateContent = TemplateDesignUndoRedo.GetTemplateContent(base.PublishmentSystemInfo, templateInfo, this.includeUrl);
                        templateContent = templateContent.Replace(this.imageUrl, uploadImageUrl);
                        TemplateDesignManager.UpdateTemplateInfo(base.PublishmentSystemInfo, templateInfo, this.includeUrl, templateContent);
                        JsUtils.Layer.CloseModalLayer(base.Page);
                    }
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "图片上传失败！");
				}
			}
		}

	}
}
