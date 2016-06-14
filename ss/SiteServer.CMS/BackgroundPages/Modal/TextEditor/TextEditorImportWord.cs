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
using BaiRong.Text.LitJson;
using System.Web;
using System.Text;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class TextEditorImportWord : BackgroundBasePage
	{
        public CheckBox cbIsClearFormat;
        public CheckBox cbIsFirstLineIndent;
        public CheckBox cbIsClearFontSize;
        public CheckBox cbIsClearFontFamily;
        public CheckBox cbIsClearImages;

        private ETextEditorType editorType;
        private string attributeName;

        public static string GetOpenWindowString(int publishmentSystemID, ETextEditorType editorType, string attributeName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("EditorType", ETextEditorTypeUtils.GetValue(editorType));
            arguments.Add("AttributeName", attributeName);
            return PageUtility.GetOpenWindowString("导入Word", "modal_textEditorImportWord.aspx", arguments, 550, 350);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.GetQueryString("upload")))
            {
                Hashtable attributes = this.Upload();
                string json = JsonMapper.ToJson(attributes);
                base.Response.Write(json);
                base.Response.End();
                return;
            }

            this.editorType = ETextEditorTypeUtils.GetEnumType(base.GetQueryString("EditorType"));
            this.attributeName = base.GetQueryString("AttributeName");

            if (!base.IsPostBack)
            {
                
            }
		}

        private Hashtable Upload()
        {
            bool success = false;
            string fileName = string.Empty;
            string message = "Word上传失败";

            if (base.Request.Files != null && base.Request.Files["filedata"] != null)
            {
                HttpPostedFile postedFile = base.Request.Files["filedata"];
                try
                {
                    if (postedFile != null && !string.IsNullOrEmpty(postedFile.FileName))
                    {
                        fileName = postedFile.FileName;
                        string extendName = fileName.Substring(fileName.LastIndexOf(".")).ToLower();
                        if (extendName == ".doc" || extendName == ".docx")
                        {
                            string filePath = WordUtils.GetWordFilePath(fileName);
                            postedFile.SaveAs(filePath);

                            success = true;
                        }
                        else
                        {
                            base.FailMessage("请选择Word文件上传！");
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
                jsonAttributes.Add("fileName", fileName);
            }
            else
            {
                jsonAttributes.Add("success", "false");
                jsonAttributes.Add("message", message);
            }

            return jsonAttributes;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            string fileNames = base.Request.Form["fileNames"];
            if (!string.IsNullOrEmpty(fileNames))
            {
                fileNames = fileNames.Trim('|');
                StringBuilder builder = new StringBuilder();

                foreach (string fileName in fileNames.Split('|'))
                {
                    string filePath = WordUtils.GetWordFilePath(fileName);
                    string wordContent = WordUtils.Parse(base.PublishmentSystemID, filePath, this.cbIsClearFormat.Checked, this.cbIsFirstLineIndent.Checked, this.cbIsClearFontSize.Checked, this.cbIsClearFontFamily.Checked, this.cbIsClearImages.Checked);
                    /****获取编辑器中内容，解析@符号，添加了远程路径处理 20151103****/
                    wordContent = StringUtility.TextEditorContentDecode(wordContent, base.PublishmentSystemInfo, true);
                    builder.Append(wordContent);
                    FileUtils.DeleteFileIfExists(filePath);
                }
                string script = "parent." + ETextEditorTypeUtils.GetInsertHtmlScript(this.editorType, this.attributeName, builder.ToString());
                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, script);
            }
            else
            {
                base.FailMessage("请选择Word文件上传！");
            }
        }

        //public override void Submit_OnClick(object sender, EventArgs E)
        //{
        //    if (myFile.PostedFile != null && "" != myFile.PostedFile.FileName)
        //    {
        //        string fileName = myFile.PostedFile.FileName;
        //        string extendName = fileName.Substring(fileName.LastIndexOf(".")).ToLower();
        //        try
        //        {
        //            if (extendName == ".doc" || extendName == ".docx")
        //            {
        //                string filePath = WordUtils.GetWordFilePath(fileName);
        //                myFile.PostedFile.SaveAs(filePath);

        //                bool isClearFormat = this.IsClearFormat.Checked;
        //                bool isClearImages = this.IsClearImages.Checked;
        //                string wordContent = WordUtils.Parse(base.PublishmentSystemID, WordUtils.GetWordFilePath(fileName), isClearFormat, isClearImages);
        //                wordContent = StringUtility.TextEditorContentDecode(wordContent, base.PublishmentSystemInfo);
        //                FileUtils.DeleteFileIfExists(filePath);
        //                if (!string.IsNullOrEmpty(wordContent))
        //                {
        //                    string script = "parent." + ETextEditorTypeUtils.GetInsertHtmlScript(this.editorType, this.attributeName, wordContent);
        //                    JsUtils.SubModal.CloseModalPageWithoutRefresh(base.Page, script);
        //                }
        //                else
        //                {
        //                    JsUtils.SubModal.CloseModalPageWithoutRefresh(base.Page);
        //                }
        //            }
        //            else
        //            {
        //                base.FailMessage("请选择Word文件上传！");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            base.FailMessage(ex, "文件上传失败！");
        //            return;
        //        }
        //    }
        //}

	}
}
