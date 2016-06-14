using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class SelectColumns : BackgroundBasePage
    {
        public CheckBoxList DisplayAttributeCheckBoxList;

        private int relatedIdentity;
        private ArrayList relatedIdentities;
        private ETableStyle tableStyle;
        private bool isList;

        public static string GetOpenWindowStringToChannel(int publishmentSystemID, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", publishmentSystemID.ToString());
            arguments.Add("IsList", isList.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.Channel));
            return PageUtility.GetOpenWindowString("选择需要显示的项", "modal_selectColumns.aspx", arguments, 520, 550);
        }

        public static string GetOpenWindowStringToContent(int publishmentSystemID, int relatedIdentity, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", relatedIdentity.ToString());
            arguments.Add("IsContent", true.ToString());
            arguments.Add("IsList", isList.ToString());
            return PageUtility.GetOpenWindowString("选择需要显示的项", "modal_selectColumns.aspx", arguments, 520, 550);
        }

        public static string GetOpenWindowStringToInputContent(int publishmentSystemID, int relatedIdentity, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("IsList", isList.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.InputContent));
            arguments.Add("RelatedIdentity", relatedIdentity.ToString());
            return PageUtility.GetOpenWindowString("选择需要显示的项", "modal_selectColumns.aspx", arguments, 520, 550);
        }

        public static string GetOpenWindowStringToWebsiteMessageContent(int publishmentSystemID, int relatedIdentity, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("IsList", isList.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.WebsiteMessageContent));
            arguments.Add("RelatedIdentity", relatedIdentity.ToString());
            return PageUtility.GetOpenWindowString("选择需要显示的项", "modal_selectColumns.aspx", arguments, 520, 550);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.relatedIdentity = base.GetIntQueryString("RelatedIdentity");
            this.isList = base.GetBoolQueryString("IsList");
            if (TranslateUtils.ToBool(base.GetQueryString("IsContent")))
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.relatedIdentity);
                this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            }
            else
            {
                this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            }

            if (this.tableStyle == ETableStyle.Channel)
            {
                string displayAttributes = base.PublishmentSystemInfo.Additional.ChannelDisplayAttributes;
                if (!this.isList)
                {
                    displayAttributes = base.PublishmentSystemInfo.Additional.ChannelEditAttributes;
                }

                if (!IsPostBack)
                {
                    //添加默认属性
                    ListItem listitem = new ListItem("栏目名称", NodeAttribute.ChannelName);
                    if (CompareUtils.Contains(displayAttributes, NodeAttribute.ChannelName))
                    {
                        listitem.Selected = true;
                    }
                    this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                    listitem = new ListItem("栏目索引", NodeAttribute.ChannelIndex);
                    if (CompareUtils.Contains(displayAttributes, NodeAttribute.ChannelIndex))
                    {
                        listitem.Selected = true;
                    }
                    this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                    listitem = new ListItem("生成页面路径", NodeAttribute.FilePath);
                    if (CompareUtils.Contains(displayAttributes, NodeAttribute.FilePath))
                    {
                        listitem.Selected = true;
                    }
                    this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                    if (!isList)
                    {
                        listitem = new ListItem("栏目图片地址", NodeAttribute.ImageUrl);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.ImageUrl))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("栏目正文", NodeAttribute.Content);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.Content))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("外部链接", NodeAttribute.LinkUrl);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.LinkUrl))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("链接类型", NodeAttribute.LinkUrl);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.LinkUrl))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("栏目模版", NodeAttribute.ChannelTemplateID);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.ChannelTemplateID))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("内容模版", NodeAttribute.ContentTemplateID);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.ContentTemplateID))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("关键字列表", NodeAttribute.Keywords);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.Keywords))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("页面描述", NodeAttribute.Description);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.Description))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);
                    }

                    listitem = new ListItem("栏目组", NodeAttribute.ChannelGroupNameCollection);
                    if (CompareUtils.Contains(displayAttributes, NodeAttribute.ChannelGroupNameCollection))
                    {
                        listitem.Selected = true;
                    }
                    this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                    ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, DataProvider.NodeDAO.TableName, this.relatedIdentities);

                    foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                    {
                        if (styleInfo.IsVisible == false) continue;
                        listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);

                        if (CompareUtils.Contains(displayAttributes, styleInfo.AttributeName))
                        {
                            listitem.Selected = true;
                        }

                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);
                    }

                    if (string.IsNullOrEmpty(displayAttributes))
                    {
                        if (!this.isList)
                        {
                            foreach (ListItem item in this.DisplayAttributeCheckBoxList.Items)
                            {
                                item.Selected = true;
                            }
                        }
                        else
                        {
                            ControlUtils.SelectListItems(this.DisplayAttributeCheckBoxList, NodeAttribute.ChannelName, NodeAttribute.ChannelIndex);
                        }
                    }
                }
            }
            else if (ETableStyleUtils.IsContent(tableStyle))
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.relatedIdentity);
                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
                this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.relatedIdentity);
                StringCollection attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(nodeInfo.Additional.ContentAttributesOfDisplay);

                if (!IsPostBack)
                {
                    ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, tableName, this.relatedIdentities);
                    ArrayList columnTableStyleInfoArrayList = ContentUtility.GetColumnTableStyleInfoArrayList(base.PublishmentSystemInfo, this.tableStyle, tableStyleInfoArrayList);
                    foreach (TableStyleInfo styleInfo in columnTableStyleInfoArrayList)
                    {
                        if (styleInfo.AttributeName == BaiRong.Model.ContentAttribute.Title) continue;
                        ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);

                        if (this.isList)
                        {
                            if (attributesOfDisplay.Contains(styleInfo.AttributeName))
                            {
                                listitem.Selected = true;
                            }
                        }
                        else
                        {
                            if (styleInfo.IsVisible)
                            {
                                listitem.Selected = true;
                            }
                        }

                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);
                    }
                }
            }
            else if (this.tableStyle == ETableStyle.InputContent)
            {
                InputInfo inputInfo = DataProvider.InputDAO.GetInputInfo(this.relatedIdentity);
                this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(this.tableStyle, base.PublishmentSystemID, this.relatedIdentity);

                if (!IsPostBack)
                {
                    ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, DataProvider.InputContentDAO.TableName, this.relatedIdentities);

                    foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                    {
                        ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);

                        if (this.isList)
                        {
                            if (styleInfo.IsVisibleInList)
                            {
                                listitem.Selected = true;
                            }
                        }
                        else
                        {
                            if (styleInfo.IsVisible)
                            {
                                listitem.Selected = true;
                            }
                        }

                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);
                    }
                }
            }
            else if (this.tableStyle == ETableStyle.WebsiteMessageContent)
            {
                WebsiteMessageInfo websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(this.relatedIdentity);
                this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(this.tableStyle, base.PublishmentSystemID, this.relatedIdentity);

                if (!IsPostBack)
                {
                    ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, DataProvider.WebsiteMessageContentDAO.TableName, this.relatedIdentities);

                    foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                    {
                        ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);

                        if (this.isList)
                        {
                            if (styleInfo.IsVisibleInList)
                            {
                                listitem.Selected = true;
                            }
                        }
                        else
                        {
                            if (styleInfo.IsVisible)
                            {
                                listitem.Selected = true;
                            }
                        }

                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);
                    }
                }
            }


        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            string displayAttributes = ControlUtils.SelectedItemsValueToStringCollection(this.DisplayAttributeCheckBoxList.Items);
            if (tableStyle == ETableStyle.Channel)
            {
                if (!this.isList)
                {
                    if (this.DisplayAttributeCheckBoxList.Items.Count == 0)
                    {
                        base.FailMessage("必须至少选择一项！");
                        return;
                    }
                    base.PublishmentSystemInfo.Additional.ChannelEditAttributes = displayAttributes;

                    StringUtility.AddLog(base.PublishmentSystemID, "设置栏目编辑项", string.Format("编辑项:{0}", displayAttributes));
                }
                else
                {
                    if (!CompareUtils.Contains(displayAttributes, NodeAttribute.ChannelName))
                    {
                        base.FailMessage("必须选择栏目名称项！");
                        return;
                    }
                    if (!CompareUtils.Contains(displayAttributes, NodeAttribute.ChannelIndex))
                    {
                        base.FailMessage("必须选择栏目索引项！");
                        return;
                    }
                    base.PublishmentSystemInfo.Additional.ChannelDisplayAttributes = displayAttributes;

                    StringUtility.AddLog(base.PublishmentSystemID, "设置栏目显示项", string.Format("显示项:{0}", displayAttributes));
                }
                DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
            }
            else if (ETableStyleUtils.IsContent(tableStyle))
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.relatedIdentity);
                string attributesOfDisplay = ControlUtils.SelectedItemsValueToStringCollection(this.DisplayAttributeCheckBoxList.Items);
                nodeInfo.Additional.ContentAttributesOfDisplay = attributesOfDisplay;

                DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

                StringUtility.AddLog(base.PublishmentSystemID, "设置内容显示项", string.Format("显示项:{0}", attributesOfDisplay));
            }
            else if (tableStyle == ETableStyle.InputContent)
            {
                InputInfo inputInfo = DataProvider.InputDAO.GetInputInfo(this.relatedIdentity);

                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, DataProvider.InputContentDAO.TableName, this.relatedIdentities);
                ArrayList selectedValues = ControlUtils.GetSelectedListControlValueArrayList(this.DisplayAttributeCheckBoxList);

                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    if (this.isList)
                    {
                        styleInfo.IsVisibleInList = selectedValues.Contains(styleInfo.AttributeName);
                    }
                    else
                    {
                        styleInfo.IsVisible = selectedValues.Contains(styleInfo.AttributeName);
                    }
                    styleInfo.RelatedIdentity = this.relatedIdentity;

                    if (styleInfo.TableStyleID == 0)
                    {
                        TableStyleManager.Insert(styleInfo, tableStyle);
                    }
                    else
                    {
                        TableStyleManager.Update(styleInfo);
                    }
                }


                if (this.isList)
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "设置提交表单显示项", string.Format("表单名称：{0},显示项:{1}", inputInfo.InputName, TranslateUtils.ObjectCollectionToString(selectedValues)));
                }
                else
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "设置提交表单编辑项", string.Format("表单名称：{0},编辑项:{1}", inputInfo.InputName, TranslateUtils.ObjectCollectionToString(selectedValues)));
                }
            }
            else if (tableStyle == ETableStyle.WebsiteMessageContent)
            {
                WebsiteMessageInfo websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(this.relatedIdentity);

                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, DataProvider.WebsiteMessageContentDAO.TableName, this.relatedIdentities);
                ArrayList selectedValues = ControlUtils.GetSelectedListControlValueArrayList(this.DisplayAttributeCheckBoxList);

                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    if (this.isList)
                    {
                        styleInfo.IsVisibleInList = selectedValues.Contains(styleInfo.AttributeName);
                    }
                    else
                    {
                        styleInfo.IsVisible = selectedValues.Contains(styleInfo.AttributeName);
                    }
                    styleInfo.RelatedIdentity = this.relatedIdentity;

                    if (styleInfo.TableStyleID == 0)
                    {
                        TableStyleManager.Insert(styleInfo, tableStyle);
                    }
                    else
                    {
                        TableStyleManager.Update(styleInfo);
                    }
                }


                if (this.isList)
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "设置网站留言显示项", string.Format("表单名称：{0},显示项:{1}", websiteMessageInfo.WebsiteMessageName, TranslateUtils.ObjectCollectionToString(selectedValues)));
                }
                else
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "设置网站留言编辑项", string.Format("表单名称：{0},编辑项:{1}", websiteMessageInfo.WebsiteMessageName, TranslateUtils.ObjectCollectionToString(selectedValues)));
                }
            }

            if (!this.isList)
            {
                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page);
            }
            else
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }

    }
}
