using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Web.UI.WebControls;
using SiteServer.CMS.Core.Office;

using BaiRong.Model;
using System.Web;
using BaiRong.Text.LitJson;

namespace SiteServer.CMS.Pages.Mlib.Modal
{
    public class TextEditorInsertAudio : SystemBasePage
    {
        public TextBox tbPlayUrl;
        public CheckBox cbIsAutoPlay;

        private ETextEditorType editorType;
        private string attributeName;
        public int PublishmentSystemID
        {
            get
            {
                if (!string.IsNullOrEmpty(base.Request.QueryString["PublishmentSystemID"]))
                {
                    return TranslateUtils.ToInt(base.Request.QueryString["PublishmentSystemID"]);
                }
                return 0;
            }
        }

        public PublishmentSystemInfo PublishmentSystemInfo
        {
            get
            {
                if (PublishmentSystemID != 0)
                {
                    return PublishmentSystemManager.GetPublishmentSystemInfo(PublishmentSystemID);
                }
                return null;
            }
        }

        public static string GetOpenWindowString(int publishmentSystemID, ETextEditorType editorType, string attributeName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("EditorType", ETextEditorTypeUtils.GetValue(editorType));
            arguments.Add("AttributeName", attributeName);
            return PageUtility.GetOpenWindowString("插入音频", "modal_textEditorInsertAudio.aspx", arguments, 550, 350);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["upload"]))
            {
                string json = JsonMapper.ToJson(this.Upload());
                base.Response.Write(json);
                base.Response.End();
                return;
            }

            this.editorType = ETextEditorTypeUtils.GetEnumType(base.Request.QueryString["EditorType"]);
            this.attributeName = base.Request.QueryString["AttributeName"];
        }

        public string TypeCollection
        {
            get { return PublishmentSystemInfo.Additional.VideoUploadTypeCollection; }
        }

        private Hashtable Upload()
        {
            bool success = false;
            string playUrl = string.Empty;
            string message = "音频上传失败";

            if (base.Request.Files != null && base.Request.Files["filedata"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["filedata"];
                try
                {
                    if (postedFile != null && !string.IsNullOrEmpty(postedFile.FileName))
                    {
                        string filePath = postedFile.FileName;
                        string fileExtName = PathUtils.GetExtension(filePath);

                        bool isAllow = true;
                        if (!PathUtility.IsVideoExtenstionAllowed(PublishmentSystemInfo, fileExtName))
                        {
                            message = "此格式不允许上传，请选择有效的音频文件";
                            isAllow = false;
                        }
                        if (!PathUtility.IsVideoSizeAllowed(PublishmentSystemInfo, postedFile.ContentLength))
                        {
                            message = "上传失败，上传文件超出规定文件大小";
                            isAllow = false;
                        }

                        if (isAllow)
                        {
                            string localDirectoryPath = PathUtility.GetUploadDirectoryPath(PublishmentSystemInfo, fileExtName);
                            string localFileName = PathUtility.GetUploadFileName(PublishmentSystemInfo, filePath);
                            string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                            postedFile.SaveAs(localFilePath);
                            playUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(PublishmentSystemInfo, localFilePath);
                            playUrl = PageUtility.GetVirtualUrl(PublishmentSystemInfo, playUrl);
                            success = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }

            Hashtable jsonAttributes = new Hashtable();
            if (success)
            {
                jsonAttributes.Add("success", "true");
                jsonAttributes.Add("playUrl", playUrl);
            }
            else
            {
                jsonAttributes.Add("success", "false");
                jsonAttributes.Add("message", message);
            }

            return jsonAttributes;
        }

        public void Submit_OnClick(object sender, EventArgs e)
        {
            string playUrl = this.tbPlayUrl.Text;

            string script = "parent." + ETextEditorTypeUtils.GetInsertAudioScript(this.editorType, this.attributeName, playUrl, this.cbIsAutoPlay.Checked);
            JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, script);
        }

    }
}
