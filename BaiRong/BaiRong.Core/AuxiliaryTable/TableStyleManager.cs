using System.Collections;
using System.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.Core;

namespace BaiRong.Core.AuxiliaryTable
{
    public sealed class TableStyleManager
    {
        private static readonly object lockObject = new object();
        private static bool async = true;//���������ݿⲻͬ��
        private const string cacheKey = "BaiRong.Core.AuxiliaryTable.TableStyleManager";

        public static PairArrayList GetAllTableStyleInfoPairs()
        {
            lock (lockObject)
            {
                if (async || CacheUtils.Get(cacheKey) == null)
                {
                    PairArrayList entries = BaiRongDataProvider.TableStyleDAO.GetAllTableStyleInfoPairs();
                    CacheUtils.Max(cacheKey, entries);
                    async = false;
                    return entries;
                }
                return (PairArrayList)CacheUtils.Get(cacheKey);
            }
        }

        public static ArrayList GetUserTableStyleInfoArrayList(string groupSN)
        {
            return GetTableStyleInfoArrayList(ETableStyle.User, groupSN, null);
        }

        public static ArrayList GetUserTableStyleInfoArrayList(string tableName, ArrayList relatedIdentities)
        {
            return GetTableStyleInfoArrayList(ETableStyle.User, tableName, relatedIdentities);
        }

        public static ArrayList GetTableStyleInfoArrayList(ETableStyle tableStyle, string tableName, ArrayList relatedIdentities)
        {
            relatedIdentities = GetRelatedIdentities(tableStyle, relatedIdentities);

            SortedList sortedlist = new SortedList();
            ArrayList allAttributeNames = new ArrayList();
            ArrayList arraylist = new ArrayList();

            PairArrayList entries = GetAllTableStyleInfoPairs();
            ArrayList attributeNames = TableManager.GetAttributeNameArrayList(tableStyle, tableName);

            if (tableStyle == ETableStyle.User)
            {
                attributeNames.AddRange(UserAttribute.BasicAttributes);
                if (!relatedIdentities.Contains(0))
                {
                    relatedIdentities.Add(0);
                }
            }
            if (tableStyle == ETableStyle.WebsiteMessageContent)
            {
                attributeNames.AddRange(WebsiteMessageContentAttribute.BasicAttributes);
                if (!relatedIdentities.Contains(0))
                {
                    relatedIdentities.Add(0);
                }
            }
            #region ���ܱ��ֶ� by 20160304 sofuny
            if (tableStyle == ETableStyle.EvaluationContent)
            {
                attributeNames.AddRange(EvaluationContentAttribute.BasicAttributes);
                if (!relatedIdentities.Contains(0))
                {
                    relatedIdentities.Add(0);
                }
            }
            if (tableStyle == ETableStyle.TrialApplyContent)
            {
                attributeNames.AddRange(TrialApplyAttribute.BasicAttributes);
                if (!relatedIdentities.Contains(0))
                {
                    relatedIdentities.Add(0);
                }
            }
            if (tableStyle == ETableStyle.TrialReportContent)
            {
                attributeNames.AddRange(TrialReportAttribute.BasicAttributes);
                if (!relatedIdentities.Contains(0))
                {
                    relatedIdentities.Add(0);
                }
            }
            if (tableStyle == ETableStyle.SurveyContent)
            {
                attributeNames.AddRange(SurveyQuestionnaireAttribute.BasicAttributes);
                if (!relatedIdentities.Contains(0))
                {
                    relatedIdentities.Add(0);
                }
            }
            if (tableStyle == ETableStyle.CompareContent)
            {
                attributeNames.AddRange(CompareContentAttribute.BasicAttributes);
                if (!relatedIdentities.Contains(0))
                {
                    relatedIdentities.Add(0);
                }
            }
            #endregion
            //�������ʵ�ʱ��������ݿ��е���ʽ
            int i = 99;
            foreach (int relatedIdentity in relatedIdentities)
            {
                if (ETableStyleUtils.IsContent(tableStyle) && relatedIdentity == 0) continue;
                string startKey = TableStyleManager.GetCacheKeyStart(relatedIdentity, tableName);
                ArrayList keyArrayList = entries.GetKeys(startKey);
                foreach (string key in keyArrayList)
                {
                    TableStyleInfo styleInfo = entries.GetValue(key) as TableStyleInfo;
                    if (!allAttributeNames.Contains(styleInfo.AttributeName))
                    {
                        allAttributeNames.Add(styleInfo.AttributeName);
                        if (styleInfo.Taxis <= 0 && attributeNames.Contains(styleInfo.AttributeName))//���ݿ��ֶ�
                        {
                            sortedlist.Add(styleInfo.AttributeName, styleInfo);
                        }
                        else if (styleInfo.Taxis > 0)                       //�����ֶ�
                        {
                            string iStr = relatedIdentity.ToString() + styleInfo.Taxis.ToString().PadLeft(3, '0');
                            sortedlist.Add("1" + iStr + "_" + styleInfo.AttributeName, styleInfo);
                        }
                        else                                                //δ�����ֶ�
                        {
                            string iStr = relatedIdentity.ToString() + i.ToString().PadLeft(3, '0');
                            sortedlist.Add("0" + iStr + "_" + styleInfo.AttributeName, styleInfo);
                            i = i - 1;
                        }
                    }
                }
            }

            foreach (string attributeName in attributeNames)
            {
                TableStyleInfo styleInfo = null;
                if (!allAttributeNames.Contains(attributeName))
                {
                    allAttributeNames.Add(attributeName);

                    if (tableStyle == ETableStyle.BackgroundContent)
                    {
                        styleInfo = TableStyleManager.GetBackgroundContentTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.GovPublicContent)
                    {
                        styleInfo = TableStyleManager.GetGovPublicContentTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.GovInteractContent)
                    {
                        styleInfo = TableStyleManager.GetGovInteractContentTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.VoteContent)
                    {
                        styleInfo = TableStyleManager.GetVoteContentTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.JobContent)
                    {
                        styleInfo = TableStyleManager.GetJobContentTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.GoodsContent)
                    {
                        styleInfo = TableStyleManager.GetGoodsContentTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.BrandContent)
                    {
                        styleInfo = TableStyleManager.GetBrandContentTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.UserDefined)
                    {
                        styleInfo = TableStyleManager.GetUserDefinedTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.User)
                    {
                        styleInfo = TableStyleManager.GetUserTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.WebsiteMessageContent)
                    {
                        styleInfo = TableStyleManager.GetWebsiteMessageTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.EvaluationContent)
                    {
                        styleInfo = TableStyleManager.GetEvaluationTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.TrialApplyContent)
                    {
                        styleInfo = TableStyleManager.GetTrialApplyTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.TrialReportContent)
                    {
                        styleInfo = TableStyleManager.GetTrialReportTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.SurveyContent)
                    {
                        styleInfo = TableStyleManager.GetSurveyQuestionnaireTableStyleInfo(tableName, attributeName);
                    }
                    else if (tableStyle == ETableStyle.CompareContent)
                    {
                        styleInfo = TableStyleManager.GetCompareTableStyleInfo(tableName, attributeName);
                    }
                    else
                    {
                        styleInfo = TableStyleManager.GetDefaultTableStyleInfo(tableName, attributeName);
                    }

                    arraylist.Add(styleInfo);
                }
                else
                {
                    styleInfo = sortedlist[attributeName] as TableStyleInfo;
                    if (styleInfo != null && styleInfo.Taxis <= 0 && attributeNames.Contains(styleInfo.AttributeName))
                        arraylist.Add(sortedlist[attributeName]);
                }
            }

            foreach (string key in sortedlist.Keys)
            {
                if (!attributeNames.Contains(key))
                {
                    arraylist.Add(sortedlist[key]);
                }
            }

            return arraylist;
        }

