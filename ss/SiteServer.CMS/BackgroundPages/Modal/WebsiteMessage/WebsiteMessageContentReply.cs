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
    public class WebsiteMessageContentReply : BackgroundBasePage
    {
        protected Repeater rptContents;
        protected BREditor breReply;

        //类型
        protected string ClassifyName;
        //回复模板
        protected DropDownList ddlReplayTemplate;
        //是否发送短信
        protected RadioButtonList rblIsSendSMS;
        protected PlaceHolder phSendSMS;

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
            return PageUtility.GetOpenWindowString("回复信息", "modal_websiteMessageContentReply.aspx", arguments);
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

            if (!Page.IsPostBack)
            {
                #region 绑定模板数据
                ArrayList templateArrayList = DataProvider.WebsiteMessageReplayTemplateDAO.GetWebsiteMessageReplayTemplateInfoArrayList("IsEnabled = 'True'");
                this.ddlReplayTemplate.DataSource = templateArrayList;
                this.ddlReplayTemplate.DataTextField = "TemplateTitle";
                this.ddlReplayTemplate.DataValueField = "ID";
                this.ddlReplayTemplate.DataBind();
                this.ddlReplayTemplate.Items.Insert(0, new ListItem("--请选择--", ""));
                #endregion

                EBooleanUtils.AddListItems(this.rblIsSendSMS, "发送", "不发送");
                ControlUtils.SelectListItems(this.rblIsSendSMS, false.ToString());

                if (this.websiteMessageInfo.Additional.IsSMS)
                {
                    this.phSendSMS.Visible = true;
                }

                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, this.relatedIdentities);

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
                dataValue = InputTypeParser.GetContentByTableStyle(dataValue, base.PublishmentSystemInfo, ETableStyle.WebsiteMessageContent, styleInfo);

                Literal ltlDataKey = e.Item.FindControl("ltlDataKey") as Literal;
                Literal ltlDataValue = e.Item.FindControl("ltlDataValue") as Literal;

                if (ltlDataKey != null) ltlDataKey.Text = styleInfo.DisplayName;
                if (ltlDataValue != null) ltlDataValue.Text = dataValue;
            }
        }
        protected void ddlReplayTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlReplayTemplate.SelectedIndex > 0)
            {
                WebsiteMessageReplayTemplateInfo templateInfo = DataProvider.WebsiteMessageReplayTemplateDAO.GetWebsiteMessageReplayTemplateInfo(TranslateUtils.ToInt(this.ddlReplayTemplate.SelectedValue));
                if (templateInfo != null)
                    this.breReply.Text = templateInfo.TemplateContent;
            }
        }


        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            try
            {
                this.contentInfo.Reply = this.breReply.Text;
                DataProvider.WebsiteMessageContentDAO.Update(this.contentInfo);
                StringUtility.AddLog(base.PublishmentSystemID, "回复提交表单内容", string.Format("提交表单:{0},回复:{1}", this.websiteMessageInfo.WebsiteMessageName, this.breReply.Text));

                ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(ETableStyle.WebsiteMessageContent, base.PublishmentSystemID, this.websiteMessageInfo.WebsiteMessageID);
                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.WebsiteMessageContent, DataProvider.WebsiteMessageContentDAO.TableName, relatedIdentities);
                //发送回复邮件
                MessageManager.SendMailByWebsiteMessageReply(base.PublishmentSystemInfo, this.websiteMessageInfo, relatedIdentities, this.contentInfo);
                //发送回复短信
                if (TranslateUtils.ToBool(this.rblIsSendSMS.SelectedValue))
                {
                    MessageManager.SendSMSByWebsiteMessageWebsiteMessageReply(base.PublishmentSystemInfo, this.websiteMessageInfo, relatedIdentities, this.contentInfo);
                }
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
