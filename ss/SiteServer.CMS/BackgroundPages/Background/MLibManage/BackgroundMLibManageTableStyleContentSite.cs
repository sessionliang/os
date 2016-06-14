using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundMLibManageTableStyleContentSite : BackgroundBasePage
    {
        public DropDownList ddlPublishmentSystem;
        public DropDownList NodeIDDropDownList;

        public DataGrid dgContents;

        public Button AddStyle;

        private NodeInfo nodeInfo;
        private ContentModelInfo modelInfo;
        private ETableStyle tableStyle;
        private ArrayList relatedIdentities;
        private string redirectUrl;
        private ArrayList hasFields = new ArrayList();
        private ArrayList arraylist;

        public void GetRedirectUrl(PublishmentSystemInfo pinfo, int nodeID)
        {
            #region 获取稿件发布都有权限的站点和栏目
            if (nodeID == 0)
            {
                this.NodeIDDropDownList.Items.Clear();
                ArrayList mLibNodeInfoArrayList = PublishmentSystemManager.GetNode(ConfigManager.Additional.UnifiedMLibAddUser, pinfo.PublishmentSystemID);
                foreach (NodeInfo nodeInfo in mLibNodeInfoArrayList)
                {
                    ListItem item = new ListItem(nodeInfo.NodeName, nodeInfo.NodeID.ToString());
                    this.NodeIDDropDownList.Items.Add(item);
                }
                nodeID = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);
            }
            #endregion

            MLibScopeInfo minfo = DataProvider.MLibScopeDAO.GetMLibScopeInfo(pinfo.PublishmentSystemID, nodeID);
            if (minfo != null)
                this.hasFields = TranslateUtils.StringCollectionToArrayList(minfo.Field);

            this.nodeInfo = NodeManager.GetNodeInfo(pinfo.PublishmentSystemID, nodeID);
            this.modelInfo = ContentModelManager.GetContentModelInfo(pinfo, this.nodeInfo.ContentModelID);
            this.tableStyle = EAuxiliaryTableTypeUtils.GetTableStyle(this.modelInfo.TableType);
            this.redirectUrl = PageUtils.GetPlatformUrl(string.Format("background_mlibManageTableStyleContentSite.aspx?PublishmentSystemID={0}",pinfo.PublishmentSystemID,nodeID));//BackgroundTableStyleContent.GetRedirectUrl(pinfo.PublishmentSystemID, nodeID);

            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(pinfo.PublishmentSystemID, nodeID);


            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.modelInfo.TableName, this.relatedIdentities);

            this.arraylist = BaiRongDataProvider.AuxiliaryTableDataDAO.GetDefaultTableMetadataInfoArrayList(this.modelInfo.TableName, EAuxiliaryTableType.ManuscriptContent);

            this.dgContents.DataSource = styleInfoArrayList;
            this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
            this.dgContents.DataBind();

            this.AddStyle.Attributes.Add("onclick", Modal.TableStyleAdd.GetOpenWindowString(pinfo.PublishmentSystemID, 0, this.relatedIdentities, this.modelInfo.TableName, string.Empty, this.tableStyle, this.redirectUrl));

            base.InfoMessage(string.Format("在此编辑内容模型字段,用户中心的投稿只显示启用的字段，复杂字段不允许启用到投稿使用; 辅助表:{0}({1}); 内容模型:{2}；选择字段后需点击【保存所选字段】才有效，否则默认为系统设置的默认字段。", BaiRongDataProvider.TableCollectionDAO.GetTableCNName(modelInfo.TableName), modelInfo.TableName, modelInfo.ModelName));

            this.ddlPublishmentSystem.SelectedValue = pinfo.PublishmentSystemID.ToString();
            this.NodeIDDropDownList.SelectedValue = nodeID.ToString();
        }

        public void GoRedirectUrl(int publishmentSystemID, int nodeID)
        {
            PageUtils.Redirect(PageUtils.GetPlatformUrl(string.Format("background_mlibManageTableStyleContentSite.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;


            if (ConfigManager.Additional.IsUseMLib == false)
            {
                PageUtils.RedirectToErrorPage("投稿中心尚未开启.请在投稿基本设置中启用投稿");
                return;
            }
            //if (ConfigManager.Additional.MLibPublishmentSystemIDs == "")
            //{
            //    PageUtils.RedirectToErrorPage("未加载到投稿范围，请检查是否设置投稿范围");
            //    return;
            //}


            #region 获取稿件发布都有权限的站点和栏目
            if (this.ddlPublishmentSystem.Items.Count == 0)
            {
                this.ddlPublishmentSystem.Items.Clear(); 
                foreach (PublishmentSystemInfo info in PublishmentSystemManager.GetPublishmentSystem(ConfigManager.Additional.UnifiedMLibAddUser))
                {
                    ListItem item = new ListItem(info.PublishmentSystemName, info.PublishmentSystemID.ToString());
                    this.ddlPublishmentSystem.Items.Add(item);
                }
            }
            #endregion
            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_MLibManage, "投稿字段设置", AppManager.User.Permission.Usercenter_MLibManageSetting);

                if (base.GetQueryString("SetTaxis") != null)
                {
                    SetTaxis();
                }

                #region 获取投稿范围
                //this.ddlPublishmentSystem.Items.Clear();
                //ArrayList MLibPublishmentSystemIDs = TranslateUtils.StringCollectionToArrayList(ConfigManager.Additional.MLibPublishmentSystemIDs);
                //foreach (string pid in MLibPublishmentSystemIDs)
                //{
                //    PublishmentSystemInfo info = PublishmentSystemManager.GetPublishmentSystemInfo(TranslateUtils.ToInt(pid));
                //    if (info == null)
                //        continue;
                //    ListItem item = new ListItem(info.PublishmentSystemName, info.PublishmentSystemID.ToString());
                //    this.ddlPublishmentSystem.Items.Add(item);
                //}
                //this.ddlPublishmentSystem.SelectedValue = base.PublishmentSystemID.ToString();
                //if (this.ddlPublishmentSystem.Items.Count > 0)
                //{
                //    int publishmentSystemId = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
                //    this.NodeIDDropDownList.Items.Clear();
                //    ArrayList mLibScopeInfoArrayList = DataProvider.MLibScopeDAO.GetInfoList(TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue));
                //    foreach (MLibScopeInfo mLibScopeInfo in mLibScopeInfoArrayList)
                //    {
                //        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemId, mLibScopeInfo.NodeID);
                //        ListItem item = new ListItem(nodeInfo.NodeName, nodeInfo.NodeID.ToString());
                //        this.NodeIDDropDownList.Items.Add(item);
                //    }
                //}
                #endregion
                ddlPublishmentSystem_SelectedIndexChanged(sender, E);
            }
        }

        private void SetTaxis()
        {
            int tableStyleID = TranslateUtils.ToInt(base.GetQueryString("TableStyleID"));
            TableStyleInfo styleInfo = BaiRongDataProvider.TableStyleDAO.GetTableStyleInfo(tableStyleID);
            if (styleInfo != null && styleInfo.RelatedIdentity == this.nodeInfo.NodeID)
            {
                string direction = base.GetQueryString("Direction");

                switch (direction.ToUpper())
                {
                    case "UP":
                        BaiRongDataProvider.TableStyleDAO.TaxisDown(tableStyleID);
                        break;
                    case "DOWN":
                        BaiRongDataProvider.TableStyleDAO.TaxisUp(tableStyleID);
                        break;
                    default:
                        break;
                }
                base.SuccessMessage("排序成功！");
            }
            else
            {
                string direction = base.GetQueryString("Direction");
                int tableMetadataId = TranslateUtils.ToInt(base.GetQueryString("TableMetadataId"));
                switch (direction.ToUpper())
                {
                    case "UP":
                        BaiRongDataProvider.TableMetadataDAO.TaxisDown(tableMetadataId, this.modelInfo.TableName);
                        break;
                    case "DOWN":
                        BaiRongDataProvider.TableMetadataDAO.TaxisUp(tableMetadataId, this.modelInfo.TableName);
                        break;
                    default:
                        break;
                }
                base.SuccessMessage("排序成功！");
            }
        }


        public void ddlPublishmentSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int publishmentSystemId = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
            PublishmentSystemInfo pinfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemId);
            GetRedirectUrl(pinfo, 0);
        }

        public void NodeIDDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int publishmentSystemId = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
            PublishmentSystemInfo pinfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemId);
            int nodeID = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);
            // GoRedirectUrl(publishmentSystemId, nodeID);
            GetRedirectUrl(pinfo, nodeID);
        }
        void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableStyleInfo styleInfo = e.Item.DataItem as TableStyleInfo;

                Literal ltlAttributeName = e.Item.FindControl("ltlAttributeName") as Literal;
                Literal ltlDataType = e.Item.FindControl("ltlDataType") as Literal;
                Literal ltlDisplayName = e.Item.FindControl("ltlDisplayName") as Literal;
                Literal ltlInputType = e.Item.FindControl("ltlInputType") as Literal;
                Literal ltlFieldType = e.Item.FindControl("ltlFieldType") as Literal; ;
                Literal ltlIsVisible = e.Item.FindControl("ltlIsVisible") as Literal;
                Literal ltlValidate = e.Item.FindControl("ltlValidate") as Literal;
                Literal ltlEditStyle = e.Item.FindControl("ltlEditStyle") as Literal;
                Literal ltlEditValidate = e.Item.FindControl("ltlEditValidate") as Literal;
                HyperLink upLinkButton = e.Item.FindControl("UpLinkButton") as HyperLink;
                HyperLink downLinkButton = e.Item.FindControl("DownLinkButton") as HyperLink;
                Literal ltlAttributeNameCollection = e.Item.FindControl("ltlAttributeNameCollection") as Literal;

                string showPopWinString = Modal.TableMetadataView.GetOpenWindowString(this.modelInfo.TableType, this.modelInfo.TableName, styleInfo.AttributeName, this.relatedIdentities);
                ltlAttributeName.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, styleInfo.AttributeName);

                ltlDisplayName.Text = styleInfo.DisplayName;
                ltlInputType.Text = EInputTypeUtils.GetText(styleInfo.InputType);
                if (TableManager.IsAttributeNameExists(this.tableStyle, this.modelInfo.TableName, styleInfo.AttributeName))
                {
                    ltlFieldType.Text = string.Format("真实 {0}", TableManager.GetTableMetadataDataType(this.modelInfo.TableName, styleInfo.AttributeName));
                }
                else
                {
                    ltlFieldType.Text = "虚拟字段";
                }

                ltlIsVisible.Text = StringUtils.GetTrueOrFalseImageHtml(styleInfo.IsVisible.ToString());
                ltlValidate.Text = EInputValidateTypeUtils.GetValidateInfo(styleInfo);

                showPopWinString = Modal.TableStyleAdd.GetOpenWindowString(base.PublishmentSystemID, styleInfo.TableStyleID, this.relatedIdentities, this.modelInfo.TableName, styleInfo.AttributeName, this.tableStyle, redirectUrl);
                string editText = "添加";
                if (styleInfo.RelatedIdentity == this.nodeInfo.NodeID)//数据库中有样式
                {
                    editText = "修改";
                }
                ltlEditStyle.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, editText);


                showPopWinString = Modal.TableStyleValidateAdd.GetOpenWindowString(styleInfo.TableStyleID, this.relatedIdentities, this.modelInfo.TableName, styleInfo.AttributeName, this.tableStyle, redirectUrl);
                ltlEditValidate.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">设置</a>", showPopWinString);

                if (styleInfo.RelatedIdentity == this.nodeInfo.NodeID)//数据库中有样式
                {
                    string urlStyle = PageUtils.GetPlatformUrl(string.Format("background_mlibManageTableStyleContentSite.aspx?PublishmentSystemID={0}&NodeID={1}&DeleteStyle=True&TableName={2}&AttributeName={3}", base.PublishmentSystemID, this.nodeInfo.NodeID, this.modelInfo.TableName, styleInfo.AttributeName));
                    ltlEditStyle.Text += string.Format(@"&nbsp;&nbsp;<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除对应显示样式，确认吗？');"">删除</a>", urlStyle);
                }

                bool isTaxisVisible = true;
                //if (TableStyleManager.IsMetadata(this.tableStyle, styleInfo.AttributeName) || styleInfo.RelatedIdentity != this.nodeInfo.NodeID)
                //{
                //    isTaxisVisible = false;
                //}
                //else
                //{
                isTaxisVisible = !TableStyleManager.IsExistsInParents(this.relatedIdentities, this.modelInfo.TableName, styleInfo.AttributeName);
                //}

                if (!isTaxisVisible)
                {
                    upLinkButton.Visible = downLinkButton.Visible = false;
                }
                else
                {
                    int tableMetadataID = BaiRongDataProvider.TableMetadataDAO.GetTableMetadataID(styleInfo.TableName, styleInfo.AttributeName);
                    upLinkButton.NavigateUrl = PageUtils.GetPlatformUrl(string.Format("background_mlibManageTableStyleContentSite.aspx?PublishmentSystemID={0}&NodeID={1}&SetTaxis=True&TableStyleID={2}&Direction=UP&TableMetadataId={3}", base.PublishmentSystemID, this.nodeInfo.NodeID, styleInfo.TableStyleID, tableMetadataID));
                    downLinkButton.NavigateUrl = PageUtils.GetPlatformUrl(string.Format("background_mlibManageTableStyleContentSite.aspx?PublishmentSystemID={0}&NodeID={1}&SetTaxis=True&TableStyleID={2}&Direction=DOWN&TableMetadataId={3}", base.PublishmentSystemID, this.nodeInfo.NodeID, styleInfo.TableStyleID, tableMetadataID));
                }


                if (styleInfo.IsVisible && !EInputTypeUtils.Equals(styleInfo.InputType, EInputType.RelatedField))
                {
                    string check = "checked=\"checked\"";
                    if (this.hasFields.Count == 0)
                    {
                        check = "";
                        foreach (TableMetadataInfo metadataInfo in this.arraylist)
                        {
                            if (metadataInfo.AttributeName == styleInfo.AttributeName)
                            {
                                check = "checked=\"checked\"";
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (this.hasFields.Contains(styleInfo.AttributeName))
                            check = "checked=\"checked\"";
                        else
                            check = "";
                    }
                    ltlAttributeNameCollection.Text = string.Format(@"<input type=""checkbox"" name=""AttributeNameCollection""  value=""{0}"" {1} />", styleInfo.AttributeName, check);
                }
            }
        }


        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                string fields = Request.Form["AttributeNameCollection"];
                if (fields.Length == 0)
                {
                    base.FailMessage("保存失败：请选择字段进行保存，如果与后台内容字段一样则无需重新设置");
                    return;
                }
                MLibScopeInfo minfo = DataProvider.MLibScopeDAO.GetMLibScopeInfo(TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue), TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue));
                if (minfo == null)
                {
                    minfo = new MLibScopeInfo();
                    minfo.PublishmentSystemID = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
                    minfo.NodeID = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);
                    minfo.Field = fields;
                    minfo.UserName = AdminManager.Current.UserName;

                    DataProvider.MLibScopeDAO.Insert(minfo);
                }
                else
                {
                    minfo.Field = fields;
                    minfo.UserName = AdminManager.Current.UserName;

                    DataProvider.MLibScopeDAO.Update(minfo);
                }

                base.SuccessMessage("保存成功");
                GoRedirectUrl(TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue), TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue));
            }
        }
    }
}
