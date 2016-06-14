using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class ContentArchive : BackgroundBasePage
	{
        private int nodeID;
        private ETableStyle tableStyle;
        private string returnUrl;
        private ArrayList contentIDArrayList;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("内容归档", "modal_contentArchive.aspx", arguments, "ContentIDCollection", "请选择需要归档的内容！", 400, 200);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "NodeID", "ReturnUrl", "ContentIDCollection");

            this.nodeID = base.GetIntQueryString("NodeID");
            this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, this.nodeID);
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeID);
            ArchiveManager.CreateArchiveTableIfNotExists(base.PublishmentSystemInfo, tableName);
            string tableNameOfArchive = TableManager.GetTableNameOfArchive(tableName);

            foreach (int contentID in this.contentIDArrayList)
            {
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, tableName, contentID);
                contentInfo.LastEditDate = DateTime.Now;
                DataProvider.ContentDAO.Insert(tableNameOfArchive, base.PublishmentSystemInfo, contentInfo);
            }

            DataProvider.ContentDAO.DeleteContents(base.PublishmentSystemID, tableName, this.contentIDArrayList, this.nodeID);

            string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Delete, ETemplateType.ChannelTemplate, this.nodeID, 0, 0);
            AjaxUrlManager.AddAjaxUrl(ajaxUrl);

            StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, 0, "归档内容", string.Empty);

            JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
		}

	}
}
