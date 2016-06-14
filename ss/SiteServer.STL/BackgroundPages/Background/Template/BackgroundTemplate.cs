using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using SiteServer.CMS.Services;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTemplate : BackgroundBasePage
	{
		public DropDownList ddlTemplateType;
		public TextBox tbKeywords;
		public DataGrid dgContents;
        public Literal ltlCommands;

        private string templateType;
        private string keywords;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.templateType = base.GetQueryString("templateType");
            this.keywords = base.GetQueryString("keywords");

			if (!base.IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Template, "模板管理", AppManager.CMS.Permission.WebSite.Template);

				this.ddlTemplateType.Items.Add(new ListItem("<所有类型>", string.Empty));
                ETemplateTypeUtils.AddListItems(this.ddlTemplateType);
                ControlUtils.SelectListItems(this.ddlTemplateType, this.templateType);

                this.tbKeywords.Text = this.keywords;

				if (base.GetQueryString("Delete") != null)
				{
					int templateID = Int32.Parse(base.GetQueryString("TemplateID"));

					try
					{
                        TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, templateID);
                        if (templateInfo != null)
                        {
                            DataProvider.TemplateDAO.Delete(base.PublishmentSystemID, templateID);
                            StringUtility.AddLog(base.PublishmentSystemID, string.Format("删除{0}", ETemplateTypeUtils.GetText(templateInfo.TemplateType)), string.Format("模板名称:{0}", templateInfo.TemplateName));
                        }
						base.SuccessDeleteMessage();
					}
					catch(Exception ex)
					{
                        base.FailDeleteMessage(ex);
					}
				}
				else if (base.GetQueryString("SetDefault") != null)
				{
                    int templateID = Int32.Parse(base.GetQueryString("TemplateID"));
			
					try
					{
                        TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, templateID);
                        if (templateInfo != null)
                        {
                            DataProvider.TemplateDAO.SetDefault(base.PublishmentSystemID, templateID);
                            StringUtility.AddLog(base.PublishmentSystemID, string.Format("设置默认{0}", ETemplateTypeUtils.GetText(templateInfo.TemplateType)), string.Format("模板名称:{0}", templateInfo.TemplateName));
                        }
						base.SuccessMessage();
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "操作失败");
					}
				}

                if (string.IsNullOrEmpty(this.templateType))
                {
                    string templateAddUrl = PageUtils.GetSTLUrl("background_templateAdd.aspx");
                    this.ltlCommands.Text = string.Format(@"
<input type=""button"" class=""btn"" onclick=""location.href='{0}?PublishmentSystemID={1}&TemplateType={2}';"" value=""添加首页模板"" />
<input type=""button"" class=""btn"" onclick=""location.href='{0}?PublishmentSystemID={1}&TemplateType={3}';"" value=""添加栏目模板"" />
<input type=""button"" class=""btn"" onclick=""location.href='{0}?PublishmentSystemID={1}&TemplateType={4}';"" value=""添加内容模板"" />
<input type=""button"" class=""btn"" onclick=""location.href='{0}?PublishmentSystemID={1}&TemplateType={5}';"" value=""添加单页模板"" />
", templateAddUrl, base.PublishmentSystemID, ETemplateTypeUtils.GetValue(ETemplateType.IndexPageTemplate), ETemplateTypeUtils.GetValue(ETemplateType.ChannelTemplate), ETemplateTypeUtils.GetValue(ETemplateType.ContentTemplate), ETemplateTypeUtils.GetValue(ETemplateType.FileTemplate));
                }
                else
                {
                    ETemplateType eTemplateType = ETemplateTypeUtils.GetEnumType(this.templateType);
                    string templateAddUrl = PageUtils.GetSTLUrl(string.Format(@"background_templateAdd.aspx?PublishmentSystemID={0}&TemplateType={1}", base.PublishmentSystemID, ETemplateTypeUtils.GetValue(eTemplateType)));
                    this.ltlCommands.Text = string.Format(@"
<input type=""button"" class=""btn btn-success"" onclick=""location.href='{0}';"" value=""添加{1}"" />
", templateAddUrl, ETemplateTypeUtils.GetText(eTemplateType));
                }

                this.dgContents.DataSource = DataProvider.TemplateDAO.GetDataSource(base.PublishmentSystemID, this.keywords, this.templateType);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
			}
		}

        public void btnSearch_Click(object sender, EventArgs e)
        {
            PageUtils.Redirect(PageUtils.GetSTLUrl(string.Format("background_template.aspx?PublishmentSystemID={0}&templateType={1}&keywords={2}", base.PublishmentSystemID, this.ddlTemplateType.SelectedValue, this.tbKeywords.Text)));
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int templateID = TranslateUtils.EvalInt(e.Item.DataItem, "TemplateID");
                ETemplateType templateType = ETemplateTypeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "TemplateType"));
                string templateName = TranslateUtils.EvalString(e.Item.DataItem, "TemplateName");
                string createdFileFullName = TranslateUtils.EvalString(e.Item.DataItem, "CreatedFileFullName");
                bool isDefault = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsDefault"));

                Literal ltlTemplateName = e.Item.FindControl("ltlTemplateName") as Literal;
                Literal ltlFileName = e.Item.FindControl("ltlFileName") as Literal;
                Literal ltlUseCount = e.Item.FindControl("ltlUseCount") as Literal;
                Literal ltlTemplateType = e.Item.FindControl("ltlTemplateType") as Literal;
                Literal ltlDefaultUrl = e.Item.FindControl("ltlDefaultUrl") as Literal;
                Literal ltlCopyUrl = e.Item.FindControl("ltlCopyUrl") as Literal;
                Literal ltlLogUrl = e.Item.FindControl("ltlLogUrl") as Literal;
                Literal ltlDesignUrl = e.Item.FindControl("ltlDesignUrl") as Literal;
                Literal ltlCreateUrl = e.Item.FindControl("ltlCreateUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                string templateAddUrl = PageUtils.GetSTLUrl(string.Format(@"background_templateAdd.aspx?PublishmentSystemID={0}&TemplateID={1}&TemplateType={2}", base.PublishmentSystemID, templateID, ETemplateTypeUtils.GetValue(templateType)));
                ltlTemplateName.Text = string.Format(@"<a href=""{0}"">{1}</a>", templateAddUrl, templateName);

                if (templateType == ETemplateType.IndexPageTemplate || templateType == ETemplateType.FileTemplate)
                {
                    string url = PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, createdFileFullName);
                    ltlFileName.Text = string.Format("<a href='{0}' target='_blank'>{1}</a>", url, createdFileFullName);
                }

                ltlUseCount.Text = DataProvider.TemplateDAO.GetTemplateUseCount(base.PublishmentSystemID, templateID, templateType, isDefault).ToString();

                ltlTemplateType.Text = ETemplateTypeUtils.GetText(templateType);

                if (templateType != ETemplateType.FileTemplate)
                {
                    if (isDefault)
                    {
                        ltlDefaultUrl.Text = "默认模板";
                    }
                    else
                    {
                        string defaultUrl = PageUtils.GetSTLUrl(string.Format(@"background_template.aspx?PublishmentSystemID={0}&TemplateID={1}&SetDefault=True", base.PublishmentSystemID, templateID));
                        ltlDefaultUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将把此模板设为默认，确认吗？');"">设为默认</a>", defaultUrl);
                    }
                }

                string copyUrl = PageUtils.GetSTLUrl(string.Format(@"background_templateAdd.aspx?PublishmentSystemID={0}&IsCopy=True&TemplateID={1}", base.PublishmentSystemID, templateID));
                ltlCopyUrl.Text = string.Format(@"<a href=""{0}"">快速复制</a>", copyUrl);

                string logUrl = PageUtils.GetSTLUrl(string.Format("background_templateLog.aspx?PublishmentSystemID={0}&TemplateID={1}", base.PublishmentSystemID, templateID));
                ltlLogUrl.Text = string.Format(@"<a href=""{0}"">修订历史</a>", logUrl);

                if (templateType == ETemplateType.IndexPageTemplate || templateType == ETemplateType.FileTemplate)
                {
                    string designUrl = PageUtility.DynamicPage.GetDesignUrl(PageUtility.DynamicPage.GetRedirectUrl(base.PublishmentSystemID, 0, 0, templateType == ETemplateType.FileTemplate ? templateID : 0, 0));
                    ltlDesignUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"">可视化编辑</a>", designUrl);
                }
                else
                {
                    ltlDesignUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">可视化编辑</a>", Modal.StlTemplate.StlTemplateSelect.GetOpenLayerString(base.PublishmentSystemID, templateType, templateID, false));
                }

                ltlCreateUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">生成页面</a>", ProgressBar.GetOpenWindowStringWithCreateByTemplate(base.PublishmentSystemID, templateID));

                if (!isDefault)
                {
                    string deleteUrl = PageUtils.GetSTLUrl(string.Format(@"background_template.aspx?PublishmentSystemID={0}&Delete=True&TemplateID={1}", base.PublishmentSystemID, templateID));
                    ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除模板“{1}”，确认吗？');"">删除</a>", deleteUrl, templateName);
                }
            }
        }

	}
}
