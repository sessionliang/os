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
    public class StlElementFocusViewer : BackgroundBasePage
    {
        public Literal ltlChannels;
        public Literal ltlSetting;

        public DropDownList ddlScope;
        public DropDownList ddlOrder;
        public TextBox tbStartNum;
        public TextBox tbTotalNum;
        public DropDownList ddlTheme;
        public TextBox tbWidth;
        public TextBox tbHeight;
        public DropDownList ddlGroup;
        public DropDownList ddlGroupNot;
        public CheckBoxList cblFilterAttributes;

        public Literal ltlStlElement;

        private TemplateInfo templateInfo;
        private string includeUrl;
        private bool isStlInsert;
        private int stlIndex;
        private string oldStlElement;

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
            return JsUtils.Layer.GetOpenLayerString("滚动焦点图（stl:focusViewer）编辑", PageUtils.GetSTLUrl("modal_stlElementFocusViewer.aspx"), arguments);
        }

        public static string GetOpenLayerStringToAdd(int publishmentSystemID, int templateID, string includeUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlInsert", true.ToString());
            arguments.Add("PLACE_HOLDER", string.Empty);
            return JsUtils.Layer.GetOpenLayerString("滚动焦点图（stl:focusViewer）新增", PageUtils.GetSTLUrl("modal_stlElementFocusViewer.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            int templateID = TranslateUtils.ToInt(base.GetQueryString("templateID"));
            this.templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, templateID);
            this.includeUrl = base.GetQueryString("includeUrl");
            this.isStlInsert = TranslateUtils.ToBool(base.GetQueryString("isStlInsert"));

            this.stlIndex = TranslateUtils.ToInt(base.GetQueryString("stlIndex"));
            this.oldStlElement = RuntimeUtils.DecryptStringByTranslate(base.GetQueryString("stlEncryptElement"));

            if (!IsPostBack)
            {
                EScopeTypeUtils.AddListItems(this.ddlScope);
                this.ddlScope.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                EStlContentOrderUtils.AddListItems(this.ddlOrder);
                this.ddlOrder.Items.Insert(0, new ListItem(string.Empty, string.Empty));

                foreach (string theme in StlFocusViewer.AttributeValuesTheme)
                {
                    ListItem listItem = new ListItem(theme, theme);
                    this.ddlTheme.Items.Add(listItem);
                }
                this.ddlTheme.Items.Insert(0, new ListItem(string.Empty, string.Empty));

                this.ddlGroup.Items.Add(new ListItem(string.Empty, string.Empty));
                this.ddlGroupNot.Items.Add(new ListItem(string.Empty, string.Empty));
                ArrayList groupNameArrayList = DataProvider.ContentGroupDAO.GetContentGroupNameArrayList(base.PublishmentSystemID);
                foreach (string groupName in groupNameArrayList)
                {
                    this.ddlGroup.Items.Add(new ListItem(groupName, groupName));
                    this.ddlGroupNot.Items.Add(new ListItem(groupName, groupName));
                }

                foreach (var value in StlFocusViewer.BooleanAttributeList)
                {
                    ListItem listItem = new ListItem(value.Value, value.Key);
                    listItem.Attributes.Add("rel", "tooltip");
                    listItem.Attributes.Add("data-original-title", value.Key + "属性");
                    this.cblFilterAttributes.Items.Add(listItem);
                }
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

            string stlElement = this.GetStlElement();

            LowerNameValueCollection attributes = StlParserUtility.GetAttributes(stlElement, false);

            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_ChannelIndex]))
            {
                this.ltlSetting.Text = string.Format("$('#channelIndex').val('{0}');", attributes[StlFocusViewer.Attribute_ChannelIndex]);
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_ChannelName]))
            {
                this.ltlSetting.Text += string.Format("$('#channelName').val('{0}');", attributes[StlFocusViewer.Attribute_ChannelName]);
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_Order]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlOrder, attributes[StlFocusViewer.Attribute_Order]);
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_Scope]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlScope, attributes[StlFocusViewer.Attribute_Scope]);
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_StartNum]))
            {
                this.tbStartNum.Text = TranslateUtils.ToInt(attributes[StlFocusViewer.Attribute_StartNum], 1).ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_TotalNum]))
            {
                this.tbTotalNum.Text = TranslateUtils.ToInt(attributes[StlFocusViewer.Attribute_TotalNum], 0).ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_Theme]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlTheme, attributes[StlFocusViewer.Attribute_Theme]);
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_Width]))
            {
                this.tbWidth.Text = TranslateUtils.ToInt(attributes[StlFocusViewer.Attribute_Width], 260).ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_Height]))
            {
                this.tbHeight.Text = TranslateUtils.ToInt(attributes[StlFocusViewer.Attribute_Height], 182).ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_Group]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroup, attributes[StlFocusViewer.Attribute_Group]);
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_GroupNot]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroupNot, attributes[StlFocusViewer.Attribute_GroupNot]);
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_GroupContent]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroup, attributes[StlFocusViewer.Attribute_GroupContent]);
            }
            if (!string.IsNullOrEmpty(attributes[StlFocusViewer.Attribute_GroupContentNot]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroupNot, attributes[StlFocusViewer.Attribute_GroupContentNot]);
            }

            foreach (ListItem listItem in this.cblFilterAttributes.Items)
            {
                if (!string.IsNullOrEmpty(attributes[listItem.Value.ToLower()]) && TranslateUtils.ToBool(attributes[listItem.Value.ToLower()]))
                {
                    listItem.Selected = true;
                }
            }

            this.ltlStlElement.Text = StringUtils.HtmlEncode(stlElement).Trim().Replace("  ", string.Empty);
        }

        private string GetStlElement()
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

            if (base.Page.IsPostBack)
            {
                NameValueCollection attributes = StlParserUtility.GetStlAttributes(stlElement);

                TranslateUtils.SetOrRemoveAttribute(attributes, "channelIndex", base.Request.Form["channelIndex"]);
                TranslateUtils.SetOrRemoveAttribute(attributes, "channelName", base.Request.Form["channelName"]);
                TranslateUtils.SetOrRemoveAttribute(attributes, "scope", this.ddlScope.SelectedValue);
                TranslateUtils.SetOrRemoveAttribute(attributes, "order", this.ddlOrder.SelectedValue);
                TranslateUtils.SetOrRemoveAttribute(attributes, "startNum", TranslateUtils.ToInt(this.tbStartNum.Text, 1) > 0 ? this.tbStartNum.Text : string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "totalNum", TranslateUtils.ToInt(this.tbTotalNum.Text, 0) > 0 ? this.tbTotalNum.Text : string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "theme", this.ddlTheme.SelectedValue);
                TranslateUtils.SetOrRemoveAttribute(attributes, "width", TranslateUtils.ToInt(this.tbWidth.Text, 0) > 0 ? this.tbWidth.Text : string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "height", TranslateUtils.ToInt(this.tbHeight.Text, 0) > 0 ? this.tbHeight.Text : string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "groupContent", string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "group", this.ddlGroup.SelectedValue);
                TranslateUtils.SetOrRemoveAttribute(attributes, "groupContentNot", string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "groupNot", this.ddlGroupNot.SelectedValue);

                foreach (ListItem listItem in this.cblFilterAttributes.Items)
                {
                    string value = listItem.Selected ? "true" : string.Empty;
                    TranslateUtils.SetOrRemoveAttribute(attributes, listItem.Value, value);
                }

                stlElement = StlParserUtility.GetStlElement(StlFocusViewer.ElementName, attributes, null);
            }
            return stlElement;
        }

        public void btnApply_OnClick(object sender, EventArgs e)
        {
            this.ltlSetting.Text += "$('#myTab a').eq(1).click();";
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
