using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;
using BaiRong.Model;

using SiteServer.STL.Parser;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class BackgroundTagStyle : BackgroundBasePage
	{
		public DataGrid dgContents;

        public Button AddButton;
        public Button Import;

        private string elementName;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.elementName = base.GetQueryString("elementName");

			if(!IsPostBack)
            {
                string tagTitle = TagStyleUtility.GetTagStyleTitle(this.elementName);
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Template, AppManager.CMS.LeftMenu.Template.ID_TagStyle, tagTitle + "样式", AppManager.CMS.Permission.WebSite.Template);

                if (Request.QueryString["Delete"] != null)
                {
                    int styleID = TranslateUtils.ToInt(Request.QueryString["StyleID"]);
                    try
                    {
                        DataProvider.TagStyleDAO.Delete(styleID);
                        
                        base.SuccessMessage("样式删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "样式删除失败！");
                    }
                }

                this.AddButton.Attributes.Add("onclick", TagStyleUtility.GetOpenWindowStringToAdd(this.elementName, base.PublishmentSystemID));

                base.InfoMessage(string.Format(@"{0}标签为&lt;{1} styleName=""样式名称""&gt;&lt;/{1}&gt;", tagTitle, this.elementName));
                this.Import.Attributes.Add("onclick", PageUtility.ModalSTL.Import.GetOpenWindowString(base.PublishmentSystemID, PageUtility.ModalSTL.Import.TYPE_TAGSTYLE));

                this.dgContents.DataSource = DataProvider.TagStyleDAO.GetDataSource(base.PublishmentSystemID, this.elementName);
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

                Literal ltlStyleName = (Literal)e.Item.FindControl("ltlStyleName");
                Literal ltlTemplateUrl = (Literal)e.Item.FindControl("ltlTemplateUrl");
                Literal ltlPreviewUrl = (Literal)e.Item.FindControl("ltlPreviewUrl");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlExportUrl = (Literal)e.Item.FindControl("ltlExportUrl");
                Literal ltlDeleteUrl = (Literal)e.Item.FindControl("ltlDeleteUrl");

                ltlStyleName.Text = styleName;

                string returnUrl = PageUtils.UrlEncode(PageUtils.GetSTLUrl(string.Format("background_tagStyle.aspx?PublishmentSystemID={0}&elementName={1}", base.PublishmentSystemID, this.elementName)));

                ltlTemplateUrl.Text = string.Format(@"<a href=""{0}"">自定义模板</a>", PageUtils.GetSTLUrl(string.Format("background_tagStyleTemplate.aspx?PublishmentSystemID={0}&StyleID={1}&ReturnUrl={2}", base.PublishmentSystemID, styleID, returnUrl)));

                ltlPreviewUrl.Text = string.Format(@"<a href=""{0}"">预览</a>", PageUtils.GetSTLUrl(string.Format("background_tagStylePreview.aspx?PublishmentSystemID={0}&StyleID={1}&ReturnUrl={2}", base.PublishmentSystemID, styleID, returnUrl)));

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", TagStyleUtility.GetOpenWindowStringToEdit(this.elementName, base.PublishmentSystemID, styleID));

                ltlExportUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">导出</a>", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToTagStyle(base.PublishmentSystemID, styleID));

                string deleteUrl = PageUtils.GetSTLUrl(string.Format(@"background_tagStyle.aspx?elementName={0}&Delete=True&PublishmentSystemID={1}&StyleID={2}", this.elementName, base.PublishmentSystemID, styleID));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除样式“{1}”，确认吗？');"">删除</a>", deleteUrl, styleName);
            }
        }
	}
}
