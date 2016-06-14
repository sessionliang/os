using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using System.Data;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Pages.Mlib
{
    public class SubmissionShow : SystemBasePage
    {
        public Repeater MyRepeater;

        public Literal ltlTabAction;
        public Literal ltlStatus;

        private int submissionID;
        private int nodeID;
        private int checkedLevel;
        private ETableStyle tableStyle;
        private string tableName;
        ArrayList relatedIdentities;
        private int contentID;
        private ContentInfo contentInfo;

        public string homeUrl;

        public void Page_Load(object sender, EventArgs E)
        {
            
            submissionID = TranslateUtils.ToInt(Request.QueryString["SubmissionID"]);
            this.checkedLevel = TranslateUtils.ToInt(Request.QueryString["level"]);

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetUniqueUserCenter();
            if (publishmentSystemInfo != null)
                homeUrl = publishmentSystemInfo.PublishmentSystemDir;


            var contentIDs = DataProvider.MlibDAO.GetContentIDsBySubmissionID(submissionID);
            #region 其他阶段稿件链接
            for (int i = 0; i < contentIDs.Tables[0].Rows.Count; i++)
            {
                int itemCheckLevel = TranslateUtils.ToInt(contentIDs.Tables[0].Rows[i]["checkedLevel"].ToString());
                ltlTabAction.Text += string.Format("<a class=\"{0}\" href=\"submissionShow.aspx?SubmissionID={1}&level={2}\"><span>{3}</span></a>",
                    checkedLevel == itemCheckLevel ? "current" : "", submissionID, itemCheckLevel, itemCheckLevel == 0 ? "草稿" : itemCheckLevel + "审稿");
            }
            #endregion


            DataView dv = new DataView(contentIDs.Tables[0]);
            dv.RowFilter = "checkedLevel=" + checkedLevel;
            this.contentID = TranslateUtils.ToInt(dv[0]["ID"].ToString());

            var nodeInfoDs = DataProvider.MlibDAO.GetNodeInfoBySubmissionID(submissionID);
            this.nodeID = TranslateUtils.ToInt(nodeInfoDs.Tables[0].Rows[0]["NodeID"].ToString());

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);



            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);

            this.contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

            if (!IsPostBack)
            {


                #region 审核信息
                if (contentInfo.CheckedLevel == 0)
                {
                    ltlStatus.Text = string.Format("{0}:由用户{1}添加草稿", contentInfo.AddDate.ToString(), contentInfo.LastEditUserName);
                }
                else
                {
                    ltlStatus.Text = string.Format("{0}:由管理员{1}{2}审{3}通过", contentInfo.AddDate.ToString(), contentInfo.LastEditUserName, Number2Chinese(contentInfo.CheckedLevel), contentInfo.IsChecked ? "" : "不");
                    
                }


                #endregion

                //base.BreadCrumb(ProductManager.WCM.LeftMenu.ID_Content, "查看内容", string.Empty);

                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, tableName, relatedIdentities);
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
