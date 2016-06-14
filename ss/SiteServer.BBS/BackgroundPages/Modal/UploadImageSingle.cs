using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Drawing;

using BaiRong.Model;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class UploadImageSingle : BackgroundBasePage
	{
        public HtmlInputFile myFile;

		string Scripting = "";

		string currentRootPath;
		string textBoxClientID;

        public static string GetOpenWindowString(int publishmentSystemID, int forumID, string currentRootPath, string textBoxClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("forumID", forumID.ToString());
            arguments.Add("CurrentRootPath", currentRootPath);
            arguments.Add("TextBoxClientID", textBoxClientID);
            return JsUtils.OpenWindow.GetOpenWindowString("上传图片", PageUtils.GetBBSUrl("modal_uploadImageSingle.aspx"), arguments, 540, 200);
        }

        public static string GetRedirectUrl(int publishmentSystemID, int forumID, string currentRootPath, string textBoxClientID)
        {
            return PageUtils.GetBBSUrl(string.Format("modal_uploadImageSingle.aspx?publishmentSystemID={0}&forumID={1}&CurrentRootPath={2}&TextBoxClientID={3}", publishmentSystemID, forumID, currentRootPath, textBoxClientID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("ForumID");
            this.currentRootPath = base.GetQueryString("CurrentRootPath");
            this.textBoxClientID = base.GetQueryString("TextBoxClientID");

            if (!IsPostBack)
            {
            }
		}

		private static int GetImageWidth(string imageSize)
		{
			string width = imageSize.Substring(0, imageSize.IndexOf(","));
			return int.Parse(width);
		}

		private static int GetImageHeight(string imageSize)
		{
			string height = imageSize.Substring(imageSize.IndexOf(",") + 1);
			return int.Parse(height);
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
			{
				string filePath = myFile.PostedFile.FileName;
				try
				{
                    string localDirectoryPath = PathUtilityBBS.GetUploadDirectoryPath(base.PublishmentSystemID);
					if (!string.IsNullOrEmpty(this.currentRootPath))
					{
                        localDirectoryPath = PathUtilityBBS.MapPath(this.currentRootPath);
					}
					string localFileName = PathUtilityBBS.GetUploadFileName(base.PublishmentSystemID, filePath);
                    string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

					string fileExtName = System.IO.Path.GetExtension(filePath).ToLower();

                    if (EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                    {
                        if (myFile.PostedFile.ContentLength > 10240 * 1024)
                        {
                            base.FailMessage("上传失败，上传文件超出规定文件大小！");
                            return;
                        }
                        myFile.PostedFile.SaveAs(localFilePath);

                        StringUtilityBBS.AddLog(base.PublishmentSystemID, "上传图片", string.Format("图片名:{0}", localFileName));

                        bool isImage = EFileSystemTypeUtils.IsImage(fileExtName);

                        if (isImage)
                        {
                            FileUtilityBBS.AddWaterMark(base.PublishmentSystemID, localFilePath);
                        }

                        Scripting = "<script type=\"text/javascript\" language=\"javascript\">";

                        string imageUrl = PageUtilityBBS.GetBBSUrlByPhysicalPath(base.PublishmentSystemID, localFilePath);
                        string textBoxUrl = PageUtilityBBS.GetVirtualUrl(imageUrl);

                        Scripting += string.Format(@"
if (parent.document.getElementById('{0}') != null)
{{
    parent.document.getElementById('{0}').value = '{1}';
}}
{2}
", this.textBoxClientID, textBoxUrl, JsUtils.OpenWindow.HIDE_POP_WIN);

                        if (isImage)
                        {
                            System.Drawing.Image uploadImage = System.Drawing.Image.FromFile(localFilePath);
                            uploadImage.Dispose();
                        }

                        Scripting += "parent.hidePopWin();</script>";
                        base.SuccessMessage("文件上传成功！");
                    }
					else
					{
                        base.FailMessage("您必须上传图片文件！");
					}
				}
				catch(Exception ex)
				{
					base.FailMessage(ex, "文件上传失败！");
				}
			}
		}

	}
}
