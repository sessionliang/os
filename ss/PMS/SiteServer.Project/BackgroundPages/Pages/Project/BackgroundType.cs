using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Web.UI;
using SiteServer.Project.Model;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundType : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button AddButton;

        private int projectID;

        public void Page_Load(object sender, EventArgs E)
        {
            this.projectID = TranslateUtils.ToInt(base.Request.QueryString["ProjectID"]);

            if (base.Request.QueryString["Delete"] != null && base.Request.QueryString["TypeID"] != null)
            {
                int typeID = TranslateUtils.ToInt(base.Request.QueryString["TypeID"]);
                try
                {
                    DataProvider.TypeDAO.Delete(typeID);
                    base.SuccessMessage("成功删除办件类型");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除办件类型失败，{0}", ex.Message));
                }
            }
            else if ((base.Request.QueryString["Up"] != null || base.Request.QueryString["Down"] != null) && base.Request.QueryString["TypeID"] != null)
            {
                int typeID = TranslateUtils.ToInt(base.Request.QueryString["TypeID"]);
                bool isDown = (base.Request.QueryString["Down"] != null) ? true : false;
                if (isDown)
                {
                    DataProvider.TypeDAO.UpdateTaxisToUp(typeID, this.projectID);
                }
                else
                {
                    DataProvider.TypeDAO.UpdateTaxisToDown(typeID, this.projectID);
                }
            }

            if (!IsPostBack)
            {
                dgContents.DataSource = DataProvider.TypeDAO.GetDataSource(this.projectID);
                dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                dgContents.DataBind();

                this.AddButton.Attributes.Add("onclick", Modal.TypeAdd.GetShowPopWinStringToAdd(this.projectID));
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int typeID = TranslateUtils.EvalInt(e.Item.DataItem, "TypeID");
                string typeName = TranslateUtils.EvalString(e.Item.DataItem, "TypeName");

                Literal ltlTypeName = e.Item.FindControl("ltlTypeName") as Literal;
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlTypeName.Text = typeName;

                hlUpLinkButton.NavigateUrl = string.Format("background_type.aspx?ProjectID={0}&TypeID={1}&Up=True", this.projectID, typeID);

                hlDownLinkButton.NavigateUrl = string.Format("background_type.aspx?ProjectID={0}&TypeID={1}&Down=True", this.projectID, typeID);

                ltlEditUrl.Text = string.Format(@"<a href='javascript:undefined' onclick=""{0}"">编辑</a>", Modal.TypeAdd.GetShowPopWinStringToEdit(this.projectID, typeID));

                ltlDeleteUrl.Text = string.Format(@"<a href=""background_type.aspx?ProjectID={0}&Delete=True&TypeID={1}"" onClick=""javascript:return confirm('此操作将删除办件类型“{2}”，确认吗？');"">删除</a>", this.projectID, typeID, typeName);
            }
        }
    }
}