        public static bool IsExistsInParents(ArrayList relatedIdentities, string tableName, string attributeName)
        {
            PairArrayList entries = GetAllTableStyleInfoPairs();
            for (int i = 1; i < relatedIdentities.Count - 1; i++)
            {
                int relatedIdentity = (int)relatedIdentities[i];
                string startKey = TableStyleManager.GetCacheKeyStart(relatedIdentity, tableName);
                ArrayList keyArrayList = entries.GetKeys(startKey);
                foreach (string key in keyArrayList)
                {
                    TableStyleInfo styleInfo = entries.GetValue(key) as TableStyleInfo;
                    if (styleInfo.AttributeName == attributeName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsChanged
        {
            set
            {
                lock (lockObject)
                {
                    if (value)
                    {
                        CacheUtils.Remove(cacheKey);
                    }
                    async = value;
                }
            }
        }

        public static void ClearAPI()
        {
            AjaxUrlManager.AddAjaxUrl(PageUtils.API.GetTableStyleClearCacheUrl());
        }

        public static string GetCacheKey(int relatedIdentity, string tableName, string attributeName)
        {
            return string.Format("{0}${1}${2}", relatedIdentity, tableName, attributeName).ToLower();
        }

        public static string GetCacheKeyStart(int relatedIdentity, string tableName)
        {
            return string.Format("{0}${1}$", relatedIdentity, tableName).ToLower();
        }

        public static TableStyleInfo GetTableStyleInfo(int tableStyleID)
        {
            return BaiRongDataProvider.TableStyleDAO.GetTableStyleInfo(tableStyleID);
        }

        public static TableStyleInfo GetTableStyleInfoActually(ETableStyle tableStyle, string tableName, string attributeName, ArrayList relatedIdentities)
        {
            relatedIdentities = GetRelatedIdentities(tableStyle, relatedIdentities);
            TableStyleInfo styleInfo = GetTableStyleInfo(tableStyle, tableName, attributeName, relatedIdentities);
            if (styleInfo.RelatedIdentity != (int)relatedIdentities[0])
            {
                styleInfo.TableStyleID = 0;
                styleInfo.RelatedIdentity = (int)relatedIdentities[0];
            }
            return styleInfo;
        }

        //relatedIdentities�Ӵ�С�������0
        public static TableStyleInfo GetTableStyleInfo(ETableStyle tableStyle, string tableName, string attributeName, ArrayList relatedIdentities)
        {
            TableStyleInfo styleInfo = null;

            relatedIdentities = GetRelatedIdentities(tableStyle, relatedIdentities);

            if (tableStyle == ETableStyle.User)
            {
                if (!relatedIdentities.Contains(0))
                {
                    relatedIdentities.Add(0);
                }
            }

            PairArrayList entries = GetAllTableStyleInfoPairs();

            foreach (int relatedIdentity in relatedIdentities)
            {
                string key = GetCacheKey(relatedIdentity, tableName, attributeName);
                if (entries.ContainsKey(key))
                {
                    styleInfo = entries.GetValue(key) as TableStyleInfo;
                    if (styleInfo != null)
                    {
                        break;
                    }
                }
            }

            if (styleInfo == null)
            {
                if (tableStyle == ETableStyle.BackgroundContent)
                {
                    styleInfo = TableStyleManager.GetBackgroundContentTableStyleInfo(tableName, attributeName);
                }
                else if (tableStyle == ETableStyle.GovPublicContent)
                {
                    styleInfo = TableStyleManager.GetGovPublicContentTableStyleInfo(tableName, attributeName);
                }
                else if (tableStyle == ETableStyle.GovInteractContent)
                {
                    styleInfo = TableStyleManager.GetGovInteractContentTableStyleInfo(tableName, attributeName);
                }
                else if (tableStyle == ETableStyle.VoteContent)
                {
                    styleInfo = TableStyleManager.GetVoteContentTableStyleInfo(tableName, attributeName);
                }
                else if (tableStyle == ETableStyle.JobContent)
                {
                    styleInfo = TableStyleManager.GetJobContentTableStyleInfo(tableName, attributeName);
                }
                else if (tableStyle == ETableStyle.GoodsContent)
                {
                    styleInfo = TableStyleManager.GetGoodsContentTableStyleInfo(tableName, attributeName);
                }
                else if (tableStyle == ETableStyle.BrandContent)
                {
                    styleInfo = TableStyleManager.GetBrandContentTableStyleInfo(tableName, attributeName);
                }
                else if (tableStyle == ETableStyle.UserDefined)
                {
                    styleInfo = TableStyleManager.GetUserDefinedTableStyleInfo(tableName, attributeName);
                }
                else if (tableStyle == ETableStyle.User)
                {
                    styleInfo = TableStyleManager.GetUserTableStyleInfo(tableName, attributeName);
                }
                else
                {
                    styleInfo = TableStyleManager.GetDefaultTableStyleInfo(tableName, attributeName);
                }
            }

            return styleInfo;
        }

        private static ArrayList GetRelatedIdentities(ETableStyle tableStyle, ArrayList arraylist)
        {
            ArrayList relatedIdentities = new ArrayList();
            if (arraylist != null && arraylist.Count > 0)
            {
                relatedIdentities.AddRange(arraylist);
            }
            if (tableStyle != ETableStyle.User)
            {
                if (relatedIdentities.Count == 0)
                {
                    relatedIdentities.Add(0);
                }
                else if ((int)relatedIdentities[relatedIdentities.Count - 1] != 0)
                {
                    relatedIdentities.Add(0);
                }
            }
            return relatedIdentities;
        }

        public static int Insert(TableStyleInfo styleInfo, ETableStyle tableStyle)
        {
            int tableStyleID = BaiRongDataProvider.TableStyleDAO.Insert(styleInfo, tableStyle);
            TableStyleManager.IsChanged = true;
            TableStyleManager.ClearAPI();
            return tableStyleID;
        }

        public static void InsertWithTaxis(TableStyleInfo styleInfo, ETableStyle tableStyle)
        {
            BaiRongDataProvider.TableStyleDAO.InsertWithTaxis(styleInfo, tableStyle);
            TableStyleManager.IsChanged = true;
            TableStyleManager.ClearAPI();
        }

        public static void Update(TableStyleInfo info)
        {
            BaiRongDataProvider.TableStyleDAO.Update(info);
            TableStyleManager.IsChanged = true;
            TableStyleManager.ClearAPI();
        }

        public static void InsertOrUpdate(TableStyleInfo styleInfo, ETableStyle tableStyle)
        {
            if (styleInfo.TableStyleID == 0)
            {
                Insert(styleInfo, tableStyle);
            }
            else
            {
                Update(styleInfo);
            }
        }

        public static void DeleteAndInsertStyleItems(int tableStyleID, ArrayList styleItems)
        {
            if (styleItems != null && styleItems.Count > 0)
            {
                BaiRongDataProvider.TableStyleDAO.DeleteStyleItems(tableStyleID);
                BaiRongDataProvider.TableStyleDAO.InsertStyleItems(styleItems);
            }
        }

        public static void Delete(int relatedIdentity, string tableName, string attributeName)
        {
            BaiRongDataProvider.TableStyleDAO.Delete(relatedIdentity, tableName, attributeName);
            TableStyleManager.IsChanged = true;
            TableStyleManager.ClearAPI();
        }

        public static bool IsExists(int relatedIdentity, string tableName, string attributeName)
        {
            string key = GetCacheKey(relatedIdentity, tableName, attributeName);
            PairArrayList entries = GetAllTableStyleInfoPairs();
            return entries.Keys.Contains(key);
        }

        public static bool IsVisibleInList(TableStyleInfo styleInfo)
        {
            if (styleInfo.AttributeName == ContentAttribute.Title || styleInfo.IsVisibleInList == false)
            {
                return false;
            }
            return true;
        }

        public static ArrayList GetStyleItemArrayList(int tableStyleID)
        {
            return BaiRongDataProvider.TableStyleDAO.GetStyleItemArrayList(tableStyleID);
        }

        public static DataSet GetStyleItemDataSet(int styleItemCount, ArrayList styleItemInfoArrayList)
        {
            DataSet dataset = new DataSet();

            DataTable dataTable = new DataTable("StyleItems");

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "TableStyleItemID";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "TableStyleID";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ItemTitle";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ItemValue";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "IsSelected";
            dataTable.Columns.Add(column);

            for (int i = 0; i < styleItemCount; i++)
            {
                DataRow dataRow = dataTable.NewRow();

                TableStyleItemInfo itemInfo = (styleItemInfoArrayList != null && styleItemInfoArrayList.Count > i) ? (TableStyleItemInfo)styleItemInfoArrayList[i] : new TableStyleItemInfo();

                dataRow["TableStyleItemID"] = itemInfo.TableStyleItemID;
                dataRow["TableStyleID"] = itemInfo.TableStyleID;
                dataRow["ItemTitle"] = itemInfo.ItemTitle;
                dataRow["ItemValue"] = itemInfo.ItemValue;
                dataRow["IsSelected"] = itemInfo.IsSelected.ToString();

                dataTable.Rows.Add(dataRow);
            }

            dataset.Tables.Add(dataTable);
            return dataset;
        }

        public static Hashtable GetTableStyleInfoWithItemsHashtable(string tableName, ArrayList allRelatedIdentities)
        {
            Hashtable hashtable = new Hashtable();

            PairArrayList entries = GetAllTableStyleInfoPairs();
            foreach (string key in entries.Keys)
            {
                int identityFromKey = TranslateUtils.ToInt(key.Split('$')[0]);
                string tableNameFromKey = key.Split('$')[1];
                if (StringUtils.EqualsIgnoreCase(tableNameFromKey, tableName) && (identityFromKey == 0 || allRelatedIdentities.Contains(identityFromKey)))
                {
                    TableStyleInfo styleInfo = (TableStyleInfo)entries.GetValue(key);
                    if (EInputTypeUtils.IsWithStyleItems(styleInfo.InputType))
                    {
                        styleInfo.StyleItems = BaiRongDataProvider.TableStyleDAO.GetStyleItemArrayList(styleInfo.TableStyleID);
                    }
                    ArrayList tableStyleInfoWithItemsArrayList = hashtable[styleInfo.AttributeName] as ArrayList;
                    if (tableStyleInfoWithItemsArrayList == null)
                    {
                        tableStyleInfoWithItemsArrayList = new ArrayList();
                    }
                    tableStyleInfoWithItemsArrayList.Add(styleInfo);
                    hashtable[styleInfo.AttributeName] = tableStyleInfoWithItemsArrayList;
                }
            }

            //ArrayList attributeNameArrayList = TableManager.GetAttributeNameArrayList(tableName);
            //foreach (string attributeName in attributeNameArrayList)
            //{
            //    int tableMetadataID = TableManager.GetTableMetadataID(tableName, attributeName);
            //    ArrayList tableStyleInfoWithItemsArrayList = BaiRongDataProvider.TableStyleDAO.GetTableStyleInfoWithItemsArrayList(tableName, attributeName);
            //    if (tableStyleInfoWithItemsArrayList != null && tableStyleInfoWithItemsArrayList.Count > 0)
            //    {
            //        hashtable[attributeName] = tableStyleInfoWithItemsArrayList;
            //    }
            //}

            return hashtable;
        }

        public static TableStyleInfo GetBackgroundContentTableStyleInfo(string tableName, string attributeName)
        {
            string lowerAttributeName = attributeName.ToLower();
            if (ContentAttribute.HiddenAttributes.Contains(lowerAttributeName))
            {
                return TableStyleManager.GetContentHiddenTableStyleInfo(tableName, attributeName);
            }
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);

            if (lowerAttributeName == ContentAttribute.Title.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.IsFormatString = true;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.SubTitle.ToLower())
            {
                styleInfo.DisplayName = "������";
            }
            else if (lowerAttributeName == BackgroundContentAttribute.ImageUrl.ToLower())
            {
                styleInfo.DisplayName = "ͼƬ";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.VideoUrl.ToLower())
            {
                styleInfo.DisplayName = "��Ƶ";
                styleInfo.InputType = EInputType.Video;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.FileUrl.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.InputType = EInputType.File;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.LinkUrl.ToLower())
            {
                styleInfo.DisplayName = "�ⲿ����";
                styleInfo.HelpText = "���ú����ӽ�ָ��˵�ַ";
            }
            else if (lowerAttributeName == BackgroundContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.Author.ToLower())
            {
                styleInfo.DisplayName = "����";
            }
            else if (lowerAttributeName == BackgroundContentAttribute.Source.ToLower())
            {
                styleInfo.DisplayName = "��Դ";
            }
            else if (lowerAttributeName == BackgroundContentAttribute.Summary.ToLower())
            {
                styleInfo.DisplayName = "����ժҪ";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.IsRecommend.ToLower())
            {
                styleInfo.DisplayName = "�Ƽ�";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.IsHot.ToLower())
            {
                styleInfo.DisplayName = "�ȵ�";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.IsColor.ToLower())
            {
                styleInfo.DisplayName = "��Ŀ";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "�ö�";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "�������";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == ContentAttribute.CheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "��ʱ�������";
                styleInfo.InputType = EInputType.DateTime;
            }
            else if (lowerAttributeName == ContentAttribute.UnCheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "��ʱ�¼�����";
                styleInfo.InputType = EInputType.DateTime;
            }
            else if (!string.IsNullOrEmpty(attributeName))
            {
                TableStyleInfo tableStyleInfo = BaiRongDataProvider.TableStyleDAO.GetTableStyleInfo(0, tableName, attributeName);
                if (tableStyleInfo != null)
                {
                    //styleInfo.DisplayName = attributeName;
                    //styleInfo.InputType = tableStyleInfo.InputType;
                    styleInfo = tableStyleInfo;
                }
            }

            return styleInfo;
        }

        public static TableStyleInfo GetGovPublicContentTableStyleInfo(string tableName, string attributeName)
        {
            string lowerAttributeName = attributeName.ToLower();
            if (ContentAttribute.HiddenAttributes.Contains(lowerAttributeName))
            {
                return TableStyleManager.GetContentHiddenTableStyleInfo(tableName, attributeName);
            }
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);

            if (lowerAttributeName == ContentAttribute.Title.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.IsFormatString = true;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.Identifier.ToLower())
            {
                styleInfo.DisplayName = "������";
            }
            else if (lowerAttributeName == GovPublicContentAttribute.Description.ToLower())
            {
                styleInfo.DisplayName = "���ݸ���";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.PublishDate.ToLower())
            {
                styleInfo.DisplayName = "��������";
                styleInfo.InputType = EInputType.Date;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.EffectDate.ToLower())
            {
                styleInfo.DisplayName = "��Ч����";
                styleInfo.InputType = EInputType.Date;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.IsAbolition.ToLower())
            {
                styleInfo.DisplayName = "�Ƿ��ֹ";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.AbolitionDate.ToLower())
            {
                styleInfo.DisplayName = "��ֹ����";
                styleInfo.InputType = EInputType.Date;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.DocumentNo.ToLower())
            {
                styleInfo.DisplayName = "�ĺ�";
            }
            else if (lowerAttributeName == GovPublicContentAttribute.Publisher.ToLower())
            {
                styleInfo.DisplayName = "��������";
            }
            else if (lowerAttributeName == GovPublicContentAttribute.Keywords.ToLower())
            {
                styleInfo.DisplayName = "�ؼ���";
            }
            else if (lowerAttributeName == GovPublicContentAttribute.ImageUrl.ToLower())
            {
                styleInfo.DisplayName = "ͼƬ";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.FileUrl.ToLower())
            {
                styleInfo.InputType = EInputType.File;
                styleInfo.IsSingleLine = true;
                styleInfo.DisplayName = "����";
            }
            else if (lowerAttributeName == GovPublicContentAttribute.IsRecommend.ToLower())
            {
                styleInfo.DisplayName = "�Ƽ�";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.IsHot.ToLower())
            {
                styleInfo.DisplayName = "�ȵ�";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.IsColor.ToLower())
            {
                styleInfo.DisplayName = "��Ŀ";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "�ö�";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "�������";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            return styleInfo;
        }

        public static TableStyleInfo GetVoteContentTableStyleInfo(string tableName, string attributeName)
        {
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);

            string lowerAttributeName = attributeName.ToLower();
            if (VoteContentAttribute.HiddenAttributes.Contains(lowerAttributeName))
            {
                if (lowerAttributeName == VoteContentAttribute.IsImageVote.ToLower())
                {
                    styleInfo.DisplayName = "ͼƬͶƱ";
                }
                else if (lowerAttributeName == VoteContentAttribute.IsSummary.ToLower())
                {
                    styleInfo.DisplayName = "��ʾ���";
                }
                else if (lowerAttributeName == VoteContentAttribute.Participants.ToLower())
                {
                    styleInfo.DisplayName = "��������";
                }
                else if (lowerAttributeName == VoteContentAttribute.IsClosed.ToLower())
                {
                    styleInfo.DisplayName = "�ѽ���";
                }
                else if (lowerAttributeName == VoteContentAttribute.IsTop.ToLower())
                {
                    styleInfo.DisplayName = "�ö�";
                }
                else
                {
                    styleInfo = TableStyleManager.GetContentHiddenTableStyleInfo(tableName, attributeName);
                }
                return styleInfo;
            }

            ArrayList styleItems = new ArrayList();
            TableStyleItemInfo itemInfo = null;

            if (lowerAttributeName == VoteContentAttribute.Title.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.InputType = EInputType.Text;
                styleInfo.Additional.IsFormatString = true;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == VoteContentAttribute.SubTitle.ToLower())
            {
                styleInfo.DisplayName = "������";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == VoteContentAttribute.MaxSelectNum.ToLower())
            {
                styleInfo.DisplayName = "��ѡ/��ѡ";
                styleInfo.InputType = EInputType.SelectOne;
            }
            else if (lowerAttributeName == VoteContentAttribute.ImageUrl.ToLower())
            {
                styleInfo.DisplayName = "ͼƬ";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == VoteContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == VoteContentAttribute.Summary.ToLower())
            {
                styleInfo.DisplayName = "����ժҪ";
                styleInfo.InputType = EInputType.TextArea;
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == VoteContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "��ʼ����";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == VoteContentAttribute.EndDate.ToLower())
            {
                styleInfo.DisplayName = "��ֹ����";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == VoteContentAttribute.IsVotedView.ToLower())
            {
                styleInfo.DisplayName = "ͶƱ���";
                styleInfo.InputType = EInputType.Radio;
            }
            else if (lowerAttributeName == VoteContentAttribute.HiddenContent.ToLower())
            {
                styleInfo.DisplayName = "��������";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == ContentAttribute.CheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "��ʱ�������";
                styleInfo.InputType = EInputType.DateTime;
            }
            else if (lowerAttributeName == ContentAttribute.UnCheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "��ʱ�¼�����";
                styleInfo.InputType = EInputType.DateTime;
            }
            return styleInfo;
        }

        public static TableStyleInfo GetJobContentTableStyleInfo(string tableName, string attributeName)
        {
            string lowerAttributeName = attributeName.ToLower();
            if (ContentAttribute.HiddenAttributes.Contains(lowerAttributeName))
            {
                return TableStyleManager.GetContentHiddenTableStyleInfo(tableName, attributeName);
            }
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);
            ArrayList styleItems = new ArrayList();
            TableStyleItemInfo itemInfo = null;

            if (lowerAttributeName == ContentAttribute.Title.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.HelpText = "���������";
                styleInfo.InputType = EInputType.Text;
                styleInfo.Additional.IsFormatString = true;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == JobContentAttribute.Department.ToLower())
            {
                styleInfo.DisplayName = "��������";
                styleInfo.HelpText = "��������";
                styleInfo.InputType = EInputType.Text;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == JobContentAttribute.Location.ToLower())
            {
                styleInfo.DisplayName = "�����ص�";
                styleInfo.HelpText = "�����ص�";
                styleInfo.InputType = EInputType.Text;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == JobContentAttribute.NumberOfPeople.ToLower())
            {
                styleInfo.DisplayName = "��Ƹ����";
                styleInfo.HelpText = "��Ƹ����";
                styleInfo.InputType = EInputType.Text;
                styleInfo.DefaultValue = "����";
                styleInfo.Additional.Width = "60px";
            }
            else if (lowerAttributeName == JobContentAttribute.Responsibility.ToLower())
            {
                styleInfo.DisplayName = "����ְ��";
                styleInfo.HelpText = "����ְ��";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == JobContentAttribute.Requirement.ToLower())
            {
                styleInfo.DisplayName = "����Ҫ��";
                styleInfo.HelpText = "����Ҫ��";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == JobContentAttribute.IsUrgent.ToLower())
            {
                styleInfo.DisplayName = "�Ƿ�Ƹ";
                styleInfo.HelpText = "�Ƿ�Ƹ";
                styleInfo.InputType = EInputType.CheckBox;
                itemInfo = new TableStyleItemInfo(0, styleInfo.TableStyleID, "��Ƹ", true.ToString(), false);
                styleItems.Add(itemInfo);
                styleInfo.StyleItems = styleItems;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "�ö�";
                styleInfo.HelpText = "�Ƿ�Ϊ�ö�����";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "�������";
                styleInfo.HelpText = "���ݵ��������";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == ContentAttribute.CheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "��ʱ�������";
                styleInfo.InputType = EInputType.DateTime;
            }
            else if (lowerAttributeName == ContentAttribute.UnCheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "��ʱ�¼�����";
                styleInfo.InputType = EInputType.DateTime;
            }
            return styleInfo;
        }

        public static TableStyleInfo GetGoodsContentTableStyleInfo(string tableName, string attributeName)
        {
            string lowerAttributeName = attributeName.ToLower();
            if (ContentAttribute.HiddenAttributes.Contains(lowerAttributeName))
            {
                return TableStyleManager.GetContentHiddenTableStyleInfo(tableName, attributeName);
            }
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);

            if (lowerAttributeName == ContentAttribute.Title.ToLower())
            {
                styleInfo.DisplayName = "��Ʒ����";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.IsFormatString = true;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "�ö�";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "�������";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == GoodsContentAttribute.SN.ToLower())
            {
                styleInfo.DisplayName = "��Ʒ���";
            }
            else if (lowerAttributeName == GoodsContentAttribute.Keywords.ToLower())
            {
                styleInfo.DisplayName = "��Ʒ�ؼ���";
                styleInfo.HelpText = "������ǰ̨����̨ɸѡ��Ʒ������ؼ����ð�Ƕ���,�ֿ�";
            }
            else if (lowerAttributeName == GoodsContentAttribute.Summary.ToLower())
            {
                styleInfo.DisplayName = "��Ʒ���";
                styleInfo.InputType = EInputType.TextArea;
                styleInfo.Additional.Height = 50;
            }
            else if (lowerAttributeName == GoodsContentAttribute.ImageUrl.ToLower())
            {
                styleInfo.DisplayName = "��ƷͼƬ";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == GoodsContentAttribute.ThumbUrl.ToLower())
            {
                styleInfo.DisplayName = "��Ʒ����ͼ";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == GoodsContentAttribute.LinkUrl.ToLower())
            {
                styleInfo.DisplayName = "�ⲿ����";
                styleInfo.HelpText = "���ú����ӽ�ָ��˵�ַ";
            }
            else if (lowerAttributeName == GoodsContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "��Ʒ����";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == GoodsContentAttribute.PriceCost.ToLower())
            {
                styleInfo.DisplayName = "�ɱ���";
                styleInfo.HelpText = "������̨ʹ��";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Currency;
                styleInfo.Additional.Width = "80px";
            }
            else if (lowerAttributeName == GoodsContentAttribute.PriceMarket.ToLower())
            {
                styleInfo.DisplayName = "�г���";
                styleInfo.HelpText = "��Ʒ���г��۸�";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Currency;
                styleInfo.Additional.Width = "80px";
            }
            else if (lowerAttributeName == GoodsContentAttribute.PriceSale.ToLower())
            {
                styleInfo.DisplayName = "���ۼ�";
                styleInfo.HelpText = "ʵ�����۵ļ۸�";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Currency;
                styleInfo.Additional.Width = "80px";
            }
            else if (lowerAttributeName == GoodsContentAttribute.Stock.ToLower())
            {
                styleInfo.DisplayName = "�����";
                styleInfo.HelpText = "�����۵���Ʒ����(-1��������,0����������)";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Integer;
                styleInfo.Additional.Width = "80px";
                styleInfo.DefaultValue = "-1";
            }
            else if (lowerAttributeName == GoodsContentAttribute.FileUrl.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.InputType = EInputType.File;
            }
            else if (lowerAttributeName == GoodsContentAttribute.IsRecommend.ToLower())
            {
                styleInfo.DisplayName = "�Ƽ�";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GoodsContentAttribute.IsNew.ToLower())
            {
                styleInfo.DisplayName = "��Ʒ";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GoodsContentAttribute.IsHot.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GoodsContentAttribute.IsOnSale.ToLower())
            {
                styleInfo.DisplayName = "�ϼ�����";
                styleInfo.HelpText = "�򹴱�ʾ�������ۣ�������������";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F1.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�1";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F2.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�2";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F3.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�3";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F4.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�4";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F5.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�5";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F6.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�6";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F7.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�7";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F8.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�8";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F9.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�9";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.SpecIDCollection.ToLower())
            {
                styleInfo.DisplayName = "SpecIDCollection";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.SpecItemIDCollection.ToLower())
            {
                styleInfo.DisplayName = "SpecItemIDCollection";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.SKUCount.ToLower())
            {
                styleInfo.DisplayName = "SKUCount";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == ContentAttribute.CheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "��ʱ�������";
                styleInfo.InputType = EInputType.DateTime;
            }
            else if (lowerAttributeName == ContentAttribute.UnCheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "��ʱ�¼�����";
                styleInfo.InputType = EInputType.DateTime;
            }
            return styleInfo;
        }

        public static TableStyleInfo GetBrandContentTableStyleInfo(string tableName, string attributeName)
        {
            string lowerAttributeName = attributeName.ToLower();
            if (ContentAttribute.HiddenAttributes.Contains(lowerAttributeName))
            {
                return TableStyleManager.GetContentHiddenTableStyleInfo(tableName, attributeName);
            }
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);

            if (lowerAttributeName == ContentAttribute.Title.ToLower())
            {
                styleInfo.DisplayName = "Ʒ������";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.IsFormatString = true;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "�ö�";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "�������";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == BrandContentAttribute.BrandUrl.ToLower())
            {
                styleInfo.DisplayName = "Ʒ����ַ";
            }
            else if (lowerAttributeName == BrandContentAttribute.ImageUrl.ToLower())
            {
                styleInfo.DisplayName = "ͼƬ";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == BrandContentAttribute.LinkUrl.ToLower())
            {
                styleInfo.DisplayName = "�ⲿ����";
                styleInfo.HelpText = "���ú����ӽ�ָ��˵�ַ";
            }
            else if (lowerAttributeName == BrandContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "Ʒ������";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == BrandContentAttribute.IsRecommend.ToLower())
            {
                styleInfo.DisplayName = "�Ƽ�";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == BrandContentAttribute.F1.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�1";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == BrandContentAttribute.F2.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�2";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == BrandContentAttribute.F3.ToLower())
            {
                styleInfo.DisplayName = "��չ�ֶ�3";
                styleInfo.IsVisible = false;
            }
            return styleInfo;
        }

