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

using SiteServer.CMS.Services;
using SiteServer.STL.IO;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTemplateAdd : BackgroundBasePage
	{
        public Literal ltlPageTitle;

		public TextBox TemplateName;

        public DropDownList CreatedFileExtNameDropDownList;

        public TextBox RelatedFileName;
        public TextBox CreatedFileFullName;
        public Control CreatedFileFullNameRow;
		public Help CreatedFileFullNameHelp;
		public NoTagText CreatedFileFullNameText;

		public DropDownList Charset;
		public string TemplateTypeString;
		public HtmlInputHidden TemplateType;
		public TextBox Content;

        public Literal ltlCommands;
        public PlaceHolder phCodeMirror;
        public Button btnEditorType;

		private ETemplateType theTemplateType = ETemplateType.IndexPageTemplate;
        private bool isCopy = false;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            TemplateInfo templateInfo = null;
            if (base.GetQueryString("TemplateID") != null)
            {
                int TemplateID = int.Parse(base.GetQueryString("TemplateID"));
                this.isCopy = TranslateUtils.ToBool(base.GetQueryString("IsCopy"));
                templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, TemplateID);
                if (templateInfo != null)
                {
                    this.theTemplateType = templateInfo.TemplateType;
                }
            }
            else
            {
                this.theTemplateType = ETemplateTypeUtils.GetEnumType(Request.QueryString["TemplateType"]);
            }
            TemplateTypeString = ETemplateTypeUtils.GetText(this.theTemplateType);

            if (this.theTemplateType == ETemplateType.IndexPageTemplate || this.theTemplateType == ETemplateType.FileTemplate)
            {
                this.CreatedFileFullNameRow.Visible = true;
            }
            else
            {
                this.CreatedFileFullNameRow.Visible = false;
            }

			if (!IsPostBack)
			{
                string pageTitle = (base.GetQueryString("TemplateID") != null) ? "编辑模板" : "添加模板";
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Template, pageTitle, AppManager.CMS.Permission.WebSite.Template);

                this.ltlPageTitle.Text = pageTitle;

                bool isCodeMirror = TranslateUtils.ToBool(base.PublishmentSystemInfo.Additional.Config_TemplateIsCodeMirror);
                this.btnEditorType.Text = isCodeMirror ? "采用纯文本编辑模式" : "采用代码编辑模式";
                this.phCodeMirror.Visible = isCodeMirror;

                EFileSystemTypeUtils.AddWebPageListItems(this.CreatedFileExtNameDropDownList);

                ECharsetUtils.AddListItems(this.Charset);

				if (base.GetQueryString("TemplateID") != null)
				{
					if (templateInfo != null)
					{
                        this.Content.Text = CreateCacheManager.FileContent.GetTemplateContent(base.PublishmentSystemInfo, templateInfo);

                        if (this.isCopy)
                        {
                            TemplateName.Text = templateInfo.TemplateName + "_复件";
                            RelatedFileName.Text = PathUtils.RemoveExtension(templateInfo.RelatedFileName) + "_复件";
                            CreatedFileFullName.Text = PathUtils.RemoveExtension(templateInfo.CreatedFileFullName) + "_复件";
                        }
                        else
                        {
                            TemplateName.Text = templateInfo.TemplateName;
                            RelatedFileName.Text = PathUtils.RemoveExtension(templateInfo.RelatedFileName);
                            CreatedFileFullName.Text = PathUtils.RemoveExtension(templateInfo.CreatedFileFullName);

                            if (templateInfo.TemplateType == ETemplateType.IndexPageTemplate || templateInfo.TemplateType == ETemplateType.FileTemplate)
                            {
                                string designUrl = PageUtility.DynamicPage.GetDesignUrl(PageUtility.DynamicPage.GetRedirectUrl(base.PublishmentSystemID, 0, 0, templateInfo.TemplateType == ETemplateType.FileTemplate ? templateInfo.TemplateID : 0, 0));
                                ltlCommands.Text += string.Format(@"<a href=""{0}"" class=""btn btn-info"" target=""_blank"">可视化编辑</a>", designUrl);
                            }
                            else
                            {
                                ltlCommands.Text += string.Format(@"<a href=""javascript:;"" class=""btn btn-info"" onclick=""{0}"">可视化编辑</a>", Modal.StlTemplate.StlTemplateSelect.GetOpenLayerString(base.PublishmentSystemID, templateInfo.TemplateType, templateInfo.TemplateID, false));
                            }

                            ltlCommands.Text += string.Format(@"<a href=""javascript:;"" class=""btn btn-info"" onclick=""{0}"">生成页面</a>", ProgressBar.GetOpenWindowStringWithCreateByTemplate(base.PublishmentSystemID, templateInfo.TemplateID));

                            ltlCommands.Text += string.Format(@"<a href=""javascript:;"" class=""btn btn-info"" onclick=""{0}"">还原历史版本</a>", Modal.TemplateRestore.GetOpenLayerString(base.PublishmentSystemID, templateInfo.TemplateID, string.Empty, false));

                            if (base.GetQueryString("TemplateLogID") != null)
                            {
                                int templateLogID = TranslateUtils.ToInt(base.GetQueryString("TemplateLogID"));
                                if (templateLogID > 0)
                                {
                                    this.Content.Text = DataProvider.TemplateLogDAO.GetTemplateContent(templateLogID);
                                    base.SuccessMessage("已导入历史版本的模板内容，点击确定保存模板");
                                }
                            }
                        }

                        ControlUtils.SelectListItemsIgnoreCase(this.Charset, ECharsetUtils.GetValue(templateInfo.Charset));

                        ControlUtils.SelectListItems(this.CreatedFileExtNameDropDownList, GetTemplateFileExtension(templateInfo));
						TemplateType.Value = ETemplateTypeUtils.GetValue(templateInfo.TemplateType);
					}
				}
				else
				{
					RelatedFileName.Text = "T_";
                    if (this.theTemplateType == ETemplateType.ChannelTemplate)
					{
						CreatedFileFullName.Text = "index";
					}
					else
					{
						CreatedFileFullName.Text = "@/";
					}
                    ControlUtils.SelectListItemsIgnoreCase(this.Charset, base.PublishmentSystemInfo.Additional.Charset);
                    ControlUtils.SelectListItems(this.CreatedFileExtNameDropDownList, EFileSystemTypeUtils.GetValue(EFileSystemType.Html));
					TemplateType.Value = base.GetQueryString("TemplateType");
				}
			}
		}

        public void EditorType_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                bool isCodeMirror = TranslateUtils.ToBool(base.PublishmentSystemInfo.Additional.Config_TemplateIsCodeMirror);
                isCodeMirror = !isCodeMirror;
                base.PublishmentSystemInfo.Additional.Config_TemplateIsCodeMirror = isCodeMirror.ToString();
                DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                this.btnEditorType.Text = isCodeMirror ? "采用纯文本编辑模式" : "采用代码编辑模式";
                this.phCodeMirror.Visible = isCodeMirror;
            }
        }

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                if (this.theTemplateType != ETemplateType.ChannelTemplate)
				{
					if (!CreatedFileFullName.Text.StartsWith("~") && !CreatedFileFullName.Text.StartsWith("@"))
					{
						CreatedFileFullName.Text = PageUtils.Combine("@", CreatedFileFullName.Text);
					}
				}
				else
				{
					CreatedFileFullName.Text = CreatedFileFullName.Text.TrimStart('~', '@');
					CreatedFileFullName.Text = CreatedFileFullName.Text.Replace("/", string.Empty);
				}

				if (base.GetQueryString("TemplateID") != null && this.isCopy == false)
				{
                    int TemplateID = base.GetIntQueryString("TemplateID");
                    TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, TemplateID);
					if (templateInfo.TemplateName != TemplateName.Text)
					{
                        ArrayList templateNameArrayList = DataProvider.TemplateDAO.GetTemplateNameArrayList(base.PublishmentSystemID, templateInfo.TemplateType);
						if (templateNameArrayList.IndexOf(TemplateName.Text) != -1)
						{
                            base.FailMessage("模板修改失败，模板名称已存在！");
							return;
						}
					}
                    TemplateInfo previousTemplateInfo = null;
                    bool isChanged = false;
                    if (PathUtils.RemoveExtension(templateInfo.RelatedFileName) != PathUtils.RemoveExtension(RelatedFileName.Text))//文件名改变
                    {
                        ArrayList fileNameArrayList = DataProvider.TemplateDAO.GetLowerRelatedFileNameArrayList(base.PublishmentSystemID, templateInfo.TemplateType);
                        foreach (string fileName in fileNameArrayList)
                        {
                            string fileNameWithoutExtension = PathUtils.RemoveExtension(fileName);
                            if (fileNameWithoutExtension == RelatedFileName.Text.ToLower())
                            {
                                base.FailMessage("模板修改失败，模板文件已存在！");
                                return;
                            }
                        }

                        isChanged = true;
                    }

                    if (GetTemplateFileExtension(templateInfo) != this.CreatedFileExtNameDropDownList.SelectedValue)//文件后缀改变
                    {
                        isChanged = true;
                    }

                    if (isChanged)
                    {
                        previousTemplateInfo = new TemplateInfo(templateInfo.TemplateID, templateInfo.PublishmentSystemID, templateInfo.TemplateName, templateInfo.TemplateType, templateInfo.RelatedFileName, templateInfo.CreatedFileFullName, templateInfo.CreatedFileExtName, templateInfo.Charset, templateInfo.IsDefault);
                    }
                    
					templateInfo.TemplateName = TemplateName.Text;
                    templateInfo.RelatedFileName = RelatedFileName.Text + this.CreatedFileExtNameDropDownList.SelectedValue;
                    templateInfo.CreatedFileExtName = this.CreatedFileExtNameDropDownList.SelectedValue;
                    templateInfo.CreatedFileFullName = CreatedFileFullName.Text + this.CreatedFileExtNameDropDownList.SelectedValue;
					templateInfo.Charset = ECharsetUtils.GetEnumType(Charset.SelectedValue);
					try
					{
						DataProvider.TemplateDAO.Update(base.PublishmentSystemInfo, templateInfo, Content.Text);
                        if (previousTemplateInfo != null)
                        {
                            FileUtils.DeleteFileIfExists(TemplateManager.GetTemplateFilePath(base.PublishmentSystemInfo, previousTemplateInfo));
                        }
                        this.CreatePages(templateInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, string.Format("修改{0}", ETemplateTypeUtils.GetText(templateInfo.TemplateType)), string.Format("模板名称:{0}", templateInfo.TemplateName));

						base.SuccessMessage("模板修改成功！");
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "模板修改失败," + ex.Message);
					}
				}
				else
				{
                    ArrayList templateNameArrayList = DataProvider.TemplateDAO.GetTemplateNameArrayList(base.PublishmentSystemID, ETemplateTypeUtils.GetEnumType(this.TemplateType.Value));
					if (templateNameArrayList.IndexOf(TemplateName.Text) != -1)
					{
                        base.FailMessage("模板添加失败，模板名称已存在！");
						return;
					}
                    ArrayList fileNameArrayList = DataProvider.TemplateDAO.GetLowerRelatedFileNameArrayList(base.PublishmentSystemID, ETemplateTypeUtils.GetEnumType(this.TemplateType.Value));
					if (fileNameArrayList.IndexOf(RelatedFileName.Text.ToLower()) != -1)
					{
                        base.FailMessage("模板添加失败，模板文件已存在！");
						return;
					}

					TemplateInfo templateInfo = new TemplateInfo();
                    templateInfo.PublishmentSystemID = base.PublishmentSystemID;
					templateInfo.TemplateName = TemplateName.Text;
					templateInfo.TemplateType = ETemplateTypeUtils.GetEnumType(TemplateType.Value);

                    templateInfo.RelatedFileName = RelatedFileName.Text + this.CreatedFileExtNameDropDownList.SelectedValue;
                    templateInfo.CreatedFileExtName = this.CreatedFileExtNameDropDownList.SelectedValue;
                    templateInfo.CreatedFileFullName = CreatedFileFullName.Text + this.CreatedFileExtNameDropDownList.SelectedValue;
					templateInfo.Charset = ECharsetUtils.GetEnumType(Charset.SelectedValue);
					templateInfo.IsDefault = false;
					try
					{
                        templateInfo.TemplateID = DataProvider.TemplateDAO.Insert(templateInfo, Content.Text);
                        this.CreatePages(templateInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, string.Format("添加{0}", ETemplateTypeUtils.GetText(templateInfo.TemplateType)), string.Format("模板名称:{0}", templateInfo.TemplateName));
						base.SuccessMessage("模板添加成功！");
                        base.AddWaitAndRedirectScript(PageUtils.GetSTLUrl(string.Format(@"background_template.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
					}
					catch(Exception ex)
					{
						base.FailMessage(ex, "模板添加失败," + ex.Message);
					}
				}
			}
		}

        private void CreatePages(TemplateInfo templateInfo)
        {
            if (templateInfo.TemplateType == ETemplateType.FileTemplate)
            {
                FileSystemObject FSO = new FileSystemObject(base.PublishmentSystemID);
                FSO.CreateFile(templateInfo.TemplateID);
            }
            else if (templateInfo.TemplateType == ETemplateType.IndexPageTemplate)
            {
                if (templateInfo.IsDefault)
                {
                    FileSystemObject FSO = new FileSystemObject(base.PublishmentSystemID);
                    FSO.AddIndexToWaitingCreate();
                }
            }
        }

        private static string GetTemplateFileExtension(TemplateInfo templateInfo)
        {
            string extension;
            if (templateInfo.TemplateType == ETemplateType.IndexPageTemplate || templateInfo.TemplateType == ETemplateType.FileTemplate)
            {
                extension = PathUtils.GetExtension(templateInfo.CreatedFileFullName);
            }
            else
            {
                extension = templateInfo.CreatedFileExtName;
            }
            return extension;
        }
	}
}
