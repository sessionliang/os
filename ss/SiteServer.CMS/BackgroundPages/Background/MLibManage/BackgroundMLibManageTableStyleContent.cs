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



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundMLibManageTableStyleContent : BackgroundBasePage
    {

        public Literal PublishmentSystemName;
        public Literal NodeName;
        public DataGrid dgContents;

        public Button AddStyle;

        private NodeInfo nodeInfo;
        private ContentModelInfo modelInfo;
        private ETableStyle tableStyle;
        private ArrayList relatedIdentities;
        private string redirectUrl;
        private bool isChecked;
        private ArrayList hasFields=new ArrayList ();


        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_mlibManageTableStyleContent.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID));
        }
        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, bool isChecked)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_mlibManageTableStyleContent.aspx?PublishmentSystemID={0}&NodeID={1}&IsChecked={2}", publishmentSystemID, nodeID, isChecked));
        }


        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            int nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"), base.PublishmentSystemID);
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.modelInfo = ContentModelManager.GetContentModelInfo(base.PublishmentSystemInfo, this.nodeInfo.ContentModelID);
            this.tableStyle = EAuxiliaryTableTypeUtils.GetTableStyle(this.modelInfo.TableType);
            this.redirectUrl = BackgroundTableStyleContent.GetRedirectUrl(base.PublishmentSystemID, nodeID);
            this.isChecked = TranslateUtils.ToBool(base.GetQueryString("IsChecked"), false);

            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);

            this.PublishmentSystemName.Text = base.PublishmentSystemInfo.PublishmentSystemName;
            this.NodeName.Text = this.nodeInfo.NodeName;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_ContentModel, "内容字段管理", AppManager.CMS.Permission.WebSite.ContentModel);

                if (base.GetQueryString("SetTaxis") != null)
                {
                    SetTaxis();
                }

                base.InfoMessage(string.Format("在此编辑内容模型字段,用户中心的投稿只显示启用的字段，复杂字段不允许启用到投稿使用; 辅助表:{0}({1}); 内容模型:{2}", BaiRongDataProvider.TableCollectionDAO.GetTableCNName(this.modelInfo.TableName), this.modelInfo.TableName, this.modelInfo.ModelName));


                ArrayList mLibScopeInfoArrayList = (ArrayList)base.Session[BackgroundMLibManageScopeSite.MLibPublishmentSystemArrayListKey];
                foreach (MLibScopeInfo mLibScopeInfo in mLibScopeInfoArrayList)
                {
                    if (mLibScopeInfo.PublishmentSystemID == base.PublishmentSystemID && mLibScopeInfo.NodeID == this.nodeInfo.NodeID)
                    {
                        this.hasFields = TranslateUtils.StringCollectionToArrayList(mLibScopeInfo.Field);
                    }
                }


                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.modelInfo.TableName, this.relatedIdentities);

                this.dgContents.DataSource = styleInfoArrayList;
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddStyle.Attributes.Add("onclick", Modal.TableStyleAdd.GetOpenWindowString(base.PublishmentSystemID, 0, this.relatedIdentities, this.modelInfo.TableName, string.Empty, this.tableStyle, this.redirectUrl));
            }
        }

        private void DeleteStyle()
        {
            string attributeName = base.GetQueryString("AttributeName");
            if (TableStyleManager.IsExists(this.nodeInfo.NodeID, this.modelInfo.TableName, attributeName))
            {
                try
                {
                    TableStyleManager.Delete(this.nodeInfo.NodeID, this.modelInfo.TableName, attributeName);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除数据表单样式", string.Format("表单:{0},字段:{1}", this.modelInfo.TableName, attributeName));
                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
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

        public void Redirect(object sender, EventArgs e)
        {
            PageUtils.Redirect(BackgroundMLibManageTableStyleContent.GetRedirectUrl(base.PublishmentSystemID, this.nodeInfo.NodeID));
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
                    string urlStyle = PageUtils.GetPlatformUrl(string.Format("background_mlibManageTableStyleContent.aspx?PublishmentSystemID={0}&NodeID={1}&DeleteStyle=True&TableName={2}&AttributeName={3}", base.PublishmentSystemID, this.nodeInfo.NodeID, this.modelInfo.TableName, styleInfo.AttributeName));
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
                    upLinkButton.NavigateUrl = PageUtils.GetPlatformUrl(string.Format("background_mlibManageTableStyleContent.aspx?PublishmentSystemID={0}&NodeID={1}&SetTaxis=True&TableStyleID={2}&Direction=UP&TableMetadataId={3}", base.PublishmentSystemID, this.nodeInfo.NodeID, styleInfo.TableStyleID, tableMetadataID));
                    downLinkButton.NavigateUrl = PageUtils.GetPlatformUrl(string.Format("background_mlibManageTableStyleContent.aspx?PublishmentSystemID={0}&NodeID={1}&SetTaxis=True&TableStyleID={2}&Direction=DOWN&TableMetadataId={3}", base.PublishmentSystemID, this.nodeInfo.NodeID, styleInfo.TableStyleID, tableMetadataID));
                }

                if (styleInfo.IsVisible && !EInputTypeUtils.Equals(styleInfo.InputType, EInputType.RelatedField))
                {
                    string check = "checked=\"checked\"";
                    if (this.hasFields.Count == 0)
                        check = "checked=\"checked\"";
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
                ArrayList mLibScopeInfoArrayList = (ArrayList)base.Session[BackgroundMLibManageScopeSite.MLibPublishmentSystemArrayListKey];
                if (mLibScopeInfoArrayList != null)
                {
                    ArrayList arraylist = new ArrayList();
                    ArrayList nodeIDs = new ArrayList();
                    foreach (MLibScopeInfo mLibScopeInfo in mLibScopeInfoArrayList)
                    {
                        if (mLibScopeInfo.PublishmentSystemID == base.PublishmentSystemID && mLibScopeInfo.NodeID == this.nodeInfo.NodeID)
                        {
                            mLibScopeInfo.Field = fields;
                            mLibScopeInfo.IsChecked = this.isChecked;
                        }
                        arraylist.Add(mLibScopeInfo);
                        nodeIDs.Add(mLibScopeInfo.NodeID);
                    }
                    if (!nodeIDs.Contains(this.nodeInfo.NodeID))
                    {
                        MLibScopeInfo info = new MLibScopeInfo();
                        info.PublishmentSystemID = base.PublishmentSystemID;
                        info.NodeID = this.nodeInfo.NodeID;
                        info.IsChecked = this.isChecked;
                        info.Field = fields;
                        arraylist.Add(info);
                    }

                    base.Session[BackgroundMLibManageScopeSite.MLibPublishmentSystemArrayListKey] = arraylist;
                }

                PageUtils.Redirect(PageUtils.GetPlatformUrl(string.Format("background_mlibManageScope.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));

            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(PageUtils.GetPlatformUrl(string.Format("background_mlibManageScope.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
        }
    }
}
