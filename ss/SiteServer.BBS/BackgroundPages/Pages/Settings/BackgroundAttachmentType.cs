using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundAttachmentType : BackgroundBasePage
    {
        public DataGrid MyDataGrid;
        public Button AddButton;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_attachmentType.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                int id = base.GetIntQueryString("ID");
                try
                {
                    DataProvider.AttachmentTypeDAO.Delete(id);
                    base.SuccessMessage("成功删除限制");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除限制失败，{0}", ex.Message));
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "附件上传设置", AppManager.BBS.Permission.BBS_Settings);

                MyDataGrid.DataSource = DataProvider.AttachmentTypeDAO.GetList(base.PublishmentSystemID);
                MyDataGrid.ItemDataBound += new DataGridItemEventHandler(MyDataGrid_ItemDataBound);
                MyDataGrid.DataBind();

                this.AddButton.Attributes.Add("onclick", Modal.AttachmentTypeAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID));
            }
        }

        public void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                AttachmentTypeInfo typeInfo = e.Item.DataItem as AttachmentTypeInfo;

                Literal ltlFileExtName = e.Item.FindControl("ltlFileExtName") as Literal;
                Literal ltlMaxSize = e.Item.FindControl("ltlMaxSize") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlFileExtName.Text = typeInfo.FileExtName;
                ltlMaxSize.Text = typeInfo.MaxSize.ToString();

                ltlEditUrl.Text = string.Format(@"<a href=""javascript:void(0);"" onclick=""{0}"">编辑</a>", Modal.AttachmentTypeAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, typeInfo.ID));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&ID={1}&Delete=True"" onClick=""javascript:return confirm('此操作将删除此限制，确认吗？');"">删除</a>", BackgroundAttachmentType.GetRedirectUrl(base.PublishmentSystemID), typeInfo.ID);
            }
        }
    }
}
