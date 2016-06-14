using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using System.Collections.Specialized;


namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class TableMetadataAdd : BackgroundBasePage
	{
		public TextBox AttributeName;
		public DropDownList DataType;
		public TextBox DataLength;
		public RadioButtonList CanBeNull;
		public TextBox DBDefaultValue;

        private string tableName;
        private EAuxiliaryTableType tableType;

        protected override bool IsSaasForbidden { get { return true; } }

        public static string GetOpenWindowStringToAdd(string tableName, EAuxiliaryTableType tableType)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("TableName", tableName);
            arguments.Add("TableType", EAuxiliaryTableTypeUtils.GetValue(tableType));
            return PageUtility.GetOpenWindowString("��Ӹ������ֶ�", "modal_tableMetadataAdd.aspx", arguments, 400, 360);
        }

        public static string GetOpenWindowStringToEdit(string tableName, EAuxiliaryTableType tableType, int tableMetadataID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("TableName", tableName);
            arguments.Add("TableType", EAuxiliaryTableTypeUtils.GetValue(tableType));
            arguments.Add("TableMetadataID", tableMetadataID.ToString());
            return PageUtility.GetOpenWindowString("�޸ĸ������ֶ�", "modal_tableMetadataAdd.aspx", arguments, 400, 360);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("TableName", "TableType");

            this.tableName = base.GetQueryString("TableName");
            this.tableType = EAuxiliaryTableTypeUtils.GetEnumType(base.GetQueryString("TableType"));

            if (!IsPostBack)
            {
                CanBeNull.Items[0].Value = true.ToString();
                CanBeNull.Items[1].Value = false.ToString();

                EDataTypeUtils.AddListItemsToAuxiliaryTable(DataType, (BaiRongDataProvider.ADOType == SqlUtils.SQL_SERVER));

                if (base.GetQueryString("TableMetadataID") != null)
                {
                    int TableMetadataID = base.GetIntQueryString("TableMetadataID");
                    TableMetadataInfo info = BaiRongDataProvider.TableMetadataDAO.GetTableMetadataInfo(TableMetadataID);
                    if (info != null)
                    {
                        AttributeName.Text = info.AttributeName;
                        AttributeName.Enabled = false;
                        ControlUtils.SelectListItemsIgnoreCase(DataType, info.DataType.ToString());
                        DataLength.Text = info.DataLength.ToString();

                        foreach (ListItem listitem in CanBeNull.Items)
                        {
                            listitem.Selected = (EBooleanUtils.Equals(info.CanBeNull, listitem.Value)) ? true : false;
                        }
                        DBDefaultValue.Text = info.DBDefaultValue;
                    }
                }
            }
		}


        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (EBooleanUtils.GetEnumType(this.CanBeNull.SelectedValue) == EBoolean.False && string.IsNullOrEmpty(this.DBDefaultValue.Text))
            {
                base.FailMessage("���ֶ�ֵ������Ϊ��ʱ��������һ�����ݿ�Ĭ��ֵ��");
                return;
            }

            if (base.GetQueryString("TableMetadataID") != null)
            {
                int TableMetadataID = base.GetIntQueryString("TableMetadataID");

                TableMetadataInfo info = BaiRongDataProvider.TableMetadataDAO.GetTableMetadataInfo(TableMetadataID);
                info.AuxiliaryTableENName = this.tableName;
                info.AttributeName = AttributeName.Text;
                info.DataType = EDataTypeUtils.GetEnumType(DataType.SelectedValue);

                Hashtable hashtable = new Hashtable();
                hashtable[EDataType.DateTime] = new string[] { "8", "false" };
                hashtable[EDataType.Integer] = new string[] { "4", "false" };
                hashtable[EDataType.NChar] = new string[] { "50", "true" };
                hashtable[EDataType.NText] = new string[] { "16", "false" };
                hashtable[EDataType.NVarChar] = new string[] { "255", "true" };

                string[] strArr = (string[])hashtable[EDataTypeUtils.GetEnumType(DataType.SelectedValue)];
                if (strArr[1].Equals("false"))
                {
                    DataLength.Text = strArr[0];
                }

                info.DataLength = int.Parse(DataLength.Text);
                if (info.DataType == EDataType.NVarChar || info.DataType == EDataType.NChar)
                {
                    int maxLength = SqlUtils.GetMaxLengthForNVarChar(BaiRongDataProvider.DatabaseType);
                    if (info.DataLength <= 0 || info.DataLength > maxLength)
                    {
                        base.FailMessage(string.Format("�ֶ��޸�ʧ�ܣ����ݳ��ȵ�ֵ����λ�� 1 �� {0} ֮��", maxLength));
                        return;
                    }
                }
                info.CanBeNull = TranslateUtils.ToBool(CanBeNull.SelectedValue);
                info.DBDefaultValue = DBDefaultValue.Text;

                try
                {
                    BaiRongDataProvider.TableMetadataDAO.Update(info);

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "�޸ĸ������ֶ�", string.Format("������:{0},�ֶ���:{1}", this.tableName, info.AttributeName));

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, ex.Message);
                }
            }
            else
            {
                ETableStyle tableStyle = EAuxiliaryTableTypeUtils.GetTableStyle(this.tableType);
                ArrayList attributeNameArrayList = TableManager.GetAttributeNameArrayList(tableStyle, this.tableName, true);
                attributeNameArrayList.AddRange(TableManager.GetHiddenAttributeNameArrayList(tableStyle));
                if (attributeNameArrayList.IndexOf(AttributeName.Text.Trim().ToLower()) != -1)
                {
                    base.FailMessage("�ֶ����ʧ�ܣ��ֶ����Ѵ��ڣ�");
                }
                else if (!SqlUtils.IsAttributeNameCompliant(this.AttributeName.Text))
                {
                    base.FailMessage("�ֶ���������ϵͳҪ��");
                }
                else
                {
                    TableMetadataInfo info = new TableMetadataInfo();
                    info.AuxiliaryTableENName = this.tableName;
                    info.AttributeName = AttributeName.Text;
                    info.DataType = EDataTypeUtils.GetEnumType(DataType.SelectedValue);

                    Hashtable hashtable = new Hashtable();
                    hashtable[EDataType.DateTime] = new string[] { "8", "false" };
                    hashtable[EDataType.Integer] = new string[] { "4", "false" };
                    hashtable[EDataType.NChar] = new string[] { "50", "true" };
                    hashtable[EDataType.NText] = new string[] { "16", "false" };
                    hashtable[EDataType.NVarChar] = new string[] { "255", "true" };

                    string[] strArr = (string[])hashtable[EDataTypeUtils.GetEnumType(DataType.SelectedValue)];
                    if (strArr[1].Equals("false"))
                    {
                        DataLength.Text = strArr[0];
                    }

                    info.DataLength = int.Parse(DataLength.Text);
                    if (info.DataType == EDataType.NVarChar || info.DataType == EDataType.NChar)
                    {
                        int maxLength = SqlUtils.GetMaxLengthForNVarChar(BaiRongDataProvider.DatabaseType);
                        if (info.DataLength <= 0 || info.DataLength > maxLength)
                        {
                            base.FailMessage(string.Format("�ֶ��޸�ʧ�ܣ����ݳ��ȵ�ֵ����λ�� 1 �� {0} ֮��", maxLength));
                            return;
                        }
                    }
                    info.CanBeNull = TranslateUtils.ToBool(CanBeNull.SelectedValue);
                    info.DBDefaultValue = DBDefaultValue.Text;
                    info.IsSystem = false;

                    try
                    {
                        BaiRongDataProvider.TableMetadataDAO.Insert(info);

                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "��Ӹ������ֶ�", string.Format("������:{0},�ֶ���:{1}", this.tableName, info.AttributeName));

                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, ex.Message);
                    }
                }
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
		}

	}
}
