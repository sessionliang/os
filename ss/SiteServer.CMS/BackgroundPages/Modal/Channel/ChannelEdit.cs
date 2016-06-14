using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;



namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class ChannelEdit : BackgroundBasePage
    {
        public Control NodeNameRow;
        public Control NodeIndexNameRow;
        public Control LinkUrlRow;
        public Control NodeGroupNameCollectionRow;
        public Control LinkTypeRow;
        public Control ChannelTemplateIDRow;
        public Control ContentTemplateIDRow;
        public Control ImageUrlRow;
        public Control FilePathRow;
        public Control ContentRow;
        public Control KeywordsRow;
        public Control DescriptionRow;

        public TextBox NodeName;
        public TextBox NodeIndexName;
        public TextBox LinkUrl;
        public CheckBoxList NodeGroupNameCollection;
        public DropDownList LinkType;
        public DropDownList ChannelTemplateID;
        public DropDownList ContentTemplateID;
        public TextBox tbImageUrl;
        public Literal ltlImageUrlButtonGroup;
        public TextBox FilePath;
        public TextBox Keywords;
        public TextBox Description;

        public TextEditorControl Content;

        public ChannelAuxiliaryControl channelControl = null;

        public Button btnSubmit;

        private int nodeID;
        private string returnUrl;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, string returnUrl)
        {
            return ChannelLoading.GetChannelEditOpenWindowString(publishmentSystemID, nodeID, returnUrl);
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtils.AddQueryString(PageUtils.GetCMSUrl("modal_channelEdit.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ReturnUrl");
            this.nodeID = base.GetIntQueryString("NodeID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));

            this.channelControl = (ChannelAuxiliaryControl)base.FindControl("ControlForAuxiliary");
            if (!IsPostBack)
            {
                if (!base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.ChannelEdit))
                {
                    PageUtils.RedirectToErrorPage("您没有修改栏目的权限！");
                    return;
                }

                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
                if (nodeInfo != null)
                {
                    if (nodeInfo.NodeType == ENodeType.BackgroundPublishNode)
                    {
                        this.LinkUrlRow.Visible = false;
                        this.LinkTypeRow.Visible = false;
                        this.ChannelTemplateIDRow.Visible = false;
                        this.FilePathRow.Visible = false;
                    }

                    this.btnSubmit.Attributes.Add("onclick", "if (UE && UE.getEditor('Content')){ UE.getEditor('Content').sync(); }");

                    if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.ChannelEditAttributes))
                    {
                        ArrayList channelEditAttributes = TranslateUtils.StringCollectionToArrayList(base.PublishmentSystemInfo.Additional.ChannelEditAttributes);
                        if (channelEditAttributes.Count > 0)
                        {
                            this.NodeNameRow.Visible = this.NodeIndexNameRow.Visible = this.LinkUrlRow.Visible = this.NodeGroupNameCollectionRow.Visible = this.LinkTypeRow.Visible = this.ChannelTemplateIDRow.Visible = this.ContentTemplateIDRow.Visible = this.ImageUrlRow.Visible = this.FilePathRow.Visible = this.ContentRow.Visible = this.KeywordsRow.Visible = this.DescriptionRow.Visible = false;
                            foreach (string attribute in channelEditAttributes)
                            {
                                if (attribute == NodeAttribute.ChannelName)
                                {
                                    this.NodeNameRow.Visible = true;
                                }
                                else if (attribute == NodeAttribute.ChannelIndex)
                                {
                                    this.NodeIndexNameRow.Visible = true;
                                }
                                else if (attribute == NodeAttribute.LinkUrl)
                                {
                                    this.LinkUrlRow.Visible = true;
                                }
                                else if (attribute == NodeAttribute.ChannelGroupNameCollection)
                                {
                                    this.NodeGroupNameCollectionRow.Visible = true;
                                }
                                else if (attribute == NodeAttribute.LinkType)
                                {
                                    this.LinkTypeRow.Visible = true;
                                }
                                else if (attribute == NodeAttribute.ChannelTemplateID)
                                {
                                    this.ChannelTemplateIDRow.Visible = true;
                                }
                                else if (attribute == NodeAttribute.ContentTemplateID)
                                {
                                    this.ContentTemplateIDRow.Visible = true;
                                }
                                else if (attribute == NodeAttribute.ImageUrl)
                                {
                                    this.ImageUrlRow.Visible = true;
                                }
                                else if (attribute == NodeAttribute.FilePath)
                                {
                                    this.FilePathRow.Visible = true;
                                }
                                else if (attribute == NodeAttribute.Content)
                                {
                                    this.ContentRow.Visible = true;
                                }
                                else if (attribute == NodeAttribute.Keywords)
                                {
                                    this.KeywordsRow.Visible = true;
                                }
                                else if (attribute == NodeAttribute.Description)
                                {
                                    this.DescriptionRow.Visible = true;
                                }
                            }
                        }
                    }

                    if (this.channelControl.Visible)
                    {
                        ArrayList displayAttributes = null;
                        if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.Additional.ChannelEditAttributes))
                        {
                            displayAttributes = TranslateUtils.StringCollectionToArrayList(base.PublishmentSystemInfo.Additional.ChannelEditAttributes);
                        }
                        this.channelControl.SetParameters(nodeInfo.Additional.Attributes, true, base.IsPostBack, displayAttributes);
                    }

                    if (this.LinkTypeRow.Visible)
                    {
                        ELinkTypeUtils.AddListItems(LinkType);
                    }

                    if (this.NodeGroupNameCollectionRow.Visible)
                    {
                        NodeGroupNameCollection.DataSource = DataProvider.NodeGroupDAO.GetDataSource(base.PublishmentSystemID);
                    }
                    if (this.ChannelTemplateIDRow.Visible)
                    {
                        ChannelTemplateID.DataSource = DataProvider.TemplateDAO.GetDataSourceByType(base.PublishmentSystemID, ETemplateType.ChannelTemplate);
                    }
                    if (this.ContentTemplateIDRow.Visible)
                    {
                        ContentTemplateID.DataSource = DataProvider.TemplateDAO.GetDataSourceByType(base.PublishmentSystemID, ETemplateType.ContentTemplate);
                    }

                    DataBind();

                    if (this.ChannelTemplateIDRow.Visible)
                    {
                        ChannelTemplateID.Items.Insert(0, new ListItem("<未设置>", "0"));
                        ControlUtils.SelectListItems(ChannelTemplateID, nodeInfo.ChannelTemplateID.ToString());
                    }

                    if (this.ContentTemplateIDRow.Visible)
                    {
                        ContentTemplateID.Items.Insert(0, new ListItem("<未设置>", "0"));
                        ControlUtils.SelectListItems(ContentTemplateID, nodeInfo.ContentTemplateID.ToString());
                    }

                    if (this.NodeNameRow.Visible)
                    {
                        NodeName.Text = nodeInfo.NodeName;
                    }
                    if (this.NodeIndexNameRow.Visible)
                    {
                        NodeIndexName.Text = nodeInfo.NodeIndexName;
                    }
                    if (this.LinkUrlRow.Visible)
                    {
                        LinkUrl.Text = nodeInfo.LinkUrl;
                    }

                    if (this.NodeGroupNameCollectionRow.Visible)
                    {
                        foreach (ListItem item in NodeGroupNameCollection.Items)
                        {
                            if (CompareUtils.Contains(nodeInfo.NodeGroupNameCollection, item.Value))
                            {
                                item.Selected = true;
                            }
                            else
                            {
                                item.Selected = false;
                            }
                        }
                    }
                    if (this.FilePathRow.Visible)
                    {
                        this.FilePath.Text = nodeInfo.FilePath;
                    }

                    if (this.LinkTypeRow.Visible)
                    {
                        ControlUtils.SelectListItems(LinkType, ELinkTypeUtils.GetValue(nodeInfo.LinkType));
                    }

                    if (this.ImageUrlRow.Visible)
                    {
                        this.tbImageUrl.Text = nodeInfo.ImageUrl;
                        this.ltlImageUrlButtonGroup.Text = StringUtility.Controls.GetImageUrlButtonGroupHtml(base.PublishmentSystemInfo, this.tbImageUrl.ClientID);
                    }
                    if (this.ContentRow.Visible)
                    {
                        NameValueCollection formCollection = new NameValueCollection();
                        formCollection[NodeAttribute.Content] = nodeInfo.Content;
                        this.Content.SetParameters(base.PublishmentSystemInfo, NodeAttribute.Content, formCollection, true, base.IsPostBack);

                        //this.Content.PublishmentSystemID = base.PublishmentSystemID;
                        //this.Content.Text = StringUtility.TextEditorContentDecode(nodeInfo.Content, ConfigUtils.Instance.ApplicationPath, base.PublishmentSystemInfo.PublishmentSystemUrl);
                    }
                    if (this.Keywords.Visible)
                    {
                        this.Keywords.Text = nodeInfo.Keywords;
                    }
                    if (this.Description.Visible)
                    {
                        this.Description.Text = nodeInfo.Description;
                    }


                }
            }
            else
            {
                if (this.channelControl.Visible)
                {
                    this.channelControl.SetParameters(base.Request.Form, true, base.IsPostBack);
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                bool isChanged = false;

                NodeInfo nodeInfo = null;
                try
                {
                    nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);

                    if (this.NodeIndexNameRow.Visible)
                    {
                        if (!nodeInfo.NodeIndexName.Equals(NodeIndexName.Text) && NodeIndexName.Text.Length != 0)
                        {
                            ArrayList NodeIndexNameList = DataProvider.NodeDAO.GetNodeIndexNameArrayList(base.PublishmentSystemID);
                            if (NodeIndexNameList.IndexOf(NodeIndexName.Text) != -1)
                            {
                                base.FailMessage("栏目修改失败，栏目索引已存在！");
                                return;
                            }
                        }
                    }

                    if (this.FilePathRow.Visible)
                    {
                        this.FilePath.Text = this.FilePath.Text.Trim();
                        if (!nodeInfo.FilePath.Equals(FilePath.Text) && FilePath.Text.Length != 0)
                        {
                            if (!DirectoryUtils.IsDirectoryNameCompliant(FilePath.Text))
                            {
                                base.FailMessage("栏目页面路径不符合系统要求！");
                                return;
                            }

                            if (PathUtils.IsDirectoryPath(this.FilePath.Text))
                            {
                                this.FilePath.Text = PageUtils.Combine(this.FilePath.Text, "index.html");
                            }

                            ArrayList filePathArrayList = DataProvider.NodeDAO.GetAllFilePathByPublishmentSystemID(base.PublishmentSystemID);
                            if (filePathArrayList.IndexOf(this.FilePath.Text) != -1)
                            {
                                base.FailMessage("栏目修改失败，栏目页面路径已存在！");
                                return;
                            }
                        }
                    }

                    if (this.channelControl.Visible)
                    {
                        ExtendedAttributes extendedAttributes = new ExtendedAttributes();
                        ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID);
                        InputTypeParser.AddValuesToAttributes(ETableStyle.Channel, DataProvider.NodeDAO.TableName, base.PublishmentSystemInfo, relatedIdentities, base.Request.Form, extendedAttributes.Attributes);
                        if (extendedAttributes.Attributes.Count > 0)
                        {
                            nodeInfo.Additional.SetExtendedAttribute(extendedAttributes.Attributes);
                        }
                    }

                    if (this.NodeNameRow.Visible)
                    {
                        nodeInfo.NodeName = NodeName.Text;
                    }
                    if (this.NodeIndexNameRow.Visible)
                    {
                        nodeInfo.NodeIndexName = NodeIndexName.Text;
                    }
                    if (this.FilePathRow.Visible)
                    {
                        nodeInfo.FilePath = FilePath.Text;
                    }

                    if (this.NodeGroupNameCollectionRow.Visible)
                    {
                        ArrayList list = new ArrayList();
                        foreach (ListItem item in NodeGroupNameCollection.Items)
                        {
                            if (item.Selected)
                            {
                                list.Add(item.Value);
                            }
                        }
                        nodeInfo.NodeGroupNameCollection = TranslateUtils.ObjectCollectionToString(list);
                    }
                    if (this.ImageUrlRow.Visible)
                    {
                        nodeInfo.ImageUrl = this.tbImageUrl.Text;
                    }
                    if (this.ContentRow.Visible)
                    {
                        nodeInfo.Content = StringUtility.TextEditorContentEncode(base.Request.Form[NodeAttribute.Content], base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.IsSaveImageInTextEditor);
                    }
                    if (this.Keywords.Visible)
                    {
                        nodeInfo.Keywords = this.Keywords.Text;
                    }
                    if (this.Description.Visible)
                    {
                        nodeInfo.Description = this.Description.Text;
                    }



                    if (this.LinkUrlRow.Visible)
                    {
                        nodeInfo.LinkUrl = LinkUrl.Text;
                    }
                    if (this.LinkTypeRow.Visible)
                    {
                        nodeInfo.LinkType = ELinkTypeUtils.GetEnumType(LinkType.SelectedValue);
                    }
                    if (this.ChannelTemplateIDRow.Visible)
                    {
                        nodeInfo.ChannelTemplateID = (ChannelTemplateID.Items.Count > 0) ? int.Parse(ChannelTemplateID.SelectedValue) : 0;
                    }
                    if (this.ContentTemplateIDRow.Visible)
                    {
                        nodeInfo.ContentTemplateID = (ContentTemplateID.Items.Count > 0) ? int.Parse(ContentTemplateID.SelectedValue) : 0;
                    }

                    DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "修改栏目", string.Format("栏目:{0}", nodeInfo.NodeName));

                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("栏目修改失败：{0}", ex.Message));
                    LogUtils.AddErrorLog(ex);
                }

                if (isChanged)
                {
                    string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Edit, ETemplateType.ChannelTemplate, this.nodeID, 0, 0);
                    AjaxUrlManager.AddAjaxUrl(ajaxUrl);

                    if (string.IsNullOrEmpty(this.returnUrl))
                    {
                        JsUtils.Layer.CloseModalLayer(Page);
                    }
                    else
                    {
                        JsUtils.Layer.CloseModalLayerAndRedirect(Page, this.returnUrl);
                    }
                }
            }
        }
    }
}
