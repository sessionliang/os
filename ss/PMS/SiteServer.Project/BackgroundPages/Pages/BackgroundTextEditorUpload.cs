using System;
using System.Globalization;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using BaiRong.Model;

using BaiRong.Core;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Collections;
using System.Net;
using System.Text;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundTextEditorUpload : BackgroundBasePage
    {
        public Literal ltlScript;

        private ETextEditorType editorType;
        private string fileType;

        public static string GetReidrectUrl(int projectID, ETextEditorType editorType, string fileType)
        {
            return PageUtils.GetAdminDirectoryUrl(string.Format("project/background_textEditorUpload.aspx?ProjectID={0}&EditorType={1}&FileType={2}", projectID, ETextEditorTypeUtils.GetValue(editorType), fileType));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("ProjectID", "EditorType", "FileType");

            this.editorType = ETextEditorTypeUtils.GetEnumType(base.Request.QueryString["EditorType"]);
            this.fileType = base.Request.QueryString["FileType"];
            if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
            {
                if (this.editorType == ETextEditorType.UEditor)
                {
                    this.UploadForUEditor();
                }
                else if (this.editorType == ETextEditorType.CKEditor)
                {
                    this.UploadForCKEditor();
                }
            }
        }

        private void UploadForCKEditor()
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["CKEditor"]))
            {
                string funcNum = base.Request.QueryString["CKEditorFuncNum"];
                if (base.Request.Files != null && base.Request.Files.Count >= 1)
                {
                    HttpPostedFile myFile = base.Request.Files[0];

                    if (myFile != null && "" != myFile.FileName)
                    {
                        string filePath = myFile.FileName;
                        string fileName = PathUtils.GetFileName(filePath);
                        try
                        {
                            string directoryPath = ProjectManager.GetDirectoryUploadPath(this.ProjectID);

                            string localFilePath = PathUtils.Combine(directoryPath, fileName);

                            string fileExtName = PathUtils.GetExtension(filePath);

                            if (StringUtils.EqualsIgnoreCase(this.fileType, "Image"))
                            {
                                if (!PathUtils.IsFileExtenstionAllowed("gif|jpg|jpeg|bmp|png", fileExtName))
                                {
                                    this.WriteScriptForCKEditor(funcNum, string.Empty, "此文件格式不正确，请更换文件上传！");
                                    return;
                                }
                            }
                            else if (StringUtils.EqualsIgnoreCase(this.fileType, "Flash"))
                            {
                                if (!PathUtils.IsFileExtenstionAllowed("swf", fileExtName))
                                {
                                    this.WriteScriptForCKEditor(funcNum, string.Empty, "此文件格式不正确，请更换文件上传！");
                                    return;
                                }
                            }
                            else if (StringUtils.EqualsIgnoreCase(this.fileType, "Flv"))
                            {
                                if (!PathUtils.IsFileExtenstionAllowed("flv", fileExtName))
                                {
                                    this.WriteScriptForCKEditor(funcNum, string.Empty, "此文件格式不正确，请更换文件上传！");
                                    return;
                                }
                            }

                            myFile.SaveAs(localFilePath);

                            string imageUrl = PageUtils.Combine(ProjectManager.GetDirectoryUploadUrl(this.ProjectID), fileName);
                            this.WriteScriptForCKEditor(funcNum, imageUrl, string.Empty);
                        }
                        catch (Exception ex)
                        {
                            this.WriteScriptForCKEditor(funcNum, string.Empty, string.Format("文件上传失败:{0}", ex.Message));
                        }
                    }
                }
            }
        }

        private void UploadForUEditor()
        {
            if (this.fileType == "Image")
            {
                Hashtable info = new Hashtable();
                UEditorUploader up = new UEditorUploader();
                info = up.upFile(HttpContext.Current, base.ProjectID);                               //获取上传状态

                string title = up.getOtherInfo(HttpContext.Current, "pictitle");                              //获取图片描述
                string oriName = up.getOtherInfo(HttpContext.Current, "fileName");                //获取原始文件名

                base.Response.ContentType = "text/plain";
                base.Response.Write("{'url':'" + info["url"] + "','title':'" + title + "','original':'" + oriName + "','state':'" + info["state"] + "'}");
                base.Response.End();
            }
            else if (this.fileType == "Scrawl")
            {
                string action = base.Request["action"];

                if (action == "tmpImg")
                {
                    Hashtable info = new Hashtable();
                    UEditorUploader up = new UEditorUploader();
                    info = up.upFile(HttpContext.Current, base.ProjectID); //获取上传状态

                    base.Response.ContentType = "text/html";
                    base.Response.Write("<script>parent.ue_callback('" + info["url"] + "','" + info["state"] + "')</script>");//回调函数
                    base.Response.End();
                }
                else
                {
                    Hashtable info = new Hashtable();
                    UEditorUploader up = new UEditorUploader();
                    info = up.upScrawl(HttpContext.Current, base.Request["content"], base.ProjectID); //获取上传状态

                    base.Response.ContentType = "text/plain";
                    base.Response.Write("{'url':'" + info["url"] + "',state:'" + info["state"] + "'}");
                    base.Response.End();
                }
            }
            else if (this.fileType == "File")
            {
                Hashtable info = new Hashtable();
                UEditorUploader up = new UEditorUploader();
                info = up.upFile(HttpContext.Current, base.ProjectID); //获取上传状态
                base.Response.ContentType = "text/plain";
                base.Response.Write("{'state':'" + info["state"] + "','url':'" + info["url"] + "','fileType':'" + info["currentType"] + "','original':'" + info["originalName"] + "'}");
                base.Response.End();
            }
            else if (this.fileType == "ImageManager")
            {
                base.Response.ContentType = "text/plain";



                string directoryPath = ProjectManager.GetDirectoryUploadPath(base.ProjectID);
                string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };                //文件允许格式

                string action = base.Server.HtmlEncode(base.Request["action"]);

                if (action == "get")
                {
                    String str = String.Empty;

                    //目录验证
                    if (DirectoryUtils.IsDirectoryExists(directoryPath))
                    {
                        string[] filePathArray = DirectoryUtils.GetFilePaths(directoryPath);
                        foreach (string filePath in filePathArray)
                        {
                            if (EFileSystemTypeUtils.IsImage(PathUtils.GetExtension(filePath)))
                            {
                                str += PageUtils.Combine(ProjectManager.GetDirectoryUploadUrl(base.ProjectID), PathUtils.GetFileName(filePath)) + "ue_separate_ue";
                                //str += PageUtility.GetPublishmentSystemUrlByPhysicalPath(this.publishmentSystemInfo, filePath) + "ue_separate_ue";
                            }
                        }
                    }
                    base.Response.Write(str);
                    base.Response.End();
                }
            }
            else if (fileType == "GetMovie")
            {
                base.Response.ContentType = "text/html";
                string key = base.Server.HtmlEncode(base.Request.Form["searchKey"]);
                string type = base.Server.HtmlEncode(base.Request.Form["videoType"]);

                Uri httpURL = new Uri("http://api.tudou.com/v3/gw?method=item.search&appKey=myKey&format=json&kw=" + key + "&pageNo=1&pageSize=20&channelId=" + type + "&inDays=7&media=v&sort=s");
                WebClient MyWebClient = new WebClient();

                //获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                Byte[] pageData = MyWebClient.DownloadData(httpURL);

                base.Response.Write(Encoding.UTF8.GetString(pageData));
                base.Response.End();
            }
        }

        private void WriteScriptForCKEditor(string funcNum, string fileUrl, string errorMsg)
        {
            this.ltlScript.Text = "window.parent.CKEDITOR.tools.callFunction(" + funcNum + ",'" + fileUrl.Replace("'", "\\'") + "','" + errorMsg.Replace("'", "\\'") + "') ;";
        }
    }
}
