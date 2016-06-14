using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using System.Data;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model; 

namespace SiteServer.CMS.Pages.MLibManage
{
    public class SubmissionShow : SystemBasePage
    {
        public Repeater MyRepeater;

        public Literal ltlTabAction;
        public Literal ltlStatus;

        private int nodeID;
        private ETableStyle tableStyle;
        private string tableName;
        ArrayList relatedIdentities;
        private int contentID;
        private ContentInfo contentInfo;
        private int publishmentSystemID;
        private PublishmentSystemInfo publishmentSystemInfo;

        public string homeUrl;

        public void Page_Load(object sender, EventArgs E)
        {

            this.contentID = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.publishmentSystemID = TranslateUtils.ToInt(Request.QueryString["PublishmentSystemID"]);
            this.nodeID = TranslateUtils.ToInt(Request.QueryString["NodeID"]);

            PublishmentSystemInfo publishmentSystemInfoUser = PublishmentSystemManager.GetUniqueUserCenter();
            if (publishmentSystemInfoUser != null)
                homeUrl = publishmentSystemInfoUser.PublishmentSystemDir;


            this.publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID);
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(this.publishmentSystemID, this.nodeID);
            this.tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            this.tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(this.publishmentSystemID, nodeInfo.NodeID);

            if (this.contentID != 0)
            {
                this.contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, this.contentID);
            }


            if (!IsPostBack)
            {


                #region 审核信息
                if (contentInfo.CheckedLevel == 0)
                {
                    ltlStatus.Text = string.Format("{0}:由用户{1}投稿", contentInfo.AddDate.ToString(), contentInfo.MemberName);
                }
                else
                {
                    ltlStatus.Text = string.Format("{0}:由管理员{1}{2}审{3}通过", contentInfo.AddDate.ToString(), contentInfo.LastEditUserName, Number2Chinese(contentInfo.CheckedLevel), contentInfo.IsChecked ? "" : "不");
                }
                #endregion

                //获取栏目字段 
                MLibScopeInfo minfo = DataProvider.MLibScopeDAO.GetMLibScopeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                string files = this.MLibDraftContentAttributeNames(tableName);
                if (minfo != null && !string.IsNullOrEmpty(minfo.Field))
                    files = minfo.Field;

                ArrayList styleInfoArrayListOld = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);

                ArrayList myStyleInfoArrayList = new ArrayList();
                if (string.IsNullOrEmpty(files))
                {
                    foreach (TableStyleInfo styleInfo in styleInfoArrayListOld)
                    {
                        if (styleInfo.IsVisible)
                            myStyleInfoArrayList.Add(styleInfo);
                    }
                }
                else
                {
                    foreach (TableStyleInfo styleInfo in styleInfoArrayListOld)
                    {
                        ArrayList filesList = TranslateUtils.StringCollectionToArrayList(files);
                        foreach (string flesName in filesList)
                        {
                            if (styleInfo.IsVisible && flesName == styleInfo.AttributeName)
                                myStyleInfoArrayList.Add(styleInfo);
                        }
                    }
                }

                this.MyRepeater.DataSource = myStyleInfoArrayList;
                this.MyRepeater.ItemDataBound += new RepeaterItemEventHandler(MyRepeater_ItemDataBound);
                this.MyRepeater.DataBind();
            }
        }


        void MyRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableStyleInfo styleInfo = (TableStyleInfo)e.Item.DataItem;

                string helpHtml = StringUtility.GetHelpHtml(styleInfo.DisplayName, styleInfo.HelpText);

                string inputHtml = InputTypeParser.GetContentByTableStyle(this.contentInfo.GetExtendedAttribute(styleInfo.AttributeName), this.publishmentSystemInfo, this.tableStyle, styleInfo);

                Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

                ltlHtml.Text = string.Format(@"
<tr>
  <td>{0}</td>
  <td colspan=""2"">{1}</td>
</tr>
", helpHtml, inputHtml);
            }
        }


        public string Number2Chinese(int n)
        {

            int MaxCheckLevel = TranslateUtils.ToInt(DataProvider.MlibDAO.GetConfigAttr("CheckLevel"));
            if (n == MaxCheckLevel)
            {
                return "终";
            }
            var chinese = new string[] { "", "初", "二", "三", "四", "五", "六", "七", "八", "九" };
            return chinese[n];
        }
    }
}
