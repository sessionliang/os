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
    public class StlPageInputContents : StlInputContents
    {
        public new const string ElementName = "stl:pageinputcontents";//可翻页提交内容列表

        public const string Attribute_PageNum = "pagenum";					//每页显示的提交内容数目

        readonly XmlNode node = null;
        readonly ContentsDisplayInfo displayInfo = null;
        readonly PageInfo pageInfo;
        readonly ContextInfo contextInfo;

        readonly DataSet dataSet = null;

        public new static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = StlInputContents.AttributeList;
                attributes.Add(Attribute_PageNum, "每页显示的提交内容数目");
                return attributes;
            }
        }

        public StlPageInputContents(string stlPageInputContentsElement, PageInfo pageInfo, ContextInfo contextInfo, bool isXmlContent)
        {
            this.pageInfo = pageInfo;
            this.contextInfo = contextInfo;
            XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlPageInputContentsElement, isXmlContent);
            node = xmlDocument.DocumentElement;
            node = node.FirstChild;

            this.displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, this.contextInfo, EContextType.InputContent);

            int inputID = DataProvider.InputDAO.GetInputIDAsPossible(displayInfo.OtherAttributes[StlInputContents.Attribute_InputName], pageInfo.PublishmentSystemID);

            this.dataSet = StlDataUtility.GetPageInputContentsDataSet(pageInfo.PublishmentSystemID, inputID, displayInfo);
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

                            rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.InputContent, this.contextInfo);
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
                                rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.InputContent, this.contextInfo);
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
                            pdlContents.ItemTemplate = new DataListTemplate(this.displayInfo.ItemTemplate, this.displayInfo.SelectedItems, this.displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.InputContent, this.contextInfo);
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
                                pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, this.displayInfo.SelectedItems, this.displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.InputContent, this.contextInfo);
                            }

                            pdlContents.DataSource = objPage;
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

            return parsedContent;
        }
    }

}
