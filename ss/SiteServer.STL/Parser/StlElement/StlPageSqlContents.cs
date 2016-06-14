using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.ListTemplate;
using SiteServer.CMS.Model;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlPageSqlContents : StlSqlContents
    {
        public new const string ElementName = "stl:pagesqlcontents";//�ɷ�ҳ�����б�

        public const string Attribute_PageNum = "pagenum";					//ÿҳ��ʾ��������Ŀ

        readonly XmlNode node = null;
        readonly ContentsDisplayInfo displayInfo = null;
        readonly PageInfo pageInfo;
        readonly ContextInfo contextInfo;
        private DataSet dataSet = null;

        public new static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = StlSqlContents.AttributeList;
                attributes.Add(Attribute_PageNum, "ÿҳ��ʾ��������Ŀ");
                return attributes;
            }
        }

        public StlPageSqlContents(string stlPageSqlContentsElement, PageInfo pageInfo, ContextInfo contextInfo, bool isXmlContent, bool isLoadData)
        {
            this.pageInfo = pageInfo;
            this.contextInfo = contextInfo;
            try
            {
                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlPageSqlContentsElement, isXmlContent);
                node = xmlDocument.DocumentElement;
                node = node.FirstChild;

                this.displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, this.contextInfo, EContextType.SqlContent);
                if (isLoadData)
                {
                    this.dataSet = StlDataUtility.GetPageSqlContentsDataSet(displayInfo.ConnectionString, displayInfo.QueryString, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString);
                }
            }
            catch
            {
                this.displayInfo = new ContentsDisplayInfo();
            }
        }

        public StlPageSqlContents(string stlPageSqlContentsElement, PageInfo pageInfo, ContextInfo contextInfo, bool isXmlContent)
        {
            this.pageInfo = pageInfo;
            this.contextInfo = contextInfo;
            try
            {
                XmlDocument xmlDocument = StlParserUtility.GetXmlDocument(stlPageSqlContentsElement, isXmlContent);
                node = xmlDocument.DocumentElement;
                node = node.FirstChild;

                this.displayInfo = ContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, this.contextInfo, EContextType.SqlContent);
                this.dataSet = StlDataUtility.GetPageSqlContentsDataSet(displayInfo.ConnectionString, displayInfo.QueryString, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString);
            }
            catch
            {
                this.displayInfo = new ContentsDisplayInfo();
            }
        }

        public void LoadData()
        {
            this.dataSet = StlDataUtility.GetPageSqlContentsDataSet(displayInfo.ConnectionString, displayInfo.QueryString, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString);
        }

        public int GetPageCount(out int contentNum)
        {
            int pageCount = 1;
            contentNum = 0;//���ݿ���ʵ�ʵ�������Ŀ
            if (this.dataSet != null)
            {
                contentNum = this.dataSet.Tables[0].DefaultView.Count;
                if (this.displayInfo.PageNum != 0 && this.displayInfo.PageNum < contentNum)//��Ҫ��ҳ
                {
                    pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(contentNum) / Convert.ToDouble(this.displayInfo.PageNum)));//��Ҫ���ɵ���ҳ��
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

                            rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.SqlContent, contextInfo);

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
                                rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.SqlContent, contextInfo);
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
                            TemplateUtility.PutContentsDisplayInfoToMyDataList(pdlContents, displayInfo);

                            pdlContents.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.SqlContent, contextInfo);
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
                                pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.SqlContent, this.contextInfo);
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

            return StlParserUtility.GetBackHtml(parsedContent, pageInfo);
        }

    }

}