        public static TableStyleInfo GetUserDefinedTableStyleInfo(string tableName, string attributeName)
        {
            string lowerAttributeName = attributeName.ToLower();
            if (ContentAttribute.HiddenAttributes.Contains(lowerAttributeName))
            {
                return TableStyleManager.GetContentHiddenTableStyleInfo(tableName, attributeName);
            }
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);
            ArrayList styleItems = new ArrayList();

            if (lowerAttributeName == ContentAttribute.Title.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.HelpText = "���������";
                styleInfo.InputType = EInputType.Text;
                styleInfo.Additional.IsFormatString = true;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "�ö�";
                styleInfo.HelpText = "�Ƿ�Ϊ�ö�����";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "�������";
                styleInfo.HelpText = "���ݵ��������";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }

            return styleInfo;
        }

        public static TableStyleInfo GetGovInteractContentTableStyleInfo(string tableName, string attributeName)
        {
            string lowerAttributeName = attributeName.ToLower();
            if (GovInteractContentAttribute.HiddenAttributes.Contains(lowerAttributeName))
            {
                return TableStyleManager.GetContentHiddenTableStyleInfo(tableName, attributeName);
            }
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, false, EInputType.Text, string.Empty, true, string.Empty);

            if (lowerAttributeName == GovInteractContentAttribute.RealName.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Organization.ToLower())
            {
                styleInfo.DisplayName = "������λ";
            }
            else if (lowerAttributeName == GovInteractContentAttribute.CardType.ToLower())
            {
                styleInfo.DisplayName = "֤������";
                styleInfo.InputType = EInputType.SelectOne;
                ArrayList arraylist = new ArrayList();
                arraylist.Add(new TableStyleItemInfo(0, 0, "���֤", "���֤", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "ѧ��֤", "ѧ��֤", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "����֤", "����֤", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "����֤", "����֤", false));
                styleInfo.StyleItems = arraylist;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.CardNo.ToLower())
            {
                styleInfo.DisplayName = "֤������";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Phone.ToLower())
            {
                styleInfo.DisplayName = "��ϵ�绰";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.PostCode.ToLower())
            {
                styleInfo.DisplayName = "��������";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Integer;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Address.ToLower())
            {
                styleInfo.DisplayName = "��ϵ��ַ";
                styleInfo.IsSingleLine = true;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Email.ToLower())
            {
                styleInfo.DisplayName = "�����ʼ�";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Email;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Fax.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.Additional.ValidateType = EInputValidateType.Integer;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.TypeID.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.InputType = EInputType.SpecifiedValue;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.IsPublic.ToLower())
            {
                styleInfo.DisplayName = "�Ƿ񹫿�";
                styleInfo.InputType = EInputType.Radio;
                ArrayList arraylist = new ArrayList();
                arraylist.Add(new TableStyleItemInfo(0, 0, "����", true.ToString(), true));
                arraylist.Add(new TableStyleItemInfo(0, 0, "������", false.ToString(), false));
                styleInfo.StyleItems = arraylist;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Title.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.IsSingleLine = true;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.IsSingleLine = true;
                styleInfo.InputType = EInputType.TextArea;
                styleInfo.Additional.Width = "90%";
                styleInfo.Additional.Height = 180;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.FileUrl.ToLower())
            {
                styleInfo.DisplayName = "����";
                styleInfo.IsSingleLine = true;
                styleInfo.InputType = EInputType.File;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.DepartmentID.ToLower())
            {
                styleInfo.DisplayName = "�ύ����";
                styleInfo.IsSingleLine = true;
                styleInfo.InputType = EInputType.SpecifiedValue;
            }
            return styleInfo;
        }

        private static TableStyleInfo GetContentHiddenTableStyleInfo(string tableName, string attributeName)
        {
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);
            switch (attributeName)
            {
                case ContentAttribute.ID:
                    styleInfo.DisplayName = "����ID";
                    styleInfo.HelpText = "����ID";
                    break;
                case ContentAttribute.NodeID:
                    styleInfo.DisplayName = "��ĿID";
                    styleInfo.HelpText = "��ĿID";
                    break;
                case ContentAttribute.PublishmentSystemID:
                    styleInfo.DisplayName = "Ӧ��ID";
                    styleInfo.HelpText = "Ӧ��ID";
                    break;
                case ContentAttribute.AddUserName:
                    styleInfo.DisplayName = "�����";
                    styleInfo.HelpText = "�����";
                    break;
                case ContentAttribute.LastEditUserName:
                    styleInfo.DisplayName = "����޸���";
                    styleInfo.HelpText = "����޸���";
                    break;
                case ContentAttribute.LastEditDate:
                    styleInfo.DisplayName = "����޸�ʱ��";
                    styleInfo.HelpText = "����޸�ʱ��";
                    break;
                case ContentAttribute.Taxis:
                    styleInfo.DisplayName = "����";
                    styleInfo.HelpText = "����";
                    break;
                case ContentAttribute.ContentGroupNameCollection:
                    styleInfo.DisplayName = "������";
                    styleInfo.HelpText = "������";
                    break;
                case ContentAttribute.Tags:
                    styleInfo.DisplayName = "��ǩ";
                    styleInfo.HelpText = "��ǩ";
                    break;
                case ContentAttribute.IsChecked:
                    styleInfo.DisplayName = "�Ƿ����ͨ��";
                    styleInfo.HelpText = "�Ƿ����ͨ��";
                    break;
                case ContentAttribute.CheckedLevel:
                    styleInfo.DisplayName = "��˼���";
                    styleInfo.HelpText = "��˼���";
                    break;
                case ContentAttribute.Comments:
                    styleInfo.DisplayName = "������";
                    styleInfo.HelpText = "������";
                    break;
                case ContentAttribute.Photos:
                    styleInfo.DisplayName = "ͼƬ��";
                    styleInfo.HelpText = "ͼƬ��";
                    break;
                case ContentAttribute.Teleplays:
                    styleInfo.DisplayName = "�缯��";
                    styleInfo.HelpText = "�缯��";
                    break;
                case ContentAttribute.Hits:
                    styleInfo.DisplayName = "�����";
                    styleInfo.HelpText = "�����";
                    break;
                case ContentAttribute.HitsByDay:
                    styleInfo.DisplayName = "�յ����";
                    styleInfo.HelpText = "�յ����";
                    break;
                case ContentAttribute.HitsByWeek:
                    styleInfo.DisplayName = "�ܵ����";
                    styleInfo.HelpText = "�ܵ����";
                    break;
                case ContentAttribute.HitsByMonth:
                    styleInfo.DisplayName = "�µ����";
                    styleInfo.HelpText = "�µ����";
                    break;
                case ContentAttribute.LastHitsDate:
                    styleInfo.DisplayName = "�����ʱ��";
                    styleInfo.HelpText = "�����ʱ��";
                    break;
            }
            return styleInfo;
        }

        public static TableStyleInfo GetDefaultTableStyleInfo(string tableName, string attributeName)
        {
            return new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);
        }

        public static bool IsMetadata(ETableStyle tableStyle, string attributeName)
        {
            bool retval = false;
            if (tableStyle == ETableStyle.BackgroundContent)
            {
                retval = BackgroundContentAttribute.AllAttributes.Contains(attributeName.ToLower());
            }
            else if (tableStyle == ETableStyle.Channel)
            {
                retval = ChannelAttribute.HiddenAttributes.Contains(attributeName.ToLower());
            }
            else if (tableStyle == ETableStyle.InputContent)
            {
                retval = InputContentAttribute.AllAttributes.Contains(attributeName.ToLower());
            }
            else if (tableStyle == ETableStyle.User)
            {
                retval = UserAttribute.HiddenAttributes.Contains(attributeName.ToLower());
            }
            return retval;
        }

        public static TableStyleInfo GetUserTableStyleInfo(string tableName, string attributeName)
        {
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);
            ArrayList styleItems = new ArrayList();
            string lowerAttributeName = attributeName.ToLower();
            if (UserAttribute.HiddenAttributes.Contains(lowerAttributeName))
            {
                if (UserAttribute.Cash.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "���";
                }
                else if (UserAttribute.CreateDate.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "ע��ʱ��";
                    styleInfo.InputType = EInputType.DateTime;
                    //styleInfo.IsVisible = false;
                }
                else if (UserAttribute.CreateIPAddress.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "ע��IP";
                    //styleInfo.IsVisible = false;
                }
                else if (UserAttribute.Credits.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "����";
                }
                else if (UserAttribute.IsChecked.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "�Ƿ�ͨ�����";
                    //styleInfo.IsVisible = false;
                    styleInfo.InputType = EInputType.Radio;
                }
                else if (UserAttribute.IsLockedOut.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "�Ƿ�����";
                    styleInfo.IsVisible = true;
                    styleInfo.InputType = EInputType.Radio;
                }
                else if (UserAttribute.IsTemporary.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "�Ƿ���ʱ�û�";
                    //styleInfo.IsVisible = false;
                    styleInfo.InputType = EInputType.Radio;
                }
                else if (UserAttribute.LastActivityDate.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "���ʱ��";
                    //styleInfo.IsVisible = false;
                    styleInfo.InputType = EInputType.DateTime;
                }
                else if (UserAttribute.LevelID.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "�ȼ�ID";
                    styleInfo.IsVisible = true;
                }
                //else if (UserAttribute.PasswordFormat.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "�����ʽ";
                //    styleInfo.HelpText = "�����ʽ";
                //}
                //else if (UserAttribute.PasswordSalt.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "�������";
                //    styleInfo.HelpText = "�������";
                //}
                //else if (UserAttribute.SettingsXML.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "��չ����";
                //    styleInfo.HelpText = "��չ����";
                //}
                //else if (UserAttribute.GroupID.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "�û���ID";
                //    styleInfo.HelpText = "�û���ID";
                //}
                //else if (UserAttribute.GroupSN.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "�û����ϱ�־";
                //    styleInfo.HelpText = "�û����ϱ�־";
                //}
                //else if (UserAttribute.Password.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "����";
                //    styleInfo.HelpText = "����";
                //}
                else if (UserAttribute.UserID.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "�û�ID";
                }
                else if (UserAttribute.UserName.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "�û���";
                    styleInfo.Additional.IsRequired = true;
                    styleInfo.Additional.IsValidate = true;
                }
                else if (UserAttribute.AvatarLarge.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "�û���ͼ";
                    styleInfo.InputType = EInputType.Image;
                }
                else if (UserAttribute.AvatarMiddle.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "�û���ͼ";
                    styleInfo.InputType = EInputType.Image;
                }
                else if (UserAttribute.AvatarSmall.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "�û�Сͼ";
                    styleInfo.InputType = EInputType.Image;
                }
                else if (UserAttribute.DisplayName.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "����";
                }
                else if (UserAttribute.OnlineSeconds.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "����ʱ��";
                    styleInfo.InputType = EInputType.DateTime;
                    //styleInfo.IsVisible = false;
                }
                else if (UserAttribute.Signature.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "����ǩ��";
                    styleInfo.InputType = EInputType.TextArea;
                }
                else if (UserAttribute.LoginNum.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "��¼����";
                }
                //else if (UserAttribute.SCQU.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "SCQU";
                //    styleInfo.IsVisible = false;
                //}
                else if (UserAttribute.Email.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "����";
                    styleInfo.Additional.IsValidate = true;
                    styleInfo.Additional.ValidateType = EInputValidateType.Email;
                }
                else if (UserAttribute.Mobile.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "�ֻ���";
                    styleInfo.Additional.IsValidate = true;
                    styleInfo.Additional.ValidateType = EInputValidateType.Phone;
                }

                return styleInfo;
            }


            if (UserAttribute.Birthday.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��������";
                styleInfo.InputType = EInputType.Date;
            }
            else if (UserAttribute.BloodType.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "Ѫ��";
                styleInfo.InputType = EInputType.SelectOne;
                ArrayList arraylist = new ArrayList();
                arraylist.Add(new TableStyleItemInfo(0, 0, "δ����", "δ����", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "O��", "O", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "B��", "B", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "A��", "A", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "AB��", "AB", false));
                styleInfo.StyleItems = arraylist;
            }
            //else if (UserAttribute.Height.ToLower() == lowerAttributeName)
            //{
            //    styleInfo.DisplayName = "���";
            //    styleInfo.Additional.IsValidate = true;
            //    styleInfo.Additional.IsRequired = true;
            //    styleInfo.Additional.ValidateType = EInputValidateType.Integer;
            //}
            else if (UserAttribute.MaritalStatus.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "����״��";
                styleInfo.InputType = EInputType.SelectOne;
                ArrayList arrayList = new ArrayList();
                arrayList.Add(new TableStyleItemInfo(0, 0, "δ����", "δ����", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "δ��", "δ��", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "�ѻ�", "�ѻ�", false));
                styleInfo.StyleItems = arrayList;
            }
            else if (UserAttribute.Education.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "�����̶�";
                styleInfo.InputType = EInputType.SelectOne;
                ArrayList arrayList = new ArrayList();
                arrayList.Add(new TableStyleItemInfo(0, 0, "δ����", "δ����", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "Сѧ", "Сѧ", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "����", "����", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "����", "����", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "��ѧ", "��ѧ", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "˶ʿ", "˶ʿ", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "��ʿ", "��ʿ", false));
                styleInfo.StyleItems = arrayList;
            }
            else if (UserAttribute.Graduation.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��ҵԺУ";
            }
            else if (UserAttribute.Profession.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��ҵ";
            }
            else if (UserAttribute.Address.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "ͨѶ��ַ";
            }
            else if (UserAttribute.QQ.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "QQ";
            }
            else if (UserAttribute.WeiBo.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "΢��";
            }
            else if (UserAttribute.WeiXin.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "΢��";
            }
            else if (UserAttribute.Interests.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��Ȥ����";
            }
            else if (UserAttribute.Organization.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��˾";
            }
            else if (UserAttribute.Department.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "����";
            }
            else if (UserAttribute.Position.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "ְλ";
            }
            else if (UserAttribute.Gender.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "�Ա�";
                styleInfo.InputType = EInputType.SelectOne;
                ArrayList array = new ArrayList();
                array.Add(new TableStyleItemInfo(0, 0, "δ����", "NotSet", false));
                array.Add(new TableStyleItemInfo(0, 0, "��", "Male", false));
                array.Add(new TableStyleItemInfo(0, 0, "Ů", "Female", false));
                styleInfo.StyleItems = array;

            }

            return styleInfo;
        }

        public static TableStyleInfo GetWebsiteMessageTableStyleInfo(string tableName, string attributeName)
        {
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);
            ArrayList styleItems = new ArrayList();
            string lowerAttributeName = attributeName.ToLower();

            if (WebsiteMessageContentAttribute.Name.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "����";
            }
            else if (WebsiteMessageContentAttribute.Phone.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "�绰";
            }
            else if (WebsiteMessageContentAttribute.Email.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "����";
            }
            else if (WebsiteMessageContentAttribute.Question.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "����";
            }
            else if (WebsiteMessageContentAttribute.Description.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "����";
            }
            else if (WebsiteMessageContentAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�1";
                styleInfo.IsVisible = false;
            }
            else if (WebsiteMessageContentAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�2";
                styleInfo.IsVisible = false;
            }
            else if (WebsiteMessageContentAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�3";
                styleInfo.IsVisible = false;
            }

            return styleInfo;
        }

        /// <summary>
        /// �����ֶ�
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static TableStyleInfo GetEvaluationTableStyleInfo(string tableName, string attributeName)
        {
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);
            ArrayList styleItems = new ArrayList();
            string lowerAttributeName = attributeName.ToLower();

            if (EvaluationContentAttribute.Description.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��������";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (EvaluationContentAttribute.CompositeScore.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "�ۺϵ÷�";
            }
            else if (EvaluationContentAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�1";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�2";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�3";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext4.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�4";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext5.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�5";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext6.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�6";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext7.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�7";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext8.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�8";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext9.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�9";
                styleInfo.IsVisible = false;
            }

            return styleInfo;
        }

        /// <summary>
        /// ���������ֶ�
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static TableStyleInfo GetTrialApplyTableStyleInfo(string tableName, string attributeName)
        {
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);
            ArrayList styleItems = new ArrayList();
            string lowerAttributeName = attributeName.ToLower();

            if (TrialApplyAttribute.Name.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "����";
            }
            else if (TrialApplyAttribute.Phone.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "�绰";
            }
            else if (TrialApplyAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�1";
                styleInfo.IsVisible = false;
            }
            else if (TrialApplyAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�2";
                styleInfo.IsVisible = false;
            }
            else if (TrialApplyAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�3";
                styleInfo.IsVisible = false;
            }

            return styleInfo;
        }

        /// <summary>
        /// ���ñ����ֶ�
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static TableStyleInfo GetTrialReportTableStyleInfo(string tableName, string attributeName)
        {
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);
            ArrayList styleItems = new ArrayList();
            string lowerAttributeName = attributeName.ToLower();

            if (TrialReportAttribute.Description.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��������";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (TrialReportAttribute.CompositeScore.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "�ۺϵ÷�";
            }
            else if (TrialReportAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�1";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�2";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�3";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext4.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�4";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext5.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�5";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext6.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�6";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext7.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�7";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext8.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�8";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext9.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�9";
                styleInfo.IsVisible = false;
            }

            return styleInfo;
        }

        /// <summary>
        /// �����ʾ��ֶ�
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static TableStyleInfo GetSurveyQuestionnaireTableStyleInfo(string tableName, string attributeName)
        {
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);
            ArrayList styleItems = new ArrayList();
            string lowerAttributeName = attributeName.ToLower();

            if (SurveyQuestionnaireAttribute.Description.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��������";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (SurveyQuestionnaireAttribute.CompositeScore.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "�ۺϵ÷�";
            }
            else if (SurveyQuestionnaireAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�1";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�2";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�3";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext4.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�4";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext5.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�5";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext6.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�6";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext7.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�7";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext8.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�8";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext9.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�9";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext10.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�10";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext11.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�11";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext12.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�12";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext13.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�13";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext14.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�14";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext15.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�15";
                styleInfo.IsVisible = false;
            }

            return styleInfo;
        }

        /// <summary>
        /// �ȽϷ����ֶ�
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static TableStyleInfo GetCompareTableStyleInfo(string tableName, string attributeName)
        {
            TableStyleInfo styleInfo = new TableStyleInfo(0, 0, tableName, attributeName, 0, attributeName, string.Empty, true, false, true, EInputType.Text, string.Empty, true, string.Empty);
            ArrayList styleItems = new ArrayList();
            string lowerAttributeName = attributeName.ToLower();

            if (CompareContentAttribute.Description.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��������";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (CompareContentAttribute.CompositeScore1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��Ʒ1�ۺ�����";
            }
            else if (CompareContentAttribute.CompositeScore2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��Ʒ2�ۺ�����";
            }
            else if (CompareContentAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�1";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�2";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�3";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext4.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�4";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext5.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�5";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext6.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�6";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext7.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�7";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext8.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�8";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext9.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "��չ�ֶ�9";
                styleInfo.IsVisible = false;
            }

            return styleInfo;
        }

        public static ArrayList GetHiddenUserTableStyleInfoArrayList()
        {
            ArrayList array = new ArrayList();
            foreach (string attribute in UserAttribute.HiddenAttributes)
            {
                if (string.Equals(attribute, UserAttribute.GroupSN.ToLower())
                    || string.Equals(attribute, UserAttribute.Password.ToLower())
                    || string.Equals(attribute, UserAttribute.PasswordFormat.ToLower())
                    || string.Equals(attribute, UserAttribute.PasswordSalt.ToLower())
                    || string.Equals(attribute, UserAttribute.SettingsXML.ToLower())
                     || string.Equals(attribute, UserAttribute.GroupID.ToLower()))
                    continue;
                array.Add(TableStyleManager.GetUserTableStyleInfo(BaiRongDataProvider.UserDAO.TABLE_NAME, attribute));
            }
            return array;
        }
    }

}
