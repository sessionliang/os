using System ;
using System.Globalization ;
using System.Xml ;
using System.Web ;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using BaiRong.Model;

using BaiRong.Core;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.TextEditorXHtmlEditor
{
    public class UploadImage : BackgroundBasePage
	{
        public Literal ltlScript;

        protected override bool IsSinglePage
        {
            get
            {
                return true;
            }
        }

        protected override bool IsAccessable
        {
            get
            {
                if (BaiRongDataProvider.UserDAO.IsAnonymous && !BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                {
                    return false;
                }
                return true;
            }
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.GetQueryString("upload")))
            {
                if (base.Request.Files != null && base.Request.Files.Count >= 1)
                {
                    HttpPostedFile myFile = base.Request.Files[0];

                    if (myFile != null && "" != myFile.FileName)
                    {
                        string filePath = myFile.FileName;
                        try
                        {
                            string fileExtName = PathUtils.GetExtension(filePath);

                            string localDirectoryPath = string.Empty;
                            string localFileName = string.Empty;

                            if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                            {
                                localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                                localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);
                            }
                            else if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                            {
                                localDirectoryPath = PathUtils.GetUserUploadDirectoryPath(BaiRongDataProvider.UserDAO.CurrentUserName);
                                localFileName = PathUtils.GetUserUploadFileName(filePath);
                            }

                            string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                            if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                            {
                                if (!PathUtility.IsImageExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                                {
                                    this.WriteScript("alert('此文件格式被管理员禁止，请更换文件上传！')");
                                    return;
                                }
                                if (!PathUtility.IsImageSizeAllowed(base.PublishmentSystemInfo, myFile.ContentLength))
                                {
                                    this.WriteScript("alert('上传失败，上传文件超出规定文件大小！')");
                                    return;
                                }
                                myFile.SaveAs(localFilePath);
                                string imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                                this.WriteScript(string.Format("LoadIMG('{0}')", imageUrl));
                            }
                            else if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                            {
                                if (PathUtils.IsFileExtenstionAllowed(UserConfigManager.Additional.UploadImageTypeCollection, fileExtName))
                                {
                                    if (myFile.ContentLength > UserConfigManager.Additional.UploadImageTypeMaxSize * 1024)
                                    {
                                        this.WriteScript("alert('上传失败，上传文件超出规定文件大小！')");
                                        return;
                                    }
                                    myFile.SaveAs(localFilePath);

                                    string imageUrl = PageUtils.GetUserUploadFileUrl(BaiRongDataProvider.UserDAO.CurrentUserName, localFileName);
                                    this.WriteScript(string.Format("LoadIMG('{0}')", imageUrl));
                                }
                                else
                                {
                                    this.WriteScript("alert('此文件格式被管理员禁止，请更换文件上传！')");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.WriteScript(string.Format("alert('文件上传失败:{0}')", ex.Message));
                        }
                    }
                } 
            }
		}

        private void WriteScript(string script)
        {
            this.ltlScript.Text = string.Format("window.parent.{0};", script);
        }
	}
}
