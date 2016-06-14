using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Web.UI;


namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundFormPage : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button btnAdd;

        private int mobanID;

        public void Page_Load(object sender, EventArgs E)
        {
            this.mobanID = TranslateUtils.ToInt(base.Request.QueryString["mobanID"]);

            if (base.Request.QueryString["Delete"] != null && base.Request.QueryString["ID"] != null)
            {
                try
                {
                    DataProvider.FormPageDAO.Delete(TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["ID"]));
                    base.SuccessMessage("成功删除分页");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除分页失败，{0}", ex.Message));
                }
            }
            else if ((base.Request.QueryString["Up"] != null || base.Request.QueryString["Down"] != null) && base.Request.QueryString["ID"] != null)
            {
                int id = TranslateUtils.ToInt(base.Request.QueryString["ID"]);
                bool isDown = (base.Request.QueryString["Down"] != null) ? true : false;
                if (isDown)
                {
                    DataProvider.FormPageDAO.UpdateTaxisToUp(id, this.mobanID);
                }
                else
                {
                    DataProvider.FormPageDAO.UpdateTaxisToDown(id, this.mobanID);
                }
            }

            if (!IsPostBack)
            {
                BindGrid();

                this.btnAdd.Attributes.Add("onclick", Modal.FormPageAdd.GetOpenWindowStringToAdd(this.mobanID));
            }
        }

        public void BindGrid()
        {
            this.dgContents.DataSource = DataProvider.FormPageDAO.GetDataSource(this.mobanID);
            this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
            this.dgContents.DataBind();
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                FormPageInfo formPageInfo = new FormPageInfo(e.Item.DataItem);

                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlTitle.Text = string.Format(@"<a href=""background_form.aspx?mobanID={0}&pageID={1}"" target=""field"">{2}</a>", this.mobanID, formPageInfo.ID, formPageInfo.Title);

                hlUpLinkButton.NavigateUrl = string.Format("?mobanID={0}&id={1}&up=true", this.mobanID, formPageInfo.ID);

                hlDownLinkButton.NavigateUrl = string.Format("?mobanID={0}&id={1}&down=true", this.mobanID, formPageInfo.ID);

                ltlEditUrl.Text = string.Format(@"<a href='javascript:;' onclick=""{0}"">编辑</a>", Modal.FormPageAdd.GetOpenWindowStringToEdit(this.mobanID, formPageInfo.ID));

                ltlDeleteUrl.Text = string.Format(@"<a href=""?mobanID={0}&Delete=True&id={1}"" onClick=""javascript:return confirm('此操作将删除分页及其表单项，确认吗？');"">删除</a>", this.mobanID, formPageInfo.ID);
            }
        }
    }
}
