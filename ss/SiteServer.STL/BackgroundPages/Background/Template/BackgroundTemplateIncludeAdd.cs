using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTemplateIncludeAdd : BackgroundBasePage
	{
        public Literal ltlPageTitle;

        public Literal ltlCreatedFileExtName;

        public TextBox RelatedFileName;
		public DropDownList Charset;
		public TextBox Content;

        private string fileName;
        private string directoryPath;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("FileName") != null)
            {
                this.fileName = base.GetQueryString("FileName");
                this.fileName = PathUtils.RemoveParentPath(this.fileName);
            }
            this.directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, "@/include");

			if (!IsPostBack)
			{
                string pageTitle = string.IsNullOrEmpty(this.fileName) ? "添加包含文件" : "编辑包含文件";
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Template, pageTitle, AppManager.CMS.Permission.WebSite.Template);

                this.ltlPageTitle.Text = pageTitle;

                ECharsetUtils.AddListItems(this.Charset);

                if (this.fileName != null)
				{
                    if (!EFileSystemTypeUtils.IsHtml(PathUtils.GetExtension(this.fileName)))
                    {
                        PageUtils.RedirectToErrorPage("对不起，此文件无法编辑！");
                    }
                    else
                    {
                        RelatedFileName.Text = PathUtils.RemoveExtension(this.fileName);
                        this.ltlCreatedFileExtName.Text = PathUtils.GetExtension(this.fileName);
                        ECharset fileCharset = FileUtils.GetFileCharset(PathUtils.Combine(this.directoryPath, this.fileName));
                        ControlUtils.SelectListItemsIgnoreCase(this.Charset, ECharsetUtils.GetValue(fileCharset));
                        Content.Text = FileUtils.ReadText(PathUtils.Combine(this.directoryPath, this.fileName), fileCharset);
                    }
				}
				else
                {
                    this.ltlCreatedFileExtName.Text = ".html";
                    ControlUtils.SelectListItemsIgnoreCase(this.Charset, base.PublishmentSystemInfo.Additional.Charset);
				}
			}
		}

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				if (this.fileName != null)
				{
                    bool isChanged = false;
                    if (PathUtils.RemoveExtension(this.fileName) != PathUtils.RemoveExtension(RelatedFileName.Text))//文件名改变
                    {
                        string[] fileNames = DirectoryUtils.GetFileNames(this.directoryPath);
                        foreach (string theFileName in fileNames)
                        {
                            string fileNameWithoutExtension = PathUtils.RemoveExtension(theFileName);
                            if (fileNameWithoutExtension == RelatedFileName.Text.ToLower())
                            {
                                base.FailMessage("包含文件修改失败，包含文件已存在！");
                                return;
                            }
                        }

                        isChanged = true;
                    }

                    if (PathUtils.GetExtension(this.fileName) != this.ltlCreatedFileExtName.Text)//文件后缀改变
                    {
                        isChanged = true;
                    }

                    string previousFileName = string.Empty;
                    if (isChanged)
                    {
                        previousFileName = this.fileName;
                    }

                    string currentFileName = this.RelatedFileName.Text + this.ltlCreatedFileExtName.Text;
                    ECharset charset = ECharsetUtils.GetEnumType(Charset.SelectedValue);
					try
					{
                        FileUtils.WriteText(PathUtils.Combine(this.directoryPath, currentFileName), charset, this.Content.Text);
                        if (!string.IsNullOrEmpty(previousFileName))
                        {
                            FileUtils.DeleteFileIfExists(PathUtils.Combine(this.directoryPath, previousFileName));
                        }
                        StringUtility.AddLog(base.PublishmentSystemID, "修改包含文件", string.Format("包含文件:{0}", currentFileName));
						base.SuccessMessage("包含文件修改成功！");
                        base.AddWaitAndRedirectScript(PageUtils.GetSTLUrl(string.Format("background_templateInclude.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
					}
					catch(Exception ex)
					{
						base.FailMessage(ex, "包含文件修改失败," + ex.Message);
					}
				}
				else
				{
                    string currentFileName = RelatedFileName.Text + this.ltlCreatedFileExtName.Text;

                    string[] fileNames = DirectoryUtils.GetFileNames(this.directoryPath);
                    foreach (string theFileName in fileNames)
                    {
                        if (StringUtils.EqualsIgnoreCase(theFileName, currentFileName))
                        {
                            base.FailMessage("包含文件添加失败，包含文件文件已存在！");
                            return;
                        }
                    }

                    ECharset charset = ECharsetUtils.GetEnumType(this.Charset.SelectedValue);
					try
					{
                        FileUtils.WriteText(PathUtils.Combine(this.directoryPath, currentFileName), charset, this.Content.Text);
                        StringUtility.AddLog(base.PublishmentSystemID, "添加包含文件", string.Format("包含文件:{0}", currentFileName));
						base.SuccessMessage("包含文件添加成功！");
                        base.AddWaitAndRedirectScript(PageUtils.GetSTLUrl(string.Format("background_templateInclude.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "包含文件添加失败," + ex.Message);
					}
				}
			}
		}
	}
}
