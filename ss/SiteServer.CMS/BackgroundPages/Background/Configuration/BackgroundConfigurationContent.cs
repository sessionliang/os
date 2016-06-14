using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using System.Web.UI.HtmlControls;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundConfigurationContent : BackgroundBasePage
	{
        public DropDownList ddlIsGroupContent;
        public DropDownList ddlIsRelatedByTags;
        public DropDownList ddlIsTranslate;
        public DropDownList ddlIsSaveImageInTextEditor;
        public DropDownList ddlIsAutoPageInTextEditor;
        public TextBox tbAutoPageWordNum;
        public DropDownList ddlIsAutoSaveContent;
        public TextBox tbAutoSaveContentInterval;
        public RadioButtonList rblIsContentTitleBreakLine;
        public DropDownList ddlTextEditorType;
        public RadioButtonList rblIsCheckContentUseLevel;
        public PlaceHolder phCheckContentLevel; 
        public DropDownList ddlCheckContentLevel;
        public RadioButtonList rblIsContentCommentable;
        public TextBox tbPhotoContentPreviewWidth;
        public TextBox tbPhotoContentPreviewHeight;

        //敏感词自动检测
        public RadioButtonList rblIsAutoCheckKeywords;

        //编辑器上传文件URL前缀
        public TextBox tbEditorUploadFilePre;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, "内容管理设置", AppManager.CMS.Permission.WebSite.Configration);

                EBooleanUtils.AddListItems(this.ddlIsGroupContent, "使用内容组", "不使用内容组");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsGroupContent, base.PublishmentSystemInfo.Additional.IsGroupContent.ToString());

                EBooleanUtils.AddListItems(this.ddlIsRelatedByTags, "使用标签", "不使用标签");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsRelatedByTags, base.PublishmentSystemInfo.Additional.IsRelatedByTags.ToString());

                EBooleanUtils.AddListItems(this.ddlIsTranslate, "使用内容转移", "不使用内容转移");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsTranslate, base.PublishmentSystemInfo.Additional.IsTranslate.ToString());

                EBooleanUtils.AddListItems(this.ddlIsSaveImageInTextEditor, "保存", "不保存");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsSaveImageInTextEditor, base.PublishmentSystemInfo.Additional.IsSaveImageInTextEditor.ToString());

                EBooleanUtils.AddListItems(this.ddlIsAutoPageInTextEditor, "自动分页", "手动分页");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsAutoPageInTextEditor, base.PublishmentSystemInfo.Additional.IsAutoPageInTextEditor.ToString());

                this.tbAutoPageWordNum.Text = base.PublishmentSystemInfo.Additional.AutoPageWordNum.ToString();

                EBooleanUtils.AddListItems(this.ddlIsAutoSaveContent, "开启自动保存功能", "关闭自动保存功能");
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsAutoSaveContent, base.PublishmentSystemInfo.Additional.IsAutoSaveContent.ToString());

                this.tbAutoSaveContentInterval.Text = base.PublishmentSystemInfo.Additional.AutoSaveContentInterval.ToString();

                EBooleanUtils.AddListItems(this.rblIsContentTitleBreakLine, "启用标题换行", "不启用");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsContentTitleBreakLine, base.PublishmentSystemInfo.Additional.IsContentTitleBreakLine.ToString());

                //保存时，敏感词自动检测
                EBooleanUtils.AddListItems(this.rblIsAutoCheckKeywords, "启用敏感词自动检测", "不启用");
                ControlUtils.SelectListItemsIgnoreCase(this.rblIsAutoCheckKeywords, base.PublishmentSystemInfo.Additional.IsAutoCheckKeywords.ToString());

                ETextEditorTypeUtils.AddListItems(this.ddlTextEditorType);
                ControlUtils.SelectListItemsIgnoreCase(this.ddlTextEditorType, ETextEditorTypeUtils.GetValue(base.PublishmentSystemInfo.Additional.TextEditorType));

                //编辑器上传文件URL前缀
                this.tbEditorUploadFilePre.Text = base.PublishmentSystemInfo.Additional.EditorUploadFilePre;

                this.rblIsCheckContentUseLevel.Items.Add(new ListItem("默认审核机制", false.ToString()));
                this.rblIsCheckContentUseLevel.Items.Add(new ListItem("多级审核机制", true.ToString()));

                ControlUtils.SelectListItems(this.rblIsCheckContentUseLevel, base.PublishmentSystemInfo.IsCheckContentUseLevel.ToString());
                if (base.PublishmentSystemInfo.IsCheckContentUseLevel)
                {
                    ControlUtils.SelectListItems(this.ddlCheckContentLevel, base.PublishmentSystemInfo.CheckContentLevel.ToString());
                    this.phCheckContentLevel.Visible = true;
                }
                else
                {
                    this.phCheckContentLevel.Visible = false;
                }

                ControlUtils.SelectListItemsIgnoreCase(this.rblIsContentCommentable, base.PublishmentSystemInfo.Additional.IsContentCommentable.ToString());

                this.tbPhotoContentPreviewWidth.Text = base.PublishmentSystemInfo.Additional.PhotoContentPreviewWidth.ToString();
                this.tbPhotoContentPreviewHeight.Text = base.PublishmentSystemInfo.Additional.PhotoContentPreviewHeight.ToString();
			}
		}

        public void rblIsCheckContentUseLevel_OnSelectedIndexChanged(object sender, EventArgs E)
        {
            if (EBooleanUtils.Equals(this.rblIsCheckContentUseLevel.SelectedValue, EBoolean.True))
            {
                this.phCheckContentLevel.Visible = true;
            }
            else
            {
                this.phCheckContentLevel.Visible = false;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.Additional.IsGroupContent = TranslateUtils.ToBool(this.ddlIsGroupContent.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsRelatedByTags = TranslateUtils.ToBool(this.ddlIsRelatedByTags.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsTranslate = TranslateUtils.ToBool(this.ddlIsTranslate.SelectedValue);

                base.PublishmentSystemInfo.Additional.IsSaveImageInTextEditor = TranslateUtils.ToBool(this.ddlIsSaveImageInTextEditor.SelectedValue, true);

                bool isReCaculate = false;
                if (TranslateUtils.ToBool(this.ddlIsAutoPageInTextEditor.SelectedValue, false) == true)
                {
                    if (base.PublishmentSystemInfo.Additional.IsAutoPageInTextEditor == false)
                    {
                        isReCaculate = true;
                    }
                    else if (base.PublishmentSystemInfo.Additional.AutoPageWordNum != TranslateUtils.ToInt(this.tbAutoPageWordNum.Text, base.PublishmentSystemInfo.Additional.AutoPageWordNum))
                    {
                        isReCaculate = true;
                    }
                }

                base.PublishmentSystemInfo.Additional.IsAutoPageInTextEditor = TranslateUtils.ToBool(this.ddlIsAutoPageInTextEditor.SelectedValue, false);

                base.PublishmentSystemInfo.Additional.AutoPageWordNum = TranslateUtils.ToInt(this.tbAutoPageWordNum.Text, base.PublishmentSystemInfo.Additional.AutoPageWordNum);

                base.PublishmentSystemInfo.Additional.IsAutoSaveContent = TranslateUtils.ToBool(this.ddlIsAutoSaveContent.SelectedValue, false);

                base.PublishmentSystemInfo.Additional.AutoSaveContentInterval = TranslateUtils.ToInt(this.tbAutoSaveContentInterval.Text, base.PublishmentSystemInfo.Additional.AutoSaveContentInterval);

                base.PublishmentSystemInfo.Additional.IsContentTitleBreakLine = TranslateUtils.ToBool(this.rblIsContentTitleBreakLine.SelectedValue, true);

                //敏感词自动检测
                base.PublishmentSystemInfo.Additional.IsAutoCheckKeywords = TranslateUtils.ToBool(this.rblIsAutoCheckKeywords.SelectedValue, true);

                base.PublishmentSystemInfo.Additional.TextEditorType = ETextEditorTypeUtils.GetEnumType(this.ddlTextEditorType.SelectedValue);

                //编辑器上传文件URL前缀
                base.PublishmentSystemInfo.Additional.EditorUploadFilePre = this.tbEditorUploadFilePre.Text;

                base.PublishmentSystemInfo.IsCheckContentUseLevel = TranslateUtils.ToBool(this.rblIsCheckContentUseLevel.SelectedValue);
                if (base.PublishmentSystemInfo.IsCheckContentUseLevel)
                {
                    base.PublishmentSystemInfo.CheckContentLevel = TranslateUtils.ToInt(this.ddlCheckContentLevel.SelectedValue);
                }

                base.PublishmentSystemInfo.Additional.IsContentCommentable = TranslateUtils.ToBool(this.rblIsContentCommentable.SelectedValue);

                base.PublishmentSystemInfo.Additional.PhotoContentPreviewWidth = TranslateUtils.ToInt(this.tbPhotoContentPreviewWidth.Text, base.PublishmentSystemInfo.Additional.PhotoContentPreviewWidth);
                base.PublishmentSystemInfo.Additional.PhotoContentPreviewHeight = TranslateUtils.ToInt(this.tbPhotoContentPreviewHeight.Text, base.PublishmentSystemInfo.Additional.PhotoContentPreviewHeight);
				
				try
				{
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    if (isReCaculate)
                    {
                        DataProvider.ContentDAO.UpdateAutoPageContent(base.PublishmentSystemInfo.AuxiliaryTableForContent, base.PublishmentSystemInfo);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, "修改内容管理设置");

                    base.SuccessMessage("内容管理设置修改成功！");
				}
                catch (Exception ex)
				{
                    base.FailMessage(ex, "内容管理设置修改失败！");
				}
			}
		}
	}
}
