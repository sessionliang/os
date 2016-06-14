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
    public class UploadImageSingleSystem : BackgroundBasePage
    {
        public HtmlInputFile hifUpload;
        public Literal ltlScript;

        string currentRootPath;
        string textBoxClientID;


        protected override bool IsSinglePage { get { return true; } }

        public static string GetOpenWindowStringToTextBox(string textBoxClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("TextBoxClientID", textBoxClientID);
            return PageUtility.GetOpenWindowString("上传图片", "modal_uploadImageSingleSystem.aspx", arguments, 480, 220);
        }

        public static string GetOpenWindowStringToList()
        { 
            return PageUtility.GetOpenWindowString("上传图片", "modal_uploadImageSingleSystem.aspx", null, 480, 220);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
             
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
                    string localDirectoryPath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\upload\\images";
                    //Server.MapPath(".");
                    DirectoryUtils.CreateDirectoryIfNotExists(localDirectoryPath);

                    string localFileName = this.GetUploadFileName(filePath, DateTime.Now, true) ;
                    string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                    if (!PathUtils.IsFileExtenstionAllowed("jpg|gif|png|bmp", fileExtName))
                    {
                        base.FailMessage("上传失败，上传图片格式不正确！");
                        return;
                    }
                    if (this.hifUpload.PostedFile.ContentLength > 10 * 1024)
                    {
                        base.FailMessage("上传失败，上传图片超出规定文件大小！");
                        return;
                    }

                    this.hifUpload.PostedFile.SaveAs(localFilePath);

                    bool isImage = EFileSystemTypeUtils.IsImage(fileExtName);


                    if (string.IsNullOrEmpty(this.textBoxClientID))
                    {
                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                    else
                    {
                        string textBoxUrl = "@/upload/images/" + localFileName;

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
                    base.FailMessage(ex, "图片上传失败！");
                }
            }
        }

        public string GetUploadFileName(string filePath, DateTime now, bool isUploadChangeFileName)
        {
            string retval = string.Empty;

            if (isUploadChangeFileName)
            {
                string strDateTime = string.Format("{0}{1}{2}{3}{4}", now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
                retval = string.Format("{0}{1}", strDateTime, PathUtils.GetExtension(filePath));
            }
            else
            {
                retval = PathUtils.GetFileName(filePath);
            }

            retval = StringUtils.ReplaceIgnoreCase(retval, "as", string.Empty);
            retval = StringUtils.ReplaceIgnoreCase(retval, ";", string.Empty);
            return retval;
        }

    }
}
