using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.ListTemplate;
using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlPageComments : StlComments
    {
        public new const string ElementName = "stl:pagecomments";//�ɷ�ҳ�����б�

        readonly XmlNode node = null;
        readonly ContentsDisplayInfo displayInfo = null;
        readonly PageInfo pageInfo;
        readonly ContextInfo contextInfo;
        readonly DataSet dataSet = null;

        public new static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = StlComments.AttributeList;
                return attributes;
            }
        }

        public StlPageComments(string stlPageCommentsElement, PageInfo pageInfo, ContextInfo contextInfo, bool isXmlContent)
        {
            this.pageInfo = pageInfo;
            this.contextInfo = contextInfo;
            XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlPageCommentsElement, isXmlContent);
            node = xmlDocument.DocumentElement;
            node = node.FirstChild;

            this.displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, this.contextInfo, EContextType.Comment);

            this.dataSet = StlDataUtility.GetPageCommentsDataSet(pageInfo.PublishmentSystemID, contextInfo.ChannelID, contextInfo.ContentID, null, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.IsRecommend, displayInfo.OrderByString, displayInfo.Where);
        }


        public int GetPageCount(out int totalNum)
        {
            int pageCount = 1;
            totalNum = 0;//���ݿ���ʵ�ʵ�������Ŀ
            if (this.dataSet != null)
            {
                totalNum = this.dataSet.Tables[0].DefaultView.Count;
                if (this.displayInfo.PageNum != 0 && this.displayInfo.PageNum < totalNum)//��Ҫ��ҳ
                {
                    pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalNum) / Convert.ToDouble(this.displayInfo.PageNum)));//��Ҫ���ɵ���ҳ��
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
                        PagedDataSource objPage = new PagedDataSource();//��ҳ��
                        objPage.DataSource = this.dataSet.Tables[0].DefaultView;

                        if (pageCount > 1)
                        {
                            objPage.AllowPaging = true;
                            objPage.PageSize = this.displayInfo.PageNum;//ÿҳ��ʾ������
                        }
                        else
                        {
                            objPage.AllowPaging = false;
                        }

                        objPage.CurrentPageIndex = currentPageIndex;//��ǰҳ������


                        if (this.displayInfo.Layout == ELayout.None)
                        {
                            Repeater rptContents = new Repeater();

                            rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Comment, this.contextInfo);
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
                                rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Comment, this.contextInfo);
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

                            //������ʾ����
                            TemplateUtility.PutContentsDisplayInfoToMyDataList(pdlContents, this.displayInfo);

                            //�����б�ģ��
                            pdlContents.ItemTemplate = new DataListTemplate(this.displayInfo.ItemTemplate, this.displayInfo.SelectedItems, this.displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Comment, this.contextInfo);
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
                                pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, this.displayInfo.SelectedItems, this.displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, this.pageInfo, EContextType.Comment, this.contextInfo);
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

            //��ԭ��ҳΪ0��ʹ�������б��ܹ���ȷ����ItemIndex
            this.contextInfo.PageItemIndex = 0;

            return parsedContent;
        }
    }

}