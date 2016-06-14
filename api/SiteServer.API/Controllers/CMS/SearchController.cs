using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Net;
using BaiRong.Model;
using SiteServer.API.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.IO;
using SiteServer.STL.Parser;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser.StlEntity;
using SiteServer.STL.StlTemplate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SiteServer.API.Controllers.CMS
{
    [RoutePrefix("api/services/search")]
    public class SearchController : ApiController
    {
        private int PublishmentSystemID;
        private PublishmentSystemInfo PublishmentSystemInfo;
        private string ResutlString;

        [HttpPost]
        [Route("output")]
        public HttpResponseMessage Output()
        {
            var response = new HttpResponseMessage();

            PublishmentSystemID = TranslateUtils.ToInt(RequestUtils.GetQueryString("PublishmentSystemID"));
            PublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(PublishmentSystemID);
            bool isCrossDomain = PageUtility.IsCrossDomain(PublishmentSystemInfo);

            if (PublishmentSystemInfo != null)
            {

                string ajaxDivID = PageUtils.FilterSqlAndXss(RequestUtils.GetRequestString("ajaxDivID"));
                int pageNum = TranslateUtils.ToInt(RequestUtils.GetRequestString("pageNum"));
                bool isHighlight = TranslateUtils.ToBool(RequestUtils.GetRequestString("isHighlight"));
                bool isRedirectSingle = TranslateUtils.ToBool(RequestUtils.GetRequestString("isRedirectSingle"));
                bool isDefaultDisplay = TranslateUtils.ToBool(RequestUtils.GetRequestString("isDefaultDisplay"));
                string dateAttribute = RequestUtils.GetRequestString("dateAttribute");
                if (string.IsNullOrEmpty(dateAttribute))
                {
                    dateAttribute = ContentAttribute.AddDate;
                }
                ECharset charset = ECharsetUtils.GetEnumType(RequestUtils.GetRequestString("charset"));
                int pageIndex = TranslateUtils.ToInt(RequestUtils.GetRequestString("pageIndex"), 0);
                bool isCrossSite = TranslateUtils.ToBool(RequestUtils.GetRequestString("isCrossSite"));

                string successTemplateString = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetRequestString("successTemplateString"));
                string failureTemplateString = RuntimeUtils.DecryptStringByTranslate(RequestUtils.GetRequestString("failureTemplateString"));

                string word = PageUtils.UrlDecode(RequestUtils.GetRequestString("word"), charset);
                string channelID = RequestUtils.GetRequestString("channelID");

                successTemplateString = StlRequestEntities.ParseRequestEntities(HttpContext.Current.Request.Form, successTemplateString, charset);
                failureTemplateString = StlRequestEntities.ParseRequestEntities(HttpContext.Current.Request.Form, failureTemplateString, charset);

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

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(PublishmentSystemID, TranslateUtils.ToInt(channelID, PublishmentSystemID));
                if (nodeInfo == null)
                {
                    nodeInfo = NodeManager.GetNodeInfo(PublishmentSystemID, PublishmentSystemID);
                }
                ETableStyle tableStyle = NodeManager.GetTableStyle(PublishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(PublishmentSystemInfo, nodeInfo);

                string dateFrom = PageUtils.FilterSqlAndXss(RequestUtils.GetRequestString("dateFrom"));
                string dateTo = PageUtils.FilterSqlAndXss(RequestUtils.GetRequestString("dateTo"));
                string date = PageUtils.FilterSqlAndXss(RequestUtils.GetRequestString("date"));
                StringCollection typeCollection = TranslateUtils.StringCollectionToStringCollection(PageUtils.UrlDecode(PageUtils.FilterSqlAndXss(RequestUtils.GetRequestString("type"))));
                string excludeAttributes = "ajaxdivid,pagenum,pageindex,iscrosssite,ishighlight,isredirectsingle,isdefaultdisplay,charset,successtemplatestring,failuretemplatestring,word,click,channelid,datefrom,dateto,date,type,dateattribute";

                //StringCollection typeCollection = TranslateUtils.StringCollectionToStringCollection(PageUtils.UrlDecode(PageUtils.FilterSqlAndXss(RequestUtils.GetRequestString("type"))));
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
                    TemplateInfo templateInfo = new TemplateInfo(0, PublishmentSystemID, string.Empty, ETemplateType.FileTemplate, string.Empty, string.Empty, string.Empty, ECharsetUtils.GetEnumType(PublishmentSystemInfo.Additional.Charset), false);

                    PageInfo pageInfo = new PageInfo(templateInfo, PublishmentSystemID, nodeInfo.NodeID, 0, PublishmentSystemInfo, PublishmentSystemInfo.Additional.VisualType);
                    ContextInfo contextInfo = new ContextInfo(pageInfo);

                    StringBuilder contentBuilder = new StringBuilder(successTemplateString);

                    List<string> stlLabelList = StlParserUtility.GetStlLabelList(contentBuilder.ToString());

                    if (StlParserUtility.IsStlElementExists(StlPageContents.ElementName, stlLabelList))
                    {
                        string stlElement = StlParserUtility.GetStlElement(StlPageContents.ElementName, stlLabelList);
                        string stlPageContentsElement = stlElement;
                        string stlPageContentsElementReplaceString = stlElement;

                        StringBuilder whereBuilder = new StringBuilder();
                        bool isSearch = DataProvider.ContentDAO.SetWhereStringBySearch(whereBuilder, PublishmentSystemInfo, nodeInfo.NodeID, tableStyle, word, typeCollection, channelID, dateFrom, dateTo, date, dateAttribute, excludeAttributes, HttpContext.Current.Request.Form, charset, settingInfo);


                        //没搜索条件时不显示搜索结果
                        if (!isSearch && !isDefaultDisplay)
                        {
                            ResutlString = this.WriteResponse(contentBuilder, pageInfo, contextInfo);
                            //模拟原有aspx页面response.write，保证前台js统一处理返回数据
                            //api默认返回的数据是string或者json
                            //update by sessionliang at 20151221
                            response.Content = new StringContent(ResutlString);
                            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                            return response;
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
", PageUtility.GetContentUrl(PublishmentSystemInfo, contentInfo, PublishmentSystemInfo.Additional.VisualType)));
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
                                            pagedBuilder.Append(RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><)*>)", word.Replace(" ", "\\s")), pagedContents, string.Format("<span style='color:#cc0000'>{0}</span>", word)));
                                        }
                                        ResutlString = this.WriteResponse(pagedBuilder, pageInfo, contextInfo);
                                        //模拟原有aspx页面response.write，保证前台js统一处理返回数据
                                        //api默认返回的数据是string或者json
                                        //update by sessionliang at 20151221
                                        response.Content = new StringContent(ResutlString);
                                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                                        return response;
                                    }
                                }
                            }
                        }
                    }
                    else if (StlParserUtility.IsStlElementExists(StlPageSqlContents.ElementName, stlLabelList))
                    {
                        int siteID = TranslateUtils.ToInt(HttpContext.Current.Request.Form["siteID"], 0);
                        string stlElement = StlParserUtility.GetStlElement(StlPageSqlContents.ElementName, stlLabelList);
                        string stlPageSqlContentsElement = stlElement;
                        string stlPageSqlContentsElementReplaceString = stlElement;

                        StringBuilder whereBuilder = new StringBuilder();
                        if (!string.IsNullOrEmpty(word))
                        {
                            whereBuilder.Append("(");
                            foreach (string type in typeCollection)
                            {
                                whereBuilder.AppendFormat("[{0}) like '%{1}%' OR ", type, word);
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
                                whereBuilder.AppendFormat("(DATEDIFF([Day), AddDate, getdate()) < {0})", days);
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
                        foreach (string key in HttpContext.Current.Request.Form.Keys)
                        {
                            if (arraylist.Contains(key.ToLower())) continue;
                            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form[key]))
                            {
                                string value = StringUtils.Trim(PageUtils.UrlDecode(HttpContext.Current.Request.Form[key], charset));
                                if (!string.IsNullOrEmpty(value))
                                {
                                    if (TableManager.IsAttributeNameExists(tableStyle, tableName, key))
                                    {
                                        if (whereBuilder.Length > 0) { whereBuilder.Append(" AND "); }
                                        whereBuilder.AppendFormat("([{0}) like '%{1}%')", key, value);
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
                            ResutlString = string.Empty;
                            //模拟原有aspx页面response.write，保证前台js统一处理返回数据
                            //api默认返回的数据是string或者json
                            //update by sessionliang at 20151221
                            response.Content = new StringContent(ResutlString);
                            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                            return response;
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
                                        pagedBuilder.Append(RegexUtils.Replace(string.Format("({0})(?!</a>)(?![^><)*>)", word.Replace(" ", "\\s")), pagedContents, string.Format("<span style='color:#cc0000'>{0}</span>", word)));
                                    }

                                    ResutlString = this.WriteResponse(pagedBuilder, pageInfo, contextInfo);
                                    return Request.CreateResponse(HttpStatusCode.OK, ResutlString, "text/html");
                                }
                            }
                        }
                    }

                    ResutlString = this.WriteResponse(contentBuilder, pageInfo, contextInfo);
                    //模拟原有aspx页面response.write，保证前台js统一处理返回数据
                    //api默认返回的数据是string或者json
                    //update by sessionliang at 20151221
                    response.Content = new StringContent(ResutlString);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
                    return response;
                }
            }
            //模拟原有aspx页面response.write，保证前台js统一处理返回数据
            //api默认返回的数据是string或者json
            //update by sessionliang at 20151221
            response.Content = new StringContent(ResutlString);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        private string WriteResponse(StringBuilder contentBuilder, PageInfo pageInfo, ContextInfo contextInfo)
        {
            StlUtility.LoadGeneratePageContent(PublishmentSystemInfo, pageInfo, contextInfo, contentBuilder, string.Empty);
            return contentBuilder.ToString();
        }
    }
}
