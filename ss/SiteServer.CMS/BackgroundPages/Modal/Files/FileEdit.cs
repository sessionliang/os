using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class FileEdit : BackgroundBasePage
	{
        protected TextBox FileName;
        protected RadioButtonList IsPureText;
        public DropDownList Charset;
        
        protected PlaceHolder PlaceHolder_PureText;
        protected TextBox FileContentTextBox;
        protected PlaceHolder PlaceHolder_TextEditor;
        protected TextEditor FileContent;

        protected Literal ltlOpen;
        protected Literal ltlView;
        protected Button btnSave;

		private string relatedPath;
        private string theFileName;
        private bool isCreate;
        private ECharset fileCharset;

        public static string GetOpenWindowString(int publishmentSystemID, string relatedPath, string fileName, bool isCreate)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedPath", relatedPath);
            arguments.Add("FileName", fileName);
            arguments.Add("IsCreate", isCreate.ToString());
            string title = isCreate ? "新建文件" : "编辑文件";
            return PageUtility.GetOpenWindowString(title, "modal_fileEdit.aspx", arguments, 580, 520);
        }

        public static string GetOpenWindowString(int publishmentSystemID, string fileUrl)
        {
            string relatedPath = "@/";
            string fileName = fileUrl;
            if (!string.IsNullOrEmpty(fileUrl))
            {
                fileUrl.Trim('/');
                int i = fileUrl.LastIndexOf('/');
                if (i != -1)
                {
                    relatedPath = fileUrl.Substring(0, i + 1);
                    fileName = fileUrl.Substring(i + 1, fileUrl.Length - i - 1);
                }
            }
            return GetOpenWindowString(publishmentSystemID, relatedPath, fileName, false);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "RelatedPath", "FileName", "IsCreate");
            this.relatedPath = base.GetQueryString("RelatedPath").Trim('/');
            if (!this.relatedPath.StartsWith("@"))
            {
                this.relatedPath = "@/" + this.relatedPath;
            }
            this.theFileName = base.GetQueryString("FileName");
            this.isCreate = TranslateUtils.ToBool(base.GetQueryString("IsCreate"), false);
            this.fileCharset = ECharset.utf_8;
            if (base.PublishmentSystemInfo != null)
            {
                this.fileCharset = ECharsetUtils.GetEnumType(base.PublishmentSystemInfo.Additional.Charset);
            }

            if (this.isCreate == false)
            {
                string filePath = string.Empty;
                if (base.PublishmentSystemInfo != null)
                {
                    filePath = PathUtility.MapPath(base.PublishmentSystemInfo, PathUtils.Combine(this.relatedPath, this.theFileName));
                }
                else
                {
                    filePath = PathUtils.MapPath(PathUtils.Combine(this.relatedPath, this.theFileName));
                }
                if (!FileUtils.IsFileExists(filePath))
                {
                    PageUtils.RedirectToErrorPage("此文件不存在！");
                    return;
                }
            }

			if (!IsPostBack)
			{
                this.Charset.Items.Add(new ListItem("默认", string.Empty));
                ECharsetUtils.AddListItems(this.Charset);
                
                this.FileContent.Type = ETextEditorTypeUtils.GetValue(ETextEditorType.FCKEditor);
                if (this.isCreate == false)
                {
                    string filePath = string.Empty;
                    if (base.PublishmentSystemInfo != null)
                    {
                        filePath = PathUtility.MapPath(base.PublishmentSystemInfo, PathUtils.Combine(this.relatedPath, this.theFileName));
                    }
                    else
                    {
                        filePath = PathUtils.MapPath(PathUtils.Combine(this.relatedPath, this.theFileName));
                    }
                    this.FileName.Text = this.theFileName;
                    this.FileName.Enabled = false;
                    this.FileContentTextBox.Text = FileUtils.ReadText(filePath, this.fileCharset);
                }

                if (!this.isCreate)
                {
                    if (base.PublishmentSystemInfo != null)
                    {
                        this.ltlOpen.Text = string.Format(@"<a href=""{0}"" target=""_blank"">浏 览</a>&nbsp;&nbsp;", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, PageUtils.Combine(this.relatedPath, this.theFileName)));
                    }
                    else
                    {
                        this.ltlOpen.Text = string.Format(@"<a href=""{0}"" target=""_blank"">浏 览</a>&nbsp;&nbsp;", PageUtils.ParseConfigRootUrl(PageUtils.Combine(this.relatedPath, this.theFileName)));
                    }
                    this.ltlView.Text = string.Format(@"<a href=""{0}"">属 性</a>", PageUtils.GetCMSUrl(string.Format("modal_fileView.aspx?PublishmentSystemID={0}&RelatedPath={1}&FileName={2}", base.PublishmentSystemID, this.relatedPath, this.theFileName))); 
                }
			}
		}

        protected void IsPureText_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (TranslateUtils.ToBool(this.IsPureText.SelectedValue))
            {
                this.PlaceHolder_PureText.Visible = true;
                this.PlaceHolder_TextEditor.Visible = false;
                this.FileContentTextBox.Text = this.FileContent.Text;
            }
            else
            {
                this.PlaceHolder_PureText.Visible = false;
                this.PlaceHolder_TextEditor.Visible = true;
                this.FileContent.Text = this.FileContentTextBox.Text;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            this.Save(true);
        }

        protected void Save_OnClick(object sender, System.EventArgs e)
        {
            this.Save(false);
        }

        private void Save(bool isClose)
        {
            bool isSuccess = false;
            string errorMessage = string.Empty;

            string content = (TranslateUtils.ToBool(this.IsPureText.SelectedValue)) ? this.FileContentTextBox.Text : this.FileContent.Text;
            if (this.isCreate == false)
            {
                string filePath = string.Empty;

                string fileExtName = PathUtils.GetExtension(this.theFileName);
                if (!PathUtility.IsFileExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                {
                    base.FailMessage("此格式不允许创建，请选择有效的文件名");
                    return;
                }

                if (base.PublishmentSystemInfo != null)
                {
                    filePath = PathUtility.MapPath(base.PublishmentSystemInfo, PathUtils.Combine(this.relatedPath, this.theFileName));
                }
                else
                {
                    filePath = PathUtils.MapPath(PathUtils.Combine(this.relatedPath, this.theFileName));
                }
                try
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(this.Charset.SelectedValue))
                        {
                            this.fileCharset = ECharsetUtils.GetEnumType(this.Charset.SelectedValue);
                        }
                        FileUtils.WriteText(filePath, this.fileCharset, content);
                    }
                    catch
                    {
                        FileUtils.RemoveReadOnlyAndHiddenIfExists(filePath);
                        FileUtils.WriteText(filePath, this.fileCharset, content);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, "新建文件", string.Format("文件名:{0}", this.theFileName));

                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }
            else
            {
                string filePath = string.Empty;

                string fileExtName = PathUtils.GetExtension(this.FileName.Text);
                if (!PathUtility.IsFileExtenstionAllowed(base.PublishmentSystemInfo, fileExtName))
                {
                    base.FailMessage("此格式不允许创建，请选择有效的文件名");
                    return;
                }

                if (base.PublishmentSystemInfo != null)
                {
                    filePath = PathUtility.MapPath(base.PublishmentSystemInfo, PathUtils.Combine(this.relatedPath, this.FileName.Text));
                }
                else
                {
                    filePath = PathUtils.MapPath(PathUtils.Combine(this.relatedPath, this.FileName.Text));
                }
                if (FileUtils.IsFileExists(filePath))
                {
                    errorMessage = "文件名已存在！";
                }
                else
                {
                    try
                    {
                        try
                        {
                            FileUtils.WriteText(filePath, this.fileCharset, content);
                        }
                        catch
                        {
                            FileUtils.RemoveReadOnlyAndHiddenIfExists(filePath);
                            FileUtils.WriteText(filePath, this.fileCharset, content);
                        }
                        StringUtility.AddLog(base.PublishmentSystemID, "编辑文件", string.Format("文件名:{0}", this.theFileName));
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                }
            }

            if (isSuccess)
            {
                if (isClose)
                {
                    if (this.isCreate)
                    {
                        JsUtils.OpenWindow.CloseModalPage(Page);
                    }
                    else
                    {
                        JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page);
                    }
                }
                else
                {
                    base.SuccessMessage("文件保存成功！");
                }
            }
            else
            {
                base.FailMessage(errorMessage);
            }
        }
	}
}
