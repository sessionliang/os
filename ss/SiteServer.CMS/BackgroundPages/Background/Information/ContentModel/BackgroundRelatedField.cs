using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Web.UI;

namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundRelatedField : BackgroundBasePage
	{
        public DataGrid dgContents;
		public Button AddButton;
        public Button ImportButton;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (base.GetQueryString("Delete") != null)
			{
                int relatedFieldID = TranslateUtils.ToInt(base.GetQueryString("RelatedFieldID"));
			
				try
				{
                    string relatedFieldName = DataProvider.RelatedFieldDAO.GetRelatedFieldName(relatedFieldID);
                    DataProvider.RelatedFieldDAO.Delete(relatedFieldID);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除联动字段", string.Format("联动字段:{0}", relatedFieldName));
					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
					base.FailDeleteMessage(ex);
				}
			}
			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_ContentModel, "联动字段管理", AppManager.CMS.Permission.WebSite.ContentModel);

                this.dgContents.DataSource = DataProvider.RelatedFieldDAO.GetDataSource(base.PublishmentSystemID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                string showPopWinString = Modal.RelatedFieldAdd.GetOpenWindowString(base.PublishmentSystemID);
				this.AddButton.Attributes.Add("onclick", showPopWinString);
                this.ImportButton.Attributes.Add("onclick", PageUtility.ModalSTL.Import.GetOpenWindowString(base.PublishmentSystemID, PageUtility.ModalSTL.Import.TYPE_RELATED_FIELD));
			}
		}

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int relatedFieldID = TranslateUtils.EvalInt(e.Item.DataItem, "RelatedFieldID");
                string relatedFieldName = TranslateUtils.EvalString(e.Item.DataItem, "RelatedFieldName");
                int totalLevel = TranslateUtils.EvalInt(e.Item.DataItem, "TotalLevel");

                Literal ltlRelatedFieldName = (Literal)e.Item.FindControl("ltlRelatedFieldName");
                Literal ltlTotalLevel = (Literal)e.Item.FindControl("ltlTotalLevel");
                Literal ltlItemsUrl = (Literal)e.Item.FindControl("ltlItemsUrl");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlExportUrl = (Literal)e.Item.FindControl("ltlExportUrl");
                Literal ltlDeleteUrl = (Literal)e.Item.FindControl("ltlDeleteUrl");

                ltlRelatedFieldName.Text = relatedFieldName;
                ltlTotalLevel.Text = totalLevel.ToString();
                string urlItems = PageUtils.GetCMSUrl(string.Format("background_relatedFieldMain.aspx?PublishmentSystemID={0}&RelatedFieldID={1}&TotalLevel={2}", base.PublishmentSystemID, relatedFieldID, totalLevel));
                ltlItemsUrl.Text = string.Format(@"<a href=""{0}"">管理字段项</a>", urlItems);

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.RelatedFieldAdd.GetOpenWindowString(base.PublishmentSystemID, relatedFieldID));
                ltlExportUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">导出</a>", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToRelatedField(base.PublishmentSystemID, relatedFieldID));
                ltlDeleteUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">删除</a>", JsUtils.GetRedirectStringWithConfirm(PageUtils.GetCMSUrl(string.Format("background_relatedField.aspx?PublishmentSystemID={0}&Delete=True&RelatedFieldID={1}", base.PublishmentSystemID, relatedFieldID)), "确认删除此联动字段吗？"));
            }
        }
	}
}
