using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;



namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundGatherDatabaseRuleAdd : BackgroundBasePage
	{
        public Literal ltlPageTitle;
		public PlaceHolder GatherRuleBase;
		public TextBox GatherRuleName;
		public DropDownList NodeIDDropDownList;
		public TextBox GatherNum;
		public RadioButtonList IsChecked;
        public RadioButtonList IsAutoCreate;

        public PlaceHolder GatherDatabaseLogin;
        public DropDownList DatabaseType;
        public TextBox DatabaseServer;
        public Control DatabaseServerRow;
        public TextBox DatabaseFilePath;
        public Control DatabaseFilePathRow;
        public TextBox UserName;
        public TextBox Password;
        public HtmlInputHidden PasswordHidden;
        public HtmlInputHidden DatabaseNameHidden;
        public HtmlInputHidden RelatedTableNameHidden;
        public HtmlInputHidden RelatedIdentityHidden;
        public HtmlInputHidden RelatedOrderByHidden;

        public PlaceHolder GatherRelatedTable;
        public DropDownList DatabaseName;
        public Control DatabaseNameRow;
        public DropDownList RelatedTableName;
        public DropDownList RelatedIdentity;
        public DropDownList RelatedOrderBy;
        public RadioButtonList IsOrderByDesc;
        public TextBox WhereString;
		
        public PlaceHolder GatherTableMatch;
        public Literal TableName;
        public Literal TableNameToMatch;
        public ListBox Columns;
        public ListBox ColumnsToMatch;

		public PlaceHolder Done;

		public PlaceHolder OperatingError;
		public Label ErrorLabel;

		public Button Previous;
		public Button Next;

		private bool isEdit = false;
		private string theGatherRuleName;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (base.GetQueryString("GatherRuleName") != null)
			{
				this.isEdit = true;
                this.theGatherRuleName = base.GetQueryString("GatherRuleName");
			}

			if (!Page.IsPostBack)
			{
                string pageTitle = this.isEdit ? "编辑数据库采集规则" : "添加数据库采集规则";
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Gather, pageTitle, AppManager.CMS.Permission.WebSite.Gather);

                this.ltlPageTitle.Text = pageTitle;

                NodeManager.AddListItemsForAddContent(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, true);
                this.DatabaseType.Items.Add(new ListItem(EDatabaseTypeUtils.GetText(EDatabaseType.SqlServer), EDatabaseTypeUtils.GetValue(EDatabaseType.SqlServer)));

				SetActivePanel(WizardPanel.GatherRuleBase, GatherRuleBase);

				if (this.isEdit)
				{
                    GatherDatabaseRuleInfo gatherDatabaseRuleInfo = DataProvider.GatherDatabaseRuleDAO.GetGatherDatabaseRuleInfo(this.theGatherRuleName, base.PublishmentSystemID);
					GatherRuleName.Text = gatherDatabaseRuleInfo.GatherRuleName;
                    this.WhereString.Text = gatherDatabaseRuleInfo.WhereString;
                    foreach (ListItem item in NodeIDDropDownList.Items)
                    {
                        if (item.Value.Equals(gatherDatabaseRuleInfo.NodeID.ToString()))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
					GatherNum.Text = gatherDatabaseRuleInfo.GatherNum.ToString();
					foreach (ListItem item in IsChecked.Items)
					{
                        if (item.Value.Equals(gatherDatabaseRuleInfo.IsChecked.ToString()))
						{
							item.Selected = true;
						}
						else
						{
							item.Selected = false;
						}
					}
                    foreach (ListItem item in IsAutoCreate.Items)
                    {
                        if (item.Value.Equals(gatherDatabaseRuleInfo.IsAutoCreate.ToString()))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    ControlUtils.SelectListItemsIgnoreCase(this.DatabaseType, EDatabaseTypeUtils.GetValue(gatherDatabaseRuleInfo.DatabaseType));

                    this.DatabaseServer.Text = SqlUtils.GetValueFromConnectionString(gatherDatabaseRuleInfo.ConnectionString, "server");
                    this.UserName.Text = SqlUtils.GetValueFromConnectionString(gatherDatabaseRuleInfo.ConnectionString, "uid");
                    this.Password.Text = SqlUtils.GetValueFromConnectionString(gatherDatabaseRuleInfo.ConnectionString, "pwd");
                    this.DatabaseNameHidden.Value = SqlUtils.GetValueFromConnectionString(gatherDatabaseRuleInfo.ConnectionString, "database");

                    this.RelatedTableNameHidden.Value = gatherDatabaseRuleInfo.RelatedTableName;
                    this.RelatedIdentityHidden.Value = gatherDatabaseRuleInfo.RelatedIdentity;
                    this.RelatedOrderByHidden.Value = gatherDatabaseRuleInfo.RelatedOrderBy;
                    
					foreach (ListItem item in IsOrderByDesc.Items)
					{
                        if (item.Value.Equals(gatherDatabaseRuleInfo.IsOrderByDesc.ToString()))
						{
							item.Selected = true;
						}
						else
						{
							item.Selected = false;
						}
					}
				}

                this.DatabaseType_Changed(null, EventArgs.Empty);
			}			

			base.SuccessMessage(string.Empty);
		}

        public void DatabaseType_Changed(object sender, EventArgs e)
        {
            this.DatabaseServerRow.Visible = true;
            this.DatabaseFilePathRow.Visible = false;
            this.DatabaseNameRow.Visible = true;
        }

        public void DatabaseName_Changed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.DatabaseName.SelectedValue))
            {
                this.RelatedTableName.Items.Clear();
            }
            else
            {
                bool isSuccess = false;
                try
                {
                    using (SqlConnection connection = new SqlConnection(GetDatabaseConnectionString()))
                    {
                        connection.Open();
                        connection.Close();
                    }
                    isSuccess = true;
                }
                catch (SqlException se)
                {
                    string errorMessage;
                    switch (se.Number)
                    {
                        case 4060:	// login fails
                            errorMessage = "You can't login to that database. Please select another one<br />" + se.Message;
                            break;
                        default:
                            errorMessage = String.Format("Number:{0}:<br/>Message:{1}", se.Number, se.Message);
                            break;
                    }
                    base.FailMessage(errorMessage);
                    SetActivePanel(WizardPanel.GatherRelatedTable, GatherRelatedTable);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex.Message);
                    SetActivePanel(WizardPanel.GatherRelatedTable, GatherRelatedTable);
                }
                if (isSuccess)
                {
                    IDictionary dictionary = this.GetTableStructureDAO().GetTablesAndViewsDictionary(this.GetDatabaseConnectionString(), this.DatabaseName.SelectedValue);
                    this.RelatedTableName.Items.Clear();
                    ListItem item = new ListItem("请选择表或视图", string.Empty);
                    this.RelatedTableName.Items.Add(item);
                    foreach (string theTableName in dictionary.Keys)
                    {
                        item = new ListItem(theTableName, dictionary[theTableName].ToString());
                        if (StringUtils.EqualsIgnoreCase(theTableName, this.RelatedTableNameHidden.Value))
                        {
                            item.Selected = true;
                        }
                        this.RelatedTableName.Items.Add(item);
                    }
                    this.RelatedTable_Changed(null, EventArgs.Empty);
                }
            }
        }

        public void RelatedTable_Changed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.RelatedTableName.SelectedValue))
            {
                this.RelatedIdentity.Items.Clear();
                this.RelatedOrderBy.Items.Clear();
            }
            else
            {
                ArrayList tableColumnInfoArrayList = this.GetTableStructureDAO().GetTableColumnInfoArrayList(this.GetDatabaseConnectionString(), this.DatabaseName.SelectedValue, this.RelatedTableName.SelectedValue);
                this.RelatedIdentity.Items.Clear();
                this.RelatedOrderBy.Items.Clear();
                ListItem item = new ListItem("请选择主键字段名称", string.Empty);
                this.RelatedIdentity.Items.Add(item);
                item = new ListItem("请选择排序字段名称", string.Empty);
                this.RelatedOrderBy.Items.Add(item);
                foreach (TableColumnInfo tableColumnInfo in tableColumnInfoArrayList)
                {
                    item = new ListItem(string.Format("{0}({1} {2})", tableColumnInfo.ColumnName, EDataTypeUtils.GetValue(tableColumnInfo.DataType), tableColumnInfo.Length), tableColumnInfo.ColumnName);
                    if (StringUtils.EqualsIgnoreCase(tableColumnInfo.ColumnName, this.RelatedIdentityHidden.Value))
                    {
                        item.Selected = true;
                    }
                    this.RelatedIdentity.Items.Add(item);
                }

                foreach (TableColumnInfo tableColumnInfo in tableColumnInfoArrayList)
                {
                    item = new ListItem(string.Format("{0}({1} {2})", tableColumnInfo.ColumnName, EDataTypeUtils.GetValue(tableColumnInfo.DataType), tableColumnInfo.Length), tableColumnInfo.ColumnName);
                    if (StringUtils.EqualsIgnoreCase(tableColumnInfo.ColumnName, this.RelatedOrderByHidden.Value))
                    {
                        item.Selected = true;
                    }
                    this.RelatedOrderBy.Items.Add(item);
                }
            }
        }

        private string GetConnectionString()
        {
            string retval = string.Empty;
            if (EDatabaseTypeUtils.Equals(EDatabaseType.SqlServer, this.DatabaseType.SelectedValue))
            {
                retval = string.Format("server={0};uid={1};pwd={2}", this.DatabaseServer.Text, this.UserName.Text, this.PasswordHidden.Value);
            }
            return retval;
        }

        private string GetDatabaseConnectionString()
        {
            string retval = string.Empty;
            if (EDatabaseTypeUtils.Equals(EDatabaseType.SqlServer, this.DatabaseType.SelectedValue))
            {
                retval = string.Format("{0};database={1}", GetConnectionString(), this.DatabaseName.SelectedValue);
            }
            return retval;
        }

        private WizardPanel CurrentWizardPanel
		{
			get
			{
				if (ViewState["WizardPanel"] != null)
					return (WizardPanel)ViewState["WizardPanel"];

				return WizardPanel.GatherRuleBase;
			}
			set
			{
				ViewState["WizardPanel"] = value;
			}
		}


		private enum WizardPanel
		{
			GatherRuleBase,
            GatherDatabaseLogin,
            GatherRelatedTable,
            GatherTableMatch,
			OperatingError,
			Done
		}

		void SetActivePanel(WizardPanel panel, Control controlToShow)
		{
			PlaceHolder currentPanel = FindControl(CurrentWizardPanel.ToString()) as PlaceHolder;
			if (currentPanel != null)
				currentPanel.Visible = false;

			switch (panel)
			{
				case WizardPanel.GatherRuleBase:
                    Previous.CssClass = "btn disabled";
                    Previous.Enabled = false;
					break;
				case WizardPanel.Done:
                    Previous.CssClass = "btn disabled";
                    Previous.Enabled = false;
                    Next.CssClass = "btn btn-primary disabled";
                    Next.Enabled = false;
                    base.AddWaitAndRedirectScript(PageUtils.GetCMSUrl(string.Format("background_gatherDatabaseRule.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
					break;
				case WizardPanel.OperatingError:
                    Previous.CssClass = "btn disabled";
                    Previous.Enabled = false;
                    Next.CssClass = "btn btn-primary disabled";
                    Next.Enabled = false;
					break;
				default:
                    Previous.CssClass = "btn";
                    Previous.Enabled = true;
                    Next.CssClass = "btn btn-primary";
                    Next.Enabled = true;
					break;
			}

			controlToShow.Visible = true;
			CurrentWizardPanel = panel;
		}

        private bool Validate_GatherRuleBase(out string errorMessage)
		{
			if (string.IsNullOrEmpty(this.GatherRuleName.Text))
			{
				errorMessage = "必须填写采集规则名称！";
				return false;
			}

			if (this.isEdit == false)
			{
				ArrayList gatherRuleNameList = DataProvider.GatherRuleDAO.GetGatherRuleNameArrayList(base.PublishmentSystemID);
				if (gatherRuleNameList.IndexOf(this.GatherRuleName.Text) != -1)
				{
					errorMessage = "采集规则名称已存在！";
					return false;
				}
			}

			errorMessage = string.Empty;
			return true;
		}

        private bool Validate_GatherDatabaseLogin(out string errorMessage)
        {
            if (EDatabaseTypeUtils.Equals(EDatabaseType.SqlServer, this.DatabaseType.SelectedValue))
            {
                if (string.IsNullOrEmpty(this.DatabaseServer.Text))
                {
                    errorMessage = "必须填写IP地址或者服务器名！";
                    return false;
                }
                else if (string.IsNullOrEmpty(this.UserName.Text))
                {
                    errorMessage = "必须填写登录账号！";
                    return false;
                }

                try
                {
                    SqlConnection connection = new SqlConnection(GetConnectionString());
                    connection.Open();
                    connection.Close();
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                    return false;
                }

                ArrayList databaseNameArrayList = this.GetTableStructureDAO().GetDatabaseNameArrayList(this.GetConnectionString());
                this.DatabaseName.Items.Clear();
                ListItem item = new ListItem("请选择数据库", string.Empty);
                this.DatabaseName.Items.Add(item);
                foreach (string theDatabaseName in databaseNameArrayList)
                {
                    item = new ListItem(theDatabaseName, theDatabaseName);
                    if (StringUtils.EqualsIgnoreCase(theDatabaseName, this.DatabaseNameHidden.Value))
                    {
                        item.Selected = true;
                    }
                    this.DatabaseName.Items.Add(item);
                }
                this.DatabaseName_Changed(null, EventArgs.Empty);
            }

            errorMessage = string.Empty;
            return true;
        }

        private bool Validate_GatherRelatedTable(out string errorMessage)
        {
            if (string.IsNullOrEmpty(this.RelatedTableName.SelectedValue))
            {
                errorMessage = "必须选择采集表名称！";
                return false;
            }
            else if (string.IsNullOrEmpty(this.RelatedIdentity.SelectedValue))
            {
                errorMessage = "必须选择主键字段名称！";
                return false;
            }
            else if (string.IsNullOrEmpty(this.RelatedOrderBy.SelectedValue))
            {
                errorMessage = "必须选择排序字段名称！";
                return false;
            }

            this.TableName.Text = this.RelatedTableName.SelectedItem.Text;
            int nodeID = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);
            this.TableNameToMatch.Text = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
            NameValueCollection columnsMap = new NameValueCollection();
            if (isEdit)
            {
                int tableMatchID = DataProvider.GatherDatabaseRuleDAO.GetTableMatchID(this.GatherRuleName.Text, base.PublishmentSystemID);
                if (tableMatchID != 0)
                {
                    TableMatchInfo tableMatchInfo = BaiRongDataProvider.TableMatchDAO.GetTableMatchInfo(tableMatchID);
                    if (tableMatchInfo != null)
                    {
                        columnsMap = tableMatchInfo.ColumnsMap;
                    }
                }
            }
            this.SetColumns(columnsMap);

            errorMessage = string.Empty;
            return true;
        }

        private static bool Validate_GatherTableMatch(out string errorMessage)
        {
            NameValueCollection columnsMap = GetColumnsMap();
            if (columnsMap == null || columnsMap.Count == 0)
            {
                errorMessage = "必须匹配表字段！";
                return false;
            }
            else
            {
                bool hasTitle = false;
                foreach (string key in columnsMap.Keys)
                {
                    string columnsToMatch = columnsMap[key];
                    if (StringUtils.EqualsIgnoreCase(ContentAttribute.Title, columnsToMatch))
                    {
                        hasTitle = true;
                        break;
                    }
                }
                if (hasTitle == false)
                {
                    errorMessage = "必须匹配标题字段！";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        private bool Validate_InsertGatherDatabaseRule(out string errorMessage)
		{
			try
			{
                bool isNeedAdd = false;
				if (this.isEdit)
				{
                    if (this.theGatherRuleName != this.GatherRuleName.Text)
                    {
                        isNeedAdd = true;
                        DataProvider.GatherDatabaseRuleDAO.Delete(this.theGatherRuleName, base.PublishmentSystemID);
                    }
                    else
                    {
                        GatherDatabaseRuleInfo gatherDatabaseRuleInfo =
                            DataProvider.GatherDatabaseRuleDAO.GetGatherDatabaseRuleInfo(this.theGatherRuleName,
                                                                                         base.PublishmentSystemID);

                        gatherDatabaseRuleInfo.DatabaseType =
                            EDatabaseTypeUtils.GetEnumType(this.DatabaseType.SelectedValue);
                        gatherDatabaseRuleInfo.ConnectionString = this.GetDatabaseConnectionString();
                        gatherDatabaseRuleInfo.RelatedTableName = this.TableName.Text;
                        gatherDatabaseRuleInfo.RelatedIdentity = this.RelatedIdentity.SelectedValue;
                        gatherDatabaseRuleInfo.RelatedOrderBy = this.RelatedOrderBy.SelectedValue;
                        gatherDatabaseRuleInfo.WhereString = this.WhereString.Text;
                        if (NodeIDDropDownList.SelectedValue != null)
                        {
                            gatherDatabaseRuleInfo.NodeID = int.Parse(NodeIDDropDownList.SelectedValue);
                        }
                        gatherDatabaseRuleInfo.GatherNum = int.Parse(GatherNum.Text);
                        gatherDatabaseRuleInfo.IsChecked = TranslateUtils.ToBool(IsChecked.SelectedValue);
                        gatherDatabaseRuleInfo.IsAutoCreate = TranslateUtils.ToBool(IsAutoCreate.SelectedValue);
                        gatherDatabaseRuleInfo.IsOrderByDesc = TranslateUtils.ToBool(IsOrderByDesc.SelectedValue);
                        gatherDatabaseRuleInfo.LastGatherDate = DateUtils.SqlMinValue;

                        TableMatchInfo tableMatchInfo =
                            BaiRongDataProvider.TableMatchDAO.GetTableMatchInfo(gatherDatabaseRuleInfo.TableMatchID);
                        if (tableMatchInfo == null)
                        {
                            tableMatchInfo =
                                new TableMatchInfo(0, gatherDatabaseRuleInfo.ConnectionString, this.TableName.Text,
                                                   BaiRongDataProvider.ConnectionString, this.TableNameToMatch.Text,
                                                   GetColumnsMap());
                            gatherDatabaseRuleInfo.TableMatchID =
                                BaiRongDataProvider.TableMatchDAO.Insert(tableMatchInfo);
                        }
                        else
                        {
                            tableMatchInfo.ConnectionString = gatherDatabaseRuleInfo.ConnectionString;
                            tableMatchInfo.TableName = this.TableName.Text;
                            tableMatchInfo.ConnectionStringToMatch = BaiRongDataProvider.ConnectionString;
                            tableMatchInfo.TableNameToMatch = this.TableNameToMatch.Text;
                            tableMatchInfo.ColumnsMap = GetColumnsMap();
                            BaiRongDataProvider.TableMatchDAO.Update(tableMatchInfo);
                        }

                        DataProvider.GatherDatabaseRuleDAO.Update(gatherDatabaseRuleInfo);
                    }
				}
				else
				{
				    isNeedAdd = true;
				}

                if (isNeedAdd)
                {
                    GatherDatabaseRuleInfo gatherDatabaseRuleInfo = new GatherDatabaseRuleInfo();
                    gatherDatabaseRuleInfo.GatherRuleName = GatherRuleName.Text;
                    gatherDatabaseRuleInfo.PublishmentSystemID = base.PublishmentSystemID;
                    gatherDatabaseRuleInfo.DatabaseType = EDatabaseTypeUtils.GetEnumType(this.DatabaseType.SelectedValue);
                    gatherDatabaseRuleInfo.ConnectionString = this.GetDatabaseConnectionString();
                    gatherDatabaseRuleInfo.RelatedTableName = this.TableName.Text;
                    gatherDatabaseRuleInfo.RelatedIdentity = this.RelatedIdentity.SelectedValue;
                    gatherDatabaseRuleInfo.RelatedOrderBy = this.RelatedOrderBy.SelectedValue;
                    gatherDatabaseRuleInfo.WhereString = this.WhereString.Text;
                    if (NodeIDDropDownList.SelectedValue != null)
                    {
                        gatherDatabaseRuleInfo.NodeID = int.Parse(NodeIDDropDownList.SelectedValue);
                    }
                    gatherDatabaseRuleInfo.GatherNum = int.Parse(GatherNum.Text);
                    gatherDatabaseRuleInfo.IsChecked = TranslateUtils.ToBool(IsChecked.SelectedValue);
                    gatherDatabaseRuleInfo.IsAutoCreate = TranslateUtils.ToBool(IsAutoCreate.SelectedValue);
                    gatherDatabaseRuleInfo.IsOrderByDesc = TranslateUtils.ToBool(IsOrderByDesc.SelectedValue);
                    gatherDatabaseRuleInfo.LastGatherDate = DateUtils.SqlMinValue;

                    TableMatchInfo tableMatchInfo = new TableMatchInfo(0, gatherDatabaseRuleInfo.ConnectionString, this.TableName.Text, BaiRongDataProvider.ConnectionString, this.TableNameToMatch.Text, GetColumnsMap());
                    gatherDatabaseRuleInfo.TableMatchID = BaiRongDataProvider.TableMatchDAO.Insert(tableMatchInfo);

                    DataProvider.GatherDatabaseRuleDAO.Insert(gatherDatabaseRuleInfo);
                }

                if (isNeedAdd)
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "添加数据库采集规则", string.Format("采集规则:{0}", GatherRuleName.Text));
                }
                else
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "编辑数据库采集规则", string.Format("采集规则:{0}", GatherRuleName.Text));
                }

                ClearColumnsMap();
				errorMessage = string.Empty;
				return true;
			}
			catch
			{
                ClearColumnsMap();
				errorMessage = "操作失败！";
				return false;
			}
		}

        private static void SaveColumnsMap(NameValueCollection columnsMap)
        {
            DbCacheManager.Insert("SiteServer.CMS.BackgroundPages.BackgroundGatherDatabaseRuleAdd.TableMatchColumnsMap", TranslateUtils.NameValueCollectionToString(columnsMap));
        }

        private static NameValueCollection GetColumnsMap()
        {
            NameValueCollection columnsMap = TranslateUtils.ToNameValueCollection(DbCacheManager.Get("SiteServer.CMS.BackgroundPages.BackgroundGatherDatabaseRuleAdd.TableMatchColumnsMap"));
            return columnsMap;
        }

        private static void ClearColumnsMap()
        {
            DbCacheManager.Remove("SiteServer.CMS.BackgroundPages.BackgroundGatherDatabaseRuleAdd.TableMatchColumnsMap");
        }

        private ITableStructureDAO GetTableStructureDAO()
        {
            return BaiRongDataProvider.CreateTableStructureDAO(EDatabaseTypeUtils.GetEnumType(this.DatabaseType.SelectedValue));
        }

        private void SetColumns(NameValueCollection columnsMap)
        {
            this.Columns.Items.Clear();
            this.ColumnsToMatch.Items.Clear();

            ArrayList tableColumnInfoArrayList = this.GetTableStructureDAO().GetTableColumnInfoArrayList(this.GetDatabaseConnectionString(), this.DatabaseName.SelectedValue, this.RelatedTableName.SelectedValue);
            ArrayList columnToMatchArrayList = new ArrayList();
            foreach (TableColumnInfo tableColumnInfo in tableColumnInfoArrayList)
            {
                string text = string.Format("{0}({1} {2})", tableColumnInfo.ColumnName, EDataTypeUtils.GetValue(tableColumnInfo.DataType), tableColumnInfo.Length);
                string value = tableColumnInfo.ColumnName.ToLower();
                string columnToMatch = columnsMap[value];
                if (!string.IsNullOrEmpty(columnToMatch))
                {
                    TableMetadataInfo tableMetadataInfoToMatch = TableManager.GetTableMetadataInfo(this.TableNameToMatch.Text, columnToMatch);
                    if (tableMetadataInfoToMatch != null)
                    {
                        columnToMatchArrayList.Add(columnToMatch);
                        text += " -> " + string.Format("{0}({1} {2})", tableMetadataInfoToMatch.AttributeName, EDataTypeUtils.GetValue(tableMetadataInfoToMatch.DataType), tableMetadataInfoToMatch.DataLength);
                        value += "&" + columnToMatch;
                    }
                }
                this.Columns.Items.Add(new ListItem(text, value));
            }

            ArrayList tableMetadataInfoArrayList = TableManager.GetTableMetadataInfoArrayList(this.TableNameToMatch.Text);
            foreach (TableMetadataInfo tableMetadataInfo in tableMetadataInfoArrayList)
            {
                string value = tableMetadataInfo.AttributeName.ToLower();
                if (!columnToMatchArrayList.Contains(tableMetadataInfo.AttributeName))
                {
                    string text = string.Format("{0}({1} {2})", tableMetadataInfo.AttributeName, EDataTypeUtils.GetValue(tableMetadataInfo.DataType), tableMetadataInfo.DataLength);
                    this.ColumnsToMatch.Items.Add(new ListItem(text, value));
                }
            }

            SaveColumnsMap(columnsMap);
        }

        protected void Add_OnClick(object sender, System.EventArgs e)
        {
            NameValueCollection columnsMap = GetColumnsMap();

            if (!string.IsNullOrEmpty(this.Columns.SelectedValue) && !string.IsNullOrEmpty(this.ColumnsToMatch.SelectedValue))
            {
                if (this.Columns.SelectedValue.IndexOf("&") != -1)
                {
                    columnsMap[this.Columns.SelectedValue.Split('&')[0].ToLower()] = this.ColumnsToMatch.SelectedValue.ToLower();
                }
                else
                {
                    columnsMap[this.Columns.SelectedValue.ToLower()] = this.ColumnsToMatch.SelectedValue.ToLower();
                }
                this.SetColumns(columnsMap);
            }
        }

        protected void Delete_OnClick(object sender, System.EventArgs e)
        {
            NameValueCollection columnsMap = GetColumnsMap();

            if (!string.IsNullOrEmpty(this.Columns.SelectedValue))
            {
                if (this.Columns.SelectedValue.IndexOf("&") != -1)
                {
                    columnsMap.Remove(this.Columns.SelectedValue.Split('&')[0]);
                    this.SetColumns(columnsMap);
                }
            }
        }

		public void NextPanel(Object sender, EventArgs e)
		{
			string errorMessage;
			switch (CurrentWizardPanel)
			{
				case WizardPanel.GatherRuleBase:

					if (this.Validate_GatherRuleBase(out errorMessage))
					{
                        SetActivePanel(WizardPanel.GatherDatabaseLogin, GatherDatabaseLogin);
					}
					else
					{
                        base.FailMessage(errorMessage);
						SetActivePanel(WizardPanel.GatherRuleBase, GatherRuleBase);
					}

					break;

                case WizardPanel.GatherDatabaseLogin:

                    this.PasswordHidden.Value = this.Password.Text;
                    if (this.Validate_GatherDatabaseLogin(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.GatherRelatedTable, GatherRelatedTable);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.GatherDatabaseLogin, GatherDatabaseLogin);
                    }

					break;

                case WizardPanel.GatherRelatedTable:

                    if (this.Validate_GatherRelatedTable(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.GatherTableMatch, GatherTableMatch);
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.GatherRelatedTable, GatherRelatedTable);
                    }

                    break;

                case WizardPanel.GatherTableMatch:

                    if (Validate_GatherTableMatch(out errorMessage))
                    {
                        if (this.Validate_InsertGatherDatabaseRule(out errorMessage))
                        {
                            SetActivePanel(WizardPanel.Done, Done);
                        }
                        else
                        {
                            ErrorLabel.Text = errorMessage;
                            SetActivePanel(WizardPanel.OperatingError, OperatingError);
                        }
                    }
                    else
                    {
                        base.FailMessage(errorMessage);
                        SetActivePanel(WizardPanel.GatherTableMatch, GatherTableMatch);
                    }

					break;

				case WizardPanel.Done:
					break;
			}
		}

		public void PreviousPanel(Object sender, EventArgs e)
		{
			switch (CurrentWizardPanel)
			{
				case WizardPanel.GatherRuleBase:
					break;

                case WizardPanel.GatherDatabaseLogin:
					SetActivePanel(WizardPanel.GatherRuleBase, GatherRuleBase);
					break;

                case WizardPanel.GatherRelatedTable:
                    SetActivePanel(WizardPanel.GatherDatabaseLogin, GatherDatabaseLogin);
                    break;

				case WizardPanel.GatherTableMatch:
                    SetActivePanel(WizardPanel.GatherRelatedTable, GatherRelatedTable);
					break;
			}
		}
	}
}
