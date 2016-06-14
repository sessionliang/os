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
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.TextEditorCKEditor
{
    public class Upload : Page
	{
        public Literal ltlScript;

        public void Page_Load(object sender, EventArgs E)
        {
            string funcNum = base.Request.QueryString["CKEditorFuncNum"];
            string type = base.Request.QueryString["type"];
            int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated && publishmentSystemInfo == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(base.Request.QueryString["CKEditor"]))
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
                                localDirectoryPath = PathUtility.GetUploadDirectoryPath(publishmentSystemInfo, fileExtName);
                                localFileName = PathUtility.GetUploadFileName(publishmentSystemInfo, filePath);
                            }
                            else if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                            {
                                localDirectoryPath = PathUtils.GetUserUploadDirectoryPath(BaiRongDataProvider.UserDAO.CurrentUserName);
                                localFileName = PathUtils.GetUserUploadFileName(filePath);
                            }

                            string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                            if (StringUtils.EqualsIgnoreCase(type, "Image"))
                            {
                                if (!PathUtils.IsFileExtenstionAllowed("gif|jpg|jpeg|bmp|png", fileExtName))
                                {
                                    this.WriteScript(funcNum, string.Empty, "���ļ���ʽ����ȷ��������ļ��ϴ���");
                                    return;
                                }
                            }
                            else if (StringUtils.EqualsIgnoreCase(type, "Flash"))
                            {
                                if (!PathUtils.IsFileExtenstionAllowed("swf", fileExtName))
                                {
                                    this.WriteScript(funcNum, string.Empty, "���ļ���ʽ����ȷ��������ļ��ϴ���");
                                    return;
                                }
                            }
                            else if (StringUtils.EqualsIgnoreCase(type, "Flv"))
                            {
                                if (!PathUtils.IsFileExtenstionAllowed("flv", fileExtName))
                                {
                                    this.WriteScript(funcNum, string.Empty, "���ļ���ʽ����ȷ��������ļ��ϴ���");
                                    return;
                                }
                            }

                            if (!PathUtility.IsImageExtenstionAllowed(publishmentSystemInfo, fileExtName))
                            {
                                this.WriteScript(funcNum, string.Empty, "���ļ���ʽ������Ա��ֹ��������ļ��ϴ���");
                                return;
                            }
                            if (!PathUtility.IsImageSizeAllowed(publishmentSystemInfo, myFile.ContentLength))
                            {
                                this.WriteScript(funcNum, string.Empty, "�ϴ�ʧ�ܣ��ϴ��ļ������涨�ļ���С��");
                                return;
                            }

                            if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
                            {
                                myFile.SaveAs(localFilePath);
                                string imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(publishmentSystemInfo, localFilePath);
                                this.WriteScript(funcNum, imageUrl, string.Empty);
                            }
                            else if (!BaiRongDataProvider.UserDAO.IsAnonymous)
                            {
                                myFile.SaveAs(localFilePath);
                                string imageUrl = PageUtils.GetUserUploadFileUrl(BaiRongDataProvider.UserDAO.CurrentUserName, localFileName);
                                this.WriteScript(funcNum, imageUrl, string.Empty);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.WriteScript(funcNum, string.Empty, string.Format("�ļ��ϴ�ʧ��:{0}", ex.Message));
                        }
                    }
                } 
            }
		}

        private void WriteScript(string funcNum, string fileUrl, string errorMsg)
        {
            this.ltlScript.Text = "window.parent.CKEDITOR.tools.callFunction(" + funcNum + ",'" + fileUrl.Replace("'", "\\'") + "','" + errorMsg.Replace("'", "\\'") + "') ;";
        }
	}
}
