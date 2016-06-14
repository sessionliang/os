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
    public class StlElementChannels : BackgroundBasePage
    {
        public Literal ltlChannels;
        public Literal ltlSetting;

        public DropDownList ddlIsAllChildren;
        public DropDownList ddlOrder;
        public TextBox tbStartNum;
        public TextBox tbTotalNum;
        public DropDownList ddlGroup;
        public DropDownList ddlGroupNot;

        public TextBox tbItemTemplate;

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
            return JsUtils.Layer.GetOpenLayerString("栏目列表（stl:channels）编辑", PageUtils.GetSTLUrl("modal_stlElementChannels.aspx"), arguments);
        }

        public static string GetOpenLayerStringToAdd(int publishmentSystemID, int templateID, string includeUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("templateID", templateID.ToString());
            arguments.Add("includeUrl", includeUrl);
            arguments.Add("isStlInsert", true.ToString());
            arguments.Add("PLACE_HOLDER", string.Empty);
            return JsUtils.Layer.GetOpenLayerString("栏目列表（stl:channels）新增", PageUtils.GetSTLUrl("modal_stlElementChannels.aspx"), arguments);
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
                EBooleanUtils.AddListItems(this.ddlIsAllChildren, "显示所有级别的子栏目", "只显示子栏目，不包括子栏目的子栏目");
                this.ddlIsAllChildren.Items.Insert(0, new ListItem(string.Empty, string.Empty));
                EStlChannelOrderUtils.AddListItems(this.ddlOrder);
                this.ddlOrder.Items.Insert(0, new ListItem(string.Empty, string.Empty));

                this.ddlGroup.Items.Add(new ListItem(string.Empty, string.Empty));
                this.ddlGroupNot.Items.Add(new ListItem(string.Empty, string.Empty));
                ArrayList groupNameArrayList = DataProvider.NodeGroupDAO.GetNodeGroupNameArrayList(base.PublishmentSystemID);
                foreach (string groupName in groupNameArrayList)
                {
                    this.ddlGroup.Items.Add(new ListItem(groupName, groupName));
                    this.ddlGroupNot.Items.Add(new ListItem(groupName, groupName));
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

            string stlElement = this.GetStlElement();

            LowerNameValueCollection attributes = StlParserUtility.GetAttributes(stlElement, false);

            if (!string.IsNullOrEmpty(attributes[StlChannels.Attribute_ChannelIndex]))
            {
                this.ltlSetting.Text = string.Format("$('#channelIndex').val('{0}');", attributes[StlChannels.Attribute_ChannelIndex]);
            }
            if (!string.IsNullOrEmpty(attributes[StlChannels.Attribute_ChannelName]))
            {
                this.ltlSetting.Text += string.Format("$('#channelName').val('{0}');", attributes[StlChannels.Attribute_ChannelName]);
            }
            if (!string.IsNullOrEmpty(attributes[StlChannels.Attribute_Order]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlOrder, attributes[StlChannels.Attribute_Order]);
            }
            if (!string.IsNullOrEmpty(attributes[StlChannels.Attribute_IsAllChildren]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlIsAllChildren, attributes[StlChannels.Attribute_IsAllChildren]);
            }
            if (!string.IsNullOrEmpty(attributes[StlChannels.Attribute_StartNum]))
            {
                this.tbStartNum.Text = TranslateUtils.ToInt(attributes[StlChannels.Attribute_StartNum], 1).ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlChannels.Attribute_TotalNum]))
            {
                this.tbTotalNum.Text = TranslateUtils.ToInt(attributes[StlChannels.Attribute_TotalNum], 0).ToString();
            }
            if (!string.IsNullOrEmpty(attributes[StlChannels.Attribute_Group]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroup, attributes[StlChannels.Attribute_Group]);
            }
            if (!string.IsNullOrEmpty(attributes[StlChannels.Attribute_GroupChannel]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroup, attributes[StlChannels.Attribute_GroupChannel]);
            }
            if (!string.IsNullOrEmpty(attributes[StlChannels.Attribute_GroupNot]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroupNot, attributes[StlChannels.Attribute_GroupNot]);
            }
            if (!string.IsNullOrEmpty(attributes[StlChannels.Attribute_GroupChannelNot]))
            {
                ControlUtils.SelectListItemsIgnoreCase(this.ddlGroupNot, attributes[StlChannels.Attribute_GroupChannelNot]);
            }

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
                TranslateUtils.SetOrRemoveAttribute(attributes, "isAllChildren", this.ddlIsAllChildren.SelectedValue.ToLower());
                TranslateUtils.SetOrRemoveAttribute(attributes, "order", this.ddlOrder.SelectedValue);
                TranslateUtils.SetOrRemoveAttribute(attributes, "startNum", TranslateUtils.ToInt(this.tbStartNum.Text, 1) > 0 ? this.tbStartNum.Text : string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "totalNum", TranslateUtils.ToInt(this.tbTotalNum.Text, 0) > 0 ? this.tbTotalNum.Text : string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "groupChannel", string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "group", this.ddlGroup.SelectedValue);
                TranslateUtils.SetOrRemoveAttribute(attributes, "groupChannelNot", string.Empty);
                TranslateUtils.SetOrRemoveAttribute(attributes, "groupNot", this.ddlGroupNot.SelectedValue);

                stlElement = StlParserUtility.GetStlElement(StlChannels.ElementName, attributes, this.tbItemTemplate.Text);
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
