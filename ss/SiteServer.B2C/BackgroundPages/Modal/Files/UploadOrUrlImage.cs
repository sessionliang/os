using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Drawing;
using SiteServer.B2C.Core;
using BaiRong.Model;
using System.Text;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages.Modal
{
	public class UploadOrUrlImage : BackgroundBasePage
	{
        public RadioButtonList rblIsUpload;
		public HtmlInputFile myFile;
        public TextBox tbImageUrl;

        public PlaceHolder phUpload;
        public PlaceHolder phUrl;

        public Literal ltlScript;

        private string inputClientID;
        private string imgClientID;
        private string originalUrl;

        public static string GetOpenWindowString(int publishmentSystemID, string inputClientID, string imgClientID, string originalUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("InputClientID", inputClientID);
            arguments.Add("ImgClientID", imgClientID);
            arguments.Add("OriginalUrl", originalUrl);
            return PageUtilityB2C.GetOpenWindowString("设置图片", "modal_uploadOrUrlImage.aspx", arguments, 540, 340);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.inputClientID = base.GetQueryString("InputClientID");
            this.imgClientID = base.GetQueryString("ImgClientID");
            this.originalUrl = base.GetQueryString("OriginalUrl");

            myFile.Attributes.Add("onchange", "InputChange(this)");

            if (!IsPostBack)
            {
                EBooleanUtils.AddListItems(this.rblIsUpload, "上传图片", "网络地址图片");

                if (!string.IsNullOrEmpty(this.originalUrl))
                {
                    this.tbImageUrl.Text = this.originalUrl;
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsUpload, false.ToString());
                    this.phUpload.Visible = false;
                }
                else
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsUpload, true.ToString());
                    this.phUrl.Visible = false;
                }
            }
		}

        public void rblIsUpload_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TranslateUtils.ToBool(this.rblIsUpload.SelectedValue))
            {
                this.phUpload.Visible = true;
                this.phUrl.Visible = false;
            }
            else
            {
                this.phUpload.Visible = false;
                this.phUrl.Visible = true;
            }
        }

		public override void Submit_OnClick(object sender, EventArgs E)
		{
            StringBuilder scriptBuilder = new StringBuilder();
            if (TranslateUtils.ToBool(this.rblIsUpload.SelectedValue))
            {
                if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
                {
                    string filePath = myFile.PostedFile.FileName;
                    try
                    {
                        string fileExtName = System.IO.Path.GetExtension(filePath).ToLower();
                        string localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                        string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);
                        string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                        if (!PathUtility.IsImageExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                        {
                            base.FailMessage("上传失败，上传图片格式不正确！");
                            return;
                        }

                        if (!PathUtility.IsImageSizeAllowed(base.PublishmentSystemInfo, this.myFile.PostedFile.ContentLength))
                        {
                            base.FailMessage("上传失败，上传图片超出规定文件大小！");
                            return;
                        }

                        if (EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                        {
                            //if (myFile.PostedFile.ContentLength > base.PublishmentSystemInfo.Additional.UploadImageTypeMaxSize * 1024)
                            //{
                            //    base.FailMessage("上传失败，上传文件超出规定文件大小！");
                            //    return;
                            //}
                            myFile.PostedFile.SaveAs(localFilePath);

                            StringUtility.AddLog(base.PublishmentSystemID, "上传图片", string.Format("图片名:{0}", localFileName));

                            bool isImage = EFileSystemTypeUtils.IsImage(fileExtName);

                            if (isImage)
                            {
                                FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);
                            }

                            string imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);

                            string virtualUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, imageUrl);

                            if (!string.IsNullOrEmpty(this.inputClientID))
                            {
                                scriptBuilder.AppendFormat(@"
if (parent.document.getElementById('{0}'))
{{
    parent.document.getElementById('{0}').value = '{1}';
}}
", this.inputClientID, virtualUrl);
                            }
                            if (!string.IsNullOrEmpty(this.imgClientID))
                            {
                                scriptBuilder.AppendFormat(@"
if (parent.document.getElementById('{0}'))
{{
    parent.document.getElementById('{0}').setAttribute('src', '{1}');
}}
", this.imgClientID, imageUrl);
                            }
                            scriptBuilder.Append(JsUtils.OpenWindow.HIDE_POP_WIN);

                            if (isImage)
                            {
                                System.Drawing.Image uploadImage = System.Drawing.Image.FromFile(localFilePath);
                                uploadImage.Dispose();
                            }

                            base.SuccessMessage("文件上传成功！");
                        }
                        else
                        {
                            base.FailMessage("您必须上传图片文件！");
                        }
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "文件上传失败！");
                    }
                }
            }
            else
            {
                string virtualUrl = this.tbImageUrl.Text;
                string imageUrl = PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, virtualUrl);

                if (!string.IsNullOrEmpty(this.inputClientID))
                {
                    scriptBuilder.AppendFormat(@"
if (parent.document.getElementById('{0}'))
{{
    parent.document.getElementById('{0}').value = '{1}';
}}
", this.inputClientID, virtualUrl);
                }
                if (!string.IsNullOrEmpty(this.imgClientID))
                {
                    if (!string.IsNullOrEmpty(this.tbImageUrl.Text))
                    {
                        scriptBuilder.AppendFormat(@"
if (parent.document.getElementById('{0}'))
{{
    parent.document.getElementById('{0}').setAttribute('src', '{1}');
    parent.document.getElementById('{0}').style.display='';
}}
", this.imgClientID, imageUrl);
                    }
                    else
                    {
                        scriptBuilder.AppendFormat(@"
if (parent.document.getElementById('{0}'))
{{
    parent.document.getElementById('{0}').style.display='none';
}}
", this.imgClientID, imageUrl);
                    }
                }
                scriptBuilder.Append(JsUtils.OpenWindow.HIDE_POP_WIN);
            }

            this.ltlScript.Text = scriptBuilder.ToString();
		}

	}
}
