using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.ListTemplate;
using SiteServer.CMS.Model;
using BaiRong.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlPageChannels : StlChannels
    {
        public new const string ElementName = "stl:pagechannels";//可翻页栏目列表

        public const string Attribute_PageNum = "pagenum";					//每页显示的栏目数目

        readonly XmlNode node = null;
        readonly ContentsDisplayInfo displayInfo = null;
        readonly PageInfo pageInfo;
        readonly ContextInfo contextInfo;
        readonly int channelID = 0;
        readonly DataSet dataSet = null;

        public new static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = StlChannels.AttributeList;
                attributes.Add(Attribute_PageNum, "每页显示的栏目数目");
                return attributes;
            }
        }

        public StlPageChannels(string stlPageChannelsElement, PageInfo pageInfo, ContextInfo contextInfo, bool isXmlContent)
        {
            this.pageInfo = pageInfo;
            this.contextInfo = contextInfo;
            XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlPageChannelsElement, isXmlContent);
            node = xmlDocument.DocumentElement;
            node = node.FirstChild;

            this.displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, this.contextInfo, EContextType.Channel);

            this.channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, this.contextInfo.ChannelID, displayInfo.UpLevel, displayInfo.TopLevel);

            this.channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, displayInfo.ChannelIndex, displayInfo.ChannelName);

            bool isTotal = TranslateUtils.ToBool(displayInfo.OtherAttributes[StlChannels.Attribute_IsTotal]);

            if (TranslateUtils.ToBool(displayInfo.OtherAttributes[StlChannels.Attribute_IsAllChildren]))
            {
                displayInfo.Scope = EScopeType.Descendant;
            }

            this.dataSet = StlDataUtility.GetPageChannelsDataSet(pageInfo.PublishmentSystemID, channelID, displayInfo.GroupChannel, displayInfo.GroupChannelNot, displayInfo.IsImageExists, displayInfo.IsImage, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString, displayInfo.Scope, isTotal, displayInfo.Where);
        }


        public int GetPageCount(out int totalNum)
        {
            int pageCount = 1;
            totalNum = 0;//数据库中实际的内容数目
            if (this.dataSet != null)
            {
                totalNum = this.dataSet.Tables[0].DefaultView.Count;
                if (this.displayInfo.PageNum != 0 && this.displayInfo.PageNum < totalNum)//需要翻页
                {
                    pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalNum) / Convert.ToDouble(this.displayInfo.PageNum)));//需要生成的总页数
                }
            }
            return pageCount;
        }

        public ContentsDisplayInfo DisplayInfo
        {
            get
            {
                return this.displayInfo;
            }
        }

        public string Parse(int currentPageIndex, int pageCount)
        {
            string parsedContent = string.Empty;

            this.contextInfo.PageItemIndex = currentPageIndex * this.displayInfo.PageNum;

            try
            {
                if (node != null)
                {
                    if (this.dataSet != null)
                    {
                        PagedDataSource objPage = new PagedDataSource();//分页类
                        objPage.DataSource = this.dataSet.Tables[0].DefaultView;

                        if (pageCount > 1)
                        {
                            objPage.AllowPaging = true;
                            objPage.PageSize = this.displayInfo.PageNum;//每页显示的项数
                        }
                        else
                        {
                            objPage.AllowPaging = false;
                        }

                        objPage.CurrentPageIndex = currentPageIndex;//当前页的索引


                        if (this.displayInfo.Layout == ELayout.None)
                        {
                            Repeater rptContents = new Repeater();

                            rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Channel, this.contextInfo);
                            if (!string.IsNullOrEmpty(displayInfo.HeaderTemplate))
                            {
                                rptContents.HeaderTemplate = new SeparatorTemplate(displayInfo.HeaderTemplate);
                            }
                            if (!string.IsNullOrEmpty(displayInfo.FooterTemplate))
                            {
                                rptContents.FooterTemplate = new SeparatorTemplate(displayInfo.FooterTemplate);
                            }
                            if (!string.IsNullOrEmpty(displayInfo.SeparatorTemplate))
                            {
                                rptContents.SeparatorTemplate = new SeparatorTemplate(displayInfo.SeparatorTemplate);
                            }
                            if (!string.IsNullOrEmpty(displayInfo.AlternatingItemTemplate))
                            {
                                rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Channel, this.contextInfo);
                            }

                            rptContents.DataSource = objPage;
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

                            //设置列表模板
                            pdlContents.ItemTemplate = new DataListTemplate(this.displayInfo.ItemTemplate, this.displayInfo.SelectedItems, this.displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Channel, this.contextInfo);
                            if (!string.IsNullOrEmpty(displayInfo.HeaderTemplate))
                            {
                                pdlContents.HeaderTemplate = new SeparatorTemplate(displayInfo.HeaderTemplate);
                            }
                            if (!string.IsNullOrEmpty(displayInfo.FooterTemplate))
                            {
                                pdlContents.FooterTemplate = new SeparatorTemplate(displayInfo.FooterTemplate);
                            }
                            if (!string.IsNullOrEmpty(displayInfo.SeparatorTemplate))
                            {
                                pdlContents.SeparatorTemplate = new SeparatorTemplate(displayInfo.SeparatorTemplate);
                            }
                            if (!string.IsNullOrEmpty(displayInfo.AlternatingItemTemplate))
                            {
                                pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, this.displayInfo.SelectedItems, this.displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Channel, this.contextInfo);
                            }

                            pdlContents.DataSource = objPage;
                            pdlContents.DataKeyField = NodeAttribute.NodeID;
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

            //还原翻页为0，使得其他列表能够正确解析ItemIndex
            this.contextInfo.PageItemIndex = 0;

            return StlParserUtility.GetBackHtml(parsedContent, pageInfo);
        }
    }

}
