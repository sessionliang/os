using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Drawing;
using SiteServer.CMS.Core;
using BaiRong.Model;
using BaiRong.Core.AuxiliaryTable;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class UploadImage : BackgroundBasePage
    {
        public HtmlInputFile hifUpload;

        public CheckBox cbIsTitleImage;
        public TextBox tbTitleImageWidth;
        public TextBox tbTitleImageHeight;
        public CheckBox cbIsTitleImageLessSizeNotThumb;

        public CheckBox cbIsShowImageInTextEditor;
        public CheckBox cbIsLinkToOriginal;
        public CheckBox cbIsSmallImage;
        public TextBox tbSmallImageWidth;
        public TextBox tbSmallImageHeight;
        public CheckBox cbIsSmallImageLessSizeNotThumb;

        public Literal ltlScript;

        private int nodeID;
        private string textBoxClientID;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, string textBoxClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("nodeID", nodeID.ToString());
            arguments.Add("textBoxClientID", textBoxClientID);
            return PageUtility.GetOpenWindowString("上传图片", "modal_uploadImage.aspx", arguments, 570, 540);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.nodeID = TranslateUtils.ToInt(base.GetQueryString("nodeID"));
            this.textBoxClientID = base.GetQueryString("TextBoxClientID");

            if (!IsPostBack)
            {
                this.ConfigSettings(true);

                this.cbIsTitleImage.Attributes.Add("onclick", "checkBoxChange();");
                this.cbIsShowImageInTextEditor.Attributes.Add("onclick", "checkBoxChange();");
                this.cbIsSmallImage.Attributes.Add("onclick", "checkBoxChange();");
            }
        }

        private void ConfigSettings(bool isLoad)
        {
            if (isLoad)
            {
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_UploadImageIsTitleImage))
                {
                    this.cbIsTitleImage.Checked = TranslateUtils.ToBool(base.PublishmentSystemInfo.Additional.Config_UploadImageIsTitleImage);
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_UploadImageTitleImageWidth))
                {
                    this.tbTitleImageWidth.Text = base.PublishmentSystemInfo.Additional.Config_UploadImageTitleImageWidth;
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_UploadImageTitleImageHeight))
                {
                    this.tbTitleImageHeight.Text = base.PublishmentSystemInfo.Additional.Config_UploadImageTitleImageHeight;
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_UploadImageIsTitleImageLessSizeNotThumb))
                {
                    this.cbIsTitleImageLessSizeNotThumb.Checked = TranslateUtils.ToBool(base.PublishmentSystemInfo.Additional.Config_UploadImageIsTitleImageLessSizeNotThumb);
                }

                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_UploadImageIsShowImageInTextEditor))
                {
                    this.cbIsShowImageInTextEditor.Checked = TranslateUtils.ToBool(base.PublishmentSystemInfo.Additional.Config_UploadImageIsShowImageInTextEditor);
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_UploadImageIsLinkToOriginal))
                {
                    this.cbIsLinkToOriginal.Checked = TranslateUtils.ToBool(base.PublishmentSystemInfo.Additional.Config_UploadImageIsLinkToOriginal);
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_UploadImageIsSmallImage))
                {
                    this.cbIsSmallImage.Checked = TranslateUtils.ToBool(base.PublishmentSystemInfo.Additional.Config_UploadImageIsSmallImage);
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_UploadImageSmallImageWidth))
                {
                    this.tbSmallImageWidth.Text = base.PublishmentSystemInfo.Additional.Config_UploadImageSmallImageWidth;
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_UploadImageSmallImageHeight))
                {
                    this.tbSmallImageHeight.Text = base.PublishmentSystemInfo.Additional.Config_UploadImageSmallImageHeight;
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.Config_UploadImageIsSmallImageLessSizeNotThumb))
                {
                    this.cbIsSmallImageLessSizeNotThumb.Checked = TranslateUtils.ToBool(base.PublishmentSystemInfo.Additional.Config_UploadImageIsSmallImageLessSizeNotThumb);
                }
            }
            else
            {
                base.PublishmentSystemInfo.Additional.Config_UploadImageIsTitleImage = this.cbIsTitleImage.Checked.ToString();
                base.PublishmentSystemInfo.Additional.Config_UploadImageTitleImageWidth = this.tbTitleImageWidth.Text;
                base.PublishmentSystemInfo.Additional.Config_UploadImageTitleImageHeight = this.tbTitleImageHeight.Text;
                base.PublishmentSystemInfo.Additional.Config_UploadImageIsTitleImageLessSizeNotThumb = this.cbIsTitleImageLessSizeNotThumb.Checked.ToString();

                base.PublishmentSystemInfo.Additional.Config_UploadImageIsShowImageInTextEditor = this.cbIsShowImageInTextEditor.Checked.ToString();
                base.PublishmentSystemInfo.Additional.Config_UploadImageIsLinkToOriginal = this.cbIsLinkToOriginal.Checked.ToString();
                base.PublishmentSystemInfo.Additional.Config_UploadImageIsSmallImage = this.cbIsSmallImage.Checked.ToString();
                base.PublishmentSystemInfo.Additional.Config_UploadImageSmallImageWidth = this.tbSmallImageWidth.Text;
                base.PublishmentSystemInfo.Additional.Config_UploadImageSmallImageHeight = this.tbSmallImageHeight.Text;
                base.PublishmentSystemInfo.Additional.Config_UploadImageIsSmallImageLessSizeNotThumb = this.cbIsSmallImageLessSizeNotThumb.Checked.ToString();

                DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (this.cbIsTitleImage.Checked && string.IsNullOrEmpty(this.tbTitleImageWidth.Text) && string.IsNullOrEmpty(this.tbTitleImageHeight.Text))
            {
                base.FailMessage("缩略图尺寸不能为空！");
                return;
            }
            if (this.cbIsSmallImage.Checked && string.IsNullOrEmpty(this.tbSmallImageWidth.Text) && string.IsNullOrEmpty(this.tbSmallImageHeight.Text))
            {
                base.FailMessage("缩略图尺寸不能为空！");
                return;
            }

            this.ConfigSettings(false);

            if (this.hifUpload.PostedFile != null && "" != this.hifUpload.PostedFile.FileName)
            {
                string filePath = this.hifUpload.PostedFile.FileName;
                try
                {
                    string fileExtName = PathUtils.GetExtension(filePath).ToLower();
                    string localDirectoryPath = PathUtility.GetUploadDirectoryPath(base.PublishmentSystemInfo, fileExtName);
                    string localFileName = PathUtility.GetUploadFileName(base.PublishmentSystemInfo, filePath);
                    string localTitleFileName = Constants.TitleImageAppendix + localFileName;
                    string localSmallFileName = Constants.SmallImageAppendix + localFileName;
                    string localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);
                    string localTitleFilePath = PathUtils.Combine(localDirectoryPath, localTitleFileName);
                    string localSmallFilePath = PathUtils.Combine(localDirectoryPath, localSmallFileName);

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

                    //处理上半部分
                    if (isImage)
                    {
                        FileUtility.AddWaterMark(base.PublishmentSystemInfo, localFilePath);
                        if (this.cbIsTitleImage.Checked)
                        {
                            int width = TranslateUtils.ToInt(this.tbTitleImageWidth.Text);
                            int height = TranslateUtils.ToInt(this.tbTitleImageHeight.Text);
                            ImageUtils.MakeThumbnail(localFilePath, localTitleFilePath, width, height, this.cbIsTitleImageLessSizeNotThumb.Checked);
                        }
                    }

                    string imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                    if (this.cbIsTitleImage.Checked)
                    {
                        imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localTitleFilePath);
                    }

                    string textBoxUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, imageUrl);

                    this.ltlScript.Text += string.Format(@"
if (parent.document.getElementById('{0}'))
{{
    parent.document.getElementById('{0}').value = '{1}';
}}
", this.textBoxClientID, textBoxUrl, imageUrl);

                    //处理下半部分
                    if (this.cbIsShowImageInTextEditor.Checked && isImage)
                    {
                        imageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localFilePath);
                        string smallImageUrl = imageUrl;
                        if (this.cbIsSmallImage.Checked)
                        {
                            smallImageUrl = PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, localSmallFilePath);
                        }

                        if (this.cbIsSmallImage.Checked)
                        {
                            int width = TranslateUtils.ToInt(this.tbSmallImageWidth.Text);
                            int height = TranslateUtils.ToInt(this.tbSmallImageHeight.Text);
                            ImageUtils.MakeThumbnail(localFilePath, localSmallFilePath, width, height, this.cbIsSmallImageLessSizeNotThumb.Checked);
                        }

                        string insertHtml = string.Empty;
                        if (this.cbIsLinkToOriginal.Checked)
                        {
                            insertHtml = string.Format(@"<a href=""{0}"" target=""_blank""><img src=""{1}"" border=""0"" /></a>", imageUrl, smallImageUrl);
                        }
                        else
                        {
                            insertHtml = string.Format(@"<img src=""{0}"" border=""0"" />", smallImageUrl);
                        }

                        ETextEditorType editorType = ETextEditorType.UEditor;
                        if (this.nodeID > 0)
                        {
                            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeID);
                            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeID);
                            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(tableStyle, tableName, BackgroundContentAttribute.Content, RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID));
                            if (styleInfo != null)
                            {
                                if (!string.IsNullOrEmpty(styleInfo.Additional.EditorTypeString))
                                    editorType = ETextEditorTypeUtils.GetEnumType(styleInfo.Additional.EditorTypeString);
                                else
                                    editorType = base.PublishmentSystemInfo.Additional.TextEditorType;
                            }
                        }
                        //this.ltlScript.Text += "try{ if(parent." + ETextEditorTypeUtils.GetEditorInstanceScript(editorType) + ") parent." + ETextEditorTypeUtils.GetInsertHtmlScript(editorType, "Content", insertHtml) + " }catch(e){}";

                        this.ltlScript.Text += "if(parent." + ETextEditorTypeUtils.GetEditorInstanceScript(editorType) + ") parent." + ETextEditorTypeUtils.GetInsertHtmlScript(editorType, "Content", insertHtml);

                        //                            this.ltlScript.Text += string.Format(@"
                        //if (parent.document.getElementById('eWebEditor_Content') != null)
                        //{{
                        //    parent.document.getElementById('eWebEditor_Content').insertHTML('{0}');
                        //}}
                        //else if (parent.FCKeditorAPI != null && parent.FCKeditorAPI.GetInstance('Content') != null)
                        //{{
                        //    var oEditor = parent.FCKeditorAPI.GetInstance('Content') ;
                        //    oEditor.InsertHtml('{0}');
                        //}}
                        //else if (parent.CKEDITOR && parent.CKEDITOR.instances && parent.CKEDITOR.instances.Content)
                        //{{
                        //    parent.CKEDITOR.instances.Content.insertHtml('{0}');
                        //}}
                        //", insertHtml);
                    }

                    this.ltlScript.Text += JsUtils.OpenWindow.HIDE_POP_WIN;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, ex.Message);
                }
            }
        }
    }
}
