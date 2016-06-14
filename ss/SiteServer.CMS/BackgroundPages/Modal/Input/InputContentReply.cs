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
using SiteServer.CMS.Controls;
using System;
using BaiRong.Controls;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class InputContentReply : BackgroundBasePage
	{
        protected Repeater rptContents;
        protected BREditor breReply;

        private int contentID;
        ArrayList relatedIdentities;
        private InputInfo inputInfo;
        private InputContentInfo contentInfo;

        public static string GetOpenWindowString(int publishmentSystemID, int inputID, int contentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("InputID", inputID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            return PageUtility.GetOpenWindowString("回复信息", "modal_inputContentReply.aspx", arguments);
        }
	
		public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "InputID", "ContentID");

            int inputID = base.GetIntQueryString("InputID");
            this.contentID = base.GetIntQueryString("ContentID");
            this.relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.InputContent, base.PublishmentSystemID, inputID);

            this.inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);

            this.contentInfo = DataProvider.InputContentDAO.GetContentInfo(this.contentID);

            if (!Page.IsPostBack)
            {
                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.InputContent, DataProvider.InputContentDAO.TableName, this.relatedIdentities);

                this.rptContents.DataSource = styleInfoArrayList;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();

                this.breReply.Text = this.contentInfo.Reply;
            }
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableStyleInfo styleInfo = e.Item.DataItem as TableStyleInfo;

                string dataValue = this.contentInfo.GetExtendedAttribute(styleInfo.AttributeName);
                dataValue = InputTypeParser.GetContentByTableStyle(dataValue, base.PublishmentSystemInfo, ETableStyle.InputContent, styleInfo);

                Literal ltlDataKey = e.Item.FindControl("ltlDataKey") as Literal;
                Literal ltlDataValue = e.Item.FindControl("ltlDataValue") as Literal;

                if (ltlDataKey != null) ltlDataKey.Text = styleInfo.DisplayName;
                if (ltlDataValue != null) ltlDataValue.Text = dataValue;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            try
            {
                this.contentInfo.Reply = this.breReply.Text;
                DataProvider.InputContentDAO.Update(this.contentInfo);
                StringUtility.AddLog(base.PublishmentSystemID, "回复提交表单内容", string.Format("提交表单:{0},回复:{1}", this.inputInfo.InputName, this.breReply.Text));
                isSuccess = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }

            if (isSuccess)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
	}
}
