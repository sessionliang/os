using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Web.UI;
using SiteServer.CMS.Model;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovInteractType : BackgroundGovInteractBasePage
    {
        public DataGrid dgContents;
        public Button AddButton;

        private int nodeID;

        public void Page_Load(object sender, EventArgs E)
        {
            this.nodeID = TranslateUtils.ToInt(base.Request.QueryString["NodeID"]);

            if (base.Request.QueryString["Delete"] != null && base.Request.QueryString["TypeID"] != null)
            {
                int typeID = TranslateUtils.ToInt(base.Request.QueryString["TypeID"]);
                try
                {
                    DataProvider.GovInteractTypeDAO.Delete(typeID);
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
                    DataProvider.GovInteractTypeDAO.UpdateTaxisToUp(typeID, this.nodeID);
                }
                else
                {
                    DataProvider.GovInteractTypeDAO.UpdateTaxisToDown(typeID, this.nodeID);
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovInteract, AppManager.CMS.LeftMenu.GovInteract.ID_GovInteractConfiguration, "办件类型管理", AppManager.CMS.Permission.WebSite.GovInteractConfiguration);

                this.dgContents.DataSource = DataProvider.GovInteractTypeDAO.GetDataSource(this.nodeID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddButton.Attributes.Add("onclick", Modal.GovInteractTypeAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, this.nodeID));
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

                hlUpLinkButton.NavigateUrl = PageUtils.GetWCMUrl(string.Format("background_govInteractType.aspx?PublishmentSystemID={0}&NodeID={1}&TypeID={2}&Up=True", base.PublishmentSystemID, this.nodeID, typeID));

                hlDownLinkButton.NavigateUrl = PageUtils.GetWCMUrl(string.Format("background_govInteractType.aspx?PublishmentSystemID={0}&NodeID={1}&TypeID={2}&Down=True", base.PublishmentSystemID, this.nodeID, typeID));

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.GovInteractTypeAdd.GetOpenWindowStringToEdit(base.PublishmentSystemID, this.nodeID, typeID));

                string urlDelete = PageUtils.GetWCMUrl(string.Format("background_govInteractType.aspx?PublishmentSystemID={0}&NodeID={1}&Delete=True&TypeID={2}", base.PublishmentSystemID, this.nodeID, typeID));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除办件类型“{1}”，确认吗？');"">删除</a>", urlDelete, typeName);
            }
        }
    }
}
