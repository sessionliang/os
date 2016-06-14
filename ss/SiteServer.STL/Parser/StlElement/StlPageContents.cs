using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.ListTemplate;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using System.Collections;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlPageContents : StlContents
    {
        public new const string ElementName = "stl:pagecontents";//可翻页内容列表

        public const string Attribute_PageNum = "pagenum";					//每页显示的内容数目

        readonly string stlPageContentsElement;
        readonly XmlNode node = null;
        private ContentsDisplayInfo displayInfo = null;
        readonly PageInfo pageInfo;
        readonly ContextInfo contextInfo;
        readonly int channelID = 0;
        readonly string sqlString = string.Empty;

        public new static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = StlContents.AttributeList;
                attributes.Add(Attribute_PageNum, "每页显示的内容数目");
                return attributes;
            }
        }

        public StlPageContents(string stlPageContentsElement, PageInfo pageInfo, ContextInfo contextInfo, bool isXmlContent)
        {
            this.pageInfo = pageInfo;
            this.contextInfo = contextInfo;
            XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlPageContentsElement, isXmlContent);
            node = xmlDocument.DocumentElement;
            this.stlPageContentsElement = node.InnerXml;
            node = node.FirstChild;

            this.displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, this.contextInfo, EContextType.Content);

            this.contextInfo.TitleWordNum = displayInfo.TitleWordNum;

            this.channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, this.contextInfo.ChannelID, displayInfo.UpLevel, displayInfo.TopLevel);

            this.channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, displayInfo.ChannelIndex, displayInfo.ChannelName);

            this.sqlString = StlDataUtility.GetPageContentsSqlString(this.pageInfo.PublishmentSystemInfo, this.channelID, displayInfo.GroupContent, displayInfo.GroupContentNot, displayInfo.Tags, displayInfo.IsImageExists, displayInfo.IsImage, displayInfo.IsVideoExists, displayInfo.IsVideo, displayInfo.IsFileExists, displayInfo.IsFile, displayInfo.IsNoDup, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString, displayInfo.IsTopExists, displayInfo.IsTop, displayInfo.IsRecommendExists, displayInfo.IsRecommend, displayInfo.IsHotExists, displayInfo.IsHot, displayInfo.IsColorExists, displayInfo.IsColor, displayInfo.Where, displayInfo.Scope, displayInfo.GroupChannel, displayInfo.GroupChannelNot);
        }

        //InnerSearchOutput调用
        public StlPageContents(string stlPageContentsElement, PageInfo pageInfo, ContextInfo contextInfo, int pageNum, string whereString)
        {
            this.pageInfo = pageInfo;
            this.contextInfo = contextInfo;
            XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlPageContentsElement, false);
            node = xmlDocument.DocumentElement;
            node = node.FirstChild;

            this.displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, this.contextInfo, EContextType.Content);
            this.displayInfo.Scope = EScopeType.All;

            this.contextInfo.TitleWordNum = displayInfo.TitleWordNum;

            this.displayInfo.Where += whereString;
            if (pageNum > 0)
            {
                this.displayInfo.PageNum = pageNum;
            }

            this.channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, this.contextInfo.ChannelID, displayInfo.UpLevel, displayInfo.TopLevel);

            this.channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, displayInfo.ChannelIndex, displayInfo.ChannelName);

            this.sqlString = StlDataUtility.GetPageContentsSqlString(this.pageInfo.PublishmentSystemInfo, this.channelID, displayInfo.GroupContent, displayInfo.GroupContentNot, displayInfo.Tags, displayInfo.IsImageExists, displayInfo.IsImage, displayInfo.IsVideoExists, displayInfo.IsVideo, displayInfo.IsFileExists, displayInfo.IsFile, displayInfo.IsNoDup, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString, displayInfo.IsTopExists, displayInfo.IsTop, displayInfo.IsRecommendExists, displayInfo.IsRecommend, displayInfo.IsHotExists, displayInfo.IsHot, displayInfo.IsColorExists, displayInfo.IsColor, displayInfo.Where, displayInfo.Scope, displayInfo.GroupChannel, displayInfo.GroupChannelNot);
        }

        public int GetPageCount(out int totalNum)
        {
            totalNum = 0;
            int pageCount = 1;
            try
            {
                totalNum = BaiRongDataProvider.DatabaseDAO.GetPageTotalCount(this.sqlString);
                if (this.displayInfo.PageNum != 0 && this.displayInfo.PageNum < totalNum)//需要翻页
                {
                    pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalNum) / Convert.ToDouble(this.displayInfo.PageNum)));//需要生成的总页数
                }
            }
            catch { }
            return pageCount;
        }

        public string SqlString
        {
            get
            {
                return this.sqlString;
            }
        }

        public ContentsDisplayInfo DisplayInfo
        {
            get
            {
                return this.displayInfo;
            }
        }

        public string Parse(int totalNum, int currentPageIndex, int pageCount)
        {
            return Parse(totalNum, currentPageIndex, pageCount, string.Empty);
        }

        public string Parse(int totalNum, int currentPageIndex, int pageCount, string ajaxDivID)
        {
            string parsedContent = string.Empty;

            this.contextInfo.PageItemIndex = currentPageIndex * this.displayInfo.PageNum;

            try
            {
                if (node != null)
                {
                    if (!string.IsNullOrEmpty(this.sqlString))
                    {
                        string pageSqlString = BaiRongDataProvider.DatabaseDAO.GetPageSqlString(this.sqlString, displayInfo.OrderByString, totalNum, this.displayInfo.PageNum, currentPageIndex);

                        IEnumerable datasource = BaiRongDataProvider.DatabaseDAO.GetDataSource(pageSqlString);

                        if (this.displayInfo.Layout == ELayout.None)
                        {
                            Repeater rptContents = new Repeater();

                            rptContents.ItemTemplate = new RepeaterTemplate(this.displayInfo.ItemTemplate, this.displayInfo.SelectedItems, this.displayInfo.SelectedValues, this.displayInfo.SeparatorRepeatTemplate, this.displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Content, this.contextInfo);
                            if (!string.IsNullOrEmpty(displayInfo.HeaderTemplate))
                            {
                                rptContents.HeaderTemplate = new SeparatorTemplate(displayInfo.HeaderTemplate);
                            }
                            if (!string.IsNullOrEmpty(displayInfo.FooterTemplate))
                            {
                                rptContents.FooterTemplate = new SeparatorTemplate(displayInfo.FooterTemplate);
                            }
                            if (!string.IsNullOrEmpty(this.displayInfo.SeparatorTemplate))
                            {
                                rptContents.SeparatorTemplate = new SeparatorTemplate(this.displayInfo.SeparatorTemplate);
                            }
                            if (!string.IsNullOrEmpty(this.displayInfo.AlternatingItemTemplate))
                            {
                                rptContents.AlternatingItemTemplate = new RepeaterTemplate(this.displayInfo.AlternatingItemTemplate, this.displayInfo.SelectedItems, this.displayInfo.SelectedValues, this.displayInfo.SeparatorRepeatTemplate, this.displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Content, this.contextInfo);
                            }

                            rptContents.DataSource = datasource;
                            rptContents.DataBind();

                            if (rptContents.Items.Count > 0)
                            {
                                parsedContent = ControlUtils.GetControlRenderHtml(rptContents);
                            }
                        }
                        else
                        {
                            ParsedDataList pdlContents = new ParsedDataList();

                            //设置显示属性
                            TemplateUtility.PutContentsDisplayInfoToMyDataList(pdlContents, this.displayInfo);

                            pdlContents.ItemTemplate = new DataListTemplate(this.displayInfo.ItemTemplate, this.displayInfo.SelectedItems, this.displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Content, this.contextInfo);
                            if (!string.IsNullOrEmpty(displayInfo.HeaderTemplate))
                            {
                                pdlContents.HeaderTemplate = new SeparatorTemplate(displayInfo.HeaderTemplate);
                            }
                            if (!string.IsNullOrEmpty(displayInfo.FooterTemplate))
                            {
                                pdlContents.FooterTemplate = new SeparatorTemplate(displayInfo.FooterTemplate);
                            }
                            if (!string.IsNullOrEmpty(this.displayInfo.SeparatorTemplate))
                            {
                                pdlContents.SeparatorTemplate = new SeparatorTemplate(this.displayInfo.SeparatorTemplate);
                            }
                            if (!string.IsNullOrEmpty(this.displayInfo.AlternatingItemTemplate))
                            {
                                pdlContents.AlternatingItemTemplate = new DataListTemplate(this.displayInfo.AlternatingItemTemplate, this.displayInfo.SelectedItems, this.displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Content, this.contextInfo);
                            }

                            pdlContents.DataSource = datasource;
                            pdlContents.DataKeyField = ContentAttribute.ID;
                            pdlContents.DataBind();

                            if (pdlContents.Items.Count > 0)
                            {
                                parsedContent = ControlUtils.GetControlRenderHtml(pdlContents);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            parsedContent = StlParserUtility.GetBackHtml(parsedContent, pageInfo);

            parsedContent = ParseDynamicPageContents(pageCount, parsedContent, ajaxDivID);

            //还原翻页为0，使得其他列表能够正确解析ItemIndex
            this.contextInfo.PageItemIndex = 0;
            return parsedContent;
        }

        public string ParseDynamicPageContents(int pageCount, string parsedContent, string ajaxDivID)
        {
            if (pageInfo.PublishmentSystemInfo.Additional.CreateStaticMaxPage > 0 && pageCount > pageInfo.PublishmentSystemInfo.Additional.CreateStaticMaxPage)
            {
                if (string.IsNullOrEmpty(ajaxDivID))
                {
                    ajaxDivID = StlParserUtility.GetAjaxDivID(pageInfo.UniqueID);
                    parsedContent = string.Format(@"<span id=""{0}"">{1}</span>", ajaxDivID, parsedContent);

                    string outputPageUrl = StlTemplateManager.Dynamic.GetPageUrlOutput(pageInfo.PublishmentSystemInfo);
                    string currentPageUrl = StlUtility.GetStlCurrentUrl(pageInfo, contextInfo.ChannelID, contextInfo.ContentID, contextInfo.ContentInfo);
                    currentPageUrl = PageUtils.AddQuestionOrAndToUrl(currentPageUrl);
                    string outputParms = StlTemplateManager.Dynamic.GetOutputParameters(contextInfo.ChannelID, contextInfo.ContentID, pageInfo.TemplateInfo.TemplateID, currentPageUrl, ajaxDivID, false, this.stlPageContentsElement);
                    string pageScripts = string.Format(@"
<script type=""text/javascript"" language=""javascript"">
var stlDynamicPageNum = {3};
function stlDynamic_page(pageNum)
{{
    try
    {{
        if(pageNum=='nextPage'){{
            stlDynamicPageNum++;
        }}
        else if(pageNum=='previousPage'){{
            stlDynamicPageNum--;
        }}
        else{{
            stlDynamicPageNum=pageNum;
        }}

        var url = ""{0}"";
        var pars = ""{1}&pageNum="" + stlDynamicPageNum;
        jQuery.post(url, pars, function(data,textStatus){{
            jQuery(""#{2}"").html(data);
        }});
        
        stlDynamicPageItem(pageNum);

    }}catch(e){{}}
}}
</script>
", outputPageUrl, outputParms, ajaxDivID, pageInfo.PublishmentSystemInfo.Additional.CreateStaticMaxPage);
                    pageInfo.AddPageScriptsIfNotExists(ajaxDivID, pageScripts);
                }
                else
                {
                    string pageScripts = string.Format(@"
<script type=""text/javascript"" language=""javascript"">
var pageContentsAjaxDivID = '{0}';
function stlDynamic_{0}(pageNum)
{{
    stlDynamic_page(pageNum);
}}
</script>", ajaxDivID);
                    parsedContent += pageScripts;
                }


            }
            return parsedContent;
        }
    }
}