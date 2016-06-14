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
        private static bool async = true;//缓存与数据库不同步
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
            #region 功能表字段 by 20160304 sofuny
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
            //添加所有实际保存在数据库中的样式
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
                        if (styleInfo.Taxis <= 0 && attributeNames.Contains(styleInfo.AttributeName))//数据库字段
                        {
                            sortedlist.Add(styleInfo.AttributeName, styleInfo);
                        }
                        else if (styleInfo.Taxis > 0)                       //排序字段
                        {
                            string iStr = relatedIdentity.ToString() + styleInfo.Taxis.ToString().PadLeft(3, '0');
                            sortedlist.Add("1" + iStr + "_" + styleInfo.AttributeName, styleInfo);
                        }
                        else                                                //未排序字段
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

        //relatedIdentities从大到小，最后是0
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
                styleInfo.DisplayName = "标题";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.IsFormatString = true;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.SubTitle.ToLower())
            {
                styleInfo.DisplayName = "副标题";
            }
            else if (lowerAttributeName == BackgroundContentAttribute.ImageUrl.ToLower())
            {
                styleInfo.DisplayName = "图片";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.VideoUrl.ToLower())
            {
                styleInfo.DisplayName = "视频";
                styleInfo.InputType = EInputType.Video;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.FileUrl.ToLower())
            {
                styleInfo.DisplayName = "附件";
                styleInfo.InputType = EInputType.File;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.LinkUrl.ToLower())
            {
                styleInfo.DisplayName = "外部链接";
                styleInfo.HelpText = "设置后链接将指向此地址";
            }
            else if (lowerAttributeName == BackgroundContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "内容";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.Author.ToLower())
            {
                styleInfo.DisplayName = "作者";
            }
            else if (lowerAttributeName == BackgroundContentAttribute.Source.ToLower())
            {
                styleInfo.DisplayName = "来源";
            }
            else if (lowerAttributeName == BackgroundContentAttribute.Summary.ToLower())
            {
                styleInfo.DisplayName = "内容摘要";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.IsRecommend.ToLower())
            {
                styleInfo.DisplayName = "推荐";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.IsHot.ToLower())
            {
                styleInfo.DisplayName = "热点";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == BackgroundContentAttribute.IsColor.ToLower())
            {
                styleInfo.DisplayName = "醒目";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "置顶";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "添加日期";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == ContentAttribute.CheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "定时审核日期";
                styleInfo.InputType = EInputType.DateTime;
            }
            else if (lowerAttributeName == ContentAttribute.UnCheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "定时下架日期";
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
                styleInfo.DisplayName = "标题";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.IsFormatString = true;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.Identifier.ToLower())
            {
                styleInfo.DisplayName = "索引号";
            }
            else if (lowerAttributeName == GovPublicContentAttribute.Description.ToLower())
            {
                styleInfo.DisplayName = "内容概述";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.PublishDate.ToLower())
            {
                styleInfo.DisplayName = "发文日期";
                styleInfo.InputType = EInputType.Date;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.EffectDate.ToLower())
            {
                styleInfo.DisplayName = "生效日期";
                styleInfo.InputType = EInputType.Date;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.IsAbolition.ToLower())
            {
                styleInfo.DisplayName = "是否废止";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.AbolitionDate.ToLower())
            {
                styleInfo.DisplayName = "废止日期";
                styleInfo.InputType = EInputType.Date;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.DocumentNo.ToLower())
            {
                styleInfo.DisplayName = "文号";
            }
            else if (lowerAttributeName == GovPublicContentAttribute.Publisher.ToLower())
            {
                styleInfo.DisplayName = "发布机构";
            }
            else if (lowerAttributeName == GovPublicContentAttribute.Keywords.ToLower())
            {
                styleInfo.DisplayName = "关键词";
            }
            else if (lowerAttributeName == GovPublicContentAttribute.ImageUrl.ToLower())
            {
                styleInfo.DisplayName = "图片";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.FileUrl.ToLower())
            {
                styleInfo.InputType = EInputType.File;
                styleInfo.IsSingleLine = true;
                styleInfo.DisplayName = "附件";
            }
            else if (lowerAttributeName == GovPublicContentAttribute.IsRecommend.ToLower())
            {
                styleInfo.DisplayName = "推荐";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.IsHot.ToLower())
            {
                styleInfo.DisplayName = "热点";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.IsColor.ToLower())
            {
                styleInfo.DisplayName = "醒目";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "置顶";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GovPublicContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "内容";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "添加日期";
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
                    styleInfo.DisplayName = "图片投票";
                }
                else if (lowerAttributeName == VoteContentAttribute.IsSummary.ToLower())
                {
                    styleInfo.DisplayName = "显示简介";
                }
                else if (lowerAttributeName == VoteContentAttribute.Participants.ToLower())
                {
                    styleInfo.DisplayName = "参与人数";
                }
                else if (lowerAttributeName == VoteContentAttribute.IsClosed.ToLower())
                {
                    styleInfo.DisplayName = "已结束";
                }
                else if (lowerAttributeName == VoteContentAttribute.IsTop.ToLower())
                {
                    styleInfo.DisplayName = "置顶";
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
                styleInfo.DisplayName = "标题";
                styleInfo.InputType = EInputType.Text;
                styleInfo.Additional.IsFormatString = true;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == VoteContentAttribute.SubTitle.ToLower())
            {
                styleInfo.DisplayName = "副标题";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == VoteContentAttribute.MaxSelectNum.ToLower())
            {
                styleInfo.DisplayName = "单选/多选";
                styleInfo.InputType = EInputType.SelectOne;
            }
            else if (lowerAttributeName == VoteContentAttribute.ImageUrl.ToLower())
            {
                styleInfo.DisplayName = "图片";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == VoteContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "内容";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == VoteContentAttribute.Summary.ToLower())
            {
                styleInfo.DisplayName = "内容摘要";
                styleInfo.InputType = EInputType.TextArea;
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == VoteContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "开始日期";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == VoteContentAttribute.EndDate.ToLower())
            {
                styleInfo.DisplayName = "截止日期";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == VoteContentAttribute.IsVotedView.ToLower())
            {
                styleInfo.DisplayName = "投票结果";
                styleInfo.InputType = EInputType.Radio;
            }
            else if (lowerAttributeName == VoteContentAttribute.HiddenContent.ToLower())
            {
                styleInfo.DisplayName = "隐藏内容";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == ContentAttribute.CheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "定时审核日期";
                styleInfo.InputType = EInputType.DateTime;
            }
            else if (lowerAttributeName == ContentAttribute.UnCheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "定时下架日期";
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
                styleInfo.DisplayName = "标题";
                styleInfo.HelpText = "标题的名称";
                styleInfo.InputType = EInputType.Text;
                styleInfo.Additional.IsFormatString = true;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == JobContentAttribute.Department.ToLower())
            {
                styleInfo.DisplayName = "所属部门";
                styleInfo.HelpText = "所属部门";
                styleInfo.InputType = EInputType.Text;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == JobContentAttribute.Location.ToLower())
            {
                styleInfo.DisplayName = "工作地点";
                styleInfo.HelpText = "工作地点";
                styleInfo.InputType = EInputType.Text;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == JobContentAttribute.NumberOfPeople.ToLower())
            {
                styleInfo.DisplayName = "招聘人数";
                styleInfo.HelpText = "招聘人数";
                styleInfo.InputType = EInputType.Text;
                styleInfo.DefaultValue = "不限";
                styleInfo.Additional.Width = "60px";
            }
            else if (lowerAttributeName == JobContentAttribute.Responsibility.ToLower())
            {
                styleInfo.DisplayName = "工作职责";
                styleInfo.HelpText = "工作职责";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == JobContentAttribute.Requirement.ToLower())
            {
                styleInfo.DisplayName = "工作要求";
                styleInfo.HelpText = "工作要求";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == JobContentAttribute.IsUrgent.ToLower())
            {
                styleInfo.DisplayName = "是否急聘";
                styleInfo.HelpText = "是否急聘";
                styleInfo.InputType = EInputType.CheckBox;
                itemInfo = new TableStyleItemInfo(0, styleInfo.TableStyleID, "急聘", true.ToString(), false);
                styleItems.Add(itemInfo);
                styleInfo.StyleItems = styleItems;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "置顶";
                styleInfo.HelpText = "是否为置顶内容";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "添加日期";
                styleInfo.HelpText = "内容的添加日期";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == ContentAttribute.CheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "定时审核日期";
                styleInfo.InputType = EInputType.DateTime;
            }
            else if (lowerAttributeName == ContentAttribute.UnCheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "定时下架日期";
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
                styleInfo.DisplayName = "商品名称";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.IsFormatString = true;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "置顶";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "添加日期";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == GoodsContentAttribute.SN.ToLower())
            {
                styleInfo.DisplayName = "商品编号";
            }
            else if (lowerAttributeName == GoodsContentAttribute.Keywords.ToLower())
            {
                styleInfo.DisplayName = "商品关键词";
                styleInfo.HelpText = "用于在前台、后台筛选商品，多个关键词用半角逗号,分开";
            }
            else if (lowerAttributeName == GoodsContentAttribute.Summary.ToLower())
            {
                styleInfo.DisplayName = "商品简介";
                styleInfo.InputType = EInputType.TextArea;
                styleInfo.Additional.Height = 50;
            }
            else if (lowerAttributeName == GoodsContentAttribute.ImageUrl.ToLower())
            {
                styleInfo.DisplayName = "商品图片";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == GoodsContentAttribute.ThumbUrl.ToLower())
            {
                styleInfo.DisplayName = "商品缩略图";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == GoodsContentAttribute.LinkUrl.ToLower())
            {
                styleInfo.DisplayName = "外部链接";
                styleInfo.HelpText = "设置后链接将指向此地址";
            }
            else if (lowerAttributeName == GoodsContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "商品内容";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == GoodsContentAttribute.PriceCost.ToLower())
            {
                styleInfo.DisplayName = "成本价";
                styleInfo.HelpText = "仅供后台使用";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Currency;
                styleInfo.Additional.Width = "80px";
            }
            else if (lowerAttributeName == GoodsContentAttribute.PriceMarket.ToLower())
            {
                styleInfo.DisplayName = "市场价";
                styleInfo.HelpText = "商品的市场价格";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Currency;
                styleInfo.Additional.Width = "80px";
            }
            else if (lowerAttributeName == GoodsContentAttribute.PriceSale.ToLower())
            {
                styleInfo.DisplayName = "销售价";
                styleInfo.HelpText = "实际销售的价格";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Currency;
                styleInfo.Additional.Width = "80px";
            }
            else if (lowerAttributeName == GoodsContentAttribute.Stock.ToLower())
            {
                styleInfo.DisplayName = "库存量";
                styleInfo.HelpText = "可销售的商品数量(-1代表不限制,0代表已售罄)";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Integer;
                styleInfo.Additional.Width = "80px";
                styleInfo.DefaultValue = "-1";
            }
            else if (lowerAttributeName == GoodsContentAttribute.FileUrl.ToLower())
            {
                styleInfo.DisplayName = "附件";
                styleInfo.InputType = EInputType.File;
            }
            else if (lowerAttributeName == GoodsContentAttribute.IsRecommend.ToLower())
            {
                styleInfo.DisplayName = "推荐";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GoodsContentAttribute.IsNew.ToLower())
            {
                styleInfo.DisplayName = "新品";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GoodsContentAttribute.IsHot.ToLower())
            {
                styleInfo.DisplayName = "热销";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GoodsContentAttribute.IsOnSale.ToLower())
            {
                styleInfo.DisplayName = "上架销售";
                styleInfo.HelpText = "打勾表示允许销售，否则不允许销售";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F1.ToLower())
            {
                styleInfo.DisplayName = "扩展字段1";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F2.ToLower())
            {
                styleInfo.DisplayName = "扩展字段2";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F3.ToLower())
            {
                styleInfo.DisplayName = "扩展字段3";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F4.ToLower())
            {
                styleInfo.DisplayName = "扩展字段4";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F5.ToLower())
            {
                styleInfo.DisplayName = "扩展字段5";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F6.ToLower())
            {
                styleInfo.DisplayName = "扩展字段6";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F7.ToLower())
            {
                styleInfo.DisplayName = "扩展字段7";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F8.ToLower())
            {
                styleInfo.DisplayName = "扩展字段8";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == GoodsContentAttribute.F9.ToLower())
            {
                styleInfo.DisplayName = "扩展字段9";
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
                styleInfo.DisplayName = "定时审核日期";
                styleInfo.InputType = EInputType.DateTime;
            }
            else if (lowerAttributeName == ContentAttribute.UnCheckTaskDate.ToLower())
            {
                styleInfo.DisplayName = "定时下架日期";
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
                styleInfo.DisplayName = "品牌名称";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.IsFormatString = true;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "置顶";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "添加日期";
                styleInfo.InputType = EInputType.DateTime;
                styleInfo.DefaultValue = TableInputParser.CURRENT;
            }
            else if (lowerAttributeName == BrandContentAttribute.BrandUrl.ToLower())
            {
                styleInfo.DisplayName = "品牌网址";
            }
            else if (lowerAttributeName == BrandContentAttribute.ImageUrl.ToLower())
            {
                styleInfo.DisplayName = "图片";
                styleInfo.InputType = EInputType.Image;
            }
            else if (lowerAttributeName == BrandContentAttribute.LinkUrl.ToLower())
            {
                styleInfo.DisplayName = "外部链接";
                styleInfo.HelpText = "设置后链接将指向此地址";
            }
            else if (lowerAttributeName == BrandContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "品牌内容";
                styleInfo.InputType = EInputType.TextEditor;
            }
            else if (lowerAttributeName == BrandContentAttribute.IsRecommend.ToLower())
            {
                styleInfo.DisplayName = "推荐";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == BrandContentAttribute.F1.ToLower())
            {
                styleInfo.DisplayName = "扩展字段1";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == BrandContentAttribute.F2.ToLower())
            {
                styleInfo.DisplayName = "扩展字段2";
                styleInfo.IsVisible = false;
            }
            else if (lowerAttributeName == BrandContentAttribute.F3.ToLower())
            {
                styleInfo.DisplayName = "扩展字段3";
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
                styleInfo.DisplayName = "标题";
                styleInfo.HelpText = "标题的名称";
                styleInfo.InputType = EInputType.Text;
                styleInfo.Additional.IsFormatString = true;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == ContentAttribute.IsTop.ToLower())
            {
                styleInfo.DisplayName = "置顶";
                styleInfo.HelpText = "是否为置顶内容";
                styleInfo.InputType = EInputType.CheckBox;
            }
            else if (lowerAttributeName == ContentAttribute.AddDate.ToLower())
            {
                styleInfo.DisplayName = "添加日期";
                styleInfo.HelpText = "内容的添加日期";
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
                styleInfo.DisplayName = "姓名";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Organization.ToLower())
            {
                styleInfo.DisplayName = "工作单位";
            }
            else if (lowerAttributeName == GovInteractContentAttribute.CardType.ToLower())
            {
                styleInfo.DisplayName = "证件名称";
                styleInfo.InputType = EInputType.SelectOne;
                ArrayList arraylist = new ArrayList();
                arraylist.Add(new TableStyleItemInfo(0, 0, "身份证", "身份证", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "学生证", "学生证", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "军官证", "军官证", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "工作证", "工作证", false));
                styleInfo.StyleItems = arraylist;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.CardNo.ToLower())
            {
                styleInfo.DisplayName = "证件号码";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Phone.ToLower())
            {
                styleInfo.DisplayName = "联系电话";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.PostCode.ToLower())
            {
                styleInfo.DisplayName = "邮政编码";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Integer;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Address.ToLower())
            {
                styleInfo.DisplayName = "联系地址";
                styleInfo.IsSingleLine = true;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Email.ToLower())
            {
                styleInfo.DisplayName = "电子邮件";
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
                styleInfo.Additional.ValidateType = EInputValidateType.Email;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Fax.ToLower())
            {
                styleInfo.DisplayName = "传真";
                styleInfo.Additional.ValidateType = EInputValidateType.Integer;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.TypeID.ToLower())
            {
                styleInfo.DisplayName = "类型";
                styleInfo.InputType = EInputType.SpecifiedValue;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.IsPublic.ToLower())
            {
                styleInfo.DisplayName = "是否公开";
                styleInfo.InputType = EInputType.Radio;
                ArrayList arraylist = new ArrayList();
                arraylist.Add(new TableStyleItemInfo(0, 0, "公开", true.ToString(), true));
                arraylist.Add(new TableStyleItemInfo(0, 0, "不公开", false.ToString(), false));
                styleInfo.StyleItems = arraylist;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Title.ToLower())
            {
                styleInfo.DisplayName = "标题";
                styleInfo.IsSingleLine = true;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.Content.ToLower())
            {
                styleInfo.DisplayName = "内容";
                styleInfo.IsSingleLine = true;
                styleInfo.InputType = EInputType.TextArea;
                styleInfo.Additional.Width = "90%";
                styleInfo.Additional.Height = 180;
                styleInfo.Additional.IsValidate = true;
                styleInfo.Additional.IsRequired = true;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.FileUrl.ToLower())
            {
                styleInfo.DisplayName = "附件";
                styleInfo.IsSingleLine = true;
                styleInfo.InputType = EInputType.File;
            }
            else if (lowerAttributeName == GovInteractContentAttribute.DepartmentID.ToLower())
            {
                styleInfo.DisplayName = "提交对象";
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
                    styleInfo.DisplayName = "内容ID";
                    styleInfo.HelpText = "内容ID";
                    break;
                case ContentAttribute.NodeID:
                    styleInfo.DisplayName = "栏目ID";
                    styleInfo.HelpText = "栏目ID";
                    break;
                case ContentAttribute.PublishmentSystemID:
                    styleInfo.DisplayName = "应用ID";
                    styleInfo.HelpText = "应用ID";
                    break;
                case ContentAttribute.AddUserName:
                    styleInfo.DisplayName = "添加者";
                    styleInfo.HelpText = "添加者";
                    break;
                case ContentAttribute.LastEditUserName:
                    styleInfo.DisplayName = "最后修改者";
                    styleInfo.HelpText = "最后修改者";
                    break;
                case ContentAttribute.LastEditDate:
                    styleInfo.DisplayName = "最后修改时间";
                    styleInfo.HelpText = "最后修改时间";
                    break;
                case ContentAttribute.Taxis:
                    styleInfo.DisplayName = "排序";
                    styleInfo.HelpText = "排序";
                    break;
                case ContentAttribute.ContentGroupNameCollection:
                    styleInfo.DisplayName = "内容组";
                    styleInfo.HelpText = "内容组";
                    break;
                case ContentAttribute.Tags:
                    styleInfo.DisplayName = "标签";
                    styleInfo.HelpText = "标签";
                    break;
                case ContentAttribute.IsChecked:
                    styleInfo.DisplayName = "是否审核通过";
                    styleInfo.HelpText = "是否审核通过";
                    break;
                case ContentAttribute.CheckedLevel:
                    styleInfo.DisplayName = "审核级别";
                    styleInfo.HelpText = "审核级别";
                    break;
                case ContentAttribute.Comments:
                    styleInfo.DisplayName = "评论数";
                    styleInfo.HelpText = "评论数";
                    break;
                case ContentAttribute.Photos:
                    styleInfo.DisplayName = "图片数";
                    styleInfo.HelpText = "图片数";
                    break;
                case ContentAttribute.Teleplays:
                    styleInfo.DisplayName = "剧集数";
                    styleInfo.HelpText = "剧集数";
                    break;
                case ContentAttribute.Hits:
                    styleInfo.DisplayName = "点击量";
                    styleInfo.HelpText = "点击量";
                    break;
                case ContentAttribute.HitsByDay:
                    styleInfo.DisplayName = "日点击量";
                    styleInfo.HelpText = "日点击量";
                    break;
                case ContentAttribute.HitsByWeek:
                    styleInfo.DisplayName = "周点击量";
                    styleInfo.HelpText = "周点击量";
                    break;
                case ContentAttribute.HitsByMonth:
                    styleInfo.DisplayName = "月点击量";
                    styleInfo.HelpText = "月点击量";
                    break;
                case ContentAttribute.LastHitsDate:
                    styleInfo.DisplayName = "最后点击时间";
                    styleInfo.HelpText = "最后点击时间";
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
                    styleInfo.DisplayName = "金币";
                }
                else if (UserAttribute.CreateDate.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "注册时间";
                    styleInfo.InputType = EInputType.DateTime;
                    //styleInfo.IsVisible = false;
                }
                else if (UserAttribute.CreateIPAddress.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "注册IP";
                    //styleInfo.IsVisible = false;
                }
                else if (UserAttribute.Credits.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "积分";
                }
                else if (UserAttribute.IsChecked.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "是否通过审核";
                    //styleInfo.IsVisible = false;
                    styleInfo.InputType = EInputType.Radio;
                }
                else if (UserAttribute.IsLockedOut.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "是否锁定";
                    styleInfo.IsVisible = true;
                    styleInfo.InputType = EInputType.Radio;
                }
                else if (UserAttribute.IsTemporary.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "是否临时用户";
                    //styleInfo.IsVisible = false;
                    styleInfo.InputType = EInputType.Radio;
                }
                else if (UserAttribute.LastActivityDate.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "最后活动时间";
                    //styleInfo.IsVisible = false;
                    styleInfo.InputType = EInputType.DateTime;
                }
                else if (UserAttribute.LevelID.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "等级ID";
                    styleInfo.IsVisible = true;
                }
                //else if (UserAttribute.PasswordFormat.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "密码格式";
                //    styleInfo.HelpText = "密码格式";
                //}
                //else if (UserAttribute.PasswordSalt.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "密码混淆";
                //    styleInfo.HelpText = "密码混淆";
                //}
                //else if (UserAttribute.SettingsXML.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "扩展属性";
                //    styleInfo.HelpText = "扩展属性";
                //}
                //else if (UserAttribute.GroupID.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "用户组ID";
                //    styleInfo.HelpText = "用户组ID";
                //}
                //else if (UserAttribute.GroupSN.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "用户集合标志";
                //    styleInfo.HelpText = "用户集合标志";
                //}
                //else if (UserAttribute.Password.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "密码";
                //    styleInfo.HelpText = "密码";
                //}
                else if (UserAttribute.UserID.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "用户ID";
                }
                else if (UserAttribute.UserName.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "用户名";
                    styleInfo.Additional.IsRequired = true;
                    styleInfo.Additional.IsValidate = true;
                }
                else if (UserAttribute.AvatarLarge.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "用户大图";
                    styleInfo.InputType = EInputType.Image;
                }
                else if (UserAttribute.AvatarMiddle.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "用户中图";
                    styleInfo.InputType = EInputType.Image;
                }
                else if (UserAttribute.AvatarSmall.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "用户小图";
                    styleInfo.InputType = EInputType.Image;
                }
                else if (UserAttribute.DisplayName.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "别名";
                }
                else if (UserAttribute.OnlineSeconds.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "在线时长";
                    styleInfo.InputType = EInputType.DateTime;
                    //styleInfo.IsVisible = false;
                }
                else if (UserAttribute.Signature.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "个性签名";
                    styleInfo.InputType = EInputType.TextArea;
                }
                else if (UserAttribute.LoginNum.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "登录次数";
                }
                //else if (UserAttribute.SCQU.ToLower() == lowerAttributeName)
                //{
                //    styleInfo.DisplayName = "SCQU";
                //    styleInfo.IsVisible = false;
                //}
                else if (UserAttribute.Email.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "邮箱";
                    styleInfo.Additional.IsValidate = true;
                    styleInfo.Additional.ValidateType = EInputValidateType.Email;
                }
                else if (UserAttribute.Mobile.ToLower() == lowerAttributeName)
                {
                    styleInfo.DisplayName = "手机号";
                    styleInfo.Additional.IsValidate = true;
                    styleInfo.Additional.ValidateType = EInputValidateType.Phone;
                }

                return styleInfo;
            }


            if (UserAttribute.Birthday.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "出生年月";
                styleInfo.InputType = EInputType.Date;
            }
            else if (UserAttribute.BloodType.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "血型";
                styleInfo.InputType = EInputType.SelectOne;
                ArrayList arraylist = new ArrayList();
                arraylist.Add(new TableStyleItemInfo(0, 0, "未设置", "未设置", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "O型", "O", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "B型", "B", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "A型", "A", false));
                arraylist.Add(new TableStyleItemInfo(0, 0, "AB型", "AB", false));
                styleInfo.StyleItems = arraylist;
            }
            //else if (UserAttribute.Height.ToLower() == lowerAttributeName)
            //{
            //    styleInfo.DisplayName = "身高";
            //    styleInfo.Additional.IsValidate = true;
            //    styleInfo.Additional.IsRequired = true;
            //    styleInfo.Additional.ValidateType = EInputValidateType.Integer;
            //}
            else if (UserAttribute.MaritalStatus.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "婚姻状况";
                styleInfo.InputType = EInputType.SelectOne;
                ArrayList arrayList = new ArrayList();
                arrayList.Add(new TableStyleItemInfo(0, 0, "未设置", "未设置", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "未婚", "未婚", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "已婚", "已婚", false));
                styleInfo.StyleItems = arrayList;
            }
            else if (UserAttribute.Education.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "教育程度";
                styleInfo.InputType = EInputType.SelectOne;
                ArrayList arrayList = new ArrayList();
                arrayList.Add(new TableStyleItemInfo(0, 0, "未设置", "未设置", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "小学", "小学", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "初中", "初中", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "高中", "高中", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "大学", "大学", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "硕士", "硕士", false));
                arrayList.Add(new TableStyleItemInfo(0, 0, "博士", "博士", false));
                styleInfo.StyleItems = arrayList;
            }
            else if (UserAttribute.Graduation.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "毕业院校";
            }
            else if (UserAttribute.Profession.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "行业";
            }
            else if (UserAttribute.Address.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "通讯地址";
            }
            else if (UserAttribute.QQ.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "QQ";
            }
            else if (UserAttribute.WeiBo.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "微博";
            }
            else if (UserAttribute.WeiXin.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "微信";
            }
            else if (UserAttribute.Interests.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "兴趣爱好";
            }
            else if (UserAttribute.Organization.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "公司";
            }
            else if (UserAttribute.Department.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "部门";
            }
            else if (UserAttribute.Position.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "职位";
            }
            else if (UserAttribute.Gender.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "性别";
                styleInfo.InputType = EInputType.SelectOne;
                ArrayList array = new ArrayList();
                array.Add(new TableStyleItemInfo(0, 0, "未设置", "NotSet", false));
                array.Add(new TableStyleItemInfo(0, 0, "男", "Male", false));
                array.Add(new TableStyleItemInfo(0, 0, "女", "Female", false));
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
                styleInfo.DisplayName = "姓名";
            }
            else if (WebsiteMessageContentAttribute.Phone.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "电话";
            }
            else if (WebsiteMessageContentAttribute.Email.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "邮箱";
            }
            else if (WebsiteMessageContentAttribute.Question.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "疑问";
            }
            else if (WebsiteMessageContentAttribute.Description.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "描述";
            }
            else if (WebsiteMessageContentAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段1";
                styleInfo.IsVisible = false;
            }
            else if (WebsiteMessageContentAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段2";
                styleInfo.IsVisible = false;
            }
            else if (WebsiteMessageContentAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段3";
                styleInfo.IsVisible = false;
            }

            return styleInfo;
        }

        /// <summary>
        /// 评价字段
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
                styleInfo.DisplayName = "评价内容";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (EvaluationContentAttribute.CompositeScore.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "综合得分";
            }
            else if (EvaluationContentAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段1";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段2";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段3";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext4.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段4";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext5.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段5";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext6.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段6";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext7.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段7";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext8.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段8";
                styleInfo.IsVisible = false;
            }
            else if (EvaluationContentAttribute.Ext9.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段9";
                styleInfo.IsVisible = false;
            }

            return styleInfo;
        }

        /// <summary>
        /// 试用申请字段
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
                styleInfo.DisplayName = "姓名";
            }
            else if (TrialApplyAttribute.Phone.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "电话";
            }
            else if (TrialApplyAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段1";
                styleInfo.IsVisible = false;
            }
            else if (TrialApplyAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段2";
                styleInfo.IsVisible = false;
            }
            else if (TrialApplyAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段3";
                styleInfo.IsVisible = false;
            }

            return styleInfo;
        }

        /// <summary>
        /// 试用报告字段
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
                styleInfo.DisplayName = "试用描述";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (TrialReportAttribute.CompositeScore.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "综合得分";
            }
            else if (TrialReportAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段1";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段2";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段3";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext4.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段4";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext5.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段5";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext6.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段6";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext7.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段7";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext8.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段8";
                styleInfo.IsVisible = false;
            }
            else if (TrialReportAttribute.Ext9.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段9";
                styleInfo.IsVisible = false;
            }

            return styleInfo;
        }

        /// <summary>
        /// 调查问卷字段
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
                styleInfo.DisplayName = "反馈描述";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (SurveyQuestionnaireAttribute.CompositeScore.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "综合得分";
            }
            else if (SurveyQuestionnaireAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段1";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段2";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段3";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext4.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段4";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext5.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段5";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext6.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段6";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext7.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段7";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext8.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段8";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext9.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段9";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext10.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段10";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext11.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段11";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext12.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段12";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext13.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段13";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext14.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段14";
                styleInfo.IsVisible = false;
            }
            else if (SurveyQuestionnaireAttribute.Ext15.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段15";
                styleInfo.IsVisible = false;
            }

            return styleInfo;
        }

        /// <summary>
        /// 比较反馈字段
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
                styleInfo.DisplayName = "反馈描述";
                styleInfo.InputType = EInputType.TextArea;
            }
            else if (CompareContentAttribute.CompositeScore1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "产品1综合评分";
            }
            else if (CompareContentAttribute.CompositeScore2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "产品2综合评分";
            }
            else if (CompareContentAttribute.Ext1.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段1";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext2.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段2";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext3.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段3";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext4.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段4";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext5.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段5";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext6.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段6";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext7.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段7";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext8.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段8";
                styleInfo.IsVisible = false;
            }
            else if (CompareContentAttribute.Ext9.ToLower() == lowerAttributeName)
            {
                styleInfo.DisplayName = "扩展字段9";
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
