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
    public class StlElementContents : BackgroundBasePage
    {
        public Literal ltlChannels;
        public Literal ltlSetting;

        public DropDownList ddlScope;
        public DropDownList ddlOrder;
        public TextBox tbStartNum;
        public TextBox tbTotalNum;
        public DropDownList ddlGroup;
        public DropDownList ddlGroupNot;
        public CheckBoxList cblFilterAttributes;
        public Literal ltlFilterOptions;
        public Literal ltlFilters;

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
            return JsUtils.Layer.GetOpenLayerString("内容列表（stl:contents）编辑", PageUtils.GetSTLUrl("modal_stlElementContents.aspx"), arguments);
        }

        public static string GetOpenLayerStringToAdd(int publishmentSystemID, int templateID, string includeUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlInsert", true.ToString());
            arguments.Add("PLACE_HOLDER", string.Empty);
            return JsUtils.Layer.GetOpenLayerString("内容列表（stl:contents）新增", PageUtils.GetSTLUrl("modal_stlElementContents.aspx"), arguments);
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
                EScopeTypeUtils.AddListItems(this.ddlScope);
                this.ddlScope.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                EStlContentOrderUtils.AddListItems(this.ddlOrder);
                this.ddlOrder.Items.Insert(0, new ListItem(string.Empty, string.Empty));

                this.ddlGroup.Items.Add(new ListItem(string.Empty, string.Empty));
                this.ddlGroupNot.Items.Add(new ListItem(string.Empty, string.Empty));
                ArrayList groupNameArrayList = DataProvider.ContentGroupDAO.GetContentGroupNameArrayList(base.PublishmentSystemID);
                foreach (string groupName in groupNameArrayList)
                {
                    this.ddlGroup.Items.Add(new ListItem(groupName, groupName));
                    this.ddlGroupNot.Items.Add(new ListItem(groupName, groupName));
                }

                foreach (var value in StlContents.BooleanAttributeList)
                {
                    ListItem listItem = new ListItem(value.Value, value.Key);
                    listItem.Attributes.Add("rel", "tooltip");
                    listItem.Attributes.Add("data-original-title", value.Key + "属性");
                    this.cblFilterAttributes.Items.Add(listItem);
                }

                this.tbItemTemplate.Text = this.GetInnerXML();
            }

            StringBuilder channelsBuilder = new StringBuilder();
            ArrayList arraylist = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
            int nodeCount = arraylist.Count;
            bool[] isLastNodeArray = new bool[nodeCount];
            foreach (int nodeID in arraylist)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

                string title = NodeManager.GetSelectText(base.PublishmentSystemInfo, nodeInfo, isLastNodeArray, true, false);
                string style = string.Empty;
                if (string.IsNullOrEmpty(nodeInfo.NodeIndexName))
                {
                    style = "color:gray;";
                }
                channelsBuilder.AppendFormat(@"{{nodeID: '{0}', title:'{1}', channelIndex:'{2}', parentsPath:'{3}', style:'{4}', channelName:'{5}'}}, ", nodeID, title, nodeInfo.NodeIndexName, nodeInfo.ParentsPath, style, nodeInfo.NodeName);
            }

            channelsBuilder.Length -= 2;
            this.ltlChannels.Text = channelsBuilder.ToString();

            StringBuilder filterOptionBuilder = new StringBuilder();

            NodeInfo nodeInfoFirst = NodeManager.GetNodeInfo(base.PublishmentSystemID, base.PublishmentSystemID);
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfoFirst);
            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfoFirst);
            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, base.PublishmentSystemID);
            ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
            ArrayList columnTableStyleInfoArrayList = ContentUtility.GetColumnTableStyleInfoArrayList(base.PublishmentSystemInfo, tableStyle, tableStyleInfoArrayList);
            this.columnNameList = new List<string>();
            foreach (TableStyleInfo styleInfo in columnTableStyleInfoArrayList)
            {
                ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);

                if (styleInfo.IsVisible)
                {
                    if (!StlContents.BooleanAttributeList.ContainsKey(styleInfo.AttributeName.ToLower()))
                    {
                        filterOptionBuilder.AppendFormat(@"<option value=""{0}"">{1}（{2}）</option>", styleInfo.AttributeName.ToLower(), styleInfo.DisplayName, styleInfo.AttributeName);
                        this.columnNameList.Add(styleInfo.AttributeName.ToLower());
                    }
                }
            }
            this.ltlFilterOptions.Text = filterOptionBuilder.ToString();

            string stlElement = this.GetStlElement();

            LowerNameValueCollection attributes = StlParserUtility.GetAttributes(stlElement, false);

            if (!string.IsNullOrEmpty(attributes[StlContents.Attribute_ChannelIndex]))
            {
                this.ltlSetting.Text = string.Format("$('#channelIndex').val('{0}');", attributes[StlContents.Attribute_ChannelIndex]);
            }
            if (!string.IsNullOrEmpty(attributes[StlContents.Attribute_ChannelName]))
            {
                this.ltlSetting.Text += string.Format("$('#channelName').val('{0}');", attributes[StlContents.Attribute_ChannelName]);
            }
            if (!string.IsNullOrEmpty(attributes[StlContents.Attribute_Order]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlOrder, attributes[StlContents.Attribute_Order]);
            }
            if (!string.IsNullOrEmpty(attributes[StlContents.Attribute_Scope]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlScope, attributes[StlContents.Attribute_Scope]);
            }
            if (!string.IsNullOrEmpty(attributes[StlContents.Attribute_StartNum]))
            {
                this.tbStartNum.Text = TranslateUtils.ToInt(attributes[StlContents.Attribute_StartNum], 1).ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlContents.Attribute_TotalNum]))
            {
                this.tbTotalNum.Text = TranslateUtils.ToInt(attributes[StlContents.Attribute_TotalNum], 0).ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlContents.Attribute_Group]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroup, attributes[StlContents.Attribute_Group]);
            }
            if (!string.IsNullOrEmpty(attributes[StlContents.Attribute_GroupNot]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroupNot, attributes[StlContents.Attribute_GroupNot]);
            }
            if (!string.IsNullOrEmpty(attributes[StlContents.Attribute_GroupContent]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroup, attributes[StlContents.Attribute_GroupContent]);
            }
            if (!string.IsNullOrEmpty(attributes[StlContents.Attribute_GroupContentNot]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroupNot, attributes[StlContents.Attribute_GroupContentNot]);
            }

            foreach (ListItem listItem in this.cblFilterAttributes.Items)
            {
                if (!string.IsNullOrEmpty(attributes[listItem.Value.ToLower()]) && TranslateUtils.ToBool(attributes[listItem.Value.ToLower()]))
                {
                    listItem.Selected = true;
                }
            }

            StringBuilder filtersBuilder = new StringBuilder();
            foreach (string attributeName in attributes)
            {
                if (this.columnNameList.Contains(attributeName))
                {
                    string attributeValue = attributes[attributeName];
                    string attributeOperate = string.Empty;
                    if (StringUtils.StartsWithIgnoreCase(attributeValue, "not:") || StringUtils.StartsWithIgnoreCase(attributeValue, "contains:") || StringUtils.StartsWithIgnoreCase(attributeValue, "start:") || StringUtils.StartsWithIgnoreCase(attributeValue, "end:"))
                    {
                        attributeOperate = attributeValue.Split(':')[0].ToLower() + ":";
                        attributeValue = attributeValue.Split(':')[1];
                    }
                    filtersBuilder.AppendFormat(@"{{attribute:'{0}', operate:'{1}', value:'{2}'}}, ", attributeName, attributeOperate, attributeValue);
                }
            }
            if (filtersBuilder.Length > 0)
            {
                filtersBuilder.Length -= 2;
            }
            this.ltlFilters.Text = filtersBuilder.ToString();

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

                TranslateUtils.SetOrRemoveAttribute(attributes, "channelIndex", base.Request.Form["channelIndex"]);
                TranslateUtils.SetOrRemoveAttribute(attributes, "channelName", base.Request.Form["channelName"]);
                TranslateUtils.SetOrRemoveAttribute(attributes, "scope", this.ddlScope.SelectedValue);
                TranslateUtils.SetOrRemoveAttribute(attributes, "order", this.ddlOrder.SelectedValue);
                TranslateUtils.SetOrRemoveAttribute(attributes, "startNum", TranslateUtils.ToInt(this.tbStartNum.Text, 1) > 0 ? this.tbStartNum.Text : string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "totalNum", TranslateUtils.ToInt(this.tbTotalNum.Text, 0) > 0 ? this.tbTotalNum.Text : string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "groupContent", string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "group", this.ddlGroup.SelectedValue);
                TranslateUtils.SetOrRemoveAttribute(attributes, "groupContentNot", string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "groupNot", this.ddlGroupNot.SelectedValue);

                foreach (ListItem listItem in this.cblFilterAttributes.Items)
                {
                    string value = listItem.Selected ? "true" : string.Empty;
                    TranslateUtils.SetOrRemoveAttribute(attributes, listItem.Value, value);
                }

                foreach (string columnName in this.columnNameList)
                {
                    attributes.Remove(columnName);
                }

                List<string> filterAttributes = TranslateUtils.StringCollectionToStringList(base.Request.Form["filterAttribute"]);
                List<string> filterOperates = TranslateUtils.StringCollectionToStringList(base.Request.Form["filterOperate"]);
                List<string> filterValues = TranslateUtils.StringCollectionToStringList(base.Request.Form["filterValue"]);
                if (filterAttributes.Count == filterOperates.Count && filterAttributes.Count == filterValues.Count)
                {
                    for (int i = 0; i < filterAttributes.Count; i++)
                    {
                        string filterAttribute = filterAttributes[i];
                        string filterOperate = filterOperates[i];
                        string filterValue = filterValues[i];
                        if (!string.IsNullOrEmpty(filterAttribute))
                        {
                            attributes[filterAttribute] = filterOperate + filterValue;
                        }
                    }
                }

                stlElement = StlParserUtility.GetStlElement(StlContents.ElementName, attributes, this.tbItemTemplate.Text);
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
