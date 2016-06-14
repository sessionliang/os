using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using System.Collections.Generic;
using SiteServer.STL.Parser.TemplateDesign;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal.StlTemplate
{
    public class StlElementLayout : BackgroundBasePage
    {
        public Literal ltlSetting;
        public Literal ltlCols;

        public TextBox tbMarginTop;
        public TextBox tbMarginBottom;

        public TextBox tbItemTemplate;

        public Literal ltlStlElement;

        private TemplateInfo templateInfo;
        private string includeUrl;
        private bool isStlInsert;
        private int stlIndex;
        private string oldStlElement;

        private List<string> columnNameList;

        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public static string GetOpenLayerStringToEdit(int publishmentSystemID, int templateID, string includeUrl, int stlIndex, string stlEncryptElement)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlInsert", false.ToString());
            arguments.Add("stlIndex", stlIndex.ToString());
            arguments.Add("stlEncryptElement", stlEncryptElement);
            return JsUtils.Layer.GetOpenLayerString("布局（stl:layout）编辑", PageUtils.GetSTLUrl("modal_stlElementLayout.aspx"), arguments);
        }

        public static string GetOpenLayerStringToAdd(int publishmentSystemID, int templateID, string includeUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlInsert", true.ToString());
            arguments.Add("PLACE_HOLDER", string.Empty);
            return JsUtils.Layer.GetOpenLayerString("布局（stl:layout）新增", PageUtils.GetSTLUrl("modal_stlElementLayout.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            int templateID = TranslateUtils.ToInt(base.GetQueryString("templateID"));
            this.templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, templateID);
            this.isStlInsert = TranslateUtils.ToBool(base.GetQueryString("isStlInsert"));

            this.stlIndex = TranslateUtils.ToInt(base.GetQueryString("stlIndex"));
            this.oldStlElement = RuntimeUtils.DecryptStringByTranslate(base.GetQueryString("stlEncryptElement"));

            if (!IsPostBack)
            {
                this.tbItemTemplate.Text = this.GetInnerXML();
            }

            string stlElement = this.GetStlElement();

            LowerNameValueCollection attributes = StlParserUtility.GetAttributes(stlElement, false);

            if (!string.IsNullOrEmpty(attributes[StlLayout.Attribute_Margin_Top]))
            {
                this.tbMarginTop.Text = TranslateUtils.ToInt(attributes[StlLayout.Attribute_Margin_Top], 5).ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlLayout.Attribute_Margin_Bottom]))
            {
                this.tbMarginBottom.Text = TranslateUtils.ToInt(attributes[StlLayout.Attribute_Margin_Bottom], 5).ToString();
            }

            StringBuilder colsBuilder = new StringBuilder();
            List<string> colList = TranslateUtils.StringCollectionToStringList(attributes[StlLayout.Attribute_Cols]);
            foreach (string col in colList)
            {
                string num = string.Empty;
                string unit = string.Empty;

                if (StringUtils.EndsWithIgnoreCase(col, "px"))
                {
                    num = col.Replace("px", string.Empty);
                    unit = "px";
                }
                else if (StringUtils.EndsWithIgnoreCase(col, "%"))
                {
                    num = col.Replace("%", string.Empty);
                    unit = "%";
                }

                colsBuilder.AppendFormat(@"{{num:'{0}', unit:'{1}'}}, ", num, unit);
            }
            if (colsBuilder.Length > 0)
            {
                colsBuilder.Length -= 2;
            }
            this.ltlCols.Text = colsBuilder.ToString();

            this.ltlStlElement.Text = StringUtils.HtmlEncode(stlElement).Trim().Replace("  ", string.Empty);
        }

        private string GetInnerXML()
        {
            string stlElement = string.Empty;
            if (!this.isStlInsert)
            {
                stlElement = this.oldStlElement;
            }
            else
            {
                string stlElementToAdd = base.GetQueryString("stlElementToAdd");
                stlElementToAdd = stlElementToAdd.Replace("__R_A_N__", StringUtils.Constants.ReturnAndNewline);
                stlElement = stlElementToAdd.Trim();
            }

            return StlParserUtility.ReplaceXmlNamespace(StlParserUtility.GetInnerXml(stlElement, false).Trim()).Replace("  ", string.Empty);
        }

        private string GetStlElement()
        {
            string stlElement = string.Empty;
            if (!this.isStlInsert)
            {
                //stlElement = TemplateDesignManager.GetStlElement(base.PublishmentSystemInfo, this.templateInfo, this.stlSequence);
                stlElement = this.oldStlElement;
            }
            else
            {
                string stlElementToAdd = base.GetQueryString("stlElementToAdd");
                stlElementToAdd = stlElementToAdd.Replace("__R_A_N__", StringUtils.Constants.ReturnAndNewline);
                stlElement = stlElementToAdd.Trim();
            }

            if (base.Page.IsPostBack)
            {
                NameValueCollection attributes = StlParserUtility.GetStlAttributes(stlElement);

                TranslateUtils.SetOrRemoveAttribute(attributes, "marginTop", this.tbMarginTop.Text);
                TranslateUtils.SetOrRemoveAttribute(attributes, "marginBottom", this.tbMarginBottom.Text);

                List<string> colNums = TranslateUtils.StringCollectionToStringList(base.Request.Form["colNum"]);
                List<string> colUnits = TranslateUtils.StringCollectionToStringList(base.Request.Form["colUnit"]);
                List<string> cols = new List<string>();

                if (colNums.Count == colUnits.Count)
                {
                    for (int i = 0; i < colNums.Count; i++)
                    {
                        string num = colNums[i];
                        string unit = colUnits[i];
                        if (string.IsNullOrEmpty(num))
                        {
                            unit = string.Empty;
                        }
                        string col = num + unit;
                        if (string.IsNullOrEmpty(col))
                        {
                            col = "*";
                        }
                        cols.Add(col);
                    }
                }

                TranslateUtils.SetOrRemoveAttribute(attributes, "cols", TranslateUtils.ObjectCollectionToString(cols));

                List<string> containerList = RegexUtils.GetTagContents(StlContainer.ElementName, this.tbItemTemplate.Text);

                var count = cols.Count;
                var length = containerList.Count;

                if (count > length)
                {
                    for (int i = 0; i < count - length; i++)
                    {
                        containerList.Add("<stl:container></stl:container>");
                    }
                }
                else if (count < length)
                {
                    for (int i = 0; i < length - count; i++)
                    {
                        containerList.RemoveAt(containerList.Count - 1);
                    }
                }

                StringBuilder builder = new StringBuilder();
                foreach (string container in containerList)
                {
                    builder.Append(container).AppendLine();
                }

                stlElement = StlParserUtility.GetStlElement(StlLayout.ElementName, attributes, builder.ToString());
            }
            return stlElement;
        }

        public void btnApply_OnClick(object sender, EventArgs e)
        {
            this.ltlSetting.Text += "$('#myTab a').eq(2).click();";
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            try
            {
                string stlElement = this.GetStlElement();

                if (this.isStlInsert)
                {
                    bool stlIsContainer = TranslateUtils.ToBool(base.GetQueryString("stlIsContainer"));
                    string stlSequenceCollection = base.GetQueryString("stlSequenceCollection");
                    int stlDivIndex = TranslateUtils.ToInt(base.GetQueryString("stlDivIndex"));

                    TemplateDesignManager.InsertStlElement(base.PublishmentSystemInfo, this.templateInfo, this.includeUrl, stlSequenceCollection, stlElement, stlDivIndex, stlIsContainer);
                }
                else
                {
                    TemplateDesignManager.UpdateStlElement(base.PublishmentSystemInfo, this.templateInfo, this.includeUrl, this.stlIndex, this.oldStlElement, stlElement);
                }

                StringUtility.AddLog(base.PublishmentSystemID, string.Format("修改{0}", ETemplateTypeUtils.GetText(this.templateInfo.TemplateType)), string.Format("模板名称:{0}", this.templateInfo.TemplateName));

                isSuccess = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }

            if (isSuccess)
            {
                JsUtils.Layer.CloseModalLayer(Page);
            }
        }
    }
}
