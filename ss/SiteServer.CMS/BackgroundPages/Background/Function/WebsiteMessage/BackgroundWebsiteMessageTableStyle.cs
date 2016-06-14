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
    public class BackgroundWebsiteMessageTableStyle : BackgroundBasePage
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
        private WebsiteMessageInfo websiteMessageInfo;

        public static string GetRedirectUrl(int publishmentSystemID, int relatedIdentity)
        {
            return PageUtils.GetCMSUrl(string.Format("background_websiteMessageTableStyle.aspx?PublishmentSystemID={0}&RelatedIdentity={2}", publishmentSystemID, relatedIdentity));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            string WebsiteMessageName = base.GetQueryStringNoSqlAndXss("WebsiteMessageName");
            this.websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(WebsiteMessageName, base.PublishmentSystemID);

            if (string.IsNullOrEmpty(base.GetQueryString("RelatedIdentity")))
            {
                this.relatedIdentity = this.websiteMessageInfo.WebsiteMessageID;
            }
            else
            {
                this.relatedIdentity = base.GetIntQueryString("RelatedIdentity");
            }

            this.tableStyle = ETableStyle.WebsiteMessageContent;
            this.tableName = DataProvider.WebsiteMessageContentDAO.TableName;

            this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(this.tableStyle, base.PublishmentSystemID, this.relatedIdentity);


            if (!IsPostBack)
            {

                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_WebsiteMessage, "字段管理", AppManager.CMS.Permission.WebSite.WebsiteMessage);

                #region 默认创建一个网站留言，网站留言分类
                DataProvider.WebsiteMessageClassifyDAO.SetDefaultWebsiteMessageClassifyInfo(base.PublishmentSystemID);
                DataProvider.WebsiteMessageDAO.SetDefaultWebsiteMessageInfo(base.PublishmentSystemID);
                #endregion

                //删除样式
                if (base.GetQueryString("DeleteStyle") != null)
                {
                    DeleteStyle();
                }
                else if (base.GetQueryString("SetTaxis") != null)
                {
                    SetTaxis();
                }


                string urlReturn = PageUtils.GetCMSUrl(string.Format("background_websiteMessage.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
                this.btnReturn.Attributes.Add("onclick", string.Format("location.href='{0}';return false;", urlReturn));


                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);

                this.dgContents.DataSource = styleInfoArrayList;
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                string redirectUrl = PageUtils.GetCMSUrl(string.Format("background_websiteMessageTableStyle.aspx?PublishmentSystemID={0}&RelatedIdentity={1}", base.PublishmentSystemID, this.relatedIdentity));
                this.btnAddStyle.Attributes.Add("onclick", Modal.TableStyleAdd.GetOpenWindowString(base.PublishmentSystemID, 0, this.relatedIdentities, this.tableName, string.Empty, this.tableStyle, redirectUrl));

                redirectUrl = PageUtils.GetCMSUrl(string.Format("background_websiteMessageTableStyle.aspx?PublishmentSystemID={0}&RelatedIdentity={1}", base.PublishmentSystemID, this.relatedIdentity));
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

                string redirectUrl = PageUtils.GetCMSUrl(string.Format("background_tableStyle.aspx?PublishmentSystemID={0}&TableStyle={1}&RelatedIdentity={2}&tableName={3}", base.PublishmentSystemID, ETableStyleUtils.GetValue(this.tableStyle), this.relatedIdentity, this.tableName));
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
