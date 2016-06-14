using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;




namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundChannelEdit : BackgroundBasePage
    {
        public Control LinkURLRow;
        public Control LinkTypeRow;
        public Control ChannelTemplateIDRow;
        public Control FilePathRow;

        public TextBox NodeName;
        public TextBox NodeIndexName;
        public DropDownList ContentModelID;
        public TextBox LinkUrl;
        public CheckBoxList NodeGroupNameCollection;
        public RadioButtonList IsChannelAddable;
        public RadioButtonList IsContentAddable;
        public RadioButtonList IsUseEvaluation;//by 20160224 sofuny 增加是否开启评价管理功能
        public RadioButtonList IsUseTrial;//by 20160303 sofuny 增加是否开启试用管理功能
        public RadioButtonList IsUseSurvey;//by 20160309 sofuny 增加是否开启调查问卷功能
        public RadioButtonList IsUseCompare;//by 20160316 sofuny 增加是否开启比较功能 
        public RadioButtonList TypeList;

        //NodeProperty
        public DropDownList LinkType;
        public DropDownList ChannelTemplateID;
        public DropDownList ContentTemplateID;
        public DropDownList dpChangeType;
        public DropDownList Priority;
        public TextBox NavigationPicPath;
        public TextBox FilePath;
        public TextBox ChannelFilePathRule;
        public TextBox ContentFilePathRule;

        public TextEditorControl Content;
        public TextBox Keywords;
        public TextBox Description;
        public ChannelAuxiliaryControl channelControl = null;

        public Button CreateChannelRule;
        public Button CreateContentRule;
        public Button SelectImage;
        public Button UploadImage;
        public Button Submit;

        private int nodeID;
        private string returnUrl;

        public string PreviewImageSrc
        {
            get
            {
                if (!string.IsNullOrEmpty(this.NavigationPicPath.Text))
                {
                    string extension = PathUtils.GetExtension(this.NavigationPicPath.Text);
                    if (EFileSystemTypeUtils.IsImage(extension))
                    {
                        return PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, this.NavigationPicPath.Text);
                    }
                    else if (EFileSystemTypeUtils.IsFlash(extension))
                    {
                        return PageUtils.GetIconUrl("flash.jpg");
                    }
                    else if (EFileSystemTypeUtils.IsPlayer(extension))
                    {
                        return PageUtils.GetIconUrl("player.gif");
                    }
                }
                return PageUtils.GetIconUrl("empty.gif");
            }
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ReturnUrl"); 
            this.nodeID = base.GetIntQueryString("NodeID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));

            if (base.GetQueryString("CanNotEdit") == null && base.GetQueryString("UncheckedChannel") == null)
            {
                if (!base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.ChannelEdit))
                {
                    PageUtils.RedirectToErrorPage("您没有修改栏目的权限！");
                    return;
                }
            }
            if (base.GetQueryString("CanNotEdit") != null)
            {
                this.Submit.Visible = false;
            }

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
            if (nodeInfo != null)
            {
                this.channelControl = (ChannelAuxiliaryControl)base.FindControl("ControlForAuxiliary");
                if (!base.IsPostBack)
                {
                    base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "编辑栏目", string.Empty);

                    ArrayList contentModelArrayList = ContentModelManager.GetContentModelArrayList(base.PublishmentSystemInfo);
                    foreach (ContentModelInfo modelInfo in contentModelArrayList)
                    {
                        this.ContentModelID.Items.Add(new ListItem(modelInfo.ModelName, modelInfo.ModelID));
                    }
                    ControlUtils.SelectListItems(this.ContentModelID, nodeInfo.ContentModelID);

                    this.channelControl.SetParameters(nodeInfo.Additional.Attributes, true, base.IsPostBack);

                    this.NavigationPicPath.Attributes.Add("onchange", JsManager.GetShowImageScript("preview_NavigationPicPath", base.PublishmentSystemInfo.PublishmentSystemUrl));

                    string showPopWinString = Modal.FilePathRule.GetOpenWindowString(base.PublishmentSystemID, this.nodeID, true, this.ChannelFilePathRule.ClientID);
                    this.CreateChannelRule.Attributes.Add("onclick", showPopWinString);

                    showPopWinString = Modal.FilePathRule.GetOpenWindowString(base.PublishmentSystemID, this.nodeID, false, this.ContentFilePathRule.ClientID);
                    this.CreateContentRule.Attributes.Add("onclick", showPopWinString);

                    showPopWinString = Modal.SelectImage.GetOpenWindowString(base.PublishmentSystemInfo, this.NavigationPicPath.ClientID);
                    this.SelectImage.Attributes.Add("onclick", showPopWinString);

                    showPopWinString = Modal.UploadImage.GetOpenWindowString(base.PublishmentSystemID, 0, this.NavigationPicPath.ClientID);
                    this.UploadImage.Attributes.Add("onclick", showPopWinString);
                    this.IsChannelAddable.Items[0].Value = true.ToString();
                    this.IsChannelAddable.Items[1].Value = false.ToString();
                    this.IsContentAddable.Items[0].Value = true.ToString();
                    this.IsContentAddable.Items[1].Value = false.ToString();

                    #region by 20160224 sofuny 增加是否开启评价管理功能
                    EBooleanUtils.AddListItems(this.IsUseEvaluation, "是", "否");
                    EBooleanUtils.AddListItems(this.IsUseTrial, "是", "否");
                    EBooleanUtils.AddListItems(this.IsUseSurvey, "是", "否");
                    EBooleanUtils.AddListItems(this.IsUseCompare, "是", "否");
                    #endregion
                    ELinkTypeUtils.AddListItems(LinkType);

                    NodeGroupNameCollection.DataSource = DataProvider.NodeGroupDAO.GetDataSource(base.PublishmentSystemID);

                    ChannelTemplateID.DataSource = DataProvider.TemplateDAO.GetDataSourceByType(base.PublishmentSystemID, ETemplateType.ChannelTemplate);

                    ContentTemplateID.DataSource = DataProvider.TemplateDAO.GetDataSourceByType(base.PublishmentSystemID, ETemplateType.ContentTemplate);

                    DataBind();

                    ChannelTemplateID.Items.Insert(0, new ListItem("<未设置>", "0"));
                    ControlUtils.SelectListItems(ChannelTemplateID, nodeInfo.ChannelTemplateID.ToString());

                    ContentTemplateID.Items.Insert(0, new ListItem("<未设置>", "0"));
                    ControlUtils.SelectListItems(ContentTemplateID, nodeInfo.ContentTemplateID.ToString());

                    NodeName.Text = nodeInfo.NodeName;
                    NodeIndexName.Text = nodeInfo.NodeIndexName;
                    LinkUrl.Text = nodeInfo.LinkUrl;

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
                    this.FilePath.Text = nodeInfo.FilePath;
                    this.ChannelFilePathRule.Text = nodeInfo.ChannelFilePathRule;
                    this.ContentFilePathRule.Text = nodeInfo.ContentFilePathRule;

                    //NodeProperty
                    ControlUtils.SelectListItems(LinkType, ELinkTypeUtils.GetValue(nodeInfo.LinkType));
                    ControlUtils.SelectListItems(this.IsChannelAddable, nodeInfo.Additional.IsChannelAddable.ToString());
                    ControlUtils.SelectListItems(this.IsContentAddable, nodeInfo.Additional.IsContentAddable.ToString());
                    #region by 20160224 sofuny 增加是否开启评价管理功能
                    ControlUtils.SelectListItems(this.IsUseEvaluation, nodeInfo.Additional.IsUseEvaluation.ToString());
                    ControlUtils.SelectListItems(this.IsUseTrial, nodeInfo.Additional.IsUseTrial.ToString());
                    ControlUtils.SelectListItems(this.IsUseSurvey, nodeInfo.Additional.IsUseSurvey.ToString());
                    ControlUtils.SelectListItems(this.IsUseCompare, nodeInfo.Additional.IsUseCompare.ToString());
                    #endregion

                    NavigationPicPath.Text = nodeInfo.ImageUrl;

                    NameValueCollection formCollection = new NameValueCollection();
                    formCollection[NodeAttribute.Content] = nodeInfo.Content;
                    this.Content.SetParameters(base.PublishmentSystemInfo, NodeAttribute.Content, formCollection, true, base.IsPostBack);

                    this.Keywords.Text = nodeInfo.Keywords;
                    this.Description.Text = nodeInfo.Description;

                    //this.Content.PublishmentSystemID = base.PublishmentSystemID;
                    //this.Content.Text = StringUtility.TextEditorContentDecode(nodeInfo.Content, ConfigUtils.Instance.ApplicationPath, base.PublishmentSystemInfo.PublishmentSystemUrl);

                    if (nodeInfo.NodeType == ENodeType.BackgroundPublishNode)
                    {
                        this.LinkURLRow.Visible = false;
                        this.LinkTypeRow.Visible = false;
                        this.ChannelTemplateIDRow.Visible = false;
                        this.FilePathRow.Visible = false;
                    }
                }
                else
                {
                    this.channelControl.SetParameters(base.Request.Form, true, base.IsPostBack);
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                NodeInfo nodeInfo = null;
                try
                {

                    nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
                    if (!nodeInfo.NodeIndexName.Equals(NodeIndexName.Text) && NodeIndexName.Text.Length != 0)
                    {
                        ArrayList NodeIndexNameList = DataProvider.NodeDAO.GetNodeIndexNameArrayList(base.PublishmentSystemID);
                        if (NodeIndexNameList.IndexOf(NodeIndexName.Text) != -1)
                        {
                            base.FailMessage("栏目属性修改失败，栏目索引已存在！");
                            return;
                        }
                    }

                    if (nodeInfo.ContentModelID != this.ContentModelID.SelectedValue)
                    {
                        nodeInfo.ContentModelID = this.ContentModelID.SelectedValue;
                        nodeInfo.ContentNum = BaiRongDataProvider.ContentDAO.GetCount(NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo.ContentModelID), nodeInfo.NodeID);
                    }

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

                    if (!string.IsNullOrEmpty(this.ChannelFilePathRule.Text))
                    {
                        string filePathRule = this.ChannelFilePathRule.Text.Replace("|", string.Empty);
                        if (!DirectoryUtils.IsDirectoryNameCompliant(filePathRule))
                        {
                            base.FailMessage("栏目页面命名规则不符合系统要求！");
                            return;
                        }
                        if (PathUtils.IsDirectoryPath(filePathRule))
                        {
                            base.FailMessage("栏目页面命名规则必须包含生成文件的后缀！");
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(this.ContentFilePathRule.Text))
                    {
                        string filePathRule = this.ContentFilePathRule.Text.Replace("|", string.Empty);
                        if (!DirectoryUtils.IsDirectoryNameCompliant(filePathRule))
                        {
                            base.FailMessage("内容页面命名规则不符合系统要求！");
                            return;
                        }
                        if (PathUtils.IsDirectoryPath(filePathRule))
                        {
                            base.FailMessage("内容页面命名规则必须包含生成文件的后缀！");
                            return;
                        }
                    }

                    ExtendedAttributes extendedAttributes = new ExtendedAttributes();
                    ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID);
                    InputTypeParser.AddValuesToAttributes(ETableStyle.Channel, DataProvider.NodeDAO.TableName, base.PublishmentSystemInfo, relatedIdentities, base.Request.Form, extendedAttributes.Attributes);
                    NameValueCollection attributes = extendedAttributes.Attributes;
                    foreach (string key in attributes)
                    {
                        nodeInfo.Additional.SetExtendedAttribute(key, attributes[key]);
                    }

                    nodeInfo.NodeName = NodeName.Text;
                    nodeInfo.NodeIndexName = NodeIndexName.Text;
                    nodeInfo.FilePath = FilePath.Text;
                    nodeInfo.ChannelFilePathRule = ChannelFilePathRule.Text;
                    nodeInfo.ContentFilePathRule = ContentFilePathRule.Text;

                    ArrayList list = new ArrayList();
                    foreach (ListItem item in NodeGroupNameCollection.Items)
                    {
                        if (item.Selected)
                        {
                            list.Add(item.Value);
                        }
                    }
                    nodeInfo.NodeGroupNameCollection = TranslateUtils.ObjectCollectionToString(list);
                    nodeInfo.ImageUrl = NavigationPicPath.Text;
                    nodeInfo.Content = StringUtility.TextEditorContentEncode(base.Request.Form[NodeAttribute.Content], base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.IsSaveImageInTextEditor);

                    nodeInfo.Keywords = this.Keywords.Text;
                    nodeInfo.Description = this.Description.Text;

                    nodeInfo.Additional.IsChannelAddable = TranslateUtils.ToBool(this.IsChannelAddable.SelectedValue);
                    nodeInfo.Additional.IsContentAddable = TranslateUtils.ToBool(this.IsContentAddable.SelectedValue);

                    #region by 20160224 sofuny 增加是否开启评价管理功能
                    nodeInfo.Additional.IsUseEvaluation = TranslateUtils.ToBool(this.IsUseEvaluation.SelectedValue);
                    nodeInfo.Additional.IsUseTrial = TranslateUtils.ToBool(this.IsUseTrial.SelectedValue);
                    nodeInfo.Additional.IsUseSurvey = TranslateUtils.ToBool(this.IsUseSurvey.SelectedValue);
                    nodeInfo.Additional.IsUseCompare = TranslateUtils.ToBool(this.IsUseCompare.SelectedValue);
                    #endregion

                    nodeInfo.LinkUrl = this.LinkUrl.Text;
                    nodeInfo.LinkType = ELinkTypeUtils.GetEnumType(LinkType.SelectedValue);
                    nodeInfo.ChannelTemplateID = (ChannelTemplateID.Items.Count > 0) ? int.Parse(ChannelTemplateID.SelectedValue) : 0;
                    nodeInfo.ContentTemplateID = (ContentTemplateID.Items.Count > 0) ? int.Parse(ContentTemplateID.SelectedValue) : 0;

                    DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("栏目修改失败：{0}", ex.Message));
                    LogUtils.AddErrorLog(ex);
                    return;
                }

                string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Edit, ETemplateType.ChannelTemplate, nodeInfo.NodeID, 0, 0);
                AjaxUrlManager.AddAjaxUrl(ajaxUrl);

                StringUtility.AddLog(base.PublishmentSystemID, "修改栏目", string.Format("栏目:{0}", NodeName.Text));

                base.SuccessMessage("栏目修改成功！");
                PageUtils.Redirect(this.returnUrl);
            }
        }

        public string ReturnUrl { get { return this.returnUrl; } }
    }
}