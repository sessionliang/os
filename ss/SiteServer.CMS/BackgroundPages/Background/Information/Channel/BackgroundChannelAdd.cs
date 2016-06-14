using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundChannelAdd : BackgroundBasePage
    {
        public DropDownList ParentNodeID;
        public TextBox NodeName;
        public TextBox NodeIndexName;
        public DropDownList ContentModelID;
        public TextBox LinkURL;
        public CheckBoxList NodeGroupNameCollection;
        public DropDownList LinkType;
        public DropDownList ChannelTemplateID;
        public DropDownList ContentTemplateID;
        public RadioButtonList IsChannelAddable;
        public RadioButtonList IsContentAddable;
        public RadioButtonList IsUseEvaluation;//by 20160224 sofuny 增加是否开启评价管理功能
        public RadioButtonList IsUseTrial;//by 20160303 sofuny 增加是否开启试用管理功能
        public RadioButtonList IsUseSurvey;//by 20160309 sofuny 增加是否开启调查问卷功能
        public RadioButtonList IsUseCompare;//by 20160316 sofuny 增加是否开启比较功能
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

        private int nodeID;
        private string returnUrl;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ReturnUrl");
            this.nodeID = base.GetIntQueryString("NodeID");
            this.returnUrl = StringUtils.ValueFromUrl(PageUtils.FilterSqlAndXss(base.GetQueryString("ReturnUrl")));
            //if (!base.HasChannelPermissions(this.nodeID, AppManager.CMS.Permission.Channel.ChannelAdd))
            //{
            //    PageUtils.RedirectToErrorPage("您没有添加栏目的权限！");
            //    return;
            //}

            NodeInfo parentNodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
            if (parentNodeInfo.Additional.IsChannelAddable == false)
            {
                PageUtils.RedirectToErrorPage("此栏目不能添加子栏目！");
                return;
            }

            this.channelControl = (ChannelAuxiliaryControl)base.FindControl("ControlForAuxiliary");

            if (!base.IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "添加栏目", string.Empty);

                NodeManager.AddListItems(this.ParentNodeID.Items, base.PublishmentSystemInfo, true, true, true);
                ControlUtils.SelectListItems(this.ParentNodeID, this.nodeID.ToString());

                ArrayList contentModelArrayList = ContentModelManager.GetContentModelArrayList(base.PublishmentSystemInfo);
                foreach (ContentModelInfo modelInfo in contentModelArrayList)
                {
                    this.ContentModelID.Items.Add(new ListItem(modelInfo.ModelName, modelInfo.ModelID));
                }
                ControlUtils.SelectListItems(this.ContentModelID, parentNodeInfo.ContentModelID);

                this.channelControl.SetParameters(null, false, base.IsPostBack);

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
                ControlUtils.SelectListItems(this.IsUseEvaluation, EBoolean.False.ToString());
                EBooleanUtils.AddListItems(this.IsUseTrial, "是", "否");
                ControlUtils.SelectListItems(this.IsUseTrial, EBoolean.False.ToString());
                EBooleanUtils.AddListItems(this.IsUseSurvey, "是", "否");
                ControlUtils.SelectListItems(this.IsUseSurvey, EBoolean.False.ToString());
                EBooleanUtils.AddListItems(this.IsUseCompare, "是", "否");
                ControlUtils.SelectListItems(this.IsUseCompare, EBoolean.False.ToString());
                #endregion 
                NodeGroupNameCollection.DataSource = DataProvider.NodeGroupDAO.GetDataSource(base.PublishmentSystemID);
                ChannelTemplateID.DataSource = DataProvider.TemplateDAO.GetDataSourceByType(base.PublishmentSystemID, ETemplateType.ChannelTemplate);
                ContentTemplateID.DataSource = DataProvider.TemplateDAO.GetDataSourceByType(base.PublishmentSystemID, ETemplateType.ContentTemplate);

                DataBind();

                ChannelTemplateID.Items.Insert(0, new ListItem("<未设置>", "0"));
                ChannelTemplateID.Items[0].Selected = true;

                ContentTemplateID.Items.Insert(0, new ListItem("<未设置>", "0"));
                ContentTemplateID.Items[0].Selected = true;
                this.Content.SetParameters(base.PublishmentSystemInfo, NodeAttribute.Content, null, false, base.IsPostBack);
            }
            else
            {
                this.channelControl.SetParameters(base.Request.Form, false, base.IsPostBack);
            }
        }

        public void ParentNodeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int theNodeID = TranslateUtils.ToInt(this.ParentNodeID.SelectedValue);
            if (theNodeID == 0)
            {
                theNodeID = this.nodeID;
            }
            PageUtils.Redirect(BackgroundChannelAdd.GetRedirectUrl(base.PublishmentSystemID, theNodeID, base.GetQueryString("ReturnUrl")));
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, string returnUrl)
        {
            return PageUtils.GetCMSUrl(string.Format("background_channelAdd.aspx?PublishmentSystemID={0}&NodeID={1}&ReturnUrl={2}", publishmentSystemID, nodeID, StringUtils.ValueToUrl(returnUrl)));
        }

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

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                int insertNodeID = 0;
                try
                {
                    int NodeID = base.GetIntQueryString("NodeID");
                    NodeInfo nodeInfo = new NodeInfo();
                    nodeInfo.ParentID = NodeID;
                    nodeInfo.ContentModelID = this.ContentModelID.SelectedValue;

                    if (NodeIndexName.Text.Length != 0)
                    {
                        ArrayList nodeIndexNameArrayList = DataProvider.NodeDAO.GetNodeIndexNameArrayList(base.PublishmentSystemID);
                        if (nodeIndexNameArrayList.IndexOf(NodeIndexName.Text) != -1)
                        {
                            base.FailMessage("栏目添加失败，栏目索引已存在！");
                            return;
                        }
                    }

                    if (FilePath.Text.Length != 0)
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
                        if (filePathArrayList.IndexOf(FilePath.Text) != -1)
                        {
                            base.FailMessage("栏目添加失败，栏目页面路径已存在！");
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(this.ChannelFilePathRule.Text))
                    {
                        if (!DirectoryUtils.IsDirectoryNameCompliant(this.ChannelFilePathRule.Text))
                        {
                            base.FailMessage("栏目页面命名规则不符合系统要求！");
                            return;
                        }
                        if (PathUtils.IsDirectoryPath(this.ChannelFilePathRule.Text))
                        {
                            base.FailMessage("栏目页面命名规则必须包含生成文件的后缀！");
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(this.ContentFilePathRule.Text))
                    {
                        if (!DirectoryUtils.IsDirectoryNameCompliant(this.ContentFilePathRule.Text))
                        {
                            base.FailMessage("内容页面命名规则不符合系统要求！");
                            return;
                        }
                        if (PathUtils.IsDirectoryPath(this.ContentFilePathRule.Text))
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

                    nodeInfo.LinkUrl = LinkURL.Text;
                    nodeInfo.LinkType = ELinkTypeUtils.GetEnumType(LinkType.SelectedValue);
                    nodeInfo.ChannelTemplateID = (ChannelTemplateID.Items.Count > 0) ? int.Parse(ChannelTemplateID.SelectedValue) : 0;
                    nodeInfo.ContentTemplateID = (ContentTemplateID.Items.Count > 0) ? int.Parse(ContentTemplateID.SelectedValue) : 0;

                    nodeInfo.AddDate = DateTime.Now;

                    insertNodeID = DataProvider.NodeDAO.InsertNodeInfo(nodeInfo);
                    //栏目选择投票样式后，内容
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("栏目添加失败：{0}", ex.Message));
                    LogUtils.AddErrorLog(ex);
                    return;
                }

                string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Add, ETemplateType.ChannelTemplate, insertNodeID, 0, 0);
                AjaxUrlManager.AddAjaxUrl(ajaxUrl);

                StringUtility.AddLog(base.PublishmentSystemID, "添加栏目", string.Format("栏目:{0}", NodeName.Text));

                base.SuccessMessage("栏目添加成功！");
                base.AddWaitAndRedirectScript(this.returnUrl);
            }
        }

        public string ReturnUrl { get { return this.returnUrl; } }
    }
}
