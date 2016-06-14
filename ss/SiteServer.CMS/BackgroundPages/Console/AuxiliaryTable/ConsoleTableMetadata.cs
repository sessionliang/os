using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;



namespace SiteServer.CMS.BackgroundPages
{
    public class ConsoleTableMetadata : BackgroundBasePage
    {
        public DataGrid dgContents;
        public Control SyncTable;
        public Literal ltlSqlString;
        private bool showSQLTable = true;
        private string tableName;
        private EAuxiliaryTableType tableType;
        private bool tableIsRealCreated = false;
        private int usedNum = 0;
        private string redirectUrl;

        public Button AddColumnButton;
        public Button BatchAddColumnButton;

        protected override bool IsSaasForbidden { get { return true; } }

        public static string GetRedirectUrl(string tableName, EAuxiliaryTableType tableType, int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("console_tableMetadata.aspx?ENName={0}&TableType={1}&PublishmentSystemID={2}", tableName, EAuxiliaryTableTypeUtils.GetValue(tableType), publishmentSystemID));
        }

        public string GetReturnUrl()
        {
            if (base.PublishmentSystemID == 0)
            {
                return PageUtils.GetCMSUrl("console_auxiliaryTable.aspx");
            }
            else
            {
                return PageUtils.GetCMSUrl(string.Format("background_contentModel.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
            }
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("ENName", "TableType");
            if (base.GetQueryString("ShowCrateDBCommand") != null)
            {
                this.showSQLTable = true;
            }
            else
            {
                this.showSQLTable = false;
            }

            this.tableName = base.GetQueryString("ENName").Trim();
            this.tableType = EAuxiliaryTableTypeUtils.GetEnumType(base.GetQueryString("TableType"));
            this.redirectUrl = ConsoleTableMetadata.GetRedirectUrl(this.tableName, this.tableType, base.PublishmentSystemID);

            AuxiliaryTableInfo tableInfo = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableInfo(this.tableName);

            if (base.GetQueryString("Delete") != null)
            {
                int tableMetadataID = TranslateUtils.ToInt(base.GetQueryString("TableMetadataID"));

                try
                {
                    TableMetadataInfo tableMetadataInfo = BaiRongDataProvider.TableMetadataDAO.GetTableMetadataInfo(tableMetadataID);
                    BaiRongDataProvider.TableMetadataDAO.Delete(tableMetadataID);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除辅助表字段", string.Format("辅助表:{0},字段名:{1}", this.tableName, tableMetadataInfo.AttributeName));

                    base.SuccessDeleteMessage();
                    PageUtils.Redirect(this.redirectUrl);
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
            else if (base.GetQueryString("DeleteStyle") != null)//删除样式
            {
                string attributeName = base.GetQueryString("AttributeName");
                if (TableStyleManager.IsExists(0, tableName, attributeName))
                {
                    try
                    {
                        TableStyleManager.Delete(0, tableName, attributeName);

                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除辅助表字段样式", string.Format("辅助表:{0},字段名:{1}", this.tableName, attributeName));

                        base.SuccessDeleteMessage();
                        PageUtils.Redirect(this.redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailDeleteMessage(ex);
                    }
                }
            }
            else if (base.GetQueryString("CreateDB") != null)
            {
                try
                {
                    BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTable(this.tableName);
                    tableInfo.IsChangedAfterCreatedInDB = false;

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "创建辅助表", string.Format("辅助表:{0}", this.tableName));

                    base.SuccessMessage("辅助表创建成功！");
                    PageUtils.Redirect(this.redirectUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "<br>辅助表创建失败，失败原因为：" + ex.Message + "<br>请检查创建表SQL命令");
                    string sqlString = BaiRongDataProvider.AuxiliaryTableDataDAO.GetCreateAuxiliaryTableSqlString(this.tableName);
                    this.ltlSqlString.Text = sqlString.Replace("\r\n", "<br>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
                    this.showSQLTable = true;
                }
            }
            else if (base.GetQueryString("DeleteDB") != null)
            {
                try
                {
                    BaiRongDataProvider.TableMetadataDAO.DeleteAuxiliaryTable(this.tableName);
                    tableInfo.IsChangedAfterCreatedInDB = false;

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除辅助表", string.Format("辅助表:{0}", this.tableName));

                    base.SuccessMessage("辅助表删除成功！");
                    PageUtils.Redirect(this.redirectUrl);
                }
                catch (Exception ex)
                {

                    base.FailMessage(ex, "<br>辅助表删除失败，失败原因为：" + ex.Message + "<br>");
                }
            }
            else if (base.GetQueryString("ReCreateDB") != null)
            {
                try
                {
                    BaiRongDataProvider.TableMetadataDAO.ReCreateAuxiliaryTable(this.tableName, tableInfo.AuxiliaryTableType);
                    DataProvider.NodeDAO.UpdateContentNumToZero(this.tableName, tableInfo.AuxiliaryTableType);
                    tableInfo.IsChangedAfterCreatedInDB = false;

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "重建辅助表", string.Format("辅助表:{0}", this.tableName));

                    base.SuccessMessage("辅助表重建成功！");
                    PageUtils.Redirect(this.redirectUrl);
                }
                catch (Exception ex)
                {

                    base.FailMessage(ex, "<br>辅助表重建失败，失败原因为：" + ex.Message + "<br>请检查创建表SQL命令");
                    string sqlString = BaiRongDataProvider.AuxiliaryTableDataDAO.GetCreateAuxiliaryTableSqlString(this.tableName);
                    this.ltlSqlString.Text = sqlString.Replace("\r\n", "<br>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
                    this.showSQLTable = true;
                }
            }
            else if (base.GetQueryString("ShowCrateDBCommand") != null)
            {
                string sqlString = BaiRongDataProvider.AuxiliaryTableDataDAO.GetCreateAuxiliaryTableSqlString(this.tableName);
                this.ltlSqlString.Text = sqlString.Replace("\r\n", "<br>").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
            }
            else if (base.GetQueryString("SetTaxis") != null)
            {
                SetTaxis();
            }

            this.tableIsRealCreated = BaiRongDataProvider.TableStructureDAO.IsTableExists(this.tableName);

            this.usedNum = BaiRongDataProvider.TableCollectionDAO.GetTableUsedNum(this.tableName, tableInfo.AuxiliaryTableType);

            this.SyncTable.Visible = this.IsNeedSync(this.tableIsRealCreated, tableInfo.IsChangedAfterCreatedInDB);

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Auxiliary, string.Format("辅助表字段管理（{0}）", this.tableName), AppManager.Platform.Permission.Platform_Auxiliary);

                this.dgContents.DataSource = BaiRongDataProvider.TableMetadataDAO.GetDataSource(this.tableName);
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddColumnButton.Attributes.Add("onclick", Modal.TableMetadataAdd.GetOpenWindowStringToAdd(this.tableName, this.tableType));

                this.BatchAddColumnButton.Attributes.Add("onclick", Modal.TableMetadataAddBatch.GetOpenWindowStringToAdd(this.tableName, this.tableType));
            }
        }

        private void SetTaxis()
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

        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int tableMetadataID = TranslateUtils.EvalInt(e.Item.DataItem, "TableMetadataID");
                string auxiliaryTableENName = TranslateUtils.EvalString(e.Item.DataItem, "AuxiliaryTableENName");
                string attributeName = TranslateUtils.EvalString(e.Item.DataItem, "AttributeName");
                string dataType = TranslateUtils.EvalString(e.Item.DataItem, "DataType");
                int dataLength = TranslateUtils.EvalInt(e.Item.DataItem, "DataLength");
                string isSystem = TranslateUtils.EvalString(e.Item.DataItem, "IsSystem");

                Literal ltlAttributeName = e.Item.FindControl("ltlAttributeName") as Literal;
                Literal ltlDisplayName = e.Item.FindControl("ltlDisplayName") as Literal;
                Literal ltlIsVisible = e.Item.FindControl("ltlIsVisible") as Literal;
                Literal ltlValidate = e.Item.FindControl("ltlValidate") as Literal;
                Literal ltlDataType = e.Item.FindControl("ltlDataType") as Literal;
                Literal ltlInputType = e.Item.FindControl("ltlInputType") as Literal;
                HyperLink upLinkButton = e.Item.FindControl("UpLinkButton") as HyperLink;
                HyperLink downLinkButton = e.Item.FindControl("DownLinkButton") as HyperLink;
                Literal ltlStyle = e.Item.FindControl("ltlStyle") as Literal;
                Literal ltlEditValidate = e.Item.FindControl("ltlEditValidate") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ArrayList relatedIdentities = new ArrayList();
                relatedIdentities.Add(0);
                string showPopWinString = Modal.TableMetadataView.GetOpenWindowString(this.tableType, this.tableName, attributeName, relatedIdentities);
                ltlAttributeName.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, attributeName);

                TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(EAuxiliaryTableTypeUtils.GetTableStyle(this.tableType), this.tableName, attributeName, null);
                ltlDisplayName.Text = styleInfo.DisplayName;

                ltlIsVisible.Text = StringUtils.GetTrueOrFalseImageHtml(styleInfo.IsVisible.ToString());
                ltlValidate.Text = EInputValidateTypeUtils.GetValidateInfo(styleInfo);

                ltlDataType.Text = EDataTypeUtils.GetTextByAuxiliaryTable(EDataTypeUtils.GetEnumType(dataType), dataLength);
                ltlInputType.Text = EInputTypeUtils.GetText(styleInfo.InputType);

                if (this.IsSystem(isSystem))
                {
                    if (upLinkButton != null)
                    {
                        upLinkButton.NavigateUrl = PageUtils.GetCMSUrl(string.Format("console_tableMetadata.aspx?PublishmentSystemID={0}&SetTaxis=True&TableStyleID={1}&Direction=UP&TableMetadataId={2}&ENName={3}&TableType={4}", base.PublishmentSystemID, styleInfo.TableStyleID, tableMetadataID, this.tableName, this.tableType));
                    }
                    if (downLinkButton != null)
                    {
                        downLinkButton.NavigateUrl = PageUtils.GetCMSUrl(string.Format("console_tableMetadata.aspx?PublishmentSystemID={0}&SetTaxis=True&TableStyleID={1}&Direction=DOWN&TableMetadataId={2}&ENName={3}&TableType={4}", base.PublishmentSystemID, styleInfo.TableStyleID, tableMetadataID, this.tableName,this.tableType));
                    }
                }

                ltlStyle.Text = GetEditStyleHtml(tableMetadataID, attributeName);

                showPopWinString = Modal.TableStyleValidateAdd.GetOpenWindowString(styleInfo.TableStyleID, relatedIdentities, this.tableName, styleInfo.AttributeName, EAuxiliaryTableTypeUtils.GetTableStyle(this.tableType), redirectUrl);
                ltlEditValidate.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">设置</a>", showPopWinString);

                ltlEditUrl.Text = GetEditHtml(isSystem, tableMetadataID);

                if (!this.IsSystem(isSystem))
                {
                    NameValueCollection attributes = new NameValueCollection();
                    attributes.Add("Delete", true.ToString());
                    attributes.Add("TableMetadataID", tableMetadataID.ToString());
                    string deleteUrl = PageUtils.AddQueryString(this.redirectUrl, attributes);
                    ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除辅助字段“{1}”，确认吗？');"">删除字段</a>", deleteUrl, attributeName);
                }
            }
        }

        public string GetShowCommandElementStyle()
        {
            if (this.tableName != null)
            {
                return Constants.SHOW_ELEMENT_STYLE;
            }
            return Constants.HIDE_ELEMENT_STYLE;
        }

        public string GetCreateDBCommandElementStyle()
        {
            if (this.tableName != null)
            {
                if (!tableIsRealCreated)
                {
                    return Constants.SHOW_ELEMENT_STYLE;
                }
            }
            return Constants.HIDE_ELEMENT_STYLE;
        }

        public string GetDeleteDBCommandElementStyle()
        {
            if (this.tableName != null)
            {
                if (this.tableIsRealCreated && this.usedNum == 0)
                {
                    return Constants.SHOW_ELEMENT_STYLE;
                }
            }
            return Constants.HIDE_ELEMENT_STYLE;
        }

        public string GetReCreateDBCommandElementStyle()
        {
            if (this.tableName != null)
            {
                if (tableIsRealCreated)
                {
                    return Constants.SHOW_ELEMENT_STYLE;
                }
            }
            return Constants.HIDE_ELEMENT_STYLE;
        }

        public string GetSQLTableStyle()
        {
            if (showSQLTable)
            {
                return Constants.SHOW_ELEMENT_STYLE;
            }
            return Constants.HIDE_ELEMENT_STYLE;
        }

        public bool IsSystem(string isSystem)
        {
            return TranslateUtils.ToBool(isSystem);
        }

        public string GetEditHtml(string isSystem, int tableMetadataID)
        {
            string retval = string.Empty;

            if (!this.IsSystem(isSystem))
            {
                retval = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">修改字段</a>", Modal.TableMetadataAdd.GetOpenWindowStringToEdit(this.tableName, this.tableType, tableMetadataID));
            }
            return retval;
        }

        public string GetEditStyleHtml(int tableMetadataID, string attributeName)
        {
            string retval = string.Empty;
            ETableStyle tableStyle = EAuxiliaryTableTypeUtils.GetTableStyle(this.tableType);
            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(tableStyle, this.tableName, attributeName, null);
            ArrayList relatedIdentities = new ArrayList();
            relatedIdentities.Add(0);
            string showPopWinString = Modal.TableStyleAdd.GetOpenWindowString(0, 0, relatedIdentities, this.tableName, attributeName, tableStyle, redirectUrl);

            string editText = "设置";
            if (styleInfo.TableStyleID != 0)//数据库中有样式
            {
                editText = "修改";
            }
            retval = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, editText);

            if (styleInfo.TableStyleID != 0)//数据库中有样式
            {
                NameValueCollection attributes = new NameValueCollection();
                attributes.Add("DeleteStyle", true.ToString());
                attributes.Add("TableMetadataID", tableMetadataID.ToString());
                attributes.Add("AttributeName", attributeName);
                string deleteUrl = PageUtils.AddQueryString(this.redirectUrl, attributes);

                retval += string.Format(@"&nbsp;&nbsp;<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除对应显示样式，确认吗？');"">删除</a>", deleteUrl);
            }
            return retval;
        }

        public bool IsNeedSync(bool isCreatedInDB, bool isChangedAfterCreatedInDB)
        {
            if (isCreatedInDB == false)
            {
                return false;
            }
            else
            {
                if (isChangedAfterCreatedInDB == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void MyDataGrid_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            int TableMetadataID = (int)this.dgContents.DataKeys[e.Item.ItemIndex];
            string direction = e.CommandName;

            switch (direction.ToUpper())
            {
                case "UP":
                    BaiRongDataProvider.TableMetadataDAO.TaxisDown(TableMetadataID, this.tableName);
                    break;
                case "DOWN":
                    BaiRongDataProvider.TableMetadataDAO.TaxisUp(TableMetadataID, this.tableName);
                    break;
                default:
                    break;
            }
            base.SuccessMessage("排序成功！");
            PageUtils.Redirect(this.redirectUrl);
        }

        public void SyncTableButton_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    BaiRongDataProvider.TableMetadataDAO.SyncTable(this.tableName);
                    this.SyncTable.Visible = false;
                    base.SuccessMessage("同步辅助表成功！");
                    PageUtils.Redirect(this.redirectUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "<br>同步辅助表失败，失败原因为：" + ex.Message);
                }
            }
        }


    }
}
