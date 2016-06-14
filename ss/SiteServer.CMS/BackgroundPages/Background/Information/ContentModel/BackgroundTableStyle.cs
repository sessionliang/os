using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundTableStyle : BackgroundBasePage
    {
        public DataGrid dgContents;

        public Button btnAddStyle;
        public Button btnAddStyles;
        public Button btnImport;
        public Button btnExport;
        public Button btnReturn;

        private ETableStyle tableStyle;
        private int relatedIdentity;
        private ArrayList relatedIdentities;
        private string tableName;
        private int itemID;

        public static string GetRedirectUrl(int publishmentSystemID, ETableStyle tableStyle, string tableName, int relatedIdentity)
        {
            return PageUtils.GetCMSUrl(string.Format("background_tableStyle.aspx?PublishmentSystemID={0}&TableStyle={1}&TableName={2}&RelatedIdentity={3}", publishmentSystemID, ETableStyleUtils.GetValue(tableStyle), tableName, relatedIdentity));
        }
        public static string GetRedirectUrl(int publishmentSystemID, ETableStyle tableStyle, string tableName, int relatedIdentity, int itemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_tableStyle.aspx?PublishmentSystemID={0}&TableStyle={1}&TableName={2}&RelatedIdentity={3}&itemID={4}", publishmentSystemID, ETableStyleUtils.GetValue(tableStyle), tableName, relatedIdentity, itemID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (string.IsNullOrEmpty(base.GetQueryString("RelatedIdentity")))
            {
                this.relatedIdentity = base.PublishmentSystemID;
            }
            else
            {
                this.relatedIdentity = base.GetIntQueryString("RelatedIdentity");
            }

            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            this.tableName = base.GetQueryString("TableName");
            this.itemID = base.GetIntQueryString("itemID");

            this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(this.tableStyle, base.PublishmentSystemID, this.relatedIdentity);


            if (!IsPostBack)
            {
                if (this.tableStyle == ETableStyle.InputContent)
                {
                    base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Input, "提交表单管理", AppManager.CMS.Permission.WebSite.Input);
                }
                if (this.tableStyle == ETableStyle.ClassifyInputContent)
                {
                    base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Input, "提交表单管理", AppManager.CMS.Permission.WebSite.Input);
                }
                else if (this.tableStyle == ETableStyle.Site)
                {
                    base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Configration, "应用属性设置", AppManager.CMS.Permission.WebSite.Configration);
                }


                //删除样式
                if (base.GetQueryString("DeleteStyle") != null)
                {
                    DeleteStyle();
                }
                else if (base.GetQueryString("SetTaxis") != null)
                {
                    SetTaxis();
                }

                if (this.tableStyle == ETableStyle.BackgroundContent)
                {
                    string urlModel = PageUtils.GetCMSUrl(string.Format("background_contentModel.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
                    this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", urlModel));
                }
                else if (this.tableStyle == ETableStyle.InputContent)
                {
                    string urlReturn = PageUtils.GetCMSUrl(string.Format("background_input.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
                    this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", urlReturn));
                }
                else if (this.tableStyle == ETableStyle.ClassifyInputContent)// by 20151028 sofuny
                {
                    string urlReturn = PageUtils.GetCMSUrl(string.Format("background_inputMainContent.aspx?PublishmentSystemID={0}&itemID={1}", base.PublishmentSystemID, this.itemID));
                    this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", urlReturn));
                }
                else if (this.tableStyle == ETableStyle.GovInteractContent)
                {
                    string urlReturn = PageUtils.GetWCMUrl(string.Format("background_govInteract.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
                    this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", urlReturn));
                }
                else if (this.tableStyle == ETableStyle.Site)
                {
                    string urlReturn = PageUtils.GetCMSUrl(string.Format("background_configurationSite.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
                    this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", urlReturn));
                }
                else
                {
                    this.btnReturn.Visible = false;
                }

                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);

                this.dgContents.DataSource = styleInfoArrayList;
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                string redirectUrl = PageUtils.GetCMSUrl(string.Format("background_tableStyle.aspx?PublishmentSystemID={0}&TableStyle={1}&RelatedIdentity={2}&tableName={3}&itemID={4}", base.PublishmentSystemID, ETableStyleUtils.GetValue(this.tableStyle), this.relatedIdentity, this.tableName, this.itemID));
                this.btnAddStyle.Attributes.Add("onclick", Modal.TableStyleAdd.GetOpenWindowString(base.PublishmentSystemID, 0, this.relatedIdentities, this.tableName, string.Empty, this.tableStyle, redirectUrl));

                redirectUrl = PageUtils.GetCMSUrl(string.Format("background_tableStyle.aspx?PublishmentSystemID={0}&TableStyle={1}&RelatedIdentity={2}&tableName={3}&itemID={4}", base.PublishmentSystemID, ETableStyleUtils.GetValue(this.tableStyle), this.relatedIdentity, this.tableName, this.itemID));
                this.btnAddStyles.Attributes.Add("onclick", Modal.TableStylesAdd.GetOpenWindowString(base.PublishmentSystemID, this.relatedIdentities, this.tableName, this.tableStyle, redirectUrl));

                this.btnImport.Attributes.Add("onclick", PageUtility.ModalSTL.TableStyleImport_GetOpenWindowString(this.tableName, this.tableStyle, base.PublishmentSystemID, this.relatedIdentity));
                this.btnExport.Attributes.Add("onclick", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToSingleTableStyle(this.tableStyle, this.tableName, base.PublishmentSystemID, this.relatedIdentity));
            }
        }

        private void DeleteStyle()
        {
            string attributeName = base.GetQueryString("AttributeName");
            if (TableStyleManager.IsExists(this.relatedIdentity, this.tableName, attributeName))
            {
                try
                {
                    TableStyleManager.Delete(this.relatedIdentity, this.tableName, attributeName);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除数据表单样式", string.Format("表单:{0},字段:{1}", this.tableName, attributeName));
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
        }

        private void SetTaxis()
        {
            int tableStyleID = TranslateUtils.ToInt(base.GetQueryString("TableStyleID"));
            TableStyleInfo styleInfo = BaiRongDataProvider.TableStyleDAO.GetTableStyleInfo(tableStyleID);
            if (styleInfo != null && styleInfo.RelatedIdentity == this.relatedIdentity)
            {
                string direction = base.GetQueryString("Direction");

                switch (direction.ToUpper())
                {
                    case "UP":
                        BaiRongDataProvider.TableStyleDAO.TaxisDown(tableStyleID);
                        break;
                    case "DOWN":
                        BaiRongDataProvider.TableStyleDAO.TaxisUp(tableStyleID);
                        break;
                    default:
                        break;
                }
                base.SuccessMessage("排序成功！");
            }
            else
            {
                string direction = base.GetQueryString("Direction");
                int tableMetadataId = TranslateUtils.ToInt(base.GetQueryString("TableMetadataId"));
                switch (direction.ToUpper())
                {
                    case "UP":
                        BaiRongDataProvider.TableMetadataDAO.TaxisDown(tableMetadataId, this.tableName);
                        break;
                    case "DOWN":
                        BaiRongDataProvider.TableMetadataDAO.TaxisUp(tableMetadataId, this.tableName);
                        break;
                    default:
                        break;
                }
                base.SuccessMessage("排序成功！");
            }
        }

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableStyleInfo styleInfo = e.Item.DataItem as TableStyleInfo;

                Literal ltlAttributeName = e.Item.FindControl("ltlAttributeName") as Literal;
                Literal ltlDataType = e.Item.FindControl("ltlDataType") as Literal;
                Literal ltlDisplayName = e.Item.FindControl("ltlDisplayName") as Literal;
                Literal ltlInputType = e.Item.FindControl("ltlInputType") as Literal;
                Literal ltlIsVisible = e.Item.FindControl("ltlIsVisible") as Literal;
                Literal ltlValidate = e.Item.FindControl("ltlValidate") as Literal;
                Literal ltlEditStyle = e.Item.FindControl("ltlEditStyle") as Literal;
                Literal ltlEditValidate = e.Item.FindControl("ltlEditValidate") as Literal;
                HyperLink upLinkButton = e.Item.FindControl("UpLinkButton") as HyperLink;
                HyperLink downLinkButton = e.Item.FindControl("DownLinkButton") as HyperLink;

                string showPopWinString = Modal.TableMetadataView.GetOpenWindowString(ETableStyleUtils.GetTableType(this.tableStyle), this.tableName, styleInfo.AttributeName, this.relatedIdentities);
                ltlAttributeName.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, styleInfo.AttributeName);

                ltlDisplayName.Text = styleInfo.DisplayName;
                ltlInputType.Text = EInputTypeUtils.GetText(styleInfo.InputType);

                ltlIsVisible.Text = StringUtils.GetTrueOrFalseImageHtml(styleInfo.IsVisible.ToString());
                ltlValidate.Text = EInputValidateTypeUtils.GetValidateInfo(styleInfo);

                string redirectUrl = PageUtils.GetCMSUrl(string.Format("background_tableStyle.aspx?PublishmentSystemID={0}&TableStyle={1}&RelatedIdentity={2}&tableName={3}&itemID={4}", base.PublishmentSystemID, ETableStyleUtils.GetValue(this.tableStyle), this.relatedIdentity, this.tableName, this.itemID));
                showPopWinString = Modal.TableStyleAdd.GetOpenWindowString(base.PublishmentSystemID, styleInfo.TableStyleID, this.relatedIdentities, this.tableName, styleInfo.AttributeName, this.tableStyle, redirectUrl);
                string editText = "设置";
                if (styleInfo.RelatedIdentity == this.relatedIdentity)//数据库中有样式
                {
                    editText = "修改";
                }
                ltlEditStyle.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, editText);

                showPopWinString = Modal.TableStyleValidateAdd.GetOpenWindowString(styleInfo.TableStyleID, this.relatedIdentities, this.tableName, styleInfo.AttributeName, this.tableStyle, redirectUrl);
                ltlEditValidate.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">设置</a>", showPopWinString);

                if (styleInfo.RelatedIdentity == this.relatedIdentity)//数据库中有样式
                {
                    string urlStyle = PageUtils.GetCMSUrl(string.Format("background_tableStyle.aspx?PublishmentSystemID={0}&TableStyle={1}&RelatedIdentity={2}&DeleteStyle=True&TableName={3}&AttributeName={4}", base.PublishmentSystemID, ETableStyleUtils.GetValue(this.tableStyle), this.relatedIdentity, this.tableName, styleInfo.AttributeName));
                    ltlEditStyle.Text += string.Format(@"&nbsp;&nbsp;<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除对应显示样式，确认吗？');"">删除</a>", urlStyle);
                }

                bool isTaxisVisible = true;
                //if (TableStyleManager.IsMetadata(this.tableStyle, styleInfo.AttributeName) || styleInfo.RelatedIdentity != this.relatedIdentity)
                //{
                //    isTaxisVisible = false;
                //}
                //else
                //{
                isTaxisVisible = !TableStyleManager.IsExistsInParents(this.relatedIdentities, this.tableName, styleInfo.AttributeName);
                //}

                if (!isTaxisVisible)
                {
                    upLinkButton.Visible = downLinkButton.Visible = false;
                }
                else
                {
                    int tableMetadataID = BaiRongDataProvider.TableMetadataDAO.GetTableMetadataID(styleInfo.TableName, styleInfo.AttributeName);

                    upLinkButton.NavigateUrl = PageUtils.GetCMSUrl(string.Format("background_tableStyle.aspx?PublishmentSystemID={0}&SetTaxis=True&TableStyleID={1}&Direction=UP&TableMetadataId={2}&tableName={3}&TableStyle={4}&RelatedIdentity={5}", base.PublishmentSystemID, styleInfo.TableStyleID, tableMetadataID, this.tableName, ETableStyleUtils.GetValue(this.tableStyle), this.relatedIdentity));
                    downLinkButton.NavigateUrl = PageUtils.GetCMSUrl(string.Format("background_tableStyle.aspx?PublishmentSystemID={0}&SetTaxis=True&TableStyleID={1}&Direction=DOWN&TableMetadataId={2}&tableName={3}&TableStyle={4}&RelatedIdentity={5}", base.PublishmentSystemID, styleInfo.TableStyleID, tableMetadataID, this.tableName, ETableStyleUtils.GetValue(this.tableStyle), this.relatedIdentity));
                }
            }
        }
    }
}
