using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


using System.Collections.Generic;

namespace BaiRong.BackgroundPages
{
    public class BackgroundSystemMessage : BackgroundBasePage
    {

        public DropDownList ddlPageNum;
        public TextBox tbKeyword;
        public DropDownList ddlCreationDate;

        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button Delete;

        public TextBox tbDays;
        public Button btnUpdateDays;

        public int pageNum;
        public string keyWord;
        public int daysToCurrent;

        public static string GetRedirectUrl()
        {
            return PageUtils.GetPlatformUrl(string.Format("background_systemMessage.aspx"));
        }

        public string GetDateTime(DateTime datetime)
        {
            string retval = string.Empty;
            if (datetime > DateUtils.SqlMinValue)
            {
                retval = DateUtils.GetDateString(datetime);
            }
            return retval;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                List<int> systemMessageIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("SystemMessageIDCollection"));
                try
                {
                    foreach (int messageID in systemMessageIDList)
                    {
                        BaiRongDataProvider.UserMessageDAO.Delete(messageID);
                    }

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除系统公告", string.Empty);

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
            else if (base.GetQueryString("SetDays") != null)
            {
                int days = TranslateUtils.ToInt(base.GetQueryString("days"));
                if (days >= 0)
                {
                    UserConfigManager.Additional.NewOfDays = days;
                    BaiRongDataProvider.UserConfigDAO.Update(UserConfigManager.Instance);
                    UserConfigManager.IsChanged = true;
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = "IsViewed,AddDate";
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.ItemsPerPage = TranslateUtils.ToInt(this.ddlPageNum.SelectedValue, 15);

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserMessage, "系统公告", AppManager.User.Permission.Usercenter_Msg);

                pageNum = TranslateUtils.ToInt(base.GetQueryString("PageNum"), 15);
                this.ddlPageNum.SelectedValue = pageNum.ToString();
                keyWord = PageUtils.FilterSql(base.GetQueryString("Keyword"));
                this.tbKeyword.Text = keyWord;
                daysToCurrent = TranslateUtils.ToInt(base.GetQueryString("AddDate"), 0);
                this.ddlCreationDate.SelectedValue = daysToCurrent.ToString();

                string backgroundUrl = BackgroundSystemMessage.GetRedirectUrl();

                this.btnAdd.Attributes.Add("onclick", Modal.SystemMessageAdd.GetRedirectUrlToAdd(this.PageUrl));


                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}?Delete=True", backgroundUrl), "SystemMessageIDCollection", "SystemMessageIDCollection", "请选择需要删除的系统公告！", "此操作将删除所选系统公告，确认吗？"));



                this.spContents.SelectCommand = BaiRongDataProvider.UserMessageDAO.GetSqlString(string.Empty, EUserMessageType.SystemAnnouncement, TranslateUtils.ToInt(this.ddlCreationDate.SelectedValue), this.tbKeyword.Text);
                this.spContents.DataBind();
                this.tbDays.Text = UserConfigManager.Additional.NewOfDays.ToString();
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                LinkButton ltTitle = (LinkButton)e.Item.FindControl("ltTitle");
                Literal ltAddDate = (Literal)e.Item.FindControl("ltAddDate");
                LinkButton ltEdit = (LinkButton)e.Item.FindControl("ltEdit");

                ltTitle.Text = TranslateUtils.EvalString(e.Item.DataItem, "Title");
                DateTime addDate = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate");
                bool isViewed = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsViewed"));
                ltTitle.Text = FormatMessageTitle(ltTitle.Text, addDate, isViewed);

                ltTitle.OnClientClick = Modal.SystemMessageAdd.GetRedirectUrlToView(TranslateUtils.EvalInt(e.Item.DataItem, "ID"), BackgroundSystemMessage.GetRedirectUrl());
                ltAddDate.Text = TranslateUtils.EvalDateTime(e.Item.DataItem, "AddDate").ToString("yyyy-MM-dd");

                ltEdit.Text = "编辑";
                ltEdit.OnClientClick = Modal.SystemMessageAdd.GetRedirectUrlToEdit(TranslateUtils.EvalInt(e.Item.DataItem, "ID"), BackgroundSystemMessage.GetRedirectUrl());
            }
        }

        public static string FormatMessageTitle(string title, DateTime addDate, bool isViewed)
        {
            //IsViewed=true 并且 发布日期在n天内
            if (!isViewed && (DateTime.Now - addDate).TotalSeconds < 3600 * 24 * UserConfigManager.Additional.NewOfDays && (DateTime.Now - addDate).TotalSeconds > 0)
            {
                //n天内，消息为最新
                title = title + " [最新]";
            }
            return title;
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        public void UpdateDays_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(string.Format("{0}&SetDays=True&days={1}", this.PageUrl, this.tbDays.Text));
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = string.Format("{0}?PageNum={1}&Keyword={2}&AddDate={3}", BackgroundSystemMessage.GetRedirectUrl(), this.ddlPageNum.SelectedValue, this.tbKeyword.Text, this.ddlCreationDate.SelectedValue);
                }
                return this._pageUrl;
            }
        }
    }
}
