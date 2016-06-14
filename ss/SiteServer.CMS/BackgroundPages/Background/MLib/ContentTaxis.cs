using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.BackgroundPages.MLib
{
    public class ContentTaxis : MLibBackgroundBasePage
    {
        protected RadioButtonList TaxisType;
        protected TextBox TaxisNum;

        private int nodeID;
        private string returnUrl;
        private ArrayList contentIDArrayList;
        private ETableStyle tableStyle;
        private string tableName;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return JsUtils.OpenWindow.GetOpenWindowStringWithCheckBoxValue("内容排序", "modal_contentTaxis.aspx", arguments, "ContentIDCollection", "请选择需要排序的内容！", 300, 220);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ReturnUrl", "ContentIDCollection");

            this.nodeID = int.Parse(base.Request.QueryString["NodeID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["ReturnUrl"]);
            this.contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.QueryString["ContentIDCollection"]);
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeID);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeID);

            if (!IsPostBack)
            {
                this.TaxisType.Items.Add(new ListItem("上升", "Up"));
                this.TaxisType.Items.Add(new ListItem("下降", "Down"));
                ControlUtils.SelectListItems(this.TaxisType, "Up");


            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isUp = (this.TaxisType.SelectedValue == "Up");
            int taxisNum = int.Parse(this.TaxisNum.Text);

            if (isUp == false)
            {
                this.contentIDArrayList.Reverse();
            }

            foreach (int contentID in this.contentIDArrayList)
            {
                DataProvider.MlibDAO.UpdateSubmissionTaxis(nodeID, contentID, isUp, taxisNum);
            }

            JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
        }

    }
}
