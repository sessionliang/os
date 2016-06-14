using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CRM.Core;
using System.Web.UI;
using SiteServer.CRM.Model;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages
{
    public class BackgroundDocType : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button AddButton;
        private int docTypeID;

        public void Page_Load(object sender, EventArgs E)
        {
            this.docTypeID = TranslateUtils.ToInt(base.Request.QueryString["DocTypeID"]);

            if (base.Request.QueryString["Delete"] != null && base.Request.QueryString["TypeID"] != null)
            {
                int typeID = TranslateUtils.ToInt(base.Request.QueryString["TypeID"]);
                try
                {
                    DataProvider.DocTypeDAO.Delete(typeID);
                    base.SuccessMessage("成功删除文档类别");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除文档类别失败，{0}", ex.Message));
                }
            }
            else if ((base.Request.QueryString["Up"] != null || base.Request.QueryString["Down"] != null) && base.Request.QueryString["TypeID"] != null)
            {
                int typeID = TranslateUtils.ToInt(base.Request.QueryString["TypeID"]);
                int parentID = TranslateUtils.ToInt(base.Request.QueryString["ParentID"]);
                bool isDown = (base.Request.QueryString["Down"] != null) ? true : false;
                if (isDown)
                {
                    DataProvider.DocTypeDAO.UpdateTaxisToUp(parentID, typeID);
                }
                else
                {
                    DataProvider.DocTypeDAO.UpdateTaxisToDown(parentID, typeID);
                }
            }

            if (!IsPostBack)
            {
                this.dgContents.DataSource = DataProvider.DocTypeDAO.GetDataSource(this.docTypeID);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddButton.Attributes.Add("onclick", Modal.DocTypeAdd.GetShowPopWinStringToAdd(this.docTypeID));
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int typeID = TranslateUtils.EvalInt(e.Item.DataItem, "TypeID");
                int parentID = TranslateUtils.EvalInt(e.Item.DataItem, "ParentID");
                string typeName = TranslateUtils.EvalString(e.Item.DataItem, "TypeName");
                string description = TranslateUtils.EvalString(e.Item.DataItem, "Description");

                Literal ltlTypeName = e.Item.FindControl("ltlTypeName") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                HyperLink hlUpLinkButton = e.Item.FindControl("hlUpLinkButton") as HyperLink;
                HyperLink hlDownLinkButton = e.Item.FindControl("hlDownLinkButton") as HyperLink;
                Literal ltlDocumentUrl = e.Item.FindControl("ltlDocumentUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                if (this.docTypeID == 0)
                {
                    ltlTypeName.Text = string.Format(@"<a href=""background_docType.aspx?DocTypeID={0}"">{1}</a>", typeID, typeName);
                }
                else
                {
                    ltlTypeName.Text = typeName;
                }
                ltlDescription.Text = description;

                hlUpLinkButton.NavigateUrl = string.Format("background_docType.aspx?DocTypeID={0}&TypeID={1}&ParentID={2}&Up=True", this.docTypeID, typeID, parentID);

                hlDownLinkButton.NavigateUrl = string.Format("background_docType.aspx?DocTypeID={0}&TypeID={1}&ParentID={2}&Down=True", this.docTypeID, typeID, parentID);

                int count = DataProvider.DocumentDAO.GetCountByCategory(typeID);
                if (count == 0)
                {
                    ltlDocumentUrl.Text = string.Format(@"<a href=""{0}"">文档列表</a>", BackgroundDocument.GetRedirectUrlByCategory(typeID, string.Format("background_docType.aspx?DocTypeID={0}", typeID)));
                }
                else
                {
                    ltlDocumentUrl.Text = string.Format(@"<a href=""{0}"">文档列表({1})</a>", BackgroundDocument.GetRedirectUrlByCategory(typeID, string.Format("background_docType.aspx?DocTypeID={0}", typeID)), count);
                }

                ltlEditUrl.Text = string.Format(@"<a href='javascript:undefined' onclick=""{0}"">编辑</a>", Modal.DocTypeAdd.GetShowPopWinStringToEdit(typeID));

                ltlDeleteUrl.Text = string.Format(@"<a href=""background_docType.aspx?DocTypeID={0}&Delete=True&TypeID={1}"" onClick=""javascript:return confirm('此操作将删除文档类别“{2}”及其文档，确认吗？');"">删除</a>", this.docTypeID, typeID, typeName);
            }
        }
    }
}
