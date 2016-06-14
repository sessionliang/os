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
            return PageUtility.GetOpenWindowString("������Ӹ������ֶ�", "modal_tableMetadataAddBatch.aspx", arguments);
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
                    base.FailMessage("���ֶ�ֵ������Ϊ��ʱ��������һ�����ݿ�Ĭ��ֵ��");
                    return;
                }


                if (attributeNameArrayList.IndexOf(attributeName.Trim().ToLower()) != -1)
                {
                    base.FailMessage("�ֶ����ʧ�ܣ��ֶ����Ѵ��ڣ�");
                }
                else if (!SqlUtils.IsAttributeNameCompliant(attributeName))
                {
                    base.FailMessage("�ֶ���������ϵͳҪ��");
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
                            base.FailMessage(string.Format("�ֶ��޸�ʧ�ܣ����ݳ��ȵ�ֵ����λ�� 1 �� {0} ֮��", maxLength));
                            return;
                        }
                    }
                    info.CanBeNull = canBeNull;
                    info.DBDefaultValue = dbDefaultValue;
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
