using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;


namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class WebsiteMessageContentView : BackgroundBasePage
    {
        protected Repeater rptContents;
        protected Literal ltlAddUserName;
        protected Literal ltlIPAddress;
        protected Literal ltlLocation;
        protected Literal ltlAddDate;
        protected Literal ltlReply;

        private int contentID;
        ArrayList relatedIdentities;
        private WebsiteMessageInfo websiteMessageInfo;
        private WebsiteMessageContentInfo contentInfo;

        public static string GetOpenWindowString(int publishmentSystemID, int websiteMessageID, int contentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("WebsiteMessageID", websiteMessageID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            return PageUtility.GetOpenWindowString("查看信息", "modal_websiteMessageContentView.aspx", arguments, 700, 630, true);
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "WebsiteMessageID", "ContentID");

            int websiteMessageID = base.GetIntQueryString("WebsiteMessageID");
            this.contentID = base.GetIntQueryString("ContentID");
            this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, base.PublishmentSystemID, websiteMessageID);

            this.websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageID);

            this.contentInfo = DataProvider.WebsiteMessageContentDAO.GetContentInfo(this.contentID);
            if (!string.IsNullOrEmpty(contentInfo.UserName))
            {
                //group_todo
                string showPopWinString = BackgroundPages.Modal.UserView.GetOpenWindowString(base.PublishmentSystemID, contentInfo.UserName);
                this.ltlAddUserName.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", showPopWinString, contentInfo.UserName);
            }
            else
            {
                this.ltlAddUserName.Text = "匿名";
            }

            this.ltlIPAddress.Text = contentInfo.IPAddress;
            this.ltlLocation.Text = contentInfo.Location;
            this.ltlAddDate.Text = DateUtils.GetDateAndTimeString(contentInfo.AddDate);
            this.ltlReply.Text = contentInfo.Reply;

            ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, this.relatedIdentities);

            this.rptContents.DataSource = styleInfoArrayList;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.rptContents.DataBind();
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableStyleInfo styleInfo = e.Item.DataItem as TableStyleInfo;

                string dataValue = this.contentInfo.GetExtendedAttribute(styleInfo.AttributeName);
                dataValue = InputTypeParser.GetContentByTableStyle(dataValue, base.PublishmentSystemInfo, ETableStyle.WebsiteMessageContent, styleInfo);

                Literal ltlDataKey = e.Item.FindControl("ltlDataKey") as Literal;
                Literal ltlDataValue = e.Item.FindControl("ltlDataValue") as Literal;

                if (ltlDataKey != null) ltlDataKey.Text = styleInfo.DisplayName;
                if (ltlDataValue != null) ltlDataValue.Text = dataValue.Replace("\r\n", "<br/>");
            }
        }
    }
}
