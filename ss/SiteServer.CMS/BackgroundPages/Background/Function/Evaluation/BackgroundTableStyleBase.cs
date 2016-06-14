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
using BaiRong.Controls;



namespace SiteServer.CMS.BackgroundPages
{
    public abstract class BackgroundTableStyleBase : BackgroundBasePage
    {
        public DropDownList NodeIDDropDownList;

        public DataGrid dgContents;

        public Button AddStyle;
        public Button AddStyles;
        public Button Import;
        public Button Export;
        public Alerts alertsID;

        protected ETableStyle tableStyle;
        protected NodeInfo nodeInfo;
        protected ArrayList relatedIdentities;
        protected string redirectUrl;

        //current page aspx name
        protected abstract string currentAspxName { get; }
        protected abstract string tableName { get; }

        //bread crumb
        protected abstract string pageTitle { get; }
        protected abstract string leftMenu { get; }
        protected abstract string leftSubMenu { get; }
        protected abstract string permission { get; }

        public string GetRedirectUrl(int publishmentSystemID, int nodeID, ETableStyle tableStyle)
        {
            return PageUtils.GetCMSUrl(string.Format("{3}?PublishmentSystemID={0}&NodeID={1}&TableStyle={2}", publishmentSystemID, nodeID, ETableStyleUtils.GetValue(tableStyle), currentAspxName));
        }

        public virtual void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            int nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"), base.PublishmentSystemID);
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.redirectUrl = this.GetRedirectUrl(base.PublishmentSystemID, nodeID, this.tableStyle);

            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);

            if (!IsPostBack)
            {
                base.BreadCrumb(leftMenu, leftSubMenu, pageTitle, permission);

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

                if ((ETableStyleUtils.Equals(ETableStyle.EvaluationContent, this.tableStyle) && nodeInfo.Additional.IsUseEvaluation) || (ETableStyleUtils.Equals(ETableStyle.TrialApplyContent, this.tableStyle) && nodeInfo.Additional.IsUseTrial) || (ETableStyleUtils.Equals(ETableStyle.TrialReportContent, this.tableStyle) && nodeInfo.Additional.IsUseTrial) || (ETableStyleUtils.Equals(ETableStyle.SurveyContent, this.tableStyle) && nodeInfo.Additional.IsUseSurvey) || (ETableStyleUtils.Equals(ETableStyle.CompareContent, this.tableStyle) && nodeInfo.Additional.IsUseCompare))
                {
                    ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, tableName, this.relatedIdentities);

                    this.dgContents.DataSource = styleInfoArrayList;
                    this.dgContents.ItemDataBound += new DataGridItemEventHandler(dgContents_ItemDataBound);
                    this.dgContents.DataBind();

                    this.AddStyle.Attributes.Add("onclick", Modal.TableStyleAdd.GetOpenWindowString(base.PublishmentSystemID, 0, this.relatedIdentities, this.tableName, string.Empty, this.tableStyle, this.redirectUrl));
                    this.AddStyles.Attributes.Add("onclick", Modal.TableStylesAdd.GetOpenWindowString(base.PublishmentSystemID, this.relatedIdentities, this.tableName, this.tableStyle, this.redirectUrl));
                    this.Import.Attributes.Add("onclick", PageUtility.ModalSTL.TableStyleImport_GetOpenWindowString(this.tableName, this.tableStyle, base.PublishmentSystemID, nodeID));
                    this.Export.Attributes.Add("onclick", PageUtility.ModalSTL.ExportMessage.GetOpenWindowStringToSingleTableStyle(this.tableStyle, this.tableName, base.PublishmentSystemID, nodeID));
                }
                else
                {
                    this.AddStyle.Visible = false;
                    this.AddStyles.Visible = false;
                    this.Import.Visible = false;
                    this.Export.Visible = false;
                    this.alertsID.Text = "当前栏目未启用" + AppManager.CMS.LeftMenu.GetSubText(this.leftSubMenu) + "功能，" + ETableStyleUtils.GetText(this.tableStyle) + "功能不可用！";
                }
            }
        }

        protected virtual void DeleteStyle()
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

        protected virtual void SetTaxis()
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

        public virtual void Redirect(object sender, EventArgs e)
        {
            PageUtils.Redirect(this.GetRedirectUrl(base.PublishmentSystemID, TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue), this.tableStyle));
        }

        protected virtual void dgContents_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableStyleInfo styleInfo = e.Item.DataItem as TableStyleInfo;

                Literal ltlAttributeName = e.Item.FindControl("ltlAttributeName") as Literal;
                Literal ltlDataType = e.Item.FindControl("ltlDataType") as Literal;
                Literal ltlDisplayName = e.Item.FindControl("ltlDisplayName") as Literal;
                Literal ltlInputType = e.Item.FindControl("ltlInputType") as Literal; 
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
                

                ltlIsVisible.Text = StringUtils.GetTrueOrFalseImageHtml(styleInfo.IsVisible.ToString());
                ltlValidate.Text = EInputValidateTypeUtils.GetValidateInfo(styleInfo);

                if (SurveyQuestionnaireAttribute.IsExtendAttribute(styleInfo.AttributeName))
                    showPopWinString = Modal.TableStyleUseStatisticsAdd.GetOpenWindowString(base.PublishmentSystemID, styleInfo.TableStyleID, this.relatedIdentities, this.tableName, styleInfo.AttributeName, this.tableStyle, redirectUrl);
                else
                    showPopWinString = Modal.TableStyleAdd.GetOpenWindowString(base.PublishmentSystemID, styleInfo.TableStyleID, this.relatedIdentities, this.tableName, styleInfo.AttributeName, this.tableStyle, redirectUrl);

                string editText = "设置";
                if (styleInfo.RelatedIdentity == this.nodeInfo.NodeID)//数据库中有样式
                {
                    editText = "修改";
                }
                ltlEditStyle.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">{1}</a>", showPopWinString, editText);

                showPopWinString = Modal.TableStyleValidateAdd.GetOpenWindowString(styleInfo.TableStyleID, this.relatedIdentities, this.tableName, styleInfo.AttributeName, this.tableStyle, redirectUrl);
                ltlEditValidate.Text = string.Format("<a href=\"javascript:void 0;\" onClick=\"{0}\">设置</a>", showPopWinString);

                if (styleInfo.RelatedIdentity == this.nodeInfo.NodeID)//数据库中有样式
                {
                    string urlStyle = PageUtils.GetCMSUrl(string.Format("{0}?PublishmentSystemID={1}&NodeID={2}&DeleteStyle=True&TableName={3}&AttributeName={4}&tableStyle={5}", currentAspxName, base.PublishmentSystemID, this.nodeInfo.NodeID, this.tableName, styleInfo.AttributeName, tableStyle));
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
                    upLinkButton.NavigateUrl = PageUtils.GetCMSUrl(string.Format("{0}?PublishmentSystemID={1}&NodeID={2}&SetTaxis=True&TableStyleID={3}&Direction=UP&tableStyle={4}", currentAspxName, base.PublishmentSystemID, this.nodeInfo.NodeID, styleInfo.TableStyleID, tableStyle));
                    downLinkButton.NavigateUrl = PageUtils.GetCMSUrl(string.Format("{0}?PublishmentSystemID={1}&NodeID={2}&SetTaxis=True&TableStyleID={3}&Direction=DOWN&tableStyle={4}", currentAspxName, base.PublishmentSystemID, this.nodeInfo.NodeID, styleInfo.TableStyleID, tableStyle));
                }
            }
        }
    }
}
