using System;
using System.Web;
using System.Collections;
using System.Data;
using System.Text;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core.AuxiliaryTable
{
    public sealed class TableManager
    {
        private static object lockObject = new object();
        private static bool async = true;//缓存与数据库不同步
        private const string cacheKey = "BaiRong.Core.AuxiliaryTable.TableManager";

        /// <summary>
        /// 得到辅助表tableName数据库中的字段名称的集合
        /// </summary>
        public static ArrayList GetAttributeNameArrayList(ETableStyle tableStyle, string tableName, bool toLower)
        {
            ArrayList attributeNameArrayList = new ArrayList();
            if (ETableStyleUtils.IsContent(tableStyle))
            {
                ArrayList tableMetadataInfoArrayList = GetTableMetadataInfoArrayList(tableName);
                foreach (TableMetadataInfo tableMetadataInfo in tableMetadataInfoArrayList)
                {
                    attributeNameArrayList.Add(tableMetadataInfo.AttributeName);
                }
            } 

            if (attributeNameArrayList.Count > 0 && toLower)
            {
                ArrayList lowerArrayList = new ArrayList();
                foreach (string attributeName in attributeNameArrayList)
                {
                    lowerArrayList.Add(attributeName.ToLower());
                }
                return lowerArrayList;
            }

            return attributeNameArrayList;
        }

        /// <summary>
        /// 得到辅助表的所有字段名称的集合
        /// </summary>
        public static ArrayList GetAttributeNameArrayList(ETableStyle tableStyle, string tableName)
        {
            return GetAttributeNameArrayList(tableStyle, tableName, false);
        }


        //获取辅助表隐藏字段名称集合
        public static ArrayList GetHiddenAttributeNameArrayList(ETableStyle tableStyle)
        {
            if (ETableStyleUtils.IsContent(tableStyle))
            {
                return ContentAttribute.HiddenAttributes;
            }
            else if (tableStyle == ETableStyle.Channel)
            {
                return ChannelAttribute.HiddenAttributes;
            }
            else if (tableStyle == ETableStyle.InputContent)
            {
                return InputContentAttribute.HiddenAttributes;
            }
            return new ArrayList();
        }

        public static ArrayList GetExcludeAttributeNames(ETableStyle tableStyle)
        {
            if (tableStyle == ETableStyle.BackgroundContent)
            {
                return BackgroundContentAttribute.ExcludeAttributes;
            }
            else if (tableStyle == ETableStyle.GovPublicContent)
            {
                return GovPublicContentAttribute.ExcludeAttributes;
            }
            else if (tableStyle == ETableStyle.GovInteractContent)
            {
                return GovInteractContentAttribute.ExcludeAttributes;
            }
            else if (tableStyle == ETableStyle.VoteContent)
            {
                return VoteContentAttribute.ExcludeAttributes;
            }
            else if (tableStyle == ETableStyle.JobContent)
            {
                return JobContentAttribute.ExcludeAttributes;
            }
            else if (tableStyle == ETableStyle.GoodsContent)
            {
                return GoodsContentAttribute.ExcludeAttributes;
            }
            else if (tableStyle == ETableStyle.BrandContent)
            {
                return BrandContentAttribute.ExcludeAttributes;
            }
            else if (tableStyle == ETableStyle.UserDefined)
            {
                return ContentAttribute.ExcludeAttributes;
            }
            return new ArrayList();
        }

        public static bool IsAttributeNameExists(ETableStyle tableStyle, string tableName, string attributeName)
        {
            ArrayList arraylist = GetAttributeNameArrayList(tableStyle, tableName, true);
            return arraylist.Contains(attributeName.ToLower());
        }

        /// <summary>
        /// 得到辅助表tableName的所有元数据的集合
        /// </summary>
        public static ArrayList GetTableMetadataInfoArrayList(string tableName)
        {
            ArrayList metadataList = new ArrayList();

            Hashtable tableENNameAndTableMetadataInfoArrayListHashtable = GetTableENNameAndTableMetadataInfoArrayListHashtable();
            if (tableENNameAndTableMetadataInfoArrayListHashtable != null && tableENNameAndTableMetadataInfoArrayListHashtable[tableName] != null)
            {
                ArrayList additionMetadataList = (ArrayList)tableENNameAndTableMetadataInfoArrayListHashtable[tableName];
                if (additionMetadataList != null)
                {
                    foreach (TableMetadataInfo metadataInfo in additionMetadataList)
                    {
                        bool contains = false;
                        foreach (TableMetadataInfo info in metadataList)
                        {
                            if (StringUtils.EqualsIgnoreCase(info.AttributeName, metadataInfo.AttributeName))
                            {
                                contains = true;
                                break;
                            }
                        }
                        if (!contains)
                        {
                            metadataList.Add(metadataInfo);
                        }
                    }
                }
            }

            return metadataList;
        }


        public static Hashtable GetTableENNameAndTableMetadataInfoArrayListHashtable()
        {
            lock (lockObject)
            {
                if (async || CacheUtils.Get(cacheKey) == null)
                {
                    Hashtable tableHashtable = BaiRongDataProvider.TableMetadataDAO.GetTableENNameAndTableMetadataInfoArrayListHashtable();
                    CacheUtils.Max(cacheKey, tableHashtable);
                    async = false;
                    return tableHashtable;
                }
                return (Hashtable)CacheUtils.Get(cacheKey);
            }
        }

        public static TableMetadataInfo GetTableMetadataInfo(string tableName, string attributeName)
        {
            ArrayList tableMetadataInfoArrayList = GetTableMetadataInfoArrayList(tableName);
            foreach (TableMetadataInfo tableMetadataInfo in tableMetadataInfoArrayList)
            {
                if (StringUtils.EqualsIgnoreCase(tableMetadataInfo.AttributeName, attributeName))
                {
                    return tableMetadataInfo;
                }
            }
            return null;
        }

        public static string GetTableMetadataDataType(string tableName, string attributeName)
        {
            TableMetadataInfo metadataInfo = TableManager.GetTableMetadataInfo(tableName, attributeName);
            if (metadataInfo != null)
            {
                return EDataTypeUtils.GetTextByAuxiliaryTable(metadataInfo.DataType, metadataInfo.DataLength);
            }
            return string.Empty;
        }

        public static int GetTableMetadataID(string tableName, string attributeName)
        {
            int metadataID = 0;
            ArrayList tableMetadataInfoArrayList = GetTableMetadataInfoArrayList(tableName);
            foreach (TableMetadataInfo tableMetadataInfo in tableMetadataInfoArrayList)
            {
                if (StringUtils.EqualsIgnoreCase(tableMetadataInfo.AttributeName, attributeName))
                {
                    metadataID = tableMetadataInfo.TableMetadataID;
                    break;
                }
            }
            return metadataID;
        }

        public static bool IsChanged
        {
            set
            {
                lock (lockObject)
                {
                    if (value == true)
                    {
                        BaiRong.Core.Data.SqlUtils.Cache_RemoveTableColumnInfoArrayListCache();
                    }
                    async = value;
                }
            }
        }

        public static string GetSerializedString(string tableName)
        {
            StringBuilder builder = new StringBuilder();
            ArrayList metadataInfoArrayList = TableManager.GetTableMetadataInfoArrayList(tableName);
            SortedList sortedlist = new SortedList();
            foreach (TableMetadataInfo metadataInfo in metadataInfoArrayList)
            {
                if (metadataInfo.IsSystem == false)
                {
                    /*
                     * AttributeName,
                     * DataType,
                     * DataLength,
                     * CanBeNull,
                     * DBDefaultValue
                     * */
                    string serialize = string.Format("AttributeName:{0};DataType:{1};DataLength={2};CanBeNull={3};DBDefaultValue={4}", metadataInfo.AttributeName, EDataTypeUtils.GetValue(metadataInfo.DataType), metadataInfo.DataLength, metadataInfo.CanBeNull.ToString(), metadataInfo.DBDefaultValue);
                    sortedlist.Add(metadataInfo.AttributeName, serialize);
                }
            }

            foreach (string attributeName in sortedlist.Keys)
            {
                builder.Append(sortedlist[attributeName]);
            }

            return builder.ToString();
        }

        public static string GetTableNameOfArchive(string tableName)
        {
            return tableName + "_Archive";
        }
    }

}
