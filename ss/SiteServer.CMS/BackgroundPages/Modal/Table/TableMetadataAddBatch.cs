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
using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class TableMetadataAddBatch : BackgroundBasePage
    {
        private string tableName;
        private EAuxiliaryTableType tableType;

        public static string GetOpenWindowStringToAdd(string tableName, EAuxiliaryTableType tableType)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("TableName", tableName);
            arguments.Add("TableType", EAuxiliaryTableTypeUtils.GetValue(tableType));
            return PageUtility.GetOpenWindowString("批量添加辅助表字段", "modal_tableMetadataAddBatch.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("TableName", "TableType");

            this.tableName = base.GetQueryString("TableName");
            this.tableType = EAuxiliaryTableTypeUtils.GetEnumType(base.GetQueryString("TableType"));

            if (!IsPostBack)
            {

            }
        }


        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            List<string> attributeNameList = TranslateUtils.StringCollectionToStringList(base.Request.Form["attributeName"]);
            List<string> dataTypeList = TranslateUtils.StringCollectionToStringList(base.Request.Form["dataType"]);
            List<string> dataLengthList = TranslateUtils.StringCollectionToStringList(base.Request.Form["dataLength"]);
            List<string> canBeNullList = TranslateUtils.StringCollectionToStringList(base.Request.Form["canBeNull"]);
            List<string> dbDefaultValueList = TranslateUtils.StringCollectionToStringList(base.Request.Form["dbDefaultValue"]);

            for (int i = 0; i < attributeNameList.Count; i++)
            {
                if (dataTypeList.Count < attributeNameList.Count)
                    dataTypeList.Add(string.Empty);
                if (dataLengthList.Count < attributeNameList.Count)
                    dataLengthList.Add(string.Empty);
                if (canBeNullList.Count < attributeNameList.Count)
                    canBeNullList.Add(string.Empty);
                if (dbDefaultValueList.Count < attributeNameList.Count)
                    dbDefaultValueList.Add(string.Empty);
            }


            ETableStyle tableStyle = EAuxiliaryTableTypeUtils.GetTableStyle(this.tableType);
            ArrayList attributeNameArrayList = TableManager.GetAttributeNameArrayList(tableStyle, this.tableName, true);
            attributeNameArrayList.AddRange(TableManager.GetHiddenAttributeNameArrayList(tableStyle));

            for (int i = 0; i < attributeNameList.Count; i++)
            {
                string attributeName = attributeNameList[i];
                string dataType = dataTypeList[i];
                string dataLength = dataLengthList[i];
                bool canBeNull = TranslateUtils.ToBool(canBeNullList[i], true);
                string dbDefaultValue = dbDefaultValueList[i];

                if (!canBeNull && string.IsNullOrEmpty(dbDefaultValue))
                {
                    base.FailMessage("当字段值不允许为空时必须设置一个数据库默认值！");
                    return;
                }


                if (attributeNameArrayList.IndexOf(attributeName.Trim().ToLower()) != -1)
                {
                    base.FailMessage("字段添加失败，字段名已存在！");
                }
                else if (!SqlUtils.IsAttributeNameCompliant(attributeName))
                {
                    base.FailMessage("字段名不符合系统要求！");
                }
                else
                {
                    TableMetadataInfo info = new TableMetadataInfo();
                    info.AuxiliaryTableENName = this.tableName;
                    info.AttributeName = attributeName;
                    info.DataType = EDataTypeUtils.GetEnumType(dataType);

                    Hashtable hashtable = new Hashtable();
                    hashtable[EDataType.DateTime] = new string[] { "8", "false" };
                    hashtable[EDataType.Integer] = new string[] { "4", "false" };
                    hashtable[EDataType.NChar] = new string[] { "50", "true" };
                    hashtable[EDataType.NText] = new string[] { "16", "false" };
                    hashtable[EDataType.NVarChar] = new string[] { "255", "true" };

                    string[] strArr = (string[])hashtable[EDataTypeUtils.GetEnumType(dataType)];
                    if (strArr[1].Equals("false"))
                    {
                        dataLength = strArr[0];
                    }

                    if (string.IsNullOrEmpty(dataLength))
                    {
                        dataLength = strArr[0];
                    }

                    info.DataLength = int.Parse(dataLength);
                    if (info.DataType == EDataType.NVarChar || info.DataType == EDataType.NChar)
                    {
                        int maxLength = SqlUtils.GetMaxLengthForNVarChar(BaiRongDataProvider.DatabaseType);
                        if (info.DataLength <= 0 || info.DataLength > maxLength)
                        {
                            base.FailMessage(string.Format("字段修改失败，数据长度的值必须位于 1 和 {0} 之间", maxLength));
                            return;
                        }
                    }
                    info.CanBeNull = canBeNull;
                    info.DBDefaultValue = dbDefaultValue;
                    info.IsSystem = false;

                    try
                    {
                        BaiRongDataProvider.TableMetadataDAO.Insert(info);

                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加辅助表字段", string.Format("辅助表:{0},字段名:{1}", this.tableName, info.AttributeName));

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
