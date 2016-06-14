using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Text;
using System.Collections.Specialized;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundContentView : BackgroundBasePage
    {
        public Literal ltlScripts;
        public Literal ltlNodeName;

        public Literal ltlContentGroup;
        public Literal ltlTags;

        public Literal ltlLastEditDate;
        public Literal ltlAddUserName;
        public Literal ltlLastEditUserName;
        public Literal ltlContentLevel;

        public Repeater MyRepeater;

        public Control RowTags;
        public Control RowContentGroup;

        public Button Submit;

        private int nodeID;
        private ETableStyle tableStyle;
        private string tableName;
        ArrayList relatedIdentities;
        private int contentID;
        private string returnUrl;
        private ContentInfo contentInfo;

        public static string GetContentViewUrl(int publishmentSystemID, int nodeID, int contentID, string returnUrl)
        {
            return PageUtils.GetCMSUrl(string.Format("background_contentView.aspx?PublishmentSystemID={0}&NodeID={1}&ID={2}&ReturnUrl={3}", publishmentSystemID, nodeID, contentID, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ID", "ReturnUrl");

            this.nodeID = base.GetIntQueryString("NodeID");
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            this.contentID = base.GetIntQueryString("ID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));

            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID);

            this.contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, this.contentID);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "查看内容", string.Empty);

                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);
                ArrayList myStyleInfoArrayList = new ArrayList();
                if (styleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (styleInfo.IsVisible)
                        {
                            myStyleInfoArrayList.Add(styleInfo);
                        }
                    }
                }

                this.MyRepeater.DataSource = myStyleInfoArrayList;
                this.MyRepeater.ItemDataBound += new RepeaterItemEventHandler(MyRepeater_ItemDataBound);
                this.MyRepeater.DataBind();

                this.ltlNodeName.Text = NodeManager.GetNodeName(base.PublishmentSystemID, nodeID);
                this.ltlNodeName.Text += string.Format(@"
<script>
function submitPreview(){{
    window.open(""{0}"");
}}
</script>
", PageUtility.GetContentPreviewUrl(base.PublishmentSystemInfo, this.nodeID, this.contentID));

                if (base.PublishmentSystemInfo.Additional.IsRelatedByTags)
                {
                    this.ltlTags.Text = contentInfo.Tags;
                }
                if (string.IsNullOrEmpty(this.ltlTags.Text))
                {
                    this.RowTags.Visible = false;
                }

                this.ltlContentGroup.Text = contentInfo.ContentGroupNameCollection;
                if (string.IsNullOrEmpty(this.ltlContentGroup.Text))
                {
                    this.RowContentGroup.Visible = false;
                }

                this.ltlLastEditDate.Text = DateUtils.GetDateAndTimeString(this.contentInfo.LastEditDate);
                this.ltlAddUserName.Text = AdminManager.GetDisplayName(this.contentInfo.AddUserName, true);
                this.ltlLastEditUserName.Text = AdminManager.GetDisplayName(this.contentInfo.LastEditUserName, true);

                this.ltlContentLevel.Text = LevelManager.GetCheckState(base.PublishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel);

                if (this.contentInfo.ReferenceID > 0 && this.contentInfo.GetExtendedAttribute(ContentAttribute.TranslateContentType) != ETranslateContentType.ReferenceContent.ToString())
                {
                    int referencePublishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(this.contentInfo.SourceID);
                    PublishmentSystemInfo referencePublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(referencePublishmentSystemID);
                    ETableStyle referenceTableStyle = NodeManager.GetTableStyle(referencePublishmentSystemInfo, this.contentInfo.SourceID);
                    string referenceTableName = NodeManager.GetTableName(referencePublishmentSystemInfo, this.contentInfo.SourceID);
                    ContentInfo referenceContentInfo = DataProvider.ContentDAO.GetContentInfo(referenceTableStyle, referenceTableName, this.contentInfo.ReferenceID);

                    if (referenceContentInfo != null)
                    {
                        string pageUrl = PageUtility.GetContentUrl(referencePublishmentSystemInfo, referenceContentInfo, referencePublishmentSystemInfo.Additional.VisualType);
                        NodeInfo referenceNodeInfo = NodeManager.GetNodeInfo(referenceContentInfo.PublishmentSystemID, referenceContentInfo.NodeID);
                        this.ltlScripts.Text += string.Format(@"
<div class=""tips"">此内容为对内容 （站点：{3},栏目：{4}）“<a href=""{0}"" target=""_blank"">{1}</a>”（<a href=""{2}"">编辑</a>） 的引用，内容链接将和原始内容链接一致</div>
", pageUrl, contentInfo.Title, WebUtils.GetContentAddEditUrl(referencePublishmentSystemInfo.PublishmentSystemID, referenceNodeInfo, this.contentInfo.ReferenceID, base.GetQueryString("ReturnUrl")), referencePublishmentSystemInfo.PublishmentSystemName, referenceNodeInfo.NodeName);
                    }
                }

                this.Submit.Attributes.Add("onclick", Modal.ContentCheck.GetOpenWindowString(base.PublishmentSystemInfo.PublishmentSystemID, this.nodeID, this.contentID, this.returnUrl));
            }
        }

        void MyRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableStyleInfo styleInfo = (TableStyleInfo)e.Item.DataItem;

                string helpHtml = StringUtility.GetHelpHtml(styleInfo.DisplayName, styleInfo.HelpText);

                string inputHtml = InputTypeParser.GetContentByTableStyle(this.contentInfo.GetExtendedAttribute(styleInfo.AttributeName), base.PublishmentSystemInfo, this.tableStyle, styleInfo);

                Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

                ltlHtml.Text = string.Format(@"
<tr>
  <td>{0}</td>
  <td colspan=""2"">{1}</td>
</tr>
", helpHtml, inputHtml);
            }
        }

        public string ReturnUrl { get { return this.returnUrl; } }
    }
}
