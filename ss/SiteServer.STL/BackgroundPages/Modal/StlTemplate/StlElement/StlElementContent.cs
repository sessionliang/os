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
using BaiRong.Text.LitJson;
using System.Web;
using SiteServer.STL.Parser.TemplateDesign;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal.StlTemplate
{
    public class StlElementContent : BackgroundBasePage
    {
        public Literal ltlSetting;

        public DropDownList ddlType;
        public TextBox tbWordNum;
        public CheckBox cbIsClearTags;
        public CheckBox cbIsReturnToBR;
        public CheckBox cbIsUpper;
        public CheckBox cbIsLower;

        public Literal ltlStlElement;

        private TemplateInfo templateInfo;
        private int nodeID;
        private string includeUrl;
        private bool isStlInsert;
        private int stlIndex;
        private string oldStlElement;

        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public static string GetOpenLayerStringToEdit(int publishmentSystemID, int templateID, int nodeID, string includeUrl, int stlIndex, string stlEncryptElement)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("nodeID", nodeID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlInsert", false.ToString());
            arguments.Add("stlIndex", stlIndex.ToString());
            arguments.Add("stlEncryptElement", stlEncryptElement);
            return JsUtils.Layer.GetOpenLayerString("内容值（stl:content）编辑", PageUtils.GetSTLUrl("modal_stlElementContent.aspx"), arguments);
        }

        public static string GetOpenLayerStringToAdd(int publishmentSystemID, int templateID, int nodeID, string includeUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("nodeID", nodeID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlInsert", true.ToString());
            arguments.Add("PLACE_HOLDER", string.Empty);
            return JsUtils.Layer.GetOpenLayerString("内容值（stl:content）新增", PageUtils.GetSTLUrl("modal_stlElementContent.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            int templateID = TranslateUtils.ToInt(base.GetQueryString("templateID"));
            this.templateInfo = TemplateManager.GetTemplateInfo(base.PublishmentSystemID, templateID);
            this.nodeID = TranslateUtils.ToInt(base.GetQueryString("nodeID"));
            this.includeUrl = base.GetQueryString("includeUrl");
            this.isStlInsert = TranslateUtils.ToBool(base.GetQueryString("isStlInsert"));

            this.stlIndex = TranslateUtils.ToInt(base.GetQueryString("stlIndex"));
            this.oldStlElement = RuntimeUtils.DecryptStringByTranslate(base.GetQueryString("stlEncryptElement"));

            if (!IsPostBack)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID);
                StringCollection attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(nodeInfo.Additional.ContentAttributesOfDisplay);

                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
                ArrayList columnTableStyleInfoArrayList = ContentUtility.GetColumnTableStyleInfoArrayList(base.PublishmentSystemInfo, tableStyle, tableStyleInfoArrayList);
                foreach (TableStyleInfo styleInfo in columnTableStyleInfoArrayList)
                {
                    ListItem listItem = new ListItem(styleInfo.DisplayName + "(" + styleInfo.AttributeName + ")", styleInfo.AttributeName);
                    this.ddlType.Items.Add(listItem);
                }

                this.ddlType.Items.Insert(0, new ListItem(string.Empty, string.Empty));
            }

            string stlElement = this.GetStlElement();

            LowerNameValueCollection attributes = StlParserUtility.GetAttributes(stlElement, false);
           
            if (!string.IsNullOrEmpty(attributes[StlContent.Attribute_Type]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlType, attributes[StlContent.Attribute_Type]);
            }
            if (!string.IsNullOrEmpty(attributes[StlContent.Attribute_WordNum]))
            {
                int wordNum = TranslateUtils.ToInt(attributes[StlContent.Attribute_WordNum]);
                this.tbWordNum.Text = wordNum == 0 ? string.Empty : wordNum.ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlContent.Attribute_IsClearTags]))
            {
                this.cbIsClearTags.Checked = TranslateUtils.ToBool(attributes[StlContent.Attribute_IsClearTags]);
            }
            if (!string.IsNullOrEmpty(attributes[StlContent.Attribute_IsReturnToBR]))
            {
                this.cbIsReturnToBR.Checked = TranslateUtils.ToBool(attributes[StlContent.Attribute_IsReturnToBR]);
            }
            if (!string.IsNullOrEmpty(attributes[StlContent.Attribute_IsUpper]))
            {
                this.cbIsUpper.Checked = TranslateUtils.ToBool(attributes[StlContent.Attribute_IsUpper]);
            }
            if (!string.IsNullOrEmpty(attributes[StlContent.Attribute_IsLower]))
            {
                this.cbIsLower.Checked = TranslateUtils.ToBool(attributes[StlContent.Attribute_IsLower]);
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

                TranslateUtils.SetOrRemoveAttribute(attributes, "type", this.ddlType.SelectedValue);
                TranslateUtils.SetOrRemoveAttribute(attributes, "wordNum", TranslateUtils.ToInt(this.tbWordNum.Text, 0) > 0 ? this.tbWordNum.Text : string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "isClearTags", this.cbIsClearTags.Checked.ToString().ToLower());
                TranslateUtils.SetOrRemoveAttribute(attributes, "isReturnToBR", this.cbIsReturnToBR.Checked.ToString().ToLower());
                TranslateUtils.SetOrRemoveAttribute(attributes, "isUpper", this.cbIsUpper.Checked.ToString().ToLower());
                TranslateUtils.SetOrRemoveAttribute(attributes, "isLower", this.cbIsLower.Checked.ToString().ToLower());

                stlElement = StlParserUtility.GetStlElement(StlContent.ElementName, attributes, null);
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
