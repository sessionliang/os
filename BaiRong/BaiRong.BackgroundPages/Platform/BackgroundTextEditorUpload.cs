using System ;
using System.Globalization ;
using System.Xml ;
using System.Web ;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using BaiRong.Model;

using BaiRong.Core;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using BaiRong.Core.Data.Provider;

namespace BaiRong.BackgroundPages
{
    public class BackgroundTextEditorUpload : Page
	{
        public Literal ltlScript;

        private ETextEditorType editorType;
        private string fileType;

        public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("EditorType", "FileType");

            if (BaiRongDataProvider.AdministratorDAO.IsAuthenticated)
            {
                this.editorType = ETextEditorTypeUtils.GetEnumType(base.Request.QueryString["EditorType"]);
                this.fileType = base.Request.QueryString["FileType"];

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

        private void UploadForUEditor()
        {
            if (this.fileType == "Image")
            {
                Hashtable info = new Hashtable();
                UEditorUploaderForUser up = new UEditorUploaderForUser();
                info = up.upFile(HttpContext.Current);                               //��ȡ�ϴ�״̬

                string title = up.getOtherInfo(HttpContext.Current, "pictitle");                              //��ȡͼƬ����
                string oriName = up.getOtherInfo(HttpContext.Current, "fileName");                //��ȡԭʼ�ļ���

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
                    UEditorUploaderForUser up = new UEditorUploaderForUser();
                    info = up.upFile(HttpContext.Current); //��ȡ�ϴ�״̬

                    base.Response.ContentType = "text/html";
                    base.Response.Write("<script>parent.ue_callback('" + info["url"] + "','" + info["state"] + "')</script>");//�ص�����
                    base.Response.End();
                }
                else
                {
                    Hashtable info = new Hashtable();
                    UEditorUploaderForUser up = new UEditorUploaderForUser();
                    info = up.upScrawl(HttpContext.Current, base.Request["content"]); //��ȡ�ϴ�״̬

                    base.Response.ContentType = "text/plain";
                    base.Response.Write("{'url':'" + info["url"] + "',state:'" + info["state"] + "'}");
                    base.Response.End();
                }
            }
            else if (this.fileType == "File")
            {
                Hashtable info = new Hashtable();
                UEditorUploaderForUser up = new UEditorUploaderForUser();
                info = up.upFile(HttpContext.Current); //��ȡ�ϴ�״̬
                base.Response.ContentType = "text/plain";
                base.Response.Write("{'state':'" + info["state"] + "','url':'" + info["url"] + "','fileType':'" + info["currentType"] + "','original':'" + info["originalName"] + "'}");
                base.Response.End();
            }
            else if (this.fileType == "ImageManager")
            {
                base.Response.ContentType = "text/plain";

                string directoryPath = PathUtils.GetUserUploadDirectoryPath(string.Empty);
                string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };                //�ļ������ʽ

                string action = base.Server.HtmlEncode(base.Request["action"]);

                if (action == "get")
                {
                    String str = String.Empty;

                    //Ŀ¼��֤
                    if (DirectoryUtils.IsDirectoryExists(directoryPath))
                    {
                        string[] filePathArray = DirectoryUtils.GetFilePaths(directoryPath);
                        foreach (string filePath in filePathArray)
                        {
                            if (EFileSystemTypeUtils.IsImage(PathUtils.GetExtension(filePath)))
                            {
                                str += PageUtils.GetRootUrlByPhysicalPath(filePath) + "ue_separate_ue";
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

                //��ȡ������������Internet��Դ��������������֤������ƾ��
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                Byte[] pageData = MyWebClient.DownloadData(httpURL);

                base.Response.Write(Encoding.UTF8.GetString(pageData));
                base.Response.End();
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
                        try
                        {
                            string localDirectoryPath = PathUtils.GetUserUploadDirectoryPath(string.Empty);
                            string localFileName = PathUtils.GetUserUploadFileName(filePath);
                            string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);
                            string fileExtName = PathUtils.GetExtension(filePath);

                            if (StringUtils.EqualsIgnoreCase(this.fileType, "Image"))
                            {
                                if (!PathUtils.IsFileExtenstionAllowed("gif|jpg|jpeg|bmp|png", fileExtName))
                                {
                                    this.WriteScriptForCKEditor(funcNum, string.Empty, "���ļ���ʽ����ȷ��������ļ��ϴ���");
                                    return;
                                }
                            }
                            else if (StringUtils.EqualsIgnoreCase(this.fileType, "Flash"))
                            {
                                if (!PathUtils.IsFileExtenstionAllowed("swf", fileExtName))
                                {
                                    this.WriteScriptForCKEditor(funcNum, string.Empty, "���ļ���ʽ����ȷ��������ļ��ϴ���");
                                    return;
                                }
                            }
                            else if (StringUtils.EqualsIgnoreCase(this.fileType, "Flv"))
                            {
                                if (!PathUtils.IsFileExtenstionAllowed("flv", fileExtName))
                                {
                                    this.WriteScriptForCKEditor(funcNum, string.Empty, "���ļ���ʽ����ȷ��������ļ��ϴ���");
                                    return;
                                }
                            }

                            if (PathUtils.IsFileExtenstionAllowed(UserConfigManager.Additional.UploadImageTypeCollection, fileExtName))
                            {
                                if (myFile.ContentLength > UserConfigManager.Additional.UploadImageTypeMaxSize * 1024)
                                {
                                    this.WriteScriptForCKEditor(funcNum, string.Empty, "�ϴ�ʧ�ܣ��ϴ��ļ������涨�ļ���С��");
                                    return;
                                }
                                myFile.SaveAs(localFilePath);

                                string imageUrl = PageUtils.GetRootUrlByPhysicalPath(localFilePath);
                                this.WriteScriptForCKEditor(funcNum, imageUrl, string.Empty);
                            }
                            else
                            {
                                this.WriteScriptForCKEditor(funcNum, string.Empty, "���ļ���ʽ������Ա��ֹ��������ļ��ϴ���");
                            }
                        }
                        catch (Exception ex)
                        {
                            this.WriteScriptForCKEditor(funcNum, string.Empty, string.Format("�ļ��ϴ�ʧ��:{0}", ex.Message));
                        }
                    }
                }
            }
        }

        private void WriteScriptForCKEditor(string funcNum, string fileUrl, string errorMsg)
        {
            this.ltlScript.Text = "window.parent.CKEDITOR.tools.callFunction(" + funcNum + ",'" + fileUrl.Replace("'", "\\'") + "','" + errorMsg.Replace("'", "\\'") + "') ;";
        }
	}
}
