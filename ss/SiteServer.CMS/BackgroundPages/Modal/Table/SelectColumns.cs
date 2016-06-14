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
            return PageUtility.GetOpenWindowString("ѡ����Ҫ��ʾ����", "modal_selectColumns.aspx", arguments, 520, 550);
        }

        public static string GetOpenWindowStringToContent(int publishmentSystemID, int relatedIdentity, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", relatedIdentity.ToString());
            arguments.Add("IsContent", true.ToString());
            arguments.Add("IsList", isList.ToString());
            return PageUtility.GetOpenWindowString("ѡ����Ҫ��ʾ����", "modal_selectColumns.aspx", arguments, 520, 550);
        }

        public static string GetOpenWindowStringToInputContent(int publishmentSystemID, int relatedIdentity, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("IsList", isList.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.InputContent));
            arguments.Add("RelatedIdentity", relatedIdentity.ToString());
            return PageUtility.GetOpenWindowString("ѡ����Ҫ��ʾ����", "modal_selectColumns.aspx", arguments, 520, 550);
        }

        public static string GetOpenWindowStringToWebsiteMessageContent(int publishmentSystemID, int relatedIdentity, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("IsList", isList.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.WebsiteMessageContent));
            arguments.Add("RelatedIdentity", relatedIdentity.ToString());
            return PageUtility.GetOpenWindowString("ѡ����Ҫ��ʾ����", "modal_selectColumns.aspx", arguments, 520, 550);
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
                    //���Ĭ������
                    ListItem listitem = new ListItem("��Ŀ����", NodeAttribute.ChannelName);
                    if (CompareUtils.Contains(displayAttributes, NodeAttribute.ChannelName))
                    {
                        listitem.Selected = true;
                    }
                    this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                    listitem = new ListItem("��Ŀ����", NodeAttribute.ChannelIndex);
                    if (CompareUtils.Contains(displayAttributes, NodeAttribute.ChannelIndex))
                    {
                        listitem.Selected = true;
                    }
                    this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                    listitem = new ListItem("����ҳ��·��", NodeAttribute.FilePath);
                    if (CompareUtils.Contains(displayAttributes, NodeAttribute.FilePath))
                    {
                        listitem.Selected = true;
                    }
                    this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                    if (!isList)
                    {
                        listitem = new ListItem("��ĿͼƬ��ַ", NodeAttribute.ImageUrl);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.ImageUrl))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("��Ŀ����", NodeAttribute.Content);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.Content))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("�ⲿ����", NodeAttribute.LinkUrl);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.LinkUrl))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("��������", NodeAttribute.LinkUrl);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.LinkUrl))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("��Ŀģ��", NodeAttribute.ChannelTemplateID);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.ChannelTemplateID))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("����ģ��", NodeAttribute.ContentTemplateID);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.ContentTemplateID))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("�ؼ����б�", NodeAttribute.Keywords);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.Keywords))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);

                        listitem = new ListItem("ҳ������", NodeAttribute.Description);
                        if (CompareUtils.Contains(displayAttributes, NodeAttribute.Description))
                        {
                            listitem.Selected = true;
                        }
                        this.DisplayAttributeCheckBoxList.Items.Add(listitem);
                    }

                    listitem = new ListItem("��Ŀ��", NodeAttribute.ChannelGroupNameCollection);
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
                        base.FailMessage("��������ѡ��һ�");
                        return;
                    }
                    base.PublishmentSystemInfo.Additional.ChannelEditAttributes = displayAttributes;

                    StringUtility.AddLog(base.PublishmentSystemID, "������Ŀ�༭��", string.Format("�༭��:{0}", displayAttributes));
                }
                else
                {
                    if (!CompareUtils.Contains(displayAttributes, NodeAttribute.ChannelName))
                    {
                        base.FailMessage("����ѡ����Ŀ�����");
                        return;
                    }
                    if (!CompareUtils.Contains(displayAttributes, NodeAttribute.ChannelIndex))
                    {
                        base.FailMessage("����ѡ����Ŀ�����");
                        return;
                    }
                    base.PublishmentSystemInfo.Additional.ChannelDisplayAttributes = displayAttributes;

                    StringUtility.AddLog(base.PublishmentSystemID, "������Ŀ��ʾ��", string.Format("��ʾ��:{0}", displayAttributes));
                }
                DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
            }
            else if (ETableStyleUtils.IsContent(tableStyle))
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.relatedIdentity);
                string attributesOfDisplay = ControlUtils.SelectedItemsValueToStringCollection(this.DisplayAttributeCheckBoxList.Items);
                nodeInfo.Additional.ContentAttributesOfDisplay = attributesOfDisplay;

                DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

                StringUtility.AddLog(base.PublishmentSystemID, "����������ʾ��", string.Format("��ʾ��:{0}", attributesOfDisplay));
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
                    StringUtility.AddLog(base.PublishmentSystemID, "�����ύ����ʾ��", string.Format("�����ƣ�{0},��ʾ��:{1}", inputInfo.InputName, TranslateUtils.ObjectCollectionToString(selectedValues)));
                }
                else
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "�����ύ���༭��", string.Format("�����ƣ�{0},�༭��:{1}", inputInfo.InputName, TranslateUtils.ObjectCollectionToString(selectedValues)));
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
                    StringUtility.AddLog(base.PublishmentSystemID, "������վ������ʾ��", string.Format("�����ƣ�{0},��ʾ��:{1}", websiteMessageInfo.WebsiteMessageName, TranslateUtils.ObjectCollectionToString(selectedValues)));
                }
                else
                {
                    StringUtility.AddLog(base.PublishmentSystemID, "������վ���Ա༭��", string.Format("�����ƣ�{0},�༭��:{1}", websiteMessageInfo.WebsiteMessageName, TranslateUtils.ObjectCollectionToString(selectedValues)));
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
