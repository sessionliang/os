using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using BaiRong.Controls;
using System.Web;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser;
using SiteServer.STL.Parser.StlElement;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Core;
using SiteServer.STL.Parser.StlEntity;
using System.Collections.Generic;

namespace SiteServer.CMS.Services
{
    public class SearchOutput : BasePage
    {
        public Literal HtmlOutput;

        public void Page_Load(object sender, EventArgs E)
        {
            string ajaxDivID = PageUtils.FilterSqlAndXss(base.Request["ajaxDivID"]);
            int pageNum = TranslateUtils.ToInt(base.Request["pageNum"]);
            bool isHighlight = TranslateUtils.ToBool(base.Request["isHighlight"]);
            bool isRedirectSingle = TranslateUtils.ToBool(base.Request["isRedirectSingle"]);
            bool isDefaultDisplay = TranslateUtils.ToBool(base.Request["isDefaultDisplay"]);
            string dateAttribute = base.Request["dateAttribute"];
            if (string.IsNullOrEmpty(dateAttribute))
            {
                dateAttribute = ContentAttribute.AddDate;
            }
            ECharset charset = ECharsetUtils.GetEnumType(base.Request["charset"]);
            int pageIndex = TranslateUtils.ToInt(base.Request["pageIndex"], 0);
            bool isCrossSite = TranslateUtils.ToBool(base.Request.QueryString["isCrossSite"]);

            string successTemplateString = RuntimeUtils.DecryptStringByTranslate(Request["successTemplateString"]);
            string failureTemplateString = RuntimeUtils.DecryptStringByTranslate(Request["failureTemplateString"]);

            string word = PageUtils.UrlDecode(base.Request["word"], charset);
            string channelID = base.Request["channelID"];

            successTemplateString = StlRequestEntities.ParseRequestEntities(base.Request.Form, successTemplateString, charset);
            failureTemplateString = StlRequestEntities.ParseRequestEntities(base.Request.Form, failureTemplateString, charset);

            if (isCrossSite)
            {
                word = PageUtils.UrlDecode(word, charset);
            }
            if (!string.IsNullOrEmpty(word))
            {
                word = PageUtils.FilterSql(word);
            }

            #region update 20151106, 记录搜索关键词
            SearchwordInfo searchwordInfo = DataProvider.SearchwordDAO.GetSearchwordInfo(PublishmentSystemID, word);
            if (searchwordInfo == null)
            {
                //不存在，保存
                searchwordInfo = new SearchwordInfo();
                searchwordInfo.PublishmentSystemID = PublishmentSystemID;
                searchwordInfo.Searchword = word;
                searchwordInfo.SearchCount = 1;
                searchwordInfo.IsEnabled = true;
                DataProvider.SearchwordDAO.Insert(searchwordInfo);
            }
            else
            {
                //存在，搜索次数加1
                searchwordInfo.SearchCount++;
                DataProvider.SearchwordDAO.Update(searchwordInfo);
            }
            #endregion

            #region update 20151106, 根据站内搜索设置搜索范围查询
            SearchwordSettingInfo settingInfo = DataProvider.SearchwordSettingDAO.GetSearchwordSettingInfo(PublishmentSystemID);
            if (settingInfo == null)
                settingInfo = new SearchwordSettingInfo();
            #endregion

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, TranslateUtils.ToInt(channelID, base.PublishmentSystemID));
            if (nodeInfo == null)
            {
                nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, base.PublishmentSystemID);
            }
            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);

            string dateFrom = PageUtils.FilterSqlAndXss(base.Request["dateFrom"]);
            string dateTo = PageUtils.FilterSqlAndXss(base.Request["dateTo"]);
            string date = PageUtils.FilterSqlAndXss(base.Request["date"]);
            StringCollection typeCollection = TranslateUtils.StringCollectionToStringCollection(PageUtils.UrlDecode(PageUtils.FilterSqlAndXss(base.Request["type"])));
            string excludeAttributes = "ajaxdivid,pagenum,pageindex,iscrosssite,ishighlight,isredirectsingle,isdefaultdisplay,charset,successtemplatestring,failuretemplatestring,word,click,channelid,datefrom,dateto,date,type,dateattribute";

            //StringCollection typeCollection = TranslateUtils.StringCollectionToStringCollection(PageUtils.UrlDecode(PageUtils.FilterSqlAndXss(base.Request["type"])));
            //if (typeCollection.Count == 0)
            //{
            //    typeCollection.Add(ContentAttribute.Title);
            //    if (tableStyle == ETableStyle.BackgroundContent)
            //    {
            //        typeCollection.Add(BackgroundContentAttribute.Content);
            //    }
            //    else if (tableStyle == ETableStyle.JobContent)
            //    {
            //        typeCollection.Add(JobContentAttribute.Location);
            //        typeCollection.Add(JobContentAttribute.Department);
            //        typeCollection.Add(JobContentAttribute.Requirement);
            //        typeCollection.Add(JobContentAttribute.Responsibility);
            //    }
            //}

            if (!string.IsNullOrEmpty(successTemplateString))
            {
                TemplateInfo templateInfo = new TemplateInfo(0, base.PublishmentSystemID, string.Empty, ETemplateType.FileTemplate, string.Empty, string.Empty, string.Empty, ECharsetUtils.GetEnumType(base.PublishmentSystemInfo.Additional.Charset), false);

                PageInfo pageInfo = new PageInfo(templateInfo, base.PublishmentSystemID, nodeInfo.NodeID, 0, base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.VisualType);
                ContextInfo contextInfo = new ContextInfo(pageInfo);

                StringBuilder contentBuilder = new StringBuilder(successTemplateString);

                List<string> stlLabelList = StlParserUtility.GetStlLabelList(contentBuilder.ToString());

                if (StlParserUtility.IsStlElementExists(StlPageContents.ElementName, stlLabelList))
                {
                    string stlElement = StlParserUtility.GetStlElement(StlPageContents.ElementName, stlLabelList);
                    string stlPageContentsElement = stlElement;
                    string stlPageContentsElementReplaceString = stlElement;

                    StringBuilder whereBuilder = new StringBuilder();
                    bool isSearch = DataProvider.ContentDAO.SetWhereStringBySearch(whereBuilder, base.PublishmentSystemInfo, nodeInfo.NodeID, tableStyle, word, typeCollection, channelID, dateFrom, dateTo, date, dateAttribute, excludeAttributes, base.Request.Form, charset, settingInfo);

                    //bool isSearch = false;
                    //StringBuilder whereBuilder = new StringBuilder();
                    //whereBuilder.AppendFormat("(PublishmentSystemID = {0}) ", base.PublishmentSystemID);
                    //if (!string.IsNullOrEmpty(word))
                    //{
                    //    whereBuilder.Append(" AND (");
                    //    foreach (string type in typeCollection)
                    //    {
                    //        whereBuilder.AppendFormat("[{0}] like '%{1}%' OR ", type, word);
                    //    }
                    //    whereBuilder.Length = whereBuilder.Length - 3;
                    //    whereBuilder.Append(")");
                    //    isSearch = true;
                    //}
                    //if (channelID > 0)
                    //{
                    //    if (nodeInfo.NodeID != base.PublishmentSystemID)
                    //    {
                    //        whereBuilder.Append(" AND ");
                    //        ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(nodeInfo.NodeID);
                    //        nodeIDArrayList.Add(nodeInfo.NodeID);
                    //        whereBuilder.AppendFormat("(NodeID IN ({0})) ", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList));
                    //    }
                    //    isSearch = true;
                    //}
                    //if (!string.IsNullOrEmpty(dateFrom))
                    //{
                    //    whereBuilder.Append(" AND ");
                    //    whereBuilder.AppendFormat(" {0} >= '{1}' ", dateAttributeName, dateFrom);
                    //    isSearch = true;
                    //}
                    //if (!string.IsNullOrEmpty(dateTo))
                    //{
                    //    whereBuilder.Append(" AND ");
                    //    whereBuilder.AppendFormat(" {0} <= '{1}' ", dateAttributeName, dateTo);
                    //    isSearch = true;
                    //}
                    //if (!string.IsNullOrEmpty(date))
                    //{
                    //    int days = TranslateUtils.ToInt(date);
                    //    if (days > 0)
                    //    {
                    //        whereBuilder.Append(" AND ");
                    //        whereBuilder.AppendFormat("(DATEDIFF([Day], {0}, getdate()) < {1})", dateAttributeName, days);
                    //    }
                    //    isSearch = true;
                    //}

                    //ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeInfo.NodeID);
                    //ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);

                    //ArrayList arraylist = TranslateUtils.StringCollectionToArrayList("ajaxdivid,pagenum,pageindex,iscrosssite,ishighlight,isredirectsingle,isdefaultdisplay,charset,successtemplatestring,failuretemplatestring,word,click,channelid,datefrom,dateto,date,type");
                    //foreach (string key in base.Request.Form.Keys)
                    //{
                    //    if (arraylist.Contains(key.ToLower())) continue;
                    //    if (!string.IsNullOrEmpty(base.Request.Form[key]))
                    //    {
                    //        string value = StringUtils.Trim(PageUtils.UrlDecode(base.Request.Form[key], charset));
                    //        if (!string.IsNullOrEmpty(value))
                    //        {
                    //            if (TableManager.IsAttributeNameExists(tableStyle, tableName, key))
                    //            {
                    //                whereBuilder.Append(" AND ");
                    //                whereBuilder.AppendFormat("([{0}] LIKE '%{1}%')", key, value);
                    //            }
                    //            else
                    //            {
                    //                foreach (TableStyleInfo tableStyleInfo in styleInfoArrayList)
                    //                {
                    //                    if (StringUtils.EqualsIgnoreCase(tableStyleInfo.AttributeName, key))
                    //                    {
                    //                        whereBuilder.Append(" AND ");
                    //                        whereBuilder.AppendFormat("({0} LIKE '%{1}={2}%')", ContentAttribute.SettingsXML, key, value);
                    //                        break;
                    //                    }
                    //                }
                    //            }

                    //            if (tableStyle == ETableStyle.GovPublicContent)
                    //            {
                    //                if (StringUtils.EqualsIgnoreCase(key, GovPublicContentAttribute.DepartmentID))
                    //                {
                    //                    whereBuilder.Append(" AND ");
                    //                    whereBuilder.AppendFormat("([{0}] = {1})", key, TranslateUtils.ToInt(value));
                    //                }
                    //            }
                    //            isSearch = true;
                    //        }
                    //    }
                    //}

                    //没搜索条件时不显示搜索结果
                    if (!isSearch && !isDefaultDisplay)
                    {
                        return;
                    }

                    StlPageContents stlPageContents = new StlPageContents(stlPageContentsElement, pageInfo, contextInfo, pageNum, whereBuilder.ToString());

                    int totalNum = 0;
                    int pageCount = stlPageContents.GetPageCount(out totalNum);

                    if (totalNum == 0)
                    {
                        contentBuilder = new StringBuilder(failureTemplateString);
                    }
                    else
                    {
                        bool isRedirect = false;
                        if (isRedirectSingle && totalNum == 1)
                        {
                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, stlPageContents.SqlString);
                            if (contentInfo != null)
                            {
                                isRedirect = true;
                                contentBuilder = new StringBuilder(string.Format(@"
<script>
location.href = '{0}';
</script>
", PageUtility.GetContentUrl(base.PublishmentSystemInfo, contentInfo, base.PublishmentSystemInfo.Additional.VisualType)));
                            }
                        }
                        if (!isRedirect)
                        {
                            for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                            {
                                if (currentPageIndex == pageIndex)
                                {
                                    string pageHtml = stlPageContents.Parse(totalNum, currentPageIndex, pageCount);
                                    StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageContentsElementReplaceString, pageHtml));

                                    StlParserManager.ReplacePageElementsInSearchPage(pagedBuilder, pageInfo, stlLabelList, ajaxDivID, pageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);

                                    if (isHighlight && !string.IsNullOrEmpty(word))
                                    {
                                        string pagedContents = pagedBuilder.ToString();
                                        pagedBuilder = new StringBuilder();
                                        pagedBuilder.Append(RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><]*>)", word.Replace(" ", "\\s")), pagedContents, string.Format("<span style='color:#cc0000'>{0}</span>", word)));
                                    }

                                    this.WriteResponse(pagedBuilder, pageInfo, contextInfo);
                                    return;
                                }
                            }
                        }
                    }
                }
                else if (StlParserUtility.IsStlElementExists(StlPageSqlContents.ElementName, stlLabelList))
                {
                    int siteID = TranslateUtils.ToInt(base.Request.Form["siteID"], 0);
                    string stlElement = StlParserUtility.GetStlElement(StlPageSqlContents.ElementName, stlLabelList);
                    string stlPageSqlContentsElement = stlElement;
                    string stlPageSqlContentsElementReplaceString = stlElement;

                    StringBuilder whereBuilder = new StringBuilder();
                    if (!string.IsNullOrEmpty(word))
                    {
                        whereBuilder.Append("(");
                        foreach (string type in typeCollection)
                        {
                            whereBuilder.AppendFormat("[{0}] like '%{1}%' OR ", type, word);
                        }
                        whereBuilder.Length = whereBuilder.Length - 3;
                        whereBuilder.Append(")");
                    }
                    if (!string.IsNullOrEmpty(dateFrom))
                    {
                        if (whereBuilder.Length > 0) { whereBuilder.Append(" AND "); }
                        //whereBuilder.AppendFormat("(AddDate >= '{0}' OR LastEditDate >= '{0}') ", dateFrom);
                        whereBuilder.AppendFormat(" AddDate >= '{0}' ", dateFrom);
                    }
                    if (!string.IsNullOrEmpty(dateTo))
                    {
                        if (whereBuilder.Length > 0) { whereBuilder.Append(" AND "); }
                        //whereBuilder.AppendFormat("(AddDate <= '{0}' OR LastEditDate <= '{0}') ", dateTo);
                        whereBuilder.AppendFormat(" AddDate <= '{0}' ", dateTo);
                    }
                    if (!string.IsNullOrEmpty(date))
                    {
                        int days = TranslateUtils.ToInt(date);
                        if (days > 0)
                        {
                            if (whereBuilder.Length > 0) { whereBuilder.Append(" AND "); }
                            whereBuilder.AppendFormat("(DATEDIFF([Day], AddDate, getdate()) < {0})", days);
                        }
                    }
                    if (siteID > 0)
                    {
                        if (whereBuilder.Length > 0) { whereBuilder.Append(" AND "); }
                        whereBuilder.AppendFormat("(PublishmentSystemID = {0})", siteID);
                    }

                    if (whereBuilder.Length > 0) { whereBuilder.Append(" AND "); }
                    whereBuilder.Append("(NodeID > 0) ");

                    tableName = BaiRongDataProvider.TableCollectionDAO.GetFirstTableNameByTableType(EAuxiliaryTableType.BackgroundContent);
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList("ajaxdivid,pagenum,pageindex,iscrosssite,ishighlight,isredirectsingle,isdefaultdisplay,charset,successtemplatestring,failuretemplatestring,word,click,channelid,datefrom,dateto,date,type,siteid");
                    foreach (string key in base.Request.Form.Keys)
                    {
                        if (arraylist.Contains(key.ToLower())) continue;
                        if (!string.IsNullOrEmpty(base.Request.Form[key]))
                        {
                            string value = StringUtils.Trim(PageUtils.UrlDecode(base.Request.Form[key], charset));
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (TableManager.IsAttributeNameExists(tableStyle, tableName, key))
                                {
                                    if (whereBuilder.Length > 0) { whereBuilder.Append(" AND "); }
                                    whereBuilder.AppendFormat("([{0}] like '%{1}%')", key, value);
                                }
                                else
                                {
                                    if (whereBuilder.Length > 0) { whereBuilder.Append(" AND "); }
                                    whereBuilder.AppendFormat("({0} like '%{1}={2}%')", ContentAttribute.SettingsXML, key, value);
                                }
                            }
                        }
                    }

                    //没搜索条件时不显示搜索结果
                    if (whereBuilder.Length == 0 && isDefaultDisplay == false)
                    {
                        return;
                    }

                    StlPageSqlContents stlPageSqlContents = new StlPageSqlContents(stlPageSqlContentsElement, pageInfo, contextInfo, false, false);
                    if (string.IsNullOrEmpty(stlPageSqlContents.DisplayInfo.QueryString))
                    {
                        stlPageSqlContents.DisplayInfo.QueryString = string.Format("SELECT * FROM {0} WHERE {1}", tableName, whereBuilder.ToString());
                    }
                    stlPageSqlContents.LoadData();

                    int totalNum = 0;
                    int pageCount = stlPageSqlContents.GetPageCount(out totalNum);

                    if (totalNum == 0)
                    {
                        contentBuilder = new StringBuilder(failureTemplateString);
                    }
                    else
                    {
                        for (int currentPageIndex = 0; currentPageIndex < pageCount; currentPageIndex++)
                        {
                            if (currentPageIndex == pageIndex)
                            {
                                string pageHtml = stlPageSqlContents.Parse(currentPageIndex, pageCount);
                                StringBuilder pagedBuilder = new StringBuilder(contentBuilder.ToString().Replace(stlPageSqlContentsElementReplaceString, pageHtml));

                                StlParserManager.ReplacePageElementsInSearchPage(pagedBuilder, pageInfo, stlLabelList, ajaxDivID, pageInfo.PageNodeID, currentPageIndex, pageCount, totalNum);

                                if (isHighlight && !string.IsNullOrEmpty(word))
                                {
                                    string pagedContents = pagedBuilder.ToString();
                                    pagedBuilder = new StringBuilder();
                                    pagedBuilder.Append(RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><]*>)", word.Replace(" ", "\\s")), pagedContents, string.Format("<span style='color:#cc0000'>{0}</span>", word)));
                                }

                                this.WriteResponse(pagedBuilder, pageInfo, contextInfo);
                                return;
                            }
                        }
                    }
                }

                this.WriteResponse(contentBuilder, pageInfo, contextInfo);
            }
        }

        private void WriteResponse(StringBuilder contentBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            StlUtility.LoadGeneratePageContent(base.PublishmentSystemInfo, pageInfo, contextInfo, contentBuilder, string.Empty);
            this.HtmlOutput.Text = contentBuilder.ToString();
        }
    }
}
