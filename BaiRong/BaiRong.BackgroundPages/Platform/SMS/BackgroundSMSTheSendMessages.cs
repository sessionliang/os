using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Controls;
using BaiRong.Core;

namespace BaiRong.BackgroundPages
{
    public class BackgroundSMSTheSendMessages : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Button Delete;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_SMS, "已发短信查询", AppManager.Platform.Permission.platform_SMS);

                if (string.Equals(base.GetQueryString("DeleteAll"), "true"))
                {
                    BaiRongDataProvider.SMSMessageDAO.DeleteAll();
                    base.SuccessMessage("清除成功！");
                }

                if (string.IsNullOrEmpty(ConfigManager.Additional.SMSAccount))
                {
                    base.FailMessage(string.Format("无法查询记录，请先注册短信通账号"));
                }
                else
                {
                    this.spContents.ControlToPaginate = this.rptContents;
                    this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                    this.spContents.ItemsPerPage = 18;
                    this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
                    this.spContents.SelectCommand = BaiRongDataProvider.SMSMessageDAO.GetSelectCommand();
                    this.spContents.SortField = "ID";
                    this.spContents.SortMode = SortMode.DESC;
                    if (!this.Page.IsPostBack)
                    {
                        this.spContents.DataBind();
                    }
                }
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlMobile = e.Item.FindControl("ltlMobile") as Literal;
                Literal ltlContent = e.Item.FindControl("ltlContent") as Literal;
                Literal ltlSendtime = e.Item.FindControl("ltlSendtime") as Literal;

                ltlMobile.Text = TranslateUtils.EvalString(e.Item.DataItem, "MobilesList");
                ltlContent.Text = TranslateUtils.EvalString(e.Item.DataItem, "SMSContent");
                ltlSendtime.Text = TranslateUtils.EvalString(e.Item.DataItem, "SendDate");
            }
        }

        public void Delete_OnClick(object sender, EventArgs E)
        {
            string url = PageUtils.GetPlatformUrl(string.Format("background_smsTheSendMessages.aspx?DeleteAll={0}", string.Format("true")));
            Response.Redirect(url);
        }
    }
}
