using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;
using BaiRong.Model;


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundContentModel : BackgroundBasePage
	{
        public DataGrid dgContents;
        public Button AddContentModel;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("Delete") != null)
            {
                string modelID = base.GetQueryString("ModelID");
                try
                {
                    BaiRongDataProvider.ContentModelDAO.Delete(modelID, AppManager.CMS.AppID, base.PublishmentSystemID);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除内容模型", string.Format("内容模型:{0}", modelID));
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

			if(!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_ContentModel, "内容模型管理", AppManager.CMS.Permission.WebSite.ContentModel);

                this.dgContents.DataSource = ContentModelManager.GetContentModelArrayList(base.PublishmentSystemInfo);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddContentModel.Attributes.Add("onclick", Modal.ContentModelAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
			}
		}

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ContentModelInfo modelInfo = e.Item.DataItem as ContentModelInfo;

                Literal ltlItemIcon = (Literal)e.Item.FindControl("ltlItemIcon");
                Literal ltlModelID = (Literal)e.Item.FindControl("ltlModelID");
                Literal ltlModelName = (Literal)e.Item.FindControl("ltlModelName");
                Literal ltlDescription = (Literal)e.Item.FindControl("ltlDescription");
                Literal ltlTableName = (Literal)e.Item.FindControl("ltlTableName");
                Literal ltlFieldUrl = (Literal)e.Item.FindControl("ltlFieldUrl");
                Literal ltlStyleUrl = (Literal)e.Item.FindControl("ltlStyleUrl");
                Literal ltlEditUrl = (Literal)e.Item.FindControl("ltlEditUrl");
                Literal ltlDeleteUrl = (Literal)e.Item.FindControl("ltlDeleteUrl");

                if (!string.IsNullOrEmpty(modelInfo.IconUrl))
                {
                    ltlItemIcon.Text = string.Format("<img src='{0}' />", PageUtils.GetIconUrl(string.Format("tree/{0}", modelInfo.IconUrl)));
                }
                ltlModelID.Text = modelInfo.ModelID;
                ltlModelName.Text = modelInfo.ModelName;
                ltlTableName.Text = modelInfo.TableName;
                ltlDescription.Text = modelInfo.Description;

                ltlFieldUrl.Text = string.Format(@"<a href=""{0}"">真实字段管理</a>", ConsoleTableMetadata.GetRedirectUrl(modelInfo.TableName, modelInfo.TableType, base.PublishmentSystemID));

                ltlStyleUrl.Text = string.Format(@"<a href=""{0}"">虚拟字段管理</a>", BackgroundTableStyle.GetRedirectUrl(base.PublishmentSystemID, EAuxiliaryTableTypeUtils.GetTableStyle(modelInfo.TableType), modelInfo.TableName, base.PublishmentSystemID));

                if (!modelInfo.IsSystem)
                {
                    ltlEditUrl.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.ContentModelAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, modelInfo.ModelID));

                    string urlDelete = PageUtils.GetCMSUrl(string.Format("background_contentModel.aspx?PublishmentSystemID={0}&ModelID={1}&Delete=True", base.PublishmentSystemID, modelInfo.ModelID));
                    ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除内容模型“{1}”，确认吗？');"">删除</a>", urlDelete, modelInfo.ModelName);
                }
            }
        }
	}
}
