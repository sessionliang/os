using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;
using BaiRong.Model;
using SiteServer.STL.Parser.StlElement;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTagStyleContentInput : BackgroundBasePage
	{
        public DataGrid dgContents;

        public Button AddButton;
        public Button Import;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if(!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Template, AppManager.CMS.LeftMenu.Template.ID_TagStyle, "内容提交样式", AppManager.CMS.Permission.WebSite.Template);

                if (base.GetQueryString("Delete") != null)
                {
                    int styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
                    string styleName = base.GetQueryString("StyleName");
                    try
                    {
                        DataProvider.TagStyleDAO.Delete(styleID);
                        StringUtility.AddLog(base.PublishmentSystemID, "删除内容提交样式", string.Format("样式名称:{0}", styleName));
                        
                        base.SuccessMessage("删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }

                this.AddButton.Attributes.Add("onclick", PageUtility.ModalSTL.TagStyleContentInputAdd_GetOpenWindowStringToAdd(base.PublishmentSystemID));
                this.Import.Attributes.Add("onclick", PageUtility.ModalSTL.Import.GetOpenWindowString(base.PublishmentSystemID, PageUtility.ModalSTL.Import.TYPE_TAGSTYLE));

                this.dgContents.DataSource = DataProvider.TagStyleDAO.GetDataSource(base.PublishmentSystemID, StlContentInput.ElementName);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
			}
		}

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int styleID = TranslateUtils.EvalInt(e.Item.DataItem, "StyleID");
                string styleName = TranslateUtils.EvalString(e.Item.DataItem, "StyleName");
                string settingsXML = TranslateUtils.EvalString(e.Item.DataItem, "SettingsXML");

                TagStyleContentInputInfo inputInfo = new TagStyleContentInputInfo(settingsXML);

                if (inputInfo.ChannelID == 0)
                {
                    inputInfo.ChannelID = base.PublishmentSystemID;
                }

                Literal ltlStyleName = (Literal)e.Item.FindControl("ltlStyleName");
                Literal ltlIsChecked = (Literal)e.Item.FindControl("ltlIsChecked");
                Literal ltlStyleUrl = (Literal)e.Item.FindControl("ltlStyleUrl");
                Literal ltlTemplateUrl = (Literal)e.Item.FindControl("ltlTemplateUrl");
                Literal ltlPreviewUrl = (Literal)e.Item.FindControl("ltlPreviewUrl");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlExportUrl = (Literal)e.Item.FindControl("ltlExportUrl");

                ltlStyleName.Text = styleName;
                ltlIsChecked.Text = StringUtils.GetTrueImageHtml(!inputInfo.IsChecked);

                ContentModelInfo modelInfo = ContentModelManager.GetContentModelInfo(base.PublishmentSystemInfo, EContentModelTypeUtils.GetValue(EContentModelType.Content));

                ltlStyleUrl.Text = string.Format(@"<a href=""{0}"">表单字段</a>", BackgroundTableStyle.GetRedirectUrl(base.PublishmentSystemID, ETableStyle.BackgroundContent, modelInfo.TableName, inputInfo.ChannelID));

                string returnUrl = PageUtils.UrlEncode(PageUtils.GetSTLUrl(string.Format("background_tagStyleContentInput.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));

                ltlTemplateUrl.Text = string.Format(@"<a href=""{0}"">自定义模板</a>", PageUtils.GetSTLUrl(string.Format("background_tagStyleTemplate.aspx?PublishmentSystemID={0}&StyleID={1}&ReturnUrl={2}", base.PublishmentSystemID, styleID, returnUrl)));

                ltlPreviewUrl.Text = string.Format(@"<a href=""{0}"">预览</a>", PageUtils.GetSTLUrl(string.Format("background_tagStylePreview.aspx?PublishmentSystemID={0}&StyleID={1}&ReturnUrl={2}", base.PublishmentSystemID, styleID, returnUrl)));

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", PageUtility.ModalSTL.TagStyleContentInputAdd_GetOpenWindowStringToEdit(base.PublishmentSystemID, styleID));

                ltlExportUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">导出</a>", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToTagStyle(base.PublishmentSystemID, styleID));
            }
        }
	}
}
