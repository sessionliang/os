using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using SiteServer.CMS.Controls;
using System.Web.UI.HtmlControls;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundConfigurationSite : BackgroundBasePage
	{
		public TextBox PublishmentSystemName;
        public AuxiliaryControl acAttributes;

        public PlaceHolder phVisutalType;
        public RadioButtonList IsStaticVisutalType;
        public DropDownList Charset;
        public TextBox PageSize;
        public RadioButtonList IsCountHits;
        public PlaceHolder phIsCountHitsByDay;
        public RadioButtonList IsCountHitsByDay;
        public RadioButtonList IsCountDownload;
        public RadioButtonList IsCreateDoubleClick;

        public Literal ltlSettings;

        private ArrayList relatedIdentities;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.Site, base.PublishmentSystemID, base.PublishmentSystemID);

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, "应用设置", AppManager.CMS.Permission.WebSite.Configration);

				PublishmentSystemName.Text = base.PublishmentSystemInfo.PublishmentSystemName;

                if (EPublishmentSystemTypeUtils.IsMobile(base.PublishmentSystemInfo.PublishmentSystemType))
                {
                    this.phVisutalType.Visible = false;
                }
                else
                {
                    this.phVisutalType.Visible = true;
                    EBooleanUtils.AddListItems(this.IsStaticVisutalType, "静态", "动态");
                    ControlUtils.SelectListItemsIgnoreCase(this.IsStaticVisutalType, (base.PublishmentSystemInfo.Additional.VisualType == EVisualType.Static).ToString());
                }

                ECharsetUtils.AddListItems(this.Charset);
                ControlUtils.SelectListItems(this.Charset, base.PublishmentSystemInfo.Additional.Charset);

                this.PageSize.Text = base.PublishmentSystemInfo.Additional.PageSize.ToString();

                EBooleanUtils.AddListItems(this.IsCountHits, "统计", "不统计");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCountHits, base.PublishmentSystemInfo.Additional.IsCountHits.ToString());

                EBooleanUtils.AddListItems(this.IsCountHitsByDay, "统计", "不统计");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCountHitsByDay, base.PublishmentSystemInfo.Additional.IsCountHitsByDay.ToString());

                EBooleanUtils.AddListItems(this.IsCountDownload, "统计", "不统计");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCountDownload, base.PublishmentSystemInfo.Additional.IsCountDownload.ToString());

                EBooleanUtils.AddListItems(this.IsCreateDoubleClick, "启用双击生成", "不启用");
                ControlUtils.SelectListItemsIgnoreCase(this.IsCreateDoubleClick, base.PublishmentSystemInfo.Additional.IsCreateDoubleClick.ToString());
                
                this.IsCountHits_SelectedIndexChanged(null, EventArgs.Empty);

                this.ltlSettings.Text = string.Format(@"<a class=""btn btn-success"" href=""{0}"">设置应用属性</a>", BackgroundTableStyle.GetRedirectUrl(base.PublishmentSystemID, ETableStyle.Site, DataProvider.PublishmentSystemDAO.TableName, base.PublishmentSystemID));

                this.acAttributes.SetParameters(base.PublishmentSystemInfo.Additional.Attributes, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.Site, DataProvider.PublishmentSystemDAO.TableName, true, base.IsPostBack);
            }
            else
            {
                this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, 0, this.relatedIdentities, ETableStyle.Site, DataProvider.PublishmentSystemDAO.TableName, true, base.IsPostBack);
            }
		}

        public void IsCountHits_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.phIsCountHitsByDay.Visible = TranslateUtils.ToBool(this.IsCountHits.SelectedValue);
        }

        public string GetChangeTabFunction()
        {
            if (!string.IsNullOrEmpty(base.Request.Form["index"]))
            {
                return string.Format(@"changeTab({0});", base.Request.Form["index"]);
            }
            return string.Empty;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
				base.PublishmentSystemInfo.PublishmentSystemName = PublishmentSystemName.Text;

                if (EPublishmentSystemTypeUtils.IsMobile(base.PublishmentSystemInfo.PublishmentSystemType))
                {
                    base.PublishmentSystemInfo.Additional.VisualType = EVisualType.Static;
                }
                else
                {
                    if (TranslateUtils.ToBool(this.IsStaticVisutalType.SelectedValue))
                    {
                        base.PublishmentSystemInfo.Additional.VisualType = EVisualType.Static;
                    }
                    else
                    {
                        base.PublishmentSystemInfo.Additional.VisualType = EVisualType.Dynamic;
                    }
                }

                if (base.PublishmentSystemInfo.Additional.Charset != this.Charset.SelectedValue)
                {
                    base.PublishmentSystemInfo.Additional.Charset = this.Charset.SelectedValue;
                }

                base.PublishmentSystemInfo.Additional.PageSize = TranslateUtils.ToInt(this.PageSize.Text, base.PublishmentSystemInfo.Additional.PageSize);
                base.PublishmentSystemInfo.Additional.IsCountHits = TranslateUtils.ToBool(this.IsCountHits.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsCountHitsByDay = TranslateUtils.ToBool(this.IsCountHitsByDay.SelectedValue);
                base.PublishmentSystemInfo.Additional.IsCountDownload = TranslateUtils.ToBool(this.IsCountDownload.SelectedValue);

                base.PublishmentSystemInfo.Additional.IsCreateDoubleClick = TranslateUtils.ToBool(this.IsCreateDoubleClick.SelectedValue);
                
				try
				{
                    //修改所有模板编码
                    ArrayList templateInfoArrayList = DataProvider.TemplateDAO.GetTemplateInfoArrayListByPublishmentSystemID(base.PublishmentSystemID);
                    ECharset charset = ECharsetUtils.GetEnumType(base.PublishmentSystemInfo.Additional.Charset);
                    foreach (TemplateInfo templateInfo in templateInfoArrayList)
                    {
                        if (templateInfo.Charset != charset)
                        {
                            string templateContent = CreateCacheManager.FileContent.GetTemplateContent(base.PublishmentSystemInfo, templateInfo);
                            templateInfo.Charset = charset;
                            DataProvider.TemplateDAO.Update(base.PublishmentSystemInfo, templateInfo, templateContent);
                        }
                    }

                    InputTypeParser.AddValuesToAttributes(ETableStyle.Site, DataProvider.PublishmentSystemDAO.TableName, base.PublishmentSystemInfo, this.relatedIdentities, this.Page.Request.Form, base.PublishmentSystemInfo.Additional.Attributes);

                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改应用设置");

					base.SuccessMessage("应用设置修改成功！");
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "应用设置修改失败！");
				}
			}
		}
	}
}
