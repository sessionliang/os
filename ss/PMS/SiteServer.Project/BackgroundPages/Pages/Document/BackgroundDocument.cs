using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.Project.Core;
using System.Web.UI;
using SiteServer.Project.Model;


namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundDocument : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Button AddButton;

        private int contractID;
        private int typeID;

        public static string GetRedirectUrlByContract(int contractID, string returnUrl)
        {
            return string.Format("background_document.aspx?ContractID={0}&ReturnUrl={1}", contractID, StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetRedirectUrlByCategory(int typeID, string returnUrl)
        {
            return string.Format("background_document.aspx?TypeID={0}&ReturnUrl={1}", typeID, StringUtils.ValueToUrl(returnUrl));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.typeID = TranslateUtils.ToInt(base.Request.QueryString["TypeID"]);
            this.contractID = TranslateUtils.ToInt(base.Request.QueryString["ContractID"]);

            if (base.Request.QueryString["Delete"] != null && base.Request.QueryString["DocumentID"] != null)
            {
                int documentID = TranslateUtils.ToInt(base.Request.QueryString["DocumentID"]);
                try
                {
                    DocumentManager.Delete(documentID);
                    base.SuccessMessage("成功删除文档");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除文档失败，{0}", ex.Message));
                }
            }

            if (!IsPostBack)
            {
                if (this.contractID > 0)
                {
                    this.dgContents.DataSource = DataProvider.DocumentDAO.GetDataSourceByContract(this.contractID);
                    this.AddButton.Attributes.Add("onclick", Modal.DocumentAdd.GetShowPopWinStringToAddByContract(this.contractID));
                }
                else if (this.typeID > 0)
                {
                    this.dgContents.DataSource = DataProvider.DocumentDAO.GetDataSourceByCategory(this.typeID);
                    this.AddButton.Attributes.Add("onclick", Modal.DocumentAdd.GetShowPopWinStringToAddByCategory(this.typeID));
                }
                
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int documentID = TranslateUtils.EvalInt(e.Item.DataItem, "DocumentID");
                EDocumentType documentType = EDocumentTypeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "DocumentType"));
                int contractID = TranslateUtils.EvalInt(e.Item.DataItem, "ContractID");
                int typeID = TranslateUtils.EvalInt(e.Item.DataItem, "TypeID");
                string fileName = TranslateUtils.EvalString(e.Item.DataItem, "FileName");
                string version = TranslateUtils.EvalString(e.Item.DataItem, "Version");
                string description = TranslateUtils.EvalString(e.Item.DataItem, "Description");
                string userName = TranslateUtils.EvalString(e.Item.DataItem, "UserName");
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");

                Literal ltlFileName = e.Item.FindControl("ltlFileName") as Literal;
                Literal ltlVersion = e.Item.FindControl("ltlVersion") as Literal;
                Literal ltlDescription = e.Item.FindControl("ltlDescription") as Literal;
                Literal ltlUserName = e.Item.FindControl("ltlUserName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                if (documentType == EDocumentType.Category)
                {
                    ltlFileName.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtils.Combine(DocumentManager.GetDirectoryUrlByCategory(typeID), fileName), fileName);
                }
                else if (documentType == EDocumentType.Contract)
                {
                    ltlFileName.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", PageUtils.Combine(DocumentManager.GetDirectoryUrlByContract(contractID), fileName), fileName);
                }
                
                ltlVersion.Text = version;
                ltlDescription.Text = description;
                ltlUserName.Text = AdminManager.GetDisplayName(userName, false);
                ltlAddDate.Text = DateUtils.GetDateString(addDate);
                if (userName == AdminManager.Current.UserName)
                {
                    ltlDeleteUrl.Text = string.Format(@"<a href=""background_document.aspx?ContractID={0}&TypeID={1}&Delete=True&DocumentID={2}"" onClick=""javascript:return confirm('此操作将删除文档“{3}”，确认吗？');"">删除</a>", contractID, typeID, documentID, fileName);
                }
            }
        }
    }
}
