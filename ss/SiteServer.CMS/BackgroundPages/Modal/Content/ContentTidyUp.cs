using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Web.UI.WebControls;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class ContentTidyUp : BackgroundBasePage
    {
        public RadioButtonList rblAttributeName;
        public RadioButtonList rblIsDesc;
        
        private string tableName;
        private string returnUrl;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("重新排序", "modal_contentTidyUp.aspx", arguments, "ContentIDCollection", "", 430, 280);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl")).Replace("&DateFrom=&SearchType=&Keyword=&taxisByAddDate=", "");
            if (!IsPostBack)
            {
                ListItem listItem = new ListItem("内容ID", ContentAttribute.ID);
                listItem.Selected = true;
                this.rblAttributeName.Items.Add(listItem);
                listItem = new ListItem("添加日期", ContentAttribute.AddDate);
                this.rblAttributeName.Items.Add(listItem);

                listItem = new ListItem("正序", false.ToString());
                this.rblIsDesc.Items.Add(listItem);
                listItem = new ListItem("倒序", true.ToString());
                listItem.Selected = true;
                this.rblIsDesc.Items.Add(listItem);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            int nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"));
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);

            DataProvider.ContentDAO.TidyUp(tableName, nodeID, this.rblAttributeName.SelectedValue, TranslateUtils.ToBool(this.rblIsDesc.SelectedValue));

            JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
        }
    }
}
