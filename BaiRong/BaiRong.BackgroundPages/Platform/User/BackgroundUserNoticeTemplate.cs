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
    public class BackgroundUserNoticeTemplate : BackgroundBasePage
    {

        public DropDownList ddlUserNoticeType;
        public TextBox tbKeyword;
        public DropDownList ddlUserNoticeTemplateType;

        public Repeater rptContents;
        public SqlPager spContents;

        public Button btnAdd;
        public Button Delete;

        public string keyWord;
        public string userNoticeType;
        public string userNoticeTemplateType;

        public static string GetRedirectUrl()
        {
            return PageUtils.GetPlatformUrl(string.Format("background_UserNoticeTemplate.aspx"));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("Delete") != null)
            {
                List<int> UserNoticeTemplateIDList = TranslateUtils.StringCollectionToIntList(base.GetQueryString("UserNoticeTemplateIDCollection"));
                try
                {
                    foreach (int userNoticeTemplateID in UserNoticeTemplateIDList)
                    {
                        //删除远程模板
                        UserNoticeTemplateInfo userNoticeTemplateInfo = BaiRongDataProvider.UserNoticeTemplateDAO.GetNoticeTemplateInfo(userNoticeTemplateID);
                        if (userNoticeTemplateInfo != null)
                        {
                            if (userNoticeTemplateInfo.Type == EUserNoticeTemplateType.Phone)
                            {
                                string errorMessage = string.Empty;
                                SMSServerManager.DeleteTemplate(userNoticeTemplateInfo.RemoteTemplateID, out errorMessage);
                            }

                            BaiRongDataProvider.UserNoticeTemplateDAO.Delete(userNoticeTemplateID);
                        }

                    }

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除系统通知", string.Empty);

                    base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.SortField = BaiRongDataProvider.UserNoticeTemplateDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.spContents.ItemsPerPage = 15;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserMessage, "消息模板管理", AppManager.User.Permission.Usercenter_Template);

                EUserNoticeTypeUtils.AddListItemsToInstall(this.ddlUserNoticeType);
                this.ddlUserNoticeType.Items.Insert(0, new ListItem("-全部-", string.Empty));
                EUserNoticeTemplateTypeUtils.AddListItemsToInstall(this.ddlUserNoticeTemplateType);
                this.ddlUserNoticeTemplateType.Items.Insert(0, new ListItem("-全部-", string.Empty));


                keyWord = PageUtils.FilterSql(base.GetQueryString("Keyword"));
                userNoticeType = PageUtils.FilterSql(base.GetQueryString("userNoticeType"));
                userNoticeTemplateType = PageUtils.FilterSql(base.GetQueryString("userNoticeTemplateType"));
                this.tbKeyword.Text = keyWord;
                this.ddlUserNoticeType.SelectedValue = userNoticeType;
                this.ddlUserNoticeTemplateType.SelectedValue = userNoticeTemplateType;

                string backgroundUrl = BackgroundUserNoticeTemplate.GetRedirectUrl();

                this.btnAdd.Attributes.Add("onclick", Modal.UserNoticeTemplateAdd.GetRedirectUrlToAdd(this.PageUrl));


                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}?Delete=True", backgroundUrl), "UserNoticeTemplateIDCollection", "UserNoticeTemplateIDCollection", "请选择需要删除的消息模板！", "此操作将删除所选消息模板，确认吗？"));

                this.spContents.SelectCommand = BaiRongDataProvider.UserNoticeTemplateDAO.GetSqlString(this.ddlUserNoticeType.SelectedValue, this.ddlUserNoticeTemplateType.SelectedValue, keyWord);
                this.spContents.DataBind();
            }
        }

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Literal ltName = (Literal)e.Item.FindControl("ltName");
                Literal ltTitle = (Literal)e.Item.FindControl("ltTitle");
                Literal ltContent = (Literal)e.Item.FindControl("ltContent");
                Literal ltUserNoticetType = (Literal)e.Item.FindControl("ltUserNoticetType");
                Literal ltUesrNoticeTemplateType = (Literal)e.Item.FindControl("ltUesrNoticeTemplateType");
                Literal ltIsEnable = (Literal)e.Item.FindControl("ltIsEnable");

                ltName.Text = TranslateUtils.EvalString(e.Item.DataItem, "Name");
                ltTitle.Text = TranslateUtils.EvalString(e.Item.DataItem, "Title");
                ltContent.Text = TranslateUtils.EvalString(e.Item.DataItem, "Content");
                ltUserNoticetType.Text = EUserNoticeTypeUtils.GetText(EUserNoticeTypeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "RelatedIdentity")));
                ltUesrNoticeTemplateType.Text = EUserNoticeTemplateTypeUtils.GetText(EUserNoticeTemplateTypeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "Type")));
                ltIsEnable.Text = TranslateUtils.ToBool(TranslateUtils.EvalString(e.Item.DataItem, "IsEnable")) ? "可用" : "不可用";


                LinkButton ltEdit = (LinkButton)e.Item.FindControl("ltEdit");

                ltEdit.Text = "编辑";
                ltEdit.OnClientClick = Modal.UserNoticeTemplateAdd.GetRedirectUrlToEdit(TranslateUtils.EvalInt(e.Item.DataItem, "ID"), BackgroundUserNoticeTemplate.GetRedirectUrl());
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
                    this._pageUrl = string.Format("{0}?userNoticeType={1}&userNoticeTemplateType={2}&keyword={3}", BackgroundUserNoticeTemplate.GetRedirectUrl(), this.ddlUserNoticeType.SelectedValue, this.ddlUserNoticeTemplateType.SelectedValue, this.tbKeyword.Text);
                }
                return this._pageUrl;
            }
        }
    }
}
