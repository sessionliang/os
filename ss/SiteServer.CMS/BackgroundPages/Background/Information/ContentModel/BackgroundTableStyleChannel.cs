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
	public class BackgroundTableStyleChannel : BackgroundBasePage
	{
        public DropDownList NodeIDDropDownList;

		public DataGrid dgContents;

        public Button AddStyle;
        public Button AddStyles;
        public Button Import;
        public Button Export;

        private ETableStyle tableStyle;
        private string tableName;
        private NodeInfo nodeInfo;
        private ArrayList relatedIdentities;
        private string redirectUrl;

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID, ETableStyle tableStyle)
        {
            return PageUtils.GetCMSUrl(string.Format("background_tableStyleChannel.aspx?PublishmentSystemID={0}&NodeID={1}&TableStyle={2}", publishmentSystemID, nodeID, ETableStyleUtils.GetValue(tableStyle)));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            this.tableName = DataProvider.NodeDAO.TableName;
            int nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"), base.PublishmentSystemID);
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.redirectUrl = BackgroundTableStyleChannel.GetRedirectUrl(base.PublishmentSystemID, nodeID, this.tableStyle);

            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);

			if(!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_ContentModel, (this.tableStyle == ETableStyle.Channel) ? "栏目字段管理" : "评论字段管理", AppManager.CMS.Permission.WebSite.ContentModel);
                
                //删除样式
                if (base.GetQueryString("DeleteStyle") != null)
                {
                    DeleteStyle();
                }
                else if (base.GetQueryString("SetTaxis") != null)
                {
                    SetTaxis();
                }

                NodeManager.AddListItems(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, false, true, true);
                ControlUtils.SelectListItems(this.NodeIDDropDownList, nodeID.ToString());

                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, tableName, this.relatedIdentities);

                this.dgContents.DataSource = styleInfoArrayList;
                this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                this.dgContents.DataBind();

                this.AddStyle.Attributes.Add("onclick", Modal.TableStyleAdd.GetOpenWindowString(base.PublishmentSystemID, 0, this.relatedIdentities, this.tableName, string.Empty, this.tableStyle, this.redirectUrl));
                this.AddStyles.Attributes.Add("onclick", Modal.TableStylesAdd.GetOpenWindowString(base.PublishmentSystemID, this.relatedIdentities, this.tableName, this.tableStyle, this.redirectUrl));
                this.Import.Attributes.Add("onclick", PageUtility.ModalSTL.TableStyleImport_GetOpenWindowString(this.tableName, this.tableStyle, base.PublishmentSystemID, nodeID));
                this.Export.Attributes.Add("onclick", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToSingleTableStyle(this.tableStyle, this.tableName, base.PublishmentSystemID, nodeID));
			}
		}

        private void DeleteStyle()
        {
            string attributeName = base.GetQueryString("AttributeName");
            if (TableStyleManager.IsExists(this.nodeInfo.NodeID, this.tableName, attributeName))
            {
                try
                {
                    TableStyleManager.Delete(this.nodeInfo.NodeID, this.tableName, attributeName);
                    StringUtility.AddLog(base.PublishmentSystemID, "删除数据表单样式", string.Format("表单:{0},字段:{1}", this.tableName, attributeName));
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
            if (styleInfo.RelatedIdentity == this.nodeInfo.NodeID)
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
        }

        public void Redirect(object sender, EventArgs e)
        {
            PageUtils.Redirect(BackgroundTableStyleChannel.GetRedirectUrl(base.PublishmentSystemID, TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue), this.tableStyle));
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
                Literal ltlFieldType = e.Item.FindControl("ltlFieldType") as Literal;;
                Literal ltlIsVisible = e.Item.FindControl("ltlIsVisible") as Literal;
                Literal ltlValidate = e.Item.FindControl("ltlValidate") as Literal;
                Literal ltlEditStyle = e.Item.FindControl("ltlEditStyle") as Literal;
                Literal ltlEditValidate = e.Item.FindControl("ltlEditValidate") as Literal;
                HyperLink upLinkButton = e.Item.FindControl("UpLinkButton") as HyperLink;
                HyperLink downLinkButton = e.Item.FindControl("DownLinkButton") as HyperLink;

                string showPopWinString = Modal.TableMetadataView.GetOpenWindowString(ETableStyleUtils.GetTableType(this.tableStyle), this.tableName, styleInfo.AttributeName, this.relatedIdentities);
                ltlAttributeName.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, styleInfo.AttributeName);

                ltlDisplayName.Text = styleInfo.DisplayName;
                ltlInputType.Text = EInputTypeUtils.GetText(styleInfo.InputType);
                if (TableManager.IsAttributeNameExists(this.tableStyle, this.tableName, styleInfo.AttributeName))
                {
                    ltlFieldType.Text = string.Format("真实 {0}", TableManager.GetTableMetadataDataType(this.tableName, styleInfo.AttributeName));
                }
                else
                {
                    ltlFieldType.Text = "虚拟字段";
                }

                ltlIsVisible.Text = StringUtils.GetTrueOrFalseImageHtml(styleInfo.IsVisible.ToString());
                ltlValidate.Text = EInputValidateTypeUtils.GetValidateInfo(styleInfo);

                showPopWinString = Modal.TableStyleAdd.GetOpenWindowString(base.PublishmentSystemID, styleInfo.TableStyleID, this.relatedIdentities, this.tableName, styleInfo.AttributeName, this.tableStyle, redirectUrl);
                string editText = "添加";
                if (styleInfo.RelatedIdentity == this.nodeInfo.NodeID)//数据库中有样式
                {
                    editText = "修改";
                }
                ltlEditStyle.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, editText);

                showPopWinString = Modal.TableStyleValidateAdd.GetOpenWindowString(styleInfo.TableStyleID, this.relatedIdentities, this.tableName, styleInfo.AttributeName, this.tableStyle, redirectUrl);
                ltlEditValidate.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">设置</a>", showPopWinString);

                if (styleInfo.RelatedIdentity == this.nodeInfo.NodeID)//数据库中有样式
                {
                    string urlStyle = PageUtils.GetCMSUrl(string.Format("background_tableStyleChannel.aspx?PublishmentSystemID={0}&NodeID={1}&DeleteStyle=True&TableName={2}&AttributeName={3}&tableStyle={4}", base.PublishmentSystemID, this.nodeInfo.NodeID, this.tableName, styleInfo.AttributeName, tableStyle));
                    ltlEditStyle.Text += string.Format(@"&nbsp;&nbsp;<a href=""{0}"" onClick=""javascript:return confirm('此操作将删除对应显示样式，确认吗？');"">删除</a>", urlStyle);
                }

                bool isTaxisVisible = true;
                if (TableStyleManager.IsMetadata(this.tableStyle, styleInfo.AttributeName) || styleInfo.RelatedIdentity != this.nodeInfo.NodeID)
                {
                    isTaxisVisible = false;
                }
                else
                {
                    isTaxisVisible = !TableStyleManager.IsExistsInParents(this.relatedIdentities, this.tableName, styleInfo.AttributeName);
                }

                if (!isTaxisVisible)
                {
                    upLinkButton.Visible = downLinkButton.Visible = false;
                }
                else
                {
                    upLinkButton.NavigateUrl = PageUtils.GetCMSUrl(string.Format("background_tableStyleChannel.aspx?PublishmentSystemID={0}&NodeID={1}&SetTaxis=True&TableStyleID={2}&Direction=UP&tableStyle={3}", base.PublishmentSystemID, this.nodeInfo.NodeID, styleInfo.TableStyleID, tableStyle));
                    downLinkButton.NavigateUrl = PageUtils.GetCMSUrl(string.Format("background_tableStyleChannel.aspx?PublishmentSystemID={0}&NodeID={1}&SetTaxis=True&TableStyleID={2}&Direction=DOWN&tableStyle={3}", base.PublishmentSystemID, this.nodeInfo.NodeID, styleInfo.TableStyleID, tableStyle));
                }
            }
        }
	}
}
