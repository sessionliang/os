using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;
using System.Text;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundSubscribePushRecord : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlHeadRowReply;
        public DropDownList State;
        public TextBox Mobile;
        public TextBox Email;
        public DateTimeTextBox DateFrom;
        public DateTimeTextBox DateTo;

        public Button ManualPush;


        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            PageUtils.CheckRequestParameter("PublishmentSystemID");


            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            if ( base.PublishmentSystemID==0)
            {
                this.spContents.SelectCommand = DataProvider.SubscribePushRecordDAO.GetAllString(base.PublishmentSystemID, string.Empty);//string.Empty
            }
            else
            {
                ETriState stateType = ETriStateUtils.GetEnumType(base.GetQueryString("SearchState"));
                this.spContents.SelectCommand = DataProvider.SubscribePushRecordDAO.GetSelectCommend(base.PublishmentSystemID, base.GetQueryString("Mobile"), base.GetQueryString("Email"), base.GetQueryString("DateFrom"), base.GetQueryString("DateTo"), stateType);
            }
            this.spContents.SortField = SubscribePushRecordAttribute.AddDate;
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
            {
                this.spContents.DataBind();

                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Subscribe, "订阅内容推送记录", AppManager.CMS.Permission.WebSite.Subscribe);

                ETriStateUtils.AddListItems(this.State, "全部", "成功", "失败");


                ControlUtils.SelectListItems(this.State, base.GetQueryString("SearchState"));
                this.Mobile.Text = base.GetQueryString("Mobile");
                this.Email.Text = base.GetQueryString("Email");
                this.DateFrom.Text = base.GetQueryString("DateFrom");
                this.DateTo.Text = base.GetQueryString("DateTo");


                this.ManualPush.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("modal_subscribePushAgain.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)), "IDsCollection", "IDsCollection", "请选择需要推送订阅信息的会员！"));

            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int RecordID = TranslateUtils.EvalInt(e.Item.DataItem, "RecordID");
                string email = TranslateUtils.EvalString(e.Item.DataItem, "Email");
                string subscribeName = TranslateUtils.EvalString(e.Item.DataItem, "SubscribeName");
                string mobile = TranslateUtils.EvalString(e.Item.DataItem, "Mobile");
                string pushType = TranslateUtils.EvalString(e.Item.DataItem, "PushType");
                string pushStatu = TranslateUtils.EvalString(e.Item.DataItem, "PushStatu");
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");


                Literal itemEmail = (Literal)e.Item.FindControl("ItemEmail");
                Literal itemMobile = (Literal)e.Item.FindControl("ItemMobile");
                Literal itemSubscribeName = (Literal)e.Item.FindControl("ItemSubscribeName");
                Literal itemPushType = (Literal)e.Item.FindControl("ItemPushType");
                Literal itemAddDate = (Literal)e.Item.FindControl("ItemAddDate");
                Literal itemPushStatu = (Literal)e.Item.FindControl("ItemPushStatu");
                Literal itemEidtRow = (Literal)e.Item.FindControl("ItemEidtRow");

                if (string.IsNullOrEmpty(mobile) && EBooleanUtils.Equals(EBoolean.False, pushStatu) && email.IndexOf('@') > 0)
                    itemEidtRow.Text = string.Format(@"<a href=""{0}"" >再次推送</a>", PageUtils.GetCMSUrl(string.Format("modal_subscribePushAgain.aspx?PublishmentSystemID={0}&IDsCollection={1}", base.PublishmentSystemID, RecordID)));

                itemEmail.Text = email;
                itemSubscribeName.Text = subscribeName;
                itemMobile.Text = mobile;
                itemPushType.Text = ESubscribePushTypeUtils.GetText(ESubscribePushTypeUtils.GetEnumType(pushType));
                itemPushStatu.Text = EBooleanUtils.Equals(EBoolean.True, pushStatu) ? "成功" : "失败";
                itemAddDate.Text = addDate.ToString("yyyy-MM-dd HH:mm:ss");

            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }
 

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_subscribePushRecord.aspx?PublishmentSystemID={0}&SearchState={1}&Mobile={2}&Email={3}&DateFrom={4}&DateTo={5}", base.PublishmentSystemID,  this.State.SelectedValue, this.Mobile.Text, this.Email.Text, this.DateFrom.Text, this.DateTo.Text));
                }
                return _pageUrl;
            }
        }

    }
}
