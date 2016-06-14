using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using BaiRong.Model;

namespace BaiRong.Core.AuxiliaryTable
{
	public class TableUtils
	{
		private TableUtils()
		{
		}

        public static void SetDefaultValues(Control containerControl, ETableStyle tableStyle, string tableName, ArrayList relatedIdentities)
        {
            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
            foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
            {
                SetDefaultValue(containerControl, styleInfo);
            }
        }

        public static void SetDefaultValue(Control containerControl, ETableStyle tableStyle, string tableName, string attributeName, ArrayList relatedIdentities)
		{
            TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(tableStyle, tableName, attributeName, relatedIdentities);
            ControlUtils.SetInputValue(containerControl, attributeName, styleInfo);
		}

        public static void SetDefaultValue(Control containerControl, TableStyleInfo styleInfo)
        {
            ControlUtils.SetInputValue(containerControl,styleInfo.AttributeName, styleInfo);
        }

        //public static void SetDefaultValue(Control containerControl, EAuxiliaryTableType tableType, TableMetadataInfo metaInfo)
        //{
        //    TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(tableType, metaInfo);
        //    ControlUtils.SetInputValue(containerControl, metaInfo.AttributeName, styleInfo);
        //}

        public static void SetNameValueCollection(Control containerControl, string tableName, NameValueCollection attributes)
		{
            ArrayList metaInfoArrayList = TableManager.GetTableMetadataInfoArrayList(tableName);
			foreach (TableMetadataInfo metaInfo in metaInfoArrayList)
			{
                string value = ControlUtils.GetInputValue(containerControl, metaInfo.AttributeName);
                if (value != null)
				{
                    attributes[metaInfo.AttributeName] = value;
				}
			}
		}

        //public static string GetInputValue(Control containerControl, TableMetadataInfo metaInfo, ArrayList relatedIdentities)
        //{
        //    TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(metaInfo.AuxiliaryTableENName, metaInfo.AttributeName, relatedIdentities);
        //    return ControlUtils.GetInputValue(containerControl, metaInfo.AttributeName, styleInfo);
        //}

        //public static string GetInputValue(Control containerControl, TableMetadataInfo metaInfo)
        //{
        //    TableStyleInfo styleInfo = TableStyleManager.GetTableStyleInfo(metaInfo.AuxiliaryTableENName, metaInfo.AttributeName);
        //    return ControlUtils.GetInputValue(containerControl, metaInfo.AttributeName, styleInfo);
        //}
	}
}
